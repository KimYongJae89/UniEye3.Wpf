using DynMvp.Base;
using DynMvp.UI;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    internal class OpenCvImageProcessing : ImageProcessing
    {
        // Auto Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvGreyImage;
                var openCvDestImage = destImage as OpenCvGreyImage;

                CvInvoke.Threshold(openCvSrcImage.Image, openCvDestImage.Image, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        // Single Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvGreyImage;
                var openCvDestImage = destImage as OpenCvGreyImage;

                //            IntPtr ptrImage = openCvSrcImage.Image.Ptr;
                //            CvInvoke.cvThreshold(ptrImage, openCvDestImage.Image.Ptr, threshold, 255, THRESH.CV_THRESH_BINARY);

                openCvDestImage.Image = openCvSrcImage.Image.ThresholdBinary(new Gray(threshold), new Gray(255));

                //openCvSrcImage.Save(@"D:\aa.bmp", null);
                //openCvDestImage.Save(@"D:\bb.bmp", null);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        // Double Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvGreyImage;
                var openCvDestImage = destImage as OpenCvGreyImage;

                Image<Gray, byte> lowerThresholdImage = openCvSrcImage.Image.ThresholdBinary(new Gray(thresholdLower), new Gray(255));
                Image<Gray, byte> upperThresholdImage = openCvSrcImage.Image.ThresholdBinary(new Gray(thresholdUpper), new Gray(255));
                upperThresholdImage = upperThresholdImage.Not();

                openCvDestImage.Image = lowerThresholdImage.And(upperThresholdImage);
                if (inverse)
                {
                    openCvDestImage.Image = openCvDestImage.Image.Not();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void AdaptiveBinarize(AlgoImage algoImage1, AlgoImage algoImage2, int thresholdLower)
        {
            var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

            var ptrImage1 = (IInputArray)openCvGreyImage1.Image.GetInputArray();
            var ptrImage2 = (IOutputArray)openCvGreyImage2.Image.GetOutputArray();

            CvInvoke.AdaptiveThreshold(ptrImage1, ptrImage2, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 3, thresholdLower);

            //openCvGreyImage2.Image.Ptr = ptrImage2;
        }

        public override void CustomBinarize(AlgoImage algoImage1, AlgoImage algoImage2, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray)
        {
            try
            {
                if ((srcImage is OpenCvGreyImage src) && (destImage is OpenCvGreyImage dst))
                {
                    dst.Image = src.Image.Erode(numErode);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            try
            {
                if ((srcImage is OpenCvGreyImage src) && (destImage is OpenCvGreyImage dst))
                {
                    dst.Image = src.Image.Dilate(numDilate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            try
            {
                if ((srcImage is OpenCvGreyImage src) && (destImage is OpenCvGreyImage dst))
                {
                    dst.Image = src.Image.Erode(numDilate).Dilate(numDilate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)
        {
            try
            {
                if ((srcImage is OpenCvGreyImage src) && (destImage is OpenCvGreyImage dst))
                {
                    dst.Image = src.Image.Dilate(numClose).Erode(numClose);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
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
            int count = 0;
            try
            {
                if (algoImage is OpenCvGreyImage)
                {
                    var openCvGreyImage = algoImage as OpenCvGreyImage;

                    Image<Gray, byte> greyImage;

                    if (maskImage != null)
                    {
                        var openCvMaskImage = maskImage as OpenCvGreyImage;
                        greyImage = openCvGreyImage.Image.And(openCvMaskImage.Image);
                    }
                    else
                    {
                        greyImage = openCvGreyImage.Image;
                    }

                    count = greyImage.CountNonzero()[0];
#if DEBUG
                    string path = string.Format("{0}\\{1}.bmp", BaseConfig.Instance().TempPath, "OpenCV");
                    greyImage.Save(path);
#endif
                }
                else
                {
                    var openCvDepthImage = algoImage as OpenCvDepthImage;

                    Image<Gray, byte> greyImage = openCvDepthImage.Image.InRange(new Gray(120), new Gray(140));
                    count = greyImage.CountNonzero()[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Count2(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return count;
        }

        private double length(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public override Color GetColorAverage(AlgoImage algoImage, Rectangle rect)
        {
            try
            {
                var openCvColorImage = algoImage as OpenCvColorImage;
                openCvColorImage.Image.ROI = rect;
                Bgr bgrColor = openCvColorImage.Image.GetAverage();
                openCvColorImage.Image.ROI = Rectangle.Empty;

                return Color.FromArgb((int)bgrColor.Red, (int)bgrColor.Green, (int)bgrColor.Blue);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetColorAverage(AlgoImage algoImage, Rectangle rect)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString() + "OpenCvImageProcessing.GetColorAverage(AlgoImage algoImage, Rectangle rect)", ex.InnerException.Message);
            }

            return Color.FromArgb(0, 0, 0);
        }


        public override Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            try
            {
                var openCvColorImage = algoImage as OpenCvColorImage;
                var openCvMaskImage = maskImage as OpenCvGreyImage;

                Bgr bgrColor = openCvColorImage.Image.GetAverage(openCvMaskImage.Image);

                return Color.FromArgb((int)bgrColor.Red, (int)bgrColor.Green, (int)bgrColor.Blue);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return Color.FromArgb(0, 0, 0);
        }

        public override float GetGreyAverage(AlgoImage algoImage)
        {
            try
            {
                if (algoImage is OpenCvDepthImage)
                {
                    var openCvDepthImage = algoImage as OpenCvDepthImage;
                    return (float)openCvDepthImage.Image.GetAverage().Intensity;
                }
                else
                {
                    var openCvGreyImage = algoImage as OpenCvGreyImage;
                    return (float)openCvGreyImage.Image.GetAverage().Intensity;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetGreyAverage(AlgoImage algoImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString() + "OpenCvImageProcessing.GetGreyAverage(AlgoImage greyImage)", ex.InnerException.Message);
            }

            return 0;
        }

        public override float GetGreyAverage(AlgoImage greyImage, AlgoImage maskImage)
        {
            try
            {
                var openCvGreyImage = greyImage as OpenCvGreyImage;
                var openCvMaskImage = maskImage as OpenCvGreyImage;

                return (float)openCvGreyImage.Image.GetAverage(openCvMaskImage.Image).Intensity;
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetGreyAverage(AlgoImage greyImage, AlgoImage maskImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return 0;
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
            try
            {
                var openCvGreyImage = algoImage as OpenCvGreyImage;

                byte[,,] imageData = openCvGreyImage.Image.Data;
                int width = openCvGreyImage.Image.Width;
                int height = openCvGreyImage.Image.Height;
                int size = width * height;

                float sumValue = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        sumValue += imageData[y, x, 0];
                    }
                }


                float avgValue = sumValue / size;

                double doubleSumValue = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        doubleSumValue += Math.Pow(avgValue - imageData[y, x, 0], 2);
                    }
                }

                doubleSumValue /= size;

                return (float)Math.Sqrt(doubleSumValue);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetStdDev(AlgoImage algoImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return 0;
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            try
            {
                var openCvGreyImage = algoImage as OpenCvGreyImage;
                var openCvMaskImage = maskImage as OpenCvGreyImage;

                byte[,,] imageData = openCvGreyImage.Image.Data;
                byte[,,] maskImageData = openCvMaskImage.Image.Data;
                int width = openCvGreyImage.Image.Width;
                int height = openCvGreyImage.Image.Height;
                int size = width * height;

                float sumValue = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maskImageData[y, x, 0] > 0)
                        {
                            sumValue += imageData[y, x, 0];
                        }
                    }
                }


                float avgValue = sumValue / size;

                double doubleSumValue = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maskImageData[y, x, 0] > 0)
                        {
                            doubleSumValue += Math.Pow(avgValue - imageData[y, x, 0], 2);
                        }
                    }
                }

                doubleSumValue /= size;

                return (float)Math.Sqrt(doubleSumValue);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.GetStdDev(AlgoImage algoImage, AlgoImage maskImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return 0;
        }

        public override float[] Projection(AlgoImage algoImage, AlgoImage maskImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage algoImage, TwoWayDirection projectionDir, ProjectionType projectionType)
        {
            try
            {
                if (!(algoImage is OpenCvGreyImage openCvGreyImage))
                {
                    return null;
                }

                int width = openCvGreyImage.Image.Width;
                int height = openCvGreyImage.Image.Height;
                int nbEntries = (projectionDir == TwoWayDirection.Horizontal) ? width : height;

                float[] projection = new float[nbEntries];
                var reduceImage = new Mat();
                CvInvoke.Reduce(openCvGreyImage.Image.Mat, reduceImage, (ReduceDimension)projectionDir, (ReduceType)projectionType, DepthType.Cv32F);
                Marshal.Copy(reduceImage.DataPointer, projection, 0, projection.Length);

                return projection;
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Projection(AlgoImage greyImage, AlgoImage maskImage, ProjectionDir projectionDir)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
                return null;
            }
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, int size = 3)
        {
            try
            {
                var cvSrcImage = srcImage as OpenCvGreyImage;
                var cvDestImage = destImage as OpenCvGreyImage;

                Image<Gray, float> sobelXImage = cvSrcImage.Image.Sobel(1, 0, 3);
                Image<Gray, float> sobelYImage = cvSrcImage.Image.Sobel(0, 1, 3);
                Image<Gray, float> sobelXImage2 = sobelXImage.Mul(sobelXImage);
                Image<Gray, float> sobelYImage2 = sobelYImage.Mul(sobelYImage);
                Image<Gray, float> sobelSum = sobelXImage2.Add(sobelYImage2);
                Image<Gray, float> sobel = sobelXImage2.Clone();

                CvInvoke.Sqrt(sobelSum, sobel);

                sobel.Save(string.Format("{0}\\Sobel.bmp", BaseConfig.Instance().TempPath));

                cvDestImage.Image = sobel.ConvertScale<byte>(1, 0);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Sobel(AlgoImage algoImage, int size)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override AlgoImage And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            try
            {
                var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
                var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;
                if (!(destImage is OpenCvGreyImage openCvGreyImageDst))
                {
                    destImage = openCvGreyImageDst = new OpenCvGreyImage();
                    openCvGreyImageDst.Image = new Image<Gray, byte>(openCvGreyImage1.Width, openCvGreyImage1.Height);
                }

                openCvGreyImageDst.Image = openCvGreyImage1.Image.And(openCvGreyImage2.Image);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return destImage;
        }

        public override AlgoImage Not(AlgoImage algoImage, AlgoImage destImage)
        {
            try
            {
                var openCvGreyImage1 = algoImage as OpenCvGreyImage;

                if (!(destImage is OpenCvGreyImage openCvGreyImage2))
                {
                    destImage = openCvGreyImage2 = new OpenCvGreyImage();
                    openCvGreyImage2.Image = new Image<Gray, byte>(openCvGreyImage1.Width, openCvGreyImage1.Height);
                }

                openCvGreyImage2.Image = openCvGreyImage1.Image.Not();
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Not(AlgoImage algoImage, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return destImage;
        }

        public double MatchShape(AlgoImage algoImage1, AlgoImage algoImage2)
        {
            try
            {
                var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
                var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

                return CvInvoke.MatchShapes(openCvGreyImage1.Image, openCvGreyImage2.Image, ContoursMatchType.I3, 0);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.MatchShape(AlgoImage algoImage1, AlgoImage algoImage2)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return 0;
        }

        public void AdaptiveThreshold(AlgoImage algoImage1, AlgoImage algoImage2, double param1)
        {
            try
            {
                var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
                var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

                CvInvoke.AdaptiveThreshold(openCvGreyImage1.Image, openCvGreyImage2.Image, 255,
                    AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, param1);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.AdaptiveThreshold(AlgoImage algoImage1, AlgoImage algoImage2, double param1)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            try
            {
                var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
                var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

                //            openCvGreyImage2.Image = openCvGreyImage1.Image.GoodFeaturesToTrack(Canny(dblParams[0], dblParams[1]);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return 0;
        }

        public override void Canny(AlgoImage srcImage, AlgoImage dstImage, double threshold, double thresholdLinking)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvGreyImage;
                var openCvDstImage = dstImage as OpenCvGreyImage;
                openCvDstImage.Image = openCvSrcImage.Image.Canny(threshold, thresholdLinking);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Canny(AlgoImage greyImage, double threshold, double thresholdLinking)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            try
            {
                var openCvGreyImage = greyImage as OpenCvGreyImage;

                var centerPt = new PointF(openCvGreyImage.Image.Width / 2, openCvGreyImage.Image.Height / 2);

                openCvGreyImage.Image = openCvGreyImage.Image.LogPolar(centerPt, 15, Inter.Linear, Warp.FillOutliers);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.LogPolar(AlgoImage greyImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam)
        {
            var blobRectList = new BlobRectList();

            try
            {
                var openCvGreyImage = algoImage as OpenCvGreyImage;

                var blobs = new CvBlobs();
                var blobDetector = new CvBlobDetector();
                blobDetector.Detect(openCvGreyImage.Image, blobs);

                foreach (CvBlob blob in blobs.Values.ToList())
                {
                    var blobRect = new BlobRect
                    {
                        Area = blob.Area,
                        BoundingRect = blob.BoundingBox,
                        CenterPt = blob.Centroid
                    };
                    blobRectList.AddBlobRect(blobRect);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Blob(AlgoImage algoImage, BlobParam blobParam)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return blobRectList;
        }

        public override BlobRectList Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam)
        {
            var blobRectList = new BlobRectList();
            try
            {
                var openCvGreyImage = algoImage as OpenCvGreyImage;
                var greyMaskImage = greyMask as OpenCvGreyImage;

                openCvGreyImage.Image.Or(greyMaskImage.Image);
                var blobs = new CvBlobs();
                var blobDetector = new CvBlobDetector();
                blobDetector.Detect(openCvGreyImage.Image, blobs);

                foreach (CvBlob blob in blobs.Values.ToList())
                {
                    var blobRect = new BlobRect
                    {
                        Area = blob.Area,
                        BoundingRect = blob.BoundingBox,
                        CenterPt = blob.Centroid
                    };
                    blobRectList.AddBlobRect(blobRect);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"OpenCvmageProcessing.Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam){ex.InnerException.Message}");
                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
            return blobRectList;
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
        {
            try
            {
                CvInvoke.Rectangle(((OpenCvGreyImage)algoImage).Image, Rectangle.Round(blobRect.BoundingRect), new MCvScalar(127), 1, LineType.FourConnected, 0);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            avgX = new float[0];
            avgY = new float[0];
            stdDevX = new float[0];
            stdDevY = new float[0];

            try
            {
                var openCvGreyImage = greyImage as OpenCvGreyImage;
                var openCvMaskImage = maskImage as OpenCvGreyImage;

                byte[,,] imageData = openCvGreyImage.Image.Data;
                byte[,,] maskData = null;
                if (maskImage != null)
                {
                    maskData = openCvMaskImage.Image.Data;
                }

                int width = openCvGreyImage.Image.Width;
                int height = openCvGreyImage.Image.Height;

                avgX = new float[width];
                avgY = new float[height];
                stdDevX = new float[width];
                stdDevY = new float[height];

                float[] squareSumX = new float[width];
                float[] squareSumY = new float[height];
                float[] sumX = new float[width];
                float[] sumY = new float[height];

                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        if (maskData == null || maskData[y, x, 0] > 0)
                        {
                            squareSumX[x] += imageData[y, x, 0] * imageData[y, x, 0];
                            squareSumY[y] += imageData[y, x, 0] * imageData[y, x, 0];
                            sumX[x] += imageData[y, x, 0];
                            sumY[y] += imageData[y, x, 0];
                        }
                    }
                }

                for (int x = 0; x < width; x++)
                {
                    avgX[x] = sumX[x] / height;
                    stdDevX[x] = (float)Math.Sqrt(squareSumX[x] / height - avgX[x] * avgX[x]);
                }

                for (int y = 0; y < height; y++)
                {
                    avgY[y] = sumY[y] / width;
                    stdDevY[y] = (float)Math.Sqrt(squareSumY[y] / width - avgY[y] * avgY[y]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage = null)
        {
            try
            {
                if (srcImage is OpenCvGreyImage)
                {
                    var openCvGreyImage = srcImage as OpenCvGreyImage;

                    if (destImage != null)
                    {
                        ((OpenCvGreyImage)destImage).Image = openCvGreyImage.Image.SmoothBlur(3, 3);
                    }
                    else
                    {
                        openCvGreyImage.Image = openCvGreyImage.Image.SmoothBlur(3, 3);
                    }
                }
                else
                {
                    var openCvDepthImage = srcImage as OpenCvDepthImage;

                    if (destImage != null)
                    {
                        ((OpenCvDepthImage)destImage).Image = openCvDepthImage.Image.SmoothBlur(3, 3);
                    }
                    else
                    {
                        openCvDepthImage.Image = openCvDepthImage.Image.SmoothBlur(7, 7);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Average(AlgoImage srcImage, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool abs = false)
        {
            try
            {
                System.Diagnostics.Debug.Assert((image1.Width == image2.Width) && (image1.Height == image2.Height));

                var openCvImage1 = image1 as OpenCvGreyImage;
                var openCvImage2 = image2 as OpenCvGreyImage;

                if (!(destImage is OpenCvGreyImage openCvImageDst))
                {
                    destImage = openCvImageDst = new OpenCvGreyImage();
                    openCvImageDst.Image = new Image<Gray, byte>(openCvImage1.Width, openCvImage1.Height);
                }

                if ((image1.Width != image2.Width) || (image1.Height != image2.Height))
                {
                    return openCvImageDst;
                }

                openCvImageDst.Image = openCvImage1.Image.Sub(openCvImage2.Image);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return destImage;
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            try
            {
                if (srcImage is OpenCvGreyImage)
                {
                    var openCvGreyImage = srcImage as OpenCvGreyImage;

                    if (destImage != null)
                    {
                        ((OpenCvGreyImage)destImage).Image = openCvGreyImage.Image.SmoothMedian(size);
                    }
                    else
                    {
                        openCvGreyImage.Image = openCvGreyImage.Image.SmoothMedian(size);
                    }
                }
                else
                {
                    var openCvDepthImage = srcImage as OpenCvDepthImage;

                    if (destImage != null)
                    {
                        ((OpenCvDepthImage)destImage).Image = openCvDepthImage.Image.SmoothMedian(size);
                    }
                    else
                    {
                        openCvDepthImage.Image = openCvDepthImage.Image.SmoothMedian(5);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Median(AlgoImage srcImage, AlgoImage destImage, int size)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public void Median33F(AlgoImage srcImage, AlgoImage destImage)
        {
            try
            {
                int W = srcImage.Width;
                int H = srcImage.Height;
                int i, j, m, n;

                var openCvSrcDepthImage = srcImage as OpenCvDepthImage;
                var openCvDestDepthImage = destImage as OpenCvDepthImage;

                float[,,] srcArray = openCvSrcDepthImage.Image.Data;
                float[,,] destArray = openCvDestDepthImage.Image.Data;

                float[] t = new float[9];
                float ftemp;

                int j0, j1, j2;

                for (j = 1; j < H - 1; j++)
                {
                    for (i = 1; i < W - 1; i++)
                    {
                        j0 = W * (j - 1);
                        j1 = W * (j + 0);
                        j2 = W * (j + 1);

                        if (srcArray[j1, i, 0] == 0)
                        {
                            continue;
                        }

                        t[0] = srcArray[j0, i - 1, 0]; t[1] = srcArray[j0, i, 0]; t[2] = srcArray[j0, i + 1, 0];
                        t[3] = srcArray[j1, i - 1, 0]; t[4] = srcArray[j1, i, 0]; t[5] = srcArray[j1, i + 1, 0];
                        t[6] = srcArray[j2, i - 1, 0]; t[7] = srcArray[j2, i, 0]; t[8] = srcArray[j2, i + 1, 0];

                        if (srcArray[j0, i - 1, 0] == 0)
                        {
                            t[0] = t[4];
                        }

                        if (srcArray[j0, i + 0, 0] == 0)
                        {
                            t[1] = t[4];
                        }

                        if (srcArray[j0, i + 1, 0] == 0)
                        {
                            t[2] = t[4];
                        }

                        if (srcArray[j1, i - 1, 0] == 0)
                        {
                            t[3] = t[4];
                        }

                        if (srcArray[j1, i + 1, 0] == 0)
                        {
                            t[5] = t[4];
                        }

                        if (srcArray[j2, i - 1, 0] == 0)
                        {
                            t[6] = t[4];
                        }

                        if (srcArray[j2, i + 0, 0] == 0)
                        {
                            t[7] = t[4];
                        }

                        if (srcArray[j2, i + 1, 0] == 0)
                        {
                            t[8] = t[4];
                        }

                        for (n = 8; n >= 4; n--)
                        {
                            for (m = 0; m < n; m++)
                            {
                                if (t[m] > t[m + 1])
                                {
                                    ftemp = t[m];
                                    t[m] = t[m + 1];
                                    t[m + 1] = ftemp;
                                }
                            }
                        }
                        if (srcArray[j1, i, 0] != 0)
                        {
                            destArray[j1, i, 0] = t[4];
                        }
                    }
                }

                openCvDestDepthImage.Image.Data = destArray;
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Median33F(AlgoImage srcImage, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void Clear(AlgoImage srcImage, byte value)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvGreyImage;
                openCvSrcImage.Image.SetValue(value);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Clear(AlgoImage srcImage, byte value)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            try
            {
                var openCvSrcImage = srcImage as OpenCvColorImage;
                openCvSrcImage.Image.ROI = rect;
                openCvSrcImage.Image.SetValue(new MCvScalar(value.B, value.G, value.R));
                openCvSrcImage.Image.ROI = Rectangle.Empty;
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Clear(AlgoImage srcImage, Rectangle rect, Color value)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
        }

        public override AlgoImage Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            try
            {
                var openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
                var openCvGreyImage2 = algoImage2 as OpenCvGreyImage;
                if (!(destImage is OpenCvGreyImage openCvGreyImageDst))
                {
                    destImage = openCvGreyImageDst = new OpenCvGreyImage();
                }

                openCvGreyImageDst.Image = openCvGreyImage1.Image.Or(openCvGreyImage2.Image);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvImageProcessing.Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return destImage;
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

        public override void DrawRotateRact(AlgoImage srcImage, DynMvp.UI.RotatedRect rotateRect, double value, bool filled)
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
            MCvScalar[] scalar = new MCvScalar[2];
            Point[] point = new Point[2];
            OpenCvGreyImage openCvAlgoImage = algoImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;
            StatResult statResult = new StatResult();

            CvInvoke.MeanStdDev(openCvAlgoImage.Image, ref scalar[0], ref scalar[1], openCvMaskImage.Image);
            statResult.average =  (float)scalar[0].V0;
            statResult.stdDev = (float)scalar[1].V0;
            double fmin = 0;
            double fmax = 0;

            CvInvoke.MinMaxLoc(openCvAlgoImage.Image, ref fmin, ref fmax, ref point[0], ref point[1], openCvMaskImage.Image);

            statResult.min = (float)fmin;
            statResult.max = (float)fmax;

            statResult.squareSum = 0; // (float)CvInvoke.Sum(openCvImage.GetOutputArray()).V0;
            statResult.count = openCvAlgoImage.Width * openCvAlgoImage.Height;

            return statResult;
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            const int HIST_NUM_INTENSITIES = 256;
            //MIL_INT[]
            var openCvImage = (OpenCvGreyImage)algoImage;
            //OpenCvImage openCvImage = (OpenCvImage)algoImage;
            //if (openCvImage.IsCudaImage)
            //{
            //    OpenCvCudaImage openCvCudaImage = (OpenCvCudaImage)openCvImage;
            //    openCvCudaImage.UpdateHostImage();

            //    OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
            //    openCvGreyImage.Image = openCvCudaImage.Image;
            //    long[] d = this.Histogram(openCvGreyImage);
            //    openCvGreyImage.Dispose();

            //    return d;
            //    //GpuMat hist = new GpuMat(1, HIST_NUM_INTENSITIES, DepthType.Cv32S, 1);
            //    //CudaInvoke.HistEven(openCvImage.InputArray, hist, HIST_NUM_INTENSITIES, 0, 255);

            //    //Mat hist2 = new Mat(1, HIST_NUM_INTENSITIES, DepthType.Cv32S, 1);
            //    //hist.Download(hist2);
            //    //long[] a = hist2.Data.Cast<long>().ToArray();
            //    //return a;
            //}
            //else
            {
                //Mat hist = new Mat();
                //VectorOfMat vectorOfMat = new VectorOfMat();
                //vectorOfMat.Push(openCvImage.InputArray);

                //CvInvoke.CalcHist(vectorOfMat, new int[] { 0 }, null, hist, new int[] { HIST_NUM_INTENSITIES }, new float[] { 0, 255 }, false);
                var histo = new DenseHistogram(256, new RangeF(0, 256));

                if (algoImage is OpenCvGreyImage)
                {
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);
                }
                else if (algoImage is OpenCvColorImage)
                {
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);
                }
                else if (algoImage is OpenCvDepthImage)
                {
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);
                }

                float[] h = histo.GetBinValues();
                long[] l = Array.ConvertAll<float, long>(h, f => (long)f);
                return l;
            }
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
