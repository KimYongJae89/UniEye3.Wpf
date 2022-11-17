using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public class IoLightCtrlInfo : LightCtrlInfo
    {
        public List<IoPort> LightCtrlIoPortList { get; set; } = new List<IoPort>();

        public IoLightCtrlInfo()
        {
            Type = LightCtrlType.IO;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            XmlElement ioLightCtrlElement = lightInfoElement.OwnerDocument.CreateElement("", "IoLightController", "");
            lightInfoElement.AppendChild(ioLightCtrlElement);

            foreach (IoPort ioLightPort in LightCtrlIoPortList)
            {
                XmlElement ioLightPortElement = lightInfoElement.OwnerDocument.CreateElement("", "IoLightPort", "");
                ioLightCtrlElement.AppendChild(ioLightPortElement);

                ioLightPort.Save(ioLightPortElement);
            }
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            XmlElement ioLightCtrlElement = lightInfoElement["IoLightController"];
            if (ioLightCtrlElement != null)
            {
                int lightIndex = 0;
                foreach (XmlElement ioLightPortElement in ioLightCtrlElement)
                {
                    if (ioLightPortElement.Name == "IoLightPort")
                    {
                        string lightName = string.Format("Light{0}", lightIndex);

                        var ioLightPort = new IoPort(IoDirection.Output, lightName, lightName);
                        ioLightPort.Load(ioLightPortElement);
                    }
                }
            }
        }

        public override LightCtrlInfo Clone()
        {
            var IoLightCtrlInfo = new IoLightCtrlInfo();
            IoLightCtrlInfo.Copy(this);

            return IoLightCtrlInfo;
        }

        public override Form GetAdvancedConfigForm()
        {
            return null;
        }
    }

    public class IoLightCtrl : LightCtrl
    {
        private DigitalIoHandler digitalIoHandler;
        private IoPort[] lightPorts;

        public override int NumChannel => lightPorts.Count();

        public IoLightCtrl(string name, DigitalIoHandler digitalIoHandler)
            : base(LightCtrlType.IO, name)
        {
            this.digitalIoHandler = digitalIoHandler;
        }

        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
        {
            var ioLightCtrlInfo = (IoLightCtrlInfo)lightCtrlInfo;

            lightPorts = ioLightCtrlInfo.LightCtrlIoPortList.ToArray();

            return true;
        }

        public override int GetMaxLightLevel()
        {
            return 1;
        }

        public override void TurnOn()
        {
            LogHelper.Debug(LoggerType.Grab, "Turn on light");
            TurnOn(new LightValue(lightPorts.Count(), 256));
        }

        public override void TurnOff()
        {
            LogHelper.Debug(LoggerType.Grab, "Turn off light");
            TurnOn(new LightValue(lightPorts.Count(), 0));
        }

        public override void Release()
        {
            base.Release();
        }

        public int GetNumLight()
        {
            return lightPorts.Count();
        }

        public override void TurnOn(LightValue lightValue)
        {
            DioValue outputValue = digitalIoHandler.ReadOutput(true);
            for (int i = 0; i < outputValue.Count; i++)
            {
                outputValue.UpdateBitFlag(lightPorts[i], (lightValue.Value[StartChannelIndex + i] > 0));
            }

            digitalIoHandler.WriteOutput(outputValue, true);

            Thread.Sleep(LightConfig.Instance().LightStableTime);
        }

        public override void TurnOn(LightValue lightValue, int deviceIndex)
        {

        }

        public override void TurnOff(int deviceIndex)
        {
            throw new NotImplementedException();
        }
    }
}
