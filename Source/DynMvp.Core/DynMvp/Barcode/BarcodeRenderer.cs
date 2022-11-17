using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Barcode
{
    public enum BarcodeRendererType
    {
        Code128, Code39, DataMatrix, QRCode
    }

    public abstract class BarcodeRenderer
    {
        public Size Size { get; set; }

        public static BarcodeRenderer GetBarcodeRenderer(BarcodeRendererType barcodeRendererType)
        {
            switch (barcodeRendererType)
            {
                case BarcodeRendererType.Code128:
                    return new Code128Renderer();
                case BarcodeRendererType.Code39:
                    return new Code39Renderer();
                //case BarcodeRendererType.DataMatrix:
                //    return new DataMatrixRenderer();
                //case BarcodeRendererType.QRCode:
                //    return new QRCodeRenderer();
                default:
                    return new Code128Renderer();
            }

        }

        public abstract Image GetBarcodeImage(string inputData);
    }
}
