using DynMvp.Base;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvGreyImage : AlgoImage
    {
        public OpenCvGreyImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;
        }

        public override void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }
        }

        public override void Clear(byte initVal = 0)
        {
            Image.SetValue(new MCvScalar(initVal));
        }

        public override AlgoImage Clone()
        {
            var cloneImage = new OpenCvGreyImage { Image = Image.Clone() };
            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var clipImage = new OpenCvGreyImage { Image = Image.Copy(rectangle) };
            return clipImage;
        }

        public Image<Gray, byte> Image { get; set; }

        public override int Width => Image.Width;

        public override int Height => Image.Height;

        public override int Pitch => Image.Data.GetUpperBound(1) + 1;

        public override byte[] CloneByte()
        {
            return Image.Bytes;
        }

        public override void CopyByte(byte[] imageBuffer)
        {
        }

        public override void PutByte(byte[] data)
        {
            Image.Bytes = data;
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext != null)
            {
                if (debugContext.SaveDebugImage == false)
                {
                    return;
                }

                Directory.CreateDirectory(debugContext.Path);

                string filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
                Image.Save(filePath);
            }
            else
            {
                Image.Save(fileName);
            }
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                default:
                    var cloneImage = new OpenCvGreyImage { Image = Image.Clone() };
                    return cloneImage;
            }
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            return Clip(rectangle);
        }

        public override Bitmap ToBitmap()
        {
            throw new NotImplementedException();
        }

        public override IntPtr GetPtr()
        {
            //TODO:[김태현] image.Ptr로 리턴하니까 보호된 메모리로 뜸.(why?) 대신 GetByte()로 얻는건 잘 되서 그렇게 사용중
            //return image.Ptr;
            return IntPtr.Zero;
        }
    }

    public class OpenCvDepthImage : AlgoImage
    {
        public OpenCvDepthImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;
        }

        public override void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }
        }

        public override void Clear(byte initVal = 0)
        {
            Image.SetValue(new MCvScalar(initVal));
        }

        public override AlgoImage Clone()
        {
            var cloneImage = new OpenCvDepthImage { Image = Image.Clone() };
            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var clipImage = new OpenCvDepthImage { Image = Image.Copy(rectangle) };
            return clipImage;
        }
        public Image<Gray, float> Image { get; set; }

        public override int Width => Image.Width;

        public override int Pitch => Image.Data.GetUpperBound(1) + 1;

        public override int Height => Image.Height;

        public override byte[] CloneByte()
        {
            return Image.Bytes;
        }

        public override void CopyByte(byte[] imageBuffer)
        {
        }
        public override void PutByte(byte[] data)
        {
            Image.Bytes = data;
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext != null)
            {
                if (debugContext.SaveDebugImage == false)
                {
                    return;
                }

                Directory.CreateDirectory(debugContext.Path);

                string filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
                Image.Save(filePath);
            }
            else
            {
                Image.Save(fileName);
            }
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            var cloneImage = new OpenCvDepthImage();
            cloneImage.Image = Image.Clone();

            return cloneImage;
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.GetChildImage] float data type is not support");
        }

        public override Bitmap ToBitmap()
        {
            throw new NotImplementedException();
        }

        public override IntPtr GetPtr()
        {
            throw new NotImplementedException();
        }
    }

    public class OpenCvColorImage : AlgoImage
    {
        public OpenCvColorImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;
        }

        public override void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }
        }

        public override void Clear(byte initVal = 0)
        {
            Image.SetValue(new Bgr(initVal, initVal, initVal));
        }

        public override AlgoImage Clone()
        {
            var cloneImage = new OpenCvColorImage
            {
                Image = Image.Clone(),
                FilteredList = FilteredList
            };
            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var clipImage = new OpenCvColorImage
            {
                Image = Image.Copy(rectangle),
                FilteredList = FilteredList
            };

            return clipImage;
        }
        public Image<Bgr, byte> Image { get; set; }

        public override int Width => Image.Width;

        public override int Height => Image.Height;

        public override int Pitch => Image.Data.GetUpperBound(1) + 1;

        public override byte[] CloneByte()
        {
            throw new NotImplementedException();
        }
        public override void CopyByte(byte[] imageBuffer)
        {
        }
        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext != null)
            {
                if (debugContext.SaveDebugImage == false)
                {
                    return;
                }

                Directory.CreateDirectory(debugContext.Path);

                string filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
                Image.Save(filePath);
            }
            else
            {
                Image.Save(fileName);
            }
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.GetChildImage] float data type is not support");
        }

        public override Bitmap ToBitmap()
        {
            throw new NotImplementedException();
        }

        public override IntPtr GetPtr()
        {
            throw new NotImplementedException();
        }
    }
}
