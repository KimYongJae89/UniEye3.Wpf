using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public class SerialLightCtrlInfo : LightCtrlInfo
    {
        public SerialPortInfo SerialPortInfo { get; set; } = new SerialPortInfo();

        public SerialLightCtrlInfo()
        {
            Type = LightCtrlType.Serial;
        }

        public override Form GetAdvancedConfigForm()
        {
            return null;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            SerialPortInfo.Save(lightInfoElement, "SerialLightController");
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            SerialPortInfo.Load(lightInfoElement, "SerialLightController");
        }

        public override LightCtrlInfo Clone()
        {
            var serialLightCtrlInfo = new SerialLightCtrlInfo();
            serialLightCtrlInfo.Copy(this);

            return serialLightCtrlInfo;
        }

        public override void Copy(LightCtrlInfo srcInfo)
        {
            base.Copy(srcInfo);

            var serialLightCtrlInfo = (SerialLightCtrlInfo)srcInfo;

            SerialPortInfo.Copy(serialLightCtrlInfo.SerialPortInfo);
        }
    }

    public class SerialLightCtrl : LightCtrl
    {
        protected SerialPortEx lightSerialPort = null;
        private int numChannel = 0;
        private LightValue lastLightValue;

        protected ManualResetEvent responseReceived = new ManualResetEvent(false);
        protected object responce = null;
        protected object sent = null;

        public override int NumChannel => numChannel;

        public SerialLightCtrl(string name) : base(LightCtrlType.Serial, name) { }

        public override int GetMaxLightLevel()
        {
            return 255;
        }

        public override void TurnOn()
        {
            if (lastLightValue == null)
            {
                lastLightValue = new LightValue(numChannel);
            }

            lastLightValue.TurnOn();
            TurnOn(lastLightValue);
        }

        public override void TurnOff()
        {
            if (lastLightValue == null)
            {
                lastLightValue = new LightValue(numChannel);
            }

            LightValue lv = lastLightValue.Clone();
            lv.TurnOff();
            TurnOn(lv);
        }

        private void TurnOffPSCC()
        {
            string turnOffPacket = string.Format("@00L0007C\r\n");
            byte[] turnOffByte = Encoding.UTF8.GetBytes(turnOffPacket);
            lightSerialPort.WritePacket(turnOffByte, 0, 11);
        }

        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
        {
            try
            {
                var serialLightCtrlInfo = (SerialLightCtrlInfo)lightCtrlInfo;

                PacketParser packetParser = CreatePacketParser();

                lightSerialPort = new SerialPortEx();
                lightSerialPort.Open(serialLightCtrlInfo.Name, serialLightCtrlInfo.SerialPortInfo);
                lightSerialPort.StartListening();
                //lightSerialPort.PacketReceived += lightSerialPort_PacketReceived;
                numChannel = serialLightCtrlInfo.NumChannel;

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("Can't open serial port. {0}", ex.Message));

                lightSerialPort = null;
            }

            return false;
        }

        protected virtual PacketParser CreatePacketParser()
        {
            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += lightSerialPort_PacketReceived;
            return packetParser;
        }

        private void lightSerialPort_PacketReceived(ReceivedPacket receivedPacket)
        {
            string[] ss = Array.ConvertAll(receivedPacket.ReceivedDataByte, f => f.ToString("X02"));
            System.Diagnostics.Debug.WriteLine(string.Format("lightSerialPort_PacketReceived - {0}", string.Join(", ", ss)));

            // 아무거나 응답이 오면 수신 확인됨 체크.
            responce = receivedPacket;
            responseReceived.Set();
        }

        public override void Release()
        {
            base.Release();

            lightSerialPort.Close();
        }

        public virtual void TurnOn(int channel, LightValue lightValue)
        {

        }

        public override void TurnOn(LightValue lightValue)
        {
            if (lightSerialPort != null)
            {
                lastLightValue = lightValue.Clone();

                LogHelper.Debug(LoggerType.Grab, string.Format("Turn on light : {0}", lightValue.KeyValue));

                var timeOutTimer = new TimeOutTimer();
                timeOutTimer.Start(10000);

                for (int i = 0; i < numChannel; i++)
                {
                    TurnOn(i, lightValue);
                }

                Thread.Sleep(DeviceConfig.Instance().LightStableTimeMs);
            }
        }

        public override void TurnOn(LightValue lightValue, int deviceIndex)
        {
            if (lightSerialPort != null)
            {
                lastLightValue = lightValue.Clone();

                LogHelper.Debug(LoggerType.Grab, string.Format("Turn on light : {0}", lightValue.KeyValue));

                int lightValueInt = lightValue.Value[deviceIndex];

                string packet = string.Format("C{0}{1:000}\r\n", deviceIndex + 1, lightValueInt);
                byte[] StrByte = Encoding.UTF8.GetBytes(packet);

                lightSerialPort.WritePacket(StrByte, 0, 7);

                Thread.Sleep(DeviceConfig.Instance().LightStableTimeMs);
            }
        }

        public override void TurnOff(int deviceIndex)
        {
            if (lastLightValue == null)
            {
                lastLightValue = new LightValue(numChannel);
            }

            LightValue lv = lastLightValue.Clone();
            lv.TurnOff();
            TurnOn(lv, deviceIndex);
        }
    }

    internal class IovisLightCtrl : SerialLightCtrl
    {
        public IovisLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            string packet = string.Format("#CH{0:00}BW{1:0000}E", channel + 1, lightValue.Value[StartChannelIndex + channel]);
            lightSerialPort.WritePacket(packet);
        }
    }

    internal class MovisLightCtrl : SerialLightCtrl
    {
        public MovisLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            byte[] bytePacket = new byte[6];
            bytePacket[0] = 0x95;
            bytePacket[1] = 0x2;
            bytePacket[2] = (byte)(channel + 1);
            bytePacket[3] = (byte)(lightValue.Value[StartChannelIndex + channel] > 0 ? 1 : 0);
            bytePacket[4] = (byte)(lightValue.Value[StartChannelIndex + channel]);
            bytePacket[5] = (byte)(bytePacket[0] + bytePacket[1] + bytePacket[2] + bytePacket[3] + bytePacket[4]);

            lightSerialPort.WritePacket(bytePacket, 0, 6);
        }
    }

    internal class AltLightCtrl : SerialLightCtrl
    {
        public AltLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            string packet = string.Format("L{0}{1:000}\r\n", channel, lightValue.Value[StartChannelIndex + channel]);
            byte[] StrByte = Encoding.UTF8.GetBytes(packet);
            lightSerialPort.WritePacket(StrByte, 0, 7);
        }
    }

    internal class LvsLightCtrl : SerialLightCtrl
    {
        public LvsLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            string packet = string.Format("L{0}{1:000}\r\n", channel + 1, lightValue.Value[StartChannelIndex + channel]);
            byte[] StrByte = Encoding.UTF8.GetBytes(packet);
            lightSerialPort.WritePacket(StrByte, 0, 7);
        }
    }

    internal class VitLightCtrl : SerialLightCtrl
    {
        public VitLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            int lightValueInt = lightValue.Value[StartChannelIndex + channel];

            string packet = string.Format("C{0}{1:000}\r\n", channel + 1, lightValueInt);
            byte[] StrByte = Encoding.UTF8.GetBytes(packet);

            lightSerialPort.WritePacket(StrByte, 0, 7);
        }
    }

    internal class CcsLightCtrl : SerialLightCtrl
    {
        public CcsLightCtrl(string name) : base(name)
        {
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            string turnOnPacket = string.Format("@00L1007D\r\n");
            byte[] turnOnByte = Encoding.UTF8.GetBytes(turnOnPacket);
            lightSerialPort.WritePacket(turnOnByte, 0, turnOnByte.Length);
            Thread.Sleep(100);

            string preparePacket = string.Format("@00F{0:000}00", lightValue.Value[StartChannelIndex + channel]);

            byte[] toByte = Encoding.Default.GetBytes(preparePacket);
            int sum = 0;
            for (int b = 0; b < toByte.Length; b++)
            {
                sum += toByte[b];
            }

            string checkSum = string.Format("{0:x2}", sum);
            checkSum = checkSum.ToUpper();
            if (checkSum.Length > 2)
            {
                checkSum = checkSum.Remove(0, 1);
            }

            string packet = string.Format("{0}{1}\r\n", preparePacket, checkSum);

            byte[] StrByte = Encoding.UTF8.GetBytes(packet);
            lightSerialPort.WritePacket(StrByte, 0, StrByte.Length);
        }
    }

    internal class LfineLightCtrl : SerialLightCtrl
    {
        public LfineLightCtrl(string name) : base(name)
        {
        }

        public override int GetMaxLightLevel()
        {
            return 1023;
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            string valueStr = lightValue.Value[StartChannelIndex + channel].ToString("0000");

            byte[] bytePacket = new byte[8];
            bytePacket[0] = 0x02;
            bytePacket[1] = (byte)(channel + '0');
            bytePacket[2] = (byte)'w';
            bytePacket[3] = (byte)valueStr[0];
            bytePacket[4] = (byte)valueStr[1];
            bytePacket[5] = (byte)valueStr[2];
            bytePacket[6] = (byte)valueStr[3];
            bytePacket[7] = 0x03;

            lightSerialPort.WritePacket(bytePacket, 0, 8);
        }
    }
}
