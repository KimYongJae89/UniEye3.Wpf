using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.Vision
{
    //2개의 영역 분리값이 가장 커지는 값인데..
    //http://darkpgmr.tistory.com/115

    public enum LightAutoTuneType
    {
        Average, Otsu, Saturation, TopArea
    }

    public abstract class LightTuneValueCalculator
    {
        public abstract float GetValue(ImageD image);

        public static LightTuneValueCalculator CreateCalculator(LightAutoTuneType type)
        {
            switch (type)
            {
                case LightAutoTuneType.Average:
                    break;
                case LightAutoTuneType.Otsu:
                    return new OtsuCalculator();
                case LightAutoTuneType.Saturation:
                    return new SaturationCalculator();
            }

            return null;
        }
    }

    public class OtsuCalculator : LightTuneValueCalculator
    {
        public override float GetValue(ImageD image)
        {
            ImagingLibrary imagingLibrary = OperationConfig.Instance().ImagingLibrary;
            var imageBuilder = ImageBuilder.GetInstance(imagingLibrary);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(imagingLibrary);

            AlgoImage algoImage = imageBuilder.Build(image, ImageType.Grey);
            AlgoImage maskImage = imageBuilder.Build(algoImage.ImageType, algoImage.Width, algoImage.Height);
            imageProcessing.Binarize(algoImage, maskImage);

            float avgUpper = imageProcessing.GetGreyAverage(algoImage, maskImage);
            StatResult statResultUpper = imageProcessing.GetStatValue(algoImage, maskImage);

            imageProcessing.Not(maskImage, maskImage);

            float avgLower = imageProcessing.GetGreyAverage(algoImage, maskImage);
            StatResult statResultLower = imageProcessing.GetStatValue(algoImage, maskImage);

            algoImage.Dispose();
            maskImage.Dispose();

            double area = algoImage.Width * algoImage.Height;
            double upperRatio = statResultUpper.count / area;
            double lowerRatio = statResultLower.count / area;

            return (float)(upperRatio * lowerRatio * Math.Pow(avgLower - avgUpper, 2));
        }
    }

    public class SaturationCalculator : LightTuneValueCalculator
    {
        private float saturaionRatio;

        public SaturationCalculator(float saturaionRatio = 0.03f)
        {
            this.saturaionRatio = saturaionRatio;
        }

        public override float GetValue(ImageD image)
        {
            ImagingLibrary imagingLibrary = OperationConfig.Instance().ImagingLibrary;
            var imageBuilder = ImageBuilder.GetInstance(imagingLibrary);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(imagingLibrary);

            AlgoImage algoImage = imageBuilder.Build(image, ImageType.Grey);

            //1. Histogram
            long[] histo = imageProcessing.Histogram(algoImage);

            long sum = 0;
            double avg = 0;
            long targetsize = (long)(algoImage.Width * algoImage.Height * saturaionRatio);
            for (int i = 254; i >= 0; i--)
            {
                sum += histo[i];

                avg += histo[i] * i;
                if (sum > targetsize)
                {
                    break;
                }
            }

            double averageIntensityTopArea = avg / sum;

            algoImage.Dispose();

            //최대값 1
            return (float)(averageIntensityTopArea / 255.0);
        }
    }
}
