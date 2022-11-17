using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using UniScanC.Enums;

namespace UniScanC.Algorithm.Simple
{
    public class HorizentalEdgeFinder
    {
        public static (int, int) Find(float[] data, ECamPosition camPosition, int threshold)
        {
            int leftEdge = 0;
            int rightEdge = data.Length;

            Image<Gray, float> profileImg = null;
            Image<Gray, float> edgeImg = null;
            var binImg = new Image<Gray, byte>(data.Length, 1);

            try
            {
                if (camPosition == ECamPosition.Mid)
                {
                    return (leftEdge, rightEdge);
                }

                profileImg = new Image<Gray, float>(data.Length, 1);
                Buffer.BlockCopy(data, 0, profileImg.ManagedArray, 0, data.Length * sizeof(float));

                profileImg = profileImg.SmoothBlur(11, 1);
                CvInvoke.MedianBlur(profileImg, profileImg, 5);
                edgeImg = profileImg.Sobel(1, 0, 3);
                edgeImg = edgeImg.SmoothBlur(3, 1);

                // 절대값 처리
                for (int i = 0; i < edgeImg.Width; ++i)
                {
                    edgeImg.Data[0, i, 0] = Math.Abs(edgeImg.Data[0, i, 0]);
                }
                CvInvoke.Normalize(edgeImg, edgeImg, 0, 255, NormType.MinMax);
                // Byte화
                for (int i = 0; i < binImg.Width; ++i)
                {
                    binImg.Data[0, i, 0] = (byte)Math.Round(edgeImg.Data[0, i, 0]);
                }
                // 이진화
                CvInvoke.Threshold(binImg, binImg, threshold, 255, ThresholdType.Binary);

                if (camPosition == ECamPosition.OneCam || camPosition == ECamPosition.Left)
                {
                    leftEdge = GetLeftEdge(binImg);
                }

                if (camPosition == ECamPosition.OneCam || camPosition == ECamPosition.Right)
                {
                    rightEdge = GetRightEdge(binImg);
                }

                return (leftEdge, rightEdge);
            }
            catch (Exception)
            {
                return (0, data.Length);
            }
            finally
            {
                binImg?.Dispose();
                edgeImg?.Dispose();
                profileImg?.Dispose();
            }
        }

        public static int GetLeftEdge(Image<Gray, byte> thresholdImg)
        {
            int leftStartIndex = 0;
            int leftEndIndex = 0;
            for (int i = 1; i < thresholdImg.Width; i++)
            {
                if (leftStartIndex == 0 && thresholdImg.Data[0, 0, 0] != thresholdImg.Data[0, i, 0])
                {
                    leftStartIndex = i;
                }
                else if (leftStartIndex != 0 && thresholdImg.Data[0, 0, 0] == thresholdImg.Data[0, i, 0])
                {
                    leftEndIndex = i;
                    break;
                }
            }
            return (leftStartIndex + leftEndIndex) / 2;
        }

        public static int GetRightEdge(Image<Gray, byte> thresholdImg)
        {
            int rightStartIndex = 0;
            int rightEndIndex = 0;
            for (int i = thresholdImg.Width - 2; i > 0; i--)
            {
                if (rightStartIndex == 0 && thresholdImg.Data[0, thresholdImg.Width - 1, 0] != thresholdImg.Data[0, i, 0])
                {
                    rightStartIndex = i;
                }
                else if (rightStartIndex != 0 && thresholdImg.Data[0, thresholdImg.Width - 1, 0] == thresholdImg.Data[0, i, 0])
                {
                    rightEndIndex = i;
                    break;
                }
            }
            return (rightStartIndex + rightEndIndex) / 2;
        }
    }
}

// 기존 변수
//double min = 0;
//double max = 0;
//var minPnt = new Point();
//var maxPnt = new Point();

// 미분 영상의 극성(+/-)으로 좌/우 엣지 판단됨 (기존 방법)
//CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);
//leftEdge = Math.Min(maxPnt.X, minPnt.X);
//rightEdge = Math.Max(maxPnt.X, minPnt.X);

// 기존 방법 (좌우 엣지 찾는 방식)
//int avg = (int)edgeImg.GetAverage().Intensity;
//edgeImg.ROI = new Rectangle(0, 0, data.Length, 1);

//CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);

//if (camPosition == ECamPosition.Left)
//{
//    leftEdge = (max > avg * 2.0) ? maxPnt.X : 0;
//}

//if (camPosition == ECamPosition.Right)
//{
//    rightEdge = (max > avg * 2.0) ? maxPnt.X : data.Length;
//}
