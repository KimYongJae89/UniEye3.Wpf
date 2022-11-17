using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.Planbss;
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
    public class RectCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(RectChecker))
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
                 value is RectChecker)
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class RectCheckerParam : AlgorithmParam
    {
        public EdgeType EdgeType { get; set; } = 0;
        public int EdgeThickWidth { get; set; } = 1;
        public int EdgeThickHeight { get; set; } = 5;
        public int EdgeDistance { get; set; } = 5;
        public int GrayValue { get; set; } = 20;
        public int ProjectionHeight { get; set; } = 3;
        public int PassRate { get; set; } = 30;
        public CardinalPoint CardinalPoint { get; set; } = CardinalPoint.NorthWest;
        public bool OutToIn { get; set; } = true;
        public int SearchRange { get; set; } = 20;
        public int SearchLength { get; set; } = 10;
        public ConvexShape ConvexShape { get; set; } = ConvexShape.None;

        public RectCheckerParam()
        {
        }

        public override AlgorithmParam Clone()
        {
            var param = new RectCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (RectCheckerParam)srcAlgorithmParam;

            EdgeType = param.EdgeType;
            EdgeThickWidth = param.EdgeThickWidth;
            EdgeThickHeight = param.EdgeThickHeight;
            GrayValue = param.GrayValue;
            ProjectionHeight = param.ProjectionHeight;
            PassRate = param.PassRate;
            OutToIn = param.OutToIn;
            SearchRange = param.SearchRange;
            SearchLength = param.SearchLength;
            ConvexShape = param.ConvexShape;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            EdgeType = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "Any"));
            EdgeThickWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThickWidth", "1"));
            EdgeThickHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThickHeight", "5"));
            GrayValue = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GrayValue", "20"));
            ProjectionHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ProjectionHeight", "3"));
            PassRate = Convert.ToInt32(XmlHelper.GetValue(paramElement, "PassRate", "30"));
            OutToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "OutToIn", "True"));
            SearchRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRange", "20"));
            SearchLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Length", "70"));
            ConvexShape = (ConvexShape)Enum.Parse(typeof(ConvexShape), XmlHelper.GetValue(paramElement, "ConvexShape", "None"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "SearchRange", SearchRange.ToString());
            XmlHelper.SetValue(paramElement, "EdgeType", EdgeType.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThickWidth", EdgeThickWidth.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThickHeight", EdgeThickHeight.ToString());
            XmlHelper.SetValue(paramElement, "GrayValue", GrayValue.ToString());
            XmlHelper.SetValue(paramElement, "projectionHeightf", ProjectionHeight.ToString());
            XmlHelper.SetValue(paramElement, "PassRate", PassRate.ToString());
            XmlHelper.SetValue(paramElement, "CardinalPoint", CardinalPoint.ToString());
            XmlHelper.SetValue(paramElement, "OutToIn", OutToIn.ToString());
            XmlHelper.SetValue(paramElement, "SearchLength", SearchLength.ToString());
            XmlHelper.SetValue(paramElement, "ConvexShape", ConvexShape.ToString());
        }
    }

    [TypeConverterAttribute(typeof(RectCheckerConverter))]
    public class RectChecker : Algorithm
    {
        public RectChecker()
        {
            param = new RectCheckerParam();
        }

        public override Algorithm Clone()
        {
            var rectChecker = new RectChecker();
            rectChecker.Copy(this);

            return rectChecker;
        }

        public const string TypeName = "RectChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Rect";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            newInspRegion.Inflate(((RectCheckerParam)param).SearchLength + 5, ((RectCheckerParam)param).SearchLength + 5);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Angle", "", 0, 360, 0));
            resultValues.Add(new ResultValue("CenterPos", "", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("LeftTop", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("RightTop", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("RightBottom", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("LeftBottom", new Point(0, 0)));

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

            var algorithmResult = new AlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov;

            bool bAuto = false;  // Graphic use or ignore

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Vision Initialize
            ///////////////////////////////////////////////////////////////////////////////////////////  
            var vcFpcb = new VCFpcb();
            var vcFpcbParam = new VCFpcbParam();
            HelpDraw helpDraw = bAuto ? null : new HelpDraw();

            VCLens.gsUser = EUSER.DEVELOPER;
            vcFpcbParam.gsMatch.GsUse = false;
            vcFpcbParam.gsQuadrangle.GsTcRoi = Rectangle.Truncate(probeRegionInClip);

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Connect Parameter
            ///////////////////////////////////////////////////////////////////////////////////////////  
            switch (((RectCheckerParam)param).EdgeType)
            {
                case EdgeType.DarkToLight: vcFpcbParam.gsQuadrangle.GsWtoB = 2; break;
                case EdgeType.LightToDark: vcFpcbParam.gsQuadrangle.GsWtoB = 1; break;
                default:
                    vcFpcbParam.gsQuadrangle.GsWtoB = 0;
                    break;
            }

            vcFpcbParam.gsQuadrangle.GsThickW = ((RectCheckerParam)param).EdgeThickWidth;     // 1
            vcFpcbParam.gsQuadrangle.GsThickH = ((RectCheckerParam)param).EdgeThickHeight;    // 5
            vcFpcbParam.gsQuadrangle.GsDistance = 5;                        // 5
            vcFpcbParam.gsQuadrangle.GsGv = ((RectCheckerParam)param).GrayValue;          // 20
            vcFpcbParam.gsQuadrangle.GsScan = ((RectCheckerParam)param).ProjectionHeight;   // 3
            vcFpcbParam.gsQuadrangle.SetRateIn(((RectCheckerParam)param).PassRate);             // 30
            vcFpcbParam.gsQuadrangle.GsCardinal = ((RectCheckerParam)param).CardinalPoint;      // NE
            vcFpcbParam.gsQuadrangle.GsOtuToIn = ((RectCheckerParam)param).OutToIn;            // true
            vcFpcbParam.gsQuadrangle.GsRange = ((RectCheckerParam)param).SearchRange;        // 20
            vcFpcbParam.gsQuadrangle.GsLength = ((RectCheckerParam)param).SearchLength;       // 70
            vcFpcbParam.gsQuadrangle.GsConvex = ((RectCheckerParam)param).ConvexShape;        // None

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Vision Inspection and Result
            ///////////////////////////////////////////////////////////////////////////////////////////            
            int vResult = vcFpcb.Inspection(clipImage, vcFpcbParam, debugContext, helpDraw);

            int offsetX = (int)inspectRegionInFov.X;
            int offsetY = (int)inspectRegionInFov.Y;
            double outAngle = vcFpcbParam.gsQuadrangle.GsOutAngle;
            var outCenter = new PointF((float)vcFpcbParam.gsQuadrangle.GsOutPt.X + offsetX,
                                          (float)vcFpcbParam.gsQuadrangle.GsOutPt.Y + offsetY);

            algorithmResult.SetResult(vResult == 1);
            if (algorithmResult.IsNG())
            {
                algorithmResult.AddResultValue(new ResultValue("Defect Message", "", vcFpcb.gsError.ToString()));
            }

            algorithmResult.AddResultValue(new ResultValue("CenterPos", "", outCenter));
            algorithmResult.AddResultValue(new ResultValue("Angle", "", outAngle));

            CopyGraphics(algorithmResult, helpDraw, offsetX, offsetY);
            helpDraw.Clear();

            return algorithmResult;
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
}
