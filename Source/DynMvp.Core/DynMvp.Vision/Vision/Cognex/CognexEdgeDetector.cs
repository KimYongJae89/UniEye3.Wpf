using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Cognex
{
    public class CognexEdgeDetector : EdgeDetector
    {
        public override AlgoImage GetEdgeImage(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext)
        {
            var result = new EdgeDetectionResult();

            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            var cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, rotatedRect.Width, rotatedRect.Height, MathHelper.DegToRad(rotatedRect.Angle), 0);

            var cogCaliper = new CogCaliper();

            var greyImage = (CognexGreyImage)algoImage;
            greyImage.Save("Caliper.bmp", debugContext);

            cogCaliper.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
            cogCaliper.Edge0Polarity = GetCogPolarity();
            cogCaliper.ContrastThreshold = Param.Threshold;
            cogCaliper.FilterHalfSizeInPixels = Param.GausianFilterSize;

            CogCaliperResults cogResults = cogCaliper.Execute(greyImage.Image, cogRectangle);

            if (cogResults.Edges.Count > 0)
            {
                result.Result = true;

                CogCaliperEdge maxEdge = cogResults.Edges[0];
                foreach (CogCaliperEdge edge in cogResults.Edges)
                {
                    if (cogCaliper.Edge0Polarity == CogCaliperPolarityConstants.LightToDark)
                    {
                        if (maxEdge.Contrast > edge.Contrast)
                        {
                            maxEdge = edge;
                        }
                    }
                    else if (cogCaliper.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)
                    {
                        if (maxEdge.Contrast < edge.Contrast)
                        {
                            maxEdge = edge;
                        }
                    }
                    else
                    {
                        if (Math.Abs(maxEdge.Contrast) < Math.Abs(edge.Contrast))
                        {
                            maxEdge = edge;
                        }
                    }
                }
                result.FallingEdgePosition = new PointF((float)maxEdge.PositionX, (float)maxEdge.PositionY);
            }

            return result;
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
