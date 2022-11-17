using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Comm
{
    public class CasLoadCellReceivedPacket : ReceivedPacket
    {
        public float ResultValue { get; set; }

        public CasLoadCellReceivedPacket(float resultValue)
        {
            ResultValue = resultValue;
        }
    }

    public class CasLoadCellProtocol : IProtocol
    {
        public DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket)
        {
            string packetString = packetBuffer.ToString();
            LogHelper.Debug(LoggerType.Comm, string.Format("Received Packet : {0}", packetString));

            if (packetBuffer.DataByteFull.Length >= 21)
            {
                LogHelper.Debug(LoggerType.Comm, "Packet is valid.");

                int startPos = GetValidStartPos(packetString);

                if (startPos > -1)
                {
                    string onePacket = packetString.Substring(startPos);
                    string startString = onePacket.Substring(0, 2);

                    char sign = onePacket[8];
                    string weightString = onePacket.Substring(9, 8).Trim();

                    try
                    {
                        float resultValue = (float)Convert.ToDouble(weightString);

                        if (sign == '-')
                        {
                            resultValue *= -1;
                        }

                        receivedPacket = new CasLoadCellReceivedPacket(resultValue);

                        return DecodeResult.Complete;
                    }
                    catch (FormatException ex)
                    {
                        LogHelper.Debug(LoggerType.Comm, string.Format("Invalid string format : {0} - {1}", weightString, ex.Message));
                    }
                }

                receivedPacket = new CasLoadCellReceivedPacket(0);

                return DecodeResult.PacketError;
            }
            else
            {
                receivedPacket = new CasLoadCellReceivedPacket(0);
            }

            return DecodeResult.Incomplete;
        }

        public byte[] EncodePacket(SendPacket sendPacket)
        {
            return new byte[1] { 1 };
        }

        private int GetValidStartPos(string fullString)
        {
            string[] validStarters = new string[] { "ST", "US", "OL" };

            foreach (string starter in validStarters)
            {
                int startPos = fullString.IndexOf(starter);
                if (startPos > -1)
                {
                    return startPos;
                }
            }

            return -1;
        }
    }


    public class CasLoadCellPacketParser : PacketParser
    {
        private string lastString;

        public override byte[] GetRequestPacket()
        {
            byte[] dataByte = new byte[1];
            dataByte[0] = 1;

            return dataByte;
        }

        public override bool ParsePacket(byte[] dataByte)
        {
            string packetString = System.Text.Encoding.Default.GetString(dataByte.ToArray());
            LogHelper.Debug(LoggerType.Comm, string.Format("Received Packet : {0}", packetString));

            if (dataByte.Length >= 21)
            {
                string currentString = System.Text.Encoding.Default.GetString(dataByte.ToArray());
                //if (lastString == currentString)
                //    return true;

                LogHelper.Debug(LoggerType.Comm, string.Format("Received Valid Packet : {0}", currentString));
                lastString = currentString;

                int startPos = GetValidStartPos(currentString);

                if (startPos > -1)
                {
                    string onePacket = currentString.Substring(startPos);
                    string startString = onePacket.Substring(0, 2);

                    char sign = onePacket[8];
                    string weightString = onePacket.Substring(9, 8).Trim();

                    try
                    {
                        float resultValue = (float)Convert.ToDouble(weightString);

                        if (sign == '-')
                        {
                            resultValue *= -1;
                        }

                        if (DataReceived != null)
                        {
                            var receivedPacket = new CasLoadCellReceivedPacket(resultValue);
                            DataReceived(receivedPacket);
                        }
                        else
                        {
                            LogHelper.Debug(LoggerType.Comm, "DataReceived is Empty");
                        }
                    }
                    catch (FormatException ex)
                    {
                        LogHelper.Debug(LoggerType.Comm, string.Format("Invalid string format : {0} - {1}", weightString, ex.Message));
                    }
                }
            }

            return true;
        }

        private int GetValidStartPos(string fullString)
        {
            string[] validStarters = new string[] { "ST", "US", "OL" };

            foreach (string starter in validStarters)
            {
                int startPos = fullString.IndexOf(starter);
                if (startPos > -1)
                {
                    return startPos;
                }
            }

            return -1;
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

    public class CasLoadCellPacketHandler : PacketHandler
    {
        public CasLoadCellPacketHandler()
        {
            StartChar = null;
            EndChar = Encoding.ASCII.GetBytes("\n");

            PacketParser = new CasLoadCellPacketParser();
        }
    }
}
