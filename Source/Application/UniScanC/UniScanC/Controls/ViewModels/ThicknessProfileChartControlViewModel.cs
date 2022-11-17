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
    public class ThicknessProfileChartControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public ThicknessProfileChartControlViewModel() : base(typeof(ThicknessProfileChartControlViewModel))
        {
            SettingCommand = new RelayCommand(SettingCommandAction);
        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private ThicknessChartSetting chartSetting = new ThicknessChartSetting();
        [LayoutControlViewModelPropertyAttribute]
        public ThicknessChartSetting ChartSetting
        {
            get => chartSetting;
            set => Set(ref chartSetting, value);
        }
        #endregion


        #region 속성
        public ICommand SettingCommand { get; set; }

        private ThicknessScanData scanData;
        public ThicknessScanData ScanData
        {
            get => scanData;
            set => Set(ref scanData, value);
        }

        private IEnumerable<PointF> thicknessData;
        public IEnumerable<PointF> ThicknessData
        {
            get => thicknessData;
            set => Set(ref thicknessData, value);
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
        private async void SettingCommandAction()
        {
            var chartSettingWindow = new ThicknessChartSettingWindowView();
            var chartSettingViewModel = new ThicknessChartSettingWindowViewModel(chartSetting.Clone());
            chartSettingWindow.DataContext = chartSettingViewModel;
            if (await MessageWindowHelper.ShowChildWindow<bool>(chartSettingWindow))
            {
                ChartSetting.CopyFrom(chartSettingViewModel.ChartSetting);
                UiManager.Instance.InspectLayoutHandler.Save("InspectLayout");
                UpdateChart();
            }
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
            ThicknessData = null;

            if (modelBase == null)
            {
                return;
            }

            if (modelBase.ModelDescription is ModelDescription modelDesc)
            {
                //var scanWidth = SystemConfig.Instance.ScanWidthList.Find(x => x.Name == modelDesc.ScanWidth);
                //ChartSetting.StartPos = scanWidth.Start;
                //ChartSetting.ValidStart = scanWidth.ValidStart;
                //ChartSetting.EndPos = scanWidth.End;
                //ChartSetting.ValidEnd = scanWidth.ValidEnd;

                ChartSetting.StartPos = 200;
                ChartSetting.ValidStart = 200;
                ChartSetting.EndPos = 720;
                ChartSetting.ValidEnd = 720;

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
                ThicknessData = null;
                return;
            }

            IEnumerable<ThicknessResult> measureResults = productResults.Where(x => x is ThicknessResult).Cast<ThicknessResult>();
            if (measureResults.Count() == 0)
            {
                return;
            }

            ThicknessResult measureResult = measureResults?.Last();

            var dataList = new List<PointF>();
            Dictionary<string, ThicknessScanData> scanData = measureResult?.ScanData;
            string layerName = ChartSetting.LayerName;
            if (string.IsNullOrWhiteSpace(layerName))
            {
                return;
            }

            ScanData = scanData[layerName];
            EThicknessTargetType targetType = ChartSetting.TargetType;

            switch (targetType)
            {
                case EThicknessTargetType.Thickness:
                    {
                        for (int i = 0; i < ScanData.ValidPointCount; i++)
                        {
                            dataList.Add(new PointF(ScanData.DataList[i].Position, ScanData.DataList[i].Thickness));
                        }
                    }
                    break;
                case EThicknessTargetType.Refraction:
                    {
                        for (int i = 0; i < ScanData.ValidPointCount; i++)
                        {
                            dataList.Add(new PointF(ScanData.DataList[i].Position, ScanData.DataList[i].Refraction));
                        }
                    }
                    break;
                default:
                    {
                        for (int i = 0; i < ScanData.ValidPointCount; i++)
                        {
                            dataList.Add(new PointF(ScanData.DataList[i].Position, ScanData.DataList[i].Thickness));
                        }
                    }
                    break;
            }

            ThicknessData = dataList;

            if (ChartSetting.AutoTarget)
            {
                if (ChartSetting.TargetType == EThicknessTargetType.Thickness)
                {
                    ChartSetting.TargetValue = scanData[layerName].Average;
                }
                else
                {
                    ChartSetting.TargetValue = scanData[layerName].DataList.Average(x => x.Refraction);
                }

                UpdateBoundary();
            }

            if (ChartSetting.AutoRange)
            {
                ChartSetting.StartPos = scanData[layerName].StartPosition;
                ChartSetting.ValidStart = scanData[layerName].ValidStartPosition;
                ChartSetting.EndPos = scanData[layerName].EndPosition;
                ChartSetting.ValidEnd = scanData[layerName].ValidEndPosition;

                OnPropertyChanged("LinearGradientBrush");
            }
        }

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

        private void UpdateChart()
        {
            OnPropertyChanged("LinearGradientBrush");
            UpdateBoundary();
        }

        public override UserControl CreateControlView()
        {
            return new ThicknessProfileChartControlView();
        }
        #endregion
    }
}
