using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public enum StdDevMode
    {
        Population, // 모표준편차
        Sample // 표본표준편차
    }

    public static class DataProcessing
    {
        public static float[] Differential(float[] datum)
        {
            int size = datum.Count() - 1;
            float[] derivative = new float[datum.Count() - 1];

            for (int i = 0; i < size; i++)
            {
                derivative[i] = datum[i + 1] - datum[i];
            }

            return derivative;
        }

        public static float StdDev(float[] datum, StdDevMode stdDevMode = StdDevMode.Population)
        {
            double[] newDatum = new double[datum.Length];
            for (int i = 0; i < datum.Length; i++)
            {
                newDatum[i] = Convert.ToDouble(datum[i]);
            }

            double average = newDatum.Average();
            double sum = newDatum.Sum(d => Math.Pow(d - average, 2));
            switch (stdDevMode)
            {
                case StdDevMode.Sample: return (float)Math.Sqrt((sum) / (newDatum.Length - 1));
                case StdDevMode.Population:
                default: return (float)Math.Sqrt((sum) / newDatum.Length);
            }

        }

        public static double StdDev(double[] datum, StdDevMode stdDevMode = StdDevMode.Population)
        {
            double average = datum.Average();
            double sum = datum.Sum(d => Math.Pow(d - average, 2));
            switch (stdDevMode)
            {
                case StdDevMode.Population: return Math.Sqrt((sum) / datum.Length);
                case StdDevMode.Sample:
                default: return Math.Sqrt((sum) / (datum.Length - 1));
            }
        }
    }
}