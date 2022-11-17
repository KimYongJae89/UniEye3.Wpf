using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using System;
using System.Linq;
using System.Threading;

namespace WPF.UniScanCM.Override
{
    public delegate void BooleanDelegate(bool value);

    public class DeviceMonitor : DynMvp.Devices.DeviceMonitor
    {
        #region 생성자
        #endregion


        #region 속성
        public BooleanDelegate IOStartChanged { get; set; }
        public BooleanDelegate IOHoldChanged { get; set; }
        public BooleanDelegate IOLotChanged { get; set; }

        public BooleanDelegate DefectOccrued { get; set; }
        public BooleanDelegate DefectReset { get; set; }

        public BooleanDelegate LabelRunChanged { get; set; }
        public BooleanDelegate LabelErrorChanged { get; set; }
        public BooleanDelegate LabelEmptyChanged { get; set; }

        public bool IsPlcStart { get; set; }
        public bool IsPlcHold { get; set; }
        public bool IsPlcLotChanged { get; set; }

        public bool IsDefectOccrued { get; set; }
        public bool IsDefectReset { get; set; }

        public bool IsLabelRun { get; set; }
        public bool IsLabelError { get; set; }
        public bool IsLabelEmpty { get; set; }

        private InputStateHandler PlcStartState { get; set; }
        //private InputStateHandler plcHoldState;
        //private InputStateHandler plcLotChangeState;

        private InputStateHandler DefectOccruedState { get; set; }
        private InputStateHandler DefectResetState { get; set; }

        private InputStateHandler LabelRunState { get; set; }
        private InputStateHandler LabelErrorState { get; set; }
        private InputStateHandler LabelEmptyState { get; set; }
        #endregion


        #region 메서드
        public override void Initialize()
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                return;
            }

            IgnoreAirPresure = true;

            PortMap portMap = DeviceManager.Instance().PortMap;
            DigitalIoHandler digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;

            IoPort plcStartPort = portMap.GetPort("InPLCStart");
            if (plcStartPort != null)
            {
                PlcStartState = new InputStateHandler("InPLCStart", digitalIoHandler, plcStartPort);
                PlcStartState.OnInputOn += plcStartState_OnInputOn;
                PlcStartState.OnInputOff += plcStartState_OnInputOff;
            }
            //IoPort plcHoldPort = portMap.GetPort("InPLCHold");
            //if (plcHoldPort != null)
            //{
            //plcHoldState = new InputStateHandler("PLC_Hold", digitalIoHandler, plcHoldPort);
            //plcHoldState.OnInputOn += plcHoldState_OnInputOn;
            //plcHoldState.OnInputOff += plcHoldState_OnInputOff;
            //}
            //IoPort plcLotChangePort = portMap.GetPort("InPLCLotChange");
            //if (plcLotChangePort != null)
            //{
            //    plcLotChangeState = new InputStateHandler("PLC_LotChange", digitalIoHandler, plcLotChangePort);
            //    plcLotChangeState.OnInputOn += plcLotChangeState_OnInputOn;
            //    plcLotChangeState.OnInputOff += plcLotChangeState_OnInputOff;
            //}
            IoPort DefectOccuredPort = portMap.GetPort("InDefectOccured");
            if (DefectOccuredPort != null)
            {
                DefectOccruedState = new InputStateHandler("InDefectOccured", digitalIoHandler, DefectOccuredPort);
                DefectOccruedState.OnInputOn += DefectOccruedState_OnInputOn;
                DefectOccruedState.OnInputOff += DefectOccruedState_OnInputOff;
            }
            IoPort DefectResetPort = portMap.GetPort("InDefectReset");
            if (DefectResetPort != null)
            {
                DefectResetState = new InputStateHandler("InDefectReset", digitalIoHandler, DefectResetPort);
                DefectResetState.OnInputOn += DefectResetState_OnInputOn;
                DefectResetState.OnInputOff += DefectResetState_OnInputOff;
            }
            IoPort labelRunPort = portMap.GetPort("Vision2LabelRun");
            if (labelRunPort != null)
            {
                LabelRunState = new InputStateHandler("Vision2LabelRun", digitalIoHandler, labelRunPort);
                LabelRunState.OnInputOn += labelRunState_OnInputOn;
                LabelRunState.OnInputOff += labelRunState_OnInputOff;
            }
            IoPort labelErrorPort = portMap.GetPort("Vision2LabelError");
            if (labelErrorPort != null)
            {
                LabelErrorState = new InputStateHandler("Vision2LabelError", digitalIoHandler, labelErrorPort);
                LabelErrorState.OnInputOn += labelErrorState_OnInputOn;
                LabelErrorState.OnInputOff += labelErrorState_OnInputOff;
            }
            IoPort labelEmptyPort = portMap.GetPort("Vision2LabelEmpty");
            if (labelEmptyPort != null)
            {
                LabelEmptyState = new InputStateHandler("Vision2LabelEmpty", digitalIoHandler, labelEmptyPort);
                LabelEmptyState.OnInputOn += labelEmptyState_OnInputOn;
                LabelEmptyState.OnInputOff += labelEmptyState_OnInputOff;
            }

