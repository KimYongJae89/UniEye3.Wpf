using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem
{
    public abstract class ExymaScanner : DepthScanner
    {
        protected bool motorOn;
        public bool MotorOn
        {
            get => motorOn;
            set => motorOn = value;
        }

        public ScanDoneDelegate ScanDone;
        public ImageScannedDelegate ImageScanned;

        public abstract int GetNumScanImage(int scannerIndex);
        public abstract void Grab3D(int scannerIndex);
        public abstract bool IsCompatibleImage(int scannerIdx, ImageD image);
        public abstract ImageD CreateCompatibleImage(int scannerIdx);
        public abstract void SetExposureTime(int scannerIdx, float v);
        public abstract void GrabOnce(int v);
        public abstract void GrabMulti(int scannerIdx, int v);
        public abstract bool Calibrate2D(Image2D image, float value, TransformData transformData);
        public abstract bool Calibrate3D(int deviceIndex, Image2D image, float value, TransformData transformData);
        public abstract Image3D ScanMeasure(int scannerIndex, float value);
        public abstract void SetRamData(EEPROM mAON, uint v);
    }
}
