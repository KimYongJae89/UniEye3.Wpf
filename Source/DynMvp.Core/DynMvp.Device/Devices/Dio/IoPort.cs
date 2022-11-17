using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Dio
{
    public enum IoDirection { Input, Output }
    public enum IoGroup { General, Emergency, AirPressure, Door, Light, ServoAlarm, Origin, ETC }

    public class IoPort
    {
        public const int UNUSED_PORT_NO = -1;
        public static IoPort UnkownPort = new IoPort(IoDirection.Input, "Unknown", "Unknown");
        public int DeviceNo { get; set; }
        public int GroupNo { get; set; }
        public int PortNo { get; set; } = UNUSED_PORT_NO;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public IoGroup Group { get; set; } = IoGroup.General;
        public IoDirection Direction { get; set; } = IoDirection.Input;
        public bool Invert { get; set; } = false;
        public bool ReleaseOn { get; set; } = false;

        public IoPort(IoDirection direction, string name, string description = null, int portNo = UNUSED_PORT_NO, int deviceNo = 0, int groupNo = 0)
        {
            Direction = direction;
            Name = name;
            PortNo = portNo;
            DeviceNo = deviceNo;
            GroupNo = groupNo;

            if (description == null)
            {
                Description = name;
            }
            else
            {
                Description = description;
            }
        }

        public void Set(int portNo, int deviceNo = 0)
        {
            PortNo = portNo;
            GroupNo = 0;
            DeviceNo = deviceNo;
        }

        public void Set(int portNo, int groupNo, int deviceNo)
        {
            PortNo = portNo;
            GroupNo = groupNo;
            DeviceNo = deviceNo;
        }

        public bool IsValid()
        {
            return PortNo != UNUSED_PORT_NO;
        }

        public void Load(XmlElement ioPortElement)
        {
            XmlHelper.SetValue(ioPortElement, "Direction", Direction.ToString());
            XmlHelper.SetValue(ioPortElement, "PortNo", PortNo.ToString());
            XmlHelper.SetValue(ioPortElement, "GroupNo", GroupNo.ToString());
            XmlHelper.SetValue(ioPortElement, "DeviceNo", DeviceNo.ToString());
            XmlHelper.SetValue(ioPortElement, "Invert", Invert.ToString());
            XmlHelper.SetValue(ioPortElement, "Group", Group.ToString());
        }

        public void Save(XmlElement ioPortElement)
        {
            PortNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "PortNo", "0"));
            GroupNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "GroupNo", "0"));
            DeviceNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "DeviceNo", "0"));
            Invert = Convert.ToBoolean(XmlHelper.GetValue(ioPortElement, "Invert", "false"));
            Group = (IoGroup)Enum.Parse(typeof(IoGroup), XmlHelper.GetValue(ioPortElement, "Group", IoGroup.General.ToString()));
            Direction = (IoDirection)Enum.Parse(typeof(IoDirection), XmlHelper.GetValue(ioPortElement, "Direction", IoDirection.Input.ToString()));
        }
    }
}
