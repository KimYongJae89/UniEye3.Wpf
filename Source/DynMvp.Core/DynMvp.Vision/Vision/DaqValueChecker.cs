using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    internal class DaqValueChecker : Algorithm
    {
        public DaqValueChecker()
        {
        }

        public override Algorithm Clone()
        {
            var daqValueChecker = new DaqValueChecker();
            daqValueChecker.Copy(this);

            return daqValueChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();

            return resultValues;
        }

        public static string TypeName => "DaqValueChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "DaqValue";
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            //AlgoImage probeClipImage = inspectParam.ProbeClipImage;
            //RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            //RectangleF imageRegionInFov = inspectParam.ImageRegionInFov;
            //DebugContext debugContext = inspectParam.DebugContext;

            //RotatedRect probeRegionInImage = new RotatedRect(probeRegionInFov);
            //probeRegionInImage.Offset(-imageRegionInFov.X, -imageRegionInFov.Y);

            //EdgeCheckerResult edgeCheckerResult = new EdgeCheckerResult();
            //edgeCheckerResult.ResultRect = probeRegionInFov.GetBoundRect();

            //bool result = false;

            //EdgeDetector edgeDetector = AlgorithmBuilder.CreateEdgeDetector();
            //if (edgeDetector == null)
            //    result = false;

            //EdgeDetectionResult edgeResult = null;

            //if (result == true)
            //{
            //    PointF[] points = probeRegionInImage.GetPoints();

            //    edgeDetector.Param = param;

            //    float detectorHalfHeight = MathHelper.GetLength(points[0], points[3]) / 2;
            //    float detectorHalfWidth = MathHelper.GetLength(points[0], points[1]) / 2;

            //    PointF centerPt = DrawingHelper.CenterPoint(probeRegionInImage);
            //    RotatedRect rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
            //                                        detectorHalfWidth * 2, detectorHalfHeight * 2, 270);

            //    edgeResult = edgeDetector.Detect(probeClipImage, rectangle, debugContext);
            //    result &= edgeResult.Result;
            //}

            //if (result == true)
            //{
            //    float gap = 0;

            //    switch (offsetAxisType)
            //    {
            //        case Vision.OffsetAxisType.xOffsetAxis:
            //            gap = Math.Abs(desiredOffset - edgeResult.EdgePosition.X);
            //            break;
            //        case Vision.OffsetAxisType.yOffsetAxis:
            //            gap = Math.Abs(desiredOffset - edgeResult.EdgePosition.Y);
            //            break;
            //    }

            //    edgeCheckerResult.Good = gap <= maxOffsetGap;
            //}
            //else
            //{
            //}

            //return edgeCheckerResult;
            return null;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {

        }
    }
}
