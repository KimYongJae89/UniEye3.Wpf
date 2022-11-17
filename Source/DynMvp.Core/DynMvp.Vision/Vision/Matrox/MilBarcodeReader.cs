using DynMvp.Base;
using DynMvp.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    internal class MilBarcodeReader : BarcodeReader
    {
        public override Algorithm Clone()
        {
            var barcodeReader = new MilBarcodeReader();
            barcodeReader.Copy(this);

            return barcodeReader;
        }

        public override SearchableResult Read(AlgoImage clipImage, RectangleF clipRect, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);

            var barcodeReaderParam = (BarcodeReaderParam)param;

            var barcodeReaderResult = new SearchableResult();

            const int STRING_LENGTH_MAX = 128;

            var greyImage = (MilGreyImage)clipImage;

            MIL_ID codeResults = MIL.M_NULL;
            MIL_ID barcode = MIL.M_NULL;

            MIL.McodeAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref barcode);
            MIL.McodeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref codeResults);

            bool barcode1DSearch = false;

            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                if (barcodeType == BarcodeType.Code128 || barcodeType == BarcodeType.Codabar || barcodeType == BarcodeType.Code39 || barcodeType == BarcodeType.Code93
                    || barcodeType == BarcodeType.EAN8 || barcodeType == BarcodeType.EAN13 || barcodeType == BarcodeType.EAN14 || barcodeType == BarcodeType.Interleaved2of5
                    || barcodeType == BarcodeType.Pharmacode || barcodeType == BarcodeType.GS1Databar || barcodeType == BarcodeType.PLANET || barcodeType == BarcodeType.POSTNET
                    || barcodeType == BarcodeType.FourStatePostal || barcodeType == BarcodeType.BC412 || barcodeType == BarcodeType.UPC_A || barcodeType == BarcodeType.UPC_E
                    || barcodeType == BarcodeType.GS1_128)
                {
                    barcode1DSearch = true;
                    break;
                }
            }

            if (barcode1DSearch == true)
            {
                foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
                {
                    switch (barcodeType)
                    {
                        case BarcodeType.Code128:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_CODE128, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.Codabar:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_CODABAR, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.Code39:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_CODE39, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.Code93:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_CODE93, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.EAN8:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_EAN8, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.EAN13:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_EAN13, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.EAN14:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_EAN14, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.Interleaved2of5:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_INTERLEAVED25, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.Pharmacode:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_PHARMACODE, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.GS1Databar:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_GS1_DATABAR, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.PLANET:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_PLANET, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.POSTNET:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_POSTNET, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.FourStatePostal:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_4_STATE, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.BC412:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_BC412, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.UPC_A:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_UPC_A, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.UPC_E:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_UPC_E, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.GS1_128:
                            MIL.McodeModel(barcode, MIL.M_ADD, MIL.M_GS1_128, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                    }
                }

                MIL.McodeControl(barcode, MIL.M_NUMBER, MIL.M_ALL);
                MIL.McodeControl(barcode, MIL.M_THRESHOLD_MODE, MIL.M_ADAPTIVE);
                MIL.McodeControl(barcode, MIL.M_POSITION_ACCURACY, MIL.M_HIGH);
                MIL.McodeControl(barcode, MIL.M_SEARCH_ANGLE_DELTA_NEG, 180);
                MIL.McodeControl(barcode, MIL.M_SEARCH_ANGLE_DELTA_POS, 180);
                MIL.McodeControl(barcode, MIL.M_MINIMUM_CONTRAST, 30);
                LogHelper.Debug(LoggerType.Inspection, "Mil barcode read start - 1");
                MIL.McodeRead(barcode, greyImage.Image, codeResults);

                int numberOccurrencesFound = 0;
                MIL.McodeGetResult(codeResults, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound);
                LogHelper.Debug(LoggerType.Inspection, "Mil barcode read end - 1");

                if (numberOccurrencesFound != 0)
                {
                    double[] positionX = new double[numberOccurrencesFound];
                    double[] positionY = new double[numberOccurrencesFound];
                    double[] width = new double[numberOccurrencesFound];
                    double[] height = new double[numberOccurrencesFound];
                    double[] angle = new double[numberOccurrencesFound];

                    MIL.McodeGetResult(codeResults, MIL.M_TOP_LEFT_X + MIL.M_TYPE_MIL_DOUBLE, positionX);
                    MIL.McodeGetResult(codeResults, MIL.M_TOP_LEFT_Y + MIL.M_TYPE_MIL_DOUBLE, positionY);

                    MIL.McodeGetResult(codeResults, MIL.M_SIZE_X + MIL.M_TYPE_MIL_DOUBLE, width);
                    MIL.McodeGetResult(codeResults, MIL.M_SIZE_Y + MIL.M_TYPE_MIL_DOUBLE, height);
                    MIL.McodeGetResult(codeResults, MIL.M_ANGLE + MIL.M_TYPE_MIL_DOUBLE, angle);

                    var barcodePositionList = new BarcodePositionList();

                    for (int index = 0; index < numberOccurrencesFound; index++)
                    {
                        double score = 0;
                        MIL.McodeGetResultSingle(codeResults, index, MIL.M_SCORE, ref score);

                        if (score > 0.9)
                        {
                            var barcodeString = new StringBuilder(STRING_LENGTH_MAX);
                            MIL.McodeGetResultSingle(codeResults, index, MIL.M_STRING, barcodeString);
                            var barcodePosition = new BarcodePosition();
                            barcodePosition.StringRead = barcodeString.ToString();

                            var positionList = new List<PointF>();
                            //positionList.Add(new PointF((float)positionX[index] + offsetX, (float)positionY[index] + offsetY));
                            //positionList.Add(new PointF((float)positionX[index] + (float)width[index] + offsetX, (float)positionY[index] + offsetY));
                            //positionList.Add(new PointF((float)positionX[index] + (float)width[index] + offsetX, (float)positionY[index] + (float)height[index] + offsetY));
                            //positionList.Add(new PointF((float)positionX[index] + offsetX, (float)positionY[index] + (float)height[index] + offsetY));
                            positionList.Add(new PointF((float)positionX[index], (float)positionY[index]));
                            positionList.Add(new PointF((float)positionX[index] + (float)width[index], (float)positionY[index]));
                            positionList.Add(new PointF((float)positionX[index] + (float)width[index], (float)positionY[index] + (float)height[index]));
                            positionList.Add(new PointF((float)positionX[index], (float)positionY[index] + (float)height[index]));
                            barcodePosition.FoundPosition = positionList;
                            barcodePosition.FoundAngle = (float)angle[index];

                            //barcodeReaderResult.ResultFigureGroup.AddFigure(new PolygonFigure(barcodePosition.FoundPosition, new Pen(Color.Lime)));

                            barcodePositionList.Items.Add(barcodePosition);
                        }
                    }

                    barcodeReaderResult.AddResultValue(new ResultValue("BarcodePositionList", "", barcodePositionList));
                }
            }

            MIL.McodeFree(barcode);
            MIL.McodeFree(codeResults);

            // 2D CODE READING:
            MIL_ID codeResults2D = MIL.M_NULL;
            MIL_ID barcode2D = MIL.M_NULL;

            MIL.McodeAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref barcode2D);
            MIL.McodeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref codeResults2D);

            bool barcode2DSearch = false;

            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                if (barcodeType == BarcodeType.DataMatrix || barcodeType == BarcodeType.QRCode)
                {
                    barcode2DSearch = true;
                }
            }

            if (barcode2DSearch == true)
            {
                foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
                {
                    switch (barcodeType)
                    {
                        case BarcodeType.DataMatrix:
                            MIL.McodeModel(barcode2D, MIL.M_ADD, MIL.M_DATAMATRIX, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                        case BarcodeType.QRCode:
                            MIL.McodeModel(barcode2D, MIL.M_ADD, MIL.M_QRCODE, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_NULL);
                            break;
                    }
                }

                MIL.McodeControl(barcode2D, MIL.M_DISTORTION, MIL.M_PERSPECTIVE_UNEVEN_GRID_STEP);
                //MIL.McodeControl(barcode2D, MIL.M_NUMBER, MIL.M_ALL);
                //MIL.McodeControl(barcode2D, MIL.M_TIMEOUT, 100000);
                //MIL.McodeControl(barcode2D, MIL.M_THRESHOLD_MODE, MIL.M_GLOBAL);
                MIL.McodeControl(barcode2D, MIL.M_FINDER_PATTERN_EXHAUSTIVE_SEARCH, MIL.M_ENABLE);
                MIL.McodeControl(barcode2D, MIL.M_CHECK_FINDER_PATTERN, MIL.M_ENABLE);
                //MIL.McodeControl(barcode2D, MIL.M_THRESHOLD_VALUE, imageProcessing.Otsu(algoImage, algoImage2));
                //MIL.McodeControl(barcode2D, MIL.M_FOREGROUND_VALUE, MIL.M_FOREGROUND_WHITE);
                //MIL.McodeControl(barcode2D, MIL.M_MINIMUM_CONTRAST, 100);
                //MIL.McodeControl(barcode2D, MIL.M_FINDER_PATTERN_EXHAUSTIVE_SEARCH, MIL.M_ENABLE);
                //MIL.McodeControl(barcode2D, MIL.M_USE_PRESEARCH, MIL.M_FINDER_PATTERN_BASE);
                //MIL.McodeControl(barcode2D, MIL.M_NUMBER, MIL.M_ALL);

                var barcodePositionList = new BarcodePositionList();

                if (barcodeReaderParam.UseBlobing)
                {
                    var blobParam = new BlobParam();

                    blobParam.SelectBoundingRect = true;

                    foreach (int threhold in barcodeReaderParam.ThresholdPercentList)
                    {
                        AlgoImage blobImage = clipImage.Clone();

                        imageProcessing.BinarizeHistogram(blobImage, blobImage, threhold);

                        imageProcessing.Not(blobImage, blobImage);
                        imageProcessing.Dilate(blobImage, barcodeReaderParam.CloseNum);
                        imageProcessing.Erode(blobImage, barcodeReaderParam.CloseNum);

                        blobImage.Save("blobImage.bmp", debugContext);

                        BlobRectList blobRectList = imageProcessing.Blob(blobImage, blobParam);
                        List<BlobRect> ListOfBlobRect = blobRectList.GetList();

                        ListOfBlobRect.Remove(blobRectList.GetMaxAreaBlob());

                        int imageNum = 0;

                        foreach (BlobRect blobRect in ListOfBlobRect)
                        {
                            if (barcodeReaderParam.UseAreaFilter == false ||
                                    (blobRect.Area >= barcodeReaderParam.MinArea && blobRect.Area <= barcodeReaderParam.MaxArea))
                            {
                                MIL.McodeControl(barcode2D, MIL.M_THRESHOLD_MODE, MIL.M_GLOBAL_SEGMENTATION);

                                int blobOffSetX = Math.Max(0, (int)blobRect.BoundingRect.X - 5);
                                int blobOffSetY = Math.Max(0, (int)blobRect.BoundingRect.Y - 5);
                                int blobWidth = (int)blobRect.BoundingRect.Width + 10;
                                int blobHeight = (int)blobRect.BoundingRect.Height + 10;

                                if (blobWidth + blobOffSetX > clipImage.Width)
                                {
                                    blobWidth = clipImage.Width - blobOffSetX;
                                }

                                if (blobHeight + blobOffSetY > clipImage.Height)
                                {
                                    blobHeight = clipImage.Height - blobOffSetY;
                                }

                                for (int i = 0; i < 2; i++)
                                {
                                    var milGreyImage = (MilGreyImage)clipImage.Clip(new Rectangle(blobOffSetX, blobOffSetY, blobWidth, blobHeight));

                                    if (milGreyImage.Image == MIL.M_NULL)
                                    {
                                        continue;
                                    }

                                    string imageName = "ROIImage " + imageNum++ + ".bmp";

                                    milGreyImage.Save(imageName, debugContext);
                                    LogHelper.Debug(LoggerType.Inspection, "Mil barcode read start - 2");
                                    MIL.McodeRead(barcode2D, milGreyImage.Image, codeResults2D);

                                    MIL_INT numberOccurrencesFound = 0;
                                    MIL.McodeGetResult(codeResults2D, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound);
                                    LogHelper.Debug(LoggerType.Inspection, "Mil barcode read end - 2");

                                    if (numberOccurrencesFound != 0)
                                    {
                                        double[] positionX = new double[numberOccurrencesFound];
                                        double[] positionY = new double[numberOccurrencesFound];
                                        double[] width = new double[numberOccurrencesFound];
                                        double[] height = new double[numberOccurrencesFound];
                                        double[] angle = new double[numberOccurrencesFound];

                                        MIL.McodeGetResult(codeResults2D, MIL.M_POSITION_X + MIL.M_TYPE_MIL_DOUBLE, positionX);
                                        MIL.McodeGetResult(codeResults2D, MIL.M_POSITION_Y + MIL.M_TYPE_MIL_DOUBLE, positionY);

                                        MIL.McodeGetResult(codeResults2D, MIL.M_SIZE_X + MIL.M_TYPE_MIL_DOUBLE, width);
                                        MIL.McodeGetResult(codeResults2D, MIL.M_SIZE_Y + MIL.M_TYPE_MIL_DOUBLE, height);
                                        MIL.McodeGetResult(codeResults2D, MIL.M_ANGLE + MIL.M_TYPE_MIL_DOUBLE, angle);

                                        for (int index = 0; index < numberOccurrencesFound; index++)
                                        {
                                            var barcodeString2D = new StringBuilder(STRING_LENGTH_MAX);

                                            MIL.McodeGetResultSingle(codeResults2D, index, MIL.M_STRING, barcodeString2D);
                                            var barcodePosition = new BarcodePosition();

                                            byte[] firstChar = Encoding.ASCII.GetBytes(barcodeString2D.ToString(0, 1));
                                            byte[] excludeChar = { 63 };
                                            if (firstChar[0] == excludeChar[0])
                                            {
                                                barcodePosition.StringRead = barcodeString2D.ToString().Remove(0, 1);
                                            }
                                            else
                                            {
                                                barcodePosition.StringRead = barcodeString2D.ToString();
                                            }

                                            var positionList = new List<PointF>();
                                            positionList.Add(new PointF((float)positionX[index] + blobOffSetX, (float)positionY[index] + blobOffSetY));
                                            positionList.Add(new PointF((float)positionX[index] + (float)width[index] + blobOffSetX, (float)positionY[index] + blobOffSetY));
                                            positionList.Add(new PointF((float)positionX[index] + (float)width[index] + blobOffSetX, (float)positionY[index] + (float)height[index] + blobOffSetY));
                                            positionList.Add(new PointF((float)positionX[index] + blobOffSetX, (float)positionY[index] + (float)height[index] + blobOffSetY));

                                            barcodePosition.FoundPosition = positionList; // GetBoundPointList(positionList, angle[index]);
                                            barcodePosition.FoundAngle = (float)angle[index];

                                            barcodePosition.Area = (int)blobRect.Area;

                                            float minX = Math.Min(barcodePosition.FoundPosition[0].X, barcodePosition.FoundPosition[1].X);
                                            minX = Math.Min(minX, barcodePosition.FoundPosition[2].X);
                                            minX = Math.Min(minX, barcodePosition.FoundPosition[3].X);

                                            float maxX = Math.Max(barcodePosition.FoundPosition[0].X, barcodePosition.FoundPosition[1].X);
                                            maxX = Math.Max(maxX, barcodePosition.FoundPosition[2].X);
                                            maxX = Math.Max(maxX, barcodePosition.FoundPosition[3].X);

                                            float minY = Math.Min(barcodePosition.FoundPosition[0].Y, barcodePosition.FoundPosition[1].Y);
                                            minY = Math.Min(minY, barcodePosition.FoundPosition[2].Y);
                                            minY = Math.Min(minY, barcodePosition.FoundPosition[3].Y);

                                            float maxY = Math.Max(barcodePosition.FoundPosition[0].Y, barcodePosition.FoundPosition[1].Y);
                                            maxY = Math.Max(maxY, barcodePosition.FoundPosition[2].Y);
                                            maxY = Math.Max(maxY, barcodePosition.FoundPosition[3].Y);


                                            bool includeBarcode = false;

                                            foreach (BarcodePosition barcodePositionOfList in barcodePositionList.Items)
                                            {
                                                float centerX = (barcodePositionOfList.FoundPosition[0].X + barcodePositionOfList.FoundPosition[2].X) / 2.0f;
                                                float centerY = (barcodePositionOfList.FoundPosition[0].Y + barcodePositionOfList.FoundPosition[2].Y) / 2.0f;

                                                if (centerX >= minX && centerX <= maxX && centerY >= minY && centerY <= maxY)
                                                {
                                                    includeBarcode = true;
                                                    break;
                                                }
                                            }

                                            if (includeBarcode == false)
                                            {
                                                barcodePositionList.Items.Add(barcodePosition);
                                                //barcodeReaderResult.ResultFigureGroup.AddFigure(new PolygonFigure(barcodePosition.FoundPosition, new Pen(Color.Lime, 3)));
                                            }

                                            milGreyImage.Dispose();
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        MIL.McodeControl(barcode2D, MIL.M_THRESHOLD_MODE, MIL.M_ADAPTIVE);
                                    }
                                }
                            }
                        }

                        blobImage.Dispose();
                    }
                }
                else
                {
                    MIL.McodeControl(barcode2D, MIL.M_NUMBER, MIL.M_ALL);
                    MIL.McodeControl(barcode2D, MIL.M_TIMEOUT, barcodeReaderParam.TimeoutTime);
                    MIL.McodeControl(barcode2D, MIL.M_THRESHOLD_MODE, MIL.M_ADAPTIVE);
                    LogHelper.Debug(LoggerType.Inspection, "Mil barcode read start -3");
                    MIL.McodeRead(barcode2D, ((MilGreyImage)clipImage).Image, codeResults2D);

                    MIL_INT numberOccurrencesFound = 0;
                    MIL.McodeGetResult(codeResults2D, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref numberOccurrencesFound);
                    LogHelper.Debug(LoggerType.Inspection, "Mil barcode read end - 3");

                    if (numberOccurrencesFound != 0)
                    {
                        double[] positionX = new double[numberOccurrencesFound];
                        double[] positionY = new double[numberOccurrencesFound];
                        double[] width = new double[numberOccurrencesFound];
                        double[] height = new double[numberOccurrencesFound];
                        double[] angle = new double[numberOccurrencesFound];

                        MIL.McodeGetResult(codeResults2D, MIL.M_POSITION_X + MIL.M_TYPE_MIL_DOUBLE, positionX);
                        MIL.McodeGetResult(codeResults2D, MIL.M_POSITION_Y + MIL.M_TYPE_MIL_DOUBLE, positionY);

                        MIL.McodeGetResult(codeResults2D, MIL.M_SIZE_X + MIL.M_TYPE_MIL_DOUBLE, width);
                        MIL.McodeGetResult(codeResults2D, MIL.M_SIZE_Y + MIL.M_TYPE_MIL_DOUBLE, height);
                        MIL.McodeGetResult(codeResults2D, MIL.M_ANGLE + MIL.M_TYPE_MIL_DOUBLE, angle);

                        for (int index = 0; index < numberOccurrencesFound; index++)
                        {
                            var barcodeString2D = new StringBuilder(STRING_LENGTH_MAX);

                            MIL.McodeGetResultSingle(codeResults2D, index, MIL.M_STRING, barcodeString2D);
                            var barcodePosition = new BarcodePosition();

                            byte[] firstChar = Encoding.ASCII.GetBytes(barcodeString2D.ToString(0, 1));
                            byte[] excludeChar = { 63 };
                            if (firstChar[0] == excludeChar[0])
                            {
                                barcodePosition.StringRead = barcodeString2D.ToString().Remove(0, 1);
                            }
                            else
                            {
                                barcodePosition.StringRead = barcodeString2D.ToString();
                            }

                            var positionList = new List<PointF>();
                            positionList.Add(new PointF((float)positionX[index], (float)positionY[index]));
                            positionList.Add(new PointF((float)positionX[index] + (float)width[index], (float)positionY[index]));
                            positionList.Add(new PointF((float)positionX[index] + (float)width[index], (float)positionY[index] + (float)height[index]));
                            positionList.Add(new PointF((float)positionX[index], (float)positionY[index] + (float)height[index]));

                            barcodePosition.FoundPosition = positionList; // GetBoundPointList(positionList, angle[index]);
                            barcodePosition.FoundAngle = (float)angle[index];

                            barcodePositionList.Items.Add(barcodePosition);
                            // barcodeReaderResult.ResultFigureGroup.AddFigure(new PolygonFigure(barcodePosition.FoundPosition, new Pen(Color.Lime, 3)));
                        }
                    }
                }

                barcodeReaderResult.AddResultValue(new ResultValue("BarcodePositionList", "", barcodePositionList));
            }

            MIL.McodeFree(barcode2D);
            MIL.McodeFree(codeResults2D);

            greyImage.Dispose();

            return barcodeReaderResult;
        }

        private List<PointF> GetBoundPointList(List<PointF> pointList, double angle)
        {
            var boundPointList = new List<PointF>();

            PointF centerPoint = DrawingHelper.CenterPoint(pointList[0], pointList[2]);

            boundPointList.Add(MathHelper.Rotate(pointList[0], pointList[0], angle));
            boundPointList.Add(MathHelper.Rotate(pointList[1], pointList[0], angle));
            boundPointList.Add(MathHelper.Rotate(pointList[2], pointList[0], angle));
            boundPointList.Add(MathHelper.Rotate(pointList[3], pointList[0], angle));

            return boundPointList;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
