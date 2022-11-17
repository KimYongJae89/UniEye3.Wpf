using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface.RabbitMQ;
using UniEye.Base.MachineInterface.TcpIp;
using UniScanC.Comm;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using WPF.UniScanIM.Manager;
using WPF.UniScanIM.Service;
using ModelManager = UniScanC.Models.ModelManager;

namespace WPF.UniScanIM.Override
{
    public class CommManager : UniScanC.Comm.CommManager
    {
        #region 생성자
        public CommManager()
        {
            var tcpIpInfo = new TcpIpInfo(SystemConfig.Instance.CMMQTTIpAddress, 4000);
            var IfSetting = new TcpIpMachineIfSetting(UniEye.Base.MachineInterface.EMachineIfType.TcpIp) { TcpIpInfo = tcpIpInfo };
            var rabbitMQMachineIf = new RabbitMQMachineIf(IfSetting, "IM", "IM");
            rabbitMQMachineIf.DataReceived = DataReceived;
            rabbitMQMachineIf.IsAsync = true;

            MachineIf = rabbitMQMachineIf;
            CommandParser = new UniScanCCommandParser();

            TopicInitialize();
        }
        #endregion


        #region 속성
        private object StateLockObj { get; set; } = new object();

        private object InspectLockObj { get; set; } = new object();

        private object ModelLockObj { get; set; } = new object();

        private IMInspectRunner InspectRunner => SystemManager.Instance().InspectRunner as IMInspectRunner;

        private TeachingManager TeachingManager => SystemManager.Instance().TeachingManager as TeachingManager;
        #endregion


        #region 메서드
        public void TopicInitialize()
        {
            var rabbitMQMachineIf = MachineIf as RabbitMQMachineIf;
            SystemConfig config = SystemConfig.Instance;

            rabbitMQMachineIf.ResetSendTopic();
            foreach (ModuleInfo moduleInfo in config.ModuleList)
            {
                rabbitMQMachineIf.AddSendTopic(moduleInfo.Topic);
            }

            rabbitMQMachineIf.ResetReceiveTopic();
            rabbitMQMachineIf.AddReceiveTopic(config.CMTopicName);
        }

        public override void Connect()
        {
            base.Connect();
            while (MachineIf?.IsStarted() == false)
            {
                if (MessageBox.Show("RabbitMQ 서버가 열려있지 않습니다. 서버를 확인 해주세요", "에러", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    base.Connect();
                }
                else
                {
                    break;
                }
            }
        }

        protected override void DataReceived(ReceivedPacket receivedPacket)
        {
            CommandInfo commandInfo = CommandParser?.Parse(receivedPacket);
            if (commandInfo == null)
            {
                return;
            }

            switch (commandInfo.Command)
            {
                case EUniScanCCommand.GetState: GetState(commandInfo); break;
                case EUniScanCCommand.SetTime: SetTime(commandInfo); break;

                case EUniScanCCommand.OpenModel: OpenModel(commandInfo); break;
                case EUniScanCCommand.CloseModel: break;

                case EUniScanCCommand.EnterWait: EnterWait(commandInfo); break;
                case EUniScanCCommand.ExitWait: ExitWait(commandInfo); break;
                case EUniScanCCommand.SkipMode: SkipMode(commandInfo); break;

                case EUniScanCCommand.LightCalibrationStart: LightCalibrationStart(commandInfo); break;
                case EUniScanCCommand.LightCalibrationTopGrab: LightCalibrationGrab(commandInfo); break;
                case EUniScanCCommand.LightCalibrationBottomGrab: LightCalibrationGrab(commandInfo); break;
                case EUniScanCCommand.LightCalibrationFinish: LightCalibrationFinish(commandInfo); break;

                case EUniScanCCommand.TeachGrab: TeachGrab(commandInfo); break;
                case EUniScanCCommand.TeachInspect: TeachInspect(commandInfo); break;

                case EUniScanCCommand.Alarm: break;
                default: break;
            }
        }

        private void GetState(CommandInfo commandInfo)
        {
            lock (StateLockObj)
            {
                SendMessage(EUniScanCCommand.GetState, SystemState.Instance().OpState.ToString(), SystemConfig.Instance.IMIpAddress);
            }
        }

        private void SetTime(CommandInfo commandInfo)
        {
            var time = new SystemTimeWrapper.SYSTEMTIME();
            time.year = Convert.ToInt16(commandInfo.Parameters[0]);
            time.month = Convert.ToInt16(commandInfo.Parameters[1]);
            time.dayOfWeek = Convert.ToInt16(commandInfo.Parameters[2]);
            time.day = Convert.ToInt16(commandInfo.Parameters[3]);
            time.hour = Convert.ToInt16(commandInfo.Parameters[4]);
            time.minute = Convert.ToInt16(commandInfo.Parameters[5]);
            time.second = Convert.ToInt16(commandInfo.Parameters[6]);
            time.milliseconds = Convert.ToInt16(commandInfo.Parameters[7]);
            SystemTimeWrapper.SetLocalTime(ref time);
            SendMessage(EUniScanCCommand.SetTime);
        }

        private void OpenModel(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(InspectLockObj))
            {
                try
                {
                    string modelName = commandInfo.Parameters[0];
                    string modelPath = $@"\\{SystemConfig.Instance.CMMQTTIpAddress}\{commandInfo.Parameters[1]}";

                    var modelManager = ModelManager.Instance();
                    modelManager.ModelPath = modelPath;
                    modelManager.Refresh();
                    modelManager.OpenModel(modelName, null);

                    var model = modelManager.CurrentModel as UniScanC.Models.Model;
                    InspectRunner.VisionModels = model.VisionModels;

                    SendMessage(EUniScanCCommand.OpenModel, "");
                    LogHelper.Debug(LoggerType.Comm, "CommManager::OpenModel - Complete");
                }
                catch (Exception ex)
                {
                    SendMessage(EUniScanCCommand.OpenModel, ex.Message);
                    LogHelper.Debug(LoggerType.Error, "CommManager::OpenModel - Fail");
                    LogHelper.Debug(LoggerType.Error, ex.StackTrace);
                }
                finally
                {
                    Monitor.Exit(InspectLockObj);
                }
            }
        }

