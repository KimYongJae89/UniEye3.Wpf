using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Comm
{
    public class TensionPacketHandler : PacketHandler
    {
        public TensionPacketHandler()
        {
            StartChar = null;
            EndChar = Encoding.ASCII.GetBytes("\r");

            PacketParser = new TensionPacketPaser();
        }
    }

    public class TensionReceivedPacket : ReceivedPacket
    {
        public string ResultValue { get; set; }
    }

    public class TensionPacketPaser : PacketParser
    {
        private string lastString;

        public override byte[] GetRequestPacket()
        {
            byte[] dataByte = new byte[2];
            dataByte[0] = 0x0d;
            dataByte[1] = 0x0a;
            return dataByte;
        }
        public override bool ParsePacket(byte[] PacketContents)
        {
            string currentString = System.Text.Encoding.Default.GetString(PacketContents.ToArray());

            LogHelper.Debug(LoggerType.Comm, string.Format("Received Valid Packet : {0}", currentString));
            lastString = currentString;
            try
            {
                if (DataReceived != null)
                {
                    var receivedPacket = new TensionReceivedPacket();
                    string result = lastString.Substring(3); //.Remove(0 , 3);
                    float convertResult = Convert.ToSingle(result);

                    receivedPacket.ResultValue = convertResult.ToString();
                    DataReceived(receivedPacket);
                }
                else
                {
                    LogHelper.Debug(LoggerType.Comm, "DataReceived is Empty");
                }
            }
            catch (FormatException)
            {
                LogHelper.Debug(LoggerType.Comm, string.Format("Invalid string format"));
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
