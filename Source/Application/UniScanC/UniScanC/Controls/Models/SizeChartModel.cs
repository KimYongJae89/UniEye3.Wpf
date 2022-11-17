using System;
using System.Collections.Generic;
using System.Linq;
using UniScanC.Data;

namespace UniScanC.Controls.Models
{
    public class SizeChartModel
    {
        public Dictionary<string, int> SizeCountDic { get; set; }

        public static SizeChartModel CreateModel(IEnumerable<InspectResult> inspectResults, int[] sizeArray)
        {
            var model = new SizeChartModel();
            model.SizeCountDic = new Dictionary<string, int>();

            foreach (InspectResult result in inspectResults)
            {
                int defectCount = result.DefectList.Count();
                if (defectCount == 0)
                {
                    continue;
                }

                for (int i = 0; i < sizeArray.Count(); i++)
                {
                    int maxSize = sizeArray[i];
                    if (!model.SizeCountDic.ContainsKey(maxSize.ToString()))
                    {
                        model.SizeCountDic.Add(maxSize.ToString(), 0);
                    }

                    if (maxSize != -1)
                    {
                        int minSize = 0;
                        if (i != 0)
                        {
                            minSize = sizeArray[i - 1];
                        }

                        int searchCount = result.DefectList.Count(x =>
                        {
                            // 불량 정보는 mm단위이기 때문에 um단위로 변경하여 비교한다.
                            double max = Math.Max(x.BoundingRect.Width, x.BoundingRect.Height) * 1000;
                            return max >= minSize && max < maxSize;
                        });

                        model.SizeCountDic[maxSize.ToString()] += searchCount;
                        defectCount -= searchCount;
                    }
                    else
                    {
                        model.SizeCountDic[maxSize.ToString()] += defectCount;
                    }
                }
            }

            return model;
        }
    }
}
