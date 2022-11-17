using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Infragistics;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Models;

namespace UniScanC.Controls.ViewModels
{
    public class DefectMapFrameControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged, INotifySelectedDefectChanged, INotifyPatternWidthChanged
    {
        #region 생성자
        public DefectMapFrameControlViewModel() : base(typeof(DefectMapFrameControlViewModel))
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

        private double patternWidth = 500;
        [LayoutControlViewModelPropertyAttribute]
        public double PatternWidth
        {
            get => patternWidth;
            set => Set(ref patternWidth, value);
        }
        #endregion


        #region 속성
        public ICommand FitToSizeCommand { get; }

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

        internal const string MarkerCategoryName = "MARKER";

        private Model Model { get; set; } = null;
        private List<string> KeyList { get; set; } = new List<string>();

        // 나타낼 모듈의 번호 리스트
        public List<string> ModuleList => string.IsNullOrWhiteSpace(ModuleNoList) ? null : ModuleNoList.Split(',').ToList();

        // 그래프의 Point 선택시 Defect의 정보를 가져오기 위한 필드
        public Dictionary<Defect, PointF> DicDefectMappingMap { get; set; } = new Dictionary<Defect, PointF>();

        // 각 컨트롤의 데이터
        public List<InspectResult> ProductResults { get; set; }

        // 각 컨트롤의 데이터 (프레임)
        private List<InspectResult> frameProductResults;
        public List<InspectResult> FrameProductResults
        {
            get => frameProductResults;
            set => Set(ref frameProductResults, value);
        }

        // 결과의 프레임 데이터
        private List<FrameInfo> frameInfos;
        public List<FrameInfo> FrameInfos
        {
            get => frameInfos;
            set => Set(ref frameInfos, value);
        }

        // 선택한 프레임
        private FrameInfo selectedFrameInfo;
        public FrameInfo SelectedFrameInfo
        {
            get => selectedFrameInfo;
            set
            {
                Set(ref selectedFrameInfo, value);
                if (value != null)
                {
                    FrameProductResults = null;
                    FrameProductResults = ProductResults.Where(x => x.FrameIndex == value.FrameIndex).ToList();
                    UpdateChartLength(value);
                    UpdateFrameProductResult(FrameProductResults);
                }
            }
        }

        // 한 프레임 길이
        private double frameHeight;
        public double FrameHeight
        {
            get => frameHeight;
            set => Set(ref frameHeight, value);
        }

        // 그래프에 넣어줄 데이터를 담고있는 속성
        private ObservableCollection<SeriesViewModel> seriesSource = new ObservableCollection<SeriesViewModel>();
        public ObservableCollection<SeriesViewModel> SeriesSource
        {
            get => seriesSource;
            set => Set(ref seriesSource, value);
        }

        private double currentLength = 1;
        public double CurrentLength
        {
            get => currentLength;
            set => Set(ref currentLength, value);
        }

