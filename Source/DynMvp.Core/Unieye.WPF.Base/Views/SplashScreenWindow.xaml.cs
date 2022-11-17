using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Unieye.WPF.Base.ViewModels;
using UniEye.Base.Config;

namespace Unieye.WPF.Base.Views
{
    /// <summary>
    /// SplashScreenWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashScreenWindow : Window, IReportProgress, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        //SplashScreenWindowModel viewModel;

        private SplashActionDelegate SetupAction;
        private SplashActionDelegate ConfigAction;

        public bool SetupResult = false;

        #region Binding Variable

        private string splashTitle;
        public string SplashTitle
        {
            get => splashTitle;
            set => Set(ref splashTitle, value);
        }

        private string versionText;
        public string VersionText
        {
            get => versionText;
            set => Set(ref versionText, value);
        }

        private string buildText;
        public string BuildText
        {
            get => buildText;
            set => Set(ref buildText, value);
        }

        private string progressMessage;
        public string ProgressMessage
        {
            get => progressMessage;
            set => Set(ref progressMessage, value);
        }

        private string copyRightText;
        public string CopyRightText
        {
            get => copyRightText;
            set => Set(ref copyRightText, value);
        }

        #endregion

        public string LastError { get; set; }

        private DateTime startTime;
        private DispatcherTimer timer;
        public SplashScreenWindow(SplashActionDelegate _SetupAction, SplashActionDelegate _ConfigAction)
        {
            InitializeComponent();

            SetupAction = _SetupAction;
            ConfigAction = _ConfigAction;

            SplashTitle = UiConfig.Instance().Title;
            CopyRightText = UiConfig.Instance().Copyright;

            VersionText = string.Format("Version {0}", OperationConfig.Instance().SystemVersion);
            int revision = OperationConfig.Instance().SystemRevision;
            if (revision != -1)
            {
                BuildText = string.Format("Revision {0}", revision);
            }
            else
            {
                BuildText = OperationConfig.Instance().GetBuildNo();
            }

            DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += SetupTimer;
            timer.Start();

            startTime = DateTime.Now;
        }

        private void SetupTimer(object sender, EventArgs e)
        {
            if (!ShowActivated)
            {
                return;
            }

            TimeSpan timeSpan = DateTime.Now - startTime;
            if (!doConfigAction)
            {
                if (timeSpan.TotalSeconds >= 5)
                {
                    if (SetupTask == null)
                    {
                        Setup();
                    }
                    else
                    {
                        timer.Stop();
                        SetupTask.Wait();
                        DialogResult = SetupResult;
                    }
                }
                else
                {
                    ProgressMessage = string.Format("Start up {0} sec...", 5 - timeSpan.Seconds);
                }
            }
        }

        private Task SetupTask;
        public void Setup()
        {
            SetupTask = Task.Run(() =>
            {
                if (SetupAction != null && SetupAction(this) == false)
                {
                    MessageBox.Show("Some error is occurred. Please, check the configuration.\n\n" + LastError);
                    SetupResult = false;
                    //DialogResult = DialogResult.Abort;
                }
                else
                {
                    SetupResult = true;
                }
            });
        }

        public void SetLastError(string lastError)
        {
            LastError = lastError;
        }

        public string GetLastError()
        {
            return LastError;
        }

        public void ReportProgress(int percentage, string message)
        {
            ProgressMessage = message;
        }

        private bool doConfigAction = false;
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.F12)
            {
                if (ConfigAction != null && !doConfigAction)
                {
                    doConfigAction = true;
                    ProgressMessage = "Wait Configuration";

                    ConfigAction(this);

                    doConfigAction = false;

                    startTime = DateTime.Now;
                }
            }
        }
    }
}
