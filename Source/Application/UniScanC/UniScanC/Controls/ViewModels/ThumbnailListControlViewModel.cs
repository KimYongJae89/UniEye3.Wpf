using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;

namespace UniScanC.Controls.ViewModels
{
    public class ThumbnailListControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged, INotifySelectedDefectChanged
    {
        #region 생성자
        public ThumbnailListControlViewModel() : base(typeof(ThumbnailListControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private bool isReverse = true;
        [LayoutControlViewModelPropertyAttribute]
        public bool IsReverse
        {
            get => isReverse;
            set => Set(ref isReverse, value);
        }

        private int maxCount = 20;
        [LayoutControlViewModelPropertyAttribute]
        public int MaxCount
        {
            get => maxCount;
            set => Set(ref maxCount, value);
        }
        #endregion


        #region 속성
        public SelectedDefectUpdateDelegate SelectedDefectUpdate { get; set; }

        private Defect selectedDefect;
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set
            {
                if (Set(ref selectedDefect, value))
                {
                    SelectedDefectUpdate(value);
                }
            }
        }

        private ObservableRangeCollection<ThumbnailModel> defectModels = new ObservableRangeCollection<ThumbnailModel>();
        public ObservableRangeCollection<ThumbnailModel> DefectModels
        {
            get => defectModels;
            set => Set(ref defectModels, value);
        }

        private ThumbnailModel selectedModel;
        public ThumbnailModel SelectedModel
        {
            get => selectedModel;
            set
            {
                if (Set(ref selectedModel, value))
                {
                    SelectedDefect = selectedModel?.Defect;
                }
            }
        }
        #endregion


        #region 메서드
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
            DefectModels.Clear();
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
                DefectModels.Clear();
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

            List<ThumbnailModel> modelList = ThumbnailModel.CreateModel(filteredList, taskCancelToken);

            if (IsReverse)
            {
                DefectModels.InsertRange(0, modelList);
            }
            else
            {
                DefectModels.AddRange(modelList);
            }

            if (MaxCount != 0 && DefectModels.Count > MaxCount)
            {
                DefectModels.RemoveRange(MaxCount, DefectModels.Count - MaxCount);
            }
        }

        public void OnUpdateSelectedDefect(object selectedDefect)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateSelectedDefect(selectedDefect)));
                return;
            }
            UpdateSelectedDefect(selectedDefect as Defect);
        }

        private void UpdateSelectedDefect(Defect defect)
        {
            SelectedModel = DefectModels.FirstOrDefault(x => x.Defect == defect);
        }

        public override UserControl CreateControlView()
        {
            return new ThumbnailListControlView();
        }
        #endregion
    }
}
