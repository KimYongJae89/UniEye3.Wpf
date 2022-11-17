using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.ThicknessMeasure.Controls.Views;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Controls.ViewModels
{
    public class MotionControlViewModel : Observable
    {
        #region 속성
        public System.Windows.Input.ICommand ResetCommand { get; }

        public System.Windows.Input.ICommand SpeedSettingCommand { get; }

        public System.Windows.Input.ICommand ServoCommand { get; }

        public System.Windows.Input.ICommand HomeCommand { get; }

        public System.Windows.Input.ICommand MoveCommand { get; }

        public System.Windows.Input.ICommand JogPlusDownCommand { get; }

        public System.Windows.Input.ICommand JogMinusDownCommand { get; }

        public System.Windows.Input.ICommand JogUpCommand { get; }

        private SystemConfig SystemConfig => SystemConfig.Instance;

        private AxisHandler AxisHandler => DeviceManager.Instance().RobotStage;

        private Timer AxisStateTimer { get; }

        private bool isServoOn = false;
        public bool IsServoOn
        {
            get => isServoOn;
            set => Set(ref isServoOn, value);
        }

        private bool isOnAlarm = false;
        public bool IsOnAlarm
        {
            get => isOnAlarm;
            set => Set(ref isOnAlarm, value);
        }

        private string currentPosition = "0.000";
        public string CurrentPosition
        {
            get => currentPosition;
            set => Set(ref currentPosition, value);
        }

        private float targetPosition = 0;
        public float TargetPosition
        {
            get => targetPosition;
            set => Set(ref targetPosition, value);
        }
        #endregion


        #region 생성자
        public MotionControlViewModel()
        {
            ResetCommand = new RelayCommand(ResetCommandAction);
            SpeedSettingCommand = new RelayCommand(SpeedCommandAction);
            ServoCommand = new RelayCommand(ServoCommandAction);
            HomeCommand = new RelayCommand(HomeCommandAction);
            MoveCommand = new RelayCommand(MoveCommandAction);
            JogPlusDownCommand = new RelayCommand(JogPlusDownCommandAction);
            JogMinusDownCommand = new RelayCommand(JogMinusCommandAction);
            JogUpCommand = new RelayCommand(JogUpCommandAction);

            if (AxisHandler != null)
            {
                AxisStateTimer = new Timer(100);
                AxisStateTimer.Elapsed += AxisStateTimer_Elapsed;
                AxisStateTimer.Start();
            }
        }

        private void AxisStateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsServoOn = AxisHandler.IsAllServoOn();
            IsOnAlarm = AxisHandler.IsAmpFault().ToList().Count(x => x == true) > 0;
            CurrentPosition = (AxisHandler.GetActualPos().Position[0] / 1000f).ToString("N3");
        }
        #endregion


        #region 메서드
        private void ResetCommandAction()
        {
            AxisHandler.ResetAlarm();
        }

        private async void SpeedCommandAction()
        {
            var view = new SpeedSettingControlView();
            bool result = await MessageWindowHelper.ShowChildWindow<bool>(view);
            if (result == true)
            {
                var speedSetting = view.DataContext as SpeedSettingControlViewModel;
                SystemConfig.HomeStartSpeed = speedSetting.HomeStartSpeed;
                SystemConfig.HomeEndSpeed = speedSetting.HomeEndSpeed;
                SystemConfig.MovingSpeed = speedSetting.MovingSpeed;
                SystemConfig.JogSpeed = speedSetting.JogSpeed;
                SystemConfig.MeasureSecond = speedSetting.MeasureSecond;

                SystemConfig.Save();
            }
        }

        private void ServoCommandAction()
        {
            bool isOn = AxisHandler.IsAllServoOn();
            AxisHandler.TurnOnServo(!isOn);
        }

        private async void HomeCommandAction()
        {
            SetSpeed();

            var progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource.Step = 1;
            progressSource.Range = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            await MessageWindowHelper.ShowProgress(
                "Motion Contol",
                "Home Moving...",
                new Action(() => AxisHandler.HomeMove(progressSource.CancellationTokenSource.Token)), true, progressSource);
        }

        private async void MoveCommandAction()
        {
            SetSpeed();

            var progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource.Step = 1;
            progressSource.Range = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            await MessageWindowHelper.ShowProgress(
                "Motion Contol",
                "Moving...",
                new Action(() =>
                {
                    var axixPosition = new AxisPosition(TargetPosition * 1000f);
                    AxisHandler.Move(axixPosition, progressSource.CancellationTokenSource.Token);
                }), true, progressSource);
        }

        private void JogPlusDownCommandAction()
        {
            SetSpeed();
            AxisHandler.StartContinuousMove();
        }

        private void JogMinusCommandAction()
        {
            AxisHandler.StartContinuousMove(true);
        }

        private void JogUpCommandAction()
        {
            AxisHandler.StopMove();
        }

        private void SetSpeed()
        {
            AxisHandler.GetUniqueAxis("X").AxisParam.HomeSpeed.HighSpeed.MaxVelocity = SystemConfig.HomeStartSpeed * 1000;
            AxisHandler.GetUniqueAxis("X").AxisParam.HomeSpeed.FineSpeed.MaxVelocity = SystemConfig.HomeEndSpeed * 1000;
            AxisHandler.GetUniqueAxis("X").AxisParam.MovingParam.MaxVelocity = SystemConfig.MovingSpeed * 1000;
            AxisHandler.GetUniqueAxis("X").AxisParam.JogParam.MaxVelocity = SystemConfig.JogSpeed * 1000;
        }
        #endregion
    }
}