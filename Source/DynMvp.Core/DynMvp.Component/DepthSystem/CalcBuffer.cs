using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem
{
    public class CalcBuffer
    {
        public float[] Phase { get; private set; }
        public float[] Metric { get; private set; }
        public byte[] Modulation { get; private set; }
        public byte[] Amplitude { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;

            Phase = new float[width * height];
            Metric = new float[width * height];
            Modulation = new byte[width * height];
            Amplitude = new byte[width * height];
        }
    }
}
