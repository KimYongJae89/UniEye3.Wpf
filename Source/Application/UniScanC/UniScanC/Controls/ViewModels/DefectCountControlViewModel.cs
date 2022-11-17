using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Windows.Views;

namespace UniScanC.Controls.ViewModels
{
    public class DefectCountControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public DefectCountControlViewModel() : base(typeof(DefectCountControlViewModel))
        {
            CategorySelectCommand = new RelayCommand<DefectCountModel>(CategorySelectCommandAction);
        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private int maxImageCount = 100;
        [LayoutControlViewModelPropertyAttribute]
        public int MaxImageCount
        {
            get => maxImageCount;
            set => Set(ref maxImageCount, value);
        }
        #endregion


        #region 속성
        public ICommand CategorySelectCommand { get; }

        private int totalDefectCount = 0;
        public int TotalDefectCount
        {
            get => totalDefectCount;
            set => Set(ref totalDefectCount, value);
        }

        private List<DefectCountModel> defectCountModels = new List<DefectCountModel>();
        public List<DefectCountModel> DefectCountModels
        {
            get => defectCountModels;
            set => Set(ref defectCountModels, value);
        }

        private List<Defect> TotalDefects { get; set; } = new List<Defect>();

        private int FirstImageIndex { get; set; } = 0;
        #endregion


        #region 메서드
        private void CategorySelectCommandAction(DefectCountModel defectCountModel)
        {
            if (defectCountModel == null)
            {
                return;
            }

            List<Defect> filtteredDefects = TotalDefects.FindAll(x => x.DefectTypeName == defectCountModel.DefectCategory.Name);
            var view = new DefectTypeDetailWindow(filtteredDefects, defectCountModel.DefectCategory);
            view.ShowDialog();
        }

        public void OnUpdateModel(ModelBase modelBase)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateModel(modelBase)));
                return;
            }
            UpdateModel(modelBase);
        }

        public void UpdateModel(ModelBase modelBase)
        {
            ClearCount();
            DefectCountModels.Clear();

            var model = modelBase as Model;
            var defectCountModels = new List<DefectCountModel>();
            foreach (VisionModel visionModel in model.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCountModels.Find(x => x.DefectCategory.Name == category.Name) == null)
                    {
                        defectCountModels.Add(new DefectCountModel(new DefectCategory(category)));
                    }
                }
            }

            defectCountModels.Add(new DefectCountModel(DefectCategory.GetDefaultCategory()));
            defectCountModels.Add(new DefectCountModel(DefectCategory.GetColorCategory()));

            DefectCountModels = defectCountModels;
        }

        public void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateResult(productResults, taskCancelToken)));
                return;
            }
            UpdateResult(productResults, taskCancelToken);
        }

        public void UpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            if (productResults == null)
            {
                ClearCount();
                return;
            }

            var filteredList = new List<InspectResult>();
            foreach (ProductResult productResult in productResults)
            {
                if (productResult is InspectResult inspectResult)
                {
                    filteredList.Add(inspectResult);
                }
            }

            if (filteredList.Count == 0)
            {
                return;
            }

            var defects = filteredList.SelectMany(x => x.DefectList).ToList();

            if (MaxImageCount > 0)
            {
                int newDefectsCount = defects.Count();
                int totalDefectsCount = TotalDefects.Count();
                int targetIndex = Math.Min(totalDefectsCount - 1, FirstImageIndex + newDefectsCount);
                for (int i = FirstImageIndex; i < targetIndex; i++)
                {
                    TotalDefects[i].DefectImage = null;
                    if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
                for (int i = 0; i < Math.Min(newDefectsCount, newDefectsCount - MaxImageCount); i++)
                {
                    defects[i].DefectImage = null;
                    if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            TotalDefects.AddRange(defects);
            TotalDefectCount = TotalDefects.Count();

            if (MaxImageCount > 0)
            {
                FirstImageIndex = Math.Max(0, TotalDefects.Count() - MaxImageCount);
            }

            foreach (DefectCountModel defectCountmodel in DefectCountModels)
            {
                int count = defects.Count(x => x.DefectTypeName == defectCountmodel.DefectCategory.Name);
                defectCountmodel.Count += count;
                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void ClearCount()
        {
            foreach (DefectCountModel defectCountModel in DefectCountModels)
            {
                defectCountModel.Count = 0;
            }
            TotalDefects.Clear();
            TotalDefectCount = 0;
            FirstImageIndex = 0;
        }

        public override UserControl CreateControlView()
        {
            return new DefectCountControlView();
        }
        #endregion
    }
}
