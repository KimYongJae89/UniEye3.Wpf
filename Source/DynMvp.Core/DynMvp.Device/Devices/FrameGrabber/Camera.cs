using DynMvp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public enum BayerType
    {
        GB, BG, RG, GR
    }

    public enum ScanMode
    {
        Area, Line
    }

    public class CameraSpec
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int ImageDepth { get; set; }
    }

    public enum CameraType
    {
        Jai_GO_5000,
        PrimeTech_PXCB120VTH,
        Crevis_MC_D500B,
        PrimeTech_PXCB16QWTPM,
        PrimeTech_PXCB16QWTPMCOMPACT,
        HV_B550CTRG1,
        HV_B550CTRG2,
        RaL12288_66km,
        RaL6144_80km,
        RaL4096_80km,
        VH_5MC_C16_2Tap,
        ELIIXAp_8k,
        ELIIXAp_16k,
        VT_6K3_5X_H160,
        CXP
    }

    public enum ExpouserMode
    {
        Off,
        Timed,
        TriggerWidth,
        TriggerControlled
    }

    public enum ExpouserAuto
    {
        Off,
        Once,
        Continuous
    }

    public class CameraInitializeFailedException : ApplicationException
    {
    }

    public delegate void CameraEventDelegate(Camera camera);

    public abstract class Camera : Device
    {
        public const int CONTINUOUS = -1;

        protected int index;
        public int Index
        {
            get => index;
            set => index = value;
        }

        protected bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        protected Size imageSize = new Size(0, 0);
        public Size ImageSize
        {
            get
            {
                if (imageSize.IsEmpty)
                {
                    return cameraInfo.Size;
                }
                else
                {
                    return imageSize;
                }
            }
            set => imageSize = value;
        }

        protected int imagePitch;
        public int ImagePitch
        {
            get => imagePitch;
            set => imagePitch = value;
        }

        protected CameraInfo cameraInfo;
        public CameraInfo CameraInfo
        {
            get => cameraInfo;
            set => cameraInfo = value;
        }

        protected SizeF fovSize;
        public SizeF FovSize
        {
            get => fovSize;
            set => fovSize = value;
        }

        protected int numOfBand;
        public int NumOfBand
        {
            get => numOfBand;
            set => numOfBand = value;
        }

        protected int triggerChannel = 0;
        public int TriggerChannel
        {
            get => triggerChannel;
            set => triggerChannel = value;
        }

        protected bool grabFailed = false;
        public bool GrabFailed
        {
            get => grabFailed;
            set => grabFailed = value;
        }

        protected bool bayerCamera;
        public bool BayerCamera
        {
            get => bayerCamera;
            set => bayerCamera = value;
        }

        protected BayerType bayerType;
        public BayerType BayerType
        {
            get => bayerType;
            set => bayerType = value;
        }

        protected RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        public RotateFlipType RotateFlipType
        {
            get => rotateFlipType;
            set => rotateFlipType = value;
        }

        protected TriggerMode triggerMode;
        public TriggerMode TriggerMode => triggerMode;

        protected TriggerType triggerType;
        public TriggerType TriggerType => triggerType;

        protected Stopwatch grabTimer = new Stopwatch();
        protected ManualResetEvent exposureDoneEvent = new ManualResetEvent(true);
        protected ManualResetEvent grabDoneEvent = new ManualResetEvent(true);
        protected ManualResetEvent grabStopEvent = new ManualResetEvent(true);

        protected float exposureTimeUs = 0;
        protected int remainGrabCount = 0;
        protected Mutex grabCallBackMutex = new Mutex();

        protected CameraEventDelegate imageGrabbed = null;
        public CameraEventDelegate ImageGrabbed
        {
            get => imageGrabbed;
            set => imageGrabbed = value;
        }

        protected CameraEventDelegate exposureDone = null;
        public CameraEventDelegate ExposureDone
        {
            get => exposureDone;
            set => exposureDone = value;
        }

        // Image
        public abstract ImageD GetGrabbedImage();

        // Camera
        public virtual void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            this.triggerMode = triggerMode;
            this.triggerType = triggerType;
        }

        public virtual void SetScanMode(ScanMode scanMode) { }
        public virtual void SetAcquisitionLineRate(float grabHz) { }

        public abstract void SetTriggerDelay(int triggerDelayUs);
        public abstract void SetGain(float gain);

        public abstract void GrabOnceAsync();

        public abstract void GrabMulti();

        public Camera()
        {
            Name = "Camera";

            DeviceType = DeviceType.Camera;
            UpdateState(DeviceState.Idle);
        }

        public virtual void Initialize(CameraInfo cameraInfo)
        {
            this.cameraInfo = cameraInfo;
            Index = cameraInfo.Index;
            Enabled = cameraInfo.Enabled;
            rotateFlipType = cameraInfo.RotateFlipType;
        }

        public void UpdateFovSize(SizeF pelSize)
        {
            fovSize.Width = ImageSize.Width * pelSize.Width;
            fovSize.Height = ImageSize.Height * pelSize.Height;
        }

        public virtual bool IsOnLive()
        {
            return remainGrabCount == CONTINUOUS;
        }

        public virtual bool IsCompatibleImage(ImageD image)
        {
            LogHelper.Debug(LoggerType.Grab, "Camera - IsCompatibleBitmap");

            return (image.NumBand == numOfBand && image.DataSize == 1 && IsCompatibleSize(new Size(image.Width, image.Height)));
        }

        protected bool IsCompatibleSize(Size bitMapSize)
        {
            if (Is90Or270Rotated() == false)
            {
                return bitMapSize.Width == ImageSize.Width && bitMapSize.Height == ImageSize.Height;
            }
            else
            {
                return bitMapSize.Width == ImageSize.Height && bitMapSize.Height == ImageSize.Width;
            }
        }

        public bool Is90Or270Rotated()
        {
            return (rotateFlipType == RotateFlipType.Rotate90FlipNone || rotateFlipType == RotateFlipType.Rotate90FlipX || rotateFlipType == RotateFlipType.Rotate90FlipXY || rotateFlipType == RotateFlipType.Rotate90FlipY
                  || rotateFlipType == RotateFlipType.Rotate270FlipNone || rotateFlipType == RotateFlipType.Rotate270FlipX || rotateFlipType == RotateFlipType.Rotate270FlipXY || rotateFlipType == RotateFlipType.Rotate270FlipY);
        }

        public virtual ImageD CreateCompatibleImage()
        {
            LogHelper.Debug(LoggerType.Grab, "Camera - CreateCompatibleImage");

            var image2d = new Image2D();
            image2d.Initialize(ImageSize.Width, ImageSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), ImagePitch);

            return image2d;
        }

        public ImageD GrabOnce()
        {
            GrabOnceAsync();

            if (WaitGrabDone() == false)
            {
                return null;
            }

            return GetGrabbedImage();
        }

        public virtual bool IsGrabDone()
        {
            return grabDoneEvent.WaitOne(10, false);
        }

        public virtual bool IsGrabStop()
        {
            return grabStopEvent.WaitOne(0, false);
        }

        public virtual void Stop()
        {
            grabStopEvent.Set();
        }

        public void Reset()
        {
            exposureDoneEvent.Reset();
            grabDoneEvent.Reset();
            grabStopEvent.Reset();
            GrabFailed = false;
        }

        public float GetExposureTime()
        {
            return GetDeviceExposure();
        }

        protected virtual float GetDeviceExposure()
        {
            return exposureTimeUs;
        }

        public void SetExposureTime(float exposureTimeUs)
        {
            if ((exposureTimeUs != 0) && (this.exposureTimeUs != exposureTimeUs))
            {
                try
                {
                    SetDeviceExposure(exposureTimeUs / 1000f);
                }
                catch (DeviceException ex)
                {
                    ex.Report();
                }
            }
        }

        public virtual bool SetupGrab()
        {
            if (Enabled == false)
            {
                return false;
            }

            using (var bt = new BlockTracer("Camera.SetupGrab"))
            {
                if (grabFailed)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.InvalidState,
                        ErrorLevel.Warning, ErrorSection.Grabber.ToString(), CommonError.InvalidState.ToString(), "Grab Fail State");

                    Reset();

                    return false;
                }

                exposureDoneEvent.Reset();
                grabDoneEvent.Reset();
                grabStopEvent.Reset();

                grabTimer.Restart();

                LogHelper.Debug(LoggerType.Device, string.Format("Setup Grab [{0}].", Index));
            }

            return true;
        }

        protected void ImageGrabStarted()
        {
            //grabTimer.Restart();
            //exposureDoneEvent.Reset();
            grabDoneEvent.Reset();
        }

        protected void ExposureDoneCallback()
        {
            exposureDoneEvent.Set();
        }

        protected void ImageGrabbedCallback()
        {
            {
                grabTimer.Stop();
                exposureDoneEvent.Set();

                //LogHelper.Debug(LoggerType.Grab, String.Format("Image Grabbed [{0}]. {1}ms", Index, grabTimer.ElapsedMilliseconds.ToString()));

                ImageGrabbed?.Invoke(this);

                grabDoneEvent.Set();
            }
        }

        public bool WaitGrabDone(int timeoutMs = 0)
        {
            Thread.Sleep(10);

            if (timeoutMs == 0)
            {
                timeoutMs = DeviceConfig.Instance().GrabTimeoutMs;
            }

            if (timeoutMs == 0)
            {
                timeoutMs = int.MaxValue;
            }

            //Debug.Write("Start Wait Grab Done\n");

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsGrabDone())
                {
                    Debug.Write("End Wait Grab Done\n");

                    return true;
                }

                if (IsGrabStop())
                {
                    Debug.Write("Stop Wait Grab Done\n");

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

        public virtual bool IsLineScanCamera() { return false; }

        protected abstract void SetDeviceExposure(float exposureTimeMs);

        public virtual void SetOffset(int offsetX, int offsetY) { }

        public virtual void UpdateBuffer() { }

        public virtual void UpdateBuffer(int height) { }
    }
}
