using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace DynMvp.Vision
{
    public enum ImageBandType
    {
        Luminance, Red, Green, Blue
    }

    public enum ImageFilterType
    {
        EdgeExtraction, AverageFilter, HistogramEqualization, Binarization, Erode, Dilate
    }

    public abstract class AlgoImage : IDisposable
    {
        public ImageType ImageType { get; set; }
        public ImagingLibrary LibraryType { get; set; }

        public byte[] GetByte()
        {
            return CloneByte();
        }

        public void SetByte(byte[] data)
        {
            PutByte(data);
        }

        public SizeF Size => new SizeF(Width, Height);

        public abstract byte[] CloneByte();
        public abstract void CopyByte(byte[] imageBuffer);
        public abstract void PutByte(byte[] data);
        public abstract IntPtr GetPtr();
        public abstract int Width { get; }
        public abstract void Dispose();
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public abstract AlgoImage Clone();
        public abstract ImageD ToImageD();
        public abstract Bitmap ToBitmap();
        public abstract AlgoImage Clip(Rectangle rectangle);
        public abstract void Save(string fileName, DebugContext debugContext = null);
        public abstract void Clear(byte initVal = 0);
        public abstract AlgoImage GetChildImage(Rectangle rectangle);
        public List<ImageFilterType> FilteredList { get; set; } = new List<ImageFilterType>();

        public bool CheckFilterd(ImageFilterType imageFilterType)
        {
            return FilteredList.Contains(imageFilterType);
        }

        public abstract AlgoImage ChangeImageType(ImagingLibrary imagingLibrary);

        public BitmapSource ToBitmapSource()
        {
            System.Windows.Media.PixelFormat pixelFormat;
            int bufferSize, stride;
            switch (ImageType)
            {
                case ImageType.Grey:
                    pixelFormat = System.Windows.Media.PixelFormats.Gray8;
                    bufferSize = Pitch * Height;
                    stride = Pitch;
                    break;

                case ImageType.Color:
                    pixelFormat = System.Windows.Media.PixelFormats.Rgb24;
                    bufferSize = Pitch * Height * 3;
                    stride = Pitch * 3;
                    break;

                default:
                    throw new NotSupportedException();
            }

            BitmapSource bitmapSource;

            //LogHelper.Debug(LoggerType.Operation, $"AlgoImage::ToBitmapSource: W: {Width}, H: {Height}");
            IntPtr ptr = GetPtr();
            if (ptr != IntPtr.Zero)
            {
                bitmapSource = BitmapSource.Create(Width, Height, 96, 96, pixelFormat, null, ptr, bufferSize, stride);
            }
            else
            {
                bitmapSource = BitmapSource.Create(Width, Height, 96, 96, pixelFormat, null, GetByte(), stride);
            }

            bitmapSource.Freeze();
            return bitmapSource;
        }
        //public BitmapSource ToBitmapSource(Size size)
        //{
        //    System.Windows.Media.PixelFormat pixelFormat;
        //    int bufferSize, stride;
        //    switch (this.imageType)
        //    {
        //        case ImageType.Grey:
        //            pixelFormat = System.Windows.Media.PixelFormats.Gray8;
        //            stride = size.Width;
        //            bufferSize = size.Width * size.Height;
        //            break;

        //        case ImageType.Color:
        //            pixelFormat = System.Windows.Media.PixelFormats.Rgb24;
        //            stride = size.Width * 3;
        //            bufferSize = size.Width * size.Height * 3;
        //            break;

        //        default:
        //            throw new NotSupportedException();
        //    }

        //    BitmapSource bitmapSource;

        //    IntPtr ptr = GetPtr();
        //    if (ptr != IntPtr.Zero)
        //        bitmapSource = BitmapSource.Create(size.Width, size.Height, 96, 96, pixelFormat, null, ptr, bufferSize, stride);
        //    else
        //        bitmapSource = BitmapSource.Create(size.Width, size.Height, 96, 96, pixelFormat, null, GetByte(), stride);
        //    bitmapSource.Freeze();
        //    return bitmapSource;
        //}

        //public BitmapSource ToBitmapSource()
        //{
        //    return ToBitmapSource(new System.Drawing.Size(this.Width, this.Height));
        //}
    }
}
