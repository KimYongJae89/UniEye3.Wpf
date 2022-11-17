using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Helper;

namespace UniEye.Base.MachineInterface
{
    public abstract class MachineIfItemInfo : Observable
    {
        #region 생성자
        public MachineIfItemInfo() { }

        public MachineIfItemInfo(Enum command, bool use, int waitResponceMs)
        {
            Command = command;
            Name = command != null ? command.ToString() : "";
            Use = use;
            WaitResponceMs = waitResponceMs;
        }
        #endregion


        #region 속성
        private string _name;
        public string Name { get => _name; set => Set(ref _name, value); }

        [Newtonsoft.Json.JsonIgnore]
        public Enum Command { get; set; }

        private bool _use;
        public bool Use { get => _use; set => Set(ref _use, value); }

        private int _waitResponceMs = 500;
        public int WaitResponceMs { get => _waitResponceMs; set => Set(ref _waitResponceMs, value); }
        #endregion


        #region 메서드
        public abstract MachineIfItemInfo Clone();

        public virtual void Copyfrom(MachineIfItemInfo machineIfItemInfo)
        {
            Command = machineIfItemInfo.Command;
            Name = Command.ToString();
            Use = machineIfItemInfo.Use;
            WaitResponceMs = machineIfItemInfo.WaitResponceMs;
        }

        public abstract bool IsValid { get; }
        #endregion
    }

    public class MachineIfItemInfoWithArguments
    {
        #region 생성자
        public MachineIfItemInfoWithArguments(MachineIfItemInfo machineIfItemInfo, string[] arguments)
        {
            MachineIfItemInfo = machineIfItemInfo;
            Arguments = arguments;
        }
        #endregion


        #region 속성
        public MachineIfItemInfo MachineIfItemInfo { get; }

        public string[] Arguments { get; }
        #endregion


        #region 메서드
        public override string ToString()
        {
            string args = null;
            if (Arguments != null)
            {
                args = string.Join(",", Arguments);
            }

            if (!string.IsNullOrEmpty(args))
            {
                return string.Format("{0},{1}", MachineIfItemInfo.ToString(), args);
            }
            else
            {
                return MachineIfItemInfo.ToString();
            }
        }
        #endregion
    }

    public enum MachineIfItem
    {
        VisionState,
        MachineState
    }
}