        private void SkipMode(CommandInfo commandInfo)
        {
            try
            {
                InspectRunner.IsSkipMode = bool.Parse(commandInfo.Parameters[0]);
            }
            catch (Exception ex)
            {

            }
            SendMessage(EUniScanCCommand.SkipMode);
        }

        private void EnterWait(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(InspectLockObj))
            {
                try
                {
                    string dbName = commandInfo.Parameters[0];
                    string resultImagePath = $@"\\{SystemConfig.Instance.CMDBIpAddress}\{commandInfo.Parameters[1]}"; //resultPath에 lotName 까지만 이어진 폴더 경로
                    string lotName = commandInfo.Parameters[2];
                    string lineSpeed = commandInfo.Parameters[3];

                    InspectRunner.Initialize(dbName, resultImagePath, lotName, Convert.ToDouble(lineSpeed));
                    InspectRunner.EnterWaitInspection();

                    SendMessage(EUniScanCCommand.EnterWait);
                    LogHelper.Debug(LoggerType.Comm, "CommManager::EnterWait - Complete");
                }
                catch (Exception ex)
                {
                    SendMessage(EUniScanCCommand.EnterWait, ex.Message);
                    LogHelper.Debug(LoggerType.Error, "CommManager::EnterWait - Fail");
                    LogHelper.Debug(LoggerType.Error, ex.StackTrace);
                }
                finally
                {
                    Monitor.Exit(InspectLockObj);
                }
            }
        }

        private void ExitWait(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(InspectLockObj))
            {
                try
                {
                    InspectRunner.ExitWaitInspection();
                    LogHelper.Debug(LoggerType.Comm, "CommManager::ExitWait - Complete");
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Error, "CommManager::ExitWait - Fail");
                    LogHelper.Debug(LoggerType.Error, ex.StackTrace);
                }
                finally
                {
                    Monitor.Exit(InspectLockObj);
                    SendMessage(EUniScanCCommand.ExitWait);
                }
            }
        }

        private void LightCalibrationStart(CommandInfo commandInfo)
        {
            try
            {
                IMLightCalibrationService.LightCalibrationStart(Convert.ToDouble(commandInfo.Parameters[0]));
                LogHelper.Debug(LoggerType.Comm, "CommManager::LightCalibrationStart - LIGHT_CALIBRATION_START");
            }
            catch (Exception ex)
            {

            }
            SendMessage(EUniScanCCommand.LightCalibrationStart);
        }

        private void LightCalibrationGrab(CommandInfo commandInfo)
        {
            var param = new List<string>();
            try
            {
                IMLightCalibrationService.LightCalibrationGrab(int.Parse(commandInfo.Parameters[0]));
                Dictionary<ModuleInfo, IMLightCalibrationService.Buffer> bufferDic = IMLightCalibrationService.BufferDic;
                foreach (KeyValuePair<ModuleInfo, IMLightCalibrationService.Buffer> bufferPair in bufferDic)
                {
                    param.Add($"{bufferPair.Key.Topic};{bufferPair.Value.ImageAverageGreyValueList.LastOrDefault()}");
                }
                LogHelper.Debug(LoggerType.Comm, $"CommManager::LightCalibrationGrab - LIGHT_CALIBRATION_GRAB-{commandInfo.Parameters[0]}");
            }
            catch (Exception ex)
            {

            }
            SendMessage(commandInfo.Command, param.ToArray());
        }

        private void LightCalibrationFinish(CommandInfo commandInfo)
        {
            try
            {
                IMLightCalibrationService.LightCalibrationFinish();
                LogHelper.Debug(LoggerType.Comm, "CommManager::LightCalibrationFinish - LIGHT_CALIBRATION_FINISH");
            }
            catch (Exception ex)
            {

            }
            SendMessage(EUniScanCCommand.LightCalibrationFinish);
        }

        private void TeachGrab(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(InspectLockObj))
            {
                try
                {
                    // 현재 그랩 중인지 확인
                    bool onGrab = Convert.ToBoolean(commandInfo.Parameters[0]);
                    // 그랩 중이면 종료, 안하고 있으면 시작
                    TeachingManager.Grab(!onGrab);

                    SendMessage(EUniScanCCommand.TeachGrab);
                    LogHelper.Debug(LoggerType.Comm, $"CommManager::TeachGrab - Complete");
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Error, "CommManager::TeachGrab - Fail");
                    LogHelper.Debug(LoggerType.Error, ex.StackTrace);
                }
                finally
                {
                    Monitor.Exit(InspectLockObj);
                }
            }
        }

        private void TeachInspect(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(InspectLockObj))
            {
                try
                {
                    // 이미지 검사 진행
                    TeachingManager.Inspect();

                    SendMessage(EUniScanCCommand.TeachInspect);
                    LogHelper.Debug(LoggerType.Comm, $"CommManager::TeachInspect - Complete");
                }
                catch (Exception ex)
                {
                    SendMessage(EUniScanCCommand.TeachInspect, ex.Message);
                    LogHelper.Debug(LoggerType.Error, "CommManager::TeachInspect - Fail");
                    LogHelper.Debug(LoggerType.Error, ex.StackTrace);
                }
                finally
                {
                    Monitor.Exit(InspectLockObj);
                }
            }
        }
        #endregion
    }
}
