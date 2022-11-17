using MahApps.Metro.SimpleChildWindow;
using System;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.Melsec;
using UniEye.Base.MachineInterface.TcpIp;

namespace WPF.UniScanCM.PLC
{
    public class PlcSettingWindowViewModel : Observable
    {
        #region 생성자
        public PlcSettingWindowViewModel(MachineIfSetting machineIfSetting)
        {
            MachineIfSetting = machineIfSetting;

            OkCommand = new RelayCommand<ChildWindow>(OkCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);
        }
        #endregion


        #region 속성
        public MachineIfSetting MachineIfSetting { get; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }
        #endregion


        #region 메서드
        private void OkCommandAction(ChildWindow wnd)
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
