using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelProperty
    {
        public string Name { get; set; }
        public DaqChannelType DaqChannelType { get; set; }
        public double SamplingHz { get; set; } = 25000.0;
        public double MinValue { get; set; } = -10;
        public double MaxValue { get; set; } = 10;
        public double ScaleFactor { get; set; } = 1;
        public double ValueOffset { get; set; } = 0;
        public string ChannelName { get; set; } = "dev1/ai0";
        public string DeviceName { get; set; } = "";
        public int ResisterValue { get; set; }
        public bool UseCustomScale { get; set; }

        public DaqChannelProperty(DaqChannelType daqChannelType)
        {
            DaqChannelType = daqChannelType;
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement daqPropertyElement = xmlElement[keyName];
            if (daqPropertyElement == null)
            {
                return;
            }

            LoadXml(daqPropertyElement);
        }

        public virtual void LoadXml(XmlElement daqPropertyElement)
        {
            Name = XmlHelper.GetValue(daqPropertyElement, "Name", "");
            DaqChannelType = (DaqChannelType)Enum.Parse(typeof(DaqChannelType), XmlHelper.GetValue(daqPropertyElement, "DaqChannelType", "Nidaq"));
            SamplingHz = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "SamplingHz", "1000"));
            MinValue = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "MinValue", "-10"));
            MaxValue = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "MaxValue", "10"));
            ChannelName = XmlHelper.GetValue(daqPropertyElement, "ChannelName", "");
            DeviceName = XmlHelper.GetValue(daqPropertyElement, "DeviceName", "");
            ScaleFactor = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "ScaleFactor", "1"));
            ValueOffset = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "ValueOffset", "0"));
            ResisterValue = Convert.ToInt32(XmlHelper.GetValue(daqPropertyElement, "ResistanceValue", "0"));
            UseCustomScale = Convert.ToBoolean(XmlHelper.GetValue(daqPropertyElement, "UseCustomScale", "false"));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement daqPropertyElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(daqPropertyElement);

            SaveXml(daqPropertyElement);
        }

        public virtual void SaveXml(XmlElement daqPropertyElement)
        {
            XmlHelper.SetValue(daqPropertyElement, "Name", Name);
            XmlHelper.SetValue(daqPropertyElement, "DaqChannelType", DaqChannelType.ToString());
            XmlHelper.SetValue(daqPropertyElement, "SamplingHz", SamplingHz.ToString());
            XmlHelper.SetValue(daqPropertyElement, "MinValue", MinValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "MaxValue", MaxValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ChannelName", ChannelName);
            XmlHelper.SetValue(daqPropertyElement, "DeviceName", DeviceName.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ScaleFactor", ScaleFactor.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ValueOffset", ValueOffset.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ResistanceValue", ResisterValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "UseCustomScale", UseCustomScale.ToString());
        }

        public DaqChannelProperty Clone()
        {
            var daqChannelProperty = new DaqChannelProperty(DaqChannelType);
            daqChannelProperty.Copy(this);

            return daqChannelProperty;
        }

        public void Copy(DaqChannelProperty srcDaqChannelProperty)
        {
            Name = srcDaqChannelProperty.Name;
            DaqChannelType = srcDaqChannelProperty.DaqChannelType;
            SamplingHz = srcDaqChannelProperty.SamplingHz;
            MinValue = srcDaqChannelProperty.MinValue;
            MaxValue = srcDaqChannelProperty.MaxValue;
            ChannelName = srcDaqChannelProperty.ChannelName;
            DeviceName = srcDaqChannelProperty.DeviceName;
            ScaleFactor = srcDaqChannelProperty.ScaleFactor;
            ValueOffset = srcDaqChannelProperty.ValueOffset;
            ResisterValue = srcDaqChannelProperty.ResisterValue;
            UseCustomScale = srcDaqChannelProperty.UseCustomScale;
        }
    }

    public class DaqChannelPropertyFactory
    {
        public static DaqChannelProperty Create(DaqChannelType daqChannelType)
        {
            switch (daqChannelType)
            {
                case DaqChannelType.MeDAQ:
                    return new DaqChannelMedaqProperty(DaqChannelType.MeDAQ);
                default:
                    return new DaqChannelProperty(daqChannelType);
            }
        }
    }

    public class DaqChannelPropertyList : List<DaqChannelProperty>
    {
        public DaqChannelPropertyList Clone()
        {
            var newLightCtrlInfoList = new DaqChannelPropertyList();

            foreach (DaqChannelProperty daqChannelProperty in this)
            {
                newLightCtrlInfoList.Add(daqChannelProperty.Clone());
            }

            return newLightCtrlInfoList;
        }
    }
}
