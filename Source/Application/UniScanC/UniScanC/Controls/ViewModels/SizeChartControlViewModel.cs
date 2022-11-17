using DynMvp.Data;
using DynMvp.InspectData;
using Infragistics.Controls.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Unieye.WPF.Base.Infragistics;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;

namespace UniScanC.Controls.ViewModels
{
    public class SizeChartControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public SizeChartControlViewModel() : base(typeof(SizeChartControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private int minDefectSize = 100;
        [LayoutControlViewModelPropertyAttribute]
        public int MinDefectSize
        {
            get => minDefectSize;
            set
            {
                if (Set(ref minDefectSize, value))
                {
                    UpdateChart();
                }
            }
        }

        private int defectStep = 100;
        [LayoutControlViewModelPropertyAttribute]
        public int DefectStep
        {
            get => defectStep;
            set
            {
                if (Set(ref defectStep, value))
                {
                    UpdateChart();
                }
            }
        }

        private int defectStepCount = 5;
        [LayoutControlViewModelPropertyAttribute]
        public int DefectStepCount
        {
            get => defectStepCount;
            set
            {
                if (Set(ref defectStepCount, value))
                {
                    UpdateChart();
                }
            }
        }
        #endregion


        #region 속성
        // 그래프에 넣어줄 데이터를 담고있는 속성
        private ObservableCollection<SeriesViewModel> seriesSource = new ObservableCollection<SeriesViewModel>();
        public ObservableCollection<SeriesViewModel> SeriesSource
        {
            get => seriesSource;
            set => Set(ref seriesSource, value);
        }

        private ObservableCollection<string> columnXAxisData = new ObservableCollection<string>();
        public ObservableCollection<string> ColumnXAxisData
        {
            get => columnXAxisData;
            set => Set(ref columnXAxisData, value);
        }

        private int[] defectSize =
        {
            100,
            -1,     // Etc
        };

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
            ClearData();
            UpdateChart();
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
                ClearData();
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

            var model = Models.SizeChartModel.CreateModel(filteredList, defectSize);

            foreach (SeriesViewModel serise in SeriesSource)
            {
                if (serise.Source is Dictionary<string, int> source)
                {
                    var newSource = new Dictionary<string, int>(source);
                    if (model.SizeCountDic.ContainsKey(serise.Name))
                    {
                        newSource["Defect"] += model.SizeCountDic[serise.Name];
                        serise.Source = newSource;

                        if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void UpdateChart()
        {
            Color minColor = Colors.LightBlue;
            Color maxColor = Colors.Red;
            //Color minColor = (Color)Application.Current.Resources["BlackColor"];
            //Color maxColor = (Color)Application.Current.Resources["AccentBaseColor"];

            defectSize = new int[DefectStepCount + 1];
            for (int i = 0; i < DefectStepCount; i++)
            {
                defectSize[i] = MinDefectSize + i * DefectStep;
                if (i == DefectStepCount - 1)
                {
                    defectSize[DefectStepCount] = -1;
                }
            }

            int r, g, b;
            r = (maxColor.R - minColor.R) / Convert.ToInt32(defectSize.Count());
            g = (maxColor.G - minColor.G) / Convert.ToInt32(defectSize.Count());
            b = (maxColor.B - minColor.B) / Convert.ToInt32(defectSize.Count());

            SeriesSource.Clear();
            ColumnXAxisData.Clear();
            ColumnXAxisData.Add("Title");

            var values = new Dictionary<string, int>();
            values.Add("Defect", 0);

            for (int i = 0; i < defectSize.Count(); i++)
            {
                var color = Color.FromRgb(
                    Convert.ToByte(minColor.R + (r * i)),
                    Convert.ToByte(minColor.G + (g * i)),
                    Convert.ToByte(minColor.B + (b * i)));

                var seriesViewModel = new SeriesViewModel();
                seriesViewModel.Name = defectSize[i].ToString();
                if (defectSize[i] != -1)
                {
                    seriesViewModel.Title = string.Format("~{0} um", defectSize[i]);
                }
                else
                {
                    seriesViewModel.Title = string.Format("{0} um↑", defectSize[i - 1]);
                }

                seriesViewModel.Type = SeriesType.ColumnSeries;
                seriesViewModel.Source = values;
                seriesViewModel.YPath = "Value";
                seriesViewModel.PointOutLine = new SolidColorBrush(color);
                seriesViewModel.PointBrush = new SolidColorBrush(color);

                SeriesSource.Add(seriesViewModel);
            }
        }

        private void ClearData()
        {
            foreach (SeriesViewModel serise in SeriesSource)
            {
                if (serise.Source is Dictionary<string, int> source)
                {
                    var tempDic = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> pair in source)
                    {
                        tempDic.Add(pair.Key, 0);
                    }
                    serise.Source = tempDic;
                }
            }
        }

        public override UserControl CreateControlView()
        {
            return new SizeChartControlView();
        }
        #endregion
    }
}
