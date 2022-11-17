using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.Models;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class ExportOptionWindowViewModel : Observable
    {
        private ChildWindow ChildWindow { get; set; }

        private bool isAllDefectInfos;
        public bool IsAllDefectInfos
        {
            get => isAllDefectInfos;
            set
            {
                if (Set(ref isAllDefectInfos, value))
                {
                    if ((isAllDefectInfos == true && Model.DefectInfoPairs.Exists(x => x.Item2 == false)
                        || (isAllDefectInfos == false && !Model.DefectInfoPairs.Exists(x => x.Item2 == false))))
                    {
                        Model.DefectInfoPairs.ForEach(x => x.TuplePropertyChanged -= OnDefectInfoPairsChanged);
                        Model.DefectInfoPairs.ForEach(x => x.Item2 = value);
                        Model.DefectInfoPairs.ForEach(x => x.TuplePropertyChanged += OnDefectInfoPairsChanged);
                        ExportOptionModel temp = Model;
                        Model = null;
                        Model = temp;
                    }
                }
            }
        }

        private bool isAllDefectTypes;
        public bool IsAllDefectTypes
        {
            get => isAllDefectTypes;
            set
            {
                if (Set(ref isAllDefectTypes, value))
                {
                    if ((isAllDefectTypes == true && Model.DefectCategoryPairs.Exists(x => x.Item2 == false)
                        || (isAllDefectTypes == false && !Model.DefectCategoryPairs.Exists(x => x.Item2 == false))))
                    {
                        Model.DefectCategoryPairs.ForEach(x => x.TuplePropertyChanged -= OnDefectCategoryPairsChanged);
                        Model.DefectCategoryPairs.ForEach(x => x.Item2 = value);
                        Model.DefectCategoryPairs.ForEach(x => x.TuplePropertyChanged += OnDefectCategoryPairsChanged);
                        ExportOptionModel temp = Model;
                        Model = null;
                        Model = temp;
                    }
                }
            }
        }

        private bool isAllChartTypes;
        public bool IsAllChartTypes
        {
            get => isAllChartTypes;
            set
            {
                if (Set(ref isAllChartTypes, value))
                {
                    if ((isAllChartTypes == true && Model.ChartPairs.Exists(x => x.Item2 == false)
                        || (isAllChartTypes == false && !Model.ChartPairs.Exists(x => x.Item2 == false))))
                    {
                        Model.ChartPairs.ForEach(x => x.TuplePropertyChanged -= OnChartPairsChanged);
                        Model.ChartPairs.ForEach(x => x.Item2 = value);
                        Model.ChartPairs.ForEach(x => x.TuplePropertyChanged += OnChartPairsChanged);
                        ExportOptionModel temp = Model;
                        Model = null;
                        Model = temp;
                    }
                }
            }
        }

        private ExportOptionModel model;
        public ExportOptionModel Model
        {
            get => model;
            set => Set(ref model, value);
        }

        public ICommand LoadedCommand { get; }
        public ICommand ChartSettingCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand CancelCommand { get; }

        public ExportOptionWindowViewModel()
        {
            LoadedCommand = new RelayCommand<List<object>>((objList) =>
            {
                ChildWindow = objList[0] as ChildWindow;
                var reportModel = objList[1] as ReportModel;
                var exportModel = objList[2] as ExportOptionModel;
                Model = new ExportOptionModel();
                Model.CopyFrom(exportModel);

                foreach (EDefectInfos defectInfo in Enum.GetValues(typeof(EDefectInfos)))
                {
                    if (Model.DefectInfoPairs.Find(x => x.Item1 == defectInfo) == null)
                    {
                        Model.DefectInfoPairs.Add(new WTuple<EDefectInfos, bool>(defectInfo, false));
                    }
                }

                foreach (EChartType chartType in Enum.GetValues(typeof(EChartType)))
                {
                    if (Model.ChartPairs.Find(x => x.Item1 == chartType) == null)
                    {
                        Model.ChartPairs.Add(new WTuple<EChartType, bool>(chartType, false));
                    }
                }

                foreach (WTuple<EDefectInfos, bool> tuple in Model.DefectInfoPairs)
                {
                    tuple.TuplePropertyChanged += OnDefectInfoPairsChanged;
                }

                foreach (WTuple<string, bool> tuple in Model.DefectCategoryPairs)
                {
                    tuple.TuplePropertyChanged += OnDefectCategoryPairsChanged;
                }

                foreach (WTuple<EChartType, bool> tuple in Model.ChartPairs)
                {
                    tuple.TuplePropertyChanged += OnChartPairsChanged;
                }
            });

            ChartSettingCommand = new RelayCommand<string>((eChartType) =>
            {
                switch ((EChartType)Enum.Parse(typeof(EChartType), eChartType))
                {
                    case EChartType.SizeChart: break;
                    case EChartType.LengthChart: break;
                    case EChartType.WidthChart: break;
                    default: break;
                }
            });

            ExportCommand = new RelayCommand(() => ChildWindow?.Close(Model));

            CancelCommand = new RelayCommand(() => ChildWindow?.Close(null));
        }

        private void OnDefectInfoPairsChanged(EDefectInfos item1, bool item2)
        {
            IsAllDefectInfos = !Model.DefectInfoPairs.Exists(x => x.Item2 == false);
        }

        private void OnDefectCategoryPairsChanged(string item1, bool item2)
        {
            IsAllDefectTypes = !Model.DefectCategoryPairs.Exists(x => x.Item2 == false);
        }

        private void OnChartPairsChanged(EChartType item1, bool item2)
        {
            IsAllChartTypes = !Model.ChartPairs.Exists(x => x.Item2 == false);
        }
    }
}
