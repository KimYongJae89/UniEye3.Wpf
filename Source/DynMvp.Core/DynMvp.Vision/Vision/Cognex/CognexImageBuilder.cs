using Cognex.VisionPro;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cognex
{
    public class CognexImageBuilder : ImageBuilder
    {
        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            throw new NotImplementedException();
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
                default:
                    throw new InvalidTypeException();
            }

            return algoImage;
        }

        private AlgoImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            var cognexGreyImage = new CognexGreyImage();

            var bitmap = image.ToBitmap();

            if (image.NumBand == 3)
            {
                var cogColorImage = new CogImage24PlanarColor(bitmap);

                switch (imageBand)
                {
                    case ImageBandType.Luminance:
                        cognexGreyImage.Image = CogImageConvert.GetIntensityImage(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height);
                        break;
                    case ImageBandType.Red:
                        cognexGreyImage.Image = CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 1, 0, 0);
                        break;
                    case ImageBandType.Green:
                        cognexGreyImage.Image = CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 0, 1, 0);
                        break;
                    case ImageBandType.Blue:
                        cognexGreyImage.Image = CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 0, 0, 1);
                        break;
                }
            }
            else
            {
                cognexGreyImage.Image = new CogImage8Grey(bitmap);
            }

            bitmap.Dispose();

            return cognexGreyImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            var cognexColorImage = new CognexColorImage();


            if (image.NumBand == 3)
            {
                var bitmap = image.ToBitmap();
                cognexColorImage.Image = new CogImage24PlanarColor(bitmap);
                bitmap.Dispose();
            }
            else
            {
                Debug.Assert(false, "The image is not color image");
            }

            return cognexColorImage;
        }

        public static CogImage8Grey ConvertGreyImage(ImageD image)
        {
            return new CogImage8Grey(image.ToBitmap());
        }

        public static CogImage24PlanarColor ConvertColorImage(ImageD image)
        {
            return new CogImage24PlanarColor(image.ToBitmap());
        }

        public static ImageD ConvertImage(CogImage8Grey cogImage)
        {
            ICogImage8PixelMemory pixelMemory = cogImage.Get8GreyPixelMemory(CogImageDataModeConstants.Read, 0, 0, cogImage.Width, cogImage.Height);

            var image2d = new Image2D();
            image2d.Initialize(pixelMemory.Width, pixelMemory.Height, 1, pixelMemory.Width);

            byte[] data = new byte[pixelMemory.Width * pixelMemory.Height];
            Parallel.For(0, pixelMemory.Height, y => System.Runtime.InteropServices.Marshal.Copy(pixelMemory.Scan0 + y * pixelMemory.Stride, data, y * pixelMemory.Width, pixelMemory.Width));
            image2d.SetData(data);
            pixelMemory.Dispose();

            return image2d;
        }

        public static ImageD ConvertImage(CogImage24PlanarColor cogImage)
        {
            cogImage.Get24PlanarColorPixelMemory(CogImageDataModeConstants.Read, 0, 0, cogImage.Width, cogImage.Height, out ICogImage8PixelMemory pixelMemory0, out ICogImage8PixelMemory pixelMemory1, out ICogImage8PixelMemory pixelMemory2);

            var image2d = new Image2D();
            image2d.Initialize(pixelMemory0.Width, pixelMemory0.Height, 3, pixelMemory0.Stride);

            int size = cogImage.Width * cogImage.Height;

            unsafe
            {
                byte* pixelMemory0Ptr = (byte*)pixelMemory0.Scan0.ToPointer();
                byte* pixelMemory1Ptr = (byte*)pixelMemory1.Scan0.ToPointer();
                byte* pixelMemory2Ptr = (byte*)pixelMemory2.Scan0.ToPointer();

                byte[] imageData = image2d.Data;
                Parallel.For(0, size, pos =>
                {
                    imageData[pos * 3] = pixelMemory0Ptr[pos];
                    imageData[pos * 3 + 1] = pixelMemory1Ptr[pos];
                    imageData[pos * 3 + 2] = pixelMemory2Ptr[pos];
                });
            }

            pixelMemory0.Dispose();
            pixelMemory1.Dispose();
            pixelMemory2.Dispose();

            return image2d;
        }
    }
}
