using DynMvp.Devices.Dio;
using MahApps.Metro.SimpleChildWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.MachineInterface;
using UniScanC.Controls.Models;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class AddressSettingWindowViewModel : Observable
    {
        public List<MachineIfItemInfo> MachineIfItemInfos { get; set; } = new List<MachineIfItemInfo>();
        public int SelectedRowIndex { get; set; }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public AddressSettingWindowViewModel(MachineIfItemInfoList machineIfItemInfoList)
        {
            machineIfItemInfoList.ConvertDicToList();
            foreach (MachineIfItemInfo itemInfo in machineIfItemInfoList.DicList)
            {
                MachineIfItemInfos.Add(itemInfo.Clone());
            }

            OkCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(true));

            CancelCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(false));
        }
    }
}
