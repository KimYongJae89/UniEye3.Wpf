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
    public enum BarcodeType
    {
        //Common
        Codabar, Code128, Code39, Code93, Interleaved2of5, Pharmacode, PLANET, POSTNET, FourStatePostal, //1D Barcode
        DataMatrix, QRCode, //2D Barcode

        //Cognex
        UPCEAN, EANUCCComposite, PDF417, // 1D Barcode

        //MIL
        BC412, EAN8, EAN13, EAN14, UPC_A, UPC_E, GS1_128, GS1Databar //1D Barcode
    }

    public class BarcodeReaderConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(Algorithm))
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
                 value is BarcodeReader)
            {
                var barcodeReader = (BarcodeReader)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class BarcodeReaderParam : AlgorithmParam
    {
        public string DesiredString { get; set; } = "";
        public int DesiredNum { get; set; } = 1;
        public int SearchRangeWidth { get; set; } = 0;
        public int SearchRangeHeight { get; set; } = 0;
        public bool OffsetRange { get; set; } = false;
        public int RangeThresholdLeft { get; set; } = 0;
        public int RangeThresholdRight { get; set; } = 0;
        public int RangeThresholdBottom { get; set; } = 0;
        public int RangeThresholdTop { get; set; } = 0;
        public int MinArea { get; set; } = 0;
        public int MaxArea { get; set; } = 0;
        public int CloseNum { get; set; } = 3;
        public int ThresholdPercent { get; set; } = 50;
        public List<int> ThresholdPercentList { get; set; } = new List<int>();
        public List<BarcodeType> BarcodeTypeList { get; set; } = new List<BarcodeType>();
        public bool UseAreaFilter { get; set; } = false;
        public bool UseBlobing { get; set; } = false;
        public bool UseWholeImage { get; set; } = false;
        public int TimeoutTime { get; set; } = 5000;

        public override AlgorithmParam Clone()
        {
            var param = new BarcodeReaderParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BarcodeReaderParam)srcAlgorithmParam;

            DesiredString = param.DesiredString;
            DesiredNum = param.DesiredNum;
            BarcodeTypeList = param.BarcodeTypeList;
            OffsetRange = param.OffsetRange;
            RangeThresholdLeft = param.RangeThresholdLeft;
            RangeThresholdRight = param.RangeThresholdRight;
            RangeThresholdBottom = param.RangeThresholdBottom;
            RangeThresholdTop = param.RangeThresholdTop;
            MinArea = param.MinArea;
            MaxArea = param.MaxArea;
            ThresholdPercent = param.ThresholdPercent;
            UseAreaFilter = param.UseAreaFilter;
            CloseNum = param.CloseNum;
            UseBlobing = param.UseBlobing;
            UseWholeImage = param.UseWholeImage;

            ThresholdPercentList = param.ThresholdPercentList;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            DesiredString = XmlHelper.GetValue(algorithmElement, "DesiredString", "");
            DesiredNum = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DesiredNum", "0"));
            SearchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "30"));
            SearchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "30"));

            OffsetRange = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "OffsetRange", "false"));
            RangeThresholdRight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdRight", "30"));
            RangeThresholdLeft = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdLeft", "30"));
            RangeThresholdBottom = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdBottom", "30"));
            RangeThresholdTop = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdTop", "30"));

            MinArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinArea", "0"));
            MaxArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxArea", "0"));

            UseAreaFilter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseAreaFilter", "false"));
            UseBlobing = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseBlobing", "false"));
            UseWholeImage = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseWholeImage", "false"));

            CloseNum = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CloseNum", "0"));

            foreach (XmlElement barcodeTypeElement in algorithmElement)
            {
                if (barcodeTypeElement.Name == "BarcodeType")
                {
                    BarcodeTypeList.Add((BarcodeType)Enum.Parse(typeof(BarcodeType), barcodeTypeElement.InnerText));
                }
            }

            foreach (XmlElement thresholdPercentElement in algorithmElement)
            {
                if (thresholdPercentElement.Name == "ThresholdPercent")
                {
                    ThresholdPercentList.Add(Convert.ToInt32(thresholdPercentElement.InnerText));
                }
            }
            TimeoutTime = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "TimeoutTime", "5000"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "DesiredString", DesiredString);
            XmlHelper.SetValue(algorithmElement, "DesiredNum", DesiredNum.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeWidth", SearchRangeWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHeight", SearchRangeHeight.ToString());

            XmlHelper.SetValue(algorithmElement, "OffsetRange", OffsetRange.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdRight", RangeThresholdRight.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdLeft", RangeThresholdLeft.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdBottom", RangeThresholdBottom.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdTop", RangeThresholdTop.ToString());

            XmlHelper.SetValue(algorithmElement, "MinArea", MinArea.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxArea", MaxArea.ToString());

            XmlHelper.SetValue(algorithmElement, "UseAreaFilter", UseAreaFilter.ToString());
            XmlHelper.SetValue(algorithmElement, "UseBlobing", UseBlobing.ToString());
            XmlHelper.SetValue(algorithmElement, "UseWholeImage", UseWholeImage.ToString());

            XmlHelper.SetValue(algorithmElement, "CloseNum", CloseNum.ToString());

            foreach (BarcodeType barcodeType in BarcodeTypeList)
            {
                XmlHelper.SetValue(algorithmElement, "BarcodeType", barcodeType.ToString());
            }

            foreach (int thresholdPercentOfList in ThresholdPercentList)
            {
                XmlHelper.SetValue(algorithmElement, "ThresholdPercent", thresholdPercentOfList.ToString());
            }
            XmlHelper.SetValue(algorithmElement, "TimeoutTime", TimeoutTime.ToString());
        }
    }

    [TypeConverterAttribute(typeof(BarcodeReaderConverter))]
    public abstract class BarcodeReader : Algorithm, Searchable
    {
        public string LastReadString { get; private set; }

        public BarcodeReader()
        {
            param = new BarcodeReaderParam();
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var barcodeReader = (BarcodeReader)algorithm;
            param = (BarcodeReaderParam)barcodeReader.Param.Clone();
        }

        public const string TypeName = "BarcodeReader";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Barcode";
        }

        public bool UseWholeImage
        {
            get
            {
                var barcodeReaderParam = (BarcodeReaderParam)param;
                return barcodeReaderParam.UseWholeImage;
            }
        }

        public Size GetSearchRangeSize()
        {
            var barcodeReaderParam = (BarcodeReaderParam)param;

            return new Size(barcodeReaderParam.SearchRangeWidth, barcodeReaderParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            var barcodeReaderParam = (BarcodeReaderParam)param;

            barcodeReaderParam.SearchRangeWidth = searchRange.Width;
            barcodeReaderParam.SearchRangeHeight = searchRange.Height;
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            var barcodeReaderParam = (BarcodeReaderParam)param;

            float rectWidth = barcodeReaderParam.RangeThresholdRight + barcodeReaderParam.RangeThresholdLeft + probeRect.Width;
            float rectHeight = barcodeReaderParam.RangeThresholdBottom + barcodeReaderParam.RangeThresholdTop + probeRect.Height;

            var searchRangeRect = new RotatedRect(probeRect.Left - barcodeReaderParam.RangeThresholdLeft, probeRect.Top - barcodeReaderParam.RangeThresholdTop, rectWidth, rectHeight, probeRect.Angle);

            var pen = new Pen(Color.Purple, 1.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            var figure = new RectangleFigure(searchRangeRect, pen);

            figure.Selectable = false;
            tempFigures.AddFigure(figure);
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            var barcodeReaderParam = (BarcodeReaderParam)param;

            if (barcodeReaderParam.SearchRangeWidth != 0 || barcodeReaderParam.SearchRangeHeight != 0)
            {
                newInspRegion.Inflate(barcodeReaderParam.SearchRangeWidth, barcodeReaderParam.SearchRangeHeight);
            }

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var barcodeReaderParam = (BarcodeReaderParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("DesiredString", barcodeReaderParam.DesiredString));
            resultValues.Add(new ResultValue("BarcodePositionList", null));

            if (barcodeReaderParam.OffsetRange == true)
            {
                resultValues.Add(new ResultValue("XPosGap", "px", 100, 0, 0));
                resultValues.Add(new ResultValue("YPosGap", "px", 100, 0, 0));
            }

            return resultValues;
        }

        public override string[] GetPreviewNames()
        {
            return new string[] { "Default" };
        }

        public override ImageD Filter(ImageD image, int previewFilterType, int targetLightTypeIndex = -1)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BarcodeReader_Filter");

            var barcodeReaderParam = (BarcodeReaderParam)param;

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            switch (previewFilterType)
            {
                case 0:
                    imageProcessing.BinarizeHistogram(algoImage, algoImage, barcodeReaderParam.ThresholdPercent);

                    imageProcessing.Not(algoImage, algoImage);
                    imageProcessing.Dilate(algoImage, barcodeReaderParam.CloseNum);
                    imageProcessing.Erode(algoImage, barcodeReaderParam.CloseNum);
                    break;
            }

            return algoImage.ToImageD();
        }

        //public AlgorithmResult Inspect(List<Image2D> imageList, Size wholeImageSize, DebugContext debugContext)
        //{
        //    RotatedRect probeRegionInFov = new RotatedRect(0, 0, probeClipImage.Width, probeClipImage.Height, 0);
        //    RotatedRect imageRegionInFov = new RotatedRect(0, 0, probeClipImage.Width, probeClipImage.Height, 0);

        //    return Inspect(new AlgorithmInspectParam(imageList, probeRegionInFov, imageRegionInFov, wholeImageSize, null, debugContext));
        //}

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage inspectImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey);
            Filter(inspectImage, 0);

            var barcodeReaderParam = (BarcodeReaderParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            SearchableResult barcodeReaderResult = Read(inspectImage, probeRegionInClip, inspectParam.DebugContext);

            string desiredString = barcodeReaderParam.DesiredString;
            barcodeReaderResult.ResultRect = inspectParam.ProbeRegionInFov;
            barcodeReaderResult.AddResultValue(new ResultValue("DesiredString", desiredString));

            ResultValue barcodePositionListResult = barcodeReaderResult.GetResultValue("BarcodePositionList");
            if (barcodePositionListResult == null)
            {
                return barcodeReaderResult;
            }

            var barcodePositionList = (BarcodePositionList)barcodePositionListResult.Value;

            int barcodeNum = barcodePositionList.Items.Count();

            Size whileImageSize = inspectParam.CameraImageSize;

            var offset = new SizeF(0, 0);
            float angle = 0;

            if (barcodeNum > 0)
            {
                barcodeReaderResult.SetResult(true);

                if (string.IsNullOrEmpty(desiredString) != true)
                {
                    foreach (BarcodePosition barcodePosition in barcodePositionList.Items)
                    {
                        barcodePosition.FoundPosition = DrawingHelper.ClipToFov(inspectRegionInFov, barcodePosition.FoundPosition);
                        barcodeReaderResult.ResultFigures.AddFigure(new PolygonFigure(barcodePosition.FoundPosition, new Pen(Color.Lime, 3)));

                        barcodePosition.Good = (barcodePosition.StringRead.IndexOf(desiredString) > -1);
                        if (barcodePosition.Good == false)
                        {
                            barcodeReaderResult.SetResult(false);
                        }
                    }
                }

                if (barcodeReaderParam.DesiredNum > 0)
                {
                    barcodeReaderResult.SetResult((barcodeReaderParam.DesiredNum == barcodeNum) && barcodeReaderResult.IsGood());
                }

                RectangleF boundingBox = DrawingHelper.GetBoundRect(barcodePositionList.Items[0].FoundPosition.ToArray());

                if (barcodeNum == 1)
                {
                    BarcodePosition position = barcodePositionList.Items[0];
                    LastReadString = barcodePositionList.Items[0].StringRead;
                    angle = position.FoundAngle;

                    PointF probePosInFov = DrawingHelper.CenterPoint(probeRegionInFov);
                    PointF foundPosInFov = DrawingHelper.ClipToFov(inspectRegionInFov, DrawingHelper.CenterPoint(boundingBox));

                    offset.Width = foundPosInFov.X - probePosInFov.X;
                    offset.Height = foundPosInFov.Y - probePosInFov.Y;

                    barcodeReaderResult.OffsetFound = offset;
                    barcodeReaderResult.AngleFound = angle;

                    if (inspectParam.CameraCalibration != null)
                    {
                        PointF realProbeCenter = inspectParam.CameraCalibration.PixelToWorld(probePosInFov);
                        PointF realFoundPos = inspectParam.CameraCalibration.PixelToWorld(foundPosInFov);

                        barcodeReaderResult.RealOffsetFound = new SizeF(realFoundPos.X - realProbeCenter.X, realFoundPos.Y - realProbeCenter.Y);
                    }

                    if (barcodeReaderParam.OffsetRange == true)
                    {
                        barcodeReaderResult.AddResultValue(new ResultValue("XPosGap", "", offset.Width));
                        barcodeReaderResult.AddResultValue(new ResultValue("YPosGap", "", offset.Height));

                        float leftLimit = inspectParam.ProbeRegionInFov.Left - barcodeReaderParam.RangeThresholdLeft;
                        float RightLimit = inspectParam.ProbeRegionInFov.Right + barcodeReaderParam.RangeThresholdRight;
                        float bottomLimit = inspectParam.ProbeRegionInFov.Bottom + barcodeReaderParam.RangeThresholdBottom;
                        float topLimit = inspectParam.ProbeRegionInFov.Top - barcodeReaderParam.RangeThresholdTop;

                        bool result = position.FoundPosition[0].X > leftLimit && position.FoundPosition[2].X < RightLimit && position.FoundPosition[0].Y > topLimit && position.FoundPosition[2].Y < bottomLimit && barcodeReaderResult.IsGood();
                        barcodeReaderResult.SetResult(result);

                        if (barcodeReaderResult.IsGood() == true)
                        {
                            barcodeReaderResult.ResultFigures.AddFigure(new LineFigure(probePosInFov, foundPosInFov, new Pen(Color.Blue)));
                        }
                        else
                        {
                            barcodeReaderResult.ResultFigures.AddFigure(new LineFigure(probePosInFov, foundPosInFov, new Pen(Color.Red)));
                        }
                    }
                }
            }

            return barcodeReaderResult;
        }

        public abstract SearchableResult Read(AlgoImage clipImage, RectangleF clipRect, DebugContext debugContext);
    }

    public class BarcodePositionList : ResultValueItem
    {
        public List<BarcodePosition> Items { get; set; } = new List<BarcodePosition>();

        public BarcodePosition GetBarcodePosition(string barcode)
        {
            foreach (BarcodePosition position in Items)
            {
                if (position.StringRead == barcode)
                {
                    return position;
                }
            }

            return null;
        }

        public List<BarcodePosition> GetDuplicatePosition(string barcode)
        {
            var barcodeDuplicateList = new List<BarcodePosition>();
            foreach (BarcodePosition position in Items)
            {
                if (position.StringRead == barcode)
                {
                    barcodeDuplicateList.Add(position);
                }
            }
            return barcodeDuplicateList;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (BarcodePosition position in Items)
            {
                stringBuilder.AppendLine(position.ToString());
            }

            return stringBuilder.ToString();
        }

        public override string GetValueString()
        {
            var stringBuilder = new StringBuilder();

            foreach (BarcodePosition position in Items)
            {
                stringBuilder.Append(position.StringRead);
                stringBuilder.Append("/");
            }

            return stringBuilder.ToString();
        }
    }

    public class BarcodePosition
    {
        public bool Good { get; set; }
        public string StringRead { get; set; }
        public List<PointF> FoundPosition { get; set; } = new List<PointF>();
        public float FoundAngle { get; set; } = new float();
        public int Area { get; set; }


        public override string ToString()
        {
            PointF centerPt = DrawingHelper.CenterPoint(FoundPosition.ToArray());
            return string.Format("Barcode : {0} ( Pos : {1} / Angle : {2} / Area : {3} ) ", StringRead, centerPt.ToString(), FoundAngle, Area);
        }
    }
}
