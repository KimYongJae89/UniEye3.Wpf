using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Helpers;
using UniScanC.Models;
using UniScanC.Struct;
using WPF.UniScanIM.Inspect;
using WPF.UniScanIM.Manager;
using WPF.UniScanIM.Service;
using Point = System.Drawing.Point;

namespace WPF.UniScanIM.Override
{
    public sealed class IMInspectRunner : InspectRunner
    {
        public bool IsSkipMode { get; set; } = false; // 스킵 모드
        public bool IsSaveImage { get; set; } = false; // 디버깅용 이미지 획득

        private bool IsLighthandling { get; set; } = false; // IM 이 조명을 관리하는 경우
        private double LineSpeed { get; set; } = 10;
        private int ImageSaveCnt { get; set; } = 0;

        //private InspectSettings settings = new InspectSettings(); // 검사 관련 세팅
        //private InspectVisionParam visionParam = new InspectVisionParam(); // 파라미터 세팅

        public List<VisionModel> VisionModels { get; set; } // CM에서 설정한 파라미터

        private InspectDataExporter DataExporter { get; set; } = new InspectDataExporter(); // 데이터 내보내기

        //private ConcurrentQueue<(byte[],Size, InspectResult)>[] dataQueues; // 이미지를 넣어주는 큐
        //private ConcurrentQueue<Size>[] sizeQueues; // 사이즈 정보를 넣어주는 큐
        //private ConcurrentQueue<InspectResult>[] resultQueues; // 이미지 획득 시점을 정확하게 얻기위한 큐

        private Dictionary<ModuleInfo, AlgoTaskManager> AlgoTaskManagerDic { get; set; }

        //private int maxMergeNum = 50; // 줄무늬 찾는 알고리즘 변수

        //private object scanNoLock = new object(); // 스캔 번호가 꼬이지 않게 하는 락
        //private int scanNo; // 스캔 번호
        //private int grabNo; // 줄무늬 찾을 시 사용
        private string LotNo { get; set; } // Lot 이름 넘겨줄 때 사용

        public bool UpdateImageView { get; set; }
        public Action<int, ulong, BitmapSource> Grabbed { get; set; } // 이미지 확인용 시퀀스

        public Image2D LastGrabbedImage { get; set; } = null;

        //private int leftEdge = 0; // Image Roi

        private List<Stripe> TopStripeList { get; set; } = new List<Stripe>();
        private List<Stripe> StripeList { get; set; } = new List<Stripe>();

        public OnInspectCompletedDelegate AfterInspectCompleted { get; set; }

        public IMInspectRunner()
        {
            AlgoTaskManagerDic = new Dictionary<ModuleInfo, AlgoTaskManager>();
        }

        public void Initialize(string dbDataPath, string resultImagePath, string lotNo, double lineSpeed)
        {
            LogHelper.Debug(LoggerType.Inspection, $"IMInspectRunner::Initialize - dbDataPath: {dbDataPath}, resultImagePath: {resultImagePath}, lotNo: {lotNo}, lineSpeed: {lineSpeed}");
            SystemConfig config = SystemConfig.Instance;

            DataExporter.SetDataBaseInfo(config.CMDBIpAddress, dbDataPath, config.CMDBUserName, config.CMDBPassword);

            DataExporter.SetNetworkDriveInfo(config.CMNetworkIpAddress, config.CMNetworkUserName, config.CMNetworkPassword);
            DataExporter.SetResultImagePath(resultImagePath);
            DataExporter.SetImageSetting(config.IsSaveFrameImage, config.IsSaveDefectImage);

            if (lineSpeed != 0)
            {
                config.LineSpeed = (float)lineSpeed;
            }

            this.LotNo = lotNo;
            this.LineSpeed = lineSpeed;
        }

        public override bool EnterWaitInspection(ModelBase curMode = null)
        {
            Debug.WriteLine($"IMInspectRunner::EnterWaitInspection");

            try
            {
                DryRunOpenCV();

                ModuleInfo[] moduleInfos = SystemConfig.Instance.ModuleList.ToArray();

                if (VisionModels == null)
                {
                    throw new Exception("MODEL_NOT_OPEN_MESSAGE");
                }

                if (SystemState.Instance().OpState != OpState.Idle)
                {
                    return false;
                }

                // 검사 프로세스 생성
                foreach (ModuleInfo moduleInfo in moduleInfos)
                {
                    AlgoTaskManagerSetting algoTaskManagerSettings = LoadATMSettings($"{moduleInfo.Topic}.json");
                    AlgoTaskManager algoTaskManager = StartATM(moduleInfo, algoTaskManagerSettings, VisionModels[moduleInfo.ModuleNo]);
                    algoTaskManager.OnInspectCompleted += OnInspectCompleted;
                }

                // 카메라 설정. 그랩 시작.
                ImageSaveCnt = 0;
                FrameTriggerService.OnInspectFrameGrabbed += InspectImageGrabbed;
                FrameTriggerService.Start();
                foreach (ModuleInfo moduleInfo in moduleInfos)
                {
                    moduleInfo.Camera.SetTriggerMode(moduleInfo.TriggerMode);
                    if (moduleInfo.Camera is CameraVirtual || moduleInfo.TriggerMode == TriggerMode.Software)
                    {
                        float grabHz = UnitConvertor.GetGrabHz(LineSpeed, moduleInfo.ResolutionHeight);
                        moduleInfo.Camera.SetAcquisitionLineRate(grabHz);
                    }

                    moduleInfo.Camera.ImageGrabbed = null;
                    if (moduleInfo.UseFrameTrigger)
                    {
                        moduleInfo.Camera.ImageGrabbed = FrameTriggerService.InspectImageGrabbed;
                    }
                    else
                    {
                        moduleInfo.Camera.ImageGrabbed = InspectImageGrabbed;
                    }

                    moduleInfo.Camera.GrabMulti();
                }

                // Marking Sequence 시작
                IMDefectSignalManager.StartResultSyncTask();

                SystemState.Instance().SetInspectState(InspectState.Run);

                return true;
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                do { LogHelper.Error($"IMInspectRunner::EnterWaitInspection - {ex.GetType().Name} : {ex.Message}"); }
                while ((ex = ex.InnerException) != null);

                ExitWaitInspection();
                throw ex2;
            }
        }

