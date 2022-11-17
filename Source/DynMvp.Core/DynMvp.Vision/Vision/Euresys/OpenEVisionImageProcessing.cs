using DynMvp.Base;
using DynMvp.UI;
using Euresys.Open_eVision_1_2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Euresys
{
    internal class OpenEVisionImageProcessing : ImageProcessing
    {
        // Auto Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
            var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

            EBW8 threshold = EasyImage.AutoThreshold(openEVisionSrcImage.Image, EThresholdMode.MaxEntropy);
            EasyImage.Threshold(openEVisionSrcImage.Image, openEVisionDestImage.Image, threshold.Value);
        }

        // Single Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
            var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

            EasyImage.Threshold(openEVisionSrcImage.Image, openEVisionDestImage.Image, threshold);
        }

        // Double Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
            var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

            EasyImage.DoubleThreshold(openEVisionSrcImage.Image, openEVisionDestImage.Image, thresholdLower, thresholdUpper);
        }

        public override void AdaptiveBinarize(AlgoImage algoImage1, AlgoImage algoImage2, int thresholdLower)
        {
            throw new NotImplementedException();
        }

        public override void CustomBinarize(AlgoImage algoImage1, AlgoImage algoImage2, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray)
        {
            var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
            var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

            EasyImage.Erode(openEVisionSrcImage.Image, openEVisionDestImage.Image, numErode);
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
            var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

            EasyImage.Dilate(openEVisionSrcImage.Image, openEVisionDestImage.Image, numDilate);
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override int Count(AlgoImage algoImage, AlgoImage maskImage = null)
        {
            var greyImage = (OpenEVisionGreyImage)algoImage;

            int numBlackPixel;
            int numGreyPixel;
            int numWhitePixel;

            if (maskImage != null)
            {
                var greyMaskImage = (OpenEVisionGreyImage)maskImage;

                EasyImage.PixelCount(greyImage.Image, greyMaskImage.Image, new EBW8(10), new EBW8(250), out numBlackPixel, out numGreyPixel, out numWhitePixel);
            }
            else
            {
                EasyImage.PixelCount(greyImage.Image, new EBW8(10), new EBW8(250), out numBlackPixel, out numGreyPixel, out numWhitePixel);
            }

            return numWhitePixel;
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
            var greyImage = (OpenEVisionGreyImage)algoImage;

            EasyImage.PixelAverage(greyImage.Image, out float average);

            return average;
        }

        public override float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetStdDev(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override void Canny(AlgoImage srcImage, AlgoImage dstImage, double threshold, double thresholdLinking)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage algoImage, AlgoImage maskImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
        {
            throw new NotImplementedException();
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage = null)
        {
            Debug.Assert(destImage == null);

            if (srcImage.ImageType == ImageType.Grey)
            {
                EasyImage.ConvolLowpass1(((OpenEVisionGreyImage)srcImage).Image);
            }
            else
            {
                EasyImage.ConvolLowpass1(((OpenEVisionColorImage)srcImage).Image);
            }
        }

        private void AverageColor(OpenEVisionColorImage colorImage)
        {
            EasyImage.ConvolLowpass1(colorImage.Image);
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, int size = 3)
        {
            if (srcImage.ImageType == ImageType.Grey)
            {
                var openEVisionSrcImage = (OpenEVisionGreyImage)srcImage;
                var openEVisionDestImage = (OpenEVisionGreyImage)destImage;

                EasyImage.ConvolSobel(openEVisionSrcImage.Image, openEVisionDestImage.Image);
            }
            else
            {
                var openEVisionSrcImage = (OpenEVisionColorImage)srcImage;
                var openEVisionDestImage = (OpenEVisionColorImage)destImage;

                EasyImage.ConvolSobel(openEVisionSrcImage.Image, openEVisionDestImage.Image);
            }
        }

        public override AlgoImage And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Not(AlgoImage algoImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            if (algoImage.ImageType != ImageType.Grey)
            {
                throw new InvalidImageFormatException();
            }

            var greyImage = (OpenEVisionGreyImage)algoImage;

            var histogramVector = new EBWHistogramVector();

            EasyImage.Histogram(greyImage.Image, histogramVector);

            float maxIndex = EasyImage.AnalyseHistogram(histogramVector, EHistogramFeature.GreatestPixelValue);
            float minIndex = EasyImage.AnalyseHistogram(histogramVector, EHistogramFeature.SmallestPixelValue);

            var lutVector = new EBW8Vector();

            double scale_factor = 256.0 / (maxIndex - minIndex);

            for (int i = 0; i < 256; i++)
            {
                byte value;

                if (i < minIndex)
                {
                    value = 0;
                }
                else if (i > maxIndex)
                {
                    value = 255;
                }
                else
                {
                    int intValue = (int)((i - minIndex) * scale_factor);
                    if (intValue < 255)
                    {
                        value = (byte)((i - minIndex) * scale_factor);
                    }
                    else
                    {
                        value = 255;
                    }
                }

                lutVector.AddElement(new EBW8(value));
            }

            var destImage = new EImageBW8(greyImage.Image.Width, greyImage.Image.Height);
            EasyImage.Lut(greyImage.Image, destImage, lutVector);

            greyImage.Image = destImage;
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            if (algoImage.ImageType != ImageType.Grey)
            {
                throw new InvalidImageFormatException();
            }

            var greyImage = (OpenEVisionGreyImage)algoImage;

            var destImage = new EImageBW8(greyImage.Image.Width, greyImage.Image.Height);
            EasyImage.Equalize(greyImage.Image, destImage);

            greyImage.Image = destImage;
        }

        public override AlgoImage FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool abs = false)
        {
            throw new NotImplementedException();
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage algoImage, byte value)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, PointF centerPt, float angle)
        {
            throw new NotImplementedException();
        }

        public override void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawRotateRact(AlgoImage srcImage, RotatedRect rotateRect, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }
        public override void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage)
        {
            throw new NotImplementedException();
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            throw new NotImplementedException();
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactor)
        {
            throw new NotImplementedException();
        }

        public override void Smooth(AlgoImage srcImage, AlgoImage destImage, int numSmooth)
        {
            throw new NotImplementedException();
        }

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void Thick(AlgoImage srcImage, AlgoImage destImage, int numThick, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void FilterBlob(BlobRectList blobRectList, BlobParam blobParam, bool invert = false)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(AlgoImage srcImage, Point point, double value, string text)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numErode, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numDilate, bool useGray = false)
        {
            throw new NotImplementedException();
        }
    }
}
