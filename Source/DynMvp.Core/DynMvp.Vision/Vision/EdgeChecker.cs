using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum EdgeDirection
    {
        Horizontal, Vertical
    }

    public class EdgeCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(EdgeChecker))
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
                 value is EdgeChecker)
            {
                var edgeChecker = (EdgeChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class EdgeCheckerParam : AlgorithmParam
    {
        public EdgeDetectorParam EdgeDetectorParam { get; set; } = new EdgeDetectorParam();

        public EdgeType EdgeType
        {
            get => EdgeDetectorParam.EdgeType;
            set => EdgeDetectorParam.EdgeType = value;
        }

        public int FilterSize
        {
            get => EdgeDetectorParam.GausianFilterSize;
            set => EdgeDetectorParam.GausianFilterSize = value;
        }

        public int MedianFilterSize
        {
            get => EdgeDetectorParam.MedianFilterSize;
            set => EdgeDetectorParam.MedianFilterSize = value;
        }

        public int EdgeThreshold
        {
            get => EdgeDetectorParam.Threshold;
            set => EdgeDetectorParam.Threshold = value;
        }

        public int MorphologyFilterSize
        {
            get => EdgeDetectorParam.MorphologyFilterSize;
            set => EdgeDetectorParam.MorphologyFilterSize = value;
        }

        public int AverageCount
        {
            get => EdgeDetectorParam.AverageCount;
            set => EdgeDetectorParam.AverageCount = value;
        }
        public EdgeDirection EdgeDirection { get; set; }
        public float DesiredOffset { get; set; }
        public float MaxOffset { get; set; }

        public EdgeCheckerParam()
        {
            EdgeDirection = EdgeDirection.Horizontal;
            DesiredOffset = 700;
            MaxOffset = 5;
        }

        public override AlgorithmParam Clone()
        {
            var param = new EdgeCheckerParam();

            param.Copy(this);

            return param;
        }

        //public override object GetParamValue(string paramName)
        //{
        //    PropertyInfo propertyInfo = GetType().GetProperty(paramName);
        //    if (propertyInfo != null)
        //        return propertyInfo.GetValue(this);

        //    return edgeDetectorParam.GetParamValue(paramName);
        //}

        public override bool SetParamValue(string paramName, object value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value);
                return true;
            }

            return EdgeDetectorParam.SetParamValue(paramName, value);
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (EdgeCheckerParam)srcAlgorithmParam;
            EdgeDetectorParam = param.EdgeDetectorParam.Clone();

            EdgeDirection = param.EdgeDirection;
            DesiredOffset = param.DesiredOffset;
            MaxOffset = param.MaxOffset;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            EdgeDetectorParam.LoadParam(algorithmElement);
            EdgeDirection = (EdgeDirection)Enum.Parse(typeof(EdgeDirection), XmlHelper.GetValue(algorithmElement, "EdgeDirection", "Horizontal"));
            DesiredOffset = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "DesiredOffset", "700"));
            MaxOffset = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "MaxOffset", "5"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            EdgeDetectorParam.SaveParam(algorithmElement);
            XmlHelper.SetValue(algorithmElement, "EdgeDirection", EdgeDirection.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredOffset", DesiredOffset.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxOffset", MaxOffset.ToString());
        }
    }

    [TypeConverterAttribute(typeof(EdgeCheckerConverter))]
    public class EdgeChecker : Algorithm
    {
        public EdgeChecker()
        {
            param = new EdgeCheckerParam();
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            PointF[] points = probeRect.GetPoints();
            var edgeCheckerParam = (EdgeCheckerParam)param;

            var pen = new Pen(Color.Red, 5.0F);
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            float desiredOffset = edgeCheckerParam.DesiredOffset;

            switch (edgeCheckerParam.EdgeDirection)
            {
                case EdgeDirection.Horizontal:
                    tempFigures.AddFigure(new LineFigure(new PointF(points[0].X, desiredOffset), new PointF(points[1].X, desiredOffset), pen));
                    break;
                case EdgeDirection.Vertical:
                    tempFigures.AddFigure(new LineFigure(new PointF(points[0].X + desiredOffset, points[0].Y), new PointF(points[3].X + desiredOffset, points[3].Y), pen));
                    break;
            }

        }

        public override ImageD Filter(ImageD image, int previewFilterType, int targetLightTypeIndex = -1)
        {
            if (image is Image3D)
            {
                return image;
            }

            Debug.Assert(image != null);

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey, param.ImageBand);
            //Filter(algoImage, previewFilterType, targetLightTypeIndex);
            EdgeDetector edgeDetector = AlgorithmFactory.Instance().CreateEdgeDetector();

            var edgeCheckerParam = (EdgeCheckerParam)param;
            edgeDetector.Param = edgeCheckerParam.EdgeDetectorParam;

            var filteredImage = (Image2D)edgeDetector.GetEdgeImage(algoImage).ToImageD();
            return filteredImage;
        }

        public override Algorithm Clone()
        {
            var edgeChecker = new EdgeChecker();
            edgeChecker.Copy(this);

            return edgeChecker;
        }

        public const string TypeName = "EdgeChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Edge";
        }

        public override List<ResultValue> GetResultValues()
        {
            var edgeCheckerParam = (EdgeCheckerParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("EdgePoint", "", new PointF(0, 0)));
            resultValues.Add(new ResultValue("Gap", "", edgeCheckerParam.MaxOffset, 0, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            var edgeCheckerParam = (EdgeCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            Size cameraImageSize = inspectParam.CameraImageSize;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            var edgeCheckerResult = new AlgorithmResult();
            edgeCheckerResult.ResultRect = probeRegionInFov;

            bool result = true;

            EdgeDetector edgeDetector = AlgorithmFactory.Instance().CreateEdgeDetector();
            if (edgeDetector == null)
            {
                result = false;
            }

            EdgeDetectionResult edgeResult = null;

            if (result == true)
            {
                PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

                edgeDetector.Param = edgeCheckerParam.EdgeDetectorParam;

                float detectorHalfHeight = MathHelper.GetLength(points[0], points[3]) / 2;
                float detectorHalfWidth = MathHelper.GetLength(points[0], points[1]) / 2;

                PointF centerPt = DrawingHelper.CenterPoint(probeRegionInClip);
                var rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight, detectorHalfWidth * 2, detectorHalfHeight * 2, 270);

                edgeResult = edgeDetector.Detect(clipImage, rectangle, debugContext);

                edgeResult.FallingEdgePosition = new PointF(edgeResult.FallingEdgePosition.X + inspectParam.ProbeRegionInFov.X, edgeResult.FallingEdgePosition.Y + inspectParam.ProbeRegionInFov.Y);
                edgeResult.RissingEdgePosition = new PointF(edgeResult.RissingEdgePosition.X + inspectParam.ProbeRegionInFov.X, edgeResult.RissingEdgePosition.Y + inspectParam.ProbeRegionInFov.Y);

                // 위 아래에서 찾은 결과값을 비교하여 0.1mm 내로 들어오는지 확인
                result &= edgeResult.Result && (Math.Abs(edgeResult.RealFallingEdgePosition.Y - edgeResult.RealRissingEdgePosition.Y) <= 0.1);
            }

            var desiredOffset = new PointF();
            float realOffset = 0;

            if (result == true)
            {
                if (cameraCalibration != null && cameraCalibration.IsCalibrated())
                {
                    edgeResult.RealFallingEdgePosition = cameraCalibration.PixelToWorld(edgeResult.FallingEdgePosition);
                    edgeResult.RealRissingEdgePosition = cameraCalibration.PixelToWorld(edgeResult.RissingEdgePosition);

                    desiredOffset = cameraCalibration.PixelToWorld(new PointF(edgeResult.FallingEdgePosition.X + inspectParam.ProbeRegionInFov.X, edgeCheckerParam.DesiredOffset));
                }

                realOffset = desiredOffset.Y - edgeResult.RealFallingEdgePosition.Y;

                edgeResult.YOffset = realOffset;

                edgeCheckerResult.SetResult(Math.Abs(realOffset) <= edgeCheckerParam.MaxOffset);

                edgeCheckerResult.AddResultValue(new ResultValue("Offset", "", edgeCheckerParam.MaxOffset, 0, realOffset));
                edgeCheckerResult.AddResultValue(new ResultValue("IsFound", "", true));
                edgeCheckerResult.AddResultValue(new ResultValue("EdgePoint", "", edgeResult.FallingEdgePosition));
                edgeCheckerResult.AddResultValue(new ResultValue("RealEdgePoint", "", edgeResult.RealFallingEdgePosition));
                edgeCheckerResult.AddResultValue(new ResultValue("DesiredOffset", "", edgeCheckerParam.DesiredOffset));
                edgeCheckerResult.AddResultValue(new ResultValue("RealDesiredOffset", "", desiredOffset.Y));
            }
            else
            {
                edgeCheckerResult.SetResult(false);

                edgeCheckerResult.AddResultValue(new ResultValue("Offset", "", edgeCheckerParam.MaxOffset, 0, 0));
                edgeCheckerResult.AddResultValue(new ResultValue("IsFound", "", false));
                edgeCheckerResult.AddResultValue(new ResultValue("EdgePoint", "", new PointF(0, 0)));
                edgeCheckerResult.AddResultValue(new ResultValue("RealEdgePoint", "", new PointF(0, 0)));
                edgeCheckerResult.AddResultValue(new ResultValue("DesiredOffset", "", edgeCheckerParam.DesiredOffset));
                edgeCheckerResult.AddResultValue(new ResultValue("RealDesiredOffset", "", desiredOffset.Y));
            }

            int borderWidth = cameraImageSize.Width - 18;
            int borderHeight = cameraImageSize.Height - 18;

            ResultValue resultValue = edgeCheckerResult.GetResultValue("IsFound");

            if (Convert.ToBoolean(resultValue.Value) == true)
            {
                ResultValue edgePointValue = edgeCheckerResult.GetResultValue("EdgePoint");
                var edgePoint = (PointF)edgePointValue.Value;
                var edgePointPen = new Pen(Color.Red, 3.0F);

                var edgePointFigure = new CrossFigure(edgePoint, 40.0f, edgePointPen);
                edgeCheckerResult.ResultFigures.AddFigure(edgePointFigure);

                ResultValue offsetValue = edgeCheckerResult.GetResultValue("Offset");
                float offset = Convert.ToSingle(offsetValue.Value);
                string offsetstr = string.Format("({0:0.000})", offset);

                var offsetFigure = new TextFigure(offsetstr, new Point(80 + offsetstr.Length * 15, borderHeight - 140), new Font(FontFamily.GenericSansSerif, 80), Color.Black);
                edgeCheckerResult.ResultFigures.AddFigure(offsetFigure);
            }

            return edgeCheckerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {

        }
    }
}
