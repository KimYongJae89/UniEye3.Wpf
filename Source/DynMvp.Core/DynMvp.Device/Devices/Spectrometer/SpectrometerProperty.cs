using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Spectrometer
{
    public class SpectrometerProperty
    {
        public int SpecNum { get; set; }

        private int layerNum;
        public int LayerNum => LayerNameList.Count;
        public List<string> LayerNameList { get; set; } = new List<string>();
        public Dictionary<string, int> IntegrationTime { get; set; }
        public Dictionary<string, int> Average { get; set; }
        public Dictionary<string, int> Boxcar { get; set; }
        public Dictionary<string, int> LampPitch { get; set; }

        public SpectrometerProperty()
        {
            SpecNum = 1;
            layerNum = 1;
            IntegrationTime = new Dictionary<string, int>();
            Average = new Dictionary<string, int>();
            Boxcar = new Dictionary<string, int>();
            LampPitch = new Dictionary<string, int>();
        }

        public SpectrometerProperty Clone()
        {
            var daqChannelProperty = new SpectrometerProperty();
            daqChannelProperty.CopyFrom(this);

            return daqChannelProperty;
        }

        public void CopyFrom(SpectrometerProperty spectrometerProperty)
        {
            SpecNum = spectrometerProperty.SpecNum;
            layerNum = spectrometerProperty.layerNum;
            IntegrationTime = spectrometerProperty.IntegrationTime;
            Average = spectrometerProperty.Average;
            Boxcar = spectrometerProperty.Boxcar;
            LampPitch = spectrometerProperty.LampPitch;
        }
    }
}
