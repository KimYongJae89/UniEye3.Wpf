using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DynMvp.Data
{
    public class ModelBuilder
    {
        public virtual ModelBase CreateModel()
        {
            return new StepModel();
        }

        public virtual ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }
    }

    public delegate void ModelOpenDelegate(ModelBase model);
    public delegate void ModelCloseDelegate(ModelBase model);
    public interface IModelEventListener
    {
        void ModelListChanged();
        void ModelOpen(ModelBase model);
        void ModelClosed(ModelBase model);
    }

    public class ModelManager
    {
        public string ModelPath { get; set; }

        protected ModelBase currentModel;
        public ModelBase CurrentModel
        {
            get => currentModel;
            set => currentModel = value;
        }

        public StepModel CurrentStepModel => currentModel as StepModel;
        public HashSet<string> CategoryList { get; } = new HashSet<string>();
        public List<ModelDescription> ModelDescriptionList { get; } = new List<ModelDescription>();

        public int Count => ModelDescriptionList.Count;

        protected ModelBuilder modelBuilder = new ModelBuilder();
        public ModelBuilder ModelBuilder
        {
            set => modelBuilder = value;
            get => modelBuilder;
        }
        public string LastResultPath { get; set; }

        private List<IModelEventListener> listenerList = new List<IModelEventListener>();

        protected static ModelManager _instance = null;
        public static ModelManager Instance()
        {
            return _instance;
        }

        public static void SetInstance(ModelManager modelManager)
        {
            _instance = modelManager;
        }

        public ModelManager(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void AddListener(IModelEventListener listener)
        {
            listenerList.Add(listener);
        }

        public void ModelClosed(ModelBase model)
        {
            listenerList.ForEach(x => x.ModelClosed(model));
        }

        public void ModelOpened(ModelBase model)
        {
            listenerList.ForEach(x => x.ModelOpen(model));
        }

        public void ModelListChanged()
        {
            listenerList.ForEach(x => x.ModelListChanged());
        }

        public void Init(string modelPath)
        {
            ModelPath = modelPath;
        }

        public IEnumerator<ModelDescription> GetEnumerator()
        {
            return ModelDescriptionList.GetEnumerator();
        }

        public virtual void AddModel(ModelDescription modelDescription)
        {
            ModelDescriptionList.Add(modelDescription);
            SaveModelDescription(modelDescription);

            ModelListChanged();
        }

        public void EditModel(ModelDescription modelDescription)
        {
            SaveModelDescription(modelDescription);

            if (currentModel != null && currentModel.Name == modelDescription.Name)
            {
                CloseModel();
            }
        }

        public virtual void DeleteModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName) == true)
            {
                return;
            }

            if (currentModel != null && currentModel.Name == modelName)
            {
                CloseModel();
            }

            ModelDescription md = GetModelDescription(modelName);
            if (md != null)
            {
                ModelDescriptionList.Remove(md);

                string modelPath = GetModelPath(md.Name);

                Directory.Delete(modelPath, true);
            }

            ModelListChanged();
        }

        public virtual void CopyModelDescription(ModelDescription srcDesc, ModelDescription destDesc)
        {

        }

        public virtual void CopyModelData(string srcModelName, string destModelName)
        {
            string srcModelPath = GetModelPath(srcModelName);
            string destModelPath = GetModelPath(destModelName);

            Directory.CreateDirectory(destModelPath);

            string srcModelFileName = srcModelPath + "\\model.xml";
            if (File.Exists(srcModelFileName) == true)
            {
                File.Copy(srcModelFileName, destModelPath + "\\model.xml", true);
            }

            string srcModelSchemaFileName = srcModelPath + "\\modelSchema.xml";
            if (File.Exists(srcModelSchemaFileName) == true)
            {
                File.Copy(srcModelSchemaFileName, destModelPath + "\\modelSchema.xml", true);
            }

            string srcImagePathName = srcModelPath + "\\Image";
            if (Directory.Exists(srcImagePathName) == true)
            {
                string destImagePath = destModelPath + "\\Image";
                Directory.CreateDirectory(destImagePath);
                FileHelper.CopyDirectory(srcImagePathName, destImagePath);
            }

            ModelListChanged();
        }

        public bool IsModelExist(string name)
        {
            foreach (ModelDescription m in ModelDescriptionList)
            {
                if (m.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public int NewModelExistCount(string name)
        {
            int count;
            int countLabel = 1;
            foreach (ModelDescription m in ModelDescriptionList)
            {
                //if (m.Name.Substring(0, 3) == name)
                //{
                countLabel++;
                //}
            }
            count = countLabel;

            return count;
        }

        public virtual ModelDescription GetModelDescription(string name)
        {
            foreach (ModelDescription m in ModelDescriptionList)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }

            return null;
        }

        public virtual void Refresh(string modelPath = null)
        {
            if (modelPath == null)
            {
                modelPath = ModelPath;
            }

            if (modelPath == null)
            {
                return;
            }

            var modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            ModelPath = modelPath;

            ModelDescriptionList.Clear();

            DirectoryInfo[] dirList = modelRootDir.GetDirectories();

            foreach (DirectoryInfo modelDir in dirList)
            {
                ModelDescription modelDescription = LoadModelDescription(modelDir.Name);
                modelDescription.Name = modelDir.Name;
                ModelDescriptionList.Add(modelDescription);

                if (string.IsNullOrEmpty(modelDescription.Category) == false)
                {
                    CategoryList.Add(modelDescription.Category);
                }
            }

            ModelListChanged();
        }

        public string GetModelPath(string modelName)
        {
            return string.Format("{0}\\{1}", ModelPath, modelName);
        }

        public virtual ModelDescription LoadModelDescription(string modelName)
        {
            string filePath = string.Format("{0}\\ModelDescription.xml", GetModelPath(modelName));

            ModelDescription modelDesc = modelBuilder.CreateModelDescription();
            modelDesc.Load(filePath);

            return modelDesc;
        }

        public virtual void SaveModelDescription(ModelDescription modelDesc)
        {
            string modelPath = GetModelPath(modelDesc.Name);
            if (Directory.Exists(modelPath) == false)
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = string.Format("{0}\\ModelDescription.xml", modelPath);
            modelDesc.Save(filePath);
        }

        public virtual void SaveModelDescription(ModelBase model)
        {
            string modelPath = model.ModelPath;
            if (Directory.Exists(modelPath) == false)
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = string.Format("{0}\\ModelDescription.xml", modelPath);
            model.ModelDescription.Save(filePath);
        }

        public virtual ModelBase OpenModel(string modelName, IReportProgress reportProgress)
        {
            ModelDescription modelDesc = GetModelDescription(modelName);
            if (modelDesc == null)
            {
                return null;
            }

            return OpenModel(modelDesc, reportProgress);
        }

        public virtual ModelBase CreateModel(ModelDescription modelDesc)
        {
            if (modelDesc == null)
            {
                return null;
            }

            ModelBase model = modelBuilder.CreateModel();
            if (model == null)
            {
                return null;
            }

            model.ModelPath = GetModelPath(modelDesc.Name);
            model.ModelDescription = modelDesc;

            model.SaveModel();

            return model;
        }

        public virtual ModelBase LoadModel(string modelName)
        {
            ModelDescription desc = LoadModelDescription(modelName);
            return LoadModel(desc);
        }

        public virtual ModelBase LoadModel(ModelDescription modelDesc)
        {
            if (modelDesc == null)
            {
                return null;
            }

            ModelBase model = modelBuilder.CreateModel();
            if (model == null)
            {
                return null;
            }

            model.ModelPath = GetModelPath(modelDesc.Name);
            model.ModelDescription = modelDesc;

            if (model.OpenModel(null) == false)
            {
                model.BuildModel();
            }

            return model;
        }

        public virtual ModelBase ImportModel(string modelName, string modelDescData, string modelData)
        {
            ModelDescription modelDesc = GetModelDescription(modelName);
            if (modelDesc == null)
            {
                modelDesc = CreateModelDescription();
                modelDesc.Name = modelName;
            }

            var modelDescValueTable = new ValueTable<string>();
            modelDescValueTable.FromString(modelDescData);

            modelDesc.Load(modelDescValueTable);

            ModelBase model = modelBuilder.CreateModel();
            if (model == null)
            {
                return null;
            }

            model.ModelPath = GetModelPath(modelDesc.Name);
            model.ModelDescription = modelDesc;

            var modelValueTable = new ValueTable<string>();
            modelValueTable.FromString(modelData);
            model.Import(modelValueTable);

            model.SaveModel();

            currentModel = model;

            ModelOpened(currentModel);

            return model;
        }

        public virtual bool ExportModel(out string modelDescData, out string modelData)
        {
            if (currentModel == null)
            {
                modelDescData = "";
                modelData = "";
                return false;
            }

            var modelValueTable = new ValueTable<string>();
            currentModel.Export(modelValueTable);
            modelData = modelValueTable.ToString();

            var modelDescValueTable = new ValueTable<string>();
            currentModel.ModelDescription.Save(modelDescValueTable);
            modelDescData = modelValueTable.ToString();

            return true;
        }

        public virtual ModelBase OpenModel(ModelDescription modelDesc, IReportProgress reportProgress)
        {
            CloseModel();

            currentModel = null;

            var loadingForm = new SimpleProgressForm("Loading...");
            loadingForm.Show(new Action(() =>
            {
                ModelBase model = modelBuilder.CreateModel();
                if (model == null)
                {
                    return;
                }

                model.ModelPath = GetModelPath(modelDesc.Name);
                model.ModelDescription = modelDesc;

                if (model.OpenModel(reportProgress) == false)
                {
                    model.BuildModel();
                }

                currentModel = model;

                ModelOpened(currentModel);
            }));

            return currentModel;
        }

        public virtual void CloseModel()
        {
            if (currentModel == null)
            {
                return;
            }

            if (currentModel.Modified)
            {
                DialogResult result = MessageForm.Show(null, string.Format("Do you want to save the current model[{0}]?", currentModel.Name), MessageFormType.YesNo);
                if (result == DialogResult.Yes)
                {
                    currentModel.SaveModel();
                }
            }

            ModelClosed(currentModel);

            currentModel = null;
        }

        public ModelBase CreateModel()
        {
            return modelBuilder?.CreateModel();
        }

        public ModelDescription CreateModelDescription()
        {
            return modelBuilder?.CreateModelDescription();
        }

        public bool IsCurrentModel(string modelName)
        {
            return (currentModel != null && currentModel.Name == modelName);
        }

        public string GetImagePath()
        {
            string imagePath;

            if (string.IsNullOrEmpty(LastResultPath) == false && Directory.Exists(LastResultPath) && (Directory.GetFiles(LastResultPath, @"*.bmp").Count() > 0))
            {
                imagePath = LastResultPath;
            }
            else
            {
                imagePath = Path.Combine(currentModel.ModelPath, "Image");
            }

            imagePath = Path.GetFullPath(imagePath);
            return imagePath;
        }
    }
}