            // 모니터링 시스템 시작을 Base에서 하기 때문에 설정을 모두 한 후에 기본 이니셜라이즈를 실행시킨다.
            base.Initialize();
        }

        private void plcStartState_OnInputOn(InputStateHandler eventSource)
        {
            IsPlcStart = true;
            LogHelper.Info(LoggerType.Comm, "[PLC I/O] Start On");
            IOStartChanged?.Invoke(IsPlcStart);
        }

        private void plcStartState_OnInputOff(InputStateHandler eventSource)
        {
            IsPlcStart = false;
            LogHelper.Info(LoggerType.Comm, "[PLC I/O] Start Off");
            IOStartChanged?.Invoke(IsPlcStart);
        }

        private void plcHoldState_OnInputOn(InputStateHandler eventSource)
        {
            IsPlcHold = true;
            LogHelper.Info(LoggerType.Comm, "[PLC I/O] Hold On");
            IOHoldChanged?.Invoke(IsPlcHold);
        }

        private void plcHoldState_OnInputOff(InputStateHandler eventSource)
        {
            IsPlcHold = false;
            LogHelper.Info(LoggerType.Comm, "[PLC I/O] Hold Off");
            IOHoldChanged?.Invoke(IsPlcHold);
        }

        private void DefectOccruedState_OnInputOn(InputStateHandler eventSource)
        {
            IsDefectOccrued = true;
            LogHelper.Info(LoggerType.Comm, "[IM I/O] Defect Occrued On");
            DefectOccrued?.Invoke(IsDefectOccrued);
        }

        private void DefectOccruedState_OnInputOff(InputStateHandler eventSource)
        {
            IsDefectOccrued = false;
            LogHelper.Info(LoggerType.Comm, "[IM I/O] Defect Occrued Off");
            DefectOccrued?.Invoke(IsDefectOccrued);
        }

        private void DefectResetState_OnInputOn(InputStateHandler eventSource)
        {
            IsDefectReset = true;
            LogHelper.Info(LoggerType.Comm, "[IM I/O] Defect Reset On");
            DefectReset?.Invoke(IsDefectReset);
        }

        private void DefectResetState_OnInputOff(InputStateHandler eventSource)
        {
            IsDefectReset = false;
            LogHelper.Info(LoggerType.Comm, "[IM I/O] Defect Reset Off");
            DefectReset?.Invoke(IsDefectReset);
        }

        private void labelRunState_OnInputOn(InputStateHandler eventSource)
        {
            IsLabelRun = true;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Run On");
            LabelRunChanged?.Invoke(IsLabelRun);
        }

        private void labelRunState_OnInputOff(InputStateHandler eventSource)
        {
            IsLabelRun = false;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Run Off");
            LabelRunChanged?.Invoke(IsLabelRun);
        }

        private void labelErrorState_OnInputOn(InputStateHandler eventSource)
        {
            IsLabelError = true;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Error On");
            LabelErrorChanged?.Invoke(IsLabelError);

            // 라벨러가 에러일 때 알람 울리도록 구현
            ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, ErrorLevel.Warning, ErrorSection.DigitalIo.ToString(), "Label Error", "[Label I/O] Error On");
        }

        private void labelErrorState_OnInputOff(InputStateHandler eventSource)
        {
            IsLabelError = false;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Error Off");
            LabelErrorChanged?.Invoke(IsLabelError);
        }

        private void labelEmptyState_OnInputOn(InputStateHandler eventSource)
        {
            IsLabelEmpty = true;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Empty On");
            LabelEmptyChanged?.Invoke(IsLabelEmpty);

            // 라벨지가 비어있을 때 알람 울리도록 구현
            ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, ErrorLevel.Warning, ErrorSection.DigitalIo.ToString(), "Label Empty", "[Label I/O] Empty On");
        }

        private void labelEmptyState_OnInputOff(InputStateHandler eventSource)
        {
            IsLabelEmpty = false;
            LogHelper.Info(LoggerType.Comm, "[Label I/O] Empty Off");
            LabelEmptyChanged?.Invoke(IsLabelEmpty);
        }

        public override bool ProcessInputChanged(DioValue inputValue)
        {
            bool stateChaned = false;

            stateChaned = base.ProcessInputChanged(inputValue);

            if (PlcStartState != null)
            {
                stateChaned |= PlcStartState.CheckState(inputValue);
            }

            //if (plcHoldState != null)
            //    stateChaned |= plcHoldState.CheckState(inputValue);

            //if (plcLotChangeState != null)
            //    stateChaned |= plcLotChangeState.CheckState(inputValue);

            if (DefectOccruedState != null)
            {
                stateChaned |= DefectOccruedState.CheckState(inputValue);
            }

            if (DefectResetState != null)
            {
                stateChaned |= DefectResetState.CheckState(inputValue);
            }

            if (LabelRunState != null)
            {
                stateChaned |= LabelRunState.CheckState(inputValue);
            }

            if (LabelErrorState != null)
            {
                stateChaned |= LabelErrorState.CheckState(inputValue);
            }

            if (LabelEmptyState != null)
            {
                stateChaned |= LabelEmptyState.CheckState(inputValue);
            }

            return stateChaned;
        }
        #endregion
    }
}
