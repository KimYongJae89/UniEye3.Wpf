using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.Data;

namespace UniScanC.Controls.Models
{
    public class StatisticsLotDefectModel
    {
        public Dictionary<string, int> LotDefectDic { get; set; }

        public static StatisticsLotDefectModel CreateModel(IEnumerable<InspectResult> inspectResults)
        {
            var model = new StatisticsLotDefectModel();
            model.LotDefectDic = new Dictionary<string, int>();

            foreach (InspectResult result in inspectResults)
            {
                int defectCount = result.DefectList.Count();
                if (model.LotDefectDic.ContainsKey(result.LotNo))
                {
                    model.LotDefectDic[result.LotNo] += defectCount;
                }
                else
                {
                    model.LotDefectDic.Add(result.LotNo, defectCount);
                }
            }

            return model;
        }
    }
}
