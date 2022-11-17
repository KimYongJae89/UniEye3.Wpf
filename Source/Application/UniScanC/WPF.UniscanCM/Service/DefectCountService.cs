using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.Data;
using UniScanC.Enums;

namespace WPF.UniScanCM.Service
{
    public static class DefectCountService
    {
        public static float TargetLengthM { get; set; } = 1000;

        private static Dictionary<EDefectType, int> DefectCount { get; set; } = new Dictionary<EDefectType, int>();
        private static float InspectLength { get; set; } = 0;

        public static void CountDefect(List<InspectResult> inspectResults, float inspectLength)
        {
            lock (DefectCount)
            {
                foreach (InspectResult result in inspectResults)
                {
                    foreach (Defect defect in result.DefectList)
                    {
                        if (!DefectCount.ContainsKey(defect.DefectType))
                        {
                            DefectCount.Add(defect.DefectType, 0);
                        }

                        DefectCount[defect.DefectType]++;
                    }
                }
            }

            InspectLength = inspectLength;
        }

        public static float GetDefectCount(EDefectType defectType, bool useTargetLength)
        {
            if (DefectCount.ContainsKey(defectType))
            {
                if (useTargetLength)
                {
                    if (TargetLengthM > 0)
                    {
                        return DefectCount[defectType] / TargetLengthM;
                    }
                    else
                    {
                        return DefectCount[defectType];
                    }
                }
                else
                {
                    return DefectCount[defectType];
                }
            }
            else
            {
                return 0;
            }
        }

        public static float GetAllDefectCount(bool useTargetLength)
        {
            if (useTargetLength)
            {
                if (TargetLengthM > 0)
                {
                    return DefectCount.Sum(x => x.Value) / TargetLengthM;
                }
                else
                {
                    return DefectCount.Sum(x => x.Value);
                }
            }
            else
            {
                return DefectCount.Sum(x => x.Value);
            }
        }

        public static void ClearCount()
        {
            DefectCount.Clear();
        }
    }
}
