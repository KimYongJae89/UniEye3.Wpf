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
    public class LineCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(LineChecker))
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
                 value is LineChecker)
            {
                var lineChecker = (LineChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class LineCheckerParam : AlgorithmParam
    {
        public LineDetectorParam LineDetectorParam { get; set; } = new LineDetectorParam();
        public float StartPosition { get; set; }
        public float EndPosition { get; set; }
        public float ScaleValue { get; set; }

        public LineCheckerParam()
        {
            StartPosition = 80;
            EndPosition = 120;

            ScaleValue = 1;
        }

        public override AlgorithmParam Clone()
        {
            var param = new LineCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (LineCheckerParam)srcAlgorithmParam;
            LineDetectorParam = (LineDetectorParam)param.LineDetectorParam.Clone();

            StartPosition = param.StartPosition;
            EndPosition = param.EndPosition;
            ScaleValue = param.ScaleValue;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            StartPosition = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "StartPosition", "80"));
            EndPosition = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "EndPosition", "120"));
            ScaleValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScaleValue", "1.0"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "StartPosition", StartPosition.ToString());
            XmlHelper.SetValue(algorithmElement, "EndPosition", EndPosition.ToString());
            XmlHelper.SetValue(algorithmElement, "ScaleValue", ScaleValue.ToString());
        }
    }

    [TypeConverterAttribute(typeof(LineCheckerConverter))]
    public class LineChecker : Algorithm
    {
        public LineChecker()
        {
            param = new LineCheckerParam();
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();
            lineDetector.Param = ((LineCheckerParam)param).LineDetectorParam;

            lineDetector.AppendLineDetectorFigures(tempFigures, new PointF(probeRect.Left, probeRect.Top), new PointF(probeRect.Right, probeRect.Bottom));
        }

        public override Algorithm Clone()
        {
            var lineChecker = new LineChecker();
            lineChecker.Copy(this);

            return lineChecker;
        }

        public const string TypeName = "LineChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Line";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            LineDetectorParam lineDetectorParam = ((LineCheckerParam)param).LineDetectorParam;
            newInspRegion.Inflate(lineDetectorParam.SearchLength / 2, 0);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var lineCheckerParam = (LineCheckerParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Line Position", "", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            var lineCheckerParam = (LineCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            var lineCheckerResult = new AlgorithmResult();
            lineCheckerResult.ResultRect = probeRegionInFov;

            LineDetectorResult lineDetectorResult = GetLengthWithLine(clipImage, probeRegionInClip, debugContext);
            if (lineDetectorResult.Good == true)
            {
                lineCheckerResult.SetResult((lineDetectorResult.Position >= lineCheckerParam.StartPosition) && (lineDetectorResult.Position <= lineCheckerParam.EndPosition));
                lineCheckerResult.AddResultValue(new ResultValue("Line Position", "", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, lineDetectorResult.Position));

                lineCheckerResult.ResultFigures.AddFigure(new LineFigure(lineDetectorResult.EdgePoint1, lineDetectorResult.EdgePoint2, new Pen(Color.White, 1.0F)));
                lineCheckerResult.ResultFigures.AddFigure(new CrossFigure(lineDetectorResult.EdgePoint1, 10, new Pen(Color.White, 1.0F)));
                lineCheckerResult.ResultFigures.AddFigure(new CrossFigure(lineDetectorResult.EdgePoint2, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                lineCheckerResult.SetResult(false);
                lineCheckerResult.AddResultValue(new ResultValue("Line Position", "", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, 0));
            }

            return lineCheckerResult;
        }

        private LineDetectorResult GetLengthWithLine(AlgoImage clipImage, RectangleF probeRegion, DebugContext debugContext)
        {
            var lineDetectorResult = new LineDetectorResult();

            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();
            lineDetector.Param = ((LineCheckerParam)param).LineDetectorParam;

            var startPoint = new PointF(probeRegion.Left, probeRegion.Top);
            var endPoint = new PointF(probeRegion.Right, probeRegion.Bottom);

            _1stPoliLineEq findLineEq = lineDetector.Detect(clipImage, startPoint, endPoint, debugContext);

            PointF[] points = DrawingHelper.GetPoints(probeRegion, 0);
            var startLineEq = new _1stPoliLineEq(points[0], points[3]);

            var centerLineEq = new _1stPoliLineEq(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            var pt1 = new PointF();
            var pt2 = new PointF();

            bool result1 = _1stPoliLineEq.GetIntersectPoint(centerLineEq, startLineEq, ref pt1);
            bool result2 = _1stPoliLineEq.GetIntersectPoint(centerLineEq, findLineEq, ref pt2);

            if (result1 == true && result2 == true)
            {
                lineDetectorResult.Good = true;
                lineDetectorResult.Position = MathHelper.GetLength(pt1, pt2);
                lineDetectorResult.EdgePoint1 = pt1;
                lineDetectorResult.EdgePoint2 = pt2;
            }

            return lineDetectorResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }

    public class LineDetectorResult
    {
        public bool Good { get; set; } = false;
        public float Position { get; set; }
        public PointF EdgePoint1 { get; set; }
        public PointF EdgePoint2 { get; set; }
    }
}
