using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class _1stPoliLineEq
    {
        private PointF slope = new PointF();
        public PointF Slope
        {
            get => slope;
            set => slope = value;
        }
        public float SlopeValue => slope.Y / slope.X;

        private PointF pointOnLine = new PointF();
        public PointF PointOnLine
        {
            get => pointOnLine;
            set => pointOnLine = value;
        }

        public _1stPoliLineEq()
        {

        }

        public _1stPoliLineEq(PointF pt1, PointF pt2)
        {
            slope.X = pt2.X - pt1.X;
            slope.Y = pt2.Y - pt1.Y;
            pointOnLine = pt1;
        }

        public _1stPoliLineEq(PointF pt1, float slopeX, float slopeY)
        {
            slope.X = slopeX;
            slope.Y = slopeY;
            pointOnLine = pt1;
        }

        public bool IsValid()
        {
            return (slope.X != 0 && slope.Y != 0);
        }

        public float GetY(float valueX)
        {
            return SlopeValue * (valueX - pointOnLine.X) + pointOnLine.Y;
        }

        public static bool GetIntersectPoint(_1stPoliLineEq lineEq1, _1stPoliLineEq lineEq2, ref PointF point)
        {
            if (lineEq1.IsValid() == false && lineEq2.IsValid() == false)
            {
                return false;
            }

            if (lineEq1.Slope.X == 0 && lineEq2.Slope.X == 0)
            {
                return false;
            }

            if (lineEq1.Slope.X == 0)
            {
                point.X = lineEq1.PointOnLine.X;
            }
            else if (lineEq2.Slope.X == 0)
            {
                point.X = lineEq2.PointOnLine.X;
            }
            else
            {
                PointF p1 = lineEq1.PointOnLine;
                PointF p2 = lineEq2.PointOnLine;

                float s1 = lineEq1.SlopeValue;
                float s2 = lineEq2.SlopeValue;

                point.X = (s1 * p1.X - s2 * p2.X - p1.Y + p2.Y) / (s1 - s2);
            }

            point.Y = lineEq1.GetY(point.X);

            return true;
        }

        public void Reset()
        {
            slope = new PointF(0, 0);
            pointOnLine = new PointF(0, 0);
        }

        public void FitLine(List<PointF> ptList)
        {
            Emgu.CV.CvInvoke.FitLine(ptList.ToArray(), out slope, out pointOnLine, Emgu.CV.CvEnum.DistType.L1, 0, 0.01, 0.01);
        }
    }

    public class _2ndPoliLineEq
    {
        public double Coeff_A1 { get; set; }
        public double Coeff_B1 { get; set; }
        public double Coeff_C1 { get; set; }

        public _2ndPoliLineEq()
        {

        }
        public _2ndPoliLineEq(double A, double B, double C)
        {
            Coeff_A1 = A;
            Coeff_B1 = B;
            Coeff_C1 = C;
        }

        public double GetY(double valueX)
        {
            return Coeff_A1 * (valueX * valueX) + Coeff_B1 * (valueX) + Coeff_C1;
        }
        public void Reset()
        {
            Coeff_A1 = 0;
            Coeff_B1 = 0;
            Coeff_C1 = 0;
        }
        public bool IsValid()
        {
            return (Coeff_A1 != 0 && Coeff_B1 != 0 && Coeff_C1 != 0);
        }
        public void FitLine(double[] x, double[] y)
        {
            var polyFit = new PolyFit<double>(x, y, 2);
            Coeff_A1 = polyFit.Coeff[2];
            Coeff_B1 = polyFit.Coeff[1];
            Coeff_C1 = polyFit.Coeff[0];
        }

    }
}
