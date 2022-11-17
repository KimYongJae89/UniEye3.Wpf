using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;

namespace UniScanC.Controls.ViewModels
{
    public class FrameImageControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public FrameImageControlViewModel() : base(typeof(FrameImageControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        private int moduleNo = 0;
        [LayoutControlViewModelPropertyAttribute]
        public int ModuleNo
        {
            get => moduleNo;
            set => Set(ref moduleNo, value);
        }

        private int imageCount = 10;
        [LayoutControlViewModelPropertyAttribute]
        public int ImageCount
        {
            get => imageCount;
            set => Set(ref imageCount, value);
        }
        #endregion


        #region 속성
        private ObservableCollection<BitmapSource> frameImageList = new ObservableCollection<BitmapSource>();
        public ObservableCollection<BitmapSource> FrameImageList
        {
            get => frameImageList;
            set => Set(ref frameImageList, value);
        }

        private int lastIndex = 0;
        #endregion


        #region 메서드
        public void OnUpdateModel(ModelBase modelBase)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateModel(modelBase)));
                return;
            }
            UpdateModel(modelBase);
        }

        public void UpdateModel(ModelBase modelBase)
        {
            lastIndex = 0;
            FrameImageList.Clear();
        }

        public void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateResult(productResults, taskCancelToken)));
                return;
            }
            UpdateResult(productResults, taskCancelToken);
        }

        public void UpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            if (productResults == null || productResults.Count() == 0)
            {
                FrameImageList.Clear();
                lastIndex = 0;
                return;
            }

            try
            {
                var filteredList = new List<InspectResult>();
                foreach (ProductResult productResult in productResults)
                {
                    if (productResult is InspectResult inspectResult)
                    {
                        if (inspectResult.ModuleNo == ModuleNo)
                        {
                            filteredList.Add(inspectResult);
                        }
                    }
                }

                if (filteredList.Count == 0)
                {
                    return;
                }

                foreach (InspectResult inspectResult in filteredList)
                {
                    if (inspectResult.FrameImageData != null && inspectResult.FrameIndex > lastIndex)
                    {
                        BitmapSource frameImage = inspectResult.FrameImageData.Clone();

                        int w = frameImage.PixelWidth;
                        int h = frameImage.PixelHeight;
                        byte[] b = new byte[w * h];
                        frameImage.CopyPixels(b, w, 0);

                        var newImage = BitmapSource.Create(w, h, frameImage.DpiX, frameImage.DpiY, frameImage.Format, frameImage.Palette, b, w);
                        newImage.Freeze();
                        FrameImageList.Add(newImage);

                        if (FrameImageList.Contains(null))
                        {
                            LogHelper.Error($"FrameImageCountrol::UpdateResult - Null exist in list");
                        }
                    }
                    else
                    {
                        if (inspectResult.FrameImageData == null)
                        {
                            LogHelper.Error($"FrameImageCountrol::UpdateResult FrameImageData is Null");
                        }

                        if (inspectResult.FrameIndex <= lastIndex)
                        {
                            LogHelper.Error($"FrameImageCountrol::UpdateResult InspectResult FrameIndex({inspectResult.FrameIndex}) is Smaller than LastIndex({lastIndex})");
                        }
                    }

                    if (taskCancelToken != null && taskCancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                lastIndex = filteredList.Max(x => x.FrameIndex);

                while (FrameImageList.Count > ImageCount)
                {
                    FrameImageList.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"FrameImageCountrol::UpdateResult FrameImage Exception : {ex.Message}");
            }
        }

        public override UserControl CreateControlView()
        {
            return new FrameImageControlView();
        }
        #endregion
    }
}
