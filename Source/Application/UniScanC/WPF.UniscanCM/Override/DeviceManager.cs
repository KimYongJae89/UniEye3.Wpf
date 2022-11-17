using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using UniEye.Base.MachineInterface;
using UniScanC.Enums;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.MachineIf;
using WPF.UniScanCM.PLC;
using WPF.UniScanCM.PLC.Melsec;

namespace WPF.UniScanCM.Override
{
    public class DeviceManager : DynMvp.Devices.DeviceManager
    {
        #region 생성자
        #endregion


        #region 속성
        private SerialDeviceInfo SerialEncoderInfo { get; set; }
        public SerialEncoder SerialEncoder { get; set; }

        private LightValue LightValue { get; set; }
        private int[] TopLightChannels { get; set; } = null;
        private int[] BottomLightChannels { get; set; } = null;

        public PlcBase PLCMachineIf { get; set; } = null;

        private bool IsExcuteCommand { get; set; } = false;
        #endregion


        #region 메서드
        public override void Initialize(bool nonVision, IReportProgress reportProgress)
        {
            base.Initialize(false, reportProgress);
            CreateTowerLamp(BaseConfig.Instance().ConfigPath);

            InitLight();
            InitPLC();
            // 엔코더 속도를 사용하는 기능을 켰다면 엔코더를 초기화 시켜준다.
            if (SystemConfig.Instance.UseEncoderSpeed)
            {
                InitEncoder();
                ConnectSerialEncoder(SystemConfig.Instance.EncoderPort);
                StartPulseMonitor();
            }

            WriteAirPressure(true);
        }

        public override void Release()
        {
            PLCMachineIf?.Disconnect();
            DisconnectSerialEncoder();
            WriteAirPressure(false);
            if (SystemConfig.Instance.UseEncoderSpeed)
            {
                StopPulseMonitor();
                DisconnectSerialEncoder();
            }
        }

        public override void CreateTowerLamp(string configDir)
        {
            if (DeviceConfig.Instance().UseTowerLamp)
            {
                towerLamp = new TowerLamp();
                towerLamp.Setup(DigitalIoHandler, DeviceConfig.Instance().TowerLampUpdateInterval);

                if (DeviceConfig.Instance().UseSoundBuzzer)
                {
                    towerLamp.SetupPort(PortMap["OutTowerLampRed"], PortMap["OutTowerLampYellow"], PortMap["OutTowerLampGreen"], PortMap["OutTowerBuzzer"]);
                }
                else
                {
                    towerLamp.SetupPort(PortMap["OutTowerLampRed"], PortMap["OutTowerLampYellow"], PortMap["OutTowerLampGreen"], PortMap["OutTowerLampRed"]);
                }

                towerLamp.Load(configDir);
                towerLamp.Start();
            }
        }

