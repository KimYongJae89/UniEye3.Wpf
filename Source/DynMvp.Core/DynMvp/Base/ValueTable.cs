using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Base
{
    public static class TConverter
    {
        public static T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {

            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }

    public class ValueTable<T>
    {
        private Dictionary<string, T> values = new Dictionary<string, T>();

        public void Clear()
        {
            values.Clear();
        }

        public T this[string key]
        {
            get
            {
                if (values.TryGetValue(key, out T value) == false)
                {
                    Debug.Assert(false, "Invalid key name : " + key);
                    return default(T);
                }

                return value;
            }
            set
            {
                if (values.TryGetValue(key, out T val) == false)
                {
                    Debug.Assert(false, "Invalid key name : " + key);
                    return;
                }

                values[key] = value;
            }
        }

        public T GetValue(string key)
        {
            try
            {
                return values[key];
            }
            catch (KeyNotFoundException)
            {
            }

            return default(T);
        }

        public T GetValue(string key, T defaultValue)
        {
            var defVal = default(T);
            T value = GetValue(key);
            if (value.Equals(defVal) == false)
            {
                return value;
            }

            return defaultValue;
        }

        public void AddValue(string key, T value)
        {
            values.Add(key, value);
        }

        public void SetValue(string key, T value)
        {
            foreach (KeyValuePair<string, T> item in values)
            {
                if (item.Key.Contains(key))
                {
                    values[item.Key] = value;
                    return;
                }
            }
        }

        public void Load(XmlElement xmlElement, string sectionName)
        {
            XmlElement valueTableElement = xmlElement[sectionName];
            if (valueTableElement == null)
            {
                return;
            }

            foreach (XmlElement valueElement in valueTableElement)
            {
                values[valueElement.Name] = TConverter.ChangeType<T>(valueElement.InnerText);
            }
        }

        public void Save(XmlElement xmlElement, string sectionName)
        {
            XmlElement valueTableElement = xmlElement.OwnerDocument.CreateElement("", sectionName, "");
            xmlElement.AppendChild(valueTableElement);

            foreach (KeyValuePair<string, T> pair in values)
            {
                XmlHelper.SetValue(valueTableElement, pair.Key, pair.Value.ToString());
            }
        }

        public void FromString(string valueString)
        {
            string[] valueStrArr = valueString.Split('/');
            foreach (string valueStr in valueStrArr)
            {
                string[] keyValue = valueStr.Split(':');
                values[keyValue[0]] = TConverter.ChangeType<T>(keyValue[1]);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, T> pair in values)
            {
                sb.AppendFormat("{0}:{1}/", pair.Key, pair.Value.ToString());
            }
            return sb.ToString().TrimEnd('/');
        }
    }
}
