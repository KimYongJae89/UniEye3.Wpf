using DynMvp.Base;
using DynMvp.Devices.Comm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    public enum MotionError
    {
        Homing = ErrorSubSection.SpecificReason,
        HomingTimeOut,
        Moving,
        MovingTimeOut,
        ContinuousMoving,
        StopMove,
        EmergencyStop,
        PosLimit,
        NegLimit,
        AmpFault,
        ServoOff,
        HomeFound,
        CantFindNegLimit,
        CantFindPosLimit,
    }

    public enum MotionType
    {
        None,
        Virtual,
        AlphaMotion302,
        AlphaMotion304,
        AlphaMotion314,
        FastechEziMotionPlusR,
        PowerPmac,
        Ajin,
        AlphaMotionBx,
        AlphaMotionBBx
    }

    public enum MovingProfileType
    {
        TCurve, SCurve
    }

    public struct MotionStatus
    {
        public bool origin;
        public bool ez;
        public bool emg;
        public bool inp;
        public bool alarm;
        public bool posLimit;
        public bool negLimit;
        public bool run;
        public bool err;
        public bool home;
        public bool servoOn;
    }

    public abstract class MotionInfo
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MotionType Type { get; set; }
        public int NumAxis { get; set; }

        public MotionInfo()
        {

        }

        public MotionInfo(string name, MotionType type, int numAxis)
        {
            Name = name;
            Type = type;
            NumAxis = numAxis;
        }

        public virtual void LoadXml(XmlElement motionElement)
        {
            Name = XmlHelper.GetValue(motionElement, "Name", "");
            Type = (MotionType)Enum.Parse(typeof(MotionType), XmlHelper.GetValue(motionElement, "Type", "AlphaMotion302"));
            NumAxis = Convert.ToInt32(XmlHelper.GetValue(motionElement, "NumAxis", "4"));
        }

        public virtual void SaveXml(XmlElement motionElement)
        {
            if (string.IsNullOrEmpty(Name))
            {
                XmlHelper.SetValue(motionElement, "Name", "");
            }
            else
            {
                XmlHelper.SetValue(motionElement, "Name", Name.ToString());
            }

            XmlHelper.SetValue(motionElement, "Type", Type.ToString());
            XmlHelper.SetValue(motionElement, "NumAxis", NumAxis.ToString());
        }

        public abstract MotionInfo Clone();

        public virtual void Copy(MotionInfo srcMotionInfo)
        {
            Name = srcMotionInfo.Name;
            Type = srcMotionInfo.Type;
            NumAxis = srcMotionInfo.NumAxis;
        }
    }

    public class VirtualMotionInfo : MotionInfo
    {
        public override MotionInfo Clone()
        {
            var virtualMotionInfo = new VirtualMotionInfo();
            virtualMotionInfo.Copy(this);

            return virtualMotionInfo;
        }
    }

    public class PciMotionInfo : MotionInfo
    {
        public int Index { get; set; }

        public PciMotionInfo()
        {

        }

        public PciMotionInfo(string name, MotionType type, int numAxis, int index) : base(name, type, numAxis)
        {
            Index = index;
        }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            Index = Convert.ToInt32(XmlHelper.GetValue(motionElement, "Index", "0"));
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "Index", Index.ToString());
        }

        public override MotionInfo Clone()
        {
            var pciMotionInfo = new PciMotionInfo();
            pciMotionInfo.Copy(this);

            return pciMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            var srcPciMotionInfo = (PciMotionInfo)srcMotionInfo;
            Index = srcPciMotionInfo.Index;
        }
    }

    public class SerialMotionInfo : MotionInfo
    {
        public SerialPortInfo SerialPortInfo { get; set; } = new SerialPortInfo();

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            SerialPortInfo.Load(motionElement, "SerialPortInfo");
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            SerialPortInfo.Save(motionElement, "SerialPortInfo");
        }

        public override MotionInfo Clone()
        {
            var serialMotionInfo = new SerialMotionInfo();
            serialMotionInfo.Copy(this);

            return serialMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            var serialMotionInfo = (SerialMotionInfo)srcMotionInfo;
            SerialPortInfo = serialMotionInfo.SerialPortInfo.Clone();
        }
    }

    public class NetworkMotionInfo : MotionInfo
    {
        public string IpAddress { get; set; }
        public byte PortNo { get; set; }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            IpAddress = XmlHelper.GetValue(motionElement, "IpAddress", "");
            PortNo = Convert.ToByte(XmlHelper.GetValue(motionElement, "PortNo", "0"));
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(motionElement, "PortNo", PortNo.ToString());
        }

        public override MotionInfo Clone()
        {
            var networkMotionInfo = new NetworkMotionInfo();
            networkMotionInfo.Copy(this);

            return networkMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            var networkMotionInfo = (NetworkMotionInfo)srcMotionInfo;
            IpAddress = networkMotionInfo.IpAddress;
            PortNo = networkMotionInfo.PortNo;
        }
    }

    public class MotionInfoFactory
    {
        public static MotionInfo CreateMotionInfo(MotionType motionType)
        {
            MotionInfo motionInfo = null;
            switch (motionType)
            {
                case MotionType.AlphaMotion302:
                case MotionType.AlphaMotion304:
                case MotionType.AlphaMotion314:
                case MotionType.AlphaMotionBx:
                case MotionType.AlphaMotionBBx:
                    motionInfo = new PciMotionInfo();
                    break;
                case MotionType.PowerPmac:
                    motionInfo = new NetworkMotionInfo();
                    break;
                case MotionType.FastechEziMotionPlusR:
                    motionInfo = new SerialMotionInfo();
                    break;
                default:
                    return null;
                case MotionType.Virtual:
                    motionInfo = new VirtualMotionInfo();
                    break;
                case MotionType.Ajin:
                    motionInfo = new AjinMotionInfo();
                    break;
            }

            motionInfo.Name = "";
            motionInfo.Type = motionType;

            return motionInfo;
        }
    }

    public class MotionInfoList : List<MotionInfo>
    {
        public MotionInfoList Clone()
        {
            var newMotionInfoList = new MotionInfoList();

            foreach (MotionInfo motionInfo in this)
            {
                newMotionInfoList.Add(motionInfo.Clone());
            }

            return newMotionInfoList;
        }

        internal MotionInfo GetMotionInfo(string masterDeviceName)
        {
            return Find(x => x.Name == masterDeviceName);
        }
    }

    public class MotionException : ApplicationException
    {
        private string defaultMessage = "Motion Error";

        public MotionException()
        {
            LogHelper.Error(defaultMessage);
        }
        public MotionException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public MotionException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    //public interface IMotion
    //{
    //    string Name { get; }
    //    int NumAxis { get; }
    //    MotionType MotionType { get; }
    //    bool CanSyncMotion();
    //    void TurnOnServo(int axisNo, bool bOnOff);
    //    float GetCommandPos(int axisNo);
    //    float GetActualPos(int axisNo);
    //    void SetPosition(int axisNo, float position);
    //    bool StartHomeMove(int axisNo, HomeParam homeSpeed);
    //    bool StartMove(int axisNo, float position, MovingParam movingParam);
    //    bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
    //    bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
    //    void StopMove(int axisNo);
    //    void EmergencyStop(int axisNo);
    //    bool IsMoveDone(int axisNo);
    //    bool IsAmpFault(int axisNo);
    //    bool IsServoOn(int axisNo);
    //    bool IsHomeOn(int axisNo);
    //    bool IsPositiveOn(int axisNo);
    //    bool IsNegativeOn(int axisNo);
    //    bool IsEmgStop(int axisNo);
    //    bool ResetAlarm(int axisNo);
    //    MotionStatus GetMotionStatus(int axisNo);
    //}

    public abstract class Motion : Device
    {
        public MotionType MotionType { get; }
        public int NumAxis { get; set; }

        public Motion(MotionType motionType, string name)
        {
            if (name == "")
            {
                Name = motionType.ToString();
            }
            else
            {
                Name = name;
            }

            DeviceType = DeviceType.MotionController;
            MotionType = motionType;
            UpdateState(DeviceState.Idle, "Created");
        }

        public abstract bool Initialize(MotionInfo motionInfo);
        public abstract bool CanSyncMotion();
        public abstract void TurnOnServo(int axisNo, bool bOnOff);
        public abstract float GetCommandPos(int axisNo);
        public abstract float GetActualPos(int axisNo);
        public abstract void SetPosition(int axisNo, float position);
        public abstract bool StartHomeMove(int axisNo, HomeParam homeSpeed);
        public abstract bool StartMove(int axisNo, float position, MovingParam movingParam);
        public abstract bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
        public abstract bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
        public abstract void StopMove(int axisNo);
        public abstract void EmergencyStop(int axisNo);
        public abstract bool IsMoveDone(int axisNo);
        public abstract bool IsAmpFault(int axisNo);
        public abstract bool IsHomeOn(int axisNo);
        public abstract bool IsHomeDone(int axisNo);
        public abstract bool IsPositiveOn(int axisNo);
        public abstract bool IsNegativeOn(int axisNo);
        public abstract bool IsServoOn(int axisNo);
        public abstract MotionStatus GetMotionStatus(int axisNo);
        public abstract bool ResetAlarm(int axisNo);
        public abstract bool ResetAlarmOn(int axisNo, bool isOn);
        public abstract bool IsEmgStop(int axisNo);

        public void TurnOnServo(bool bOnOff)
        {
            for (int i = 0; i < NumAxis; i++)
            {
                TurnOnServo(i, bOnOff);
            }
        }

        public void StopMove()
        {
            for (int i = 0; i < NumAxis; i++)
            {
                StopMove(i);
            }
        }

        public void EmergencyStop()
        {
            for (int i = 0; i < NumAxis; i++)
            {
                EmergencyStop(i);
            }
        }

        public void Move(int axisNo, float position, MovingParam movingParam)
        {
            StartMove(axisNo, position, movingParam);
            while (IsMoveDone(axisNo) == false)
            {
                ;
            }
        }

        public void RelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            StartRelativeMove(axisNo, position, movingParam);
            while (IsMoveDone(axisNo) == false)
            {
                ;
            }
        }

        public void HomeMove(int axisNo, HomeParam homeParam)
        {
            StartHomeMove(axisNo, homeParam);
            while (IsMoveDone(axisNo) == false)
            {

            };

            Thread.Sleep(500);

            SetPosition(axisNo, 0);
        }

        public void ResetAlarm()
        {
            for (int i = 0; i < NumAxis; i++)
            {
                ResetAlarm(i);
            }
        }
    }
}
