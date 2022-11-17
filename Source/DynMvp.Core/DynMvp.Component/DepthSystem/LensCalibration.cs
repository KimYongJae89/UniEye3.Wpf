using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem
{
    public class LensCalibration
    {
        private int version;
        private int regressionOrder = 2;
        private double[,] coefficient = new double[8, 10];
        public Size ImageSize { get; set; }

        public void Save(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Create);
            var binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(version);
            binaryWriter.Write(regressionOrder);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    binaryWriter.Write(coefficient[i, j]);
                }
            }
        }

        public void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                return;
            }

            var fileStream = new FileStream(fileName, FileMode.Open);
            var binaryReader = new BinaryReader(fileStream);

            version = binaryReader.ReadInt32();
            regressionOrder = binaryReader.ReadInt32();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    coefficient[i, j] = binaryReader.ReadDouble();
                }
            }
        }

        public Point3d[] ConvertPoint3D(Image3D image3d)
        {
            var pointList = new List<Point3d>();
            for (int yIndex = 0; yIndex < image3d.Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < image3d.Width; xIndex++)
                {
                    int index = yIndex * image3d.Pitch + xIndex;
                    CalculateMetricXY(xIndex, yIndex, image3d.Data[index], out double xPos, out double yPos);

                    pointList.Add(new Point3d(xPos, yPos, image3d.Data[index]));
                }
            }

            return pointList.ToArray();
        }


        public void CalculateMetricXY(double Xpix, double Ypix, double metricHeight, out double dstMetricX, out double dstMetricY)
        {
            double centerX = (ImageSize.Width - 1) / 2.0f;
            double centerY = (ImageSize.Height - 1) / 2.0f;

            double Z = metricHeight;

            double Rx = coefficient[0, 0] + coefficient[0, 1] * Z + coefficient[0, 2] * Z * Z;
            double Ax = coefficient[1, 0] + coefficient[1, 1] * Z + coefficient[1, 2] * Z * Z;
            double Bx = coefficient[2, 0] + coefficient[2, 1] * Z + coefficient[2, 2] * Z * Z;
            double Dx = coefficient[3, 0] + coefficient[3, 1] * Z + coefficient[3, 2] * Z * Z;

            double Ry = coefficient[4, 0] + coefficient[4, 1] * Z + coefficient[4, 2] * Z * Z;
            double Ay = coefficient[5, 0] + coefficient[5, 1] * Z + coefficient[5, 2] * Z * Z;
            double By = coefficient[6, 0] + coefficient[6, 1] * Z + coefficient[6, 2] * Z * Z;
            double Dy = coefficient[7, 0] + coefficient[7, 1] * Z + coefficient[7, 2] * Z * Z;

            double X = Xpix - centerX;
            double Y = Ypix - centerY;

            //Dx, Dy를 다시 0으로 테스트
            Dx = 0;
            Dy = 0;
            dstMetricX = Rx * X + Ax * X * X * X + Bx * X * Y * Y + Dx;
            dstMetricY = Ry * Y + Ay * Y * Y * Y + By * Y * X * X + Dy;
        }
    }
}
