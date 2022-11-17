using DynMvp.Base;
using Euresys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraGenTL : Camera
    {
        public enum EScanMode { Area, Line }

        public enum EPRNUMode { PRNU1, PRNU2 }

        //bool initModule = false;

        private const ulong PRNUADDRESS = 0x10880000;

        private Euresys.GenTL genTL = null;
        private Euresys.RGBConverter.RGBConverter converter = null;
        protected Euresys.EGrabberCallbackSingleThread grabber = null;
        private ICustomBuffer customBuffer = null;
        public EdgeStartPos EdgeStartPos { get; set; }
        public int OffsetX { get; set; }

        private Image2D lastGrabbedImage = null;
        protected List<Image2D> grabbedImageList = new List<Image2D>();

        private Mutex imageMutex = new Mutex();

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            try
            {
                base.SetTriggerMode(triggerMode, triggerType);

                var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
                if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
                {
                    string mode = grabber.getStringRemoteModule("OperationMode");
                    if (mode != "TDI")
                    {
                        return;
                    }

                    grabber.setStringRemoteModule("TriggerMode", (triggerMode == TriggerMode.Hardware ? "On" : "Off"));
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.SetTriggerMode is failed.", ex);
            }
        }

        public void SetPRNUMode(EPRNUMode penuMode, float[] prnuData)
        {
            int imageWidth = Convert.ToInt32(grabber.getWidth().ToString());
            // PRNU 데이터가 너비와 같은지 비교하여 아니면 리턴
            if (prnuData.Length != imageWidth)
            {
                LogHelper.Debug(LoggerType.Grab, $"CamearGenTL::SetPRNUMode - prnuData length error (ImageWidth : {imageWidth} / prnuData.Length : {prnuData.Length})");
                return;
            }

            switch (penuMode)
            {
                case EPRNUMode.PRNU1: grabber.setStringRemoteModule("PRNUSelector", "PRNU1"); break;
                case EPRNUMode.PRNU2: grabber.setStringRemoteModule("PRNUSelector", "PRNU2"); break;
                default: grabber.setStringRemoteModule("PRNUSelector", "PRNU1"); break;
            }
            LogHelper.Debug(LoggerType.Grab, $"CamearGenTL::SetPRNUMode - Select PRNU Model : {penuMode})");

            // Write your PRNU data. The data type is float. 4 steps.
            for (int i = 0; i < imageWidth; i++)
            {
                grabber.gcWritePortValueRemoteModule(PRNUADDRESS + (ulong)(i * 4), prnuData[i]);
            }
        }

        public void SetFreeMode(float grabHz, int numLine)
        {
            try
            {
                var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
                if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
                {
                    string mode = grabber.getStringRemoteModule("OperationMode");
                    if (mode != "TDI")
                    {
                        grabber.setStringRemoteModule("OperationMode", "TDI");
                    }

                    switch (cameraInfoGenTL.StageType)
                    {
                        case CameraInfoGenTL.EStageType.X64:
                            grabber.setStringRemoteModule("TDIStages", "TDI64");
                            break;
                        case CameraInfoGenTL.EStageType.X128:
                            grabber.setStringRemoteModule("TDIStages", "TDI128");
                            break;
                        case CameraInfoGenTL.EStageType.X192:
                            grabber.setStringRemoteModule("TDIStages", "TDI192");
                            break;
                        case CameraInfoGenTL.EStageType.X256:
                            grabber.setStringRemoteModule("TDIStages", "TDI256");
                            break;
                    }

                    grabber.setStringRemoteModule("ScanDirection", cameraInfoGenTL.DirectionType.ToString());
                    grabber.setStringRemoteModule("TriggerMode", "Off");
                    grabber.setFloatRemoteModule("AcquisitionLineRate", grabHz);
                }

                UpdateBuffer(numLine);
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.SetFreeMode is failed.", ex);
            }
        }

        public void SetScanMode(EScanMode scanMode)
        {
            try
            {
                switch (scanMode)
                {
                    case EScanMode.Area:
                        SetAreaMode();
                        break;
                    case EScanMode.Line:
                        SetLineScanMode();
                        break;
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.SetScanMode is failed.", ex);
            }
        }

        private void SetAreaMode()
        {
            var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "Area")
                {
                    grabber.setStringRemoteModule("OperationMode", "Area");
                }

                grabber.setStringRemoteModule("TDIStages", "TDI128");
            }

            UpdateBuffer(128);
        }

        private void SetLineScanMode()
        {
            var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "TDI")
                {
                    grabber.setStringRemoteModule("OperationMode", "TDI");
                }

                switch (cameraInfoGenTL.StageType)
                {
                    case CameraInfoGenTL.EStageType.X64:
                        grabber.setStringRemoteModule("TDIStages", "TDI64");
                        break;
                    case CameraInfoGenTL.EStageType.X128:
                        grabber.setStringRemoteModule("TDIStages", "TDI128");
                        break;
                    case CameraInfoGenTL.EStageType.X192:
                        grabber.setStringRemoteModule("TDIStages", "TDI192");
                        break;
                    case CameraInfoGenTL.EStageType.X256:
                        grabber.setStringRemoteModule("TDIStages", "TDI256");
                        break;
                }
                grabber.setStringRemoteModule("ScanDirection", cameraInfoGenTL.DirectionType.ToString());
            }

            UpdateBuffer();
        }

        public override void SetAcquisitionLineRate(float grabHz)
        {
            try
            {
                grabber.setFloatRemoteModule("AcquisitionLineRate", grabHz);
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.SetAcquisitionLineRate is failed.", ex);
            }
        }

        public override void Initialize(CameraInfo cameraInfo)
        {
            try
            {
                base.Initialize(cameraInfo);

                var cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;

                genTL = new Euresys.GenTL();
                converter = new Euresys.RGBConverter.RGBConverter(genTL);
                grabber = new Euresys.EGrabberCallbackSingleThread(genTL);

                OffsetX = (int)cameraInfoGenTL.OffsetX;

                customBuffer = CustomBufferFactory.Create(cameraInfo.CustomBufferType);
                //customBuffer = CustomBufferFactory.Create(CustomBufferType.Mil);

                UpdateBuffer();

                SetDevice();

                grabber.setStringRemoteModule("BinningVertical", cameraInfoGenTL.BinningVertical ? "X2" : "X1");
                grabber.setStringRemoteModule("ReverseX", cameraInfoGenTL.MirrorX.ToString());
                grabber.setStringRemoteModule("AnalogGain", cameraInfoGenTL.AnalogGain.ToString());
                grabber.setFloatRemoteModule("DigitalGain", cameraInfoGenTL.DigitalGain);

                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', true);}");

                grabber.onNewBufferEvent = GenTLCamCallback;
                grabber.enableAllEvent();

                SizeT width = grabber.getWidth();
                SizeT height = grabber.getHeight();
            }
            catch (Euresys.gentl_error ex)
            {
                if (customBuffer != null)
                {
                    customBuffer.Dispose();
                }

                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw new CameraInitializeFailException("GenTL Exception : " + ex.Message);
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        protected virtual void SetDevice()
        {
            var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
            grabber.setStringRemoteModule("PixelFormat", "Mono8");
            switch (cameraInfoGenTL.ClientType)
            {
                case CameraInfoGenTL.EClientType.Master:
                    // Interface
                    grabber.setStringInterfaceModule("LineInputToolSelector", "LIN1");
                    grabber.setStringInterfaceModule("LineInputToolSource", "DIN11");
                    grabber.setStringInterfaceModule("LineInputToolActivation", "RisingEdge");

                    // Device
                    grabber.setStringDeviceModule("CameraControlMethod", "RC");
                    grabber.setFloatDeviceModule("CycleMinimumPeriod", 3.36);
                    grabber.setStringDeviceModule("CycleTriggerSource", "Immediate");
                    grabber.setStringDeviceModule("StartOfSequenceTriggerSource", "LIN1");
                    grabber.setStringDeviceModule("EndOfSequenceTriggerSource", "SequenceLength");
                    grabber.setStringDeviceModule("SequenceLength", "1");
                    grabber.setStringDeviceModule("CxpTriggerMessageFormat", "RisingEdge");

                    // Romote Module
                    grabber.setIntegerRemoteModule("OffsetX", cameraInfoGenTL.OffsetX);
                    grabber.setStringRemoteModule("BinningVertical", cameraInfoGenTL.BinningVertical ? "X2" : "X1");
                    grabber.setStringRemoteModule("ReverseX", cameraInfoGenTL.MirrorX.ToString());
                    grabber.setStringRemoteModule("AnalogGain", cameraInfoGenTL.AnalogGain.ToString());
                    grabber.setFloatRemoteModule("DigitalGain", cameraInfoGenTL.DigitalGain);
                    grabber.setStringRemoteModule("TriggerSource", "CXPin");
                    grabber.setStringRemoteModule("TriggerActivation", "RisingEdge");
                    //grabber.setStringRemoteModule("TriggerRescalerMode", "On");
                    //grabber.setStringRemoteModule("TriggerRescalerRate", "4.0");

                    SetLineScanMode();

                    break;
                case CameraInfoGenTL.EClientType.Slave:
                    grabber.setStringInterfaceModule("DelayToolSelector", "DEL1");
                    grabber.setStringInterfaceModule("DelayToolSource1", "EIN1");
                    grabber.setStringInterfaceModule("EventInputToolSelector", "EIN1");
                    grabber.setStringInterfaceModule("EventInputToolSource", "A");
                    grabber.setStringInterfaceModule("EventInputToolActivation", "StartOfScan");

                    grabber.setStringStreamModule("StartOfScanTriggerSource", "EIN1");
                    break;
            }
        }

        public override void SetOffset(int offsetX, int offsetY)
        {

        }

        public override void UpdateBuffer()
        {
            try
            {
                UpdateBuffer(0);
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.UpdateBuffer is failed.", ex);
            }
        }

        public override void UpdateBuffer(int height)
        {
            var cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;

            int width = cameraInfoGenTL.Width;
            int count = (int)cameraInfoGenTL.FrameNum;

            if (height <= 0)
            {
                height = cameraInfoGenTL.Height;
            }
            else
            {
                cameraInfoGenTL.Height = height;
            }

            int curWidth = (int)grabber.getIntegerRemoteModule("Width");
            int curBufferHeight = (int)grabber.getIntegerStreamModule("BufferHeight");
            int curScanLength = (int)grabber.getIntegerStreamModule("ScanLength");

            //if (curWidth == width && curBufferHeight == height && curScanLength == height)
            //    return;

            grabber.setIntegerRemoteModule("Width", width);
            grabber.setIntegerStreamModule("BufferHeight", height);
            grabber.setIntegerStreamModule("ScanLength", height);

            Thread.Sleep(200);

            ReleaseBuffer();

            grabber.flushBuffers(Euresys.gc.ACQ_QUEUE_TYPE.ACQ_QUEUE_ALL_DISCARD);
            grabber.resetBufferQueue();

            BufferIndexRange bufferIndexRange;
            if (customBuffer != null)
            {
                long bufWidth = width;
                long bufHeight = height * count;
                customBuffer.Dispose();
                customBuffer.Update(bufWidth, bufHeight);

                var userMemoryArray = new UserMemoryArray();
                userMemoryArray.memory.@base = customBuffer.Ptr;
                userMemoryArray.memory.size = (ulong)(width * height * count);
                userMemoryArray.bufferSize = (ulong)(width * height);

                bufferIndexRange = grabber.announceAndQueue(userMemoryArray);
            }
            else
            {
                bufferIndexRange = grabber.reallocBuffers((uint)count);
            }

            SizeT bufferIndex = bufferIndexRange.begin;
            while (bufferIndex != bufferIndexRange.end)
            {
                grabber.getBufferInfo(bufferIndex, Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out IntPtr imgPtr);

                var grabbedImage = new Image2D();
                grabbedImage.Initialize(width, height, 1, width, imgPtr);

                grabbedImage.Tag = new CameraBufferTag() { BufferId = grabbedImageList.Count };
                grabbedImageList.Add(grabbedImage);

                bufferIndex++;
            }

            ImageSize = new System.Drawing.Size(width, height);

            //initModule = true;
        }

        public virtual void ReleaseBuffer()
        {
            using (var bt = new BlockTracer("CameraGenTL.ReleaseBuffer"))
            {
                foreach (Image2D grabbedImage in grabbedImageList)
                {
                    grabbedImage.Dispose();
                }

                grabbedImageList.Clear();
            }
        }

        public void GenTLCamCallback(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            try
            {
                using (var buffer = new Euresys.ScopedBuffer(g, data))
                {
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out IntPtr ptr);
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_FRAMEID, out ulong frameId);
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_WIDTH, out long width);
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_HEIGHT, out long height);
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_SIZE, out long size);
                    height = size / width;
                    int bufferId = grabbedImageList.FindIndex(f => f.DataPtr == ptr);

                    var cameraBufferTag = grabbedImageList[bufferId].Tag as CameraBufferTag;
                    cameraBufferTag.FrameId = frameId;
                    cameraBufferTag.FrameSize = new System.Drawing.Size((int)width, (int)height);

                    lastGrabbedImage = grabbedImageList[bufferId];

                    LogHelper.Debug(LoggerType.Device, $"CameraGenTL::GenTLCamCallback, Id{frameId}, W{width}, H{height}");
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.GenTLCamCallback is failed.", ex);
            }

            ImageGrabbedCallback();
        }

        private static long fpsToMicroseconds(long fps)
        {
            if (fps == 0)
            {
                return 0;
            }
            else
            {
                return (1000000 + fps - 1) / fps;
            }
        }

        private void Start()
        {
            grabber.resetBufferQueue();
            grabber.start();
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {

        }

        public override void Release()
        {
            using (var bt = new BlockTracer("CameraGenTL.Release"))
            {
                base.Release();

                if (customBuffer != null)
                {
                    customBuffer.Dispose();
                }

                if (grabber != null)
                {
                    grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', false);}");
                }

                if (grabber != null)
                {
                    grabber.stop();
                    grabber.Dispose();
                }
                grabber = null;

                if (converter != null)
                {
                    converter.Dispose();
                }
                converter = null;

                if (genTL != null)
                {
                    genTL.Dispose();
                }
                genTL = null;
            }
        }

        public override ImageD GetGrabbedImage()
        {
            Debug.Assert(lastGrabbedImage != null);

            var cameraBufferTag = lastGrabbedImage.Tag as CameraBufferTag;

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraGenTL::GetGrabbedImage - {0}", cameraBufferTag.BufferId));
            return lastGrabbedImage.Clone();
        }

        public override void GrabOnceAsync()
        {
            using (var bt = new BlockTracer("CameraGenTL.GrabOnceAsync"))
            {
                try
                {
                    if (SetupGrab() == false)
                    {
                        return;
                    }

                    Start();
                }
                catch (Euresys.gentl_error ex)
                {
                    throw new DeviceException("CameraGenTL.GrabOnceAsync is failed.", ex);
                }
            }
        }

        public override void GrabMulti()
        {
            using (var bt = new BlockTracer("CameraGenTL.GrabMulti"))
            {
                try
                {
                    if (SetupGrab() == false)
                    {
                        return;
                    }

                    Start();
                }
                catch (Euresys.gentl_error ex)
                {
                    Stop();
                    throw new DeviceException("CameraGenTL.GrabOnceAsync is failed.", ex);
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
            grabber.stop();
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            Debug.Assert(exposureTimeMs > 0, "Exposure time must be positive.");

            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "Area")
                {
                    grabber.setFloatRemoteModule("ExposureTime", exposureTimeMs * 1000);
                }
                else
                {
                    float grabHz = 1 / exposureTimeMs * 1000;
                    grabber.setFloatRemoteModule("AcquisitionLineRate", grabHz);
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.SetDeviceExposure is failed.", ex);
            }
        }

        protected override float GetDeviceExposure()
        {
            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "Area")
                {
                    return (float)grabber.getFloatRemoteModule("ExposureTime") / 1000f;
                }
                else
                {
                    float grabHz = (float)grabber.getFloatRemoteModule("AcquisitionLineRate"); // [1/s]
                    return (1 / grabHz) * 1000f;
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException("CameraGenTL.GetDeviceExposure is failed.", ex);
            }
        }

        public string GetPropertyData(string itemName)
        {
            try
            {
                switch (itemName)
                {
                    case "DeviceModelName":
                    case "DeviceSerialNumber":
                    case "OperationMode":
                    case "TDIStages":
                    case "ScanDirection":
                        return grabber.getStringRemoteModule(itemName);

                    case "AcquisitionLineRate":
                        return grabber.getFloatRemoteModule(itemName).ToString();

                    case "Width":
                        return grabber.getIntegerRemoteModule(itemName).ToString();

                    case "ScanLength":
                        return grabber.getIntegerStreamModule(itemName).ToString();

                    default:
                        return null;
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException(string.Format("CameraGenTL.GetPropertyData {0} is failed.", itemName), ex);
            }
        }

        public void SetPropertyData(string itemName, string value)
        {
            try
            {
                switch (itemName)
                {
                    case "OperationMode":
                    case "TDIStages":
                    case "ScanDirection":
                        grabber.setStringRemoteModule(itemName, value);
                        break;

                    case "AcquisitionLineRate":
                        grabber.setFloatRemoteModule(itemName, Convert.ToSingle(value));
                        break;

                    case "Width":
                    case "ScanLength":
                        grabber.setIntegerRemoteModule(itemName, Convert.ToInt32(value));
                        break;

                    default:
                        break;
                }
            }
            catch (Euresys.gentl_error ex)
            {
                throw new DeviceException(string.Format("CameraGenTL.SetPropertyData {0} / {1}  is failed.", itemName, value), ex);
            }
        }

        public override void SetGain(float gain)
        {

        }

        public override bool IsLineScanCamera()
        {
            return true;
        }
    }
}
