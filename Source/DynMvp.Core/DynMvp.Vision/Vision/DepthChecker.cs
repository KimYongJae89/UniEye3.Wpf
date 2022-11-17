using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class DepthCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(DepthChecker))
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
                 value is BrightnessChecker)
            {
                var depthChecker = (DepthChecker)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public enum DepthCheckType
    {
        None, HeightAverage, HeightMax, HeightMin, Volume
    }

    [TypeConverterAttribute(typeof(DepthCheckerConverter))]
    public class DepthCheckerParam : AlgorithmParam
    {
        public DepthCheckType Type { get; set; }
        public float LowerValue { get; set; } = 100;
        public float UpperValue { get; set; } = 200;
        public override AlgorithmParam Clone()
        {
            var param = new DepthCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            Type = (DepthCheckType)Enum.Parse(typeof(DepthCheckType), XmlHelper.GetValue(algorithmElement, "DepthCheckType", "HeightAverage"));
            LowerValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "LowerValue", "100"));
            UpperValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "UpperValue", "200"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "DepthCheckType", Type.ToString());
            XmlHelper.SetValue(algorithmElement, "LowerValue", LowerValue.ToString());
            XmlHelper.SetValue(algorithmElement, "UpperValue", UpperValue.ToString());
        }
    }

    public class DepthChecker : Algorithm
    {
        public DepthChecker()
        {
            param = new DepthCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return true;
        }

        public override Algorithm Clone()
        {
            var depthChecker = new DepthChecker();
            depthChecker.Copy(this);

            return depthChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var depthChecker = (DepthChecker)algorithm;

            param = (DepthCheckerParam)depthChecker.Param.Clone();
        }

        public const string TypeName = "DepthChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Depth";
        }

        public override List<ResultValue> GetResultValues()
        {
            var depthCheckerParam = (DepthCheckerParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Value", "", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, 0));
            resultValues.Add(new ResultValue("ValueType", depthCheckerParam.Type.ToString()));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Depth);
            Filter(clipImage);

            var depthCheckerParam = (DepthCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);
            float resultValue = 0;

            switch (depthCheckerParam.Type)
            {
                case DepthCheckType.None:
                    break;
                default:
                case DepthCheckType.Volume:
                case DepthCheckType.HeightAverage:
                    if (probeRegionInFov.Angle != 0)
                    {
                        ImageD rotatedMask = ImageHelper.GetRotateMask(clipImage.Width, clipImage.Height, probeRegionInFov);
                        AlgoImage rotatedAlgoMask = ImageBuilder.Build(BinaryCounter.TypeName, rotatedMask, ImageType.Grey, ImageBandType.Luminance);
                        resultValue = imageProcessing.GetGreyAverage(clipImage, rotatedAlgoMask);
                    }
                    else
                    {
                        resultValue = imageProcessing.GetGreyAverage(clipImage);
                    }

                    if (depthCheckerParam.Type == DepthCheckType.Volume)
                    {
                        var pelSize = new SizeF(inspectParam.CameraCalibration.PixelToWorld(clipImage.Size.ToPointF()));

                        resultValue = resultValue * pelSize.Width * pelSize.Height;
                    }
                    break;
                case DepthCheckType.HeightMax:
                    resultValue = inspectParam.InspectImage3d.Data.Max();
                    break;
                case DepthCheckType.HeightMin:
                    resultValue = inspectParam.InspectImage3d.Data.Min();
                    break;
            }

            var algorithmResult = new AlgorithmResult();
            algorithmResult.AddResultValue(new ResultValue("ValueType", depthCheckerParam.Type.ToString()));

            if (depthCheckerParam.Type == DepthCheckType.None)
            {
                algorithmResult.AddResultValue(new ResultValue("Value", "", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, 0));
                return algorithmResult;
            }

            algorithmResult.ResultRect = probeRegionInFov;
            algorithmResult.SetResult((resultValue >= depthCheckerParam.LowerValue) && (resultValue <= depthCheckerParam.UpperValue));

            algorithmResult.AddResultValue(new ResultValue("Value", "", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, resultValue));

            return algorithmResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {

        }
    }
}
