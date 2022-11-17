using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.Algorithm.Base
{

    public abstract class InputOutputs : TupleElement
    {
        private Dictionary<string, object> list;
        private Dictionary<string, Type> type;

        public InputOutputs()
        {
            list = new Dictionary<string, object>();
            type = new Dictionary<string, Type>();
        }

        public (string, Type)[] GetPropNameTypes()
        {
            return type.Select(f => (f.Key, f.Value)).ToArray();
        }

        public void AddKey<T>(string propName)
        {
            list.Add(propName, null);
            type.Add(propName, typeof(T));
        }

        public void AddKeyValue<T>(string key, T value)
        {
            AddKey<T>(key);
            SetValue(key, value);
        }

        public Type GetType(int i)
        {
            return type.ElementAt(i).Value;
        }

        public Type GetType(string prop)
        {
            return type[prop];
        }

        public T GetValue<T>(int i)
        {
            return (T)list.ElementAt(i).Value;
        }

        public T GetValue<T>(string prop)
        {
            return (T)list[prop];
        }

        public void SetValue<T>(int no, T data)
        {
            string key = list.ElementAt(no).Key;
            SetValue(key, data);
        }

        public void SetValue(int no, Type type, object data)
        {
            string key = list.ElementAt(no).Key;
            SetValue(key, type, data);
        }

        public void SetValue<T>(string prop, T data)
        {
            if (data != null)
            {
                Type type = data.GetType();
                Type tType = this.type[prop];
                if (!IsCompatible(tType, type))
                {
                    throw new Exception("Type missmatch");
                }
            }
            list[prop] = data;
        }

        public virtual void SetValue(string key, Type type, object value)
        {
            bool b = IsCompatible(this.type[key], type);
            if (!b)
            {
                throw new InvalidCastException();
            }

            list[key] = value;
        }

        public void SetValues(params object[] datas)
        {
            bool[] isSame = datas.Select((f, i) => f == null ? true : IsCompatible(type.ElementAt(i).Value, f.GetType())).ToArray();
            if (!Array.TrueForAll(isSame, f => f))
            {
                throw new Exception("Type missmatch");
            }

            for (int i = 0; i < list.Count; i++)
            {
                string key = list.ElementAt(i).Key;
                list[key] = datas[i];
            }
        }

        private bool IsCompatible(Type a, Type b)
        {
            bool aa = b.IsSubclassOf(a);
            bool bb = a.IsSubclassOf(b);
            return (a == b) || (b.IsSubclassOf(a));
        }
    }

    public class InputOutputs<T1> : InputOutputs
    {
        protected T1 Item1
        {
            get => GetValue<T1>(0);
            set => SetValue(0, value);
        }
        public InputOutputs(string prop1) : base()
        {
            AddKey<T1>(prop1);
        }
    }

    public class InputOutputs<T1, T2> : InputOutputs<T1>
    {
        protected T2 Item2
        {
            get => GetValue<T2>(1);
            set => SetValue(1, value);
        }

        public InputOutputs(string prop1, string prop2) : base(prop1)
        {
            AddKey<T2>(prop2);
        }
    }

    public class InputOutputs<T1, T2, T3> : InputOutputs<T1, T2>
    {
        protected T3 Item3
        {
            get => GetValue<T3>(2);
            set => SetValue(2, value);
        }

        public InputOutputs(string prop1, string prop2, string prop3) : base(prop1, prop2)
        {
            AddKey<T3>(prop3);
        }
    }

    public class InputOutputs<T1, T2, T3, T4> : InputOutputs<T1, T2, T3>
    {
        protected T4 Item4
        {
            get => GetValue<T4>(3);
            set => SetValue(3, value);
        }

        public InputOutputs(string prop1, string prop2, string prop3, string prop4) : base(prop1, prop2, prop3)
        {
            AddKey<T4>(prop4);
        }
    }

}
