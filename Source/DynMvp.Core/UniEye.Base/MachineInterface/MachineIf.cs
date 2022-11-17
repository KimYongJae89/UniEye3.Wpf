using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface.Melsec;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface
{
    public enum EMachineIfType
    {
        None, TcpIp, Melsec, AllenBreadley
    }

    /// <summary>
    /// 장비 인터페이스를 위한 최상위 클래스
    /// </summary>
    public abstract partial class MachineIf
    {
        #region 생성자
        public MachineIf(MachineIfSetting machineIfSetting, IProtocol protocol)
        {
            MachineIfSetting = machineIfSetting;
            Protocol = protocol;
        }
        #endregion


        #region 속성
        public MachineIfSetting MachineIfSetting { get; private set; }

        public MachineIfItemInfoResponce ItemInfoResponce { get; set; } = null;

        public IProtocol Protocol { get; set; }

        private List<CommandHandler> CommandHandlerList { get; set; } = new List<CommandHandler>();
        #endregion


        #region 메서드
        public virtual void Initialize()
        {
            SystemState.Instance().SetupState(1);
        }

        public virtual object HostInfo => null;

        public abstract void Start();

        public abstract void Stop();

        public abstract bool IsStarted();

        // Command를 사용한 Packet으로 만들어주는 함수
        public virtual string MakePacket(MachineIfItemInfo machineIfProtocol, params string[] args)
        {
            var sb = new StringBuilder();
            sb.Append(machineIfProtocol.Command.ToString());
            if (args != null)
            {
                foreach (string arg in args)
                {
                    sb.AppendFormat(",{0}", arg);
                }
            }
            string packet = sb.ToString();
            return packet;
        }

        public virtual MachineIfItemInfoWithArguments BreakPacket(MachineIfItemInfoList machineIfItemInfoList, string packet)
        {
            string[] token = packet.Split(',');

            Enum e = machineIfItemInfoList.GetEnum(token[0]);
            MachineIfItemInfo machineIfItemInfo = machineIfItemInfoList.GetItemInfo(e);
            token = token.Skip(1).ToArray();

            return new MachineIfItemInfoWithArguments(machineIfItemInfo, token);
        }

        public MachineIfItemInfoResponce SendCommand(MachineIfItemInfo itemInfo, params string[] args)
        {
            int timeoutMs = itemInfo.WaitResponceMs;
            lock (this)
            {
                ItemInfoResponce = new MachineIfItemInfoResponce(itemInfo);
                if (itemInfo.Use)
                {
                    bool ok = Send(itemInfo, args);
                    if (ok)
                    {
                        if (timeoutMs != 0 && ItemInfoResponce.WaitResponce(timeoutMs) == false)
                        {
                            LogHelper.Error(LoggerType.Network, string.Format("MachineIf::SendCommand - WaitResponce Timeout. {0}", itemInfo.Command == null ? "" : itemInfo.Command.ToString()));
                        }
                    }
                }
                return ItemInfoResponce;
            }
        }

        public MachineIfItemInfoResponce SendCommand(MachineIfItemInfoList machineIfItemInfoList, Enum command, params string[] args)
        {
            LogHelper.Debug(LoggerType.Network, string.Format("MachineIf::SendCommand - Command: {0}, Args: {1}", command, args == null ? "null" : string.Join(";", args)));

            MachineIfItemInfo machineIfItemInfo = machineIfItemInfoList.GetItemInfo(command);

            return SendCommand(machineIfItemInfo, args);
        }

        public abstract bool Send(MachineIfItemInfo itemInfo, params string[] args);

        public virtual void OnConnect(bool bConnect, object sender)
        {
            foreach (CommandHandler commandHandler in CommandHandlerList)
            {
                commandHandler.OnConnect(bConnect, sender);
            }
        }

        public void AddCommandHandler(CommandHandler commandHandler)
        {
            commandHandler.MachineIf = this;
            CommandHandlerList.Add(commandHandler);
        }

        public virtual CommandResult ExecuteCommand(string commandString)
        {
            var commandResult = new CommandResult();
            if (string.IsNullOrEmpty(commandString) == true)
            {
                return commandResult;
            }

            foreach (CommandHandler commandHandler in CommandHandlerList)
            {
                commandResult = commandHandler.ExecuteCommand(commandString);
                if (commandResult.Success == true)
                {
                    break;
                }
            }

            return commandResult;
        }
        #endregion
    }

    public abstract partial class MachineIf : IInspectEventListener
    {
        public virtual bool EnterWaitInspection() { return true; }

        public virtual void ExitWaitInspection() { }

        public virtual void ProductBeginInspect(ProductResult productResult) { }

        public virtual void ProductInspected(ProductResult productResult) { }

        public virtual void ProductEndInspect(ProductResult productResult) { }

        public virtual void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult) { }

        public virtual void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }

        public virtual void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }

        public virtual void TargetBeginInspect(Target target) { }

        public virtual void TargetEndInspect(Target target, ProbeResultList probeResultList) { }

        public virtual void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList) { }

        public virtual void ProbeBeginInspect() { }

        public virtual void ProbeEndInspect() { }
    }
}
