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
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Override;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Windows.ViewModels;
using UniScanC.Windows.Views;
using ModelDescription = UniScanC.Models.ModelDescription;

namespace UniScanC.Controls.ViewModels
{
    public class GlossTrendChartControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public GlossTrendChartControlViewModel() : base(typeof(GlossTrendChartControlViewModel))
        {
            SettingCommand = new RelayCommand(SettingCommandAction);
        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private GlossChartSetting chartSetting = new GlossChartSetting();
        [LayoutControlViewModelPropertyAttribute]
        public GlossChartSetting ChartSetting
        {
            get => chartSetting;
            set => Set(ref chartSetting, value);
        }
        #endregion


        #region 속성
        public ICommand SettingCommand { get; set; }

        private GlossScanData scanData;
        public GlossScanData ScanData
        {
            get => scanData;
            set => Set(ref scanData, value);
        }

        private ObservableCollection<PointF> glossData;
        public ObservableCollection<PointF> GlossData
        {
            get => glossData;
            set => Set(ref glossData, value);
        }

        private ObservableCollection<PointF> columnGlossData;
        public ObservableCollection<PointF> ColumnGlossData
        {
            get => columnGlossData;
            set => Set(ref columnGlossData, value);
        }

        private IEnumerable<float> columnXAxisData;
        public IEnumerable<float> ColumnXAxisData
        {
            get => columnXAxisData;
            set => Set(ref columnXAxisData, value);
        }

        private BoundaryLine boundaryLine = new BoundaryLine();
        public BoundaryLine BoundaryLine
        {
            get => boundaryLine;
            set => Set(ref boundaryLine, value);
        }

        private System.Windows.Media.LinearGradientBrush linearGradientBrush = new System.Windows.Media.LinearGradientBrush();
        public System.Windows.Media.LinearGradientBrush LinearGradientBrush
        {
            get
            {
                var tmepLinearGradientBrush = new LinearGradientBrush();

                tmepLinearGradientBrush.SpreadMethod = GradientSpreadMethod.Pad;

                var invalidColor = new System.Windows.Media.Color();
                var validColor = new System.Windows.Media.Color();

                invalidColor = (System.Windows.Media.Color)Application.Current.Resources["AccentColor3"];
                validColor = (System.Windows.Media.Color)Application.Current.Resources["WhiteColor"];

                var gradientStopCollection = new GradientStopCollection();

                tmepLinearGradientBrush.StartPoint = new System.Windows.Point(0, 0);
                tmepLinearGradientBrush.EndPoint = new System.Windows.Point(1, 0);

                var gradientStop1 = new GradientStop();
                var gradientStop2 = new GradientStop();
                var gradientStop3 = new GradientStop();
                var gradientStop4 = new GradientStop();
                var gradientStop5 = new GradientStop();
                var gradientStop6 = new GradientStop();

                gradientStop1 = new GradientStop(invalidColor, 0.0f);
                gradientStop2 = new GradientStop(invalidColor, (chartSetting.ValidStart - chartSetting.StartPos - 0.1) / (chartSetting.EndPos - chartSetting.StartPos));
                gradientStop3 = new GradientStop(validColor, (chartSetting.ValidStart - chartSetting.StartPos) / (chartSetting.EndPos - chartSetting.StartPos));
                gradientStop4 = new GradientStop(validColor, (chartSetting.ValidEnd - chartSetting.StartPos) / (chartSetting.EndPos - chartSetting.StartPos));
                gradientStop5 = new GradientStop(invalidColor, (chartSetting.ValidEnd - chartSetting.StartPos + 0.1) / (chartSetting.EndPos - chartSetting.StartPos));
                gradientStop6 = new GradientStop(invalidColor, 1.0f);

                gradientStopCollection.Add(gradientStop1);
                gradientStopCollection.Add(gradientStop2);
                gradientStopCollection.Add(gradientStop3);
                gradientStopCollection.Add(gradientStop4);
                gradientStopCollection.Add(gradientStop5);
                gradientStopCollection.Add(gradientStop6);

                tmepLinearGradientBrush.GradientStops = gradientStopCollection;

                return tmepLinearGradientBrush;
            }
            set => Set(ref linearGradientBrush, value);
        }
        #endregion


