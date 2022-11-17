using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class CircleSegment
    {
        public double CenterPtX { get; set; }
        public double CenterPtY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Angle1 { get; set; }
        public double Angle2 { get; set; }

        public void Clear()
        {
            CenterPtX = 0;
            CenterPtY = 0;
            Width = 0;
            Height = 0;
            Angle1 = 0;
            Angle2 = 0;
        }

        public void Initialize(NetEntry netEntry, bool cw, bool useMultiQuad, double deltaCpX, double deltaCpY)
        {
            if (useMultiQuad)
            {
                InitializeMultiQuad(netEntry, cw, deltaCpX, deltaCpY);
            }
            else
            {
                InitializeSingleQuad(netEntry, cw, deltaCpX, deltaCpY);
            }
        }

        /* ------------------------------------------------------------------ */
        private void InitializeSingleQuad(NetEntry netEntry, bool cw, double deltaCpX, double deltaCpY)
        {
            double d1x, d1y, d2x, d2y;
            double alfa, beta;
            int quadrant = 0;

            if (netEntry.StartX > netEntry.StopX)
            {
                // 1st and 2nd quadrant
                quadrant = (netEntry.StartY < netEntry.StopY ? 1 : 2);
            }
            else
            {
                // 3rd and 4th quadrant
                quadrant = (netEntry.StartY > netEntry.StopY ? 3 : 4);
            }

            // If clockwise, rotate quadrant
            if (cw)
            {
                if (quadrant < 3)
                {
                    quadrant = quadrant + 2;
                }
                else
                {
                    quadrant = quadrant - 2;
                }
            }

            // Calculate arc center point
            switch (quadrant)
            {
                case 1:
                    CenterPtX = netEntry.StartX - deltaCpX;
                    CenterPtY = netEntry.StartY - deltaCpY;
                    break;
                case 2:
                    CenterPtX = netEntry.StartX + deltaCpX;
                    CenterPtY = netEntry.StartY - deltaCpY;
                    break;
                case 3:
                    CenterPtX = netEntry.StartX + deltaCpX;
                    CenterPtY = netEntry.StartY + deltaCpY;
                    break;
                case 4:
                    CenterPtX = netEntry.StartX - deltaCpX;
                    CenterPtY = netEntry.StartY + deltaCpY;
                    break;
            }

            /*
            * Some good values 
            */
            d1x = Math.Abs(netEntry.StartX - CenterPtX);
            d1y = Math.Abs(netEntry.StartY - CenterPtY);
            d2x = Math.Abs(netEntry.StopX - CenterPtX);
            d2y = Math.Abs(netEntry.StopY - CenterPtY);

            alfa = Math.Atan2(d1y, d1x);
            beta = Math.Atan2(d2y, d2x);

            // Avoid divide by zero when sin(0) = 0 and cos(90) = 0
            Width = (alfa < beta ? 2 * (d1x / Math.Cos(alfa)) : 2 * (d2x / Math.Cos(beta)));

            if (alfa < 0.000001 && beta < 0.000001)
            {
                Height = 0;
            }
            else
            {
                Height = (alfa > beta ? 2 * (d1y / Math.Sin(alfa)) : 2 * (d2y / Math.Sin(beta)));
            }

            alfa = MathHelper.RadToDeg(alfa);
            beta = MathHelper.RadToDeg(beta);

            switch (quadrant)
            {
                case 1:
                    Angle1 = alfa;
                    Angle2 = beta;
                    break;
                case 2:
                    Angle1 = 180.0 - alfa;
                    Angle2 = 180.0 - beta;
                    break;
                case 3:
                    Angle1 = 180.0 + alfa;
                    Angle2 = 180.0 + beta;
                    break;
                case 4:
                    Angle1 = 360.0 - alfa;
                    Angle2 = 360.0 - beta;
                    break;
            }

            if (Width < 0.0)
            {
                LogHelper.Warn(LoggerType.Operation,
                    string.Format("Negative width {0} in quadrant {1} [{2}][{3}]", Width, quadrant, alfa, beta));
            }

            if (Height < 0.0)
            {
                LogHelper.Warn(LoggerType.Operation,
                    string.Format("Negative height {0} in quadrant {1} [{2}][{3}]", Height, quadrant, alfa, beta));
            }

            return;
        }

        /* ------------------------------------------------------------------ */
        private void InitializeMultiQuad(NetEntry netEntry, bool cw, double deltaCpX, double deltaCpY)
        {
            double d1x, d1y, d2x, d2y;
            double alfa, beta;

            CenterPtX = netEntry.StartX + deltaCpX;
            CenterPtY = netEntry.StartY + deltaCpY;

            /*
            * Some good values 
            */
            d1x = netEntry.StartX - CenterPtX;
            d1y = netEntry.StartY - CenterPtY;
            d2x = netEntry.StopY - CenterPtX;
            d2y = netEntry.StopY - CenterPtY;

            alfa = Math.Atan2(d1y, d1x);
            beta = Math.Atan2(d2y, d2x);

            Width = Math.Sqrt(deltaCpX * deltaCpX + deltaCpY * deltaCpY);
            Width *= 2.0;
            Height = Width;

            Angle1 = MathHelper.RadToDeg(alfa);
            Angle2 = MathHelper.RadToDeg(beta);

            /*
            * Make sure it's always positive angles
            */
            if (Angle1 < 0.0)
            {
                Angle1 += 360.0;
                Angle2 += 360.0;
            }

            if (Angle2 < 0.0)
            {
                Angle2 += 360.0;
            }

            if (Angle2 == 0.0)
            {
                Angle2 = 360.0;
            }

            /*
            * This is a sanity check for angles after the nature of atan2.
            * If cw we must make sure angle1-angle2 are always positive,
            * If ccw we must make sure angle2-angle1 are always negative.
            * We should really return one angle and the difference as GTK
            * uses them. But what the heck, it works for me.
            */
            if (cw)
            {
                if (Angle1 <= Angle2)
                {
                    Angle2 -= 360.0;
                }
                else if (Angle1 >= Angle2)
                {
                    Angle2 += 360.0;
                }
            }

            return;
        }
    }
}
