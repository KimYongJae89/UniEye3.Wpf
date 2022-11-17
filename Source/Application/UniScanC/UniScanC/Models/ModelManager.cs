using DynMvp.Data;
using DynMvp.UI;
using System.IO;

namespace UniScanC.Models
{
    public class ModelManager : DynMvp.Data.ModelManager
    {
        public static new ModelManager Instance()
        {
            return _instance as ModelManager;
        }

        public ModelManager(DynMvp.Data.ModelBuilder modelBuilder, string modelPath = null) : base(modelBuilder)
        {
            ModelPath = modelPath;
        }

        public override void SaveModelDescription(DynMvp.Data.ModelDescription modelDesc)
        {
            string modelPath = GetModelPath(modelDesc.Name);
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = $"{modelPath}\\ModelDescription.json";
            modelDesc.Save(filePath);
        }

        public override void SaveModelDescription(ModelBase model)
        {
            string modelPath = model.ModelPath;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = $"{modelPath}\\ModelDescription.json";
            model.ModelDescription.Save(filePath);
        }

        public override DynMvp.Data.ModelDescription LoadModelDescription(string modelName)
        {
            string modelPath = GetModelPath(modelName);
            string filePath = $"{modelPath}\\ModelDescription.json";

            DynMvp.Data.ModelDescription modelDesc = modelBuilder.CreateModelDescription();
            modelDesc.Load(filePath);

            return modelDesc;
        }

        public override void CloseModel()
        {
            if (currentModel == null)
            {
                return;
            }

            ModelClosed(CurrentModel);
            currentModel = null;
        }

        public ModelBase OpenTeachModel(string modelName, IReportProgress reportProgress)
        {
            DynMvp.Data.ModelDescription modelDesc = GetModelDescription(modelName);
            if (modelDesc == null)
            {
                return null;
            }

            return OpenTeachModel(modelDesc, reportProgress);
        }

        public override ModelBase OpenModel(DynMvp.Data.ModelDescription modelDesc, IReportProgress reportProgress)
        {
            CloseModel();
            currentModel = null;

            ModelBase model = modelBuilder.CreateModel();
            model.ModelPath = GetModelPath(modelDesc.Name);
            model.ModelDescription = modelDesc;

            if (model.OpenModel(reportProgress) == false)
            {
                model.BuildModel();
            }

            currentModel = model;
            ModelOpened(currentModel);

            return currentModel;
        }

        public ModelBase OpenTeachModel(DynMvp.Data.ModelDescription modelDesc, IReportProgress reportProgress)
        {
            CloseModel();
            currentModel = null;
            ModelBase modelBase = modelBuilder.CreateModel();
            if (modelBase is Model model)
            {
                model.ModelPath = GetModelPath(modelDesc.Name);
                model.ModelDescription = modelDesc;

                if (model.OpenTeachModel(reportProgress) == false)
                {
                    model.BuildModel();
                }

            }
            currentModel = modelBase;
            ModelOpened(currentModel);

            return currentModel;
        }

        #region Custom Method 리포트 기능을 위한 모델을 저장하고 불러오는기 기능
        public void SaveModelDescription(string modelPath)
        {
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = $"{modelPath}\\ModelDescription.json";
            CurrentModel.ModelDescription.Save(filePath);
        }

        public ModelDescription LoadCopiedModelDescription(string modelPath)
        {
            var modelDescription = new ModelDescription();
            string filePath = $"{modelPath}\\ModelDescription.json";
            modelDescription.Load(filePath);
            return modelDescription;
        }

        public void SaveModel(string modelPath)
        {
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            var model = CurrentModel as Model;
            string originModelPath = model.ModelPath;
            model.ModelPath = modelPath;
            model.SaveModel();
            model.ModelPath = originModelPath;
        }

        public Model LoadCopiedModel(string modelPath)
        {
            if (!Directory.Exists(modelPath))
            {
                return null;
            }

            var model = new Model();
            model.ModelPath = modelPath;
            model.OpenModel(null);
            return model;
        }
        #endregion
    }
}
