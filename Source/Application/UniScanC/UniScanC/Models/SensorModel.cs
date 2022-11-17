using Newtonsoft.Json;
using System.IO;

namespace UniScanC.Models
{
    public class SensorModel
    {
        public string PetParamName { get; set; } = "0";
        public string SheetParamName { get; set; } = "0";
        public string ScanWidthName { get; set; } = "0";

        public double ThicknessTarget { get; set; } = 0;

        public ThicknessLayerParam PetParam { get; set; } = null;
        public ThicknessLayerParam SheetParam { get; set; } = null;
        public ScanWidth ScanWidth { get; set; } = null;


        public SensorModel() { }

        public SensorModel(SensorModel sensorModel)
        {
            if (sensorModel != null)
            {
                CopyFrom(sensorModel);
            }
        }

        public SensorModel Clone()
        {
            var model = new SensorModel();
            model.CopyFrom(this);

            return model;
        }

        public void CopyFrom(SensorModel sensorModel)
        {
            PetParamName = sensorModel.PetParamName;
            SheetParamName = sensorModel.SheetParamName;
            ScanWidthName = sensorModel.ScanWidthName;
            ThicknessTarget = sensorModel.ThicknessTarget;
            PetParam = sensorModel.PetParam;
            SheetParam = sensorModel.SheetParam;
            ScanWidth = sensorModel.ScanWidth;
        }

        public bool Load(string modelPath)
        {
            string filePath = $"{modelPath}\\SensorModel.xml";
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            string readString = File.ReadAllText(filePath);
            SensorModel model = JsonConvert.DeserializeObject<SensorModel>(readString);
            CopyFrom(model);

            return true;
        }

        public void Save(string modelPath)
        {
            string filePath = $"{modelPath}\\SensorModel.xml";
            string writeString = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, writeString);
        }
    }
}
