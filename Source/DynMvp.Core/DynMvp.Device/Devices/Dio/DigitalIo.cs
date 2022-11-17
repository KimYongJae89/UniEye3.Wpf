using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Dio
{
    public enum DigitalIoType
    {
        None, Virtual, Adlink7230, Adlink7432, AlphaMotion302, AlphaMotion304, AlphaMotion314, SusiGpio, FastechEziMotionPlusR,
        Modubus, ComizoaSd424f, TmcAexxx, NIMax, NIUsb, Ajin_Slave, Ajin_Master, KM6050, AlphaMotionBx, AlphaMotionBBx, AlphaMotionBBxDIO,
        Serial2DIO
    }

    public enum DigitalIoError
    {
        CantFindMasterMotion,
        InvalidMasterMotion
    }

    public abstract class DigitalIoInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public DigitalIoType Type { get; set; }

        protected int numInPortGroup;
        public int NumInPortGroup
        {
            get => numInPortGroup;
            set => numInPortGroup = value;
        }

        protected int inPortStartGroupIndex;
        public int InPortStartGroupIndex
        {
            get => inPortStartGroupIndex;
            set => inPortStartGroupIndex = value;
        }

        protected int numOutPortGroup;
        public int NumOutPortGroup
        {
            get => numOutPortGroup;
            set => numOutPortGroup = value;
        }

        protected int outPortStartGroupIndex;
        public int OutPortStartGroupIndex
        {
            get => outPortStartGroupIndex;
            set => outPortStartGroupIndex = value;
        }

        protected int numInPort;
        public int NumInPort
        {
            get => numInPort;
            set => numInPort = value;
        }

        protected int numOutPort;
        public int NumOutPort
        {
            get => numOutPort;
            set => numOutPort = value;
        }

        public DigitalIoInfo(DigitalIoType digitalIoType)
        {
            Type = digitalIoType;
        }

        public DigitalIoInfo(DigitalIoType digitalIoType, int index, int numInPortGroup, int inPortStartGroupIndex, int numInPort, int numOutPortGroup, int outPortStartGroupIndex, int numOutPort)
        {
            Index = index;
            Type = digitalIoType;
            this.numInPortGroup = numInPortGroup;
            this.inPortStartGroupIndex = inPortStartGroupIndex;
            this.numOutPortGroup = numOutPortGroup;
            this.outPortStartGroupIndex = outPortStartGroupIndex;
            this.numInPort = numInPort;
            this.numOutPort = numOutPort;
        }

        public virtual void LoadXml(XmlElement digitalIoInfoElement)
        {
            Name = XmlHelper.GetValue(digitalIoInfoElement, "Name", "");
            Index = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "Index", "0"));

            string digitalIoTypeStr = XmlHelper.GetValue(digitalIoInfoElement, "Type", "AlphaMotion302");
            try
            {
                Type = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), digitalIoTypeStr);
            }
            catch (Exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.InvalidType, ErrorLevel.Error,
                        ErrorSection.DigitalIo.ToString(), CommonError.InvalidType.ToString(), string.Format("Invalid Digital I/O Type : {0}", digitalIoTypeStr));
                Type = DigitalIoType.AlphaMotion302;
            }

            inPortStartGroupIndex = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "InPortStartGroupIndex", "0"));
            numInPortGroup = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumInPortGroup", "1"));
            outPortStartGroupIndex = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "OutPortStartGroupIndex", "0"));
            numOutPortGroup = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumOutPortGroup", "1"));
            numInPort = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumInPort", "16"));
            numOutPort = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumOutPort", "16"));
        }

        public virtual void SaveXml(XmlElement digitalIoInfoElement)
        {
            XmlHelper.SetValue(digitalIoInfoElement, "Name", Name);
            XmlHelper.SetValue(digitalIoInfoElement, "Index", Index.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "Type", Type.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "InPortStartGroupIndex", inPortStartGroupIndex.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "NumInPortGroup", numInPortGroup.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "OutPortStartGroupIndex", outPortStartGroupIndex.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "NumOutPortGroup", numOutPortGroup.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "NumInPort", numInPort.ToString());
            XmlHelper.SetValue(digitalIoInfoElement, "NumOutPort", numOutPort.ToString());
        }

        public abstract DigitalIoInfo Clone();

        public virtual void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            Name = srcDigitalIoInfo.Name;
            Type = srcDigitalIoInfo.Type;
            inPortStartGroupIndex = srcDigitalIoInfo.inPortStartGroupIndex;
            numInPortGroup = srcDigitalIoInfo.numInPortGroup;
            outPortStartGroupIndex = srcDigitalIoInfo.outPortStartGroupIndex;
            numOutPortGroup = srcDigitalIoInfo.numOutPortGroup;
            numInPort = srcDigitalIoInfo.numInPort;
            numOutPort = srcDigitalIoInfo.NumOutPort;
        }
    }

    public class DigitalIoInfoVirtual : DigitalIoInfo
    {
        public DigitalIoInfoVirtual() : base(DigitalIoType.Virtual)
        {
        }

        public DigitalIoInfoVirtual(DigitalIoType digitalIoType, int index,
            int numInPortGroup, int inPortStartGroupIndex, int numInPort,
            int numOutPortGroup, int outPortStartGroupIndex, int numOutPort)
            : base(digitalIoType, index, numInPortGroup, inPortStartGroupIndex, numInPort, numOutPortGroup, outPortStartGroupIndex, numOutPort)
        {
        }

        public override DigitalIoInfo Clone()
        {
            var pciDigitalIoInfo = new DigitalIoInfoVirtual();
            pciDigitalIoInfo.Copy(this);

            return pciDigitalIoInfo;
        }
    }

    public class PciDigitalIoInfo : DigitalIoInfo
    {
        public int DeviceIndex { get; set; }

        public PciDigitalIoInfo(DigitalIoType digitalIoType) : base(digitalIoType) { }

        public override void LoadXml(XmlElement digitalIoInfoElement)
        {
            base.LoadXml(digitalIoInfoElement);

            DeviceIndex = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "DeviceIndex", ""));
        }

        public override void SaveXml(XmlElement digitalIoInfoElement)
        {
            base.SaveXml(digitalIoInfoElement);

            XmlHelper.SetValue(digitalIoInfoElement, "DeviceIndex", DeviceIndex.ToString());
        }

        public override DigitalIoInfo Clone()
        {
            var pciDigitalIoInfo = new PciDigitalIoInfo(Type);
            pciDigitalIoInfo.Copy(this);

            return pciDigitalIoInfo;
        }

        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            base.Copy(srcDigitalIoInfo);

            var pciDigitalIoInfo = (PciDigitalIoInfo)srcDigitalIoInfo;
            DeviceIndex = pciDigitalIoInfo.DeviceIndex;
        }
    }

    public class MasterDigitalIoInfo : DigitalIoInfo
    {
        public string SlaveDeviceName { get; set; }

        public MasterDigitalIoInfo(DigitalIoType digitalIoType) : base(digitalIoType) { }

        public override void LoadXml(XmlElement digitalIoInfoElement)
        {
            base.LoadXml(digitalIoInfoElement);

            SlaveDeviceName = XmlHelper.GetValue(digitalIoInfoElement, "SlaveDeviceName", "");
        }

        public override void SaveXml(XmlElement digitalIoInfoElement)
        {
            base.SaveXml(digitalIoInfoElement);

            XmlHelper.SetValue(digitalIoInfoElement, "SlaveDeviceName", SlaveDeviceName);
        }

        public override DigitalIoInfo Clone()
        {
            var slaveDigitalIoInfo = new SlaveDigitalIoInfo(Type);
            slaveDigitalIoInfo.Copy(this);

            return slaveDigitalIoInfo;
        }

        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            base.Copy(srcDigitalIoInfo);

            var slaveDigitalIoInfo = (SlaveDigitalIoInfo)srcDigitalIoInfo;
            SlaveDeviceName = slaveDigitalIoInfo.MasterDeviceName;
        }
    }

    public class SlaveDigitalIoInfo : DigitalIoInfo
    {
        public string MasterDeviceName { get; set; }

        public SlaveDigitalIoInfo(DigitalIoType digitalIoType) : base(digitalIoType) { }

        public override void LoadXml(XmlElement digitalIoInfoElement)
        {
            base.LoadXml(digitalIoInfoElement);

            MasterDeviceName = XmlHelper.GetValue(digitalIoInfoElement, "MasterDeviceName", "");
        }

        public override void SaveXml(XmlElement digitalIoInfoElement)
        {
            base.SaveXml(digitalIoInfoElement);

            XmlHelper.SetValue(digitalIoInfoElement, "MasterDeviceName", MasterDeviceName);
        }

        public override DigitalIoInfo Clone()
        {
            var slaveDigitalIoInfo = new SlaveDigitalIoInfo(Type);
            slaveDigitalIoInfo.Copy(this);

            return slaveDigitalIoInfo;
        }

        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            base.Copy(srcDigitalIoInfo);

            var slaveDigitalIoInfo = (SlaveDigitalIoInfo)srcDigitalIoInfo;
            MasterDeviceName = slaveDigitalIoInfo.MasterDeviceName;
        }
    }

    public class NiDigitalIoInfo : DigitalIoInfo
    {
        public string InputChannelLine { get; set; }
        public string OutputChannelLine { get; set; }

        public NiDigitalIoInfo() : base(DigitalIoType.NIMax)
        {
        }

        public override void LoadXml(XmlElement digitalIoInfoElement)
        {
            base.LoadXml(digitalIoInfoElement);

            InputChannelLine = XmlHelper.GetValue(digitalIoInfoElement, "InputChannelLine", "Dev2/port0/line0:3");
            OutputChannelLine = XmlHelper.GetValue(digitalIoInfoElement, "OutputChannelLine", "Dev2/port1/line0:3");
        }

        public override void SaveXml(XmlElement digitalIoInfoElement)
        {
            base.SaveXml(digitalIoInfoElement);

            XmlHelper.SetValue(digitalIoInfoElement, "InputChannelLine", InputChannelLine);
            XmlHelper.SetValue(digitalIoInfoElement, "OutputChannelLine", OutputChannelLine);
        }

        public override DigitalIoInfo Clone()
        {
            var niDigitalIoInfo = new NiDigitalIoInfo();
            niDigitalIoInfo.Copy(this);

            return niDigitalIoInfo;
        }

        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            base.Copy(srcDigitalIoInfo);

            var niDigitalIoInfo = (NiDigitalIoInfo)srcDigitalIoInfo;
            InputChannelLine = niDigitalIoInfo.InputChannelLine;
            OutputChannelLine = niDigitalIoInfo.OutputChannelLine;
        }
    }

    public class SerialDigitalIoInfo : DigitalIoInfo
    {
        public SerialPortInfo SerialPortInfo { get; set; } = new SerialPortInfo();


        // 0: noUse, 1:Port0, 2:Port1, 3:Port0/1
        public int RtsPort { get; set; } = 1;

        public int DtrPort { get; set; } = 2;

        public SerialDigitalIoInfo() : base(DigitalIoType.Serial2DIO)
        {
            numInPortGroup = 0;
            inPortStartGroupIndex = 0;
            numInPort = 0;

            numOutPortGroup = 1;
            outPortStartGroupIndex = 0;
            numOutPort = 2;
        }

        public SerialDigitalIoInfo(int index,
            int numInPortGroup, int inPortStartGroupIndex, int numInPort,
            int numOutPortGroup, int outPortStartGroupIndex, int numOutPort)
            : base(DigitalIoType.Serial2DIO, index, numInPortGroup, inPortStartGroupIndex, numInPort, numOutPortGroup, outPortStartGroupIndex, numOutPort)
        {
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            SerialPortInfo.Load(xmlElement, "SerialPortInfo");

            RtsPort = XmlHelper.GetValue(xmlElement, "RtsPort", RtsPort);
            DtrPort = XmlHelper.GetValue(xmlElement, "DtrPort", DtrPort);
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            SerialPortInfo.Save(xmlElement, "SerialPortInfo");

            XmlHelper.SetValue(xmlElement, "RtsPort", RtsPort.ToString());
            XmlHelper.SetValue(xmlElement, "DtrPort", DtrPort.ToString());
        }

        public override DigitalIoInfo Clone()
        {
            var serialDigitalIoInfo = new SerialDigitalIoInfo();
            serialDigitalIoInfo.Copy(this);

            serialDigitalIoInfo.RtsPort = RtsPort;
            serialDigitalIoInfo.DtrPort = DtrPort;

            return serialDigitalIoInfo;
        }

        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
        {
            base.Copy(srcDigitalIoInfo);

            var serialDigitalIoInfo = (SerialDigitalIoInfo)srcDigitalIoInfo;
            SerialPortInfo = serialDigitalIoInfo.SerialPortInfo.Clone();
        }
    }

    public class DigitalIoInfoList : List<DigitalIoInfo>
    {
        public DigitalIoInfoList Clone()
        {
            var newDigitalIoInfoList = new DigitalIoInfoList();

            foreach (DigitalIoInfo digitalIoInfo in this)
            {
                newDigitalIoInfoList.Add(digitalIoInfo.Clone());
            }

            return newDigitalIoInfoList;
        }

    }

    public class DigitalIoInfoFactory
    {
        public static DigitalIoInfo Create(DigitalIoType digitalIoType)
        {
            DigitalIoInfo digitalIoInfo = null;
            switch (digitalIoType)
            {
                case DigitalIoType.AlphaMotion302:
                case DigitalIoType.AlphaMotion304:
                case DigitalIoType.AlphaMotion314:
                case DigitalIoType.FastechEziMotionPlusR:
                case DigitalIoType.Ajin_Slave:
                case DigitalIoType.AlphaMotionBx:
                case DigitalIoType.AlphaMotionBBx:
                    digitalIoInfo = new SlaveDigitalIoInfo(digitalIoType);
                    break;
                case DigitalIoType.Ajin_Master:
                    digitalIoInfo = new MasterDigitalIoInfo(digitalIoType);
                    break;
                case DigitalIoType.SusiGpio:
                case DigitalIoType.Adlink7230:
                case DigitalIoType.Adlink7432:
                case DigitalIoType.TmcAexxx:
                case DigitalIoType.ComizoaSd424f:
                case DigitalIoType.AlphaMotionBBxDIO:
                    digitalIoInfo = new PciDigitalIoInfo(digitalIoType);
                    break;
                case DigitalIoType.Modubus:
                case DigitalIoType.Serial2DIO:
                    digitalIoInfo = new SerialDigitalIoInfo();
                    break;
                case DigitalIoType.NIMax:
                    digitalIoInfo = new NiDigitalIoInfo();
                    break;
                case DigitalIoType.Virtual:
                default:
                    digitalIoInfo = new DigitalIoInfoVirtual();
                    break;
            }

            digitalIoInfo.Type = digitalIoType;

            return digitalIoInfo;
        }
    }

    public interface IDigitalIo
    {
        string GetName();
        int GetNumInPortGroup();
        int GetNumOutPortGroup();
        int GetInPortStartGroupIndex();
        int GetOutPortStartGroupIndex();
        int GetNumInPort();
        int GetNumOutPort();

        bool Initialize(DigitalIoInfo digitalIoInfo);
        void Release();
        bool IsReady();
        bool IsVirtual { get; }

        void UpdateState(DeviceState state, string stateMessage = "");
        void WriteOutputGroup(int groupNo, uint outputPortStatus);
        void WriteInputGroup(int groupNo, uint inputPortStatus);
        uint ReadOutputGroup(int groupNo);
        uint ReadInputGroup(int groupNo);

        void WriteOutputPort(int groupNo, int portNo, bool value);
    }

    public abstract class DigitalIo : Device, IDigitalIo
    {
        protected DigitalIoType digitalIoType;
        public DigitalIoType DigitalIoType => digitalIoType;
        public int NumInPort { get; set; }
        public int NumOutPort { get; set; }
        public int NumInPortGroup { get; set; }
        public int InPortStartGroupIndex { get; set; }
        public int NumOutPortGroup { get; set; }
        public int OutPortStartGroupIndex { get; set; }

        public virtual bool IsVirtual => false;

        public const int UnusedPortNo = 255;

        public DigitalIo(DigitalIoType digitalIoType, string name)
        {
            if (name == "")
            {
                Name = digitalIoType.ToString();
            }
            else
            {
                Name = name;
            }

            this.digitalIoType = digitalIoType;

            DeviceType = DeviceType.DigitalIo;
            UpdateState(DeviceState.Idle);
        }

        public string GetName() { return Name; }

        public int GetNumInPortGroup() { return NumInPortGroup; }
        public int GetNumOutPortGroup() { return NumOutPortGroup; }
        public int GetInPortStartGroupIndex() { return InPortStartGroupIndex; }
        public int GetOutPortStartGroupIndex() { return OutPortStartGroupIndex; }
        public int GetNumInPort() { return NumInPort; }
        public int GetNumOutPort() { return NumOutPort; }

        public virtual bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            if (digitalIoInfo == null)
            {
                return false;
            }

            NumInPort = digitalIoInfo.NumInPort;
            NumOutPort = digitalIoInfo.NumOutPort;

            NumInPortGroup = digitalIoInfo.NumInPortGroup;
            InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
            OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            return true;
        }

        public abstract void WriteOutputGroup(int groupNo, uint outputPortStatus);
        public abstract void WriteInputGroup(int groupNo, uint inputPortStatus); // 가상 입력용
        public abstract uint ReadOutputGroup(int groupNo);
        public abstract uint ReadInputGroup(int groupNo);
        public new abstract bool IsReady();
        public virtual void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            uint current = ReadOutputGroup(groupNo);
            if (value)
            {
                current |= (uint)(0x01 << portNo);
            }
            else
            {
                current &= (uint)(~(0x01 << portNo));
            }

            WriteOutputGroup(groupNo, current);
        }

        public static void UpdateBitFlag(ref uint portStatus, int bitPos, bool value)
        {
            if (bitPos == UnusedPortNo)
            {
                return;
            }

            uint nSift = 0x0001;

            nSift <<= bitPos;

            if (value == true)
            {
                portStatus |= nSift;
            }
            else
            {
                nSift = ~nSift;
                portStatus &= nSift;
            }
        }
    }
}
