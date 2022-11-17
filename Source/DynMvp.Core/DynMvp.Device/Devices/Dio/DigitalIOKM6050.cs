using DynMvp.Base;
using DynMvp.Devices.Comm;
using ModbusRTU;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoKM6050 : DigitalIo
    {
        private object lockObject = new object();
        private SerialPortEx serialDIO;
        private PacketParser packetParser;
        private string readInputData = "";
        private uint readOutput = 0;
        private uint readInput = 0;

        public DigitalIoKM6050(string name)
            : base(DigitalIoType.KM6050, name)
        {
            NumInPort = 7;
            NumOutPort = 8;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            var serialDigitalIoInfo = (SerialDigitalIoInfo)digitalIoInfo;

            serialDIO = new SerialPortEx();

            packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataRecived;

            serialDIO.PacketHandler.EndChar = new byte[1] { (byte)'\r' };
            serialDIO.PacketHandler.AddPacketParser(packetParser);
            serialDIO.Open("KM6050", serialDigitalIoInfo.SerialPortInfo);
            serialDIO.StartListening();

            return true;
        }

        private void DataRecived(ReceivedPacket receivedPacket)
        {
            readInputData = receivedPacket.ReceivedData;
        }

        public override bool IsReady()
        {
            return true;
        }

        public override void Release()
        {
            base.Release();
            serialDIO.Close();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            LogHelper.Debug(LoggerType.Device, "WriteOutputGroup");
            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.Device, "Start WriteOutputGroup");

                    bool[] value = new bool[NumOutPort];
                    //Write
                    string packet = string.Format("#01000{0}", outputPortStatus);
                    serialDIO.WritePacket(packet);

                    serialDIO.WritePacket(new byte[] { 13 }, 0, 1);

                    LogHelper.Debug(LoggerType.Device, "End WriteOutputGroup");
                    readOutput = outputPortStatus;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(string.Format("KM6050 Timeout Error : WriteOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(string.Format("KM6050 CRC Error : WriteOutputGroup {0}", ex.Message));
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.Device, "ReadOutputGroup");
            return readOutput;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.Device, "ReadInputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.Device, "Start ReadInputGroup");


                    string packet = string.Format("$016");
                    serialDIO.WritePacket(packet);
                    serialDIO.WritePacket(new byte[] { 13 }, 0, 1);
                    Thread.Sleep(50);

                    if (string.IsNullOrEmpty(readInputData))
                    {
                        return 0;
                    }

                    GetData();

                    return readInput;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(string.Format("KM6050 Timeout Error : ReadInputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(string.Format("KM6050 CRC Error : ReadInputGroup {0}", ex.Message));
            }

            return 0;
        }

        private void GetData()
        {
            string tempReadInputData;
            tempReadInputData = readInputData;

            if (tempReadInputData.Length != 7)
            {
                return;
            }

            if (tempReadInputData[0] != '!')
            {
                return;
            }

            string result = tempReadInputData;
            result = result.Substring(4, 3);

            uint result2 = Convert.ToUInt32(result[0].ToString(), 16);

            readInput = 15 - result2;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            // Do nothing
        }
    }
}
