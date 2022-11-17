using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniScanC.MachineIf.DataAdapter;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC;

namespace WPF.UniScanCM.MachineIf.DataAdapter
{
    public class ABMachineIfDataAdapterCM : ABMachineIfDataAdapterC
    {
        #region 생성자
        public ABMachineIfDataAdapterCM(MachineIfDataCM machineIfData) : base(machineIfData)
        {
            c_PItoPLC = GetItemInfoSet(MachineIfItemSet.SET_VISION_STATE, "Unieye_D_Read",
             MachineIfItemCoating.SET_VISION_COATING_INSP_READY,
             MachineIfItemCoating.SET_VISION_COATING_INSP_RUNNING,
             MachineIfItemCoating.SET_VISION_COATING_INSP_ERROR,

             MachineIfItemCoating.SET_VISION_COATING_INSP_NG_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_NG_DUST,

             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_ALL,
             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_DUST,

             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_ALL,
             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_DUST
             );

            var fieldInfoList = MachineIfData.GetType().GetFields().ToList();
            foreach (MachineIfItemCoating command in Enum.GetValues(typeof(MachineIfItemCoating)))
            {
                c_PItoPLC.EnumFieldPair.Add(command, fieldInfoList.Find(f => f.Name == command.ToString()));
            }
        }
        #endregion


        #region 속성
        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private PlcBase PLC => DeviceManager?.PLCMachineIf;

        private AllenBreadleyMachineIfItemInfoSet c_PItoPLC { get; set; }
        #endregion


        #region 메서드
        protected override AllenBreadleyMachineIfItemInfoSet GetItemInfoSet(Enum command, string tagName, params Enum[] subCommand)
        {
            MachineIfItemInfoList list = PLC.MachineIf.MachineIfSetting.MachineIfItemInfoList;
            var ItemInfoList = list.GetItemInfos().Cast<AllenBreadleyMachineIfItemInfo>().ToList();
            ItemInfoList.RemoveAll(f => !f.IsValid || !f.Use || f.TagName != tagName || !subCommand.Contains(f.Command));

            var set = new AllenBreadleyMachineIfItemInfoSet(command)
            {
                Use = true
            };

            // Write
            List<AllenBreadleyMachineIfItemInfo> writeableItemInfoList = ItemInfoList.FindAll(f => f.IsWriteable);
            if (writeableItemInfoList.Count > 0)
            {
                var ItemInfoW = new AllenBreadleyMachineIfItemInfo(null)
                {
                    WaitResponceMs = writeableItemInfoList.Max(f => f.WaitResponceMs),
                    TagName = tagName,
                    Use = true,
                    OffsetByte4 = writeableItemInfoList.Min(f => f.OffsetByte4),
                    SizeByte4 = writeableItemInfoList.Max(f => f.OffsetByte4 + f.SizeByte4),
                    IsWriteable = true
                };
                set.Add(ItemInfoW, ItemInfoList);

                ItemInfoList.RemoveAll(f => writeableItemInfoList.Contains(f));
            }

            // Read
            if (ItemInfoList.Count > 0)
            {
                var ItemInfoR = new AllenBreadleyMachineIfItemInfo(null)
                {
                    WaitResponceMs = ItemInfoList.Max(f => f.WaitResponceMs),
                    TagName = tagName,
                    Use = ItemInfoList.Count > 0,
                    OffsetByte4 = ItemInfoList.Count == 0 ? 0 : ItemInfoList.Min(f => f.OffsetByte4),
                    SizeByte4 = ItemInfoList.Count == 0 ? 0 : ItemInfoList.Max(f => f.OffsetByte4 + f.SizeByte4),
                    IsWriteable = false
                };
                set.Add(ItemInfoR, ItemInfoList);
            }

            return set;
        }

        public override void Read()
        {
            base.Read();

            Read(c_PItoPLC);
        }

        protected override void Read(AllenBreadleyMachineIfItemInfoSet set)
        {
            foreach (KeyValuePair<AllenBreadleyMachineIfItemInfo, Tuple<Enum, int, int>[]> pair in set.Dictionary)
            {
                if (pair.Key.IsWriteable)
                {
                    continue;
                }

                MachineIfItemInfoResponce responce = PLC.MachineIf.SendCommand(pair.Key);
                responce.WaitResponce();

                if (responce.IsGood && responce.IsResponced)
                {
                    Parse(set.EnumFieldPair, responce, pair.Value);
                }
            }
        }

        public override void Write()
        {
            base.Write();

            Write(c_PItoPLC);
        }

        protected override void Write(AllenBreadleyMachineIfItemInfoSet set)
        {
            foreach (KeyValuePair<AllenBreadleyMachineIfItemInfo, Tuple<Enum, int, int>[]> pair in set.Dictionary)
            {
                if (!pair.Key.IsWriteable)
                {
                    continue;
                }

                string[] arguments = GetArgument(pair, set.EnumFieldPair);
                MachineIfItemInfoResponce responce = PLC.MachineIf.SendCommand(pair.Key, arguments);
                responce.WaitResponce();
            }
        }

        private string[] GetArgument(KeyValuePair<AllenBreadleyMachineIfItemInfo, Tuple<Enum, int, int>[]> pair,
            Dictionary<Enum, FieldInfo> enumFieldPair)
        {
            int[] arguments = new int[pair.Value.Max(f => f.Item2 + f.Item3)];

            Array.ForEach(pair.Value, f =>
            {
                if (enumFieldPair.ContainsKey(f.Item1))
                {
                    FieldInfo fieldInfo = enumFieldPair[f.Item1];
                    object value = fieldInfo.GetValue(MachineIfData);
                    if (fieldInfo.FieldType.Name == "String")
                    {
                        char[] chars = ((string)value).ToArray();
                        Buffer.BlockCopy(chars, 0, arguments, f.Item2 * 4, chars.Length);
                    }
                    else
                    {
                        arguments[f.Item2] = (int)Convert.ChangeType(value, typeof(int));
                    }
                }
            });

            //return arguments;
            return arguments.Select(f => f.ToString("X08")).ToArray();
        }
        #endregion
    }
}
