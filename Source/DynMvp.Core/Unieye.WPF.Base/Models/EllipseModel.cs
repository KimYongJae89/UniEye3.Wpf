//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Unieye.WPF.Base.Models
//{
//    public class EllipseModel
//    {
//        double a, b, c, d, e, f;    // 타원방정식: ax^2 + bxy + cy^2 + dx + ey + f = 0
//        public double cx, cy, width, height;        // 표준 형태: (x - cx)^2/width^2 + (y - cy)^2/height^2 = 1
//        public double theta;				// 표준 형태 타원의 기울어진 각도

//        public bool convertStdForm()
//        {
//            theta = Math.Atan2(b, a - c) / 2;
//            double ct = Math.Cos(theta);
//            double st = Math.Sin(theta);
//            double ap = (a * ct * ct) + (b * ct * st) + (c * st * st);
//            double cp = (a * st * st) - (b * ct * st) + (c * ct * ct);

//            cx = (2 * c * d - b * e) / (b * b - 4 * a * c);
//            cy = (2 * a * e - b * d) / (b * b - 4 * a * c);

//            double val = (a * cx * cx) + (b * cx * cy) + (c * cy * cy);
//            double scale_inv = val - f;

//            if (scale_inv / ap <= 0 || scale_inv / cp <= 0)
//                return false;

//            width = Math.Sqrt(scale_inv / ap);
//            height = Math.Sqrt(scale_inv / cp);

//            return true;
//        }

//        public double ComputeDistance(Point pt)
//        {
//            double e = Math.Abs((this.a * pt.X * pt.X) + (this.b * pt.X * pt.Y) + (this.c * pt.Y * pt.Y) + (this.d * pt.X) + (this.e * pt.Y) + this.f);
//            return Math.Sqrt(e);
//        }

//        public void ComputeModelParameter(Point[] dataPt, int cntData)
//        {
//            double[,] matA = new double[cntData, 5];
//            double[,] matB = new double[cntData, 1];

//            for (int index = 0; index < cntData; index++)
//            {
//                matA[index, 0] = dataPt[index].X * dataPt[index].Y;
//                matA[index, 1] = dataPt[index].Y * dataPt[index].Y;
//                matA[index, 2] = dataPt[index].X;
//                matA[index, 3] = dataPt[index].Y;
//                matA[index, 4] = 1;

//                matB[index, 0] = -dataPt[index].X * dataPt[index].X;
//            }

//            double[,] matAt = Matrix.Transpose(matA);
//            double[,] matAtA = Matrix.Product(matAt, matA);
//            double[,] invMatAtA = Matrix.Inverse(matAtA);
//            double[,] pinvA = Matrix.Product(invMatAtA, matAt);
//            double[,] X = Matrix.Product(pinvA, matB);

//            double ratio = (X[1, 0] > 1) ? 1 / X[1, 0] : 1;

//            this.a = ratio * 1;
//            this.b = ratio * X[0, 0];
//            this.c = ratio * X[1, 0];
//            this.d = ratio * X[2, 0];
//            this.e = ratio * X[3, 0];
//            this.f = ratio * X[4, 0];
//        }

//        public double ModelVerification(Point[] srcData, int cntSrcData, double distanceThreshold, List<Point> inLiers)
//        {
//            inLiers.Clear();
//            double cost = 0;
//            int dataIndex = 0;
//            double minAxis = (width > height) ? height : width;
//            double maxAxis = (width > height) ? width : height;
//            double[] distanceData = new double[srcData.Length];
//            foreach (Point pointPt in srcData)
//            {
//                double distance = ComputeDistance(srcData[dataIndex]);

//                if (distance < distanceThreshold)
//                {
//                    inLiers.Add(srcData[dataIndex++]);
//                    cost++;
//                }
//                //distanceData[dataIndex++] = distance;
//            }

//            //double average = distanceData.Average();
//            //double sumOfDerivation = 0;
//            //foreach (double value in distanceData)
//            //{
//            //    sumOfDerivation += (value) * (value);
//            //}
//            //double sumOfDerivationAverage = sumOfDerivation / (distanceData.Length - 1);
//            //double stdDev = Math.Sqrt(sumOfDerivationAverage - (average * average));

//            //dataIndex = 0;

//            //double threshold = 3 * stdDev > 50 ? 50 : 3 * stdDev;
//            //foreach (float distance in distanceData)
//            //{
//            //    if (distance < distanceThreshold)
//            //    {
//            //        inLiers.Add(srcData[dataIndex++]);
//            //        cost++;
//            //    }   
//            //}


//            return cost;
//        }
//    }
//}
