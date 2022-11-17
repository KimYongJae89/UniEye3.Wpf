using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using UniScanC.Data;

namespace UniScanC.Controls.Models
{
    public class ThumbnailModel
    {
        public int Number { get; set; } = 0;
        public ImageSource ImageSource { get; set; } = null;
        public Defect Defect { get; set; } = null;

        public static List<ThumbnailModel> CreateModel(IEnumerable<InspectResult> inspectResults, CancellationTokenSource taskCancelToken)
        {
            var modelList = new List<ThumbnailModel>();

            foreach (InspectResult result in inspectResults)
            {
                foreach (Defect defect in result.DefectList)
                {
                    var model = new ThumbnailModel();
                    model.Number = defect.DefectNo;
                    model.ImageSource = defect.DefectImage;
                    model.Defect = defect;
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
