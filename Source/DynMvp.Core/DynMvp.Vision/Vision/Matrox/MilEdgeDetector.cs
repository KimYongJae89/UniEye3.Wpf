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
    public class MilEdgeDetector : EdgeDetector
    {
        public override AlgoImage GetEdgeImage(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext)
        {
            var milImage = (MilImage)algoImage;

            var result = new EdgeDetectionResult();

            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            MIL_ID detectorId = MIL.M_NULL;
            MIL.MmeasAllocMarker(MIL.M_DEFAULT_HOST, MIL.M_EDGE, MIL.M_DEFAULT, ref detectorId);

            /* Specify the stripe characteristics. */
            MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, GetPolarity(), MIL.M_DEFAULT);

            MIL.MmeasSetMarker(detectorId, MIL.M_ORIENTATION, MIL.M_VERTICAL, MIL.M_NULL);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_CENTER, centerPt.X, centerPt.Y);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_SIZE, rotatedRect.Width, rotatedRect.Height);
            MIL.MmeasSetMarker(detectorId, MIL.M_BOX_ANGLE, rotatedRect.Angle, MIL.M_NULL);
            MIL.MmeasSetMarker(detectorId, MIL.M_FILTER_TYPE, MIL.M_PREWITT, MIL.M_DEFAULT);

            /* Find the stripe and measure its width and angle. */
            MIL.MmeasFindMarker(MIL.M_DEFAULT, milImage.Image, detectorId, MIL.M_DEFAULT);

            MIL_INT numberOccurrencesFound = 0;
            MIL.MmeasGetResult(detectorId, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound, MIL.M_NULL);

            if (numberOccurrencesFound >= 1)
            {
                double positionX = 0;
                double positionY = 0;
                MIL.MmeasGetResult(detectorId, MIL.M_POSITION, ref positionX, ref positionY);

                result.Result = true;

                result.FallingEdgePosition = new PointF((float)positionX, (float)positionY);

                var resultImage = (MilGreyImage)algoImage.Clone();
                DrawReasult(detectorId, resultImage);

                resultImage.Save("FindEdge.bmp", debugContext);

                resultImage.Dispose();
            }

            MIL.MmeasFree(detectorId);

            return result;
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
                case EdgeType.DarkToLight: return MIL.M_POSITIVE;
                case EdgeType.LightToDark: return MIL.M_NEGATIVE;
                case EdgeType.Any: return MIL.M_ANY;
            }

            return MIL.M_NEGATIVE;
        }
    }
}
