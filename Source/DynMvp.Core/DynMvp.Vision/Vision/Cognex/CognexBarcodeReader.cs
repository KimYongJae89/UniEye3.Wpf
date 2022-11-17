using Cognex.VisionPro;
using Cognex.VisionPro.ID;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Cognex
{
    public class CognexBarcodeReader : BarcodeReader
    {
        public CogID CogId { get; set; } = new CogID();
        public int ProductCount { get; set; }

        public CognexBarcodeReader()
        {

        }

        public override Algorithm Clone()
        {
            var barcodeReader = new CognexBarcodeReader();
            barcodeReader.Copy(this);

            return barcodeReader;
        }

        public override SearchableResult Read(AlgoImage algoImage, RectangleF clipRect, DebugContext debugContext)
        {
            var centerPt = new PointF(algoImage.Width / 2, algoImage.Height / 2);

            var cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, algoImage.Width, algoImage.Height, 0, 0);

            var greyImage = (CognexGreyImage)algoImage;

            CogId.NumToFind = 100;

            EnableBarcodeTypes();
            CogId.DataMatrix.Enabled = true;

            CogIDResults results = CogId.Execute(greyImage.Image, cogRectangle);

            var barcodeReaderResult = new SearchableResult();

            if (results.Count > 0)
            {
                barcodeReaderResult.SetResult(true);

                var barcodePositionList = new BarcodePositionList();

                for (int i = 0; i < results.Count; i++)
                {
                    var barcodePosition = new BarcodePosition();
                    barcodePosition.StringRead = results[i].DecodedData.DecodedString;

                    barcodePosition.FoundPosition = new List<PointF>();

                    List<PointF> positionList = CognexHelper.Convert(results[i].BoundsPolygon);
                    for (int index = 3; index >= 0; index--)
                    {
                        barcodePosition.FoundPosition.Add(positionList[index]); // new PointF(positionList[index].X + rotatedRect.X, positionList[index].Y + rotatedRect.Y));
                    }

                    barcodePositionList.Items.Add(barcodePosition);
                }

                barcodeReaderResult.AddResultValue(new ResultValue("BarcodePositionList", "", barcodePositionList));
            }

            return barcodeReaderResult;
        }

        private void EnableBarcodeTypes()
        {
            CogId.DisableAllCodes();

            var barcodeReaderParam = (BarcodeReaderParam)param;

            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                switch (barcodeType)
                {
                    case BarcodeType.DataMatrix:
                        CogId.DataMatrix.Enabled = true;
                        break;
                    case BarcodeType.QRCode:
                        CogId.QRCode.Enabled = true;
                        break;
                    case BarcodeType.Codabar:
                        CogId.Codabar.Enabled = true;
                        break;
                    case BarcodeType.Code128:
                        CogId.Code128.Enabled = true;
                        break;
                    case BarcodeType.Code39:
                        CogId.Code39.Enabled = true;
                        break;
                    case BarcodeType.Code93:
                        CogId.Code93.Enabled = true;
                        break;
                    case BarcodeType.Interleaved2of5:
                        CogId.I2Of5.Enabled = true;
                        break;
                    case BarcodeType.Pharmacode:
                        CogId.Pharmacode.Enabled = true;
                        break;
                    case BarcodeType.PLANET:
                        CogId.Planet.Enabled = true;
                        break;
                    case BarcodeType.POSTNET:
                        CogId.Postnet.Enabled = true;
                        break;
                    case BarcodeType.FourStatePostal:
                        CogId.FourState.Enabled = true;
                        break;
                    case BarcodeType.UPCEAN:
                        CogId.UpcEan.Enabled = true;
                        break;
                    case BarcodeType.EANUCCComposite:
                        CogId.EANUCCComposite.Enabled = true;
                        break;
                    case BarcodeType.PDF417:
                        CogId.PDF417.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
