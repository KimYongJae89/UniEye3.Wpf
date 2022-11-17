using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum FilterType
    {
        None, EdgeExtraction, Average, HistogramEqualization, Binarize, Morphology, Subtraction, Mask
    }

    public abstract class IFilter
    {
        public bool EssentialFilter { get; set; } = false;

        public abstract IFilter Clone();
        public abstract FilterType GetFilterType();
        public abstract void Filter(AlgoImage algoImage);
        public abstract void LoadParam(XmlElement filterElement);
        public abstract void SaveParam(XmlElement filterElement);
    }

    public class FilterFactory
    {
        public static IFilter CreateFilter(FilterType filterType)
        {
            IFilter filter = null;

            switch (filterType)
            {
                case FilterType.EdgeExtraction:
                    filter = new EdgeExtractionFilter();
                    break;
                case FilterType.Average:
                    filter = new AverageFilter();
                    break;
                case FilterType.HistogramEqualization:
                    filter = new HistogramEqualizationFilter();
                    break;
                case FilterType.Binarize:
                    filter = new BinarizeFilter();
                    break;
                case FilterType.Morphology:
                    filter = new MorphologyFilter();
                    break;
                case FilterType.Subtraction:
                    filter = new SubtractionFilter();
                    break;
                case FilterType.Mask:
                    filter = new MaskFilter();
                    break;
            }
            return filter;
        }
    }

    public class EdgeExtractionFilter : IFilter
    {
        public int KernelSize { get; set; }

        public EdgeExtractionFilter(int kernelSize = 3)
        {
            KernelSize = kernelSize;
        }

        public override IFilter Clone()
        {
            return new EdgeExtractionFilter(KernelSize);
        }

        public override FilterType GetFilterType()
        {
            return FilterType.EdgeExtraction;
        }

        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.Sobel(algoImage, KernelSize);
        }

        public override void LoadParam(XmlElement filterElement)
        {
            KernelSize = Convert.ToInt32(XmlHelper.GetValue(filterElement, "KernelSize", "3"));
        }

        public override void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "KernelSize", KernelSize.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }
    }

    public class AverageFilter : IFilter
    {
        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.Average(algoImage);
        }

        public override IFilter Clone()
        {
            return new AverageFilter();
        }

        public override FilterType GetFilterType()
        {
            return FilterType.Average;
        }

        public override void LoadParam(XmlElement filterElement)
        {
        }

        public override void SaveParam(XmlElement filterElement)
        {
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }
    }

    public class HistogramEqualizationFilter : IFilter
    {
        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.HistogramStretch(algoImage);
        }

        public override IFilter Clone()
        {
            return new HistogramEqualizationFilter();
        }

        public override FilterType GetFilterType()
        {
            return FilterType.HistogramEqualization;
        }

        public override void LoadParam(XmlElement filterElement)
        {
        }

        public override void SaveParam(XmlElement filterElement)
        {
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }
    }

    public class BinarizeFilter : IFilter
    {
        public BinarizationType BinarizationType { get; set; }
        public bool Invert { get; set; }
        public int ThresholdLower { get; set; }
        public int ThresholdUpper { get; set; }

        public BinarizeFilter(BinarizationType binarizationType = BinarizationType.SingleThreshold, int thresholdLower = 100, int thresholdUpper = 200, bool invert = false)
        {
            BinarizationType = binarizationType;
            ThresholdLower = thresholdLower;
            ThresholdUpper = thresholdUpper;
            Invert = invert;
        }

        public override IFilter Clone()
        {
            return new BinarizeFilter(BinarizationType, ThresholdLower, ThresholdUpper, Invert);
        }

        public override FilterType GetFilterType()
        {
            return FilterType.Binarize;
        }

        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.Binarize(algoImage, BinarizationType, ThresholdLower, ThresholdUpper, Invert);
        }

        public override void LoadParam(XmlElement filterElement)
        {
            BinarizationType = (BinarizationType)Enum.Parse(typeof(BinarizationType), XmlHelper.GetValue(filterElement, "BinarizationType", "SingleThreshold"));
            ThresholdLower = Convert.ToInt32(XmlHelper.GetValue(filterElement, "ThresholdLower", "128"));
            ThresholdUpper = Convert.ToInt32(XmlHelper.GetValue(filterElement, "ThresholdUpper", "128"));
            Invert = Convert.ToBoolean(XmlHelper.GetValue(filterElement, "Invert", "false"));
        }

        public override void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "BinarizationType", BinarizationType.ToString());
            XmlHelper.SetValue(filterElement, "ThresholdLower", ThresholdLower.ToString());
            XmlHelper.SetValue(filterElement, "ThresholdUpper", ThresholdUpper.ToString());
            XmlHelper.SetValue(filterElement, "Invert", Invert.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }
    }

    public enum MorphologyType
    {
        Erode, Dilate, Open, Close
    }

    public class MorphologyFilter : IFilter
    {
        public MorphologyType MorphologyType { get; set; }
        public int NumIteration { get; set; }

        public MorphologyFilter(MorphologyType morphologyType = MorphologyType.Erode, int numIteration = 3)
        {
            MorphologyType = morphologyType;
            NumIteration = numIteration;
        }

        public override IFilter Clone()
        {
            return new MorphologyFilter(MorphologyType, NumIteration);
        }

        public override FilterType GetFilterType()
        {
            return FilterType.Morphology;
        }

        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            switch (MorphologyType)
            {
                case MorphologyType.Erode:
                    imageProcessing.Erode(algoImage, NumIteration);
                    break;
                case MorphologyType.Dilate:
                    imageProcessing.Dilate(algoImage, NumIteration);
                    break;
                case MorphologyType.Open:
                    imageProcessing.Open(algoImage, NumIteration);
                    break;
                case MorphologyType.Close:
                    imageProcessing.Close(algoImage, NumIteration);
                    break;
            }
        }

        public override void LoadParam(XmlElement filterElement)
        {
            MorphologyType = (MorphologyType)Enum.Parse(typeof(MorphologyType), XmlHelper.GetValue(filterElement, "MorphologyType", "Erode"));
            NumIteration = Convert.ToInt32(XmlHelper.GetValue(filterElement, "NumIteration", "1"));
        }

        public override void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "MorphologyType", MorphologyType.ToString());
            XmlHelper.SetValue(filterElement, "NumIteration", NumIteration.ToString());
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }
    }

    public enum SubtractionType
    {
        Absolute, SetZero
    }
    public class SubtractionFilter : IFilter
    {
        public SubtractionType SubtractionType { get; set; }
        public AlgoImage SubtractionImage { get; private set; } = null;

        public static string TypeName => "Subtraction";
        public bool UseInvert { get; set; } = false;

        public SubtractionFilter(SubtractionType subtractionType = SubtractionType.Absolute)
        {
            SubtractionType = subtractionType;
        }

        public override IFilter Clone()
        {
            return new SubtractionFilter(SubtractionType);
        }

        public override FilterType GetFilterType()
        {
            return FilterType.Subtraction;
        }

        public override void Filter(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            if (SubtractionImage != null)
            {
                if (SubtractionType == SubtractionType.SetZero)
                {
                    if (UseInvert)
                    {
                        imageProcessing.Subtract(algoImage, SubtractionImage, algoImage);
                    }
                    else
                    {
                        imageProcessing.Subtract(SubtractionImage, algoImage, algoImage);
                    }
                }
                else
                {
                    AlgoImage subImage1 = ImageBuilder.Build(GetFilterType().ToString(), algoImage.ImageType, algoImage.Width, algoImage.Height);
                    AlgoImage subImage2 = ImageBuilder.Build(GetFilterType().ToString(), algoImage.ImageType, algoImage.Width, algoImage.Height);

                    imageProcessing.Subtract(SubtractionImage, algoImage, subImage1);
                    imageProcessing.Subtract(algoImage, SubtractionImage, subImage2);
                    imageProcessing.Or(subImage1, subImage2, algoImage);

                    subImage1.Dispose();
                    subImage2.Dispose();
                }
            }
        }

        public override void LoadParam(XmlElement filterElement)
        {
            SubtractionType = (SubtractionType)Enum.Parse(typeof(SubtractionType), XmlHelper.GetValue(filterElement, "SubtractionType", SubtractionType.ToString()));
            UseInvert = bool.Parse(XmlHelper.GetValue(filterElement, "UseInvert", UseInvert.ToString()));

            string bitmapString = XmlHelper.GetValue(filterElement, "SubtractionImage", "");
            if (!string.IsNullOrEmpty(bitmapString))
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(bitmapString);
                if (bitmap != null)
                {
                    var imageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(filterElement, "SubtractionImageType", ImageType.Grey.ToString()));

                    ImageD tempImage = Image2D.ToImage2D(bitmap);
                    SubtractionImage = ImageBuilder.Build(GetFilterType().ToString(), tempImage, imageType);
                }
            }
        }

        public override void SaveParam(XmlElement filterElement)
        {
            XmlHelper.SetValue(filterElement, "SubtractionType", SubtractionType.ToString());
            XmlHelper.SetValue(filterElement, "UseInvert", UseInvert.ToString());

            if (SubtractionImage != null)
            {
                string bitmapString = ImageHelper.BitmapToBase64String(SubtractionImage.ToImageD().ToBitmap());
                XmlHelper.SetValue(filterElement, "SubtractionImage", bitmapString);
                XmlHelper.SetValue(filterElement, "SubtractionImageType", SubtractionImage.ImageType.ToString());
            }
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }

        public void SetSubtractionImage(AlgoImage subtractionImage)
        {
            SubtractionImage = subtractionImage;
        }

        public void ClearSubtractionImage()
        {
            if (SubtractionImage != null)
            {
                SubtractionImage.Dispose();
            }

            SubtractionImage = null;
        }
    }


    public class MaskFilter : IFilter
    {
        public AlgoImage MaskImage { get; private set; } = null;

        public FigureGroup MaskFigure { get; } = new FigureGroup();

        public override IFilter Clone()
        {
            var maskFilter = new MaskFilter();
            if (MaskImage != null)
            {
                maskFilter.MaskImage = MaskImage.Clone();
            }

            return maskFilter;
        }

        public override void Filter(AlgoImage algoImage)
        {
            if (MaskImage == null)
            {
                return;
            }

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            if ((algoImage.Width != MaskImage.Width) || (algoImage.Height != MaskImage.Height))
            {
                return;
            }

            imageProcessing.And(algoImage, MaskImage, algoImage);
        }

        public override FilterType GetFilterType()
        {
            return FilterType.Mask;
        }

        public override void LoadParam(XmlElement filterElement)
        {
            string imageString = XmlHelper.GetValue(filterElement, "MaskImage", "");
            if (!string.IsNullOrEmpty(imageString))
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
                if (bitmap != null)
                {
                    var imageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(filterElement, "MaskImageType", ImageType.Grey.ToString()));
                    MaskImage = ImageBuilder.Build(GetFilterType().ToString(), Image2D.ToImage2D(bitmap), imageType);
                }
            }
        }

        public override void SaveParam(XmlElement filterElement)
        {
            if (MaskImage != null)
            {
                string imageString = ImageHelper.BitmapToBase64String(MaskImage.ToImageD().ToBitmap());
                XmlHelper.SetValue(filterElement, "MaskImage", imageString);
                XmlHelper.SetValue(filterElement, "MaskImageType", MaskImage.ImageType.ToString());
            }
        }

        public override string ToString()
        {
            return StringManager.GetString(GetFilterType().ToString());
        }

        public void SetMaskImage(AlgoImage maskImage)
        {
            MaskImage = maskImage;
        }

        public void ClearMaskImage()
        {
            if (MaskImage != null)
            {
                MaskImage.Dispose();
            }

            MaskImage = null;
        }
    }
}
