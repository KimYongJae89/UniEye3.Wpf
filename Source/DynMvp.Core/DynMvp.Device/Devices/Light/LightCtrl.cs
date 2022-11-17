using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light.SerialLigth.iCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public enum LightCtrlType
    {
        None, IO, Serial
    }

    public enum LightCtrlSubType
    {
        None, Iovis, Movis, ALT, LFine, LVS, CCS, VIT, iCore
    }

    public class InvalidLightSizeException : ApplicationException
    {

    }

    public abstract class LightCtrlInfo
    {
        public string Name { get; set; } = "";
        public LightCtrlType Type { get; set; }
        public LightCtrlSubType SubType { get; set; }
        public int NumChannel { get; set; }

        public virtual void SaveXml(XmlElement lightElement)
        {
            XmlHelper.SetValue(lightElement, "Name", Name.ToString());
            XmlHelper.SetValue(lightElement, "LightCtrlType", Type.ToString());
            XmlHelper.SetValue(lightElement, "LightCtrlSubType", SubType.ToString());
            XmlHelper.SetValue(lightElement, "NumChannel", NumChannel.ToString());
        }

        public virtual void LoadXml(XmlElement lightInfoElement)
        {
            Name = XmlHelper.GetValue(lightInfoElement, "Name", "");
            Type = (LightCtrlType)Enum.Parse(typeof(LightCtrlType), XmlHelper.GetValue(lightInfoElement, "LightCtrlType", LightCtrlType.IO.ToString()));
            SubType = (LightCtrlSubType)Enum.Parse(typeof(LightCtrlSubType), XmlHelper.GetValue(lightInfoElement, "LightCtrlSubType", LightCtrlSubType.None.ToString()));
            NumChannel = Convert.ToInt32(XmlHelper.GetValue(lightInfoElement, "NumChannel", ""));
        }

        public abstract System.Windows.Forms.Form GetAdvancedConfigForm();

        public abstract LightCtrlInfo Clone();

        public virtual void Copy(LightCtrlInfo srcInfo)
        {
            Name = srcInfo.Name;
            Type = srcInfo.Type;
            SubType = srcInfo.SubType;
            NumChannel = srcInfo.NumChannel;
        }
    }

    public class LightCtrlInfoList : List<LightCtrlInfo>
    {
        public LightCtrlInfoList Clone()
        {
            var newLightCtrlInfoList = new LightCtrlInfoList();

            foreach (LightCtrlInfo lightCtrlInfo in this)
            {
                newLightCtrlInfoList.Add(lightCtrlInfo.Clone());
            }

            return newLightCtrlInfoList;
        }
    }

    public class LightCtrlInfoFactory
    {
        public static LightCtrlInfo Create(LightCtrlType lightCtrlType, LightCtrlSubType lightCtrlSubType)
        {
            switch (lightCtrlType)
            {
                case LightCtrlType.IO:
                    return new IoLightCtrlInfo();
                case LightCtrlType.Serial:
                    {
                        switch (lightCtrlSubType)
                        {
                            case LightCtrlSubType.iCore: return new SerialLightCtrlInfoIPulse();
                            default: return new SerialLightCtrlInfo();
                        }
                    }
            }

            return null;
        }
    }

    public class LightCtrlFactory
    {
        public static LightCtrl Create(LightCtrlInfo lightCtrlInfo, DigitalIoHandler digitalIoHandler, bool isVirtualMode)
        {
            LightCtrl lightCtrl = null;

            if (isVirtualMode)
            {
                lightCtrl = new LightCtrlVirtual(LightCtrlType.None, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
            }
            else
            {
                switch (lightCtrlInfo.Type)
                {
                    case LightCtrlType.IO:
                        lightCtrl = new IoLightCtrl(lightCtrlInfo.Name, digitalIoHandler);
                        break;
                    case LightCtrlType.Serial:
                        switch (lightCtrlInfo.SubType)
                        {
                            case LightCtrlSubType.ALT:
                                lightCtrl = new AltLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.CCS:
                                lightCtrl = new CcsLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.Iovis:
                                lightCtrl = new IovisLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.Movis:
                                lightCtrl = new MovisLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.LFine:
                                lightCtrl = new LfineLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.LVS:
                                lightCtrl = new LvsLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.VIT:
                                lightCtrl = new VitLightCtrl(lightCtrlInfo.Name);
                                break;
                            case LightCtrlSubType.iCore:
                                lightCtrl = new SerialLightIPulse(lightCtrlInfo);
                                break;
                        }
                        break;
                }
            }

            if (lightCtrl == null)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToCreate, ErrorLevel.Error,
                    ErrorSection.Light.ToString(), CommonError.FailToCreate.ToString(), string.Format("Can't create light controller. {0}", lightCtrlInfo.Type.ToString()));
                return null;
            }

            if (lightCtrl.Initialize(lightCtrlInfo) == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                    ErrorSection.Light.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Can't initialize light controller. {0}", lightCtrlInfo.Type.ToString()));

                lightCtrl = new LightCtrlVirtual(lightCtrlInfo.Type, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
                lightCtrl.UpdateState(DeviceState.Error, "Light controller is invalid.");
            }
            else
            {
                lightCtrl.UpdateState(DeviceState.Ready, "Light controller initialization succeeded.");
            }

            return lightCtrl;
        }
    }

    public abstract class LightCtrl : Device
    {
        public LightCtrlType LightCtrlType { get; set; }
        public int StartChannelIndex { get; set; }

        public int EndChannelIndex => StartChannelIndex + NumChannel;

        protected bool isToggleLight;
        public bool IsToggleLight => isToggleLight;

        public LightCtrl(LightCtrlType lightCtrlType, string name)
        {
            if (name == "")
            {
                Name = lightCtrlType.ToString();
            }
            else
            {
                Name = name;
            }

            DeviceType = DeviceType.LightController;
            LightCtrlType = lightCtrlType;
            UpdateState(DeviceState.Idle);
        }

        public abstract int GetMaxLightLevel();
        public abstract int NumChannel { get; }
        public abstract bool Initialize(LightCtrlInfo lightCtrlInfo);
        public abstract void TurnOn();
        public abstract void TurnOn(LightValue lightValue);
        public abstract void TurnOn(LightValue lightValue, int deviceIndex);
        public abstract void TurnOff();
        public abstract void TurnOff(int deviceIndex);
    }
}
