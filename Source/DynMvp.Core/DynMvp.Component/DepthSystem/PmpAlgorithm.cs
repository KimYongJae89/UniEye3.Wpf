using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem
{
    public class PmpAlgorithm
    {
        private Size imageSize = new Size(0, 0);
        private CalcBuffer[] calcBuffer;
        private LensCalibration lensCalibration;
        private const int AtanLut4Size = (511 * 511);
        private const int AtanLut5Size = ((785 * 2 + 1) * (825 * 2 + 1));
        private float[] atanLut4 = new float[AtanLut4Size];
        private float[] atanLut5 = new float[AtanLut5Size];

        private void MakeAtanLut()
        {
            int i, j;

            //4bucket
            for (j = -255; j <= 255; j++)
            {
                for (i = -255; i <= 255; i++)
                {
                    // atan2(SinTerm=i, CosTerm=j)
                    atanLut4[(j + 255) * 511 + i + 255] = (float)Math.Atan2(i, j);
                }
            }

            //5bucket
            int dxi, dyi, tablesizeX, tablesizeY;
            tablesizeX = 825 * 2 + 1;
            tablesizeY = 785 * 2 + 1;
            double dx, dy;
            for (dyi = 0; dyi < tablesizeY; dyi++)
            {
                for (dxi = 0; dxi < tablesizeX; dxi++)
                {
                    dx = (dxi - 825) / 2.0f;
                    dy = (dyi - 785) / 2.0f;
                    atanLut5[dyi * tablesizeX + dxi] = (float)Math.Atan2(dx, dy);
                }
            }
        }

        public PmpAlgorithm()
        {
        }

        public void initialize(int width, int height, PmpData[] pmpData, LensCalibration lensCalibration)
        {
            MakeAtanLut();

            calcBuffer = new CalcBuffer[pmpData.Count()];
            imageSize = new Size(width, height);

            for (int i = 0; i < pmpData.Count(); i++)
            {
                if (pmpData[i].enable)
                {
                    calcBuffer[i] = new CalcBuffer();
                    calcBuffer[i].Initialize(width, height);
                }
            }
            this.lensCalibration = lensCalibration;
        }

        public int Calculate(PmpData[] pmpData, RoiInfo roiInfo)
        {
            LogHelper.Debug(LoggerType.Grab, "Start Calculate");

            byte mode = 0;

            for (int i = pmpData.Length - 1; i >= 0; i--)
            {
                if (pmpData[i].enable)
                {
                    mode += (byte)(0x01 << i);

                    LogHelper.Debug(LoggerType.Grab, string.Format("Calculate Height {0}", i));

                    CalculateHeight(pmpData[i], roiInfo, calcBuffer[i]);
#if DEBUG
                    string fileName = string.Format("e:\\ExymaLog\\PMP\\Phase{0}.bmp", i);
                    ImageHelper.SaveImage(calcBuffer[i].Phase, calcBuffer[i].Width, calcBuffer[i].Height, fileName);
#endif
                }
            }

            switch (mode)
            {
                case 3: //double PMP
                    Beat(2, pmpData, roiInfo, calcBuffer);
                    roiInfo.ZHeight.CopyTo(calcBuffer[0].Phase, 0);
                    break;
                case 7: //Triple PMP
                    Beat(3, pmpData, roiInfo, calcBuffer);
                    calcBuffer[0].Phase.CopyTo(roiInfo.ZHeight, 0);
                    break;
                default:
                    MessageBox.Show(StringManager.GetString("CPMPAlgorithm::Calculate(...) default error"));
                    break;
            }

            ZCalbration(pmpData, roiInfo);

            Calibration2D(roiInfo);

            NoiseCheck(roiInfo, 30);

            RemoveNoise(roiInfo, 10);

            LogHelper.Debug(LoggerType.Grab, "End Calculate");

            return 0;
        }

        private long RemoveNoise(RoiInfo roiInfo, float noiseAmplitude)
        {
            LogHelper.Debug(LoggerType.Grab, "RemoveNoise");

            int width = roiInfo.Region.Width;
            int height = roiInfo.Region.Height;
            byte[] amplitude = roiInfo.AmplitudeBuffer;
            float[] pZ = roiInfo.ZHeight;

            float[] t = new float[9];
            for (int j = 0; j < width * height; j++)
            {
                if (amplitude[j] < noiseAmplitude)
                {
                    pZ[j] = 0;
                    amplitude[j] = 0;
                }
            }

            return 0;
        }

        private long NoiseCheck(RoiInfo roiInfo, float NoiseHeight)
        {
            LogHelper.Debug(LoggerType.Grab, "NoiseCheck");

            int width = roiInfo.Region.Width;
            int height = roiInfo.Region.Height;
            byte[] pNoise = roiInfo.AmplitudeBuffer;
            float[] pZ = roiInfo.ZHeight;

            Parallel.For(0, height, j =>
            {
                float[] t = new float[9];
                int i, k;
                int j0, j1, j2;
                float fmin, fmax, fdiff;

                for (i = 0; i < width; i++)
                {
                    j0 = width * (j - 1);
                    j1 = width * (j + 0);
                    j2 = width * (j + 1);

                    if (j == 0 || i == 0 || j == height - 1 || i == width - 1)
                    {
                        pNoise[j1 + i] = 0;
                        continue;
                    }

                    if (pNoise[j1 + i] < 3)
                    {
                        pNoise[j1 + i] = 0;
                        continue;
                    }

                    t[0] = pZ[j0 + i - 1]; t[1] = pZ[j0 + i]; t[2] = pZ[j0 + i + 1];
                    t[3] = pZ[j1 + i - 1]; t[4] = pZ[j1 + i]; t[5] = pZ[j1 + i + 1];
                    t[6] = pZ[j2 + i - 1]; t[7] = pZ[j2 + i]; t[8] = pZ[j2 + i + 1];

                    fmin = 999999; fmax = -99999999;

                    for (k = 0; k < 9; k++)
                    {
                        if (fmin > t[k])
                        {
                            fmin = t[k];
                        }

                        if (fmax < t[k])
                        {
                            fmax = t[k];
                        }
                    }
                    fdiff = Math.Abs(fmax - fmin);

                    if (fdiff > NoiseHeight)
                    {
                        pNoise[j1 + i] = 0;
                    }
                }
            });

            DilationMask(width, height, pNoise, 0);

            return 0;
        }

        private int Median33_Roi(RoiInfo roiInfo)
        {
            int W = roiInfo.Region.Width;
            int H = roiInfo.Region.Height;
            int i, j, m, n;

            float[] pSrc = roiInfo.ZHeight;
            float[] pData = new float[W * H];
            byte[] pNoise = roiInfo.AmplitudeBuffer;
            float[] t = new float[9];
            float ftemp;
            pSrc.CopyTo(pData, 0);

            int j0, j1, j2;

            for (j = 1; j < H - 1; j++)
            {
                for (i = 1; i < W - 1; i++)
                {
                    j0 = W * (j - 1);
                    j1 = W * (j + 0);
                    j2 = W * (j + 1);

                    if (pNoise[j1 + i] == 0)
                    {
                        continue;
                    }

                    t[0] = pData[j0 + i - 1]; t[1] = pData[j0 + i]; t[2] = pData[j0 + i + 1];
                    t[3] = pData[j1 + i - 1]; t[4] = pData[j1 + i]; t[5] = pData[j1 + i + 1];
                    t[6] = pData[j2 + i - 1]; t[7] = pData[j2 + i]; t[8] = pData[j2 + i + 1];

                    if (pNoise[j0 + i - 1] == 0)
                    {
                        t[0] = t[4];
                    }

                    if (pNoise[j0 + i + 0] == 0)
                    {
                        t[1] = t[4];
                    }

                    if (pNoise[j0 + i + 1] == 0)
                    {
                        t[2] = t[4];
                    }

                    if (pNoise[j1 + i - 1] == 0)
                    {
                        t[3] = t[4];
                    }

                    if (pNoise[j1 + i + 1] == 0)
                    {
                        t[5] = t[4];
                    }

                    if (pNoise[j2 + i - 1] == 0)
                    {
                        t[6] = t[4];
                    }

                    if (pNoise[j2 + i + 0] == 0)
                    {
                        t[7] = t[4];
                    }

                    if (pNoise[j2 + i + 1] == 0)
                    {
                        t[8] = t[4];
                    }

                    for (n = 8; n >= 4; n--)
                    {
                        for (m = 0; m < n; m++)
                        {
                            if (t[m] > t[m + 1])
                            {
                                ftemp = t[m];
                                t[m] = t[m + 1];
                                t[m + 1] = ftemp;
                            }
                        }
                    }
                    if (pNoise[j1 + i] != 0)
                    {
                        pSrc[j1 + i] = t[4];
                    }
                }
            }

            return 0;
        }

        private int Mean33_Roi(RoiInfo roiInfo)
        {
            int W = roiInfo.Region.Width;
            int H = roiInfo.Region.Height;
            int i, j;
            float fsum = 0;
            int index0, index1, index2;
            byte[] pNoise = roiInfo.AmplitudeBuffer;
            float[] pSrc = roiInfo.ZHeight;
            float[] pData = new float[W * H];
            int cnt = 0;
            pSrc.CopyTo(pData, 0);
            for (j = 1; j < H - 1; j++)
            {
                for (i = 1; i < W - 1; i++)
                {
                    cnt = 0;
                    index0 = (j - 1) * W + i;

                    if (pNoise[index0] == 0)
                    {
                        continue;
                    }

                    if (pNoise[index0 - 1] > 0) { fsum = pData[index0 - 1]; cnt++; }
                    if (pNoise[index0] > 0) { fsum += pData[index0]; cnt++; }
                    if (pNoise[index0 + 1] > 0) { fsum += pData[index0 + 1]; cnt++; }

                    index1 = j * W + i;
                    if (pNoise[index1 - 1] > 0) { fsum += pData[index1 - 1]; cnt++; }
                    if (pNoise[index1] > 0) { fsum += pData[index1]; cnt++; }
                    if (pNoise[index1 + 1] > 0) { fsum += pData[index1 + 1]; cnt++; }


                    index2 = (j + 1) * W + i;
                    if (pNoise[index2 - 1] > 0) { fsum += pData[index2 - 1]; cnt++; }
                    if (pNoise[index2] > 0) { fsum += pData[index2]; cnt++; }
                    if (pNoise[index2 + 1] > 0) { fsum += pData[index2 + 1]; cnt++; }

                    pSrc[index1] = fsum / cnt;
                }
            }
            return 0;
        }

        private void DilationMask(int nWidth, int nHeight, byte[] pMask, byte type)
        {
            LogHelper.Debug(LoggerType.Grab, "  DilationMask");

            const int edgeWidth = 1;
            /////////////masking Edge//////////////////////////////////////////////////////
            int i, j;
            int j0, j1, j2;

            byte[] pSrc = new byte[nWidth * nHeight];
            Array.Copy(pMask, pSrc, nWidth * nHeight);

            for (j = 0 + edgeWidth; j < nHeight - edgeWidth; j++)
            {
                j0 = (j - 1) * nWidth;
                j1 = (j) * nWidth;
                j2 = (j + 1) * nWidth;
                for (i = 0 + edgeWidth; i < nWidth - edgeWidth; i++)
                {
                    if (pSrc[j1 + i] != type)
                    {
                        if (pSrc[j0 + i - 1] == type ||
                            pSrc[j0 + i] == type ||
                            pSrc[j0 + i + 1] == type ||

                            pSrc[j1 + i - 1] == type ||
                            //pSrc[j1+i]   == type ||
                            pSrc[j1 + i + 1] == type ||

                            pSrc[j2 + i - 1] == type ||
                            pSrc[j2 + i] == type ||
                            pSrc[j2 + i + 1] == type
                            )
                        {
                            pMask[j1 + i] = type;
                        }
                    }
                }
            }
        }

        private int CalculateHeight(PmpData pmpData, RoiInfo roiInfo, CalcBuffer calcBuffer)
        {
            int fovWidth = imageSize.Width;
            int fovHeight = imageSize.Height;
            int pitch = imageSize.Width;
            uint bucket = pmpData.bucket;

            int xOffset = roiInfo.Region.X;
            int roiWidth = roiInfo.Region.Width;
            int xEnd = roiInfo.Region.Right;

            int yOffset = roiInfo.Region.Y;
            int roiHeight = roiInfo.Region.Height;
            int yEnd = roiInfo.Region.Bottom;

            List<Image2D> imageBufferList = pmpData.imageBufferList;

            calcBuffer.Initialize(roiWidth, roiHeight);
            Image3D referenceData = pmpData.referenceData;

            float[] phaseBuffer = calcBuffer.Phase;
            float[] metricBuffer = calcBuffer.Metric;

            byte[] roiAmplitudeBuffer = roiInfo.AmplitudeBuffer;
            byte[] roiModulationBuffer = roiInfo.ModulationBuffer;


            //#if DEBUG == true
            //            StringBuilder stringBuilder = new StringBuilder();
            //            int offset = fovHeight / 2 * fovWidth;
            //            for (int i = fovWidth / 2 - 100; i < fovWidth / 2 + 100; i++)
            //            {
            //                for (int n = 0; n < bucket; n++)
            //                {
            //                    stringBuilder.AppendFormat(imageBufferList[n].Data[offset + i].ToString());
            //                }
            //                stringBuilder.AppendLine();
            //            }

            //            File.WriteAllText(String.Format("e:\\ExymaLog\\Intensity(%dBK).csv", bucket), stringBuilder.ToString());
            //#endif

            double[] sinValue = new double[bucket];
            double[] cosValue = new double[bucket];

            for (int i = 0; i < bucket; i++)
            {
                sinValue[i] = Math.Sin((Math.PI * 2.0 * i) / bucket);
                cosValue[i] = Math.Cos((Math.PI * 2.0 * i) / bucket);
            }

            Parallel.For(yOffset, yEnd, lc =>
            //            for(H = 0, j = yOffset; j < yEnd; j++, H++)
            {
                int H = lc - yOffset;

                for (int i = xOffset; i < xEnd; i++)
                {
                    int W = i - xOffset;
                    int BufIndex = lc * fovWidth + i;
                    int ImgIndex = lc * pitch + i;
                    int RoiIndex = H * roiWidth + W;

                    double nSum = 0;
                    double dSinTerm = 0;
                    double dCosTerm = 0;

                    for (int k = 0; k < bucket; k++)
                    {
                        byte intensity = imageBufferList[k].Data[ImgIndex];
                        dSinTerm += intensity * sinValue[k];
                        dCosTerm += intensity * cosValue[k];
                        nSum += intensity;
                    }

                    //Amplitude
                    double fAmp = (float)Math.Sqrt((dSinTerm * dSinTerm + dCosTerm * dCosTerm));
                    roiAmplitudeBuffer[RoiIndex] = (byte)((fAmp < 255) ? fAmp : 255);
                    if (fAmp > 2)
                    {
                        double objectPhase = (float)Math.Atan2(dSinTerm, dCosTerm);
                        double fMoire = objectPhase - referenceData.Data[BufIndex];
                        if (fMoire > Math.PI)
                        {
                            fMoire -= (float)Math.PI * 2;
                        }
                        else if (fMoire < -Math.PI)
                        {
                            fMoire += (float)Math.PI * 2;
                        }

                        phaseBuffer[RoiIndex] = (float)fMoire;

                    }
                    else
                    {
                        phaseBuffer[RoiIndex] = 0;
                    }


                    //pModule
                    if (nSum < bucket || nSum > 254 * bucket)
                    {
                        roiModulationBuffer[RoiIndex] = roiAmplitudeBuffer[RoiIndex] = 0;
                        phaseBuffer[RoiIndex] = 0.0f;
                        continue;
                    }
                    else
                    {
                        roiModulationBuffer[RoiIndex] = (byte)(nSum / bucket);
                    }
                }

                H++;
            });
            return 0;
        }

        private int Beat(int beatcount, PmpData[] pmpData, RoiInfo roiInfo, CalcBuffer[] calcBuffer)
        {
            LogHelper.Debug(LoggerType.Grab, "Beat");

            int XOffset = roiInfo.Region.X;
            int RoiWidth = roiInfo.Region.Width;
            int XEnd = XOffset + RoiWidth;

            int YOffset = roiInfo.Region.Y;
            int RoiHeight = roiInfo.Region.Height;
            int YEnd = YOffset + RoiHeight;

            int size = RoiWidth * RoiHeight;

            float[] pfMoi0 = calcBuffer[0].Phase;
            float[] pf3DPhase = calcBuffer[0].Phase;
            float[] pfMoi1 = calcBuffer[1].Phase;
            float[] pfMoi2 = calcBuffer[2].Phase;

            //float BeatPhase, BeatPhase01, BeatPhase12;
            //float beatlength, beatlength01, beatlength12;
            //float fMerr = 0;
            //int m;
            double lamda0 = pmpData[0].T2;
            double lamda1 = pmpData[1].T2;
            double lamda2 = pmpData[2].T2;

            double pi2 = Math.PI * 2;

            //string strdata = "";

#if DEBUG == true
            //int index = 0;

            //int offset = RoiHeight / 2 * RoiWidth;
            //for (int i = 0; i < RoiWidth; i++)
            //{
            //    index = offset + i;
            //    strdata += String.Format("{0},{1},{2}\n", pfMoi0[index], pfMoi1[index], pfMoi2[index]);
            //}
            //File.WriteAllText("E:\\ExymaLog\\3Phase.csv", strdata);
#endif

            if (beatcount == 2)
            {
                float beatlength = (float)((lamda1 * lamda0) / Math.Abs(lamda1 - lamda0));
                for (int i = 0; i < size; i++)
                {
                    float BeatPhase = pfMoi0[i] - pfMoi1[i];
                    if (BeatPhase > Math.PI)
                    {
                        BeatPhase = (float)(BeatPhase - Math.PI * 2);
                    }
                    else if (BeatPhase < -Math.PI)
                    {
                        BeatPhase = (float)(BeatPhase + Math.PI * 2);
                    }

                    float fMerr = (float)((1.0 / Math.PI * 2) * ((beatlength / lamda0) * BeatPhase - pfMoi0[i]));
                    int m = (int)(fMerr);
                    if (fMerr - m > 0.5)
                    {
                        m = m + 1;
                    }
                    else if (fMerr - m < -0.5)
                    {
                        m = m - 1;
                    }

                    pfMoi0[i] = (float)(pfMoi0[i] + pi2 * m);
                }
            }
            else if (beatcount == 3)
            {
                float[] pBuffer0 = new float[size];
                float[] pBuffer1 = new float[size];
                float[] pBuffer2 = new float[size];
                int[] pOrderM = new int[size];
                int[] pOrderK = new int[size];

                float beatlength01 = (float)(((lamda1 * lamda0)) / Math.Abs(lamda1 - lamda0));

                float beatlength12 = (float)(((lamda2 * lamda1)) / Math.Abs(lamda2 - lamda1));

                float beatlength = ((beatlength01 * beatlength12)) / Math.Abs(beatlength01 - beatlength12);

                Parallel.For(0, size, i =>
                // Parallel.for (int i = 0; i < size; i++)
                {
                    //0,1 wave
                    float BeatPhase01 = pfMoi0[i] - pfMoi1[i];
                    if (BeatPhase01 > Math.PI)
                    {
                        BeatPhase01 = (float)(BeatPhase01 - Math.PI * 2);
                    }
                    else if (BeatPhase01 < -Math.PI)
                    {
                        BeatPhase01 = (float)(BeatPhase01 + Math.PI * 2);
                    }

                    float fMerr = (float)(((beatlength01 / lamda0) * BeatPhase01 - pfMoi0[i]) / pi2);
                    int m = (int)(fMerr);
                    if (fMerr - m > 0.5)
                    {
                        m = m + 1;
                    }
                    else if (fMerr - m < -0.5)
                    {
                        m = m - 1;
                    }
                    //pf3DPhase[i] =(pfMoi0[i] + SVM_2PI * m);continue;//

                    //1,2 wave
                    float BeatPhase12 = pfMoi1[i] - pfMoi2[i];
                    if (BeatPhase12 > Math.PI)
                    {
                        BeatPhase12 = (float)(BeatPhase12 - pi2);
                    }
                    else if (BeatPhase12 < -Math.PI)
                    {
                        BeatPhase12 = (float)(BeatPhase12 + pi2);
                    }
                    //fMerr=((beatlength12/lamda1)*BeatPhase12-pfMoi1[i])/SVM_2PI;
                    //m = (int)(fMerr);
                    //if (fMerr - m > 0.5) m = m+1;
                    //else if (fMerr - m < -0.5) m = m-1;
                    //pf3DPhase[i] =(pfMoi1[i] + SVM_2PI * m);continue;//

                    //01,12 wave
                    float BeatPhase = BeatPhase01 - BeatPhase12;
                    if (BeatPhase > Math.PI)
                    {
                        BeatPhase = (float)(BeatPhase - pi2);
                    }
                    else if (BeatPhase < -Math.PI)
                    {
                        BeatPhase = (float)(BeatPhase + pi2);
                    }

                    fMerr = (float)(((beatlength / beatlength01) * BeatPhase - BeatPhase01) / pi2);
                    int k = (int)(fMerr);

                    if (fMerr - k > 0.5)
                    {
                        k = k + 1;
                    }
                    else if (fMerr - k < -0.5)
                    {
                        k = k - 1;
                    }

                    pf3DPhase[i] = (float)(pfMoi0[i] + pi2 * m + pi2 * (beatlength01 / lamda0) * k);

                    pBuffer0[i] = BeatPhase01;
                    pBuffer1[i] = BeatPhase12;
                    pBuffer2[i] = BeatPhase;
                    pOrderM[i] = m;
                    pOrderK[i] = k;
                });

#if DEBUG == true
                //int j = RoiHeight / 2;
                //{
                //    for (int i = 0; i < RoiWidth; i++)
                //    {
                //        index = RoiWidth * j + i;
                //        strdata += String.Format("{0}, {1}\n", pOrderM[index], pOrderK[index]);
                //    }
                //    File.WriteAllText("E:\\ExymaLog\\orderMK.csv", strdata);
                //}
#endif
            }
            return 0;
        }

        private void Calibration2D(RoiInfo roiInfo)
        {
            LogHelper.Debug(LoggerType.Grab, "Calibration2D");

            //            for (int j = roiInfo.Region.Y; j < roiInfo.Region.Y + roiInfo.Region.Height; j++)
            Parallel.For(roiInfo.Region.Y, roiInfo.Region.Y + roiInfo.Region.Height, j =>
           {
               for (int i = roiInfo.Region.X; i < roiInfo.Region.X + roiInfo.Region.Width; i++)
               {
                   int index = j * imageSize.Width + i;
                   int roiIndex = (j - roiInfo.Region.Y) * roiInfo.Region.Width + (i - roiInfo.Region.X);
                   double metricZ = roiInfo.ZHeight[roiIndex];
                   lensCalibration.CalculateMetricXY(i, j, metricZ, out double metricX, out double metricY);

                   roiInfo.XPos[roiIndex] = (float)metricX;
                   roiInfo.YPos[roiIndex] = (float)metricY;
               }
           });
        }

        private int ZCalbration(PmpData[] pmpData, RoiInfo roiInfo)
        {
            LogHelper.Debug(LoggerType.Grab, "ZCalbration");

            int CoeffCount = 4;
            int size = roiInfo.Region.Width * roiInfo.Region.Height;
            float[] zCalData = pmpData[0].zCalibrationData.Data;

            int endX = roiInfo.Region.X + roiInfo.Region.Width;
            int endY = roiInfo.Region.Y + roiInfo.Region.Height;

            int CenterIndex = imageSize.Height / 2 * imageSize.Width + imageSize.Width / 2;

            // for (H = 0, j = roiInfo.Region.X; j < endY; j++, H++)
            Parallel.For(roiInfo.Region.X, endY, j =>
           {
               int H = j - roiInfo.Region.X;

               for (int i = roiInfo.Region.Y; i < endX; i++)
               {
                   int W = i - roiInfo.Region.Y;
                   int fovIndex = j * imageSize.Width + i;
                   int roiIndex = H * roiInfo.Region.Width + W;

                   if (roiInfo.AmplitudeBuffer[roiIndex] < 10)
                   {
                       roiInfo.ZHeight[roiIndex] = 0;
                   }
                   else
                   {
                       float fdata = roiInfo.ZHeight[roiIndex];

                       roiInfo.ZHeight[roiIndex] =
                           zCalData[fovIndex * CoeffCount + 3] * fdata * fdata * fdata
                           + zCalData[fovIndex * CoeffCount + 2] * fdata * fdata
                           + zCalData[fovIndex * CoeffCount + 1] * fdata
                           + zCalData[fovIndex * CoeffCount + 0];
                   }
               }
           });

            return 0;
        }

    }
}