        private void DryRunOpenCV()
        {
            //처음 검사속도가 느릴 수도 있기 때문에 미리 한번 한다.
            var temp = new Image<Gray, byte>(100, 100);
            temp = temp.SmoothBlur(3, 3);
            temp.Dispose();
        }

        private AlgoTaskManagerSetting LoadATMSettings(string fileName)
        {
            string[] paths = new string[]
            {
                UniScanC.Models.ModelManager.Instance().ModelPath,
                UniScanC.Models.ModelManager.Instance().CurrentModel?.Name,
                fileName
            };

            string fullPath;
            if (Array.TrueForAll(paths, f => !string.IsNullOrEmpty(f)))
            {
                fullPath = Path.GetFullPath(Path.Combine(paths));
            }
            else
            {
                fullPath = Path.GetFullPath(Path.Combine(BaseConfig.Instance().ConfigPath, fileName));
            }

            AlgoTaskManagerSetting settings;
            if (!SystemConfig.Instance.OverrideATMSetting)
            {
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"LoadATMSettings - [{fullPath}] not founded.");
                }

                settings = AlgoTaskManagerSetting.Load(fullPath);
            }
            else
            {
                try
                {
                    settings = AlgoTaskManagerSetting.Load(fullPath);
                    throw new Exception("Always Throw");
                }
                catch
                {
                    settings = new AlgoTaskManagerSetting();
                    if (true)
                    // test - 성형검사기
                    {
                        settings.AddTask(
                            new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
                            new UniScanC.Algorithm.RoiFind.RoiFinderParam(), // 1
                            new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam() // 2
                        );

                        // input 0 - LineCalibrator
                        settings.AddLink(new LinkS("ModuleImageDataByte", "ImageDataByte", "LineCalibrator", "ImageDataByte"));

                        // input 1 - RoiFinder
                        settings.AddLink(new LinkS("LineCalibrator", "ImageData", "RoiFinder", "ImageData"));

                        // input 2 - PlainFilmChecker
                        settings.AddLink(new LinkS("LineCalibrator", "ImageData", "PlainFilmChecker", "ImageData"));
                        settings.AddLink(new LinkS("RoiFinder", "RoiMask", "PlainFilmChecker", "RoiMask"));
                        settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "PlainFilmChecker", "FrameNo"));

                        // Result
                        settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "InspectResult", "FrameIndex"));
                        settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "FrameImageData", typeof(ImageData), typeof(BitmapSource)));
                        settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "InspectRegion", typeof(ImageData), typeof(SizeF)));
                        settings.AddLink(new LinkEx("RoiFinder", "RoiMask", "InspectResult", "EdgePos", typeof(RoiMask), typeof(float)));
                        settings.AddLink(new LinkS("RoiFinder", "PatternSize", "InspectResult", "PatternSize"));
                        settings.AddLink(new LinkS("PlainFilmChecker", "DefectList", "InspectResult", "DefectList"));
                    }
                    else
                    // test - 전지테이프표면
                    {
                        //settings.AddTask(
                        //    new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
                        //    new UniScanC.Algorithm.PatternSizeCheck.PatternSizeCheckerParam(), // 1
                        //    new UniScanC.Algorithm.ColorCheck.ColorCheckerParam(), // 2
                        //    new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam(), // 3
                        //    new UniScanC.Algorithm.Base.SetNodeParam<Defect>("UnionNode", UniScanC.Algorithm.Base.ESetNodeType.Union) // 4
                        //);

                        //// input 0 - LineCalibrator
                        //settings.AddLink(new LinkS("ModuleImageDataByte", "ImageDataByte", "LineCalibrator", "ImageDataByte"));

                        //// input 1 - PatternSizeChecker
                        //settings.AddLink(new LinkS("LineCalibrator", "ImageData", "PatternSizeChecker", "ImageData"));

                        //// input 2 - ColorChecker
                        //settings.AddLink(new LinkS("LineCalibrator", "ImageData", "ColorChecker", "ImageData"));
                        //settings.AddLink(new LinkS("PatternSizeChecker", "RoiMask", "ColorChecker", "RoiMask"));
                        //settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "ColorChecker", "FrameNo"));

                        //// input 3 - PlainFilmChecker
                        //settings.AddLink(new LinkS("LineCalibrator", "ImageData", "PlainFilmChecker", "ImageData"));
                        //settings.AddLink(new LinkS("PatternSizeChecker", "RoiMask", "PlainFilmChecker", "RoiMask"));
                        //settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "PlainFilmChecker", "FrameNo"));

                        //// Node 4 - Union
                        //settings.AddLink(new LinkS("ColorChecker", "DefectList", "UnionNode", "List"));
                        //settings.AddLink(new LinkS("PlainFilmChecker", "DefectList", "UnionNode", "List"));

                        //// Result
                        //settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "InspectResult", "FrameIndex"));
                        //settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "FrameImageData", typeof(ImageData), typeof(BitmapSource)));
                        //settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "InspectRegion", typeof(ImageData), typeof(SizeF)));
                        //settings.AddLink(new LinkEx("PatternSizeChecker", "RoiMask", "InspectResult", "EdgePos", typeof(RoiMask), typeof(float)));
                        //settings.AddLink(new LinkS("PatternSizeChecker", "PatternSize", "InspectResult", "PatternSize"));
                        //settings.AddLink(new LinkS("UnionNode", "List", "InspectResult", "DefectList"));

                        //settings.AddLink(new LinkListAdd<Defect>("InspectResult", "DefectList", new (string, string)[] { ("ColorChecker", "DefectList"), ("PlainFilmChecker", "DefectList") }));

                        //settings.AddLink(new LinkS("ColorChecker", "DefectList", "InspectResult", "DefectList"));
                        //settings.AddLink(new LinkS("PlainFilmChecker", "DefectList", "InspectResult", "DefectList"));
                    }

                    if (!string.IsNullOrEmpty(fullPath))
                    {
                        settings.Save(fullPath);
                    }
                }
            }
            return settings;


            //return Task.Run<AlgoTaskManagerSetting>(async () =>
            //{
            //    AlgoTaskManagerSetting settings;
            //    try
            //    {
            //        settings = await AlgoTaskManagerSetting.LoadAsync(fileName);
            //    }
            //    catch (Exception)
            //    {
            //        settings = null;
            //    }

            //    //if (settings == null)
            //    {
            //        settings = new AlgoTaskManagerSetting();
            //        if (false)
            //        // test - 성형검사기
            //        {
            //            settings.AddTask(new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam());
            //            settings.AddTask(new UniScanC.Algorithm.RoiFind.RoiFinderParam());
            //            settings.AddTask(new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam());
            //        }
            //        else if (true)
            //        // test - 전지테이프표면
            //        {
            //            settings.AddTask
            //            (
            //                new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
            //                new UniScanC.Algorithm.PatternSizeCheck.PatternSizeCheckerParam(), // 1
            //                new UniScanC.Algorithm.ColorCheck.ColorCheckerParam(), // 2
            //                new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam()); // 3

            //            // input 0 - LineCalibratorParam
            //            settings.AddLink(new Link(-1, 0, 0, 0)); // ModuleImageDataByte.Out.ImageDataByte -> LineCalibrator.In.ImageDataByte

            //            // input 1 - PatternSizeCheckerParam
            //            settings.AddLink(new Link(0, 0, 1, 0)); // LineCalibrator.Out.ImageData -> PatternSizeChecker.In.ImageData

            //            // input 2 - ColorCheckerParam
            //            settings.AddLink(new Link[]
            //            {
            //                new Link(0, 0, 2, 0), // LineCalibrator.Out.ImageData -> ColorChecker.In.ImageData
            //                new Link(1, 0, 2, 1), // PatternSizeChecker.Out.RoiMask -> ColorChecker.In.RoiMask
            //                new Link(-1, 1, 2, 2) // Camera.Out.FrameNo -> ColorChecker.In.FrameNo
            //            });

            //            // input 3 - PlainFilmCheckerParam
            //            settings.AddLink(new Link[]
            //            {
            //                new Link(0, 0, 3, 0), // LineCalibrator.Out.ImageData -> PlainFilmChecker.In.ImageData
            //                new Link(1, 0, 3, 1), // PatternSizeChecker.Out.RoiMask -> PlainFilmChecker.In.RoiMask
            //                new Link(-1, 1, 3, 2) // Camera.Out.FrameNo -> PlainFilmChecker.In.FrameNo
            //            });
            //        }
            //    }
            //    await settings.SaveAsync(fileName);
            //    return settings;
            //}).Result;
        }

        public override void ExitWaitInspection()
        {
            Debug.WriteLine($"IMInspectRunner::ExitWaitInspection");
            if (SystemState.Instance().OpState == OpState.Idle)
            {
                return;
            }

            // Marking Sequence 종료
            IMDefectSignalManager.StopResultSyncTask();

            ModuleInfo[] moduleInfos = SystemConfig.Instance.ModuleList.ToArray();

            // 카메라 정지
            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                moduleInfo.Camera.Stop();
                moduleInfo.Camera.ImageGrabbed = null;
                if (moduleInfo.UseFrameTrigger)
                {
                    var deviceMonitor = DeviceMonitor.Instance() as DeviceMonitor;
                    deviceMonitor.FrameTriggerChanged = null;
                }
            }
            FrameTriggerService.Stop();
            FrameTriggerService.OnInspectFrameGrabbed -= InspectImageGrabbed;

            cancellationTokenSource?.Cancel();

            // 검사 프로세스 종료
            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                StopATM(moduleInfo);
            }

            Release();

            if (IsLighthandling)
            {
                DeviceManager.Instance().LightCtrlHandler.GetLightCtrl(0).TurnOff();

                var lightValue = new LightValue(DeviceManager.Instance().LightCtrlHandler.NumLight);
                int numLight = DeviceManager.Instance().LightCtrlHandler.NumLight;
                for (int i = 0; i < numLight; ++i)
                {
                    lightValue.Value[i] = 0;
                }
                DeviceManager.Instance().LightCtrlHandler.TurnOn(lightValue);
            }
            base.ExitWaitInspection();
        }

        private void Release()
        {
            //scanNo = 0;
            //grabNo = 0;

            foreach (Stripe stripe in TopStripeList)
            {
                stripe.image.Dispose();
            }
            TopStripeList.Clear();

            foreach (Stripe stripe in StripeList)
            {
                stripe.image.Dispose();
            }
            StripeList.Clear();
        }

        private InspectResult BuildProductResult(ModuleInfo moduleInfo, int frameId)
        {
            //Calibration calibration = SystemManager.Instance().GetCameraCalibration(camera.Index);

            var inspectResult = new InspectResult
            {
                StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                LotNo = LotNo,
                ModuleNo = moduleInfo.ModuleNo,
                FrameIndex = frameId,
                InspectRegion = SizeF.Empty,
                InspectStartTime = DateTime.Now,
                InspectEndTime = DateTime.Now,
                Resolution = new SizeF(moduleInfo.ResolutionWidth, moduleInfo.ResolutionHeight),
                Judgment = IsSkipMode ? Judgment.Skip : Judgment.OK
            };

            //Debug.WriteLine($"IMInspectRunner::BuildProductResult - FrameId: {frameId}");

            return inspectResult;
        }


        // 검사
        public void InspectImageGrabbed(Camera camera)
        {
            //Debug.WriteLine($"IMInspectRunner::InspectImageGrabbed");
            try
            {
                ModuleInfo moduleInfo = SystemConfig.Instance.ModuleList.FirstOrDefault(f => f.Camera == camera);
                if (moduleInfo == null)
                {
                    return;
                }

                if (!(camera.GetGrabbedImage() is Image2D image))
                {
                    return;
                }

                InspectImageGrabbed(moduleInfo, image);
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Exception - {0}", e));
                LogHelper.Debug(LoggerType.Inspection, string.Format("IMInspectRunner::InspectImageGrabbed - Exception - {0}", e));
            }

            //Debug.WriteLine(string.Format("grabNo - {0}", grabNo));
        }

        public void InspectImageGrabbed(ModuleInfo moduleInfo, Image2D image)
        {
            var cameraBufferTag = (CameraBufferTag)image?.Tag;
            int frameNo = (int)cameraBufferTag.FrameId;
            Size size = cameraBufferTag.FrameSize;
            LogHelper.Debug(LoggerType.Inspection, $"IMInspectRunner::InspectImageGrabbed - FrameId: {frameNo}, Size: {size}");

            if (size.Width <= 0 || size.Height <= 0)
            {
                LogHelper.Debug(LoggerType.Inspection, $"IMInspectRunner::InspectImageGrabbed - cameraBufferTag.Size is Error");
                return;
            }

            {
                if (UpdateImageView)
                {
                    Grabbed?.BeginInvoke(moduleInfo.Camera.Index, (ulong)frameNo, InspectRunnerUtil.CreateImageSource(image), null, null);
                }

                if (!AlgoTaskManagerDic.ContainsKey(moduleInfo))
                {
                    return;
                }

                ImageSaveCnt++;

                InspectResult inspectResult = BuildProductResult(moduleInfo, frameNo);
                var inspectBufferSet = new InspectBufferSet(new ImageDataByte(image, size), frameNo);

                AlgoTaskManager algoTaskManager = AlgoTaskManagerDic[moduleInfo];
                algoTaskManager.Enqueue((inspectBufferSet, inspectResult));
            }
        }

        public void OnInspectCompleted(InspectResult inspectResult)
        {
            LogHelper.Debug(LoggerType.Inspection, $"IMInspectRunner::OnInspectCompleted - FrameId: {inspectResult.FrameIndex}, Judgement: {inspectResult.Judgment}");

            // Label IO
            // IM이 CM에게 보내던 시퀀스를 바로 IO로 쏘는 방법으로 변경
            ModuleInfo moduleInfo = SystemConfig.Instance.ModuleList.ToList().Find(x => x.ModuleNo == inspectResult.ModuleNo);
            if (moduleInfo?.UseDefectSignal == true && inspectResult.Judgment != Judgment.Skip)
            {
                IMDefectSignalManager.AddJudgment(moduleInfo, inspectResult);
            }

            DataExporter.ExportDefectData(inspectResult);

            //Grabbed?.BeginInvoke(inspectResult.ModuleNo, (uint)inspectResult.FrameIndex, (UpdateImageView ? inspectResult.DefectImageData : null), null, null);
            AfterInspectCompleted?.Invoke(inspectResult);
        }

        private AlgoTaskManager StartATM(ModuleInfo moduleInfo, AlgoTaskManagerSetting algoTaskManagerSettings, VisionModel visionModel)
        {
            var algoTaskManager = new AlgoTaskManager(moduleInfo, algoTaskManagerSettings, visionModel);
            if (AlgoTaskManagerDic.ContainsKey(moduleInfo))
            {
                AlgoTaskManagerDic[moduleInfo] = algoTaskManager;
            }
            else
            {
                AlgoTaskManagerDic.Add(moduleInfo, algoTaskManager);
            }

            algoTaskManager.Start();
            return algoTaskManager;
        }

        private void StopATM(ModuleInfo moduleInfo)
        {
            if (AlgoTaskManagerDic.ContainsKey(moduleInfo))
            {
                AlgoTaskManagerDic[moduleInfo].WaitAllCompleted();
                AlgoTaskManagerDic[moduleInfo].Stop();
                AlgoTaskManagerDic.Remove(moduleInfo);
            }
        }


        // 더미 데이터
        //private void SendDumyResult(int width, int height, InspectResult inspectResult, string reson = "")
        //{
        //    //Monitor.Enter(thisLock);
        //    if (inspectResult.DefectList.Count > 0)
        //    {
        //        var isSkip = true;
        //        foreach (var dustDefect in inspectResult.DefectList)
        //        {
        //            if (dustDefect.DefectCategories.Count > 0)
        //            {
        //                isSkip &= dustDefect.DefectCategories.First().IsSkip;
        //            }
        //        }
        //        inspectResult.Judgment = isSkip ? Judgment.Skip : Judgment.NG;
        //    }
        //    else
        //    {
        //        inspectResult.Judgment = Judgment.OK;
        //    }

        //    if (IsSkipMode/* || exportTaskQueue.Count >= maxExportQueueNum*/)
        //    {
        //        inspectResult.Judgment = Judgment.Skip;
        //    }

        //    inspectResult.InspectEndTime = DateTime.Now;

        //    // IM이 CM에게 보내던 시퀀스를 바로 IO로 쏘는 방법으로 변경
        //    IMDefectSignalManager.AddJudgment(inspectResult.InspectStartTime, inspectResult.Judgment == Judgment.NG ? true : false, inspectResult.FrameIndex, inspectResult.ModuleNo);

        //    dataExporter.ExportDefectData(inspectResult);

        //    //Debug.WriteLine(string.Format("sent Dumy Result - {0} Because {1}", inspectResult.FrameIndex, reson));
        //}

        // 줄무늬 찾기 (현재는 안씀)
        //private void StripeDefectTaskStart(List<Defect> dustDefects, int taskNum)
        //{
        //    try
        //    {
        //        for (var i = 0; i < topStripeList.Count; ++i)
        //        {
        //            for (var k = 0; k < stripeList.Count; ++k)
        //            {
        //                var resolution = SystemConfig.Instance.ModuleList.First().ResolutionHeight;
        //                if (CheckStripeIsSame(topStripeList[i], stripeList[k], resolution))
        //                {
        //                    if (stripeList[k].typeofStrip == TypeofStrip.Body)
        //                    {
        //                        StripeMerge(topStripeList[i], stripeList[k]);
        //                        topStripeList[i].checkCnt = 0;
        //                    }
        //                    else if (stripeList[k].typeofStrip == TypeofStrip.Tail)
        //                    {
        //                        StripeMerge(topStripeList[i], stripeList[k]);
        //                        topStripeList[i].isContinue = false;
        //                    }
        //                }
        //            }

        //            topStripeList[i].checkCnt++;
        //        }
        //        for (var i = 0; i < topStripeList.Count; ++i)
        //        {
        //            if (topStripeList[i].megedCnt > maxMergeNum || topStripeList[i].grabCnt < (grabNo - 2))
        //            {
        //                topStripeList[i].isContinue = false;
        //                Debug.WriteLine(string.Format("Over Merge Memory!!!!!!!!"));
        //            }
        //        }

        //        // 꼬리를 만나거나 더이상 없을 경우
        //        var defectCnt = 0;
        //        defectCnt = dustDefects.Count;
        //        for (var i = 0; i < topStripeList.Count; ++i)
        //        {
        //            if (topStripeList[i].isContinue == false)
        //            {
        //                defectCnt++;
        //                topStripeList[i].image.MinMax(out var min, out var max, out var minPt, out var maxPt);
        //                var dustDefect = new Defect()
        //                {
        //                    ModuleNo = SystemConfig.Instance.ModuleList.First().ModuleNo,
        //                    FrameIndex = topStripeList[i].dustDefect.FrameIndex,
        //                    Area = topStripeList[i].rectangle.Width * topStripeList[i].rectangle.Height,
        //                    //BoundingRect = Rectangle.Inflate(topStripeList[i].rectangle, -settings.Inflate, 0),
        //                    BoundingRect = Rectangle.Inflate(topStripeList[i].rectangle, 0, 0),
        //                    DefectNo = defectCnt,
        //                    AvgGv = Convert.ToInt32(topStripeList[i].image.GetAverage().Intensity),
        //                    MinGv = Convert.ToInt32(min[0]),
        //                    MaxGv = Convert.ToInt32(max[0]),
        //                    DefectPos = new PointF(topStripeList[i].rectangle.Location.X - leftEdge, topStripeList[i].rectangle.Location.Y),
        //                    DefectImage = UniScanC.Inspect.BmpImaging.CreateBitmapSourceFromBitmap(topStripeList[i].image.ToBitmap()),
        //                };

        //                if (IsSaveImage)
        //                {
        //                    topStripeList[i].image.Save(string.Format("C:\\Image\\M_{0},{1},{2}.bmp", topStripeList[i].beginInspectNum, topStripeList[i].megedCnt, topStripeList[i].image.Height));
        //                }

        //                dustDefects.Add(dustDefect);
        //            }
        //        }

        //        for (var i = 0; i < topStripeList.Count; ++i)
        //        {
        //            if (topStripeList[i].isContinue == false)
        //            {
        //                DeleteStripe(topStripeList, i);
        //                if (i > 0)
        //                {
        //                    i--;
        //                }
        //            }
        //        }

        //        // 나머지 제거
        //        if (stripeList != null)
        //        {
        //            for (var k = 0; k < stripeList.Count; ++k)
        //            {
        //                if (stripeList[k].grabCnt < grabNo + 1)
        //                {
        //                    DeleteStripe(stripeList, k);
        //                    if (k > 0)
        //                    {
        //                        k--;
        //                    }
        //                }
        //            }
        //            stripeList.Clear();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Debug.WriteLine(string.Format("topStripeList - {0} stripeList - {1}", topStripeList.Count, stripeList.Count));
        //        //LogHelper.Debug(LoggerType.Inspection, string.Format("topStripeList - {0} stripeList - {1}", topStripeDic.Count, stripeDic.Count));
        //    }
        //}

        //private bool CheckStripeIsSame(Stripe stripeA, Stripe stripeB, float resolution)
        //{
        //    var needMerge = false;

        //    var PosionA = stripeA.rectangle.Left + stripeA.rectangle.Width / 2;
        //    var PosionB = stripeB.rectangle.Left + stripeB.rectangle.Width / 2;

        //    // 20mm
        //    if (Math.Abs(PosionA - PosionB) <= (20.0f * 1000.0f / resolution) &&
        //        (stripeA.grabCnt + 1) == stripeB.grabCnt)
        //    {
        //        needMerge = true;
        //    }
        //    else
        //    {
        //        needMerge = false;
        //    }

        //    return needMerge;
        //}

        //private void StripeMerge(Stripe stripeA, Stripe stripeB)
        //{
        //    try
        //    {
        //        stripeA.image.ROI = new Rectangle(0, stripeA.image.Height - 2, stripeA.image.Width, 2);
        //        stripeB.image.ROI = new Rectangle(0, 0, stripeB.image.Width, 2);
        //        var centerA_PtX = 0;
        //        var centerB_PtX = 0;

        //        var moment = new MCvMoments();
        //        if (stripeB.image.GetAverage().Intensity < 128)
        //        {
        //            moment = stripeA.image.ThresholdBinaryInv(new Gray(100), new Gray(255)).GetMoments(true);
        //            centerA_PtX = (int)Math.Round(moment.GravityCenter.X);

        //            moment = stripeB.image.ThresholdBinaryInv(new Gray(100), new Gray(255)).GetMoments(true);
        //            centerB_PtX = (int)Math.Round(moment.GravityCenter.X);
        //        }
        //        else
        //        {
        //            moment = stripeA.image.ThresholdBinary(new Gray(160), new Gray(255)).GetMoments(true);
        //            centerA_PtX = (int)Math.Round(moment.GravityCenter.X);

        //            moment = stripeB.image.ThresholdBinary(new Gray(160), new Gray(255)).GetMoments(true);
        //            centerB_PtX = (int)Math.Round(moment.GravityCenter.X);
        //        }

        //        if (centerA_PtX <= 0 || centerB_PtX <= 0)
        //        {
        //            stripeA.isContinue = false;
        //            return;
        //        }

        //        stripeA.image.ROI = Rectangle.Empty;
        //        stripeB.image.ROI = Rectangle.Empty;

        //        var RoiA = new Rectangle(0, 0, stripeA.image.Width, stripeA.image.Height);
        //        var RoiB = new Rectangle(0, 0, stripeB.image.Width, stripeB.image.Height);

        //        RoiA.Offset(-centerA_PtX, 0);
        //        RoiB.Offset(-centerB_PtX, 0);

        //        var newImageRect = new Rectangle(0, 0,
        //        Math.Max(RoiA.Right, RoiB.Right) - Math.Min(RoiA.Left, RoiB.Left),
        //        stripeA.image.Height + stripeB.image.Height);

        //        var tempImg = new Image<Gray, byte>(newImageRect.Width, newImageRect.Height);
        //        tempImg.SetValue(new Gray(128));
        //        var copyRoi = new Rectangle();

        //        var minX = Math.Min(RoiA.Left, RoiB.Left);
        //        RoiA.Offset(-minX, 0);
        //        RoiB.Offset(-minX, 0);

        //        copyRoi = RoiA;
        //        tempImg.ROI = copyRoi;
        //        if (tempImg.ROI.Size != copyRoi.Size)
        //        {
        //            Debug.WriteLine(string.Format("StripeMerged - {0},{1}", tempImg.ROI.Width, copyRoi.Size.Width));
        //            LogHelper.Debug(LoggerType.Inspection, string.Format("StripeMerged - {0},{1}", tempImg.ROI.Width, copyRoi.Size.Width));
        //        }
        //        stripeA.image.CopyTo(tempImg);

        //        copyRoi = RoiB;
        //        copyRoi.Y = RoiA.Height;
        //        tempImg.ROI = copyRoi;
        //        if (tempImg.ROI.Size != copyRoi.Size)
        //        {
        //            Debug.WriteLine(string.Format("StripeMerged - {0},{1}", tempImg.ROI.Width, copyRoi.Size.Width));
        //            LogHelper.Debug(LoggerType.Inspection, string.Format("StripeMerged - {0},{1}", tempImg.ROI.Width, copyRoi.Size.Width));
        //        }
        //        stripeB.image.CopyTo(tempImg);

        //        stripeA.image.Dispose();

        //        tempImg.ROI = System.Drawing.Rectangle.Empty;
        //        stripeA.image = tempImg;

        //        stripeA.rectangle.X = stripeB.rectangle.X;
        //        stripeA.rectangle.Y = stripeA.rectangle.Y;
        //        stripeA.rectangle.Width = Math.Max(stripeA.rectangle.Width, stripeB.rectangle.Width);
        //        stripeA.rectangle.Height = stripeA.rectangle.Height + stripeB.rectangle.Height;
        //        stripeA.endInspectNum = stripeB.beginInspectNum;
        //        stripeA.grabCnt = stripeB.grabCnt;

        //        stripeA.isContinue = stripeB.typeofStrip == TypeofStrip.Tail ? false : true;

        //        stripeA.megedCnt++;

        //        Debug.WriteLine(string.Format("StripeMerged - {0},{1}", stripeA.beginInspectNum, stripeA.megedCnt));
        //        LogHelper.Debug(LoggerType.Inspection, string.Format("StripeMerged - {0},{1}", stripeA.beginInspectNum, stripeA.megedCnt));
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(string.Format("Exception - {0}", e));
        //        LogHelper.Debug(LoggerType.Inspection, string.Format("Exception - {0}", e));
        //    }
        //}

        //private void DeleteStripe(List<Stripe> stripes, int delteIndex)
        //{
        //    stripes[delteIndex].image.Dispose();
        //    stripes.RemoveAt(delteIndex);
        //}

        // 이미지 그랩
        public void GrabImageGrabbed(Camera camera)
        {
            //Debug.WriteLine("IMInspectRunner::GrabImageGrabbed");
            if (!(camera.GetGrabbedImage() is Image2D image))
            {
                return;
            }

            ulong frameNo = ((CameraBufferTag)image.Tag).FrameId;
            Size frameSize = ((CameraBufferTag)image.Tag).FrameSize;
            LogHelper.Debug(LoggerType.Inspection, $"IMInspectRunner::GrabImageGrabbed - FrameId: {frameNo}, FrameSize: {frameSize}");

            //byte[] data;
            //if (image.DataPtr == IntPtr.Zero)
            //{
            //    data = (byte[])image.Data.Clone();
            //}
            //else
            //{
            //    data = new byte[image.Width * image.Height];
            //    for (int h = 0; h < image.Height; ++h)
            //    {
            //        Marshal.Copy(IntPtr.Add(image.DataPtr, image.Pitch * h), data, image.Width * h, image.Width);// 100ms
            //    }
            //}

            // 티칭을 위한 이미지를 남기기 위함
            //if (LastGrabbedImage != null)
            //    LastGrabbedImage.Dispose();
            image.ConvertFromDataPtr();
            LastGrabbedImage = (Image2D)image.ClipImage(new Rectangle(Point.Empty, frameSize));

            if (UpdateImageView)
            {
                Grabbed?.Invoke(camera.Index, frameNo, InspectRunnerUtil.CreateImageSource(LastGrabbedImage));
            }
        }

        public void StartGrab(int moduleIndex)
        {
            var moduleInfoList = new List<ModuleInfo>();
            if (moduleIndex < 0)
            {
                moduleInfoList.AddRange(SystemConfig.Instance.ModuleList);
            }
            else
            {
                moduleInfoList.Add(SystemConfig.Instance.ModuleList.FirstOrDefault(f => f.ModuleNo == moduleIndex));
            }

            StartGrab(moduleInfoList.ToArray());
        }

        private void StartGrab(ModuleInfo[] moduleInfos)
        {
            Array.ForEach(moduleInfos, f =>
             {
                 Camera camera = f.Camera;
                 camera.ImageGrabbed = GrabImageGrabbed;
                 switch (f.TriggerMode)
                 {
                     case TriggerMode.Software:
                         camera.SetTriggerMode(TriggerMode.Software, TriggerType.RisingEdge);
                         //camera.SetExposureTime(150);
                         camera.SetAcquisitionLineRate(5120);
                         break;
                     case TriggerMode.Hardware:
                         //camera.SetExposureTime(250);
                         camera.SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);
                         break;
                 }
                 camera.GrabMulti();
             });
        }

        public void StopGrab(int moduleIndex)
        {
            var moduleInfoList = new List<ModuleInfo>();
            if (moduleIndex < 0)
            {
                moduleInfoList.AddRange(SystemConfig.Instance.ModuleList);
            }
            else
            {
                moduleInfoList.Add(SystemConfig.Instance.ModuleList.FirstOrDefault(f => f.ModuleNo == moduleIndex));
            }

            StopGrab(moduleInfoList.ToArray());
        }

        private void StopGrab(ModuleInfo[] moduleInfos)
        {
            Array.ForEach(moduleInfos, f =>
             {
                 Camera camera = f.Camera;
                 camera.Stop();
                 camera.ImageGrabbed = null;
             });
        }

        public override void Inspect(int triggerIndex = -1)
        {
            throw new NotImplementedException();
        }

        public InspectResult TeachInspect(ModuleInfo moduleInfo)
        {
            // 검사 프로세스 생성
            //AlgoTaskManagerSetting algoTaskManagerSettings = LoadATMSettings("test.json");
            AlgoTaskManagerSetting algoTaskManagerSettings = LoadATMSettings($"{moduleInfo.Topic}.json");
            AlgoTaskManager algoTaskManager = StartATM(moduleInfo, algoTaskManagerSettings, VisionModels[moduleInfo.ModuleNo]);
            algoTaskManager.OnInspectCompleted += (InspectResult r) =>
            {

            };

            if (!(LastGrabbedImage is Image2D image))
            {
                throw new Exception("Grab_IMAGE_MESSAGE");
            }

            if (image.DataPtr == IntPtr.Zero)
            {
                image.ConvertFromData();
            }

            var cameraBufferTag = (CameraBufferTag)image.Tag;
            Size size = cameraBufferTag.FrameSize;
            int frameNo = (int)cameraBufferTag.FrameId;

            byte[] data;
            if (image.DataPtr != IntPtr.Zero)
            {
                data = new byte[image.Width * image.Height];
                for (int y = 0; y < image.Height; y++)
                {
                    int srcOffset = y * image.Pitch;
                    int dstOffset = y * image.Width;

                    Marshal.Copy(image.DataPtr + srcOffset, data, dstOffset, image.Width);
                }
                //Buffer.BlockCopy(image.Data, y * image.Pitch, data, y * image.Width, image.Width);
            }
            else
            {
                data = image.Data;
            }

            InspectResult inspectResult = BuildProductResult(moduleInfo, -1);
            var inspectBufferSet = new InspectBufferSet(new ImageDataByte(image, size), -1);

            bool ok = AlgoTaskManagerDic[moduleInfo].Enqueue((inspectBufferSet, inspectResult));
            if (ok)
            {
                AlgoTaskManagerDic[moduleInfo].Stop();
            }

            StopATM(moduleInfo);

            return inspectResult;
        }
    }
}

