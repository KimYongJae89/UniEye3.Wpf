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
    public class BlobSubtractorParam : AlgorithmParam
    {
        private List<AlgoImage> maskImageList;
        public List<AlgoImage> MaskImageList
        {
            get => maskImageList;
            set => MaskImageList = value;
        }
        public int EdgeThreshold { get; set; }
        public int MinPixelCount { get; set; }
        public int MaxPixelCount { get; set; }
        public int AreaMin { get; set; }

        public BlobSubtractorParam()
        {
            EdgeThreshold = 20;
            MinPixelCount = 0;
            MaxPixelCount = 0;
            maskImageList = new List<AlgoImage>();
        }

        public override AlgorithmParam Clone()
        {
            var param = new BlobSubtractorParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BlobSubtractorParam)srcAlgorithmParam;

            EdgeThreshold = param.EdgeThreshold;
            MinPixelCount = param.MinPixelCount;
            MaxPixelCount = param.MaxPixelCount;

            RemoveAllMaskImage();
            foreach (AlgoImage maskImage in param.MaskImageList)
            {
                AddMaskImage(maskImage.ToImageD());
            }
        }

        public void AddMaskImage(ImageD maskImage)
        {
            maskImageList.Add(ImageBuilder.MilImageBuilder.Build(maskImage, ImageType.Grey));
        }

        public void RemoveMaskImage(int index)
        {
            maskImageList[index].Dispose();
            maskImageList.RemoveAt(index);
        }

        public void RemoveAllMaskImage()
        {
            foreach (AlgoImage maskImage in maskImageList)
            {
                maskImage.Dispose();
            }

            maskImageList.Clear();
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);
            EdgeThreshold = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThreshold", "20"));
            MinPixelCount = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinPixelCount", "0"));
            MaxPixelCount = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxPixelCount", "0"));
            AreaMin = Convert.ToInt32(XmlHelper.GetValue(paramElement, "AreaMin", "50"));

            foreach (XmlElement maskElement in paramElement)
            {
                if (maskElement.Name == "MaskImage")
                {
                    Bitmap maskBitmap = ImageHelper.Base64StringToBitmap(maskElement.InnerText);
                    if (maskBitmap != null)
                    {
                        var maskImage = Image2D.ToImage2D(maskBitmap);
                        maskBitmap.Dispose();
                        maskImageList.Add(ImageBuilder.Build(BlobSubtractor.TypeName, maskImage, ImageType.Grey));
                    }
                }
            }
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);
            XmlHelper.SetValue(paramElement, "EdgeThreshold", EdgeThreshold.ToString());
            XmlHelper.SetValue(paramElement, "MinPixelCount", MinPixelCount.ToString());
            XmlHelper.SetValue(paramElement, "MaxPixelCount", MaxPixelCount.ToString());
            XmlHelper.SetValue(paramElement, "AreaMin", AreaMin.ToString());

            foreach (AlgoImage maskImage in maskImageList)
            {
                var patternBitmap = maskImage.ToImageD().ToBitmap();
                string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
                patternBitmap.Dispose();

                XmlHelper.SetValue(paramElement, "MaskImage", patternImageString);
            }
        }
    }

    public class BlobSubtractor : Algorithm
    {
        public BlobSubtractor()
        {
            Param = new BlobSubtractorParam();
            FilterList.Add(new BinarizeFilter(BinarizationType.SingleThreshold, 100));
        }

        public override void Clear()
        {
        }

        public override Algorithm Clone()
        {
            var blobSubtractor = new BlobSubtractor();
            blobSubtractor.Copy(this);

            return blobSubtractor;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var blobSubtractor = (BlobSubtractor)algorithm;
            param = (BlobSubtractorParam)blobSubtractor.Param.Clone();
        }

        public const string TypeName = "BlobSubtractor";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "BlobSub";
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();

            var blobSubtractorParam = (BlobSubtractorParam)Param;

            resultValues.Add(new ResultValue("Pixel Count", "", 0, 0, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage, 0);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;

            var blobSubtractorParam = (BlobSubtractorParam)Param;

            var algorithmResult = new AlgorithmResult();
            algorithmResult.ResultRect = inspectParam.ProbeRegionInFov;
            algorithmResult.SetResult(true);

            DebugHelper.SaveImage(clipImage, "filterdImage.bmp", inspectParam.DebugContext);

            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

            var blobParam = new BlobParam();
            blobParam.SelectBoundingRect = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectLabelValue = true;
            blobParam.AreaMin = blobSubtractorParam.AreaMin;

            BlobRectList blobRectList = imageProcessing.Blob(clipImage, blobParam);
            List<BlobRect> blobRects = blobRectList.GetList();

            int width = clipImage.Width;
            int height = clipImage.Height;

            if (blobRects.Count != 0 && blobSubtractorParam.MaskImageList.Count != 0)
            {
                AlgoImage tempImage = ImageBuilder.Build(GetAlgorithmType().ToString(), ImageType.Grey, width, height);
                AlgoImage tempClipImage = ImageBuilder.Build(GetAlgorithmType().ToString(), ImageType.Grey, width, height);

                AlgoImage subImage1 = ImageBuilder.Build(GetAlgorithmType().ToString(), ImageType.Grey, width, height);
                AlgoImage subImage2 = ImageBuilder.Build(GetAlgorithmType().ToString(), ImageType.Grey, width, height);

                int[] minCount = new int[blobRects.Count];

                for (int i = 0; i < blobRects.Count; i++)
                {
                    minCount[i] = 5000;
                }

                var minCountImage = new AlgoImage[blobRects.Count];

                AlgoImage saveImage = ImageBuilder.Build(GetAlgorithmType().ToString(), ImageType.Grey, width, height);

                int maskIndex = 0;
                foreach (AlgoImage maskImage in blobSubtractorParam.MaskImageList)
                {
                    BlobRectList refBlobRectList = imageProcessing.Blob(maskImage, blobParam);
                    List<BlobRect> refBlobs = refBlobRectList.GetList();
                    if (refBlobs.Count == 0)
                    {
                        continue;
                    }

                    int blobIndex = 0;

                    DebugHelper.SaveImage(maskImage, string.Format("maskImage{0}.bmp", maskIndex), inspectParam.DebugContext);

                    int refIndex = 0;
                    foreach (BlobRect blobRect in blobRects)
                    {
                        int subMinCount = int.MaxValue;

                        var drawBlobOption = new DrawBlobOption();
                        drawBlobOption.SelectBlob = true;

                        tempClipImage.Clear();
                        imageProcessing.DrawBlob(tempClipImage, blobRectList, blobRect, drawBlobOption);

                        DebugHelper.SaveImage(tempClipImage, string.Format("tempClipImage{0}.bmp", refIndex++), inspectParam.DebugContext);

                        foreach (BlobRect refBlob in refBlobs)
                        {
                            //if (blobRect.BoundingRect.Contains(refBlob.CenterPt))
                            {
                                tempImage.Clear();

                                imageProcessing.DrawBlob(tempImage, refBlobRectList, refBlob, drawBlobOption);

                                //DebugHelper.SaveImage(tempImage, string.Format("tempImage{0}.bmp", refIndex++), inspectParam.DebugContext);

                                AlgoImage sobelImage = tempImage.Clone();
                                imageProcessing.Sobel(sobelImage);
                                imageProcessing.Dilate(sobelImage, 1);
                                imageProcessing.Binarize(sobelImage, sobelImage, blobSubtractorParam.EdgeThreshold);

                                imageProcessing.Translate(tempImage, tempImage, Point.Round(new PointF(blobRect.CenterPt.X - refBlob.CenterPt.X, blobRect.CenterPt.Y - refBlob.CenterPt.Y)));
                                imageProcessing.Translate(sobelImage, sobelImage, Point.Round(new PointF(blobRect.CenterPt.X - refBlob.CenterPt.X, blobRect.CenterPt.Y - refBlob.CenterPt.Y)));

                                imageProcessing.Subtract(tempImage, tempClipImage, subImage1);
                                imageProcessing.Subtract(tempClipImage, tempImage, subImage2);
                                imageProcessing.Or(subImage1, subImage2, tempImage);
                                imageProcessing.Subtract(tempImage, sobelImage, tempImage);

                                var tempBlobParam = new BlobParam();
                                tempBlobParam.SelectLabelValue = true;
                                BlobRectList tempBlobRectList = imageProcessing.Blob(tempImage, tempBlobParam);

                                if (tempBlobRectList.GetList().Count != 0)
                                {
                                    tempImage.Clear();
                                    imageProcessing.DrawBlob(tempImage, tempBlobRectList, tempBlobRectList.GetMaxAreaBlob(), drawBlobOption);
                                }

                                int pixelCount = imageProcessing.Count(tempImage);

                                if (pixelCount < subMinCount || subMinCount == -1)
                                {
                                    subMinCount = pixelCount;
                                    if (saveImage != null)
                                    {
                                        saveImage.Dispose();
                                    }

                                    saveImage = tempImage.Clone();
                                }

                                sobelImage.Dispose();
                                tempBlobRectList.Dispose();
                            }
                        }

                        if (subMinCount < minCount[blobIndex] || minCount[blobIndex] == -1)
                        {
                            if (minCountImage[blobIndex] != null)
                            {
                                minCountImage[blobIndex].Dispose();
                            }

                            if (saveImage != null)
                            {
                                minCountImage[blobIndex] = saveImage.Clone();
                            }

                            minCount[blobIndex] = subMinCount;
                        }

                        blobIndex++;
                    }

                    maskIndex++;

                    refBlobRectList.Dispose();
                }

                var subResultList = new List<SubResult>();
                for (int i = 0; i < blobRects.Count; i++)
                {
                    var subAlgorithmResult = new SubResult("Blob " + i, minCount[i]);
                    subAlgorithmResult.Good = (minCount[i] >= blobSubtractorParam.MinPixelCount) && (minCount[i] <= blobSubtractorParam.MaxPixelCount);

                    RectangleF rect = blobRects[i].BoundingRect;
                    rect.Offset(inspectRegionInFov.X, inspectRegionInFov.Y);

                    if (subAlgorithmResult.Good == true)
                    {
                        subAlgorithmResult.FigureGroup.AddFigure(new RectangleFigure(rect, new Pen(Color.Green)));
                    }
                    else
                    {
                        subAlgorithmResult.FigureGroup.AddFigure(new RectangleFigure(rect, new Pen(Color.Red)));
                        algorithmResult.SetResult(false);
                    }

                    subResultList.Add(subAlgorithmResult);

                    if (minCountImage[i] != null)
                    {
                        DebugHelper.SaveImage(minCountImage[i], string.Format("minCountImage {0}.bmp", i), inspectParam.DebugContext);
                        minCountImage[i].Dispose();
                    }
                }

                if (saveImage != null)
                {
                    saveImage.Dispose();
                }

                tempImage.Dispose();
                tempClipImage.Dispose();
                subImage1.Dispose();
                subImage2.Dispose();
            }
            else
            {
                algorithmResult.SetResult(false);
            }

            blobRectList.Dispose();
            clipImage.Dispose();

            return algorithmResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
