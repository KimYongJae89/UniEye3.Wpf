using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public class LightValue
    {
        public int this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }
        public int[] Value { get; private set; }

        public LightValue(int numLight, int defaultValue = 0)
        {
            Value = new int[numLight];
            for (int i = 0; i < numLight; i++)
            {
                Value[i] = defaultValue;
            }
        }

        public LightValue Clone()
        {
            var lightValue = new LightValue(Value.Count());
            lightValue.Copy(this);

            return lightValue;
        }

        internal void Clip(int value) //What is means?
        {
            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = Math.Min(Value[i], value);
            }
        }

        public void Copy(LightValue lightValue)
        {
            Value = new int[lightValue.NumLight];
            for (int i = 0; i < lightValue.NumLight; i++)
            {
                Value[i] = lightValue.Value[i];
            }
        }

        public void TurnOn()
        {
            for (int i = 0; i < Value.Count(); i++)
            {
                Value[i] = 128;
            }
        }

        public void TurnOff()
        {
            for (int i = 0; i < Value.Count(); i++)
            {
                Value[i] = 0;
            }
        }

        public int NumLight => Value.Count();

        public string KeyValue
        {
            get
            {
                string keyValue = "";

                for (int i = 0; i < Value.Count(); i++)
                {
                    keyValue += Value[i].ToString("x2");
                }

                return keyValue;
            }
        }
    }

    public class LightParamList
    {
        private List<LightParam> lightParamList = new List<LightParam>();

        public IEnumerator<LightParam> GetEnumerator()
        {
            return lightParamList.GetEnumerator();
        }

        public void AddLightValue(LightParam lightParam)
        {
            if (IsContained(lightParam) == false)
            {
                lightParamList.Add(lightParam);
            }
        }

        public LightParam this[int index] => lightParamList[index];

        public bool IsContained(LightParam lightParam)
        {
            foreach (LightParam lp in lightParamList)
            {
                if (lp.KeyValue == lightParam.KeyValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
