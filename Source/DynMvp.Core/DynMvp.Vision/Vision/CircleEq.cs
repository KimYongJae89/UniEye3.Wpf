using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class CircleEq
    {
        public PointF Center { get; set; } = new PointF();
        public float Radius { get; set; }

        public CircleEq()
        {

        }

        public CircleEq(PointF center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool IsValid()
        {
            return Center.X != 0 && Center.Y != 0 && Radius > 0;
        }
    }
}
