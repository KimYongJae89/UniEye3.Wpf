using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    internal class Histogram
    {
        private int numStep;
        private float minValue;
        private float maxValue;
        public int[] HistogramData { get; set; }

        public void Create(int numStep, float minValue, float maxValue)
        {
            this.numStep = numStep;
            this.minValue = minValue;
            this.maxValue = maxValue;

            HistogramData = new int[numStep];
        }

        public void Create(float[] datum, int numStep)
        {
            this.numStep = numStep;
            HistogramData = new int[numStep];

            minValue = datum.Min();
            maxValue = datum.Max();

            float stepDistance = (maxValue - minValue) / numStep;

            foreach (float data in datum)
            {
                int index = GetIndex(data);
                HistogramData[index]++;
            }
        }

        public void Create(float[] datum, int numStep, float stepDistance)
        {
            this.numStep = numStep;
            HistogramData = new int[numStep];

            float averageValue = datum.Average();

            minValue = averageValue - (numStep / 2) * stepDistance;
            maxValue = averageValue + (numStep / 2) * stepDistance;

            foreach (float data in datum)
            {
                int index = GetIndex(data);
                HistogramData[index]++;
            }
        }

        public void Add(float[] datum, bool ignoreOutboundValue)
        {
            foreach (float data in datum)
            {
                if (ignoreOutboundValue == true)
                {
                    if (data > maxValue || data < minValue)
                    {
                        continue;
                    }
                }

                int index = GetIndex(data);
                HistogramData[index]++;
            }
        }

        private int GetIndex(float value)
        {
            float stepDistance = (maxValue - minValue) / numStep;

            int index = (int)((value - minValue) / stepDistance);
            if (index >= numStep || index < 0)
            {
                if (index >= numStep)
                {
                    index = numStep - 1;
                }

                if (index < 0)
                {
                    index = 0;
                }
            }

            return index;
        }

        public int GetHistogramCount(float value)
        {
            int index = GetIndex(value);
            return HistogramData[index];
        }

        public int GetMaxCount()
        {
            return HistogramData.Max();
        }
    }
}
