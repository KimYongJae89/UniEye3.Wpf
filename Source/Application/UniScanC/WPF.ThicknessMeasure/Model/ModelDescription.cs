using DynMvp.Base;
using System.Collections.Generic;
using System.Xml;
using WPF.ThicknessMeasure.Windows.ViewModels;

namespace WPF.ThicknessMeasure.Model
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        #region 생성자
        public ModelDescription() : base()
        {
            LayerParamList = new Dictionary<string, string>();
            ScanWidth = "";
            SensorType = "";
        }
        #endregion

        #region 속성
        public Dictionary<string, string> LayerParamList { get; set; }

        public string ScanWidth { get; set; }

        public string SensorType { get; set; }
        #endregion

        #region 메서드
        public override DynMvp.Data.ModelDescription Clone()
        {
            var modelDescription = new ModelDescription();

            modelDescription.Name = Name;
            modelDescription.SensorType = SensorType;

            return modelDescription;
        }

        public ModelWindowResult ConvertToModelWindowResult()
        {
            var modelResult = new ModelWindowResult();
            modelResult.Name = Name;
            modelResult.Description = Description;
            foreach (KeyValuePair<string, string> pair in LayerParamList)
            {
                modelResult.LayerParamList.Add(pair.Key, pair.Value);
            }

            modelResult.ScanWidth = ScanWidth;
            modelResult.SensorType = SensorType;

            return modelResult;
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);

            XmlElement thicknessElement = modelDescElement.OwnerDocument.CreateElement("", "Thickness", "");
            modelDescElement.AppendChild(thicknessElement);

            XmlElement layerParamElement = thicknessElement.OwnerDocument.CreateElement("", "LayerParam", "");
            thicknessElement.AppendChild(layerParamElement);
            foreach (string key in LayerParamList.Keys)
            {
                XmlHelper.SetValue(layerParamElement, key, LayerParamList[key]);
            }

            XmlHelper.SetValue(thicknessElement, "ScanWidth", ScanWidth);
            XmlHelper.SetValue(thicknessElement, "SensorType", SensorType);
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            XmlElement thicknessElement = modelDescElement["Thickness"];

            XmlElement layerParamElement = thicknessElement["LayerParam"];

            LayerParamList.Clear();
            foreach (XmlElement xmlElement in layerParamElement.ChildNodes)
            {
                LayerParamList.Add(xmlElement.Name, xmlElement.InnerText);
            }

            ScanWidth = XmlHelper.GetValue(thicknessElement, "ScanWidth", "");
            SensorType = XmlHelper.GetValue(thicknessElement, "SensorType", "");
        }
        #endregion
    }
}
