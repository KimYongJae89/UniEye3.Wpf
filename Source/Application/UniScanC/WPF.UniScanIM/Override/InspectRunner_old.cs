using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using DynMvp.Vision.Matrox;
using Unieye.WPF.Base.Override;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniScanC.Data;
using UniScanC.Models;
using WPF.UniScanIM.Override;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


using ModelManager = UniScanC.Models.ModelManager;
using SystemManager = WPF.UniScanIM.Override.SystemManager;

namespace WPF.UniScanIM.Override
{
    public static class InspectRunnerUtil
    {
        public static ImageSource CreateImageSource(int width, int height, byte[] data)
        {
            BitmapSource image = BitmapSource.Create(width, height,
                                                    96, 96, System.Windows.Media.PixelFormats.Gray8, null,
                                                    data, width);

            image.Freeze();

            return image;
        }

        public static ImageSource GetDefectImage(int width, int height, IEnumerable<DustDefect> defects)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                foreach (var defect in defects)
                {
                    SolidColorBrush brush = new SolidColorBrush(Colors.Red);

                    StreamGeometry streamGeometry = new StreamGeometry();


                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        System.Windows.Point[] points = new System.Windows.Point[]
                        {
                            new System.Windows.Point(defect.BoundingRect.Left, defect.BoundingRect.Top),
                            new System.Windows.Point(defect.BoundingRect.Right, defect.BoundingRect.Top),
                            new System.Windows.Point(defect.BoundingRect.Right, defect.BoundingRect.Bottom),
                            new System.Windows.Point(defect.BoundingRect.Left, defect.BoundingRect.Bottom),
                        };

                        geometryContext.BeginFigure(points[0], true, true);
                        geometryContext.PolyLineTo(points, true, true);
                    }

