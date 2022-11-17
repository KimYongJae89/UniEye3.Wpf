using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;

namespace UniScanC.Struct
{
    public class RoiMask : InputOutputs<AlgoImage, RectangleF[]>
    {
        public AlgoImage Mask { get => Item1; set => Item1 = value; }
        public RectangleF[] ROIs { get => Item2; set => Item2 = value; }

        public RoiMask() : base("Mask", "ROIs")
        {
            SetValues(null, new RectangleF[0]);
        }

        public RoiMask(AlgoImage mask, RectangleF[] ROIs) : this()
        {
            SetValues(mask, ROIs);
        }
    }
}
