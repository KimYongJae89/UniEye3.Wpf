using DynMvp.Devices.Comm;
using UniEye.Base.MachineInterface;

namespace UniEye.Base.MachineInterface.TcpIp
{
    public class TcpIpMachineIfSetting : MachineIfSetting
    {
        #region 생성자
        public TcpIpMachineIfSetting(EMachineIfType machineIfType) : base(machineIfType) { }

        public TcpIpMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        #endregion


        #region 속성
        public TcpIpInfo TcpIpInfo { get; set; } = new TcpIpInfo();
        #endregion


        #region 메서드
        public override void CopyFrom(MachineIfSetting src)
        {
            base.CopyFrom(src);
            if (src is TcpIpMachineIfSetting tcpipMachineIfSetting)
            {
                TcpIpInfo = tcpipMachineIfSetting.TcpIpInfo.Clone();
            }
        }

        public override MachineIfSetting Clone()
        {
            return new TcpIpMachineIfSetting(this);
        }
        #endregion
    }
}
