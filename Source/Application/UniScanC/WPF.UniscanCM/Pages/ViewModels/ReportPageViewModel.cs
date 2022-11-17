using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Layout.ViewModels;
using UniEye.Translation.Helpers;
using UniScanC.Controls.Views;
using UniScanC.Data;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Models;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Service;
using ICommand = System.Windows.Input.ICommand;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class ReportPageViewModel : Observable
    {
        private ReportService ReportService { get; set; } = new ReportService();

        // 좌측 결과 창의 팝업 상태
        private bool isSearchPanelOpen = true;
        public bool IsSearchPanelOpen
        {
            get => isSearchPanelOpen;
            set => Set(ref isSearchPanelOpen, value);
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set => Set(ref startDate, value);
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set => Set(ref endDate, value);
        }

        private string[] defectTypesNames;
        public string[] DefectTypesNames
        {
            get => defectTypesNames;
            set => Set(ref defectTypesNames, value);
        }

        // 결과 목록 창 전체 내용
        private IEnumerable<ReportModel> reportModels;
        public IEnumerable<ReportModel> ReportModels
        {
            get => reportModels;
            set => Set(ref reportModels, value);
        }

        // 결과 목록 창 분류 내용
        private IEnumerable<ReportModel> sortedReportModels;
        public IEnumerable<ReportModel> SortedReportModels
        {
            get => sortedReportModels;
            set => Set(ref sortedReportModels, value);
        }

        // 결과 목록 창에서 선택한 결과
        private ReportModel searchReportModel;
        public ReportModel SearchReportModel
        {
            get => searchReportModel;
            set => Set(ref searchReportModel, value);
        }

        // 검색한 모델 이름 리스트
        private IEnumerable<string> resultModelList;
        public IEnumerable<string> ResultModelList
        {
            get => resultModelList;
            set => Set(ref resultModelList, value);
        }

        // 콤보박스에서 선택한 모델의 이름
        private string selectedTargetModel;
        public string SelectedTargetModel
        {
            get => selectedTargetModel;
            set
            {
                Set(ref selectedTargetModel, value);

                if (value != "ALL")
                {
                    SortedReportModels = ReportModels.Where(x => x.ModelName == value);
                    IEnumerable<ReportModel> targetReportModel = SortedReportModels.Where(x => x.DefectTypes.Count == SortedReportModels.Max(y => y.DefectTypes.Count));
                    if (targetReportModel.Count() > 0)
                    {
                        DefectTypesNames = targetReportModel.FirstOrDefault().DefectTypes.Select(x => x.Key).ToArray();
                    }
                }
                else
                {
                    SortedReportModels = ReportModels;
                    DefectTypesNames = null;
                }
            }
        }

        // 선택한 모델
        private UniScanC.Models.Model selectedModel;
        public UniScanC.Models.Model SelectedModel
        {
            get => selectedModel;
            set => Set(ref selectedModel, value);
        }

        // 검사 데이터
        private IEnumerable<ProductResult> searchResults;
        public IEnumerable<ProductResult> SearchResults
        {
            get => searchResults;
            set => Set(ref searchResults, value);
        }

        // 불량 데이터
        private IEnumerable<Defect> searchDefects;
        public IEnumerable<Defect> SearchDefects
        {
            get => searchDefects;
            set => Set(ref searchDefects, value);
        }

        private Defect selectedDefect;
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set => Set(ref selectedDefect, value);
        }

        // 선택한 결과 제품의 패턴 너비
        private double selectedPatternWidth;
        public double SelectedPatternWidth
        {
            get => selectedPatternWidth;
            set => Set(ref selectedPatternWidth, value);
        }

        private LayoutHandler layoutHandler;
        public LayoutHandler LayoutHandler
        {
            get => layoutHandler;
            set => Set(ref layoutHandler, value);
        }

        private LayoutViewModel layoutViewModel;
        public LayoutViewModel LayoutViewModel
        {
            get => layoutViewModel;
            set => Set(ref layoutViewModel, value);
        }

        private ModelEventDelegate UpdateModelDelegate { get; set; }
        private UpdateResultDelegate UpdateResultDelegate { get; set; }
        private SelectedDefectUpdateDelegate UpdateSelectedDefectDelegate { get; set; }
        private PatternWidthUpdateDelegate UpdatePatternWidthDelegate { get; set; }

        private bool isLoadImage = true;
        public bool IsLoadImage
        {
            get => isLoadImage;
            set => Set(ref isLoadImage, value);
        }

        public ICommand FlyoutControlOpenCloseCommand { get; }
        public ICommand OneDayCommand { get; }
        public ICommand OneWeekCommand { get; }
        public ICommand OneMonthCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SearchItemDoubleClick { get; }
        public ICommand ExportCommand { get; }
        public ICommand ExportSearchListCommand { get; }
        public ICommand ClearSearchResultCommand { get; }

        public ReportPageViewModel()
        {
            FlyoutControlOpenCloseCommand = new RelayCommand<bool>(FlyoutControlOpenCloseCommandAction);
            OneDayCommand = new RelayCommand(OneDayCommandAction);
            OneWeekCommand = new RelayCommand(OneWeekCommandAction);
            OneMonthCommand = new RelayCommand(OneMonthCommandAction);
            SearchCommand = new RelayCommand(SearchCommandAction);
            SearchItemDoubleClick = new RelayCommand<ReportModel>(SearchItemDoubleClickAction);
            ExportCommand = new RelayCommand(ExportCommandAction);
            ExportSearchListCommand = new RelayCommand(ExportSearchListCommandAction);
            ClearSearchResultCommand = new RelayCommand(ClearSearchResultCommandAction);

            EndDate = DateTime.Now;
            StartDate = EndDate - TimeSpan.FromDays(1);

            OnLayoutChanged(UiManager.Instance.ReportLayoutHandler);
            UiManager.Instance.OnReportLayoutChanged += OnLayoutChanged;
        }

        private void FlyoutControlOpenCloseCommandAction(bool searchPanelOpenState)
        {
            IsSearchPanelOpen = !searchPanelOpenState;
        }

        private void OneDayCommandAction()
        {
            EndDate = DateTime.Now;
            StartDate = EndDate - TimeSpan.FromDays(1);
        }

        private void OneWeekCommandAction()
        {
            EndDate = DateTime.Now;
            StartDate = EndDate - TimeSpan.FromDays(7);
        }

        private void OneMonthCommandAction()
        {
            EndDate = DateTime.Now;
            StartDate = EndDate - TimeSpan.FromDays(30);
        }

        // 결과 리스트 검색 메서드
        private async void SearchCommandAction()
        {
            var progressSource = new ProgressSource();
            progressSource.Range = ReportService.GetLotCount(StartDate.Date, EndDate.Date.AddDays(1));
            progressSource.Step = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            await MessageWindowHelper.ShowProgress(
                TranslationHelper.Instance.Translate("Search_List"),
                TranslationHelper.Instance.Translate("Loading") + ("..."),
                new Action(() =>
                {
                    ReportModels = ReportService.SearchResult(StartDate.Date, EndDate.Date.AddDays(1), progressSource);
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    var modelList = new List<string>();
                    modelList.Add("ALL");
                    foreach (ReportModel reportModel in ReportModels)
                    {
                        if (modelList.Contains(reportModel.ModelName) == false)
                        {
                            modelList.Add(reportModel.ModelName);
                        }
                    }
                    ResultModelList = modelList;
                    SelectedTargetModel = modelList[0];
                }), true, progressSource);
        }

        // 결과 세부 데이터 검색 메서드
        private async void SearchItemDoubleClickAction(ReportModel reportModel)
        {
            var progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource.Step = 1;
            progressSource.Range = ReportService.GetDefectCount(reportModel.LotNo) + 4;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            await MessageWindowHelper.ShowProgress(
                TranslationHelper.Instance.Translate("Load_Database"),
                TranslationHelper.Instance.Translate("Loading") + ("..."),
                new Action(() =>
                {
                    SelectedModel = ReportService.GetModel(reportModel);
                    UpdateModelDelegate(SelectedModel);

                    // TODO:[송현석] 일단은 VisionModel에서 가장 큰 사이즈 패턴으로 가져온다.
                    SelectedPatternWidth = SelectedModel.VisionModels.Max(x => x.PatternWidth);
                    UpdatePatternWidthDelegate(SelectedPatternWidth);

                    SearchReportModel = reportModel;

                    ClearResults(progressSource);

                    List<InspectResult> detailResults = ReportService.SearchDetailResult(reportModel, IsLoadImage, progressSource);
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    SearchResults = detailResults.OrderBy(x => x.FrameIndex);

                    List<Defect> repositionDefects = ReportService.RepositionDefects(detailResults, progressSource);
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    SearchDefects = repositionDefects?.OrderBy(x => x.DefectNo);

                    UpdateResultDelegate(SearchResults);
                }), true, progressSource);

            if (!progressSource.CancellationTokenSource.IsCancellationRequested)
            {
                IsSearchPanelOpen = false;
            }
        }

        // 결과 리스트 Excell Export 메서트
        private void ExportCommandAction()
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                ReportService.ExportAction(SelectedModel, SearchResults, SearchDefects, SearchReportModel);
            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => ReportService.ExportAction(SelectedModel, SearchResults, SearchDefects, SearchReportModel)));
            }
        }

        private async void ExportSearchListCommandAction()
        {
            if (SortedReportModels == null)
            {
                return;
            }

            var progressSource = new ProgressSource();
            progressSource.Range = SortedReportModels.Count();
            progressSource.Step = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Filter = "Excel(*.xlsx)|*.xlsx";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await MessageWindowHelper.ShowProgress(
                    TranslationHelper.Instance.Translate("Excel_Export"),
                    TranslationHelper.Instance.Translate("Loading") + ("..."),
                    new Action(() =>
                    {
                        ReportService.ExcellExportSearchList(dlg.FileName, SortedReportModels, DefectTypesNames, progressSource);
                        if (progressSource.CancellationTokenSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }), true, progressSource);
            }
        }

        private async void ClearSearchResultCommandAction()
        {
            var progressSource = new ProgressSource();
            progressSource.Range = 1;
            progressSource.Step = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            await MessageWindowHelper.ShowProgress(
                TranslationHelper.Instance.Translate("Clear"),
                TranslationHelper.Instance.Translate("Loading") + ("..."),
                new Action(() =>
                {
                    System.Threading.Thread.Sleep(1000);
                    ClearResults(progressSource);
                    System.Threading.Thread.Sleep(1000);
                }), true, progressSource);
        }

        private void OnUpdateSelectedDefect(object selectedDefect)
        {
            var defect = selectedDefect as Defect;
            SelectedDefect = defect;
            UpdateSelectedDefectDelegate?.Invoke(selectedDefect);
        }

        private void OnLayoutChanged(LayoutHandler layoutHandler)
        {
            LayoutHandler = layoutHandler;
            LayoutViewModel = new LayoutViewModel(LayoutHandler);
            LinkAllDelegate();
            //LinkModelDelegate();
            //LinkProductResultsDelegate();
            //LinkSelectedDefectDelegate();
            //LinkPatternWidthDelegate();
        }

        // 하기 4개의 메서드를 한번에 진행하는 메서드
        private void LinkAllDelegate()
        {
            UpdateModelDelegate = null;
            UpdateResultDelegate = null;
            UpdateSelectedDefectDelegate = null;
            UpdatePatternWidthDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyModelChanged notifyModelControl)
                    {
                        UpdateModelDelegate += notifyModelControl.OnUpdateModel;
                    }

                    if (model.ControlViewModel is INotifyProductResultChanged notifyProductResultControl)
                    {
                        UpdateResultDelegate += notifyProductResultControl.OnUpdateResult;
                    }

                    if (model.ControlViewModel is INotifySelectedDefectChanged notifySelectedDefectControl)
                    {
                        UpdateSelectedDefectDelegate += notifySelectedDefectControl.OnUpdateSelectedDefect;
                        notifySelectedDefectControl.SelectedDefectUpdate += OnUpdateSelectedDefect;
                    }
                    if (model.ControlViewModel is INotifyPatternWidthChanged notifyPatternWidthControl)
                    {
                        UpdatePatternWidthDelegate += notifyPatternWidthControl.OnUpdatePatternWidth;
                    }
                }
            }
        }

        private void LinkModelDelegate()
        {
            UpdateModelDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyModelChanged notifyModelControl)
                    {
                        UpdateModelDelegate += notifyModelControl.OnUpdateModel;
                    }
                }
            }
        }

        private void LinkProductResultsDelegate()
        {
            UpdateResultDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyProductResultChanged notifyProductResultControl)
                    {
                        UpdateResultDelegate += notifyProductResultControl.OnUpdateResult;
                    }
                }
            }
        }

        private void LinkSelectedDefectDelegate()
        {
            UpdateSelectedDefectDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifySelectedDefectChanged notifySelectedDefectControl)
                    {
                        UpdateSelectedDefectDelegate += notifySelectedDefectControl.OnUpdateSelectedDefect;
                        notifySelectedDefectControl.SelectedDefectUpdate += OnUpdateSelectedDefect;
                    }
                }
            }
        }

        private void LinkPatternWidthDelegate()
        {
            UpdatePatternWidthDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyPatternWidthChanged notifyPatternWidthControl)
                    {
                        UpdatePatternWidthDelegate += notifyPatternWidthControl.OnUpdatePatternWidth;
                    }
                }
            }
            //foreach (var layoutPageHandler in LayoutHandler)
            //{
            //    var layoutModelList = layoutPageHandler.ModelList.ToList();
            //    foreach (var model in layoutModelList)
            //    {
            //        Binding binding = new Binding();
            //        binding.Source = this;
            //        binding.Path = new PropertyPath("SelectedPatternWidth");
            //        binding.Mode = BindingMode.TwoWay;

            //        if (model.ControlViewModelType == typeof(DefectMapControlView))
            //        {
            //            var control = model.ControlViewModel as DefectMapControlView;
            //            BindingOperations.SetBinding(control, DefectMapControlView.PatternWidthProperty, binding);
            //        }
            //        else if (model.ControlViewModelType == typeof(DefectMapFrameControlView))
            //        {
            //            var control = model.ControlViewModel as DefectMapFrameControlView;
            //            BindingOperations.SetBinding(control, DefectMapFrameControlView.PatternWidthProperty, binding);
            //        }
            //    }
            //}
        }

        private void ClearResults(ProgressSource progressSource)
        {
            var ClearTask = Task.Run(() => UpdateResultDelegate(null), progressSource.CancellationTokenSource.Token);
            Task.WaitAll(ClearTask);
            progressSource.StepIt();
        }
    }

    public class FrameInfo
    {
        public int FrameIndex { get; set; }
        public double FramePosition { get; set; }
        public int DefectCount { get; set; }
        public float InspectRegionHeight { get; set; }
    }
}