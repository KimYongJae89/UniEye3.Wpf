using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class CalibrationCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                            System.Type destinationType)
        {
            if (destinationType == typeof(CalibrationChecker))
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
                    value is CalibrationChecker)
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class CalibrationCheckerParam : AlgorithmParam
    {
        public int NumRow { get; set; } = 3;
        public int NumCol { get; set; } = 3;
        public float RowSpace { get; set; } = 1.0f;
        public float ColSpace { get; set; } = 1.0f;

        public override AlgorithmParam Clone()
        {
            var param = new CalibrationCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (CalibrationCheckerParam)srcAlgorithmParam;

            NumRow = param.NumRow;
            NumCol = param.NumCol;
            RowSpace = param.RowSpace;
            ColSpace = param.ColSpace;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            NumRow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumRow", "3"));
            NumCol = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumCol", "3"));
            RowSpace = Convert.ToSingle(XmlHelper.GetValue(paramElement, "RowSpace", "1.0"));
            ColSpace = Convert.ToSingle(XmlHelper.GetValue(paramElement, "ColSpace", "1.0"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "NumRow", NumRow.ToString());
            XmlHelper.SetValue(paramElement, "NumCol", NumCol.ToString());
            XmlHelper.SetValue(paramElement, "RowSpace", RowSpace.ToString());
            XmlHelper.SetValue(paramElement, "ColSpace", ColSpace.ToString());
        }
    }

    public interface ICalibration
    {

    }

    public class CalibrationChecker : Algorithm, ICalibration
    {
        public CalibrationChecker()
        {
            param = new CalibrationCheckerParam();
        }

        public override Algorithm Clone()
        {
            var calibrationChecker = new CalibrationChecker();
            calibrationChecker.Copy(this);

            return calibrationChecker;
        }

        public const string TypeName = "CalibrationChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Calibration";
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Calibration", null));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            var algorithmResult = new AlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov;

            Calibration calibration = AlgorithmFactory.Instance().CreateCalibration();

            calibration.CalibrationType = CalibrationType.Grid;

            var calibCheckerParam = (CalibrationCheckerParam)Param;

            CalibrationResult result = calibration.Calibrate(inspectParam.InspectImageList[0], CalibrationGridType.Dots,
                calibCheckerParam.NumRow, calibCheckerParam.NumCol, calibCheckerParam.RowSpace, calibCheckerParam.ColSpace);

            algorithmResult.SetResult(result.IsGood);
            if (result.IsGood == false)
            {
                algorithmResult.BriefMessage = "Fail to calibration.";
            }
            else
            {
                algorithmResult.AddResultValue(new ResultValue("ScaleX", "", calibration.PelSize.Width));
                algorithmResult.AddResultValue(new ResultValue("ScaleY", "", calibration.PelSize.Height));
                algorithmResult.AddResultValue(new ResultValue("Calibration", "", calibration));
            }

            return algorithmResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
