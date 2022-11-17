using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using DynMvp.Base;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cognex
{
    public class CogImageSaver
    {
        public static void Save(ICogImage cogImage, string fileName, DebugContext debugContext)
        {
            string filePath;
            if (debugContext != null)
            {
                if (debugContext.SaveDebugImage == false)
                {
                    return;
                }

                if (Directory.Exists(debugContext.Path) == false)
                {
                    Directory.CreateDirectory(debugContext.Path);
                }

                filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
            }
            else
            {
                filePath = fileName;
            }

            Save(cogImage, filePath);
        }

        public static void Save(ICogImage cogImage, string filePath)
        {
            var imageFile = new CogImageFile();
            imageFile.Open(filePath, CogImageFileModeConstants.Write);
            imageFile.Append(cogImage);
            imageFile.Close();
        }
    }

    public class CognexGreyImage : AlgoImage
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public CognexGreyImage()
        {
            LibraryType = ImagingLibrary.CognexVisionPro;
            ImageType = ImageType.Grey;
        }

        public override void Dispose()
        {

        }

        public override void Clear(byte initVal = 0)
        {

        }

        public override AlgoImage Clone()
        {
            var cloneImage = new CognexGreyImage();
            cloneImage.Image = Image.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            CogImage8Grey cogImage = CogImageConvert.GetIntensityImage(Image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            var cloneImage = new CognexGreyImage();
            cloneImage.Image = cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override int Width => Image.Width;

        public override int Pitch => Image.Width;

        public override int Height => Image.Height;

        public override byte[] CloneByte()
        {
            byte[] byteArray = new byte[Image.Width * Image.Height];

            int index = 0;

            for (int yIndex = 0; yIndex < Image.Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < Image.Width; xIndex++)
                {
                    byteArray[index++] = Image.GetPixel(xIndex, yIndex);
                }
            }

            return byteArray;
        }

        public override void CopyByte(byte[] imageBuffer)
        {
        }

        public override void PutByte(byte[] data)
        {
            int index = 0;

            for (int yIndex = 0; yIndex < Image.Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < Image.Width; xIndex++)
                {
                    Image.SetPixel(xIndex, yIndex, data[index++]);
                }
            }
        }

        public override ImageD ToImageD()
        {
            return CognexImageBuilder.ConvertImage(Image);
        }
        public CogImage8Grey Image { get; set; }

        public override void Save(string fileName, DebugContext debugContext)
        {
            CogImageSaver.Save(Image, fileName, debugContext);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                case ImagingLibrary.OpenCv:

                    ICogImage8PixelMemory pixelMemory = Image.Get8GreyPixelMemory(CogImageDataModeConstants.Read, 0, 0, Image.Width, Image.Height);

                    var openCvGreyImage = new OpenCv.OpenCvGreyImage();
                    openCvGreyImage.Image = new Image<Gray, byte>(pixelMemory.Width, pixelMemory.Height);

                    for (int j = 0; j < Image.Height; j++)
                    {
                        for (int i = 0; i < Image.Width; i++)
                        {
                            openCvGreyImage.Image.Data[j, i, 0] = Image.GetPixel(i, j);
                        }
                    }

                    openCvGreyImage.FilteredList = FilteredList;

                    return openCvGreyImage;
                default:

                    return Clone();
            }
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

    public class CognexColorImage : AlgoImage
    {
        public CognexColorImage()
        {
            LibraryType = ImagingLibrary.CognexVisionPro;
            ImageType = ImageType.Color;
        }

        public override void Dispose()
        {

        }

        public override void Clear(byte initVal = 0)
        {

        }

        public override AlgoImage Clone()
        {
            var cloneImage = new CognexColorImage();
            cloneImage.Image = Image.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var cogImage = (CogImage24PlanarColor)CogImageConvert.GetRGBImage(Image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            var cloneImage = new CognexColorImage();
            cloneImage.Image = cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

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
            return CognexImageBuilder.ConvertImage(Image);
        }
        public CogImage24PlanarColor Image { get; set; }

        public override void Save(string fileName, DebugContext debugContext)
        {
            CogImageSaver.Save(Image, fileName, debugContext);
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
