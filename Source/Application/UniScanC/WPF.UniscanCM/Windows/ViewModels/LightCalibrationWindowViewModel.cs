using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Comm;
using UniScanC.Enums;
using UniScanC.Module;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC;
using WPF.UniScanCM.Service;
using WPF.UniScanCM.Windows.Views;
using CommManager = WPF.UniScanCM.Override.CommManager;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class LightCalibrationWindowViewModel : Observable
    {
        #region 생성자
        public LightCalibrationWindowViewModel()
        {
            LoadedCommand = new RelayCommand<LightCalibrationWindowView>(LoadedCommandAction);
            CloseCommand = new RelayCommand(CloseCommandAction);

            CommManager.LightCalibrationStartDelegate += LightCalibrationStartAction;
            CommManager.LightCalibrationTopGrabDelegate += LightCalibrationTopGrabAction;
            CommManager.LightCalibrationBottomGrabDelegate += LightCalibrationBottomGrabAction;
            CommManager.LightCalibrationFinishDelegate += LightCalibrationFinishAction;
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand LoadedCommand { get; }

        public System.Windows.Input.ICommand CloseCommand { get; }

        private EProcessingState startDoneInfo = new EProcessingState();
        public EProcessingState StartDoneInfo
        {
            get => startDoneInfo;
            set => Set(ref startDoneInfo, value);
        }

        private EProcessingState lightGrabDoneInfo = new EProcessingState();
        public EProcessingState LightGrabDoneInfo
        {
            get => lightGrabDoneInfo;
            set => Set(ref lightGrabDoneInfo, value);
        }

        private EProcessingState finishDoneInfo = new EProcessingState();
        public EProcessingState FinishDoneInfo
        {
            get => finishDoneInfo;
            set => Set(ref finishDoneInfo, value);
        }

        private string lightGrabText = TranslationHelper.Instance.Translate("Calibration");
        public string LightGrabText
        {
            get => lightGrabText;
            set => Set(ref lightGrabText, value);
        }

        public float TargetSpeed { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        private LightCalibrationWindowView ParentWindow { get; set; } = null;

        private bool OnCalibration { get; set; } = true;

        // Getter
        private CommManager CommManager => CommManager.Instance() as CommManager;

        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private PlcBase PLCMachineIf => DeviceManager.PLCMachineIf;
        #endregion


        #region 메서드
        private void LoadedCommandAction(LightCalibrationWindowView childWindow)
        {
            ParentWindow = childWindow;
            LightCalibrationStart();
        }

        private void CloseCommandAction()
        {
            CancellationTokenSource.Cancel();
            ClearDelegate();
            ParentWindow?.Close(false);
            Thread.Sleep(100);
        }

        private void ClearDelegate()
        {
            CommManager.LightCalibrationStartDelegate -= LightCalibrationStartAction;
            CommManager.LightCalibrationTopGrabDelegate -= LightCalibrationTopGrabAction;
            CommManager.LightCalibrationBottomGrabDelegate -= LightCalibrationBottomGrabAction;
            CommManager.LightCalibrationFinishDelegate -= LightCalibrationFinishAction;
        }

        private void LightCalibrationStartAction(CommandInfo commandInfo)
        {
            CMLightCalibrationService.LightCalibrationInitialize();
        }

        private void LightCalibrationTopGrabAction(CommandInfo commandInfo)
        {
            CMLightCalibrationService.AddImageAverageValue(commandInfo.Sender, commandInfo.Parameters);
        }

        private void LightCalibrationBottomGrabAction(CommandInfo commandInfo)
        {
            CMLightCalibrationService.AddImageAverageValue(commandInfo.Sender, commandInfo.Parameters);
        }

        private void LightCalibrationFinishAction(CommandInfo commandInfo)
        {
            
        }

        private async void LightCalibrationStart()
        {
            try
            {
                if (CancellationTokenSource == null)
                {
                    CancellationTokenSource = new CancellationTokenSource();
                }

                // 시작 커맨드 송신
                StartDoneInfo = await CalibrationExecuteProc(EUniScanCCommand.LightCalibrationStart, TargetSpeed.ToString());
                if (StartDoneInfo == EProcessingState.Fail)
                {
                    return;
                }

                DeviceManager.TopLightOff();
                DeviceManager.BottomLightOff();
                OnCalibration = true;

                int targetLight = CMLightCalibrationService.TargetLigthValue;
                int maxLight = CMLightCalibrationService.maxLightValue;

                // Bottom Grab
                while (OnCalibration)
                {
                    LightGrabText = $"{TranslationHelper.Instance.Translate("CALIBRATION_LIGHT")}\n({targetLight})";

                    if (CMLightCalibrationService.TargetLigthValue <= maxLight)
                    {
                        DeviceManager.BottomLightOn(targetLight);
                        DeviceManager.TopLightOn(0);
                    }
                    else
                    {
                        DeviceManager.BottomLightOn(maxLight);
                        DeviceManager.TopLightOn(targetLight - maxLight);
                    }

                    LightGrabDoneInfo = await CalibrationExecuteProc(EUniScanCCommand.LightCalibrationBottomGrab, targetLight.ToString());
                    if (LightGrabDoneInfo == EProcessingState.Fail)
                    {
                        LogHelper.Debug(LoggerType.Operation, $"LightCalibrationWindowViewModel::LightCalibrationStart - Fail");
                        return;
                    }

                    targetLight = CMLightCalibrationService.TargetLigthValue;
                    OnCalibration = CMLightCalibrationService.OnCalibration;
                }

                FinishDoneInfo = await CalibrationExecuteProc(EUniScanCCommand.LightCalibrationFinish);
                if (FinishDoneInfo == EProcessingState.Fail)
                {
                    return;
                }

                if (targetLight <= maxLight)
                {
                    DeviceManager.BottomLightOn(targetLight);
                    DeviceManager.TopLightOn(0);
                }
                else
                {
                    DeviceManager.BottomLightOn(maxLight);
                    DeviceManager.TopLightOn(targetLight - maxLight);
                }

                ClearDelegate();
                ParentWindow.Close(true);
            }
            catch (OperationCanceledException ocex)
            {
                ClearDelegate();
                ParentWindow.Close(false);
                return;
            }
            catch (Exception ex)
            {
                ClearDelegate();
                ParentWindow.Close(false);
                return;
            }
        }

        private async Task<EProcessingState> CalibrationExecuteProc(EUniScanCCommand command, params string[] args)
        {
            return await Task.Run(() =>
            {
                List<ModuleState> moduleStateList = ModuleManager.Instance.ModuleStateList.FindAll(x => x.ModuleStateType != EModuleStateType.Thickness);
                foreach (ModuleState moduleState in moduleStateList)
                {
                    moduleState.ResetCommandDone(command);
                }

                CommManager.SendMessage(command, args);

                bool isDone = false;
                while (!isDone)
                {
                    isDone = true;

                    foreach (ModuleState moduleState in moduleStateList)
                    {
                        if (!moduleState.IsCommandDone(command))
                        {
                            isDone = false;
                            break;
                        }
                    }
                    Thread.Sleep(10);
                }

                return EProcessingState.Success;
            }, CancellationTokenSource.Token);
        }
        #endregion
    }
}
