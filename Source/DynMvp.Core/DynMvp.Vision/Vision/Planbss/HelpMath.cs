using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Planbss
{
    internal class HelpMath
    {
        public bool ToSplit(string strData)
        {
            bool bUse = true;

            if (strData.Equals("0") || strData.Equals("No") || strData.Equals("False"))
            {
                return false;
            }

            if (strData.Equals("1") || strData.Equals("Yes") || strData.Equals("True"))
            {
                return true;
            }

            Debug.Assert(false, "HelpMath::ToSplit-bool");

            return bUse;
        }

        public bool ToSplit(string strData, ref Point outData)
        {
            string[] strToken = strData.Split(',');
            if (strToken.Length != 2)
            {
                Debug.Assert(false, "HelpMath::ToSplit-POINT");
                return false;
            }

            outData.X = Convert.ToInt32(strToken[0]);
            outData.Y = Convert.ToInt32(strToken[1]);

            return true;
        }

        public bool ToSplit(string strData, ref Rectangle outData)
        {
            string[] strToken = strData.Split(',');
            if (strToken.Length != 4)
            {
                Debug.Assert(false, "HelpMath::ToSplit-RECT");
                return false;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // int[] arrData = strToken.Select(n => Convert.ToInt32(n)).ToArray();
            ///////////////////////////////////////////////////////////////////////////////////////////

            int L = Convert.ToInt32(strToken[0]);
            int T = Convert.ToInt32(strToken[1]);
            int R = Convert.ToInt32(strToken[2]);
            int B = Convert.ToInt32(strToken[3]);

            outData = Rectangle.FromLTRB(L, T, R, B);

            return true;
        }

        public double GetDistance(PointD pt1, PointD pt2)
        {
            return Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2));
        }

        public double GetDistance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2));
        }

        public PointD GetPtWithDA(PointD ptStd, double dDist, double dAngle, bool InverseY = true)
        {
            var OutPt = new PointD(0, 0);

            OutPt.X = ptStd.X + (dDist * Math.Cos(dAngle));
            OutPt.Y = ptStd.Y + (dDist * Math.Sin(dAngle) * (InverseY ? -1 : 1));

            return OutPt;
        }

        public PointD GetLinePt(PointD ptInput, double dAngle, PointD pOutPt)
        {
            var ptLineCenter = new PointD(pOutPt.X, pOutPt.Y);

            double slope = Math.Tan(dAngle) * (-1); // tan(dAngle) * (-1);
            double yCut = ptInput.Y - (slope * ptInput.X);

            if (Math.Abs(slope) > 1) { ptLineCenter.X = (pOutPt.Y - yCut) / slope; }
            else { ptLineCenter.Y = (slope * pOutPt.X) + yCut; }

            return ptLineCenter;
        }

        public bool GetCrossPt(double LineAngle1, PointD LintPt1, double LineAngle2, PointD LintPt2, out PointD ptCross)
        {
            ptCross = new PointD();

            ptCross.X = 0;
            ptCross.Y = 0;

            double fSlope1 = (-1) * Math.Tan(LineAngle1);	///< (-1) : Y-Axis Reverse
            double fSlope2 = (-1) * Math.Tan(LineAngle2);  ///< (-1) : Y-Axis Reverse

            double InterceptY_L1;
            double InterceptY_L2;
            double MaxSlope = 10000;

            // Case 1. Parallel Line
            if (Math.Abs(fSlope1 - fSlope2) < 0.001)
            {
                return false;
            }

            // Case 2. Infinite Slope
            if ((fSlope1 > MaxSlope) || (fSlope2 > MaxSlope))
            {
                if (fSlope1 > MaxSlope)
                {
                    InterceptY_L2 = (LintPt2.Y - fSlope2 * LintPt2.X);
                    ptCross.X = LintPt1.X;
                    ptCross.Y = fSlope2 * ptCross.X + InterceptY_L2;
                    return true;
                }

                if (fSlope2 > MaxSlope)
                {
                    InterceptY_L1 = (LintPt1.Y - fSlope1 * LintPt1.X);
                    ptCross.X = LintPt2.X;
                    ptCross.Y = fSlope1 * ptCross.X + InterceptY_L1;
                    return true;
                }

                return false;
            }

            // Case 3. Normal Line
            InterceptY_L1 = (LintPt1.Y - fSlope1 * LintPt1.X);
            InterceptY_L2 = (LintPt2.Y - fSlope2 * LintPt2.X);

            ptCross.X = (-1) * ((InterceptY_L1 - InterceptY_L2) / (fSlope1 - fSlope2));
            ptCross.Y = fSlope1 * ptCross.X + InterceptY_L1;

            return true;
        }

    }
}
