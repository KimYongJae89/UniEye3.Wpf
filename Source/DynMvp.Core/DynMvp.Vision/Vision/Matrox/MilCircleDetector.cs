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
    public class MilCircleDetector : CircleDetector
    {
        public override CircleEq Detect(AlgoImage algoImage, DebugContext debugContext)
        {
            var circle = new CircleEq();

            var greyImage = (MilGreyImage)algoImage;

            MIL_ID detectorId = MIL.MmeasAllocMarker(MIL.M_DEFAULT_HOST, MIL.M_CIRCLE, MIL.M_DEFAULT, MIL.M_NULL);

            SetMarker(detectorId);
            MIL.MmeasFindMarker(MIL.M_DEFAULT, greyImage.Image, detectorId, MIL.M_DEFAULT);

            MIL_INT numberOccurrencesFound = 0;
            MIL.MmeasGetResult(detectorId, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound, MIL.M_NULL);

            if (numberOccurrencesFound >= 1)
            {
                double positionX = 0;
                double positionY = 0;
                double radius = 0;

                MIL.MmeasGetResult(detectorId, MIL.M_POSITION, ref positionX, ref positionY);
                MIL.MmeasGetResult(detectorId, MIL.M_RADIUS, ref radius, MIL.M_NULL);

                circle.Center = new PointF((float)positionX, (float)positionY);
                circle.Radius = (float)radius;
            }

            MIL.MmeasFree(detectorId);
            return circle;
        }

        private void DrawReasult(MIL_ID detectorId, MilGreyImage resultImage)
        {
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_BLUE);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_SEARCH_REGION, MIL.M_DEFAULT, MIL.M_MARKER);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_RESULT);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_RESULT);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_YELLOW);
            MIL.MmeasDraw(MIL.M_DEFAULT, detectorId, resultImage.Image, MIL.M_DRAW_SUB_POSITIONS, MIL.M_DEFAULT, MIL.M_RESULT);
        }

        private void SetMarker(MIL_ID detectorId)
        {
            MIL.MmeasSetMarker(detectorId, MIL.M_RING_CENTER, Param.CenterPosition.X, Param.CenterPosition.Y);
            MIL.MmeasSetMarker(detectorId, MIL.M_POLARITY, GetPolarity(), MIL.M_DEFAULT);

            MIL.MmeasSetMarker(detectorId, MIL.M_CIRCLE_INSIDE_SEARCH_REGION, MIL.M_ENABLE, MIL.M_NULL);
            MIL.MmeasSetMarker(detectorId, MIL.M_RING_RADII, Param.InnerRadius, Param.OutterRadius);
            MIL.MmeasSetMarker(detectorId, MIL.M_MAX_ASSOCIATION_DISTANCE, MIL.M_DEFAULT, MIL.M_NULL);
            MIL.MmeasSetMarker(detectorId, MIL.M_EDGEVALUE_MIN, Param.MinEdgeValue, MIL.M_NULL);

            if (Param.Smallest == true)
            {
                MIL.MmeasSetScore(detectorId, MIL.M_RADIUS_SCORE, 0, 0, 0, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MmeasSetScore(detectorId, MIL.M_RADIUS_SCORE, 0, 0, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);
            }
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
