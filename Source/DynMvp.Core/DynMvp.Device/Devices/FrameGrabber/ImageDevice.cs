using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public delegate void ImageDeviceEventDelegate(ImageDevice imageDevice);

    public enum TriggerMode
    {
        Software, Hardware
    }

    public enum TriggerType
    {
        RisingEdge, FallingEdge
    }

    public abstract class ImageDevice : Device
    {
        public const int CONTINUOUS = -1;
        public CameraEventDelegate ImageGrabbed { get; set; } = null;

        public ImageDeviceEventDelegate exposureDone = null;
        public ImageDeviceEventDelegate ExposureDone
        {
            get => exposureDone;
            set => exposureDone = value;
        }
        public int Index { get; set; }
        public bool Enabled { get; set; } = true;
        public Size ImageSize { get; set; }
        public int ImagePitch { get; set; }

        protected TriggerMode triggerMode;
        public TriggerMode TriggerMode => triggerMode;

        protected int triggerChannel = 0;
        public int TriggerChannel
        {
            get => triggerChannel;
            set => triggerChannel = value;
        }

        protected TriggerType triggerType;
        public TriggerType TriggerType => triggerType;

        protected bool grabFailed = false;
        public bool GrabFailed
        {
            get => grabFailed;
            set => grabFailed = value;
        }

        // Image
        public abstract bool IsCompatibleImage(ImageD image);
        public abstract ImageD CreateCompatibleImage();
        public abstract ImageD GetGrabbedImage();
        public virtual List<ImageD> GetGrabbedImageList()
        {
            return new List<ImageD>() { GetGrabbedImage() };
        }

        public virtual bool IsDepthScanner()
        {
            return false;
        }

        public virtual void Grab2D()
        {

        }

        public virtual void Grab3D()
        {

        }

        public virtual Image3D Calculate(Rectangle rectangle, float pixelRes)
        {
            return null;
        }

        public virtual Image3D Calculate(Rectangle rectangle, TransformDataList transformDataList, float pixelRes)
        {
            return null;
        }

        public virtual bool IsOnLive()
        {
            return false;
        }

        // Camera
        public virtual void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            this.triggerMode = triggerMode;
            this.triggerType = triggerType;
        }

        public abstract void SetTriggerDelay(int triggerDelayUs);
        public abstract bool SetGain(float gain);
        public abstract void SetExposureTime(float exposureTimeUs);

        public virtual void SetExposureTime3d(float exposureTime3dUs)
        {

        }

        public bool WaitGrabDone(int timeoutMs = 0)
        {
            Thread.Sleep(10);

            if (timeoutMs == 0)
            {
                timeoutMs = DeviceConfig.Instance().GrabTimeoutMs;
            }

            //Debug.Write("Start Wait Grab Done\n");

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsGrabDone())
                {
                    //Debug.Write("End Wait Grab Done\n");

                    return true;
                }

                Thread.Sleep(10);
            }

            Debug.Write("Fail to Wait Grab Done\n");

            if (IsGrabDone() == false)
            {
                GrabFailed = true;
            }

            return false;
        }

        // File
        // 가상 카메라용 인터페이스 함수
        public virtual void SetImageIndex(int imageIndex, int lightTypeIndex) { }
        public virtual void SetImagePath(string imagePath, int numLIghtType = 1) { }

        /// <summary>
        /// 동기 Grab 동작을 수행한다.
        /// </summary>
        public abstract void GrabOnce();
        /// <summary>
        /// 지정한 개수만큼의 영상을 얻어 온다.
        /// </summary>
        public abstract void GrabMulti(int grabCount = CONTINUOUS);

        public abstract void Stop();
        public abstract void Reset();
        public abstract bool IsGrabDone();
    }
}
