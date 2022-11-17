using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Models
{
    public class LineModel
    {
        private double mx, my;

        public double Radian { get; set; }

        public double SX { get; set; }
        public double SY { get; set; }

        public LineModel compute_line(double dist)
        {
            var distLine = new LineModel();
            distLine.Radian = Radian;
            distLine.mx = mx;
            distLine.my = my;
            distLine.SX = SX + (dist * my);
            distLine.SY = SY + (dist * mx);

            //double 직교 = -(mx / my);

            return distLine;
        }

        public double compute_distance(Point pt)
        {
            // 한 점(x)로부터 직선(line)에 내린 수선의 길이(distance)를 계산한다.
            return Math.Abs((pt.X - SX) * my - (pt.Y - SY) * mx) / Math.Sqrt(mx * mx + my * my);
        }

        public int compute_model_parameter(Point[] inlierPoint)
        {
            // PCA 방식으로 직선 모델의 파라메터를 예측한다.
            double sx = 0, sy = 0;
            double sxx = 0, syy = 0;
            double sxy = 0, sw = 0;
            for (int i = 0; i < inlierPoint.Length; ++i)
            {
                double x = (double)inlierPoint[i].X;
                double y = (double)inlierPoint[i].Y;
                sx += x;
                sy += y;
                sxx += x * x;
                sxy += x * y;
                syy += y * y;
                sw += 1;
            }
            //variance;
            double vxx = (sxx - sx * sx / sw) / sw;
            double vxy = (sxy - sx * sy / sw) / sw;
            double vyy = (syy - sy * sy / sw) / sw;
            //principal axis
            double theta = Math.Atan2(2 * vxy, vxx - vyy) / 2;
            mx = Math.Cos(theta);
            my = Math.Sin(theta);
            //center of mass(xc, yc)
            SX = sx / sw;
            SY = sy / sw;

            Radian = my / mx;

            //직선의 방정식: sin(theta)*(x - sx) = cos(theta)*(y - sy);
            //직선의 방정식: my*(x - sx) = mx*(y - sy);
            //직선의 방정식: (my / mx)*(x - sx) + sy = y;
            //직선의 방정식: Radian*(x - sx) + sy = y;
            return 1;
        }

        public double model_verification(Point[] srcData, double distance_threshold, List<Point> inliner)
        {
            double cost = 0;
            for (int i = 0; i < srcData.Length; i++)
            {
                // 직선에 내린 수선의 길이를 계산한다.
                double distance = compute_distance(srcData[i]);
                // 예측된 모델에서 유효한 데이터인 경우, 유효한 데이터 집합에 더한다.
                if (distance < distance_threshold)
                {
                    cost++;
                    inliner.Add(srcData[i]);
                }
            }
            return cost;
        }

        public int GetX(int yIdx)
        {
            return (int)((mx * (yIdx - SY)) / my + SX);
        }
    }
}
