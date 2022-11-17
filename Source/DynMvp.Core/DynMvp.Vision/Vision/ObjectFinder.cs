using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.OpenCv;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class ObjectFinderParam : AlgorithmParam
    {
        public int SearchRangeWidth { get; set; }
        public int SearchRangeHeight { get; set; }
        public bool UseWholeImage { get; set; }
        public List<Image2D> PatternList { get; set; } = new List<Image2D>();

        public ObjectFinderParam()
        {
            SearchRangeWidth = 50;
            SearchRangeHeight = 50;
            UseWholeImage = false;
        }

        public override AlgorithmParam Clone()
        {
            var param = new ObjectFinderParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (ObjectFinderParam)srcAlgorithmParam;

            SearchRangeWidth = param.SearchRangeWidth;
            SearchRangeHeight = param.SearchRangeHeight;
            UseWholeImage = param.UseWholeImage;

            foreach (ImageD patternImage in param.PatternList)
            {
                PatternList.Add((Image2D)patternImage.Clone());
            }
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            SearchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "50"));
            SearchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "50"));
            UseWholeImage = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "WholeImage", "False"));

            foreach (XmlElement patternElement in algorithmElement)
            {
                if (patternElement.Name == "Pattern")
                {
                    string imageString = XmlHelper.GetValue(patternElement, "Image", "");
                    Bitmap patternImage = ImageHelper.Base64StringToBitmap(imageString);
                    if (patternImage != null)
                    {
                        AddPattern(Image2D.ToImage2D(patternImage));
                        patternImage.Dispose();
                    }
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "SearchRangeWidth", SearchRangeWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHeight", SearchRangeHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "WholeImage", UseWholeImage.ToString());

            foreach (Image2D patternImage in PatternList)
            {
                XmlElement patternImageElement = algorithmElement.OwnerDocument.CreateElement("", "Pattern", "");
                algorithmElement.AppendChild(patternImageElement);

                var bitmap = patternImage.ToBitmap();

                XmlHelper.SetValue(patternImageElement, "Image", ImageHelper.BitmapToBase64String(bitmap));
            }
        }

        public void AddPattern(Image2D patternImage)
        {
            PatternList.Add(patternImage);
        }

        public void RemovePattern(Image2D patternImage)
        {
            PatternList.Remove(patternImage);
        }

        public void RemoveAllPatterns()
        {
            PatternList.Clear();
        }
    }

    public class ObjectFinder : Algorithm, Searchable
    {
        public ObjectFinder()
        {
            param = new ObjectFinderParam();
        }

        public override Algorithm Clone()
        {
            var objectFinder = new ObjectFinder();
            objectFinder.Copy(this);

            return objectFinder;
        }

        public Size GetSearchRangeSize()
        {
            var objectFinderParam = (ObjectFinderParam)param;

            return new Size(objectFinderParam.SearchRangeWidth, objectFinderParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            var objectFinderParam = (ObjectFinderParam)param;

            objectFinderParam.SearchRangeWidth = searchRange.Width;
            objectFinderParam.SearchRangeHeight = searchRange.Height;
        }

        public bool UseWholeImage
        {
            get
            {
                var objectFinderParam = (ObjectFinderParam)param;

                return objectFinderParam.UseWholeImage;
            }
        }

        public void AddPattern(Image2D patternImage)
        {
            var objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.AddPattern(patternImage);
        }

        public void RemovePattern(Image2D patternImage)
        {
            var objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.RemovePattern(patternImage);
        }

        public void RemoveAllPatterns()
        {
            var objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.RemoveAllPatterns();
        }

        public static string TypeName => "ObjectFinder";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Object";
        }

        public override DynMvp.UI.RotatedRect AdjustInspectRegion(DynMvp.UI.RotatedRect inspRegion)
        {
            DynMvp.UI.RotatedRect newInspRegion = inspRegion;

            var objectFinderParam = (ObjectFinderParam)param;
            newInspRegion.Inflate(objectFinderParam.SearchRangeWidth, objectFinderParam.SearchRangeHeight);

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            DynMvp.UI.RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            DynMvp.UI.RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;

            var pmResult = new SearchableResult();

            double hessianThresh = 1000;

            lock (this)
            {
                var surfCPU = new SURF(hessianThresh);
                var modelKeyPoints = new VectorOfKeyPoint();
                var observedKeyPoints = new VectorOfKeyPoint();

                Mat homography = null;
                Mat mask;
                int k = 2;
                double uniquenessThreshold = 0.8;

                var openCvTargetImage = clipImage as OpenCvGreyImage;
                var objectFinderParam = (ObjectFinderParam)param;

                // extract features from the observed image
                var observedDescriptors = new UMat();
                surfCPU.DetectAndCompute(openCvTargetImage.Image, null, observedKeyPoints, observedDescriptors, false);

                var matches = new VectorOfVectorOfDMatch();

                var patternResultList = new List<PatternResult>();

                foreach (Image2D patternImage in objectFinderParam.PatternList)
                {
                    var openCvPattern = ImageBuilder.OpenCvImageBuilder.Build(patternImage, ImageType.Grey) as OpenCvGreyImage;

                    //extract features from the model image
                    var modelDescriptors = new UMat();
                    surfCPU.DetectAndCompute(openCvPattern.Image, null, modelKeyPoints, modelDescriptors, false);

                    var matcher = new BFMatcher(DistanceType.L2);
                    matcher.Add(modelDescriptors);

                    matcher.KnnMatch(observedDescriptors, matches, k, null);

                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));

                    Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                    int nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                        {
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
                        }
                    }

                    if (homography != null)
                    {
                        RectangleF findedRect = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);
                        PointF[] pts = DrawingHelper.GetPoints(findedRect, 0);
                        pts = CvInvoke.PerspectiveTransform(pts, homography);

                        var rectangle1 = RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[2].X, pts[2].Y);
                        var rectangle2 = RectangleF.FromLTRB(pts[3].X, pts[1].Y, pts[1].X, pts[3].Y);
                        var findedRectangle = RectangleF.Union(rectangle1, rectangle2);

                        var maxMatchPos = new PointF(findedRectangle.X + findedRectangle.Width / 2, findedRectangle.Y + findedRectangle.Height / 2);
                        maxMatchPos = DrawingHelper.ClipToFov(inspectRegionInFov, maxMatchPos);

                        var matchPos = new MatchPos(maxMatchPos, 1);
                        matchPos.PatternSize = openCvPattern.Image.Size;

                        var patternResult = new PatternResult();
                        patternResult.AddMatchPos(matchPos);

                        patternResultList.Add(patternResult);

                        if (pmResult.IsGood())
                        {
                            patternResult.Good = true;

                            PointF probeCenter = DrawingHelper.CenterPoint(probeRegionInFov);
                            PointF foundPosInFov = DrawingHelper.ClipToFov(inspectRegionInFov, matchPos.Pos);

                            var offset = new SizeF();
                            offset.Width = Math.Abs(maxMatchPos.X - probeCenter.X);
                            offset.Height = Math.Abs(maxMatchPos.Y - probeCenter.Y);

                            pmResult.OffsetFound = offset;

                            if (cameraCalibration != null && cameraCalibration.IsCalibrated())
                            {
                                PointF realRefPos = cameraCalibration.PixelToWorld(probeCenter);
                                PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

                                pmResult.RealOffsetFound = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
                            }

                            DynMvp.UI.RotatedRect resultRect = probeRegionInFov;
                            resultRect.Offset(offset.Width, offset.Height);
                            pmResult.ResultRect = resultRect;

                            pmResult.AddResultValue(new ResultValue("Matching Pos", "", matchPos.Pos));
                            pmResult.AddResultValue(new ResultValue("Offset", "", pmResult.OffsetFound));
                            pmResult.AddResultValue(new ResultValue("RealOffset", "", pmResult.RealOffsetFound));

                            pmResult.SetResult(true);

                            break;
                        }
                    }
                }

                pmResult.AddResultValue(new ResultValue("PatternResultList", "", patternResultList));
            }

            return pmResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            var pmResult = algorithmResult as SearchableResult;

            var matchingPos = (PointF)algorithmResult.GetResultValue("Matching Pos").Value;

            resultMessage.BeginTable(null, "Item", "Value");

            resultMessage.AddTableRow("Result", algorithmResult.GetGoodNgStr());
            resultMessage.AddTableRow("Matching Pos", matchingPos.ToString());
            resultMessage.AddTableRow("Offset", string.Format("({0:0.00}, {1:0.00})", pmResult.OffsetFound.Width, pmResult.OffsetFound.Height));
            resultMessage.AddTableRow("RealOffset", string.Format("({0:0.00}, {1:0.00})", pmResult.RealOffsetFound.Width, pmResult.RealOffsetFound.Height));

            resultMessage.EndTable();

            var patternResultList = (List<PatternResult>)algorithmResult.GetResultValue("PatternResultList").Value;

            pmResult.AddResultValue(new ResultValue("PatternResultList", "", patternResultList));

            resultMessage.BeginTable(null, "No", "Found");

            for (int i = 0; i < patternResultList.Count; i++)
            {
                PatternResult patternResult = patternResultList[i];
                resultMessage.AddTableRow(i.ToString(), (patternResult.Found ? "OK" : ""));
            }

            resultMessage.EndTable();
        }
    }
}