                    drawingContext.DrawGeometry(brush, new System.Windows.Media.Pen(brush, 1), streamGeometry);
                }
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);

            renderTargetBitmap.Freeze();

            return renderTargetBitmap;
        }
    }

    public sealed class InspectInfos : Unieye.WPF.Base.Helpers.Observable
    {
        int inspectCount;
        public int InspectCount
        {
            get => inspectCount;
            set => Set(ref inspectCount, value);
        }

        int exportCount;
        public int ExportCount
        {
            get => exportCount;
            set => Set(ref exportCount, value);
        }
    }

    public sealed class InspectSettings
    {
        public int Inflate { get; set; }
        public int NonSheetValue { get; set; }
        //public int CalibrationImageGettingTimeSec { get; set; }
        public uint MinCalDataLength { get; set; }
        public float LineSpeedMps { get; set; }
        public TriggerMode TriggerMode { get; set; }
        public byte CalibrationValue { get; set; }
        public uint MaxDefctNum { get; set; }
    }

    public class IMInspectResult
    {
        public System.Windows.Media.ImageSource SourceImage { get; set; }
        public System.Windows.Media.ImageSource DefectImage { get; set; }
    }

    public class InspectingVisionParam
    {
        public float pps = 357000;//595000;// 3750000;//357000; //70000;
        public float pulsePerR = 10000; // pulse/Revolution
        public float mmPerR = 10 * 1000; //250 * 1000 //um

        public float resolution = 50.0f; //um

        //public float rollSpeed = 250 * 1000; // m/min        
        //
        public float inspectionFrameRate = 4.0f;

        // Light Calibration
        public float lightCalibFrameRate = 50.0f; // Frame/Sec

        public int lightCalibStep = 10;
        public int lightCalibStart = 0;
        public int lightCalibEnd = 255 + 0;
        public int lightstabilizingTimeMSec = 0; //msec

        // Image Uniformity(vignetting)
        public float uniformityCalibFrameNum = 20.0f; // Frames

        public float getRollSpeed()
        {
            float rollSpeed = ((pps / pulsePerR) * mmPerR);

            //rollSpeed = (115 * 1000 * 1000) / 60 ;
            rollSpeed = (15 * 1000 * 1000) / 60;

            return rollSpeed; // um/sec
        }

        public int getGrabHz()
        {
            // (150000.0f / 60.0f * 1000.0f) / 49.0f;
            int inspectImageHeight = (int)Math.Round(getRollSpeed() / resolution);
            if (inspectImageHeight > 38500)// 76900 at 7us, 38500 at 20us
            {
                Debug.WriteLine(string.Format("GrabHz is too big-{0}", inspectImageHeight));
            }
            return inspectImageHeight;
        }

        public float getEstimatedCalibrationTime()
        {
            float estimatedCalibrationTime = ((((lightCalibEnd - lightCalibStart) / lightCalibStep) / lightCalibFrameRate) +
                (((lightCalibEnd - lightCalibStart) / lightCalibStep) * lightstabilizingTimeMSec / 1000.0f) +
                (uniformityCalibFrameNum)) * 4.0f;

            return estimatedCalibrationTime;
        }

        public int getInspectImageHeight()
        {
            int inspectImageHeight = (int)Math.Round(getGrabHz() / inspectionFrameRate);
            return inspectImageHeight;
        }

        public int getLightCalibImageHeight()
        {
            int lightCalibImageHeight = (int)Math.Round(getGrabHz() / lightCalibFrameRate);
            return lightCalibImageHeight;
        }

        public int getUniformityCalibImageHeight()
        {
            int uniformityCalibImageHeight = (int)Math.Round(getGrabHz() * uniformityCalibFrameNum);
            return uniformityCalibImageHeight;
        }
    }










    public sealed class IMInspectRunner : InspectRunner
    {
        public bool IsSkipMode { get; set; } = false;

        public bool IsLightCalibMode { get; set; } = false;

        public InspectSettings Settings { get; } = new InspectSettings() { CalibrationValue = 127, MaxDefctNum = 5, Inflate = 10 };
        InspectingVisionParam inspectingVisionParam = new InspectingVisionParam();

        ManualResetEvent[] inspectEndEvent;
        ManualResetEvent[] inspectResetEvent;
        private DbDataExporter DbDataExporter = new DbDataExporter();
        private InspectInfos InspectInfo = new InspectInfos();

        private VisionModel visionModel;
        public VisionModel VisionModel { get => visionModel; set => visionModel = value; }

        private ConcurrentQueue<Task> exportTaskQueue;

        static ConcurrentQueue<byte[]> profileQueue;
        private ConcurrentQueue<byte[]>[] dataQueues;
        public int MaxTaskNum = 6;

        private bool isCalibrated;
        private float[] calibrationDatas;

        private AlgoImage[] srcBuffer;
        private AlgoImage[] labelBuffer;
        private AlgoImage profileBuffer;
        private CudaImage[] preProcessBuffer;

        private object scanNoLock = new object();
        private int scanNo;
        private AutoResetEvent lightAdjustingImageGrabed;

        public Action<IMInspectResult> Inspected;
        public Action<ImageSource> Grabbed;

        int currentScanNo;
        ConcurrentDictionary<int, Tuple<InspectResult, byte[], int, BlobRectList>> longDefectDictionray;
        ManualResetEvent longDefectEvent;
        ManualResetEvent longDefectEndEvent;

        private BlobRectList PrevBlobRect;
        private List<byte[]> PrevDataList;
        int removeImageNum = 0;
        int maxPrevDataNum = 10;

        AutoResetEvent[] autoResetEvent;

        List<byte> lightValues = new List<byte>();

        string lotNo;
        public string LotNo => lotNo;

        int leftEdge = 0;
        int rightEdge = 0;

        public override bool EnterWaitInspection(ModelBase curModel = null)
        {
            float estimatedCalibrationTime = inspectingVisionParam.getEstimatedCalibrationTime();
            float hz = inspectingVisionParam.getGrabHz();
            float inspectImageHeight = inspectingVisionParam.getInspectImageHeight();
            float lightCalibImageHeight = inspectingVisionParam.getLightCalibImageHeight();
            float uniformityCalibImageHeight = inspectingVisionParam.getUniformityCalibImageHeight();

            if (SystemState.Instance().OpState != OpState.Idle)
                return false;

            var commDictionary = SystemManager.Instance().Settings;

            float grabHz = inspectingVisionParam.getGrabHz();
            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.CameraInfo.Height = inspectingVisionParam.getInspectImageHeight();
            SetupCamera(grabHz);
            SetupBuffer();

            if (visionModel == null)
                visionModel = new VisionModel() { ThresholdDust = 50, ThresholdHole = 50 };

            foreach (var camera in DeviceManager.Instance().CameraHandler)
            {
                for (int i = 0; i < MaxTaskNum; i++)
                    InspectTaskStart(i, camera.ImageSize.Width, camera.ImageSize.Height);

                LongDefectTaskStart(camera.ImageSize.Width, camera.ImageSize.Height);
            }
            Thread.Sleep(100);


            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.ImageGrabbed += InspectImageGrabbed;

            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.GrabMulti();

            SystemState.Instance().SetWait();

            return true;
        }

        public override void ExitWaitInspection()
        {
            if (SystemState.Instance().OpState == OpState.Idle)
                return;

            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.ImageGrabbed -= InspectImageGrabbed;

            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.Stop();

            cancellationTokenSource?.Cancel();

            if (inspectResetEvent != null)
            {
                foreach (var inspectEvent in inspectResetEvent)
                    inspectEvent?.Set();
            }

            if (inspectEndEvent != null)
            {
                foreach (var endEvent in inspectEndEvent)
                {
                    endEvent?.WaitOne(1000);
                    //    Debug.WriteLine(string.Format("manualResetEvent Timeout"));                    
                }
            }

            if (longDefectEvent != null)
            {
                longDefectEvent.Set();
                longDefectEndEvent?.WaitOne(1000);
            }

            if (exportTaskQueue != null)
                while (!exportTaskQueue.IsEmpty)
                    Thread.Sleep(100);

            //Thread.Sleep(1000);

            Release();

            DeviceManager.Instance().LightCtrlHandler.GetLightCtrl(0).TurnOff();

            LightValue lightValue = new LightValue(DeviceManager.Instance().LightCtrlHandler.NumLight);
            int numLight = DeviceManager.Instance().LightCtrlHandler.NumLight;
            for (int i = 0; i < numLight; ++i)
            {
                lightValue.Value[i] = 0;
            }
            DeviceManager.Instance().LightCtrlHandler.TurnOn(lightValue);

            base.ExitWaitInspection();
        }

        private void Release()
        {
            isCalibrated = false;
            calibrationDatas = null;

            scanNo = 0;

            PrevDataList = null;

            PrevBlobRect?.Dispose();
            PrevBlobRect = null;
            longDefectDictionray = null;

            if (srcBuffer != null)
            {
                for (int i = 0; i < MaxTaskNum; i++)
                {
                    srcBuffer[i]?.Dispose();
                    labelBuffer[i]?.Dispose();
                    preProcessBuffer[i]?.Dispose();
                }
            }

            profileBuffer?.Dispose();

            InspectInfo.ExportCount = 0;
            InspectInfo.InspectCount = 0;

            lightAdjustingImageGrabed = null;
            inspectResetEvent = null;
            //visionModel = null;
            exportTaskQueue = null;
            profileQueue = null;
            dataQueues = null;
            inspectEndEvent = null;
            longDefectEvent = null;
            longDefectEndEvent = null;
        }

        private void SetupBuffer()
        {
            cancellationTokenSource = new CancellationTokenSource();

            exportTaskQueue = new ConcurrentQueue<Task>();
            profileQueue = new ConcurrentQueue<byte[]>();

            longDefectDictionray = new ConcurrentDictionary<int, Tuple<InspectResult, byte[], int, BlobRectList>>();
            currentScanNo = 0;
            longDefectEvent = new ManualResetEvent(false);
            longDefectEndEvent = new ManualResetEvent(false);
            removeImageNum = 0;


            var enumrator = DeviceManager.Instance().CameraHandler.GetEnumerator();
            enumrator.MoveNext();
            var camera = enumrator.Current;


            isCalibrated = false;
            calibrationDatas = new float[camera.ImageSize.Width];

            lightAdjustingImageGrabed = new AutoResetEvent(false);

            dataQueues = new ConcurrentQueue<byte[]>[MaxTaskNum];
            inspectResetEvent = new ManualResetEvent[MaxTaskNum];
            inspectEndEvent = new ManualResetEvent[MaxTaskNum];
            srcBuffer = new AlgoImage[MaxTaskNum];
            labelBuffer = new AlgoImage[MaxTaskNum];
            preProcessBuffer = new CudaImage[MaxTaskNum];
            for (int i = 0; i < MaxTaskNum; i++)
            {
                dataQueues[i] = new ConcurrentQueue<byte[]>();
                inspectResetEvent[i] = new ManualResetEvent(false);
                inspectEndEvent[i] = new ManualResetEvent(false);

                srcBuffer[i] = new MilGreyImage(camera.ImageSize.Width, camera.ImageSize.Height);
                labelBuffer[i] = new MilGreyImage(camera.ImageSize.Width, camera.ImageSize.Height);

                CudaImage cudaImage = new CudaDepthImage<byte>();
                cudaImage.Alloc(camera.ImageSize.Width, camera.ImageSize.Height);

                preProcessBuffer[i] = cudaImage;
            }

            profileBuffer = new MilGreyImage(camera.ImageSize.Width, camera.ImageSize.Height);

            scanNo = 0;

            PrevDataList = new List<byte[]>();
        }

        private void SetupCamera(float grabHz)
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

            for (int camIndex = 0; camIndex < cameraHandler.NumCamera; camIndex++)
            {
                Camera camera = cameraHandler.GetCamera(camIndex);

                if (Settings.TriggerMode == TriggerMode.Software)
                {
                    camera.UpdateBuffer(camera.CameraInfo.Height);
                    camera.SetAcquisitionLineRate(grabHz);

                    if(inspectingVisionParam.getRollSpeed() > 1900000)
                        camera.SetExposureTime(0.007f);
                    else
                        camera.SetExposureTime(0.020f);
                }
                else
                {
                    camera.UpdateBuffer();
                }
                //camera.UpdateBuffer();
                //imageDevicePtrQueue.Add(camera, new ConcurrentQueue<IntPtr>());
            }
        }



        public void Initialize(string lotNo, string dbDataPath)
        {
            this.lotNo = lotNo;

            DbDataExporter.Initialize(SystemConfig.Instance.CMIpAddress, dbDataPath);
            DbDataExporter.IsSaveImage = false;
        }

        private InspectResult BuildProductResult(int width, int height)
        {
            //Calibration calibration = SystemManager.Instance().GetCameraCalibration(camera.Index);
            float pixelResoultion = 40;

            InspectResult inspectResult = new InspectResult();
            inspectResult.LotNo = LotNo;
            inspectResult.InspectRegion = new SizeF(width, height);

            lock (scanNoLock)
            {
                inspectResult.ScanNo = scanNo;
                scanNo++;
            }

            //inspectResult.Resolution = new System.Drawing.SizeF(calibration.PelSize.Width, calibration.PelSize.Height);
            inspectResult.Resolution = new System.Drawing.SizeF(pixelResoultion, pixelResoultion);
            inspectResult.InterestRegion = new System.Drawing.RectangleF(0, 0, width, height);
            //inspectResult.Judgment = IsSkipMode ? Judgment.Skip : Judgment.OK;


            return inspectResult;
        }






        private void InspectTaskStart(int taskNo, int width, int height)
        {
            bool saveImg = true;

            Task.Factory.StartNew(() =>
            {
                while (cancellationTokenSource.IsCancellationRequested == false)
                {
                    try
                    {
                        if (dataQueues[taskNo].IsEmpty)
                        {
                            lock (inspectResetEvent)
                                inspectResetEvent[taskNo].Reset();
                        }

                        if (!inspectResetEvent[taskNo].WaitOne())
                        {
                            //Debug.WriteLine(string.Format("inspectResetEvent Timeout-{0}", taskNo));
                            continue;
                        }

                        if (cancellationTokenSource.IsCancellationRequested == true)
                            break;

                        if (dataQueues[taskNo].TryDequeue(out byte[] data))
                        {
                            var inspectResult = BuildProductResult(width, height);
                            inspectResult.InspectStartTime = DateTime.Now;//
                            inspectResult.ModuleNo = visionModel.ModuleIndex;
                            if (visionModel.ModuleIndex > 0)
                                inspectResult.StartPos += width;

                            
                                //inspectResult.ImageData = ImageHelper.ByteArrayToBitmapSource(camera.ImageSize.Width, camera.ImageSize.Height, PixelFormats.Gray8, data);
                                //if (saveImg) preProcessDictionary[camera].ToBitmap().Save(string.Format("C:\\Image\\0_raw.JPG"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                var imageID = preProcessBuffer[taskNo].ImageID;
                                preProcessBuffer[taskNo].SetByte(data);

                                if (saveImg) preProcessBuffer[taskNo].Save(string.Format("C:\\Image\\0_raw.JPG"), new DebugContext(saveImg, "C:\\Image\\"));

                                CudaMethods.CUDA_MATH_MUL(imageID, calibrationDatas);

                                data = preProcessBuffer[taskNo].CloneByte();

                            if (IsSkipMode)
                                data.SetValue(Settings.CalibrationValue, data.Length);

                                //if (saveImg) preProcessDictionary[camera].ToBitmap().Save(string.Format("C:\\Image\\1_raw.JPG"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                //CudaMethods.CUDA_BINARIZE(imageID, imageID, Math.Max(0, Settings.CalibrationValue - visionModel.ThresholdDust), Math.Min(255, Settings.CalibrationValue + visionModel.ThresholdHole), false);

                            srcBuffer[taskNo].SetByte(data);

                                if (saveImg) srcBuffer[taskNo].ToBitmap().Save(string.Format("C:\\Image\\1_raw.JPG"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                               
                                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(labelBuffer[taskNo]);

                                Rectangle rectangle = new Rectangle(500, 0, width - 500, height);

                                var labelChild = labelBuffer[taskNo].GetChildImage(rectangle);
                                var srcChild = srcBuffer[taskNo].GetChildImage(rectangle);

                                //if (saveImg) srcChild.ToBitmap().Save(string.Format("C:\\Image\\srcChild.JPG"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                imageProcessing.Binarize(srcChild, labelChild,
                                    Math.Max(0, Settings.CalibrationValue - visionModel.ThresholdDust), Math.Min(255, Settings.CalibrationValue + visionModel.ThresholdHole), true);

                                BlobParam blobParam = new BlobParam();
                                blobParam.SelectMeanValue = true;
                                blobParam.SelectMinValue = true;
                                blobParam.SelectMaxValue = true;
                                blobParam.SelectCenterPt = true;
                                blobParam.AreaMin = visionModel.ThresholdSize;
                                BlobRectList blobRects = imageProcessing.Blob(labelChild, srcChild, blobParam);

                                labelChild.Dispose();
                                srcChild.Dispose();

                                int defectNo = 0;
                                Rectangle sourceRect = new Rectangle(0, 0, width, height);
                                Random random = new Random();

                                var list = blobRects.ToList();

                                //비동기 처리
                                foreach (var blobRect in list)
                                {
                                    // 왠지 모르지만 0으로 출력되는 오류가 있음 
                                    if (blobRect.MeanValue == 0)
                                        continue;

                                    if (blobRect.BoundingRect.Y == 0)
                                        continue;

                                    //if (inspectResult.DustDefectList.Count >= random.Next(10))
                                    //    break;

                                    var rect = Rectangle.Round(blobRect.BoundingRect);
                                    rect.Offset(rectangle.X, rectangle.Y);

                                    var imageRect = Rectangle.Round(blobRect.BoundingRect);
                                    imageRect.Offset(rectangle.X, rectangle.Y);
                                    imageRect.Inflate(Settings.Inflate, Settings.Inflate);
                                    imageRect.Intersect(sourceRect);

                                    AlgoImage defectImage = srcBuffer[taskNo].GetChildImage(imageRect);
                                    AlgoImage processedImage = labelBuffer[taskNo].GetChildImage(imageRect);

                                    DustDefect dustDefect = new DustDefect()
                                    {
                                        ModuleNo = visionModel.ModuleIndex,
                                        ScanNo = inspectResult.ScanNo,
                                        Area = blobRect.Area,
                                        BoundingRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height),
                                        DefectNo = defectNo++,
                                        AvgGv = blobRect.MeanValue,
                                        MinGv = blobRect.MinValue,
                                        MaxGv = blobRect.MaxValue,
                                        DefectPos = new PointF(blobRect.CenterPt.X + rectangle.X, blobRect.CenterPt.Y + rectangle.Y),
                                        DefectImage = defectImage.ToBitmapSource(),
                                        BinaryImage = processedImage.ToBitmapSource()
                                    };
                                    inspectResult.DustDefectList.Add(dustDefect);
                                    //if (saveImg) defectImage.ToBitmap().Save(string.Format("C:\\Image\\defectImage_{0}.JPG", inspectResult.DustDefectList.Count), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    defectImage.Dispose();
                                    processedImage.Dispose();
                                }

                                // 키가 있으면 false
                                while (longDefectDictionray.TryAdd(inspectResult.ScanNo, new Tuple<InspectResult, byte[], int, BlobRectList>(inspectResult, data, rectangle.X, blobRects)) == false)
                                {

                                }

                                lock (longDefectEvent)
                                    longDefectEvent.Set();

                                inspectResult.Judgment = inspectResult.DustDefectList.Count > 0 ? Judgment.NG : Judgment.OK;
                            
                            //else
                            //{
                            //    inspectResult.Judgment = Judgment.Skip;
                            //}



                            inspectResult.InspectEndTime = DateTime.Now;
                            //Debug.WriteLine(string.Format($"InspectCount : {InspectInfo.InspectCount}"));

                            InspectInfo.InspectCount = dataQueues.Max(queue => queue.Count);

                            Debug.WriteLine(string.Format("Inspect - {0}", InspectInfo.InspectCount));
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                inspectEndEvent[taskNo].Set();
            }, TaskCreationOptions.LongRunning);
        }



        private IEnumerable<DustDefect> LongDefectTask(byte[] data, BlobRectList blobRects, int offsetX, int width, int height)
        {
            try
            {
                int lastHeight = height * ((PrevDataList.Count + 1) + removeImageNum);

                BlobParam nextBlobParam = new BlobParam();
                nextBlobParam.BoundingRectMaxY = lastHeight - 1;

                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(srcBuffer[0]);
                BlobParam blobParam = new BlobParam();
                blobParam.SelectMeanValue = true;
                blobParam.SelectMinValue = true;
                blobParam.SelectMaxValue = true;
                blobParam.SelectCenterPt = true;
                List<DustDefect> defectList = new List<DustDefect>();

                if (PrevBlobRect != null)
                {
                    BlobParam borderBlobParam = new BlobParam();
                    borderBlobParam.EraseBorderBlob = true;
                    borderBlobParam.SelectMeanValue = true;
                    borderBlobParam.SelectMinValue = true;
                    borderBlobParam.SelectMaxValue = true;
                    borderBlobParam.SelectCenterPt = true;



                    BlobParam defectBlobParam = new BlobParam();
                    defectBlobParam.BoundingRectMaxY = lastHeight - 2;
                    defectBlobParam.SelectMeanValue = true;
                    defectBlobParam.SelectMinValue = true;
                    defectBlobParam.SelectMaxValue = true;
                    defectBlobParam.SelectCenterPt = true;



                    BlobParam topBlobParam = new BlobParam();
                    topBlobParam.BoundingRectMinY = (lastHeight - height);
                    topBlobParam.SelectMeanValue = true;
                    topBlobParam.SelectMinValue = true;
                    topBlobParam.SelectMaxValue = true;
                    topBlobParam.SelectCenterPt = true;


                    //가장자리 블랍 제외 제거
                    imageProcessing.FilterBlob(blobRects, borderBlobParam, true);

                    var defectBlobs = imageProcessing.BlobMerge(PrevBlobRect, blobRects, blobParam);
                    var nextBlobs = imageProcessing.BlobMerge(PrevBlobRect, blobRects, blobParam);
                    PrevBlobRect.Dispose();

                    if (PrevDataList.Count > maxPrevDataNum)
                    {
                        imageProcessing.FilterBlob(defectBlobs, topBlobParam, true);
                        nextBlobs.Dispose();
                        PrevBlobRect = null;

                        Debug.WriteLine(string.Format("****************************************"));
                        Debug.WriteLine(string.Format("Over Max Stripe Defect Length - {0}", maxPrevDataNum));
                        Debug.WriteLine(string.Format("****************************************"));
                    }
                    else
                    {
                        imageProcessing.FilterBlob(defectBlobs, topBlobParam, true);
                        imageProcessing.FilterBlob(defectBlobs, defectBlobParam);

                        imageProcessing.FilterBlob(nextBlobs, nextBlobParam, true);

                        if (nextBlobs.Count > 0)
                        {
                            PrevBlobRect = nextBlobs;
                            PrevDataList.Add(data);
                        }
                        else
                        {
                            nextBlobs.Dispose();
                            PrevBlobRect = null;
                        }
                    }

                    defectBlobs.Dispose();
                    blobRects.Dispose();

                    var srcRect = new Rectangle(0, removeImageNum * height, width, height * PrevDataList.Count);
                    foreach (var blobRect in defectBlobs)
                    {
                        // 왠지 모르지만 0으로 출력되는 오류가 있음 
                        //if (blobRect.MeanValue == 0)
                        //    continue;

                        var rect = Rectangle.Round(blobRect.BoundingRect);
                        var imageRect = Rectangle.Round(blobRect.BoundingRect);
                        rect.Offset(offsetX, 0);
                        imageRect.Offset(offsetX, 0);
                        imageRect.Inflate(Settings.Inflate, Settings.Inflate);
                        imageRect.Intersect(srcRect);

                        byte[] defectData = new byte[imageRect.Width * imageRect.Height];
                        var startY = imageRect.Top;
                        var endY = imageRect.Bottom;

                        for (int y = startY, defectIndex = 0; y < endY; y++)
                        {
                            int imageIndex = (y / height) - removeImageNum;
                            int realY = y - (removeImageNum * height);
                            int startX = (realY % height) * width + imageRect.X;
                            int endX = startX + imageRect.Width;

                            for (int index = startX; index < endX; index++)
                                defectData[defectIndex++] = PrevDataList[imageIndex][index];
                        }

                        var defectImage = BitmapSource.Create(imageRect.Width, imageRect.Height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, defectData, imageRect.Width);
                        defectImage.Freeze();

                        DustDefect dustDefect = new DustDefect()
                        {
                            ModuleNo = visionModel.ModuleIndex,
                            Area = blobRect.Area,
                            BoundingRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height),
                            AvgGv = blobRect.MeanValue,
                            MinGv = blobRect.MinValue,
                            MaxGv = blobRect.MaxValue,
                            DefectPos = new PointF(blobRect.CenterPt.X + offsetX, blobRect.CenterPt.Y),
                            DefectImage = defectImage
                        };

                        //using (var fileStream = new FileStream($"C:\\Image\\{rect.Height}.jpg", FileMode.Create))
                        //{
                        //    BitmapEncoder encoder = new JpegBitmapEncoder();

                        //    encoder.Frames.Add(BitmapFrame.Create(defectImage as BitmapSource));
                        //    encoder.Save(fileStream);
                        //}

                        defectList.Add(dustDefect);
                    }

                    if (PrevBlobRect == null)
                    {
                        PrevDataList.Clear();
                        removeImageNum = 0;
                    }
                    else
                    {
                        int imageIndex = ((int)PrevBlobRect.Min(blob => blob.BoundingRect.Y)) / height - removeImageNum;
                        if (imageIndex > 0)
                        {
                            PrevDataList.RemoveRange(0, imageIndex);
                            removeImageNum += imageIndex;
                        }
                    }
                }
                else
                {
                    imageProcessing.FilterBlob(blobRects, nextBlobParam, true);
                    if (blobRects.Count > 0)
                    {
                        PrevDataList.Add(data);
                        PrevBlobRect = blobRects;
                    }
                    else
                    {
                        blobRects.Dispose();
                    }
                }
                return defectList;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void LongDefectTaskStart(int width, int height)
        {
            Task.Factory.StartNew(() =>
            {
                while (cancellationTokenSource.IsCancellationRequested == false)
                {
                    try
                    {
                        if (longDefectDictionray.IsEmpty)
                        {
                            lock (longDefectEvent)
                                longDefectEvent.Reset();
                        }

                        if (!longDefectEvent.WaitOne())
                        {
                            Debug.WriteLine(string.Format("longDefectEvent Timeout"));
                            continue;
                        }

                        if (cancellationTokenSource.IsCancellationRequested == true)
                            break;

                        if (longDefectDictionray.ContainsKey(currentScanNo))
                        {
                            if (longDefectDictionray.TryRemove(currentScanNo, out Tuple<InspectResult, byte[], int, BlobRectList> tuple))
                            {
                                var inspectResult = tuple.Item1;
                                var data = tuple.Item2;
                                var offset = tuple.Item3;
                                var blobRects = tuple.Item4;

                                var result = LongDefectTask(data, blobRects, offset, width, height);

                                var defectNo = inspectResult.NumDefect;
                                foreach (var blob in result)
                                {
                                    blob.ModuleNo = visionModel.ModuleIndex;
                                    blob.DefectNo = defectNo++;
                                    blob.ScanNo = currentScanNo;
                                    //using (var fileStream = new FileStream($"C:\\Image\\{blob.DefectImage.Height}.jpg", FileMode.Create))
                                    //{
                                    //    BitmapEncoder encoder = new JpegBitmapEncoder();
                                    //    encoder.Frames.Add(BitmapFrame.Create(blob.DefectImage as BitmapSource));
                                    //    encoder.Save(fileStream);
                                    //}
                                }
                                inspectResult.DustDefectList.AddRange(result);
                                exportTaskQueue.Enqueue(ExportResult(inspectResult));
                                currentScanNo++;
                            }
                        }
                        else
                        {
                            lock (longDefectEvent)
                                longDefectEvent.Reset();
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

                foreach (var tuple in longDefectDictionray.Values)
                    tuple.Item4.Dispose();

                longDefectDictionray.Clear();

                longDefectEndEvent.Set();
            }, TaskCreationOptions.LongRunning);
        }

        public Task ExportResult(InspectResult inspectResult)
        {
            return Task.Run(async () =>
            {
                InspectInfo.ExportCount = exportTaskQueue.Count;

                Debug.WriteLine(string.Format("ScanNum - {0}", inspectResult.ScanNo));

                await DbDataExporter.ExportAsync(inspectResult);
                //InspectEventHandler.ProductInspected(inspectResult);

                //Grabbed?.Invoke(InspectRunnerUtil.CreateImageSource(image.Width, image.Height, data));

                while (exportTaskQueue.TryDequeue(out Task temp) == false)
                {

                }

                Debug.WriteLine(string.Format("Export - {0}", InspectInfo.ExportCount));

                InspectInfo.ExportCount = exportTaskQueue.Count;
            });
        }


        public void InspectImageGrabbed(Camera camera)
        {
            bool saveImg = false;
            var image = camera.GetGrabbedImage() as Image2D;
            if (image == null || image.DataPtr == IntPtr.Zero)
                return;

            var model = curModel;
            InspectInfo.InspectCount = dataQueues.Max(queue => queue.Count);
            byte[] data = new byte[image.Width * image.Height];
            Marshal.Copy(image.DataPtr, data, 0, image.Pitch * image.Height);// 100ms

            if (isCalibrated == false)
            {
                profileBuffer.SetByte(data);
                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(profileBuffer);
                var profile = imageProcessing.Projection(profileBuffer, TwoWayDirection.Horizontal, ProjectionType.Mean);

                byte[] profileByte = new byte[profile.Length];
                for (int i = 0; i < profile.Length; ++i)
                    profileByte[i] = Convert.ToByte(profile[i]);

                profileQueue.Enqueue(profileByte);

                if (profileQueue.Count >= inspectingVisionParam.uniformityCalibFrameNum)
                {
                    Calibration(camera);
                    isCalibrated = true;
                }
                return;
            }

            if (cancellationTokenSource.IsCancellationRequested == false)
            {
                int min = 0;
                bool enqueued = false;
                while (true)
                {
                    for (int i = 0; i < MaxTaskNum; i++)
                    {
                        if (dataQueues[i].Count <= min)
                        {
                            dataQueues[i].Enqueue(data);
                            inspectResetEvent[i].Set();
                            enqueued = true;
                            break;
                        }
                    }

                    if (enqueued)
                        break;

                    min++;
                }
            }
        }















        private void EdgeFinder(float []data, ref int left, ref int right)
        {
            int leftEdge = 0;
            int rightEdge = data.Length;
            Image<Gray, float> profileImg = new Image<Gray, float>(data.Length, 1);

            for (int i = 0; i < data.Length; ++i)
            {
                profileImg.Data[0, i, 0] = data[i];
            }

            // data --%로 필터링
            int filterWidth = (int)Math.Round(data.Length * 0.02);
            filterWidth = (filterWidth % 2 == 0) ? filterWidth : filterWidth - 1;
            profileImg = profileImg.SmoothBlur(filterWidth, 1);
            Image<Gray, float> edgeImg = profileImg.Sobel(1, 0, 3);

            //edgeImg = edgeImg.SubR(edgeImg.GetAverage());
            CvInvoke.Normalize(edgeImg, edgeImg, -1, 1, NormType.MinMax);

            //Case A: 가장 큰 것은 밝은->어두운
            //Case B: 가장 작은 것은 어두운->밝은


            double min=0;
            double max=0;
            Point minPnt = new Point();
            Point maxPnt = new Point();

            Rectangle ROI = new Rectangle(0, 0, data.Length, 1);
            // 1/5 지점에서 Case A 찾기
            ROI.X = 0;
            ROI.Width = (int)Math.Round(data.Length * 0.2);
            edgeImg.ROI = ROI;
            CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);
            ROI.Width = data.Length;
            edgeImg.ROI = ROI;

            if (min < -0.8)
                leftEdge = minPnt.X;
            else
                leftEdge = 0;

            // 4/5 지점에서 Case B 찾기
            ROI.X = (int)Math.Round(data.Length * 0.8);
            ROI.Width = data.Length - ROI.X;
            edgeImg.ROI = ROI;
            CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);
            ROI.Width = data.Length;
            edgeImg.ROI = ROI;

            if (max > 0.8)
                rightEdge = (ROI.X+ maxPnt.X);
            else
                rightEdge = data.Length;

            left = leftEdge;
            right = rightEdge;



            edgeImg.Dispose();
            profileImg.Dispose();
        }





        private bool Calibration(Camera camera)
        {
            var queue = profileQueue;

            if (queue.IsEmpty)
                return false;

            var width = camera.ImageSize.Width;
            Image<Gray, float> profileImg = new Image<Gray, float>(calibrationDatas.Length, 1);

            var count = queue.Count;
            while (queue.IsEmpty == false)
            {
                queue.TryDequeue(out byte[] profile);
                for (int x = 0; x < width; x++)
                {
                    calibrationDatas[x] += profile[x];
                }
                
            }

            Parallel.For(0, width, x =>
            {
                profileImg.Data[0, x, 0] = calibrationDatas[x];
            });
            //int leftEdge = 0;
            //int rightEdge = 0;
            EdgeFinder(calibrationDatas, ref leftEdge, ref rightEdge);

            Parallel.For(0, width, x =>
            {
                if (calibrationDatas[x] == 0)
                    calibrationDatas[x] = 0;
                else
                    calibrationDatas[x] = (float)Settings.CalibrationValue / (calibrationDatas[x] / (float)count);


                profileImg.Data[0, x, 0] = calibrationDatas[x];
            });
            

            profileImg = profileImg.SmoothBlur(51, 1);

            //
            /// 
            int range = rightEdge - leftEdge;
            double[] xData = new double[range];
            double[] yData = new double[range];

            _2ndPoliLineEq _2NdPoliLineEq = new _2ndPoliLineEq();

            for (int i = leftEdge, k = 0 ; i < rightEdge; ++i, ++k)
            {
                xData[k] = i;
                yData[k] = profileImg.Data[0,i,0];
            }

            _2NdPoliLineEq.FitLine(xData, yData);

            for (int i = 0; i < profileImg.Width; ++i)
            {
                profileImg.Data[0, i, 0] = (float)(
                    (_2NdPoliLineEq.Coeff_A1 * i * i) +
                    (_2NdPoliLineEq.Coeff_B1 * i) +
                    (_2NdPoliLineEq.Coeff_C1));

            }
                //




                Parallel.For(0, width, x =>
            {
                calibrationDatas[x] = profileImg.Data[0, x, 0];
            });

            profileImg.Dispose();

            return true;
        }

        public void lightCalibrationStart()
        {
            lightValues.Clear();
            float grabHz = inspectingVisionParam.getGrabHz();
            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.CameraInfo.Height = inspectingVisionParam.getLightCalibImageHeight();

            SetupCamera(grabHz);
            SetupBuffer();

            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.ImageGrabbed += adjustingLightImageGrabbed;

            int NumCamera = DeviceManager.Instance().CameraHandler.NumCamera;

            autoResetEvent = new AutoResetEvent[NumCamera];
            for (int i = 0; i < NumCamera; ++i)
            {
                autoResetEvent[i] = lightAdjustingImageGrabed;
                autoResetEvent[i].Reset();
            }
        }

        public string lightCalibrationEnd()
        {
            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.ImageGrabbed -= adjustingLightImageGrabbed;
            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.Stop();



            List<float> avgList = new List<float>();
            List<byte[]> profileList = new List<byte[]>();

            byte[] profile = null;
            while (!profileQueue.IsEmpty)
            {
                profileQueue.TryDequeue(out profile);
                profileList.Add(profile);
                avgList.Add(Convert.ToSingle(profile.Select(x => (int)x).Average()));

                Debug.WriteLine(string.Format("ScanNum-{0}, {1}", avgList.Count, avgList[avgList.Count - 1]));
            }

            int nearIndex = 0;
            float nearValue = float.MaxValue;
            List<float> diffValueList = new List<float>();
            for (int i = 0; i < avgList.Count; i += 1)
            {
                if (Math.Abs(avgList[i] - Settings.CalibrationValue) < nearValue)
                {
                    nearValue = Math.Abs(avgList[i] - Settings.CalibrationValue);
                    diffValueList.Add(nearValue * nearValue);
                    nearIndex = i;
                }
            }
            //if (nearIndex == 0)
            //    return false;


            Image<Gray, byte> profileImg = new Image<Gray, byte>(profileList[nearIndex].Length, 1);
            profileImg.Bytes = profileList[nearIndex];
            profileImg = profileImg.SmoothMedian(21).SmoothGaussian(21);
            Image<Gray, float> sobelImg = profileImg.Sobel(1, 0, 7);

            Array.Copy(profileImg.Bytes, profileList[nearIndex], profileList[nearIndex].Length);


            


            //


            int lightValueFitted = lightValues[nearIndex];// (inspectingVisionParam.lightCalibStart + (nearIndex * inspectingVisionParam.lightCalibStep));

            LightValue lightValueList = new LightValue(4);
            lightValueList[3] = lightValueFitted;
            DeviceManager.Instance().LightCtrlHandler.TurnOn(lightValueList);
            lightValues.Clear();

            sobelImg.Dispose();
            profileImg.Dispose();

            return string.Format("{0},{1}", 0, lightValueFitted);
        }

        public void lightCalibrationGrab(byte lightValue)
        {
            LightValue lightValueList = new LightValue(4);
            lightValueList[3] = lightValue;
            DeviceManager.Instance().LightCtrlHandler.TurnOn(lightValueList);
            Thread.Sleep(50);


            lightValues.Add(lightValue);

            foreach (var camera in DeviceManager.Instance().CameraHandler)
                camera.GrabOnceAsync();

            if (WaitHandle.WaitAll(autoResetEvent, 3000))
            {
                lightAdjustingImageGrabed.Reset();
                DeviceManager.Instance().CameraHandler.Stop();
            }
            else
            {
                Debug.WriteLine(string.Format("autoResetEvent Timeout"));
            }
        }


        public void adjustingLightImageGrabbed(Camera camera)
        {
            //camera.Stop();
            bool saveImg = false;
            var image = camera.GetGrabbedImage() as Image2D;
            if (image == null || image.DataPtr == IntPtr.Zero)
                return;

            byte[] data = new byte[image.Width * image.Height];
            Marshal.Copy(image.DataPtr, data, 0, image.Pitch * image.Height);// 100ms
            var algoImage = profileBuffer;
            algoImage.SetByte(data);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            var profile = imageProcessing.Projection(algoImage, TwoWayDirection.Horizontal, ProjectionType.Mean);
            byte[] profileByte = new byte[profile.Length];
            for (int i = 0; i < profile.Length; ++i)
                profileByte[i] = Convert.ToByte(profile[i]);

            // 중간 부분만으로 연산
            int left = (int)Math.Round(profile.Length * 0.3);
            int right = (int)Math.Round(profile.Length * 0.7);

            float[] cropedProfile = new float[profile.Length - left]; // (int)Math.Round((double)profile.Length * 0.6)];
            Array.Copy(profile, left, cropedProfile, 0, cropedProfile.Length);

            //if (saveImg) algoImage.ToBitmap().Save(string.Format("C:\\Image\\calib_{0}.JPG", profileQueueDictionary[camera].Count), System.Drawing.Imaging.ImageFormat.Jpeg);

            profileQueue.Enqueue(profileByte);

            lightAdjustingImageGrabed.Set();
        }



























        public void GrabImageGrabbed(Camera camera)
        {
            var image = camera.GetGrabbedImage() as Image2D;

            if (image == null || image.DataPtr == IntPtr.Zero)
                return;

            byte[] data = new byte[image.Width * image.Height];

            for (int src = 0, dest = 0; src < image.Pitch * image.Height; src += image.Pitch, dest += image.Width)
                Marshal.Copy(image.DataPtr + src, data, dest, image.Width);

            Task.Run(() =>
            {
                Grabbed?.Invoke(InspectRunnerUtil.CreateImageSource(image.Width, image.Height, data));
            });
        }

        public override void Inspect(int triggerIndex = -1)
        {

        }

        CancellationTokenSource taskTokenSource;
        Random testRandom = new Random();

        public void StartGrab(int cameraIndex)
        {
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(cameraIndex);
            camera.ImageGrabbed += GrabImageGrabbed;

            camera.GrabMulti();
        }

        public void StopGrab(int cameraIndex)
        {
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(cameraIndex);
            camera.ImageGrabbed -= GrabImageGrabbed;

            camera.Stop();
        }


        //public void TestStart(int time)
        //{
        //    Task.Run(() =>
        //    {
        //        taskTokenSource = new CancellationTokenSource();

        //        scanNo1 = 0;
        //        scanNo2 = 0;

        //        while (taskTokenSource.IsCancellationRequested == false)
        //        {
        //            foreach (var module in SystemConfig.Instance.ModuleList)
        //                Task.Run(() => Test(module.ModuleNo));

        //            Thread.Sleep(time);
        //        }
        //    });
        //}

        //public void TestStop()
        //{
        //    taskTokenSource.Cancel();

        //    Thread.Sleep(100);

        //    taskTokenSource = null;
        //}

        //int scanNo1;
        //int scanNo2;

        //public void Test(int moduleNo)
        //{
        //    InspectResult inspectResult = new InspectResult();
        //    inspectResult.ModuleNo = moduleNo;
        //    inspectResult.LotNo = LotNo;
        //    if (moduleNo == 0)
        //        inspectResult.ScanNo = scanNo1++;
        //    else
        //        inspectResult.ScanNo = scanNo2++;

        //    inspectResult.Resolution = new SizeF(49, 49);
        //    inspectResult.InterestRegion = new System.Drawing.RectangleF(0, 0, 6144, 1000);

        //    int randNum = (int)Math.Round(testRandom.NextDouble() * 5);

        //    inspectResult.Judgment = randNum == 0 ? Judgment.OK : Judgment.NG;

        //    for (int i= 0; i <randNum; i++)
        //    {
        //        RectangleF boundingRect = new RectangleF((int)Math.Round(testRandom.NextDouble() * 6134), (int)Math.Round(testRandom.NextDouble() * 990), 10, 10);

        //        DustDefect dustDefect = new DustDefect()
        //        {
        //            ModuleNo = moduleNo,
        //            ScanNo = inspectResult.ScanNo,
        //            Area = (int)Math.Round(testRandom.NextDouble() * 100.0),
        //            BoundingRect = boundingRect,
        //            DefectNo = i,
        //            AvgGv = (int)Math.Round(testRandom.NextDouble() * 255.0),
        //            MinGv = (int)Math.Round(testRandom.NextDouble() * 255.0),
        //            MaxGv = (int)Math.Round(testRandom.NextDouble() * 255.0),
        //            DefectPos = new PointF(boundingRect.X + 5, boundingRect.Y + 5)
        //        };

        //        inspectResult.DustDefectList.Add(dustDefect);
        //    }

        //    DbDataExporter.Export(inspectResult);
        //}
    }
}
