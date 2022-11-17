using Unieye.WPF.Base.Helpers;
using UniScanC.Data;

namespace UniScanC.Controls.Models
{
    public class DefectCountModel : Observable
    {
        private DefectCategory defectCategory;
        public DefectCategory DefectCategory
        {
            get => defectCategory;
            set => Set(ref defectCategory, value);
        }

        private double count;
        public double Count
        {
            get => count;
            set => Set(ref count, value);
        }

        public DefectCountModel(DefectCategory defectCategory)
        {
            DefectCategory = defectCategory;
        }
    }
}
