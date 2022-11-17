using DynMvp.Base;
using DynMvp.Vision.Cognex;
using DynMvp.Vision.Cuda;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
using DynMvp.Vision.OpenCv;
using DynMvp.Vision.Planbss;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    //Grey(영국식), Gray(미국식)
    public enum ImageType
    {
        Grey, Color, Depth
    }

    public abstract class ImageBuilder
    {
        public static CudaImageBuilder CudaImageBuilder => new CudaImageBuilder();

        public static OpenEVisionImageBuilder OpenEVisionImageBuilder => new OpenEVisionImageBuilder();

        public static MilImageBuilder MilImageBuilder => new MilImageBuilder();

        public static OpenCvImageBuilder OpenCvImageBuilder => new OpenCvImageBuilder();

        public static CognexImageBuilder CognexImageBuilder => new CognexImageBuilder();

        public static ImageBuilder GetInstance(ImagingLibrary libraryType)
        {
            switch (libraryType)
            {
                case ImagingLibrary.EuresysOpenEVision: return OpenEVisionImageBuilder;
                case ImagingLibrary.OpenCv: return OpenCvImageBuilder;
                case ImagingLibrary.MatroxMIL: return MilImageBuilder;
                case ImagingLibrary.CognexVisionPro: return CognexImageBuilder;
                case ImagingLibrary.Cuda: return CudaImageBuilder;
                default: throw new InvalidTypeException();
            }
        }

        public static AlgoImage BuildSameTypeSize(AlgoImage algoImage)
        {
            return Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
        }

        public static bool CheckImageType(AlgorithmStrategy strategy, ref ImagingLibrary libraryType, ref ImageType imageType)
        {
            if (strategy != null)
            {
                libraryType = strategy.LibraryType;
                //if (strategy.ImageType != ImageType.Gpu)
                imageType = strategy.ImageType;
            }

            return true;
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageType imageType, Size size, ImageBandType imageBand = ImageBandType.Luminance)
        {
            //CheckImageType(null, ref libraryType, ref imageType);
            return GetInstance(libraryType).Build(imageType, size.Width, size.Height);
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageType imageType, int width, int height, ImageBandType imageBand = ImageBandType.Luminance)
        {
            //CheckImageType(null, ref libraryType, ref imageType);
            return GetInstance(libraryType).Build(imageType, width, height);
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            CheckImageType(null, ref libraryType, ref imageType);
            AlgoImage algoImage = GetInstance(libraryType).Build(image, imageType, imageBand);
            //algoImage.Tag = image.Tag;
            return algoImage;
        }

        public static AlgoImage Build(string algorithmType, ImageType imageType, int width, int height)
        {
            AlgorithmStrategy strategy = AlgorithmFactory.Instance().GetStrategy(algorithmType);
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(imageType, width, height);
        }

        public static AlgoImage Build(string algorithmType, Size size)
        {
            return Build(algorithmType, size.Width, size.Height);
        }

        public static AlgoImage Build(string algorithmType, int width, int height)
        {
            AlgorithmStrategy strategy = AlgorithmFactory.Instance().GetStrategy(algorithmType);
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(strategy.ImageType, width, height);
        }

        public static AlgoImage Build(string algorithmType, ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgorithmStrategy strategy = AlgorithmFactory.Instance().GetStrategy(algorithmType);
            //ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(image, imageType, imageBand);
        }

        public static AlgoImage Build(string algorithmType, ImageD image, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgorithmStrategy strategy = AlgorithmFactory.Instance().GetStrategy(algorithmType);

            //ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(image, strategy.ImageType, imageBand);
        }

        public abstract AlgoImage Build(ImageType imageType, int width, int height);

        public abstract AlgoImage Build(ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance);
    }
}
