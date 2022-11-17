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
    public class WidthCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(WidthChecker))
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
                 value is WidthChecker)
            {
                var widthChecker = (WidthChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class WidthCheckerParam : AlgorithmParam
    {
        public EdgeType Edge1Type { get; set; } = EdgeType.DarkToLight;
        public EdgeType Edge2Type { get; set; } = EdgeType.DarkToLight;
        public float MaxWidthRatio { get; set; }
        public float MaxCenterGap { get; set; }
        public float MinWidthRatio { get; set; }
        public float ScaleValue { get; set; }
        public LineDetectorParam LineDetectorParam { get; set; }

        public WidthCheckerParam()
        {
            MinWidthRatio = 80;
            MaxWidthRatio = 120;
            MaxCenterGap = 50;

            Edge1Type = EdgeType.LightToDark;
            Edge2Type = EdgeType.DarkToLight;

            ScaleValue = 1;

            LineDetectorParam = new LineDetectorParam();
        }

        public override AlgorithmParam Clone()
        {
            var param = new WidthCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (WidthCheckerParam)srcAlgorithmParam;

            MinWidthRatio = param.MinWidthRatio;
            MaxWidthRatio = param.MaxWidthRatio;
            Edge1Type = param.Edge1Type;
            Edge2Type = param.Edge2Type;
            ScaleValue = param.ScaleValue;
            MaxCenterGap = param.MaxCenterGap;

            LineDetectorParam.Copy(param.LineDetectorParam);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            Edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge1Type", "LightToDark"));
            Edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge2Type", "DarkToLight"));

            MinWidthRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinWidthRatio", "80"));
            MaxWidthRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxWidthRatio", "120"));
            MaxCenterGap = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxCenterGap", "50"));
            ScaleValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScaleValue", "1.0"));

            LineDetectorParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "Edge1Type", Edge1Type.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge2Type", Edge2Type.ToString());

            XmlHelper.SetValue(algorithmElement, "MinWidthRatio", MinWidthRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxWidthRatio", MaxWidthRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxCenterGap", MaxCenterGap.ToString());

            XmlHelper.SetValue(algorithmElement, "ScaleValue", ScaleValue.ToString());

            LineDetectorParam.SaveParam(algorithmElement);
        }
    }

    [TypeConverterAttribute(typeof(WidthCheckerConverter))]
    public class WidthChecker : Algorithm
    {
        public WidthChecker()
        {
            param = new WidthCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            //return true;
            return false;
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            PointF[] points = probeRect.GetPoints();

            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();
            if (lineDetector == null)
            {
                return;
            }

            lineDetector.Param = ((WidthCheckerParam)Param).LineDetectorParam;

            lineDetector.AppendLineDetectorFigures(tempFigures, points[0], points[3]);
            lineDetector.AppendLineDetectorFigures(tempFigures, points[2], points[1]);
        }

        public override Algorithm Clone()
        {
            var widthDetector = new WidthChecker();
            widthDetector.Copy(this);

            return widthDetector;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var widthDetector = (WidthChecker)algorithm;

            param.Copy(widthDetector.param);
        }

        public const string TypeName = "WidthChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Width";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            newInspRegion.Inflate(((WidthCheckerParam)param).LineDetectorParam.SearchLength / 2, 0);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var widthCheckerParam = (WidthCheckerParam)Param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Width Ratio", "", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
            resultValues.Add(new ResultValue("Width", "", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
            resultValues.Add(new ResultValue("Center Gap", "", widthCheckerParam.MaxCenterGap));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            bool on3dInspection = false;
            Image2D clipImage;
            Image2D clipImageRotate = null;
            if (inspectParam.InspectImageList[0] is Image3D)
            {
                clipImage = Image2D.ToImage2D(inspectParam.InspectImageList[0].ToBitmap());
                on3dInspection = true;
            }
            else
            {
                clipImage = (Image2D)inspectParam.InspectImageList[0];
            }

            var widthCheckerParam = (WidthCheckerParam)Param;

            DebugHelper.SaveImage(clipImage, "ClipImage.bmp", inspectParam.DebugContext);

            AlgoImage probeClipImage = ImageBuilder.Build(GetAlgorithmType(), clipImage, ImageType.Grey, param.ImageBand);

            if (clipImageRotate != null)
            {
                probeClipImage = ImageBuilder.Build(GetAlgorithmType(), clipImageRotate, ImageType.Grey, param.ImageBand);
            }

            Filter(probeClipImage);

            DebugHelper.SaveImage(probeClipImage, "ProbeClipImage.bmp", inspectParam.DebugContext);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);
            if (inspectRegionInFov.Angle != 0)
            {
                probeRegionInClip.Offset(-probeRegionInClip.X, -probeRegionInClip.Y);
            }

            bool result = false;

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            var widthCheckerResult = new AlgorithmResult();
            widthCheckerResult.ResultRect = probeRegionInFov;

            float desiredLength = MathHelper.GetLength(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            WidthResult widthResult = null;

            var temp = new RectangleF(probeRegionInClip.X, probeRegionInClip.Y, probeRegionInClip.Width, probeRegionInClip.Height);
            temp.Offset(-probeRegionInClip.X, -probeRegionInClip.Y);

            if (((WidthCheckerParam)param).LineDetectorParam.NumEdgeDetector == 1)
            {
                widthResult = GetLengthWithEdge(probeClipImage, temp, debugContext, probeRegionInFov.Angle);
            }
            else
            {
                widthResult = GetLengthWithLine(probeClipImage, probeRegionInClip, debugContext);
            }

            float measureLength = widthResult.MeasureLength;
            if (on3dInspection == true)
            {
                desiredLength *= 0.3f;
                measureLength *= 0.3f;
                widthResult.MeasureLength = measureLength;
            }
            widthResult.DesiredLength = desiredLength;

            result = widthResult.Good;

            if (result == true)
            {
                float ratio = widthResult.MeasureLength / desiredLength * 100;

                widthCheckerResult.SetResult((ratio >= widthCheckerParam.MinWidthRatio) && (ratio <= widthCheckerParam.MaxWidthRatio)); //  && gap <= maxCenterGap;

                var edgePoint1 = new PointF(widthResult.EdgePoint1.X + inspectRegionInFov.X,
                                                                widthResult.EdgePoint1.Y + inspectRegionInFov.Y);
                var edgePoint2 = new PointF(widthResult.EdgePoint2.X + inspectRegionInFov.X,
                                                                widthResult.EdgePoint2.Y + inspectRegionInFov.Y);

                widthCheckerResult.AddResultValue(new ResultValue("Width Ratio", "", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, ratio));
                widthCheckerResult.AddResultValue(new ResultValue("Width", "", desiredLength * widthCheckerParam.MaxWidthRatio / 100,
                                                                desiredLength * widthCheckerParam.MinWidthRatio / 100, measureLength));
                widthCheckerResult.AddResultValue(new ResultValue("Center Gap", "", widthCheckerParam.MaxCenterGap, 0, 0));

                widthCheckerResult.ResultFigures.AddFigure(new CrossFigure(DrawingHelper.CenterPoint(edgePoint1, edgePoint2), 10, new Pen(Color.White, 1.0F)));
                widthCheckerResult.ResultFigures.AddFigure(new CrossFigure(edgePoint1, 10, new Pen(Color.White, 1.0F)));
                widthCheckerResult.ResultFigures.AddFigure(new CrossFigure(edgePoint2, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                widthCheckerResult.SetResult(false);
                widthCheckerResult.AddResultValue(new ResultValue("Width Ratio", "", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
                widthCheckerResult.AddResultValue(new ResultValue("Width", "", desiredLength * widthCheckerParam.MaxWidthRatio / 100,
                                                                desiredLength * widthCheckerParam.MinWidthRatio / 100, measureLength));
                widthCheckerResult.AddResultValue(new ResultValue("Center Gap", "", widthCheckerParam.MaxCenterGap, 0, 0));
            }

            return widthCheckerResult;
        }

        private WidthResult GetLengthWithLine(AlgoImage probeClipImage, RectangleF probeRegion, DebugContext debugContext)
        {
            var widthResult = new WidthResult();

            LineDetector lineDetector = AlgorithmFactory.Instance().CreateLineDetector();
            if (lineDetector == null)
            {
                return widthResult;
            }

            var widthCheckerParam = (WidthCheckerParam)Param;

            PointF[] points = DrawingHelper.GetPoints(probeRegion, 0);

            var centerLineEq = new _1stPoliLineEq(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            lineDetector.Param = widthCheckerParam.LineDetectorParam;

            lineDetector.Param.EdgeType = widthCheckerParam.Edge1Type;
            _1stPoliLineEq startLineEq = lineDetector.Detect(probeClipImage, points[0], points[3], debugContext);

            lineDetector.Param.EdgeType = widthCheckerParam.Edge2Type;
            _1stPoliLineEq endLineEq = lineDetector.Detect(probeClipImage, points[2], points[1], debugContext);

            var pt1 = new PointF();
            var pt2 = new PointF();

            bool result1 = _1stPoliLineEq.GetIntersectPoint(centerLineEq, startLineEq, ref pt1);
            bool result2 = _1stPoliLineEq.GetIntersectPoint(centerLineEq, endLineEq, ref pt2);

            widthResult.Good = (result1 == true && result2 == true);
            if (widthResult.Good)
            {
                widthResult.MeasureLength = MathHelper.GetLength(pt1, pt2);
                widthResult.EdgePoint1 = pt1;
                widthResult.EdgePoint2 = pt2;
            }

            return widthResult;
        }

        public RotatedRect GetDetectorRect(PointF startPt, PointF endPt, bool isHorizontal) //isHorizontal : true면 수평꺼
        {
            LineDetectorParam lineDetectorParam = ((WidthCheckerParam)Param).LineDetectorParam;

            float detectorHalfHeight = lineDetectorParam.ProjectionHeight / 2;
            float detectorHalfWidth = lineDetectorParam.SearchLength / 2;

            float theta = (float)(MathHelper.RadToDeg(MathHelper.arctan(endPt.Y - startPt.Y, endPt.X - startPt.X))) - 90;

            var centerPt = new PointF(startPt.X + (endPt.X - startPt.X) / 2, startPt.Y + (endPt.Y - startPt.Y) / 2);

            RotatedRect rectangle;

            if (startPt.Y < endPt.Y)
            {
                rectangle = new RotatedRect(centerPt.X, centerPt.Y - detectorHalfHeight,
                                                detectorHalfWidth * 2, detectorHalfHeight * 2, theta);
            }
            else
            {
                rectangle = new RotatedRect(centerPt.X - detectorHalfWidth * 2, centerPt.Y - detectorHalfHeight,
                                                    detectorHalfWidth * 2, detectorHalfHeight * 2, theta);
            }

            return rectangle;
        }

        private WidthResult GetLengthWithEdge(AlgoImage probeClipImage, RectangleF probeRegion, DebugContext debugContext, float angle)
        {
            var widthResult = new WidthResult();

            EdgeDetector edgeDetector1 = AlgorithmFactory.Instance().CreateEdgeDetector();
            EdgeDetector edgeDetector2 = AlgorithmFactory.Instance().CreateEdgeDetector();
            if (edgeDetector1 == null || edgeDetector2 == null)
            {
                return widthResult;
            }

            var widthCheckerParam = (WidthCheckerParam)Param;
            LineDetectorParam lineDetectorParam = widthCheckerParam.LineDetectorParam;

            PointF[] points;

            RotatedRect rect1, rect2;

            EdgeDetectionResult result1, result2;

            if (angle != 0)  //90도
            {
                points = DrawingHelper.GetPoints(probeRegion, angle);
                for (int i = 0; i < 4; i++)
                {
                    if (points[i].X < 0 && points[i].X > -1)
                    {
                        points[i].X = 0;
                    }

                    if (points[i].Y < 0 && points[i].Y > -1)
                    {
                        points[i].Y = 0;
                    }

                    if (points[i].X > 0 && points[i].X < 1)
                    {
                        points[i].X = 0;
                    }

                    if (points[i].Y > 0 && points[i].Y < 1)
                    {
                        points[i].Y = 0;
                    }
                }
                //PointF[] points = DrawingHelper.GetPoints(probeRegion, 0);

                lineDetectorParam.Threshold = 3;
                edgeDetector1.Param = lineDetectorParam;
                edgeDetector2.Param = lineDetectorParam;

                rect1 = GetDetectorRect(points[0], points[3], false);
                rect1.Angle = angle + (lineDetectorParam.SearchAngle - 90);

                rect2 = GetDetectorRect(points[2], points[1], false);
                rect2.Angle = angle + (lineDetectorParam.SearchAngle - 90);

                edgeDetector1.Param.EdgeType = widthCheckerParam.Edge1Type;
                result1 = edgeDetector1.Detect(probeClipImage, rect1, debugContext);

                edgeDetector2.Param.EdgeType = widthCheckerParam.Edge2Type;
                result2 = edgeDetector2.Detect(probeClipImage, rect2, debugContext);
            }
            else
            {
                points = DrawingHelper.GetPoints(probeRegion, 0);

                lineDetectorParam.Threshold = 3;
                edgeDetector1.Param = lineDetectorParam;
                edgeDetector2.Param = lineDetectorParam;

                rect1 = GetDetectorRect(points[0], points[3], true);
                rect1.Angle = rect1.Angle + (lineDetectorParam.SearchAngle - 90);

                rect2 = GetDetectorRect(points[2], points[1], true);
                rect2.Angle = rect2.Angle + (lineDetectorParam.SearchAngle - 90);

                edgeDetector1.Param.EdgeType = widthCheckerParam.Edge1Type;
                result1 = edgeDetector1.Detect(probeClipImage, rect1, debugContext);

                edgeDetector2.Param.EdgeType = widthCheckerParam.Edge2Type;
                result2 = edgeDetector2.Detect(probeClipImage, rect2, debugContext);
            }

            if (result1.Result == true && result2.Result == true)
            {
                widthResult.MeasureLength = MathHelper.GetLength(result1.FallingEdgePosition, result2.FallingEdgePosition);
                widthResult.EdgePoint1 = result1.FallingEdgePosition;
                widthResult.EdgePoint2 = result2.FallingEdgePosition;
                widthResult.Good = true;
            }

            return widthResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            ResultValue resultValue = algorithmResult.GetResultValue("Defect Message");
            if (resultValue != null)
            {
                resultMessage.AddTextLine(resultValue.Value.ToString());
            }
        }
    }

    public class WidthResult
    {
        public bool Good { get; set; }
        public float DesiredLength { get; set; }
        public float MeasureLength { get; set; }
        public float MeasureCenter { get; set; }
        public float DesiredCenter { get; set; }
        public PointF EdgePoint1 { get; set; }
        public PointF EdgePoint2 { get; set; }
    }
}
