using DynMvp.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace UniScanC.Models
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        public Dictionary<string, string> CategoryDic { get; set; } = new Dictionary<string, string>();

        public override void CopyTo(DynMvp.Data.ModelDescription dstDesc)
        {
            base.CopyTo(dstDesc);

            if (dstDesc is ModelDescription dstDescUniScanC)
            {
                if (dstDescUniScanC.CategoryDic != null)
                {
                    dstDescUniScanC.CategoryDic.Clear();
                }
                else
                {
                    dstDescUniScanC.CategoryDic = new Dictionary<string, string>();
                }

                foreach (KeyValuePair<string, string> pair in CategoryDic)
                {
                    dstDescUniScanC.CategoryDic.Add(pair.Key, pair.Value);
                }
            }
        }

        public override void Save(string filePath)
        {
            string writeString = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, writeString);
        }

        public override void Save(XmlElement modelDescElement) { }
        public override void Save(ValueTable<string> valueTable) { }

        public override void Load(string fileName)
        {
            string readString = File.ReadAllText(fileName);
            ModelDescription modelDesc = JsonConvert.DeserializeObject<ModelDescription>(readString);
            modelDesc.CopyTo(this);
        }

        public override void Load(XmlElement modelDescElement) { }
        public override void Load(ValueTable<string> valueTable) { }
    }
}
