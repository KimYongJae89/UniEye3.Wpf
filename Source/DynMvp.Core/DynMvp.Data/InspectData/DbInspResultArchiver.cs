using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.InspectData
{
    public class DbInspResultArchiver : InspResultArchiver
    {
        public override void GetProbeResult(ProductResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public override List<ProductResult> Load(string dataPath, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public override void Save(ProductResult inspectionResult)
        {
            throw new NotImplementedException();
        }
    }
}
