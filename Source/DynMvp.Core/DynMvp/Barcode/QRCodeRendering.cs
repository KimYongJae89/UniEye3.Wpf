//using System;
//using System.Drawing;
//using System.Diagnostics;
//using System.Collections.Generic;

//using ZXing;
//using ZXing.Client.Result;
//using ZXing.Common;
//using ZXing.PDF417.Internal;
//using ZXing.QrCode.Internal;
//using ZXing.Rendering;
//using ZXing.Interop;
//using ZXing.Datamatrix;
//using ZXing.QrCode;

//namespace DynMvp.Barcode
//{
//    public class QRCodeRenderer : BarcodeRenderer
//    {
//        private Type Renderer { get; set; }
//        private bool TryMultipleBarcodes { get; set; }
//        BarcodeWriter writer = new BarcodeWriter();
//        QrCodeEncodingOptions options;
//        public QRCodeRenderer()
//        {
//            Renderer = typeof(BitmapRenderer);
//            writer.Format = BarcodeFormat.QR_CODE;
//            options = new QrCodeEncodingOptions();
//            options.PureBarcode = true;
//            options.Margin = 1;
//            writer.Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer);
//        }

//        public override Image GetBarcodeImage(string inputData)
//        {
//            int width = Size.Width;
//            int height = Size.Height;

//            if (string.IsNullOrEmpty(inputData))
//                inputData = "null";
//            if (width != height) // 사이즈가  같지 않을 경우 큰 사이즈 기준으로 사이즈를 통일 한다.
//            {
//                if (width > height)
//                    height = width;
//                else
//                    width = height;
//            }

//            //writer.Options.Width = width;
//            //writer.Options.Height = height;

//            writer.Options.PureBarcode = true;
//            writer.Options.Width *= 8;
//            writer.Options.Height *= 8;
//            writer.Options.Margin = 0;

//            Image image = writer.Write(inputData);
//            writer.Options.Width /= 8;
//            writer.Options.Height /= 8;
//#if DEBUG
//            image.Save(@"D:\QRTest.bmp");
//#endif
//            return image;
//        }
//    }
//}
