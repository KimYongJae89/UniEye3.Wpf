using DynMvp.Base;
using DynMvp.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    internal class MilCharReader : CharReader
    {
        public override Algorithm Clone()
        {
            var charReader = new MilCharReader();
            charReader.Copy(this);

            return charReader;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            var charReaderResult = new CharReaderResult();
            charReaderResult.SetResult(false);

            var charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == false)
            {
                return charReaderResult;
            }

            MIL_ID ocrFont = MIL.M_NULL;                      // OCR font identifier.
            MIL_ID ocrResult = MIL.M_NULL;                    // OCR result buffer identifier.

            var readString = new StringBuilder(9999); // Characters to read.

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            double charNum = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_NUMBER, ref charNum);

            var sb = new StringBuilder((int)charNum);
            MIL.MocrInquire(ocrFont, MIL.M_CONSTRAINT, sb);
            char[] constraintInFont = sb.ToString().ToArray();

            int count = 0;

            for (int i = 0; i < constraintInFont.Count(); i++)
            {
                if (constraintInFont[i] != 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            MIL.MocrAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref ocrResult);

            var greyImage = (MilGreyImage)clipImage;

            MIL.MocrReadString(greyImage.Image, ocrFont, ocrResult);

            double strNum = 0;

            MIL.MocrGetResult(ocrResult, MIL.M_NB_STRING, ref strNum);

            if (strNum == 0 || count >= charNum)
            {
                MIL.MocrFree(ocrFont);
                MIL.MocrFree(ocrResult);
                return charReaderResult;
            }

            for (int stringIndex = 0; stringIndex < strNum; stringIndex++)
            {
                MIL.MocrControl(ocrResult, MIL.M_SELECT_STRING, stringIndex);
                MIL.MocrGetResult(ocrResult, MIL.M_STRING, readString);

                if (readString.Length >= 1)
                {
                    double[] charScores = new double[readString.Length];
                    double[] posX = new double[readString.Length];
                    double[] posY = new double[readString.Length];
                    double[] sizeX = new double[readString.Length];
                    double[] sizeY = new double[readString.Length];

                    MIL.MocrGetResult(ocrResult, MIL.M_CHAR_SCORE, charScores);
                    MIL.MocrGetResult(ocrResult, MIL.M_CHAR_POSITION_X, posX);
                    MIL.MocrGetResult(ocrResult, MIL.M_CHAR_POSITION_Y, posY);
                    MIL.MocrGetResult(ocrResult, MIL.M_CHAR_SIZE_X, sizeX);
                    MIL.MocrGetResult(ocrResult, MIL.M_CHAR_SIZE_Y, sizeY);

                    float offsetX = inspectParam.ProbeRegionInFov.X - 10;
                    float offsetY = inspectParam.ProbeRegionInFov.Y - 10;

                    charReaderResult.StringRead += readString.ToString();

                    for (int index = 0; index < readString.Length; index++)
                    {
                        charReaderResult.ScoreList.Add((float)charScores[index]);

                        var charPosition = new CharPosition(readString[index],
                            new RotatedRect((float)posX[index] - ((float)sizeX[index] / 2), (float)posY[index] - ((float)sizeY[index] / 2), (float)sizeX[index], (float)sizeY[index], 0));

                        charPosition.Offset(offsetX, offsetY);

                        charReaderResult.AddCharPosition(charPosition);
                    }
                }
            }



            if (charReaderResult.CharPositionList.Count > 0)
            {
                charReaderResult.DesiredString = charReaderParam.DesiredString;

                if (charReaderParam.DesiredString.Length == 0 && charReaderParam.DesiredNumCharacter == 0)
                {
                    charReaderResult.SetResult(true);
                }
                else
                {
                    bool desiredStringResult = true;

                    if (charReaderParam.DesiredString != "")
                    {
                        desiredStringResult = (charReaderResult.StringRead.ToString().IndexOf(charReaderParam.DesiredString) > -1);
                    }

                    bool result = ((charReaderParam.DesiredNumCharacter == charReaderResult.StringRead.Length || charReaderParam.DesiredNumCharacter == 0) && desiredStringResult);
                    charReaderResult.SetResult(result);
                }

                if (charReaderParam.DesiredString == "" && charReaderResult.IsNG())
                {
                    for (int index = 0; index < charReaderResult.CharPositionList.Count; index++)
                    {
                        if (charReaderResult.ScoreList[index] < charReaderParam.MinScore)
                        {
                            charReaderResult.CharPositionList[index].ResultType = -1;
                            charReaderResult.SetResult(false);
                        }
                        else
                        {
                            charReaderResult.CharPositionList[index].ResultType = 1;
                        }
                    }
                }
                else if (charReaderResult.IsNG())
                {
                    int indexOffset = charReaderResult.StringRead.ToString().IndexOf(charReaderParam.DesiredString);
                    if (indexOffset != -1)
                    {
                        for (int index = 0; index < indexOffset; index++)
                        {
                            charReaderResult.CharPositionList[index].ResultType = -1;
                        }

                        for (int index = indexOffset + charReaderParam.DesiredString.Length; index < charReaderResult.StringRead.Length; index++)
                        {
                            charReaderResult.CharPositionList[index].ResultType = -1;
                        }

                        for (int index = 0; index < charReaderParam.DesiredString.Length; index++)
                        {
                            if (charReaderParam.DesiredString[index] != charReaderResult.StringRead[index + indexOffset] || charReaderResult.ScoreList[index + indexOffset] < charReaderParam.MinScore)
                            {
                                charReaderResult.CharPositionList[index + indexOffset].ResultType = -1;
                            }
                            else
                            {
                                charReaderResult.CharPositionList[index + indexOffset].ResultType = 1;
                            }
                        }
                    }
                }
                else
                {
                    int indexOffset = charReaderResult.StringRead.ToString().IndexOf(charReaderParam.DesiredString);
                    if (indexOffset != -1)
                    {
                        for (int index = 0; index < indexOffset; index++)
                        {
                            if (charReaderResult.ScoreList[index] < charReaderParam.MinScore)
                            {
                                charReaderResult.CharPositionList[index].ResultType = -1;
                                charReaderResult.SetResult(false);
                            }
                            else
                            {
                                charReaderResult.CharPositionList[index].ResultType = 0;
                            }
                        }

                        for (int index = indexOffset + charReaderParam.DesiredString.Length; index < charReaderResult.StringRead.Length; index++)
                        {
                            if (charReaderResult.ScoreList[index] < charReaderParam.MinScore)
                            {
                                charReaderResult.CharPositionList[index].ResultType = -1;
                                charReaderResult.SetResult(false);
                            }
                            else
                            {
                                charReaderResult.CharPositionList[index].ResultType = 0;
                            }
                        }

                        for (int index = 0; index < charReaderParam.DesiredString.Length; index++)
                        {
                            if (charReaderParam.DesiredString[index] != charReaderResult.StringRead[index + indexOffset] || charReaderResult.ScoreList[index] < charReaderParam.MinScore)
                            {
                                charReaderResult.CharPositionList[index + indexOffset].ResultType = -1;
                                charReaderResult.SetResult(false);
                            }
                            else
                            {
                                charReaderResult.CharPositionList[index + indexOffset].ResultType = 1;
                            }
                        }
                    }
                }
            }


            MIL.MocrFree(ocrFont);
            MIL.MocrFree(ocrResult);

            charReaderResult.AddResultValue(new ResultValue("Desired String", charReaderResult.DesiredString));
            charReaderResult.AddResultValue(new ResultValue("String Read", charReaderResult.StringRead));

            return charReaderResult;
        }


        public override CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, DebugContext debugContext)
        {
            throw new NotImplementedException();
        }

        public override void AddCharactor(CharPosition charPosition, int charactorCode)
        {
            throw new NotImplementedException();
        }

        public override void AddCharactor(AlgoImage charImage, string charactorCode)
        {
            var charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == false)
            {
                return;
            }

            MIL_ID ocrFont = MIL.M_NULL;

            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            double charNum = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_NUMBER, ref charNum);

            //var constraintInFont = new char[(int)charNum];
            var constraintInFont = new StringBuilder();
            string constraintInFontStr = constraintInFont.ToString();

            MIL.MocrInquire(ocrFont, MIL.M_CONSTRAINT, constraintInFont);

            int count = 0;

            for (int i = 0; i < constraintInFontStr.Count(); i++)
            {
                if (constraintInFont[i] != 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            double masStringSize = 0;
            MIL.MocrInquire(ocrFont, MIL.M_STRING_SIZE_MAX, ref masStringSize);

            if (count >= charNum)
            {
                string constraintString = charactorCode.ToString();
                string zeroString = '\0'.ToString();

                for (int i = 0; i < masStringSize; i++)
                {
                    if (i == 0)
                    {
                        MIL.MocrSetConstraint(ocrFont, i, MIL.M_ANY, constraintString);
                    }
                    else
                    {
                        MIL.MocrSetConstraint(ocrFont, i, MIL.M_ANY, zeroString);
                    }
                }
            }
            else
            {
                //var constraintString = new string(constraintInFont);
                string constraintString = constraintInFont.ToString();

                char separator = '\0';

                string[] separatedString = constraintString.Split(separator);

                if (separatedString[0].IndexOf(charactorCode) == -1)
                {
                    separatedString[0] += charactorCode;
                }

                for (int i = 0; i < masStringSize; i++)
                {
                    MIL.MocrSetConstraint(ocrFont, i, MIL.M_ANY, separatedString[0]);
                }
            }

            MIL.MocrCopyFont(((MilGreyImage)charImage).Image, ocrFont, MIL.M_COPY_TO_FONT, charactorCode);

            MIL.MocrSaveFont(charReaderParam.FontFileName, MIL.M_SAVE, ocrFont);
            MIL.MocrFree(ocrFont);
        }

        public override void RemoveFont(CharFont charFont)
        {
            var charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == false)
            {
                return;
            }

            MIL_ID ocrFont = MIL.M_NULL;

            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            //short[] constraintInFont = new short[200];
            var constraintInFont = new StringBuilder();
            double masStringSize = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CONSTRAINT, constraintInFont);
            MIL.MocrInquire(ocrFont, MIL.M_STRING_SIZE_MAX, ref masStringSize);

            //string constraintString = new string(constraintInFont);
            string constraintString = constraintInFont.ToString();
            char separator = '\0';

            string[] separatedString = constraintString.Split(separator);

            int index = separatedString[0].IndexOf(charFont.Character);

            separatedString[0] = separatedString[0].Remove(index, 1);

            for (int i = 0; i < masStringSize; i++)
            {
                MIL.MocrSetConstraint(ocrFont, i, MIL.M_ANY, separatedString[0]);
            }

            MIL.MocrSaveFont(charReaderParam.FontFileName, MIL.M_SAVE, ocrFont);
            MIL.MocrFree(ocrFont);
        }

        public override List<CharFont> GetFontList()
        {
            var charReaderParam = (CharReaderParam)param;

            var fontList = new List<CharFont>();

            MIL_ID ocrFont = MIL.M_NULL;                      // OCR font identifier.
            MIL_ID ocrResult = MIL.M_NULL;                    // OCR result buffer identifier.

            var readString = new StringBuilder(9999); // Characters to read.

            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            double charNum = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_NUMBER, ref charNum);

            //var constraintInFont = new char[(int)charNum];
            //MIL.MocrInquire(ocrFont, MIL.M_CONSTRAINT, constraintInFont);

            var sb = new StringBuilder((int)charNum);
            MIL.MocrInquire(ocrFont, MIL.M_CONSTRAINT, sb);
            char[] constraintInFont = sb.ToString().ToArray();

            double charWidth = 0;
            double charHeight = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_CELL_SIZE_X, ref charWidth);
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_CELL_SIZE_Y, ref charHeight);
            var milImageBuilder = new MilImageBuilder();

            var milImage = (MilGreyImage)milImageBuilder.Build(ImageType.Grey, (int)charWidth, (int)charHeight);

            int count = 0;

            for (int i = 0; i < constraintInFont.Count(); i++)
            {
                if (constraintInFont[i] != 0)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count >= charNum)
            {
                return fontList;
            }

            foreach (char character in constraintInFont)
            {
                if (character == 0)
                {
                    break;
                }

                string charInString = character.ToString();
                MIL.MocrCopyFont(milImage.Image, ocrFont, MIL.M_COPY_FROM_FONT, charInString);
                var charFont = new CharFont(milImage.ToImageD(), charInString, character);
                fontList.Add(charFont);
            }

            milImage.Dispose();

            MIL.MocrFree(ocrFont);

            return fontList;
        }

        public override void AutoSegmentation(AlgoImage algoImage, RotatedRect rotatedRect, string desiredString)
        {
            throw new NotImplementedException();
        }

        public override void Train(string fontFileName)
        {
            throw new NotImplementedException();
        }

        public override void SaveFontFile(string fontFileName)
        {

            return;
        }

        public override CharReaderResult Extract(AlgoImage algoImage, RectangleF characterRegion, int threshold, DebugContext debugContext)
        {
            var charReaderResult = new CharReaderResult();
            var charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == false)
            {
                return charReaderResult;
            }

            MIL_ID ocrFont = MIL.M_NULL;

            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var blobParam = new BlobParam();

            AlgoImage blobImage = algoImage.Clone();

            algoImage.Save("AlgoImage.bmp", debugContext);

            blobParam.SelectBoundingRect = true;
            blobParam.SelectCenterPt = true;

            imageProcessing.Binarize(blobImage);
            imageProcessing.Not(blobImage, blobImage);

            blobParam.SelectArea = true;
            blobParam.AreaMin = 10;

            BlobRectList blobRectList = imageProcessing.Blob(blobImage, blobParam);

            float offsetX = characterRegion.X;
            float offsetY = characterRegion.Y;

            double charWidth = 0;
            double charHeight = 0;
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_CELL_SIZE_X, ref charWidth);
            MIL.MocrInquire(ocrFont, MIL.M_CHAR_CELL_SIZE_Y, ref charHeight);

            for (int i = 0; i < blobRectList.GetList().Count; i++)
            {
                var blobRectangle = new Rectangle((int)((blobRectList.GetList()[i].BoundingRect.X + (blobRectList.GetList()[i].BoundingRect.Width / 2)) - (charWidth / 2)), (int)((blobRectList.GetList()[i].BoundingRect.Y + (blobRectList.GetList()[i].BoundingRect.Height / 2)) - (charHeight / 2)), (int)charWidth, (int)charHeight);
                var charPosition = new CharPosition(0, new RotatedRect(blobRectangle.X, blobRectangle.Y, blobRectangle.Width, blobRectangle.Height, 0));
                charPosition.Offset(offsetX, offsetY);

                charReaderResult.AddCharPosition(charPosition);
            }

            blobImage.Dispose();
            MIL.MocrFree(ocrFont);

            return charReaderResult;
        }

        public override bool Trained => false;

        public override int CalibrateFont(AlgoImage charImage, string calibrationString)
        {
            var calibrateImage = (MilGreyImage)charImage.Clone();
            var charReaderParam = (CharReaderParam)param;

            MIL_ID ocrFont = MIL.M_NULL;
            MIL.MocrRestoreFont(charReaderParam.FontFileName, MIL.M_RESTORE, MIL.M_DEFAULT_HOST, ref ocrFont);

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(charImage);
            var blobParam = new BlobParam();

            AlgoImage blobImage = charImage.Clone();

            blobParam.SelectBoundingRect = true;
            blobParam.SelectCenterPt = true;

            imageProcessing.Binarize(blobImage);
            imageProcessing.Not(blobImage, blobImage);

            blobParam.SelectArea = true;
            blobParam.AreaMin = 10;

            BlobRectList blobRectList = imageProcessing.Blob(blobImage, blobParam);

            int count = blobRectList.GetList().Count();

            if (blobRectList.GetList().Count() != calibrationString.Count())
            {
                calibrateImage.Dispose();
                blobImage.Dispose();

                MIL.MocrFree(ocrFont);

                return count;
            }

            BlobRect maxBlob = blobRectList.GetMaxAreaBlob();

            MIL.MocrControl(ocrFont, MIL.M_TARGET_CHAR_SIZE_X, maxBlob.BoundingRect.Width);
            MIL.MocrControl(ocrFont, MIL.M_TARGET_CHAR_SIZE_Y, maxBlob.BoundingRect.Height);

            MIL.MocrModifyFont(ocrFont, MIL.M_RESIZE, MIL.M_BILINEAR);

            MIL.MocrSaveFont(charReaderParam.FontFileName, MIL.M_SAVE, ocrFont);

            calibrateImage.Dispose();
            blobImage.Dispose();

            MIL.MocrFree(ocrFont);

            return count;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
