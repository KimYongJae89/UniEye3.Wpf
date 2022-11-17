using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision
{
    public enum TwoWayDirection
    {
        Horizontal, Vertical
    }

    public enum ProjectionType
    {
        Sum, Mean, Max, Min
    }

    public enum BinarizationType
    {
        SingleThreshold, DoubleThreshold, AutoThreshold, AdaptiveThreshold, Cuda, Custom
    }

    public enum ScoreType
    {
        Ratio, Count
    }

    public struct StatResult
    {
        public float count;
        public float average;
        public float min;
        public float max;
        public float stdDev;
        public float squareSum;
    }

    public abstract class ImageProcessing
    {
        public void Binarize(AlgoImage srcImage, BinarizationType binarizationType, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            Binarize(srcImage, srcImage, binarizationType, thresholdLower, thresholdUpper, inverse);
        }

        public void Binarize(AlgoImage srcImage, AlgoImage destImage, BinarizationType binarizationType, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            switch (binarizationType)
            {
                case BinarizationType.SingleThreshold:
                    Binarize(srcImage, destImage, thresholdLower, inverse);
                    break;
                case BinarizationType.DoubleThreshold:
                    Binarize(srcImage, destImage, thresholdLower, thresholdUpper, inverse);
                    break;
                case BinarizationType.AutoThreshold:
                    //Otsu(srcImage, destImage);
                    Binarize(srcImage, srcImage, inverse);
                    break;
                case BinarizationType.AdaptiveThreshold:
                    AdaptiveBinarize(srcImage, destImage, thresholdLower);
                    break;
                case BinarizationType.Custom:
                    CustomBinarize(srcImage, destImage, inverse);
                    break;
            }
        }

        public abstract void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse);
        public abstract void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower);

        public void Binarize(AlgoImage algoImage, bool inverse = false)
        {
            Binarize(algoImage, algoImage, inverse);
        }

        public void Binarize(AlgoImage algoImage, int threshold, bool inverse = false)
        {
            Binarize(algoImage, algoImage, threshold, inverse);
        }

        public void Binarize(AlgoImage algoImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            Binarize(algoImage, algoImage, thresholdLower, thresholdUpper, inverse);
        }

        public void Erode(AlgoImage algoImage, int numErode)
        {
            Erode(algoImage, algoImage, numErode);
        }

        public void Dilate(AlgoImage algoImage, int numDilate)
        {
            Dilate(algoImage, algoImage, numDilate);
        }

        public void Open(AlgoImage algoImage, int numOpen)
        {
            Open(algoImage, algoImage, numOpen);
        }

        public void Close(AlgoImage algoImage, int numClose)
        {
            Close(algoImage, algoImage, numClose);
        }

        public abstract void Smooth(AlgoImage srcImage, AlgoImage destImage, int numSmooth);

        // Auto Threshold
        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false);

        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false);
        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false);

        public abstract void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray = false);
        public abstract void Erode(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numErode, bool useGray = false);
        public abstract void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray = false);
        public abstract void Dilate(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numDilate, bool useGray = false);
        public abstract void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false);
        public abstract void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false);
        public abstract void TopHat(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false);
        public abstract void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false);
        public abstract void Thick(AlgoImage srcImage, AlgoImage destImage, int numThick, bool useGray = false);

        public abstract int Count(AlgoImage algoImage, AlgoImage maskImage = null);

        public abstract float GetGreyAverage(AlgoImage algoImage);
        public abstract float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage);

        public Color GetColorAverage(AlgoImage algoImage)
        {
            return GetColorAverage(algoImage, Rectangle.Empty);
        }

        public abstract Color GetColorAverage(AlgoImage algoImage, Rectangle rect);
        public abstract Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage);

        public abstract float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage);

        public abstract float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage);

        public abstract float GetStdDev(AlgoImage algoImage);
        public abstract float GetStdDev(AlgoImage algoImage, AlgoImage maskImage);

        public StatResult GetStatValue(AlgoImage algoImage) { return GetStatValue(algoImage, null); }
        public abstract StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage);

        public Bitmap CreateRectMask(int width, int height, RectangleF maskRect)
        {
            var imageRect = new Rectangle(0, 0, width, height);

            var rgbImage = new Bitmap(width, height);

            var g = Graphics.FromImage(rgbImage);
            g.FillRectangle(new SolidBrush(Color.Black), imageRect);
            var figure = new RectangleFigure(maskRect, new Pen(Color.White), new SolidBrush(Color.White));
            figure.Draw(g, new CoordTransformer(), true);

            g.Dispose();

            Bitmap grayImage = ImageHelper.ConvertGrayImage(rgbImage);

            rgbImage.Dispose();

            return grayImage;
        }

        public Bitmap CreateCircleMask(int width, int height, int xRadius = 0, int yRadius = 0, int centerX = 0, int centerY = 0)
        {
            var imageRect = new Rectangle(0, 0, width, height);

            Rectangle circleRect;
            if (xRadius == 0 || yRadius == 0)
            {
                circleRect = imageRect;
            }
            else
            {
                if (centerX == 0 && centerY == 0)
                {
                    circleRect = new Rectangle(width / 2 - xRadius, height / 2 - yRadius, xRadius * 2, yRadius * 2);
                }
                else
                {
                    circleRect = new Rectangle(centerX - xRadius, centerY - yRadius, xRadius * 2, yRadius * 2);
                }
            }

            var rgbImage = new Bitmap(width, height);

            var g = Graphics.FromImage(rgbImage);
            g.FillRectangle(new SolidBrush(Color.Black), imageRect);
            var figure = new EllipseFigure(circleRect, new Pen(Color.White), new SolidBrush(Color.White));
            figure.Draw(g, new CoordTransformer(), true);

            g.Dispose();

            //            ImageHelper.SaveImage(rgbImage, String.Format("{0}\\MaskImage.bmp", Configuration.TempFolder));

            Bitmap grayImage = ImageHelper.ConvertGrayImage(rgbImage);

            rgbImage.Dispose();

            return grayImage;
        }

        public float Otsu(AlgoImage srcImage, AlgoImage maskImage = null)
        {
            byte[] imageData = srcImage.GetByte();
            byte[] maskData = maskImage?.GetByte();
            if (maskData != null)
            {
                Debug.Assert(imageData.Length == maskData.Length);
            }

            long length = imageData.Length;

            float[] histogram = new float[256];
            if (maskData == null)
            {
                // 650ms
                //var groups = imageData.GroupBy(f => f);
                //foreach (IGrouping<byte, byte> group in groups)
                //    histogram[group.Key] = group.Count();

                // 200ms
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        int index = (j * srcImage.Width) + i;
                        histogram[imageData[index]]++;
                    }
                }
            }
            else
            {
                //imageData.Select((f, i) => maskData[i] > 0 ? f : -1).ToList();
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        int index = (j * srcImage.Width) + i;
                        if (maskData[index] > 0)
                        {
                            histogram[imageData[index]]++;
                        }
                    }
                }
            }

            int total = (int)histogram.Sum();

            float thresholdValue = Otsu(histogram, total);
            return thresholdValue;
        }

        private float Otsu(float[] histogram, int total)
        {
            double sum = 0;
            for (int i = 1; i < 256; ++i)
            {
                sum += i * histogram[i];
            }

            double sumB = 0;
            double wB = 0;
            double wF = 0;
            double mB;
            double mF;
            double max = 0.0;
            double between = 0.0;
            double threshold1 = 0.0;
            double threshold2 = 0.0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0)
                {
                    continue;
                }

                wF = total - wB;
                if (wF == 0)
                {
                    break;
                }

                sumB += i * histogram[i];
                mB = sumB / wB;
                mF = (sum - sumB) / wF;
                between = wB * wF * (mB - mF) * (mB - mF);
                if (between >= max)
                {
                    threshold1 = i;
                    if (between > max)
                    {
                        threshold2 = i;
                    }
                    max = between;
                }
            }

            return (float)((threshold1 + threshold2) / 2.0F);
        }

        public float Triangle(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null)
        {
            byte[] imageData = srcImage.GetByte();
            byte[] maskData = null;

            if (maskImage != null)
            {
                maskData = maskImage.GetByte();
            }

            //int pitch = 4 * ((int)(Math.Truncate(((float)srcImage.Width - (float)1) / (float)4)) + 1);

            int total = 0;
            float[] histogram = new float[256];

            int index = 0;

            for (int i = 0; i < imageData.Count(); i++)
            {
                if ((maskData == null) || (maskData != null && maskData[index] > 0))
                {
                    total++;
                    histogram[imageData[index]]++;
                }

                index++;
            }

            float thresholdValue = Triangle(histogram);

            index = 0;

            for (int i = 0; i < imageData.Count(); i++)
            {
                if ((maskData == null) || (maskData != null && maskData[index] > 0))
                {
                    if (imageData[index] >= thresholdValue)
                    {
                        imageData[index] = 255;
                    }
                    else
                    {
                        imageData[index] = 0;
                    }
                }
                else
                {
                    imageData[index] = 0;
                }

                index++;
            }
            if (destImage == null)
            {
                destImage.SetByte(imageData);
            }

            return thresholdValue;
        }

        private float Triangle(float[] histogram)
        {
            int min = 0, max = 0, min2 = 0;
            float dmax = 0;

            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] > 0)
                {
                    min = i;
                    break;
                }
            }
            if (min > 0)
            {
                min--; // line to the (p==0) point, not to data[min]
            }

            for (int i = 255; i > 0; i--)
            {
                if (histogram[i] > 0)
                {
                    min2 = i;
                    break;
                }
            }
            if (min2 < 255)
            {
                min2++; // line to the (p==0) point, not to data[min]
            }

            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] > dmax)
                {
                    max = i;
                    dmax = histogram[i];
                }
            }
            // find which is the furthest side
            //IJ.log(""+min+" "+max+" "+min2);
            bool inverted = false;
            if ((max - min) < (min2 - max))
            {
                // reverse the histogram
                //IJ.log("Reversing histogram.");
                inverted = true;
                int left = 0;          // index of leftmost element
                int right = 255; // index of rightmost element
                while (left < right)
                {
                    // exchange the left and right elements
                    float temp = histogram[left];
                    histogram[left] = histogram[right];
                    histogram[right] = temp;
                    // move the bounds toward the center
                    left++;
                    right--;
                }
                min = 255 - min2;
                max = 255 - max;
            }

            if (min == max)
            {
                //IJ.log("Triangle:  min == max.");
                return min;
            }

            // describe line by nx * x + ny * y - d = 0
            double nx, ny, d;
            // nx is just the max frequency as the other point has freq=0
            nx = histogram[max];   //-min; // data[min]; //  lowest value bmin = (p=0)% in the image
            ny = min - max;
            d = Math.Sqrt(nx * nx + ny * ny);
            nx /= d;
            ny /= d;
            d = nx * min + ny * histogram[min];

            // find split point
            int split = min;
            double splitDistance = 0;
            for (int i = min + 1; i <= max; i++)
            {
                double newDistance = nx * i + ny * histogram[i] - d;
                if (newDistance > splitDistance)
                {
                    split = i;
                    splitDistance = newDistance;
                }
            }
            split--;

            if (inverted)
            {
                // The histogram might be used for something else, so let's reverse it back
                int left = 0;
                int right = 255;
                while (left < right)
                {
                    float temp = histogram[left];
                    histogram[left] = histogram[right];
                    histogram[right] = temp;
                    left++;
                    right--;
                }
                return (255 - split);
            }
            else
            {
                return split;
            }
        }

        public void Sobel(AlgoImage srcImage, int size = 3)
        {
            Sobel(srcImage, srcImage, size);
        }

        public abstract void Sobel(AlgoImage srcImage, AlgoImage destImage, int size = 3);

        public abstract AlgoImage And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage);
        public abstract AlgoImage Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage);

        public abstract AlgoImage Not(AlgoImage algoImage, AlgoImage destImage);
        public abstract void Clear(AlgoImage algoImage, byte value);

        public void Clear(AlgoImage srcImage, Color value)
        {
            Clear(srcImage, Rectangle.Empty, value);
        }

        public abstract void Clear(AlgoImage srcImage, Rectangle rect, Color value);

        public abstract void Canny(AlgoImage srcImage, AlgoImage destImage, double threshold, double thresholdLinking);

        public abstract float[] Projection(AlgoImage greyImage, TwoWayDirection projectionDir, ProjectionType projectionType = ProjectionType.Sum);
        public abstract float[] Projection(AlgoImage greyImage, AlgoImage maskImage, TwoWayDirection projectionDir, ProjectionType projectionType = ProjectionType.Sum);

        public abstract void LogPolar(AlgoImage greyImage);

        public abstract void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY);

        public abstract BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam);
        public abstract BlobRectList Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam);
        public abstract void FilterBlob(BlobRectList blobRectList, BlobParam blobParam, bool invert = false);
        public abstract BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam);

        public abstract void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption);

        public abstract AlgoImage FillHoles(AlgoImage srcImage, AlgoImage destImage);

        public abstract double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams);

        public abstract void Average(AlgoImage srcImage, AlgoImage destImage = null);

        public abstract long[] Histogram(AlgoImage algoImage);
        public abstract void HistogramEqualization(AlgoImage algoImage);
        public abstract void HistogramStretch(AlgoImage algoImage);

        public abstract AlgoImage Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool abs = false);

        public abstract void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent);

        public abstract void Median(AlgoImage srcImage, AlgoImage destImage, int size);

        public abstract void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset);
        public abstract void Rotate(AlgoImage srcImage, AlgoImage destImage, PointF centerPt, float angle);

        //Draw Function
        public abstract void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled);
        public abstract void DrawText(AlgoImage srcImage, Point point, double value, string text);
        public abstract void DrawRotateRact(AlgoImage srcImage, RotatedRect rotateRect, double value, bool filled);
        public abstract void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled);

        public abstract void EraseBoder(AlgoImage srcImage, AlgoImage destImage);
        public abstract void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage);

        public abstract void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1);

        public abstract void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactor);
    }
}
