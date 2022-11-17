using DynMvp.Vision.Cognex;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
using DynMvp.Vision.OpenCv;
using DynMvp.Vision.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision
{
    public class ImageProcessingPool
    {
        private static OpenEVisionImageProcessing openEVisionImageProcessing = new OpenEVisionImageProcessing();
        private static MilImageProcessing milImageProcessing = new MilImageProcessing();
        private static CognexImageProcessing cognexImageProcessing = new CognexImageProcessing();
        private static OpenCvImageProcessing openCvImageProcessing = new OpenCvImageProcessing();
        private static CudaImageProcessing cudaImageProcessing = new CudaImageProcessing();

        public static ImageProcessing GetImageProcessing(AlgoImage algoImage)
        {
            return GetImageProcessing(algoImage.LibraryType);
        }

        public static ImageProcessing GetImageProcessing(ImagingLibrary imageLibrary)
        {
            switch (imageLibrary)
            {
                case ImagingLibrary.EuresysOpenEVision:
                    return openEVisionImageProcessing;
                case ImagingLibrary.MatroxMIL:
                    return milImageProcessing;
                case ImagingLibrary.CognexVisionPro:
                    return cognexImageProcessing;
                case ImagingLibrary.Cuda:
                    return cudaImageProcessing;
            }

            return openCvImageProcessing;
        }
    }
}
