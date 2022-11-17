using DALSA.SaperaLT.SapClassBasic;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber
{
    public delegate void OnFrameValidChangedDelegate(bool value);

    internal delegate void ProgrammaryStartEndDelegate();
    public class CameraSapera : Camera
    {
        public CameraInfoSapera CameraInfoSapera => (CameraInfoSapera)base.cameraInfo;
        public bool IsGrabProgrammary
            => CameraInfoSapera.FrameTirggerMode == CameraInfoSapera.EFrameTirggerMode.RisingSnap
            || CameraInfoSapera.FrameTirggerMode == CameraInfoSapera.EFrameTirggerMode.RisingGrabFallingFreeze;

        //public event OnFrameValidChangedDelegate OnFrameValidChanged;
        private ProgrammaryStartEndDelegate OnProgrammaryStart;
        private ProgrammaryStartEndDelegate OnProgrammaryEnd;

        private int programmaryAbortFrameHeight = -1;

        private SapLocation m_ServerLocation;
        private SapAcquisition m_Acquisition; //Frame-Grabber
        private SapAcqDevice m_AcqDevice;  //Camera-Device  for GeniCam communication

        private SapBuffer m_Buffers;
        private SapTransfer m_Xfer;
        private SapGio[] sapGio;

        // IO를 관찰하여 Snap/Grab 동작 수행. (Frame Trigger 대체)
        private bool onProgrammaticGrab = false;

        //private System.Timers.Timer signalFallingCheckTimer;
        private System.Timers.Timer programmaryPollingCheckTimer;

        // for Stop and Go
        private bool isGrabReady = false;

        public bool IsFrameValid { get; private set; }

        private int frameCount = 0;

        private Mutex imageMutex = new Mutex();
        protected IntPtr lastGrabbedImgaePtr = IntPtr.Zero;
        protected List<Image2D> grabbedImageList = new List<Image2D>();
        protected Image2D lastGrabbedImage = null;

        public int GetBufferIndex(IntPtr ptr)
        {
            return grabbedImageList.FindIndex(f => f.DataPtr == ptr);
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetTriggerMode");

            base.SetTriggerMode(triggerMode, triggerType);

            var cameraInfoSapera = (CameraInfoSapera)CameraInfo;
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                try
                {
                    if (triggerMode == TriggerMode.Hardware)
                    {
                        SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0);
                        SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 1);
                        SetGrabberParam(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MIN, 1);
                        SetGrabberParam(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MAX, 10000000);

                        SapAcquisition.Val trigger = (triggerType == TriggerType.RisingEdge ? SapAcquisition.Val.RISING_EDGE : SapAcquisition.Val.FALLING_EDGE);
                        SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_DETECTION, trigger);
                    }
                    else
                    {
                        SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 0);
                        SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 1);
                    }

                    //if (triggerMode == TriggerMode.Hardware)
                    //{
                    //    this.m_AcqDevice.SetFeatureValue("TriggerMode", "External");
                    //    this.m_AcqDevice.SetFeatureValue("TriggerSource", "CLHS");
                    //}
                    //else
                    //{
                    //    this.m_AcqDevice.SetFeatureValue("TriggerMode", "Internal");
                    //}
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, ex.Message);
                    return;
                }
            }
        }

        private bool SetGrabberParam(SapAcquisition.Prm param, string value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<string> - {param}, {value}"); return m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, int value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<int> - {param}, {value}"); return m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, int[] value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<int[]> - {param}, {value}"); return m_Acquisition.SetParameter(param, value, true); }
        private bool SetGrabberParam(SapAcquisition.Prm param, SapAcquisition.Val value) { LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetGrabberParam<Val> - {param}, {value}"); return m_Acquisition.SetParameter(param, value, true); }

        private bool SetDeviceFeatureValue(string featureName, string value)
        {
            bool ok = m_AcqDevice.SetFeatureValue(featureName, value);
            if (ok)
            {
                LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetDeviceFeatureValue - Name: {featureName}, Value: {value}, OK");
            }
            else
            {
                LogHelper.Error(LoggerType.Device, $"CameraSapera::SetDeviceFeatureValue - Name: {featureName}, Value: {value}, Error");
            }

            return ok;
        }

        public override void SetScanMode(ScanMode scanMode)
        {
            switch (scanMode)
            {
                case ScanMode.Area:
                    SetAreaMode();
                    break;
                case ScanMode.Line:
                    SetLineScanMode();
                    break;
            }
        }

        private void SetAreaMode()
        {
            var cameraInfoSapera = ((CameraInfoSapera)cameraInfo);

            SetGrabberParam(SapAcquisition.Prm.STROBE_ENABLE, 0);

            SetFrameTriggerMode(false);

            SetGrabberParam(SapAcquisition.Prm.SCAN, (int)SapAcquisition.Val.SCAN_AREA);

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                m_AcqDevice.SetFeatureValue("sensorTDIModeSelection", "TdiArea");
            }

            SetTriggerMode(TriggerMode.Software);

            UpdateBuffer(cameraInfoSapera.TdiAreaWidth, cameraInfoSapera.TdiAreaHeight);
        }

        private int GetGrabberParam(SapAcquisition.Prm param)
        {
            bool good = m_Acquisition.GetParameter(param, out int res);
            //if (!good)
            //    throw new Exception(string.Format("CameraSapera::SetParam - {0} was Exception", param));
            return res;
        }

        private void SetLineScanMode()
        {
            LogHelper.Debug(LoggerType.Device, $"CameraSapera::SetLineScanMode");
            var cameraInfoSapera = ((CameraInfoSapera)cameraInfo);

            SetGrabberParam(SapAcquisition.Prm.SCAN, (int)SapAcquisition.Val.SCAN_LINE);

            SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ENABLE, 0);

            // Trigger Signal and Action
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_METHOD, 0x2); //CORACQ_VAL_LINE_TRIGGER_METHOD_2
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_DELAY, 0);
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_ENABLE, 1);

            // Rotate
            SapAcquisition.Val flipVal = SapAcquisition.Val.FLIP_OFF;
            if (cameraInfo.RotateFlipType == RotateFlipType.RotateNoneFlipX)
            {
                flipVal = SapAcquisition.Val.FLIP_HORZ;
            }

            SetGrabberParam(SapAcquisition.Prm.FLIP, flipVal);

            // Grabber Strobe
            SetGrabberParam(SapAcquisition.Prm.STROBE_METHOD, SapAcquisition.Val.STROBE_METHOD_3);
            SetGrabberParam(SapAcquisition.Prm.STROBE_POLARITY, SapAcquisition.Val.ACTIVE_HIGH);
            SetGrabberParam(SapAcquisition.Prm.STROBE_DURATION, 1000); //매뉴얼은 us, 실제CamExpert에는 ns ??????
            SetGrabberParam(SapAcquisition.Prm.STROBE_DELAY, 0);  //매뉴얼은 us, 실제CamExpert에는 ns ??????
            SetGrabberParam(SapAcquisition.Prm.STROBE_ENABLE, 0x00000001);

            SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, 0);
            if (cameraInfoSapera.FrameTirggerMode == CameraInfoSapera.EFrameTirggerMode.FrameTrigger)
            {
                //External Frame Trigger
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, 0x00000001);
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_DETECTION, SapAcquisition.Val.ACTIVE_HIGH);
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_LEVEL, SapAcquisition.Val.LEVEL_24VOLTS);
                SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_SOURCE, 0x00000001);

                SetGrabberParam(SapAcquisition.Prm.EXT_TRIGGER_DURATION, 255);
            }

            //External Line Trigger
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_DIRECTION, SapAcquisition.Val.SHAFT_ENCODER_DIRECTION_IGNORE);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_DROP, cameraInfoSapera.EncDrop);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_MULTIPLY, (int)cameraInfoSapera.EncMultiply);
            SetGrabberParam(SapAcquisition.Prm.SHAFT_ENCODER_ORDER, 0);

            //Line Sync Source
            //SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_DETECTION, SapAcquisition.Val.RISING_EDGE);
            //SetGrabberParam(SapAcquisition.Prm.EXT_LINE_TRIGGER_SOURCE, 1);
            SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                // 32k
                m_AcqDevice.SetFeatureValue("sensorTDIModeSelection", "TdiMTF");
                m_AcqDevice.SetFeatureValue("superResolutionMode", "srMapped");  //srDetailRestored 

                // 16k
                //this.m_AcqDevice.SetFeatureValue("sensorTDIModeSelection", "Tdi");

                m_AcqDevice.SetFeatureValue("sensorScanDirection", ((CameraInfoSapera)cameraInfo).ScanDirectionType.ToString());

                m_AcqDevice.SetFeatureValue("GainSelector", "All");

                // Camera Trigger Input
                m_AcqDevice.SetFeatureValue("TriggerMode", "External");
                m_AcqDevice.SetFeatureValue("TriggerSource", "CLHS");

                // Trigger Mirroring Output
                m_AcqDevice.SetFeatureValue("LineSelector", "GPIO4");
                m_AcqDevice.SetFeatureValue("outputLineSource", "On");
                m_AcqDevice.SetFeatureValue("outputLinePulseDelay", "0"); //us
                m_AcqDevice.SetFeatureValue("outputLinePulseDuration", "1"); //us 
                m_AcqDevice.SetFeatureValue("LineInverter", "Off");
            }
            UpdateBuffer(cameraInfo.Width, cameraInfo.Height);
        }

        private void SetFrameTriggerMode(bool enable)
        {
            SetGrabberParam(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, enable ? 1 : 0);
        }

        public override bool SetupGrab()
        {
            if (!base.SetupGrab())
            {
                return false;
            }

            frameCount = 0;
            IsFrameValid = true;
            isGrabReady = ((CameraInfoSapera)cameraInfo).FrameTirggerMode == CameraInfoSapera.EFrameTirggerMode.None;
            programmaryAbortFrameHeight = -1;

            OnProgrammaryStart = null;
            OnProgrammaryEnd = null;

            switch (CameraInfoSapera.FrameTirggerMode)
            {
                case CameraInfoSapera.EFrameTirggerMode.RisingSnap:
                    OnProgrammaryStart += new ProgrammaryStartEndDelegate(() =>
                    {
                        SetDeviceFeatureValue("TriggerMode", "External");
                        if (!m_Xfer.Grabbing && m_Xfer.Snap())
                        {
                            LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryStart - Snap");
                        }
                    });

                    OnProgrammaryEnd = null;
                    break;

                case CameraInfoSapera.EFrameTirggerMode.RisingGrabFallingFreeze:
                    OnProgrammaryStart += new ProgrammaryStartEndDelegate(() =>
                    {
                        SetDeviceFeatureValue("TriggerMode", "External");
                        if (!m_Xfer.Grabbing && m_Xfer.Grab())
                        {
                            LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryStart - Grab");
                        }
                    });

                    if (false)
                    {
                        //this.OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() =>
                        //{
                        //    if (this.m_Xfer.Freeze() && this.m_Xfer.Abort())
                        //        LogHelper.Debug(LoggerType.Grab, "CameraSapera::OnProgrammaryEnd");
                        //});
                    }
                    else
                    // Abort 안쓰기..
                    {
                        OnProgrammaryEnd += new ProgrammaryStartEndDelegate(() => ProgrammaryAbort());
                    }
                    break;

                default:
                    OnProgrammaryStart = null;
                    OnProgrammaryEnd = null;
                    break;
            }


            //signalFallingCheckTimer?.Start();
            programmaryPollingCheckTimer?.Stop();
            return true;
        }

        private void ProgrammaryAbort()
        {
            if (!m_Xfer.Grabbing)
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::Programmary_Abort - Transfer is not Grabbing");
                return;
            }

            // 현재까지 얻은 라인 수 저장
            int index = m_Buffers.Index;
            int pitch = m_Buffers.Pitch;
            int spaceUsed = m_Buffers.SpaceUsed;
            int validLines = spaceUsed / pitch;
            programmaryAbortFrameHeight = validLines;
            LogHelper.Debug(LoggerType.Grab, $"CameraSapera::Programmary_Abort - index : {index}, validLines: {validLines}");

            // Freeze - 현재 프레임까지만 그랩 후 정지.
            if (m_Xfer.Freeze())
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::Programmary_Abort - Set Freeze");
            }

            // Trigger Mode 전환 - 현재 프레임까지 그랩
            if (SetDeviceFeatureValue("TriggerMode", "Internal"))
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::Programmary_Abort - Set TriggerMode Internal");
            }
        }

        private void Xfer_XferNotify(object sender, SapXferNotifyEventArgs argsNotify)
        {
            LogHelper.Debug(LoggerType.Grab, $"CameraSapera::xfer_XferNotify - Start, EventType : {argsNotify.EventType}");
            if (argsNotify.EventType != SapXferPair.XferEventType.EndOfFrame)
            {
                return;
            }

            var camera = argsNotify.Context as CameraSapera;
            var transfer = sender as SapTransfer;

            m_Buffers.GetAddress(m_Buffers.Index, out IntPtr ptr);
            int sizeBytes = m_Buffers.get_SpaceUsed(m_Buffers.Index);
            int actualWidth = m_Buffers.Width;
            int actualHeigth = sizeBytes / m_Buffers.Pitch;
            if (programmaryAbortFrameHeight >= 0)
            {
                actualHeigth = programmaryAbortFrameHeight;
                programmaryAbortFrameHeight = -1;
            }

            if (!isGrabReady)
            // Partial Frame이면 첫번째 프레임은 버림.
            {
                LogHelper.Debug(LoggerType.Grab, $"CameraSapera::xfer_XferNotify - isGrabReady is false.");
                isGrabReady = true;
                return;
            }

            var actialSize = new Size(actualWidth, actualHeigth);
            int bufferId = grabbedImageList.FindIndex(f => f.DataPtr == ptr);

            var tag = new CameraBufferTag(bufferId, (ulong)frameCount++, 0, actialSize);
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::xfer_XferNotify - BufferId: {0}, FrameId: {1}, Heigth: {2}", tag.BufferId, tag.FrameId, tag.FrameSize.Height));
            grabbedImageList[bufferId].Tag = tag;

            lastGrabbedImgaePtr = ptr;
            lastGrabbedImage = grabbedImageList[bufferId];

            exposureDoneEvent.Set();

            ImageGrabbedCallback();
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::xfer_XferNotify - End");

        }

        private void SignalStatusEvent(object sender, SapSignalNotifyEventArgs arg)
        {
            // Signal 이벤트가 먼저 들어올까? Frame Done 이벤트가 먼저 들어올까?
            SignalStatus(arg.SignalStatus);
        }

        private void SignalStatus(SapAcquisition.AcqSignalStatus acqSignalStatus)
        {
            if (!onProgrammaticGrab)
            {
                return;
            }

            // Signal 이벤트에서는 신호 끄기만 함. FrameDone 이벤트에서는 신호 켜기만 함.
            bool isFrameValid = acqSignalStatus.HasFlag(SapAcquisition.AcqSignalStatus.FrameValidPresent);
            bool isLineValid = acqSignalStatus.HasFlag(SapAcquisition.AcqSignalStatus.LineValidPresent);
            //LogHelper.Debug(LoggerType.Grab, $"CameraSapera::UpdateSignalStatus - isFrameValid: {isFrameValid}, isLineValid: {isLineValid}");

            // 그랩 On시 FrameValid 유효.
            //isFrameValid &= isLineValid;

            // 수정: 첫번째 프레임은 항상 버림.
            // on -> off 엣지가 확인되어야 그랩 유효함.
            //if (!this.isGrabValid && this.isFrameValid && !isFrameValid)
            //{
            //    LogHelper.Debug(LoggerType.Grab, "CameraSapera::UpdateSignalStatus - Set isGrabValid True");
            //    this.isGrabValid = true;
            //}
        }

        public override void Initialize(CameraInfo cameraInfo)
        {
            try
            {
                base.Initialize(cameraInfo);

                ScanMode scanMode = (!this.cameraInfo.IsLineScan) ? ScanMode.Area : ScanMode.Line;
                InitializeDevice(scanMode);
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "UniEye3Wpf", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw new CameraInitializeFailException("Sapera Exception : " + ex.Message);
            }
            finally
            {
            }
        }

        private void InitializeDevice(ScanMode scanMode)
        {
            var cameraInfoSapera = cameraInfo as CameraInfoSapera;

            m_ServerLocation = new SapLocation("Xtium2-CLHS_PX8_1", 0);

            if (System.IO.File.Exists(cameraInfoSapera.CcfFIlePath) && scanMode == ScanMode.Line)
            {
                InitDeviceWithCcf(scanMode, cameraInfoSapera);
            }
            else
            {
                InitDeviceWithoutCcf(scanMode, cameraInfoSapera);
            }

            if (true)
            {
                // event 
                m_Acquisition.SignalNotifyEnable = true;
                m_Acquisition.SignalNotify += new SapSignalNotifyHandler(SignalStatusEvent);
                m_Acquisition.SignalNotifyContext = this;
            }
            else
            {
                //this.signalFallingCheckTimer = new System.Timers.Timer();
                //this.signalFallingCheckTimer.Interval = 50;
                //this.signalFallingCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) =>
                //{
                //    if (!this.m_Xfer.Grabbing)
                //        return;

                //    var isFrameValid = this.m_Acquisition.IsSignalStatusActive(SapAcquisition.AcqSignalStatus.FrameValidPresent);
                //    //bool isFrameValid = this.m_Acquisition.IsSignalStatusActive(SapAcquisition.AcqSignalStatus.LineValidPresent);
                //    if (!this.IsFrameValid && isFrameValid)
                //        this.isGrabReady = true;
                //    if (this.IsFrameValid != isFrameValid)
                //    {
                //        this.IsFrameValid = isFrameValid;
                //        OnFrameValidChanged?.Invoke(isFrameValid);
                //    }
                //});
            }

            InitGio();
        }

        private void InitGio()
        {
            int gioCount = SapManager.GetResourceCount(m_ServerLocation, SapManager.ResourceType.Gio);
            sapGio = new SapGio[gioCount];
            for (int i = 0; i < gioCount; i++)
            {
                var sapLocation = new SapLocation(m_ServerLocation.ServerIndex, i);
                var sapGio = new SapGio(sapLocation);
                if (!sapGio.Create())
                {
                    throw new Exception($"CameraSapera GIO Manager Initialize Fail");
                }

                sapGio.GetCapability(SapGio.Cap.DIR_INPUT, out int inputCapValue);
                sapGio.GetCapability(SapGio.Cap.DIR_OUTPUT, out int outputCapValue);

                if (outputCapValue == 0 && inputCapValue > 0)
                {
                    sapGio.GioNotify += new SapGioNotifyHandler(GioNotify);
                    sapGio.EnableEvent(0, SapGio.EventType.RisingEdge | SapGio.EventType.FallingEdge);
                }
                this.sapGio[i] = sapGio;
            }

            programmaryPollingCheckTimer = new System.Timers.Timer();
            programmaryPollingCheckTimer.AutoReset = false;
            programmaryPollingCheckTimer.Interval = 100;
            programmaryPollingCheckTimer.Elapsed += ProgrammaryPollingCheckTimer_Elapsed;
        }

        private void ProgrammaryPollingCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LogHelper.Debug(LoggerType.Grab, $"CameraSapera::ProgrammaryPollingCheckTimer_Elapsed");
            OnProgrammaryEnd?.Invoke();
        }

        private void GioNotify(object sender, SapGioNotifyEventArgs e)
        {
            if (!onProgrammaticGrab)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, $"CameraSapera::GioNotify - PinNumber: {e.PinNumber}, Type: {e.EventType}, Count: {e.EventCount}");

            if (e.PinNumber == 0)
            {
                switch (e.EventType)
                {
                    case SapGio.EventType.RisingEdge:
                        if (programmaryPollingCheckTimer.Enabled)
                        {
                            programmaryPollingCheckTimer.Stop();
                        }
                        else
                        {
                            OnProgrammaryStart?.Invoke();
                        }
                        break;
                    case SapGio.EventType.FallingEdge:
                        {
                            programmaryPollingCheckTimer.Start();
                        }
                        break;
                }
            }
        }

        private void InitDeviceWithCcf(ScanMode scanMode, CameraInfoSapera cameraInfoSapera)
        {
            m_Acquisition = new SapAcquisition(m_ServerLocation, @"C:\\CameraSapera\\3_32K_Default_Default.ccf");
            CreateObject(m_Acquisition);

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                m_AcqDevice = new SapAcqDevice(m_ServerLocation, false);
                CreateObject(m_AcqDevice);
            }
            UpdateBuffer(cameraInfo.Width, cameraInfo.Height);
        }

        private void InitDeviceWithoutCcf(ScanMode scanMode, CameraInfoSapera cameraInfoSapera)
        {
            //Frame Grabber: SapAcquisition
            m_Acquisition = new SapAcquisition(m_ServerLocation);
            CreateObject(m_Acquisition);
            ConfigAcquisition();

            //Camera Device: SapAcqDevice
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                m_AcqDevice = new SapAcqDevice(m_ServerLocation, false);
                CreateObject(m_AcqDevice);
            }
            SetScanMode(scanMode);
        }

        private bool SettingFrameGrabber_Test(SapAcquisition framegrabber)
        {
            bool success;
            bool result = framegrabber.GetCapability(SapAcquisition.Cap.SCAN, out int[] capValue);
            success = framegrabber.GetParameter(SapAcquisition.Prm.SCAN, out int outvalue);
            SapCapPrmType vvval = SapAcquisition.GetParameterType(SapAcquisition.Prm.SCAN);

            //Basic
            success = framegrabber.SetParameter(SapAcquisition.Prm.SCAN, SapAcquisition.Val.SCAN_LINE, true);
            //success = framegrabber.SetParameter(SapAcquisition.Prm.VIDEO, SapAcquisition.Val.VIDEO_MONO, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.PIXEL_DEPTH, 8, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.DATA_LANES, 5, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.CROP_WIDTH, 32768, true);
            success = framegrabber.SetParameter(SapAcquisition.Prm.HACTIVE, 32768, true); //가로 Pixel 사이즈
            success = framegrabber.SetParameter(SapAcquisition.Prm.DATA_VALID_ENABLE, 0, true); //diable
            //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CLHS_CONFIGURATION, 0, true); //none
            success = framegrabber.SetParameter(SapAcquisition.Prm.POCL_ENABLE, SapAcquisition.Val.SIGNAL_POCL_ACTIVE, true); //enalble

            var cameraInfoSapera = (CameraInfoSapera)CameraInfo;
            //grabber.setStringRemoteModule("PixelFormat", "Mono8");
            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                //Advanced Control
                success = framegrabber.SetParameter(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, 0, true);        //Line Sync Source
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.INT_FRAME_TRIGGER_FREQ, 34567, true);         //Internal Frame Trigger Frequency (in Hz)
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MIN, 1, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.CAM_LINE_TRIGGER_FREQ_MAX, 100000, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.INT_FRAME_TRIGGER_FREQ, 34567, true);

                success = framegrabber.SetParameter(SapAcquisition.Prm.LINE_INTEGRATE_METHOD, SapAcquisition.Val.LINE_INTEGRATE_METHOD_7, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.LINE_TRIGGER_METHOD, SapAcquisition.Val.LINE_TRIGGER_METHOD_1, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.LINE_INTEGRATE_ENABLE, SapAcquisition.Val.LINE_INTEGRATE_METHOD_1, true);

                success = framegrabber.SetParameter(SapAcquisition.Prm.STROBE_ENABLE, 0, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.STROBE_METHOD, SapAcquisition.Val.STROBE_METHOD_1, true);

                //External Trigger
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_LEVEL, SapAcquisition.Val.LEVEL_TTL, true);
                success = framegrabber.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_ENABLE, SapAcquisition.Val.EXT_TRIGGER_OFF, true);
                //success = framegrabber.SetParameter(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, 0, true);
                //success = m_Acquisition.SetParameter(SapAcquisition.Prm.EXT_TRIGGER_DETECTION, SapAcquisition.Val.RISING_EDGE, true);
                //External Trigger
            }
            else if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Slave)
            {

            }
            //framegrabber.SetParameter(SapAcquisition.Prm.CROP_HEIGHT, imageHeight, true);
            framegrabber.GetParameter(SapAcquisition.Prm.CROP_WIDTH, out int width);   // 버퍼 width 가져옴 -> SizeX 
            framegrabber.GetParameter(SapAcquisition.Prm.CROP_HEIGHT, out int height);  // 버퍼 height 가져옴 -> SizeY

            ImageSize = new System.Drawing.Size(width, height);
            ImagePitch = width;
            return true;
        }

        private bool SettingCamera_Test(SapAcqDevice cameraDev)
        {
            bool success = false;
            var cameraInfoSapera = (CameraInfoSapera)CameraInfo;

            if (cameraInfoSapera.ClientType == CameraInfoSapera.EClientType.Master)
            {
                ///Camera Control
                success = cameraDev.SetFeatureValue("AcquisitionLineRate", 87719.298); //Acquisition line Rate
                success = cameraDev.SetFeatureValue("sensorTDIModeSelection", "TdiMTF"); //TDI Super Resolution
                                                                                         //success = cameraDev.SetFeatureValue("sensorTDIStagesSelection", "Lines128");
                                                                                         //success = cameraDev.SetFeatureValue("sensorScanDirectionSource", "Internal"); //GPIO2  //Encoder //worng
                success = cameraDev.SetFeatureValue("sensorScanDirection", "Forward"); //Reverse
                success = cameraDev.SetFeatureValue("GainSelector", "All");   //System
                success = cameraDev.SetFeatureValue("superResolutionMode", "srMapped");  //srDetailRestored
                                                                                         //success = cameraDev.SetFeatureValue("srStrength", 0.1);
                                                                                         //Digital IO Control
                success = cameraDev.SetFeatureValue("TriggerMode", "Internal"); //External
                                                                                //success = cameraDev.SetFeatureValue("TriggerSource", "CLHS"); //Encoder GPIO1
            }
            return success;
        }

        private bool ConfigAcquisition()
        {
            var cameraInfoSapera = (CameraInfoSapera)CameraInfo;

            //Basic
            SetGrabberParam(SapAcquisition.Prm.PIXEL_DEPTH, 8);
            SetGrabberParam(SapAcquisition.Prm.DATA_LANES, 5);
            SetGrabberParam(SapAcquisition.Prm.DATA_VALID_ENABLE, 0);
            SetGrabberParam(SapAcquisition.Prm.POCL_ENABLE, SapAcquisition.Val.SIGNAL_POCL_ACTIVE); //enalble

            SetGrabberParam(SapAcquisition.Prm.FRAME_LENGTH, SapAcquisition.Val.FRAME_LENGTH_VARIABLE);

            return true;
        }

        private bool CreateObject(SapXferNode node)
        {
            // Create acquisition object
            if (node != null && !node.Initialized)
            {
                if (node.Create() == false)
                {
                    ReleaseObject(node);
                    return false;
                }
            }
            return true;
        }

        private void ReleaseObject(SapXferNode node)
        {
            if (node != null)
            {
                if (node.Initialized)
                {
                    node.Destroy();
                }

                node.Dispose();
            }
        }

        public bool UpdateBuffer(int width, int height)
        {
            LogHelper.Debug(LoggerType.Device, $"CameraSapera::UpdateBuffer");
            var cameraInfoSapera = (CameraInfoSapera)CameraInfo;

            try
            {
                m_Acquisition.GetParameter(SapAcquisition.Prm.HACTIVE, out int hActive);
                if (hActive > width)
                {
                    SetGrabberParam(SapAcquisition.Prm.CROP_WIDTH, width);
                    SetGrabberParam(SapAcquisition.Prm.HACTIVE, width);
                }
                else
                {
                    SetGrabberParam(SapAcquisition.Prm.HACTIVE, width);
                    SetGrabberParam(SapAcquisition.Prm.CROP_WIDTH, width);
                }
                SetGrabberParam(SapAcquisition.Prm.CROP_HEIGHT, height);

                {
                    int frameNum = (int)cameraInfoSapera.FrameNum;
                    if (SapBuffer.IsBufferTypeSupported(m_ServerLocation, SapBuffer.MemoryType.ScatterGather))
                    {
                        m_Buffers = new SapBufferWithTrash(frameNum, m_Acquisition, SapBuffer.MemoryType.ScatterGather);
                    }
                    else
                    {
                        m_Buffers = new SapBufferWithTrash(frameNum, m_Acquisition, SapBuffer.MemoryType.ScatterGatherPhysical);
                    }

                    CreateObject(m_Buffers);

                    //Transfer
                    m_Xfer = new SapAcqToBuf(m_Acquisition, m_Buffers);
                    m_Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
                    m_Xfer.XferNotify += new SapXferNotifyHandler(Xfer_XferNotify);
                    m_Xfer.XferNotifyContext = this;
                    m_Xfer.Create();

                    ImageSize = new System.Drawing.Size(width, height);
                    ImagePitch = width;

                    grabbedImageList.ForEach(f => f.Dispose());
                    grabbedImageList.Clear();

                    for (int i = 0; i < frameNum; i++)
                    {
                        m_Buffers.GetAddress(i, out IntPtr intPtr);
                        var imageBuffer = new Image2D();
                        imageBuffer.Initialize(width, height, 1, width, intPtr);
                        grabbedImageList.Add(imageBuffer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AlarmException(string.Format("CameraSapera::UpdateBuffer() - Exception - {0}", new object[] { ex.Message }, ""));
            }

            return true;
        }

        public virtual void ReleaseBuffer()
        {
            foreach (Image2D grabbedImage in grabbedImageList)
            {
                grabbedImage.Dispose();
            }

            grabbedImageList.Clear();
        }

        private void Start()
        {
            //grabber.resetBufferQueue(); //off for buffer Operate round-robin
            //grabber.flushBuffers();

        }


        public override void SetTriggerDelay(int triggerDelayUs)
        {
            SetGrabberParam(SapAcquisition.Prm.LINE_TRIGGER_DELAY, triggerDelayUs);
        }

        public override void Release()
        {
            if (m_Xfer != null && m_Xfer.Grabbing)
            {
                m_Xfer.Freeze();
                bool waitDone = m_Xfer.Wait(1000);
                if (!waitDone)
                {
                    LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                    //m_Xfer.Abort();
                }
            }

            ReleaseBuffer();

            m_Xfer?.Destroy();
            m_Xfer?.Dispose();
            m_Xfer = null;

            m_Buffers?.Destroy();
            m_Buffers?.Dispose();
            m_Buffers = null;

            // Dispose 하면 다음 Create 이후 오류 발생함..
            // Destory 하면 오류는 안나지만 Frame Grabbed Event 안들어옴...
            m_AcqDevice?.Destroy();
            m_AcqDevice?.Dispose();
            m_AcqDevice = null;

            m_Acquisition?.Destroy();
            m_Acquisition?.Dispose();
            m_Acquisition = null;

            m_ServerLocation?.Dispose();
            m_ServerLocation = null;

            base.Release();
        }

        public override ImageD GetGrabbedImage()
        {
            Debug.Assert(lastGrabbedImage != null);

            var cameraBufferTag = lastGrabbedImage.Tag as CameraBufferTag;

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::GetGrabbedImage - {0}", cameraBufferTag.BufferId));

            return lastGrabbedImage.Clone();
        }

        // 포인터로 받는 코드
        //public override ImageD GetGrabbedImage(IntPtr ptr)
        //{
        //    //return m_LastGrabbedImage;
        //    if (ptr == IntPtr.Zero)
        //        ptr = this.lastGrabbedImgaePtr;

        //    int bufferIndex = GetBufferIndex(ptr);

        //    //Debug.Assert(bufferIndex >= 0);
        //    if (bufferIndex < 0)
        //        return null;

        //    LogHelper.Debug(LoggerType.Grab, string.Format("CameraGenTL::GetGrabbedImage - {0}", bufferIndex));
        //    return grabbedImageList[bufferIndex].Clone();
        //}

        //public override List<ImageD> GetImageBufferList()
        //{
        //    LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GetImageBufferList");
        //    return new List<ImageD>(grabbedImageList);
        //}

        //public override int GetImageBufferCount()
        //{
        //    //LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GetImageBufferCount");
        //    return grabbedImageList.Count;
        //}

        public override void GrabOnceAsync()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabOnce");

            try
            {
                if (SetupGrab() == false)
                {
                    return;
                }

                LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabOnce");
                m_Xfer.Snap();
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::GrabOnce Exception - " + e.Message);
            }
        }

        public override void GrabMulti()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabMulti");

            try
            {
                if (SetupGrab() == false)
                {
                    return;
                }

                if (!m_Xfer.Grabbing)
                {
                    m_Buffers.ResetIndex();
                    m_Buffers.Clear();

                    if (IsGrabProgrammary)
                    {
                        onProgrammaticGrab = true;
                    }
                    else
                    {
                        m_Xfer.Grab();
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::GrabMulti Exception - " + e.Message);
            }
        }

        // Grab Count 세주는 코드
        //public override void GrabMulti(int grabCount)
        //{
        //    LogHelper.Debug(LoggerType.Grab, "CameraSapera::GrabMulti");

        //    try
        //    {
        //        if (SetupGrab() == false)
        //            return;

        //        m_Buffers.ResetIndex();
        //        m_Buffers.Clear();

        //        this.remainGrabCount = grabCount;

        //        DebugWriteLine("CameraSapera::GrabMulti");
        //        m_Xfer.Grab();
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Error(LoggerType.Error, "CameraSapera::GrabMulti Exception - " + e.Message);
        //    }
        //}

        public override void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraSapera::Stop");
            if (IsGrabStop())
            {
                return;
            }

            base.Stop();

            isGrabReady = false;
            try
            {
                if (true)
                {
                    //this.signalFallingCheckTimer?.Stop();
                    IsFrameValid = false;
                    onProgrammaticGrab = false;

                    if (m_Xfer.Grabbing)
                    {
                        m_Xfer.Freeze();
                        bool waitDone = m_Xfer.Wait(5000);
                        if (!waitDone)
                        {
                            LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                            if (IsGrabProgrammary)
                            {
                                ProgrammaryAbort();
                            }
                            else
                            {
                                m_Xfer.Abort();
                            }
                        }
                    }
                }
                else
                {
                    // Stop 하지 않음.
                    //Thread.Sleep(100);

                    //m_Xfer.Freeze();
                    //bool waitDone = m_Xfer.Wait(1000);
                    //if (!waitDone)
                    //{
                    //    LogHelper.Error(LoggerType.Grab, string.Format("CameraSapera::Stop - Transfer Wait Timeout"));
                    //    DebugWriteLine("CameraSapera::Stop - Wait Timeout");
                    //    m_Xfer.Abort();
                    //}
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::Stop Exception - " + e.Message);
            }
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs <= 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::SetDeviceExposure"));
            try
            {
                if (!cameraInfo.IsLineScan)
                // Area Mode
                {
                    double framePerSec = 1000 / exposureTimeMs * ImageSize.Height;
                    m_AcqDevice.SetFeatureValue("AcquisitionFrameRate", framePerSec);
                }
                else
                // Line Mode
                {
                    int hz = (int)Math.Round(1000 / exposureTimeMs);
                    SetGrabberParam(SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ, hz);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::SetDeviceExposure Fail. " + e.Message);
            }
        }

        public float GetDeviceExposureMs()
        {
            double framePerSec = double.NaN; ;
            if (((CameraInfoSapera)cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
            {
                m_AcqDevice.GetFeatureValue("AcquisitionFrameRate", out framePerSec);
            }

            return (float)(1000 / framePerSec);
        }

        public override void SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
            {
                return;
            }

            int iHz = (int)Math.Floor(hz);
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraSapera::SetAcquisitionLineRate {0:F3}kHz", iHz / 1000f));
            try
            {
                if (((CameraInfoSapera)cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
                {
                    m_AcqDevice.SetFeatureValue("AcquisitionLineRate", hz);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraSapera::SetDeviceExposure Fail. " + e.Message);
            }
        }

        public float GetAcquisitionLineRate()
        {
            float hz = float.NaN;
            if (((CameraInfoSapera)cameraInfo).ClientType == CameraInfoSapera.EClientType.Master)
            {
                m_AcqDevice.GetFeatureValue("AcquisitionLineRate", out hz);
            }

            return hz;
        }

        public override void SetGain(float gain) { return; }
    }
}
