using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.UI;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class BoltCheckerParam : AlgorithmParam
    {
        public int MinSilkArea { get; set; } = 100;
        public int ThresholdBolt { get; set; } = 120;
        public int ThresholdSilk { get; set; } = 240;
        public int MaxHoleCount { get; set; } = 2000;

        public override AlgorithmParam Clone()
        {
            var param = new BoltCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BoltCheckerParam)srcAlgorithmParam;

            MinSilkArea = param.MinSilkArea;
            ThresholdBolt = param.ThresholdBolt;
            ThresholdSilk = param.ThresholdSilk;
            MaxHoleCount = param.MaxHoleCount;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            MinSilkArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinSilkArea", "100"));
            ThresholdBolt = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdBolt", "120"));
            ThresholdSilk = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdSilk", "240"));
            MaxHoleCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxHoleCount", "2000"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MinSilkArea", MinSilkArea.ToString());
            XmlHelper.SetValue(algorithmElement, "ThresholdBolt", ThresholdBolt.ToString());
            XmlHelper.SetValue(algorithmElement, "ThresholdSilk", ThresholdSilk.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxHoleCount", MaxHoleCount.ToString());
        }
    }

    public class BoltChecker : Algorithm
    {
        public BoltChecker()
        {
            param = new BoltCheckerParam();
        }

        public override Algorithm Clone()
        {
            var boltChecker = new BoltChecker();
            boltChecker.Copy(this);

            return boltChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var boltChecker = (BoltChecker)algorithm;
            param = (BoltCheckerParam)boltChecker.Param.Clone();
        }

        public override List<ResultValue> GetResultValues()
        {
            var boltCheckerParam = (BoltCheckerParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Hole Count", "", boltCheckerParam.MaxHoleCount, 0, 0));

            return resultValues;
        }

        public static string TypeName => "BoltChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Bolt";
        }

        public override string[] GetPreviewNames()
        {
            return new string[] { "Silk", "Bolt" };
        }

        public override ImageD Filter(ImageD image, int previewFilterType, int targetLightTypeIndex = -1)
        {
            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey, param.ImageBand);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var boltCheckerParam = (BoltCheckerParam)param;

            switch (previewFilterType)
            {
                case 0:
                    imageProcessing.Binarize(algoImage, BinarizationType.SingleThreshold, boltCheckerParam.ThresholdSilk, 0);
                    break;
                case 1:
                    imageProcessing.Binarize(algoImage, BinarizationType.SingleThreshold, boltCheckerParam.ThresholdBolt, 0);
                    break;
            }

            return algoImage.ToImageD();
        }

        private AlgoImage GetConvexHullMask(AlgoImage binaryImage, out RectangleF resultRect, out int maskArea)
        {
            var openCvGreyImage = binaryImage as OpenCv.OpenCvGreyImage;

            var blobs = new CvBlobs();
            var blobDetector = new CvBlobDetector();
            blobDetector.Detect(openCvGreyImage.Image, blobs);

            var boltCheckerParam = (BoltCheckerParam)param;

            blobs.FilterByArea(boltCheckerParam.MinSilkArea, int.MaxValue);

            var silkList = blobs.Values.ToList();
            CvBlob silkBlob = silkList.First();

            foreach (CvBlob blob in silkList)
            {
                if (blob.BoundingBox.Width > silkBlob.BoundingBox.Width)
                {
                    silkBlob = blob;
                }
            }

            resultRect = new RectangleF(silkBlob.BoundingBox.Location, silkBlob.BoundingBox.Size);

            PointF[] ptArr = Array.ConvertAll<Point, PointF>(silkBlob.GetContour(), new Converter<Point, PointF>((pt) => new PointF(pt.X, pt.Y)));
            PointF[] convexHullPtArr = CvInvoke.ConvexHull(ptArr, true);

            AlgoImage maskImage = binaryImage.Clone();
            var openCvMaskImage = maskImage as OpenCv.OpenCvGreyImage;
            openCvMaskImage.Image.SetZero();

            int width = openCvMaskImage.Image.Width;
            int height = openCvMaskImage.Image.Height;
            int pitch = 4 * ((int)(Math.Truncate((width - (float)1) / 4)) + 1);

            maskArea = 0;

            for (int yPos = 0; yPos < height; yPos++)
            {
                for (int xPos = 0; xPos < pitch; xPos++)
                {
                    var position = new Point(xPos, yPos);

                    if (CvInvoke.PointPolygonTest(new VectorOfPointF(convexHullPtArr), position, false) >= 0)
                    {
                        openCvMaskImage.Image.Data[yPos, xPos, 0] = byte.MaxValue;
                        maskArea++;
                    }
                }
            }

            return maskImage;
        }

        private int GetHoleCount(AlgoImage holeImage)
        {
            var openCvHoleImage = (OpenCv.OpenCvGreyImage)holeImage;

            var blobs = new CvBlobs();
            var blobDetector = new CvBlobDetector();
            blobDetector.Detect(openCvHoleImage.Image, blobs);

            var holeList = blobs.Values.ToList();

            int sumHoleCount = 0;

            foreach (CvBlob hole in holeList)
            {
                sumHoleCount += hole.Area;
            }

            return sumHoleCount;
        }


        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            var boltCheckerResult = new AlgorithmResult();

            var boltCheckerParam = (BoltCheckerParam)param;

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage, 0);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);
            AlgoImage silkImage = clipImage.Clone();
            imageProcessing.Binarize(clipImage, silkImage, boltCheckerParam.ThresholdSilk);

            if (inspectParam.DebugContext.SaveDebugImage)
            {
                clipImage.Save("Source.bmp", inspectParam.DebugContext);
                silkImage.Save("Silk.bmp", inspectParam.DebugContext);
            }

            AlgoImage maskImage = GetConvexHullMask(silkImage, out RectangleF resultRect, out int maskArea);

            AlgoImage boltImage = clipImage.Clone();
            imageProcessing.Binarize(clipImage, boltImage, boltCheckerParam.ThresholdBolt);

            if (inspectParam.DebugContext.SaveDebugImage)
            {
                boltImage.Save("Bolt.bmp", inspectParam.DebugContext);
                maskImage.Save("Mask.bmp", inspectParam.DebugContext);
            }
            var openCvBoltImage = (OpenCv.OpenCvGreyImage)boltImage;
            openCvBoltImage.Image = openCvBoltImage.Image.Not();

            var openCvMaskImage = (OpenCv.OpenCvGreyImage)maskImage;

            AlgoImage holeImage = clipImage.Clone();
            var openCvHoleImage = (OpenCv.OpenCvGreyImage)holeImage;

            openCvHoleImage.Image = openCvBoltImage.Image.And(openCvMaskImage.Image);

            if (inspectParam.DebugContext.SaveDebugImage)
            {
                holeImage.Save("Hole.bmp", inspectParam.DebugContext);
            }

            int holeCount = GetHoleCount(holeImage);

            resultRect.Offset(inspectParam.ProbeRegionInFov.X, inspectParam.ProbeRegionInFov.Y);
            boltCheckerResult.ResultRect = new DynMvp.UI.RotatedRect(resultRect, 0);

            if (holeCount < boltCheckerParam.MaxHoleCount)
            {
                boltCheckerResult.SetResult(true);
            }

            boltCheckerResult.AddResultValue(new ResultValue("Hole Count", "", boltCheckerParam.MaxHoleCount, 0, holeCount));

            return boltCheckerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
