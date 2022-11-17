using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class SystemSettingViewModel : SystemConfigSettingViewModel
    {
        private double windowWidth = 1000;
        public double WindowWidth
        {
            get => windowWidth;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref windowWidth, value);
        }

        private double windowHeight = 800;
        public double WindowHeight
        {
            get => windowHeight;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref windowHeight, value);
        }

        private bool isSaveFrameImage = false;
        public bool IsSaveFrameImage
        {
            get => isSaveFrameImage;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref isSaveFrameImage, value);
        }

        private bool isSaveDefectImage = false;
        public bool IsSaveDefectImage
        {
            get => isSaveDefectImage;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref isSaveDefectImage, value);
        }

        private bool isSaveDebugData = false;
        public bool IsSaveDebugData
        {
            get => isSaveDebugData;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref isSaveDebugData, value);
        }

        private int isSaveDebugDataCount = 0;
        public int IsSaveDebugDataCount
        {
            get => isSaveDebugDataCount;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref isSaveDebugDataCount, value);
        }
    }
}
