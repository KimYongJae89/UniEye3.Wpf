using DynMvp.Base;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cuda
{
    public class CudaImageBuilder : ImageBuilder
    {
        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            CudaImage cudaImage = null; ;

            switch (imageType)
            {
                case ImageType.Grey:
                    cudaImage = new CudaDepthImage<byte>();
                    break;
                case ImageType.Color:
                    cudaImage = new CudaDepthImage<byte>();
                    break;
                case ImageType.Depth:
                    cudaImage = new CudaDepthImage<int>();
                    break;
            }

            cudaImage.Alloc(width, height);
            return cudaImage;
        }

        public override AlgoImage Build(ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            int width = image.Width;
            int height = image.Height;

            var image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            CudaImage cudaImage = null; ;

            switch (imageType)
            {
                case ImageType.Grey:
                    cudaImage = new CudaDepthImage<byte>();
                    break;
                case ImageType.Color:
                    cudaImage = new CudaDepthImage<byte>();
                    break;
                case ImageType.Depth:
                    cudaImage = new CudaDepthImage<int>();
                    break;
            }

            if (image2d.IsUseIntPtr())
            {
                cudaImage.Alloc(width, height, image2d.ImageData.DataPtr);
            }
            else
            {
                if (imageBuf == null)
                {
                    throw new InvalidTypeException();
                }

                cudaImage.Alloc(width, height);
                cudaImage.PutByte(imageBuf);
            }

            return cudaImage;
        }
    }
}
