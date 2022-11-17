using Authentication.Core;
using Authentication.Core.Datas;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Models;
using Unieye.WPF.Base.Services;
using ICommand = System.Windows.Input.ICommand;
using UserControl = System.Windows.Controls.UserControl;

namespace Unieye.WPF.Base.Controls
{
    public class FigureDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PointDataTemaplate { get; set; }
        public DataTemplate LineDataTemaplate { get; set; }
        public DataTemplate RectangleDataTemaplate { get; set; }
        public DataTemplate EllipseDataTemaplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FigureModel figure)
            {
                switch (figure.FigureType)
                {
                    case FigureType.Point:
                        return PointDataTemaplate;
                    case FigureType.Line:
                        return LineDataTemaplate;
                    case FigureType.Rectangle:
                        return RectangleDataTemaplate;
                    case FigureType.Ellipse:
                        return EllipseDataTemaplate;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// ImageCanvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageCanvas : UserControl, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public static readonly DependencyProperty ImageButtonEnabledProperty = DependencyProperty.Register(
            "ImageButtonEnabled", typeof(bool), typeof(ImageCanvas),
            new FrameworkPropertyMetadata(true));

        public bool ImageButtonEnabled
        {
            get => (bool)GetValue(ImageButtonEnabledProperty);
            set => SetValue(ImageButtonEnabledProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(ImageCanvas),
            new FrameworkPropertyMetadata(UpdateImageSource));

        private static void UpdateImageSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var local = d as ImageCanvas;
            if (local.StretchEachUpdate || e.OldValue == null)
            {
                local.FitToSize();
            }
        }

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty ShapeSourceProperty = DependencyProperty.Register(
            "ShapeSource", typeof(IEnumerable<FigureModel>), typeof(ImageCanvas));

        public IEnumerable<FigureModel> ShapeSource
        {
            get => (IEnumerable<FigureModel>)GetValue(ShapeSourceProperty);
            set => SetValue(ShapeSourceProperty, value);
        }

        public static readonly DependencyProperty ZoomServiceProperty = DependencyProperty.Register(
            "ZoomService", typeof(ZoomService), typeof(ImageCanvas));

        public ZoomService ZoomService
        {
            get => (ZoomService)GetValue(ZoomServiceProperty);
            set => SetValue(ZoomServiceProperty, value);
        }

        public static readonly DependencyProperty StretchEachUpdateProperty = DependencyProperty.Register(
            "StretchEachUpdate", typeof(bool), typeof(ImageCanvas),
            new PropertyMetadata(false));

        public bool StretchEachUpdate
        {
            get => (bool)GetValue(StretchEachUpdateProperty);
            set => SetValue(StretchEachUpdateProperty, value);
        }

        private Visibility visibleShape = Visibility.Visible;
        public Visibility VisibleShape
        {
            get => visibleShape;
            set => Set(ref visibleShape, value);
        }

        private ICommand fitToSizeCommand;
        public ICommand FitToSizeCommand => fitToSizeCommand ?? (fitToSizeCommand = new RelayCommand(FitToSize));

        private void FitToSize()
        {
            if (ImageSource != null)
            {
                ZoomService.FitToSize(ImageSource.Width, ImageSource.Height);
            }
        }

        private ICommand openImageCommand;
        public ICommand OpenImageCommand => openImageCommand ?? (openImageCommand = new RelayCommand(OpenImage));

        private async void OpenImage()
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var bmp = ImageHelper.LoadImage(dlg.FileName) as Bitmap;
                    ImageSource = ImageHelper.BitmapToBitmapSource(bmp);
                    bmp.Dispose();
                    //ImageSource = new BitmapImage(new Uri(dlg.FileName));
                    FitToSize();
                }
                catch (Exception)
                {
                    await MessageWindowHelper.ShowMessageBox("이미지 로드 에러", "이미지 파일이 아닙니다");
                }
            }
        }

        private ICommand saveImageCommand;
        public ICommand SaveImageCommand => saveImageCommand ?? (saveImageCommand = new RelayCommand(SaveImage));

        private void SaveImage()
        {
            if (ImageSource == null)
            {
                return;
            }

            var dlg = new SaveFileDialog();
            dlg.DefaultExt = "bmp";
            dlg.AddExtension = true;
            dlg.Filter = "BMP|*.bmp|JPG|*.jpg|PNG|*.png";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (var fileStream = new FileStream(dlg.FileName, FileMode.Create))
                {
                    var bitmapImage = ImageSource as BitmapSource;

                    BitmapEncoder encoder;
                    switch (System.IO.Path.GetExtension(dlg.FileName).ToUpper())
                    {
                        case ".JPG":
                        case ".JPEG":
                            encoder = new JpegBitmapEncoder();
                            ((JpegBitmapEncoder)encoder).QualityLevel = 100;
                            break;
                        case ".PNG":
                            encoder = new PngBitmapEncoder();
                            break;
                        case ".BMP":
                        default:
                            encoder = new BmpBitmapEncoder();
                            break;
                    }

                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(fileStream);
                }
            }
        }

        private ICommand showDefectCommand;
        public ICommand ShowDefectCommand => showDefectCommand ?? (showDefectCommand = new RelayCommand(ShowDefect));

        private void ShowDefect()
        {
            if (VisibleShape == Visibility.Visible)
            {
                VisibleShape = Visibility.Hidden;
            }
            else
            {
                VisibleShape = Visibility.Visible;
            }
        }

        public ImageCanvas()
        {
            InitializeComponent();
            ZoomService = new ZoomService(imageCanvas);
            imageCanvas.IsVisibleChanged += ImageCanvas_IsVisibleChanged;
            imageCanvas.SizeChanged += ImageCanvas_SizeChanged;
        }

        //스타트되어있는지 가져와서,, or 쳐가지고 isEnable에 바인딩걸어야함
        private void OnInspectionStarted(bool isStarted)
        {
            IsEnable = IsTeachUser & !isStarted;
        }

        //실제로 바인딩 걸어야할 것
        private bool isEnable;
        public bool IsEnable
        {
            get => isEnable;
            set => Set(ref isEnable, value);
        }
        //현재유저에 따라 
        private bool isTeachUser;
        public bool IsTeachUser
        {
            get => isTeachUser;
            set => Set(ref isTeachUser, value);
        }

        private void ImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (StretchEachUpdate)
            {
                FitToSize();
            }
        }

        private void ImageCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && StretchEachUpdate)
            {
                FitToSize();
            }
        }
    }
}
