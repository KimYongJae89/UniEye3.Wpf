using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unieye.WPF.Base.Helpers;
using UniScanC.Controls.Models;
using WPF.UniScanCM.Windows.ViewModels;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class LogPageViewModel
    {
        #region 생성자
        public LogPageViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandAction);

            LogHelper.NotifyLog += NotifyLog;
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand SaveCommand { get; }

        public ObservableCollection<LogModel> LogModels { get; set; } = new ObservableCollection<LogModel>();
        #endregion


        #region 메서드
        private async void SaveCommandAction()
        {
            var collectLogWindowViewModel = new CollectLogWindowViewModel();
            var collectLogWindowView = new CollectLogWindowView();
            collectLogWindowView.DataContext = collectLogWindowViewModel;

            if (await MessageWindowHelper.ShowChildWindow<bool>(collectLogWindowView))
            {

            }
        }

        public void NotifyLog(LogLevel logLevel, LoggerType loggerType, string message)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current?.Dispatcher;
            if (dispatcher != null)
            {
                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(new Action(() => NotifyLog(logLevel, loggerType, message)));
                    return;
                }
                NotifyLogAction(logLevel, loggerType, message);
            }
        }

        public void NotifyLogAction(LogLevel logLevel, LoggerType loggerType, string message)
        {
            if (logLevel == LogLevel.Info)
            {
                LogModels.Insert(0, new LogModel(logLevel, loggerType, message));
            }
        }
        #endregion
    }
}
