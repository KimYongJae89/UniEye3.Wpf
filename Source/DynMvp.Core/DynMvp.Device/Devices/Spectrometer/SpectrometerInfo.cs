namespace DynMvp.Devices.Spectrometer
{
    public class SpectrometerInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string SerialNumbers { get; set; }
        public double[] WaveLengths { get; set; }
        public int NumberOfPixels { get; set; }
    }
}
