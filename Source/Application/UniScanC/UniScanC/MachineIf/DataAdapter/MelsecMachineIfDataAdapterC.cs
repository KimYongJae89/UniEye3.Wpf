using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.Melsec;

namespace UniScanC.MachineIf.DataAdapter
{
    public abstract class MelsecMachineIfDataAdapterC : MachineIfDataAdapterC
    {
        protected MelsecMachineIfItemInfoSet GetMachineStateItemInfoSet { get; set; }

        protected abstract string MakeArgument(Tuple<Enum, int, int>[] tuples);

        public MelsecMachineIfDataAdapterC(UniEye.Base.MachineInterface.MachineIfDataBase machineIfData) : base(machineIfData)
        {
            GetMachineStateItemInfoSet = GetItemInfoSet(ItemSet.GET_MACHINE_STATE,
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
        }

        protected abstract MelsecMachineIfItemInfoSet GetItemInfoSet(Enum command, params Enum[] subCommand);

        protected abstract void Read(MelsecMachineIfItemInfoSet melsecMachineIfItemInfoSet);

        public virtual void Parse(MachineIfItemInfoResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            MachineIfDataC machineIfDataC = MachineIfData;

            Parse<bool>(responce.ReciveData, MachineIfItemCommon.GET_READY_MACHINE, ref machineIfDataC.GET_READY_MACHINE, tuples);
            Parse<bool>(responce.ReciveData, MachineIfItemCommon.GET_START_MACHINE, ref machineIfDataC.GET_START_MACHINE, tuples);
            Parse<bool>(responce.ReciveData, MachineIfItemCommon.GET_START_COATING, ref machineIfDataC.GET_START_COATING, tuples);
            Parse<bool>(responce.ReciveData, MachineIfItemCommon.GET_START_THICKNESS, ref machineIfDataC.GET_START_THICKNESS, tuples);
            Parse<bool>(responce.ReciveData, MachineIfItemCommon.GET_START_GLOSS, ref machineIfDataC.GET_START_GLOSS, tuples);

            Parse(responce.ReciveData, MachineIfItemCommon.GET_TARGET_SPEED, ref machineIfDataC.GET_TARGET_SPEED, tuples);
            Parse(responce.ReciveData, MachineIfItemCommon.GET_PRESENT_SPEED, ref machineIfDataC.GET_PRESENT_SPEED, tuples);
            Parse(responce.ReciveData, MachineIfItemCommon.GET_TARGET_POSITION, ref machineIfDataC.GET_TARGET_POSITION, tuples);
            Parse(responce.ReciveData, MachineIfItemCommon.GET_PRESENT_POSITION, ref machineIfDataC.GET_PRESENT_POSITION, tuples);

            Parse<string>(responce.ReciveData, MachineIfItemCommon.GET_LOT, ref machineIfDataC.GET_LOT, tuples);
            Parse<string>(responce.ReciveData, MachineIfItemCommon.GET_MODEL, ref machineIfDataC.GET_MODEL, tuples);
            Parse<string>(responce.ReciveData, MachineIfItemCommon.GET_WORKER, ref machineIfDataC.GET_WORKER, tuples);
            Parse<string>(responce.ReciveData, MachineIfItemCommon.GET_PASTE, ref machineIfDataC.GET_PASTE, tuples);
        }

        private bool Parse(string data, Enum command, ref float value, Tuple<Enum, int, int>[] tuples, float div)
        {
            Tuple<Enum, int, int> tuple = Array.Find(tuples, f => f.Item1.Equals(command));
            if (tuple == null)
            {
                return false;
            }

            try
            {
                value = Parse<int>(data, tuple.Item2 * 2, tuple.Item3 * 2) / div;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool Parse<T>(string data, Enum command, ref T value, Tuple<Enum, int, int>[] tuples)
        {
            Tuple<Enum, int, int> tuple = Array.Find(tuples, f => f.Item1.Equals(command));
            if (tuple == null)
            {
                return false;
            }

            try
            {
                value = Parse<T>(data, tuple.Item2 * 2, tuple.Item3 * 2);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"MelsecMachineIfDataAdapter::Parse - {ex.GetType().Name} - {ex.Message} - {command}");
                return false;
            }
        }

        private T Parse<T>(string reciveData, int start, int count)
        {
            string hexBytes = reciveData;
            string hexData = hexBytes.Substring(start, count);

            Type type = typeof(T);
            if (type.Name == "String")
            {
                int len = hexData.Length / 2;
                char[] chars = new char[len];
                for (int i = 0; i < len; i += 2)
                {
                    chars[i] = (char)Convert.ToByte(hexData.Substring((i + 1) * 2, 2), 16);
                    chars[i + 1] = (char)Convert.ToByte(hexData.Substring(i * 2, 2), 16);
                }
                string str = new string(chars).Trim(' ', '\0');
                return (T)Convert.ChangeType(str, typeof(T));
            }
            else
            {
                char[] convertChars = new char[count];
                int wards = count / 4;
                for (int i = 0; i < wards; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        convertChars[4 * i + j] = hexData[4 * (wards - 1 - i) + j];
                    }
                }
                string convertString = string.Concat(convertChars);
                uint decInt = uint.Parse(convertString, System.Globalization.NumberStyles.AllowHexSpecifier);
                return (T)Convert.ChangeType(decInt, typeof(T));
            }
        }

        protected abstract void Write(MelsecMachineIfItemInfoSet melsecMachineIfItemInfoSet);
    }

    public class MelsecMachineIfItemInfoSet : MelsecMachineIfItemInfo
    {
        // this.Name: 통합명령 이름
        // this.Address: [Empty]

        // Key.Name: [Empty]
        // Key.Address: 연속된 주소의 시작
        // Key.Size: 연속된 주소의 크기

        // Value[i].Item1: 연속된 주소를 이루는 명령의 이름
        // Value[i].Item2: 연속된 주소 내 하위 명령의 위치
        // Value[i].Item2: 연속된 주소 내 하위 명령의 크기

        public Dictionary<MelsecMachineIfItemInfo, Tuple<Enum, int, int>[]> Dictionary { get; }

        public MelsecMachineIfItemInfoSet(Enum command) : base(command)
        {
            Dictionary = new Dictionary<MelsecMachineIfItemInfo, Tuple<Enum, int, int>[]>();
        }

        public void AddProtocol(MelsecMachineIfItemInfo protocol, Tuple<Enum, int, int>[] tuples)
        {
            Dictionary.Add(protocol, tuples);
        }
    }
}
