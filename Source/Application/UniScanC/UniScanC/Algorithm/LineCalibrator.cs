using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using DynMvp.Vision.Vision.SIMD;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.LineCalibrate
{
    public class Inputs : InputOutputs<ImageDataByte>
    {
        public ImageDataByte ImageDataByte { get => Item1; set => Item1 = value; }

        public Inputs() : base("ImageDataByte") { }

        public Inputs(ImageDataByte imageDataByte) : this()
        {
            SetValues(imageDataByte);
        }
    }

    public class Outputs : InputOutputs<ImageData>, IResultBufferItem
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }

        public Outputs() : base("ImageData")
        {
            ImageData = new ImageData();
        }

        public void CopyFrom(IResultBufferItem from) { }

        public bool Request(InspectBufferPool bufferPool)
        {
            var algoImages = new AlgoImage[1];
            if (bufferPool.RequestBuffers(algoImages, 1))
            {
                ImageData.SetValue("Image", algoImages[0]);
                return true;
            }
            return false;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            bufferPool.ReturnBuffer(ImageData.Image);
            ImageData.SetValue<ImageData>("Image", null);
        }

        public void SaveDebugInfo(DebugContextC debugContextC)
        {
            LogHelper.Debug(LoggerType.Inspection, $"LineCalibrate.Outputs::SaveDebugInfo");
            ImageData.Image.Save($"{debugContextC.FrameNo}.bmp", new DebugContext(true, Path.Combine(debugContextC.Path, "LineCalibrate")));
        }
    }

    [AlgorithmBaseParam]
    public class LineCalibratorParam : AlgorithmBaseParam<LineCalibrator, Inputs, Outputs>
    {
        public bool SkipFirstImage { get; set; }
        public bool GpuProcessing { get; set; }
        public float CalibrateFrameCnt { get; set; }
        public int MaxDegreeOfParallelism { get; set; }

        public float FrameMarginL { get; set; }
        public float FrameMarginT { get; set; }
        public float FrameMarginR { get; set; }
        public float FrameMarginB { get; set; }

        public float PatternMarginX { get; set; }
        public float PatternMarginY { get; set; }

        public float TargetIntensity { get; set; }
        public float OutTargetIntensity { get; set; }

        public LineCalibratorParam() : base()
        {
            GpuProcessing = false;
            CalibrateFrameCnt = 0;
            MaxDegreeOfParallelism = 5;

            FrameMarginL = 0;
            FrameMarginT = 0;
            FrameMarginR = 0;
            FrameMarginB = 0;

            PatternMarginX = 10;
            PatternMarginY = 10;

            TargetIntensity = 128;
            OutTargetIntensity = 255;
        }

        public LineCalibratorParam(LineCalibratorParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
            SkipFirstImage = visionModel.SkipFirstImage;
            GpuProcessing = visionModel.GpuProcessing;
            CalibrateFrameCnt = visionModel.CalibrateFrameCnt;
            MaxDegreeOfParallelism = visionModel.MaxDegreeOfParallelism;

            FrameMarginL = visionModel.FrameMarginL;
            FrameMarginT = visionModel.FrameMarginT;
            FrameMarginR = visionModel.FrameMarginR;
            FrameMarginB = visionModel.FrameMarginB;

            PatternMarginX = (float)visionModel.PatternMarginX;
            PatternMarginY = (float)visionModel.PatternMarginY;

            TargetIntensity = visionModel.TargetIntensity;
            OutTargetIntensity = visionModel.OutTargetIntensity;
        }

        public override INodeParam Clone()
        {
            return new LineCalibratorParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (LineCalibratorParam)algorithmBaseParam;
            Name = param.Name;
            GpuProcessing = param.GpuProcessing;
            CalibrateFrameCnt = param.CalibrateFrameCnt;
            MaxDegreeOfParallelism = param.MaxDegreeOfParallelism;
            FrameMarginL = param.FrameMarginL;
            FrameMarginT = param.FrameMarginT;
            FrameMarginR = param.FrameMarginR;
            FrameMarginB = param.FrameMarginB;
            PatternMarginX = param.PatternMarginX;
            PatternMarginY = param.PatternMarginY;
            TargetIntensity = param.TargetIntensity;
            OutTargetIntensity = param.OutTargetIntensity;
        }
    }

    public class LineCalibrator : AlgorithmBase<Inputs, Outputs>
    {
        public new LineCalibratorParam Param => (LineCalibratorParam)base.Param;

        public override int RequiredBufferCount => 1;

        // TDI 카메라를 사용하거나 Partial Close를 사용할 경우 이미지가 온전하지 못한 경우가 있어서 첫 이미지는 스킵한다.
        private bool isFristImage;
        private bool isCalibrated;
        private List<float[]> profileList; // N-Frame의 프로파일 데이터 저장
        private float[] calibrationDatas; // 최종 프로파일 데이터
        private byte[] calibrationDatasSIMD; // 최종 프로파일 데이터 (SIMD 전용)

        public LineCalibrator(ModuleInfo moduleInfo, LineCalibratorParam param) : base(moduleInfo, param)
        {
            isFristImage = false;
            isCalibrated = false;
            profileList = new List<float[]>();
            calibrationDatas = null;
            calibrationDatasSIMD = null;
        }

        public override AlgoImage[] BuildBuffer()
        {
            LogHelper.Debug(LoggerType.Inspection, $"LineCalibrator::BuildBuffer - GpuProcessing: {Param.GpuProcessing}");

            if (Param.GpuProcessing)
            {
                Size size = ModuleInfo.BufferSize;
                CudaImage cudaImage = new CudaDepthImage<byte>();
                cudaImage.Alloc(size.Width, size.Height);
                return new AlgoImage[] { cudaImage };
            }

            return base.BuildBuffer();
        }

        public void Release()
        {
            profileList.Clear();
            profileList = null;

            calibrationDatas = null;
            calibrationDatasSIMD = null;
        }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                if (Param.SkipFirstImage && !isFristImage)
                {
                    isFristImage = true;
                    return false;
                }

                AlgoImage outImage = output.ImageData.Image;
                output.ImageData.SetValue(1, input.ImageDataByte.Size);

                byte[] inputBytes = input.ImageDataByte.Data;
                Size inputSize = input.ImageDataByte.Size;
                if (Param.TargetIntensity < 0 || Param.CalibrateFrameCnt == 0)
                {
                    outImage.SetByte(inputBytes);
                    return true;
                }

                if (!isCalibrated)
                {
                    outImage.SetByte(inputBytes);
                    ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(outImage);

                    float l = Helpers.UnitConvertor.Mm2Px(Param.FrameMarginL, ModuleInfo.ResolutionWidth);
                    if (ModuleInfo.CamPos == ECamPosition.Mid || ModuleInfo.CamPos == ECamPosition.Right)
                    {
                        l = 0;
                    }
                    float t = Helpers.UnitConvertor.Mm2Px(Param.FrameMarginT, ModuleInfo.ResolutionHeight);
                    float r = inputSize.Width - Helpers.UnitConvertor.Mm2Px(Param.FrameMarginR, ModuleInfo.ResolutionWidth);
                    if (ModuleInfo.CamPos == ECamPosition.Mid || ModuleInfo.CamPos == ECamPosition.Left)
                    {
                        r = inputSize.Width;
                    }
                    float b = inputSize.Height - Helpers.UnitConvertor.Mm2Px(Param.FrameMarginB, ModuleInfo.ResolutionHeight);
                    var roiRect = Rectangle.Round(RectangleF.FromLTRB(l, t, r, b));

                    using (AlgoImage rotImage = outImage.GetChildImage(roiRect))
                    {
                        float[] profile = imageProcessing.Projection(rotImage, TwoWayDirection.Horizontal, ProjectionType.Mean);
                        profileList.Add(profile);
                    }

                    if (profileList.Count >= Param.CalibrateFrameCnt)
                    {
                        isCalibrated = Calibration();
                    }
                }

                if (isCalibrated)
                {
                    AlgoImage preProcess = workingBuffers[0];
                    if (preProcess is CudaImage cudaImage)
                    {
                        //var swSetByte = Stopwatch.StartNew();
                        cudaImage.SetByte(inputBytes); //CPU->GPU Upload
                        //LogHelper.Debug(LoggerType.Inspection, $"LineCalibrator::Run - CudaImage.SetByte: { swSetByte.ElapsedMilliseconds}[ms]");

                        //var swMathMul = Stopwatch.StartNew();
                        CudaMethods.CUDA_MATH_MUL(cudaImage.ImageID, calibrationDatas);
                        //LogHelper.Debug(LoggerType.Inspection, $"LineCalibrator::Run - CudaImage.CUDA_MATH_MUL: { swMathMul.ElapsedMilliseconds}[ms]");

                        //var swCopyByte = Stopwatch.StartNew();
                        cudaImage.CopyByte(inputBytes); //GPU->CPU Download
                        //LogHelper.Debug(LoggerType.Inspection, $"LineCalibrator::Run - CudaImage.CopyByte: { swCopyByte.ElapsedMilliseconds}[ms]");
                    }
                    else
                    {
                        // SIMD 기능을 사용하여 알고리즘 진행
                        if (true)
                        {
                            SIMDHelper.IterateProduct(inputBytes, calibrationDatasSIMD, inputBytes, input.ImageDataByte.Size);
                        }
                        else
                        {
                            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Param.MaxDegreeOfParallelism };
                            Parallel.For(0, inputBytes.Length, parallelOptions, i =>
                            {
                                float v = inputBytes[i] * calibrationDatas[i % calibrationDatas.Length];
                                inputBytes[i] = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, v));
                            });
                        }
                    }
                    outImage.SetByte(inputBytes);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"LineCalibrator::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }

            return false;
        }

        private bool Calibration()
        {
            profileList.RemoveAll(f => f.Length == 0);

            if (profileList.Count == 0)
            {
                return false;
            }

            int count = profileList.Count;
            int length = profileList.Min(f => f.Length);

            Image<Gray, float> profileImg = null;
            Image<Gray, float> smoothBlur = null;
            Image<Gray, float> sobel = null;
            try
            {
                profileImg = new Image<Gray, float>(length, 1);
                profileImg.SetZero();
                profileList.ForEach(f =>
                {
                    for (int x = 0; x < length; x++)
                    {
                        //각 프로파일들의 값을 전부다 누적시킨다.
                        profileImg.Data[0, x, 0] += f[x];
                    }
                });

                //값 평균
                profileImg = profileImg / (float)count;

                //필터처리
                sobel = profileImg.SmoothBlur(5, 1).Sobel(1, 0, 31);
                smoothBlur = profileImg.SmoothBlur(51, 1);

                // 패턴을 찾고 패턴 영역만 보정한다??
                float[] profile4EdgeFinder = new float[length];
                Buffer.BlockCopy(smoothBlur.ManagedArray, 0, profile4EdgeFinder, 0, length * sizeof(float));

                int threshold = (int)Math.Round((Param.TargetIntensity + Param.OutTargetIntensity) / 2.0f);
                (int leftEdge, int rightEdge) = Algorithm.Simple.HorizentalEdgeFinder.Find(profile4EdgeFinder, ModuleInfo.CamPos, threshold);

                int cameraWidth = ModuleInfo.Camera.ImageSize.Width;

                float leftMargin = Param.FrameMarginL * 1000 / ModuleInfo.ResolutionWidth;
                leftEdge = Math.Min(cameraWidth, Convert.ToInt32(Math.Ceiling(leftEdge + leftMargin)));
                if (ModuleInfo.CamPos == ECamPosition.Mid || ModuleInfo.CamPos == ECamPosition.Right)
                {
                    leftEdge = 0;
                    leftMargin = 0;
                }

                float rightMargin = Param.FrameMarginR * 1000 / ModuleInfo.ResolutionWidth;
                rightEdge = Math.Max(0, Convert.ToInt32(Math.Ceiling(rightEdge - rightMargin)));
                if (ModuleInfo.CamPos == ECamPosition.Mid || ModuleInfo.CamPos == ECamPosition.Left)
                {
                    rightEdge = cameraWidth;
                    rightMargin = 0;
                }

                // 이중 동작 (RoiFInder 나 PatternSizeChecker에서 PatternMargin 만큼을 빼고 있음)
                //switch (ModuleInfo.CamPos)
                //{
                //    case ECamPosition.Left:
                //        leftEdge += (int)Helpers.UnitConvertor.Um2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                //        rightEdge = profile4EdgeFinder.Length - 1;
                //        break;

                //    case ECamPosition.Right:
                //        leftEdge = 0;
                //        rightEdge -= (int)Helpers.UnitConvertor.Um2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                //        break;

                //    case ECamPosition.OneCam:
                //        leftEdge += (int)Helpers.UnitConvertor.Um2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                //        rightEdge -= (int)Helpers.UnitConvertor.Um2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                //        break;

                //    case ECamPosition.Mid:
                //    default:
                //        break;
                //}

                // TODO:[김태현] 모서리 부분에서 튀는 경향이 있음. 패턴사이즈만큼 더 짤라서 검사를 한다면 문제 없지만 모든 영역을 검사해야된다면 문제가 될 수 있음.
                // ▶ 카메라 캘을 하는것이 목적이라면 최초 캘을 하고 나서 그 값을 고정으로 쓰도록 하기.
                calibrationDatas = new float[cameraWidth];
                calibrationDatasSIMD = new byte[cameraWidth];
                for (int i = 0; i < calibrationDatas.Length; ++i)
                {
                    if ((leftEdge <= i) && (i <= rightEdge))
                    {
                        calibrationDatas[i] = Param.TargetIntensity / smoothBlur.Data[0, i - (int)leftMargin, 0];
                        calibrationDatasSIMD[i] = Convert.ToByte(Math.Min(byte.MaxValue, Param.TargetIntensity / smoothBlur.Data[0, i - (int)leftMargin, 0] * 100));
                    }
                    else
                    {
                        calibrationDatas[i] = Param.OutTargetIntensity;
                        calibrationDatasSIMD[i] = Convert.ToByte(Math.Min(byte.MaxValue, Param.OutTargetIntensity * 100));
                    }
                }
#if DEBUG
                try
                {
                    string path = @"D:\DalsaImageSave\calibrationDatas.csv";
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    //File.WriteAllText(path, string.Join(Environment.NewLine, calibrationDatas));
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"LineCalibrator::Calibration - {ex.GetType().Name}: {ex.Message}");
                }
#endif
                #region 코드키핑, 디버깅 이미지 저장모드일 때 사용예정
                //if (SystemConfig.Instance.IsSaveDebugData)
                //{
                //    string tempPath = UniEye.Base.Config.PathConfig.Instance().Temp;

                //    smoothBlur.Save(Path.Combine(tempPath, "smoothBlur.JPG"));
                //    sobel.Save(Path.Combine(tempPath, "sobel.JPG"));
                //    profileImg.Save(Path.Combine(tempPath, "profileImg.JPG"));

                //    string csvFile = Path.Combine(tempPath, "calibrationDatas.csv");
                //    if (File.Exists(csvFile))
                //        File.Delete(csvFile);

                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendLine("Index, Data, Profile, Sobel, Blur");
                //    for (int i = 0; i < calibrationDatas.Length; ++i)
                //    {
                //        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}",
                //            i, calibrationDatas[i],
                //            profileImg.Data[0, i, 0],
                //            sobel.Data[0, i, 0],
                //            smoothBlur.Data[0, i, 0]));
                //    }

                //    using (StreamWriter csvFileWriter = new StreamWriter(csvFile, true, Encoding.Default))
                //        csvFileWriter.WriteLine(sb.ToString());
                //}
                #endregion
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                smoothBlur?.Dispose();
                sobel?.Dispose();
                profileImg?.Dispose();
            }
            return true;
        }


    }
}