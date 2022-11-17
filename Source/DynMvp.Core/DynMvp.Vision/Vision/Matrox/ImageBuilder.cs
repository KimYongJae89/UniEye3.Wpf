using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Matrox
{
    public class MilImageBuilder : ImageBuilder
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            AlgoImage algoImage = null;

            switch (imageType)
            {
                case ImageType.Grey:
                    var milGreyImage = new MilGreyImage();
                    milGreyImage.Alloc(width, height);
                    //MIL.MbufClear(milGreyImage.Image, MIL.M_COLOR_BLACK);
                    algoImage = milGreyImage;
                    break;
                case ImageType.Color:
                    var milColorImage = new MilColorImage();
                    milColorImage.Alloc(width, height);
                    MIL.MbufClear(milColorImage.Image, MIL.M_COLOR_BLACK);
                    algoImage = milColorImage;
                    break;
                case ImageType.Depth:
                    var milDepthImage = new MilDepthImage();
                    milDepthImage.Alloc(width, height);
                    MIL.MbufClear(milDepthImage.Image, MIL.M_COLOR_BLACK);
                    algoImage = milDepthImage;
                    break;
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
                case ImageType.Grey:
                    algoImage = BuildGreyImage(image, imageBand);
                    break;
                case ImageType.Color:
                    algoImage = BuildColorImage(image);
                    break;
                case ImageType.Depth:
                    algoImage = BuildDepthImage(image);
                    break;
                default:
                    throw new InvalidTypeException();
            }

            return algoImage;
        }

        private AlgoImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            if (image.NumBand == 3 || image.NumBand == 4)
            {
                MilColorImage milColorImage = ConvertColorImage(image);

                return milColorImage.Clone(imageBand);
            }
            else
            {
                return ConvertGreyImage(image);
            }
        }

        private AlgoImage BuildDepthImage(ImageD image)
        {
            return ConvertDepthImage(image);
        }

        private MilGreyImage ConvertGreyImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;

            var image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            var milGreyImage = new MilGreyImage();

            if (image2d.IsUseIntPtr())
            {
                milGreyImage.Alloc(width, height, image2d.ImageData.DataPtr, image.Pitch);
                //milGreyImage.DataPtr = image2d.DataPtr;
            }
            else
            {
                if (imageBuf == null)
                {
                    throw new InvalidTypeException();
                }

                milGreyImage.Alloc(width, height);

                int pitch = milGreyImage.Pitch;

                byte[] milBuf = new byte[width * height];

                Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width));

                //for (int y = 0; y < height; y++)
                //    Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width);

                milGreyImage.Put(milBuf);
            }

            return milGreyImage;
        }

        private MilDepthImage ConvertDepthImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;

            var milDepthImage = new MilDepthImage();
            milDepthImage.Alloc(width, height);

            var image3d = (Image3D)image;
            float[] imageBuf = image3d.Data;

            float[] milBuf = new float[width * height];

            Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width));

            //for (int y = 0; y < height; y++)
            //    Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width);

            milDepthImage.Put(milBuf);

            return milDepthImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            return ConvertColorImage(image);
        }

        private MilColorImage ConvertColorImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;
            int bufPitch = width * 3; //  milColorImage.Pitch;

            var milColorImage = new MilColorImage();
            milColorImage.Alloc(width, height);

            var image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            byte[] milBuf = new byte[bufPitch * height];

            if (image.NumBand == 3)
            {
                Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, milBuf, bufPitch * y, width * 3));
            }
            else
            {
                Parallel.For(0, height, y =>
                //for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int srcOffsetByte = image.Pitch * y + image.NumBand * x;
                        int dstOffsetByte = bufPitch * y + 3 * x;
                        Array.Copy(imageBuf, srcOffsetByte, milBuf, dstOffsetByte, 3);
                    }
                });
            }
            //for (int y = 0; y < height; y++)
            //    Array.Copy(imageBuf, image.Pitch * y, milBuf, bufPitch * y, width*3);

            milColorImage.Put(milBuf);

            return milColorImage;
        }

        public static ImageD ConvertImage(MilGreyImage milGreyImage)
        {
            if (milGreyImage == null || milGreyImage.Image == null)
            {
                return null;
            }

            int width = milGreyImage.Width;
            int height = milGreyImage.Height;
            int pitch = milGreyImage.Pitch;
            int size = width * height;

            var image = new Image2D();
            image.Initialize(width, height, 1);

            byte[] imageBuf = image.Data;
            byte[] milBuf = new byte[width * height];// milGreyImage.GetByte();

            milGreyImage.Get(milBuf);

            Parallel.For(0, height, y => Array.Copy(milBuf, width * y, imageBuf, image.Pitch * y, width));

            //Array.Copy(milBuf, imageBuf, imageBuf.Length);

            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        imageBuf[y * image.Pitch + x] = milBuf[y * width + x];
            //    }
            //}

            return image;
        }

        public static ImageD ConvertImage(MilDepthImage milDepthImage)
        {
            if (milDepthImage == null || milDepthImage.Image == null)
            {
                return null;
            }

            int width = milDepthImage.Width;
            int height = milDepthImage.Height;

            int size = width * height;

            var image = new Image3D();
            image.Initialize(width, height, 1);

            float[] imageBuf = image.Data;
            float[] milBuf = new float[width * height];

            milDepthImage.Get(milBuf);

            Array.Copy(milBuf, imageBuf, width * height);

            return image;
        }

        public static ImageD ConvertImage(MilColorImage milColorImage)
        {
            if (milColorImage == null || milColorImage.Image == null)
            {
                return null;
            }

            int width = milColorImage.Width;
            int height = milColorImage.Height;

            int size = width * height * 3;

            var image = new Image2D();
            image.Initialize(width, height, 3);

            byte[] imageBuf = image.Data;
            byte[] milBuf = new byte[size];

            milColorImage.Get(milBuf);

            Array.Copy(milBuf, imageBuf, size);

            return image;
        }
    }
}
