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

//namespace DynMvp.Barcode
//{
//    /// <summary>
//    /// Summary description for Barcode DataMatrix.
//    /// </summary>
//    public class DataMatrixRenderer : BarcodeRenderer
//    {
//        private Type Renderer { get; set; }
//        private bool TryMultipleBarcodes { get; set; }
//        BarcodeWriter writer = new BarcodeWriter();
//        DatamatrixEncodingOptions option;


//        public DataMatrixRenderer()
//        {
//            Renderer = typeof(BitmapRenderer);
//            writer.Format = BarcodeFormat.DATA_MATRIX;
//            option = new DatamatrixEncodingOptions();
//            option.Margin = 2;
//            option.PureBarcode = true;
//            option.SymbolShape = ZXing.Datamatrix.Encoder.SymbolShapeHint.FORCE_SQUARE;
//            writer.Options = option;
//            writer.Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer);
//        }

//        public override Image GetBarcodeImage(string inputData)
//        {
//            inputData = inputData.Replace("\u001e", "");
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

//            writer.Options.Width = Size.Width;
//            writer.Options.Height = Size.Height;
//            writer.Options.Width = Size.Width;
//            writer.Options.Height = Size.Height;
//            writer.Options.Margin = 0;
//            writer.Options.PureBarcode = true;

//            Image image = (Image)writer.Write(inputData).Clone();

//            //writer.Options.Width /= 8;
//            //writer.Options.Height /= 8;
//#if DEBUG
//            image.Save(@"D:\DataMatrix.bmp");
//#endif 
//            return image;
//        }
//    }
//}
