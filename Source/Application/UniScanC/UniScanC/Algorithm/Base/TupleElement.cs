using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.Algorithm.Base
{

    public interface TupleElement
    {
        //int GetPropIndex(string propName);

        //string GetPropName(int i);

        (string, Type)[] GetPropNameTypes();

        void AddKey<T>(string key);
        void AddKeyValue<T>(string key, T value);

        Type GetType(int i);
        Type GetType(string prop);

        void SetValue<T>(int i, T value);
        void SetValue(int i, Type type, object value);

        void SetValue<T>(string prop, T value);
        void SetValue(string key, Type type, object value);

        T GetValue<T>(int i);
        T GetValue<T>(string prop);
    }

}
