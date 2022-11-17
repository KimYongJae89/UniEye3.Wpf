using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Events;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Override;
using Unieye.WPF.Base.Services;
using Unieye.WPF.Base.ViewModels;
using UniEye.Base.Config;

namespace Unieye.WPF.Base.Views
{
    public class ButtonOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ButtonBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Application.Current.Resources["AccentBaseColorBrush"] : Application.Current.Resources["Transparent"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// SamsungShellWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SamsungShellWindow : MetroWindow, IShellWindow, IModelPropertyEvent
    {
        #region 생성자
        public SamsungShellWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion


        #region 속성
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(IEnumerable<NavigationMenuItem>), typeof(SamsungShellWindow));

        public IEnumerable<NavigationMenuItem> MenuItems
        {
            get => (IEnumerable<NavigationMenuItem>)GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public static readonly DependencyProperty OptionMenuItemsProperty =
            DependencyProperty.Register("OptionMenuItems", typeof(IEnumerable<NavigationMenuItem>), typeof(SamsungShellWindow));

        public IEnumerable<NavigationMenuItem> OptionMenuItems
        {
            get => (IEnumerable<NavigationMenuItem>)GetValue(OptionMenuItemsProperty);
            set => SetValue(OptionMenuItemsProperty, value);
        }

        public static readonly DependencyProperty SelectedMenuItemProperty =
            DependencyProperty.Register("SelectedMenuItem", typeof(NavigationMenuItem), typeof(SamsungShellWindow),
                new FrameworkPropertyMetadata(OnSelectedMenuItemChanged));

        private static void OnSelectedMenuItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is NavigationMenuItem menuItem)
            {
                LogHelper.Info(LoggerType.Operation, $"[Main Page] Select [{menuItem.Text}] Page");
            }
        }

        public NavigationMenuItem SelectedMenuItem
        {
            get => (NavigationMenuItem)GetValue(SelectedMenuItemProperty);
            set => SetValue(SelectedMenuItemProperty, value);
        }

        private List<NavigationMenuItem> MenuItemList = new List<NavigationMenuItem>();

        public static readonly DependencyProperty ShowProgramTitleProperty =
            DependencyProperty.Register("ShowProgramTitle", typeof(bool), typeof(SamsungShellWindow));

        public bool ShowProgramTitle
        {
            get => (bool)GetValue(ShowProgramTitleProperty);
            set => SetValue(ShowProgramTitleProperty, value);
        }

        public static readonly DependencyProperty ProgramTitleProperty =
            DependencyProperty.Register("ProgramTitle", typeof(string), typeof(SamsungShellWindow));

        public string ProgramTitle
        {
            get => (string)GetValue(ProgramTitleProperty);
            set => SetValue(ProgramTitleProperty, value);
        }

        public static readonly DependencyProperty TitleBarProperty =
            DependencyProperty.Register("TitleBar", typeof(ImageSource), typeof(SamsungShellWindow));

        public ImageSource TitleBar
        {
            get => (ImageSource)GetValue(TitleBarProperty);
            set => SetValue(TitleBarProperty, value);
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(GridLength), typeof(SamsungShellWindow));

