using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public enum FinsCommandType
    {
        RequestAddress, ReadData, WriteData
    }

    public enum FinsDataType
    {
        CIO_Bit = 0x30,
        WR_Bit = 0x31,
        HR_Bit = 0x32,
        AR_Bit = 0x33,
        CIO_Word = 0xB0,
        WR_Word = 0xB1,
        HR_Word = 0xB2,
        AR_Word = 0xB3,
        TIM_PV = 0x89,
        DM_Bit = 0x02,
        DM_Word = 0x82,
        EMx_Bit = 0x20,
        EMx_Word = 0xA0,
        EM_Current = 0x98,
        IR_PV = 0xDC,
        DR_PV = 0xBC
    }

    public class FinsInfo
    {
        public int NetworkNo { get; set; }
        public long PlcStateAddress { get; set; }
        public long PcStateAddress { get; set; }
        public long ResultAddress { get; set; }
        public string IpAddress { get; set; }
        public int PortNo { get; set; }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement finsElement = xmlElement[keyName];
            if (finsElement == null)
            {
                return;
            }

            NetworkNo = Convert.ToInt32(XmlHelper.GetValue(finsElement, "NetworkNo", ""));
            PlcStateAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "PlcStateAddress", ""));
            PcStateAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "PcStateAddress", ""));
            ResultAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "ResultAddress", ""));
            IpAddress = XmlHelper.GetValue(finsElement, "IpAddress", "");
            PortNo = Convert.ToInt32(XmlHelper.GetValue(finsElement, "PortNo", ""));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement finsElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(finsElement);

            XmlHelper.SetValue(finsElement, "NetworkNo", NetworkNo.ToString());
            XmlHelper.SetValue(finsElement, "PlcStateAddress", PlcStateAddress.ToString());
            XmlHelper.SetValue(finsElement, "PcStateAddress", PcStateAddress.ToString());
            XmlHelper.SetValue(finsElement, "ResultAddress", ResultAddress.ToString());
            if (string.IsNullOrEmpty(IpAddress))
            {
                XmlHelper.SetValue(finsElement, "IpAddress", "");
            }
            else
            {
                XmlHelper.SetValue(finsElement, "IpAddress", IpAddress);
            }

            XmlHelper.SetValue(finsElement, "PortNo", PortNo.ToString());
        }
    }

    public class FinsReceivedPacket : ReceivedPacket
    {
        public FinsCommandType RequestCommand { get; }
        public FinsDataType DataType { get; }
        public long DataAddress { get; }
        public int PlcNodeNo { get; } = 0;
        public int PcNodeNo { get; } = 0;
        public byte[] BinaryData { get; }

        public FinsReceivedPacket(int plcNodeNo, int pcNodeNo, FinsCommandType requestCommand, FinsDataType dataType, long dataAddress, string stringData, byte[] binaryData)
        {
            PlcNodeNo = plcNodeNo;
            PcNodeNo = pcNodeNo;
            RequestCommand = requestCommand;
            DataType = dataType;
            DataAddress = dataAddress;
            ReceivedData = stringData;
            BinaryData = binaryData;
        }
    }

    public class FinsPacketParser : PacketParser
    {
        public FinsCommandType RequestCommand { get; }

        private FinsDataType dataType;
        private int plcNetworkNo;
        public int PlcNodeNo { get; private set; }

        private int pcNetworkNo;
        public int PcNodeNo { get; private set; }
        public long DataAddress { get; }

        private int bitAddress;
        private int dataSize;
        private string data;
        private int sequence = 0;
        private static int nextSequence = 0;
        private string lastSendPacket;
        public bool Successed { get; set; }

        public FinsPacketParser()
        {
            RequestCommand = FinsCommandType.RequestAddress;
        }

        public FinsPacketParser(int plcNetworkNo, int plcNodeNo, int pcNetworkNo, int pcNodeNo, FinsDataType dataType, long dataAddress, int bitAddress, int dataSize)
        {
            RequestCommand = FinsCommandType.ReadData;
            this.plcNetworkNo = plcNetworkNo;
            PlcNodeNo = plcNodeNo;
            this.pcNetworkNo = pcNetworkNo;
            PcNodeNo = pcNodeNo;
            this.dataType = dataType;
            DataAddress = dataAddress;
            this.bitAddress = bitAddress;
            this.dataSize = dataSize;

            sequence = nextSequence;
            nextSequence++;
            nextSequence = nextSequence % 255;
        }

        public FinsPacketParser(int plcNetworkNo, int plcNodeNo, int pcNetworkNo, int pcNodeNo, FinsDataType dataType, long dataAddress, int bitAddress, string data)
        {
            RequestCommand = FinsCommandType.WriteData;
            this.plcNetworkNo = plcNetworkNo;
            PlcNodeNo = plcNodeNo;
            this.pcNetworkNo = pcNetworkNo;
            PcNodeNo = pcNodeNo;
            this.dataType = dataType;
            DataAddress = dataAddress;
            this.bitAddress = bitAddress;
            this.data = data;

            sequence = nextSequence;
            nextSequence++;
            nextSequence = nextSequence % 255;
        }

        public override byte[] GetRequestPacket()
        {
            Successed = false;
            string packet = "";

            string sequenceStr = sequence.ToString("X2");

            switch (RequestCommand)
            {
                case FinsCommandType.RequestAddress:
                    {
                        string commandString = "00000000";
                        string errorCode = "00000000";
                        string clientNodeAddress = "00000000";

                        packet = commandString + errorCode + clientNodeAddress;
                    }
                    break;
                case FinsCommandType.ReadData:
                    {
                        string commandString = "00000002";
                        string errorCode = "00000000";
                        string finsCommand = "0101";

                        packet = commandString + errorCode + "800002" + plcNetworkNo.ToString("X02") + PlcNodeNo.ToString("X02")
                                            + "00" // PLC CPU Unit 번호
                                            + pcNetworkNo.ToString("X02") + PcNodeNo.ToString("X02")
                                            + "00" // PLC CPU Unit 번호
                                            + sequenceStr // Sequence. 자동 증가
                                            + finsCommand + ((int)dataType).ToString("X02") + DataAddress.ToString("X04") + bitAddress.ToString("X02") + dataSize.ToString("X04");
                    }
                    break;
                case FinsCommandType.WriteData:
                    {
                        string commandString = "00000002";
                        string errorCode = "00000000";
                        string finsCommand = "0102";

                        int dataSize = data.Length;

                        packet = commandString + errorCode + "800002" + plcNetworkNo.ToString("X02") + PlcNodeNo.ToString("X02")
                                            + "00" // PLC CPU Unit 번호
                                            + pcNetworkNo.ToString("X02") + PcNodeNo.ToString("X02")
                                            + "00" // PLC CPU Unit 번호
                                            + sequenceStr // Sequence. 자동 증가
                                            + finsCommand + ((int)dataType).ToString("X02") + DataAddress.ToString("X04") +
                                            bitAddress.ToString("X02") + (dataSize / 4).ToString("X04") + data;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(packet))
            {
                return null;
            }

            lastSendPacket = "46494E53" + (packet.Length / 2).ToString("X08") + packet;

            var valueList = new List<byte>();
            for (int i = 0; i < lastSendPacket.Length; i += 2)
            {
                // Convert the number expressed in base-16 to an integer.
                byte value = Convert.ToByte(lastSendPacket.Substring(i, 2), 16);
                valueList.Add(value);
            }

            //            LogHelper.Debug(LoggerType.Network, String.Format("Send Packet {0}", lastSendPacket));

            byte[] bytePacket = valueList.ToArray();

            return bytePacket;
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            string currentString = ""; //  System.Text.Encoding.Default.GetString(packetContents.ToArray(), );

            for (int i = 0; i < packetContents.Length; i++)
            {
                currentString += packetContents[i].ToString("X02");
            }

            //            LogHelper.Debug(LoggerType.Network, String.Format("Receive Packet {0}", currentString));

            string header = currentString.Substring(0, 8);
            if (header.ToUpper() != "46494E53")
            {
                return false;
            }

            string packetSizeStr = currentString.Substring(8, 8);
            if (packetSizeStr.Length < 8)
            {
                return false;
            }

            int packetSize = Convert.ToInt32(packetSizeStr, 16);
            string packet = currentString.Substring(16);
            if (packetSize * 2 < packet.Length)
            {
                return false;
            }

            string command = packet.Substring(0, 8);
            string errorCode = packet.Substring(8, 8);

            if (errorCode != "00000000")
            {
                LogHelper.Debug(LoggerType.Comm, string.Format("Error Occurred. {0}", errorCode));
                return true;
            }

            string receivedData = "";
            byte[] receivedBinaryData = null;

            switch (RequestCommand)
            {
                case FinsCommandType.RequestAddress:
                    {
                        string pcNodeNoStr = packet.Substring(16, 8);
                        string plcNodeNoStr = packet.Substring(24, 8);
                        PcNodeNo = Convert.ToInt32(pcNodeNoStr, 16);
                        PlcNodeNo = Convert.ToInt32(plcNodeNoStr, 16);

                        Successed = true;
                    }
                    break;
                case FinsCommandType.ReadData:
                    {
                        string returnCommand = packet.Substring(36, 4);
                        string frameErrorCode = packet.Substring(40, 4);

                        if (returnCommand != "0101")
                        {
                            LogHelper.Debug(LoggerType.Comm, string.Format("Frame Command is Invalid. {0}", returnCommand));
                            LogHelper.Debug(LoggerType.Comm, string.Format("Send Packet. {0}", lastSendPacket));
                            LogHelper.Debug(LoggerType.Comm, string.Format("Return Packet. {0}", currentString));
                            return true;
                        }

                        if (frameErrorCode != "0000")
                        {
                            LogHelper.Debug(LoggerType.Comm, string.Format("Frame Error. {0}", frameErrorCode));
                            return true;
                        }

                        receivedData = packet.Substring(44);
                        receivedBinaryData = packetContents.Skip(30).Take(4).ToArray();
                        Successed = true;
                    }
                    break;
                case FinsCommandType.WriteData:
                    {
                        string returnCommand = packet.Substring(36, 4);
                        string frameErrorCode = packet.Substring(40, 4);

                        if (returnCommand != "0102")
                        {
                            LogHelper.Debug(LoggerType.Comm, string.Format("Frame Command is Invalid. {0}", returnCommand));
                            LogHelper.Debug(LoggerType.Comm, string.Format("Send Packet. {0}", lastSendPacket));
                            LogHelper.Debug(LoggerType.Comm, string.Format("Return Packet. {0}", currentString));
                            return true;
                        }

                        if (frameErrorCode != "0000")
                        {
                            LogHelper.Debug(LoggerType.Comm, string.Format("Frame Error. {0}", frameErrorCode));
                            return true;
                        }

                        Successed = true;
                    }
                    break;
            }

            if (DataReceived != null)
            {
                var receivedPacket = new FinsReceivedPacket(PlcNodeNo, PcNodeNo, RequestCommand, dataType, DataAddress, receivedData, receivedBinaryData);
                DataReceived(receivedPacket);
            }

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }
}
