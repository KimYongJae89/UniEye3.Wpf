using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum ColorCheckerType
    {
        Average, Segmentation
    }

    public class ColorCheckerParam : AlgorithmParam
    {
        public ColorCheckerType Type { get; set; } = ColorCheckerType.Average;
        public List<ColorValue> ColorValueList { get; } = new List<ColorValue>();
        public int AcceptanceScore { get; set; } = 80;
        public float ScoreWeightValue1 { get; set; } = 1;
        public float ScoreWeightValue2 { get; set; } = 1;
        public float ScoreWeightValue3 { get; set; } = 1;
        public ColorSpace ColorSpace { get; set; } = ColorSpace.RGB;
        public GridParam GridParam { get; private set; } = new GridParam();
        public bool UseSegmentation { get; set; }
        public int SegmentScore { get; set; }
        public ScoreType ScoreType { get; set; }

        public override AlgorithmParam Clone()
        {
            var param = new ColorCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (ColorCheckerParam)srcAlgorithmParam;

            ColorValueList.Clear();
            ColorValueList.AddRange(param.ColorValueList);

            AcceptanceScore = param.AcceptanceScore;
            ScoreWeightValue1 = param.ScoreWeightValue1;
            ScoreWeightValue2 = param.ScoreWeightValue2;
            ScoreWeightValue3 = param.ScoreWeightValue3;
            ColorSpace = param.ColorSpace;
            GridParam = param.GridParam.Clone();

            UseSegmentation = param.UseSegmentation;
            ScoreType = param.ScoreType;
            SegmentScore = param.SegmentScore;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            AcceptanceScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "AcceptanceSore", "10"));
            ScoreWeightValue1 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue1", "1"));
            ScoreWeightValue2 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue2", "1"));
            ScoreWeightValue3 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue3", "1"));
            ColorSpace = (ColorSpace)Enum.Parse(typeof(ColorSpace), XmlHelper.GetValue(algorithmElement, "ColorSpace", "RGB"));

            XmlElement colorValueListElement = algorithmElement["ColorValueList"];
            if (colorValueListElement != null)
            {
                foreach (XmlElement colorValueElement in colorValueListElement)
                {
                    var colorValue = new ColorValue();
                    if (colorValueElement.Name == "ColorValue")
                    {
                        colorValue.Load(colorValueElement);
                        ColorValueList.Add(colorValue);
                    }
                }
            }

            GridParam.LoadParam(algorithmElement);

            UseSegmentation = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseSegmentation", "False"));
            ScoreType = (ScoreType)Enum.Parse(typeof(ScoreType), XmlHelper.GetValue(algorithmElement, "ScoreType", "Ratio"));
            SegmentScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SegmentScore", "100"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "AcceptanceSore", AcceptanceScore.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue1", ScoreWeightValue1.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue2", ScoreWeightValue2.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue3", ScoreWeightValue3.ToString());
            XmlHelper.SetValue(algorithmElement, "ColorSpace", ColorSpace.ToString());

            XmlElement colorValueListElement = algorithmElement.OwnerDocument.CreateElement("", "ColorValueList", "");
            algorithmElement.AppendChild(colorValueListElement);

            foreach (ColorValue colorValue in ColorValueList)
            {
                XmlElement colorValueElement = algorithmElement.OwnerDocument.CreateElement("", "ColorValue", "");
                colorValueListElement.AppendChild(colorValueElement);

                colorValue.Save(colorValueElement);
            }

            GridParam.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseSegmentation", UseSegmentation.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreType", ScoreType.ToString());
            XmlHelper.SetValue(algorithmElement, "SegmentScore", SegmentScore.ToString());
        }
    }

    public class ColorChecker : Algorithm
    {
        public ColorChecker()
        {
            isColorAlgorithm = true;
            Param = new ColorCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            var colorChecker = new ColorChecker();
            colorChecker.Copy(this);

            return colorChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var colorChecker = (ColorChecker)algorithm;

            param = (ColorCheckerParam)colorChecker.Param.Clone();
        }

        public const string TypeName = "ColorChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Color";
        }

        public override ImageD Filter(ImageD image, int previewFilterTyype, int targetLightTypeIndex = -1)
        {
            if (image is Image3D)
            {
                return image;
            }

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Color);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var colorCheckerParam = (ColorCheckerParam)Param;

            switch (previewFilterTyype)
            {
                case 0:
                    if (colorCheckerParam.UseSegmentation == true)
                    {
                        if (colorCheckerParam.GridParam.UseGrid == true)
                        {
                            int gridCount = colorCheckerParam.GridParam.GetNumCol() * colorCheckerParam.GridParam.GetNumRow();
                            float widthStep = image.Width / colorCheckerParam.GridParam.GetNumCol();
                            float heightStep = image.Height / colorCheckerParam.GridParam.GetNumRow();

                            for (float y = 0; y < image.Height; y += heightStep)
                            {
                                for (float x = 0; x < image.Width; x += widthStep)
                                {
                                    var cellRect = Rectangle.Truncate(new RectangleF(x, y, widthStep, heightStep));

                                    Color color = imageProcessing.GetColorAverage(algoImage, cellRect);
                                    imageProcessing.Clear(algoImage, cellRect, color);
                                }
                            }
                        }
                        else
                        {

                            GetSegmentCount((Image2D)image, out Image2D segmentImage);

                            return segmentImage.GetColorImage();
                        }

                    }
                    else
                    {
                        Color color = imageProcessing.GetColorAverage(algoImage);
                        imageProcessing.Clear(algoImage, color);
                    }
                    break;
            }

            var fillterredImage = (Image2D)algoImage.ToImageD();

            return fillterredImage.GetColorImage();
        }

        public override List<ResultValue> GetResultValues()
        {
            var colorCheckerParam = (ColorCheckerParam)Param;

            var resultValues = new List<ResultValue>();

            if (colorCheckerParam.GridParam.UseGrid)
            {
                if (colorCheckerParam.ScoreType == ScoreType.Ratio)
                {
                    resultValues.Add(new ResultValue("Match Ratio", "", colorCheckerParam.GridParam.CellAcceptRatio, 0, 0));
                }
                else
                {
                    resultValues.Add(new ResultValue("Match Count", "", colorCheckerParam.GridParam.CellAcceptRatio, 0, 0));
                }
            }
            else
            {
                resultValues.Add(new ResultValue("Match Index", "", 0, 0, 0));
                resultValues.Add(new ResultValue("Match Distance", "", colorCheckerParam.AcceptanceScore, 0, 0));
            }

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;

            var image = inspectParam.InspectImageList[0] as Image2D;

            var algorithmResult = new AlgorithmResult();

            if (image.NumBand < 3)
            {
                algorithmResult.BriefMessage = "Invalid Image Type : Image is not color image";
                return algorithmResult;
            }

            var clipImage = (Image2D)image.ClipImage(probeRegionInFov);

            var colorCheckerParam = (ColorCheckerParam)Param;

            bool result = false;
            if (colorCheckerParam.UseSegmentation == true)
            {
                if (colorCheckerParam.GridParam.UseGrid == true)
                {
                    result = InspectGridCell(inspectParam, clipImage, algorithmResult);
                }
                else
                {
                    result = InspectSegment(image, algorithmResult);
                }
            }
            else
            {
                result = InspectAverage(clipImage, algorithmResult);
            }

            algorithmResult.SetResult(result);

            return algorithmResult;
        }

        private bool InspectAverage(Image2D srcImage, AlgorithmResult algorithmResult, int cellIndex = -1)
        {
            var colorCheckerParam = (ColorCheckerParam)Param;

            AlgoImage clipAlgoImage = ImageBuilder.Build(GetAlgorithmType(), srcImage, ImageType.Color);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipAlgoImage);

            Color averageColor = imageProcessing.GetColorAverage(clipAlgoImage);

            var colorValue = new ColorValue(averageColor);
            if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
            {
                colorValue = colorValue.RgbToHsi();
            }

            int matchIndex = 0;
            matchIndex = GetColorIndex(colorValue, algorithmResult, out float matchScore);

            bool result = (matchIndex > -1);
            if (result == true)
            {
                string cellPrefix = "";
                if (cellIndex > -1)
                {
                    cellPrefix = "[ Cell " + cellIndex + " ]";
                }

                algorithmResult.AddResultValue(new ResultValue(cellPrefix + "Match Index", "", 0, 0, matchIndex));
                algorithmResult.AddResultValue(new ResultValue(cellPrefix + "Match Score", "", colorCheckerParam.AcceptanceScore, 0, matchScore));
            }

            return result;
        }

        private int GetColorIndex(ColorValue colorValue, AlgorithmResult algorithmResult, out float matchScore)
        {
            var colorCheckerParam = (ColorCheckerParam)Param;

            int index = 0, matchedIndex = -1;

            matchScore = 9999;
            float score = 0;
            foreach (ColorValue referenceColor in colorCheckerParam.ColorValueList)
            {
                ColorValue referenceColorValue = referenceColor;
                if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                {
                    referenceColorValue = referenceColor.GetColor(ColorSpace.HSI);
                }

                float diffValue1 = Math.Abs(referenceColorValue.Value1 - colorValue.Value1);
                float diffValue2 = Math.Abs(referenceColorValue.Value2 - colorValue.Value2);
                float diffValue3 = Math.Abs(referenceColorValue.Value3 - colorValue.Value3);

                score = (float)Math.Sqrt(diffValue1 * diffValue1 + diffValue2 * diffValue2 + diffValue3 * diffValue3);
                bool result = (score < colorCheckerParam.AcceptanceScore);

                if (colorCheckerParam.GridParam.UseGrid == false)
                {
                    algorithmResult.AddResultValue(new ResultValue("[" + index + "] Score ", "", colorCheckerParam.AcceptanceScore, 0, score));
                }

                if (result)
                {
                    if (score < matchScore)
                    {
                        matchScore = score;
                        matchedIndex = index;
                    }
                }

                index++;
            }

            if (matchScore == 9999)
            {
                matchScore = 0;
            }

            return matchedIndex;
        }

        private int GetSegmentCount(Image2D srcImage, out Image2D segmentImage, AlgorithmResult algorithmResult = null)
        {
            byte[] data = srcImage.Data;
            var colorCheckerParam = (ColorCheckerParam)Param;

            segmentImage = new Image2D(srcImage.Width, srcImage.Height, 1);

            int pixelCount = 0;
            float matchDistance;
            if (srcImage.NumBand == 3)
            {
                for (int y = 0; y < srcImage.Height; y++)
                {
                    for (int x = 0; x < srcImage.Width; x++)
                    {
                        byte red = data[y * srcImage.Pitch + x * 3];
                        byte green = data[y * srcImage.Pitch + x * 3 + 1];
                        byte blue = data[y * srcImage.Pitch + x * 3 + 2];

                        var colorValue = new ColorValue(red, green, blue);
                        if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                        {
                            colorValue = colorValue.GetColor(ColorSpace.HSI);
                        }

                        int index = GetColorIndex(colorValue, algorithmResult, out matchDistance);
                        if (index > -1)
                        {
                            pixelCount++;
                            segmentImage.Data[y * segmentImage.Pitch + x] = 255;
                        }
                    }
                }

                return pixelCount;
            }
            else
            {
                for (int y = 0; y < srcImage.Height; y++)
                {
                    for (int x = 0; x < srcImage.Width; x++)
                    {
                        byte alpha = data[y * srcImage.Pitch + x * 4];
                        byte red = data[y * srcImage.Pitch + x * 4 + 1];
                        byte green = data[y * srcImage.Pitch + x * 4 + 2];
                        byte blue = data[y * srcImage.Pitch + x * 4 + 3];

                        var colorValue = new ColorValue(red, green, blue);
                        if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                        {
                            colorValue = colorValue.GetColor(ColorSpace.HSI);
                        }

                        int index = GetColorIndex(colorValue, algorithmResult, out matchDistance);
                        if (index > -1)
                        {
                            pixelCount++;
                            segmentImage.Data[y * segmentImage.Pitch + x] = 255;
                        }
                    }
                }

                return pixelCount;
            }
        }

        private bool InspectSegment(Image2D inspImage, AlgorithmResult algorithmResult, int cellIndex = -1)
        {
            var colorCheckerParam = (ColorCheckerParam)Param;

            int segmentCount = GetSegmentCount(inspImage, out Image2D segmentImage, algorithmResult);

            string cellPrefix = "";
            if (cellIndex > -1)
            {
                cellPrefix = "[ Cell " + cellIndex + " ]";
            }

            bool result = false;

            if (colorCheckerParam.ScoreType == ScoreType.Count)
            {
                result = (segmentCount > colorCheckerParam.SegmentScore);
                algorithmResult.AddResultValue(new ResultValue(cellPrefix + "Segment Count", "", colorCheckerParam.SegmentScore, 0, segmentCount));
            }
            else
            {
                float area = inspImage.Width * inspImage.Height;
                float segmentRatio = (segmentCount / area * 100);
                result = (segmentRatio > colorCheckerParam.SegmentScore);
                algorithmResult.AddResultValue(new ResultValue(cellPrefix + "Segment Ratio", "", colorCheckerParam.SegmentScore, 0, segmentRatio));
            }

            return result;
        }

        private bool InspectGridCell(AlgorithmInspectParam inspectParam, Image2D clipImage, AlgorithmResult algorithmResult)
        {
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;

            var colorCheckerParam = (ColorCheckerParam)Param;

            int gridCount = colorCheckerParam.GridParam.GetNumCol() * colorCheckerParam.GridParam.GetNumRow();
            float widthStep = clipImage.Width / colorCheckerParam.GridParam.GetNumCol();
            float heightStep = clipImage.Height / colorCheckerParam.GridParam.GetNumRow();

            int cellIndex = 0;
            bool result = false;
            int goodCount = 0;
            for (float y = 0; y < clipImage.Height; y += heightStep)
            {
                for (float x = 0; x < clipImage.Width; x += widthStep)
                {
                    var cellRect = new RectangleF(x, y, widthStep, heightStep);
                    var cellImage = (Image2D)clipImage.ClipImage(Rectangle.Truncate(cellRect));

                    if (colorCheckerParam.UseSegmentation == true)
                    {
                        result = InspectSegment(cellImage, algorithmResult, cellIndex);
                    }
                    else
                    {
                        result = InspectAverage(cellImage, algorithmResult, cellIndex);
                    }

                    if (result == true)
                    {
                        goodCount++;
                    }
                    else
                    {
                        RotatedRect fovCellRect = DrawingHelper.ClipToFov(probeRegionInFov, cellRect);
                        algorithmResult.ResultFigures.AddFigure(new XRectFigure(fovCellRect, new Pen(Color.Red, 1.0F)));
                    }
                }
            }

            int gridAcceptanceScore = colorCheckerParam.GridParam.CellAcceptRatio;

            if (colorCheckerParam.ScoreType == ScoreType.Ratio)
            {
                float matchRatio = (float)goodCount / gridCount * 100;
                algorithmResult.SetResult(matchRatio > gridAcceptanceScore);
                algorithmResult.AddResultValue(new ResultValue("Match Ratio", "", gridAcceptanceScore, 0, matchRatio));
                algorithmResult.AddResultValue(new ResultValue("Match Count", "", 0, 0, goodCount));
            }
            else
            {
                algorithmResult.SetResult(goodCount > gridAcceptanceScore);
                algorithmResult.AddResultValue(new ResultValue("Match Count", "", gridAcceptanceScore, 0, goodCount));
            }

            return algorithmResult.IsGood();
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            resultMessage.BeginTable(null, "Item", "Value", "Good Range");
            if (algorithmResult.IsNG())
            {
                resultMessage.AddTextLine(string.Format("Not Matched"));
                return;
            }

            ResultValue resultValueMatchCount = algorithmResult.GetResultValue("Match Count");

            resultMessage.AddTableRow("Result", algorithmResult.GetGoodNgStr());

            var colorCheckerParam = (ColorCheckerParam)Param;
            //if (colorCheckerParam.ScoreType == ScoreType.Ratio)
            //{
            //    resultMessage.AddTableRow("Matched Index", resultValue.Value.ToString());
            //    resultMessage.AddTableRow("Match Distance", resultValue2.Value.ToString());
            //}
            //else
            //{

            //}
            //resultMessage.EndTable();

            //resultMessage.BeginTable(null, "Color Index", " Distance", "Result");

            //for (int i = 0; i < algorithmResult.SubResultList.Count; i++)
            //{
            //    ColorCheckerResult colorResult = (ColorCheckerResult)algorithmResult.SubResultList[i];
            //    resultMessage.AddTableRow((i + 1).ToString(), colorResult.MatchDistance.ToString(), (colorResult.Result == true ? "Match" : ""));
            //}

            resultMessage.EndTable();
        }
    }

    public class ColorCheckerResult : AlgorithmResult
    {
        protected ColorCheckerParam param = null;
        public ColorCheckerParam Param
        {
            get => param;
            set => param = value;
        }
        public int MatchedIndex { get; set; }
        public float MatchValue { get; set; }
        public float MatchDistance { get; set; }
        public float MatchRatio { get; set; }

        public ColorCheckerResult(ColorCheckerParam param)
        {
            this.param = param;

            MatchedIndex = -1;
            MatchDistance = -1;
        }
    }
}
