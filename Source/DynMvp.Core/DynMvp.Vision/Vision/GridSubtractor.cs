using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
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
    public class GridSubtractorParam : AlgorithmParam
    {
        private List<Image2D> maskImageList;
        public List<Image2D> MaskImageList
        {
            get => maskImageList;
            set => MaskImageList = value;
        }
        public int MinThreshold { get; set; }
        public int MaxThreshold { get; set; }
        public int MinCount { get; set; }
        public float ProjectionRatio { get; set; }
        public bool Invert { get; set; }
        public int MaxOffsetX { get; set; }
        public int MaxOffsetY { get; set; }
        public int SearchRangeX { get; set; }
        public int SearchRangeY { get; set; }
        public int MatchScore { get; set; }
        public bool UseWholeImage { get; set; }

        public GridSubtractorParam()
        {
            maskImageList = new List<Image2D>();

            MinCount = 0;

            MinThreshold = 100;
            MaxThreshold = 150;

            ProjectionRatio = 0.005f;
            Invert = false;

            MaxOffsetX = 20;
            MaxOffsetY = 20;
            SearchRangeX = 50;
            SearchRangeY = 50;
            MatchScore = 50;

            UseWholeImage = false;
        }

        public override AlgorithmParam Clone()
        {
            var param = new GridSubtractorParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (GridSubtractorParam)srcAlgorithmParam;

            MinCount = param.MinCount;
            ProjectionRatio = param.ProjectionRatio;
            Invert = param.Invert;

            MatchScore = param.MatchScore;

            MaxOffsetX = param.MaxOffsetX;
            MaxOffsetY = param.MaxOffsetY;
            SearchRangeX = param.SearchRangeX;
            SearchRangeY = param.SearchRangeY;
            MinThreshold = param.MinThreshold;
            MaxThreshold = param.MaxThreshold;
            UseWholeImage = param.UseWholeImage;

            RemoveAllMaskImage();

            maskImageList.AddRange(param.MaskImageList);
        }

        public void AddMaskImage(ImageD maskImage)
        {
            maskImageList.Add((Image2D)maskImage);
        }

        public void RemoveMaskImage(int index)
        {
            maskImageList[index].Dispose();
            maskImageList.RemoveAt(index);
        }

        public void RemoveAllMaskImage()
        {
            maskImageList.Clear();
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            MinCount = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinCount", "0"));

            ProjectionRatio = Convert.ToSingle(XmlHelper.GetValue(paramElement, "ProjectionRatio", "0"));
            Invert = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "Invert", "false"));

            MaxOffsetX = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxOffsetX", "0"));
            MaxOffsetY = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxOffsetY", "0"));

            SearchRangeX = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeX", "20"));
            SearchRangeY = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeY", "20"));

            MinThreshold = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinThreshold", "100"));
            MaxThreshold = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxThreshold", "150"));

            MatchScore = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MatchScore", "0"));
            UseWholeImage = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "WholeImage", "False"));

            foreach (XmlElement maskElement in paramElement)
            {
                if (maskElement.Name == "MaskImage")
                {
                    Bitmap maskBitmap = ImageHelper.Base64StringToBitmap(maskElement.InnerText);
                    if (maskBitmap != null)
                    {
                        var maskImage = Image2D.ToImage2D(maskBitmap);
                        maskBitmap.Dispose();
                        maskImageList.Add(maskImage);
                    }
                }
            }
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "MinCount", MinCount.ToString());

            XmlHelper.SetValue(paramElement, "ProjectionRatio", ProjectionRatio.ToString());
            XmlHelper.SetValue(paramElement, "Invert", Invert.ToString());

            XmlHelper.SetValue(paramElement, "MaxOffsetX", MaxOffsetX.ToString());
            XmlHelper.SetValue(paramElement, "MaxOffsetY", MaxOffsetY.ToString());

            XmlHelper.SetValue(paramElement, "SearchRangeX", SearchRangeX.ToString());
            XmlHelper.SetValue(paramElement, "SearchRangeY", SearchRangeY.ToString());

            XmlHelper.SetValue(paramElement, "MinThreshold", MinThreshold.ToString());
            XmlHelper.SetValue(paramElement, "MaxThreshold", MaxThreshold.ToString());

            XmlHelper.SetValue(paramElement, "MatchScore", MatchScore.ToString());
            XmlHelper.SetValue(paramElement, "WholeImage", UseWholeImage.ToString());

            foreach (Image2D maskImage in maskImageList)
            {
                var patternBitmap = maskImage.ToBitmap();
                string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
                patternBitmap.Dispose();

                XmlHelper.SetValue(paramElement, "MaskImage", patternImageString);
            }
        }
    }

    public class GridSubtractorResult
    {
        public PatternResult PatternResult { get; set; }
        public List<GridResult> GridResultList { get; set; } = new List<GridResult>();
    }

    public class GridResult
    {
        public int No { get; set; }
        public PatternResult PatternResult { get; set; }
        public Rectangle GridRect { get; set; }
        public List<BlobRect> InBlobDefectList { get; set; }
        public List<BlobRect> OutBlobDefectList { get; set; }
    }

    public class GridSubtractor : Algorithm, Searchable
    {
        public enum DefectType
        {
            NotDefect, NotFound, OffsetError, InBlob, OutBlob, Contour
        }

        public GridSubtractor()
        {
            Param = new GridSubtractorParam();
            //FilterList.Add(new BinarizeFilter(BinarizationType.SingleThreshold, 100));
        }

        public override void Clear()
        {
        }

        public override Algorithm Clone()
        {
            var gridSubtractor = new GridSubtractor();
            gridSubtractor.Copy(this);

            return gridSubtractor;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var gridSubtractor = (GridSubtractor)algorithm;
            param = (GridSubtractorParam)gridSubtractor.Param.Clone();
        }

        public const string TypeName = "GridSubtractor";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "BlobSub";
        }

        public override string[] GetPreviewNames()
        {
            return new string[] { "Region", "Defect" };
        }

        public override void Filter(AlgoImage algoImage, int previewFilterType = 0, int targetLightTypeIndex = -1)
        {
            //algoImage.Save(@"d:\preFilter.bmp", null);

            var gridSubtractorParam = (GridSubtractorParam)param;

            AlgoImage srcAlgoImage = algoImage;

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(srcAlgoImage);

            if (previewFilterType == 0)
            {
                imageProcessing.Binarize(srcAlgoImage, srcAlgoImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.Invert);
            }
            else if (previewFilterType == 1)
            {
                imageProcessing.Binarize(srcAlgoImage, srcAlgoImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.MaxThreshold, gridSubtractorParam.Invert);
            }
        }

        public Size GetSearchRangeSize()
        {
            var gridSubtractorParam = (GridSubtractorParam)param;

            return new Size(gridSubtractorParam.MaxOffsetX, gridSubtractorParam.MaxOffsetY);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            var gridSubtractorParam = (GridSubtractorParam)param;
            gridSubtractorParam.SearchRangeX = searchRange.Width;
            gridSubtractorParam.SearchRangeY = searchRange.Height;
        }

        public bool UseWholeImage
        {
            get
            {
                var gridSubtractorParam = (GridSubtractorParam)param;

                return gridSubtractorParam.UseWholeImage;
            }
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            var gridSubtractorParam = (GridSubtractorParam)param;

            //찾는건 넓게
            int inflateWidth = gridSubtractorParam.SearchRangeX;
            int inflateHeight = gridSubtractorParam.SearchRangeY;

            newInspRegion.Inflate(inflateWidth, inflateHeight);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();

            var gridSubtractorParam = (GridSubtractorParam)Param;

            resultValues.Add(new ResultValue("Total Count", "", gridSubtractorParam.MinCount));
            resultValues.Add(new ResultValue("Defect Type", "", DefectType.NotFound));

            resultValues.Add(new ResultValue("Matching Pos", "", 100, gridSubtractorParam.MatchScore, 0, 0));
            resultValues.Add(new ResultValue("Max Offset X", "", gridSubtractorParam.MaxOffsetX, 0));
            resultValues.Add(new ResultValue("Max Offset Y", "", gridSubtractorParam.MaxOffsetY, 0));
            resultValues.Add(new ResultValue("Real Offset", "", new SizeF(0, 0)));

            return resultValues;
        }

        public AlgoImage GetRotateImage(AlgoImage algoImage, RotatedRect rotatedRect)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

            var boundRect = Rectangle.Round(rotatedRect.GetBoundRect());
            var imageRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            if (Rectangle.Intersect(boundRect, imageRect) != boundRect)
            {
                return null;
            }

            AlgoImage boundImage = algoImage.Clip(boundRect);

            var boundImageRect = new RectangleF(0, 0, boundRect.Width, boundRect.Height);
            PointF centerPt = DrawingHelper.CenterPoint(boundImageRect);

            RectangleF centerRect = DrawingHelper.FromCenterSize(centerPt, new SizeF(rotatedRect.Width, rotatedRect.Height));

            imageProcessing.Rotate(boundImage, boundImage, centerPt, -rotatedRect.Angle);

            AlgoImage rotatedImage = boundImage.Clip(Rectangle.Round(centerRect));
            boundImage.Dispose();

            return rotatedImage;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            DebugContext debugContext = inspectParam.DebugContext;

            var gridSubtractorParam = (GridSubtractorParam)Param;

            var algorithmResult = new AlgorithmResult();


            if (gridSubtractorParam.MaskImageList.Count == 0)
            {
                algorithmResult.AddResultValue(new ResultValue("Exception", "", "No mask"));
                return algorithmResult;
            }

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);

            bool patternFound = true;
            bool gridFound = false;

            var gridSubtractorResultList = new List<GridSubtractorResult>();

            foreach (Image2D maskImage in gridSubtractorParam.MaskImageList)
            {
                AlgoImage maskAlgoImage = ImageBuilder.Build(GetAlgorithmType(), maskImage, ImageType.Grey);
                //Filter(maskAlgoImage);

                PatternResult patternResult = PatternMatching(clipImage, maskImage, debugContext);
                if (patternResult.MaxScore * 100 < gridSubtractorParam.MatchScore)
                {
                    patternFound = false;
                    continue;
                }

                List<Rectangle> gridList = GetGridRectList(maskImage);

                if (gridList.Count == 0)
                {
                    continue;
                }

                gridFound = true;

                int gridIndex = 0;

                var boundRect = Rectangle.Round(patternResult.ResultRect.GetBoundRect());

                var clipRect = new Rectangle(0, 0, clipImage.Width, clipImage.Height);
                boundRect.Intersect(clipRect);

                AlgoImage boundClipImage = clipImage.Clip(boundRect);

                var gridSubtractorResult = new GridSubtractorResult();
                foreach (Rectangle grid in gridList)
                {
                    if (Rectangle.Intersect(grid, new Rectangle(0, 0, maskImage.Width, maskImage.Height)) != grid)
                    {
                        continue;
                    }

                    var maskGridImage = (Image2D)maskImage.ClipImage(grid);
                    PatternResult gridPatternResult = null;
                    AlgoImage gridAlignImage = GridAlign(boundClipImage, maskGridImage, debugContext, ref gridPatternResult);
                    maskGridImage.Dispose();

                    if (gridAlignImage == null)
                    {
                        patternFound = false;
                        continue;
                    }


                    var gridAlignRect = new Rectangle(grid.X, grid.Y, gridAlignImage.Width, gridAlignImage.Height);
                    AlgoImage gridMaskImage = maskAlgoImage.Clip(gridAlignRect);

                    GridResult gridResult = GetGridResult(gridIndex, grid, gridAlignImage, gridMaskImage);
                    gridResult.PatternResult = gridPatternResult;

                    gridMaskImage.Dispose();

                    gridSubtractorResult.GridResultList.Add(gridResult);
                    gridIndex++;
                }

                boundClipImage.Dispose();


                maskAlgoImage.Dispose();

                gridSubtractorResult.PatternResult = patternResult;

                if (gridSubtractorResult.GridResultList.Count > 0)
                {
                    gridSubtractorResultList.Add(gridSubtractorResult);
                }
            }

            clipImage.Dispose();

            if (gridFound == false)
            {
                algorithmResult.AddResultValue(new ResultValue("Exception", "", "Grid not found"));
                algorithmResult.BriefMessage = "Pattern NG";
                return algorithmResult;
            }

            // Find Min Count && Max Grid Num\
            var gridCount = new List<int>();
            GridSubtractorResult minCountGridResultList = FindminCountGridSubtractorResult(gridSubtractorResultList, ref gridCount);

            if (minCountGridResultList == null)
            {
                algorithmResult.AddResultValue(new ResultValue("Exception", "", "Rotate error"));
                algorithmResult.BriefMessage = "Pattern NG";
                return algorithmResult;
            }

            bool good = true;

            MatchPos matchPos = minCountGridResultList.PatternResult.MaxMatchPos;
            var infaltePos = new PointF(gridSubtractorParam.SearchRangeX, gridSubtractorParam.SearchRangeY);
            var matchPosInFov = new PointF(matchPos.Pos.X - infaltePos.X, matchPos.Pos.Y - infaltePos.Y);

            PointF probeCenter = DrawingHelper.CenterPoint(inspectParam.ProbeRegionInFov);
            probeCenter = new PointF(probeCenter.X - inspectParam.ProbeRegionInFov.X, probeCenter.Y - inspectParam.ProbeRegionInFov.Y);

            matchPosInFov.X = matchPosInFov.X - probeCenter.X;
            matchPosInFov.Y = matchPosInFov.Y - probeCenter.Y;

            RotatedRect resultRect = inspectParam.ProbeRegionInFov;
            resultRect.Offset(matchPosInFov.X, matchPosInFov.Y);
            algorithmResult.ResultRect = resultRect;

            bool offsetNG = Math.Abs(matchPosInFov.X) > gridSubtractorParam.MaxOffsetX || Math.Abs(matchPosInFov.Y) > gridSubtractorParam.MaxOffsetY;

            if (offsetNG == true)
            {
                algorithmResult.AddResultValue(new ResultValue("Offset NG", ""));
                algorithmResult.BriefMessage = "Offset NG";
                return algorithmResult;
            }

            algorithmResult.AddResultValue(new ResultValue("X offset", "", gridSubtractorParam.MaxOffsetX, 0, matchPosInFov.X, offsetNG));
            algorithmResult.AddResultValue(new ResultValue("Y offset", "", gridSubtractorParam.MaxOffsetY, 0, matchPosInFov.Y, offsetNG));

            var subResultList = new SubResultList();

            foreach (GridResult minCountGridResult in minCountGridResultList.GridResultList)
            {
                int inBlobCount = 0;
                int outBlobCount = 0;

                foreach (BlobRect blobRect in minCountGridResult.InBlobDefectList)
                {
                    if (inBlobCount < (int)blobRect.Area)
                    {
                        inBlobCount = (int)blobRect.Area;
                    }
                }


                foreach (BlobRect blobRect in minCountGridResult.OutBlobDefectList)
                {
                    if (outBlobCount < (int)blobRect.Area)
                    {
                        outBlobCount = (int)blobRect.Area;
                    }
                }

                float value = Math.Max(inBlobCount, outBlobCount);

                var subResult = new SubResult("BlobResult", value);

                PointF gridCenter = DrawingHelper.CenterPoint(minCountGridResult.GridRect);
                PointF gridRotateOffset = MathHelper.Rotate(gridCenter, probeCenter, matchPos.Angle);

                gridRotateOffset.X = gridRotateOffset.X - gridCenter.X;
                gridRotateOffset.Y = gridRotateOffset.Y - gridCenter.Y;

                var gridRect = new RotatedRect(minCountGridResult.GridRect, matchPos.Angle);
                gridRect.Offset(gridRotateOffset);

                bool gridResult = true;

                if (inBlobCount > gridSubtractorParam.MinCount || outBlobCount > gridSubtractorParam.MinCount)
                {
                    algorithmResult.BriefMessage = "Hole NG";
                    gridResult = false;
                    good = false;
                }

                algorithmResult.AddResultValue(new ResultValue("Count", minCountGridResult.No.ToString(), gridSubtractorParam.MinCount, 0, Math.Max(inBlobCount, outBlobCount), gridResult));
                subResult.FigureGroup.AddFigure(new RectangleFigure(gridRect, new Pen(gridResult == true ? Color.Green : Color.Red)));

                foreach (BlobRect blobRect in minCountGridResult.InBlobDefectList)
                {
                    if (blobRect.Area <= gridSubtractorParam.MinCount)
                    {
                        continue;
                    }

                    RectangleF offsetRect = blobRect.BoundingRect;
                    offsetRect.Offset(minCountGridResult.GridRect.Location);
                    offsetRect.Offset(gridRotateOffset);
                    subResult.FigureGroup.AddFigure(new RectangleFigure(offsetRect, new Pen(Color.Blue)));
                }

                foreach (BlobRect blobRect in minCountGridResult.OutBlobDefectList)
                {
                    if (blobRect.Area <= gridSubtractorParam.MinCount)
                    {
                        continue;
                    }

                    RectangleF offsetRect = blobRect.BoundingRect;
                    offsetRect.Offset(minCountGridResult.GridRect.Location);
                    offsetRect.Offset(gridRotateOffset);
                    subResult.FigureGroup.AddFigure(new RectangleFigure(offsetRect, new Pen(Color.Yellow)));
                }

                subResultList.AddSubResult(subResult);
            }

            algorithmResult.AddResultValue(new ResultValue("SubResultList", "", null, subResultList));

            if (patternFound == false)
            {
                algorithmResult.AddResultValue(new ResultValue("Pattern NG", "", "Pattern not found"));
                algorithmResult.BriefMessage = "Pattern NG";
                return algorithmResult;
            }

            algorithmResult.SetResult(offsetNG == false ? good : offsetNG);

            return algorithmResult;
        }

        private GridSubtractorResult FindminCountGridSubtractorResult(List<GridSubtractorResult> GridSubtractorResultList, ref List<int> gridCount)
        {
            GridSubtractorResult minCountGridSubtractorResult = null;

            int maxGridNum = 0;

            float minCount = float.MaxValue;

            foreach (GridSubtractorResult gridSubtractorResult in GridSubtractorResultList)
            {
                if (maxGridNum <= gridSubtractorResult.GridResultList.Count)
                {
                    maxGridNum = gridSubtractorResult.GridResultList.Count;

                    var sumList = new List<int>();

                    foreach (GridResult gridResult in gridSubtractorResult.GridResultList)
                    {
                        float sum = 0;

                        foreach (BlobRect blobRect in gridResult.InBlobDefectList)
                        {
                            sum += blobRect.Area;
                        }

                        foreach (BlobRect blobRect in gridResult.OutBlobDefectList)
                        {
                            sum += blobRect.Area;
                        }

                        sumList.Add((int)sum);
                    }

                    int gridSum = sumList.Sum();
                    if (gridSum < minCount)
                    {
                        minCount = gridSum;
                        minCountGridSubtractorResult = gridSubtractorResult;

                        gridCount = sumList;
                    }
                }
            }

            return minCountGridSubtractorResult;
        }

        private AlgoImage GridAlign(AlgoImage clipImage, Image2D maskGridImage, DebugContext debugContext, ref PatternResult patternResult)
        {
            var gridSubtractorParam = (GridSubtractorParam)param;

            patternResult = PatternMatching(clipImage, maskGridImage, debugContext);
            if (patternResult.MaxScore * 100 < gridSubtractorParam.MatchScore)
            {
                return null;
            }

            var boundRect = Rectangle.Round(patternResult.ResultRect.GetBoundRect());
            var imageRect = new Rectangle(0, 0, clipImage.Width, clipImage.Height);

            if (Rectangle.Intersect(boundRect, imageRect) != boundRect)
            {
                return null;
            }

            AlgoImage boundImage = clipImage.Clip(boundRect);

            var boundImageRect = new RectangleF(0, 0, boundRect.Width, boundRect.Height);
            PointF centerPt = DrawingHelper.CenterPoint(boundImageRect);

            RectangleF centerRect = DrawingHelper.FromCenterSize(centerPt, new SizeF(patternResult.ResultRect.Width, patternResult.ResultRect.Height));

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);
            imageProcessing.Rotate(boundImage, boundImage, centerPt, -patternResult.ResultRect.Angle);

            AlgoImage rotatedImage = boundImage.Clip(Rectangle.Round(centerRect));
            boundImage.Dispose();

            return rotatedImage;
        }

        private PatternResult PatternMatching(AlgoImage clipImage, Image2D maskImage, DebugContext debugContext)
        {
            var blobSubtractorParam = (GridSubtractorParam)Param;

            var patternMatchingParam = new PatternMatchingParam();
            patternMatchingParam.MinAngle = -30;
            patternMatchingParam.MaxAngle = 30;

            Pattern pattern = AlgorithmFactory.Instance().CreatePattern();

            pattern.Train(maskImage, patternMatchingParam);
            PatternResult patternResult = pattern.Inspect(clipImage, patternMatchingParam, debugContext);

            pattern.Dispose();

            return patternResult;
        }

        private GridResult GetGridResult(int gridIndex, Rectangle grid, AlgoImage rotatedClipImage, AlgoImage maskClipImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(rotatedClipImage);
            var gridSubtractorParam = (GridSubtractorParam)param;

            int width = rotatedClipImage.Width;
            int height = rotatedClipImage.Height;

            // Image Alloc
            AlgoImage subtractionImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
            AlgoImage contourImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
            AlgoImage blobImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
            AlgoImage backgroundImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
            AlgoImage resultImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
            AlgoImage defectImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);

            imageProcessing.Binarize(rotatedClipImage, defectImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.MaxThreshold, gridSubtractorParam.Invert);

            float clipGray = imageProcessing.GetGreyAverage(rotatedClipImage);
            float maskGray = imageProcessing.GetGreyAverage(maskClipImage);

            imageProcessing.Binarize(rotatedClipImage, rotatedClipImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.Invert); //Filter(rotatedClipImage);

            float maskTh = gridSubtractorParam.MinThreshold - (clipGray - maskGray);
            imageProcessing.Binarize(maskClipImage, maskClipImage, (int)Math.Round(maskTh), gridSubtractorParam.Invert);

            // Create Mask - In, Out, Contour

            //AlignBlobCenter(maskGridImage, clipGridImage);

            //AlignBlobCenter(maskGridImage, clipGridImage);
            // Subtract

            imageProcessing.Subtract(maskClipImage, rotatedClipImage, subtractionImage, true);
            CreateBlobMask(maskClipImage, blobImage, contourImage, backgroundImage);
            imageProcessing.Subtract(defectImage, contourImage, defectImage);

            var gridSubtractorResult = new GridResult();
            gridSubtractorResult.GridRect = grid;
            gridSubtractorResult.No = gridIndex;

            // get value
            var blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectBoundingRect = true;

            // In Blob
            imageProcessing.And(defectImage, blobImage, defectImage);
            imageProcessing.And(subtractionImage, blobImage, resultImage);
            imageProcessing.Or(resultImage, defectImage, resultImage);

            BlobRectList inBlobRectList = imageProcessing.Blob(resultImage, blobParam);
            gridSubtractorResult.InBlobDefectList = inBlobRectList.GetList();
            inBlobRectList.Dispose();
            // Out blob
            imageProcessing.And(subtractionImage, backgroundImage, resultImage);
            BlobRectList outBlobRectList = imageProcessing.Blob(resultImage, blobParam);
            gridSubtractorResult.OutBlobDefectList = outBlobRectList.GetList();
            outBlobRectList.Dispose();
            //contourImage.Save("contourImage.bmp", new DebugContext(true, "d:\\"));
            //Dispose
            subtractionImage.Dispose();
            contourImage.Dispose();
            blobImage.Dispose();
            backgroundImage.Dispose();
            resultImage.Dispose();
            defectImage.Dispose();

            return gridSubtractorResult;
        }

        private void AlignBlobCenter(AlgoImage maskGridImage, AlgoImage clipGridImage)
        {
            var gridSubtractorParam = (GridSubtractorParam)param;

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(maskGridImage);

            AlgoImage maskBinImage = maskGridImage.Clone();
            AlgoImage clipBinImage = clipGridImage.Clone();

            imageProcessing.Binarize(maskBinImage, maskBinImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.Invert);
            imageProcessing.Binarize(clipBinImage, clipBinImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.Invert);

            var blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = false;

            BlobRectList maskBlobRectList = imageProcessing.Blob(maskGridImage, blobParam);
            List<BlobRect> maskBlobList = maskBlobRectList.GetList();
            maskBlobRectList.Dispose();

            double maskSumCX = 0;
            double maskSumCY = 0;
            double maskSum = 0;

            foreach (BlobRect maskBlob in maskBlobList)
            {
                maskSum += maskBlob.Area;
                maskSumCX += maskBlob.CenterPt.X * maskBlob.Area;
                maskSumCY += maskBlob.CenterPt.Y * maskBlob.Area;
            }

            var maskCenter = new PointF((float)(maskSumCX / maskSum), (float)(maskSumCY / maskSum));

            BlobRectList clipBlobRectList = imageProcessing.Blob(clipGridImage, blobParam);
            List<BlobRect> clipBlobList = clipBlobRectList.GetList();
            clipBlobRectList.Dispose();

            clipBinImage.Dispose();
            maskBinImage.Dispose();

            double clipSumCX = 0;
            double clipSumCY = 0;
            double clipSum = 0;

            foreach (BlobRect clipBlob in clipBlobList)
            {
                clipSum += clipBlob.Area;
                clipSumCX += clipBlob.CenterPt.X * clipBlob.Area;
                clipSumCY += clipBlob.CenterPt.Y * clipBlob.Area;
            }

            var clipCenter = new PointF((float)(clipSumCX / clipSum), (float)(clipSumCY / clipSum));

            var alignOffset = new PointF(maskCenter.X - clipCenter.X, maskCenter.Y - clipCenter.Y);

            imageProcessing.Translate(clipGridImage, clipGridImage, Point.Round(alignOffset));
        }

        private void CreateBlobMask(AlgoImage binaryImage, AlgoImage blobImage, AlgoImage contourImage, AlgoImage backgroundImage)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(binaryImage);

            //imageProcessing.FillHoles(binaryImage, binaryImage);

            var blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectBoundingRect = true;
            //blobParam.EraseBorderBlob = true;

            BlobRectList blobRectList = imageProcessing.Blob(binaryImage, blobParam);
            var drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;
            imageProcessing.DrawBlob(blobImage, blobRectList, null, drawBlobOption);

            var drawBlobContourOption = new DrawBlobOption();
            drawBlobContourOption.SelectBlobContour = true;
            drawBlobContourOption.SelectHolesContour = true;
            imageProcessing.DrawBlob(contourImage, blobRectList, null, drawBlobContourOption);
            blobRectList.Dispose();

            imageProcessing.Dilate(contourImage, 1);
            imageProcessing.Subtract(blobImage, contourImage, blobImage);

            imageProcessing.Dilate(contourImage, 1);
            imageProcessing.Not(blobImage, backgroundImage);
            imageProcessing.Subtract(backgroundImage, contourImage, backgroundImage);

            imageProcessing.Dilate(contourImage, 5);
            imageProcessing.And(contourImage, backgroundImage, backgroundImage);
        }

        private List<Rectangle> GetGridSubRectList(AlgoImage algoImage, List<Rectangle> xRectList)
        {
            int width = algoImage.Width;

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var gridSubtractorParam = (GridSubtractorParam)param;

            var subRectList = new List<Rectangle>();

            var imageRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            foreach (Rectangle xRect in xRectList)
            {
                AlgoImage xRectImage = algoImage.Clip(xRect);

                float[] yProjectionArray = imageProcessing.Projection(xRectImage, TwoWayDirection.Vertical, ProjectionType.Sum);

                xRectImage.Dispose();

                int startPosY = 0;
                int endPosY = 0;

                for (int y = 0; y < yProjectionArray.Length; y++)
                {
                    bool forground = false;

                    if (gridSubtractorParam.Invert == false)
                    {
                        if (yProjectionArray[y] / width >= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }
                    else
                    {
                        if (yProjectionArray[y] / width <= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }

                    if (forground == true)
                    {
                        startPosY = y;
                        break;
                    }
                }

                for (int y = yProjectionArray.Length - 1; y >= 0; y--)
                {
                    bool forground = false;

                    if (gridSubtractorParam.Invert == false)
                    {
                        if (yProjectionArray[y] / width >= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }
                    else
                    {
                        if (yProjectionArray[y] / width <= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }

                    if (forground == true)
                    {
                        endPosY = y;
                        break;
                    }
                }

                xRectImage.Dispose();

                if (endPosY - startPosY > 0)
                {
                    var gridRect = new Rectangle(xRect.X, xRect.Y + startPosY, xRect.Width, endPosY - startPosY);
                    gridRect.Inflate(5, 5);

                    gridRect.Intersect(imageRect);

                    subRectList.Add(gridRect);
                }
            }

            return subRectList;
        }

        private List<Rectangle> GetGridRectSubList(AlgoImage algoImage, List<Rectangle> yRectList)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var gridSubtractorParam = (GridSubtractorParam)param;

            int width = algoImage.Width;
            int height = algoImage.Height;

            var xRectList = new List<Rectangle>();

            int gapTh = 5;

            foreach (Rectangle yRect in yRectList)
            {
                AlgoImage yRectImage = algoImage.Clip(yRect);
                float[] xProjectionArray = imageProcessing.Projection(yRectImage, TwoWayDirection.Horizontal);
                yRectImage.Dispose();

                bool startScan = false;
                int startPosX = 0;
                int endPosX = 0;

                for (int x = 0; x < xProjectionArray.Length; x++)
                {
                    bool forground = false;

                    if (gridSubtractorParam.Invert == false)
                    {
                        if (xProjectionArray[x] / height >= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }
                    else
                    {
                        if (xProjectionArray[x] / height <= gridSubtractorParam.ProjectionRatio)
                        {
                            forground = true;
                        }
                    }

                    if (forground == true)
                    {
                        if (startScan == false)
                        {
                            startScan = true;
                        }

                        endPosX = x;
                    }
                    else
                    {
                        if (startScan == false)
                        {
                            startPosX = x;
                        }
                        else
                        {
                            if (endPosX - startPosX > 0)
                            {
                                if (x - endPosX > gapTh)
                                {
                                    xRectList.Add(new Rectangle(startPosX, yRect.Y, endPosX - startPosX, yRect.Height));
                                    startScan = false;
                                }
                            }
                        }
                    }
                }

                if (startScan == true)
                {
                    if (endPosX - startPosX > 0)
                    {
                        xRectList.Add(new Rectangle(startPosX, yRect.Y, endPosX - startPosX, yRect.Height));
                    }
                }
            }

            List<Rectangle> gridRectList = GetGridSubRectList(algoImage, xRectList);

            return gridRectList;
        }

        private List<Rectangle> GetGridRectList(Image2D maskImage)
        {
            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), maskImage, ImageType.Grey);
            //Filter(algoImage);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var gridSubtractorParam = (GridSubtractorParam)param;

            imageProcessing.Binarize(algoImage, algoImage, gridSubtractorParam.MinThreshold, gridSubtractorParam.Invert);

            int width = maskImage.Width;
            int height = maskImage.Height;

            var yRectList = new List<Rectangle>();

            float[] yProjectionArray = imageProcessing.Projection(algoImage, TwoWayDirection.Vertical);

            bool startScan = false;
            int startPosY = 0;
            int endPosY = 0;

            for (int y = 0; y < yProjectionArray.Length; y++)
            {
                bool forground = false;

                if (gridSubtractorParam.Invert == false)
                {
                    if (yProjectionArray[y] / width >= gridSubtractorParam.ProjectionRatio)
                    {
                        forground = true;
                    }
                }
                else
                {
                    if (yProjectionArray[y] / width <= gridSubtractorParam.ProjectionRatio)
                    {
                        forground = true;
                    }
                }

                if (forground == true)
                {
                    if (startScan == false)
                    {
                        startScan = true;
                    }

                    endPosY = y;
                }
                else
                {
                    if (startScan == false)
                    {
                        startPosY = y;
                    }
                    else
                    {
                        if (endPosY - startPosY > 0)
                        {
                            yRectList.Add(new Rectangle(0, startPosY, width, endPosY - startPosY));
                        }

                        startScan = false;
                    }
                }
            }

            if (startScan == true)
            {
                if (endPosY - startPosY > 0)
                {
                    yRectList.Add(new Rectangle(0, startPosY, width, endPosY - startPosY));
                }
            }

            List<Rectangle> gridRectList = GetGridRectSubList(algoImage, yRectList);

            algoImage.Dispose();

            return gridRectList;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {

        }
    }
}
