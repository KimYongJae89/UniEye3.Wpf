using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class ArcEq
    {
        public PointF Center { get; set; } = new PointF();
        public float XRadius { get; set; }
        public float YRadius { get; set; }
        public float StartAngle { get; set; }
        public float EndAngle { get; set; }

        public ArcEq()
        {

        }

        public ArcEq(PointF center, float xRadius, float yRadius, float startAngle, float endAngle)
        {
            Center = center;
            XRadius = xRadius;
            YRadius = yRadius;
            StartAngle = startAngle;
            EndAngle = endAngle;
        }

        public bool IsValid()
        {
            return XRadius > 0 && YRadius > 0;
        }
    }
}
