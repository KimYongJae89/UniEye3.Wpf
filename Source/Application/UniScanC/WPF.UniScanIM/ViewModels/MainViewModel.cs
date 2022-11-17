using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Services;
using UniEye.Base.Config;
using UniScanC.Data;
using WPF.UniScanIM.Inspect;
using WPF.UniScanIM.Override;
using SystemManager = WPF.UniScanIM.Override.SystemManager;

namespace WPF.UniScanIM.ViewModels
{
    public class MainViewModel : Observable
    {
        public Dictionary<int, BitmapSource> SourceImageList { get; } = new Dictionary<int, BitmapSource>();

        private ZoomService zoomService;
        public ZoomService ZoomService
        {
            get => zoomService;
            set => Set(ref zoomService, value);
        }

        private BitmapSource selectedSourceImage = null;
        public BitmapSource SelectedSourceImage
        {
            get => selectedSourceImage;
            set => Set(ref selectedSourceImage, value);
        }

        public MainViewModel()
        {
            // ShellWindowView::Initialize에서 대신 실행하는 방법으로 수정
            //ZoomService = new ZoomService(zoomElement);

            var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;
            inspectRunner.Grabbed += Grabbed;

            SystemConfig.Instance.SelectedModuleInfoChangedDelegate += SelectedModuleInfoChangedAction;
            SystemConfig.Instance.SaveSelectedSourceImage += SaveSelectedImage;
        }

        private void SelectedModuleInfoChangedAction(ModuleInfo SelectedModuleInfo)
        {
            if (SelectedModuleInfo == null)
            {
                return;
            }

            if (!SourceImageList.ContainsKey(SelectedModuleInfo.ModuleNo))
            {
                SourceImageList.Add(SelectedModuleInfo.ModuleNo, null);
            }

            SelectedSourceImage = GetScaledImage(SourceImageList[SelectedModuleInfo.ModuleNo]);
        }

        public void Grabbed(int moduleNo, ulong frameNo, BitmapSource bitmapSource)
        {
            SetSourceImage(moduleNo, bitmapSource);
        }

        private void SetSourceImage(int moduleNo, BitmapSource bitmapSource)
        {
            if (!SourceImageList.ContainsKey(moduleNo))
            {
                SourceImageList.Add(moduleNo, null);
            }

            SourceImageList[moduleNo] = bitmapSource;

            if (SystemConfig.Instance.SelectedModuleInfo != null && moduleNo == SystemConfig.Instance.SelectedModuleInfo.ModuleNo)
            {
                SelectedSourceImage = GetScaledImage(bitmapSource);
            }
        }

        private BitmapSource GetScaledImage(BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
            {
                return null;
            }

            if (Math.Max(bitmapSource.PixelWidth, bitmapSource.PixelHeight) <= 2048)
            {
                return bitmapSource.Clone();
            }

            double scale = 2048.0 / Math.Max(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            BitmapSource newBitmapSource = new TransformedBitmap(bitmapSource, new ScaleTransform(scale, scale));
            newBitmapSource.Freeze();
            return newBitmapSource;
        }

        public void SaveSelectedImage(ModuleInfo SelectedModuleInfo, string file)
        {
            BitmapSource bitmapSource = SourceImageList[SelectedModuleInfo.ModuleNo];
            UniScanC.Inspect.BmpImaging.SaveBitmapSource(bitmapSource, file);
        }
    }
}
