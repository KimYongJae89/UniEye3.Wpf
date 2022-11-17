using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public class VisorInfo
    {
        public string Name { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public int DataPort { get; set; } = 2005;
        public int CommandPort { get; set; } = 2006;
        public string ResultPath { get; set; }

        public VisorInfo()
        {
            ResultPath = string.Format("{0}\\..\\Result\\Visor", Environment.CurrentDirectory);
            if (Directory.Exists(ResultPath) == false)
            {
                Directory.CreateDirectory(ResultPath);
            }
        }


        public void Load(XmlElement xmlElement)
        {
            Name = XmlHelper.GetValue(xmlElement, "Name", "");
            IpAddress = XmlHelper.GetValue(xmlElement, "IpAddress", "");
            DataPort = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "DataPort", "2005"));
            CommandPort = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CommandPort", "2006"));

            string resultPath = XmlHelper.GetValue(xmlElement, "VisorResult", ResultPath);
            if (Directory.Exists(resultPath) == true)
            {
                ResultPath = resultPath;
            }
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Name", Name);
            XmlHelper.SetValue(xmlElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(xmlElement, "DataPort", DataPort.ToString());
            XmlHelper.SetValue(xmlElement, "CommandPort", CommandPort.ToString());
            XmlHelper.SetValue(xmlElement, "VisorResult", ResultPath);
        }

        public VisorInfo Clone()
        {
            var cloneVisor = new VisorInfo();
            cloneVisor.Name = Name;
            cloneVisor.IpAddress = IpAddress;
            cloneVisor.DataPort = DataPort;
            cloneVisor.CommandPort = CommandPort;
            cloneVisor.ResultPath = ResultPath;

            return cloneVisor;
        }
    }
}
