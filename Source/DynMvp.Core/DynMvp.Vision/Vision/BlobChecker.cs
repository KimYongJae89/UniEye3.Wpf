using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class BlobCheckerParam : AlgorithmParam
    {
        public bool DarkBlob { get; set; } = true;
        public int SearchRangeWidth { get; set; } = 0;
        public int SearchRangeHeight { get; set; } = 0;
        public float CenterX { get; set; } = 0;
        public float CenterY { get; set; } = 0;
        public float OffsetRangeX { get; set; } = 0;
        public float OffsetRangeY { get; set; } = 0;
        public int MinArea { get; set; } = 0;
        public int MaxArea { get; set; } = 0;
        public bool UseRealOffset { get; set; }
        public bool UseWholeImage { get; set; }
        public bool ActAsFiducial { get; set; }

        public override AlgorithmParam Clone()
        {
            var param = new BlobCheckerParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (BlobCheckerParam)srcAlgorithmParam;

            DarkBlob = param.DarkBlob;
            SearchRangeWidth = param.SearchRangeWidth;
            SearchRangeHeight = param.SearchRangeHeight;
            OffsetRangeX = param.OffsetRangeX;
            OffsetRangeY = param.OffsetRangeY;
            MinArea = param.MinArea;
            MaxArea = param.MaxArea;
            UseWholeImage = param.UseWholeImage;
            ActAsFiducial = param.ActAsFiducial;
            UseRealOffset = param.UseRealOffset;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            DarkBlob = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "DarkBlob", "false"));
            SearchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeWidth", "0"));
            SearchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeHeight", "0"));
            CenterX = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterX", "0"));
            CenterY = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterY", "0"));

            OffsetRangeX = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRangeX", "0"));
            OffsetRangeY = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRangeY", "0"));
            MinArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinArea", "0"));
            MaxArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxArea", "0"));

            UseWholeImage = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseWholeImage", "false"));
            UseRealOffset = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseRealOffset", "false"));
            ActAsFiducial = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "ActAsFiducial", "false"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "DarkBlob", DarkBlob.ToString());
            XmlHelper.SetValue(paramElement, "SearchRangeWidth", SearchRangeWidth.ToString());
            XmlHelper.SetValue(paramElement, "SearchRangeHeight", SearchRangeHeight.ToString());
            XmlHelper.SetValue(paramElement, "CenterX", CenterX.ToString());
            XmlHelper.SetValue(paramElement, "CenterY", CenterY.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRangeX", OffsetRangeX.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRangeY", OffsetRangeY.ToString());

            XmlHelper.SetValue(paramElement, "MinArea", MinArea.ToString());
            XmlHelper.SetValue(paramElement, "MaxArea", MaxArea.ToString());

            XmlHelper.SetValue(paramElement, "UseWholeImage", UseWholeImage.ToString());
            XmlHelper.SetValue(paramElement, "UseRealOffset", UseRealOffset.ToString());
            XmlHelper.SetValue(paramElement, "ActAsFiducial", ActAsFiducial.ToString());
        }
    }

    public class BlobChecker : Algorithm, Searchable
    {
        public BlobChecker()
        {
            param = new BlobCheckerParam();
        }

        public override Algorithm Clone()
        {
            var blobChecker = new BlobChecker();
            blobChecker.Copy(this);

            return blobChecker;
        }

        public Size GetSearchRangeSize()
        {
            var blobCheckerParam = (BlobCheckerParam)param;

            return new Size(blobCheckerParam.SearchRangeWidth, blobCheckerParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            var blobCheckerParam = (BlobCheckerParam)param;

            blobCheckerParam.SearchRangeWidth = searchRange.Width;
            blobCheckerParam.SearchRangeHeight = searchRange.Height;
        }

        public const string TypeName = "BlobChecker";

        public bool UseWholeImage => throw new NotImplementedException();

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Blob";
        }

        public override RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            RotatedRect newInspRegion = inspRegion;

            var blobCheckerParam = (BlobCheckerParam)param;

            if (blobCheckerParam.ActAsFiducial)
            {
                newInspRegion.Inflate(blobCheckerParam.SearchRangeWidth, blobCheckerParam.SearchRangeHeight);
            }

            return newInspRegion;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Area", "", 0, 0, 0));

            resultValues.Add(new ResultValue("Blob Center X", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Blob Center Y", "", 0, 0, 0));

            resultValues.Add(new ResultValue("Ref. Center X", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Ref. Center Y", "", 0, 0, 0));

            resultValues.Add(new ResultValue("Offset X", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Offset Y", "", 0, 0, 0));

            resultValues.Add(new ResultValue("Real Offset Y", "", 0, 0, 0));
            resultValues.Add(new ResultValue("Real Offset Y", "", 0, 0, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.InspectRegionInFov;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            DebugContext debugContext = inspectParam.DebugContext;
            Size cameraImageSize = inspectParam.CameraImageSize;

            clipImage.Save("clipImage2.bmp", debugContext);

            Filter(clipImage, 0);

            clipImage.Save("FilterImage.bmp", debugContext);

            var blobCheckerResult = new SearchableResult();

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);

            var blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;

            var blobCheckerParam = (BlobCheckerParam)param;

            AlgoImage blobImage = clipImage.Clone();
            if (blobCheckerParam.DarkBlob)
            {
                imageProcessing.Not(blobImage, blobImage);
            }

            blobImage.Save("Blob.bmp", debugContext);

            BlobRectList blobRectList = imageProcessing.Blob(blobImage, blobParam);

            BlobRect blobRect = blobRectList.GetMaxAreaBlob();
            if (blobRect == null)
            {
                return blobCheckerResult;
            }

            PointF refPosInFov;
            PointF foundPosInFov = DrawingHelper.ClipToFov(clipRegionInFov, blobRect.CenterPt);
            if (blobCheckerParam.UseWholeImage)
            {
                refPosInFov = new PointF(cameraImageSize.Width / 2, cameraImageSize.Height / 2);
            }
            else
            {
                refPosInFov = DrawingHelper.CenterPoint(probeRegionInFov);
            }

            var realOffset = new SizeF(0, 0);
            var offset = new SizeF(foundPosInFov.X - refPosInFov.X, foundPosInFov.Y - refPosInFov.Y);

            bool fCalibrated = false;

            if (cameraCalibration != null)
            {
                fCalibrated = cameraCalibration.IsCalibrated();
            }

            if (fCalibrated)
            {
                PointF realRefPos = cameraCalibration.PixelToWorld(refPosInFov);
                PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

                realOffset = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
            }

            blobCheckerResult.OffsetFound = offset;
            blobCheckerResult.RealOffsetFound = realOffset;

            bool areaResult = (blobRect.Area >= blobCheckerParam.MinArea && blobRect.Area <= blobCheckerParam.MaxArea);

            bool offsetResultX = false;
            bool offsetResultY = false;
            if (blobCheckerParam.UseRealOffset)
            {
                if (fCalibrated)
                {
                    offsetResultX = (Math.Abs(realOffset.Width) < blobCheckerParam.OffsetRangeX);
                    offsetResultY = (Math.Abs(realOffset.Height) < blobCheckerParam.OffsetRangeY);
                }
            }
            else
            {
                offsetResultX = (Math.Abs(offset.Width) < blobCheckerParam.OffsetRangeX);
                offsetResultY = (Math.Abs(offset.Height) < blobCheckerParam.OffsetRangeY);
            }

            RotatedRect resultRect = probeRegionInFov;
            if (blobCheckerParam.ActAsFiducial)
            {
                resultRect.Offset(offset.Width, offset.Height);
            }

            blobCheckerResult.ResultRect = resultRect;

            blobCheckerResult.SetResult(areaResult && offsetResultX && offsetResultY);
            blobCheckerResult.AddResultValue(new ResultValue("Area", "", 0, 0, blobRect.Area, areaResult));

            blobCheckerResult.AddResultValue(new ResultValue("BlobCenterX", "", 0, 0, foundPosInFov.X));
            blobCheckerResult.AddResultValue(new ResultValue("BlobCenterY", "", 0, 0, foundPosInFov.Y));

            blobCheckerResult.AddResultValue(new ResultValue("RefCenterX", "", 0, 0, refPosInFov.X));
            blobCheckerResult.AddResultValue(new ResultValue("RefCenterY", "", 0, 0, refPosInFov.Y));

            if (blobCheckerParam.UseRealOffset)
            {
                blobCheckerResult.AddResultValue(new ResultValue("OffsetX", "", 0, 0, offset.Width));
                blobCheckerResult.AddResultValue(new ResultValue("OffsetY", "", 0, 0, offset.Height));

                blobCheckerResult.AddResultValue(new ResultValue("RealOffsetY", "", 0, 0, realOffset.Width, fCalibrated && offsetResultX));
                blobCheckerResult.AddResultValue(new ResultValue("RealOffsetY", "", 0, 0, realOffset.Height, fCalibrated && offsetResultY));
            }
            else
            {
                blobCheckerResult.AddResultValue(new ResultValue("OffsetX", "", 0, 0, offset.Width, offsetResultX));
                blobCheckerResult.AddResultValue(new ResultValue("OffsetY", "", 0, 0, offset.Height, offsetResultY));
            }

            var pen = new Pen(Color.Purple, 10.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            var crossFigure = new CrossFigure(foundPosInFov, 3, pen);
            blobCheckerResult.ResultFigures.AddFigure(crossFigure);

            return blobCheckerResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
