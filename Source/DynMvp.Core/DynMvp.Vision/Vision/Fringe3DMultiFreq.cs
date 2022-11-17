using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision
{
    public class Fringe3DMultiFreq : Fringe3D
    {


        public Fringe3DMultiFreq(int shiftNumber, int width, int height, float frequencyRatio)
            : base(shiftNumber, width, height)
        {
            FrequencyRatio = frequencyRatio;
            MaxPhase = 1.0f / FrequencyRatio * (float)Math.PI;
            MinPhase = -MaxPhase;
            LowFreqRawImages = new float[ShiftNumber][];
        }


        public float FrequencyRatio { get; private set; } = 0.0f;

        public List<Bitmap> LowFreqBitmaps { get; } = new List<Bitmap>();

        private float[][] LowFreqRawImages { get; set; } = null;

        private float[] LowFreqReference { get; set; } = null;


        public override void AddImage(byte[] rawImageBytes, int index)
        {
            if (rawImageBytes.Length != ImageWidth * ImageHeight)
            {
                throw new Exception("이미지 크기 불일치");
            }

            if (index >= ShiftNumber * 2)
            {
                throw new Exception("인덱스 초과");
            }

            if (index < ShiftNumber)
            {
                RawImages[index] = Array.ConvertAll(rawImageBytes, new Converter<byte, float>(x => x / 255.0f));
            }
            else
            {
                LowFreqRawImages[index - ShiftNumber] = Array.ConvertAll(rawImageBytes, new Converter<byte, float>(x => x / 255.0f));
            }
        }

        public void SetLowFrequencyImages(List<Bitmap> bitmaps)
        {
            if (bitmaps.Count != LowFreqRawImages.Length || bitmaps == null)
            {
                throw new Exception("이미지 개수가 다름");
            }

            for (int i = 0; i < LowFreqRawImages.Length; ++i)
            {
                LowFreqRawImages[i] = BitmapToRawImage(bitmaps[i]);
            }
        }

        public override bool SaveReference(string path)
        {
            foreach (float[] rawImage in RawImages)
            {
                if (rawImage == null)
                {
                    return false;
                }
            }

            foreach (float[] lowFreqRawImage in LowFreqRawImages)
            {
                if (lowFreqRawImage == null)
                {
                    return false;
                }
            }

            Reference = ComputeWrappedPhaseMap(RawImages);
            LowFreqReference = ComputeWrappedPhaseMap(LowFreqRawImages);
            using (var writer = new BinaryWriter(File.OpenWrite(path)))
            {
                writer.Write(_stx);
                writer.Write(ImageWidth);
                writer.Write(ImageHeight);
                writer.Write(FrequencyRatio);
                byte[] tempMap = new byte[ImageWidth * ImageHeight * sizeof(float)];
                Buffer.BlockCopy(Reference, 0, tempMap, 0, tempMap.Length);
                writer.Write(tempMap);
                Buffer.BlockCopy(LowFreqReference, 0, tempMap, 0, tempMap.Length);
                writer.Write(tempMap);
            }
            return true;
        }

        public override bool LoadReference(string path)
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
                float frequencyRatio = reader.ReadSingle();
                if (width != ImageWidth || height != ImageHeight)
                {
                    throw new Exception("크기가 다른 레퍼런스");
                }

                if (frequencyRatio != FrequencyRatio)
                {
                    throw new Exception("주파수 비율이 다름");
                }

                byte[] tempMap = reader.ReadBytes(ImageWidth * ImageHeight * sizeof(float));
                Reference = new float[ImageWidth * ImageHeight];
                Buffer.BlockCopy(tempMap, 0, Reference, 0, tempMap.Length);
                tempMap = reader.ReadBytes(ImageWidth * ImageHeight * sizeof(float));
                LowFreqReference = new float[ImageWidth * ImageHeight];
                Buffer.BlockCopy(tempMap, 0, LowFreqReference, 0, tempMap.Length);
            }
            return true;
        }

        public override float[] Solve(Rectangle roi)
        {
            if (!CheckRoi(roi) || !IsReady())
            {
                return null;
            }

            // Roi 이미지 (소스, 레퍼런스)
            float[][] rawRoiImages = new float[RawImages.Length][];
            float[][] lowFreqRawRoiImages = new float[RawImages.Length][];
            for (int i = 0; i < RawImages.Length; ++i)
            {
                rawRoiImages[i] = RoiImage(RawImages[i], roi);
                lowFreqRawRoiImages[i] = RoiImage(LowFreqRawImages[i], roi);
            }

            // 생성
            float[] wrappedPhaseMap = ComputeWrappedPhaseMap(rawRoiImages);
            float[] lowFreqWrappedPhaseMap = ComputeWrappedPhaseMap(lowFreqRawRoiImages);
            wrappedPhaseMap = ComputeDiffPhaseMap(wrappedPhaseMap, RoiImage(Reference, roi));
            lowFreqWrappedPhaseMap = ComputeDiffPhaseMap(lowFreqWrappedPhaseMap, RoiImage(LowFreqReference, roi));
            float[] unwrappedPhaseMap = Unwrap(wrappedPhaseMap, lowFreqWrappedPhaseMap);
            NormalizePhaseMap(ref unwrappedPhaseMap);
            return unwrappedPhaseMap;
        }

        public override bool IsReady()
        {
            return base.IsReady() && LowFreqReference != null && LowFreqRawImages[0] != null;
        }

        private float[] Unwrap(float[] phaseMap, float[] lowFreqPhaseMap)
        {
            int imageSize = phaseMap.Length;
            float pi2 = 2.0f * (float)Math.PI;
            float pi2inv = 1.0f / pi2;
            float range = MaxPhase - MinPhase;
            float waveLengthRatio = 1.0f / FrequencyRatio;

            float[] unwrappedPhase = new float[imageSize];
            for (int i = 0; i < imageSize; ++i)
            {
                float k = (float)Math.Round((waveLengthRatio * lowFreqPhaseMap[i] - phaseMap[i]) * pi2inv);
                float phase = phaseMap[i] + pi2 * k;
                if (phase > MaxPhase)
                {
                    phase -= range;
                }
                else if (phase < MinPhase)
                {
                    phase += range;
                }

                unwrappedPhase[i] = phase;
            }
            return unwrappedPhase;
        }
    }
}
