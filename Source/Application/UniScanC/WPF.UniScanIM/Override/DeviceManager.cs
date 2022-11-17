using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Translation.Helpers;
using UniScanC.Comm;
using UniScanC.Enums;

namespace WPF.UniScanIM.Override
{
    public class DeviceManager : DynMvp.Devices.DeviceManager
    {
        public override void Initialize(bool nonVision, IReportProgress reportProgress)
        {
            base.Initialize(nonVision, reportProgress);

            foreach (Camera camera in CameraHandler.cameraList)
            {
                if (camera is CameraPylonLine cameraPylonLine)
                {
                    cameraPylonLine.CameraConnectionLost += CameraPylonLine_CameraConnectionLost;
                }
            }
        }

        private void CameraPylonLine_CameraConnectionLost(CameraInfo cameraInfo)
        {
            var cameraInfoPylonLine = cameraInfo as CameraInfoPylonLine;
            string message = string.Format("Pylon camera connection lost. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} ",
                    cameraInfoPylonLine.DeviceIndex, cameraInfoPylonLine.DeviceUserId, cameraInfoPylonLine.IpAddress, cameraInfoPylonLine.SerialNo);
            CommManager.Instance().SendMessage(EUniScanCCommand.Alarm, message);
        }

        public override void CreatePortMap()
        {
            base.CreatePortMap();

            SystemConfig config = SystemConfig.Instance;

            // Out
            foreach (UniScanC.Data.ModuleInfo module in config.ModuleList.ToList())
            {
                if (module.DefectSignalPort != -1)
                {
                    PortMap.AddPort(new IoPort(IoDirection.Output, $"OutDefectSignal{module.ModuleNo}", $"Defect Signal {module.ModuleNo}", module.DefectSignalPort));
                    PortMap.GetPort($"OutDefectSignal{module.ModuleNo}").Group = IoGroup.General;
                }
            }

            // In
            if (config.DIFrameTriggerSignal != -1)
            {
                PortMap.AddPort(new IoPort(IoDirection.Input, "InFrameTriggerSignal", "Frame Trigger Signal", config.DIFrameTriggerSignal));
                PortMap.GetPort("InFrameTriggerSignal").Group = IoGroup.General;
            }
        }

        #region IOPort
        public void SendDefectSignal(int moduleNo, bool isOn)
        {
            LogHelper.Debug(LoggerType.Comm, $"[PLC IO] Module {moduleNo} / NG Signal {isOn}");

            if (DigitalIoHandler.Get(0) is DigitalIoSerial digitalIoSerial)
            {
                if (isOn)
                {
                    digitalIoSerial.WriteOutputGroup(0, 2);
                }
                else
                {
                    digitalIoSerial.WriteOutputGroup(0, 0);
                }
            }
            else
            {
                IoPort ioPort = PortMap.GetPort($"OutDefectSignal{moduleNo}");
                if (ioPort != null)
                {
                    DigitalIoHandler.WriteOutput(ioPort, isOn);
                }
            }
        }
        #endregion
    }
}
