using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public enum ESerialDeviceType { SerialEncoder, BarcodeReader, SerialSensor }

    public class SerialDeviceInfo
    {
        public string DeviceName { get; set; }
        public ESerialDeviceType DeviceType { get; set; }
        public SerialPortInfo SerialPortInfo { get; set; } = new SerialPortInfo();

        public bool IsVirtual => SerialPortInfo.PortNo < 0;

        public virtual SerialDevice BuildSerialDevice(bool virtualMode)
        {
            throw new NotImplementedException();
        }

        public virtual SerialDeviceInfo Clone()
        {
            var serialDeviceInfo = new SerialDeviceInfo();
            serialDeviceInfo.CopyFrom(this);

            return serialDeviceInfo;
        }

        public virtual void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            DeviceName = serialDeviceInfo.DeviceName;
            DeviceType = serialDeviceInfo.DeviceType;
            SerialPortInfo = serialDeviceInfo.SerialPortInfo.Clone();
        }

        public void Save(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }
            SaveXml(xmlElement);
        }

        public virtual void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DeviceName", DeviceName);
            XmlHelper.SetValue(xmlElement, "DeviceType", DeviceType.ToString());
            SerialPortInfo.Save(xmlElement, "SerialPortInfo");
        }

        public void Load(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement[subKey];
                Load(subElement);
                return;
            }
            LoadXml(xmlElement);
        }

        public virtual void LoadXml(XmlElement xmlElement)
        {
            DeviceName = XmlHelper.GetValue(xmlElement, "DeviceName", "");
            DeviceType = (ESerialDeviceType)Enum.Parse(typeof(ESerialDeviceType), XmlHelper.GetValue(xmlElement, "DeviceType", ""));
            SerialPortInfo.Load(xmlElement, "SerialPortInfo");
        }
    }

    public class SerialDeviceInfoList : List<SerialDeviceInfo>
    {
        public SerialDeviceInfoList Clone()
        {
            var newSerialPortInfoList = new SerialDeviceInfoList();

            foreach (SerialDeviceInfo serialPortInfo in this)
            {
                newSerialPortInfoList.Add(serialPortInfo.Clone());
            }

            return newSerialPortInfoList;
        }
    }

    public class SerialDevice
    {
        //public EncodePacketDelegate EncodePacket = null;
        //public DecodePacketDelegate DecodePacket = null;

        protected byte[] receiveBuffer;
        protected PacketBuffer packetBuffer = new PacketBuffer();

        protected SerialDeviceInfo deviceInfo = null;
        public SerialDeviceInfo DeviceInfo
        {
            get => deviceInfo;
            set => deviceInfo = value;
        }

        protected bool isAlarmed;
        public bool IsAlarmed => isAlarmed;

        protected SerialPortEx serialPortEx = null;
        public SerialPortEx SerialPortEx => serialPortEx;

        private object lockObject = new object();
        protected ManualResetEvent waitResponce = new ManualResetEvent(false);
        protected string lastResponce = null;
        protected TimeOutTimer timeOutTimer = null;

        public SerialDevice(SerialDeviceInfo deviceInfo)
        {
            this.deviceInfo = deviceInfo;
            ErrorManager.Instance().OnStartAlarm += ErrorManager_OnStartAlarm;
            ErrorManager.Instance().OnResetAlarmStatus += ErrorManager_OnResetAlarmStatus;
        }

        private void ErrorManager_OnStartAlarm()
        {
            isAlarmed = true;
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            isAlarmed = false;
        }

        public virtual bool Initialize()
        {
            if (serialPortEx != null && serialPortEx.IsOpen)
            {
                serialPortEx.Close();
            }

            serialPortEx = new SerialPortEx();

            PacketParser packetParser = CreatePacketParser();
            serialPortEx.PacketHandler = new PacketHandler();
            serialPortEx.PacketHandler.AddPacketParser(packetParser);
            packetParser.DataReceived += packetParser_DataReceived;

            if (deviceInfo.IsVirtual)
            {
                return true;
            }

            bool ok = serialPortEx.Open(deviceInfo.DeviceName, deviceInfo.SerialPortInfo);
            if (ok)
            {
                serialPortEx.StartListening();
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Machine.ToString(), CommonError.FailToInitialize.ToString(), "Fail to Open Port");
            }
            return ok;
        }

        protected virtual void packetParser_DataReceived(ReceivedPacket receivedPacket)
        {
            if (receivedPacket.ReceivedDataByte == null)
            {
                receivedPacket.ReceivedDataByte = Encoding.ASCII.GetBytes(receivedPacket.ReceivedData);
            }

            lastResponce = serialPortEx.PacketHandler.PacketParser.DecodePacket(receivedPacket.ReceivedDataByte);
            if (lastResponce == string.Empty)
            {
                waitResponce.Reset();
                return;
            }

            waitResponce.Set();
        }

        public virtual void Release()
        {
            if (serialPortEx != null)
            {
                serialPortEx.StopListening();
                serialPortEx.Close();
            }
        }

        public virtual Enum GetCommand(string command) { throw new NotImplementedException(); }
        public virtual string MakePacket(string command, params string[] args) { throw new NotImplementedException(); }

        public string[] ExcuteCommand(Enum command, params string[] args)
        {
            return ExcuteCommand(command.ToString(), args);
        }

        public string[] ExcuteCommand(string command, params string[] args)
        {
            string packetString = MakePacket(command, args);
            return ExcuteCommand(packetString);
        }

        public string[] ExcuteCommand(string packetString)
        {
            if (isAlarmed)
            {
                return null;
            }

            lock (lockObject)
            {
                lastResponce = null;
                waitResponce.Reset();

                bool sendOk = SendCommand(packetString);
                if (sendOk == false)
                {
                    return null;
                }

                bool waitOk = WaitResponce(1000);
                if (waitOk == false)
                {
                    LogHelper.Error(string.Format("SerialDevice({0} responce timeout)", deviceInfo.DeviceName));
                    //this.isAlarmed = true;
                    //ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.Serial,
                    //   ErrorLevel.Fatal, MachineError.Serial.ToString(), this.deviceInfo.DeviceName, "Serial Device Responce Timeout");
                    //throw new TimeoutException(string.Format("SerialDevice {0}", this.deviceInfo.DeviceName));

                    return null;
                    //return new string[0];
                }

                string[] token = lastResponce.Split(',');
                lastResponce = null;
                return token;
            }
        }

        protected virtual bool SendCommand(string packetString)
        {
            if (isAlarmed)
            {
                return false;
            }

            byte[] packet = serialPortEx.PacketHandler.PacketParser.EncodePacket(packetString);

            serialPortEx.WritePacket(packet, 0, packet.Length);

            return true;
        }

        private bool WaitResponce(int waitTimeMs = -1)
        {
            return waitResponce.WaitOne(waitTimeMs);
        }

        public virtual PacketParser CreatePacketParser()
        {
            var simplePacketParser = new SimplePacketParser();
            return simplePacketParser;
        }
    }

    public class SerialDeviceHandler
    {
        private List<SerialDevice> serialDeviceList = new List<SerialDevice>();

        public void Add(SerialDevice serialDevice)
        {
            serialDeviceList.Add(serialDevice);
        }

        public SerialDevice Find(Predicate<SerialDevice> p)
        {
            return serialDeviceList.Find(p);
        }

        public void Release()
        {
            serialDeviceList.ForEach(f => f.Release());
            serialDeviceList.Clear();
        }
    }
}
