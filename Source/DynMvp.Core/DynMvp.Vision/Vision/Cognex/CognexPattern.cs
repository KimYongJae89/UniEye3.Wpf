//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using System.Windows.Forms;

//using DynMvp.Base;
//using DynMvp.UI;

//using Cognex.VisionPro;
//using Cognex.VisionPro.PMAlign;

//namespace DynMvp.Vision.Cognex
//{
//    public class CognexPattern : Pattern
//    {
//        private string featureType;
//        private CogPMAlignTrainAlgorithmConstants trainAlgorithm;
//        private CogPMAlignPattern cogPMAlignPattern = null;

//        public CognexPattern(string featureType)
//        {
//            this.featureType = featureType;

//            if (featureType.Contains("VxPatMax") == true && CognexHelper.LicenseExist("VxPatMax") == true)
//            {
//                trainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;
//            }
//            else
//            {
//                trainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatQuick;
//            }
//        }

//        ~CognexPattern()
//        {
//            Dispose();
//        }

//        public override void Dispose()
//        {
//            base.Dispose();

//            cogPMAlignPattern = null;
//        }

//        public override Pattern Clone()
//        {
//            CognexPattern cognexPattern = new CognexPattern(featureType);
//            cognexPattern.Copy(this);

//            return cognexPattern;
//        }

//        public override void Train(Image2D image, PatternMatchingParam patternMatchingParam)
//        {
//            patternImage = (Image2D)image.Clone();

//            CognexGreyImage greyImage = (CognexGreyImage)ImageBuilder.CognexImageBuilder.Build(patternImage, ImageType.Grey);

//            cogPMAlignPattern = new CogPMAlignPattern();
//            cogPMAlignPattern.TrainAlgorithm = trainAlgorithm;
//            cogPMAlignPattern.TrainMode = CogPMAlignTrainModeConstants.Image;
//            cogPMAlignPattern.TrainImage = greyImage.Image;

//            PointF centerPt = new PointF(greyImage.Width / 2, greyImage.Height / 2);

//            cogPMAlignPattern.Origin.TranslationX = centerPt.X;
//            cogPMAlignPattern.Origin.TranslationY = centerPt.Y;

//            CogRectangleAffine cogRectangle = new CogRectangleAffine();
//            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, greyImage.Width, greyImage.Height, 0, 0);

//            cogPMAlignPattern.TrainRegion = cogRectangle;
//            cogPMAlignPattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

//            try
//            {
//                cogPMAlignPattern.Train();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        public override void UpdateMaskImage()
//        {

//        }

//        public override PatternResult Inspect(AlgoImage probeClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
//        {
//            CognexGreyImage greyImage = (CognexGreyImage)probeClipImage;
//            greyImage.Save("TargetImage1.bmp", debugContext);

//            if (debugContext.SaveDebugImage == true)
//            {
//                Bitmap targetImage = CognexImageBuilder.ConvertImage(greyImage.Image);
//                DebugHelper.SaveImage(targetImage, "TargetImage2.bmp", debugContext);
//            }

//            DebugHelper.SaveImage(patternImage, "PatternImage.bmp", debugContext);

//            PatternResult pmResult = new PatternResult();

//            if (cogPMAlignPattern.Trained == false)
//            {
//                pmResult.ErrorString = "NotTrained";
//                LogHelper.Error(pmResult.ErrorString);
//                return pmResult;
//            }

//            PointF centerPt = new PointF(probeClipImage.Width / 2, probeClipImage.Height / 2);

//            CogRectangleAffine cogRectangle = new CogRectangleAffine();
//            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, probeClipImage.Width, probeClipImage.Height, 0, 0);

//            CogPMAlignRunParams runParams = new CogPMAlignRunParams();

//            runParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatMax;

//            runParams.XYOverlap = 0;
//            runParams.TimeoutEnabled = true;
//            runParams.Timeout = 5000;

//            runParams.AcceptThreshold = (double)patternMatchingParam.MatchScore / 100;
//            runParams.ApproximateNumberToFind = patternMatchingParam.NumToFind;

//#if true
//            runParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
//            runParams.ZoneAngle.Low = MathHelper.DegToRad(patternMatchingParam.MinAngle);
//            runParams.ZoneAngle.High = MathHelper.DegToRad(patternMatchingParam.MaxAngle);
//            runParams.ZoneAngle.Overlap = MathHelper.DegToRad(Math.Abs(patternMatchingParam.MaxAngle - patternMatchingParam.MinAngle) * 2);

//            runParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
//            runParams.ZoneScale.Low = patternMatchingParam.MinScale;
//            runParams.ZoneScale.High = patternMatchingParam.MaxScale;
//            runParams.ZoneScale.Overlap = (patternMatchingParam.MaxScale / patternMatchingParam.MinScale) * 2;

//            runParams.ZoneScaleX.Configuration = CogPMAlignZoneConstants.LowHigh;
//            runParams.ZoneScaleX.Low = patternMatchingParam.MinScale;
//            runParams.ZoneScaleX.High = patternMatchingParam.MaxScale;
//            runParams.ZoneScale.Overlap = (patternMatchingParam.MaxScale / patternMatchingParam.MinScale) * 2;

//            runParams.ZoneScaleY.Configuration = CogPMAlignZoneConstants.LowHigh;
//            runParams.ZoneScaleY.Low = patternMatchingParam.MinScale;
//            runParams.ZoneScaleY.High = patternMatchingParam.MaxScale;
//            runParams.ZoneScale.Overlap = (patternMatchingParam.MaxScale / patternMatchingParam.MinScale) * 2;
//#else
//            runParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
//            runParams.ZoneAngle.Nominal = 0;

//            runParams.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
//            runParams.ZoneScale.Nominal = 1;

//            runParams.ZoneScaleX.Configuration = CogPMAlignZoneConstants.Nominal;
//            runParams.ZoneScaleX.Nominal = 1;

//            runParams.ZoneScaleY.Configuration = CogPMAlignZoneConstants.Nominal;
//            runParams.ZoneScaleY.Nominal = 1;
//#endif

//            CogPMAlignResults results;
//            try
//            {
//                results = cogPMAlignPattern.Execute(greyImage.Image, cogRectangle, runParams);
//            }
//            catch (Exception ex)
//            {
//                pmResult.ErrorString = "Matching Error" + ex.ToString();
//                LogHelper.Error(pmResult.ErrorString);
//                return pmResult;
//            }

//            Size patternSize = new Size(patternImage.Width, patternImage.Height);
//            RectangleF imageRect = new RectangleF(0, 0, probeClipImage.Width, probeClipImage.Height);

//            foreach (CogPMAlignResult result in results)
//            {
//                MatchPos matchPos = new MatchPos();
//                matchPos.Score = (float)result.Score;
//                CogTransform2DLinear pose = result.GetPose();
//                matchPos.Pos = new PointF((float)pose.TranslationX, (float)pose.TranslationY);
//                matchPos.Angle = (float)(360 - MathHelper.RadToDeg(pose.MapAngle(0)));

//                matchPos.PatternSize = patternSize;
//                matchPos.PatternType = PatternType;

//                RectangleF patternRect = DrawingHelper.FromCenterSize(matchPos.Pos, new SizeF(patternSize.Width, patternSize.Height));
//                if (imageRect.Contains(patternRect))
//                    pmResult.AddMatchPos(matchPos);
//            }

//            return pmResult;
//        }
//    }
//}
