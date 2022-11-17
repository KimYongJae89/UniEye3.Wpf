using DynMvp.Devices.Comm;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface.AllenBreadley
{
    public class AllenBreadleyMachineIfSetting : TcpIpMachineIfSetting
    {
        #region 생성자
        public AllenBreadleyMachineIfSetting() : base(EMachineIfType.AllenBreadley) { }

        public AllenBreadleyMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        #endregion


        #region 속성
        public string CpuType { get; set; } = "LGX"; // CPU TYPE

        public string PlcPath { get; set; } = "1,0"; // PLC Path
        #endregion


        #region 메서드
        public override MachineIfSetting Clone()
        {
            var newSettings = new AllenBreadleyMachineIfSetting(this);
            return newSettings;
        }
        #endregion
    }
}
