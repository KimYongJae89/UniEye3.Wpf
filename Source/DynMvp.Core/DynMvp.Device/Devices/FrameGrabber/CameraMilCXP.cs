using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraMilCXP : Camera
    {
        #region 필드
        private const int bufferPoolCount = 10;
        private MIL_ID digitizerId;
        #endregion


        #region 속성
        private uint FrameId { get; set; } = 0;
        private MIL_ID WhiteKernel { get; set; } = MIL.M_NULL;
        private GCHandle ThisHandle { get; set; }
        private MIL_DIG_HOOK_FUNCTION_PTR FrameTransferEndPtr { get; set; } = null;
        private MIL_DIG_HOOK_FUNCTION_PTR ProcessingFunctionPtr { get; set; } = null;
        private ConcurrentQueue<MIL_ID> GrabbedImageQ { get; set; } = null;
        private ConcurrentQueue<int> ValidHeightBuffer { get; set; } = new ConcurrentQueue<int>();
        private MIL_ID SourceImage { get; set; } = MIL.M_NULL;
        //더 많은 버퍼를 사용하려면 MilConfig -> Non-paged Memory size를 많이 할당해야된다. (기본은 64MByte)
        private MIL_ID[] GrabImageBuffer { get; set; } = new MIL_ID[bufferPoolCount];
        #endregion


        #region 메서드
        public override void Initialize(CameraInfo cameraInfo)
        {
            var cameraInfoMilCXP = (CameraInfoMilCXP)cameraInfo;
            base.Initialize(cameraInfo);

            cameraInfoMilCXP.MilSystem = GrabberMilCXP.GetMilSystem(cameraInfoMilCXP.SystemType, cameraInfoMilCXP.SystemNum);

            if (cameraInfoMilCXP.MilSystem == null)
            {
                LogHelper.Error("MilSystem is empty. Skip create the digitizer.");
                return;
            }

            string dcfFileName = cameraInfoMilCXP.DcfFilePath;
            if (string.IsNullOrEmpty(dcfFileName))
            {
                dcfFileName = GetDcfFile(cameraInfoMilCXP.CameraType);
            }

            MIL.MdigAlloc(cameraInfoMilCXP.MilSystem.SystemId, cameraInfoMilCXP.DigitizerNum, dcfFileName, MIL.M_DEFAULT, ref digitizerId);
            if (digitizerId == null)
            {
                LogHelper.Error(string.Format("Digitizer Allocation is Failed.{0}, {1}, {2}, {3}",
                    cameraInfoMilCXP.SystemType.ToString(), cameraInfoMilCXP.MilSystem.SystemId, cameraInfoMilCXP.DigitizerNum, cameraInfoMilCXP.CameraType.ToString()));
                return;
            }

            MIL.MdigControl(digitizerId, MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS);
            MIL.MdigControl(digitizerId, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE);
            MIL.MdigControl(digitizerId, MIL.M_GRAB_LINE_COUNTER, MIL.M_ENABLE);

            MIL.MdigControl(digitizerId, MIL.M_SOURCE_SIZE_Y, cameraInfoMilCXP.Height);

            ThisHandle = GCHandle.Alloc(this);

            //frameExposureEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameExposureEnd);
            //MIL.MdigHookFunction(digitizerId, MIL.M_GRAB_FRAME_START, frameExposureEndPtr, GCHandle.ToIntPtr(thisHandle));

            FrameTransferEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameTransferEnd);
            MIL.MdigHookFunction(digitizerId, MIL.M_GRAB_END, FrameTransferEndPtr, GCHandle.ToIntPtr(ThisHandle));

            MIL_INT tempValue = 0;
            MIL_INT width = 0;
            MIL_INT height = 0;

            MIL.MdigInquire(digitizerId, MIL.M_SIZE_X, ref width);
            MIL.MdigInquire(digitizerId, MIL.M_SIZE_Y, ref height);
            ImageSize = new Size((int)width, (int)height);

            MIL.MdigInquire(digitizerId, MIL.M_SIZE_BAND, ref tempValue);
            NumOfBand = (int)tempValue;

            ImagePitch = (int)width * NumOfBand;

            if (NumOfBand == 1)
            {
                for (int i = 0; i < GrabImageBuffer.Length; i++)
                {
                    GrabImageBuffer[i] = MIL.MbufAlloc2d(cameraInfoMilCXP.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                }
            }
            else
            {
                for (int i = 0; i < GrabImageBuffer.Length; i++)
                {
                    GrabImageBuffer[i] = MIL.MbufAllocColor(cameraInfoMilCXP.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                }
            }

            GrabbedImageQ = new ConcurrentQueue<MIL_ID>();

            if (cameraInfoMilCXP.BayerCamera == true)
            {
                SourceImage = MIL.MbufAllocColor(cameraInfoMilCXP.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                MIL.MbufPut(WhiteKernel, cameraInfoMilCXP.WhiteBalanceCoefficient);
                BayerType = cameraInfoMilCXP.BayerType;
            }
            else
            {
                SourceImage = MIL.MbufAllocColor(cameraInfoMilCXP.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                BayerType = BayerType.GB;
            }
        }

        public override void SetTriggerDelay(int exposureTimeUs)
        {
            //MIL.MdigControl(digitizerId, MIL.M_GRAB_TRIGGER_DELAY, exposureTimeUs * 1000);
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Grab, "CameraMil - Release Mil System");
            MIL.MdigFree(digitizerId);

            for (int i = 0; i < GrabImageBuffer.Length; i++)
            {
                MIL.MbufFree(GrabImageBuffer[i]);
            }

            if (WhiteKernel != MIL.M_NULL)
            {
                MIL.MbufFree(WhiteKernel);
                MIL.MbufFree(SourceImage);
            }
        }

        public MIL_INT FrameExposureEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            //LogHelper.Debug(LoggerType.Grab, "CameraMil - FrameExposureEnd");

            ExposureDoneCallback();

            return MIL.M_NULL;
        }

        public MIL_INT FrameTransferEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            //LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin FrameTransferEnd");

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            long grabLine = 0;
            MIL.MdigGetHookInfo(EventId, MIL.M_GRAB_LINE_COUNT, ref grabLine);
            ValidHeightBuffer.Enqueue((int)grabLine);

            GrabbedImageQ.Enqueue(currentImageId);
            LogHelper.Debug(LoggerType.Grab, $"CameraMil::ProcessingFunction - Name: {Name}, ID: {currentImageId}, grabbedImageQ: {GrabbedImageQ.Count}");
            ImageGrabbedCallback();

            //LogHelper.Debug(LoggerType.Grab, "CameraMil - End FrameTransferEnd");
            LogHelper.Debug(LoggerType.Grab, $"CameraMil::FrameTransferEnd - End FrameTransferEnd, ValidHeight={grabLine}");

            return MIL.M_NULL;
        }

        private string GetDcfFile(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.VT_6K3_5X_H160:
                    return "VT_6K3_5X_H160.dcf";
                case CameraType.CXP:
                    return "CXP_2022.02.04.dcf";
            }
            return "";
        }

        private static MIL_INT ProcessingFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            if (IntPtr.Zero.Equals(HookDataPtr) == true)
            {
                return MIL.M_NULL;
            }

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            var hUserData = GCHandle.FromIntPtr(HookDataPtr);

            // get a reference to the DigHookUserData object
            var cameraMilCXP = hUserData.Target as CameraMilCXP;
            cameraMilCXP.GrabbedImageQ.Enqueue(currentImageId);
            LogHelper.Debug(LoggerType.Grab, $"CameraMil::ProcessingFunction - Name: {cameraMilCXP.Name}, grabbedImageQ: {cameraMilCXP.GrabbedImageQ.Count}");

            cameraMilCXP.ImageGrabbedCallback();

            return MIL.M_NULL;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin SetTriggerMode");

            base.SetTriggerMode(triggerMode, triggerType);

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigControlFeature(digitizerId, MIL.M_FEATURE_VALUE, "TriggerSelector", MIL.M_TYPE_STRING, "LineStart");
                MIL.MdigControlFeature(digitizerId, MIL.M_FEATURE_VALUE, "TriggerMode", MIL.M_TYPE_STRING, "Off");
            }
            else
            {
                MIL.MdigControlFeature(digitizerId, MIL.M_FEATURE_VALUE, "TriggerSelector", MIL.M_TYPE_STRING, "LineStart");
                MIL.MdigControlFeature(digitizerId, MIL.M_FEATURE_VALUE, "TriggerMode", MIL.M_TYPE_STRING, "On");
            }

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End SetTriggerMode");
        }

        public override void SetAcquisitionLineRate(float grabHz)
        {
            // The following code can be used to control feature values.
            double AcquisitionLineRate = grabHz;
            MIL.MdigControlFeature(digitizerId, MIL.M_FEATURE_VALUE, "AcquisitionLineRate", MIL.M_TYPE_DOUBLE, ref AcquisitionLineRate);
        }

        public override ImageD GetGrabbedImage()
        {
            if (!GrabbedImageQ.TryDequeue(out MIL_ID grabbedImage))
            {
                return null;
            }

            if (grabbedImage != null)
            {
                if (NumOfBand == 1)
                {
                    if (BayerCamera == true)
                    {
                        MIL.MbufBayer(grabbedImage, SourceImage, WhiteKernel, (long)BayerType);
                        return CreateGrabbedImage(true);
                    }
                    else
                    {
                        SourceImage = grabbedImage;
                        return CreateGrabbedImage(false);
                    }
                }
                else if (NumOfBand == 3)
                {
                    //sourceImage = grabbedImage;
                    //return CreateGrabbedImage(true);
                    //MIL.MbufBayer(grabbedImage, sourceImage, MIL.M_DEFAULT, (long)BayerType);
                    MIL.MbufBayer(grabbedImage, SourceImage, MIL.M_DEFAULT, MIL.M_BAYER_BG);
                    return CreateBayerImage();
                }
            }

            return null;
        }

        public ImageD CreateGrabbedImage(bool colorImage)
        {
            if (SourceImage == MIL.M_NULL)
            {
                return null;
            }

            if (ValidHeightBuffer.Count == 0)
            {
                return null;
            }

            ValidHeightBuffer.TryDequeue(out int valH);

            IntPtr hostAddress = IntPtr.Zero;
            MIL_INT addr = MIL.MbufInquire(SourceImage, MIL.M_HOST_ADDRESS, hostAddress);
            int w = (int)MIL.MbufInquire(SourceImage, MIL.M_SIZE_X, IntPtr.Zero);
            int h = (int)MIL.MbufInquire(SourceImage, MIL.M_SIZE_Y, IntPtr.Zero);
            int pitch = (int)MIL.MbufInquire(SourceImage, MIL.M_PITCH, hostAddress);

            byte[] data = new byte[w * h];
            for (int y = 0; y < valH; ++y)
            {
                int ptrOffset = y * pitch;
                Marshal.Copy(addr + ptrOffset, data, y * w, w);
            }

            var image2d = new Image2D();
            image2d.Initialize(w, h, colorImage ? 3 : 1, w, data);
            image2d.Tag = new CameraBufferTag(0, FrameId++, 0, new Size(w, valH));
            return image2d;
        }

        public ImageD CreateBayerImage()
        {
            if (SourceImage == MIL.M_NULL)
            {
                return null;
            }

            var image2d = new Image2D();
            image2d.Initialize(ImageSize.Width, ImageSize.Height, 3, ImagePitch);

            CopyColorImage(image2d);

            return image2d;
        }

        public bool CopyGrayImage(ImageD image)
        {
            if (SourceImage == MIL.M_NULL)
            {
                return false;
            }

            var image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height];

            MIL.MbufGet(SourceImage, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public bool CopyColorImage(ImageD image)
        {
            if (SourceImage == MIL.M_NULL)
            {
                return false;
            }

            var image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height * 3];

            MIL.MbufGetColor(SourceImage, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public override void GrabOnceAsync()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabOnce");

            if (SetupGrab() == false)
            {
                return;
            }

            MIL.MdigGrab(digitizerId, GrabImageBuffer[0]);
            GrabbedImageQ.Enqueue(GrabImageBuffer[0]);
        }

        public override void GrabMulti()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabContinuous");

            if (SetupGrab() == false)
            {
                return;
            }

            base.remainGrabCount = -1;
            //if (triggerMode == TriggerMode.Software)
            //{
            //    //MIL.MdigGrabContinuous(digitizerId, grabbedImage);
            //    if (ProcessingFunctionPtr != null)
            //    {
            //        ProcessingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);
            //    }

            //    MIL.MdigProcess(digitizerId, GrabImageBuffer, GrabImageBuffer.Length, MIL.M_START, MIL.M_DEFAULT, ProcessingFunctionPtr, GCHandle.ToIntPtr(ThisHandle));
            //}
            //else
            {
                if (ProcessingFunctionPtr != null)
                {
                    ProcessingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);
                }

                MIL.MdigProcess(digitizerId, GrabImageBuffer, GrabImageBuffer.Length, MIL.M_START, MIL.M_DEFAULT, ProcessingFunctionPtr, GCHandle.ToIntPtr(ThisHandle));
            }
        }

        public override void Stop()
        {
            base.remainGrabCount = 0;
            FrameId = 0;
            //if (triggerMode == TriggerMode.Software)
            //{
            //    MIL.MdigHalt(digitizerId);
            //}
            //else
            {
                try
                {
                    // 그랩 중 정지하면 안내같은 Mil Popup이 뜬다...
                    MIL.MdigProcess(digitizerId, GrabImageBuffer, GrabImageBuffer.Length, MIL.M_STOP, MIL.M_DEFAULT, ProcessingFunctionPtr, GCHandle.ToIntPtr(ThisHandle));
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, $"{ex.GetType().Name} : {ex.Message}");
                }
                while (GrabbedImageQ.TryDequeue(out MIL_ID id))
                {
                    ;
                }
            }
            Thread.Sleep(50);
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            MIL.MdigControl(digitizerId, MIL.M_TIMER_DURATION + MIL.M_TIMER1, exposureTimeMs * 1000);
        }

        public override void SetGain(float gain)
        {

        }
        #endregion
    }
}
