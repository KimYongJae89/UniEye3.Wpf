using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Light.SerialLigth.iCore
{

    public enum Address
    {
        RebootDevice_RW = 0x0000,
        SaveParameter_RW = 0x0001,
        LoadParameter_RW = 0x0002,
        WriteEnable_RW = 0x0003,
        AlarmReset_RW = 0x0004,
        SoftTrigger_RW = 0x0005,
        TriggerConterReset_W = 0x0006,
        ErrorConterReset_RW = 0x0007,
        SequenceIndexReset_RW = 0x0008,

        TriggerCount_R = 0x0100,
        ErrorCount_R = 0x0102,
        AlarmCode_R = 0x0104,
        SequenceIndexNumber_R = 0x0106,
        PeriodLimit_R = 0x0108,
        LEDPlusVoltage_R = 0x010c,
        PCBTemperature_R = 0x010e,
        LEDMinusVoltage_R = 0x0110,

        DeviceAddress_RW = 0x0200,
        BootLoaderVersion_R = 0x0202,
        SWVersion_R = 0x0204,
        FPGAVersion_R = 0x0206,
        SerialNumber_R = 0x0208,
        ModelCode_R = 0x020a,

        OperationMode_RW = 0x0300,
        TriggerInputSource_RW = 0x0301,
        TriggerInputActivation_RW = 0x0302,
        TiggerOutputSource_RW = 0x0303,
        TiggerOutputInverter_RW = 0x0304,
        SequenceMode_RW = 0x0305,
        SequenceInitialIndex_RW = 0x0306,
        SequenceNumber_RW = 0x0307,
        AutoSequenceReset_RW = 0x0308,
        AutoSequenceResetTimeout_RW = 0x0309,
        AutoVoltage_RW = 0x030a,
        LPFMode_RW = 0x030b,
        PowerDown_RW = 0x030c,
        Duration_RW = 0x0310,
        Period_RW = 0x03012,
        TriggerDelay_RW = 0x0314,
        Voltage_RW = 0x0316,
        LED1Enable_RW = 0x0320,
        LED2Enable_RW = 0x0321,
        LED3Enable_RW = 0x0322,
        LED4Enable_RW = 0x0323,
        LED1CurrentRateContinuous_RW = 0x0330,
        LED2CurrentRateContinuous_RW = 0x0331,
        LED3CurrentRateContinuous_RW = 0x0332,
        LED4CurrentRateContinuous_RW = 0x0333,
        LED1CurrentRatePulse_RW = 0x0340,
        LED2CurrentRatePulse_RW = 0x0341,
        LED3CurrentRatePulse_RW = 0x0342,
        LED4CurrentRatePulse_RW = 0x0343,
        LED1RatedCurrent_RW = 0x0350,
        LED2RatedCurrent_RW = 0x0351,
        LED3RatedCurrent_RW = 0x0352,
        LED4RatedCurrent_RW = 0x0353,
        SequenceData_RW = 0x0380,
        MultiSequenceData_RW = 0x0390,
    }
    public enum OperationMode { Off, Continuous, Pulse }
    public enum TriggerInputSource { Internal, DigitalIO, RJ45, Soft }
    public enum TriggerInputActivation { Rising, Falling }
    public enum TiggerOutputSource { LED_Output_Sync, Tringger_Input, ErrorEvent, Low, High }
    public enum TiggerOutputInverter { None, Invert }
    public enum SequenceMode { Off, Single, Multi }

    public enum Function
    {
        Read = 0x03,
        Write = 0x06,
        MultiWrite = 0x10
    }

    internal enum IPulseAlarm
    {
        PCBTemperatureOverhead = 0x0002,
        FPGARegisterCheckError = 0x0008,
        FlashMemoryCheckError = 0x0010,
        PowerRangeOver = 0x0020,
        LEDShortCircuit = 0x0040,
        LED1NotConnected = 0x0100,
        LED2NotConnected = 0x0200,
        LED3NotConnected = 0x0400,
        LED4NotConnected = 0x0800
    }

    public class SeqInfo
    {
        public SequenceMode Mode { get; set; }

        public ushort Count { get; set; }

        public ushort[] Sequences { get; private set; }

        public ushort this[int i] { get => Sequences[i]; set => Sequences[i] = value; }

        public SeqInfo()
        {
            Mode = SequenceMode.Off;
            Count = 8;
            Sequences = new ushort[8];
        }

        public void SaveXml(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                SaveXml(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Mode", Mode);
            XmlHelper.SetValue(xmlElement, "Count", Count);

            XmlElement seqElement = xmlElement.OwnerDocument.CreateElement("Seq");
            for (int i = 0; i < Sequences.Length; i++)
            {
                XmlHelper.SetValue(seqElement, string.Format("Seq{0:D02}", i), Sequences[i]);
            }

            xmlElement.AppendChild(seqElement);
        }

        public void LoadXml(XmlElement xmlElement, string key = null)
        {
            Mode = XmlHelper.GetValue(xmlElement, "Mode", Mode);
            Count = XmlHelper.GetValue(xmlElement, "Count", Count);

            Array.Clear(Sequences, 0, Sequences.Length);
            XmlElement seqElement = xmlElement["Seq"];
            if (seqElement != null)
            {
                for (int i = 0; i < Sequences.Length; i++)
                {
                    Sequences[i] = XmlHelper.GetValue(seqElement, string.Format("Seq{0:D02}", i), Sequences[i]);
                }
            }
        }

        internal void Copy(SeqInfo src)
        {
            Mode = src.Mode;
            Count = src.Count;
            Sequences = (ushort[])src.Sequences.Clone();
        }
    }

    public class IPulseFrame
    {
        private byte targetDeviceId;
        private IPulseFunction function;

        public bool IsRead => (function.Function == Function.Read);

        public static IPulseFrame CreateRFrame(byte targetDeviceId, Address register, ushort argument)
        {
            byte[] datas = BitConverter.GetBytes(argument);
            return new IPulseFrame(targetDeviceId, Function.Read, register, datas);
        }

        public static IPulseFrame CreateWFrame(byte targetDeviceId, Address register, bool onOff)
        {
            byte[] datas = BitConverter.GetBytes((ushort)(onOff ? 1 : 0));
            return new IPulseFrame(targetDeviceId, Function.Write, register, datas);
        }

        public static IPulseFrame CreateWFrame(byte targetDeviceId, Address register, ushort argument)
        {
            byte[] datas = BitConverter.GetBytes(argument);
            return new IPulseFrame(targetDeviceId, Function.Write, register, datas);
        }

        public static IPulseFrame CreateWFrame(byte targetDeviceId, Address register, Enum argument)
        {
            //ushort obj = (ushort)Convert.ChangeType(argument, typeof(ushort));
            ushort obj = Convert.ToUInt16(argument);
            byte[] datas = BitConverter.GetBytes(obj);
            return new IPulseFrame(targetDeviceId, Function.Write, register, datas);
        }

        public static IPulseFrame CreateWFrame(byte targetDeviceId, Address register, ushort[] arguments)
        {
            var byteList = new List<byte>();
            Array.ForEach(arguments, f => byteList.AddRange(BitConverter.GetBytes(f)));

            return new IPulseFrame(targetDeviceId, Function.MultiWrite, register, byteList.ToArray());
        }

        public static IPulseFrame CreateWFrame(byte targetDeviceId, Address register, float argument)
        {
            byte[] datas = BitConverter.GetBytes(argument);
            return new IPulseFrame(targetDeviceId, Function.MultiWrite, register, datas);
        }

        private IPulseFrame(byte targetDeviceId, Function function, Address register, byte[] datasLE)
        {
            this.targetDeviceId = targetDeviceId;
            this.function = new IPulseFunctionRW(function, register, datasLE);
        }

        // Responce
        public IPulseFrame(bool isReadFrameResponce, byte[] bytes)
        {
            targetDeviceId = bytes[0];
            if (isReadFrameResponce)
            {
                function = new IPulseFunctionRecive(bytes);
            }
            else
            {
                function = new IPulseFunctionRW(bytes);
            }
        }

        public byte[] ToBytes()
        {
            var bytes = new List<byte>();
            bytes.Add(targetDeviceId);
            bytes.AddRange(function.GetBytes());
            bytes.AddRange(CalcCRC(bytes.ToArray()));
            return bytes.ToArray();
        }

        public static byte[] CalcCRC(byte[] bytes)
        {
            ushort crc = 0xffff;
            for (int i = 0; i < bytes.Length; i++)
            {
                crc ^= bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    bool xor = (crc & 0x0001) > 0;

                    crc >>= 1;
                    if (xor)
                    {
                        crc ^= 0xA001;
                    }
                }
            }

            return BitConverter.GetBytes(crc);
        }

        //unsigned short CIP_ModBusRTU::CRC16(unsigned char* buff, int num)
        //{
        //    unsigned short CRC = 0xFFFF;
        //    int i;

        //    while (num--)
        //    {
        //        CRC ^= *buff++;
        //        for (i = 0; i < 8; i++)
        //        {
        //            if (CRC & 1)
        //            {
        //                CRC >>= 1;
        //                CRC ^= 0xA001;
        //            }
        //            else
        //            {
        //                CRC >>= 1;
        //            }
        //        }
        //    }

        //    return CRC;
        //}
    }

    public abstract class IPulseFunction
    {
        public Function Function => function;
        protected Function function;

        public byte[] Bytes => bytes;
        protected byte[] bytes;

        public ushort[] Data16 { get; private set; }

        public float[] Data32 { get; private set; }

        protected IPulseFunction(byte[] bytes)
        {
            function = (Function)bytes[1];
        }

        protected IPulseFunction(Function function)
        {
            this.function = function;
        }

        protected abstract byte[] GetMidBytes();

        public byte[] GetBytes()
        {
            var byteList = new List<byte>();
            byteList.Add((byte)function);
            byteList.AddRange(GetMidBytes());
            byteList.AddRange(bytes);
            return byteList.ToArray();
        }

        protected void UpdateData()
        {
            var byteList = new List<byte>();
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < this.bytes.Length; i += 2)
                {
                    byte[] revBytes = this.bytes.Skip(i).Take(2).ToArray();
                    Array.Reverse(revBytes);
                    byteList.AddRange(revBytes);
                }
            }
            else
            {
                byteList.AddRange(this.bytes);
            }

            byte[] bytes = byteList.ToArray();
            Data16 = new ushort[bytes.Length / 2];
            Data32 = new float[bytes.Length / 4];

            for (int i = 0; i < Data16.Length; i++)
            {
                Data16[i] = BitConverter.ToUInt16(bytes, i * 2);
            }

            for (int i = 0; i < Data32.Length; i++)
            {
                Data32[i] = BitConverter.ToSingle(bytes, i * 4);
            }
        }
    }

    public class IPulseFunctionRecive : IPulseFunction
    {
        private byte byteSize;

        public IPulseFunctionRecive(byte[] bytes) : base(bytes)
        {
            byteSize = bytes[2];
            this.bytes = bytes.Skip(3).Take(byteSize).ToArray();
            UpdateData();

            byte[] crc0 = bytes.Skip(byteSize + 3).ToArray();
            byte[] crc1 = IPulseFrame.CalcCRC(bytes.Take(byteSize + 3).ToArray());
            if (!crc0.SequenceEqual(crc1))
            {
                throw new Exception("CRC Missmatch");
            }
        }

        protected override byte[] GetMidBytes()
        {
            return new byte[] { byteSize };
        }
    }

    public class IPulseFunctionRW : IPulseFunction
    {
        private Address regAddr;

        public IPulseFunctionRW(Function function, Address register, byte[] datas) : base(function)
        {
            regAddr = register;

            bytes = SwapByteByByte(datas);
            UpdateData();
        }

        public IPulseFunctionRW(byte[] bytes) : base(0)
        {
            function = (Function)bytes[1];
            regAddr = (Address)BitConverter.ToInt16(bytes, 2);

            int dataLength = bytes.Length - 1 - 1 - 2 - 2; // ID1, Function1, Addr2, CRC2;
            this.bytes = bytes.Skip(4).Take(dataLength).ToArray();
            UpdateData();
        }

        protected override byte[] GetMidBytes()
        {
            var byteList = new List<byte>();

            byte[] addrBytes = BitConverter.GetBytes((ushort)regAddr);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addrBytes);
            }

            byteList.AddRange(addrBytes);

            if (function == Function.MultiWrite)
            {
                byte byteCount = (byte)bytes.Length;
                ushort regCount = (ushort)(byteCount / 2);

                byte[] regCountBytes = BitConverter.GetBytes(regCount);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(regCountBytes);
                }

                byteList.AddRange(regCountBytes);

                byteList.Add(byteCount);
            }
            return byteList.ToArray();
        }

        private byte[] SwapByteByByte(byte[] bytes)
        {
            var byteList = bytes.ToList();

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < byteList.Count; i += 2)
                {
                    byte temp = byteList[i];
                    byteList[i] = byteList[i + 1];
                    byteList[i + 1] = temp;
                }
            }
            return byteList.ToArray();
        }
    }
}
