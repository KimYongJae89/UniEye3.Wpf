using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public class MilPointCloud
    {
        protected MIL_ID objectId = MIL.M_NULL;
        public MIL_ID ObjectId
        {
            get => objectId;
            set => objectId = value;
        }

        public MilPointCloud(Image3D image3d)
        {
            Import(image3d, 1);
        }

        public void Free()
        {

        }

        public void Import(Image3D image3d, int lable)
        {
            MIL_INT milLabel = lable | MIL.M_POINT_CLOUD_LABEL_FLAG;

            // 컨테이너 생성 및 설정
            MIL.M3dmapAllocResult(MIL.M_DEFAULT_HOST, MIL.M_POINT_CLOUD_CONTAINER, MIL.M_DEFAULT, ref objectId);
            MIL.M3dmapControl(objectId, MIL.M_GENERAL, MIL.M_MAX_FRAMES, 2048);

            int width = image3d.Width;
            int height = image3d.Height;

            // dataArray에는 z값만 들어있음.
            int dataCount = image3d.Data.Count();  // 4byte x, 4byte y, 4byte z = 1set
            var arrayX = new List<double>();
            var arrayY = new List<double>();
            var arrayZ = new List<double>();
            int vaildCount = 0;
            for (int i = 0; i < dataCount; i++)
            {
                if (image3d.Data[i] > 5)
                {
                    vaildCount++;
                    arrayX.Add(i % width);     // x pos
                    arrayY.Add(i / width);     // y pos
                    arrayZ.Add(image3d.Data[i]);    // z pos
                }
            }

            double[] x = arrayX.ToArray();
            double[] y = arrayY.ToArray();
            double[] z = arrayZ.ToArray();

            MIL.M3dmapPut(objectId, milLabel, MIL.M_POSITION, 64 + MIL.M_FLOAT, vaildCount, x, y, z, MIL.M_NULL, MIL.M_DEFAULT);
        }

        public Image3D Export(int width, int height, int label)
        {
            MIL_INT milLabel = label | MIL.M_POINT_CLOUD_LABEL_FLAG;

            MIL_INT bufSize = 0;
            MIL.M3dmapGet(objectId, milLabel, MIL.M_POSITION, MIL.M_DEFAULT, 64 + MIL.M_FLOAT, 0, null, null, (double[])null, ref bufSize);

            double[] arrayX = new double[bufSize];
            double[] arrayY = new double[bufSize];
            double[] arrayZ = new double[bufSize];

            MIL.M3dmapGet(objectId, milLabel, MIL.M_POSITION, MIL.M_DEFAULT, 64 + MIL.M_FLOAT, bufSize, arrayX, arrayY, arrayZ, ref bufSize);

            var pointArray = new Point3d[bufSize];

            for (int i = 0; i < bufSize; i++)
            {
                pointArray[i] = new Point3d(arrayX[i], arrayY[i], arrayZ[i]);
            }

            var imageMapper = new ImageMapper();
            imageMapper.Initialize(new RectangleF(0, 0, width, height), 1);
            imageMapper.Mapping(pointArray);

            imageMapper.Image3d.SaveImage(@"D:\AlignedImage.bmp", ImageFormat.Bmp);

            return imageMapper.Image3d;
        }
    }
}
