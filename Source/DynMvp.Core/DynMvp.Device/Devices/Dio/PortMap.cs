using DynMvp.Base;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Dio
{
    public class PortMap
    {
        protected Dictionary<string, IoPort> ioPortMap = new Dictionary<string, IoPort>();

        public Dictionary<string, IoPort>.Enumerator GetEnumerator()
        {
            return ioPortMap.GetEnumerator();
        }

        public IoPort this[string name] => GetPort(name);

        // string[,] portNames = new string[,] { { "InEmergency", "Emergency" },  { "InStartSw", "Start Switch" }, .... };
        public void AddPorts(IoDirection direction, string[,] portNames, int deviceNo = 0, int groupNo = 0)
        {
            string[,] names = new string[,] { { "", "" } };

            for (int portNo = 0; portNo < portNames.Length; portNo++)
            {
                ioPortMap.Add(portNames[portNo, 0], new IoPort(direction, portNames[portNo, 0], portNames[portNo, 1], portNo, deviceNo, groupNo));
            }
        }

        public void AddPort(IoPort ioPort)
        {
            ioPortMap.Add(ioPort.Name, ioPort);
        }

        public IoPort GetPort(string portName)
        {

            if (ioPortMap.TryGetValue(portName, out IoPort ioPort) == false)
            {
                //Debug.Assert(ioPort != null, String.Format("I/O Port {0} is not defined.", portName));
                return null;
            }

            return ioPort;
        }

        public List<IoPort> GetPorts(IoGroup group)
        {
            List<KeyValuePair<string, IoPort>> keyValueList = ioPortMap.ToList().FindAll(x => x.Value.Group == group);

            var ioPortList = new List<IoPort>();
            keyValueList.ForEach(x => ioPortList.Add(x.Value));
            return ioPortList;
        }

        // deviceIndex가 지정되면 groupIndex도 같이 지정되어야 한다.
        public List<IoPort> GetPorts(IoDirection direction, int deviceNo = -1, int groupNo = -1)
        {
            List<KeyValuePair<string, IoPort>> keyValueList;

            if (deviceNo > -1)
            {
                keyValueList = ioPortMap.ToList().FindAll(x => x.Value.Direction == direction && x.Value.DeviceNo == deviceNo && x.Value.GroupNo == groupNo);
            }
            else
            {
                keyValueList = ioPortMap.ToList().FindAll(x => x.Value.Direction == direction);
            }

            var ioPortList = new List<IoPort>();
            keyValueList.ForEach(x => ioPortList.Add(x.Value));
            return ioPortList;
        }

        public string GetPortName(int deviceNo, int portNo)
        {
            IoPort ioPort = ioPortMap.ToList().Find(x => x.Value.DeviceNo == deviceNo && x.Value.PortNo == portNo).Value;

            if (ioPort == null)
            {
                return "";
            }

            return ioPort.Name;
        }

        public bool SetPort(string portName, int deviceNo, int portNo)
        {
            IoPort ioPort = GetPort(portName);
            if (ioPort == null)
            {
                return false;
            }

            ioPort.DeviceNo = deviceNo;
            ioPort.PortNo = portNo;

            return true;
        }

        public void ClearPort(string portName)
        {
            ioPortMap.Remove(portName);
        }

        public List<string> GetPortNames(int deviceIndex, int groupIndex, int numPorts)
        {
            string[] portNames = new string[numPorts];
            ioPortMap.ToList().ForEach(x => { if (x.Value.PortNo != IoPort.UNUSED_PORT_NO && x.Value.GroupNo == groupIndex && x.Value.DeviceNo == deviceIndex) { portNames[x.Value.PortNo] = x.Value.Name; } });

            return portNames.ToList();
        }

        public List<string> GetPortNames()
        {
            var portNames = new List<string>();
            ioPortMap.ToList().ForEach(x => portNames.Add(x.Key));

            return portNames;
        }

        public void Save()
        {
            string fileName = string.Format(@"{0}\PortMap.xml", DeviceConfig.Instance().ConfigPath);

            var xmlDocument = new XmlDocument();

            XmlElement portListElement = xmlDocument.CreateElement("", "PortList", "");
            xmlDocument.AppendChild(portListElement);

            ioPortMap.ToList().ForEach(x =>
            {
                XmlElement portElement = portListElement.OwnerDocument.CreateElement("", "Port", "");
                portListElement.AppendChild(portElement);

                x.Value.Save(portElement);
            });

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = string.Format(@"{0}\PortMap.xml", DeviceConfig.Instance().ConfigPath);

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return;
            }

            XmlElement portListElement = xmlDocument.DocumentElement;
            foreach (XmlElement portElement in portListElement)
            {
                if (portElement.Name == "Port")
                {
                    string portName = XmlHelper.GetValue(portElement, "Name", "");
                    if (string.IsNullOrEmpty(portName) == false)
                    {
                        IoPort ioPort = GetPort(portName);
                        if (ioPort == null)
                        {
                            continue;
                        }

                        ioPort.Load(portElement);
                    }
                }
            }
        }

        public void Clear()
        {
            ioPortMap.Clear();
        }
    }
}
