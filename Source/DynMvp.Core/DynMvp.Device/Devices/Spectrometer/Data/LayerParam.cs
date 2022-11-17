using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Spectrometer.Data
{
    public class LayerParam : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string paramName;
        public string ParamName { get => paramName; set => Set(ref paramName, value); }

        private int startWavelength;
        public int StartWavelength { get => startWavelength; set => Set(ref startWavelength, value); }

        private int endWavelength;
        public int EndWavelength { get => endWavelength; set => Set(ref endWavelength, value); }

        private int minThickness;
        public int MinThickness { get => minThickness; set => Set(ref minThickness, value); }

        private int maxThickness;
        public int MaxThickness { get => maxThickness; set => Set(ref maxThickness, value); }

        private float calibParam0;
        public float CalibParam0 { get => calibParam0; set => Set(ref calibParam0, value); }

        private float calibParam1;
        public float CalibParam1 { get => calibParam1; set => Set(ref calibParam1, value); }

        private float threshold;
        public float Threshold { get => threshold; set => Set(ref threshold, value); }

        private int dataLengthPow;
        public int DataLengthPow { get => dataLengthPow; set => Set(ref dataLengthPow, value); }

        private double refraction;
        public double Refraction { get => refraction; set => Set(ref refraction, value); }

        private int validDataNum;
        public int ValidDataNum { get => validDataNum; set => Set(ref validDataNum, value); }

        public LayerParam(string paramName = "LayerParam1")
        {
            ParamName = paramName;
            StartWavelength = 500;
            EndWavelength = 2100;
            MinThickness = 0;
            MaxThickness = 50;
            CalibParam0 = 0;
            CalibParam1 = 1;
            Threshold = 0;
            DataLengthPow = 16;
            Refraction = 1;
            ValidDataNum = 0;
        }

        public LayerParam Clone()
        {
            var layerParam = new LayerParam();
            layerParam.CopyFrom(this);
            return layerParam;
        }

        public void CopyFrom(LayerParam layerParam)
        {
            paramName = layerParam.paramName;
            startWavelength = layerParam.startWavelength;
            endWavelength = layerParam.endWavelength;
            minThickness = layerParam.minThickness;
            maxThickness = layerParam.maxThickness;
            calibParam0 = layerParam.calibParam0;
            calibParam1 = layerParam.calibParam1;
            threshold = layerParam.threshold;
            dataLengthPow = layerParam.dataLengthPow;
            refraction = layerParam.refraction;
            validDataNum = layerParam.validDataNum;
        }

        public void CopyTo(LayerParam layerParam)
        {
            layerParam.paramName = paramName;
            layerParam.startWavelength = startWavelength;
            layerParam.endWavelength = endWavelength;
            layerParam.minThickness = minThickness;
            layerParam.maxThickness = maxThickness;
            layerParam.calibParam0 = calibParam0;
            layerParam.calibParam1 = calibParam1;
            layerParam.threshold = threshold;
            layerParam.dataLengthPow = dataLengthPow;
            layerParam.refraction = refraction;
            layerParam.validDataNum = validDataNum;
        }

        public void SaveXml(XmlElement xmlElement)
        {
            XmlElement paramPropertyElement = xmlElement.OwnerDocument.CreateElement("", "LayerParam", "");
            xmlElement.AppendChild(paramPropertyElement);

            XmlHelper.SetValue(paramPropertyElement, "ParamName", paramName);
            XmlHelper.SetValue(paramPropertyElement, "WavelengthS", startWavelength.ToString());
            XmlHelper.SetValue(paramPropertyElement, "WavelengthE", endWavelength.ToString());
            XmlHelper.SetValue(paramPropertyElement, "ThicknessMin", minThickness.ToString());
            XmlHelper.SetValue(paramPropertyElement, "ThicknessMax", maxThickness.ToString());
            XmlHelper.SetValue(paramPropertyElement, "CalibParam0", calibParam0.ToString());
            XmlHelper.SetValue(paramPropertyElement, "CalibParam1", calibParam1.ToString());
            XmlHelper.SetValue(paramPropertyElement, "Threshold", threshold.ToString());
            XmlHelper.SetValue(paramPropertyElement, "DataLengthPow", dataLengthPow.ToString());
            XmlHelper.SetValue(paramPropertyElement, "Refraction", refraction.ToString());
            XmlHelper.SetValue(paramPropertyElement, "ValidDataNum", validDataNum.ToString());
        }

        public void LoadXml(XmlElement xmlElement)
        {
            XmlElement paramPropertyElement = xmlElement["LayerParam"];
            if (paramPropertyElement != null)
            {
                paramName = XmlHelper.GetValue(paramPropertyElement, "ParamName", "LayerParam 1");
                startWavelength = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "WavelengthS", "500"));
                endWavelength = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "WavelengthE", "2100"));
                minThickness = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "ThicknessMin", "0"));
                maxThickness = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "ThicknessMax", "50"));
                calibParam0 = Convert.ToSingle(XmlHelper.GetValue(paramPropertyElement, "CalibParam0", "0"));
                calibParam1 = Convert.ToSingle(XmlHelper.GetValue(paramPropertyElement, "CalibParam1", "1"));
                threshold = Convert.ToSingle(XmlHelper.GetValue(paramPropertyElement, "Threshold", "0"));
                dataLengthPow = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "DataLengthPow", "16"));
                refraction = Convert.ToSingle(XmlHelper.GetValue(paramPropertyElement, "Refraction", "1"));
                validDataNum = Convert.ToInt32(XmlHelper.GetValue(paramPropertyElement, "ValidDataNum", "0"));
            }
        }
    }
}
