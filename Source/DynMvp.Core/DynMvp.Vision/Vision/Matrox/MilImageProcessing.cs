using DynMvp.Base;
using DynMvp.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Matrox
{
    public abstract class MilObject : IDisposable
    {
        protected MIL_ID id = MIL.M_NULL;
        public MIL_ID Id
        {
            get => id;
            set => id = value;
        }

        public MilObject()
        {
#if DEBUG
            //stackTrace = Environment.StackTrace;
#endif
        }

        ~MilObject()
        {
            Dispose();
        }

        public abstract void Dispose();
    }

    public class MilImObject : MilObject
    {
        public override void Dispose()
        {
            if (id != MIL.M_NULL)
            {
                MIL.MimFree(id);
                id = MIL.M_NULL;
            }
        }
    }

    public class MilBufObject : MilObject
    {
        public override void Dispose()
        {
            if (id != MIL.M_NULL)
            {
                MIL.MbufFree(id);
                id = MIL.M_NULL;
            }
        }
    }

    public class MilBlobObject : MilObject
    {
        public override void Dispose()
        {
            if (id != MIL.M_NULL)
            {
                MIL.MblobFree(id);
                id = MIL.M_NULL;
            }
        }
    }

    internal class MilBlobRectList : BlobRectList
    {
        public MilBlobObject BlobResult { get; set; }

        public override void Dispose()
        {
            BlobResult.Dispose();
        }
    }

    public class MilImageProcessing : ImageProcessing
    {
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            MilGreyImage milSrcImage = MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilGreyImage milDestImage = MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            //MIL_INT threshold = MIL.MimBinarize(milSrcImage.Image, MIL.M_NULL, MIL.M_BIMODAL, MIL.M_NULL, MIL.M_NULL);

            if (inverse == false)
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_BIMODAL + MIL.M_GREATER_OR_EQUAL, MIL.M_NULL, MIL.M_NULL);
            }
            else
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_BIMODAL + MIL.M_LESS, MIL.M_NULL, MIL.M_NULL);
            }
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (inverse == false)
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_GREATER_OR_EQUAL, threshold, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_LESS, threshold, MIL.M_DEFAULT);
            }
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (inverse == false)
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_IN_RANGE, thresholdLower, thresholdUpper);
            }
            else
            {
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_OUT_RANGE, thresholdLower, thresholdUpper);
            }
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_PERCENTILE_VALUE + MIL.M_GREATER_OR_EQUAL, percent, MIL.M_NULL);
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
        {
            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            var imContext = new MilBufObject();
            var imResult = new MilBufObject();

            imContext.Id = MIL.MimAlloc(MIL.M_DEFAULT_HOST, MIL.M_BINARIZE_ADAPTIVE_CONTEXT, MIL.M_DEFAULT, MIL.M_NULL);

            /* Set binarization controls. */
            MIL.MimControl(imContext.Id, MIL.M_THRESHOLD_MODE, MIL.M_NIBLACK);
            MIL.MimControl(imContext.Id, MIL.M_FOREGROUND_VALUE, MIL.M_FOREGROUND_WHITE);
            //MIL.MimControl(imContext.Id, MIL.M_FOREGROUND_VALUE, MIL.M_FOREGROUND_BLACK);
            MIL.MimControl(imContext.Id, MIL.M_GLOBAL_MIN, thresholdLower);

            MIL.MimBinarizeAdaptive(imContext.Id, milSrcImage.Image, MIL.M_NULL, MIL.M_NULL, milDestImage.Image, MIL.M_NULL, MIL.M_DEFAULT);
        }

        public override void CustomBinarize(AlgoImage algoImage1, AlgoImage algoImage2, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void Smooth(AlgoImage srcImage, AlgoImage destImage, int numSmooth)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Erode", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Erode", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimConvolve(milSrcImage.Image, milDestImage.Image, MIL.M_SMOOTH);
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Erode", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Erode", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;


            if (useGray)
            {
                MIL.MimErode(milSrcImage.Image, milDestImage.Image, numErode, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimErode(milSrcImage.Image, milDestImage.Image, numErode, MIL.M_BINARY);
            }
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numErode, bool isGrey)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Open", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Open", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            int h = structureXY.GetLength(0);
            int w = structureXY.GetLength(1);
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, w, h, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufPut2d(structureElement, 0, 0, w, h, structureXY);

            if (isGrey)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_ERODE, numErode, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_ERODE, numErode, MIL.M_BINARY);
            }

            MIL.MbufFree(structureElement);
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Dilate", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Dilate", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (useGray)
            {
                MIL.MimDilate(milSrcImage.Image, milDestImage.Image, numDilate, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimDilate(milSrcImage.Image, milDestImage.Image, numDilate, MIL.M_BINARY);
            }
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numDilate, bool isGrey)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Open", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Open", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            int w = structureXY.GetLength(0);
            int h = structureXY.GetLength(1);
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, w, h, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufPut2d(structureElement, 0, 0, w, h, structureXY);

            if (isGrey)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_DILATE, numDilate, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_DILATE, numDilate, MIL.M_BINARY);
            }

            MIL.MbufFree(structureElement);
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Open", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Open", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 1);

            if (useGray)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_OPEN, numOpen, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_OPEN, numOpen, MIL.M_BINARY);
            }

            MIL.MbufFree(structureElement);

            //if (useGray)
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numOpen, MIL.M_GRAYSCALE);
            //else
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numOpen, MIL.M_BINARY);
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Close", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Close", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 1);

            if (useGray)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_CLOSE, numClose, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_CLOSE, numClose, MIL.M_BINARY);
            }

            MIL.MbufFree(structureElement);

            //if (useGray)
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numClose, MIL.M_GRAYSCALE);
            //else
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numClose, MIL.M_BINARY);
        }
        public override void Thick(AlgoImage srcImage, AlgoImage destImage, int numThick, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.TopHat", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.TopHat", "Destination");

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 0);

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (useGray)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_THICK, numThick, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_THICK, numThick, MIL.M_BINARY);
            }
        }


        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.TopHat", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.TopHat", "Destination");

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 0);

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (useGray)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_TOP_HAT, numTopHat, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_TOP_HAT, numTopHat, MIL.M_BINARY);
            }
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numBottomHat, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.BottomHat", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.BottomHat", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 0);

            if (useGray)
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_BOTTOM_HAT, numBottomHat, MIL.M_GRAYSCALE);
            }
            else
            {
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_BOTTOM_HAT, numBottomHat, MIL.M_BINARY);
            }
        }

        private MilImObject Stat(AlgoImage algoImage, int type, int condition = 0, double conditionLow = 0, double conditionHigh = 0)
        {
            return Stat(algoImage, new int[] { type }, condition, conditionLow, conditionHigh);
        }

        private MilImObject Stat(AlgoImage algoImage, int[] types, int condition = 0, double conditionLow = 0, double conditionHigh = 0)
        {
            return Stat(algoImage, types, null, condition, conditionLow, conditionHigh);
        }

        private MilImObject Stat(AlgoImage algoImage, int type, AlgoImage maskImage, int condition = 0, double conditionLow = 0, double conditionHigh = 0)
        {
            return Stat(algoImage, new int[] { type }, maskImage, condition, conditionLow, conditionHigh);
        }

        private MilImObject Stat(AlgoImage algoImage, int[] types, AlgoImage maskImage, int condition = 0, double conditionLow = 0, double conditionHigh = 0)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Stat", "Source");

            var milGreyImage = algoImage as MilGreyImage;

            var imContext = new MilImObject()
            {
                Id = MIL.MimAlloc(MIL.M_DEFAULT_HOST, MIL.M_STATISTICS_CONTEXT, MIL.M_DEFAULT, MIL.M_NULL)
            };

            MIL.MimControl(imContext.Id, MIL.M_CONDITION, condition);
            MIL.MimControl(imContext.Id, MIL.M_COND_LOW, conditionLow);
            MIL.MimControl(imContext.Id, MIL.M_COND_HIGH, conditionHigh);

            if (maskImage != null)
            {
                MilGreyImage milMaskImage = MilImage.CheckGreyImage(maskImage, "MilImageProcessing.Stat(with Mask)", "Mask");
                MIL.MbufSetRegion(milGreyImage.Image, milMaskImage.Image, MIL.M_DEFAULT, MIL.M_RASTERIZE, MIL.M_DEFAULT);
            }

            foreach (int type in types)
            {
                MIL.MimControl(imContext.Id, type, MIL.M_ENABLE);
            }

            var imResult = new MilImObject()
            {
                Id = MIL.MimAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_STATISTICS_RESULT, MIL.M_NULL)
            };
            MIL.MimStatCalculate(imContext.Id, milGreyImage.Image, imResult.Id, MIL.M_DEFAULT);

            if (maskImage != null)
            {
                MIL.MbufSetRegion(milGreyImage.Image, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_DELETE, MIL.M_DEFAULT);
            }

            imContext.Dispose();

            return imResult;
        }

        private double length(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public override int Count(AlgoImage algoImage, AlgoImage maskImage = null)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Count(with Mask)", "Source");
            //MilImage.CheckGreyImage(maskImage, "MilImageProcessing.Count(with Mask)", "Mask");

            var milGreyImage = algoImage as MilGreyImage;

            int numWhitePixel;

            if (maskImage != null)
            {
                var milTempImage = (MilGreyImage)milGreyImage.Clone();
                var milMaskImage = maskImage as MilGreyImage;

                MIL.MimArith(milGreyImage.Image, milGreyImage.Image, milTempImage.Image, MIL.M_AND);

                algoImage = milTempImage;
            }

            MIL_INT tempValue = 0;
            MilImObject whitePixelResult = Stat(algoImage, MIL.M_STAT_NUMBER, MIL.M_GREATER_OR_EQUAL, 200, 0);
            MIL.MimGetResult(whitePixelResult.Id, MIL.M_STAT_NUMBER + MIL.M_TYPE_MIL_INT, ref tempValue);
            numWhitePixel = (int)tempValue;
            whitePixelResult.Dispose();

            return numWhitePixel;
        }

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage(with Mask)", "Source");

            var statResult = new StatResult();

            MilImObject result = Stat(algoImage, new int[] { MIL.M_STAT_MEAN, MIL.M_STAT_MIN, MIL.M_STAT_MAX, MIL.M_STAT_STANDARD_DEVIATION, MIL.M_STAT_SUM_OF_SQUARES }, maskImage);
            MIL.MimGetResult(result.Id, MIL.M_STAT_NUMBER + MIL.M_TYPE_MIL_FLOAT, ref statResult.count);
            MIL.MimGetResult(result.Id, MIL.M_STAT_MEAN + MIL.M_TYPE_MIL_FLOAT, ref statResult.average);
            MIL.MimGetResult(result.Id, MIL.M_STAT_MIN + MIL.M_TYPE_MIL_FLOAT, ref statResult.min);
            MIL.MimGetResult(result.Id, MIL.M_STAT_MAX + MIL.M_TYPE_MIL_FLOAT, ref statResult.max);
            MIL.MimGetResult(result.Id, MIL.M_STAT_STANDARD_DEVIATION + MIL.M_TYPE_MIL_FLOAT, ref statResult.stdDev);
            MIL.MimGetResult(result.Id, MIL.M_STAT_SUM_OF_SQUARES + MIL.M_TYPE_MIL_FLOAT, ref statResult.squareSum);

            result.Dispose();

            return statResult;
        }

        public override Color GetColorAverage(AlgoImage algoImage, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public override Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyAverage(AlgoImage algoImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage", "Source");

            float average = 0;
            MilImObject result = Stat(algoImage, MIL.M_STAT_MEAN);
            MIL.MimGetResult(result.Id, MIL.M_STAT_MEAN + MIL.M_TYPE_MIL_FLOAT, ref average);

            result.Dispose();

            return average;
        }

        public override float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage(with Mask)", "Source");

            float average = 0;
            MilImObject result = Stat(algoImage, MIL.M_STAT_MEAN, maskImage);
            MIL.MimGetResult(result.Id, MIL.M_STAT_MEAN + MIL.M_TYPE_MIL_FLOAT, ref average);

            result.Dispose();

            return average;
        }

        public override float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyMax(with Mask)", "Source");

            float max = 0;
            MilImObject result;

            if (maskImage != null)
            {
                result = Stat(algoImage, MIL.M_STAT_MAX, maskImage);
            }
            else
            {
                result = Stat(algoImage, MIL.M_STAT_MAX);
            }

            MIL.MimGetResult(result.Id, MIL.M_STAT_MAX + MIL.M_TYPE_MIL_FLOAT, ref max);

            result.Dispose();

            return max;
        }

        public override float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyMin(with Mask)", "Source");

            float min = 0;
            MilImObject result;

            if (maskImage != null)
            {
                result = Stat(algoImage, MIL.M_STAT_MIN, maskImage);
            }
            else
            {
                result = Stat(algoImage, MIL.M_STAT_MIN);
            }

            MIL.MimGetResult(result.Id, MIL.M_STAT_MIN + MIL.M_TYPE_MIL_FLOAT, ref min);

            result.Dispose();

            return min;
        }

        public override float GetStdDev(AlgoImage algoImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetStdDev", "Source");

            float stdDev = 0;
            MilImObject result = Stat(algoImage, MIL.M_STANDARD_DEVIATION);
            MIL.MimGetResult(result.Id, MIL.M_STANDARD_DEVIATION + MIL.M_TYPE_MIL_FLOAT, ref stdDev);

            result.Dispose();

            return stdDev;
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetStdDev", "Source");

            float stdDev = 0;
            MilImObject result = Stat(algoImage, MIL.M_STANDARD_DEVIATION, maskImage);
            MIL.MimGetResult(result.Id, MIL.M_STANDARD_DEVIATION + MIL.M_TYPE_MIL_FLOAT, ref stdDev);

            result.Dispose();

            return stdDev;
        }

        public override float[] Projection(AlgoImage algoImage, AlgoImage maskImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage algoImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Projection", "Source");

            var milGreyImage = algoImage as MilGreyImage;

            int nbEntries;
            double projAngle;
            if (projectionDir == TwoWayDirection.Horizontal)
            {
                projAngle = MIL.M_0_DEGREE;
                nbEntries = milGreyImage.Width;
            }
            else
            {
                projAngle = MIL.M_90_DEGREE;
                nbEntries = milGreyImage.Height;
            }

            double[] projection = new double[nbEntries];
            float[] floatProjection = new float[nbEntries];

            MIL_ID projResult = MIL.MimAllocResult(MIL.M_DEFAULT_HOST, nbEntries, MIL.M_PROJ_LIST, MIL.M_NULL);
            if (projResult == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilImageProcessing.Projection]");
            }

            if (projectionType == ProjectionType.Mean)
            {
                MIL.MimProjection(milGreyImage.Image, projResult, projAngle, MIL.M_MEAN, MIL.M_NULL);
            }
            else
            {
                MIL.MimProjection(milGreyImage.Image, projResult, projAngle, MIL.M_SUM, MIL.M_NULL);
            }

            MIL.MimGetResult(projResult, MIL.M_VALUE + MIL.M_TYPE_FLOAT, floatProjection);

            //MIL.MimGetResult(projResult, MIL.M_VALUE + MIL.M_TYPE_DOUBLE, projection);

            //for (long index = 0; index < nbEntries; index++)
            //    floatProjection[index] = (float)(projection[index] / 255.0);

            MIL.MimFree(projResult);

            return floatProjection;
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, int size = 3)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Sobel", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Sobel", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            if (size != 3 && size != 5 && size != 7)
            {
                throw new Exception("Size is must 3, 5, 7.");
            }

            if (size == 3)
            {
                MIL.MimConvolve(milSrcImage.Image, milDestImage.Image, MIL.M_EDGE_DETECT);
            }
            else
            {
                MIL_ID xKernel1 = MIL.M_NULL;
                MIL_ID xKernel2 = MIL.M_NULL;
                MIL_ID yKernel1 = MIL.M_NULL;
                MIL_ID yKernel2 = MIL.M_NULL;

                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref xKernel1);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref xKernel2);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref yKernel1);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref yKernel2);

                sbyte[,] xKernelData1 = null;
                sbyte[,] xKernelData2 = null;
                sbyte[,] yKernelData1 = null;
                sbyte[,] yKernelData2 = null;

                if (size == 5)
                {
                    xKernelData1 = new sbyte[5, 5] { { 5, 8, 10, 8, 5 }, { 4, 10, 20, 10, 4 }, { 0, 0, 0, 0, 0 }, { -4, -10, -20, -10, -4 }, { -5, -8, -10, -8, -5 } };
                    xKernelData2 = new sbyte[5, 5] { { -5, -8, -10, -8, -5 }, { -4, -10, -20, -10, -4 }, { 0, 0, 0, 0, 0 }, { 4, 10, 20, 10, 4 }, { 5, 8, 10, 8, 5 } };
                    yKernelData1 = new sbyte[5, 5] { { -5, -4, 0, 4, 5 }, { -8, -10, 0, 10, 8 }, { -10, -20, 0, 20, 10 }, { -8, -10, 0, 10, 8 }, { -5, -4, 0, 4, 5 } };
                    yKernelData2 = new sbyte[5, 5] { { 5, 4, 0, -4, -5 }, { 8, 10, 0, -10, -8 }, { 10, 20, 0, -20, -10 }, { 8, 10, 0, -10, -8 }, { 5, 4, 0, -4, -5 } };
                }

                MIL.MbufPut(xKernel1, xKernelData1);
                MIL.MbufPut(xKernel2, xKernelData2);
                MIL.MbufPut(yKernel1, yKernelData1);
                MIL.MbufPut(yKernel2, yKernelData2);

                MIL.MbufControl(xKernel1, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControl(xKernel2, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControl(yKernel1, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControl(yKernel2, MIL.M_NORMALIZATION_FACTOR, 64);

                MIL_ID xGradientImage1 = MIL.M_NULL;
                MIL_ID xGradientImage2 = MIL.M_NULL;
                MIL_ID yGradientImage1 = MIL.M_NULL;
                MIL_ID yGradientImage2 = MIL.M_NULL;

                MIL_ID xGradientImage = MIL.M_NULL;
                MIL_ID yGradientImage = MIL.M_NULL;

                int width = milSrcImage.Width;
                int height = milSrcImage.Height;

                xGradientImage1 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                xGradientImage2 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage1 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage2 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                xGradientImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                MIL.MimConvolve(milSrcImage.Image, xGradientImage1, xKernel1);
                MIL.MimConvolve(milSrcImage.Image, xGradientImage2, xKernel2);

                MIL.MimConvolve(milSrcImage.Image, yGradientImage1, yKernel1);
                MIL.MimConvolve(milSrcImage.Image, yGradientImage2, yKernel2);

                MIL.MimArith(xGradientImage1, xGradientImage2, xGradientImage, MIL.M_ADD);
                MIL.MimArith(yGradientImage1, yGradientImage2, yGradientImage, MIL.M_ADD);

                MIL.MimArith(xGradientImage, yGradientImage, milDestImage.Image, MIL.M_ADD);

                MIL.MbufFree(xKernel1);
                MIL.MbufFree(xKernel2);
                MIL.MbufFree(yKernel1);
                MIL.MbufFree(yKernel2);

                MIL.MbufFree(xGradientImage1);
                MIL.MbufFree(xGradientImage2);
                MIL.MbufFree(yGradientImage1);
                MIL.MbufFree(yGradientImage2);
                MIL.MbufFree(xGradientImage);
                MIL.MbufFree(yGradientImage);
            }
        }

        public override AlgoImage And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage1, "MilImageProcessing.And", "Source1");
            MilImage.CheckGreyImage(algoImage2, "MilImageProcessing.And", "Source2");

            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.And", "Destination");
            }

            var milSrc1Image = algoImage1 as MilGreyImage;
            var milSrc2Image = algoImage2 as MilGreyImage;
            MilGreyImage milDestImage;

            if (destImage == null)
            {
                destImage = new MilGreyImage(milSrc1Image.Width, milSrc1Image.Height);
            }

            milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_AND);

            return destImage;
        }

        public override AlgoImage Not(AlgoImage algoImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Not", "Source");

            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.Not", "Destination");
            }

            var milSrcImage = algoImage as MilGreyImage;
            MilGreyImage milDestImage;

            if (destImage == null)
            {
                destImage = new MilGreyImage(milSrcImage.Width, milSrcImage.Height);
            }

            milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_NOT);

            return destImage;
        }

        public double MatchShape(AlgoImage algoImage1, AlgoImage algoImage2)
        {
            throw new NotImplementedException();
        }

        public void AdaptiveThreshold(AlgoImage algoImage1, AlgoImage algoImage2, double param1)
        {
            throw new NotImplementedException();
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override void Canny(AlgoImage srcImage, AlgoImage dstImage, double threshold, double thresholdLinking)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Canny", "Source");

            var milSrcImage = srcImage as MilGreyImage;
            var milDstImage = dstImage as MilGreyImage;

            int filterValue = Math.Min(100, Math.Max(0, (int)threshold));

            MIL.MimConvolve(milSrcImage.Image, milDstImage.Image, MIL.M_DERICHE_FILTER(MIL.M_EDGE_DETECT, filterValue));
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        private void SelectBlobFeature(MilBlobObject blobFeatureList, BlobParam blobParam)
        {
            if (blobParam.SelectArea == true)
            {
                //MIL.MblobControl(blobFeatureList.Id, MIL.M_AREA, MIL.M_ENABLE);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_SORT1, MIL.M_AREA);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_SORT1_DIRECTION, MIL.M_SORT_DOWN);
            }

            if (blobParam.SelectBoundingRect == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_BOX, MIL.M_ENABLE);
            }

            if (blobParam.SelectCenterPt == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CENTER_OF_GRAVITY, MIL.M_ENABLE);
                //MIL.MblobControl(blobFeatureList.Id, MIL.M_CENTER_OF_GRAVITY_Y, MIL.M_ENABLE);
            }

            if (blobParam.SelectConvexFillRatio == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CONVEX_HULL_FILL_RATIO, MIL.M_ENABLE);
            }

            //if (blobParam.SelectLabelValue == true)
            //MIL.MblobControl(blobFeatureList.Id, MIL.M_LABEL_VALUE, MIL.M_ENABLE);

            if (blobParam.SelectCompactness == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_COMPACTNESS, MIL.M_ENABLE);
            }

            if (blobParam.SelectNumberOfHoles == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_NUMBER_OF_HOLES, MIL.M_ENABLE);
            }

            if (blobParam.SelectMeanValue == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MEAN_PIXEL, MIL.M_ENABLE);
            }

            if (blobParam.SelectMinValue == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MIN_PIXEL, MIL.M_ENABLE);
            }

            if (blobParam.SelectMaxValue == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MAX_PIXEL, MIL.M_ENABLE);
            }

            if (blobParam.SelectSigmaValue == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_SIGMA_PIXEL, MIL.M_ENABLE);
            }

            if (blobParam.SelectAspectRatio == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_BOX_ASPECT_RATIO, MIL.M_ENABLE);
            }

            if (blobParam.SelectRectangularity == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_RECTANGULARITY, MIL.M_ENABLE);
            }

            if (blobParam.GroupSameLabelAndTouchingBlobs == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_BLOB_IDENTIFICATION, MIL.M_LABELED);
            }

            if (blobParam.SelectMinAreaBox == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MIN_AREA_BOX, MIL.M_ENABLE);
            }

            if (blobParam.SelectConvexHull == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CHAIN_X, MIL.M_ENABLE);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CHAIN_Y, MIL.M_ENABLE);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CHAIN_INDEX, MIL.M_ENABLE);
            }

            if (blobParam.MaxCount > 0)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_RETURN_PARTIAL_RESULTS, MIL.M_ENABLE);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MAX_BLOBS, blobParam.MaxCount);
            }
        }

        public override void FilterBlob(BlobRectList blobRectList, BlobParam blobParam, bool invert = false)
        {
            MilBlobObject blobResult = (blobRectList as MilBlobRectList).BlobResult;
            blobRectList.Clear();

            FilterBlob(blobResult, blobParam, invert);
            GetBlobBox(blobResult, blobRectList, blobParam);
        }

        private void FilterBlob(MilBlobObject blobResult, BlobParam blobParam, bool invert = false)
        {
            //MIL.MblobSelect(blobResult.Id, MIL.M_EXCLUDE, MIL.M_AREA, MIL.M_LESS_OR_EQUAL, 1L, MIL.M_NULL);

            if (blobParam.AreaMin > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, invert ? MIL.M_GREATER : MIL.M_LESS, blobParam.AreaMin, MIL.M_NULL);
            }

            if (blobParam.AreaMax > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, invert ? MIL.M_LESS : MIL.M_GREATER, blobParam.AreaMax, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMinX > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_X_MIN, invert ? MIL.M_GREATER : MIL.M_LESS, blobParam.BoundingRectMinX, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMinY > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_Y_MIN, invert ? MIL.M_GREATER : MIL.M_LESS, blobParam.BoundingRectMinY, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMaxX > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_X_MAX, invert ? MIL.M_LESS : MIL.M_GREATER, blobParam.BoundingRectMaxX, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMaxY > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_Y_MAX, invert ? MIL.M_LESS : MIL.M_GREATER, blobParam.BoundingRectMaxY, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMinWidth > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_WIDTH, invert ? MIL.M_GREATER : MIL.M_LESS, blobParam.BoundingRectMinWidth, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMaxWidth > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_WIDTH, invert ? MIL.M_LESS : MIL.M_GREATER, blobParam.BoundingRectMaxWidth, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMinHeight > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_HEIGHT, invert ? MIL.M_GREATER : MIL.M_LESS, blobParam.BoundingRectMinHeight, MIL.M_NULL);
            }

            if (blobParam.BoundingRectMaxHeight > 0)
            {
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_HEIGHT, invert ? MIL.M_LESS : MIL.M_GREATER, blobParam.BoundingRectMaxHeight, MIL.M_NULL);
            }

            if (blobParam.EraseBorderBlob == true)
            {
                MIL.MblobSelect(blobResult.Id, invert ? MIL.M_INCLUDE_ONLY : MIL.M_DELETE, MIL.M_BLOB_TOUCHING_IMAGE_BORDERS, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL);
            }
        }

        private void GetBlobBox(MilBlobObject blobResult, BlobRectList blobRectList, BlobParam blobParam)
        {
            double numBlobDbl = 0;
            MIL.MblobGetResult(blobResult.Id, MIL.M_GENERAL, MIL.M_NUMBER, ref numBlobDbl);

            int numBlob = (int)numBlobDbl;
            if (numBlob > 0)
            {
                double[] leftArray = new double[numBlob];
                double[] rightArray = new double[numBlob];
                double[] topArray = new double[numBlob];
                double[] bottomArray = new double[numBlob];
                double[] centerXArray = new double[numBlob];
                double[] centerYArray = new double[numBlob];
                double[] areaArray = new double[numBlob];
                double[] labelNumArray = new double[numBlob];
                double[] sigmaValueArray = new double[numBlob];
                double[] minValueArray = new double[numBlob];
                double[] maxValueArray = new double[numBlob];
                double[] meanValueArray = new double[numBlob];
                double[] compactnessArray = new double[numBlob];
                double[] convexFillRatioArray = new double[numBlob];
                double[] aspectRetioArray = new double[numBlob];
                double[] rectangularityArray = new double[numBlob];
                var numberOfHoles = new MIL_INT[numBlob];

                double[] convexHullArray = new double[numBlob];

                if (blobParam.SelectCenterPt)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_CENTER_OF_GRAVITY_X + MIL.M_TYPE_DOUBLE, centerXArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_CENTER_OF_GRAVITY_Y + MIL.M_TYPE_DOUBLE, centerYArray);
                }

                if (blobParam.SelectBoundingRect == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_BOX_X_MIN + MIL.M_TYPE_DOUBLE, leftArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_BOX_Y_MIN + MIL.M_TYPE_DOUBLE, topArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_BOX_X_MAX + MIL.M_TYPE_DOUBLE, rightArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_BOX_Y_MAX + MIL.M_TYPE_DOUBLE, bottomArray);
                }

                if (blobParam.SelectArea == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_AREA + MIL.M_TYPE_DOUBLE, areaArray);
                }

                if (blobParam.SelectLabelValue == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_LABEL_VALUE + MIL.M_TYPE_DOUBLE, labelNumArray);
                }

                if (blobParam.SelectNumberOfHoles)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_NUMBER_OF_HOLES + MIL.M_TYPE_MIL_INT, numberOfHoles);
                }

                if (blobParam.SelectMinValue == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_MIN_PIXEL + MIL.M_TYPE_DOUBLE, minValueArray);
                }

                if (blobParam.SelectMaxValue == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_MAX_PIXEL + MIL.M_TYPE_DOUBLE, maxValueArray);
                }

                if (blobParam.SelectMeanValue == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_MEAN_PIXEL + MIL.M_TYPE_DOUBLE, meanValueArray);
                }

                if (blobParam.SelectSigmaValue == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_SIGMA_PIXEL + MIL.M_TYPE_DOUBLE, sigmaValueArray);
                }

                if (blobParam.SelectCompactness == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_COMPACTNESS + MIL.M_TYPE_DOUBLE, compactnessArray);
                }

                if (blobParam.SelectConvexFillRatio == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_CONVEX_HULL_FILL_RATIO + MIL.M_TYPE_DOUBLE, convexFillRatioArray);
                }

                if (blobParam.SelectAspectRatio == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_BOX_ASPECT_RATIO + MIL.M_TYPE_DOUBLE, aspectRetioArray);
                }

                if (blobParam.SelectRectangularity == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_INCLUDED_BLOBS, MIL.M_RECTANGULARITY + MIL.M_TYPE_DOUBLE, rectangularityArray);
                }

                for (int i = 0; i < numBlob; i++)
                {
                    if (blobParam.MaxCount != 0 && i >= blobParam.MaxCount)
                    {
                        break;
                    }

                    var blobRect = new BlobRect();
                    blobRect.LabelNumber = (int)labelNumArray[i];
                    blobRect.Area = (long)areaArray[i];
                    blobRect.CenterPt = new PointF((float)centerXArray[i], (float)centerYArray[i]);
                    blobRect.BoundingRect = new RectangleF((float)leftArray[i], (float)topArray[i], (float)(rightArray[i] - leftArray[i] + 1), (float)(bottomArray[i] - topArray[i] + 1));
                    blobRectList.AddBlobRect(blobRect);
                    blobRect.SigmaValue = (float)sigmaValueArray[i];
                    blobRect.MinValue = (float)minValueArray[i];
                    blobRect.MaxValue = (float)maxValueArray[i];
                    blobRect.MeanValue = (float)meanValueArray[i];
                    blobRect.Compactness = (float)compactnessArray[i];
                    blobRect.NumberOfHoles = (int)numberOfHoles[i];
                    blobRect.ConvexFillRatio = (int)convexFillRatioArray[i];
                    blobRect.AspectRetio = (float)aspectRetioArray[i];
                    blobRect.Rectangularity = (float)rectangularityArray[i];

                    if (blobParam.SelectConvexHull == true)
                    {
                        blobRect.ConvexHullPointList = new List<PointF>();

                        double[] xArray = new double[(int)convexHullArray[i]];
                        double[] yArray = new double[(int)convexHullArray[i]];
                        double[] indexArray = new double[(int)convexHullArray[i]];

                        MIL.MblobGetResult(blobResult.Id, MIL.M_BLOB_LABEL((int)labelNumArray[i]), MIL.M_CHAIN_X + MIL.M_TYPE_DOUBLE, xArray);
                        MIL.MblobGetResult(blobResult.Id, MIL.M_BLOB_LABEL((int)labelNumArray[i]), MIL.M_CHAIN_Y + MIL.M_TYPE_DOUBLE, yArray);
                        MIL.MblobGetResult(blobResult.Id, MIL.M_BLOB_LABEL((int)labelNumArray[i]), MIL.M_CHAIN_INDEX + MIL.M_TYPE_DOUBLE, indexArray);

                        for (int j = 0; j < convexHullArray[i]; j++)
                        {
                            if (indexArray[j] == 1)
                            {
                                blobRect.ConvexHullPointList.Add(new PointF((float)xArray[j], (float)yArray[j]));
                            }
                        }
                    }
                }
            }
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Blob", "Source");

            var milGreyImage = algoImage as MilGreyImage;

            var blobRectList = new MilBlobRectList();
            var blobFeatureList = new MilBlobObject();

            var blobResult = new MilBlobObject();

            blobRectList.BlobResult = blobResult;

            MIL_ID null_id = MIL.M_NULL;
            blobFeatureList.Id = MIL.MblobAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref null_id);
            blobResult.Id = MIL.MblobAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref null_id);

            if (blobFeatureList.Id == MIL.M_NULL || blobResult.Id == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilImageProcessing.Blob]");
            }

            if (blobParam.SelectWholeImage == true)
            {
                MIL.MblobControl(blobResult.Id, MIL.M_BLOB_IDENTIFICATION, MIL.M_WHOLE_IMAGE);
            }

            MIL.MblobControl(blobResult.Id, MIL.M_CONNECTIVITY, MIL.M_8_CONNECTED);
            MIL.MblobControl(blobResult.Id, MIL.M_IDENTIFIER_TYPE, MIL.M_BINARY);

            SelectBlobFeature(blobFeatureList, blobParam);

            if (blobParam.SaveContour)
            {
                MIL.MblobControl(blobResult.Id, MIL.M_SAVE_RUNS, MIL.M_ENABLE);
            }

            MIL.MblobCalculate(blobFeatureList.Id, milGreyImage.Image, MIL.M_NULL, blobResult.Id);

            FilterBlob(blobResult, blobParam);

            GetBlobBox(blobResult, blobRectList, blobParam);

            blobFeatureList.Dispose();

            return blobRectList;
        }

        public override BlobRectList Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam)
        {
            var blobRectList = new MilBlobRectList();

            try
            {
                MilGreyImage milGreyImage = MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Blob", "Source");
                MilGreyImage milGreyMaskImage = MilImage.CheckGreyImage(greyMask, "MilImageProcessing.Blob", "GreyMask");
                var blobFeatureList = new MilBlobObject();
                var blobResult = new MilBlobObject();

                blobRectList.BlobResult = blobResult;

                MIL_ID null_id = MIL.M_NULL;
                blobFeatureList.Id = MIL.MblobAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref null_id);
                blobResult.Id = MIL.MblobAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref null_id);

                if (blobFeatureList.Id == MIL.M_NULL || blobResult.Id == MIL.M_NULL)
                {
                    throw new AllocFailedException("[MilImageProcessing.Blob]");
                }

                SelectBlobFeature(blobFeatureList, blobParam);

                MIL.MblobControl(blobResult.Id, MIL.M_IDENTIFIER_TYPE, MIL.M_BINARY);

                if (blobParam.SaveContour)
                {
                    MIL.MblobControl(blobResult.Id, MIL.M_SAVE_RUNS, MIL.M_ENABLE);
                }

                MIL.MblobCalculate(blobFeatureList.Id, milGreyImage.Image, milGreyMaskImage.Image, blobResult.Id);

                FilterBlob(blobResult, blobParam);

                GetBlobBox(blobResult, blobRectList, blobParam);

                //MIL.MblobFree(blobFeatureList.Id);
                //MIL.MblobFree(blobResult.Id);

                blobFeatureList.Dispose();
                //blobResult.Dispose();
            }
            catch (EntryPointNotFoundException e)
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("Blob Error : {0}", e.Message));
                //algoImage.Save(@"D:\Debug\BlobFailImage.bmp");
            }

            return blobRectList;
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            var milBlobRectList1 = (MilBlobRectList)blobRectList1;
            var milBlobRectList2 = (MilBlobRectList)blobRectList2;

            var blobRectList = new MilBlobRectList();

            var blobResult = new MilBlobObject();

            blobRectList.BlobResult = blobResult;

            MIL_ID null_id = MIL.M_NULL;
            blobResult.Id = MIL.MblobAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref null_id);

            if (blobResult.Id == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilImageProcessing.Blob]");
            }

            MIL.MblobMerge(milBlobRectList1.BlobResult.Id, milBlobRectList2.BlobResult.Id, blobResult.Id, MIL.M_DEFAULT);

            FilterBlob(blobResult, blobParam);
            GetBlobBox(blobResult, blobRectList, blobParam);

            return blobRectList;
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.DrawBlob", "AlgoImage");

            var milGreyImage = algoImage as MilGreyImage;

            var milBlobRectList = (MilBlobRectList)blobRectList;

            if (blobRect == null)
            {
                if (drawBlobOption.SelectBlob)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS, MIL.M_ALL_BLOBS, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectBlobContour)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS_CONTOUR, MIL.M_ALL_BLOBS, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectHoles)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES, MIL.M_ALL_BLOBS, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectHolesContour)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES_CONTOUR, MIL.M_ALL_BLOBS, MIL.M_DEFAULT);
                }
            }
            else
            {
                if (drawBlobOption.SelectBlob)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS, blobRect.LabelNumber, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectBlobContour)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS_CONTOUR, blobRect.LabelNumber, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectHoles)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES, blobRect.LabelNumber, MIL.M_DEFAULT);
                }

                if (drawBlobOption.SelectHolesContour)
                {
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES_CONTOUR, blobRect.LabelNumber, MIL.M_DEFAULT);
                }
            }
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage = null)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");
            }

            var milSrcImage = srcImage as MilGreyImage;
            if (destImage == null)
            {
                destImage = milSrcImage;
            }

            var milDestImage = destImage as MilGreyImage;

            MIL.MimConvolve(milSrcImage.Image, milDestImage.Image, MIL.M_SMOOTH);
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            const int HIST_NUM_INTENSITIES = 256;
            MIL_ID HistResult = MIL.M_NULL;                       // Histogram buffer identifier.
            var milSrcImage = algoImage as MilGreyImage;
            MIL.MimAllocResult(MIL.M_DEFAULT_HOST, HIST_NUM_INTENSITIES, MIL.M_HIST_LIST, ref HistResult);
            MIL.MimHistogram(milSrcImage.Image, HistResult);
            var HistValues = new MIL_INT[HIST_NUM_INTENSITIES];
            MIL.MimGetResult(HistResult, MIL.M_VALUE, HistValues);

            long[] retVal = new long[HistValues.Length];
            for (int i = 0; i < HistValues.Length; i++)
            {
                retVal[i] = HistValues[i];
            }

            MIL.MbufFree(HistResult);
            return retVal;
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {

        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            var milSrcImage = algoImage as MilGreyImage;

            MIL.MimHistogramEqualize(milSrcImage.Image, milSrcImage.Image, MIL.M_HYPER_LOG, 0, 0, 255);
        }

        public override AlgoImage FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");
            }

            var milSrcImage = srcImage as MilGreyImage;

            if (destImage == null)
            {
                destImage = new MilGreyImage(milSrcImage.Width, milSrcImage.Height);
            }

            var milDestImage = destImage as MilGreyImage;

            MIL.MblobReconstruct(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_FILL_HOLES, MIL.M_DEFAULT);

            return destImage;
        }

        public override AlgoImage Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool abs = false)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Subtract", "Source1");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Subtract", "Source2");
            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.Subtract", "Destination");
            }

            var milSrc1Image = image1 as MilGreyImage;
            var milSrc2Image = image2 as MilGreyImage;
            MilGreyImage milDestImage;

            if (destImage == null)
            {
                destImage = new MilGreyImage(milSrc1Image.Width, milSrc1Image.Height);
            }

            milDestImage = destImage as MilGreyImage;

            if (abs == false)
            {
                MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_SUB + MIL.M_SATURATION);
            }
            else
            {
                MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_SUB_ABS);
            }

            return destImage;
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");
            }

            var milSrcImage = srcImage as MilGreyImage;
            if (destImage == null)
            {
                destImage = milSrcImage;
            }

            var milDestImage = destImage as MilGreyImage;
            MIL.MimRank(milSrcImage.Image, milDestImage.Image, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_GRAYSCALE);
        }

        public override void Clear(AlgoImage algoImage, byte value)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Or", "Source1");

            var milSrcImage = algoImage as MilGreyImage;

            MIL.MbufClear(milSrcImage.Image, value);
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Or", "Source1");

            var milSrcImage = srcImage as MilGreyImage;

            if (value == Color.Black)
            {
                MIL.MbufClear(milSrcImage.Image, MIL.M_COLOR_BLACK);
            }
            else if (value == Color.Blue)
            {
                MIL.MbufClear(milSrcImage.Image, MIL.M_COLOR_BLUE);
            }
        }

        public override AlgoImage Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage1, "MilImageProcessing.Or", "Source1");
            MilImage.CheckGreyImage(algoImage2, "MilImageProcessing.Or", "Source2");

            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.Or", "Destination");
            }

            var milSrc1Image = algoImage1 as MilGreyImage;
            var milSrc2Image = algoImage2 as MilGreyImage;
            MilGreyImage milDestImage;

            if (destImage == null)
            {
                destImage = new MilGreyImage(milSrc1Image.Width, milSrc1Image.Height);
            }

            milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_OR);

            return destImage;
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Translate", "Source1");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Translate", "Source2");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimTranslate(milSrcImage.Image, milDestImage.Image, offset.X, offset.Y, MIL.M_DEFAULT);
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, PointF centerPt, float angle)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Rotate", "Source1");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Rotate", "Source2");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimRotate(milSrcImage.Image, milDestImage.Image, angle, centerPt.X, centerPt.Y, centerPt.X, centerPt.Y, MIL.M_BILINEAR);
        }

        public override void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled)
        {
            var milSrcImage = srcImage as MilImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);

            if (milSrcImage is MilGreyImage)
            {
                MIL.MgraColor(milGraphicsContext, value);
            }
            else if (milSrcImage is MilColorImage)
            {
                int v = (int)value;
                int a = (v >> 24) & 0xff;
                int r = (v >> 16) & 0xff;
                int g = (v >> 8) & 0xff;
                int b = (v >> 0) & 0xff;
                MIL.MgraColor(milGraphicsContext, MIL.M_RGB888(r, g, b));
            }

            if (filled == true)
            {
                MIL.MgraRectFill(milGraphicsContext, milSrcImage.Image, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }
            else
            {
                MIL.MgraRect(milGraphicsContext, milSrcImage.Image, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            MIL.MgraFree(milGraphicsContext);
        }

        public override void DrawText(AlgoImage srcImage, Point point, double value, string text)
        {
            var milImage = srcImage as MilImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);
            if (milImage is MilGreyImage)
            {
                MIL.MgraColor(milGraphicsContext, value);
            }
            else if (milImage is MilColorImage)
            {
                int v = (int)value;
                int a = (v >> 24) & 0xff;
                int r = (v >> 16) & 0xff;
                int g = (v >> 8) & 0xff;
                int b = (v >> 0) & 0xff;
                MIL.MgraColor(milGraphicsContext, MIL.M_RGB888(r, g, b));
            }
            MIL.MgraText(milGraphicsContext, milImage.Image, point.X, point.Y, text);
            MIL.MgraFree(milGraphicsContext);
        }

        public override void DrawRotateRact(AlgoImage srcImage, RotatedRect rotateRect, double value, bool filled)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.DarwRotateRact", "Source");

            var milSrcImage = srcImage as MilGreyImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);
            MIL.MgraColor(milGraphicsContext, value);

            if (filled == true)
            {
                MIL.MgraRectAngle(milGraphicsContext, milSrcImage.Image, rotateRect.Left, rotateRect.Top, rotateRect.Right, rotateRect.Bottom, rotateRect.Angle, MIL.M_CENTER_AND_DIMENSION + MIL.M_FILLED);
            }
            else
            {
                MIL.MgraRectAngle(milGraphicsContext, milSrcImage.Image, rotateRect.Left, rotateRect.Top, rotateRect.Right, rotateRect.Bottom, rotateRect.Angle, MIL.M_CENTER_AND_DIMENSION);
            }

            MIL.MgraFree(milGraphicsContext);
        }

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.DarwCircle", "Source");

            var milSrcImage = srcImage as MilGreyImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);
            MIL.MgraColor(milGraphicsContext, value);

            if (filled == true)
            {
                MIL.MgraArcFill(milGraphicsContext, milSrcImage.Image, arcEq.Center.X, arcEq.Center.Y, arcEq.XRadius, arcEq.YRadius, arcEq.StartAngle, arcEq.EndAngle);
            }
            else
            {
                MIL.MgraArc(milGraphicsContext, milSrcImage.Image, arcEq.Center.X, arcEq.Center.Y, arcEq.XRadius, arcEq.YRadius, arcEq.StartAngle, arcEq.EndAngle);
            }

            MIL.MgraFree(milGraphicsContext);
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.EraseBoder", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.EraseBoder", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MblobReconstruct(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_ERASE_BORDER_BLOBS, MIL.M_DEFAULT);
        }
        public override void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.ResconstructIncludeBlob", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.ResconstructIncludeBlob", "Destination");
            MilImage.CheckGreyImage(seedImage, "MilImageProcessing.ResconstructIncludeBlob", "Seed");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;
            var milSeedImage = seedImage as MilGreyImage;

            MIL.MblobReconstruct(milSrcImage.Image, milSeedImage.Image, milDestImage.Image, MIL.M_RECONSTRUCT_FROM_SEED, MIL.M_DEFAULT);
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.EdgeDetect", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.EdgeDetect", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MilGreyImage milMaskImage = null;

            if (maskImage != null)
            {
                MilImage.CheckGreyImage(maskImage, "MilImageProcessing.EdgeDetect", "Mask");
                milMaskImage = maskImage as MilGreyImage;
            }

            MIL_ID milEdgeContext = MIL.M_NULL;
            MIL_ID milEdgeResult = MIL.M_NULL;

            MIL.MedgeAlloc(MIL.M_DEFAULT_HOST, MIL.M_CONTOUR, MIL.M_DEFAULT, ref milEdgeContext);

            MIL.MedgeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref milEdgeResult);

            MIL.MedgeControl(milEdgeContext, MIL.M_FILTER_TYPE, MIL.M_SOBEL);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILTER_SMOOTHNESS, 0);

            MIL.MedgeControl(milEdgeContext, MIL.M_ACCURACY, MIL.M_HIGH);
            MIL.MedgeControl(milEdgeContext, MIL.M_ANGLE_ACCURACY, MIL.M_HIGH);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_MODE, MIL.M_MEDIUM);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_TYPE, MIL.M_HYSTERESIS);

            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_DISTANCE, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_ANGLE, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_CONTINUITY, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_TYPE, MIL.M_HYSTERESIS);

            MIL.MedgeControl(milEdgeContext, MIL.M_EXTRACTION_SCALE, scaleFactor);

            MIL.MedgeControl(milEdgeContext, MIL.M_STRENGTH, MIL.M_ENABLE);
            if (maskImage != null)
            {
                MIL.MedgeMask(milEdgeContext, milMaskImage.Image, MIL.M_DEFAULT);
            }

            MIL.MedgeCalculate(milEdgeContext, milSrcImage.Image, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, milEdgeResult, MIL.M_DEFAULT);

            MIL.MedgeSelect(milEdgeResult, MIL.M_DELETE, MIL.M_STRENGTH, MIL.M_LESS, 5000, MIL.M_NULL);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
            MIL.MedgeDraw(MIL.M_DEFAULT, milEdgeResult, milDestImage.Image, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MedgeFree(milEdgeContext);
            MIL.MedgeFree(milEdgeResult);
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactor)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Resize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Resize", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimResize(milSrcImage.Image, milDestImage.Image, scaleFactor, scaleFactor, MIL.M_INTERPOLATE);
        }

        public void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Resize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Resize", "Destination");

            var milSrcImage = srcImage as MilGreyImage;
            var milDestImage = destImage as MilGreyImage;

            MIL.MimResize(milSrcImage.Image, milDestImage.Image, scaleFactorX, scaleFactorY, MIL.M_INTERPOLATE);
        }
    }
}
