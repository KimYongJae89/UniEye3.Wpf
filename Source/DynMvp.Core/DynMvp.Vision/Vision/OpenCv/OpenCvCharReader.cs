using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    internal class OpenCvCharReader : CharReader
    {
        public override Algorithm Clone()
        {
            var charReader = new OpenCvCharReader();
            charReader.Copy(this);

            return charReader;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            var charReaderResult = new CharReaderResult();

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

        }

        public override void RemoveFont(CharFont charFont)
        {

        }

        public override List<CharFont> GetFontList()
        {
            var fontList = new List<CharFont>();

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

            return charReaderResult;
        }

        public override bool Trained => false;

        public override int CalibrateFont(AlgoImage charImage, string calibrationString)
        {
            return 0;
        }
    }
}
