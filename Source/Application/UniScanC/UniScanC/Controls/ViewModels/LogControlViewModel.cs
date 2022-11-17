using DynMvp.Base;
using System;
using System.Windows;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;

namespace UniScanC.Controls.ViewModels
{
    public class LogControlViewModel : CustomizeControlViewModel, INotifyLogChanged
    {
        #region 생성자
        public LogControlViewModel() : base(typeof(LogControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private bool isReverse = true;
        [LayoutControlViewModelPropertyAttribute]
        public bool IsReverse
        {
            get => isReverse;
            set => Set(ref isReverse, value);
        }

        private int maxCount = 20;
        [LayoutControlViewModelPropertyAttribute]
        public int MaxCount
        {
            get => maxCount;
            set => Set(ref maxCount, value);
        }
        #endregion


        #region 속성
        public ObservableRangeCollection<LogModel> LogModels { get; set; } = new ObservableRangeCollection<LogModel>();
        #endregion


        #region 메서드
        public void NotifyLog(LogLevel logLevel, LoggerType loggerType, string message)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => NotifyLogAction(logLevel, loggerType, message)));
        }

        public void NotifyLogAction(LogLevel logLevel, LoggerType loggerType, string message)
        {
            if (logLevel == LogLevel.Info)
            {
                if (IsReverse)
                {
                    LogModels.Insert(0, new LogModel(logLevel, loggerType, message));
                }
                else
                {
                    LogModels.Add(new LogModel(logLevel, loggerType, message));
                }
            }
        }

        public override UserControl CreateControlView()
        {
            return new LogControlView();
        }
        #endregion
    }
}
