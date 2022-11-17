using DynMvp.Base;
using DynMvp.InspectData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniEye.Translation.Helpers;
using UniScanC.Data;
using UniScanC.Enums;

namespace WPF.UniScanCM.Service
{
    // 일정 구간 연속된 불량이 나올 시에 알람을 띄우려고 했던 기능
    // 현재 쓰이지는 않음
    public static class DefectAlarmService
    {
        private static float DetectLengthRangeM { get; set; } = 1; // m
        private static float LineDefectHeightMm { get; set; } = 50; // mm
        private static int DefectCount { get; set; } = 5; // ea
        private static List<InspectResult> subTotalResultList = new List<InspectResult>();

        public static void SetParam(float lengthRangeM, float defectHeightMm, int defectCount)
        {
            DetectLengthRangeM = lengthRangeM;
            LineDefectHeightMm = defectHeightMm;
            DefectCount = defectCount;
        }

        public static void ClearData()
        {
            lock (subTotalResultList)
            {
                subTotalResultList.Clear();
            }
        }

        public static bool HasDefectType(IEnumerable<InspectResult> InspectResultList, EDefectType defectType)
        {
            return InspectResultList.Count(result => result.DefectList.Count(defect => defect.DefectType == defectType) > 1) > 1;
        }

        public static void CheckCustomDefect(IEnumerable<ProductResult> productResults, double lastInspectLength)
        {
            lock (subTotalResultList)
            {
                // 일정 영역만큼 데이터를 가지고 있는다
                IEnumerable<InspectResult> inspectResults = productResults.Where(x => x is InspectResult).Cast<InspectResult>();
                subTotalResultList.AddRange(inspectResults);
                float heightValue = subTotalResultList.Last().InspectRegion.Height / 1000f;
                subTotalResultList = subTotalResultList.Where(x => x.FrameIndex * heightValue > lastInspectLength - DefectAlarmService.DetectLengthRangeM).ToList();

                // 마지막에서 일정 거리에 비슷한 위치의 불량이 지속될 경우 알람을 띄운다.
                IEnumerable<InspectResult> noneSkipResultList = subTotalResultList.Where(x => x.DefectList.Count(y => y.IsSkip == false) > 0);
                if (noneSkipResultList != null && noneSkipResultList.Count() > 0)
                {
                    HasCustomDefect(noneSkipResultList);
                }
            }
        }

        private static void HasCustomDefect(IEnumerable<InspectResult> InspectResultList)
        {
            Task.Run(() =>
            {
                var dustDefectList = new List<Defect>();

                foreach (InspectResult InspectResult in InspectResultList)
                {
                    foreach (Defect dustDefect in InspectResult.DefectList)
                    {
                        dustDefectList.Add(dustDefect);
                    }
                }

                foreach (Defect dustDefect in dustDefectList)
                {
                    List<Defect> findDefects = dustDefectList.FindAll(x => x.BoundingRect.Height >= LineDefectHeightMm);
                    if (findDefects != null && findDefects.Count() > 0)
                    {
                        subTotalResultList.Clear();
                        ErrorManager.Instance().Report((int)ErrorSection.Inspect, ErrorLevel.Warning, ErrorSection.Inspect.ToString(),
                            "Line Defect", string.Format($"{TranslationHelper.Instance.Translate("LINE_DEFECT_OCCURRED_MESSAGE")}"));
                        return;
                    }
                }

                if (dustDefectList.Count >= DefectCount)
                {
                    subTotalResultList.Clear();
                    ErrorManager.Instance().Report((int)ErrorSection.Inspect, ErrorLevel.Warning, ErrorSection.Inspect.ToString(),
                            "Defect", string.Format($"{TranslationHelper.Instance.Translate("DEFECT_OCCURRED_MESSAGE")}" + " : [{0}]", DefectCount));

                    return;
                }
            });
        }
    }
}

//// Keep
//int count = dustDefectList.Count(x => dustDefect.ModuleNo == x.ModuleNo &&
//                                      dustDefect.DefectPos.X - DefectHeight < x.DefectPos.X &&
//                                      dustDefect.DefectPos.X + DefectHeight > x.DefectPos.X);

//if (count > DefectCount)
//{
//    ErrorManager.Instance().Report((int)ErrorSection.Inspect, ErrorLevel.Warning, ErrorSection.Inspect.ToString(),
//        "Line Defect", string.Format("Line Defect Occurred", DefectCount));

//    return;
//}
