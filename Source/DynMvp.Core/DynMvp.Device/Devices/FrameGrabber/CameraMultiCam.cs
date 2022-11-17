using DynMvp.Base;
using Euresys.MultiCam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public enum EuresysBoardType
    {
        GrabLink_Value, GrabLink_Base, GrabLink_DualBase, GrabLink_Full, Picolo
    }

    public enum EuresysImagingType
    {
        AREA, LINE //LINE 확인 O, AREA는 확인 X
    }

    internal class McSurface
    {
        private uint handle;
        private byte[] surfaceData;
        private GCHandle pinnedArray;
        private IntPtr dataPtr;

        public void Create(int width, int height, int pitch)
        {
            MC.Create(MC.DEFAULT_SURFACE_HANDLE, out handle);
            MC.SetParam(handle, "SurfaceSize", width * height);
            MC.SetParam(handle, "SurfacePitch", pitch);

            surfaceData = new byte[pitch * height];
            var pinnedArray = GCHandle.Alloc(surfaceData, GCHandleType.Pinned);
            dataPtr = pinnedArray.AddrOfPinnedObject();

            MC.SetParam(handle, "SurfaceAddr", dataPtr);
        }

        public void Release()
        {
            pinnedArray.Free();
        }
    }

    public class CameraMultiCam : Camera
    {
        private EuresysBoardType boardType;

        // The MultiCam object that controls the acquisition
        private uint channel;
        // The MultiCam object that contains the acquired buffer
        private uint currentSurface;
        private MC.CALLBACK multiCamCallback;
        private uint grabCount = 0;
        private int grabIndex = 0;
        private Image2D[] grabbedImages = null;
        private bool isContinuos = false;

        // The Mutex object that will protect image objects during processing
        private Mutex imageMutex = new Mutex();

        private object grabLock = new object();

        public EuresysImagingType imagingType;
        public EuresysImagingType ImagingType => imagingType;
        public EdgeStartPos EdgeStartPos { get; set; }
        public uint ROIStartPos { get; set; }
        public uint ROIWidth { get; set; }

        public override void Initialize(CameraInfo cameraInfo)
        {
            var cameraInfoMultiCam = (CameraInfoMultiCam)cameraInfo;

            base.Initialize(cameraInfo);

            try
            {
                boardType = cameraInfoMultiCam.BoardType;
                //SetBoardTopology(cameraInfoMultiCam);

                // Create a channel and associate it with the first connector on the first board
                MC.Create("CHANNEL", out channel);
                MC.SetParam(channel, "DriverIndex", cameraInfoMultiCam.BoardId);

                // For all GrabLink boards exect Grablink Expert 2 and Dualbase
                string connectorString = GetConnectorString(cameraInfoMultiCam.ConnectorId);
                if (connectorString == "")
                {
                    throw new CameraInitializeFailException(string.Format("Connector String is not specified. BoardType = {0} / ConnectorId = {1}", cameraInfoMultiCam.BoardType.ToString(), cameraInfoMultiCam.ConnectorId));
                }
                MC.SetParam(channel, "Connector", connectorString);

                string camFile = Path.Combine(BaseConfig.Instance().ConfigPath, GetCamFile(cameraInfoMultiCam.CameraType));
                if (camFile == "")
                {
                    throw new CameraInitializeFailException("Cam File is not specified");
                }
                MC.SetParam(channel, "CamFile", camFile);


                MC.GetParam(channel, "Imaging", out string imagingType);
                this.imagingType = (EuresysImagingType)Enum.Parse(typeof(EuresysImagingType), imagingType);

                switch (this.imagingType)
                {
                    case EuresysImagingType.AREA:
                        // Choose the camera expose duration
                        MC.SetParam(channel, "Expose_us", 20);
                        // Choose the pixel color format
                        SetColorFormat(cameraInfoMultiCam.PixelFormat != PixelFormat.Format8bppIndexed);

                        //Set the acquisition mode to Snapshot
                        MC.SetParam(channel, "AcquisitionMode", "SNAPSHOT");
                        // Choose the way the first acquisition is triggered
                        MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                        // Choose the triggering mode for subsequent acquisitions
                        MC.SetParam(channel, "NextTrigMode", "REPEAT");
                        // Choose the number of images to acquire
                        MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                        break;
                    case EuresysImagingType.LINE:
                        MC.SetParam(channel, "Expose_us", 2);
                        SetColorFormat(cameraInfoMultiCam.PixelFormat != PixelFormat.Format8bppIndexed);

                        MC.SetParam(channel, "AcquisitionMode", "WEB");
                        MC.SetParam(channel, "TrigMode", "IMMEDIATE");

                        MC.SetParam(channel, "PageLength_Ln", cameraInfoMultiCam.PageLength);
                        MC.SetParam(channel, "SeqLength_Ln", MC.INDETERMINATE);
                        break;
                }

                // Register the callback function
                multiCamCallback = new MC.CALLBACK(MultiCamCallback);
                MC.RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_END_EXPOSURE, "ON");
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_FILLED, "ON");
                MC.SetParam(channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");

                int height;
                height = (int)cameraInfoMultiCam.PageLength;
                MC.GetParam(channel, "ImageSizeX", out int width);
                //width /= 2;
                //MC.GetParam(channel, "ImageSizeY", out height);
                //MC.GetParam(channel, "BufferPitch", out bufferPitch);

                // Linescan 카메라는 찍는 길이를 가변할 수 있으므로 설정한 셋팅대로 넣는다.
                MC.SetParam(channel, "ImageSizeY", height);
                MC.GetParam(channel, "BufferPitch", out int bufferPitch);

                MC.GetParam(channel, "ImagePlaneCount", out int planeCount);

                ImageSize = new Size(width, height);
                NumOfBand = planeCount;
                ImagePitch = bufferPitch;

                MC.SetParam(channel, "SurfaceCount", cameraInfoMultiCam.SurfaceNum);


                if (grabbedImages != null)
                {
                    for (int i = 0; i < grabbedImages.Length; i++)
                    {
                        grabbedImages[i].Dispose();
                    }
                    grabbedImages = null;
                }

                grabbedImages = new Image2D[cameraInfoMultiCam.SurfaceNum];
                for (int i = 0; i < cameraInfoMultiCam.SurfaceNum; i++)
                {
                    grabbedImages[i] = new Image2D();
                    grabbedImages[i].Initialize(width, height, planeCount, bufferPitch, IntPtr.Zero);
                }

                cameraInfoMultiCam.Width = width;
                cameraInfoMultiCam.Height = height;

                EdgeStartPos = cameraInfoMultiCam.EdgeStartPos;
                ROIStartPos = cameraInfoMultiCam.ROIStartPos;
                ROIWidth = cameraInfoMultiCam.ROIWidth;

                //MC.SetParam(channel, "ChannelState", "ACTIVE");
                //MC.SetParam(channel, "ChannelState", "IDLE");
            }
            catch (Euresys.MultiCamException exc)
            {
                throw new CameraInitializeFailException("MultiCam Exception : " + exc.Message);
            }
        }

        public override void UpdateBuffer()
        {
            MC.Delete(channel);
            Initialize(CameraInfo);
        }

        public override void UpdateBuffer(int height)
        {
            int planeCount = 1;
            var cameraInfoMultiCam = (CameraInfoMultiCam)CameraInfo;
            try
            {
                CameraInfo.Height = height;
                MC.SetParam(channel, "ImageSizeY", height);
                MC.SetParam(channel, "PageLength_Ln", CameraInfo.Height);

                MC.GetParam(channel, "ImageSizeX", out int width);
                MC.GetParam(channel, "BufferPitch", out int bufferPitch);
                MC.GetParam(channel, "ImagePlaneCount", out planeCount);

                ImageSize = new Size(width, height);

                if (grabbedImages != null)
                {
                    for (int i = 0; i < grabbedImages.Length; i++)
                    {
                        grabbedImages[i].Dispose();
                    }
                    grabbedImages = null;
                }

                grabbedImages = new Image2D[cameraInfoMultiCam.SurfaceNum];
                for (int i = 0; i < cameraInfoMultiCam.SurfaceNum; i++)
                {
                    grabbedImages[i] = new Image2D();
                    grabbedImages[i].Initialize(width, height, planeCount, bufferPitch, IntPtr.Zero);
                }

                cameraInfoMultiCam.Width = width;
                cameraInfoMultiCam.Height = height;
            }
            catch (Euresys.MultiCamException exc)
            {
                throw new CameraInitializeFailException("MultiCam Exception : " + exc.Message);
            }
        }

        private void SetBoardTopology(CameraInfoMultiCam cameraInfoMultiCam)
        {
            if (boardType == EuresysBoardType.Picolo)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "1_01_2");
            }
            if (boardType == EuresysBoardType.GrabLink_Full)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "MONO_DECA");
            }
        }

        private void SetColorFormat(bool color)
        {
            if (boardType == EuresysBoardType.Picolo)
            {
                if (color)
                {
                    MC.SetParam(channel, "ColorFormat", "RGB24");
                }
                else
                {
                    MC.SetParam(channel, "ColorFormat", "Y8");
                }
            }
            else
            {
                if (color)
                {
                    MC.SetParam(channel, "ColorFormat", "RGB24");
                }
                else
                {
                    MC.SetParam(channel, "ColorFormat", "Y8");
                }
            }
        }

        private string GetCamFile(CameraType cameraType)
        {
            if (boardType == EuresysBoardType.Picolo)
            {
                return "NTSC";
            }
            else
            {
                switch (cameraType)
                {
                    case CameraType.Jai_GO_5000:
                        return "GO-5000M-PMCL_3T8_RG(2560, 2048)_TRG_DN";
                    case CameraType.RaL12288_66km:
                        return "raL12288-66km";
                    case CameraType.RaL6144_80km:
                        return "raL6144-80km";
                    case CameraType.RaL4096_80km:
                        return "raL4096-80km";
                    case CameraType.ELIIXAp_8k:
                        return "ELIIXAp-16kCL_L8192RG";
                    case CameraType.ELIIXAp_16k:
                        return "ELIIXAp-16kCL_L16384RG";
                }
            }

            return "";
        }

        private string GetConnectorString(uint connectorId)
        {
            switch (boardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_Full:
                    return "M";
                case EuresysBoardType.GrabLink_DualBase:
                    if (connectorId == 0)
                    {
                        return "A";
                    }
                    else
                    {
                        return "B";
                    }

                case EuresysBoardType.Picolo:
                    return string.Format("VID{0}", connectorId + 1);
            }

            return "";
        }

        private string GetTriggerLineName(int triggerChannel)
        {
            switch (boardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_DualBase:
                case EuresysBoardType.GrabLink_Full:
                    string[] grabLinkTriggerNameList = new string[] { "NOM", "DIN1", "DIN2", "IIN1", "IIN2", "IIN3", "IIN4" };
                    return grabLinkTriggerNameList[triggerChannel];
            }

            return "NOM";
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            try
            {
                if (triggerMode == TriggerMode.Software)
                {
                    // Choose the way the first acquisition is triggered
                    switch (imagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                            MC.SetParam(channel, "ChannelState", "IDLE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "LineRateMode", "PULSE");
                            //MC.SetParam(channel, "ChannelState", "IDLE");
                            break;
                    }
                }
                else
                {
                    switch (imagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "HARD");

                            MC.SetParam(channel, "TrigLine", GetTriggerLineName(triggerChannel));        // Norminal

                            if (triggerType == TriggerType.FallingEdge)
                            {
                                MC.SetParam(channel, "TrigEdge", "GOLOW");
                            }
                            else
                            {
                                MC.SetParam(channel, "TrigEdge", "GOHIGH");
                            }

                            MC.SetParam(channel, "TrigFilter", "ON");

                            MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);

                            // Parameter valid only for Grablink Full, DualBase, Base
                            MC.SetParam(channel, "TrigCtl", "ISO");
                            MC.SetParam(channel, "ChannelState", "ACTIVE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "LineCaptureMode", "ALL");
                            //MC.SetParam(channel, "LineTrigCtl", "DIFF");
                            MC.SetParam(channel, "LineTrigEdge", "RISING_A");
                            //MC.SetParam(channel, "LineTrigFilter", "OFF");
                            //MC.SetParam(channel, "LineTrigLine", "DIN1");
                            break;
                    }
                }
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            MC.SetParam(channel, "TrigDelay_us", triggerDelayUs);
        }

        public override void Release()
        {
            base.Release();
            MC.Delete(channel);

            Array.ForEach(grabbedImages, f => f?.Dispose());
        }

        public override ImageD GetGrabbedImage()
        {
            Debug.Assert(grabbedImages != null);

            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - UpdateImage");
            return grabbedImages[grabIndex];

        }

        public ImageD GetGrabbedImage(uint index)
        {
            Debug.Assert(grabbedImages != null);

            lock (grabLock)
            {
                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - UpdateImage");
                return grabbedImages[index];
            }
        }

        public override bool SetupGrab()
        {
            return base.SetupGrab();
        }

        public override void GrabOnceAsync()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabSingle");

            try
            {
                isContinuos = false;
                grabIndex = 0;
                this.grabCount = 0;

                SetupGrab();

                MC.GetParam(channel, "SeqLength_Pg", out int grabCount);

                // 기존 설정 저장
                //MC.GetParam(channel, "AcquisitionMode", out AcquisitionMode);
                //MC.GetParam(channel, "TrigMode", out TrigMode);

                //// Software Trigger 로 변경
                //MC.SetParam(channel, "AcquisitionMode", "PAGE");
                //MC.SetParam(channel, "TrigMode", "SOFT");

                //MC.SetParam(channel, "LineRateMode", "PERIOD");
                MC.SetParam(channel, "LineCaptureMode", "ALL");
                MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                MC.SetParam(channel, "ChannelState", "ACTIVE");
                //MC.SetParam(channel, "ForceTrig", "TRIG");
                MC.SetParam(channel, "ChannelState", "IDLE");

                //MC.SetParam(channel, "AcquisitionMode", AcquisitionMode);
                //MC.SetParam(channel, "TrigMode", TrigMode);

                if (DeviceConfig.Instance().GrabTimeoutMs == 0)
                {
                    MC.SetParam(channel, "AcqTimeout_ms", "-1");
                }
                else
                {
                    MC.SetParam(channel, "AcqTimeout_ms", DeviceConfig.Instance().GrabTimeoutMs);
                }

                MC.SetParam(channel, "SeqLength_Pg", grabCount);

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
        }

        public override void GrabMulti()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabContinuous");

            try
            {
                SetupGrab();
                //MC.SetParam(channel, "LineCaptureMode", "ALL");
                //MC.SetParam(channel, "LineTrigCtl", "DIFF");
                //MC.SetParam(channel, "LineTrigEdge", "RISING_A");
                //MC.SetParam(channel, "LineTrigFilter", "OFF");
                //MC.SetParam(channel, "LineTrigLine", "DIN1");

                if (DeviceConfig.Instance().GrabTimeoutMs == 0)
                {
                    MC.SetParam(channel, "AcqTimeout_ms", "-1");
                }
                else
                {
                    MC.SetParam(channel, "AcqTimeout_ms", DeviceConfig.Instance().GrabTimeoutMs);
                }

                isContinuos = true;
                //MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);

                MC.SetParam(channel, "ChannelState", "ACTIVE");

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
        }

        private void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case MC.SIG_END_EXPOSURE:
                    ProcessingEndExposureCallback();
                    break;
                case MC.SIG_SURFACE_FILLED:
                    ProcessingSurfaceFilledCallback(signalInfo);
                    break;
                case MC.SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    Debug.WriteLine("Multicam - Grab Timeout", DateTime.Now.ToString("HHmmss"));
                    break;
                default:
                    throw new Euresys.MultiCamException("Unknown signal");
            }
        }

        private void ProcessingEndExposureCallback()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingEndExposureCallback");

            ExposureDoneCallback();
        }

        private void ProcessingSurfaceFilledCallback(MC.SIGNALINFO signalInfo)
        {
            if (imageMutex.WaitOne(0) == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback");

            var cameraInfoMultiCam = (CameraInfoMultiCam)CameraInfo;

            uint currentChannel = (uint)signalInfo.Context;

            currentSurface = signalInfo.SignalInfo;

            try
            {
                lock (grabLock)
                {
                    grabIndex = (grabIndex + 1) % grabbedImages.Length;
                    MC.GetParam(channel, "ImageSizeX", out double width);
                    MC.GetParam(channel, "ImageSizeY", out double height);
                    MC.GetParam(channel, "PageLength_Ln", out double length);

                    // Update the image with the acquired image buffer data 
                    MC.GetParam(currentSurface, "SurfaceAddr", out IntPtr bufferAddress);

                    LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback - SetData");

                    grabbedImages[grabIndex].SetDataPtr(bufferAddress);
                    grabbedImages[grabIndex].Tag = new CameraBufferTag(-1, grabCount, 0, new Size((int)width, (int)length));
                    Debug.WriteLine(LoggerType.Grab, $"ProcessingSurfaceFilledCallback - grabIndex: {grabIndex}");
                    grabCount++;

                    LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback - Call ImageGrabbedCallback");

                    ImageGrabbedCallback();
                }

                imageMutex.ReleaseMutex();
            }
            catch (Euresys.MultiCamException exc)
            {
                LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
            catch (System.Exception exc)
            {
                LogHelper.Error("System Exception : " + exc.Message);
            }
        }

        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            LogHelper.Error("Acquisition Failure, Channel State: IDLE");
            GrabFailed = true;

            MC.GetParam(channel, "ChannelState", out string grabState);

            // Grab 도중 끊겼을 경우 Continuos grab상태면 다시 Grab을 실행한다.
            if (grabState == "IDLE" && isContinuos)
            {
                GrabMulti();
            }
        }

        public override void Stop()
        {
            isContinuos = false;
            MC.SetParam(channel, "ChannelState", "IDLE");

            grabIndex = 0;
            grabCount = 0;
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            MC.SetParam(channel, "Expose_us", exposureTimeMs * 1000);
        }

        public override void SetGain(float gain)
        {
        }

        public override void SetAcquisitionLineRate(float grabHz)
        {
            double period_us = Math.Ceiling(1000000.0 / grabHz);
            MC.SetParam(channel, "LineCaptureMode", "ALL");
            MC.SetParam(channel, "LineRateMode", "PERIOD");//"PERIOD");
            MC.SetParam(channel, "Period_us", period_us);
        }
    }
}
