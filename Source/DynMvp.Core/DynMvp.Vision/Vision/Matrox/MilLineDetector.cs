using DynMvp.Base;
using DynMvp.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    internal class MilLineDetector : LineDetector
    {
        public override _1stPoliLineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext)
        {
            var greyImage = (MilGreyImage)algoImage;
            greyImage.Save("LineDetector.bmp", debugContext);

            var result = new EdgeDetectionResult();

            PointF centerPt = DrawingHelper.CenterPoint(startPt, endPt);
            float ptDistance = MathHelper.GetLength(startPt, endPt);

            endPt.X -= startPt.X;
            endPt.Y -= startPt.Y;

            endPt.Y *= -1;

            float angle = 0; // (float)MathHelper.GetAngle(new Point(0, 0), new PointF(1, 0), endPt);

            MIL_ID detectorId = MIL.MmeasAllocMarker(MIL.M_DEFAULT_HOST, MIL.M_EDGE, MIL.M_DEFAULT, MIL.M_NULL);

            /* Specify the stripe characteristics. */
            if (Param.EdgeType == EdgeType.DarkToLight)
            {
                MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, MIL.M_POSITIVE, MIL.M_DEFAULT);
            }
            else if (Param.EdgeType == EdgeType.LightToDark)
            {
                MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, MIL.M_NEGATIVE, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, MIL.M_ANY, MIL.M_DEFAULT);
            }

            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_CENTER, centerPt.X, centerPt.Y);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_SIZE, Param.SearchLength - 2, ptDistance - 2);
            MIL.MmeasSetMarker(detectorId, MIL.M_ORIENTATION, MIL.M_VERTICAL, MIL.M_NULL);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_ANGLE, angle, MIL.M_NULL);

            /* Find the stripe and measure its width and angle. */
            MIL.MmeasFindMarker(MIL.M_DEFAULT, greyImage.Image, detectorId, MIL.M_POSITION + MIL.M_LINE_EQUATION + MIL.M_ANGLE);

            long numberOccurrencesFound = 0;
            MIL.MmeasGetResult(detectorId, MIL.M_NUMBER + MIL.M_TYPE_LONG, ref numberOccurrencesFound, MIL.M_NULL);

            var lineEq = new _1stPoliLineEq();

            if (numberOccurrencesFound >= 1)
            {
                double lineAngle = 0;
                double[] posX = new double[numberOccurrencesFound];
                double[] posY = new double[numberOccurrencesFound];
                double coefficientA = 0;
                double coefficientB = 0;
                double coefficientC = 0;

                MIL.MmeasGetResult(detectorId, MIL.M_ANGLE + MIL.M_TYPE_DOUBLE, ref lineAngle, MIL.M_NULL);
                MIL.MmeasGetResult(detectorId, MIL.M_POSITION + MIL.M_TYPE_DOUBLE, posX, posY);
                MIL.MmeasGetResult(detectorId, MIL.M_LINE_A + MIL.M_TYPE_DOUBLE, ref coefficientA, MIL.M_NULL);
                MIL.MmeasGetResult(detectorId, MIL.M_LINE_B + MIL.M_TYPE_DOUBLE, ref coefficientB, MIL.M_NULL);
                MIL.MmeasGetResult(detectorId, MIL.M_LINE_C + MIL.M_TYPE_DOUBLE, ref coefficientC, MIL.M_NULL);

                if (coefficientB != 0)
                {
                    centerPt.Y = (float)((-coefficientA * centerPt.X - coefficientC) / coefficientB);
                }
                else
                {
                    centerPt.X = (float)(-coefficientC / coefficientA);
                    centerPt.Y = 0;
                }

                centerPt = new PointF((float)posX[0], (float)posY[0]);
                lineEq.Slope = new PointF(1, (float)Math.Tan(lineAngle / 180 * Math.PI));
                lineEq.PointOnLine = centerPt;

                var resultImage = (MilGreyImage)algoImage.Clone();
                DrawReasult(detectorId, resultImage);

                resultImage.Save("FindEdge.bmp", debugContext);

                result.Result = true;
            }

            MIL.MmeasFree(detectorId);

            return lineEq;
        }

        private void DrawReasult(MIL_ID detectorId, MilGreyImage resultImage)
        {
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_RESULT);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_RESULT);
        }

        private int GetPolarity()
        {
            switch (Param.EdgeType)
            {
                case EdgeType.DarkToLight: return MIL.M_NEGATIVE;
                case EdgeType.LightToDark: return MIL.M_POSITIVE;
                default:
                case EdgeType.Any: return MIL.M_ANY;
            }
        }
    }
}
