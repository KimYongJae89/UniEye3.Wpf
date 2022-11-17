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
    public class BrightnessCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(BrightnessChecker))
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
                var brightnessChecker = (BrightnessChecker)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }


    [TypeConverterAttribute(typeof(BrightnessCheckerConverter))]
    public class BrightnessCheckerParam : AlgorithmParam
    {
        public int LowerValue { get; set; } = 100;
        public int UpperValue { get; set; } = 200;

        public override AlgorithmParam Clone()
        {
            var param = new BrightnessCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BrightnessCheckerParam)srcAlgorithmParam;

            LowerValue = param.LowerValue;
            UpperValue = param.UpperValue;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            LowerValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "LowerValue", "100"));
            UpperValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "UpperValue", "200"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "LowerValue", LowerValue.ToString());
            XmlHelper.SetValue(algorithmElement, "UpperValue", UpperValue.ToString());
        }
    }

    [TypeConverterAttribute(typeof(BrightnessCheckerConverter))]
    public class BrightnessChecker : Algorithm
    {
        public BrightnessChecker()
        {
            param = new BrightnessCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            var brightnessChecker = new BrightnessChecker();
            brightnessChecker.Copy(this);

            return brightnessChecker;
        }

        public const string TypeName = "BrightnessChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Bright";
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Brightness", "", ((BrightnessCheckerParam)param).UpperValue, ((BrightnessCheckerParam)param).LowerValue, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            ImageType imageType = (inspectParam.InspectImageList[0] is Image3D ? ImageType.Depth : ImageType.Grey);

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], imageType, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);

            float average = imageProcessing.GetGreyAverage(clipImage);

            var brightnessCheckerResult = new AlgorithmResult();
            brightnessCheckerResult.ResultRect = probeRegionInFov;

            bool result = (average >= ((BrightnessCheckerParam)param).LowerValue) && (average <= ((BrightnessCheckerParam)param).UpperValue);
            brightnessCheckerResult.SetResult(result);

            brightnessCheckerResult.AddResultValue(new ResultValue("Brightness", "", ((BrightnessCheckerParam)param).UpperValue, ((BrightnessCheckerParam)param).LowerValue, average));

            return brightnessCheckerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
