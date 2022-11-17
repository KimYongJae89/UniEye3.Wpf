using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniScanC.Data;
using UniScanC.Module;

namespace UniScanC.Controls.Models
{
    public class DefectSummaryModel
    {
        public int Number { get; set; } = 0;
        public double Length { get; set; } = 0;
        public string DefectType { get; set; } = "";
        public double Area { get; set; } = 0;
        public double AvgGv { get; set; } = 0;
        public double MinGv { get; set; } = 0;
        public double MaxGv { get; set; } = 0;
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public double PosX { get; set; } = 0;
        public double PosY { get; set; } = 0;
        public Defect Defect { get; set; } = null;

        public static List<DefectSummaryModel> CreateModel(IEnumerable<InspectResult> inspectResults, CancellationTokenSource taskCancelToken = null)
        {
            var modelList = new List<DefectSummaryModel>();
            foreach (InspectResult result in inspectResults)
            {
                foreach (Defect defect in result.DefectList)
                {
                    var model = new DefectSummaryModel();
                    model.Number = defect.DefectNo;
                    model.DefectType = defect.DefectTypeName;
                    model.Area = defect.Area;
                    model.AvgGv = defect.AvgGv;
                    model.MinGv = defect.MinGv;
                    model.MaxGv = defect.MaxGv;
                    model.Width = defect.BoundingRect.Width;
                    model.Height = defect.BoundingRect.Height;
                    model.PosX = defect.DefectPos.X;
                    model.PosY = defect.DefectPos.Y;
                    model.Defect = defect;
                    model.Length = ((result.FrameIndex + 1) * result.InspectRegion.Height) / 1000;

                    modelList.Add(model);
                    if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }
            return modelList;
        }
    }
}
