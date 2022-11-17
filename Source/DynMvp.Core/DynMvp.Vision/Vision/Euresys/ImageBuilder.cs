using DynMvp.Base;
using Euresys.Open_eVision_1_2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Euresys
{
    public class OpenEVisionImageBuilder : ImageBuilder
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

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
            var euresysGreyImage = new OpenEVisionGreyImage();

            if (image.NumBand == 3)
            {
                EImageC24 euresysColorImage = ConvertColorImage(image);

                euresysGreyImage.Image = new EImageBW8();
                euresysGreyImage.Image.SetSize(euresysColorImage.Width, euresysColorImage.Height);

                switch (imageBand)
                {
                    case ImageBandType.Luminance:
                        var colorLookup = new EColorLookup();
                        colorLookup.ConvertFromRgb(EColorSystem.Ish);

                        EasyColor.GetComponent(euresysColorImage, euresysGreyImage.Image, 0, colorLookup);
                        break;
                    case ImageBandType.Red:
                        EasyColor.GetComponent(euresysColorImage, euresysGreyImage.Image, 0);
                        break;
                    case ImageBandType.Green:
                        EasyColor.GetComponent(euresysColorImage, euresysGreyImage.Image, 1);
                        break;
                    case ImageBandType.Blue:
                        EasyColor.GetComponent(euresysColorImage, euresysGreyImage.Image, 2);
                        break;
                }
            }
            else
            {
                euresysGreyImage.Image = ConvertGreyImage(image);
            }

            return euresysGreyImage;
        }

        private EImageBW8 ConvertGreyImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;

            var euresysGreyImage = new EImageBW8();
            euresysGreyImage.SetSize(width, height);

            var image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            byte[] euresysBuf = new byte[euresysGreyImage.RowPitch * euresysGreyImage.Height];

            Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, euresysBuf, euresysGreyImage.RowPitch * y, width));

            // for (int y = 0; y < height; y++)
            //      Array.Copy(imageBuf, image.Pitch * y, euresysBuf, euresysGreyImage.RowPitch * y, width);

            System.Runtime.InteropServices.Marshal.Copy(euresysBuf, 0, euresysGreyImage.GetImagePtr(), euresysBuf.Length);

            return euresysGreyImage;
        }

        private EImageC24 ConvertColorImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;

            var euresysColorImage = new EImageC24();
            euresysColorImage.SetSize(width, height);

            byte[] euresysBuf = new byte[euresysColorImage.RowPitch * euresysColorImage.Height];

            var image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, euresysBuf, euresysColorImage.RowPitch * y, width));

            //for (int y = 0; y < height; y++)
            //    Array.Copy(imageBuf, image.Pitch * y, euresysBuf, euresysColorImage.RowPitch * y, width*3);

            return euresysColorImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            var euresysColorImage = new OpenEVisionColorImage();
            euresysColorImage.Image = ConvertColorImage(image);

            return euresysColorImage;
        }

        public static ImageD ConvertImage(EImageBW8 eureImage)
        {
            if (eureImage == null || eureImage.IsVoid)
            {
                return null;
            }

            int width = eureImage.Width;
            int height = eureImage.Height;

            int size = width * height;

            var image = new Image2D();
            image.Initialize(width, height, 1, eureImage.RowPitch);

            byte[] imageBuf = image.Data;
            IntPtr euresysBufPtr = eureImage.GetImagePtr();

            System.Runtime.InteropServices.Marshal.Copy(euresysBufPtr, imageBuf, 0, eureImage.RowPitch * height);

            return image;
        }

        public static ImageD ConvertImage(EImageC24 eureImage)
        {
            if (eureImage == null || eureImage.IsVoid)
            {
                return null;
            }

            int width = eureImage.Width;
            int height = eureImage.Height;

            int size = width * height;

            var image = new Image2D();
            image.Initialize(width, height, 3, eureImage.RowPitch);

            byte[] imageBuf = image.Data;
            IntPtr euresysBufPtr = eureImage.GetImagePtr();

            System.Runtime.InteropServices.Marshal.Copy(euresysBufPtr, imageBuf, 0, eureImage.RowPitch * height);

            return image;
        }
    }
}
