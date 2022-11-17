using System;
using System.Diagnostics;

namespace WPF.UniScanIM.Inspect
{
    public class InspectVisionParam
    {
        public System.Drawing.Size imageWholeSize = new System.Drawing.Size(0, 0);
        public float pps = 357000;//595000;// 3750000;//357000; //70000;
        public float pulsePerR = 10000; // pulse/Revolution
        public float mmPerR = 10 * 1000; //250 * 1000 //um

        //public float resolution = 10.0f; //um
        public float inspectionFrameRate = 5.0f;
        // Light Calibration
        public float lightCalibFrameRate = 10.0f; // Frame/Sec

        public int lightCalibStep = 10;
        public int lightCalibStart = 0;
        public int lightCalibEnd = 255 + 0;
        public int lightstabilizingTimeMSec = 0; //msec

        public float uniformityCalibFrameNum = 1.0f; // Frames

        public double GetRollSpeed(double lineSpeed)
        {
            //double rollSpeed = ((pps / pulsePerR) * mmPerR);
            double rollSpeed = (lineSpeed * 1000.0 * 1000.0) / 60.0;
            return rollSpeed; // um/sec
        }

        public int GetGrabHz(double lineSpeed, float resolution)
        {
            int Hz = (int)Math.Round(lineSpeed * 1000000.0 / 60.0 / resolution); // lineSpeed :m/min, 
            return Hz;
        }

        public float GetEstimatedCalibrationTime()
        {
            float estimatedCalibrationTime = ((((lightCalibEnd - lightCalibStart) / lightCalibStep) / lightCalibFrameRate) +
                (((lightCalibEnd - lightCalibStart) / lightCalibStep) * lightstabilizingTimeMSec / 1000.0f) +
                (uniformityCalibFrameNum)) * 4.0f;

            return estimatedCalibrationTime;
        }

        public int GetInspectImageHeight(double grabHz)
        {
            int Height = (int)Math.Round(grabHz / inspectionFrameRate);
            return Height;
        }

        public int GetInspectImageHeightBymm(double mm, double resolution)
        {
            int Height = (int)Math.Round(mm * 1000.0 / resolution);
            return Height;
        }

        public int GetLightCalibImageHeight(double grabHz)
        {
            int Height = (int)Math.Round(grabHz / lightCalibFrameRate);
            return Height;
        }

        public int GetLightCalibImageHeightBymm(double mm, double resolution)
        {
            int Height = (int)Math.Round(mm * 1000.0 / resolution);
            return Height;
        }

        public int GetUniformityCalibImageHeight(double grabHz)
        {
            int Height = (int)Math.Round(grabHz * uniformityCalibFrameNum);
            return Height;
        }

        public int GetUniformityCalibImageHeightBymm(double mm, double resolution)
        {
            int Height = (int)Math.Round(mm * 1000.0 / resolution);
            return Height;
        }
    }
}
