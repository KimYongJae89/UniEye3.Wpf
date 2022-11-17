using DynMvp.Base;
using Euresys.Open_eVision_1_2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Euresys
{
    public class OpenEVisionGreyImage : AlgoImage
    {
        public OpenEVisionGreyImage()
        {
            LibraryType = ImagingLibrary.EuresysOpenEVision;
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

        }

        public override AlgoImage Clone()
        {
            var cloneImage = new OpenEVisionGreyImage();
            cloneImage.Image = new EImageBW8();
            cloneImage.Image.SetSize(Image.Width, Image.Height);

            EasyImage.Copy(Image, cloneImage.Image);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            DrawingHelper.Arrange(rectangle, new Size(Image.Width, Image.Height));

            var roi = new EROIBW8();
            roi.Attach(Image);
            roi.SetPlacement(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            var clipImage = new OpenEVisionGreyImage();
            clipImage.Image = new EImageBW8();
            clipImage.Image.SetSize(rectangle.Width, rectangle.Height);

            EasyImage.Copy(roi, clipImage.Image);

            roi.Dispose();

            clipImage.FilteredList = FilteredList;

            return clipImage;
        }
        public EImageBW8 Image { get; set; }

        public override int Width => Image.Width;

        public override int Pitch => Image.Width;

        public override int Height => Image.Height;

        public override ImageD ToImageD()
        {
            return OpenEVisionImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext.SaveDebugImage == false)
            {
                return;
            }

            Directory.CreateDirectory(debugContext.Path);

            string filePath = Path.Combine(debugContext.Path, fileName);
            Image.Save(filePath, EImageFileType.Bmp);
        }

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

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
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

    public class OpenEVisionColorImage : AlgoImage
    {
        public OpenEVisionColorImage()
        {
            LibraryType = ImagingLibrary.EuresysOpenEVision;
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

        }

        public override AlgoImage Clone()
        {
            var cloneImage = new OpenEVisionColorImage();
            cloneImage.Image = new EImageC24();
            cloneImage.Image.SetSize(Image.Width, Image.Height);

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var cloneImage = new OpenEVisionColorImage();
            cloneImage.Image = new EImageC24();
            cloneImage.Image.SetSize(rectangle.Width, rectangle.Height);

            return cloneImage;
        }
        public EImageC24 Image { get; set; }

        public override int Width => Image.Width;

        public override int Pitch => Image.Width;

        public override int Height => Image.Height;

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
            return OpenEVisionImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext.SaveDebugImage == false)
            {
                return;
            }

            Directory.CreateDirectory(debugContext.Path);

            string filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
            Image.Save(filePath, EImageFileType.Bmp);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
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
