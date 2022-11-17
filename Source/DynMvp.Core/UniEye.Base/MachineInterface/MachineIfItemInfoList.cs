using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;

namespace UniEye.Base.MachineInterface
{
    public abstract class MachineIfItemInfoList
    {
        #region 생성자
        public MachineIfItemInfoList(params Type[] itemInfoTypeList)
        {
            ItemInfoTypeList = itemInfoTypeList;
            for (int i = 0; i < itemInfoTypeList.Length; i++)
            {
                AddItemInfo(itemInfoTypeList[i]);
            }
        }

        public MachineIfItemInfoList(MachineIfItemInfoList machineIfItemInfoList) : this(machineIfItemInfoList.ItemInfoTypeList)
        {
            Dictionary<Enum, MachineIfItemInfo>.Enumerator enumerator = machineIfItemInfoList.Dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (Dic.ContainsKey(enumerator.Current.Key))
                {
                    Dic[enumerator.Current.Key] = enumerator.Current.Value.Clone();
                }
            }
        }
        #endregion


        #region 속성
        public Type[] ItemInfoTypeList { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Dictionary<Enum, MachineIfItemInfo> Dic { get; set; } = new Dictionary<Enum, MachineIfItemInfo>();

        // Json 저장을 위한 클래스
        public List<MachineIfItemInfo> DicList { get; set; } = new List<MachineIfItemInfo>();

        public static MachineIfItemInfoList List { get; private set; } = null;
        public static void Set(MachineIfItemInfoList list)
        {
            MachineIfItemInfoList.List = list;
        }
        #endregion


        #region 메서드
        public Enum GetEnum(string command)
        {
            foreach (Type type in ItemInfoTypeList)
            {
                if (type.IsEnumDefined(command))
                {
                    var e = (Enum)Enum.Parse(type, command);
                    return e;
                }
            }
            return null;
        }

        public MachineIfItemInfo[] GetItemInfos()
        {
            var list = Dic.Values.ToList();
            list.RemoveAll(f => f == null);
            return list.ToArray();
        }

        // Json 저장을 위한 메서드
        public void ConvertListToDic()
        {
            var tempDic = new Dictionary<Enum, MachineIfItemInfo>();
            foreach (KeyValuePair<Enum, MachineIfItemInfo> pair in Dic)
            {
                MachineIfItemInfo ItemInfo = DicList.Find(x => x.Name == pair.Key.ToString());
                if (ItemInfo != null)
                {
                    ItemInfo.Command = pair.Key;
                    tempDic.Add(pair.Key, ItemInfo.Clone());
                }
            }
            Dic.Clear();
            Dic = tempDic;
        }

        // Json 저장을 위한 메서드
        public void ConvertDicToList()
        {
            DicList.Clear();
            foreach (KeyValuePair<Enum, MachineIfItemInfo> pair in Dic)
            {
                DicList.Add(pair.Value);
            }
        }

        public abstract MachineIfItemInfoList Clone();

        public virtual void CopyFrom(MachineIfItemInfoList machineIfProtocolList)
        {
            ItemInfoTypeList = (Type[])machineIfProtocolList.ItemInfoTypeList.Clone();

            Dic.Clear();
            Dictionary<Enum, MachineIfItemInfo>.Enumerator v = machineIfProtocolList.Dic.GetEnumerator();
            while (v.MoveNext())
            {
                Dic.Add(v.Current.Key, v.Current.Value.Clone());
            }
        }

        public virtual void Initialize(EMachineIfType machineIfType)
        {
            var func = new Func<KeyValuePair<Enum, MachineIfItemInfo>, MachineIfItemInfo>(f =>
            {
                switch (machineIfType)
                {
                    case EMachineIfType.Melsec:
                        return new MelsecMachineIfItemInfo(f.Key);
                    case EMachineIfType.AllenBreadley:
                        return new AllenBreadleyMachineIfItemInfo(f.Key);
                    default:
                        return null;
                }
            });

            Dic = Dic.ToDictionary(f => f.Key, f => func(f));
        }

        public virtual MachineIfItemInfo GetItemInfo(string e)
        {
            foreach (KeyValuePair<Enum, MachineIfItemInfo> pair in Dic)
            {
                if (pair.Key.ToString() == e)
                {
                    return pair.Value.Clone();
                }
            }
            return null;
        }

        public virtual MachineIfItemInfo GetItemInfo(Enum e)
        {
            return Dic[e]?.Clone();
        }

        private void AddItemInfo(Type itemInfoType)
        {
            //this.dic.Clear();
            Array array = Enum.GetValues(itemInfoType);
            foreach (Enum e in array)
            {
                Dic.Add(e, null);
            }
        }
        #endregion

    }
}
