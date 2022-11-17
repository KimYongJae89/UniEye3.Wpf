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
    public class OpenCvColorMatchChecker : ColorMatchChecker
    {
        public OpenCvColorMatchChecker()
        {

        }

        public override Algorithm Clone()
        {
            var openCvColorMatchChecker = new OpenCvColorMatchChecker();
            openCvColorMatchChecker.Copy(this);

            return openCvColorMatchChecker;
        }

        public override bool Trained => false;

        public override void PrepareInspection()
        {
            Train();
        }

        public override void Train()
        {

        }

        public override SubResultList Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext)
        {
            var result = new SubResultList();

            return result;
        }

        public override void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
