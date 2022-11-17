using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;
using UniScanC.Data;
using UniScanC.Enums;
using WPF.UniScanCM.MachineIf;
using WPF.UniScanCM.MachineIf.DataAdapter;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC.AllenBreadley;
using WPF.UniScanCM.PLC.Melsec;
using WPF.UniScanCM.Service;

namespace WPF.UniScanCM.PLC
{
    public delegate void StringValueDelegate(string value);
    public delegate void FloatValueDelegate(float value);
    public delegate void DoubleValueDelegate(double value);
    public delegate void BoolValueDelegate(bool value);

    public abstract class PlcBase
    {
        #region 생성자
        public PlcBase() { }
        #endregion


        #region 속성
        public UniEye.Base.MachineInterface.MachineIf MachineIf { get; set; }

        public MachineIfMonitorCM MachineIfMonitor { get; protected set; }

        public MachineIfDataCM PreviousMachineIfData { get; set; }
        #endregion


        #region 대리자
        public BoolValueDelegate MachineReadyChanged { get; set; }
        public BoolValueDelegate MachineStartChanged { get; set; }

        public BoolValueDelegate CoatingStartChanged { get; set; }
        public BoolValueDelegate GlossStartChanged { get; set; }
        public BoolValueDelegate ThicknessStartChanged { get; set; }

        public FloatValueDelegate TargetSpeedChanged { get; set; }
        public FloatValueDelegate CurrentSpeedChanged { get; set; }
        public FloatValueDelegate TargetPositionChanged { get; set; }
        public FloatValueDelegate CurrentPositionChanged { get; set; }

        public StringValueDelegate LotNoChanged { get; set; }
        public StringValueDelegate ModelChanged { get; set; }
        public StringValueDelegate WorkerChanged { get; set; }
        public StringValueDelegate PasteChanged { get; set; }

        public BoolValueDelegate AlarmOccured { get; set; }
        #endregion


        #region 메서드
        public static PlcBase Create()
        {
            switch (SystemConfig.Instance.PlcType)
            {
                case EPlcType.Melsec:
                    return new MelsecPLC();
                case EPlcType.AllenBreadley:
                    return new AllenBreadleyPLC();
                case EPlcType.None:
                default: return null;
            }
        }

        public void Initialize()
        {
            EPlcType plcType = SystemConfig.Instance.PlcType;
            if (plcType != EPlcType.None)
            {
                MachineIfDataAdapter adapter = null;
                MachineIfSetting machineIfSetting = SystemConfig.Instance.MachineIfSetting;
                switch (plcType)
                {
                    case EPlcType.Melsec:
                        MachineIf = new MelsecMachineIf(machineIfSetting);
                        adapter = new MelsecMachineIfDataAdapterCM(new MachineIfDataCM());
                        break;
                    case EPlcType.AllenBreadley:
                        MachineIf = new AllenBreadleyMachineIf(machineIfSetting);
                        adapter = new ABMachineIfDataAdapterCM(new MachineIfDataCM());
                        break;
                }

                MachineIfMonitor = new MachineIfMonitorCM(adapter);
            }
        }

        public void Connect()
        {
            MachineIf.Start();
            MachineIfMonitor.OnUpdated += MachineIfMonitor_OnUpdated;
            MachineIfMonitor.Start();
        }

        public void Disconnect()
        {
            MachineIf.Stop();
            MachineIfMonitor.Stop();
            MachineIfMonitor.OnUpdated -= MachineIfMonitor_OnUpdated;
        }

        public bool IsConnected()
        {
            return MachineIf.IsStarted();
        }

        public void UpdateRealTimeDefectAlarm(List<InspectResult> inspectResults)
        {
            var machineIfData = MachineIfMonitor.MachineIfData as MachineIfDataCM;
            // 데이터 수정
            lock (machineIfData)
            {
                machineIfData.SET_VISION_COATING_INSP_NG_DUST = DefectAlarmService.HasDefectType(inspectResults, EDefectType.Dust);
                machineIfData.SET_VISION_COATING_INSP_NG_PINHOLE = DefectAlarmService.HasDefectType(inspectResults, EDefectType.Pinhole);
            }
        }

