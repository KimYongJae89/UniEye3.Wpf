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
    public class DefectMapControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged, INotifySelectedDefectChanged, INotifyPatternWidthChanged
    {
        #region 생성자
        public DefectMapControlViewModel() : base(typeof(DefectMapControlViewModel))
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

        private bool showEdgeLine = false;
        [LayoutControlViewModelPropertyAttribute]
        public bool ShowEdgeLine
        {
            get => showEdgeLine;
            set => Set(ref showEdgeLine, value);
        }

        private double patternWidth = 500;
        [LayoutControlViewModelPropertyAttribute]
        public double PatternWidth
        {
            get => patternWidth;
            set => Set(ref patternWidth, value);
        }

        private double maxLength = 10;
        [LayoutControlViewModelPropertyAttribute]
        public double MaxLength
        {
            get => maxLength;
            set => Set(ref maxLength, value);
        }
        #endregion


        #region 속성
        public ICommand FitToSizeCommand { get; }

        public SelectedDefectUpdateDelegate SelectedDefectUpdate { get; set; }

        internal const string MarkerCategoryName = "MARKER";
        internal const string EdgeCategoryName = "EDGE";

        private Model Model { get; set; } = null;
        private List<string> KeyList { get; set; } = new List<string>();

        // 나타낼 모듈의 번호 리스트
        public List<string> ModuleList => string.IsNullOrWhiteSpace(ModuleNoList) ? null : ModuleNoList.Split(',').ToList();

        // 그래프의 Point 선택시 Defect의 정보를 가져오기 위한 필드
        public Dictionary<Defect, PointF> DicDefectMappingMap { get; set; } = new Dictionary<Defect, PointF>();

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

        //private double PreviousLength { get; set; } = 0;
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
            markerSeriesViewModel.PointOutLine = Application.Current.Resources["BlackBrush"] as SolidColorBrush;
            markerSeriesViewModel.PointBrush = Application.Current.Resources["AccentBaseColorBrush"] as SolidColorBrush;

            SeriesSource.Add(markerSeriesViewModel);

            if (ShowEdgeLine)
            {
                var edgeValues = new ObservableCollection<PointF>();
                var edgeSeriesViewModel = new SeriesViewModel();
                edgeSeriesViewModel.Name = EdgeCategoryName;
                edgeSeriesViewModel.Type = SeriesType.ScatterSeries;
                edgeSeriesViewModel.Source = edgeValues;
                edgeSeriesViewModel.XPath = "X";
                edgeSeriesViewModel.YPath = "Y";
                edgeSeriesViewModel.PointType = Infragistics.Controls.Charts.MarkerType.Tetragram;
                edgeSeriesViewModel.PointOutLine = new SolidColorBrush(Colors.Transparent);
                edgeSeriesViewModel.PointBrush = Application.Current.Resources["BlackBrush"] as SolidColorBrush;

                SeriesSource.Add(edgeSeriesViewModel);
            }
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

                if (filteredList.Count == 0)
                {
                    return;
                }

                // 스캔 길이 값을 갱신
                int maxFrameIndex = filteredList.Max(x => x.FrameIndex);
                float maxInspectRegionHeight = filteredList.Max(x => x.InspectRegion.Height);
                double newLength = Math.Round(((double)(maxFrameIndex + 1) * maxInspectRegionHeight) / 1000f, 2);
                CurrentLength = Math.Max(CurrentLength, newLength);
                if (MaxLength != 0)
                {
                    // 새로 계산한 거리 값과 기존에 계산한 거리 값 차이가 그래프의 범위 값보다 작을 경우에만 이전 거리 값을 수정해준다.
                    //if (Math.Abs(newLength - PreviousLength) > MaxLength)
                    //{
                    //    PreviousLength = newLength;
                    //}
                    //BaseLength = Math.Max(0, PreviousLength - MaxLength);

                    BaseLength = Math.Max(0, newLength - MaxLength);
                }

                // 검사 결과들의 모서리 포인트를 표시
                // 모듈 별로 Lookup 한 후에 Key 숫자 순으로 정렬하고 앞에 있는 배열을 불러옴
                if (ShowEdgeLine)
                {
                    SeriesViewModel edgeSeries = SeriesSource.FirstOrDefault(x => x.Name == EdgeCategoryName);
                    var source = edgeSeries.Source as ObservableCollection<PointF>;
                    var newSource = new ObservableCollection<PointF>(source);
                    foreach (InspectResult inspectResult in filteredList.ToLookup(x => x.ModuleNo).OrderBy(x => x.Key).FirstOrDefault())
                    {
                        if (source != null)
                        {
                            float edgeLength = (float)Math.Round(((double)(inspectResult.FrameIndex + 1) * maxInspectRegionHeight - maxInspectRegionHeight / 2.0f) / 1000f, 2);
                            newSource.Add(new PointF(inspectResult.EdgePos, edgeLength));
                            newSource.Add(new PointF(inspectResult.EdgePos + (float)Model.VisionModels.FirstOrDefault().PatternWidth, edgeLength));
                        }
                        if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                    edgeSeries.Source = newSource;
                }

                // 불량 점들을 표시
                var model = DefectMapChartModel.CreateModel(filteredList, KeyList);
                foreach (SeriesViewModel serise in SeriesSource)
                {
                    if (serise.Source is ObservableCollection<PointF> source)
                    {
                        var newSource = new ObservableCollection<PointF>(source);
                        if (model.PointList.ContainsKey(serise.Name))
                        {
                            foreach (PointF point in model.PointList[serise.Name])
                            {
                                newSource.Add(point);
                                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                                {
                                    break;
                                }
                            }
                            serise.Source = newSource;
                        }
                    }
                    if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                foreach (KeyValuePair<Defect, PointF> pair in model.DicDefectMappingMap)
                {
                    if (!DicDefectMappingMap.ContainsKey(pair.Key))
                    {
                        DicDefectMappingMap.Add(pair.Key, pair.Value);
                    }
                }
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

        private void ClearData()
        {
            foreach (SeriesViewModel serise in SeriesSource)
            {
                serise.Source = null;
                serise.Source = new ObservableCollection<PointF>();
            }
            DicDefectMappingMap.Clear();

            CurrentLength = 1;
            //BaseLength = PreviousLength = 0;
            BaseLength = 0;
        }

        public override UserControl CreateControlView()
        {
            return new DefectMapControlView();
        }
        #endregion
    }
}
