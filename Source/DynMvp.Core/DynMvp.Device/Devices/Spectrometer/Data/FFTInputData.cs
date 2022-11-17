using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Spectrometer.Data
{
    // [ 2. 각 Layer별 Divided Data ]
    public class FFTInputData
    {
        public DateTime Time { get; set; }
        public float Position { get; set; }
        public Dictionary<string, double[]> DividedWavelength { get; set; }
        public Dictionary<string, double[]> DividedSpectrums { get; set; }
        public Dictionary<string, double[]> DividedWavelengthRefraction { get; set; }
        public Dictionary<string, double[]> DividedSpectrumsRefraction { get; set; }

        public FFTInputData()
        {
            Time = new DateTime();
            Position = new float();

            DividedWavelength = new Dictionary<string, double[]>();
            DividedSpectrums = new Dictionary<string, double[]>();

            DividedWavelengthRefraction = new Dictionary<string, double[]>();
            DividedSpectrumsRefraction = new Dictionary<string, double[]>();
        }
    }
}