        public GridLength TitleBarHeight
        {
            get => (GridLength)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public static readonly DependencyProperty BlankHeightProperty =
            DependencyProperty.Register("BlankHeight", typeof(GridLength), typeof(SamsungShellWindow));

        public GridLength BlankHeight
        {
            get => (GridLength)GetValue(BlankHeightProperty);
            set => SetValue(BlankHeightProperty, value);
        }

        public static readonly DependencyProperty CompanyLogoProperty =
            DependencyProperty.Register("CompanyLogo", typeof(BitmapImage), typeof(SamsungShellWindow));

        public BitmapImage CompanyLogo
        {
            get => (BitmapImage)GetValue(CompanyLogoProperty);
            set => SetValue(CompanyLogoProperty, value);
        }

        public static readonly DependencyProperty ProductLogoProperty =
            DependencyProperty.Register("ProductLogo", typeof(BitmapImage), typeof(SamsungShellWindow));

        public BitmapImage ProductLogo
        {
            get => (BitmapImage)GetValue(ProductLogoProperty);
            set => SetValue(ProductLogoProperty, value);
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(SamsungShellWindow));

        public string Date
        {
            get => (string)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("CurrentUser", typeof(string), typeof(SamsungShellWindow));

        public string CurrentUser
        {
            get => (string)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("CurrentModel", typeof(string), typeof(SamsungShellWindow));

        public string CurrentModel
        {
            get => (string)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(SamsungShellWindow));

        public string Time
        {
            get => (string)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        private System.Windows.Input.ICommand logInCommand;
        public System.Windows.Input.ICommand LogInCommand => logInCommand ?? (logInCommand = new Helpers.RelayCommand(LogIn));

        private Timer DateTimer { get; set; }
        #endregion


        #region 이벤트
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Button;
            var menuItem = control.Tag as NavigationMenuItem;
            if (!SelectedMenuItem.Equals(menuItem))
            {
                ShowTab(menuItem.TabKey);
            }
        }
        #endregion


        #region 메서드
        public void Initialize()
        {
            var uiMgr = UiManager.Instance as UiManager;
            IEnumerable<NavigationMenuItem> menuItems = uiMgr.CreateMenuItems();
            IEnumerable<NavigationMenuItem> optionMenuItems = uiMgr.CreateOptionMenuItems();

            Initialize(menuItems, optionMenuItems);
        }

        public virtual void Initialize(IEnumerable<NavigationMenuItem> navigationMenuItems, IEnumerable<NavigationMenuItem> navigationOptionMenuItems)
        {
            ShowProgramTitle = UiConfig.Instance().ShowProgramTitle;
            if (ShowProgramTitle)
            {
                TitleBarHeight = new GridLength(80, GridUnitType.Pixel);
                BlankHeight = new GridLength(0, GridUnitType.Pixel);

                ProgramTitle = UiConfig.Instance().ProgramTitle;
                if (File.Exists(PathConfig.Instance().TitleBar))
                {
                    TitleBar = new BitmapImage(new Uri(PathConfig.Instance().TitleBar));
                }

                if (File.Exists(PathConfig.Instance().CompanyLogo))
                {
                    CompanyLogo = new BitmapImage(new Uri(PathConfig.Instance().CompanyLogo));
                }

                if (File.Exists(PathConfig.Instance().ProductLogo))
                {
                    ProductLogo = new BitmapImage(new Uri(PathConfig.Instance().ProductLogo));
                }
            }
            else
            {
                TitleBarHeight = new GridLength(0, GridUnitType.Pixel);
                BlankHeight = new GridLength(20, GridUnitType.Pixel);
            }

            MenuItems = navigationMenuItems;
            OptionMenuItems = navigationOptionMenuItems;

            MenuItemList.AddRange(navigationMenuItems);
            MenuItemList.AddRange(navigationOptionMenuItems);

            SelectedMenuItem = navigationMenuItems.First();
            SelectedMenuItem.IsSelected = true;
            Closing += ShellWindow_Closing;

            DateTimer = new Timer();
            DateTimer.Interval = 500;
            DateTimer.Elapsed += Timer_Elapsed;
            DateTimer.Start();

            UserHandler.Instance.OnUserChanged += OnUserChanged;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetDateTime();
        }

        private void SetDateTime()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetDateTime());
                return;
            }

            DateTime dateTime = DateTime.Now;
            Date = dateTime.ToString("yyyy - MM - dd");
            Time = dateTime.ToString("HH : mm : ss");
        }

        private bool isExitProgram = false;
        private async void ShellWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isExitProgram)
            {
                e.Cancel = !isExitProgram;
                if (await CloseProgram())
                {
                    isExitProgram = true;
                    DateTimer.Stop();
                    Close();
                }
            }
        }

        private async Task<bool> CloseProgram()
        {
            bool result = await MessageWindowHelper.ShowMessageBox(
                UniEye.Translation.Helpers.TranslationHelper.Instance.Translate("EXIT_PROGRAM"),
                UniEye.Translation.Helpers.TranslationHelper.Instance.Translate("EXIT_PROGRAM_MESSAGE"),
                MessageBoxButton.OKCancel);
            return result == true;
        }

        public void ShowTab(UniEye.Base.UI.TabKey tabKey)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ShowTab(tabKey));
                return;
            }

            ClearSelection();
            SelectedMenuItem = MenuItemList.Find(x => x.TabKey == tabKey);
            SelectedMenuItem.IsSelected = true;
        }

        public void EnableTab(UniEye.Base.UI.TabKey tabKey, bool enable)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => EnableTab(tabKey, enable));
                return;
            }

            NavigationMenuItem menuItem = MenuItemList.Find(x => x.TabKey == tabKey);
            if (menuItem != null)
            {
                menuItem.IsEnabled = enable;
            }
        }

        private void ClearSelection()
        {
            foreach (NavigationMenuItem item in MenuItems)
            {
                item.IsSelected = false;
            }
            foreach (NavigationMenuItem item in OptionMenuItems)
            {
                item.IsSelected = false;
            }
        }

        private void LogIn()
        {
            var loginWindow = new LoginWindow();
            var loginViewModel = new LoginViewModel(false);
            loginWindow.DataContext = loginViewModel;
            loginWindow.ShowDialog();
        }

        public void OnUpdateModel(ModelBase modelBase)
        {
            CurrentModel = modelBase.ModelDescription.Name;
        }

        private void OnUserChanged(User user)
        {
            CurrentUser = user.UserId;

            if (user.IsAuth(ERoleType.ModelPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, true);
            }
            else
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, false);
            }

            if (user.IsAuth(ERoleType.InspectPage) && CurrentModel != null)
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, true);
            }
            else
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, false);
            }

            if (user.IsAuth(ERoleType.TeachPage) && CurrentModel != null)
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, true);
            }
            else
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, false);
            }

            if (user.IsAuth(ERoleType.ReportPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Report, true);
            }
            else
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Report, false);
            }

            if (user.IsAuth(ERoleType.SettingPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, true);
            }
            else
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, false);
            }
        }
        #endregion
    }
}