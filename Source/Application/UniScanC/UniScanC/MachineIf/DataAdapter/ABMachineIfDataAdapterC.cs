using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;

namespace UniScanC.MachineIf.DataAdapter
{
    public abstract class ABMachineIfDataAdapterC : MachineIfDataAdapterC
    {
        private AllenBreadleyMachineIfItemInfoSet c_PLCtoUniEye;

        public ABMachineIfDataAdapterC(MachineIfDataBase machineIfData) : base(machineIfData)
        {
            c_PLCtoUniEye = GetItemInfoSet(ItemSet.GET_MACHINE_STATE, "Unieye_Write",
                MachineIfItemCommon.GET_READY_MACHINE,
                MachineIfItemCommon.GET_START_MACHINE,
                MachineIfItemCommon.GET_START_COATING,
                MachineIfItemCommon.GET_START_THICKNESS,
                MachineIfItemCommon.GET_START_GLOSS,

                MachineIfItemCommon.GET_TARGET_WIDTH,
                MachineIfItemCommon.GET_TARGET_SPEED,
                MachineIfItemCommon.GET_PRESENT_SPEED,
                MachineIfItemCommon.GET_TARGET_POSITION,
                MachineIfItemCommon.GET_PRESENT_POSITION,
                MachineIfItemCommon.GET_TARGET_THICKNESS,
                MachineIfItemCommon.GET_DEFECT_NG_COUNT,

                MachineIfItemCommon.GET_LOT,
                MachineIfItemCommon.GET_MODEL,
                MachineIfItemCommon.GET_WORKER,
                MachineIfItemCommon.GET_PASTE);

            var fieldInfoList = MachineIfData.GetType().GetFields().ToList();
            foreach (MachineIfItemCommon command in Enum.GetValues(typeof(MachineIfItemCommon)))
            {
                c_PLCtoUniEye.EnumFieldPair.Add(command, fieldInfoList.Find(f => f.Name == command.ToString()));
            }
        }

        protected abstract AllenBreadleyMachineIfItemInfoSet GetItemInfoSet(Enum command, string tagName, params Enum[] subCommand);

        public override void Read()
        {
            Read(c_PLCtoUniEye);
        }

        protected abstract void Read(AllenBreadleyMachineIfItemInfoSet set);

        protected void Parse(Dictionary<Enum, FieldInfo> pairs, MachineIfItemInfoResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            int l = responce.ReciveData.Length / 8;
            int[] values = new int[l];
            for (int i = 0; i < l; i++)
            {
                values[i] = Convert.ToInt32(responce.ReciveData.Substring(i * 8, 8), 16);
            }

            foreach (KeyValuePair<Enum, FieldInfo> pair in pairs)
            {
                Parse(values, pair.Value, Array.Find(tuples, f => f.Item1.Equals(pair.Key)));
            }
        }

        protected void Parse(int[] values, FieldInfo fieldInfo, Tuple<Enum, int, int> tuple)
        {
            if (tuple == null)
            {
                return;
            }

            if (fieldInfo.FieldType.Name == "String")
            {
                //char[] subValues = new char[tuple.Item3 * 4];
                //Buffer.BlockCopy(values, tuple.Item2 * 4, subValues, 0, tuple.Item3 * 4);
                //string str = new string(subValues).Trim('\0');
                //fieldInfo.SetValue(this.machineIfData, str);

                var byteList = new List<byte>();
                for (int i = 0; i < tuple.Item3; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(values[tuple.Item2 + i]);
                    byteList.AddRange(bytes.Reverse());
                }
                string strr = new string(byteList.ConvertAll<char>(f => (char)f).ToArray()).Trim('\0');
                fieldInfo.SetValue(MachineIfData, strr);
            }
            else
            {
                int value = values[tuple.Item2];
                object var = Convert.ChangeType(value, fieldInfo.FieldType);
                fieldInfo.SetValue(MachineIfData, var);
            }
        }

        public override void Write()
        {
            Write(c_PLCtoUniEye);
        }

        protected abstract void Write(AllenBreadleyMachineIfItemInfoSet set);
    }

    public class AllenBreadleyMachineIfItemInfoSet : AllenBreadleyMachineIfItemInfo
    {
        // this.Name: 통합 명령 이름
        // this.Address: 통합 시작주소

        // Key.Name: 통합 명령 이름
        // Key.Address: 연속된 주소의 시작
        // Key.Size: 연속된 주소의 크기

        // Value[i].Item1: 연속된 주소를 이루는 명령의 이름
        // Value[i].Item2: 연속된 주소 내 하위 명령의 위치
        // Value[i].Item2: 연속된 주소 내 하위 명령의 크기
        public Dictionary<AllenBreadleyMachineIfItemInfo, Tuple<Enum, int, int>[]> Dictionary { get; }

        public Dictionary<Enum, FieldInfo> EnumFieldPair { get; } = new Dictionary<Enum, FieldInfo>();

        public AllenBreadleyMachineIfItemInfoSet(Enum command) : base(command)
        {
            Dictionary = new Dictionary<AllenBreadleyMachineIfItemInfo, Tuple<Enum, int, int>[]>();
        }

        public void Add(AllenBreadleyMachineIfItemInfo ItemInfo, List<AllenBreadleyMachineIfItemInfo> ItemInfoList)
        {
            var orderBy = ItemInfoList.OrderBy(f => f.OffsetByte4).ToList();
            IEnumerable<Tuple<Enum, int, int>> enumerable = orderBy.Select(f => new Tuple<Enum, int, int>(f.Command, f.OffsetByte4, f.SizeByte4));
            Dictionary.Add(ItemInfo, enumerable.ToArray());
        }
    }
}
