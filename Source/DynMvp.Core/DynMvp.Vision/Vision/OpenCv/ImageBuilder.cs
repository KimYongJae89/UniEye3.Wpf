using DynMvp.Base;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvImageBuilder : ImageBuilder
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            AlgoImage algoImage = null;
            Bitmap bitmap = null;
            ImageD image2D = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    image2D = Image2D.ToImage2D(bitmap);
                    algoImage = BuildGreyImage(image2D, ImageBandType.Luminance);
                    break;
                case ImageType.Color:
                    bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    image2D = Image2D.ToImage2D(bitmap);
                    algoImage = BuildColorImage(image2D);
                    break;
                case ImageType.Depth:
                    throw new NotImplementedException();
                default:
                    throw new InvalidTypeException();
            }

            return algoImage;
        }

        public override AlgoImage Build(ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgoImage algoImage = null;

            switch (imageType)
            {
                case ImageType.Grey: algoImage = BuildGreyImage(image, imageBand); break;
                case ImageType.Color: algoImage = BuildColorImage(image); break;
                case ImageType.Depth: algoImage = BuildDepthImage(image); break;
                default: throw new InvalidTypeException();
            }

            return algoImage;
        }

        private AlgoImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            var openCvGreyImage = new OpenCvGreyImage();

            var image2d = (Image2D)image;

            if (image.NumBand == 3 || image.NumBand == 4)
            {
                var colorImage = new Image<Bgr, byte>(image2d.Width, image2d.Height);
                byte[] imageBytes = colorImage.Bytes;

                int imagePitch = colorImage.Data.GetUpperBound(1) + 1;

                if (image2d.NumBand == 3)
                {
                    for (int y = 0; y < image2d.Height; y++)
                    {
                        Array.Copy(image2d.Data, y * image2d.Width * 3, imageBytes, y * imagePitch * 3, image2d.Width * 3);
                    }
                }
                else
                {
                    Parallel.For(0, image2d.Height, y =>
                    //for (int y = 0; y < image2d.Height; y++)
                    {
                        for (int x = 0; x < image2d.Width; x++)
                        {
                            int srcOffsetByte = (image2d.Pitch * y) + (image.NumBand * x);
                            int dstOffsetByte = (imagePitch * 3 * y) + (3 * x);
                            Array.Copy(image2d.Data, srcOffsetByte, imageBytes, dstOffsetByte, 3);
                        }
                    });
                }

                colorImage.Bytes = imageBytes;

                if (imageBand == ImageBandType.Luminance)
                {
                    openCvGreyImage.Image = colorImage.Convert<Gray, byte>();
                }
                else
                {
                    openCvGreyImage.Image = new Image<Gray, byte>(image2d.Width, image2d.Height);

                    var rgb = new VectorOfMat(3);
                    CvInvoke.Split(colorImage, rgb);

                    switch (imageBand)
                    {
                        case ImageBandType.Red: openCvGreyImage.Image = rgb[0].ToImage<Gray, byte>(); break;
                        case ImageBandType.Green: openCvGreyImage.Image = rgb[1].ToImage<Gray, byte>(); break;
                        case ImageBandType.Blue: openCvGreyImage.Image = rgb[2].ToImage<Gray, byte>(); break;
                    }
                }
            }
            else
            {
                var cvImage = new Image<Gray, byte>(image2d.Width, image2d.Height);
                byte[] imageBytes = cvImage.Bytes;

                int imagePitch = cvImage.Data.GetUpperBound(1) + 1;

                image2d.ConvertFromDataPtr();
                for (int y = 0; y < image2d.Height; y++)
                {
                    Array.Copy(
                        image2d.Data, y * image2d.Pitch,
                        imageBytes, y * cvImage.MIplImage.WidthStep /*imagePitch*/,
                        /*imagePitch*/image2d.Width);
                }
                cvImage.Bytes = imageBytes;
                if (image2d.Roi.IsEmpty == false)
                {
                    cvImage.ROI = image2d.Roi;
                }

                openCvGreyImage.Image = cvImage;
            }

            return openCvGreyImage;
        }

        private AlgoImage BuildDepthImage(ImageD image)
        {
            var openCvDepthImage = new OpenCvDepthImage();
            var image3d = (Image3D)image;
            openCvDepthImage.Image = new Image<Gray, float>(image3d.Width, image3d.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    openCvDepthImage.Image.Data[y, x, 0] = image3d.Data[(y * image.Width) + x];
                }
            }

            return openCvDepthImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            var openCvColorImage = new OpenCvColorImage();
            var image2d = (Image2D)image;
            var bitmap = image2d.ToBitmap();
            var cvImage = new Image<Bgr, byte>(bitmap) { ROI = image2d.Roi };

            //Image<Bgr, Byte> cvImage = new Image<Bgr, Byte>(image2d.Width, image2d.Height);
            //byte[] imageBytes = cvImage.Bytes;

            //int imagePitch = (cvImage.Data.GetUpperBound(1) + 1) * 3;

            //for (int y = 0; y < image2d.Height; y++)
            //{
            //    Array.Copy(image2d.Data, y * image2d.Width * 3, imageBytes, y * imagePitch, image2d.Width * 3);
            //}
            //cvImage.Bytes = imageBytes;
            openCvColorImage.Image = cvImage;

            return openCvColorImage;
        }

        public static ImageD ConvertImage(Image<Gray, byte> cvImage)
        {
            if (cvImage == null)
            {
                return null;
            }

            int width = cvImage.Width;
            int height = cvImage.Height;
            int band = cvImage.NumberOfChannels;
            int imagePitch = cvImage.MIplImage.WidthStep * cvImage.NumberOfChannels;

            var image = new Image2D();
            image.Initialize(width, height, band, imagePitch);
            image.SetData(cvImage.Bytes);

            return image;
        }

        public static ImageD ConvertImage(Image<Gray, float> cvImage)
        {
            if (cvImage == null)
            {
                return null;
            }

            int width = cvImage.Width;
            int height = cvImage.Height;
            int imagePitch = cvImage.Data.GetUpperBound(1) + 1;

            var image = new Image3D();
            image.Initialize(width, height, 1, imagePitch);
            image.SetData(cvImage.Bytes);

            return image;
        }

        public static ImageD ConvertImage(Image<Bgr, byte> cvImage)
        {
            if (cvImage == null)
            {
                return null;
            }

            int width = cvImage.Width;
            int height = cvImage.Height;
            int imagePitch = (cvImage.Data.GetUpperBound(1) + 1) * 3;

            var image = new Image2D();
            image.Initialize(width, height, 3, imagePitch);
            image.SetData(cvImage.Bytes);

            return image;
        }
    }
}
