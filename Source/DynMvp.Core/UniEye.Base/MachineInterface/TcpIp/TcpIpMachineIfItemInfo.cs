using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface.TcpIp
{
    public class TcpIpMachineIfItemInfo : MachineIfItemInfo
    {
        #region 생성자
        public TcpIpMachineIfItemInfo() { }

        public TcpIpMachineIfItemInfo(Enum command) : base(command, false, 500) { }

        public TcpIpMachineIfItemInfo(Enum command, bool use, int waitResponceMs) : base(command, use, waitResponceMs) { }
        #endregion


        #region 메서드
        public override MachineIfItemInfo Clone()
        {
            return new TcpIpMachineIfItemInfo(Command, Use, WaitResponceMs);
        }

        public override void Copyfrom(MachineIfItemInfo machineIfItemInfo)
        {
            base.Copyfrom(machineIfItemInfo);
        }

        public override bool IsValid => true;
        #endregion
    }
}
