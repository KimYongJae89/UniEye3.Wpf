using System;

namespace UniScanC.Data
{
    public class GlossData
    {
        public DateTime Time { get; set; }

        public float Position { get; set; }

        public float Data { get; set; }

        public GlossData(DateTime time, float position, float data)
        {
            Time = time;
            Position = position;
            Data = data;
        }
    }
}