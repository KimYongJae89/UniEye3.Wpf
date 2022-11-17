using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision
{
    public class SimpleEdgeDetector
    {
        public enum SearchDireciton
        {
            LeftToRight, RightToLeft
        }

        public SimpleEdgeDetector()
        {

        }

        public double FindEdgePosition(AlgoImage algoImage, double[] thresholdArray, SearchDireciton searchDirection = SearchDireciton.LeftToRight)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            float[] sheetProfile = imageProcessing.Projection(algoImage, TwoWayDirection.Horizontal, ProjectionType.Mean);

            int index = 0;

            if (searchDirection == SearchDireciton.RightToLeft)
            {
                index = Array.FindLastIndex(sheetProfile, x => x > thresholdArray[0]);
            }
            else
            {
                index = Array.FindIndex(sheetProfile, x => x > thresholdArray[0]);
            }

            //if (index >= 0)
            //{
            //    return index;
            //}
            //else
            //{
            //    DilatedDiff(sheetProfile, 30, out float[] diff);

            //    FindEdgeIndex(sheetProfile, thresholdArray, out double[] edgeIndex);

            //    return edgeIndex[0];
            //}
            return index;
        }

        private void FindEdgeIndex(float[] src, double[] thres, out double[] indices)
        {
            indices = new double[thres.Length];

            const int dilatedSize = 20;
            int dilatedIndex = 0;
            int edgeIndexSize = thres.Length;

            int edgeIndex = 0;
            bool begin = false;
            int beginIndex = 0;

            for (int i = 0; i < src.Length; ++i)
            {
                if (src[i] > thres[edgeIndex])
                {
                    if (!begin)
                    {
                        begin = true;
                        beginIndex = i;
                    }
                    else
                    {
                        dilatedIndex = 0;
                    }
                }
                else
                {
                    if (begin)
                    {
                        if (dilatedIndex < dilatedSize)
                        {
                            dilatedIndex++;
                        }
                        else
                        {
                            indices[edgeIndex] = (beginIndex + i - dilatedSize) / 2;
                            dilatedIndex = 0;
                            begin = false;
                            edgeIndex++;

                            if (edgeIndex >= edgeIndexSize)
                            {
                                break;
                            }

                            if (thres[edgeIndex] < thres[edgeIndex - 1])
                            {
                                int halfThres = (int)(thres[edgeIndex] / 2);
                                while (i < src.Length)
                                {
                                    if (src[i] < halfThres)
                                    {
                                        break;
                                    }

                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
