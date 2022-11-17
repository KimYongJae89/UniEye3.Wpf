using DynMvp.Base;
using DynMvp.UI;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvEdgeDetector : EdgeDetector
    {
        private static object lockThis = new object();
        public override AlgoImage GetEdgeImage(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            //float otsuValue = imageProcessing.Otsu(algoImage);

            var binImage = (OpenCvGreyImage)algoImage.Clone();

            imageProcessing.Binarize(algoImage, binImage, Param.Threshold);
            //imageProcessing.Binarize(algoImage, binImage, (int)otsuValue);

            imageProcessing.Erode(binImage, Param.MorphologyFilterSize);
            imageProcessing.Dilate(binImage, Param.MorphologyFilterSize * 2);
            imageProcessing.Erode(binImage, Param.MorphologyFilterSize);

            return binImage;
        }

        public int GetEdgePos(OpenCvGreyImage edgeImage, bool IsRissing)
        {
            var matrix = new Matrix<int>(edgeImage.Height, 1);
            edgeImage.Image.Reduce(matrix, ReduceDimension.SingleCol, ReduceType.ReduceAvg);

            var xList = new List<int>();
            var yList = new List<int>();

            //if (matrix[edgeImage.Height - 1, 0] < 127)
            //{
            //    return -1;
            //}

            if (IsRissing)
            {
                int i = 0;

                for (; i <= edgeImage.Height - 1; i++)
                {
                    if (matrix[i, 0] < 127)
                    {
                        break;
                    }
                }

                if (i < edgeImage.Height - 1)
                {
                    for (; i <= edgeImage.Height - 1; i++)
                    {
                        if (matrix[i, 0] > 127)
                        {
                            return i;
                        }
                    }
                }
            }
            else
            {
                int i = edgeImage.Height - 1;

                for (; i >= 0; i--)
                {
                    if (matrix[i, 0] > 127)
                    {
                        break;
                    }
                }

                if (i > 0)
                {
                    for (; i >= 0; i--)
                    {
                        if (matrix[i, 0] < 127)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }

        public List<Matrix<double>> Allsac(List<Matrix<double>> points, double distantThreshold)
        {
            if (points.Count < 2)
            {
                return points;
            }

            var lines = new List<Tuple<Matrix<double>, int, double>>(); // c = 1.0
            var cVec = new Matrix<double>(2, 1);
            cVec.Data[0, 0] = -1.0;
            cVec.Data[1, 0] = -1.0;

            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    Matrix<double> matA = points[i].ConcateVertical(points[j]);
                    CvInvoke.Invert(matA, matA, DecompMethod.LU);
                    Matrix<double> lineAB = matA * cVec;

                    // 평가 및 투표
                    double lineNorm = lineAB.Norm;
                    int vote = 0;
                    double distantSum = 0;
                    for (int iN = 0; iN < points.Count; ++iN)
                    {
                        if (iN == i || iN == j)
                        {
                            continue;
                        }

                        double distant = Math.Abs((points[iN] * lineAB).Data[0, 0] + 1.0) / lineNorm;
                        if (distant < distantThreshold)
                        {
                            vote++;
                        }

                        distantSum += distant;
                    }

                    // 라인 추가
                    lines.Add(new Tuple<Matrix<double>, int, double>(lineAB, vote, distantSum));
                }
            }

            // 베스트 라인 찾기
            Matrix<double> bestLine = null;
            int bestVote = -1;
            for (int i = 0; i < lines.Count; ++i)
            {
                if (lines[i].Item2 > bestVote)
                {
                    bestLine = lines[i].Item1;
                    bestVote = lines[i].Item2;
                }
            }

            // 베스트 라인 기준 점 필터링
            var pointsFiltered = new List<Matrix<double>>();
            double bestLineNorm = bestLine.Norm;
            for (int iN = 0; iN < points.Count; ++iN)
            {
                double distant = Math.Abs((points[iN] * bestLine).Data[0, 0] + 1.0) / bestLineNorm;
                if (distant < distantThreshold)
                {
                    pointsFiltered.Add(points[iN]);
                }
            }

            return pointsFiltered;
        }

        public override EdgeDetectionResult Detect(AlgoImage algoImage, DynMvp.UI.RotatedRect rotatedRect, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            var openCvImageProcessing = (OpenCvImageProcessing)imageProcessing;

            //float otsuValue = imageProcessing.Otsu(algoImage);

            var binImage = (OpenCvGreyImage)algoImage.Clone();
            imageProcessing.Binarize(algoImage, binImage, Param.Threshold);
            //imageProcessing.Binarize(algoImage, binImage, (int)otsuValue);

            imageProcessing.Erode(binImage, Param.MorphologyFilterSize);
            imageProcessing.Dilate(binImage, Param.MorphologyFilterSize * 2);
            imageProcessing.Erode(binImage, Param.MorphologyFilterSize);

            var result = new EdgeDetectionResult();

            for (int i = 0; i < 2; i++)
            {
                var matPos = new List<Matrix<double>>();

                // 이미지의 구간을 정하여 모서리 검출
                float step = ((float)binImage.Width) / Param.AverageCount;
                for (float xPos = 0; xPos < binImage.Width; xPos += step)
                {
                    var clipImage = (OpenCvGreyImage)binImage.Clip(Rectangle.FromLTRB((int)xPos, 0, Math.Min((int)(xPos + step), binImage.Width - 1), binImage.Height - 1));
                    int yPos = GetEdgePos(clipImage, Convert.ToBoolean(i));
                    if (yPos > -1)
                    {
                        var pos = new Matrix<double>(1, 2);
                        pos.Data[0, 0] = xPos + step / 2;
                        pos.Data[0, 1] = yPos;

                        matPos.Add(pos);

                        //xList.Add((int)(xPos + step / 2));
                        //yList.Add(yPos);
                    }
                }

                // 직선성을 검사하여 튀어나온 점들은 제거
                matPos = Allsac(matPos, 20);

                var xList = new List<double>();
                var yList = new List<double>();

                for (int k = 0; k < matPos.Count; k++)
                {
                    xList.Add(matPos[k].Data[0, 0]);
                    yList.Add(matPos[k].Data[0, 1]);
                }

                // 셈플 개수가 모자라면 NG로 판단
                if (xList.Count >= Param.AverageCount / 2)
                {
                    if (Convert.ToBoolean(i))
                    {
                        result.FallingEdgePosition = new PointF((float)xList.Average(), (float)yList.Average());
                        result.Result = true;
                    }
                    else
                    {
                        result.RissingEdgePosition = new PointF((float)xList.Average(), (float)yList.Average());
                    }
                }
            }

            return result;
        }

        public Image<Gray, byte> Save1D_Data2Img(string fileName, ref int[] data, int CamUum)
        {
            if (data.Length == 0)
            {
                return null;
            }

            var image = new Image<Gray, byte>(data.Length, 512);

            try
            {
                lock (lockThis)
                {
                    int x, y;
                    int max = data.Max<int>();
                    int min = data.Min<int>();
                    int height = 0;
                    float fData = 0.0f;

                    image.SetValue(255 * 0.9);
                    for (x = 0; x < data.Length; ++x)
                    {
                        fData = (float)(data[x] - min) * image.Height / (max - min);
                        height = (int)Math.Floor(fData);
                        for (y = 0; y < height; ++y)
                        {
                            image.Data[image.Height - 1 - y, x, 0] = (byte)(255 * 0.4);
                        }
                    }
                    image.Save(fileName);
                }
            }
            catch
            {

            }
            return image;
        }

        public int[] DataMedianFiltering(int[] data, int filterSize)
        {
            int unit = filterSize / 2;
            if (filterSize % 2 == 0)
            {
                throw new FormatException();
            }

            int[] result = new int[data.Length];
            for (int i = 0; i < unit; i++)
            {
                result[i] = data[i];
            }

            for (int i = data.Length - 1; i > data.Length - 1 - unit; i--)
            {
                result[i] = data[i];
            }

            for (int i = unit; i < data.Length - unit; i++)
            {
                int[] d = new int[filterSize];
                int index = 0;
                for (int j = i - unit; j < unit + i + 1; j++)
                {
                    d[index] = data[j];
                    index++;
                }

                result[i] = FindMedianValue(d);
            }

            return result;
        }

        public int FindMedianValue(int[] data)
        {
            int result = 0;

            var d = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                d.Add(data[i]);
            }

            d.Sort();
            result = d[d.Count / 2];

            return result;
        }
    }
}
