using DynMvp.Devices.Comm;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface.Melsec
{
    public enum EMelsecMachineIfType
    {
        Binary, Ascii
    }

    public class MelsecMachineIfSetting : TcpIpMachineIfSetting
    {
        #region 생성자
        public MelsecMachineIfSetting() : base(EMachineIfType.Melsec) { }

        public MelsecMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        #endregion


        #region 속성
        public MelsecInfo MelsecInfo { get; set; } = new MelsecInfo();

        public EMelsecMachineIfType MelsecMachineIfType { get; set; } = EMelsecMachineIfType.Binary;
        #endregion


        #region 메서드
        public override void CopyFrom(MachineIfSetting src)
        {
            base.CopyFrom(src);
            if (src is MelsecMachineIfSetting melsecMachineIfSetting)
            {
                MelsecInfo = melsecMachineIfSetting.MelsecInfo.Clone();
                MelsecMachineIfType = melsecMachineIfSetting.MelsecMachineIfType;
            }
        }

        public override MachineIfSetting Clone()
        {
            return new MelsecMachineIfSetting(this);
        }
        #endregion
    }
}
