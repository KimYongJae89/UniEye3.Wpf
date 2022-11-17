using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;
using UniScanC.MachineIf;

namespace WPF.UniScanCM.MachineIf
{
    public enum MachineIfItemCoating
    {
        // 검사기 상태
        SET_VISION_COATING_INSP_READY,
        SET_VISION_COATING_INSP_RUNNING,
        SET_VISION_COATING_INSP_ERROR,

        SET_VISION_COATING_INSP_NG_PINHOLE,
        SET_VISION_COATING_INSP_NG_DUST,

        SET_VISION_COATING_INSP_CNT_ALL,
        SET_VISION_COATING_INSP_CNT_PINHOLE,
        SET_VISION_COATING_INSP_CNT_DUST,

        SET_VISION_COATING_INSP_TOTAL_CNT_ALL,
        SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE,
        SET_VISION_COATING_INSP_TOTAL_CNT_DUST,
    };

    public class MachineIfItemInfoListCM : MachineIfItemInfoListC
    {
        public MachineIfItemInfoListCM() : base(typeof(MachineIfItemCommon), typeof(MachineIfItemCoating)) { }

        public MachineIfItemInfoListCM(MachineIfItemInfoList MachineIfItemCoatingList) : base(MachineIfItemCoatingList) { }

        public override MachineIfItemInfoList Clone()
        {
            var MachineIfItemCoatingList = new MachineIfItemInfoListCM(this);
            return MachineIfItemCoatingList;
        }

        public override void Initialize(EMachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(MachineIfItemCoating));
            foreach (Enum key in values)
            {
                if (!Dic.ContainsKey(key))
                {
                    continue;
                }

                MachineIfItemInfo machineIfProtocol = Dic[key];
                SetDefault(machineIfProtocol, machineIfType);
            }
        }

        protected new void SetDefault(MachineIfItemInfo machineIfItemInfo, EMachineIfType machineIfType)
        {
            var key = (MachineIfItemCoating)machineIfItemInfo.Command;

            if (machineIfType == EMachineIfType.Melsec)
            {
                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_READY:
                        address = "D1850"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_RUNNING:
                        address = "D1851"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_ERROR:
                        address = "D1852"; sizeWord = 1; isReadCommand = false; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_PINHOLE:
                        address = "D1853"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_DUST:
                        address = "D1854"; sizeWord = 1; isReadCommand = false; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_ALL:
                        address = "D1858"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_PINHOLE:
                        address = "D1859"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_DUST:
                        address = "D1860"; sizeWord = 1; isReadCommand = false; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_ALL:
                        address = "D1858"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE:
                        address = "D1859"; sizeWord = 1; isReadCommand = false; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_DUST:
                        address = "D1860"; sizeWord = 1; isReadCommand = false; break;

                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                var melsecMachineIfItemInfo = (MelsecMachineIfItemInfo)machineIfItemInfo;
                melsecMachineIfItemInfo.Use = isValid;
                melsecMachineIfItemInfo.Address = address;
                melsecMachineIfItemInfo.SizeWord = sizeWord;
                melsecMachineIfItemInfo.IsReadCommand = isReadCommand;
            }
            else if (machineIfType == EMachineIfType.AllenBreadley)
            {
                string tagName = "Unieye_D_Read";
                int offsetByte4; // 1 = 4byte
                int sizeByte4; // 1 = 4byte
                bool use = true;
                bool isWriteable = true;
                switch (key)
                {
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_READY:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_RUNNING:
                        offsetByte4 = 1; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_ERROR:
                        offsetByte4 = 2; sizeByte4 = 1; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_PINHOLE:
                        offsetByte4 = 3; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_DUST:
                        offsetByte4 = 4; sizeByte4 = 1; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_ALL:
                        offsetByte4 = 20; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_PINHOLE:
                        offsetByte4 = 22; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_DUST:
                        offsetByte4 = 23; sizeByte4 = 1; break;

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_ALL:
                        offsetByte4 = 20; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE:
                        offsetByte4 = 22; sizeByte4 = 1; break;
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_DUST:
                        offsetByte4 = 23; sizeByte4 = 1; break;

                    default:
                        offsetByte4 = 0; sizeByte4 = 0; break;
                }

                var allenBreadleyMachineIfItemInfo = (AllenBreadleyMachineIfItemInfo)machineIfItemInfo;
                allenBreadleyMachineIfItemInfo.Use = use;
                allenBreadleyMachineIfItemInfo.TagName = tagName;
                allenBreadleyMachineIfItemInfo.OffsetByte4 = offsetByte4;
                allenBreadleyMachineIfItemInfo.SizeByte4 = sizeByte4;
                allenBreadleyMachineIfItemInfo.IsWriteable = isWriteable;
            }
        }

        public override MachineIfItemInfo GetItemInfo(Enum command)
        {
            //if (command == null)
            //    return GetProtocol(MachineIfItemCoating.GET_MACHINE_STATE);

            return base.GetItemInfo(command);
        }
    }
}
