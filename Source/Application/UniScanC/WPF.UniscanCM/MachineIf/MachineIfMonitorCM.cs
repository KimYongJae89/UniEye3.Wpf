using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanC.Enums;
using UniScanC.MachineIf;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Service;

namespace WPF.UniScanCM.MachineIf
{
    public class MachineIfMonitorCM : UniScanC.MachineIf.MachineIfMonitorC
    {
        #region 생성자
        public MachineIfMonitorCM(MachineIfDataAdapter adapter) : base(adapter) { }
        #endregion


        #region 이벤트
        public event OnUpdatedDelegate OnUpdated;
        #endregion


        #region 속성
        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;
        #endregion


        #region 메서드
        public override void PropagateData()
        {
            var machineIfData = (MachineIfDataCM)Adapter.MachineIfData;

        }

        public override void ApplyData()
        {
            var machineIfData = (MachineIfDataCM)Adapter.MachineIfData;
            machineIfData.SET_VISION_COATING_INSP_READY = AliveService.Heart;
            machineIfData.SET_VISION_COATING_INSP_RUNNING = SystemState.Instance().OpState == OpState.Inspect;
            machineIfData.SET_VISION_COATING_INSP_ERROR = ErrorManager.Instance().IsAlarmed();
        }

        public override void Start()
        {
            if (Thread == null)
            {
                Thread = new ThreadHandler("MachineIfMonitor", new System.Threading.Thread(ThreadProc));

                if (DeviceManager != null && DeviceManager.PLCMachineIf != null)
                {
                    Thread.Start();
                }
            }
        }

        public override void Stop()
        {
            Thread?.Stop();
            Thread = null;
        }

        public override void ThreadProc()
        {
            //bool isVirtual = SystemManager.Instance().DeviceBox.MachineIf.IsVirtual;
            while (!Thread.RequestStop)
            {
                try
                {
                    bool wasConnect = Adapter.MachineIfData.IsConnected;
                    bool isConnect = DeviceManager.PLCMachineIf.IsConnected();
                    if (wasConnect && !isConnect)
                    // 연결 되어 있다가 끊어짐.
                    {
                        //ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Disconnected, ErrorLevel.Info, "Printer", "Printer Disconnected.", null, ""));
                        MachineIfData.Reset();
                    }
                    else if (isConnect && !wasConnect)
                    // 끊겨 있다가 연결됨.
                    {
                        //ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected, ErrorLevel.Info, "Printer", "Printer Connected.", null, ""));
                    }
                    MachineIfData.IsConnected = isConnect;

                    if (isConnect)
                    {
                        //if (isVirtual)
                        //{
                        //    VirtualRead();
                        //    VirtualWrite();
                        //}
                        //else
                        //{
                        Read();
                        Write();
                        //}
                    }

                    OnUpdated?.Invoke();
                    System.Threading.Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, $"MachineIfMonitor::ThreadProc - {ex.GetType().Name} : {ex.Message}");
                }
            }
        }
        #endregion
    }
}
