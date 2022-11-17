using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class Point3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool Empty { get; private set; } = true;

        public Point3d()
        {

        }

        public Point3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

            Empty = false;
        }

        public void Reset()
        {
            X = Y = Z = 0;
            Empty = true;
        }

        public double GetAngle()
        {
            double angleRad = Math.Atan2(Y, X);
            return MathHelper.RadToDeg(angleRad);
        }

        public bool IsEmpty()
        {
            return Empty;
        }
    }
}
