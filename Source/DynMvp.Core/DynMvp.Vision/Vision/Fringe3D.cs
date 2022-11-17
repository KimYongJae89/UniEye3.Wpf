using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision
{
    public class Fringe3D
    {
        protected static string _stx = "FringePhaseMapFile";

        public Fringe3D(int shiftNumber, int width, int height)
        {
            if (shiftNumber != 4)
            {
                throw new Exception("아직 4-shift 만 구현됨");
            }

            if (width < 0 || height < 0)
            {
                throw new Exception("잘못된 이미지 크기");
            }

            ShiftNumber = shiftNumber;
            ImageWidth = width;
            ImageHeight = height;
            RawImages = new float[ShiftNumber][];
        }


        public int ShiftNumber { get; private set; }

        public int ImageWidth { get; private set; }

        public int ImageHeight { get; private set; }

        public float MinPhase { get; protected set; } = (float)(-Math.PI);

        public float MaxPhase { get; protected set; } = (float)(Math.PI);

        protected float[][] RawImages { get; private set; } = null;

        protected float[] Reference { get; set; } = null;


        public virtual void AddImage(byte[] rawImageBytes, int index)
        {
            if (rawImageBytes.Length != ImageWidth * ImageHeight)
            {
                throw new Exception("이미지 크기 불일치");
            }

            if (index >= RawImages.Length)
            {
                throw new Exception("인덱스 초과");
            }

            RawImages[index] = Array.ConvertAll(rawImageBytes, new Converter<byte, float>(x => x / 255.0f));
        }

        public virtual bool SaveReference(string path)
        {
            foreach (float[] rawImage in RawImages)
            {
                if (rawImage == null)
                {
                    return false;
                }
            }

            Reference = ComputeWrappedPhaseMap(RawImages);
            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                writer.Write(_stx);
                writer.Write(ImageWidth);
                writer.Write(ImageHeight);
                byte[] tempMap = new byte[ImageWidth * ImageHeight * sizeof(float)];
                Buffer.BlockCopy(Reference, 0, tempMap, 0, tempMap.Length);
                writer.Write(tempMap);
            }
            return true;
        }

        public virtual bool LoadReference(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (reader.ReadString() != _stx)
                {
                    throw new Exception("Moire 위상 맵 파일이 아님");
                }

                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                if (width != ImageWidth || height != ImageHeight)
                {
                    throw new Exception("크기가 다른 레퍼런스");
                }

                byte[] tempMap = reader.ReadBytes(ImageWidth * ImageHeight * sizeof(float));
                Reference = new float[ImageWidth * ImageHeight];
                Buffer.BlockCopy(tempMap, 0, Reference, 0, tempMap.Length);
            }
            return true;
        }

        //
        // ROI 전달시 0 에서 1 사이 정규화된 높이값을 가지는 배열이 생성된다.
        //
        public virtual float[] Solve(Rectangle roi)
        {
            if (!CheckRoi(roi) || !IsReady())
            {
                return null;
            }

            // Roi 이미지 (소스, 레퍼런스)
            float[][] rawRoiImages = new float[RawImages.Length][];
            for (int i = 0; i < RawImages.Length; ++i)
            {
                rawRoiImages[i] = RoiImage(RawImages[i], roi);
            }

            // 생성
            float[] wrappedPhaseMap = ComputeWrappedPhaseMap(rawRoiImages);
            wrappedPhaseMap = ComputeDiffPhaseMap(wrappedPhaseMap, RoiImage(Reference, roi));
            NormalizePhaseMap(ref wrappedPhaseMap);
            return wrappedPhaseMap;
        }

        public virtual bool IsReady()
        {
            return Reference != null && RawImages[0] != null;
        }

        protected bool CheckRoi(Rectangle roi)
        {
            if (roi.X < 0)
            {
                roi.X = 0;
            }

            if (roi.Y < 0)
            {
                roi.Y = 0;
            }

            if (roi.X + roi.Width > ImageWidth)
            {
                roi.Width = ImageWidth - roi.X;
            }

            if (roi.Y + roi.Height > ImageHeight)
            {
                roi.Height = ImageHeight - roi.Y;
            }

            if (roi.Width <= 0 || roi.Height <= 0)
            {
                return false;
            }

            return true;
        }

        protected float[] BitmapToRawImage(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int bitmapSize = bitmapData.Stride * bitmapData.Height;
            byte[] rawImageBytes = new byte[bitmapSize];
            Marshal.Copy(bitmapData.Scan0, rawImageBytes, 0, bitmapSize);
            bitmap.UnlockBits(bitmapData);
            float[] rawImage = Array.ConvertAll(rawImageBytes, new Converter<byte, float>(x => x / 255.0f));
            return rawImage;
        }

        protected float[] RoiImage(float[] rawImage, Rectangle roi)
        {
            float[] rawRoiImage = new float[roi.Width * roi.Height];
            for (int h = 0, idxSrc = roi.Y * ImageWidth + roi.X, idxDest = 0; h < roi.Height; ++h, idxSrc += ImageWidth, idxDest += roi.Width)
            {
                Array.Copy(rawImage, idxSrc, rawRoiImage, idxDest, roi.Width);
            }

            return rawRoiImage;
        }

        protected float[] ComputeWrappedPhaseMap(float[][] rawRoiImages)
        {
            int size = rawRoiImages[0].Length;
            float[] targetPhaseMap = new float[size];
            if (ShiftNumber == 4)
            {
                for (int i = 0; i < size; ++i)
                {
                    targetPhaseMap[i] = (float)Math.Atan2(rawRoiImages[3][i] - (double)rawRoiImages[1][i], rawRoiImages[0][i] - (double)rawRoiImages[2][i]);
                }
            }
            return targetPhaseMap;
        }

        protected float[] ComputeDiffPhaseMap(float[] sourcePhase, float[] referncePhase)
        {
            float pi = (float)Math.PI;
            float pi2 = 2 * pi;
            float piMin = -pi;
            float piMax = pi;

            int imageSize = sourcePhase.Length;
            float[] diff = new float[imageSize];
            for (int i = 0; i < imageSize; ++i)
            {
                float phase = sourcePhase[i] - referncePhase[i];
                if (phase > piMax)
                {
                    phase -= pi2;
                }
                else if (phase < piMin)
                {
                    phase += pi2;
                }

                diff[i] = phase;
            }
            return diff;
        }

        protected void NormalizePhaseMap(ref float[] phaseMap)
        {
            int imageSize = phaseMap.Length;
            for (int i = 0; i < imageSize; ++i)
            {
                phaseMap[i] = 1.0f - (phaseMap[i] - MinPhase) / (MaxPhase - MinPhase);
            }
        }
    }
}
