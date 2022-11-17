using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    internal class OpenCvBarcodeReader : BarcodeReader
    {
        public override Algorithm Clone()
        {
            var barcodeReader = new OpenCvBarcodeReader();
            barcodeReader.Copy(this);

            return barcodeReader;
        }

        public override SearchableResult Read(AlgoImage algoImage, RectangleF clipRect, DebugContext debugContext)
        {
            var barcodeReaderResult = new SearchableResult();

            return barcodeReaderResult;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
