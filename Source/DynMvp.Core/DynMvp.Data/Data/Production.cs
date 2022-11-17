using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Data
{
    public class Production
    {
        public Production()
        {
            Total = Ng = 0;
        }

        public virtual void Reset()
        {
            LotNo = "";
            TargetProduction = 0;
            Total = Ng = Pass = 0;
            LastUpdateTime = DateTime.Now;
        }
        public string LotNo { get; set; }
        public int Total { get; set; }
        public int Ng { get; set; }
        public int Pass { get; set; }

        public int Good => Total - Ng - Pass;

        public Production Clone()
        {
            var production = new Production();
            production.LastUpdateTime = LastUpdateTime;
            production.LotNo = LotNo;
            production.Ng = Ng;
            production.Pass = Pass;
            production.TargetProduction = TargetProduction;

            return production;
        }
        public DateTime LastUpdateTime { get; set; }
        public int TargetProduction { get; set; }

        public void ProduceNG()
        {
            Total++;
            Ng++;

            LastUpdateTime = DateTime.Now;
        }

        public void ProducePass()
        {
            Total++;
            Pass++;

            LastUpdateTime = DateTime.Now;
        }

        public void ProduceGood()
        {
            Total++;

            LastUpdateTime = DateTime.Now;
        }

        private bool IsDateChanged()
        {
            return (LastUpdateTime.Day != DateTime.Now.Day);
        }

        public void Load(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlElement productionElement = xmlDocument.DocumentElement;

            Load(productionElement);
        }

        public virtual void Load(XmlElement productionElement)
        {
            LotNo = XmlHelper.GetValue(productionElement, "LotNo", "");
            TargetProduction = Convert.ToInt32(XmlHelper.GetValue(productionElement, "TargetProduction", "0"));
            Total = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Total", "0"));
            Ng = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Ng", "0"));
            Pass = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Pass", "0"));
            string lastUpdateTimeStr = XmlHelper.GetValue(productionElement, "LastUpdateTime", "");
            if (lastUpdateTimeStr == "")
            {
                LastUpdateTime = DateTime.Now;
            }
            else
            {
                LastUpdateTime = Convert.ToDateTime(lastUpdateTimeStr);
            }
        }

        public void Save(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement productionElement = xmlDocument.CreateElement("", "Production", "");
            xmlDocument.AppendChild(productionElement);

            Save(productionElement);

            xmlDocument.Save(fileName);
        }

        public virtual void Save(XmlElement productionElement)
        {
            XmlHelper.SetValue(productionElement, "LotNo", LotNo);
            XmlHelper.SetValue(productionElement, "TargetProduction", TargetProduction.ToString());
            XmlHelper.SetValue(productionElement, "Total", Total.ToString());
            XmlHelper.SetValue(productionElement, "Ng", Ng.ToString());
            XmlHelper.SetValue(productionElement, "Pass", Pass.ToString());
            XmlHelper.SetValue(productionElement, "LastUpdateTime", LastUpdateTime.ToString());
        }
    }
}

