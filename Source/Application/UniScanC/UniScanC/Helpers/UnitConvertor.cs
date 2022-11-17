using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.Helpers
{
    public static class UnitConvertor
    {
        public static float Px2Um(float px, float resolution)
        {
            return px * resolution;
        }

        public static float Px2Mm(float px, float resolution)
        {
            return Px2Um(px, resolution / 1000);
        }

        public static float Um2Px(float um, float resolution)
        {
            return um / resolution;
        }

        public static float Mm2Px(float um, float resolution)
        {
            return Um2Px(um * 1000, resolution);
        }

        public static double GetRollSpeedUMpS(double lineSpeedMpM)
        {
            //double rollSpeed = ((pps / pulsePerR) * mmPerR);
            double rollSpeed = (lineSpeedMpM * 1000.0 * 1000.0) / 60.0;
            return rollSpeed; // um/sec
        }

        public static int GetGrabHz(double lineSpeed, float resolution)
        {
            int Hz = (int)Math.Round(lineSpeed * 1000000.0 / 60.0 / resolution); // lineSpeed :m/min, 
            return Hz;
        }
    }
}
