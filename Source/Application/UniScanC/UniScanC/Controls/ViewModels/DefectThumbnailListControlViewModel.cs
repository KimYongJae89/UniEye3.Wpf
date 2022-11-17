using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class DefectThumbnailListControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged, INotifySelectedDefectChanged
    {
        #region 생성자
        public DefectThumbnailListControlViewModel() : base(typeof(DefectThumbnailListControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private string moduleNoList = "";
        [LayoutControlViewModelPropertyAttribute]
        public string ModuleNoList
        {
            get => moduleNoList;
            set => Set(ref moduleNoList, value);
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

        private IEnumerable<DefectInfoThumbnailListModel> models;
        public IEnumerable<DefectInfoThumbnailListModel> Models
        {
            get => models;
            set
            {
                Set(ref models, value);
                if (models != null)
                {
                    DefectCount = models.Count();
                }
            }
        }

        private DefectInfoThumbnailListModel selectedModel;
        public DefectInfoThumbnailListModel SelectedModel
        {
            get => selectedModel;
            set
            {
                if (Set(ref selectedModel, value))
                {
                    SelectedDefect = SelectedModel?.Defect;
                }
            }
        }

        private int defectCount;
        public int DefectCount
        {
            get => defectCount;
            set => Set(ref defectCount, value);
        }

        private double imageCount = 3;
        public double ImageCount
        {
            get => imageCount;
            set
            {
                if (Set(ref imageCount, value))
                {
                    ColNum = imageCount;
                }
            }
        }

        private double colNum = 4;
        public double ColNum
        {
            get => colNum;
            set => Set(ref colNum, value);
        }

        private Model Model { get; set; } = null;

        // 나타낼 모듈의 번호 리스트
        public List<string> ModuleList => string.IsNullOrWhiteSpace(ModuleNoList) ? null : ModuleNoList.Split(',').ToList();
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
            Model = modelBase as Model;
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
            if (productResults == null || productResults.Count() == 0)
            {
                ClearData();
                return;
            }

            var filteredList = new List<InspectResult>();
            foreach (ProductResult productResult in productResults)
            {
                if (productResult is InspectResult inspectResult)
                {
                    if (ModuleList != null)
                    {
                        if (ModuleList.Contains(inspectResult.ModuleNo.ToString()))
                        {
                            filteredList.Add(inspectResult);
                        }
                    }
                    else
                    {
                        filteredList.Add(inspectResult);
                    }
                }
            }

            Models = DefectInfoThumbnailListModel.CreateModel(filteredList.SelectMany(x => x.DefectList), Model, taskCancelToken);
            ColNum = ImageCount;
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

        public void UpdateSelectedDefect(Defect defect)
        {
            SelectedModel = Models?.FirstOrDefault(x => ReferenceEquals(x.Defect, defect));
        }

        private void ClearData()
        {
            Models = null;
        }

        private List<Defect> RepositionDefects(List<InspectResult> results)
        {
            var defects = new List<Defect>();
            ILookup<int, InspectResult> moduleSortResults = results.ToLookup(x => x.FrameIndex);
            foreach (IGrouping<int, InspectResult> pair in moduleSortResults)
            {
                float leftEdge = 0;
                InspectResult firstResult = pair.FirstOrDefault(x => x.ModuleNo == 0);
                if (firstResult != null)
                {
                    leftEdge = firstResult.EdgePos;
                }

                foreach (InspectResult result in pair)
                {
                    foreach (Defect defect in result.DefectList)
                    {
                        defect.BoundingRect = new RectangleF(
                            defect.BoundingRect.Left - leftEdge,
                            defect.BoundingRect.Top,
                            defect.BoundingRect.Width,
                            defect.BoundingRect.Height);

                        defect.DefectPos = new PointF(
                            defect.DefectPos.X - leftEdge,
                            defect.DefectPos.Y);

                        defects.Add(defect);
                    }
                }
            }

            return defects;
        }

        public override UserControl CreateControlView()
        {
            return new DefectThumbnailListControlView();
        }
        #endregion
    }
}
