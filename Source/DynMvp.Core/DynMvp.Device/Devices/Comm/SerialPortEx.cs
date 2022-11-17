using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Comm
{
    public delegate void PacketTransferredDelegate(byte[] dataByte);

    public class SerialPortEx
    {
        public SerialPort SerialPort { get; set; } = new SerialPort();

        private bool virtualPort = false;
        public string Name { get; set; }

        public string PortName
        {
            get => SerialPort.PortName;
            set => SerialPort.PortName = value;
        }
        public PacketData PacketData { get; } = new PacketData();
        public PacketHandler PacketHandler { get; set; } = new PacketHandler();

        public PacketTransferredDelegate PacketReceived;
        public PacketTransferredDelegate PacketTransmitted;

        public SerialPortEx() { }

        public SerialPortEx(PacketParser packetParser)
        {
            PacketHandler = new PacketHandler(packetParser);
        }

        public bool Open(string name, SerialPortInfo serialPortInfo)
        {
            if (serialPortInfo.PortName != "")
            {
                if (serialPortInfo.PortName != "Virtual")
                {
                    if (SerialPortManager.Instance().IsPortAvailable(serialPortInfo.PortName) == false)
                    {
                        return false;
                    }

                    Name = name;

                    SerialPort.PortName = serialPortInfo.PortName;
                    SerialPort.BaudRate = serialPortInfo.BaudRate;
                    SerialPort.DataBits = serialPortInfo.DataBits;
                    SerialPort.StopBits = serialPortInfo.StopBits;
                    SerialPort.Parity = serialPortInfo.Parity;
                    SerialPort.RtsEnable = serialPortInfo.RtsEnable;
                    SerialPort.DtrEnable = serialPortInfo.DtrEnable;

                    try
                    {
                        SerialPort.Open();

                        SerialPortManager.Instance().AddSerialPort(this);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(string.Format("Serial Port Open Error : Name - {0} / Msg - {1} ", name, ex.Message));
                        return false;
                    }
                }
                else
                {
                    virtualPort = true;
                }
            }
            else
            {
                virtualPort = true;
            }

            return true;
        }

        public bool IsOpen => SerialPort.IsOpen;

        public void Close()
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.Close();
                SerialPortManager.Instance().ReleaseSerialPort(this);
            }
        }

        public void StartListening()
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.DiscardInBuffer();

                if (virtualPort == false)
                {
                    SerialPort.DataReceived += PortDataReceived;
                }
            }
        }

        public void Purge()
        {
            if (SerialPort.IsOpen)
            {
                SerialPort.DiscardInBuffer();
            }
        }

        public void StopListening()
        {
            if (virtualPort == false)
            {
                SerialPort.DataReceived -= PortDataReceived;
            }
        }

        // MSYS : 시리얼 데이터 받는 위치
        private void PortDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                int byteToRead = SerialPort.BytesToRead;

                byte[] dataByte = new byte[byteToRead];
                try
                {
                    SerialPort.Read(dataByte, 0, byteToRead);
                    ProcessPacket(dataByte);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("Serial Port Read Error : Name - {0} / Msg - {1} ", Name, ex.Message));
                }
            }

        }

        // 가상 모드 동작을 위해 별도의 함수로 분리
        public void ProcessPacket(byte[] dataByte)
        {
            if (PacketReceived != null)
            {
                PacketReceived(dataByte);
            }
            else if (PacketHandler != null)
            {
                PacketHandler.ProcessPacket(dataByte, PacketData);
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Packet Handler is Empty.");
            }
        }

        public void SendRequest()
        {
            PacketParser packetParser = PacketHandler.PacketParser;

            if (packetParser != null)
            {
                if (SerialPort.IsOpen == true)
                {
                    byte[] dataByte = packetParser.GetRequestPacket();
                    WritePacket(dataByte, 0, dataByte.Length);

                    string writePacket = System.Text.Encoding.Default.GetString(dataByte.ToArray());
                    LogHelper.Debug(LoggerType.Comm, "Write Packet : " + writePacket);
                }
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Packet Parser is Empty.");
            }
        }

        public void ReleaseEventHandler()
        {
            //            DataReceived -= new SerialDataReceivedEventHandler(PortDataReceived);
        }

        public void WritePacket(byte[] buffer, int offset, int count)
        {
            if (virtualPort == false)
            {
                SerialPort.Write(buffer, offset, count);
            }

            PacketTransmitted?.Invoke(buffer);
        }

        public void WritePacket(string packet)
        {
            byte[] bytes = Encoding.Default.GetBytes(packet);
            if (virtualPort == false)
            {
                //serialPort.Write(packet);
                WritePacket(bytes, 0, bytes.Count());
            }

            //if (PacketTransmitted != null)
            //    PacketTransmitted(Encoding.Default.GetBytes(packet));
        }
    }
}
