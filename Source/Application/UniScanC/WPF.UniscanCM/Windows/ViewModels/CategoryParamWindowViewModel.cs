using MahApps.Metro.SimpleChildWindow;
using System.Collections.Generic;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Data;
using UniScanC.Models;
using WPF.UniScanCM.Windows.Models;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class CategoryParamWindowViewModel : Observable
    {
        private IEnumerable<DefectTypeParameterModel> defectCategories;
        public IEnumerable<DefectTypeParameterModel> DefectCategories
        {
            get => defectCategories;
            set => Set(ref defectCategories, value);
        }

        public ICommand CloseCommand { get; }

        public CategoryParamWindowViewModel()
        {
            var model = ModelManager.Instance().CurrentModel as UniScanC.Models.Model;
            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in model.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }
                }
            }

            DefectCategories = DefectTypeParameterModel.CreateModel(defectCategories);

            CloseCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(false));
        }
    }
}
