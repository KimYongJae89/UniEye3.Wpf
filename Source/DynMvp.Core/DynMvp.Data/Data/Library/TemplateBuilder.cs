using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Library
{
    public class Footprint
    {
        public RectangleF BodyPos { get; set; }
        public float BodyHeight { get; set; }
        public float BodyStandOffHeight { get; set; }
        public RectangleF PolarityMarkPos { get; set; }
        public List<RectangleF> LeadPosList { get; set; }
    }

    public class PackageBuilder
    {

    }
}
