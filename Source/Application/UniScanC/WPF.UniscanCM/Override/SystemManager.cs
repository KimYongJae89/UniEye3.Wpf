using DynMvp.Base;
using DynMvp.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.Data;
using UniEye.Translation.Helpers;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.PLC;
using WPF.UniScanCM.Service;
using WPF.UniScanCM.Windows.ViewModels;
using WPF.UniScanCM.Windows.Views;
using ModelManager = UniScanC.Models.ModelManager;

namespace WPF.UniScanCM.Override
{
    public class SystemManager : Unieye.WPF.Base.Override.SystemManager
    {
        #region 생성자
        public SystemManager()
        {
            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;

            var config = SystemConfig.Instance as SystemConfig;
            DataExporter = new InspectDataExporter();
            DataExporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);

            DataImporter = new InspectDataImporter();
            DataImporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);

            // CM이 지속적으로 PLC로 통신을 보내고 있는지 확인한다.
            if (SystemConfig.Instance.UseHeartBeat)
            {
                AliveService.StartAliveCheckTimer(SystemConfig.Instance.HeartBeatSignalDuration, true);
            }
        }
        #endregion


        #region 속성
        private UniScanC.Models.Model Model { get; set; }

        private InspectDataExporter DataExporter { get; set; }

        private InspectDataImporter DataImporter { get; set; }

        private CancellationTokenSource CalibrationCancelToken { get; set; }

        // 중복 시작 정지를 막기 위한 수단
        private bool IsStarting { get; set; } = false;

        private bool IsStopping { get; set; } = false;

        private DeviceManager DeviceManager => Override.DeviceManager.Instance() as DeviceManager;

        private PlcBase PLCMachineIf => DeviceManager.PLCMachineIf;

        private ModelManager ModelManager => ModelManager.Instance() as ModelManager;

        private SystemConfig SystemConfig => SystemConfig.Instance;
        #endregion


        #region 메서드
        public static new SystemManager Instance()
        {
            return instance as SystemManager;
        }

        private void ModelOpened(ModelBase model)
        {
            Model = model as UniScanC.Models.Model;
        }

        private void ModelClosed()
        {
            Model = null;
        }

        public async Task<bool> StartInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "SystemManager::StartInspection - Start");
            bool responce = true;
            if (!IsStarting)
            {
                IsStarting = true;

                if (ModelManager.Instance().CurrentModel != null)
                {
                    // 장비 구동 시작하면 처음에 NG 신호 0으로 바꿔주기
                    DeviceManager.SendDefectSignal(false);
                    // 불량 알람 서비스 초기화 하기
                    if (SystemConfig.UseDefectAlarm)
                    {
                        DefectAlarmService.SetParam(SystemConfig.AlarmDetectLengthRangeM, SystemConfig.AlarmLineDefectHeightMm, SystemConfig.AlarmDefectCount);
                    }
                    // 불량 카운트 서비스 초기화 하기
                    if (SystemConfig.UseDefectCount)
                    {
                        DefectCountService.ClearCount();
                        DefectCountService.TargetLengthM = SystemConfig.DefectCountTartgetLengthM;
                    }
                    // 현재 상태 바꿔주기
                    SystemState.Instance().SetInspectState(InspectState.Run);

                    // 입력된 속도 또는 PLC 에서 가져온 속도 확인
                    float targetSpeed = SystemConfig.Instance.TargetSpeed;
                    if (PLCMachineIf != null)
                    {
                        targetSpeed = PLCMachineIf.PreviousMachineIfData.GET_TARGET_SPEED / 10.0f;
                    }
                    LogHelper.Debug(LoggerType.Inspection, $"SystemManager::StartInspection - LineSpeed : {targetSpeed}");
                    if (targetSpeed > 0)
                    {
                        // 조명 값 설정
                        if (Model.VisionModels.FirstOrDefault().UseAutoLight == true)
                        {
                            var calibrationView = new LightCalibrationWindowView();
                            var calibrationViewModel = new LightCalibrationWindowViewModel();
                            calibrationViewModel.TargetSpeed = targetSpeed;
                            calibrationViewModel.CancellationTokenSource = CalibrationCancelToken = new CancellationTokenSource();
                            calibrationView.DataContext = calibrationViewModel;
                            responce = await MessageWindowHelper.ShowChildWindow<bool>(calibrationView);
                            CalibrationCancelToken.Dispose();
                            CalibrationCancelToken = null;
                            if (!responce)
                            {
                                ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                                    TranslationHelper.Instance.Translate("CALIBRATION_LIGHT"), TranslationHelper.Instance.Translate("LIGHT_CALIBRATION_FAIL"));
                            }
                        }
                        // TODO:[송현석] 우선은 VisionModel별로 라이트 값을 가져가고 후에 라이트를 모델 하나에 종속시키는 방법으로 변경 예정
                        else
                        {
                            DeviceManager.TopLightOff();
                            DeviceManager.BottomLightOff();

                            VisionModel firstVisionModel = Model.VisionModels.FirstOrDefault();
                            if (firstVisionModel.UseTDILight)
                            {
                                // TDI 카메라의 경우 설정한 조명 값을 사용하여 비례하는 조명 값으로 설정 해준다.
                                if (CMLightCalibrationService.CalTDILightValue(firstVisionModel, targetSpeed, out int TopLightValue, out int BottomLightValue))
                                {
                                    DeviceManager.TopLightOn(TopLightValue);
                                    DeviceManager.BottomLightOn(BottomLightValue);
                                }
                                else
                                {
                                    ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                                        TranslationHelper.Instance.Translate("LIGHT_SETTING_WARNING"),
                                        TranslationHelper.Instance.Translate("LIGHT_SETTING_WARNING") + "\n" +
                                        TranslationHelper.Instance.Translate("MIN_SPEED") + $" : {firstVisionModel.MinSpeed}" + "\n" +
                                        TranslationHelper.Instance.Translate("MAX_SPEED") + $" : {firstVisionModel.MaxSpeed}");
                                    responce = false;
                                }
                            }
                            else
                            {
                                DeviceManager.TopLightOn(firstVisionModel.TopLightValue);
                                DeviceManager.BottomLightOn(firstVisionModel.BottomLightValue);
                            }
                        }

                        if (responce)
                        {
                            string lotNo = SystemConfig.Instance.LastLotNo;
                            if (PLCMachineIf != null && PLCMachineIf.PreviousMachineIfData.GET_LOT != "")
                            {
                                lotNo = SystemConfig.Instance.LastLotNo = PLCMachineIf.PreviousMachineIfData.GET_LOT;
                            }
                            else
                            {
                                lotNo = SystemConfig.Instance.LastLotNo = CheckLotNoService.CheckLotNo(DataImporter, SystemConfig.Instance.LastLotNo);
                            }
                            int frameNo = 0;
                            //resultPath에 lotNo 까지만 이어진 폴더 경로
                            var resultPathInfo = new DirectoryInfo(SystemConfig.Instance.ResultPath);
                            string cmResultImagePath = Path.Combine(resultPathInfo.FullName, lotNo);
                            string imResultImagePath = $@"{resultPathInfo.Name}\{lotNo}";
                            string copiedModelPath = Path.Combine(cmResultImagePath, "Model");

                            //이미지 저장 경로 생성
                            if (!Directory.Exists(cmResultImagePath))
                            {
                                Directory.CreateDirectory(cmResultImagePath);
                            }
                            //현재 모델 정보를 저장 폴더에 복사 - 리포트에서 검사할 당시의 모델 정보 필요
                            ModelManager.SaveModelDescription(copiedModelPath);
                            ModelManager.SaveModel(copiedModelPath);
                            //Lot 정보 DB에 저장
                            DataExporter.ExportLotData(lotNo);

                            var commMgr = CommManager.Instance() as CommManager;
                            //IM에 현재 선택한 모델 보냄
                            var modelPath = new DirectoryInfo(ModelManager.Instance().ModelPath);
                            responce = await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, ModelManager.CurrentModel.Name, modelPath.Name);
                            if (responce)
                            {
                                //IM에 시작 신호 보냄
                                responce = await commMgr.ExecuteCommand(EUniScanCCommand.EnterWait, DataExporter.DbName, imResultImagePath, lotNo, targetSpeed.ToString());
                                if (responce)
                                {
                                    InspectEventListener.Instance.StartResultReader(lotNo, frameNo);
                                    InspectRunner.InspectEventHandler?.EnterWaitInspection();
                                    SystemState.Instance().SetInspectState(InspectState.Run);
                                    EnableEncoder(true);
                                }
                                else
                                {
                                    SystemState.Instance().SetIdle();
                                }
                            }
                            else
                            {
                                SystemState.Instance().SetIdle();
                            }
                        }
                    }
                    else
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                                TranslationHelper.Instance.Translate("LINE_SPEED_WARNING"), TranslationHelper.Instance.Translate("LINE_SPEED") + $" : {targetSpeed}");
                        responce = false;
                    }
                }
                else
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                        TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"), TranslationHelper.Instance.Translate("MODEL_NOT_OPEN_MESSAGE"));
                    responce = false;
                }

                IsStarting = false;
            }
            LogHelper.Debug(LoggerType.Inspection, "SystemManager::StartInspection - End");
            return responce;
        }

        public async Task StopInspection()
        {
            if (SystemState.Instance().OpState == OpState.Idle)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SystemManager::StopInspection - Start");

            CalibrationCancelToken?.Cancel(true);

            if (!IsStopping)
            {
                IsStopping = true;

                EnableEncoder(false);

                var commMgr = CommManager.Instance() as CommManager;
                if (await commMgr.ExecuteCommand(EUniScanCCommand.ExitWait))
                {
                    InspectRunner.InspectEventHandler?.ExitWaitInspection();
                }

                if (DeviceManager.Instance().LightCtrlHandler != null)
                {
                    DeviceManager.Instance().LightCtrlHandler.TurnOff();
                }

                await InspectEventListener.Instance.StopResultReader();

                DeviceManager.SendDefectSignal(false);

                if (SystemConfig.Instance.UseDefectCount)
                {
                    if (PLCMachineIf != null && PLCMachineIf.IsConnected())
                    {
                        PLCMachineIf.UpdateTotalDefectCountResult();
                    }
                }

                IsStopping = false;
            }

            LogHelper.Debug(LoggerType.Inspection, "SystemManager::StopInspection - End");
        }

        public void EnableEncoder(bool enable)
        {
            SystemConfig config = SystemConfig.Instance;
            if (config.UseEncoder)
            {
                if (!config.UseEncoderSpeed)
                {
                    var deviceManager = DeviceManager.Instance() as DeviceManager;
                    if (deviceManager.IsConnectSerialEncoder())
                    {
                        deviceManager.DisconnectSerialEncoder();
                    }

                    deviceManager.InitEncoder();
                    if (deviceManager.ConnectSerialEncoder(config.EncoderPort))
                    {
                        deviceManager.EnableEncoder(enable);
                        deviceManager.DisconnectSerialEncoder();
                    }
                }
            }
            else
            {
                return;
            }
        }

        public override void Release()
        {
            base.Release();

            CommManager.Instance().Disconnect();
        }
        #endregion
    }
}
