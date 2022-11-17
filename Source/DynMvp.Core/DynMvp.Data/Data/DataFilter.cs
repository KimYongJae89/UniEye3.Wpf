using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public enum DataFiltrType
    {
        Average, planeFitAverage
    }

    public class DataFilter
    {
        public double Filtering(DataFiltrType type, double[] values)
        {
            switch (type)
            {
                default:
                case DataFiltrType.Average:
                    {
                        double average = 0;
                        if (values.Count() > 0)
                        {
                            average = values.Average();
                        }

                        return average;
                    }
                case DataFiltrType.planeFitAverage:
                    return PlaneFitAverageFilter(values);
            }
        }

        public double PlaneFitAverageFilter(double[] values)
        {
            double average = 0;
            if (values.Count() > 0)
            {
                average = values.Average();
            }

            return average;
        }
    }
}
