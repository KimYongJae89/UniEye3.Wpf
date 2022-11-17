using DynMvp.Base;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public enum LightParamType
    {
        Value, Calculation
    }

    public class LightParam : ICloneable
    {
        public int Index { get; set; }
        public string Name { get; set; } = "";
        public LightParamType LightParamType { get; set; } = LightParamType.Value;
        public LightValue LightValue { get; private set; }
        public ImageOperationType OperationType { get; set; }
        public int FirstImageIndex { get; set; } = 0;
        public int SecondImageIndex { get; set; } = 0;

        public string KeyValue
        {
            get
            {
                string keyValue = "";
                if (LightValue != null)
                {
                    if (LightParamType == LightParamType.Value)
                    {
                        keyValue = string.Format("V{0}", LightValue.KeyValue);
                    }
                    else
                    {
                        keyValue = string.Format("C{0}", LightValue.KeyValue);
                    }
                }
                return keyValue;
            }
        }

        public LightParam(int index, string name, int numLight)
        {
            Index = index;
            Name = name;
            LightValue = new LightValue(numLight);
        }

        internal bool IsSame(LightParam lightParam)
        {
            if (LightParamType != lightParam.LightParamType)
            {
                return false;
            }

            if (LightValue.KeyValue != lightParam.LightValue.KeyValue)
            {
                return false;
            }

            return true;
        }

        public object Clone()
        {
            var lightParam = new LightParam(Index, Name, LightValue.NumLight);
            lightParam.Copy(this);

            return lightParam;
        }

        public void Copy(LightParam lightParam)
        {
            Name = lightParam.Name;

            LightParamType = lightParam.LightParamType;

            LightValue = new LightValue(lightParam.LightValue.NumLight);
            LightValue.Copy(lightParam.LightValue);

            OperationType = lightParam.OperationType;
            FirstImageIndex = lightParam.FirstImageIndex;
            SecondImageIndex = lightParam.SecondImageIndex;
        }

        public void Save(XmlElement lightParamElement)
        {
            XmlHelper.SetValue(lightParamElement, "Name", Name.ToString());

            XmlHelper.SetValue(lightParamElement, "LightParamType", LightParamType.ToString());

            for (int i = 0; i < LightValue.NumLight; i++)
            {
                XmlHelper.SetValue(lightParamElement, string.Format("LightOnValue{0}", i), LightValue.Value[i].ToString());
            }

            XmlHelper.SetValue(lightParamElement, "OperationType", OperationType.ToString());
            XmlHelper.SetValue(lightParamElement, "FirstImageIndex", FirstImageIndex.ToString());
            XmlHelper.SetValue(lightParamElement, "SecondImageIndex", SecondImageIndex.ToString());
        }

        public void Load(XmlElement lightParamElement)
        {
            Name = XmlHelper.GetValue(lightParamElement, "Name", "");

            LightParamType = (LightParamType)Enum.Parse(typeof(LightParamType), XmlHelper.GetValue(lightParamElement, "LightParamType", "Value"));

            for (int i = 0; i < LightValue.NumLight; i++)
            {
                LightValue.Value[i] = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, string.Format("LightOnValue{0}", i), "0"));
            }

            OperationType = (ImageOperationType)Enum.Parse(typeof(ImageOperationType), XmlHelper.GetValue(lightParamElement, "OperationType", "Subtract"));
            FirstImageIndex = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "FirstImageIndex", "0"));
            SecondImageIndex = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "SecondImageIndex", "1"));
        }
    }

    public class LightParamSet
    {
        public List<LightParam> LightParamList { get; set; } = new List<LightParam>();

        public LightParam this[int index]
        {
            get => LightParamList[index];
            set => LightParamList[index] = value;
        }

        public int NumLightDevice
        {
            get
            {
                if (LightParamList != null && LightParamList.Count > 0)
                {
                    return LightParamList[0].LightValue.NumLight;
                }

                return 0;
            }
        }

        public int NumLightType
        {
            get
            {
                if (LightParamList != null)
                {
                    return LightParamList.Count;
                }

                return 0;
            }
        }

        public IEnumerator<LightParam> GetEnumerator()
        {
            return LightParamList.GetEnumerator();
        }

        public void Initialize(int numLightDevice, int numLightType)
        {
            LightParamList = new List<LightParam>();

            for (int i = 0; i < numLightType; i++)
            {
                LightParamList.Add(new LightParam(i, string.Format("Light {0}", i + 1), numLightDevice));
            }
        }

        public LightParamSet Clone()
        {
            var cloneLightParamSet = new LightParamSet();
            cloneLightParamSet.Initialize(NumLightDevice, NumLightType);

            for (int i = 0; i < LightParamList.Count(); i++)
            {
                cloneLightParamSet[i] = (LightParam)LightParamList[i].Clone();
            }

            return cloneLightParamSet;
        }

        public void Load(XmlElement lightParamSetElement)
        {
            int lightParamIndex = 0;
            foreach (XmlElement lightParamElement in lightParamSetElement)
            {
                if (lightParamElement.Name == "LightParam")
                {
                    if (LightParamList == null)
                    {
                        break;
                    }

                    LightParamList[lightParamIndex].Load(lightParamElement);

                    if (string.IsNullOrEmpty(LightParamList[lightParamIndex].Name))
                    {
                        LightParamList[lightParamIndex].Name = string.Format("LightType {0}", lightParamIndex);
                    }
                    lightParamIndex++;

                    if (lightParamIndex >= LightParamList.Count)
                    {
                        break;
                    }
                }
            }
        }

        public void Save(XmlElement lightParamSetElement)
        {
            if (LightParamList != null)
            {
                foreach (LightParam lightParam in LightParamList)
                {
                    XmlElement lightParamElement = lightParamSetElement.OwnerDocument.CreateElement("", "LightParam", "");
                    lightParamSetElement.AppendChild(lightParamElement);

                    lightParam.Save(lightParamElement);
                }
            }
        }

        public List<LightParam> GetValueTypeList()
        {
            return LightParamList.FindAll(f => f.LightParamType == LightParamType.Value);
        }

        public List<LightParam> GetCalculationTypeList()
        {
            return LightParamList.FindAll(f => f.LightParamType == LightParamType.Calculation);
        }

        public LightParam GetLightParam(string lightTypeName)
        {
            return LightParamList.Find(f => f.Name == lightTypeName);
        }
    }
}
