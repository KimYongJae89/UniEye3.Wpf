using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Controls.ViewModels
{
    public class SpeedSettingControlViewModel : Observable
    {
        #region 필드
        private double homeStartSpeed = 100;
        private double homeEndSpeed = 10;
        private double jogSpeed = 200;
        private double movingSpeed = 200;
        private double measureSecond = 6;
        #endregion


        #region 속성
        public ICommand OKCommand { get; }

        public ICommand CancelCommand { get; }

        public double HomeStartSpeed { get => homeStartSpeed; set => Set(ref homeStartSpeed, value); }

        public double HomeEndSpeed { get => homeEndSpeed; set => Set(ref homeEndSpeed, value); }

        public double JogSpeed { get => jogSpeed; set => Set(ref jogSpeed, value); }

        public double MovingSpeed { get => movingSpeed; set => Set(ref movingSpeed, value); }

        public double MeasureSecond { get => measureSecond; set => Set(ref measureSecond, value); }
        #endregion


        #region 생성자
        public SpeedSettingControlViewModel()
        {
            OKCommand = new RelayCommand<ChildWindow>(OKCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            SystemConfig systemConfig = SystemConfig.Instance;
            HomeStartSpeed = systemConfig.HomeStartSpeed;
            HomeEndSpeed = systemConfig.HomeEndSpeed;
            JogSpeed = systemConfig.JogSpeed;
            MovingSpeed = systemConfig.MovingSpeed;
            MeasureSecond = systemConfig.MeasureSecond;
        }
        #endregion


        #region 메서드
        private void OKCommandAction(ChildWindow wnd)
        {
            wnd.Close(true);
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(false);
        }
        #endregion
    }
}
