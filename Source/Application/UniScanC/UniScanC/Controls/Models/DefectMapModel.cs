using System.Collections.Generic;
using System.Drawing;
using UniScanC.Data;

namespace UniScanC.Controls.Models
{
    public class DefectMapChartModel
    {
        public Dictionary<string, List<PointF>> PointList { get; set; } = new Dictionary<string, List<PointF>>();
        public Dictionary<Defect, PointF> DicDefectMappingMap { get; set; } = new Dictionary<Defect, PointF>();

        public static DefectMapChartModel CreateModel(IEnumerable<InspectResult> InspectResults, IEnumerable<string> keys)
        {
            var model = new DefectMapChartModel();
            foreach (string key in keys)
            {
                model.PointList.Add(key, new List<PointF>());
            }

            // 데이터를 분할 정리해서 Dictionary에 넣는다
            foreach (InspectResult inspectResult in InspectResults)
            {
                foreach (Defect defect in inspectResult.DefectList)
                {
                    string categoryName = defect.DefectTypeName;
                    if (categoryName == null)
                    {
                        continue;
                    }

                    var point = new PointF();
                    point.X = defect.DefectPos.X;
                    point.Y = defect.DefectPos.Y;

                    if (model.PointList.TryGetValue(categoryName, out List<PointF> list))
                    {
                        list.Add(point);
                    }

                    if (model.DicDefectMappingMap.ContainsKey(defect) == false)
                    {
                        model.DicDefectMappingMap.Add(defect, point);
                    }
                }
            }
            return model;
        }
    }
}
