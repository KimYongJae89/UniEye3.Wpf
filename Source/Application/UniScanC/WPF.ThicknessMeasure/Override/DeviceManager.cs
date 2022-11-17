using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System.Linq;
using System.Threading;

namespace WPF.ThicknessMeasure.Override
{
    public class DeviceManager : DynMvp.Devices.DeviceManager
    {
        #region 속성
        public DynMvp.Devices.Spectrometer.Spectrometer Spectrometer { get; set; }

        public DynMvp.Devices.Daq.DaqChannelNiDaqmx DaqChannelNiDaqmx => DaqChannelList.FirstOrDefault() as DynMvp.Devices.Daq.DaqChannelNiDaqmx;

        public string LaserPortName = "Dev2/port1";
        #endregion


        #region 메서드
        public override void Initialize(bool nonVision, IReportProgress reportProgress)
        {
            base.Initialize(false, reportProgress);

            // Spectrometer 초기화
            if (DeviceConfig.Instance().VirtualMode == true)
            {
                Spectrometer = new DynMvp.Devices.Spectrometer.VirtualSpectrometer();
            }
            else
            {
                Spectrometer = new DynMvp.Devices.Spectrometer.Spectrometer();
            }

            Spectrometer.Initialize(SystemConfig.Instance.SpectrometerProperty);
            SystemConfig.Instance.Save();

            if (DeviceConfig.Instance().UseRobotStage == true)
            {
                AxisHandler robotStage = DeviceManager.Instance().RobotStage;

                // 모션별 펄스 값 입력하여 구분
                //if (robotStage.AxisList[0].Motion.MotionType == MotionType.AlphaMotionBx)
                //    robotStage.AxisList[0].AxisParam.MicronPerPulse = 1f;
                //else if (robotStage.AxisList[0].Motion.MotionType == MotionType.FastechEziMotionPlusR)
                //    robotStage.AxisList[0].AxisParam.MicronPerPulse = 10f;

                robotStage.AxisList[0].HomeOrder = 0;

                // Servo Off
                robotStage.TurnOnServo(false);

                // Servo Reset
                robotStage.ResetAlarmOn(true);
                Thread.Sleep(1000);
                robotStage.ResetAlarmOn(false);

                // Servo On
                robotStage.TurnOnServo(true);
            }

            CreateTowerLamp(BaseConfig.Instance().ConfigPath);

            // 할로겐 램프 켜기
            foreach (IoPort ioPort in portMap.GetPorts(IoGroup.Light))
            {
                digitalIoHandler.WriteOutput(ioPort, true);
            }

            // 레이져 켜기
            LaserOn(true, 3);
        }

        public override void Release()
        {
            base.Release();

            // 타워램프 끄기
            if (DeviceConfig.Instance().UseTowerLamp == true)
            {
                TowerLamp?.Stop();
            }

            // 모션 끄기
            if (DeviceConfig.Instance().UseRobotStage == true)
            {
                AxisHandler robotStage = DeviceManager.Instance().RobotStage;

                // Servo Off
                robotStage.TurnOnServo(false);
            }

            // 할로겐 램프 끄기
            foreach (IoPort ioPort in portMap.GetPorts(IoGroup.Light))
            {
                digitalIoHandler.WriteOutput(ioPort, false);
            }

            // 레이져 끄기
            LaserOn(false, 0);
        }

        public override void CreatePortMap()
        {
            base.CreatePortMap();

            SystemConfig config = SystemConfig.Instance;

            // In
            if (config.EmergencyFront != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InEmergencyFront", "Emergency Front", config.EmergencyFront));
                PortMap.GetPort("InEmergencyFront").Group = IoGroup.Emergency;
            }
            if (config.EmergencyBack != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InEmergencyBack", "Emergency Back", config.EmergencyBack));
                PortMap.GetPort("InEmergencyBack").Group = IoGroup.Emergency;
            }
            if (config.AreaSensor != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InAreaSensor", "Area Sensor", config.AreaSensor));
                PortMap.GetPort("InAreaSensor").Group = IoGroup.Door;
            }
            // Out
            if (config.TowerLampRed != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampRed", "Tower Lamp Red", config.TowerLampRed));
                PortMap.GetPort("OutTowerLampRed").Group = IoGroup.General;
            }
            if (config.TowerLampYellow != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampYellow", "Tower Lamp Yellow", config.TowerLampYellow));
                PortMap.GetPort("OutTowerLampYellow").Group = IoGroup.General;
            }
            if (config.TowerLampGreen != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerLampGreen", "Tower Lamp Green", config.TowerLampGreen));
                PortMap.GetPort("OutTowerLampGreen").Group = IoGroup.General;
            }
            if (config.TowerLampBuzzer != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutTowerBuzzer", "Tower Buzzer", config.TowerLampBuzzer));
                PortMap.GetPort("OutTowerBuzzer").Group = IoGroup.General;
            }
            if (config.HalogenLampVIS != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutHalogenLampVisible", "Halogen Lamp Visible", config.HalogenLampVIS));
                PortMap.GetPort("OutHalogenLampVisible").Group = IoGroup.Light;
            }
            if (config.HalogenLampNIR != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Output, "OutHalogenLampNIR", "Halogen Lamp NIR", config.HalogenLampNIR));
                PortMap.GetPort("OutHalogenLampNIR").Group = IoGroup.Light;
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

        public double[,] GetAngleData()
        {
            return DaqChannelNiDaqmx.ReadAnalogMultiData(-1);
        }

        public void AngleScanStartStop(bool start, bool isContinuous = true)
        {
            if (start)
            {
                DaqChannelNiDaqmx.CreateMultiAnalogReader();
            }
            else
            {
                DaqChannelNiDaqmx.DisposeMultiAnalogReader();
            }
        }

        public void LaserOn(bool isOn, int data)
        {
            if (isOn)
            {
                DaqChannelNiDaqmx.WriteData(LaserPortName, data);
            }
            else
            {
                DaqChannelNiDaqmx.WriteData(LaserPortName, 0);
            }
        }
        #endregion
    }
}