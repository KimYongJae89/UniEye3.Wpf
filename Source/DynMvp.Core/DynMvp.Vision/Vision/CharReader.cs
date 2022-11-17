using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum CharactorPolarity
    {
        DarkOnLight, LightOnDark
    }

    public class CharFont
    {
        public ImageD Image { get; set; }
        public string Character { get; set; }

        public object AlgorithmCharObject { get; set; }

        public CharFont(ImageD image, string character, object algorithmCharObject)
        {
            Image = image;
            Character = character;
            AlgorithmCharObject = algorithmCharObject;
        }
    }

    public class CharReaderParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CharReaderParam))
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
                 value is CharReaderParam)
            {
                var charReaderParam = (CharReaderParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(CharReaderParamConverter))]
    public class CharReaderParam : AlgorithmParam
    {
        public int CharacterMaxHeight { get; set; }
        public int CharacterMinHeight { get; set; }
        public int CharacterMaxWidth { get; set; }
        public int CharacterMinWidth { get; set; }
        public CharactorPolarity CharactorPolarity { get; set; }
        public int XOverlapRatio { get; set; }
        public List<int> ThresholdList { get; set; } = new List<int>();
        public float MinScore { get; set; }
        public int DesiredNumCharacter { get; set; }
        public string DesiredString { get; set; } = "";
        public string FontFileName { get; set; } = "";

        public override AlgorithmParam Clone()
        {
            var param = new CharReaderParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (CharReaderParam)srcAlgorithmParam;

            CharacterMaxHeight = param.CharacterMaxHeight;
            CharacterMinHeight = param.CharacterMinHeight;
            CharacterMaxWidth = param.CharacterMaxWidth;
            CharacterMinWidth = param.CharacterMinWidth;
            CharactorPolarity = param.CharactorPolarity;
            DesiredNumCharacter = param.DesiredNumCharacter;
            DesiredString = param.DesiredString;
            ThresholdList = param.ThresholdList;
            FontFileName = param.FontFileName;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            CharacterMaxHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMaxHeight", "0"));
            CharacterMinHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMinHeight", "0"));
            CharacterMaxWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMaxWidth", "0"));
            CharacterMinWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMinWidth", "0"));
            XOverlapRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "XOverlapRatio", "0"));
            CharactorPolarity = (CharactorPolarity)Enum.Parse(typeof(CharactorPolarity), XmlHelper.GetValue(algorithmElement, "CharactorPolarity", "DarkOnLight"));
            DesiredNumCharacter = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DesiredNumCharacter", "0"));
            DesiredString = XmlHelper.GetValue(algorithmElement, "DesiredString", "");
            FontFileName = XmlHelper.GetValue(algorithmElement, "FontFileName", "");

            foreach (XmlElement thresholdElement in algorithmElement)
            {
                if (thresholdElement.Name == "Threshold")
                {
                    int threshold = Convert.ToInt32(thresholdElement.InnerText);
                    ThresholdList.Add(threshold);
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CharacterMaxHeight", CharacterMaxHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMinHeight", CharacterMinHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMaxWidth", CharacterMaxWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMinWidth", CharacterMinWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "XOverlapRatio", XOverlapRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "CharactorPolarity", CharactorPolarity.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredNumCharacter", DesiredNumCharacter.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredString", DesiredString);
            XmlHelper.SetValue(algorithmElement, "FontFileName", FontFileName);

            foreach (int threshold in ThresholdList)
            {
                XmlHelper.SetValue(algorithmElement, "Threshold", threshold.ToString());
            }
        }
    }

    public class CharReaderConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CharReader))
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
                 value is CharReader)
            {
                var charReader = (CharReader)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(CharReaderConverter))]
    public abstract class CharReader : Algorithm
    {
        public ImagingLibrary ImagingLibrary { get; set; } = ImagingLibrary.MatroxMIL;


        public CharReader()
        {
            param = new CharReaderParam();
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var charReader = (CharReader)algorithm;

            param.Copy(charReader.Param);
        }

        public const string TypeName = "CharReader";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Char";
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            var charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == true)
            {
                if (ImagingLibrary != ImagingLibrary.MatroxMIL)
                {
                    Train(charReaderParam.FontFileName);
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
            XmlHelper.SetValue(algorithmElement, "ImagingLibrary", ImagingLibrary.ToString());
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            newInspRegion.Inflate(10, 10);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var charReaderParam = (CharReaderParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("String Read", charReaderParam.DesiredString));

            return resultValues;
        }

        public AlgorithmResult Extract(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, int threshold, DebugContext debugContext)
        {
            AlgoImage clipAlgoImage = ImageBuilder.Build(GetAlgorithmType(), clipImage, ImageType.Grey, param.ImageBand);
#if DEBUG
            //clipAlgoImage.Save("Extract_clipAlgoImage.bmp", debugContext);
#endif
            Filter(clipAlgoImage);

            CharReaderResult charReaderResult = null;

            var charReaderParam = (CharReaderParam)param;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            if (Trained == true)
            {
                charReaderResult = Extract(clipAlgoImage, probeRegionInClip, threshold, debugContext);
            }
            else
            {
                charReaderResult = new CharReaderResult();
                charReaderResult.ErrorMessage = "Font is not trained";
                charReaderResult.SetResult(false);

                charReaderResult.AddResultValue(new ResultValue("String Read", charReaderParam.DesiredString, charReaderResult.ErrorMessage));
            }
            charReaderResult.ResultRect = probeRegionInFov;
            charReaderResult.OffsetCharPosition(clipRegionInFov.X, clipRegionInFov.Y);

            return charReaderResult;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage, 0);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            CharReaderResult charReaderResult = null;

            var charReaderParam = (CharReaderParam)param;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            if (Trained == true)
            {
                charReaderResult = Read(clipImage, probeRegionInClip, debugContext);
            }
            else
            {
                charReaderResult = new CharReaderResult();
                charReaderResult.ErrorMessage = "Font is not trained";
                charReaderResult.SetResult(false);

                charReaderResult.AddResultValue(new ResultValue("String Read", charReaderParam.DesiredString, charReaderResult.ErrorMessage));
            }

            charReaderResult.ResultRect = probeRegionInFov;

            if (charReaderResult.IsGood() == false)
            {
                charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                string pathName = Path.Combine(BaseConfig.Instance().TempPath, "OcrFailed");
                var ocrDebugContext = new DebugContext(true, pathName);

                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                clipImage.Save(timeStamp + ".bmp", ocrDebugContext);

                var clipBmpImage = inspectParam.InspectImageList[0].ToBitmap();
                Bitmap probeImage = ImageHelper.ClipImage(clipBmpImage, probeRegionInClip);
                ImageHelper.SaveImage(probeImage, Path.Combine(pathName, timeStamp + "_0.bmp"));

                clipBmpImage.Dispose();
                probeImage.Dispose();
            }

            charReaderResult.OffsetCharPosition(inspectRegionInFov.X, inspectRegionInFov.Y);

            return charReaderResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }

        public abstract bool Trained { get; }
        public abstract void Train(string fontFileName);
        public abstract CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, DebugContext debugContext);
        public abstract CharReaderResult Extract(AlgoImage algoImage, RectangleF characterRegion, int threshold, DebugContext debugContext);

        public abstract void AutoSegmentation(AlgoImage algoImage, RotatedRect rotatedRect, string desiredString);
        public abstract void AddCharactor(CharPosition charPosition, int charactorCode);
        public abstract void AddCharactor(AlgoImage charImage, string charactorCode);
        public abstract void SaveFontFile(string fontFileName);

        public abstract List<CharFont> GetFontList();
        public abstract void RemoveFont(CharFont charFont);

        public abstract int CalibrateFont(AlgoImage charImage, string calibrationString);
    }

    public class CharPosition
    {
        public int CharCode { get; set; }
        public int ResultType { get; set; } = 0;
        public RotatedRect Position { get; set; }

        public CharPosition(int charCode, RotatedRect position)
        {
            CharCode = charCode;
            Position = position;
        }

        public void Offset(float offsetX, float offsetY)
        {
            Position.Offset(offsetX, offsetY);
        }
    }

    public class CharReaderResult : AlgorithmResult
    {
        public string DesiredString { get; set; }
        public string StringRead { get; set; }
        public List<float> ScoreList { get; set; } = new List<float>();
        public string ErrorMessage { get; set; } = "";
        public List<string> TrialResult { get; set; } = new List<string>();
        public List<CharPosition> CharPositionList { get; set; } = new List<CharPosition>();

        public CharReaderResult()
        {

        }

        public override string ToString()
        {
            string msg = string.Format("Chracter Result : {0} ", StringRead);
            if (ErrorMessage != "")
            {
                msg += Environment.NewLine + "    Error Message : " + ErrorMessage;
            }

            foreach (string trialStr in TrialResult)
            {
                msg += Environment.NewLine + trialStr;
            }

            msg += Environment.NewLine + string.Format("Desired String : {0} ", DesiredString);

            for (int index = 0; index < ScoreList.Count; index++)
            {
                msg += Environment.NewLine + string.Format("Char : {0} - Score : {1} ", StringRead[index], ScoreList[index]);
            }

            return msg;
        }

        public void AddCharPosition(CharPosition charPosition)
        {
            CharPositionList.Add(charPosition);
        }

        public void OffsetCharPosition(float offsetX, float offsetY)
        {
            foreach (CharPosition charPosition in CharPositionList)
            {
                charPosition.Offset(offsetX, offsetY);
            }
        }

        public override void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            foreach (CharPosition charPosition in CharPositionList)
            {
                RotatedRect position = charPosition.Position;
                if (charPosition.ResultType == 1)
                {
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Lime)));
                }
                else if (charPosition.ResultType == -1)
                {
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Red)));
                }
                else if (charPosition.ResultType == 0)
                {
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Yellow)));
                }
            }
        }
    }
}
