using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class CircleCheckerParam : AlgorithmParam
    {
        public CircleDetectorParam CircleDetectorParam { get; set; } = new CircleDetectorParam();
        public bool UseImageCenter { get; set; }
        public bool ShowOffset { get; set; }

        public CircleCheckerParam()
        {

        }

        public override AlgorithmParam Clone()
        {
            var param = new CircleCheckerParam();
            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (CircleCheckerParam)srcAlgorithmParam;

            UseImageCenter = param.UseImageCenter;
            ShowOffset = param.UseImageCenter;
            CircleDetectorParam.Copy(param.CircleDetectorParam);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            CircleDetectorParam.LoadParam(algorithmElement);

            UseImageCenter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseImageCenter", "false"));
            ShowOffset = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "ShowOffset", "false"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            CircleDetectorParam.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseImageCenter", UseImageCenter.ToString());
            XmlHelper.SetValue(algorithmElement, "ShowOffset", ShowOffset.ToString());
        }
    }

    public class CircleChecker : Algorithm
    {
        public CircleChecker()
        {
            param = new CircleCheckerParam();
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            PointF centerPt = DrawingHelper.CenterPoint(probeRect);

            CircleDetectorParam circleDetectorParam = ((CircleCheckerParam)param).CircleDetectorParam;

            var innerPen = new Pen(Color.Tomato, 3.0F);
            var outterPen = new Pen(Color.BlueViolet, 3.0F);
            innerPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            outterPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            var innerCircleRect = new RectangleF(centerPt.X - circleDetectorParam.InnerRadius, centerPt.Y - circleDetectorParam.InnerRadius, circleDetectorParam.InnerRadius * 2, circleDetectorParam.InnerRadius * 2);
            tempFigures.AddFigure(new EllipseFigure(innerCircleRect, innerPen));

            var outterCircleRect = new RectangleF(centerPt.X - circleDetectorParam.OutterRadius, centerPt.Y - circleDetectorParam.OutterRadius, circleDetectorParam.OutterRadius * 2, circleDetectorParam.OutterRadius * 2);
            tempFigures.AddFigure(new EllipseFigure(outterCircleRect, outterPen));
        }

        public override Algorithm Clone()
        {
            var circleChecker = new CircleChecker();
            circleChecker.Copy(this);

            return circleChecker;
        }

        public const string TypeName = "CircleChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Circle";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            CircleDetectorParam circleDetectorParam = ((CircleCheckerParam)param).CircleDetectorParam;

            PointF centerPt = DrawingHelper.CenterPoint(inspRegion);
            newInspRegion = new RotatedRect(centerPt.X - circleDetectorParam.OutterRadius, centerPt.Y - circleDetectorParam.OutterRadius, circleDetectorParam.OutterRadius * 2, circleDetectorParam.OutterRadius * 2, 0);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("CenterX", "", 0));
            resultValues.Add(new ResultValue("CenterY", "", 0));
            resultValues.Add(new ResultValue("CenterPos", "", new Point(0, 0)));
            resultValues.Add(new ResultValue("Radius", "", 0));
            resultValues.Add(new ResultValue("Offset X", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Offset Y", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Real Offset X", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Real Offset Y", "", 0, 0, 0));
            resultValues.Add(new ResultValue("CenterPos", "", new Point(0, 0)));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            Size cameraImageSize = inspectParam.CameraImageSize;

            var circleCheckerResult = new AlgorithmResult();
            circleCheckerResult.ResultRect = probeRegionInFov;

            CircleDetector circleDetector = AlgorithmFactory.Instance().CreateCircleDetector();
            if (circleDetector == null)
            {
                return circleCheckerResult;
            }

            var circleCheckerParam = (CircleCheckerParam)param;
            CircleDetectorParam circleDetectorParam = ((CircleCheckerParam)param).CircleDetectorParam;

            circleDetector.Param = circleDetectorParam;
            circleDetector.Param.CenterPosition = new PointF(clipImage.Width / 2, clipImage.Height / 2);

            CircleEq circleFound = circleDetector.Detect(clipImage, debugContext);
            if (circleFound != null && circleFound.IsValid())
            {
                circleCheckerResult.SetResult(true);

                circleFound.Center = new PointF(circleFound.Center.X + inspectRegionInFov.X, circleFound.Center.Y + inspectRegionInFov.Y);

                circleCheckerResult.AddResultValue(new ResultValue("Center X", "", 0, 0, circleFound.Center.X));
                circleCheckerResult.AddResultValue(new ResultValue("Center Y", "", 0, 0, circleFound.Center.Y));
                circleCheckerResult.AddResultValue(new ResultValue("CenterPos", "", circleFound.Center));
                circleCheckerResult.AddResultValue(new ResultValue("Radius", "", 0, 0, circleFound.Radius));
                circleCheckerResult.AddResultValue(new ResultValue("CenterPos", "", circleFound));

                PointF centerPt = DrawingHelper.CenterPoint(probeRegionInFov);
                if (circleCheckerParam.UseImageCenter)
                {
                    centerPt = new PointF(cameraImageSize.Width / 2, cameraImageSize.Height / 2);
                }

                var offset = PointF.Subtract(circleFound.Center, new SizeF(centerPt));
                offset.Y = -offset.Y;

                circleCheckerResult.AddResultValue(new ResultValue("Offset X", "", 0, 0, offset.X));
                circleCheckerResult.AddResultValue(new ResultValue("Offset Y", "", 0, 0, offset.Y));

                if (cameraCalibration != null)
                {
                    PointF circleReal = cameraCalibration.PixelToWorld(centerPt);
                    PointF circleFoundReal = cameraCalibration.PixelToWorld(circleFound.Center);

                    offset = PointF.Subtract(circleFoundReal, new SizeF(circleReal));
                    offset.Y = -offset.Y;

                    circleCheckerResult.AddResultValue(new ResultValue("Real Offset X", "", 0, 0, offset.X));
                    circleCheckerResult.AddResultValue(new ResultValue("Real Offset Y", "", 0, 0, offset.Y));
                }
                else
                {
                    circleCheckerResult.AddResultValue(new ResultValue("Real Offset X", "", 0, 0, 0));
                    circleCheckerResult.AddResultValue(new ResultValue("Real Offset Y", "", 0, 0, 0));
                }

                var defectRect = new RectangleF(circleFound.Center.X - circleFound.Radius, circleFound.Center.Y - circleFound.Radius, circleFound.Radius * 2, circleFound.Radius * 2);
                circleCheckerResult.ResultFigures.AddFigure(new EllipseFigure(defectRect, new Pen(Color.Green, 4)));

                if (circleCheckerParam.ShowOffset)
                {
                    circleCheckerResult.ResultFigures.AddFigure(new LineFigure(circleFound.Center, centerPt, new Pen(Color.OrangeRed, 4)));
                    circleCheckerResult.ResultFigures.AddFigure(new CrossFigure(circleFound.Center, 6, new Pen(Color.OrangeRed)));
                    circleCheckerResult.ResultFigures.AddFigure(new TextFigure(string.Format("({0:0.000} ,{1:0.000})", offset.X, offset.Y), Point.Truncate(circleFound.Center), new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold), Color.Green));
                }
            }
            else
            {
                circleCheckerResult.SetResult(false);

                circleCheckerResult.AddResultValue(new ResultValue("CenterX", "", 0));
                circleCheckerResult.AddResultValue(new ResultValue("CenterY", "", 0));
                circleCheckerResult.AddResultValue(new ResultValue("Radius", "", 0));

                circleCheckerResult.AddResultValue(new ResultValue("Offset X", "", 0));
                circleCheckerResult.AddResultValue(new ResultValue("Offset Y", "", 0));
                circleCheckerResult.AddResultValue(new ResultValue("Real Offset X", "", 0));
                circleCheckerResult.AddResultValue(new ResultValue("Real Offset Y", "", 0));
            }

            return circleCheckerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
