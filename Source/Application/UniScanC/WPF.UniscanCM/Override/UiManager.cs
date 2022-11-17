using System.Collections.Generic;
using System.Windows;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Override;
using Unieye.WPF.Base.Views;
using UniScanC.Controls.ViewModels;
using WPF.UniScanCM.Pages.Views;

namespace WPF.UniScanCM.Override
{
    public class UiManager : Unieye.WPF.Base.Override.UiManager
    {
        private PopupWindow defectDetailView;
        public PopupWindow DefectDetailView => defectDetailView ?? (defectDetailView = new PopupWindow(null, false));

        public UiManager() : base()
        {
            DefectDetailView.Width = 800;
            DefectDetailView.Height = 450;

            InitInspectLayoutControlTypeList();
            InitRepotLayoutControlTypeList();
            InitStatisticsLayoutControlTypeList();
        }

        private void InitInspectLayoutControlTypeList()
        {
            InspectLayoutControlTypeList.Clear();
            if (SystemConfig.Instance.UseInspectModule)
            {
                InspectLayoutControlTypeList.Add(typeof(DefectMapControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(DefectSummaryControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(DefectCountControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(FrameImageControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(SizeChartControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(ThumbnailListControlViewModel));

                // 패턴 길이 찾기 기능을 사용할 시
                if (SystemConfig.Instance.IsInspectPattern)
                {
                    InspectLayoutControlTypeList.Add(typeof(PatternSizeControlViewModel));
                }

                // IO 기능을 사용할 시
                if (SystemConfig.Instance.UseIO)
                {
                    InspectLayoutControlTypeList.Add(typeof(IOPortStatusControlViewModel));
                    InspectLayoutControlTypeList.Add(typeof(LabelerControlViewModel));
                }
            }

            if (SystemConfig.Instance.UseThicknessModule)
            {
                InspectLayoutControlTypeList.Add(typeof(ThicknessProfileChartControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(ThicknessTrendChartControlViewModel));
            }

            if (SystemConfig.Instance.UseGlossModule)
            {
                InspectLayoutControlTypeList.Add(typeof(GlossProfileChartControlViewModel));
                InspectLayoutControlTypeList.Add(typeof(GlossTrendChartControlViewModel));
            }
        }

        private void InitRepotLayoutControlTypeList()
        {
            ReportLayoutControlTypeList.Clear();
            if (SystemConfig.Instance.UseInspectModule)
            {
                ReportLayoutControlTypeList.Add(typeof(DefectMapControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(DefectMapFrameControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(DefectCountControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(SizeChartControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(DefectThumbnailListControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(DefectDetailHorizentalControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(DefectDetailVerticalControlViewModel));

                // 패턴 길이 찾기 기능을 사용할 시
                if (SystemConfig.Instance.IsInspectPattern)
                {
                    ReportLayoutControlTypeList.Add(typeof(PatternSizeControlViewModel));
                }
            }

            if (SystemConfig.Instance.UseThicknessModule)
            {
                ReportLayoutControlTypeList.Add(typeof(ThicknessProfileChartControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(ThicknessTrendChartControlViewModel));
            }

            if (SystemConfig.Instance.UseGlossModule)
            {
                ReportLayoutControlTypeList.Add(typeof(GlossProfileChartControlViewModel));
                ReportLayoutControlTypeList.Add(typeof(GlossTrendChartControlViewModel));
            }
        }

        private void InitStatisticsLayoutControlTypeList()
        {
            StatisticsLayoutControlTypeList.Clear();
            if (SystemConfig.Instance.UseInspectModule)
            {
                StatisticsLayoutControlTypeList.Add(typeof(StatisticsLotDefectControlViewModel));
            }
        }

        public override IShellWindow CreateMainWindow()
        {
            switch (SystemConfig.Instance.Customer)
            {
                case Enums.ECustomer.Samsung: MainWindow = new SamsungShellWindow(); break;
                default: MainWindow = new ShellWindow(); break;
            }
            return MainWindow;
        }

        public override IEnumerable<NavigationMenuItem> CreateMenuItems()
        {
            var modelPage = new ModelPageView();
            var monitoringPage = new MonitoringPageView();
            var teachingPage = new TeachingPageView();
            var reportPage = new ReportPageView();
            var statisticsPage = new StatisticsPageView();

            // MainWindow에 있는 타이틀 바의 모델 정보를 변경 시켜주기 위해서 바인딩을 사용
            SetModelBinding(MainWindow as Window, monitoringPage.DataContext);

            // enable 초기값 설정
            return new NavigationMenuItem[]
            {
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Model, "Model", char.ConvertFromUtf32(0xE82D), modelPage),
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Inspect, "Inspect", char.ConvertFromUtf32(0xE773), monitoringPage){IsEnabled=false },
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Teach, "Teach", char.ConvertFromUtf32(0xE7BE), teachingPage){IsEnabled=false },
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Report, "Report", char.ConvertFromUtf32(0xE9F9), reportPage),
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Statistics, "Statistics", char.ConvertFromUtf32(0xE9D2), statisticsPage),
            };
        }

        public override IEnumerable<NavigationMenuItem> CreateOptionMenuItems()
        {
            var logPage = new LogPageView();
            var settingPage = new SettingPage();
            settingPage.CustomSettingControl = CreateCustomSettingControl();

            return new NavigationMenuItem[]
            {
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Log, "Log", char.ConvertFromUtf32(0xF584),logPage),
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Setting, "Setting", char.ConvertFromUtf32(0xE713),settingPage)
            };
        }

        public override ICustomSettingControl CreateCustomSettingControl()
        {
            return new CustomSettingView();
        }
    }
}
