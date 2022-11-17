using System;

namespace UniScanC.Data
{
    public class ThicknessData
    {
        public DateTime Time { get; set; }

        public float Position { get; set; }

        public float Thickness { get; set; }

        public float Refraction { get; set; }

        public float Angle { get; set; }

        public ThicknessData(DateTime time, float position, float thickness, float refraction = 1, float angle = 1)
        {
            Time = time;
            Position = position;
            Thickness = thickness;
            Refraction = refraction;
            Angle = angle;
        }
    }
}