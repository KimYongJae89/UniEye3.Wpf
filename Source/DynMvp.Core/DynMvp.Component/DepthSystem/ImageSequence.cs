using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Component.DepthSystem
{
    public delegate void ScanDoneDelegate();
    public delegate void ImageScannedDelegate(ImageD image);

    public abstract class ImageSequence
    {
        public ScanDoneDelegate ScanDone;
        public ImageScannedDelegate ImageScanned;

        protected int imageIndex = 0;
        protected List<ImageD> imageList = new List<ImageD>();
        public List<ImageD> ImageList => imageList;

        public abstract void Initialize(Camera camera);
        public abstract void Scan(int numImage);
        public abstract bool IsScanDone();
        public abstract void Stop();
    }
}
