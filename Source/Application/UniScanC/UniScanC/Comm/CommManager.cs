using DynMvp.Devices.Comm;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.Melsec;
using UniEye.Base.MachineInterface.TcpIp;
using UniScanC.Enums;

namespace UniScanC.Comm
{
    public abstract class CommManager
    {
        private static CommManager _instance;

        public UniEye.Base.MachineInterface.MachineIf MachineIf { get; set; }
        public UniScanCCommandParser CommandParser { get; set; }

        public static CommManager Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            return null;
        }

        public static void SetInstance(CommManager instance)
        {
            _instance = instance;
        }

        protected virtual void DataReceived(ReceivedPacket receivedPacket) { }

        public virtual void Connect()
        {
            MachineIf?.Start();
        }

        public virtual void Disconnect()
        {
            MachineIf?.Stop();
        }

        public bool IsConnected()
        {
            if (MachineIf == null)
            {
                return false;
            }

            return MachineIf.IsStarted();
        }

        public bool SendMessage(EUniScanCCommand command)
        {
            return SendMessage(command, null);
        }

        public bool SendMessage(EUniScanCCommand command, params string[] args)
        {
            if (MachineIf == null)
            {
                return false;
            }

            var machineIfItemInfo = new TcpIpMachineIfItemInfo(command, true, 500);
            return MachineIf.Send(machineIfItemInfo, args);
        }
    }
}
