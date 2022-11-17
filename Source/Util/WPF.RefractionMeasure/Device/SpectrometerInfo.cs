using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.RefractionMeasure.Device
{
    public class SpectrometerInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public double[] WaveLengths { get; set; }
        public int NumberOfPixels { get; set; }
        public int IntegrationTime { get; set; }
        public int ScansToAverage { get; set; }
        public int BoxcarWidth { get; set; }
    }
}
