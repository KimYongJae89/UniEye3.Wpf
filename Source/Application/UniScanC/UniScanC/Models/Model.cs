using DynMvp.Data;
using DynMvp.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace UniScanC.Models
{
    public class Model : ModelBase
    {
        public int ModuleCount { get; set; } = 0;
        public List<VisionModel> VisionModels { get; set; } = new List<VisionModel>();
        //public SensorModel SensorModel { get; set; }

        public Model() { }

        public Model(int moduleCount)
        {
            ModuleCount = moduleCount;
        }

        public Model(Model model)
        {
            if (model != null)
            {
                CopyFrom(model);
            }
        }

        public override ModelBase Clone()
        {
            return new Model(this);
        }

        public override void CopyFrom(ModelBase modelBase)
        {
            base.CopyFrom(modelBase);
            var model = modelBase as Model;
            ModuleCount = model.ModuleCount;
            foreach (VisionModel visionModel in model.VisionModels)
            {
                VisionModels.Add(visionModel.Clone());
            }
            //this.SensorModel = model.SensorModel.Clone();
        }

        public override void BuildModel()
        {
            for (int index = 0; index < ModuleCount; index++)
            {
                VisionModels.Add(new VisionModel());
            }
            //this.SensorModel = new SensorModel();
        }

        public override bool OpenModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            string filePath = $"{modelPath}\\Model.json";
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            string readString = File.ReadAllText(filePath);
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            Model model = JsonConvert.DeserializeObject<Model>(readString, setting);
            CopyFrom(model);

            return true;
        }

        public bool OpenTeachModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            string filePath = $"{modelPath}\\Model_Teach.json";
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            string readString = File.ReadAllText(filePath);
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            Model model = JsonConvert.DeserializeObject<Model>(readString, setting);
            CopyFrom(model);

            return true;
        }

        public override void SaveModel(IReportProgress reportProgress = null)
        {
            string filePath = Path.GetFullPath($"{modelPath}\\Model.json");
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            string writeString = JsonConvert.SerializeObject(this, Formatting.Indented, setting);
            File.WriteAllText(filePath, writeString);
        }

        public void SaveTeachModel(IReportProgress reportProgress = null)
        {
            string filePath = Path.GetFullPath($"{modelPath}\\Model_Teach.json");
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            string writeString = JsonConvert.SerializeObject(this, Formatting.Indented, setting);
            File.WriteAllText(filePath, writeString);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override void Clear() { }
    }
}
