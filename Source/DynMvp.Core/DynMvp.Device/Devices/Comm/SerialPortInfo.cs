using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public class SerialPortInfo
    {
        public string Name { get; set; } = "NewSerialDevice";

        private string portName;
        public string PortName
        {
            get => portName;
            set
            {
                portName = value;
                if (portName == "None")
                {
                    portName = "";
                }
            }
        }

        public int PortNo
        {
            get
            {
                if (string.IsNullOrEmpty(portName) == false)
                {
                    return int.Parse(string.Join("", portName.Where(char.IsDigit)));
                }
                else
                {
                    return 0;
                }
            }
        }
        public int BaudRate { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public Handshake Handshake { get; set; }
        public bool RtsEnable { get; set; } = false;
        public bool DtrEnable { get; set; } = false;

        public SerialPortInfo(string portName = "", int baudRate = 9600, StopBits stopBits = StopBits.One, Parity parity = Parity.None, int dataBits = 8,
            Handshake handshake = Handshake.None, bool rtsEnable = false, bool dtrEnable = false)
        {
            Initialize(portName, baudRate, stopBits, parity, dataBits, handshake, rtsEnable, dtrEnable);
        }

        public void Initialize(string portName = "", int baudRate = 9600, StopBits stopBits = StopBits.One, Parity parity = Parity.None, int dataBits = 8,
            Handshake handshake = Handshake.None, bool rtsEnable = false, bool dtrEnable = false)
        {
            this.portName = portName;
            BaudRate = baudRate;
            StopBits = stopBits;
            Parity = parity;
            DataBits = dataBits;
            Handshake = handshake;
            RtsEnable = rtsEnable;
            DtrEnable = dtrEnable;
        }

        public SerialPortInfo Clone()
        {
            var serialPortInfo = new SerialPortInfo();
            serialPortInfo.Copy(this);

            return serialPortInfo;
        }

        public void Copy(SerialPortInfo srcInfo)
        {
            Name = srcInfo.Name;
            portName = srcInfo.portName;
            BaudRate = srcInfo.BaudRate;
            StopBits = srcInfo.StopBits;
            Parity = srcInfo.Parity;
            DataBits = srcInfo.DataBits;
            Handshake = srcInfo.Handshake;
            RtsEnable = srcInfo.RtsEnable;
            DtrEnable = srcInfo.DtrEnable;
        }

        public bool Load(XmlElement xmlElement, string keyName)
        {
            XmlElement serialPortElement = xmlElement[keyName];
            if (serialPortElement == null)
            {
                return false;
            }

            Name = XmlHelper.GetValue(serialPortElement, "Name", "NewSerialDevice");
            portName = XmlHelper.GetValue(serialPortElement, "PortName", "None");
            BaudRate = Convert.ToInt32(XmlHelper.GetValue(serialPortElement, "BaudRate", "9600"));
            StopBits = (StopBits)Enum.Parse(typeof(StopBits), XmlHelper.GetValue(serialPortElement, "StopBits", "One"));
            Parity = (Parity)Enum.Parse(typeof(Parity), XmlHelper.GetValue(serialPortElement, "Parity", "None"));
            DataBits = Convert.ToInt32(XmlHelper.GetValue(serialPortElement, "DataBits", "8"));
            Handshake = (Handshake)Enum.Parse(typeof(Handshake), XmlHelper.GetValue(serialPortElement, "HandShake", "None"));
            RtsEnable = Convert.ToBoolean(XmlHelper.GetValue(serialPortElement, "RtsEnable", "False"));
            DtrEnable = Convert.ToBoolean(XmlHelper.GetValue(serialPortElement, "DtrEnable", "False"));

            return true;
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement serialPortElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(serialPortElement);

            XmlHelper.SetValue(serialPortElement, "Name", Name);
            XmlHelper.SetValue(serialPortElement, "PortName", portName);
            XmlHelper.SetValue(serialPortElement, "BaudRate", BaudRate.ToString());
            XmlHelper.SetValue(serialPortElement, "StopBits", StopBits.ToString());
            XmlHelper.SetValue(serialPortElement, "Parity", Parity.ToString());
            XmlHelper.SetValue(serialPortElement, "DataBits", DataBits.ToString());
            XmlHelper.SetValue(serialPortElement, "HandShake", Handshake.ToString());
            XmlHelper.SetValue(serialPortElement, "RtsEnable", RtsEnable.ToString());
            XmlHelper.SetValue(serialPortElement, "DtrEnable", DtrEnable.ToString());
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}", portName, BaudRate.ToString(), StopBits.ToString(), Parity.ToString(), DataBits.ToString(),
                Handshake.ToString(), RtsEnable.ToString(), DtrEnable.ToString());
        }
    }

    public class SerialPortInfoList : List<SerialPortInfo>
    {
        public SerialPortInfoList Clone()
        {
            var newSerialPortInfoList = new SerialPortInfoList();

            foreach (SerialPortInfo serialPortInfo in this)
            {
                newSerialPortInfoList.Add(serialPortInfo.Clone());
            }

            return newSerialPortInfoList;
        }
    }
}
