using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Cognex
{
    public class CognexLineDetector : LineDetector
    {
        public override _1stPoliLineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext)
        {
            var cogFindLine = new CogFindLine();

            DebugHelper.SaveImage(algoImage, "Caliper.bmp", debugContext);

            var greyImage = (CognexGreyImage)algoImage;

            cogFindLine.NumCalipers = Param.NumEdgeDetector;
            cogFindLine.ExpectedLineSegment.SetStartEnd(startPt.X, startPt.Y, endPt.X, endPt.Y);

            cogFindLine.CaliperProjectionLength = Param.ProjectionHeight;
            cogFindLine.CaliperSearchLength = Param.SearchLength;
            cogFindLine.CaliperRunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
            cogFindLine.CaliperRunParams.Edge0Polarity = GetCogPolarity();
            cogFindLine.CaliperSearchDirection = MathHelper.DegToRad(Param.SearchAngle);

            cogFindLine.CaliperRunParams.ContrastThreshold = Param.Threshold;
            cogFindLine.CaliperRunParams.FilterHalfSizeInPixels = Param.GausianFilterSize;

            CogFindLineResults results = cogFindLine.Execute(greyImage.Image);

            return GetLineParameter(results);
        }

        public _1stPoliLineEq GetLineParameter(CogFindLineResults results)
        {
            var lineEq = new _1stPoliLineEq();
            if (results != null && results.Count > 0)
            {
                CogLine line = results.GetLine();
                if (line != null)
                {
                    lineEq.Slope = new PointF(1, (float)Math.Tan(line.Rotation));
                    lineEq.PointOnLine = new PointF((float)line.X, (float)line.Y);
                }
            }

            return lineEq;
        }

        private CogCaliperPolarityConstants GetCogPolarity()
        {
            switch (Param.EdgeType)
            {
                case EdgeType.DarkToLight: return CogCaliperPolarityConstants.DarkToLight;
                case EdgeType.LightToDark: return CogCaliperPolarityConstants.LightToDark;
                case EdgeType.Any: return CogCaliperPolarityConstants.DontCare;
            }

            return CogCaliperPolarityConstants.DarkToLight;
        }
    }
}
