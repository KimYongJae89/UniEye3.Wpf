using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public class MelsecQSendPacket : SendPacket
    {
        public short Command { get; private set; }
        public short SubCommand { get; private set; }
        public string AddressName { get; private set; }

        public bool IsReadCommand()
        {
            return Command == 0x0401;
        }
        public int DataLength { get; private set; } = 0;

        private StringBuilder dataList = new StringBuilder();

        public string GetHexString()
        {
            return dataList.ToString();
        }

        public override byte[] Data => Encoding.UTF8.GetBytes(dataList.ToString());

        public string DataAsString => dataList.ToString();

        public MelsecQSendPacket()
        {
            WaitResponse = true;
        }

        public void SetupReadPacket(string addressName, int numSize)
        {
            Command = 0x0401;
            SubCommand = 0x0000;

            AddressName = addressName;

            DataLength = numSize;
        }

        public void SetupWritePacket(string addressName, string data = "")
        {
            Command = 0x1401;
            SubCommand = 0x0000;

            AddressName = addressName;
        }

        public void AppendData(string value)
        {
            dataList.Append(value);
            DataLength += value.Length / 4;
        }

        public void Add16BitData(int value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 3; i >= 0; i--)
            {
                hexStr += ((int)valueByte[i]).ToString("X2");
            }

            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void Add32BitData(int value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 1; i >= 0; i--)
            {
                hexStr += ((int)valueByte[i]).ToString("X2");
            }

            for (int i = 3; i >= 2; i--)
            {
                hexStr += ((int)valueByte[i]).ToString("X2");
            }

            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void AddData(float value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 3; i >= 0; i--)
            {
                hexStr += ((int)valueByte[i]).ToString("X2");
            }

            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void AddData(short value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 1; i >= 0; i--)
            {
                hexStr += ((int)valueByte[i]).ToString("X2");
            }

            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void AddData(string data)
        {
            string hexStr = "";
            foreach (char ch in data)
            {
                hexStr += ((int)ch).ToString("X2");
            }
            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void AddData(byte[] byteData)
        {
            string hexStr = "";
            foreach (byte bt in byteData)
            {
                hexStr += ((int)bt).ToString("X2");
            }
            dataList.Append(hexStr);

            DataLength += hexStr.Length / 4;
        }

        public void AddDataSwap(string data)
        {
            string hexStr = "";

            for (int i = 0; i < data.Length; i = i + 2)
            {
                if (i + 1 == data.Length)
                {
                    hexStr += ((int)' ').ToString("X2");
                    hexStr += ((int)data[i]).ToString("X2");
                    continue;
                }
                string temp1 = ((int)data[i]).ToString("X2");
                string temp2 = ((int)data[i + 1]).ToString("X2");

                hexStr += temp2;
                hexStr += temp1;
            }

            dataList.Append(hexStr);

            DataLength += hexStr.Length;
        }

        public void ClearData()
        {
            DataLength = 0;
            dataList.Clear();
        }
    }

    public class MelsecQReceivedPacket : ReceivedPacket
    {
        public bool GetBit(int startIndex)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = ReceivedDataByte[1];
            valueByte[1] = ReceivedDataByte[0];

            bool[] valueBool = new bool[16];

            for (int i = 0; i < 8; i++)
            {
                valueBool[i] = ((valueByte[0] >> i) & 0x01) == 0x01;
            }

            for (int i = 8; i < 16; i++)
            {
                valueBool[i] = ((valueByte[1] >> i - 8) & 0x01) == 0x01;
            }

            return valueBool[startIndex];
        }

        public short GetShort(int startIndex)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = ReceivedDataByte[startIndex + 1];
            valueByte[1] = ReceivedDataByte[startIndex];

            return BitConverter.ToInt16(valueByte, 0);
        }

        public short GetSwapShort(int startIndex)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = ReceivedDataByte[startIndex];
            valueByte[1] = ReceivedDataByte[startIndex + 1];

            return BitConverter.ToInt16(valueByte, 0);
        }


        public int GetInt(int startIndex)
        {
            byte[] valueByte = new byte[4];
            byte[] tempByte = new byte[4];
            for (int i = 0; i <= 3; i++)
            {
                tempByte[i] = ReceivedDataByte[startIndex + 3 - i];
            }

            byte temp = tempByte[0];
            tempByte[0] = tempByte[1];
            tempByte[1] = temp;

            temp = tempByte[2];
            tempByte[2] = tempByte[3];
            tempByte[3] = temp;

            for (int i = 0; i <= 3; i++)
            {
                valueByte[i] = tempByte[3 - i];
            }

            return BitConverter.ToInt32(valueByte, 0);
        }

        public string GetString(int startIndex, int length)
        {
            var valueStrBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                valueStrBuilder.Append((char)ReceivedDataByte[startIndex + i]);
            }

            return valueStrBuilder.ToString();
        }


        public string GetSwapString(int startIndex, int length)
        {
            var valueStrBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                valueStrBuilder.Append((char)ReceivedDataByte[startIndex + i]);
            }
            string result = valueStrBuilder.ToString();
            char[] charResult = result.ToArray();


            if (result.Length % 2 == 0)
            {
                for (int i = 0; i < result.Length; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }
            else
            {
                for (int i = 0; i < result.Length - 1; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }

            string returnResult = "";
            for (int i = 0; i < charResult.Length; i++)
            {
                returnResult += charResult[i].ToString();
            }


            return returnResult;
        }

        public byte[] GetByte(int startIndex, int length)
        {
            byte[] getByte = new byte[length];
            for (int i = 0; i < length; i++)
            {
                getByte[i] = ReceivedDataByte[startIndex + i];
            }

            return getByte;
        }

        public byte[] GetSwapBit(byte[] data)
        {
            for (int i = 0; i < data.Length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }

        public byte[] GetSwapBit(byte[] data, int startIndex, int length)
        {
            for (int i = startIndex; i < length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }
    }

    public class MelsecInfo
    {
        public TcpIpInfo TcpIpInfo { get; set; } = new TcpIpInfo();
        public byte NetworkNo { get; set; } = 0;
        public byte PlcNo { get; set; } = 0xff;
        public short ModuleIoNo { get; set; } = 0x03FF;
        public byte ModuleDeviceNo { get; set; } = 0;
        public short CpuInspectorData { get; set; } = 0x000A;

        public MelsecInfo() { }

        public MelsecInfo(MelsecInfo melsecInfo)
        {
            TcpIpInfo = new TcpIpInfo(melsecInfo.TcpIpInfo);
            melsecInfo.NetworkNo = NetworkNo;
            melsecInfo.PlcNo = PlcNo;
            melsecInfo.ModuleIoNo = ModuleIoNo;
            melsecInfo.ModuleDeviceNo = ModuleDeviceNo;
            melsecInfo.CpuInspectorData = CpuInspectorData;
        }

        public MelsecInfo(string ipAddress, int portNo, byte networkNo = 0, byte plcNo = 0xFF, short moduleIoNo = 0x03FF,
                                byte moduleDeviceNo = 0, short cpuInspectorData = 0x000A)
        {
            TcpIpInfo = new TcpIpInfo(ipAddress, portNo);

            NetworkNo = networkNo;
            PlcNo = plcNo;
            ModuleIoNo = moduleIoNo;
            ModuleDeviceNo = moduleDeviceNo;
            CpuInspectorData = cpuInspectorData;
        }

        public MelsecInfo Clone()
        {
            return new MelsecInfo(this);
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement[keyName];
            if (tcpIpElement == null)
            {
                return;
            }

            TcpIpInfo.Load(xmlElement, keyName + "TcpIp");
            NetworkNo = Convert.ToByte(XmlHelper.GetValue(tcpIpElement, "NetworkNo", "0"), 16);
            PlcNo = Convert.ToByte(XmlHelper.GetValue(tcpIpElement, "PlcNo", "ff"), 16);
            ModuleIoNo = Convert.ToInt16(XmlHelper.GetValue(tcpIpElement, "ModuleIoNo", "03FF"), 16);
            ModuleDeviceNo = Convert.ToByte(XmlHelper.GetValue(tcpIpElement, "ModuleDeviceNo", "0"), 16);
            CpuInspectorData = Convert.ToInt16(XmlHelper.GetValue(tcpIpElement, "CpuInspectorData", "000A"), 16);
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(tcpIpElement);

            TcpIpInfo.Save(xmlElement, keyName + "TcpIp");
            XmlHelper.SetValue(tcpIpElement, "NetworkNo", NetworkNo.ToString("X02"));
            XmlHelper.SetValue(tcpIpElement, "PlcNo", PlcNo.ToString("X02"));
            XmlHelper.SetValue(tcpIpElement, "ModuleIoNo", ModuleIoNo.ToString("X04"));
            XmlHelper.SetValue(tcpIpElement, "ModuleDeviceNo", ModuleDeviceNo.ToString("X02"));
            XmlHelper.SetValue(tcpIpElement, "CpuInspectorData", CpuInspectorData.ToString("X04"));
        }

        public override bool Equals(object obj)
        {
            if (obj is MelsecInfo melsecInfo)
            {
                return (melsecInfo.NetworkNo == NetworkNo
                    && melsecInfo.PlcNo == PlcNo
                    && melsecInfo.ModuleIoNo == ModuleIoNo
                    && melsecInfo.ModuleDeviceNo == ModuleDeviceNo
                    );
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -1869651984;
            hashCode = hashCode * -1521134295 + EqualityComparer<TcpIpInfo>.Default.GetHashCode(TcpIpInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<TcpIpInfo>.Default.GetHashCode(TcpIpInfo);
            hashCode = hashCode * -1521134295 + NetworkNo.GetHashCode();
            hashCode = hashCode * -1521134295 + NetworkNo.GetHashCode();
            hashCode = hashCode * -1521134295 + PlcNo.GetHashCode();
            hashCode = hashCode * -1521134295 + PlcNo.GetHashCode();
            hashCode = hashCode * -1521134295 + ModuleIoNo.GetHashCode();
            hashCode = hashCode * -1521134295 + ModuleIoNo.GetHashCode();
            hashCode = hashCode * -1521134295 + ModuleDeviceNo.GetHashCode();
            hashCode = hashCode * -1521134295 + ModuleDeviceNo.GetHashCode();
            hashCode = hashCode * -1521134295 + CpuInspectorData.GetHashCode();
            hashCode = hashCode * -1521134295 + CpuInspectorData.GetHashCode();
            return hashCode;
        }
    }

    public abstract class MelsecQProtocol : IProtocol
    {
        public abstract byte[] EncodePacket(SendPacket sendPacket);

        public abstract DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket);
    }

    public class MelsecQAsciiProtocol : MelsecQProtocol
    {
        private MelsecInfo melsecInfo;

        private bool readMode = false;
        public bool ReadMode
        {
            set => readMode = value;
        }

        private const string COMMAND_HEADER = "5000";
        private const string RESPONSE_HEADER = "D000";

        public MelsecQAsciiProtocol(MelsecInfo melsecInfo)
        {
            this.melsecInfo = melsecInfo;
        }

        private string GetAddressCode(string addressName)
        {
            switch (addressName[0])
            {
                case 'D': return "D*";  // 데이터 레지스터
                case 'M': return "M*";  // 내부 릴레이
                case 'X': return "X*";  // 입력 릴레이
                case 'Y': return "Y*";  // 출력 릴레이
                case 'R': return "R*";  // 파일 레지스터
            }

            throw new ArgumentOutOfRangeException();
        }

        public override byte[] EncodePacket(SendPacket sendPacket)
        {
            var melsecQSendPacket = (MelsecQSendPacket)sendPacket;

            string addressName = melsecQSendPacket.AddressName;
            string addressCode = GetAddressCode(addressName);
            string addressNumber = new string('0', 6 - (addressName.Count() - 1)) + (addressName.Substring(1));

            string commandHeader = COMMAND_HEADER + melsecInfo.NetworkNo.ToString("X2") + melsecInfo.PlcNo.ToString("X2") + melsecInfo.ModuleIoNo.ToString("X4") + melsecInfo.ModuleDeviceNo.ToString("X2");
            string commandData = "";
            if (melsecQSendPacket.IsReadCommand())
            {
                commandData = melsecInfo.CpuInspectorData.ToString("X4") + melsecQSendPacket.Command.ToString("X4") + melsecQSendPacket.SubCommand.ToString("X4") +
                                    addressCode + addressNumber + melsecQSendPacket.DataLength.ToString("X4");
            }
            else
            {
                commandData = melsecInfo.CpuInspectorData.ToString("X4") + melsecQSendPacket.Command.ToString("X4") + melsecQSendPacket.SubCommand.ToString("X4") +
                                    addressCode + addressNumber + (melsecQSendPacket.Data.Length / 2).ToString("X4") + melsecQSendPacket.DataAsString;
            }

            string command = commandHeader + commandData.Length.ToString("X4") + commandData;

            return Encoding.UTF8.GetBytes(command);
        }

        public override DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket)
        {
            receivedPacket = new MelsecQReceivedPacket();

            string receivedStr = Encoding.UTF8.GetString(packetBuffer.DataByteFull);
            int startPos = receivedStr.IndexOf("D000");
            if (startPos == -1)
            {
                return DecodeResult.Incomplete;
            }

            const int subHeaderSize = 22;

            if ((startPos + subHeaderSize) > receivedStr.Length)
            {
                return DecodeResult.Incomplete;
            }

            byte rNetworkNo = Convert.ToByte(receivedStr.Substring(startPos + 4, 2), 16);
            byte rPlcNo = Convert.ToByte(receivedStr.Substring(startPos + 6, 2), 16);
            short rModuleIoNo = Convert.ToInt16(receivedStr.Substring(startPos + 8, 4), 16);
            byte rModuleDeviceNo = Convert.ToByte(receivedStr.Substring(startPos + 12, 2), 16);
            int length = Convert.ToInt32(receivedStr.Substring(startPos + 14, 4), 16);
            short rEndCode = Convert.ToInt16(receivedStr.Substring(startPos + 18, 4), 16);

            int lastPos = startPos + subHeaderSize + length - 4;
            if (lastPos > receivedStr.Length)
            {
                return DecodeResult.Incomplete;
            }

            if (rNetworkNo != melsecInfo.NetworkNo || rPlcNo != melsecInfo.PlcNo ||
                        rModuleIoNo != melsecInfo.ModuleIoNo || rModuleDeviceNo != melsecInfo.ModuleDeviceNo)
            {
                packetBuffer.RemoveData(lastPos);
                return DecodeResult.OtherTarget;
            }

            receivedPacket.Valid = true;
            receivedPacket.ErrCode = rEndCode.ToString("X04");
            receivedPacket.ReceivedData = receivedStr.Substring(startPos + 22, length - 4);
            receivedPacket.ReceivedDataByte = StringHelper.HexStringToByteArray(receivedPacket.ReceivedData);
            receivedPacket.LogString = receivedPacket.ReceivedData;
            packetBuffer.RemoveData(lastPos);

            return DecodeResult.Complete;
        }
    }

    public class MelsecQBinaryProtocol : MelsecQProtocol
    {
        private MelsecInfo melsecInfo;

        private bool readMode = false;
        public bool ReadMode
        {
            set => readMode = value;
        }

        private const string COMMAND_HEADER = "5000";
        private const string RESPONSE_HEADER = "D000";

        protected int IndexOf(byte[] dataByte, byte[] searchByte)
        {
            int searchByteCnt = searchByte.Count();
            for (int dataIndex = 0; dataIndex < dataByte.Count() - searchByteCnt + 1; dataIndex++)
            {
                bool match = true;
                for (int searchIndex = 0; searchIndex < searchByteCnt; searchIndex++)
                {
                    if (dataByte[dataIndex + searchIndex] != searchByte[searchIndex])
                    {
                        match = false;
                        break;
                    }
                }

                if (match == true)
                {
                    return dataIndex;
                }
            }

            return -1;
        }

        public MelsecQBinaryProtocol(MelsecInfo melsecInfo)
        {
            this.melsecInfo = melsecInfo;
        }

        private byte GetAddressCode(string addressName)
        {
            switch (addressName[0])
            {
                case 'D': return 0xA8;  // 데이터 레지스터
                case 'M': return 0x90;  // 내부 릴레이
                case 'X': return 0x9C;  // 입력 릴레이
                case 'Y': return 0x9D;  // 출력 릴레이
                case 'R': return 0xAF;  // 파일 레지스터
                case 'Z': return 0xB0;  // 파일 레지스터
            }

            throw new ArgumentOutOfRangeException();
        }

        public override byte[] EncodePacket(SendPacket sendPacket)
        {
            var melsecQSendPacket = (MelsecQSendPacket)sendPacket;

            string addressName = melsecQSendPacket.AddressName;
            byte addressCode = GetAddressCode(addressName);
            byte[] addressNumber = BitConverter.GetBytes(int.Parse(addressName.Substring(1)));

            var header = new List<byte>();
            header.AddRange(BitConverter.GetBytes(Convert.ToInt16("0050", 16)));
            header.Add(melsecInfo.NetworkNo);
            header.Add(melsecInfo.PlcNo);
            header.AddRange(BitConverter.GetBytes(melsecInfo.ModuleIoNo));
            header.Add(melsecInfo.ModuleDeviceNo);

            var command = new List<byte>();
            command.AddRange(BitConverter.GetBytes(melsecInfo.CpuInspectorData));
            command.AddRange(BitConverter.GetBytes(melsecQSendPacket.Command));   // Batch Read
            command.AddRange(BitConverter.GetBytes(melsecQSendPacket.SubCommand));   // SubCommand

            command.Add(addressNumber[0]);
            command.Add(addressNumber[1]);
            command.Add(addressNumber[2]);
            command.Add(addressCode);

            command.AddRange(BitConverter.GetBytes((short)melsecQSendPacket.DataLength));

            if (melsecQSendPacket.IsReadCommand() == false)
            {
                command.AddRange(StringHelper.HexStringToByteArray(StringHelper.SwapWordHex(melsecQSendPacket.GetHexString())));
            }

            var frame3E = new List<byte>();
            frame3E.AddRange(header);
            frame3E.AddRange(BitConverter.GetBytes((short)command.Count));
            frame3E.AddRange(command);

            return frame3E.ToArray();
        }

        public override DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket)
        {
            receivedPacket = new MelsecQReceivedPacket();

            byte[] stx = new byte[2] { 0xD0, 0x00 };

            int startPos = IndexOf(packetBuffer.DataByteFull, stx);
            if (startPos < 0)
            {
                return DecodeResult.Incomplete;
            }

            byte[] contents = packetBuffer.DataByteFull.Skip(startPos).ToArray();
            if (contents.Length < 11)
            {
                return DecodeResult.Incomplete;
            }

            byte rNetworkNo = contents[2];
            byte rPlcNo = contents[3];
            short rModuleIoNo = BitConverter.ToInt16(contents.Skip(4).Take(2).ToArray(), 0);
            byte rModuleDeviceNo = contents[6];
            short dataLength = BitConverter.ToInt16(contents.Skip(7).Take(2).ToArray(), 0);
            ushort rEndCode = BitConverter.ToUInt16(contents.Skip(9).Take(2).ToArray(), 0);

            if (dataLength > contents.Length - 6)
            {
                return DecodeResult.Incomplete;
            }

            int lastPos = startPos + 9 + dataLength;
            if (rNetworkNo != melsecInfo.NetworkNo || rPlcNo != melsecInfo.PlcNo ||
                        rModuleIoNo != melsecInfo.ModuleIoNo || rModuleDeviceNo != melsecInfo.ModuleDeviceNo)
            {
                packetBuffer.RemoveData(lastPos);
                return DecodeResult.OtherTarget;
            }

            if (rEndCode != 0)
            {
                LogHelper.Error(string.Format("MelsecQBinaryProtocol::DecodePacket - EndCode is {0}", rEndCode.ToString("X")));
            }

            receivedPacket.Valid = true;
            receivedPacket.ErrCode = rEndCode.ToString("X04");
            receivedPacket.ReceivedDataByte = contents.Skip(11).Take(dataLength).ToArray();
            //receivedPacket.ReceivedData = BitConverter.ToString(receivedPacket.ReceivedDataByte).Replace("-", "");
            receivedPacket.ReceivedData = StringHelper.SwapWordHex(BitConverter.ToString(receivedPacket.ReceivedDataByte).Replace("-", ""));
            receivedPacket.LogString = receivedPacket.ReceivedData;
            receivedPacket.SenderInfo = melsecInfo.TcpIpInfo.IpAddress;
            packetBuffer.RemoveData(lastPos);

            return DecodeResult.Complete;
        }
    }
}