        public override void CreatePortMap()
        {
            base.CreatePortMap();

            SystemConfig config = SystemConfig.Instance;

            // PLC
            if (config.DIStart != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InPLCStart", "PLC Start", config.DIStart));
                PortMap.GetPort("InPLCStart").Group = IoGroup.General;
            }
            //if (config.DIPLCHold != -1)
            //{
            //    PortMap.AddPort(new IoPort(IoDirection.Input, "InPLCHold", "PLC Hold", config.DIPLCHold));
            //    PortMap.GetPort("InPLCStart").Group = IoGroup.General;
            //}
            //if (config.DIPLCLotChange != -1)
            //{
            //    PortMap.AddPort(new IoPort(IoDirection.Input, "InPLCLotChange", "PLC Lot Change", config.DIPLCLotChange));
            //    PortMap.GetPort("InPLCLotChange").Group = IoGroup.General;
            //}

            // IM
            if (config.DIDefectOccured != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InDefectOccured", "Defect_Occured", config.DIDefectOccured));
                PortMap.GetPort("InDefectOccured").Group = IoGroup.General;
            }
            if (config.DIDefectReset != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InDefectReset", "Defect_Reset", config.DIDefectReset));
                PortMap.GetPort("InDefectReset").Group = IoGroup.General;
            }

            // Label
            if (config.DILabelRun != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "Vision2LabelRun", "Label_Run", config.DILabelRun));
                PortMap.GetPort("Vision2LabelRun").Group = IoGroup.General;
            }
            if (config.DILabelError != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "Vision2LabelError", "Label_Error", config.DILabelError));
                PortMap.GetPort("Vision2LabelError").Group = IoGroup.General;
            }
            if (config.DILabelEmpty != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "Vision2LabelEmpty", "Label_Empty", config.DILabelEmpty));
                PortMap.GetPort("Vision2LabelEmpty").Group = IoGroup.General;
            }

            // Out
            if (config.DOAirPressure != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutAirPressure", "Air_Pressure", config.DOAirPressure));
                PortMap.GetPort("OutAirPressure").Group = IoGroup.AirPressure;
            }
            if (config.DODefectSignal != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutPLCDefectSignal", "PLC_Defect_Signal", config.DODefectSignal));
                PortMap.GetPort("OutPLCDefectSignal").Group = IoGroup.General;
            }
            if (config.DOHeartSignal != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutHeartSignal", "Heart_Signal", config.DOHeartSignal));
                PortMap.GetPort("OutHeartSignal").Group = IoGroup.General;
            }

            // Tower Lamp
            if (config.DOTowerLampRed != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampRed", "Tower_Lamp_Red", config.DOTowerLampRed));
                PortMap.GetPort("OutTowerLampRed").Group = IoGroup.ETC;
            }
            if (config.DOTowerLampYellow != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampYellow", "Tower_Lamp_Yellow", config.DOTowerLampYellow));
                PortMap.GetPort("OutTowerLampYellow").Group = IoGroup.ETC;
            }
            if (config.DOTowerLampGreen != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampGreen", "Tower_Lamp_Green", config.DOTowerLampGreen));
                PortMap.GetPort("OutTowerLampGreen").Group = IoGroup.ETC;
            }
            if (config.DOTowerLampBuzzer != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerBuzzer", "Tower_Buzzer", config.DOTowerLampBuzzer));
                PortMap.GetPort("OutTowerBuzzer").Group = IoGroup.ETC;
            }

            // Label
            if (config.DOLabelReady != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "Vision2LabelReady", "Label_Ready", config.DOLabelReady));
                PortMap.GetPort("Vision2LabelReady").Group = IoGroup.ETC;
            }
            if (config.DOLabelPublish != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "Vision2LabelPublish", "Label_Publish", config.DOLabelPublish));
                PortMap.GetPort("Vision2LabelPublish").Group = IoGroup.General;
            }
            if (config.DOLabelReset != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "Vision2LabelReset", "Label_Reset", config.DOLabelReset));
                PortMap.GetPort("Vision2LabelReset").Group = IoGroup.General;
            }
        }


        #region Encoder
        public void InitEncoder()
        {
            SystemConfig config = SystemConfig.Instance;
            if (config.UseEncoder)
            {
                SerialEncoderInfo = SerialDeviceInfoFactory.CreateSerialDeviceInfo(ESerialDeviceType.SerialEncoder);
                SerialEncoderInfo.DeviceName = "Encoder";
                switch (config.EncoderModel)
                {
                    case "SerialV105": SerialEncoder = new SerialEncoderV105(SerialEncoderInfo); break;
                    case "SerialV107": SerialEncoder = new SerialEncoderV107(SerialEncoderInfo); break;
                    default: SerialEncoder = new SerialEncoderV105(SerialEncoderInfo); break;
                }
            }
        }

        public bool IsConnectSerialEncoder()
        {
            return SerialEncoder != null && SerialEncoder.IsOpen;
        }

        public bool ConnectSerialEncoder(string port = "")
        {
            if (DeviceConfig.Instance().VirtualMode || SerialEncoder == null)
            {
                return false;
            }

            if (!SerialEncoder.IsOpen)
            {
                if (port == "")
                {
                    port = SystemConfig.Instance.EncoderPort;
                }

                SerialEncoderInfo.SerialPortInfo.Initialize(port, 115200);
                SerialEncoder.DeviceInfo = SerialEncoderInfo;
                SerialEncoder.Initialize();
            }

            return SerialEncoder.IsOpen;
        }

        public void DisconnectSerialEncoder()
        {
            SerialEncoder?.Release();
        }

        public void StartPulseMonitor()
        {
            SerialEncoder?.StartPulseMonitor();
        }

        public void StopPulseMonitor()
        {
            SerialEncoder?.StopPulseMonitor();
        }

        public void EnableEncoder(bool enable)
        {
            SendEncoderMessage($"EN,{Convert.ToInt32(enable)}");
        }

        public void ChangeEncoderFrequency(int freq)
        {
            SendEncoderMessage($"FQ,{freq}");
        }

        public string[] SendEncoderMessage(string packetString)
        {
            return SerialEncoder.ExcuteCommand(packetString);
        }
        #endregion


        #region Light
        private void InitLight()
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                return;
            }

            LightCtrl lightCtrl = LightCtrlHandler.GetLightCtrlByIndex(0);
            if (lightCtrl != null)
            {
                int channelCount = lightCtrl.NumChannel;
                int topLightIndex = 0;
                int bottomLightIndex = 0;

                TopLightChannels = new int[channelCount / 2];
                BottomLightChannels = new int[channelCount / 2];

                for (int i = 0; i < channelCount; i++)
                {
                    if (i < (channelCount / 2))
                    {
                        TopLightChannels[topLightIndex++] = i;
                    }
                    else
                    {
                        BottomLightChannels[bottomLightIndex++] = i;
                    }
                }

                LightValue = new LightValue(channelCount);
            }
        }

        public void TopLightOn(int lightValue)
        {
            if (!(LightCtrlHandler.GetLightCtrl(0) is SerialLightCtrl lightCtrl))
            {
                return;
            }

            foreach (int ch in TopLightChannels)
            {
                LightValue[ch] = lightValue;
                lightCtrl.TurnOn(ch, LightValue);
            }
        }

        public void TopLightOff()
        {
            if (!(LightCtrlHandler.GetLightCtrl(0) is SerialLightCtrl lightCtrl))
            {
                return;
            }

            foreach (int ch in TopLightChannels)
            {
                LightValue[ch] = 0;
                lightCtrl.TurnOn(ch, LightValue);
            }
        }

        public void BottomLightOn(int lightValue)
        {
            if (!(LightCtrlHandler.GetLightCtrl(0) is SerialLightCtrl lightCtrl))
            {
                return;
            }

            foreach (int ch in BottomLightChannels)
            {
                LightValue[ch] = lightValue;
                lightCtrl.TurnOn(ch, LightValue);
            }
        }

        public void BottomLightOff()
        {
            if (!(LightCtrlHandler.GetLightCtrl(0) is SerialLightCtrl lightCtrl))
            {
                return;
            }

            foreach (int ch in BottomLightChannels)
            {
                LightValue[ch] = 0;
                lightCtrl.TurnOn(ch, LightValue);
            }
        }
        #endregion


        #region PLC
        public void InitPLC()
        {
            if (!SystemConfig.Instance.UsePLC)
            {
                return;
            }

            PLCMachineIf = PlcBase.Create();
            if (PLCMachineIf != null)
            {
                PLCMachineIf.Initialize();
                PLCMachineIf.Connect();
                PLCMachineIf.MachineIfMonitor.OnUpdated += MachineIfMonitor_OnUpdated;
            }
        }

        public void DisconnectPLC()
        {
            if (PLCMachineIf != null)
            {
                PLCMachineIf.Disconnect();
                PLCMachineIf.MachineIfMonitor.OnUpdated -= MachineIfMonitor_OnUpdated;
            }
        }

        public void SendPLCHeartSignal(bool heart)
        {

        }

        //private int TestInt { get; set; } = 0;
        private void MachineIfMonitor_OnUpdated()
        {
            UniScanC.MachineIf.MachineIfDataC MachineIfData = PLCMachineIf.MachineIfMonitor.MachineIfData;
            //if (MachineIfData is MachineIfDataCM MachineIfDataCM)
            //{
            //    MachineIfDataCM.SET_VISION_COATING_INSP_CNT_DUST = TestInt++ * 2;
            //    MachineIfDataCM.SET_VISION_COATING_INSP_CNT_PINHOLE = TestInt++ * 3;
            //    MachineIfDataCM.SET_VISION_COATING_INSP_CNT_ALL = MachineIfDataCM.SET_VISION_COATING_INSP_CNT_DUST + MachineIfDataCM.SET_VISION_COATING_INSP_CNT_PINHOLE;
            //}
        }
        #endregion


        #region IOPort
        public void SendDefectSignal(bool isExistDefect)
        {
            IoPort ioPort = PortMap.GetPort("OutPLCDefectSignal");
            if (ioPort != null)
            {
                DigitalIoHandler?.WriteOutput(ioPort, isExistDefect);
            }
        }

        public void SendIOHeartSignal(bool heart)
        {
            IoPort ioPort = PortMap.GetPort("OutHeartSignal");
            if (ioPort != null)
            {
                DigitalIoHandler?.WriteOutput(ioPort, heart);
            }
        }

        public void WriteAirPressure(bool isOn)
        {
            IoPort ioPort = PortMap.GetPort("OutAirPressure");
            if (ioPort != null)
            {
                DigitalIoHandler?.WriteOutput(ioPort, isOn);
            }
        }
        #endregion
        #endregion
    }
}
