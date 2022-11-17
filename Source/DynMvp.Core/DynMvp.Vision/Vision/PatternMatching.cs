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
    public class PatternMatchingParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PatternMatchingParam))
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
                 value is PatternMatchingParam)
            {
                var patternMatchingParam = (PatternMatchingParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(PatternMatchingParamConverter))]
    public class PatternMatchingParam : AlgorithmParam
    {
        public float MinAngle { get; set; }
        public float MaxAngle { get; set; }
        public float MinScale { get; set; }
        public float MaxScale { get; set; }
        public bool IgnorePolarity { get; set; }
        public int SearchRangeWidth { get; set; }
        public int SearchRangeHeight { get; set; }
        public int ToleranceX { get; set; }
        public int ToleranceY { get; set; }
        public int MatchScore { get; set; }
        public int NumToFind { get; set; }
        public bool UseImageCenter { get; set; }
        public bool UseWholeImage { get; set; }
        public bool UseAllMatching { get; set; }
        public string RecogString { get; set; }
        //[BrowsableAttribute(false)]
        public List<Pattern> PatternList { get; set; } = new List<Pattern>();

        public PatternMatchingParam()
        {
            MinAngle = -30.0f;
            MaxAngle = 30.0f;
            MinScale = 0.95f;
            MaxScale = 1.05f;
            IgnorePolarity = false;
            MatchScore = 80;
            SearchRangeWidth = 100;
            SearchRangeHeight = 100;
            ToleranceX = 0;
            ToleranceY = 0;
            NumToFind = 1;
            RecogString = "";
            UseWholeImage = false;
            UseAllMatching = false;
            UseImageCenter = false;
        }

        public void Clear()
        {
            foreach (Pattern pattern in PatternList)
            {
                pattern.Dispose();
            }
        }

        public override AlgorithmParam Clone()
        {
            var param = new PatternMatchingParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (PatternMatchingParam)srcAlgorithmParam;

            MinAngle = param.MinAngle;
            MaxAngle = param.MaxAngle;

            MinScale = param.MinScale;
            MaxScale = param.MaxScale;

            IgnorePolarity = param.IgnorePolarity;

            MatchScore = param.MatchScore;
            NumToFind = param.NumToFind;

            SearchRangeWidth = param.SearchRangeWidth;
            SearchRangeHeight = param.SearchRangeHeight;

            RecogString = param.RecogString;

            UseWholeImage = param.UseWholeImage;
            UseAllMatching = param.UseAllMatching;
            UseImageCenter = param.UseImageCenter;

            if (PatternList.Count != 0)
            {
                PatternList.Clear();
            }

            foreach (Pattern pattern in param.PatternList)
            {
                //AddPattern(pattern.PatternImage);
                PatternList.Add(pattern);
            }


            //patternList.Add(pattern.Clone());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            MinAngle = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinAngle", "-30"));
            MaxAngle = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxAngle", "30"));

            MinScale = (float)Convert.ToDouble(XmlHelper.GetValue(algorithmElement, "MinScale", "0.95"));
            MaxScale = (float)Convert.ToDouble(XmlHelper.GetValue(algorithmElement, "MaxScale", "1.05"));

            SearchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "30"));
            SearchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "30"));

            ToleranceX = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ToleranceX", "0"));
            ToleranceY = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ToleranceY", "0"));

            MatchScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MatchScore", "80"));
            NumToFind = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "NumToFind", "100"));

            IgnorePolarity = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "IgnorePolarity", "False"));
            RecogString = XmlHelper.GetValue(algorithmElement, "RecogString", "");

            UseWholeImage = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "WholeImage", "False"));
            UseAllMatching = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "AllMatching", "False"));
            UseImageCenter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseImageCenter", "False"));

            foreach (XmlElement patternElement in algorithmElement)
            {
                if (patternElement.Name == "Pattern")
                {
                    Pattern pattern = AlgorithmFactory.Instance().CreatePattern();

                    pattern.SetPatternImageString(XmlHelper.GetValue(patternElement, "Image", ""));
                    pattern.PatternType = (PatternType)Enum.Parse(typeof(PatternType), XmlHelper.GetValue(patternElement, "PatternType", "Good"));
                    pattern.UseEdgeFilter = Convert.ToBoolean(XmlHelper.GetValue(patternElement, "UseEdgeFilter", "False"));

                    pattern.Train(pattern.PatternImage, this);

                    foreach (XmlElement maskFiguresElement in patternElement)
                    {
                        if (maskFiguresElement.Name == "MaskFigures")
                        {
                            pattern.MaskFigures.Load(maskFiguresElement);

                            pattern.UpdateMaskImage();

                            break;
                        }
                    }

                    AddPattern(pattern);
                    //patternList.Add(pattern);
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MinAngle", MinAngle.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxAngle", MaxAngle.ToString());
            XmlHelper.SetValue(algorithmElement, "MinScale", MinScale.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxScale", MaxScale.ToString());
            XmlHelper.SetValue(algorithmElement, "IgnorePolarity", IgnorePolarity.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeWidth", SearchRangeWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHeight", SearchRangeHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "ToleranceX", ToleranceX.ToString());
            XmlHelper.SetValue(algorithmElement, "ToleranceY", ToleranceY.ToString());
            XmlHelper.SetValue(algorithmElement, "MatchScore", MatchScore.ToString());
            XmlHelper.SetValue(algorithmElement, "RecogString", RecogString.ToString());
            XmlHelper.SetValue(algorithmElement, "WholeImage", UseWholeImage.ToString());
            XmlHelper.SetValue(algorithmElement, "AllMatching", UseAllMatching.ToString());
            XmlHelper.SetValue(algorithmElement, "UseImageCenter", UseImageCenter.ToString());

            foreach (Pattern pattern in PatternList)
            {
                XmlElement patternElement = algorithmElement.OwnerDocument.CreateElement("", "Pattern", "");
                algorithmElement.AppendChild(patternElement);

                XmlHelper.SetValue(patternElement, "Image", pattern.GetPatternImageString());
                XmlHelper.SetValue(patternElement, "PatternType", pattern.PatternType.ToString());
                XmlHelper.SetValue(patternElement, "UseEdgeFilter", pattern.UseEdgeFilter.ToString());

                if (pattern.MaskFigures.FigureExist)
                {
                    XmlElement maskFiguresElement = patternElement.OwnerDocument.CreateElement("", "MaskFigures", "");
                    patternElement.AppendChild(maskFiguresElement);

                    pattern.MaskFigures.Save(maskFiguresElement);
                }
            }
        }

        public Pattern AddPattern(Image2D image2d)
        {
            Pattern pattern = AlgorithmFactory.Instance().CreatePattern();
            pattern.Train(image2d, this);

            PatternList.Insert(0, pattern);

            return pattern;
        }

        public void AddPattern(Pattern pattern)
        {
            PatternList.Add(pattern);
        }

        public void RemovePattern(Pattern pattern)
        {
            pattern.Dispose();
            PatternList.Remove(pattern);
        }

        public void RemovePattern(int index)
        {
            PatternList[index].Dispose();
            PatternList.RemoveAt(index);
        }

        public void RemoveAllPatterns()
        {
            foreach (Pattern pattern in PatternList)
            {
                pattern.Dispose();
            }

            PatternList.Clear();
        }

        public Pattern GetPattern(int index)
        {
            return PatternList[index];
        }
    }

    public class PatternMatchingConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PatternMatching))
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
                 value is PatternMatching)
            {

                var patternMatching = (PatternMatching)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(PatternMatchingConverter))]
    public class PatternMatching : Algorithm, Searchable
    {
        public PatternMatching()
        {
            param = new PatternMatchingParam();
        }

        public override void Clear()
        {
            ((PatternMatchingParam)param).Clear();
        }

        public override Algorithm Clone()
        {
            var patternMatching = new PatternMatching();
            patternMatching.Copy(this);

            return patternMatching;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var patternMatching = (PatternMatching)algorithm;

            param.Copy(patternMatching.param);
        }

        public const string TypeName = "PatternMatching";

        public bool fCalibrated { get; private set; }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "PatMat";
        }

        public bool UseWholeImage
        {
            get
            {
                var patternMatchingParam = (PatternMatchingParam)param;

                return patternMatchingParam.UseWholeImage;
            }
        }

        public bool UseAllMatching
        {
            get
            {
                var patternMatchingParam = (PatternMatchingParam)param;

                return patternMatchingParam.UseAllMatching;
            }
        }

        public Size GetSearchRangeSize()
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            return new Size(patternMatchingParam.SearchRangeWidth, patternMatchingParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            var patternMatchingParam = (PatternMatchingParam)param;
            patternMatchingParam.SearchRangeWidth = searchRange.Width;
            patternMatchingParam.SearchRangeHeight = searchRange.Height;
        }

        public Pattern AddPattern(Image2D image2d)
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            return patternMatchingParam.AddPattern(image2d);
        }

        public void RemovePattern(Pattern pattern)
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            patternMatchingParam.RemovePattern(pattern);
        }

        public void RemoveAllPatterns()
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            patternMatchingParam.RemoveAllPatterns();
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            var patternMatchingParam = (PatternMatchingParam)param;
            newInspRegion.Inflate(patternMatchingParam.SearchRangeWidth, patternMatchingParam.SearchRangeHeight);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Matching Ratio", "", 100, patternMatchingParam.MatchScore, 0));
            resultValues.Add(new ResultValue("Matchint Pos", "", 100, patternMatchingParam.MatchScore, 0));
            resultValues.Add(new ResultValue("Offset X", "", patternMatchingParam.SearchRangeWidth, 0, 0));
            resultValues.Add(new ResultValue("Offset Y", "", patternMatchingParam.SearchRangeHeight, 0, 0));
            resultValues.Add(new ResultValue("RealOffset", "", new SizeF(0, 0)));

            if (patternMatchingParam.RecogString != "")
            {
                resultValues.Add(new ResultValue("String Read", patternMatchingParam.RecogString));
            }

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage grayImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(grayImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;

            var pmResult = new SearchableResult();
            var patternMatchingParam = (PatternMatchingParam)param;

            var tolerance = new PointF(patternMatchingParam.ToleranceX, patternMatchingParam.ToleranceY);

            MatchPos maxMatchPos = null;

            var patternResultList = new List<PatternResult>();

            foreach (Pattern pattern in patternMatchingParam.PatternList)
            {
                PatternResult pmSubResult = pattern.Inspect(grayImage, patternMatchingParam, debugContext);

                pmSubResult.RemoveInvalidResult(patternMatchingParam.MatchScore);

                if (pmSubResult.MatchPosList.Count > 0)
                {
                    patternResultList.Add(pmSubResult);

                    MatchPos matchPos = pmSubResult.MaxMatchPos;
                    pmSubResult.Good = ((matchPos.Score * 100) >= patternMatchingParam.MatchScore);

                    if (pmSubResult.Good == true)
                    {
                        pmResult.SetResult(true);

                        if (maxMatchPos == null)
                        {
                            maxMatchPos = matchPos;
                        }
                    }
                }
            }

            pmResult.AddResultValue(new ResultValue("Matching Pos List", "", patternResultList));

            if (maxMatchPos == null)
            {
                maxMatchPos = new MatchPos();
            }

            PointF refPosInFov;
            PointF foundPosInFov;
            if (patternMatchingParam.UseImageCenter)
            {
                refPosInFov = new PointF(grayImage.Width / 2, grayImage.Height / 2);
            }
            else
            {
                refPosInFov = DrawingHelper.CenterPoint(probeRegionInFov);
            }

            var realOffset = new SizeF(0, 0);
            var offset = new SizeF(0, 0);

            float angle = 0;

            if (pmResult.IsGood())
            {
                foreach (PatternResult patternResult in patternResultList)
                {
                    foreach (MatchPos matchPos in patternResult.MatchPosList)
                    {
                        matchPos.Pos = DrawingHelper.ClipToFov(inspectRegionInFov, matchPos.Pos);
                    }
                }

                if (maxMatchPos.Pos != new PointF(0, 0))
                {
                    pmResult.Found = true;

                    foundPosInFov = maxMatchPos.Pos;

                    offset = new SizeF(foundPosInFov.X - refPosInFov.X, foundPosInFov.Y - refPosInFov.Y);
                    angle = maxMatchPos.Angle - probeRegionInFov.Angle;

                    if (cameraCalibration != null || cameraCalibration.IsCalibrated())
                    {
                        PointF realRefPos = cameraCalibration.PixelToWorld(refPosInFov);
                        PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

                        pmResult.RealOffsetFound = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
                    }
                }
            }

            pmResult.OffsetFound = offset;
            pmResult.AngleFound = angle;

            RotatedRect resultRect = probeRegionInFov;
            resultRect.Offset(offset.Width, offset.Height);
            resultRect.Angle = angle;
            pmResult.ResultRect = resultRect;

            int searchRangeWidth = patternMatchingParam.SearchRangeWidth;
            int searchRangeHeight = patternMatchingParam.SearchRangeHeight;

            bool toleranceIsGood = true;
            if (pmResult.IsGood())
            {
                if (maxMatchPos.PatternType == PatternType.Ng)
                {
                    pmResult.SetResult(false);
                }
                if (searchRangeWidth == 0 && searchRangeHeight == 0)
                {
                    if (inspectRegionInFov.X > resultRect.X || inspectRegionInFov.Y > resultRect.Y ||
                            (inspectRegionInFov.X + inspectRegionInFov.Width) < (resultRect.X + resultRect.Width) ||
                            (inspectRegionInFov.Y + inspectRegionInFov.Height) < (resultRect.Y + resultRect.Height))
                    {
                        pmResult.SetResult(false);
                    }

                    searchRangeWidth = (int)(inspectRegionInFov.Width / 2);
                    searchRangeHeight = (int)(inspectRegionInFov.Height / 2);
                }
                else if (patternMatchingParam.UseWholeImage == false)
                {
                    if ((Math.Abs(offset.Width) > searchRangeWidth) || (Math.Abs(offset.Height) > searchRangeHeight))
                    {
                        pmResult.SetResult(false);
                    }
                }
                if (tolerance.X != 0 && tolerance.Y != 0)
                {
                    var realOffsetFound = new PointF(pmResult.RealOffsetFound.ToPointF().X, pmResult.RealOffsetFound.ToPointF().Y);
                    if (realOffsetFound.X > tolerance.X || realOffsetFound.Y > tolerance.Y)
                    {
                        pmResult.SetResult(false);
                        toleranceIsGood = false;
                    }
                }
            }

            if (patternMatchingParam.UseAllMatching == false)
            {
                var probeCenter = new PointF((probeRegionInFov.X + (probeRegionInFov.Width / 2)),
                (probeRegionInFov.Y + (probeRegionInFov.Height / 2)));

                PointF realProbeCenter = cameraCalibration.PixelToWorld(probeCenter);
                PointF imageRealSize = cameraCalibration.PixelToWorld(new PointF(grayImage.Width, grayImage.Height));

                pmResult.AddResultValue(new ResultValue("Matching Score", "", 100, patternMatchingParam.MatchScore, maxMatchPos.Score * 100));
                pmResult.AddResultValue(new ResultValue("Matching Pos", "", maxMatchPos.Pos));
                pmResult.AddResultValue(new ResultValue("Offset X", "", searchRangeWidth, 0, offset.Width));
                pmResult.AddResultValue(new ResultValue("Offset Y", "", searchRangeHeight, 0, offset.Height));
                pmResult.AddResultValue(new ResultValue("RealOffset", "", pmResult.RealOffsetFound));
                pmResult.AddResultValue(new ResultValue("Tolerance", "", toleranceIsGood));
                pmResult.AddResultValue(new ResultValue("ProbeCenter", "", probeCenter));
                pmResult.AddResultValue(new ResultValue("RealProbeCenter", "", realProbeCenter));
                pmResult.AddResultValue(new ResultValue("Result", "", pmResult.Result));
                pmResult.AddResultValue(new ResultValue("RealCamSize", "", imageRealSize));

                if (pmResult.IsGood() && patternMatchingParam.RecogString != "")
                {
                    pmResult.AddResultValue(new ResultValue("String Read", patternMatchingParam.RecogString));
                }

                pmResult.BriefMessage = string.Format("Matching Score : {0} \n Offset X : {1}, Y : {2}", (maxMatchPos.Score * 100).ToString(), pmResult.OffsetFound.Width, pmResult.OffsetFound.Height);
            }
            else
            {
                if (patternResultList.Count > 0)
                {
                    pmResult.BriefMessage = string.Format("Matching Point Count : {0}", patternResultList[0].MatchPosList.Count);
                }

                foreach (PatternResult patternResult in patternResultList)
                {
                    for (int i = 0; i < patternResult.MatchPosList.Count; i++)
                    {
                        MatchPos matchPos = patternResult.MatchPosList[i];

                        pmResult.AddResultValue(new ResultValue(string.Format("Matching Pos {0}", i + 1), "", matchPos,
                            new RotatedRect(
                                matchPos.Pos.X - matchPos.PatternSize.Width / 2,
                                matchPos.Pos.Y - matchPos.PatternSize.Height / 2,
                                matchPos.PatternSize.Width,
                                matchPos.PatternSize.Height,
                                matchPos.Angle)));
                    }
                }
            }

            return pmResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            var patternMatchingParam = (PatternMatchingParam)param;

            if (!(algorithmResult is SearchableResult pmResult))
            {
                return;
            }

            var patternResultList = (List<PatternResult>)pmResult.GetResultValue("Matching Pos List").Value;

            resultMessage.BeginTable(null, "Item", "Value");

            if (patternMatchingParam.UseAllMatching == false)
            {
                var matchingPos = (PointF)pmResult.GetResultValue("Matching Pos").Value;
                float matchingScore = (float)pmResult.GetResultValue("Matching Score").Value;

                resultMessage.AddTableRow("Result", pmResult.GetGoodNgStr());
                resultMessage.AddTableRow("Matching Score", matchingScore.ToString());
                resultMessage.AddTableRow("Matching Pos", matchingPos.ToString());
                resultMessage.AddTableRow("Offset", string.Format("({0:0.00}, {1:0.00})", pmResult.OffsetFound.Width, pmResult.OffsetFound.Height));
                resultMessage.AddTableRow("RealOffset", string.Format("({0:0.00}, {1:0.00})", pmResult.RealOffsetFound.Width, pmResult.RealOffsetFound.Height));

                resultMessage.EndTable();

                resultMessage.BeginTable(null, "No", "Score", "Found");

                for (int i = 0; i < patternResultList.Count; i++)
                {
                    PatternResult patternResult = patternResultList[i];
                    resultMessage.AddTableRow(i.ToString(), (patternResult.MaxScore * 100).ToString("0.00"),
                        (patternResult.Found ? "OK" : (string.IsNullOrEmpty(patternResult.ErrorString) ? "NG" : patternResult.ErrorString)));
                }

                resultMessage.EndTable();
            }
            else
            {
                if (patternResultList.Count < 1)
                {
                    return;
                }

                int matchingPosCount = patternResultList[0].MatchPosList.Count;

                for (int i = 0; i < matchingPosCount; i++)
                {
                    resultMessage.AddTableRow(string.Format("Matching Pos {0}", i + 1),
                        string.Format("(X:{0:0}, Y:{1:0}, Score:{2:0}",
                        patternResultList[0].MatchPosList[i].Pos.X,
                        patternResultList[0].MatchPosList[i].Pos.Y,
                        patternResultList[0].MatchPosList[i].Score * 100));
                }
            }
        }
    }

    public class SearchableResult : AlgorithmResult
    {
        public bool Found { get; set; }
        public SizeF OffsetFound { get; set; }
        public float AngleFound { get; set; }
        public SizeF RealOffsetFound { get; set; }
    }
}
