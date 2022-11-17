using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;

namespace UniEye.Base.MachineInterface
{
    public abstract class MachineIfSetting
    {
        #region 생성자
        protected MachineIfSetting(MachineIfSetting machineIfSetting)
        {
            CopyFrom(machineIfSetting);
        }

        protected MachineIfSetting(EMachineIfType machineIfType)
        {
            MachineIfType = machineIfType;
        }
        #endregion


        #region 속성
        public EMachineIfType MachineIfType { get; set; }

        public MachineIfItemInfoList MachineIfItemInfoList { get; set; }

        public string Name { get; set; } = "";

        public bool IsVirtualMode { get; set; } = false;
        #endregion


        #region 메서드
        public static MachineIfSetting Create(EMachineIfType machineIfType)
        {
            MachineIfSetting machineIfSetting = null;
            switch (machineIfType)
            {
                case EMachineIfType.None:
                    machineIfSetting = null;
                    break;
                case EMachineIfType.Melsec:
                    machineIfSetting = new MelsecMachineIfSetting();
                    break;
                case EMachineIfType.AllenBreadley:
                    machineIfSetting = new AllenBreadleyMachineIfSetting();
                    break;
            }

            if (machineIfSetting != null)
            {
                MachineIfItemInfoList list = MachineIfItemInfoList.List;
                list.Initialize(machineIfType);
                machineIfSetting.MachineIfItemInfoList = list;
            }

            return machineIfSetting;
        }

        public MachineIfItemInfo CreateItemInfo(Enum command)
        {
            switch (MachineIfType)
            {
                case EMachineIfType.Melsec: return new MelsecMachineIfItemInfo(command);
                case EMachineIfType.AllenBreadley: return new AllenBreadleyMachineIfItemInfo(command);
                default: return null;
            }
        }

        public abstract MachineIfSetting Clone();

        public virtual void CopyFrom(MachineIfSetting src)
        {
            IsVirtualMode = src.IsVirtualMode;
            MachineIfType = src.MachineIfType;
            MachineIfItemInfoList = src.MachineIfItemInfoList?.Clone();
        }
        #endregion
    }
}
