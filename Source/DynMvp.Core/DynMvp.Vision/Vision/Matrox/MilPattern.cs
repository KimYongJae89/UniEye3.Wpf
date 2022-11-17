using DynMvp.Base;
using DynMvp.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public class MilPattern : Pattern
    {
        public MIL_ID PatternId { get; set; } = MIL.M_NULL;

        ~MilPattern()
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (PatternId != MIL.M_NULL)
            {
                MIL.MpatFree(PatternId);
                PatternId = MIL.M_NULL;
            }
        }

        public MilPattern() : base()
        {

        }

        public override Pattern Clone()
        {
            var milImage = new MilPattern();
            milImage.Copy(this);

            return milImage;
        }

        public override void Train(Image2D image, PatternMatchingParam patternMatchingParam)
        {
            AlgoImage algoImage = ImageBuilder.MilImageBuilder.Build(image, ImageType.Grey);

            MilImage.CheckGreyImage(algoImage, "MilPattern.Train", "Source");

            if (PatternId != MIL.M_NULL)
            {
                MIL.MpatFree(PatternId);
                PatternId = MIL.M_NULL;
            }

            var greyImage = (MilImage)algoImage;

            if (patternImage != null)
            {
                patternImage.Dispose();
                patternImage = null;
            }

            patternImage = (Image2D)MilImageBuilder.ConvertImage((MilGreyImage)greyImage);

            PatternId = MIL.MpatAlloc(MIL.M_DEFAULT_HOST, MIL.M_NORMALIZED, MIL.M_DEFAULT, MIL.M_NULL);
            MIL.MpatDefine(PatternId, MIL.M_REGULAR_MODEL, greyImage.Image, 0, 0, greyImage.Width, greyImage.Height, MIL.M_DEFAULT);

            //MIL.MpatSetSearchParameter(patternId, MIL.M_ALL, MIL.M_DEFAULT);// Parameter Reset
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_NUMBER, MIL.M_ALL);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_ACCEPTANCE, patternMatchingParam.MatchScore);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_CERTAINTY, 50);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_HIGH);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_MEDIUM);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_VERY_LOW);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_HIGH);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_HIGH);

            if (patternMatchingParam.MaxAngle > 0 || patternMatchingParam.MinAngle > 0)
            {
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_POS, patternMatchingParam.MaxAngle);
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_NEG, patternMatchingParam.MinAngle);
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_ACCURACY, 0.1);
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_NEAREST_NEIGHBOR);
                MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_TOLERANCE, 180);
            }

            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_OFFSET_X, MIL.M_DEFAULT);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_OFFSET_Y, MIL.M_DEFAULT);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_SIZE_X, MIL.M_ALL);
            MIL.MpatControl(PatternId, MIL.M_ALL, MIL.M_SEARCH_SIZE_Y, MIL.M_ALL);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_SCALE_RANGE, MIL.M_ALL);

            MIL.MpatPreprocess(PatternId, MIL.M_DEFAULT, MIL.M_NULL);
        }

        public override void UpdateMaskImage()
        {
            if (MaskFigures.FigureExist == false)
            {
                return;
            }

            Image2D maskImage = GetMaskedImage();
            var greyImage = (MilGreyImage)ImageBuilder.MilImageBuilder.Build(maskImage, ImageType.Grey, ImageBandType.Luminance);
            MIL.MpatMask(PatternId, MIL.M_DEFAULT, greyImage.Image, MIL.M_DONT_CARE, MIL.M_DEFAULT);

            PreprocModel();

            maskImage.Dispose();
        }

        private void PreprocModel()
        {
            if (PatternId == MIL.M_NULL)
            {
                throw new InvalidOperationException();
            }

            MIL.MpatPreprocess(MIL.M_NULL, PatternId, MIL.M_DEFAULT);
        }

        public override Image2D GetMaskedImage()
        {
            var rgbImage = new Bitmap(PatternImage.Width, PatternImage.Height);
            ImageHelper.Clear(rgbImage, 0);

            Bitmap maskImage;
            if (MaskFigures.FigureExist == false)
            {
                maskImage = ImageHelper.ConvertGrayImage(rgbImage);
            }
            else
            {
                var g = Graphics.FromImage(rgbImage);

                MaskFigures.SetTempBrush(new SolidBrush(Color.White));

                MaskFigures.Draw(g, new CoordTransformer(), true);

                MaskFigures.ResetTempProperty();

                g.Dispose();

                maskImage = ImageHelper.ConvertGrayImage(rgbImage);
            }

            rgbImage.Dispose();

            return Image2D.ToImage2D(maskImage);
        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            MilImage.CheckGreyImage(targetClipImage, "MilPattern.Inspect", "Source");

            var greyImage = (MilGreyImage)targetClipImage;
            greyImage.Save("TargetImage1.bmp", debugContext);

            if (debugContext.SaveDebugImage == true)
            {
                ImageD image = MilImageBuilder.ConvertImage(greyImage);
                DynMvp.Vision.DebugHelper.SaveImage(image, "TargetImage2.bmp", debugContext);
            }
            DynMvp.Vision.DebugHelper.SaveImage(patternImage, "PatternImage.bmp", debugContext);

            var pmResult = new PatternResult();

            if (PatternId == MIL.M_NULL)
            {
                pmResult.ErrorString = "NotTrained";
                return pmResult;
            }

            MIL_ID patResultId = MIL.MpatAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_NULL);

            // Activate the search model angle mode.
            //MIL.MpatControl(patResultId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);

            //// Set the search model range angle.
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_DELTA_NEG, patternMatchingParam.MinAngle);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_DELTA_POS, patternMatchingParam.MaxAngle);

            //// Set the search model angle accuracy.
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_ACCURACY, patternMatchingParam.MinAngle);

            //// Set the search model angle interpolation mode to bilinear.
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_BILINEAR);

            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_DELTA_NEG, lfRange);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_DELTA_POS, lfRange);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_ACCURACY, angleAccuracy);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, interpolationMode);
            //MIL.MpatSetAngle(patternId, MIL.M_SEARCH_ANGLE_TOLERANCE, 180);

            MIL.MpatFind(PatternId, greyImage.Image, patResultId);

            Size patternSize = patternImage.Size;
            var imageRect = new RectangleF(0, 0, targetClipImage.Width, targetClipImage.Height);

            double numOccurDbl = 0;
            MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_NUMBER, ref numOccurDbl);
            long numOccurrences = (int)numOccurDbl;
            if (numOccurrences > 0)
            {
                double[] posX = new double[numOccurrences];
                double[] posY = new double[numOccurrences];
                double[] score = new double[numOccurrences];
                double[] angle = new double[numOccurrences];
                //double[] scale = new double[numOccurrences];

                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_SCORE, score);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_X, posX);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_Y, posY);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_ANGLE, angle);
                //MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_SCALE, scale);

                for (int i = 0; i < numOccurrences; i++)
                {
                    var matchPos = new MatchPos();
                    matchPos.Score = (float)score[i];
                    matchPos.Pos = new PointF((float)posX[i], (float)posY[i]);
                    matchPos.PatternSize = patternSize;
                    matchPos.PatternType = PatternType;
                    matchPos.Angle = (float)angle[i];
                    //matchPos.Scale = (float)scale[i];

                    RectangleF patternRect = DrawingHelper.FromCenterSize(matchPos.Pos, new SizeF(patternSize.Width, patternSize.Height));

                    //if (imageRect.Contains(patternRect))
                    {
                        pmResult.ResultRect = new RotatedRect(patternRect, matchPos.Angle);

                        pmResult.AddMatchPos(matchPos);
                    }
                }
            }

            MIL.MpatFree(patResultId);

            return pmResult;
        }
    }
}

