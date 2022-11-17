using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.UI;

namespace UniEye.Base.Devices
{
    /// <summary>
    /// 시스템에 연결된 장치 목록을 관리
    /// </summary>
    public class DeviceManager : DynMvp.Devices.DeviceManager
    {
        public DeviceManager()
        {
        }

        public override void CreatePortMap()
        {
            PortMap = new PortMap();

            PortMap.AddPort(new IoPort(IoDirection.Input, "InEmergency", "Emergency"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InStartSw", "Start Switch"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InStopSw", "Stop Switch"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InResetSw", "Reset Switch"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InDoorOpen1", "Door Open1"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InDoorOpen2", "Door Open2"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InDoorOpen3", "Door Open3"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InAirPressureLow", "Air Pressure Low"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutStartLamp", "Start Lamp"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutStopLamp", "Stop Lamp"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutResetLamp", "Reset Lamp"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutTowerLampRed", "Tower Lamp Red"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutTowerLampYellow", "Tower Lamp Yellow"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutTowerLampGreen", "Tower Lamp Green"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutTowerBuzzer", "Tower Buzzer"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutLight1", "Light 1"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutLight2", "Light 2"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutLight3", "Light 3"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutVisionReady", "Vision Ready"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutOnWorking", "On Working"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutCommandWait", "Command Wait"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InInspStart", "Insp Start"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InInspEnd", "Insp End"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InTrigger", "Trigger"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InTriggerCh1", "Trigger Ch1"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InTriggerCh2", "Trigger Ch2"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutComplete", "Complete"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "InCommandDone", "Command Done"));
            PortMap.AddPort(new IoPort(IoDirection.Input, "OutResultNg", "Result NG"));
        }

        public bool IsInnerExist()
        {
            return false;
        }

        //public override void RobotOrigin()
        //{
        //    AxisHandler robotStage = DeviceManager.Instance().RobotStage;
        //    if (robotStage == null)
        //        return;

        //    LogHelper.Debug(LoggerType.StartUp, "Start RobotOrigin");

        //    string message = StringManager.GetString("Do you want the robot move to origin?");

        //    Form mainForm = (Form)UiManager.Instance().MainForm;

        //    if (MessageForm.Show(mainForm, message, MessageFormType.YesNo) == DialogResult.Yes)
        //    {
        //        if (IsInnerExist() == true)
        //        {
        //            string message2 = StringManager.GetString("Keep clear in the machine.");
        //            if (MessageForm.Show(mainForm, message2, MessageFormType.RetryCancel) == DialogResult.Cancel)
        //            {
        //                return;
        //            }
        //        }

        //        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        //        SimpleProgressForm loadingForm = new SimpleProgressForm("Move to origin");
        //        loadingForm.Show(new Action(() =>
        //        {
        //            robotStage.HomeMove(cancellationTokenSource.Token);
        //        }), cancellationTokenSource);
        //    }
        //}
    }
}