using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.UI;
using PylonC.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public enum TriggerSelector
    {
        AcquisitionStart, FrameStart
    }

    public enum LineSelector
    {
        Line1, Line2, Line3
    }

    public enum TriggerSource
    {
        Line1, Line2, Line3, Software
    }

    public enum LineMode
    {
        Input, Output
    }

    public enum LineSource
    {
        ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2
    }

    public class CameraPylon : Camera, IDigitalIo
    {
        private ImageProvider imageProvider = new ImageProvider();

        ~CameraPylon()
        {

        }

        private string modelName;

        public string GetName() { return Name; }
        public int GetNumInPort() { return 1; }
        public int GetNumOutPort() { return 1; }
        public int GetNumInPortGroup() { return 1; }
        public int GetNumOutPortGroup() { return 1; }
        public int GetInPortStartGroupIndex() { return 0; }
        public int GetOutPortStartGroupIndex() { return 0; }

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Pylon Camera");

            base.Initialize(cameraInfo);

            var cameraInfoPylon = (CameraInfoPylon)cameraInfo;

            modelName = cameraInfoPylon.ModelName;

            LogHelper.Debug(LoggerType.StartUp, string.Format("Open pylon camera - Device Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3}", cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo));

            try
            {
                imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(ImageReadyEventCallback);
                imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(DeviceOpenedEventHandler);
                imageProvider.GrabbingStartedEvent += ImageProvider_GrabbingStartedEvent;

                imageProvider.Open(cameraInfoPylon.DeviceIndex);

                SetupImageFormat(cameraInfo.Width, cameraInfo.Height, cameraInfo.PixelFormat);

                cameraInfo.SetNumBand(NumOfBand);
            }
            catch (Exception)
            {
                ImageSize = new Size(cameraInfo.Width, cameraInfo.Height);
                cameraInfo.SetNumBand(NumOfBand);
                MessageBox.Show(string.Format("Can't open camera. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} / Message : {4} ", cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, imageProvider.GetLastErrorMessage()));
                return;
            }

            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;
            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "ExposureMode", "Timed");
            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "ExposureAuto", "Off");

            //if (rotateFlipType == RotateFlipType.RotateNoneFlipXY)
            //{
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseX", true);
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseY", true);
            //}
            //else if (rotateFlipType == RotateFlipType.RotateNoneFlipX)
            //{
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseX", true);
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseY", false);
            //}
            //else if (rotateFlipType == RotateFlipType.RotateNoneFlipY)
            //{
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseX", false);
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseY", true);
            //}
            //else
            //{
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseY", false);
            //    Pylon.DeviceSetBooleanFeature(deviceHandle, "ReverseX", false);
            //}
        }

        public override void UpdateBuffer()
        {
            imageProvider.ReleaseImage();
            imageProvider.Close();

            Initialize(CameraInfo);
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public bool IsVirtual => false;

        public void TriggerOn()
        {
            PylonC.NET.Pylon.DeviceExecuteCommandFeature(imageProvider.DeviceHandle, "TriggerSoftware");
        }

        public void TurnOffTriggerMode()
        {
            PylonC.NET.Pylon.DeviceFeatureFromString(imageProvider.DeviceHandle, "TriggerMode", "Off");
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            // 2019.05.16 mhcho : LineStart 옵션이 Area 카메라에선 설정에 없어서 문제가 발생함.
            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", "LineStart");

            if (triggerMode == TriggerMode.Software)
            {
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "Off");
                //                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Software");
            }
            else
            {
                // Basler 카메라는 Trigger Line이 하나 밖에 없기 때문에 triggerChannel은 사용되지 않음
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "FrequencyConverter");

                if (triggerType == TriggerType.FallingEdge)
                {
                    PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", "FallingEdge");
                }
                else
                {
                    PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", "RisingEdge");
                }
            }
        }

        // Area camera
        public void SetTriggerMode(TriggerSelector triggerSelector, TriggerMode triggerMode, TriggerSource triggerSource, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", triggerSelector.ToString());

            if (triggerMode == TriggerMode.Software)
            {
                //PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "Off");
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", triggerSource.ToString());
                //Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Software");
            }
            else
            {
                // Basler 카메라는 Trigger Line이 하나 밖에 없기 때문에 triggerChannel은 사용되지 않음
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", triggerSource.ToString());

                if (triggerType == TriggerType.FallingEdge)
                {
                    PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", "FallingEdge");
                }
                else
                {
                    PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", "RisingEdge");
                }
            }
        }

        public void SetDigitalIOControl(LineSelector lineSelector, LineMode lineMode, LineSource lineSource = LineSource.ExposureActive, bool lineInverter = false)
        {
            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "LineSelector", lineSelector.ToString());
            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "LineMode", lineMode.ToString());

            if (lineMode == LineMode.Output)
            {
                PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "LineSource", lineSource.ToString());
            }

            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "LineInverter", lineInverter ? 1.ToString() : 0.ToString());
        }

        public void SetAcquisitionFrameRate(float acquisitionFrameRate, bool enable)
        {
            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "AcquisitionFrameRateAbs", acquisitionFrameRate.ToString());
            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "AcquisitionFrameRateEnable", enable ? 1.ToString() : 0.ToString());
        }

        public void SetLineDebouncerTimeAbs(float lineDebouncerTimeAbs)
        {
            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            PylonC.NET.Pylon.DeviceFeatureFromString(deviceHandle, "LineDebouncerTimeAbs", lineDebouncerTimeAbs.ToString());
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("TriggerDelayAbs");
            PylonC.NET.GenApi.FloatSetValue(nodeHandle, triggerDelayUs);
        }

        private void SetupImageFormat(long width = -1, long height = -1, PixelFormat pixelFormat = PixelFormat.Undefined)
        {
            if (width < 0)
            {
                width = PylonC.NET.Pylon.DeviceGetIntegerFeature(imageProvider.DeviceHandle, "Width");
            }

            if (height < 0)
            {
                height = PylonC.NET.Pylon.DeviceGetIntegerFeature(imageProvider.DeviceHandle, "Height");
            }

            if (pixelFormat == PixelFormat.Undefined)
            {
                string imageFormat = PylonC.NET.Pylon.DeviceFeatureToString(imageProvider.DeviceHandle, "PixelFormat");
                if (imageFormat == "Mono8")
                {
                    pixelFormat = PixelFormat.Format8bppIndexed;
                }
                else
                {
                    pixelFormat = PixelFormat.Format32bppRgb;
                }
            }

            ImageSize = new Size((int)width, (int)height);
            if (pixelFormat == PixelFormat.Format8bppIndexed)
            {
                NumOfBand = 1;
                PylonC.NET.Pylon.DeviceFeatureFromString(imageProvider.DeviceHandle, "PixelFormat", "Mono8");
            }
            else
            {
                NumOfBand = 3;
                PylonC.NET.Pylon.DeviceFeatureFromString(imageProvider.DeviceHandle, "PixelFormat", "Mono12");
            }

            ImagePitch = (int)width * NumOfBand;

            PylonC.NET.Pylon.DeviceSetIntegerFeature(imageProvider.DeviceHandle, "Width", width);
            PylonC.NET.Pylon.DeviceSetIntegerFeature(imageProvider.DeviceHandle, "Height", height);

            LogHelper.Debug(LoggerType.Grab, string.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", width, height, ImagePitch, NumOfBand == 1 ? "Mono" : "Color"));
        }

        public override ImageD GetGrabbedImage()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraPylon - GetGrabbedImage Start");

            ImageProvider.Image pylonImage = imageProvider.GetLatestImage();

            var image2d = new Image2D();

            if (pylonImage != null)
            {
                image2d.Initialize(ImageSize.Width, ImageSize.Height, CameraInfo.GetNumBand(),
                                        CameraInfo.GetNumBand() * ImageSize.Width, pylonImage.Buffer);
                image2d.ConvertFromData();
            }
            else
            {
                image2d.Initialize(ImageSize.Width, ImageSize.Height, CameraInfo.GetNumBand(),
                                        CameraInfo.GetNumBand() * ImageSize.Width);
            }

            if (rotateFlipType != RotateFlipType.RotateNoneFlipNone)
            {
                image2d.RotateFlip(rotateFlipType);
            }

            LogHelper.Debug(LoggerType.Grab, "CameraPylon - GetGrabbedImage End");

            return image2d;
        }

        private void ImageReadyEventCallback()
        {
            ImageGrabStarted();

            LogHelper.Debug(LoggerType.Grab, string.Format("ImageReadyEventCallback {0}", Index));

            ImageGrabbedCallback();
        }

        public override void Release()
        {
            base.Release();

            imageProvider.ReleaseImage();
            imageProvider.Close();
        }

        public override void GrabOnceAsync()
        {
            if (SetupGrab() == false)
            {
                MessageBox.Show("Setup Grab is Failed");
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot {0}", Index));

            try
            {
                imageProvider.OneShot();
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : {0} \n {1}", e.Message, imageProvider.GetLastErrorMessage()));
            }
        }

        public override void GrabMulti()
        {
            if (SetupGrab() == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Start Continuous {0}", Index));

            try
            {
                imageProvider.ContinuousShot();
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("StartContinuous Error : {0} \n {1}", e.Message, imageProvider.GetLastErrorMessage()));
            }
        }

        private void ImageProvider_GrabbingStartedEvent()
        {

        }

        public override void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("Stop Continuous {0}", Index));

            imageProvider.Stop();
        }

        private void DeviceOpenedEventHandler()
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("Device Is Opened {0}", Index));
        }

        public void SetImageSize(int width, int height)
        {
            NODE_HANDLE widthNodeHandle = imageProvider.GetNodeFromDevice("Width");
            NODE_HANDLE heightNodeHandle = imageProvider.GetNodeFromDevice("Height");
            try
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Change Size {0} - Width : {1} / Height : {2}", Index, width, height));
                PylonC.NET.GenApi.IntegerSetValue(widthNodeHandle, width);
                PylonC.NET.GenApi.IntegerSetValue(heightNodeHandle, height);
            }
            catch
            {
            }
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("ExposureTimeRaw");
            try
            {
                float exposureTimeUs = exposureTimeMs * 1000;
                LogHelper.Debug(LoggerType.Grab, string.Format("Change Exposure {0} - {1}", Index, exposureTimeUs));

                PylonC.NET.GenApi.IntegerSetValue(nodeHandle, (int)(exposureTimeUs / 35) * 35);
            }
            catch (Exception ex)
            {
                throw new DeviceException("CameraPylon.SetDeviceExposure is failed.", ex);
            }
        }

        public override void SetGain(float gain)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("GainRaw");
            try
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Change Exposure {0} - {1}", Index, exposureTimeUs));

                PylonC.NET.GenApi.IntegerSetValue(nodeHandle, (int)gain);
            }
            catch (Exception ex)
            {
                throw new DeviceException("CameraPylon.SetGain is failed.", ex);
            }
        }

        public bool IsInitialized()
        {
            return true;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("UserOutputValue");

            bool value = (outputPortStatus & 0x1) == 1;
            LogHelper.Debug(LoggerType.Grab, string.Format("User Output Value {0} - {1}", 0, value));
            PylonC.NET.GenApi.BooleanSetValue(nodeHandle, value);
        }

        public uint ReadOutputGroup(int groupNo)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("UserOutputValue");

            bool value = PylonC.NET.GenApi.BooleanGetValue(nodeHandle);
            if (value == true)
            {
                return 1;
            }

            return 0;
        }

        public uint ReadInputGroup(int groupNo)
        {
            return 0;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }

        public override void SetAcquisitionLineRate(float hz)
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraPylon::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));

            try
            {
                NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("AcquisitionLineRateAbs");
                GenApi.FloatSetValue(nodeHandle, hz);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("CameraPylon::SetDeviceExposure - {0}", ex.Message));
            }
        }
    }
}
