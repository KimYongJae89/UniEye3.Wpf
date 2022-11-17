using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Component.DepthSystem
{
    public class ExymaPacketHandler : PacketParser
    {
        public override byte[] GetRequestPacket()
        {
            return Encoding.ASCII.GetBytes("GIM0");
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            string packetString = System.Text.Encoding.Default.GetString(packetContents);
            if (packetString.Contains("OK") || packetString.Contains("ERR") || packetString.Contains('G') || packetString.Contains('f') || packetString.Contains('+') || packetString.Contains('E'))
            {
                var receivedPacket = new ReceivedPacket();
                receivedPacket.ReceivedData = packetString;
                DataReceived(receivedPacket);

                return true;
            }

            return false;
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

    public class ExymaController
    {
        private ExymaScannerInfo exymaScannerInfo;
        private SerialPortEx serialPortEx;
        private ManualResetEvent dataReceivedEvent;
        private bool packetResult = false;
        private string packetString;

        public void Initialize(ExymaScannerInfo exymaScannerInfo, SerialPortInfo serialPortInfo)
        {
            this.exymaScannerInfo = exymaScannerInfo;

            Connect(serialPortInfo);

            //for (EEPROM eepromIndex = EEPROM.S1EN; eepromIndex < EEPROM.END; eepromIndex++)
            //{
            //    SetRamData(eepromIndex, exymaScannerInfo.BoardSettingData[(int)eepromIndex]);
            //}
        }

        public void Connect(SerialPortInfo serialPortInfo)
        {
            serialPortEx = new SerialPortEx();
            serialPortEx.Open("Exyma", serialPortInfo);
            serialPortEx.StartListening();

            var packetHandler = new ExymaPacketHandler();
            packetHandler.DataReceived = packetHandler_DataReceived;
            serialPortEx.PacketHandler.PacketParser = packetHandler;

            dataReceivedEvent = new ManualResetEvent(false);
        }

        private void packetHandler_DataReceived(ReceivedPacket receivedPacket)
        {
            packetString = receivedPacket.ReceivedData;
            packetResult = (packetString.Contains("E") != true);
            dataReceivedEvent.Set();
        }

        private string MakeCommand(char command, long data)
        {
            string dataString = data.ToString();

            byte transCheck = 0;
            for (int j = 0; j < dataString.Length; j++)
            {
                transCheck ^= (byte)dataString[j];
            }

            if (transCheck == 0)
            {
                transCheck = (byte)'#';  //  PYC
            }

            return string.Format("{0}{1}e{2}", command, dataString, transCheck.ToString());
        }

        private bool WriteCommand(string command)
        {
            serialPortEx.Purge();
            packetResult = false;

            byte[] data = Encoding.ASCII.GetBytes(command);
            serialPortEx.WritePacket(data, 0, data.Length);

            TimeOutHandler.Wait(1000, dataReceivedEvent);

            return packetResult;
        }

        public bool MotorOnOff(bool flag)
        {
            bool result = false;
            if (flag == true)
            {
                result = WriteCommand(MakeCommand('a', 1));
            }
            else
            {
                result = WriteCommand(MakeCommand('s', 1));
            }

            return result;
        }

        public bool SetMotorSpeed(int RPM)
        {
            return WriteCommand(MakeCommand('b', RPM));
        }

        public bool LaserOnOff(bool flag)
        {
            bool result = false;
            if (flag == true)
            {
                result = WriteCommand(MakeCommand('l', 1));
            }
            else
            {
                result = WriteCommand(MakeCommand('o', 1));
            }

            return result;
        }

        public bool Set_T1(int time)
        {
            return WriteCommand(MakeCommand('z', time));
        }

        public bool Set_T2_1(int time)
        {
            return WriteCommand(MakeCommand('c', time));
        }

        public bool Set_T2_2(int time)
        {
            return WriteCommand(MakeCommand('w', time));
        }

        public bool Set_T3(int time)
        {
            return WriteCommand(MakeCommand('x', time));
        }

        public bool SetOffsetLamda1(int time)
        {
            return WriteCommand(MakeCommand('m', time));
        }

        public bool SetOffsetLamda2(int time)
        {
            return WriteCommand(MakeCommand('n', time));
        }

        public bool SetPhaseShiftLamda1()
        {
            return WriteCommand("*");
        }

        public bool SetPhaseShiftLamda2()
        {
            return WriteCommand("&");
        }

        public bool SetChangePeriodLamda1()
        {
            return WriteCommand("(");
        }

        public bool ChangePeriodLamda2()
        {
            return WriteCommand(")");
        }

        public bool SetGenerateLatticeWBS()
        {
            return WriteCommand("Q");
        }

        public bool GetRamData(EEPROM eeprom, ref uint value)
        {
            string command = string.Format("_PR {0}{1}", eeprom.ToString(), 0x0D);

            WriteCommand(command);

            for (int i = 0; i < packetString.Length; i++)
            {
                if (packetString[i] < '0' || packetString[i] > '9')
                {
                    continue;
                }

                value = Convert.ToUInt32(packetString.Skip(i));
                return true;
            }

            return false;
        }

        public bool SetRamData(EEPROM eeprom, uint value)
        {
            string command = string.Format("_{0} {1}\r", eeprom.ToString(), value);
            return WriteCommand(command);
        }
    }
}
