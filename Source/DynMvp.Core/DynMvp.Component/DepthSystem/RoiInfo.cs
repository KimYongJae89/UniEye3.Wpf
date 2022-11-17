using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Component.DepthSystem
{
    public class RoiInfo
    {
        public Rectangle Region { get; private set; }
        public byte[] MaskArray { get; private set; }
        public float[] XPos { get; private set; }
        public float[] YPos { get; private set; }
        public float[] ZHeight { get; private set; }
        public byte[] ModulationBuffer { get; private set; }
        public byte[] AmplitudeBuffer { get; private set; }

        //object refValue;

        public RoiInfo(int x, int y, int width, int height)
        {
            Initialize(new Rectangle(x, y, width, height));
        }

        public RoiInfo(Rectangle region)
        {
            Initialize(region);
        }

        private void Initialize(Rectangle region)
        {
            Region = region;

            int size = region.Width * region.Height;

            MaskArray = new byte[size];
            XPos = new float[size]; //3차원 데이터 저장용 
            YPos = new float[size]; //3차원 데이터 저장용 

            ZHeight = new float[size]; //3차원 데이터 저장용 
            for (int i = 0; i < size; i++)
            {
                ZHeight[i] = (float)i / size * 100;
            }

            AmplitudeBuffer = new byte[size];
            ModulationBuffer = new byte[size]; //모듈레이션 // 노이즈 처리용

            //refValue = null;
        }

        public Point3d[] ToArray()
        {
            var pointList = new List<Point3d>();

            //Parallel.For(0, xPos.Length, () => new List<Point3d>(), (i, loop, localPointList) =>
            //{
            //    if (zHeight[i] != 0)
            //    {
            //        localPointList.Add(new Point3d(xPos[i], yPos[i], zHeight[i]));
            //    }
            //    return localPointList;
            //},
            //(localPointList) =>
            //{
            //    lock (pointList)
            //    {
            //        pointList.AddRange(localPointList);
            //    }
            //});

            //Parallel.For(0, xPos.Length, i =>
            //{
            //    if (zHeight[i] != 0)
            //    {
            //        pointList.Add(new Point3d(xPos[i], yPos[i], zHeight[i]));
            //    }
            //});

            //for (int i = 0; i < xPos.Length; i++)
            //{
            //    if (zHeight[i] != 0)
            //    {
            //        pointList.Add(new Point3d(xPos[i], yPos[i], zHeight[i]));
            //    }
            //}

            //return pointList.ToArray();

            LogHelper.Debug(LoggerType.Grab, "Start Zero Count");

            int count = 0;
            for (int i = 0; i < XPos.Length; i++)
            {
                if (ZHeight[i] != 0)
                {
                    count++;
                }
            }

            LogHelper.Debug(LoggerType.Grab, "Start Build Array");

            int index = 0;
            var pointArray = new Point3d[count];
            for (int i = 0; i < XPos.Length; i++)
            {
                if (ZHeight[i] != 0)
                {
                    pointArray[index] = new Point3d(XPos[i], YPos[i], ZHeight[i]);
                    index++;
                }
            }

            LogHelper.Debug(LoggerType.Grab, "End Build Array");

            return pointArray;
        }
    }
}
