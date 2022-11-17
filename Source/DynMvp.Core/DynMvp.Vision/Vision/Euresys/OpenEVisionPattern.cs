using DynMvp.Base;
using DynMvp.UI;
using Euresys.Open_eVision_1_2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DynMvp.Vision.Euresys
{
    public class OpenEVisionPattern : Pattern
    {
        private EMatcher matchPattern = null;
        private EMatcher edgePattern = null;

        public OpenEVisionPattern()
            : base()
        {

        }

        ~OpenEVisionPattern()
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (matchPattern != null)
            {
                matchPattern.Dispose();
            }

            if (edgePattern != null)
            {
                edgePattern.Dispose();
            }
        }

        public override Pattern Clone()
        {
            var openEVisionPattern = new OpenEVisionPattern();
            openEVisionPattern.Copy(this);

            return openEVisionPattern;
        }

        public override Image2D GetMaskedImage()
        {
            if (MaskFigures.FigureExist == false)
            {
                return PatternImage;
            }

            var maskedPatternImage = PatternImage.ToBitmap();

            ImageHelper.Add(maskedPatternImage, 1);

            var rgbImage = new Bitmap(maskedPatternImage);

            var g = Graphics.FromImage(rgbImage);

            MaskFigures.Draw(g, new CoordTransformer(), true);

            g.Dispose();

            maskedPatternImage = ImageHelper.ConvertGrayImage(rgbImage);

            rgbImage.Dispose();
            maskedPatternImage.Dispose();

            return Image2D.ToImage2D(maskedPatternImage);
        }

        public override void Train(Image2D image, PatternMatchingParam patternMatchingParam)
        {
            patternImage = (Image2D)image.Clone();

            var greyImage = (OpenEVisionGreyImage)ImageBuilder.OpenEVisionImageBuilder.Build(patternImage, ImageType.Grey);

            matchPattern = new EMatcher();
            matchPattern.FilteringMode = EFilteringMode.LowPass;
            matchPattern.LearnPattern(greyImage.Image);

            TrainEdgePattern(greyImage.Image);
        }

        private void TrainEdgePattern(EImageBW8 pattern)
        {
            edgePattern = new EMatcher();
            edgePattern.FilteringMode = EFilteringMode.LowPass;

            // Edge Mask 생성
            var maskImageEdge1 = new EImageBW8(pattern.Width, pattern.Height);
            var maskImageEdge2 = new EImageBW8(pattern.Width, pattern.Height);
            EasyImage.Copy(pattern, maskImageEdge1);

            EasyImage.ConvolSobel(maskImageEdge1);

            EBW8 threshold = EasyImage.AutoThreshold(maskImageEdge1, EThresholdMode.MaxEntropy);
            EasyImage.Threshold(maskImageEdge1, maskImageEdge2, threshold.Value, 0);

            // Highlight Mask 생성
            var maskImageHighLight1 = new EImageBW8(pattern.Width, pattern.Height);
            var maskImageHighLight2 = new EImageBW8(pattern.Width, pattern.Height);
            EasyImage.Copy(pattern, maskImageHighLight1);

            EasyImage.Threshold(maskImageHighLight1, maskImageHighLight2, 150, 255, 0);
            EasyImage.Dilate(maskImageHighLight2, maskImageHighLight1, 2);

            var maskPattern1 = new EImageBW8(pattern.Width, pattern.Height);
            var maskPattern2 = new EImageBW8(pattern.Width, pattern.Height);
            EasyImage.Threshold(pattern, maskPattern1, 128, 0, 0);
            EasyImage.Threshold(pattern, maskPattern2, 128, 0, 0);

            EasyImage.Oper(EArithmeticLogicOperation.SetNonZero, maskImageEdge2, pattern, maskPattern1);

            EasyImage.Copy(maskPattern1, maskPattern1);
            EasyImage.Oper(EArithmeticLogicOperation.SetNonZero, maskImageHighLight1, maskPattern1, maskPattern2);

            edgePattern.MaxPositions = 5;
            edgePattern.Interpolate = true;

            edgePattern.MinAngle = -10;// patternMatchingParam.MinAngle;
            edgePattern.MaxAngle = 10; //  patternMatchingParam.MaxAngle;
            edgePattern.MinScale = 0.95f; //  patternMatchingParam.MinScale;
            edgePattern.MaxScale = 1.05f; //  patternMatchingParam.MaxScale;

            edgePattern.DontCareThreshold = 1;
            edgePattern.MinReducedArea = 128;
            edgePattern.LearnPattern(maskPattern2);
        }

        public override void UpdateMaskImage()
        {
            if (MaskFigures.FigureExist == false)
            {
                return;
            }

            matchPattern.DontCareThreshold = 1;
            var greyImage = (OpenEVisionGreyImage)ImageBuilder.OpenEVisionImageBuilder.Build(GetMaskedImage(), ImageType.Grey, ImageBandType.Luminance);
            matchPattern.LearnPattern(greyImage.Image);
        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            var greyImage = (OpenEVisionGreyImage)targetClipImage;
            greyImage.Save("TargetImage1.bmp", debugContext);

            if (debugContext.SaveDebugImage == true)
            {
                ImageD targetImage = OpenEVisionImageBuilder.ConvertImage(greyImage.Image);
                DynMvp.Vision.DebugHelper.SaveImage(targetImage, "TargetImage2.bmp", debugContext);
            }
            DynMvp.Vision.DebugHelper.SaveImage(patternImage, "PatternImage.bmp", debugContext);

            matchPattern.MaxPositions = patternMatchingParam.NumToFind + 5;
            matchPattern.Interpolate = true;
            matchPattern.MinScore = ((float)patternMatchingParam.MatchScore / 2) / 100;

            matchPattern.MinAngle = -10;// patternMatchingParam.MinAngle;
            matchPattern.MaxAngle = 10; //  patternMatchingParam.MaxAngle;
            matchPattern.MinScale = 0.95f; //  patternMatchingParam.MinScale;
            matchPattern.MaxScale = 1.05f; //  patternMatchingParam.MaxScale;

            patternMatchingParam.MinAngle = -10;// patternMatchingParam.MinAngle;
            patternMatchingParam.MaxAngle = 10; //  patternMatchingParam.MaxAngle;
            patternMatchingParam.MinScale = 0.95f; //  patternMatchingParam.MinScale;
            patternMatchingParam.MaxScale = 1.05f; //  patternMatchingParam.MaxScale;

            if (patternMatchingParam.IgnorePolarity)
            {
                matchPattern.MinAngle = -90.0f;
                matchPattern.MaxAngle = 90.0f;
            }

            var pmResult = new PatternResult();

            try
            {
                matchPattern.Match(greyImage.Image);
            }
            catch (Exception ex)
            {
                pmResult.ErrorString = "Matching Error" + ex.ToString();
                LogHelper.Error(pmResult.ErrorString);
                return pmResult;
            }

            Size patternSize = patternImage.Size;

            int numOccurrences = matchPattern.NumPositions;
            if (numOccurrences > 0)
            {
                for (int i = 0; i < numOccurrences; i++)
                {
                    EMatchPosition euresysMatchPos = matchPattern.GetPosition(i);

                    var matchPos = new MatchPos();
                    matchPos.Score = euresysMatchPos.Score;
                    matchPos.Pos = new PointF(euresysMatchPos.CenterX, euresysMatchPos.CenterY);
                    matchPos.PatternSize = patternSize;
                    matchPos.PatternType = PatternType;

                    pmResult.AddMatchPos(matchPos);
                }

                // 동작 상의 문제가 있어 Remark 처리함. 향후, 이러한 형태의 매칭 방식을 적용하는 것을 연구해 볼 필요는 있어 코드 남겨 놓음
                //if (patternMatchingParam.SubtractMatching == true  && edgePattern != null && edgePattern.PatternLearnt == true)
                //{
                //    MatchPos maxMatchPos = pmResult.MaxMatchPos;
                //    Rectangle findedRect = new Rectangle((int)maxMatchPos.Pos.X - patternSize.Width / 2,
                //            (int)maxMatchPos.Pos.Y - patternSize.Height / 2, patternSize.Width, patternSize.Height);
                //    findedRect.Inflate(5, 5);

                //    if (DrawingHelper.IsValid(findedRect, new Size(greyImage.Width, greyImage.Height)) == true)
                //    {
                //        findedRect.Inflate(-4, -4); // 영역을 넓혀, 영역 벗어남을 검사한 후, 영역을 다시 줄여 검사 영역을 설정한다.

                //        EROIBW8 findedRoi = new EROIBW8();
                //        findedRoi.Attach(greyImage.Image);
                //        findedRoi.SetPlacement(findedRect.X, findedRect.Y, findedRect.Width, findedRect.Height);

                //        try
                //        {
                //            edgePattern.MinScore = ((float)patternMatchingParam.MatchScore / 2) / 100;

                //            edgePattern.Match(findedRoi);

                //            int numOccurrences2 = edgePattern.NumPositions;
                //            if (numOccurrences2 > 0)
                //            {
                //                double maxScore = 0;
                //                for (int i = 0; i < numOccurrences2; i++)
                //                {
                //                    EMatchPosition euresysMatchPos = edgePattern.GetPosition(i);

                //                    if (maxScore < euresysMatchPos.Score)
                //                        maxScore = euresysMatchPos.Score;
                //                }

                //                maxMatchPos.Score = maxScore;
                //            }
                //            else
                //            {
                //                maxMatchPos.Score = 0;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            LogHelper.Error("Matching Error" + ex.ToString());
                //        }

                //        findedRoi.Detach();
                //    }
                //    else
                //    {
                //        maxMatchPos.Score = 0;
                //    }
                //}
            }

            return pmResult;
        }

        private float SubtractMatching(OpenEVisionGreyImage findedPosImage)
        {
            var pattern = (EImageBW8)matchPattern.GetPattern(0);

            EasyImage.PixelAverage(pattern, out float patternAverage);

            EasyImage.PixelAverage(findedPosImage.Image, out float findedPosAverage);

            byte diffAverage = (byte)(findedPosAverage - patternAverage);

            int width = Math.Min(patternImage.Size.Width, findedPosImage.Width);
            int height = Math.Min(patternImage.Size.Height, findedPosImage.Height);

            IntPtr findedPosImagePixelAddr;
            IntPtr patternImagePixelAddr;

            float weight;
            float totalWeight = 0;
            float totalValue = 0;
            for (int y = 0; y < height; ++y)
            {
                findedPosImagePixelAddr = findedPosImage.Image.GetImagePtr(0, y);
                patternImagePixelAddr = pattern.GetImagePtr(0, y);
                for (int x = 0; x < width; ++x)
                {
                    byte findedImagePixel = Marshal.ReadByte(findedPosImagePixelAddr, x);
                    //if (findedImagePixel > diffAverage)
                    //    findedImagePixel = (byte)(findedImagePixel - diffAverage);
                    byte patternImagePixel = Marshal.ReadByte(patternImagePixelAddr, x);

                    weight = 1;
                    //                    weight = (float)patternImagePixel/255 + (float)findedImagePixel/255;
                    //                    weight = (float)patternImagePixel / 255;
                    //                    weight = (float)(Math.Pow(patternImagePixel / 255, 2) + Math.Pow(findedImagePixel / 255, 2));
                    totalValue += Math.Abs(findedImagePixel - patternImagePixel) * weight;
                    totalWeight += weight;
                }
            }

            float score = 0;

            if (totalWeight > 0)
            {
                score = (100 - totalValue / totalWeight) / 100;
            }

            if (score < 0)
            {
                score = 0;
            }

            return score;
        }

        private float EdgeSubtractMatching(OpenEVisionGreyImage findedPosImage)
        {
            var pattern = (EImageBW8)matchPattern.GetPattern(0);

            var edgeImage = new EImageBW8();
            edgeImage.SetSize(pattern.Width, pattern.Height);

            EasyImage.Copy(pattern, edgeImage);

            EasyImage.ConvolSobel(edgeImage);

            EBW8 threshold = EasyImage.AutoThreshold(edgeImage, EThresholdMode.MaxEntropy);

            EasyImage.PixelAverage(pattern, out float patternAverage);

            EasyImage.PixelAverage(findedPosImage.Image, out float findedPosAverage);

            byte diffAverage = (byte)(findedPosAverage - patternAverage);

            int width = Math.Min(patternImage.Size.Width, findedPosImage.Width);
            int height = Math.Min(patternImage.Size.Height, findedPosImage.Height);

            IntPtr findedPosImagePixelAddr;
            IntPtr patternImagePixelAddr;
            IntPtr edgeImagePixelAddr;

            float weight;
            float totalWeight = 0;
            float totalValue = 0;
            for (int y = 0; y < height; ++y)
            {
                findedPosImagePixelAddr = findedPosImage.Image.GetImagePtr(0, y);
                patternImagePixelAddr = pattern.GetImagePtr(0, y);
                edgeImagePixelAddr = edgeImage.GetImagePtr(0, y);

                for (int x = 0; x < width; ++x)
                {
                    byte findedImagePixel = Marshal.ReadByte(findedPosImagePixelAddr, x);
                    byte patternImagePixel = Marshal.ReadByte(patternImagePixelAddr, x);
                    byte edgeImagePixel = Marshal.ReadByte(edgeImagePixelAddr, x);

                    if (edgeImagePixel > threshold.Value)
                    {
                        weight = 1;
                        //                    weight = (float)patternImagePixel/255 + (float)findedImagePixel/255;
                        //                    weight = (float)patternImagePixel / 255;
                        //                    weight = (float)(Math.Pow(patternImagePixel / 255, 2) + Math.Pow(findedImagePixel / 255, 2));
                        totalValue += Math.Abs(findedImagePixel - patternImagePixel) * weight;
                        totalWeight += weight;
                    }
                }
            }

            float score = (100 - totalValue / totalWeight) / 100;
            if (score < 0)
            {
                score = 0;
            }

            return score;
        }
    }
}