        #region 메서드
        public void UpdateBoundary()
        {
            var targetlist = new List<PointF>();
            targetlist.Add(new PointF(chartSetting.StartPos, chartSetting.TargetValue));
            targetlist.Add(new PointF(chartSetting.EndPos, chartSetting.TargetValue));
            BoundaryLine.Target = targetlist;

            var upperErrorlist = new List<PointF>();
            upperErrorlist.Add(new PointF(chartSetting.StartPos, chartSetting.UpperErrorValue));
            upperErrorlist.Add(new PointF(chartSetting.EndPos, chartSetting.UpperErrorValue));
            BoundaryLine.UError = upperErrorlist;

            var upperWarninglist = new List<PointF>();
            upperWarninglist.Add(new PointF(chartSetting.StartPos, chartSetting.UpperWarningValue));
            upperWarninglist.Add(new PointF(chartSetting.EndPos, chartSetting.UpperWarningValue));
            BoundaryLine.UWarning = upperWarninglist;

            var lowerWarninglist = new List<PointF>();
            lowerWarninglist.Add(new PointF(chartSetting.StartPos, chartSetting.LowerWarningValue));
            lowerWarninglist.Add(new PointF(chartSetting.EndPos, chartSetting.LowerWarningValue));
            BoundaryLine.LWarning = lowerWarninglist;

            var lowerErrorlist = new List<PointF>();
            lowerErrorlist.Add(new PointF(chartSetting.StartPos, chartSetting.LowerErrorValue));
            lowerErrorlist.Add(new PointF(chartSetting.EndPos, chartSetting.LowerErrorValue));
            BoundaryLine.LError = lowerErrorlist;
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
            GlossData = new ObservableCollection<PointF>();
            ColumnGlossData = new ObservableCollection<PointF>();

            if (modelBase == null)
            {
                return;
            }

            if (modelBase.ModelDescription is ModelDescription modelDesc)
            {
                ChartSetting.StartPos = 0;
                ChartSetting.ValidStart = 0;
                ChartSetting.EndPos = 10000;
                ChartSetting.ValidEnd = 10000;
                UpdateChart();
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
                ScanData = null;
                GlossData = new ObservableCollection<PointF>();
                ColumnGlossData = new ObservableCollection<PointF>();

                var axisData = new List<float>();
                for (int i = 0; i <= ChartSetting.EndPos; i++)
                {
                    axisData.Add(i);
                }

                ColumnXAxisData = axisData;

                return;
            }

            IEnumerable<GlossResult> measureResults = productResults.Where(x => x is GlossResult).Cast<GlossResult>();
            if (measureResults.Count() == 0)
            {
                return;
            }

            IEnumerable<Dictionary<string, GlossScanData>> tempScanDataList = measureResults.Select(x => x.ScanData);
            var scanDataList = new List<Dictionary<string, GlossScanData>>();
            scanDataList.AddRange(tempScanDataList);

            string layerName = ChartSetting.LayerName;
            if (string.IsNullOrWhiteSpace(layerName))
            {
                return;
            }

            GlossResult measureResult = measureResults?.Last();
            ScanData = measureResult?.ScanData[layerName];

            var columnDataList = new List<PointF>();
            var GlossDataList = new List<PointF>();
            foreach (Dictionary<string, GlossScanData> scanDatas in scanDataList)
            {
                GlossScanData scanData = scanDatas[layerName];
                int position = scanData.ReelPosition;

                for (int i = ColumnGlossData.Count; i < position; i++)
                {
                    columnDataList.Add(new PointF(scanData.Average, scanData.Average));
                }

                columnDataList.Add(new PointF(scanData.Min, scanData.Max));
                GlossDataList.Add(new PointF(scanData.ReelPosition, scanData.Average));
            }

            foreach (PointF columnData in columnDataList)
            {
                ColumnGlossData.Add(columnData);
            }

            foreach (PointF glossData in GlossDataList)
            {
                GlossData.Add(glossData);
            }

            if (ChartSetting.AutoTarget == true)
            {
                ChartSetting.TargetValue = scanDataList.Last()[layerName].Average;
                UpdateChart();
            }
        }

        private async void SettingCommandAction()
        {
            var chartSettingWindow = new GlossChartSettingWindowView();
            var chartSettingViewModel = new GlossChartSettingWindowViewModel(chartSetting.Clone());
            chartSettingWindow.DataContext = chartSettingViewModel;
            if (await MessageWindowHelper.ShowChildWindow<bool>(chartSettingWindow))
            {
                ChartSetting.CopyFrom(chartSettingViewModel.ChartSetting);
                UiManager.Instance.InspectLayoutHandler.Save("InspectLayout");
                UpdateChart();
            }
        }

        private void UpdateChart()
        {
            OnPropertyChanged("LinearGradientBrush");
            UpdateBoundary();
        }

        public override UserControl CreateControlView()
        {
            return new GlossTrendChartControlView();
        }
        #endregion
    }
}
