using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum CornerType
    {
        LeftTop, RightTop, LeftBottom, RightBottom
    }

    public enum RectType
    {
        Vertical, Horizontal
    }

    public class CornerCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CornerChecker))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(string) &&
                 value is CornerChecker)
            {
                var widthChecker = (CornerChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class CornerCheckerParam : AlgorithmParam
    {
        public CornerType CornerType { get; set; } = CornerType.LeftTop;
        public EdgeType Edge1Type { get; set; } = EdgeType.DarkToLight;
        public EdgeType Edge2Type { get; set; } = EdgeType.DarkToLight;
        public LineDetectorParam LineDetectorParam { get; set; } = new LineDetectorParam();

        public override AlgorithmParam Clone()
        {
            var param = new CornerCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (CornerCheckerParam)srcAlgorithmParam;

            CornerType = param.CornerType;
            Edge1Type = param.Edge1Type;
            Edge2Type = param.Edge2Type;
            LineDetectorParam = (LineDetectorParam)param.LineDetectorParam.Clone();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            CornerType = (CornerType)Enum.Parse(typeof(CornerType), XmlHelper.GetValue(algorithmElement, "CornerType", CornerType.LeftTop.ToString()));
            Edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge1Type", EdgeType.DarkToLight.ToString()));
            Edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge2Type", EdgeType.DarkToLight.ToString()));
            LineDetectorParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CornerType", CornerType.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge1Type", Edge1Type.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge2Type", Edge2Type.ToString());
            LineDetectorParam.SaveParam(algorithmElement);
        }
    }

    [TypeConverterAttribute(typeof(WidthCheckerConverter))]
    public class CornerChecker : Algorithm
    {
        public CornerChecker()
        {
            param = new CornerCheckerParam();
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            PointF[] points = probeRect.GetPoints();

            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();
            //((LineDetectorParam)lineDetector). = param ;

            PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);
            PointF topCenterPoint = DrawingHelper.CenterPoint(points[1], points[0]);
            PointF bottomCenterPoint = DrawingHelper.CenterPoint(points[3], points[2]);
            PointF leftCenterPoint = DrawingHelper.CenterPoint(points[3], points[0]);
            PointF rightCenterPoint = DrawingHelper.CenterPoint(points[2], points[1]);

            var cornerCheckerParam = (CornerCheckerParam)param;

            switch (cornerCheckerParam.CornerType)
            {
                case CornerType.RightBottom:
                    lineDetector.AppendLineDetectorFigures(tempFigures, leftCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(tempFigures, topCenterPoint, centerPoint);
                    break;
                case CornerType.RightTop:
                    lineDetector.AppendLineDetectorFigures(tempFigures, rightCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(tempFigures, topCenterPoint, centerPoint);
                    break;
                case CornerType.LeftBottom:
                    lineDetector.AppendLineDetectorFigures(tempFigures, leftCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(tempFigures, bottomCenterPoint, centerPoint);
                    break;
                case CornerType.LeftTop:
                    lineDetector.AppendLineDetectorFigures(tempFigures, rightCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(tempFigures, bottomCenterPoint, centerPoint);
                    break;
            }
        }

        public override Algorithm Clone()
        {
            var cornerChecker = new CornerChecker();
            cornerChecker.Copy(this);

            return cornerChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var cornerChecker = (CornerChecker)algorithm;

            param = (CornerCheckerParam)cornerChecker.Param.Clone();
        }

        public static string TypeName => "CornerChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Corner";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            newInspRegion.Inflate(((CornerCheckerParam)param).LineDetectorParam.SearchLength / 2, 0);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("CornerPoint", "", new PointF(0, 0)));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            var cornerCheckerResult = new SearchableResult();
            cornerCheckerResult.ResultRect = probeRegionInFov;

            CornerResult cornerResult = GetCornerWithLine(clipImage, probeRegionInClip, debugContext);

            if (cornerResult.Good == true)
            {
                PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);

                var offset = new SizeF();
                offset.Width = cornerResult.CornerPoint.X - centerPoint.X;
                offset.Height = cornerResult.CornerPoint.Y - centerPoint.Y;

                cornerCheckerResult.OffsetFound = offset;
                var cornerPoint = new PointF(cornerResult.CornerPoint.X + inspectRegionInFov.X,
                                                                cornerResult.CornerPoint.Y + inspectRegionInFov.Y);

                cornerCheckerResult.AddResultValue(new ResultValue("CornerPoint", "", cornerPoint));
                cornerCheckerResult.ResultFigures.AddFigure(new CrossFigure(cornerPoint, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                cornerCheckerResult.OffsetFound = new SizeF(0, 0);
                cornerCheckerResult.AddResultValue(new ResultValue("CornerPoint", "", new PointF(0, 0)));
            }

            return cornerCheckerResult;
        }

        private CornerResult GetCornerWithLine(AlgoImage probeClipImage, RectangleF probeRegionInClip, DebugContext debugContext)
        {
            var cornerResult = new CornerResult();

            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            var cornerCheckerParam = (CornerCheckerParam)param;
            lineDetector.Param = cornerCheckerParam.LineDetectorParam;

            PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);
            PointF topCenterPoint = DrawingHelper.CenterPoint(points[1], points[0]);
            PointF bottomCenterPoint = DrawingHelper.CenterPoint(points[3], points[2]);
            PointF leftCenterPoint = DrawingHelper.CenterPoint(points[3], points[0]);
            PointF rightCenterPoint = DrawingHelper.CenterPoint(points[2], points[1]);

            _1stPoliLineEq lineEq1 = new _1stPoliLineEq(), lineEq2 = new _1stPoliLineEq();

            switch (cornerCheckerParam.CornerType)
            {
                case CornerType.RightBottom:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, leftCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, topCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.RightTop:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, rightCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, topCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.LeftBottom:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, leftCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, bottomCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.LeftTop:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, rightCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, bottomCenterPoint, centerPoint, debugContext);
                    break;
            }

            var pt = new PointF();
            cornerResult.Good = _1stPoliLineEq.GetIntersectPoint(lineEq1, lineEq2, ref pt);
            if (cornerResult.Good == true)
            {
                cornerResult.CornerPoint = pt;
            }

            return cornerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {

        }
    }

    public class CornerResult
    {
        public bool Good { get; set; }
        public PointF CornerPoint { get; set; }
    }
}
