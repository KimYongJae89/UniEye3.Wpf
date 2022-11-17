using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraVirtual : Camera
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private bool onGrabMulti = false;
        private Timer callbackTimer = new Timer();

        private object bitmapLock = new object();
        private IImageSequence imageSequence;
        private int lastGrabIndex = -1;
        private DateTime lastGrabTime = DateTime.MinValue;

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Virtual Camera");

            base.Initialize(cameraInfo);

            var cameraInfoVirtual = (CameraInfoVirtual)cameraInfo;

            //IImageSequence imageSequence = new ImageSequenceFolder();
            IImageSequence imageSequence;
            if (cameraInfo.IsLineScan)
            {
                imageSequence = new ImageSequenceRoll(cameraInfoVirtual.Size);
            }
            else
            {
                imageSequence = new ImageSequenceFolder();
            }

            imageSequence.SetImagePath(cameraInfoVirtual.FolderPath);
            imageSequence.SetImageNameFormat("*.*");
            SetImageSource(imageSequence);

            ImageSize = new Size(cameraInfo.Width, cameraInfo.Height);
            NumOfBand = cameraInfo.GetNumBand();
            ImagePitch = cameraInfo.Width * NumOfBand;

            callbackTimer.Interval = cameraInfoVirtual.Interval;
            callbackTimer.Elapsed += new ElapsedEventHandler(callbackTimer_Elapsed);
        }

        public override void Release()
        {
            callbackTimer.Stop();
            imageSequence.Dispose();

            base.Release();
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);
        }

        public override void SetTriggerDelay(int triggerDelay)
        {

        }

        public void SetImageSource(IImageSequence imageSequence)
        {
            this.imageSequence = imageSequence;
        }

        public override ImageD GetGrabbedImage() // ref
        {
            ImageD imageD = imageSequence?.GetImage(index);
            if (imageD == null)
            {
                imageD = CreateCompatibleImage();
            }

            imageD.Tag = new CameraBufferTag(0, (ulong)lastGrabIndex, 0, imageD.Size);
            return imageD;
        }

        public override void GrabOnceAsync()
        {
            lastGrabIndex = -1;
            lastGrabTime = DateTime.MinValue;
            //imageSequence.ResetIndex();

            callbackTimer.Start();
        }

        public override void GrabMulti()
        {
            lastGrabIndex = -1;
            lastGrabTime = DateTime.MinValue;
            //imageSequence.ResetIndex();

            onGrabMulti = true;
            callbackTimer.Start();
        }

        public override void Stop()
        {
            callbackTimer.Stop();
            onGrabMulti = false;
        }

        private void callbackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            callbackTimer.Stop();
            lastGrabIndex++;
            lastGrabTime = DateTime.Now;
            imageSequence.MoveNext();

            ImageGrabbedCallback();

            if (onGrabMulti)
            {
                callbackTimer.Start();
            }
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            if (cameraInfo.IsLineScan)
            {
                callbackTimer.Interval = exposureTimeMs * imageSize.Height;
            }
            else
            {
                callbackTimer.Interval = exposureTimeMs;
            }
        }

        public override void SetAcquisitionLineRate(float grabHz)
        {
            if (cameraInfo.IsLineScan)
            {
                base.SetAcquisitionLineRate(grabHz);
                float timer = imageSize.Height / grabHz * 1000;
                callbackTimer.Interval = timer;
            }
        }

        public override void SetGain(float gain)
        {

        }
    }
}
