using Authentication.Core;
using Authentication.Core.Datas;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Events;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Views;
using UniEye.Base.UI;

namespace Unieye.WPF.Base.Override
{
    public delegate void LayoutChangedDelegate(LayoutHandler layoutHandler);

    public class UiManager
    {
        #region Single Instance
        public delegate void InspectionStartedDelegate(bool isStarted);
        public InspectionStartedDelegate OnInspectionStarted { get; set; }

        private static UiManager _instance = null;
        public static UiManager Instance => _instance ?? (_instance = new UiManager());

        public static void SetInstance(UiManager uiManager)
        {
            _instance = uiManager;
            UserHandler.Instance.OnUserChanged = uiManager.OnUserChanged;
        }
        #endregion

        public IShellWindow MainWindow { get; set; }

        public List<Type> InspectLayoutControlTypeList { get; set; }

        public LayoutHandler InspectLayoutHandler { get; set; } = new LayoutHandler();

        public LayoutChangedDelegate OnInspectLayoutChanged;

        public List<Type> ReportLayoutControlTypeList { get; set; }

        public LayoutHandler ReportLayoutHandler { get; set; } = new LayoutHandler();

        public LayoutChangedDelegate OnReportLayoutChanged;

        public List<Type> StatisticsLayoutControlTypeList { get; set; }

        public LayoutHandler StatisticsLayoutHandler { get; set; } = new LayoutHandler();

        public LayoutChangedDelegate OnStatisticsLayoutChanged;

        public UiManager()
        {
            InspectLayoutHandler.Load("InspectLayout");
            InspectLayoutControlTypeList = CustomizeControlViewModel.GetControlProperties();

            ReportLayoutHandler.Load("ReportLayout");
            ReportLayoutControlTypeList = CustomizeControlViewModel.GetControlProperties();

            StatisticsLayoutHandler.Load("StatisticsLayout");
            StatisticsLayoutControlTypeList = CustomizeControlViewModel.GetControlProperties();
        }

        public virtual IShellWindow CreateMainWindow()
        {
            MainWindow = new ShellWindow();
            return MainWindow;
        }

        public virtual DependencyObject SetModelBinding(DependencyObject dependencyObject, object BindingSource)
        {
            var binding = new Binding();
            binding.Source = BindingSource;
            binding.Path = new PropertyPath("Model");
            BindingOperations.SetBinding(dependencyObject, ModelBehavior.ModelBaseProperty, binding);

            return dependencyObject;
        }

        public virtual IEnumerable<NavigationMenuItem> CreateMenuItems()
        {
            // 각 프로젝트에서 페이지 구성하여 함수 구현
            return null;
        }

        public virtual IEnumerable<NavigationMenuItem> CreateOptionMenuItems()
        {
            var settingPage = new SettingPage();
            settingPage.CustomSettingControl = CreateCustomSettingControl();

            return new NavigationMenuItem[]
            {
                new NavigationMenuItem(UniEye.Base.UI.TabKey.Setting, "Setting", char.ConvertFromUtf32(0xE713),settingPage)
            };
        }

        public virtual ICustomSettingControl CreateCustomSettingControl()
        {
            // 각 프로젝트에서 페이지 구성하여 함수 구현
            return null;
        }

        public virtual void ShowTab(TabKey tabkey)
        {
            MainWindow.ShowTab(tabkey);
        }

        public virtual void EnableTab(TabKey tabkey, bool enable)
        {
            MainWindow?.EnableTab(tabkey, enable);
        }

        public virtual void OnUserChanged(User user)
        {

        }
    }
}
