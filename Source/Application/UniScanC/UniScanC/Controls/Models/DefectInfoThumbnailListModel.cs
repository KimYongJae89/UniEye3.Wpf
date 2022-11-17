using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniScanC.Data;
using UniScanC.Models;

namespace UniScanC.Controls.Models
{
    public class DefectInfoThumbnailListModel
    {
        public Defect Defect { get; set; }
        public DefectCategory DefectCategory { get; set; }

        public static IEnumerable<DefectInfoThumbnailListModel> CreateModel(IEnumerable<Defect> defects, Model model, CancellationTokenSource taskCancelToken = null)
        {
            if (defects == null || model == null)
            {
                return null;
            }

            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in model.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }

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

            defectCategories.Add(DefectCategory.GetDefaultCategory());
            defectCategories.Add(DefectCategory.GetColorCategory());

            var modelList = new List<DefectInfoThumbnailListModel>();
            foreach (Defect defect in defects)
            {
                var defectInfoThumbnailListModel = new DefectInfoThumbnailListModel();
                defectInfoThumbnailListModel.Defect = defect;
                defectInfoThumbnailListModel.DefectCategory = defectCategories.FirstOrDefault(x => x.Name == defect.DefectTypeName);
                modelList.Add(defectInfoThumbnailListModel);
                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return modelList;
        }
    }
}
