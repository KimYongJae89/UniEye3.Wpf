using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.InspectData
{
    public enum InspResultArchiverType
    {
        Text, Database
    }

    public class InspResultArchiverFactory
    {
        public InspResultArchiver Create(InspResultArchiverType type)
        {
            switch (type)
            {
                case InspResultArchiverType.Database:
                    return new DbInspResultArchiver();
                default:
                case InspResultArchiverType.Text:
                    return new TextInspResultArchiver();
            }
        }
    }

    public abstract class InspResultArchiver
    {
        public abstract void Save(ProductResult inspectionResult);
        public abstract List<ProductResult> Load(string dataPath, DateTime startDate, DateTime endDate);
        public abstract void GetProbeResult(ProductResult inspectionResult);
    }
}
