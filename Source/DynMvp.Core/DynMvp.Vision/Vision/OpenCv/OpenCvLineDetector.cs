using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.OpenCv
{
    internal class OpenCvLineDetector : LineDetector
    {
        public override _1stPoliLineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext)
        {
            var greyImage = (OpenCvGreyImage)algoImage;

            var result = new EdgeDetectionResult();

            var lineEq = new _1stPoliLineEq();

            return lineEq;
        }
    }
}
