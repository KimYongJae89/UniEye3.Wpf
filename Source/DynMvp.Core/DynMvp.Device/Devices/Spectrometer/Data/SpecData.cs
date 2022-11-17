using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Spectrometer.Data
{
    public class SpecRawData
    {
        public Dictionary<string, double[]> Wavelength { get; set; } = new Dictionary<string, double[]>();
        public Dictionary<string, double[]> NewSpectrum { get; set; } = new Dictionary<string, double[]>();

        public SpecRawData Clone()
        {
            var specRawData = new SpecRawData();
            specRawData.CopyFrom(this);
            return specRawData;
        }

        public void CopyFrom(SpecRawData specRawData)
        {
            foreach (KeyValuePair<string, double[]> pair in specRawData.Wavelength)
            {
                Wavelength.Add(pair.Key, (double[])pair.Value.Clone());
            }

            foreach (KeyValuePair<string, double[]> pair in specRawData.NewSpectrum)
            {
                NewSpectrum.Add(pair.Key, (double[])pair.Value.Clone());
            }
        }
    }

    public class SpecDividedData
    {
        public double[] DividedWavelength { get; set; }
        public double[] DividedSpectrums { get; set; }
        public double[] DividedWavelengthRefraction { get; set; }
        public double[] DividedSpectrumsRefraction { get; set; }

        public SpecDividedData Clone()
        {
            var specDividedData = new SpecDividedData();
            specDividedData.CopyFrom(this);
            return specDividedData;
        }

        public void CopyFrom(SpecDividedData specDividedData)
        {
            DividedWavelength = (double[])specDividedData.DividedWavelength.Clone();
            DividedSpectrums = (double[])specDividedData.DividedSpectrums.Clone();

            DividedWavelengthRefraction = (double[])specDividedData.DividedWavelengthRefraction?.Clone();
            DividedSpectrumsRefraction = (double[])specDividedData.DividedSpectrumsRefraction?.Clone();
        }
    }

    public class SpecThicknessData
    {
        // [ 3. 각 Layer별 FFT 결과 FFTThickness Data]
        private double[] fftThickness = null;
        public double[] FFTThickness { get => fftThickness; set => fftThickness = value; }

        // [ 3. 각 Layer별 FFT 결과 FFTSpectrum Data]
        private double[] fftSpectrum = null;
        public double[] FFTSpectrum { get => fftSpectrum; set => fftSpectrum = value; }

        private double[] fftPhase = null;
        public double[] FFTPhase { get => fftPhase; set => fftPhase = value; }

        // FFT 계산을 위한 리셈플된 파장
        private double[] resampledWavelength = null;
        public double[] ResampledWavelength { get => resampledWavelength; set => resampledWavelength = value; }

        public SpecThicknessData Clone()
        {
            var specThicknessData = new SpecThicknessData();
            specThicknessData.CopyFrom(this);
            return specThicknessData;
        }

        public void CopyFrom(SpecThicknessData specThicknessData)
        {
            FFTThickness = (double[])specThicknessData.FFTThickness.Clone();
            FFTSpectrum = (double[])specThicknessData.FFTSpectrum.Clone();
            FFTPhase = (double[])specThicknessData.FFTPhase.Clone();
        }

        public int Initialize(LayerParam layerParam)
        {
            int size = (int)Math.Pow(2, layerParam.DataLengthPow);

            if (resampledWavelength == null)
            {
                resampledWavelength = new double[size];
            }
            else
            {
                Array.Resize<double>(ref resampledWavelength, size);
            }

            if (fftSpectrum == null)
            {
                fftSpectrum = new double[size / 2];
            }
            else
            {
                Array.Resize<double>(ref fftSpectrum, size / 2);
            }

            if (fftPhase == null)
            {
                fftPhase = new double[size / 2];
            }
            else
            {
                Array.Resize<double>(ref fftPhase, size / 2);
            }

            if (fftThickness == null)
            {
                fftThickness = new double[size / 2];
            }
            else
            {
                Array.Resize<double>(ref fftThickness, size / 2);
            }

            return 0;
        }

        public int MakeFFTThinkness(Layer layer)
        {
            LayerParam param = layer.Param;
            int size = (int)Math.Pow(2, param.DataLengthPow);
            int power = param.DataLengthPow;
            int validData = param.ValidDataNum;
            int startWave = param.StartWavelength;
            int endWave = param.EndWavelength;
            double refraction = param.Refraction;

            //              i * startWave * endWave
            // (2.0 * (endWave - startWave) * 1.0 * Math.Cos(0)))
            //             2^N * refraction * 1000.0
            //                     validData

            // i  *  validData  *         start * end                *       1      *   1  
            //          2^N        2 * (end - start) * 1.0 * cos(0)     refraction     1000

            for (int i = 0; i < size / 2; ++i)
            {
                FFTThickness[i] = i * (validData / Math.Pow(2, power)) * (startWave * endWave / (2.0 * (endWave - startWave) * 1.0 * Math.Cos(0))) / refraction / 1000.0;
                FFTThickness[i] = param.CalibParam1 * FFTThickness[i] + param.CalibParam0;
            }
            return 0;
        }
    }
}
