using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.Data
{
    public class Model : DynMvp.Data.StepModel
    {
        public override bool OpenModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            if (OperationConfig.Instance().UseAlgorithmPool)
            {
                string algorithmPoolFilePath = string.Format("{0}\\{1}\\AlgorithmPool.xml", PathConfig.Instance().Model, Name);
                if (File.Exists(algorithmPoolFilePath))
                {
                    AlgorithmPool.Instance().Load(algorithmPoolFilePath);
                }
            }

            ModelPath = string.Format("{0}\\{1}", PathConfig.Instance().Model, Name);

            base.OpenModel(reportProgress, CreateCustomInfo);

            return false;
        }

        public override void SaveModel(IReportProgress reportProgress = null)
        {
            try
            {
                if (OperationConfig.Instance().UseAlgorithmPool)
                {
                    string tempAlgorithmPoolFilePath = string.Format("{0}\\~AlgorithmPool.xml", ModelPath);
                    AlgorithmPool.Instance().Save(tempAlgorithmPoolFilePath);

                    string algorithmPoolFilePath = string.Format("{0}\\AlgorithmPool.xml", ModelPath);
                    string bakAlgorithmPoolFilePath = string.Format("{0}\\AlgorithmPool.xml.bak", ModelPath);
                    FileHelper.SafeSave(tempAlgorithmPoolFilePath, bakAlgorithmPoolFilePath, algorithmPoolFilePath);
                }

                base.SaveModel(reportProgress);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Model Save Error : " + ex.Message);
            }
        }
    }
}
