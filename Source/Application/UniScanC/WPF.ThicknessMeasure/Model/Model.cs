using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace WPF.ThicknessMeasure.Model
{
    public class Model : DynMvp.Data.ModelBase
    {
        #region 메서드
        public override void BuildModel()
        {

        }

        public override void Clear()
        {

        }

        public override ModelBase Clone()
        {
            var thicknessModel = new Model();
            thicknessModel.CopyFrom(this);

            return thicknessModel;
        }

        public override void CopyFrom(ModelBase model)
        {
            base.CopyFrom(model);
        }

        public override bool OpenModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            base.OpenModel(reportProgress, CreateCustomInfo);

            string modelFilePath = Path.Combine(ModelPath, "Model.xml");
            if (File.Exists(modelFilePath) == false)
            {
                SaveModel();
                return false;
            }

            string readString = File.ReadAllText(modelFilePath);
            Model model = JsonConvert.DeserializeObject<Model>(readString);

            CopyFrom(model);

            return true;
        }

        public override void SaveModel(IReportProgress reportProgress = null)
        {
            base.SaveModel(reportProgress);

            string tempModelFilePath = string.Format("{0}\\~Model.xml", ModelPath);

            string writeString = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(tempModelFilePath, writeString);

            string modelFilePath = string.Format("{0}\\Model.xml", ModelPath);
            string bakModelFilePath = string.Format("{0}\\Model.xml.bak", ModelPath);

            FileHelper.SafeSave(tempModelFilePath, bakModelFilePath, modelFilePath);
        }

        public override bool IsEmpty()
        {
            return false;
        }
        #endregion
    }
}