// 코드 키핑 
//private Rectangle? FindROI(AlgoImage algoImage)
//{
//    int left = 0;
//    int right = 0;
//    int top = 0;
//    int bottom = 0;

//    ImageProcessing ip = ImageProcessingPool.GetImageProcessing(algoImage);
//    float[] profileY = ip.Projection(algoImage, TwoWayDirection.Vertical, ProjectionType.Mean); //좌우로 프로젝션 => 데이터는 세로크기
//    EdgeFinder(profileY, ref top, ref bottom, "Y");

//    Rectangle roi = new Rectangle(0, 0, algoImage.Width, algoImage.Height);
//    if (top > -1 && bottom > -1) // 여긴 들어오면 안되지..
//    {
//        return null;
//    }
//    else if (top > -1) //테이프 위쪽 끝이 보이고 있음 (┌┐↓).
//    {
//        roi.Y = top;
//        roi.Height = roi.Height - top;
//    }
//    else if (bottom > -1) //테이프 아래쪽 끝이 보이고 있음. (└┘↓)
//    {
//        roi.Height = bottom;
//    }
//    else // 엣지없다? 테이프중간 이미지 이거나, 아예 테이프가 없을경우임..
//    {

//    }

//    var roiImage = algoImage.GetChildImage(roi);
//    float[] profileX = ip.Projection(roiImage, TwoWayDirection.Horizontal, ProjectionType.Mean); //위아래로 프로젝션 => 데이터는 가로크기
//    EdgeFinder(profileX, ref left, ref right, "X");
//    roiImage.Dispose();

