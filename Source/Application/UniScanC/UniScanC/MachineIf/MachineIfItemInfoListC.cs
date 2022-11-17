using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;
using UniScanC.Enums;

namespace UniScanC.MachineIf
{
    public enum MachineIfItemCommon
    {
        // 설비 상태
        GET_READY_MACHINE,
        GET_START_MACHINE,
        GET_START_COATING,
        GET_START_THICKNESS,
        GET_START_GLOSS,

        GET_AUTO_MODEL_COATING,
        GET_AUTO_MODEL_THICKNESS,
        GET_AUTO_MODEL_GLOSS,

        GET_TARGET_WIDTH,
        GET_TARGET_SPEED,
        GET_PRESENT_SPEED,
        GET_TARGET_POSITION,
        GET_PRESENT_POSITION,
        GET_TARGET_THICKNESS,
        GET_DEFECT_NG_COUNT,

        GET_LOT,
        GET_MODEL,
        GET_WORKER,
        GET_PASTE
    };

    public abstract class MachineIfItemInfoListC : MachineIfItemInfoList
    {
        public MachineIfItemInfoListC(MachineIfItemInfoList melsecProtocolList) : base(melsecProtocolList) { }
        public MachineIfItemInfoListC(params Type[] protocolListType) : base(protocolListType) { }

        protected void SetDefault(MachineIfItemInfo machineIfItemInfo, EMachineIfType machineIfType)
        {
            var key = (MachineIfItemCommon)machineIfItemInfo.Command;
            if (machineIfType == EMachineIfType.Melsec)
            {
                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case MachineIfItemCommon.GET_READY_MACHINE:
                        address = "D1000"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_START_MACHINE:
                        address = "D1000"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_START_COATING:
                        address = "D1001"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_START_THICKNESS:
                        address = "D1002"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_START_GLOSS:
                        address = "D1003"; sizeWord = 1; isReadCommand = true; break;

                    case MachineIfItemCommon.GET_AUTO_MODEL_COATING:
                        address = "D1004"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_AUTO_MODEL_THICKNESS:
                        address = "D1005"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_AUTO_MODEL_GLOSS:
                        address = "D1006"; sizeWord = 1; isReadCommand = true; break;

                    case MachineIfItemCommon.GET_TARGET_WIDTH:
                        address = "D1007"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_TARGET_SPEED:
                        address = "D1010"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_PRESENT_SPEED:
                        address = "D1011"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_TARGET_POSITION:
                        address = "D1012"; sizeWord = 2; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_PRESENT_POSITION:
                        address = "D1013"; sizeWord = 2; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_TARGET_THICKNESS:
                        address = "D1014"; sizeWord = 1; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_DEFECT_NG_COUNT:
                        address = "D1015"; sizeWord = 1; isReadCommand = true; break;

                    case MachineIfItemCommon.GET_LOT:
                        address = "D1016"; sizeWord = 10; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_MODEL:
                        address = "D1017"; sizeWord = 10; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_WORKER:
                        address = "D1018"; sizeWord = 10; isReadCommand = true; break;
                    case MachineIfItemCommon.GET_PASTE:
                        address = "D1019"; sizeWord = 10; isReadCommand = true; break;

                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                var MelsecMachineIfItemInfo = (MelsecMachineIfItemInfo)machineIfItemInfo;
                MelsecMachineIfItemInfo.Use = isValid;
                MelsecMachineIfItemInfo.Address = address;
                MelsecMachineIfItemInfo.SizeWord = sizeWord;
                MelsecMachineIfItemInfo.IsReadCommand = isReadCommand;
            }
            else if (machineIfType == EMachineIfType.AllenBreadley)
            {
                string tagName = "Unieye_Write";
                int offsetByte4; // 1 = 4byte
                int sizeByte4; // 1 = 4byte
                bool use = true;
                bool isWriteable = false;
                switch (key)
                {
                    case MachineIfItemCommon.GET_READY_MACHINE:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_START_MACHINE:
                        offsetByte4 = 0; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_START_COATING:
                        offsetByte4 = 1; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_START_THICKNESS:
                        offsetByte4 = 2; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_START_GLOSS:
                        offsetByte4 = 3; sizeByte4 = 1; break;

                    case MachineIfItemCommon.GET_AUTO_MODEL_COATING:
                        offsetByte4 = 4; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_AUTO_MODEL_THICKNESS:
                        offsetByte4 = 5; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_AUTO_MODEL_GLOSS:
                        offsetByte4 = 6; sizeByte4 = 1; break;

                    case MachineIfItemCommon.GET_TARGET_WIDTH:
                        offsetByte4 = 7; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_TARGET_SPEED:
                        offsetByte4 = 30; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_PRESENT_SPEED:
                        offsetByte4 = 31; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_TARGET_POSITION:
                        offsetByte4 = 32; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_PRESENT_POSITION:
                        offsetByte4 = 33; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_TARGET_THICKNESS:
                        offsetByte4 = 34; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_DEFECT_NG_COUNT:
                        offsetByte4 = 35; sizeByte4 = 1; break;

                    case MachineIfItemCommon.GET_LOT:
                        offsetByte4 = 60; sizeByte4 = 10; break;
                    case MachineIfItemCommon.GET_MODEL:
                        offsetByte4 = 70; sizeByte4 = 10; break;
                    case MachineIfItemCommon.GET_WORKER:
                        offsetByte4 = 80; sizeByte4 = 1; break;
                    case MachineIfItemCommon.GET_PASTE:
                        offsetByte4 = 90; sizeByte4 = 10; break;

                    default:
                        offsetByte4 = 0; sizeByte4 = 0; use = false; break;
                }

                var allenBreadleyMachineIfItemInfo = (AllenBreadleyMachineIfItemInfo)machineIfItemInfo;
                allenBreadleyMachineIfItemInfo.Use = use;
                allenBreadleyMachineIfItemInfo.TagName = tagName;
                allenBreadleyMachineIfItemInfo.OffsetByte4 = offsetByte4;
                allenBreadleyMachineIfItemInfo.SizeByte4 = sizeByte4;
                allenBreadleyMachineIfItemInfo.IsWriteable = isWriteable;
            }
        }

        public override void CopyFrom(MachineIfItemInfoList machineIfItemInfoList)
        {
            base.CopyFrom(machineIfItemInfoList);
        }

        public override void Initialize(EMachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            Array values = Enum.GetValues(typeof(MachineIfItemCommon));
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

        public override MachineIfItemInfo GetItemInfo(Enum command)
        {
            //if (command == null)
            //    return GetProtocol(MelsecProtocolCommon.GET_MACHINE_STATE);

            return base.GetItemInfo(command);
        }
    }
}