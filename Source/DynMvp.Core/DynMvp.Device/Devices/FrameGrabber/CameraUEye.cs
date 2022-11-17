using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraUEye : Camera
    {
        private uEye.Camera camera = new uEye.Camera();
        public string SerialNo { get; set; } = null;

        ~CameraUEye()
        {

        }

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Pylon Camera");

            base.Initialize(cameraInfo);

            var cameraInfoUEye = (CameraInfoUEye)cameraInfo;

            LogHelper.Debug(LoggerType.StartUp, string.Format("Open uEye camera - Device Serial : {0}", cameraInfoUEye.SerialNo));

            // 카메라 목록을 가져옴
            uEye.Info.Camera.GetCameraList(out uEye.Types.CameraInformation[] cameraList);

            // 가져온 목록 중 Serial이 같은 카메라의 Dev ID를 가져옴
            foreach (uEye.Types.CameraInformation info in cameraList)
            {
                if (info.SerialNumber == cameraInfoUEye.SerialNo)
                {
                    cameraInfoUEye.DeviceId = (int)info.DeviceID;
                    break;
                }
            }

            uEye.Defines.Status statusRet = camera.Init(cameraInfoUEye.DeviceId | (int)uEye.Defines.DeviceEnumeration.UseDeviceID);
            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                LogHelper.Error("Initializing the camera failed");
                return;
            }

            statusRet = camera.Memory.Allocate();
            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                LogHelper.Error("Allocating memory failed");
                return;
            }

            if (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                camera.PixelFormat.Set(uEye.Defines.ColorMode.Mono8);
                NumOfBand = 1;
            }
            else
            {
                camera.PixelFormat.Set(uEye.Defines.ColorMode.RGB8Packed);
                NumOfBand = 3;
            }

            camera.Size.AOI.Get(out Rectangle aoiRect);
            ImagePitch = aoiRect.Width * NumOfBand;
            ImageSize = aoiRect.Size;
            if (cameraInfo.MirrorX)
            {
                camera.RopEffect.Set(uEye.Defines.RopEffectMode.LeftRight, true);
            }

            if (cameraInfo.MirrorY)
            {
                camera.RopEffect.Set(uEye.Defines.RopEffectMode.UpDown, true);
            }
            //statusRet = camera.Parameter.Load("C:\\Unieye\\Config\\IDS_Camera_Setting.ini");
            //if (statusRet != uEye.Defines.Status.Success)
            //{
            //    LogHelper.Error("Loading parameter failed: " + statusRet);
            //}

            camera.EventFrame += onFrameEvent;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            if (triggerMode == TriggerMode.Software)
            {
                camera.Trigger.Set(uEye.Defines.TriggerMode.Software);
            }
            else
            {
                uEye.Defines.TriggerMode uEyeTriggerMode;
                if (triggerType == TriggerType.FallingEdge)
                {
                    uEyeTriggerMode = uEye.Defines.TriggerMode.Hi_Lo;
                }
                else
                {
                    uEyeTriggerMode = uEye.Defines.TriggerMode.Lo_Hi;
                }

                camera.Trigger.Set(uEyeTriggerMode);
            }
        }

        public override void SetTriggerDelay(int triggerDelay)
        {
            camera.Trigger.Delay.Set(triggerDelay);
            camera.Timeout.Set(uEye.Defines.TimeoutMode.Trigger, 4000000);
            LogHelper.Debug(LoggerType.Operation, string.Format("Ueye camera set trigger delay : {0}", triggerDelay));
        }

        public override ImageD GetGrabbedImage() //ref
        {
            camera.Memory.GetActive(out int memoryId);
            camera.Memory.Lock(memoryId);

            camera.Memory.ToIntPtr(memoryId, out IntPtr srcImagePtr);

            var image2d = new Image2D();
            image2d.Initialize(ImageSize.Width, ImageSize.Height, NumOfBand, ImagePitch, srcImagePtr);

            camera.Memory.Unlock(memoryId);

            return image2d;
        }

        private void onFrameEvent(object sender, EventArgs e)
        {
            if (camera.IsOpened)
            {
                camera.Device.GetDeviceID(out int deviceId);

                LogHelper.Debug(LoggerType.Grab, string.Format("onFrameEvent : DeviceId {0}", deviceId));

                ImageGrabbedCallback();
            }
            else
            {
                LogHelper.Debug(LoggerType.Grab, "Camera is closed");
            }
        }

        public override void Release()
        {
            base.Release();

            camera.Exit();
        }

        public override void GrabOnceAsync()
        {
            if (SetupGrab() == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot {0}", Index));

            uEye.Defines.Status statusRet = camera.Acquisition.Freeze();

            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                LogHelper.Error(string.Format("Single Shot Error : Status Code = {0}", statusRet));
                return;
            }
        }

        public override void GrabMulti()
        {
            if (SetupGrab() == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Start GrabMulti {0}", Index));

            uEye.Defines.Status statusRet = camera.Acquisition.Capture();

            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                LogHelper.Error(string.Format("StartContinuous Error : Status Code = {0}", statusRet));
                return;
            }
        }

        public override void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("Stop Grab {0}", Index));

            uEye.Defines.Status statusRet = camera.Acquisition.Stop();

            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                LogHelper.Error(string.Format("Stop Grab Error : Status Code = {0}", statusRet));
                return;
            }
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            uEye.Defines.Status statusRet;

            statusRet = camera.Timing.Exposure.GetRange(out uEye.Types.Range<double> range);

            int exposureStep = Convert.ToInt32(Math.Floor((exposureTimeMs - range.Minimum) / range.Increment));

            float adjustExposureTime = (float)(range.Minimum + exposureStep * range.Increment);

            statusRet = camera.Timing.Exposure.Set(adjustExposureTime);
        }

        public override void SetGain(float gain)
        {

        }
    }
}