        public void UpdateRealTimeDefectCountResult()
        {
            var machineIfData = MachineIfMonitor.MachineIfData as MachineIfDataCM;
            // 데이터 수정
            lock (machineIfData)
            {
                machineIfData.SET_VISION_COATING_INSP_CNT_ALL = (int)Math.Round(DefectCountService.GetAllDefectCount(false));
                machineIfData.SET_VISION_COATING_INSP_CNT_DUST = (int)Math.Round(DefectCountService.GetDefectCount(EDefectType.Dust, false));
                machineIfData.SET_VISION_COATING_INSP_CNT_PINHOLE = (int)Math.Round(DefectCountService.GetDefectCount(EDefectType.Pinhole, false));
            }
        }

        public void UpdateTotalDefectCountResult()
        {
            var machineIfData = MachineIfMonitor.MachineIfData as MachineIfDataCM;
            // 데이터 수정
            lock (machineIfData)
            {
                machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_ALL = (int)Math.Round(DefectCountService.GetAllDefectCount(true));
                machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_DUST = (int)Math.Round(DefectCountService.GetDefectCount(EDefectType.Dust, true));
                machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE = (int)Math.Round(DefectCountService.GetDefectCount(EDefectType.Pinhole, true));
            }
        }

        private void MachineIfMonitor_OnUpdated()
        {
            if (PreviousMachineIfData != null)
            {
                // 장비 준비
                if (MachineIfMonitor.MachineIfData.GET_READY_MACHINE != PreviousMachineIfData.GET_READY_MACHINE)
                {
                    MachineReadyChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_READY_MACHINE);
                }
                //// 장비 운영 시작
                //if (MachineIfMonitor.MachineIfData.GET_START_MACHINE != PreviousMachineIfData.GET_START_MACHINE)
                //    MachineStartChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_START_MACHINE);

                // 성형 검사 시작
                if (MachineIfMonitor.MachineIfData.GET_START_COATING != PreviousMachineIfData.GET_START_COATING)
                {
                    CoatingStartChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_START_COATING);
                }
                //// 광택도 검사 시작
                //if (MachineIfMonitor.MachineIfData.GET_START_GLOSS != PreviousMachineIfData.GET_START_GLOSS)
                //    GlossStartChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_START_GLOSS);
                //// 두께 검사 시작
                //if (MachineIfMonitor.MachineIfData.GET_START_THICKNESS != PreviousMachineIfData.GET_START_THICKNESS)
                //    ThicknessStartChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_START_THICKNESS);

                // 목표 속도
                if (MachineIfMonitor.MachineIfData.GET_TARGET_SPEED != PreviousMachineIfData.GET_TARGET_SPEED)
                {
                    TargetSpeedChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_TARGET_SPEED);
                }
                // 현재 속도
                if (MachineIfMonitor.MachineIfData.GET_PRESENT_SPEED != PreviousMachineIfData.GET_PRESENT_SPEED)
                {
                    CurrentSpeedChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_PRESENT_SPEED);
                }
                // 목표 거리
                if (MachineIfMonitor.MachineIfData.GET_TARGET_POSITION != PreviousMachineIfData.GET_TARGET_POSITION)
                {
                    TargetPositionChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_TARGET_POSITION);
                }
                // 현재 거리
                if (MachineIfMonitor.MachineIfData.GET_PRESENT_POSITION != PreviousMachineIfData.GET_PRESENT_POSITION)
                {
                    CurrentPositionChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_PRESENT_POSITION);
                }

                //// Lot No
                //if (MachineIfMonitor.MachineIfData.GET_LOT != PreviousMachineIfData.GET_LOT)
                //    LotNoChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_LOT);
                //// 모델
                //if (MachineIfMonitor.MachineIfData.GET_MODEL != PreviousMachineIfData.GET_MODEL)
                //    ModelChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_MODEL);
                //// 작업자
                //if (MachineIfMonitor.MachineIfData.GET_WORKER != PreviousMachineIfData.GET_WORKER)
                //    WorkerChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_WORKER);
                //// 슬러리
                //if (MachineIfMonitor.MachineIfData.GET_PASTE != PreviousMachineIfData.GET_PASTE)
                //    PasteChanged?.Invoke(MachineIfMonitor.MachineIfData.GET_PASTE);
            }

            PreviousMachineIfData = MachineIfMonitor.MachineIfData.Clone() as MachineIfDataCM;
        }
        #endregion
    }
}
