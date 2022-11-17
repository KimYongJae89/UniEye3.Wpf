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
    public class BinaryCounterConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(BinaryCounter))
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
                 value is BinaryCounter)
            {
                var binaryCounter = (BinaryCounter)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(BinaryCounterConverter))]
    public class BinaryCounterParam : AlgorithmParam
    {
        public int MaxScore { get; set; } = 100;
        public int MinScore { get; set; } = 50;
        public ScoreType ScoreType { get; set; }
        public GridParam GridParam { get; private set; } = new GridParam();

        public bool UseGrid => GridParam.UseGrid;
        public int RowCount => GridParam.RowCount;
        public int ColumnCount => GridParam.ColumnCount;
        public float CellAcceptRatio => GridParam.CellAcceptRatio;

        public override AlgorithmParam Clone()
        {
            var param = new BinaryCounterParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BinaryCounterParam)srcAlgorithmParam;

            MaxScore = param.MaxScore;
            MinScore = param.MinScore;
            ScoreType = param.ScoreType;
            GridParam = param.GridParam.Clone();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            MaxScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxScore", "100"));
            MinScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinScore", "50"));
            ScoreType = (ScoreType)Enum.Parse(typeof(ScoreType), XmlHelper.GetValue(algorithmElement, "ScoreType", "Ratio"));

            GridParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MaxScore", MaxScore.ToString());
            XmlHelper.SetValue(algorithmElement, "MinScore", MinScore.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreType", ScoreType.ToString());

            GridParam.SaveParam(algorithmElement);
        }
    }

    [TypeConverterAttribute(typeof(BinaryCounterConverter))]
    public class BinaryCounter : Algorithm
    {
        public BinaryCounter()
        {
            param = new BinaryCounterParam();

            IFilter filter = new BinarizeFilter(BinarizationType.SingleThreshold, 100);
            filter.EssentialFilter = true;
            FilterList.Add(filter);
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            var binaryCounter = new BinaryCounter();
            binaryCounter.Copy(this);

            return binaryCounter;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var binaryCounter = (BinaryCounter)algorithm;

            param = (BinaryCounterParam)binaryCounter.Param.Clone();
        }

        public const string TypeName = "BinaryCounter";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Binary";
        }

        public override void Filter(AlgoImage algoImage, int previewFilterType = 0, int targetLightTypeIndex = -1)
        {
            base.Filter(algoImage, previewFilterType, targetLightTypeIndex);
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            var binaryCounterParam = (BinaryCounterParam)param;

            if (binaryCounterParam.UseGrid)
            {
                DrawingHelper.DrawGrid(probeRect, binaryCounterParam.RowCount, binaryCounterParam.ColumnCount, tempFigures, new Pen(Color.YellowGreen, 0));
            }
        }

        public override List<ResultValue> GetResultValues()
        {
            var binaryCounterParam = (BinaryCounterParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Score", "", binaryCounterParam.MaxScore, binaryCounterParam.MinScore, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            ImageType imageType = (inspectParam.InspectImageList[0] is Image3D ? ImageType.Depth : ImageType.Grey);

            ImageD clipImage = inspectParam.InspectImageList[0];
            AlgoImage probeClipImage = ImageBuilder.Build(GetAlgorithmType(), clipImage, imageType, param.ImageBand);
            Filter(probeClipImage, 0);

            DebugHelper.SaveImage(probeClipImage, "ProcImage.bmp", inspectParam.DebugContext);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(probeClipImage);

            var binaryCounterParam = (BinaryCounterParam)param;
            var algorithmResult = new AlgorithmResult();

            int pixelCount = 0;

            int area = 0;

            if (binaryCounterParam.GridParam.UseGrid)
            {
                area = binaryCounterParam.RowCount * binaryCounterParam.ColumnCount;
                pixelCount = CalcGrid(inspectParam, probeClipImage, algorithmResult);
            }
            else
            {
                area = clipImage.Width * clipImage.Height;
                if (inspectParam.ProbeRegionInFov.Angle != 0)
                {
                    ImageD rotatedMask = ImageHelper.GetRotateMask(clipImage.Width, clipImage.Height, inspectParam.ProbeRegionInFov);
                    AlgoImage rotatedAlgoMask = ImageBuilder.Build(BinaryCounter.TypeName, rotatedMask, ImageType.Grey, ImageBandType.Luminance);
                    pixelCount = imageProcessing.Count(probeClipImage, rotatedAlgoMask);
                }
                else
                {
                    pixelCount = imageProcessing.Count(probeClipImage);
                }
            }

            float score = 0;

            if (binaryCounterParam.ScoreType == ScoreType.Ratio)
            {
                if (area > 0)
                {
                    score = ((float)pixelCount) / area * 100;
                }
                else
                {
                    score = 0;
                }
            }
            else
            {
                score = pixelCount;
            }

            algorithmResult.ResultRect = inspectParam.ProbeRegionInFov;
            algorithmResult.SetResult((score >= binaryCounterParam.MinScore) && (score <= binaryCounterParam.MaxScore));

            algorithmResult.AddResultValue(new ResultValue("Score", "", binaryCounterParam.MaxScore, binaryCounterParam.MinScore, score));

            return algorithmResult;
        }

        private int CalcGrid(AlgorithmInspectParam inspectParam, AlgoImage probeClipImage, AlgorithmResult algorithmResult)
        {
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;

            RectangleF probeRect = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            var binaryCounterParam = (BinaryCounterParam)Param;
            GridParam gridParam = binaryCounterParam.GridParam;

            int gridCount = gridParam.GetNumCol() * gridParam.GetNumRow();
            float widthStep = probeRect.Width / gridParam.GetNumCol();
            float heightStep = probeRect.Height / gridParam.GetNumRow();

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(probeClipImage);

            int goodCount = 0;
            for (float y = probeRect.Y; y < probeRect.Y + probeRect.Height; y += heightStep)
            {
                for (float x = probeRect.X; x < probeRect.X + probeRect.Width; x += widthStep)
                {
                    var cellRect = new RectangleF(x, y, widthStep, heightStep);
                    AlgoImage subClipImage = probeClipImage.Clip(Rectangle.Truncate(cellRect));

                    int pixelCount = imageProcessing.Count(subClipImage);

                    float area = widthStep * heightStep;
                    float pixelRatio = 0;
                    if (area > 0)
                    {
                        pixelRatio = pixelCount / area * 100;
                    }
                    else
                    {
                        pixelRatio = 0;
                    }

                    if (pixelRatio > binaryCounterParam.GridParam.CellAcceptRatio)
                    {
                        goodCount++;
                    }
                    else
                    {
                        RotatedRect fovCellRect = DrawingHelper.ClipToFov(inspectRegionInFov, cellRect);
                        algorithmResult.ResultFigures.AddFigure(new XRectFigure(fovCellRect, new Pen(Color.Red, 1.0F)));
                    }
                }
            }

            return goodCount;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
