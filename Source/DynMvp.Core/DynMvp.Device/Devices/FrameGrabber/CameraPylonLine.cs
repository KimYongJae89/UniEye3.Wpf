using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoPylonBufferTag : CameraBufferTag
    {
        public CameraInfoPylonBufferTag(int bufferId, ulong frameId, Size frameSize) : base(bufferId, frameId, 0, frameSize) { }

        public override string ToString()
        {
            return string.Format("Buffer {0} / Frame {1}", BufferId, FrameId);
        }
    }

    //public enum TriggerSelector
    //{
    //    AcquisitionStart, FrameStart
    //}

    //public enum LineSelector
    //{
    //    Line1, Line2, Line3
    //}

    //public enum TriggerSource
    //{
    //    Line1, Line2, Line3
    //}

    //public enum LineMode
    //{
    //    Input, Output
    //}

    //public enum LineSource
    //{
    //    ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2
    //}

    /// <summary>
    /// Line Scan용으로 제작됨.
    /// raL2048-48gm 테스트 완료, Area는 테스트한적 없음, 
    /// ColorSensor, PinholeSensor, 파스용
    /// </summary>
    public class CameraPylonLine : Camera, IDigitalIo
    {
        public delegate void CameraOpenedEventDelegate();
        public delegate void CameraOpeningEventDelegate();
        public delegate void CameraClosedEventDelegate();
        public delegate void CameraClosingEventDelegate();
        public delegate void CameraConnectionLostEventDelegate(CameraInfo cameraInfo);
        public delegate void GrabStartedEventDelegate();
        public delegate void GrabStartingEventDelegate();
        public delegate void GrabStoppedEventDelegate();
        public delegate void GrabStoppingEventDelegate();
        public delegate void GrabSucceededEventDelegate(Image2D image);
        public delegate void GrabFailedEventDelegate(int errorCode, string errorDescription);

        public Basler.Pylon.Camera MainCamera { get; private set; }

        private List<Basler.Pylon.IGrabResult> GrabResultBuffer { get; set; } = new List<Basler.Pylon.IGrabResult>();

        public uint GrabResultBufferSize { get; set; } = 50; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!꼭봐라 !!!!

        private Basler.Pylon.IGrabResult LastGrabResult { get; set; } = null;

        bool IDigitalIo.IsVirtual => throw new NotImplementedException();

        public event CameraOpenedEventDelegate CameraOpened;
        public event CameraOpeningEventDelegate CameraOpening;
        public event CameraClosedEventDelegate CameraClosed;
        public event CameraClosingEventDelegate CameraClosing;
        public event CameraConnectionLostEventDelegate CameraConnectionLost;
        public event GrabStartedEventDelegate GrabStarted; // 그랩 세션 시작 후
        public event GrabStartingEventDelegate GrabStarting; // 그랩 세션 시작 전
        public event GrabStoppedEventDelegate GrabStopped; // 그랩 세션 중단 후
        public event GrabStoppingEventDelegate GrabStopping; // 그랩 세션 중단 전
        //public event GrabSucceededEventDelegate GrabSucceeded; // 그랩 성공
        public new event GrabFailedEventDelegate GrabFailed; // 실패하면 GrabFailed 가 호출됨.

        public CameraPylonLine() : base() { base.Name = "CameraPylonLine"; }
        public CameraPylonLine(CameraInfo cameraInfo) : base() { base.Name = "CameraPylonLine"; }

        protected ManualResetEvent isWholeGrabDone = new ManualResetEvent(false); //N장의 이미지 모두 받음.

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize PylonLine Camera");
            this.cameraInfo = cameraInfo;
            var cameraInfoPylonLine = cameraInfo as CameraInfoPylonLine;

            LogHelper.Debug(LoggerType.StartUp, string.Format("Open PylonLine camera - Device Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3}",
                cameraInfoPylonLine.DeviceIndex, cameraInfoPylonLine.DeviceUserId, cameraInfoPylonLine.IpAddress, cameraInfoPylonLine.SerialNo));

            try
            {
                MainCamera = new Basler.Pylon.Camera(cameraInfoPylonLine.SerialNo);

                MainCamera.CameraOpened += OnCameraOpened;
                MainCamera.CameraOpening += OnCameraOpening;
                MainCamera.CameraClosed += OnCameraClosed;
                MainCamera.CameraClosing += OnCameraClosing;
                MainCamera.ConnectionLost += OnCameraConnectionLost;
                MainCamera.StreamGrabber.GrabStarted += OnGrabStarted;
                MainCamera.StreamGrabber.GrabStarting += OnGrabStarting;
                MainCamera.StreamGrabber.GrabStopped += OnGrabStopped;
                MainCamera.StreamGrabber.GrabStopping += OnGrabStopping;
                MainCamera.StreamGrabber.ImageGrabbed += OnImageGrabbed;

                //open camera
                MainCamera.Open(5000, Basler.Pylon.TimeoutHandling.ThrowException);

                MainCamera.Parameters[PLCamera.AcquisitionMode].ParseAndSetValue("Continuous");


                SetupImageFormat(cameraInfoPylonLine.UpdateDeviceFeature);
                cameraInfo.Width = ImageSize.Width;
                cameraInfo.Height = ImageSize.Height;
                cameraInfo.SetNumBand(NumOfBand);

                MainCamera.Parameters[PLCamera.ExposureMode].ParseAndSetValue("Timed");
                MainCamera.Parameters[PLCamera.ExposureAuto].ParseAndSetValue("Off");

                MainCamera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(GrabResultBufferSize);

                //FlipX
                MainCamera.Parameters[PLCamera.ReverseX].SetValue(this.cameraInfo.MirrorX);

                // Set trigger
                SetTrigger(new TriggerStruct("AcquisitionStart", TriggerStruct.EMode.Off));
                SetTrigger(cameraInfoPylonLine.LineTriggerStruct);
                SetTrigger(cameraInfoPylonLine.FrameTriggerStruct, cameraInfoPylonLine.TriggerPartialClosingFrame);

                // Set DIO
                SetDigitalIo(cameraInfoPylonLine.DIO1);
                SetDigitalIo(cameraInfoPylonLine.DIO2);
                SetDigitalIo(cameraInfoPylonLine.DIO3);

                // Set FQ.Converter
                SetFrequencyConverter(cameraInfoPylonLine.FrequencyConverter);

                if (MainCamera.Parameters["ChunkModeActive"].IsWritable)// Activate the chunk mode.
                {
                    MainCamera.Parameters[PLCamera.ChunkModeActive].SetValue(cameraInfoPylonLine.UseChunkMode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, "Pylon Exception - {0}", new object[] { ex.Message }, "");
                //string message = "Can't open camera. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} / Message : {4} ";
                //string[] args = new string[] { cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo };
                //throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, message, args, "");
            }

        }

        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            //Debug.WriteLine("CameraPylonLine.OnImageGrabbed().....................");
            try
            {
                IGrabResult _grabResult;
                if (e.GrabResult.GrabSucceeded)
                {
                    lock (GrabResultBuffer)
                    {
                        _grabResult = e.GrabResult.Clone(); //얉은복사, 데이터는 그냥 참조되며, 복사객체는 디스포즈되야함.

                        if (GrabResultBuffer.Count > GrabResultBufferSize)
                        {
                            IGrabResult taken = GrabResultBuffer[0];
                            GrabResultBuffer.Remove(taken);
                            taken.Dispose();
                            Debug.WriteLine("CameraPylonLine.Camera Buffer has overrun !!!!!!!!!!!!!!!!");
                        }

                        GrabResultBuffer.Add(_grabResult); //유효범위가 카메라에 세팅한 버퍼 사이즈임. 맞는지 확인필요.
                    }

                    var ptr = (IntPtr)_grabResult.BlockID; //GrabResult.ID;
                    //ID: 콜백 순서로 번호가 부여되며, 프로그램을 재시작 됐을때 리셋됨
                    //BlockID: 카메라에서 그랩한 프래임 번호이며, 다시 그랩하면 리셋됨, 만일 번호가 연속적이지 않다면 버퍼 오버런, 언더런 난것임=> 즉 잘못된것임.
                    //Debug.WriteLine($"CameraPylonLine.OnImageGrabbed. (Cam: {this.index}, ID:{_grabResult.ID}, BlockID:{_grabResult.BlockID})");
                    base.ImageGrabbedCallback();
                    if (base.remainGrabCount == 0) // 전체 그랩 (1~N장) 완료시.
                    {
                        isWholeGrabDone.Set();
                        //Debug.WriteLine(string.Format("CameraPylonLine.OnImageGrabbed() >> isWholeGrabDone.Set() "));
                    }
                    //GrabResult.Dispose();
                }
                else
                {
                    GrabFailed?.Invoke(e.GrabResult.ErrorCode, e.GrabResult.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        private Image2D PopImageFromBuffer(long imageID) //ID는 1부터 시작 됨, 0이며 마지막 이미지 리턴
        {
            Image2D imageD = null;
            lock (GrabResultBuffer)
            {
                IGrabResult grabResult = null;
                if (imageID == 0)
                {
                    if (GrabResultBuffer.Count > 0)
                    {
                        grabResult = GrabResultBuffer[GrabResultBuffer.Count - 1];
                    }
                }
                else
                {
                    grabResult = GrabResultBuffer.Find(a => a.BlockID == imageID);
                }

                if (grabResult == null && LastGrabResult != null)
                {
                    grabResult = LastGrabResult.Clone();
                }

                if (grabResult != null)
                {
                    Size frameSize = Size.Empty;
                    switch (grabResult.PixelTypeValue)
                    {
                        case Basler.Pylon.PixelType.Mono8:
                            imageD = new Image2D();
                            //imageD.Initialize(grabResult.Width, grabResult.Height, 1);
                            imageD.Initialize(ImageSize.Width, ImageSize.Height, 1);
                            var payloadSize = new Size(grabResult.Width, (int)(grabResult.PayloadSize / grabResult.Width));
                            var validSize = new Size(grabResult.Width, grabResult.Height);
                            Debug.Assert(payloadSize.Height == validSize.Height);
                            frameSize = validSize;
                            break;
                        default:
                            break;
                    }

                    if (grabResult.PayloadTypeValue == PayloadType.ChunkData)
                    {
                        //청크데이터의 경우 실제 이미지정보 + 추가정보가 PixelData에 포함되어 있어서 이미지정보 맞게 넣어줘야 된다.
                        if (grabResult.PixelData is byte[] pixelData)
                        {
                            byte[] bufferData = new byte[grabResult.Width * grabResult.Height];
                            Array.Copy(pixelData, bufferData, bufferData.Length);
                            imageD.SetData(bufferData);
                        }
                        if (imageD != null)
                        {
                            int w = 0, h = 0;
                            if (grabResult.ChunkData[PLChunkData.ChunkWidth].IsReadable)
                            {
                                w = (int)grabResult.ChunkData[PLChunkData.ChunkWidth].GetValue();
                            }

                            if (grabResult.ChunkData[PLChunkData.ChunkHeight].IsReadable)
                            {
                                h = (int)grabResult.ChunkData[PLChunkData.ChunkHeight].GetValue();
                            }

                            //imageD.Tag = new CameraInfoPylonBufferTag((UInt64)grabResult.ID, new Size(w, h), (int)grabResult.BlockID);
                            imageD.Tag = new CameraInfoPylonBufferTag(0, (ulong)grabResult.ImageNumber - 1, new Size(w, h));
                            //Debug.WriteLine(string.Format("♠ ChunkWidth : {0}, ChunkHeight : {1})", w, h));
                            Debug.WriteLine($"ID: {grabResult.ID}, BlockID: {grabResult.BlockID}, ImageNumber: {grabResult.ImageNumber}");
                        }
                    }
                    else
                    {
                        imageD.SetData(grabResult.PixelData as byte[]);
                        imageD.Tag = new CameraBufferTag(0, (ulong)grabResult.ImageNumber - 1, (ulong)grabResult.Timestamp, frameSize);
                    }


                    GrabResultBuffer.Remove(grabResult);

                    LastGrabResult?.Dispose(); // 반드시 디스포즈 해줘야 카메라 내부에서 언더런이 안남...
                    LastGrabResult = grabResult;
                }
            }
            return imageD;
        }

        private void OnGrabStopping(object sender, GrabStopEventArgs e)
        {
            GrabStopping?.Invoke();
        }

        private void OnGrabStopped(object sender, GrabStopEventArgs e)
        {
            Debug.WriteLine($"CameraPylonLine.OnGrabStopped()  >>>>>>>>>>>>>>>>>>>>> {index}");
            GrabStopped?.Invoke();
            //           Started = false;
        }

        private void OnGrabStarting(object sender, EventArgs e)
        {
            GrabStarting?.Invoke();
        }

        private void OnGrabStarted(object sender, EventArgs e)
        {
            Debug.WriteLine($"CameraPylonLine.OnGrabStarted()  >>>>>>>>>>>>>>>>>>>>> {index}");
            GrabStarted?.Invoke();
        }

        private void OnCameraConnectionLost(object sender, EventArgs e)
        {
            var cameraInfoPylonLine = CameraInfo as CameraInfoPylonLine;
            string message = string.Format("Pylon camera connection lost. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} ",
                    cameraInfoPylonLine.DeviceIndex, cameraInfoPylonLine.DeviceUserId, cameraInfoPylonLine.IpAddress, cameraInfoPylonLine.SerialNo);
            CameraConnectionLost?.Invoke(CameraInfo);
            //LogHelper.Error(message);
            //ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.InvalidState, 
            //ErrorLevel.Warning, ErrorSection.Grabber.ToString(), CommonError.InvalidState.ToString(), message);
        }

        private void OnCameraClosing(object sender, EventArgs e)
        {
            CameraClosing?.Invoke();
        }

        private void OnCameraClosed(object sender, EventArgs e)
        {
            CameraClosed?.Invoke();
        }

        private void OnCameraOpening(object sender, EventArgs e)
        {
            CameraOpening?.Invoke();
        }

        private void OnCameraOpened(object sender, EventArgs e)
        {
            CameraOpened?.Invoke();
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            //MainCamera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
            //if (triggerMode == TriggerMode.Software)
            //    MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
            //else
            //    MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");

            if (cameraInfo.IsLineScan)
            {
                MainCamera.Parameters[PLCamera.TriggerSelector].SetValue("LineStart");
                if (triggerMode == TriggerMode.Software)
                {
                    MainCamera.Parameters["TriggerMode"].ParseAndSetValue("Off");
                }
                else
                {
                    MainCamera.Parameters["TriggerMode"].ParseAndSetValue("On");
                }
            }
            MainCamera.Parameters["TriggerActivation"].ParseAndSetValue(triggerType == TriggerType.RisingEdge ? "RisingEdge" : "FallingEdge");
        }

        private void SetTrigger(TriggerStruct triggerStruct)
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
                LogHelper.Error(LoggerType.Device, $"CameraPylonLine::SetTrigger - Exception in {triggerStruct.Name} - {ex.Message}");
            }
        }

        private void SetTrigger(TriggerStruct triggerStruct, bool partialClosing)
        {
            try
            {
                SetTrigger(triggerStruct);

                if (MainCamera.Parameters[PLCamera.TriggerPartialClosingFrame].IsWritable)
                {
                    MainCamera.Parameters[PLCamera.TriggerPartialClosingFrame].SetValue(partialClosing);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Device, $"CameraPylonLine::SetTrigger - Exception in {triggerStruct.Name} - {ex.Message}");
            }
        }


        private void SetDigitalIo(IoControlStruct ioControlStruct)
        {
            try
            {
                MainCamera.Parameters[PLCamera.LineSelector].SetValue(ioControlStruct.Name);

                MainCamera.Parameters[PLCamera.LineMode].SetValue(ioControlStruct.LineMode.ToString());
                if (ioControlStruct.LineMode == IoControlStruct.ELineMode.Output)
                {
                    MainCamera.Parameters[PLCamera.LineSource].SetValue(ioControlStruct.OutputLineSource.ToString());
                }

                MainCamera.Parameters[PLCamera.LineInverter].SetValue(ioControlStruct.InverterMode);
                MainCamera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(ioControlStruct.DebouncerUs);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Device, $"CameraPylonLine::SetDigitalIo - Exception in {ioControlStruct.Name} / Message: {ex.Message}");
            }
        }

        private void SetFrequencyConverter(FrequencyConverterStruct frequencyConverterStruct)
        {
            MainCamera.Parameters[PLCamera.FrequencyConverterInputSource].SetValue(frequencyConverterStruct.Source.ToString());
            MainCamera.Parameters[PLCamera.FrequencyConverterSignalAlignment].SetValue(frequencyConverterStruct.Alignment.ToString());
            MainCamera.Parameters[PLCamera.FrequencyConverterPreDivider].SetValue(frequencyConverterStruct.PreDivider);
            MainCamera.Parameters[PLCamera.FrequencyConverterMultiplier].SetValue(frequencyConverterStruct.Multiplier);
            MainCamera.Parameters[PLCamera.FrequencyConverterPostDivider].SetValue(frequencyConverterStruct.PostDivider);
        }


        public override void SetTriggerDelay(int triggerDelayUs)
        {
            //MainCamera.Parameters["TriggerDelay"].ParseAndSetValue(triggerDelayUs.ToString());
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

        private void SetupImageFormat(bool updateToDeviceFeature)
        {
            var imageSize = new Size(cameraInfo.Width, cameraInfo.Height);
            if (updateToDeviceFeature)
            {
                MainCamera.Parameters[PLCamera.Width].TrySetValue(imageSize.Width);
                MainCamera.Parameters[PLCamera.Height].TrySetValue(imageSize.Height);
            }
            else
            {
                imageSize.Width = (int)MainCamera.Parameters[PLCamera.Width].GetValue();
                imageSize.Height = (int)MainCamera.Parameters[PLCamera.Height].GetValue();
            }
            ImageSize = imageSize;

            string imageFormat = MainCamera.Parameters[PLCamera.PixelFormat].GetValue();
            if (imageFormat == "Mono8")
            {
                NumOfBand = 1;
            }
            else
            {
                NumOfBand = 3;
            }

            ImagePitch = imageSize.Width * NumOfBand;

            LogHelper.Debug(LoggerType.Grab, string.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", imageSize.Width, imageSize.Height, ImagePitch, imageFormat));
        }

        /*
         * 한 번 그랩 (동기)
         */
        public void GrabOnceSync()
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot (Sync)", Index));

            try
            {
                IGrabResult grabResult = MainCamera.StreamGrabber.GrabOne(5000, Basler.Pylon.TimeoutHandling.ThrowException);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("Single Shot Error : {0}\n", e.Message));
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

        //public void GrabOnce() //overlap
        //{
        //    Reset();
        //    base.remainGrabCount = 1;
        //    base.grabbedCount = 0;
        //    //GrabOnceSync();
        //    GrabOnceAsync();
        //}

        private void ClearGrabBuffer()
        {
            GrabResultBuffer.ForEach(d => d.Dispose());
            GrabResultBuffer.Clear();
        }
        public override void GrabMulti()
        {
            if (MainCamera.StreamGrabber.IsGrabbing)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Continuous Shot", Index));

            base.Reset();
            //lock (grabCountLockObj)
            base.remainGrabCount = -1;
            ClearGrabBuffer();

            try
            {
                //if (grabCount == -1)
                MainCamera.StreamGrabber.Start(Basler.Pylon.GrabStrategy.OneByOne, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
                //else
                //    MainCamera.StreamGrabber.Start(grabCount, Basler.Pylon.GrabStrategy.OneByOne, Basler.Pylon.GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("CameraPylonLine.GrabMulti()  Continuous Shot Error : {0}\n", e.Message));
            }
        }
        /*
         * 그랩 중지
         */
        public override void Stop()
        {
            if (MainCamera == null)
            {
                return;
            }

            base.Stop();
            LogHelper.Debug(LoggerType.Grab, string.Format("Stop Continuous {0}", Index));
            MainCamera.StreamGrabber.Stop();
            remainGrabCount = 0;
        }

        /*
         *  해제 
         */
        public override void Release()
        {
            base.Release();
            Stop();
            ClearGrabBuffer();
            MainCamera?.Close();
        }

        public new void Reset()
        {
            base.Reset();
            isWholeGrabDone.Reset();
            base.remainGrabCount = 0;
        }
        public new bool WaitGrabDone(int timeoutMs = 0)
        {
            //if (timeoutMs == 0)
            //    timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            LogHelper.Debug(LoggerType.Grab, "CameraPylonLine::WaitGrabDone");
            bool ok = false;

            while (timeoutMs > 10 && ok == false)
            {
                Thread.Sleep(10);
                bool isGrabbed = isWholeGrabDone.WaitOne(0);
                bool isStopped = base.grabStopEvent.WaitOne(0); //사용자 취소.
                if (isGrabbed || isStopped)
                {
                    ok = true;
                }

                timeoutMs -= 10;
            }
            return ok;
        }

        protected override void SetDeviceExposure(float exposureTimeMs)
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraPylonLine.Change Exposure {0} - {1}", Index, exposureTimeMs));
            float exposureTimeUs = exposureTimeMs * 1000f; //다시 us
            if (exposureTimeUs < 2)
            {
                exposureTimeUs = 2;
            }

            MainCamera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposureTimeUs);
        }

        public float GetDeviceExposureMs()
        {
            float expus = float.Parse(MainCamera.Parameters["ExposureTimeAbs"].ToString());
            return (float)(expus / 1000.0);
        }

        public override ImageD GetGrabbedImage() //ptr = imageID
        {
            //IntPtr.Zero) //마지막 이미지 리턴
            return PopImageFromBuffer(0);
        }

        public override void SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraPylon::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));
            try
            {
                MainCamera.Parameters[PLCamera.AcquisitionLineRateAbs].SetValue(hz);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("CameraPylon::SetDeviceExposure - {0}", ex.Message));
                return;
            }
            return;
        }

        //public override float GetAcquisitionLineRate()
        //{
        //    float hz = (float)MainCamera.Parameters[PLCamera.ResultingLineRateAbs].GetValue();//실제 카메라가 그랩할수 있는 성능
        //    return hz;
        //}

        ////////////////////////////////////////////////////DIO////////////////////////////////////////////////////////////////
        string IDigitalIo.GetName()
        {
            return Name;
        }

        int IDigitalIo.GetNumInPortGroup()
        {
            return 1;
        }

        int IDigitalIo.GetNumOutPortGroup()
        {
            return 1;
        }

        int IDigitalIo.GetInPortStartGroupIndex()
        {
            return 0;
        }

        int IDigitalIo.GetOutPortStartGroupIndex()
        {
            return 0;
        }

        int IDigitalIo.GetNumInPort()
        {
            throw new NotImplementedException();
        }

        int IDigitalIo.GetNumOutPort()
        {
            throw new NotImplementedException();
        }

        bool IDigitalIo.Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        void IDigitalIo.Release()
        {
            throw new NotImplementedException();
        }

        bool IDigitalIo.IsReady()
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.UpdateState(DeviceState state, string stateMessage)
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            //MainCamera.Parameters["UserOutputValue"].ParseAndSetValue(outputPortStatus == 1 ? "True" : "False");
            throw new NotImplementedException();
        }
        void IDigitalIo.WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        uint IDigitalIo.ReadOutputGroup(int groupNo)
        {
            //bool userOutputValue = bool.Parse(MainCamera.Parameters["UserOutputValue"].ToString());
            //return userOutputValue ? (uint)1 : 0;
            throw new NotImplementedException();
        }

        uint IDigitalIo.ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        void IDigitalIo.WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }


        public override void SetGain(float gain)
        {
            throw new NotImplementedException();
        }
    }
}
