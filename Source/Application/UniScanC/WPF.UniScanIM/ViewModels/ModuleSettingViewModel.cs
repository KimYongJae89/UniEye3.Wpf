using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Data;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class ModuleSettingViewModel : SystemConfigSettingViewModel
    {
        public ModuleSettingViewModel() { }

        public IEnumerable<Camera> Cameras => Override.DeviceManager.Instance().CameraHandler.cameraList;

        private ICommand addCommand;
        public ICommand AddCommand => (addCommand ?? (addCommand = new RelayCommand(Add)));

        private ICommand removeCommand;
        public ICommand RemoveCommand => (removeCommand ?? (removeCommand = new RelayCommand<ModuleInfo>(Remove)));

        private ObservableCollection<ModuleInfo> moduleList = new ObservableCollection<ModuleInfo>();
        public ObservableCollection<ModuleInfo> ModuleList
        {
            get => moduleList;
            set => Set(ref moduleList, value);
        }

        private void Add()
        {
            var module = new ModuleInfo();
            module.BufferWidth = module.Camera.ImageSize.Width;
            module.BufferHeight = module.Camera.ImageSize.Height;
            ModuleList.Add(module);
            SystemConfig.FlyoutSettingViewModelChanged = true;
        }

        private void Remove(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                return;
            }

            ModuleList.Remove(moduleInfo);

            SystemConfig.FlyoutSettingViewModelChanged = true;
        }
    }
}
