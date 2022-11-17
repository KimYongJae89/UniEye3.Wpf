using DynMvp.Base;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvPattern : Pattern
    {
        private OpenCvGreyImage cvImage;

        public OpenCvPattern() : base()
        {

        }

        public override Pattern Clone()
        {
            var openCvPattern = new OpenCvPattern();
            openCvPattern.Copy(this);

            return openCvPattern;
        }

        public override void Train(Image2D image, PatternMatchingParam patternMatchingParam)
        {
            patternImage = (Image2D)image.Clone();
            cvImage = (OpenCvGreyImage)ImageBuilder.OpenCvImageBuilder.Build(patternImage, ImageType.Grey);
        }

        public override void UpdateMaskImage()
        {

        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            var pmResult = new PatternResult();

            try
            {
                var targetOpenCvImage = targetClipImage as OpenCvGreyImage;

                Image<Gray, float> resultImage = targetOpenCvImage.Image.MatchTemplate(cvImage.Image, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);

                IntPtr ptrImage = resultImage.Ptr;

                resultImage.ConvertScale<byte>(255, 0).Save(string.Format("{0}\\Result.bmp", BaseConfig.Instance().TempPath));

                double minVal = 0, maxVal = 1;
                var minLoc = new Point();
                var maxLoc = new Point();

                while (maxVal > (float)patternMatchingParam.MatchScore / 100 && pmResult.MatchPosList.Count < 100)
                {
                    CvInvoke.MinMaxLoc(resultImage, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                    if (maxVal < (float)patternMatchingParam.MatchScore / 100)
                    {
                        break;
                    }

                    Size patternSize = cvImage.Image.Size;

                    for (int i = Math.Max(maxLoc.Y - (patternSize.Height / 2), 0); i < Math.Min(maxLoc.Y + (patternSize.Height / 2), resultImage.Size.Height); i++)
                    {
                        for (int k = Math.Max(maxLoc.X - (patternSize.Width / 2), 0); k < Math.Min(maxLoc.X + (patternSize.Width / 2), resultImage.Size.Width); k++)
                        {
                            CvInvoke.cvSetReal2D(resultImage.Ptr, i, k, 0);
                        }
                    }

                    resultImage.ConvertScale<byte>(255, 0).Save(string.Format("{0}\\Result.bmp", BaseConfig.Instance().TempPath));

                    var matchPos = new MatchPos
                    {
                        Score = (float)maxVal,
                        Pos = new PointF(maxLoc.X - (patternSize.Width / 2), maxLoc.Y - (patternSize.Height / 2)),
                        PatternSize = patternSize,
                        PatternType = PatternType
                    };

                    pmResult.AddMatchPos(matchPos);
                }

                //CvInvoke.MinMaxLoc(resultImage, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                //Size patternSize = cvImage.Image.Size;

                //MatchPos matchPos = new MatchPos();
                //matchPos.Score = (float)maxVal;
                //matchPos.Pos = new PointF(maxLoc.X + patternSize.Width / 2, maxLoc.Y + patternSize.Height / 2);
                //matchPos.PatternSize = patternSize;
                //matchPos.PatternType = PatternType;

                //if (patternMatchingParam.SubtractMatching == true)
                //{
                //    Rectangle findedPos = new Rectangle(maxLoc.X, maxLoc.Y, patternSize.Width, patternSize.Height);
                //    OpenCvGreyImage findedPosImage = (OpenCvGreyImage)targetOpenCvImage.Clone(findedPos);

                //    matchPos.Score = SubtractMatching(findedPosImage);
                //}


                //            Image<Gray, Byte> subtractImage = patternImage.Image.Sub(findedPosImage.Image);

                //pmResult.AddMatchPos(matchPos);
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvPattern.Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }
            return pmResult;
        }

        //private float SubtractMatching(OpenCvGreyImage findedPosImage)
        //{
        //    Byte[, ,] findedPosImageData = findedPosImage.Image.Data;
        //    Byte[, ,] patternImageData = patternImage.Image.Data;

        //    Size patternSize = patternImage.Image.Size;

        //    float weight;
        //    float totalWeight = 0;
        //    float totalValue = 0;
        //    for (int y = 0; y < patternSize.Height; ++y)
        //    {
        //        for (int x = 0; x < patternSize.Width; ++x)
        //        {
        //            weight = (float)(patternImageData[y, x, 0]) / 255;
        //            totalValue += Math.Abs(findedPosImageData[y, x, 0] - patternImageData[y, x, 0]) * weight;
        //            totalWeight += weight;
        //        }
        //    }

        //    float score = (100 - totalValue / totalWeight) / 100;
        //    if (score < 0)
        //        score = 0;

        //    return score;
        //}
    }
}
