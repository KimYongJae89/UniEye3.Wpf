
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Base
{
    public class TransformData
    {
        public int CameraIndex { get; set; }
        public double Rx { get; set; } = 0;
        public double Ry { get; set; } = 0;
        public double Rz { get; set; } = 0;
        public double Tx { get; set; } = 0;
        public double Ty { get; set; } = 0;
        public double Tz { get; set; } = 0;


        public void LoadData(XmlElement transformDataElement)
        {
            CameraIndex = Convert.ToInt32(XmlHelper.GetValue(transformDataElement, "CameraIndex", "-1"));
            Rx = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "RX", "0"));
            Ry = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "RY", "0"));
            Rz = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "RZ", "0"));
            Tx = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "TX", "0"));
            Ty = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "TY", "0"));
            Tz = Convert.ToDouble(XmlHelper.GetValue(transformDataElement, "TZ", "0"));
        }

        public void SaveData(XmlElement transformDataElement)
        {
            XmlHelper.SetValue(transformDataElement, "CameraIndex", CameraIndex.ToString());
            XmlHelper.SetValue(transformDataElement, "RX", Rx.ToString());
            XmlHelper.SetValue(transformDataElement, "RY", Ry.ToString());
            XmlHelper.SetValue(transformDataElement, "RZ", Rz.ToString());
            XmlHelper.SetValue(transformDataElement, "TX", Tx.ToString());
            XmlHelper.SetValue(transformDataElement, "TY", Ty.ToString());
            XmlHelper.SetValue(transformDataElement, "TZ", Tz.ToString());
        }

        public TransformData Clone()
        {
            var transformData = new TransformData();
            transformData.Copy(this);

            return transformData;
        }

        public void Copy(TransformData srcTransformData)
        {
            CameraIndex = srcTransformData.CameraIndex;
            Rx = srcTransformData.Rx;
            Ry = srcTransformData.Ry;
            Rz = srcTransformData.Rz;
            Tx = srcTransformData.Tx;
            Ty = srcTransformData.Ty;
            Tz = srcTransformData.Tz;
        }

        public static TransformData[] BuildArray(int num)
        {
            var transformDataArray = new TransformData[num];
            for (int i = 0; i < num; i++)
            {
                transformDataArray[i] = new TransformData();
            }

            return transformDataArray;
        }

        public bool IsInitialized()
        {
            return (Rx != 0 || Ry != 0 || Rz != 0 || Tx != 0 || Ty != 0 || Tz != 0);
        }

        public void Reset()
        {
            Rx = 0;
            Ry = 0;
            Rz = 0;
            Tx = 0;
            Ty = 0;
            Tz = 0;
        }
    }

    public class TransformDataList
    {
        private List<TransformData> transformDataList = new List<TransformData>();

        public void Reset()
        {
            transformDataList.Clear();
        }

        public IEnumerator<TransformData> GetEnumerator()
        {
            return transformDataList.GetEnumerator();
        }

        public void Add(TransformData transformData)
        {
            transformDataList.Add(transformData);
        }

        public TransformData GetTransformData(int cameraIndex)
        {
            foreach (TransformData transformData in transformDataList)
            {
                if (transformData.CameraIndex == cameraIndex)
                {
                    return transformData;
                }
            }

            return null;
        }

        public TransformDataList Clone()
        {
            var newTransformDataList = new TransformDataList();

            foreach (TransformData transformData in transformDataList)
            {
                newTransformDataList.Add(transformData.Clone());
            }

            return newTransformDataList;
        }

        public void SetTransformData(int cameraIndex, TransformData newTransformData)
        {
            TransformData transformData = GetTransformData(cameraIndex);
            if (transformData != null)
            {
                transformDataList.Remove(transformData);
            }

            transformDataList.Add(newTransformData);
        }
    }

    public class Transform3d
    {
        private TransformData transformData = new TransformData();
        private Point3d[] pointOrigin = new Point3d[4];

        public Transform3d()
        {
        }

        public void Transform(Point3d[] pointArray)
        {
            LogHelper.Debug(LoggerType.Grab, "Transform");

            Trace.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", transformData.Tx, transformData.Ty, transformData.Tz, transformData.Rx, transformData.Ry, transformData.Rz));

            //절대 순서를 바꾸지 말것!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Translate(transformData.Tx, transformData.Ty, transformData.Tz, pointArray);
            RotateX(transformData.Rx, pointArray);
            RotateY(transformData.Ry, pointArray);
            RotateZ(transformData.Rz, pointArray);
        }

        public void FindRT(Point3d[] pointArray)
        {
            Debug.Assert(pointArray.Length == 4);

            //절대 순서를 바꾸지 말것!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //1. calc Weight Centroid
            Point3d ptCentroid = GetWeightCentroid(pointArray);
            //2. align centroid
            ptCentroid.X *= -1;
            ptCentroid.Y *= -1;
            ptCentroid.Z *= -1;
            Translate(ptCentroid, pointArray);

            transformData.Tx = ptCentroid.X;
            transformData.Ty = ptCentroid.Y;
            transformData.Tz = ptCentroid.Z;
            PlaneFit(pointArray, out double sX, out double sY);

            double aX = Math.Atan2(sX, 1.0) * 180.0 / Math.PI;
            double aY = Math.Atan2(sY, 1.0) * 180.0 / Math.PI;
            Trace.Write(string.Format("aX={0}, aY={1}", aX, aY));

            RotateX(-aY, pointArray);
            transformData.Rx = -aY; /////////////////////////////////////

            PlaneFit(pointArray, out sX, out sY);
            aX = Math.Atan2(sX, 1.0) * 180.0 / Math.PI;
            aY = Math.Atan2(sY, 1.0) * 180.0 / Math.PI;
            Trace.Write(string.Format("aX={0}, aY={1}", aX, aY));
            RotateY(aX, pointArray);
            transformData.Ry = aX;/////////////////////////////////////

            PlaneFit(pointArray, out sX, out sY);
            aX = Math.Atan2(sX, 1.0) * 180.0 / Math.PI;
            aY = Math.Atan2(sY, 1.0) * 180.0 / Math.PI;
            Trace.Write(string.Format("aX={0}, aY={1}", aX, aY));

            //6. calc Z angle
            double aOrg, aTran, dtemp;
            double[] ar = new double[4];
            double avgZAng = 0;
            for (int i = 0; i < 4; i++)
            {
                aOrg = pointOrigin[i].GetAngle();
                aTran = pointArray[i].GetAngle();
                dtemp = aTran - aOrg;

                if (dtemp < 0)
                {
                    dtemp = 360.0 + dtemp;
                }

                ar[i] = dtemp;
                avgZAng += dtemp;
                Trace.Write(string.Format("{0:0.000} ", ar[i]));
            }
            Trace.Write("\n");
            avgZAng /= 4.0;
            transformData.Rz = -avgZAng;//////////////////////////////////

            //7. rotate Z
            RotateZ(-avgZAng, pointArray);

            //8. Verify
            double diffX, diffY, diffZ;

            for (int i = 0; i < 4; i++)
            {
                diffX = pointArray[i].X - pointOrigin[i].X;
                diffY = pointArray[i].Y - pointOrigin[i].Y;
                diffZ = pointArray[i].Z - pointOrigin[i].Z;

                Trace.WriteLine(string.Format("{0},  eX={1:F9}, eY={2:F9}, eZ={3:F9}", i, diffX, diffY, diffZ));
            }
        }

        public void SetOrigin(Point3d[] pointCenter)
        {
            for (int i = 0; i < 4; i++)
            {
                pointOrigin[i] = pointCenter[i];
            }
        }

        public void SetTransformData(TransformData srcMapData)
        {
            if (srcMapData != null)
            {
                transformData.Copy(srcMapData);
            }
        }

        public void GetMapData(TransformData destMapData)
        {
            destMapData.Copy(transformData);
        }

        private void PlaneFit(Point3d[] pointArray, out double Xslope, out double Yslope)
        {
            int datacount = 4;
            double[] x = new double[100];
            double[] y = new double[100];
            double[] f = new double[100];
            double[] xk = new double[7];
            double[] yk = new double[7];
            double[,] a = new double[10, 11];

            for (int i = 1; i <= datacount; i++)
            {
                x[i] = pointArray[i - 1].X;
                y[i] = pointArray[i - 1].Y;
                f[i] = pointArray[i - 1].Z;
            }

            for (int i = 1; i <= 6; i++)
            {
                xk[i] = 0.0;
            }

            for (int i = 1; i <= 3; i++)
            {
                yk[i] = 0.0;
            }

            xk[1] = datacount;
            for (int i = 1; i <= datacount; i++)
            {
                xk[2] += x[i];
                xk[3] += y[i];
                xk[4] += x[i] * y[i];
                xk[5] += y[i] * y[i];
                xk[6] += x[i] * x[i];
                yk[1] += f[i];
                yk[2] += f[i] * x[i];
                yk[3] += f[i] * y[i];
            }
            a[1, 1] = xk[1];
            a[1, 2] = xk[2];
            a[1, 3] = xk[3];
            a[1, 4] = yk[1];
            a[2, 1] = xk[2];
            a[2, 2] = xk[6];
            a[2, 3] = xk[4];
            a[2, 4] = yk[2];
            a[3, 1] = xk[3];
            a[3, 2] = xk[4];
            a[3, 3] = xk[5];
            a[3, 4] = yk[3];

            //gau_jor

            double temp;
            int l = 3;
            int m = l + 1;
            for (int k = 1; k <= l; k++)
            {
                temp = a[k, k];
                for (int j = k; j <= m; j++)
                {
                    a[k, j] /= temp;
                }

                for (int i = 1; i <= l; i++)
                {
                    if (i != k)
                    {
                        temp = a[i, k];
                        for (int j = k; j <= m; j++)
                        {
                            a[i, j] -= temp * a[k, j];
                        }
                    }
                }
            }

            Trace.Write(string.Format("f(x) = {0:0.000} + {1:0.000} x + {2:0.000}y\n", a[1, 4], a[2, 4], a[3, 4]));
            double c = a[1, 4];
            Xslope = a[2, 4];
            Yslope = a[3, 4];
        }

        private void RotateX(double degree, Point3d[] pointArray)
        {
            LogHelper.Debug(LoggerType.Grab, "RotateX");

            double rad = degree * Math.PI / 180.0;
            double cosValue = Math.Cos(rad);
            double sinValue = Math.Sin(rad);

            Parallel.ForEach(pointArray, point =>
            {
                var tempPoint = new Point3d(point.X, point.Y, point.Z);
                point.Y = cosValue * tempPoint.Y - sinValue * tempPoint.Z;
                point.Z = sinValue * tempPoint.Y + cosValue * tempPoint.Z;
            });
        }

        private void RotateY(double degree, Point3d[] pointArray)
        {
            LogHelper.Debug(LoggerType.Grab, "RotateY");

            double rad = degree * Math.PI / 180.0;
            double cosValue = Math.Cos(rad);
            double sinValue = Math.Sin(rad);

            Parallel.ForEach(pointArray, point =>
            {
                var tempPoint = new Point3d(point.X, point.Y, point.Z);
                point.X = cosValue * tempPoint.X + sinValue * tempPoint.Z;
                point.Z = -sinValue * tempPoint.X + cosValue * tempPoint.Z;
            });
        }

        private void RotateZ(double degree, Point3d[] pointArray)
        {
            LogHelper.Debug(LoggerType.Grab, "RotateZ");

            double rad = degree * Math.PI / 180.0;

            double cosValue = Math.Cos(rad);
            double sinValue = Math.Sin(rad);

            Parallel.ForEach(pointArray, point =>
            {
                var tempPoint = new Point3d(point.X, point.Y, point.Z);
                point.X = cosValue * tempPoint.X - sinValue * tempPoint.Y;
                point.Y = sinValue * tempPoint.X + cosValue * tempPoint.Y;
            });
        }

        private void Translate(double dX, double dY, double dZ, Point3d[] pointArray)
        {
            LogHelper.Debug(LoggerType.Grab, "Translate");

            Parallel.ForEach(pointArray, point =>
            {
                point.X += dX;
                point.Y += dY;
                point.Z += dZ;
            });
        }

        private void Translate(Point3d offset, Point3d[] pointArray)
        {
            Parallel.ForEach(pointArray, point =>
            {
                point.X += offset.X;
                point.Y += offset.Y;
                point.Z += offset.Z;
            });
        }

        private Point3d GetWeightCentroid(Point3d[] pointArray)
        {
            var ptCentroid = new Point3d();

            foreach (Point3d point in pointArray)
            {
                ptCentroid.X += point.X;
                ptCentroid.Y += point.Y;
                ptCentroid.Z += point.Z;
            }

            ptCentroid.X /= pointArray.Length;
            ptCentroid.Y /= pointArray.Length;
            ptCentroid.Z /= pointArray.Length;

            return ptCentroid;
        }
    }
}
