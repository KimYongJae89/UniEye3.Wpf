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
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;

namespace UniScanC.Controls.ViewModels
{
    public class StatisticsLotDefectControlViewModel : CustomizeControlViewModel, INotifyProductResultChanged
    {
        #region 생성자
        public StatisticsLotDefectControlViewModel() : base(typeof(StatisticsLotDefectControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
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
        #endregion


        #region 메서드
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

            var model = StatisticsLotDefectModel.CreateModel(filteredList);

            SeriesSource.Clear();
            ColumnXAxisData.Clear();
            var values = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> pair in model.LotDefectDic)
            {
                ColumnXAxisData.Add(pair.Key);
                values.Add(pair.Key, pair.Value);

                if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                {
                    break;
                }
            }

            var seriesViewModel = new SeriesViewModel();
            seriesViewModel.Name = "DefectCount";
            seriesViewModel.Title = "DefectCount";
            seriesViewModel.Type = SeriesType.ColumnSeries;
            seriesViewModel.Source = values;
            seriesViewModel.YPath = "Value";
            seriesViewModel.PointOutLine = new SolidColorBrush((Color)Application.Current.Resources["AccentBaseColor"]);
            seriesViewModel.PointBrush = new SolidColorBrush((Color)Application.Current.Resources["AccentBaseColor"]);

            SeriesSource.Add(seriesViewModel);
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
            return new StatisticsLotDefectControlView();
        }
        #endregion
    }
}
