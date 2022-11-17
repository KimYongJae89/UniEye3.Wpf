using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Models;
using UniScanC.Data;

namespace UniScanC.Windows.ViewModels
{
    internal class ThicknessChartSettingWindowViewModel : Observable
    {
        #region 필드
        private ThicknessChartSetting chartSetting;
        private ObservableCollection<string> layerNameList;
        private BoundaryLine boundaryLine;
        private System.Windows.Media.LinearGradientBrush linearGradientBrush;
        private ICommand okCommand;
        private ICommand cancelCommand;
        #endregion

        #region 생성자
        public ThicknessChartSettingWindowViewModel()
        {
            ChartSetting = new ThicknessChartSetting();
            layerNameList = new ObservableCollection<string>();
            boundaryLine = new BoundaryLine();
            linearGradientBrush = new System.Windows.Media.LinearGradientBrush();
        }

        public ThicknessChartSettingWindowViewModel(ThicknessChartSetting chartSetting)
        {
            ChartSetting = new ThicknessChartSetting();
            layerNameList = new ObservableCollection<string>();
            boundaryLine = new BoundaryLine();
            linearGradientBrush = new System.Windows.Media.LinearGradientBrush();

            //foreach (var layerName in SystemConfig.Instance.SpectrometerProperty.LayerNameList)
            //    LayerNameList.Add(layerName);

            LayerNameList.Add("Sheet");
            LayerNameList.Add("Film");

            ChartSetting = chartSetting;
            ChartSetting.PropertyChanged += ChangedChartSettingProperty;

            OnPropertyChanged("LinearGradientBrush");
            SetBoundary();
        }
        #endregion

        #region 속성
        public ThicknessChartSetting ChartSetting
        {
            get => chartSetting;
            set => Set(ref chartSetting, value);
        }

        public ObservableCollection<string> LayerNameList
        {
            get => layerNameList;
            set => Set(ref layerNameList, value);
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
        #endregion

        #region 메서드
        public void SetBoundary()
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

        public ICommand OkCommand => okCommand ?? (okCommand = new RelayCommand<ChildWindow>(OK));

        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand<ChildWindow>(Cancel));

        private void ChangedChartSettingProperty(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("LinearGradientBrush");
            SetBoundary();
        }

        private void OK(ChildWindow wnd)
        {
            wnd.Close(true);
        }

        private void Cancel(ChildWindow wnd)
        {
            wnd.Close(false);
        }
        #endregion
    }
}
