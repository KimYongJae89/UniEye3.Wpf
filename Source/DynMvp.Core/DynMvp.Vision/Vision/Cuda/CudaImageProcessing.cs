using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision.Cuda
{
    public class CudaImageProcessing : ImageProcessing
    {
        public static void CheckCudaImage(AlgoImage algoImage, string functionName, string variableName)
        {
            if (algoImage == null)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image is null", functionName, variableName));
            }

            try
            {
                var image = algoImage as CudaImage;
                if (image.ImageID == 0)
                {
                    throw new InvalidTargetException(string.Format("[{0}] {1} Image Object is null", functionName, variableName));
                }
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image must be gray image", functionName, variableName));
            }
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
        {
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "srcImage");
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "destImage");

            var cudaSrcImage = srcImage as CudaImage;
            var cudaDstImage = destImage as CudaImage;

            CudaMethods.CUDA_ADAPTIVE_BINARIZE_LOWER(cudaSrcImage.ImageID, cudaDstImage.ImageID, thresholdLower);
        }

        public override AlgoImage And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage = null)
        {
            throw new NotImplementedException();
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "srcImage");
            CheckCudaImage(destImage, "CudaImageProcessing.AdaptiveBinarize", "destImage");

            var cudaSrcImage = srcImage as CudaImage;
            var cudaDstImage = destImage as CudaImage;

            CudaMethods.CUDA_BINARIZE(cudaSrcImage.ImageID, cudaDstImage.ImageID, threshold, 255, inverse);
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "srcImage");
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "destImage");

            var cudaSrcImage = srcImage as CudaImage;
            var cudaDstImage = destImage as CudaImage;

            CudaMethods.CUDA_BINARIZE(cudaSrcImage.ImageID, cudaDstImage.ImageID, thresholdLower, thresholdUpper, inverse);
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam)
        {
            throw new NotImplementedException();
            //CheckCudaImage(algoImage, "CudaImageProcessing.AdaptiveBinarize", "algoImage");

            //CudaImage cudaBinImage = algoImage as CudaImage;
            //CudaImage cudaLabelImage = new CudaDepthImage<int>();
            //CudaImage cudaIndexImage = new CudaDepthImage<int>();
            //CudaImage cudaMaskImage = new CudaDepthImage<byte>();

            //cudaLabelImage.Alloc(algoImage.Width, algoImage.Height);
            //cudaIndexImage.Alloc(algoImage.Width, algoImage.Height);
            //cudaMaskImage.Alloc(algoImage.Width, algoImage.Height);

            ////int[] labelImage = new int[cudaSrcImage.Width * cudaSrcImage.Height];

            //CudaMethods.CUDA_LABELING(cudaBinImage.ImageID, cudaLabelImage.ImageID, cudaIndexImage.ImageID, cudaMaskImage.ImageID);

            //var array = cudaLabelImage.CloneData();

            //var labelDictionary = new Dictionary<int, BlobRect>();

            //int width = cudaBinImage.Width;

            //int[] dataArray = array as int[];

            //BlobRectList blobRectList = new BlobRectList();

            ////var query = dataArray.Select((obj, index) => new { obj = obj, index = index }).GroupBy(o => o.obj, o => o.index);
            ////foreach (var group in query)
            ////{
            ////    Console.WriteLine("Key={0} (Count={1})", group.Key, group.Count());
            ////    foreach (var index in group)
            ////    {

            ////    }
            ////}

            ////var labelMap = dataArray
            ////    .Select((n, i) => new { Value = n, Index = i })
            ////    .GroupBy(a => a.Value)
            ////    .ToDictionary(
            ////    g => g.Key,
            ////    g => g.Select(a => a.Index)
            ////    );

            ////blobRectList.Clear();

            //if (dataArray != null)
            //{
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        if (dataArray[i] != 0)
            //        {
            //            int rectX = i % width;
            //            int rectY = i / width;

            //            if (labelDictionary.ContainsKey(dataArray[i]))
            //            {
            //                BlobRect blobRect = labelDictionary[dataArray[i]];
            //                blobRect.Area++;
            //                blobRect.BoundingRect = RectangleF.Union(blobRect.BoundingRect, RectangleF.FromLTRB(rectX, rectY, rectX, rectY));
            //            }
            //            else
            //            {
            //                BlobRect newBlobRect = new BlobRect();
            //                newBlobRect.Area = 1;
            //                newBlobRect.BoundingRect = RectangleF.FromLTRB(rectX, rectY, rectX, rectY);
            //                labelDictionary.Add(dataArray[i], newBlobRect);
            //                blobRectList.AddBlobRect(newBlobRect);

            //            }
            //        }
            //    }
            //}

            //cudaLabelImage.Dispose();
            //cudaIndexImage.Dispose();

            //return blobRectList;//milProcessing.Blob(milImage, blobParam);
            //returnn
        }

        public override BlobRectList Blob(AlgoImage algoImage, AlgoImage greyMask, BlobParam blobParam)
        {
            CheckCudaImage(algoImage, "CudaImageProcessing.AdaptiveBinarize", "algoImage");

            var cudaBinImage = algoImage as CudaImage;
            var cudaGreyImage = greyMask as CudaImage;

            int count = CudaMethods.CUDA_LABELING(cudaBinImage.ImageID);

            uint[] areaArray = new uint[count];
            uint[] xMinArray = new uint[count];
            uint[] xMaxArray = new uint[count];
            uint[] yMinArray = new uint[count];
            uint[] yMaxArray = new uint[count];
            uint[] vMinArray = new uint[count];
            uint[] vMaxArray = new uint[count];
            float[] vMeanArray = new float[count];

            CudaMethods.CUDA_BLOBING(cudaBinImage.ImageID, cudaGreyImage.ImageID, count,
                areaArray, xMinArray, xMaxArray, yMinArray, yMaxArray,
                vMinArray, vMaxArray, vMeanArray);

            var blobRectList = new BlobRectList();

            for (int i = 0; i < count; i++)
            {
                var blobRect = new BlobRect();
                blobRect.BoundingRect = new RectangleF(xMinArray[i], yMinArray[i], xMaxArray[i] - xMinArray[i] + 1, yMaxArray[i] - yMinArray[i] + 1);

                blobRect.Area = areaArray[i];
                blobRect.MinValue = vMinArray[i];
                blobRect.MaxValue = vMaxArray[i];
                blobRect.MeanValue = vMeanArray[i];

                blobRectList.AddBlobRect(blobRect);
            }

            return blobRectList;//milProcessing.Blob(milImage, blobParam);
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Canny(AlgoImage srcImage, AlgoImage destImage, double lowThreshold, double highThreshold)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            //CudaMethods.CUDA_EDGE_FINDER(srcCudaImage.ImageID, dstCudaImage.ImageID, (int)lowThreshold, (int)highThreshold);
            CudaMethods.CUDA_CANNY(srcCudaImage.ImageID, dstCudaImage.ImageID, (int)lowThreshold, (int)highThreshold);
        }

        public override void Clear(AlgoImage algoImage, byte value)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            throw new NotImplementedException();
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_MORPHOLOGY_CLOSE(srcCudaImage.ImageID, dstCudaImage.ImageID, numClose);
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override int Count(AlgoImage algoImage, AlgoImage maskImage = null)
        {
            throw new NotImplementedException();
        }

        public override void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray = false)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_MORPHOLOGY_DILATE(srcCudaImage.ImageID, dstCudaImage.ImageID, numDilate);
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numDilate, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
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

        public override void DrawText(AlgoImage srcImage, Point point, double value, string text)
        {
            throw new NotImplementedException();
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            throw new NotImplementedException();
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray = false)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_MORPHOLOGY_ERODE(srcCudaImage.ImageID, dstCudaImage.ImageID, numErode);
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int[,] structureXY, int numErode, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void FilterBlob(BlobRectList blobRectList, BlobParam blobParam, bool invert = false)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
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

        public override long[] Histogram(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_MEAN_FILTER(srcCudaImage.ImageID, dstCudaImage.ImageID, size);
        }

        public override AlgoImage Not(AlgoImage algoImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_MORPHOLOGY_OPEN(srcCudaImage.ImageID, dstCudaImage.ImageID, numOpen);
        }

        public override AlgoImage Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, TwoWayDirection projectionDir, ProjectionType projectionType = ProjectionType.Sum)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, AlgoImage maskImage, TwoWayDirection projectionDir, ProjectionType projectionType = ProjectionType.Sum)
        {
            throw new NotImplementedException();
        }

        public override void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage)
        {
            throw new NotImplementedException();
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactor)
        {
            var srcCudaImage = srcImage as CudaImage;
            var dstCudaImage = destImage as CudaImage;

            CudaMethods.CUDA_RESIZE(srcCudaImage.ImageID, dstCudaImage.ImageID, scaleFactor, (int)CudaMethods.InterpolationMode.UNDEFINED);
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, PointF centerPt, float angle)
        {
            throw new NotImplementedException();
        }

        public override void Smooth(AlgoImage srcImage, AlgoImage destImage, int numSmooth)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, int size = 3)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool abs = false)
        {
            throw new NotImplementedException();
        }

        public override void Thick(AlgoImage srcImage, AlgoImage destImage, int numThick, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            throw new NotImplementedException();
        }
    }
}
