using System;
using System.Collections.Generic;
using Unieye.WPF.Base.Helpers;
using UniEye.Base;
using UniScanC.Enums;

namespace UniScanC.Module
{
    public class ModuleState : Observable
    {
        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private EModuleStateType moduleStateType;
        public EModuleStateType ModuleStateType
        {
            get => moduleStateType;
            set => Set(ref moduleStateType, value);
        }

        private bool isConnected = false;
        public bool IsConnected
        {
            get => isConnected;
            set => Set(ref isConnected, value);
        }

        private OpMode opMode;
        public OpMode OpMode
        {
            get => opMode;
            set => Set(ref opMode, value);
        }

        private string alarmMessage;
        public string AlarmMessage
        {
            get => alarmMessage;
            set => Set(ref alarmMessage, value);
        }

        protected Dictionary<EUniScanCCommand, string> CommandDoneDictionary { get; set; } = new Dictionary<EUniScanCCommand, string>();

        public ModuleState()
        {
            foreach (EUniScanCCommand command in Enum.GetValues(typeof(EUniScanCCommand)))
            {
                CommandDoneDictionary.Add(command, null);
            }
        }

        public void SetCommandDone(EUniScanCCommand command, string result)
        {
            CommandDoneDictionary[command] = result;
        }

        public bool IsCommandDone(EUniScanCCommand command)
        {
            return !(CommandDoneDictionary[command] == null);
        }

        public bool IsCommandSuccess(EUniScanCCommand command)
        {
            if (!IsCommandDone(command))
            {
                return false;
            }

            return CommandDoneDictionary[command] == "";
        }

        public bool IsCommandFail(EUniScanCCommand command)
        {
            if (!IsCommandDone(command))
            {
                return false;
            }

            return !IsCommandSuccess(command);
        }

        public string GetCommandResult(EUniScanCCommand command)
        {
            return CommandDoneDictionary[command];
        }

        public void ResetCommandDone(EUniScanCCommand command)
        {
            CommandDoneDictionary[command] = null;
        }
    }
}
