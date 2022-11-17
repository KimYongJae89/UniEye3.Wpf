using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Windows.ViewModels
{
    public class AlarmMessageWindowViewModel : Observable
    {
        #region 생성자
        public AlarmMessageWindowViewModel()
        {
            OkCommand = new RelayCommand<Window>(OkCommandAction);
            BuzzerOffCommand = new RelayCommand(BuzzerOffCommandAction);
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand OkCommand { get; }
        public System.Windows.Input.ICommand BuzzerOffCommand { get; }

        private string alarmCode;
        public string AlarmCode
        {
            get => alarmCode;
            set => Set(ref alarmCode, value);
        }

        private int alarmLevel;
        public int AlarmLevel
        {
            get => alarmLevel;
            set => Set(ref alarmLevel, value);
        }

        private string alarmMessage;
        public string AlarmMessage
        {
            get => alarmMessage;
            set => Set(ref alarmMessage, value);
        }

        private bool alarmOn = true;
        public bool AlarmOn
        {
            get => alarmOn;
            set => Set(ref alarmOn, value);
        }
        #endregion


        #region 메서드
        private void OkCommandAction(Window wnd)
        {
            ErrorManager.Instance().ResetAlarm();
            wnd.DialogResult = true;
        }

        private void BuzzerOffCommandAction()
        {
            ErrorManager.Instance().BuzzerOn = false;
        }
        #endregion
    }
}