        private double baseLength = 0;
        public double BaseLength
        {
            get => baseLength;
            set => Set(ref baseLength, value);
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
            Model = modelBase as Model;

            ClearData();

            KeyList.Clear();
            SeriesSource.Clear();
            if (Model != null)
            {
                var defectCategories = new List<DefectCategory>();
                foreach (VisionModel visionModel in Model.VisionModels)
                {
                    foreach (DefectCategory category in visionModel.DefectCategories)
                    {
                        if (defectCategories.Find(x => x.Name == category.Name) == null)
                        {
                            defectCategories.Add(new DefectCategory(category));
                        }
                    }
                }

                defectCategories.Add(DefectCategory.GetDefaultCategory());
                defectCategories.Add(DefectCategory.GetColorCategory());

                // Series 재생성
                foreach (DefectCategory category in defectCategories)
                {
                    var values = new ObservableCollection<PointF>();
                    KeyList.Add(category.Name);

                    var seriesViewModel = new SeriesViewModel();
                    seriesViewModel.Name = category.Name;
                    seriesViewModel.Title = category.Name;
                    seriesViewModel.Type = SeriesType.ScatterSeries;
                    seriesViewModel.Source = values;
                    seriesViewModel.XPath = "X";
                    seriesViewModel.YPath = "Y";
                    seriesViewModel.PointType = (Infragistics.Controls.Charts.MarkerType)Enum.Parse(typeof(Infragistics.Controls.Charts.MarkerType), category.DefectFigure.ToString());
                    seriesViewModel.PointOutLine = new SolidColorBrush(category.DefectColor);
                    seriesViewModel.PointBrush = new SolidColorBrush(category.DefectColor);

                    SeriesSource.Add(seriesViewModel);
                }
            }

            var markerValues = new ObservableCollection<PointF>();

            var markerSeriesViewModel = new SeriesViewModel();
            markerSeriesViewModel.Name = MarkerCategoryName;
            markerSeriesViewModel.Title = MarkerCategoryName;
            markerSeriesViewModel.Type = SeriesType.ScatterSeries;
            markerSeriesViewModel.Source = markerValues;
            markerSeriesViewModel.XPath = "X";
            markerSeriesViewModel.YPath = "Y";
            markerSeriesViewModel.PointType = Infragistics.Controls.Charts.MarkerType.Pentagram;
            markerSeriesViewModel.PointOutLine = new SolidColorBrush(Colors.Black);
            markerSeriesViewModel.PointBrush = new SolidColorBrush(Colors.Red);

            SeriesSource.Add(markerSeriesViewModel);
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

            try
            {
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
                ProductResults = filteredList;

                FrameInfos = GetFrameInfos(ProductResults, taskCancelToken);
                SelectedFrameInfo = FrameInfos.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Defect Map Exception : {0}", ex.Message);
                Console.WriteLine("Defect Map Exception : {0}", ex.StackTrace);
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
            if (defect != null && SeriesSource.Count > 0)
            {
                SeriesViewModel model = SeriesSource.FirstOrDefault(x => x.Name == MarkerCategoryName);
                model.Source = null;
                if (DicDefectMappingMap?.ContainsKey(defect) == true)
                {
                    PointF point = DicDefectMappingMap.FirstOrDefault(x => x.Key == defect).Value;
                    model.Source = new List<PointF>() { point };
                }
            }
        }

        public void OnUpdatePatternWidth(double patternWidth)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdatePatternWidth(patternWidth)));
                return;
            }
            UpdatePatternWidth(patternWidth);
        }

        private void UpdatePatternWidth(double patternWidth)
        {
            PatternWidth = patternWidth;
        }

        private List<FrameInfo> GetFrameInfos(List<InspectResult> detailResults, CancellationTokenSource taskCancelToken)
        {
            var frameInfos = new List<FrameInfo>();

            foreach (InspectResult detailResult in detailResults)
            {
                FrameInfo frameInfo = frameInfos.Find(x => x.FrameIndex == detailResult.FrameIndex);
                if (frameInfo != null)
                {
                    frameInfo.DefectCount += detailResult.DefectList.Count;
                }
                else
                {
                    var newFrameInfo = new FrameInfo()
                    {
                        FrameIndex = detailResult.FrameIndex,
                        FramePosition = Math.Round((detailResult.FrameIndex + 1) * detailResult.InspectRegion.Height / 1000f, 2),
                        DefectCount = detailResult.DefectList.Count,
                        InspectRegionHeight = detailResult.InspectRegion.Height
                    };
                    frameInfos.Add(newFrameInfo);
                }
                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return frameInfos;
        }

        private void UpdateFrameProductResult(List<InspectResult> FrameProductResults)
        {
            var model = DefectMapChartModel.CreateModel(FrameProductResults, KeyList);
            foreach (SeriesViewModel serise in SeriesSource)
            {
                var newSource = new ObservableCollection<PointF>();
                if (model.PointList.ContainsKey(serise.Name))
                {
                    foreach (PointF point in model.PointList[serise.Name])
                    {
                        newSource.Add(point);
                    }
                    serise.Source = newSource;
                }
            }

            DicDefectMappingMap.Clear();
            foreach (KeyValuePair<Defect, PointF> pair in model.DicDefectMappingMap)
            {
                if (!DicDefectMappingMap.ContainsKey(pair.Key))
                {
                    DicDefectMappingMap.Add(pair.Key, pair.Value);
                }
            }
        }

        private void UpdateChartLength(FrameInfo SelectedFrameInfo)
        {
            FrameHeight = Math.Round(SelectedFrameInfo.InspectRegionHeight / 1000f, 2);
            int maxFrameIndex = FrameProductResults.Max(x => x.FrameIndex);
            float maxInspectRegionHeight = FrameProductResults.Max(x => x.InspectRegion.Height);
            CurrentLength = Math.Round(((double)(maxFrameIndex + 1) * maxInspectRegionHeight) / 1000f, 2);
            if (FrameHeight != 0)
            {
                BaseLength = Math.Max(0, CurrentLength - FrameHeight);
            }
        }


        private void ClearData()
        {
            foreach (SeriesViewModel serise in SeriesSource)
            {
                serise.Source = null;
                serise.Source = new ObservableCollection<PointF>();
            }

            FrameInfos = null;
            DicDefectMappingMap.Clear();
            ProductResults?.Clear();

            CurrentLength = 1;
            BaseLength = 0;
        }

        public override UserControl CreateControlView()
        {
            return new DefectMapFrameControlView();
        }
        #endregion
    }

    public class FrameInfo
    {
        public int FrameIndex { get; set; }
        public double FramePosition { get; set; }
        public int DefectCount { get; set; }
        public float InspectRegionHeight { get; set; }
    }
}
