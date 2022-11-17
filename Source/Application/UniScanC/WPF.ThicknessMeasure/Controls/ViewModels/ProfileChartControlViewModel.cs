using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Events;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Data;
using WPF.ThicknessMeasure.Data;
using WPF.ThicknessMeasure.Override;
using WPF.ThicknessMeasure.Windows.ViewModels;
using WPF.ThicknessMeasure.Windows.Views;
using ModelDescription = WPF.ThicknessMeasure.Model.ModelDescription;

namespace WPF.ThicknessMeasure.Controls.ViewModels
{
    internal class ProfileChartControlViewModel : Observable, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 필드
        private ThicknessScanData scanData;
        private IEnumerable<PointF> thicknessData;
        private ThicknessChartSetting chartSetting;
        private BoundaryLine boundaryLine = new BoundaryLine();
        private System.Windows.Media.LinearGradientBrush linearGradientBrush = new System.Windows.Media.LinearGradientBrush();

        private ICommand settingCommand;
        #endregion

        #region 생성자
        public ProfileChartControlViewModel(ThicknessChartSetting chartSetting)
        {
            ChartSetting = chartSetting;
        }
        #endregion

        #region 속성
        public ThicknessScanData ScanData
        {
            get => scanData;
            set => Set(ref scanData, value);
        }

        public IEnumerable<PointF> ThicknessData
        {
            get => thicknessData;
            set => Set(ref thicknessData, value);
        }

        public ThicknessChartSetting ChartSetting
        {
            get => chartSetting;
            set => Set(ref chartSetting, value);
        }

        public BoundaryLine BoundaryLine
        {
            get => boundaryLine;
            set => Set(ref boundaryLine, value);
        }

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

        public ICommand SettingCommand => settingCommand ?? (settingCommand = new RelayCommand(Setting));
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

        public void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => UpdateResult(productResults)));
        }

        public void UpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            if (productResults == null || productResults.Count() == 0)
            {
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

            if (ChartSetting.AutoTarget == true)
            {
                if (ChartSetting.TargetType == EThicknessTargetType.Thickness)
                {
                    ChartSetting.TargetValue = scanData[layerName].Average;
                }
                else
                {
                    ChartSetting.TargetValue = scanData[layerName].DataList.Average(x => x.Refraction);
                }

                UpdateChart();
            }
        }

        public void OnUpdateModel(ModelBase modelBase)
        {
            ThicknessData = null;

            if (modelBase == null)
            {
                return;
            }

            if (modelBase.ModelDescription is ModelDescription modelDesc)
            {
                UniScanC.Models.ScanWidth scanWidth = SystemConfig.Instance.ScanWidthList.Find(x => x.Name == modelDesc.ScanWidth);
                ChartSetting.StartPos = scanWidth.Start;
                ChartSetting.ValidStart = scanWidth.ValidStart;
                ChartSetting.EndPos = scanWidth.End;
                ChartSetting.ValidEnd = scanWidth.ValidEnd;
                UpdateChart();
            }
        }

        private void Setting()
        {
            var chartSettingWindow = new ChartSettingWindowView();
            var chartSettingViewModel = new ChartSettingWindowViewModel(chartSetting.Clone());
            chartSettingWindow.DataContext = chartSettingViewModel;
            if (chartSettingWindow.ShowDialog() == true)
            {
                ChartSetting.CopyFrom(chartSettingViewModel.ChartSetting);
                UiManager.Instance.InspectLayoutHandler.Save();
                UpdateChart();
            }
        }

        private void UpdateChart()
        {
            OnPropertyChanged("LinearGradientBrush");
            UpdateBoundary();
        }
        #endregion
    }
}
