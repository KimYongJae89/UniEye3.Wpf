using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.Light.SerialLigth.iCore
{
    public class SerialLightCtrlInfoIPulse : SerialLightCtrlInfo
    {
        public byte SlaveId { get; set; }

        public OperationMode OperationMode { get; set; }

        public TriggerInputSource TriggerInputSource { get; set; }

        public TriggerInputActivation TriggerInputActivation { get; set; }

        public TiggerOutputSource TiggerOutputSource { get; set; }

        public TiggerOutputInverter TiggerOutputInverter { get; set; }

        public ushort MaxVoltage { get; set; }

        public float TimeDuration { get; set; }

        public bool LPFMode { get; set; }

        public SeqInfo SequenceInfo { get; }

        public SerialLightCtrlInfoIPulse() : base()
        {
            SlaveId = 0;
            OperationMode = OperationMode.Continuous;
            TriggerInputSource = TriggerInputSource.DigitalIO;
            TriggerInputActivation = TriggerInputActivation.Rising;
            TiggerOutputSource = TiggerOutputSource.LED_Output_Sync;
            TiggerOutputInverter = TiggerOutputInverter.None;

            SequenceInfo = new SeqInfo();
            MaxVoltage = 24;
            TimeDuration = 5.0f;

            LPFMode = false;
        }

        public override LightCtrlInfo Clone()
        {
            var serialLightCtrlInfoIPulse = new SerialLightCtrlInfoIPulse();
            serialLightCtrlInfoIPulse.Copy(this);
            return serialLightCtrlInfoIPulse;
        }
        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            XmlHelper.SetValue(lightInfoElement, "SlaveId", SlaveId);
            XmlHelper.SetValue(lightInfoElement, "OperationMode", OperationMode);
            XmlHelper.SetValue(lightInfoElement, "TriggerInputSource", TriggerInputSource);
            XmlHelper.SetValue(lightInfoElement, "TriggerInputActivation", TriggerInputActivation);
            XmlHelper.SetValue(lightInfoElement, "TiggerOutputSource", TiggerOutputSource);
            XmlHelper.SetValue(lightInfoElement, "TiggerOutputInverter", TiggerOutputInverter);

            XmlHelper.SetValue(lightInfoElement, "MaxVoltage", MaxVoltage);
            XmlHelper.SetValue(lightInfoElement, "TimeDuration", TimeDuration);

            XmlHelper.SetValue(lightInfoElement, "LPFMode", LPFMode);

            SequenceInfo.SaveXml(lightInfoElement, "SequenceInfo");
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            SlaveId = XmlHelper.GetValue(lightInfoElement, "SlaveId", SlaveId);

            OperationMode = XmlHelper.GetValue(lightInfoElement, "OperationMode", OperationMode);
            TriggerInputSource = XmlHelper.GetValue(lightInfoElement, "TriggerInputSource", TriggerInputSource);
            TriggerInputActivation = XmlHelper.GetValue(lightInfoElement, "TriggerInputActivation", TriggerInputActivation);
            TiggerOutputSource = XmlHelper.GetValue(lightInfoElement, "TiggerOutputSource", TiggerOutputSource);
            TiggerOutputInverter = XmlHelper.GetValue(lightInfoElement, "TiggerOutputInverter", TiggerOutputInverter);

            MaxVoltage = XmlHelper.GetValue(lightInfoElement, "MaxVoltage", MaxVoltage);
            TimeDuration = XmlHelper.GetValue(lightInfoElement, "TimeDuration", TimeDuration);

            LPFMode = XmlHelper.GetValue(lightInfoElement, "LPFMode", LPFMode);

            SequenceInfo.LoadXml(lightInfoElement, "SequenceInfo");
        }

        public override void Copy(LightCtrlInfo srcInfo)
        {
            base.Copy(srcInfo);

            var serialLightInfoIPulse = (SerialLightCtrlInfoIPulse)srcInfo;

            SlaveId = serialLightInfoIPulse.SlaveId;
            OperationMode = serialLightInfoIPulse.OperationMode;

            MaxVoltage = serialLightInfoIPulse.MaxVoltage;
            TimeDuration = serialLightInfoIPulse.TimeDuration;

            LPFMode = serialLightInfoIPulse.LPFMode;

            SequenceInfo.Copy(serialLightInfoIPulse.SequenceInfo);
        }

        public override Form GetAdvancedConfigForm()
        {
            return new iCoreConfigForm(this);
        }
    }

    public class SerialLightIPulse : SerialLightCtrl
    {
        private List<byte> recivedByteList = null;

        private SerialLightCtrlInfoIPulse serialLightCtrlInfo;

        public override int GetMaxLightLevel()
        {
            return serialLightCtrlInfo.OperationMode == OperationMode.Continuous ? 100 : 1000;
        }

        public SerialLightIPulse(LightCtrlInfo lightCtrlInfo) : base(lightCtrlInfo.Name)
        {
            serialLightCtrlInfo = lightCtrlInfo as SerialLightCtrlInfoIPulse;
            recivedByteList = new List<byte>();
        }

        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
        {
            var info = lightCtrlInfo as SerialLightCtrlInfoIPulse;

            bool ok = base.Initialize(info);
            if (!ok)
            {
                return false;
            }

            /*
             * this.slaveId = 0;
             * this.operationMode = OperationMode.Continuous;
             * this.triggerInputSource = TriggerInputSource.DigitalIO;
             * this.triggerInputActivation = TriggerInputActivation.Rising;
             * this.tiggerOutputSource = TiggerOutputSource.LED_Output_Sync;
             * this.tiggerOutputInverter = TiggerOutputInverter.None;
             */

            var frameList = new List<IPulseFrame>();
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.OperationMode_RW, info.OperationMode));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.Voltage_RW, info.MaxVoltage));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.Duration_RW, info.TimeDuration));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.LPFMode_RW, info.LPFMode));

            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceMode_RW, info.SequenceInfo.Mode));
            if (info.SequenceInfo.Mode != SequenceMode.Off)
            {
                SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceNumber_RW, info.SequenceInfo.Count));
                SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceData_RW, info.SequenceInfo.Sequences.Take(info.SequenceInfo.Count).ToArray()));
            }

            SendCommand(frameList.ToArray());
            return true;
        }

        protected override PacketParser CreatePacketParser()
        {
            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataReceived;
            return packetParser;
        }

        private void DataReceived(ReceivedPacket receivedPacket)
        {
            string[] ss = Array.ConvertAll(receivedPacket.ReceivedDataByte, f => f.ToString("X02"));
            System.Diagnostics.Debug.WriteLine(string.Format("lightSerialPort_PacketReceived - [{0}] {1}", ss.Length, string.Join(", ", ss)));
            byte[] sent = (byte[])this.sent;
            if (sent == null)
            {
                return;
            }

            recivedByteList.AddRange(receivedPacket.ReceivedDataByte);
            if (recivedByteList.Count > 3)
            {
                // 받은 명령어의 앞 2개 바이트가 보낸 명령어의 앞 2개 바이트와 일치하는지 확인.
                int idx0 = recivedByteList.IndexOf(sent[0]);
                if (idx0 < 0)
                {
                    return;
                }

                int idx1 = recivedByteList.IndexOf(sent[1], idx0);
                if (idx1 != idx0 + 1)
                {
                    return;
                }

                int totlaLength;
                if (sent[1] == (byte)Function.Write)
                {
                    // Write 응답: 받은 명령어의 전체 길이와 보낸 명령어의 전체 길이 확인.
                    if (recivedByteList.Count - idx0 < sent.Length)
                    {
                        return;
                    }

                    totlaLength = sent.Length;
                }
                else
                {
                    // Read 응답: 받은 명령어의 3번째 바이트의 값과 전체 데이터의 길이 확인.
                    if (recivedByteList.Count < idx0 + 3)
                    {
                        return;
                    }

                    int dataLength = recivedByteList[idx1 + 1];
                    totlaLength = 2 + 1 + dataLength + 2; // 헤더 2, 길이 1, 데이터 n, crc 2
                }

                if (recivedByteList.Count < idx0 + totlaLength)
                {
                    return;
                }

                responce = recivedByteList.GetRange(idx0, totlaLength).ToArray();
                recivedByteList.RemoveRange(idx0, totlaLength);
                responseReceived.Set();
            }
        }

        public IPulseFrame[] SendCommand(IPulseFrame[] frames)
        {
            var serialLightCtrlInfo = (SerialLightCtrlInfo)this.serialLightCtrlInfo;
            IPulseFrame[] responces = frames.Select(f => SendCommand(f)).ToArray();
            return responces;
        }

        public IPulseFrame SendCommand(IPulseFrame frame)
        {
            responseReceived.Reset();
            responce = null;
            sent = null;

            byte[] bytes = frame.ToBytes();

            string[] ss = Array.ConvertAll(bytes, g => g.ToString("X02"));
            //Debug.WriteLine(string.Format("SerialLightCtrlIPulse - [{0}] {1}", ss.Length, string.Join(", ", ss)));

            sent = bytes;
            lightSerialPort.WritePacket(bytes, 0, bytes.Length);
            if (frame.IsRead/* || lightCtrlInfo.ResponceTimeoutMs != 0*/)
            // Wait Responce
            {
                int timeout = /*lightCtrlInfo.ResponceTimeoutMs;*/ 0;
                if (timeout == 0)
                {
                    timeout = 1000;
                }

                bool waitDone = responseReceived.WaitOne(timeout);
                if (!waitDone)
                {
                    throw new Exception("Light Contoller has no responce");
                }

                var recivedFrame = new IPulseFrame(frame.IsRead, (byte[])responce);
                return recivedFrame;
            }
            return null;
        }

        public override void TurnOn(LightValue lightValue)
        {
            var serialLightCtrlInfo = (SerialLightCtrlInfo)this.serialLightCtrlInfo;
            lightValue.Clip(GetMaxLightLevel());

            var frameList = new List<IPulseFrame>();
            frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, Address.OperationMode_RW, (ushort)this.serialLightCtrlInfo.OperationMode));
            Address startAddress = this.serialLightCtrlInfo.OperationMode == OperationMode.Continuous ? Address.LED1CurrentRateContinuous_RW : Address.LED1CurrentRatePulse_RW;

            for (int i = 0; i < lightValue.NumLight; i++)
            {
                bool turnOn = lightValue.Value[i] > 0;
                frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, Address.LED1Enable_RW + i, turnOn));
                if (turnOn)
                {
                    frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, startAddress + i, (ushort)lightValue.Value[i]));
                }
            }

            SendCommand(frameList.ToArray());
        }

        public override void TurnOn(int channel, LightValue lightValue)
        {
            if (lightValue.Value[channel] <= 0)
            {
                return;
            }

            var serialLightCtrlInfo = (SerialLightCtrlInfo)this.serialLightCtrlInfo;
            lightValue.Clip(GetMaxLightLevel());

            var frameList = new List<IPulseFrame>();
            frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, Address.OperationMode_RW, (ushort)this.serialLightCtrlInfo.OperationMode));
            Address startAddress = this.serialLightCtrlInfo.OperationMode == OperationMode.Continuous ? Address.LED1CurrentRateContinuous_RW : Address.LED1CurrentRatePulse_RW;

            bool turnOn = lightValue.Value[channel] > 0;
            frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, Address.LED1Enable_RW + channel, turnOn));
            if (turnOn)
            {
                frameList.Add(IPulseFrame.CreateWFrame(this.serialLightCtrlInfo.SlaveId, startAddress + channel, (ushort)lightValue.Value[channel]));
            }

            SendCommand(frameList.ToArray());
        }

        //public override LightValue GetLightValue()
        //{
        //    List<IPulseFrame> frameList = new List<IPulseFrame>();
        //    for (int i = 0; i < this.lightCtrlInfo.NumChannel; i++)
        //        frameList.Add(IPulseFrame.CreateRFrame(9, Address.LED1Enable_RW + i, 0x0001));
        //    return base.GetLightValue();
        //}
    }
}
