using System.Collections.Generic;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Override;
using WPF.ThicknessMeasure.Controls.Views;
using WPF.ThicknessMeasure.Pages.Views;

namespace WPF.ThicknessMeasure.Override
{
    public class UiManager : Unieye.WPF.Base.Override.UiManager
    {
        #region 메서드
        public UiManager() : base()
        {
            InitInspectLayoutControlTypeList();
            InitRepotLayoutControlTypeList();
        }

        private void InitInspectLayoutControlTypeList()
        {
            InspectLayoutControlTypeList.Clear();
            InspectLayoutControlTypeList.Add(typeof(ProfileChartControlView));
            InspectLayoutControlTypeList.Add(typeof(TrendChartControlView));
        }

        private void InitRepotLayoutControlTypeList()
        {
            ReportLayoutControlTypeList.Clear();
            ReportLayoutControlTypeList.Add(typeof(ProfileChartControlView));
            ReportLayoutControlTypeList.Add(typeof(TrendChartControlView));
        }

        public override IEnumerable<NavigationMenuItem> CreateMenuItems()
        {
            return new NavigationMenuItem[]
            {
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Model, "Model", char.ConvertFromUtf32(0xE82D), new ModelPageView()),
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Inspect, "Inspect", char.ConvertFromUtf32(0xE773), new MonitoringPageView()),
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Teach, "Teach", char.ConvertFromUtf32(0xE7BE), new TeachingPageView()),
                //new NavigationMenuItem(UniEye.Base.UI.TabKey.Report, "Report", char.ConvertFromUtf32(0xE9F9), new ReportPage()),
            };
        }

        public override ICustomSettingControl CreateCustomSettingControl()
        {
            return new CustomSettingControl();
        }
        #endregion
    }
}
