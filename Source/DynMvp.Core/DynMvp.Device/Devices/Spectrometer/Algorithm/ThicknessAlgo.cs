using DynMvp.Devices.Spectrometer.Data;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.Spectrometer.Algorithm
{
    public class ThicknessAlgo
    {
        #region Data Build
        public int BuildData(ref double[] mergedWavelength, ref double[] mergedSpectrum, Layer layer, double[] resampledWavelength, double[] resampledSpectrum)
        {
            LayerParam param = layer.Param;
            SpecThicknessData spec = layer.SpecThicknessData;
            int k = 0;
            double angle = 0;
            var pointFs = new List<PointF>();

            for (int i = 0; i < mergedWavelength.Length - 2; i++)
            {
                if (mergedWavelength[i] < param.StartWavelength || mergedWavelength[i] > param.EndWavelength)
                {
                    continue;
                }

                angle = (mergedSpectrum[i + 1] - mergedSpectrum[i]) / (mergedWavelength[i + 1] - mergedWavelength[i]);
                while (spec.ResampledWavelength[k] <= mergedWavelength[i + 1])
                {
                    if (spec.ResampledWavelength[k] >= param.StartWavelength && spec.ResampledWavelength[k] <= param.EndWavelength)
                    {
                        // 파장 데이터 넣기
                        resampledWavelength[k] = spec.ResampledWavelength[k];
                        // 스펙트럼 데이터 넣기
                        resampledSpectrum[k] = angle * (spec.ResampledWavelength[k] - mergedWavelength[i]) + mergedSpectrum[i];
                    }
                    k++;
                }
            }

            // 데이터를 0 기준으로 형성되도록 추가 계산
            for (int i = 0; i < resampledSpectrum.Length; ++i)
            {
                if (spec.ResampledWavelength[i] >= param.StartWavelength && spec.ResampledWavelength[i] <= param.EndWavelength)
                {
                    pointFs.Add(new PointF(i, (float)resampledSpectrum[i]));
                }
            }
            FindLinearLeastSquaresFit(pointFs, out double m, out double b);
            for (int i = 0; i < resampledSpectrum.Length; ++i)
            {
                if (spec.ResampledWavelength[i] >= param.StartWavelength && spec.ResampledWavelength[i] <= param.EndWavelength)
                {
                    resampledSpectrum[i] = resampledSpectrum[i] - (m * i + b);
                    //resampledSpectrum[i] = resampledSpectrum[i];
                }
                else
                {
                    resampledSpectrum[i] = 0;
                }
            }

            return 0;
        }
        #endregion

        #region Algorithm
        private unsafe void IPP_FFT(double[] spectrum, Layer layer)
        {
            int ord = layer.Param.DataLengthPow, len = 1 << ord;
            var Src = new Ipp64fc[len];
            var Dst = new Ipp64fc[len];

            IppStatus st;
            try
            {
                // generate input data
                //fixed (Ipp64fc* pbuf = pSrc)
                //{
                //    st = sp.ippsTone_64fc(pbuf, len, 1, 1.0f / len, &pPhase, IppHintAlgorithm.ippAlgHintNone);
                //    if (IppStatus.ippStsNoErr != st)
                //        throw (new IppException("Tone", st));
                //}
                //print(pSrc);

                // FFT transform                
                //IppsFFTSpec_R_32f* spec;
                //st = sp.ippsFFTInitAlloc_R_32f(&spec, ord, 2, IppHintAlgorithm.ippAlgHintFast);
                //if (IppStatus.ippStsNoErr != st)
                //  throw (new IppException("FFTInit_R", st));
                for (int i = 0; i < len; ++i)
                {
                    if (i < spectrum.Length)
                    {
                        Src[i].re = spectrum[i];
                    }
                    else
                    {
                        Src[i].re = 0;
                    }

                    Src[i].im = 0;
                }


                IppsFFTSpec_C_64fc* pSpec = null;
                byte* pMemSpec = null;
                byte* pMemInit = null;
                byte* pMemBuffer = null;

                int sizeSpec = 0;
                int sizeInit = 0;
                int sizeBuffer = 0;
                int flag = (int)IppFlagArgforFFT.IPP_FFT_DIV_INV_BY_N;

                st = sp.ippsFFTGetSize_C_64fc(ord, flag, IppHintAlgorithm.ippAlgHintNone, &sizeSpec, &sizeInit, &sizeBuffer);
                pMemSpec = (byte*)core.ippMalloc(sizeSpec);
                pMemInit = (byte*)core.ippMalloc(sizeInit);
                pMemBuffer = (byte*)core.ippMalloc(sizeBuffer);

                sp.ippsFFTInit_C_64fc(&pSpec, ord, flag, IppHintAlgorithm.ippAlgHintNone, pMemSpec, pMemInit);



                fixed (Ipp64fc* pSrc = Src, pDst = Dst)
                {
                    st = sp.ippsFFTFwd_CToC_64fc(pSrc, pDst, pSpec, pMemBuffer);
                    if (IppStatus.ippStsNoErr != st)
                    {
                        throw (new IppException("FFTFwd_R", st));
                    }
                    //st = sp.ippsMagnitude_64fc(dst, pMag, len/2);
                    //double* pMag = (double*)layer.ThicknessCalResult.FFTSpectrum;
                    //sp.ippsMagnitude_64fc(pDst, , len / 2);
                }

                for (int i = 0; i < len / 2; ++i)
                {
                    layer.SpecThicknessData.FFTSpectrum[i] = Math.Sqrt(Dst[i].re * Dst[i].re + Dst[i].im * Dst[i].im);
                    layer.SpecThicknessData.FFTPhase[i] = Math.Atan2(Dst[i].im, Dst[i].re);
                }

                core.ippFree(pMemInit);
                core.ippFree(pMemBuffer);
                core.ippFree(pMemSpec);
            }
            catch (IppException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private int FFTEmgu(double[] spectrum, Layer layer)
        {
            int i;
            int size = spectrum.Length;
            var data = new Mat(1, spectrum.Length, DepthType.Cv64F, 1);

            for (i = 0; i < spectrum.Length; ++i)
            {
                AccessMat.SetValue(data, 0, i, (spectrum[i]/* * gauss(i, 1.0, ragePaded / 2, ragePaded/20)  */  ));
            }

            // FFT            
            var padded = new Mat();                            //expand input image to optimal size
            int m = CvInvoke.GetOptimalDFTSize(data.Rows);
            int n = (int)Math.Pow(2, layer.Param.DataLengthPow);// CvInvoke.GetOptimalDFTSize(data.Cols); // on the border add zero values
            CvInvoke.CopyMakeBorder(data, padded, 0, m - data.Rows, 0, n - data.Cols, BorderType.Constant, new MCvScalar());

            Mat[] planes = { padded.Clone(), new Mat(padded.Size, DepthType.Cv64F, 1) };
            //for (i = 0; i < n; ++i)
            //{
            //    AccessMat.SetValue(planes[1], 0, i, 0);
            //}
            var vector = new VectorOfMat(planes);
            var complexI = new Mat(padded.Size, DepthType.Cv64F, 1);
            //for (i = 0; i < n; ++i)
            //{
            //    AccessMat.SetValue(complexI, 0, i, 0);
            //}

            CvInvoke.Merge(vector, complexI);         // Add to the expanded another plane with zeros
            CvInvoke.Dft(complexI, complexI, DxtType.Forward, 0);            // this way the result may fit in the source matrix
                                                                             // compute the magnitude and switch to logarithmic scale
                                                                             // => log(1 + sqrt(Re(DFT(I))^2 + Im(DFT(I))^2))           

            CvInvoke.Split(complexI, vector);                   // planes[0] = Re(DFT(I), planes[1] = Im(DFT(I))

            double Re, Im;
            for (i = 0; i < size / 2; ++i)
            {
                Re = AccessMat.GetValue(planes[0], 0, i);
                Im = AccessMat.GetValue(planes[1], 0, i);

                layer.SpecThicknessData.FFTSpectrum[i] = Math.Sqrt(Re * Re + Im * Im);
                layer.SpecThicknessData.FFTPhase[i] = Math.Atan2(Im, Re);
            }

            data.Dispose();
            complexI.Dispose();
            vector.Dispose();
            padded.Dispose();
            data.Dispose();
            return 0;
        }

        private int FFTW(double[] spectrum, bool real, Layer layer)
        {
            double[] newSpectum = new double[(int)Math.Pow(2, layer.Param.DataLengthPow)];
            Array.Copy(spectrum, newSpectum, spectrum.Length);

            if (real)
            {
                newSpectum = ToComplex(newSpectum);
            }

            int n = newSpectum.Length;
            IntPtr ptr = fftw.malloc(n * sizeof(double));

            /* 
            IntPtr ptr;
            if (real)
                ptr = fftw.mallocReal(n * sizeof(double));
            else
                ptr = fftw.malloc(n * sizeof(double));                   
*/
            Marshal.Copy(newSpectum, 0, ptr, n);
            IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Forward, fftw_flags.Estimate);
            //IntPtr plan = fftw.dft_r2c_1d(n / 2 , ptr, ptr, fftw_flags.Estimate);
            //IntPtr plan = fftw.r2r_1d(n, ptr, ptr,fftw_kind.HC2R, fftw_flags.Estimate);

            fftw.execute(plan);

            double[] fft = new double[n];
            //Array.Clear(fft, 0, n);
            Marshal.Copy(ptr, fft, 0, n);
            double amp = 0;

            int length = 0;
            if (real)
            {
                length = newSpectum.Length / 2;
            }
            else
            {
                length = newSpectum.Length;
            }

            for (int i = 0; i < length / 2; ++i)
            {
                if (real)
                {
                    amp = Math.Sqrt(fft[i * 2 + 0] * fft[i * 2 + 0] + fft[i * 2 + 1] * fft[i * 2 + 1]);
                }
                else
                {
                    amp = fft[i];
                }

                layer.SpecThicknessData.FFTSpectrum[i] = amp;
            }

            fftw.destroy_plan(plan);
            fftw.free(ptr);
            fftw.cleanup();

            return 0;
        }

        private double GetRefractionAngle(double baseThickness, double angleThickness)
        {
            return Math.Acos(angleThickness / baseThickness);
        }

        private double GetRefraction(double baseAngleRad, double targetAngleRad)
        {
            return Math.Sin(baseAngleRad) / Math.Sin(targetAngleRad);
        }
        #endregion

        #region Data Making
        public double GetThinkness(double[] resampled_spectrum, Layer layer)
        {
            IPP_FFT(resampled_spectrum, layer);
            return CalCenterOfFFT(layer);
        }

        public double GetRefraction(double baseThickness, double angleThickness, double angle)
        {
            double targetAngle = GetRefractionAngle(baseThickness, angleThickness);
            return GetRefraction(angle * Math.PI / 180.0, targetAngle);
        }

        private double CalCenterOfFFT(Layer layer)
        {
            double[] spectrum = layer.SpecThicknessData.FFTSpectrum;
            double[] thickness = layer.SpecThicknessData.FFTThickness;

            double currentThickness = 0d;
            double maxSpectrum = double.MinValue;
            int simpleMaxIndex = int.MinValue;

            int rangeMinIndex = int.MaxValue;
            int rangeMaxIndex = int.MinValue;

            double sigXY = 0d;
            double sigY = 0d;

            for (int i = 0; i < spectrum.Length; ++i)
            {
                currentThickness = thickness[i];
                if (layer.Param.MinThickness <= currentThickness && currentThickness <= layer.Param.MaxThickness)
                {
                    if (maxSpectrum < spectrum[i])
                    {
                        maxSpectrum = spectrum[i];
                        simpleMaxIndex = i;
                    }
                }
            }

            for (int i = simpleMaxIndex; i < spectrum.Length; i++)
            {
                if (maxSpectrum * 0.2 > spectrum[i])
                {
                    rangeMaxIndex = i;
                    break;
                }
            }

            for (int i = simpleMaxIndex; i > 0; i--)
            {
                if (maxSpectrum * 0.2 > spectrum[i])
                {
                    rangeMinIndex = i;
                    break;
                }
            }

            for (int i = rangeMinIndex; i < rangeMaxIndex; i++)
            {
                sigXY += thickness[i] * spectrum[i];
                sigY += spectrum[i];
            }

            if (simpleMaxIndex >= 0 && simpleMaxIndex < thickness.Count())
            {
                layer.Thickness = Convert.ToSingle(sigXY / sigY);
            }

            return layer.Thickness;
        }
        #endregion

        #region ETC
        private double[] ToComplex(double[] real)
        {
            int n = real.Length;
            double[] comp = new double[n * 2];
            //Array.Clear(comp, 0, n * 2);
            for (int i = 0; i < n; ++i)
            {
                comp[2 * i] = real[i];
            }
            return comp;
        }

        public static double FindLinearLeastSquaresFit(List<PointF> points, out double m, out double b)
        {
            // Perform the calculation.
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            double S1 = points.Count;
            double Sx = 0;
            double Sy = 0;
            double Sxx = 0;
            double Sxy = 0;
            foreach (PointF pt in points)
            {
                Sx += pt.X;
                Sy += pt.Y;
                Sxx += pt.X * pt.X;
                Sxy += pt.X * pt.Y;
            }

            // Solve for m and b.
            m = (Sxy * S1 - Sx * Sy) / (Sxx * S1 - Sx * Sx);
            b = (Sxy * Sx - Sy * Sxx) / (Sx * Sx - S1 * Sxx);

            return Math.Sqrt(ErrorSquared(points, m, b));
        }

        public static double ErrorSquared(List<PointF> points, double m, double b)
        {
            double total = 0;
            foreach (PointF pt in points)
            {
                double dy = pt.Y - (m * pt.X + b);
                total += dy * dy;
            }
            return total;
        }
        #endregion
    }

    internal static class AccessMat
    {
        public static dynamic GetValue(Mat mat, int row, int col)
        {
            dynamic value = CreateElement(mat.Depth);
            Marshal.Copy(mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, value, 0, 1);
            return value[0];
        }
        public static void SetValue(this Mat mat, int row, int col, dynamic value)
        {
            dynamic target = CreateElement(mat.Depth, value);
            Marshal.Copy(target, 0, mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, 1);
        }
        private static dynamic CreateElement(DepthType depthType, dynamic value)
        {
            dynamic element = CreateElement(depthType);
            element[0] = value;
            return element;
        }
        private static dynamic CreateElement(DepthType depthType)
        {
            switch (depthType)
            {
                case DepthType.Cv8S:
                    return new sbyte[1];
                case DepthType.Cv8U:
                    return new byte[1];
                case DepthType.Cv16S:
                    return new short[1];
                case DepthType.Cv16U:
                    return new ushort[1];
                case DepthType.Cv32S:
                    return new int[1];
                case DepthType.Cv32F:
                    return new float[1];
                case DepthType.Cv64F:
                    return new double[1];
                default:
                    throw new NotImplementedException();
            }
        }
    }

    internal class IppException : Exception
    {
        private string funcName;
        private IppStatus status;
        public IppException(string fname, IppStatus st)
        {
            funcName = fname; status = st;
        }
        public override string Message => funcName + ": " + core.ippGetStatusString(status);
    }
}