//    if (left < 0 || right < 0)
//        return null;

//    Rectangle rc = new Rectangle(left, roi.Y, right - left, roi.Height);
//    return rc;
//}
////left, right, bottom top
//private void EdgeFinder(float[] data, ref int falling, ref int rising, string ff)
//{
//    int risingEdge = 0;
//    int fallingEdge = data.Length;
//    Image<Gray, float> profileImg = null;
//    Image<Gray, float> edgeImg = null;
//    try
//    {
//        profileImg = new Image<Gray, float>(data.Length, 1);
//        for (int i = 0; i < data.Length; ++i)
//        {
//            profileImg.Data[0, i, 0] = data[i];
//        }
//        // data --%로 필터링
//        profileImg = profileImg.SmoothBlur(11, 1, true);
//        edgeImg = profileImg.Sobel(1, 0, 3);

//        edgeImg = edgeImg.SmoothBlur(3, 1, true);
//        //CvInvoke.Normalize(edgeImg, edgeImg, 0, 200, NormType.MinMax);

//        double min = 0;
//        double max = 0;
//        Point minPnt = new Point();
//        Point maxPnt = new Point();

//        CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);
//        double avg = 20;// (max - min) / 2.0f;

//        if (max > avg)
//            risingEdge = maxPnt.X; //right or bottom(start)
//        else
//            risingEdge = -1;

//        if (Math.Abs(min) > avg)
//            fallingEdge = minPnt.X; // left or top (end)
//        else
//            fallingEdge = -1;

//        rising = risingEdge;
//        falling = fallingEdge;
//    }
//    catch (Exception e)
//    {
//        rising = 0;
//        falling = data.Length;
//    }
//    finally
//    {
//        edgeImg.Dispose();
//        profileImg.Dispose();
//    }

//    //Debug.WriteLine(string.Format("EdgeFinder(byte) - {0}, {1}", left, right));
//    if (IsSaveImage)
//    {
//        StreamWriter csvFileWriter = new StreamWriter("C:\\Image\\edge.TXT", true, Encoding.Default);
//        string oneLine = string.Format("EdgeFinder(float) - {0}, {1}", fallingEdge, risingEdge);
//        csvFileWriter.WriteLine(oneLine);
//        csvFileWriter.Flush();
//        csvFileWriter.Close();
//    }
//}