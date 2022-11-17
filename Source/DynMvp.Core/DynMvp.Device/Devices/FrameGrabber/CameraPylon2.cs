using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraPylon2 : Camera, IDigitalIo
    {
        public delegate void CameraOpenedEventDelegate();
        public delegate void CameraOpeningEventDelegate();
        public delegate void CameraClosedEventDelegate();
        public delegate void CameraClosingEventDelegate();
        public delegate void CameraConnectionLostEventDelegate();
        public delegate void GrabStartedEventDelegate();
        public delegate void GrabStartingEventDelegate();
        public delegate void GrabStoppedEventDelegate();
        public delegate void GrabStoppingEventDelegate();
        public delegate void GrabSucceededEventDelegate(Image2D image);
        public delegate void GrabFailedEventDelegate(int errorCode, string errorDescription);


        public CameraPylon2() { }


        public Basler.Pylon.Camera MainCamera { get; private set; }

        private Queue<Basler.Pylon.IGrabResult> GrabResultBuffer { get; set; } = new Queue<Basler.Pylon.IGrabResult>();

        public uint GrabResultBufferSize { get; set; } = 1;

        private Basler.Pylon.IGrabResult LastGrabResult { get; set; } = null;

        public bool IsVirtual => false;

        public event CameraOpenedEventDelegate CameraOpened;

        public event CameraOpeningEventDelegate CameraOpening;

        public event CameraClosedEventDelegate CameraClosed;

        public event CameraClosingEventDelegate CameraClosing;

        public event CameraConnectionLostEventDelegate CameraConnectionLost;

        public event GrabStartedEventDelegate GrabStarted; // 그랩 세션 시작 후

        public event GrabStartingEventDelegate GrabStarting; // 그랩 세션 시작 전

        public event GrabStoppedEventDelegate GrabStopped; // 그랩 세션 중단 후

        public event GrabStoppingEventDelegate GrabStopping; // 그랩 세션 중단 전

        public event GrabSucceededEventDelegate GrabSucceeded; // 그랩 성공

        public new event GrabFailedEventDelegate GrabFailed; // 실패하면 GrabFailed 가 호출됨.

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Pylon Camera");

            base.Initialize(cameraInfo);

            var cameraInfoPylon = (CameraInfoPylon2)cameraInfo;

            string msg = $"Open pylon camera - Device User Id : {cameraInfoPylon.DeviceUserId} / IP Address : {cameraInfoPylon.IpAddress}, Serial No : {cameraInfoPylon.SerialNo}";
            LogHelper.Debug(LoggerType.StartUp, msg);

            try
            {
                IDictionary<string, string> cameraInfoMap = new Dictionary<string, string>();
                if (cameraInfoPylon.DeviceUserId != "")
                {
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.UserDefinedName] = cameraInfoPylon.DeviceUserId;
                }

                if (cameraInfoPylon.SerialNo != "")
                {
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.SerialNumber] = cameraInfoPylon.SerialNo;
                }

                if (cameraInfoPylon.IpAddress != "")
                {
                    cameraInfoMap[Basler.Pylon.CameraInfoKey.DeviceIpAddress] = cameraInfoPylon.IpAddress;
                }

                MainCamera = new Basler.Pylon.Camera(cameraInfoPylon.SerialNo);

                AddEvent(MainCamera);

                MainCamera.Open(2000, Basler.Pylon.TimeoutHandling.ThrowException);
                if (cameraInfoPylon.AutoDetectMode)
                {
                    cameraInfoPylon.MaxSize = new Size((int)MainCamera.Parameters[PLCamera.WidthMax].GetValue(), (int)MainCamera.Parameters[PLCamera.HeightMax].GetValue());
                    cameraInfo.Width = cameraInfoPylon.MaxSize.Width;
                    cameraInfo.Height = cameraInfoPylon.MaxSize.Height;
                    ImageSize = cameraInfo.Size;
                    cameraInfoPylon.AutoDetectMode = false;
                }

                NumOfBand = cameraInfo.GetNumBand();


                SetupImageFormat();

                UpdateBuffer();

                MainCamera.Parameters[PLCamera.ExposureMode].TrySetValue(cameraInfoPylon.ExpouserMode.ToString());
                MainCamera.Parameters[PLCamera.ExposureAuto].TrySetValue(cameraInfoPylon.ExpouserAuto.ToString());

                SetTrigger(cameraInfoPylon.FrameStart);
                SetTrigger(cameraInfoPylon.AcquisitionStart);

                //MainCamera.Parameters["TriggerSelector"].ParseAndSetValue("AcquisitionStart");
                //MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
                //MainCamera.Parameters["TriggerSource"].ParseAndSetValue("Software");

                SetDigitalIOControl(cameraInfoPylon.DIO1);
                SetDigitalIOControl(cameraInfoPylon.DIO3);

                //SetAnalogControls(cameraInfoPylon.AnalogControls);
                //SetImageFormatControls(cameraInfoPylon.ImageFormatControls);
                //SetAOIControls(cameraInfoPylon.AOIControls);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Can't open camera. Device User Id : {0} / IP Address : {1}, Serial No : {2} / Message : {3} ",
                    cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, e.Message));
                return;
            }
        }

        public void TriggerOn()
        {
            if (!MainCamera.StreamGrabber.IsGrabbing)
            {
                return;
            }

            MainCamera.ExecuteSoftwareTrigger();
        }

        public override void UpdateBuffer()
        {
            SetImageSize((uint)CameraInfo.Width, (uint)CameraInfo.Height, 0, 0);
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            MainCamera.Parameters["TriggerSelector"].ParseAndSetValue("AcquisitionStart");
            if (triggerMode == TriggerMode.Software)
            {
                MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
            }
            else
            {
                MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");
            }
        }

        public void SetDigitalIOControl(IoControlStruct ioControlStruct)
        {
            MainCamera.Parameters["LineSelector"].ParseAndSetValue(ioControlStruct.Name);
            MainCamera.Parameters["LineMode"].ParseAndSetValue(ioControlStruct.LineMode.ToString());
            if (ioControlStruct.LineMode == IoControlStruct.ELineMode.Output)
            {
                MainCamera.Parameters["LineSource"].ParseAndSetValue(ioControlStruct.OutputLineSource.ToString());
            }

            MainCamera.Parameters["LineInverter"].ParseAndSetValue((ioControlStruct.InverterMode ? 1 : 0).ToString());
        }

        public void SetFrequencyConverter(FrequencyConverterStruct frequencyConverterStruct)
        {
            MainCamera.Parameters["FrequencyConverterInputSource"].ParseAndSetValue(frequencyConverterStruct.Source.ToString());
            MainCamera.Parameters["FrequencyConverterSignalAlignment"].ParseAndSetValue(frequencyConverterStruct.Alignment.ToString());
            MainCamera.Parameters["FrequencyConverterPreDivider"].ParseAndSetValue(frequencyConverterStruct.PreDivider.ToString());
            MainCamera.Parameters["FrequencyConverterMultiplier"].ParseAndSetValue(frequencyConverterStruct.Multiplier.ToString());
            MainCamera.Parameters["FrequencyConverterPostDivider"].ParseAndSetValue(frequencyConverterStruct.PostDivider.ToString());
        }

        public void SetAnalogControls(AnalogControlsStruct analogControlsStruct)
        {
            string functionName = "SetAnalogControls::";
            string errorMessage = functionName;
            bool res = false;

            res = MainCamera.Parameters[PLCamera.GainAuto].TrySetValue(analogControlsStruct.GainAuto.ToString());
            if (res == false)
            {
                errorMessage += $"[GainAuto Set False({analogControlsStruct.GainAuto.ToString()})]";
            }

            if (analogControlsStruct.GainAuto == AnalogControlsStruct.EGainAuto.Off) //OF일때만 Raw 설정 가능
            {
                res = MainCamera.Parameters[PLCamera.GainRaw].TrySetValue(analogControlsStruct.GainRaw);
                if (res == false)
                {
                    errorMessage += $"[GainRaw Set({analogControlsStruct.GainRaw})False]";
                }
            }

            res = MainCamera.Parameters[PLCamera.GainSelector].TrySetValue(analogControlsStruct.GainSelector);
            if (res == false)
            {
                errorMessage += $"[GainSelector Set({analogControlsStruct.GainSelector})False]";
            }

            res = MainCamera.Parameters[PLCamera.BlackLevelSelector].TrySetValue(analogControlsStruct.BlackLevelSelector);
            if (res == false)
            {
                errorMessage += $"[BlackLevelSelector Set({analogControlsStruct.BlackLevelSelector})False]";
            }

            res = MainCamera.Parameters[PLCamera.BlackLevelRaw].TrySetValue(analogControlsStruct.BlackLevelRaw);
            if (res == false)
            {
                errorMessage += $"[BlackLevelRaw Set({analogControlsStruct.BlackLevelRaw})False]";
            }

            res = MainCamera.Parameters[PLCamera.GammaEnable].TrySetValue(analogControlsStruct.GammaEnable);
            if (res == false)
            {
                errorMessage += $"[GammaEnable Set({analogControlsStruct.GammaEnable})False]";
            }

            res = MainCamera.Parameters[PLCamera.GammaSelector].TrySetValue(analogControlsStruct.GammaSelector.ToString());
            if (res == false)
            {
                errorMessage += $"[GammaSelector Set({analogControlsStruct.GammaSelector.ToString()})False]";
            }

            if (analogControlsStruct.GammaSelector == AnalogControlsStruct.EGammaSelector.User) //User 일때만 Gamma 설정 가능
            {
                res = MainCamera.Parameters[PLCamera.Gamma].TrySetValue(analogControlsStruct.Gamma);
                if (res == false)
                {
                    errorMessage += $"[Gamma Set False({analogControlsStruct.Gamma})]";
                }
            }

            res = MainCamera.Parameters[PLCamera.DigitalShift].TrySetValue(analogControlsStruct.DigitalShift);
            if (res == false)
            {
                errorMessage += $"[DigitalShift Set({analogControlsStruct.DigitalShift})False]";
            }

            if (errorMessage != functionName)
            {
                System.Diagnostics.Trace.WriteLine(errorMessage);
            }
        }

        public void SetImageFormatControls(ImageFormatControls imageFormatControls)
        {
            string functionName = "ImageFormatControls::";
            string errorMessage = functionName;
            bool res = false;

            res = MainCamera.Parameters[PLCamera.PixelFormat].TrySetValue(imageFormatControls.PixelFormat.ToString());
            if (res == false)
            {
                errorMessage += $"[PixelFormat Set False({imageFormatControls.PixelFormat.ToString()})]";
            }

            res = MainCamera.Parameters[PLCamera.ReverseX].TrySetValue(imageFormatControls.ReverseX);
            if (res == false)
            {
                errorMessage += $"[ReverseX Set False({imageFormatControls.ReverseX.ToString()})]";
            }

            res = MainCamera.Parameters[PLCamera.ReverseY].TrySetValue(imageFormatControls.ReverseY);
            if (res == false)
            {
                errorMessage += $"[ReverseY Set False({imageFormatControls.ReverseY.ToString()})]";
            }

            res = MainCamera.Parameters[PLCamera.TestImageSelector].TrySetValue(imageFormatControls.TestImageSelector.ToString());
            if (res == false)
            {
                errorMessage += $"[TestImageSelector Set False({imageFormatControls.TestImageSelector.ToString()})]";
            }

            res = MainCamera.Parameters[PLCamera.TestImageResetAndHold].TrySetValue(imageFormatControls.TestImageResetAndHold);
            if (res == false)
            {
                errorMessage += $"[TestImageResetAndHold Set False({imageFormatControls.TestImageResetAndHold.ToString()})]";
            }

            if (errorMessage != functionName)
            {
                System.Diagnostics.Trace.WriteLine(errorMessage);
            }
        }

        public void SetAOIControls(AOIControls aoiControls)
        {
            string functionName = "AOIControls::";
            string errorMessage = functionName;
            bool res = false;

            //Binning보다 Width 셋팅을 먼저하면 Binning 값이 1을 초과할 경우 Width를 나눠버림.
            res = MainCamera.Parameters[PLCamera.BinningHorizontalMode].TrySetValue(aoiControls.BinningHorizontalMode.ToString());
            if (res == false)
            {
                errorMessage += $"[BinningHorizontalMode Set False({aoiControls.BinningHorizontalMode.ToString()})]";
            }

            //Binning보다 Height 셋팅을 먼저하면 Binning 값이 1을 초과할 경우 Height 나눠버림.
            res = MainCamera.Parameters[PLCamera.BinningHorizontal].TrySetValue(aoiControls.BinningHorizontal);
            if (res == false)
            {
                errorMessage += $"[BinningHorizontal Set False({aoiControls.BinningHorizontal})]";
            }

            res = MainCamera.Parameters[PLCamera.BinningVerticalMode].TrySetValue(aoiControls.BinningVerticalMode.ToString());
            if (res == false)
            {
                errorMessage += $"[BinningVerticalMode Set False({aoiControls.BinningVerticalMode.ToString()})]";
            }

            res = MainCamera.Parameters[PLCamera.BinningVertical].TrySetValue(aoiControls.BinningVertical);
            if (res == false)
            {
                errorMessage += $"[BinningVertical Set False({aoiControls.BinningVertical})]";
            }

            int heightMax = int.Parse(MainCamera.Parameters[PLCamera.HeightMax].ToString());
            int widthMax = int.Parse(MainCamera.Parameters[PLCamera.WidthMax].ToString());
            int curOffsetX = int.Parse(MainCamera.Parameters[PLCamera.OffsetX].ToString());
            int curOffsetY = int.Parse(MainCamera.Parameters[PLCamera.OffsetY].ToString());

            //카메라가 사용할 수 있는 길이를 초과하도록 Width, Height를 설정한 경우 사용할 수 있는 최대 크기로 정한다.
            int setWidth = aoiControls.Width < (widthMax - curOffsetX) ? aoiControls.Width : (widthMax - curOffsetX);
            int setHeight = aoiControls.Height < (heightMax - curOffsetY) ? aoiControls.Height : (heightMax - curOffsetY);

            res = MainCamera.Parameters[PLCamera.Width].TrySetValue(setWidth);
            if (res == false)
            {
                errorMessage += $"[Width Set False({setWidth})]";
            }

            res = MainCamera.Parameters[PLCamera.Height].TrySetValue(setHeight);
            if (res == false)
            {
                errorMessage += $"[Height Set False({setHeight})]";
            }

            res = MainCamera.Parameters[PLCamera.OffsetX].TrySetValue(aoiControls.XOffset);
            if (res == false)
            {
                errorMessage += $"[XOffset Set False({aoiControls.XOffset})]";
            }

            res = MainCamera.Parameters[PLCamera.OffsetY].TrySetValue(aoiControls.YOffset);
            if (res == false)
            {
                errorMessage += $"[YOffset Set False({aoiControls.YOffset})]";
            }

            res = MainCamera.Parameters[PLCamera.CenterX].TrySetValue(aoiControls.CenterX);
            if (res == false)
            {
                errorMessage += $"[CenterX Set False({aoiControls.CenterX})]";
            }

            res = MainCamera.Parameters[PLCamera.CenterY].TrySetValue(aoiControls.CenterY);
            if (res == false)
            {
                errorMessage += $"[CenterY Set False({aoiControls.CenterY})]";
            }

            res = MainCamera.Parameters[PLCamera.Width].TrySetValue(aoiControls.Width);
            if (res == false)
            {
                errorMessage += $"[Width Set False({aoiControls.Width})]";
            }

            res = MainCamera.Parameters[PLCamera.Height].TrySetValue(aoiControls.Height);
            if (res == false)
            {
                errorMessage += $"[Height Set False({aoiControls.Height})]";
            }

            if (errorMessage != functionName)
            {
                System.Diagnostics.Trace.WriteLine(errorMessage);
            }
        }

        public void SetTrigger(TriggerStruct triggerStruct)
        {
            try
            {
                MainCamera.Parameters[PLCamera.TriggerSelector].SetValue(triggerStruct.Name);
                MainCamera.Parameters[PLCamera.TriggerMode].SetValue(triggerStruct.Mode.ToString());
                MainCamera.Parameters[PLCamera.TriggerSource].SetValue(triggerStruct.Source.ToString());
                MainCamera.Parameters[PLCamera.TriggerActivation].SetValue(triggerStruct.Activation.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Device, $"CameraPylon2::SetTrigger - Exception in {triggerStruct.Name} - {ex.Message}");
            }
        }

        public void SetAcquisitionFrameRate(float acquisitionFrameRate, bool enable)
        {
            MainCamera.Parameters["AcquisitionFrameRateAbs"].ParseAndSetValue(acquisitionFrameRate.ToString());
            MainCamera.Parameters["AcquisitionFrameRateEnable"].ParseAndSetValue(enable ? "1" : "0");
        }

        public override void SetAcquisitionLineRate(float grabHz)
        {
            base.SetAcquisitionLineRate(grabHz);

            MainCamera.Parameters["AcquisitionLineRateAbs"].ParseAndSetValue(grabHz.ToString());
        }

        public void SetLineDebouncerTimeAbs(float lineDebouncerTimeAbs)
        {
            MainCamera.Parameters["LineDebouncerTimeAbs"].ParseAndSetValue(lineDebouncerTimeAbs.ToString());
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            MainCamera.Parameters["TriggerDelay"].ParseAndSetValue(triggerDelayUs.ToString());
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            int exposureTime = (int)((exposureTimeMs * 1000) / 35) * 35;
            MainCamera.Parameters["ExposureTimeRaw"].ParseAndSetValue(exposureTime.ToString());
        }

        public override void SetGain(float gain)
        {
            MainCamera.Parameters["Gain"].ParseAndSetValue(((int)gain).ToString());
        }

        public void SetImageSize(uint width, uint height, uint offsetX, uint offsetY)
        {
            uint widthMax = uint.Parse(MainCamera.Parameters["WidthMax"].ToString());
            uint heightMax = uint.Parse(MainCamera.Parameters["HeightMax"].ToString());
            if (width > widthMax)
            {
                width = widthMax;
            }

            if (height > heightMax)
            {
                height = heightMax;
            }

            if (offsetX > widthMax - width)
            {
                offsetX = widthMax - width;
            }

            if (offsetY > heightMax - height)
            {
                offsetY = heightMax - height;
            }

            MainCamera.Parameters["Width"].ParseAndSetValue(width.ToString());
            MainCamera.Parameters["Height"].ParseAndSetValue(height.ToString());
            MainCamera.Parameters["OffsetX"].ParseAndSetValue(offsetX.ToString());
            MainCamera.Parameters["OffsetY"].ParseAndSetValue(offsetY.ToString());
        }

        public void SetImagecenter(bool centerX, bool centerY)
        {
            MainCamera.Parameters["CenterX"].ParseAndSetValue(centerX.ToString());
            MainCamera.Parameters["CenterY"].ParseAndSetValue(centerY.ToString());
        }

        private void SetupImageFormat()
        {
            int width = int.Parse(MainCamera.Parameters["Width"].ToString());
            int height = int.Parse(MainCamera.Parameters["Height"].ToString());
            string pixelFormat = MainCamera.Parameters["PixelFormat"].ToString();
            ImageSize = new Size(width, height);
            NumOfBand = pixelFormat == "Mono8" ? 1 : 3;
            ImagePitch = width * NumOfBand;
            LogHelper.Debug(LoggerType.Grab, string.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", width, height, ImagePitch, pixelFormat));
        }

        /*
         * 버퍼에서 가장 최근 이미지를 반환함.
         */
        public override ImageD GetGrabbedImage()
        {
            var image = new Image2D();
            lock (GrabResultBuffer)
            {
                if (LastGrabResult == null)
                {
                    LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : 버퍼에 이미지가 들어온 적이 없음.\n"));
                }
                else
                {
                    GrabResultToImage(LastGrabResult, ref image);
                }
            }
            return image;
        }

        private void GrabResultToImage(Basler.Pylon.IGrabResult grabResult, ref Image2D image)
        {
            switch (grabResult.PixelTypeValue)
            {
                case Basler.Pylon.PixelType.Mono8:
                    image.Initialize(grabResult.Width, grabResult.Height, 1);
                    break;
                default:
                    break;
            }
            image.SetData(grabResult.PixelData as byte[]);
        }

        /*
         * 한 번 그랩 (동기)
         */
        public ImageD GrabOnceSync()
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot (Sync)", Index));

            try
            {
                if (MainCamera.StreamGrabber.IsGrabbing)
                {
                    throw new Exception("비동기 그랩이 실행중임.");
                }

                IGrabResult grabResult = MainCamera.StreamGrabber.GrabOne(500, Basler.Pylon.TimeoutHandling.ThrowException);
                var image = new Image2D();
                switch (grabResult.PixelTypeValue)
                {
                    case Basler.Pylon.PixelType.Mono8:
                        image.Initialize(grabResult.Width, grabResult.Height, 1);
                        break;
                    default:
                        break;
                }
                byte[] rawImage = grabResult.PixelData as byte[];
                image.SetData(rawImage);
                return image;
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : {0}\n", e.Message));
                return new Image2D();
            }
        }

        /*
         * 한 번 그랩 (비동기)
         * 그랩 완료시 
         */
        public override void GrabOnceAsync()
        {
            if (MainCamera.StreamGrabber.IsGrabbing)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot (Async)", Index));

            try
            {
                MainCamera.StreamGrabber.Start(1, Basler.Pylon.GrabStrategy.LatestImages, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : {0}\n", e.Message));
            }
        }

        /*
         * 프레임 지정 그랩 (비동기)
         * 그랩 완료시 
         */
        public void GrabOnceAsync(uint grabCount)
        {
            if (grabCount == 0)
            {
                return;
            }

            if (MainCamera.StreamGrabber.IsGrabbing)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot (Async)", Index));

            try
            {
                MainCamera.StreamGrabber.Start(grabCount, Basler.Pylon.GrabStrategy.LatestImages, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : {0}\n", e.Message));
            }
        }

        /*
         * 연속 그랩
         */
        public override void GrabMulti()
        {
            if (MainCamera.StreamGrabber.IsGrabbing)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Continuous Shot", Index));

            try
            {
                MainCamera.StreamGrabber.Start(Basler.Pylon.GrabStrategy.LatestImages, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Continuous Shot Error : {0}\n", e.Message));
            }
        }

        /*
         * 그랩 완료 여부
         */
        public override bool IsGrabDone()
        {
            return !MainCamera.StreamGrabber.IsGrabbing;
        }

        /*
         * 그랩 중지
         */
        public override void Stop()
        {
            //if (MainCamera.StreamGrabber.IsGrabbing)
            //    return;

            LogHelper.Debug(LoggerType.Grab, string.Format("Stop Continuous {0}", Index));
            MainCamera?.StreamGrabber.Stop();
        }

        /*
         * 카메라 닫기
         */
        public override void Release()
        {
            base.Release();
            Stop();
            MainCamera?.Close();
        }

        private void AddEvent(Basler.Pylon.Camera mainCamera)
        {
            mainCamera.CameraOpened += OnCameraOpened;
            mainCamera.CameraOpening += OnCameraOpening;
            mainCamera.CameraClosed += OnCameraClosed;
            mainCamera.CameraClosing += OnCameraClosing;
            mainCamera.ConnectionLost += OnCameraConnectionLost;
            mainCamera.StreamGrabber.GrabStarted += OnGrabStarted;
            mainCamera.StreamGrabber.GrabStarting += OnGrabStarting;
            mainCamera.StreamGrabber.GrabStopped += OnGrabStopped;
            mainCamera.StreamGrabber.GrabStopping += OnGrabStopping;
            mainCamera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
        }

        private void OnCameraOpened(object sender, EventArgs e)
        {
            CameraOpened?.Invoke();
        }

        private void OnCameraOpening(object sender, EventArgs e)
        {
            CameraOpening?.Invoke();
        }

        private void OnCameraClosed(object sender, EventArgs e)
        {
            CameraClosed?.Invoke();
        }

        private void OnCameraClosing(object sender, EventArgs e)
        {
            CameraClosing?.Invoke();
        }

        private void OnCameraConnectionLost(object sender, EventArgs e)
        {
            CameraConnectionLost?.Invoke();
        }

        private void OnGrabStarted(object sender, EventArgs e)
        {
            GrabStarted?.Invoke();
        }

        private void OnGrabStarting(object sender, EventArgs e)
        {
            GrabStarting?.Invoke();
        }

        private void OnGrabStopped(object sender, EventArgs e)
        {
            GrabStopped?.Invoke();
        }

        private void OnGrabStopping(object sender, EventArgs e)
        {
            GrabStopping?.Invoke();
        }

        private void OnImageGrabbed(object sender, Basler.Pylon.ImageGrabbedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] - ImageNumber: {e.GrabResult.ImageNumber}");
            // 2019.05.29 mhcho - Grab 실패 여부에 관계없이 이미지가 들어옴.....
            var image = new Image2D();
            //lock (GrabResultBuffer)
            {
                LastGrabResult = e.GrabResult.Clone();
                if (GrabResultBuffer.Count >= GrabResultBufferSize)
                {
                    GrabResultBuffer.Dequeue();
                }

                GrabResultBuffer.Enqueue(LastGrabResult);
                GrabResultToImage(LastGrabResult, ref image);
                ImageGrabbed?.Invoke(this);
            }

            if (e.GrabResult.GrabSucceeded)
            {
                GrabSucceeded?.Invoke(image);
            }
            else
            {
                GrabFailed?.Invoke(e.GrabResult.ErrorCode, e.GrabResult.ErrorDescription);
            }
        }

        public string GetName()
        {
            return Name;
        }

        public int GetNumInPortGroup()
        {
            return 1;
        }

        public int GetNumOutPortGroup()
        {
            return 1;
        }

        public int GetInPortStartGroupIndex()
        {
            return 0;
        }

        public int GetOutPortStartGroupIndex()
        {
            return 0;
        }

        public int GetNumInPort()
        {
            return 1;
        }

        public int GetNumOutPort()
        {
            return 1;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public bool IsInitialized()
        {
            return true;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            MainCamera.Parameters["UserOutputValue"].ParseAndSetValue(outputPortStatus == 1 ? "True" : "False");
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public uint ReadOutputGroup(int groupNo)
        {
            bool userOutputValue = bool.Parse(MainCamera.Parameters["UserOutputValue"].ToString());
            return userOutputValue ? (uint)1 : 0;
        }

        public uint ReadInputGroup(int groupNo)
        {
            return 0;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
