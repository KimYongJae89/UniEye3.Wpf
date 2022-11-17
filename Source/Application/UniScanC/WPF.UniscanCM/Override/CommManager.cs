using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unieye.WPF.Base.Helpers;
using UniEye.Base;
using UniEye.Base.MachineInterface.Melsec;
using UniEye.Base.MachineInterface.RabbitMQ;
using UniEye.Base.MachineInterface.TcpIp;
using UniEye.Translation.Helpers;
using UniScanC.Comm;
using UniScanC.Enums;
using UniScanC.Module;
using WPF.UniScanCM.Service;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Override
{
    public delegate void CommManagerDataReceivedDelegate(CommandInfo commandInfo);

    public class CommManager : UniScanC.Comm.CommManager
    {
        #region 생성자
        public CommManager()
        {
            var tcpIpInfo = new TcpIpInfo(SystemConfig.Instance.BrokerIpAddress, 4000);
            var IfSetting = new TcpIpMachineIfSetting(UniEye.Base.MachineInterface.EMachineIfType.TcpIp) { TcpIpInfo = tcpIpInfo };
            var rabbitMQMachineIf = new RabbitMQMachineIf(IfSetting);
            rabbitMQMachineIf.DataReceived = DataReceived;
            rabbitMQMachineIf.IsAsync = true;

            MachineIf = rabbitMQMachineIf;
            CommandParser = new UniScanCCommandParser();

            TopicInitialize();
        }
        #endregion


        #region 속성
        public CommManagerDataReceivedDelegate LightCalibrationStartDelegate { get; set; }

        public CommManagerDataReceivedDelegate LightCalibrationTopGrabDelegate { get; set; }

        public CommManagerDataReceivedDelegate LightCalibrationBottomGrabDelegate { get; set; }

        public CommManagerDataReceivedDelegate LightCalibrationFinishDelegate { get; set; }
        #endregion


        #region 메서드
        public void TopicInitialize()
        {
            var rabbitMQMachineIf = MachineIf as RabbitMQMachineIf;
            SystemConfig config = SystemConfig.Instance;

            ModuleManager.Instance.ModuleStateList.Clear();

            rabbitMQMachineIf.ResetSendTopic();
            rabbitMQMachineIf.AddSendTopic(config.CMMQTTTopic);

            rabbitMQMachineIf.ResetReceiveTopic();
            if (config.UseInspectModule)
            {
                // Vision
                foreach (InspectModuleInfo moduleInfo in config.ImModuleList)
                {
                    ModuleManager.Instance.AddInspectModuleState(moduleInfo);
                    rabbitMQMachineIf.AddReceiveTopic(moduleInfo.ModuleTopic);
                }
            }

            if (config.UseThicknessModule)
            {
                // Sensor
                ModuleManager.Instance.AddThicknessModuleState();
                rabbitMQMachineIf.AddReceiveTopic(SystemConfig.Instance.ThicknessModuleTopic);
            }

            if (config.UseGlossModule)
            {
                // Sensor
                ModuleManager.Instance.AddGlossModuleState();
                rabbitMQMachineIf.AddReceiveTopic(SystemConfig.Instance.GlossModuleTopic);
            }
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

        public async Task<bool> ExecuteCommand(EUniScanCCommand command)
        {
            return await MessageWindowHelper.ShowChildWindow<bool>(new UmxCommandReceivedWindowView(command));
        }

        public async Task<bool> ExecuteCommand(EUniScanCCommand command, params string[] args)
        {
            return await MessageWindowHelper.ShowChildWindow<bool>(new UmxCommandReceivedWindowView(command, args));
        }

        protected override void DataReceived(ReceivedPacket receivedPacket)
        {
            CommandInfo commandInfo = CommandParser?.Parse(receivedPacket);
            if (commandInfo == null)
            {
                return;
            }

            ModuleState moduleState = GetModuleStateFromSender(commandInfo.Sender);
            if (moduleState != null)
            {
                moduleState.IsConnected = true;

                string result = "";
                switch (commandInfo.Command)
                {
                    case EUniScanCCommand.GetState: result = GetState(commandInfo); break;

                    case EUniScanCCommand.Unknown:
                    case EUniScanCCommand.SetTime:
                    case EUniScanCCommand.OpenModel:
                    case EUniScanCCommand.CloseModel:

                    case EUniScanCCommand.EnterWait:
                    case EUniScanCCommand.ExitWait:
                    case EUniScanCCommand.SkipMode:

                    case EUniScanCCommand.TeachGrab:
                    case EUniScanCCommand.TeachInspect: result = GetResponce(commandInfo); break;

                    case EUniScanCCommand.Alarm: result = ReportAlarm(commandInfo); break;

                    case EUniScanCCommand.LightCalibrationStart: result = LightCalibrationStart(commandInfo); break;
                    case EUniScanCCommand.LightCalibrationTopGrab: result = LightCalibrationTopGrab(commandInfo); break;
                    case EUniScanCCommand.LightCalibrationBottomGrab: result = LightCalibrationBottomGrab(commandInfo); break;
                    case EUniScanCCommand.LightCalibrationFinish: result = LightCalibrationFinish(commandInfo); break;

                    default: throw new InvalidOperationException();
                }

                moduleState.SetCommandDone(commandInfo.Command, result);
            }
        }

        private string GetResponce(CommandInfo commandInfo)
        {
            if (commandInfo.Parameters.Count == 0)
            {
                return "";
            }

            return commandInfo.Parameters[0];
        }

        private string GetState(CommandInfo commandInfo)
        {
            ModuleState moduleState = GetModuleStateFromSender(commandInfo.Sender);
            if (commandInfo.Parameters.Count != 0)
            {
                moduleState.OpMode = (OpMode)Enum.Parse(typeof(OpMode), commandInfo.Parameters[0]);
            }

            return "";
        }

        private string ReportAlarm(CommandInfo commandInfo)
        {
            string message = "Unknown";
            if (commandInfo.Parameters.Count != 0)
            {
                ModuleState moduleState = GetModuleStateFromSender(commandInfo.Sender);
                message = commandInfo.Parameters[0];
                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.InvalidType, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(),
                    $"[{moduleState.Name}] " + TranslationHelper.Instance.Translate("ALARM_OCCURRED"), message);
            }
            return message;
        }

        private string LightCalibrationStart(CommandInfo commandInfo)
        {
            LightCalibrationStartDelegate?.Invoke(commandInfo);
            return "";
        }

        private string LightCalibrationTopGrab(CommandInfo commandInfo)
        {
            LightCalibrationTopGrabDelegate?.Invoke(commandInfo);
            return "";
        }

        private string LightCalibrationBottomGrab(CommandInfo commandInfo)
        {
            LightCalibrationBottomGrabDelegate?.Invoke(commandInfo);
            return "";
        }

        private string LightCalibrationFinish(CommandInfo commandInfo)
        {
            LightCalibrationFinishDelegate?.Invoke(commandInfo);
            return "";
        }

        private ModuleState GetModuleStateFromSender(string sender)
        {
            SystemConfig config = SystemConfig.Instance;
            ModuleState moduleState = null;

            if (config.UseInspectModule)
            {
                InspectModuleInfo moduleInfo = config.ImModuleList.Find(x => x.ModuleTopic == sender);
                if (moduleInfo != null)
                {
                    moduleState = ModuleManager.Instance.GetModuleState(EModuleStateType.Inspect, moduleInfo.ModuleNo);
                }
            }

            if (moduleState == null && config.UseThicknessModule)
            {
                if (config.ThicknessModuleTopic == sender)
                {
                    moduleState = ModuleManager.Instance.GetModuleState(EModuleStateType.Thickness);
                }
            }

            if (moduleState == null && config.UseGlossModule)
            {
                if (config.GlossModuleTopic == sender)
                {
                    moduleState = ModuleManager.Instance.GetModuleState(EModuleStateType.Gloss);
                }
            }

            return moduleState;
        }
        #endregion
    }
}
