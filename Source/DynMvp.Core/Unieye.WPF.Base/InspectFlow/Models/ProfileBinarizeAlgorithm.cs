using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public enum BinarizeDirection
    {
        Lower,
        Upper,
    }

    public class ProfileBinarizeAlgorithm : FlowAlgorithmModel
    {
        [FlowAlgorithmParameter]
        public BinarizeDirection BinarizeDirection { get; set; } = BinarizeDirection.Lower;

        [FlowAlgorithmParameter]
        public float BinarizeValue { get; set; } = 0;

        public override AlgoImage Inspect(AlgoImage srcImage)
        {
            if (BufferImage == null)
            {
                BufferImage = CloneImage(srcImage);
            }

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(ImagingLibrary);

            switch (ImagingLibrary)
            {
                case ImagingLibrary.OpenCv:
                    break;
                case ImagingLibrary.EuresysOpenEVision:
                    break;
                case ImagingLibrary.CognexVisionPro:
                    break;
                case ImagingLibrary.MatroxMIL:
                    break;
                case ImagingLibrary.Halcon:
                    break;
                case ImagingLibrary.Cuda:
                    {
                        var cudaSrcImage = srcImage as CudaImage;
                        var cudaDstImage = BufferImage as CudaImage;

                        cudaSrcImage.CreateProfile();

                        switch (BinarizeDirection)
                        {
                            case BinarizeDirection.Lower:
                            default:
                                CudaMethods.CUDA_ADAPTIVE_BINARIZE_LOWER(cudaSrcImage.ImageID, cudaDstImage.ImageID, BinarizeValue);
                                break;
                            case BinarizeDirection.Upper:
                                CudaMethods.CUDA_ADAPTIVE_BINARIZE_UPPER(cudaSrcImage.ImageID, cudaDstImage.ImageID, BinarizeValue);
                                break;
                        }
                    }
                    break;
                case ImagingLibrary.Custom:
                    break;
            }

            return BufferImage;
        }
    }
}
