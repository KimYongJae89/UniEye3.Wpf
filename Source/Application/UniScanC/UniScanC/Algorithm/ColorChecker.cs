using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.ColorCheck
{
    public class Inputs : InputOutputs<ImageData, RoiMask, int>
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }
        public RoiMask RoiMask { get => Item2; set => Item2 = value; }
        public int FrameNo { get => Item3; set => Item3 = value; }

        public Inputs() : base("ImageData", "RoiMask", "FrameNo") { }

        public Inputs(ImageData imageData, RoiMask roiMask, int frameNo) : this()
        {
            SetValues(imageData, roiMask, frameNo);
        }
    }

    public class Outputs : InputOutputs<float[], List<Defect>>, IResultBufferItem
    {
        public float[] BrightDiffs { get => Item1; set => Item1 = value; }
        public List<Defect> DefectList { get => Item2; set => Item2 = value; }

        public Outputs() : base("BrightDiffs", "DefectList")
        {
            BrightDiffs = new float[0];
            DefectList = new List<Defect>();
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            return true;
        }

        public void Return(InspectBufferPool bufferPool)
        {

        }

        public void CopyFrom(IResultBufferItem from)
        {

        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, $"ColorCheck.Outputs::SaveDebugInfo");
            using (var fs = new FileStream(Path.Combine(debugContext.Path, "ColorCheck.txt"), FileMode.Append, FileAccess.Write))
            {
                foreach (float f in BrightDiffs)
                {
                    string str = $"Frame, {debugContext.FrameNo}, Diffs, {f}{Environment.NewLine}";
                    byte[] bytes = Encoding.Default.GetBytes(str);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }

    [AlgorithmBaseParam]
    public class ColorCheckerParam : AlgorithmBaseParam<ColorChecker, Inputs, Outputs>
    {
        public float FrameMarginL { get; set; }
        public float FrameMarginT { get; set; }
        public float FrameMarginR { get; set; }
        public float FrameMarginB { get; set; }

        public float ThresholdColor { get; set; }
        public int ColorAverageCount { get; set; }

        public ColorCheckerParam()
        {
            FrameMarginL = 0;
            FrameMarginT = 0;
            FrameMarginR = 0;
            FrameMarginB = 0;

            ThresholdColor = 10;
            ColorAverageCount = 3;
        }

        public ColorCheckerParam(ColorCheckerParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
            FrameMarginL = visionModel.FrameMarginL;
            FrameMarginT = visionModel.FrameMarginT;
            FrameMarginR = visionModel.FrameMarginR;
            FrameMarginB = visionModel.FrameMarginB;

            ThresholdColor = visionModel.ThresholdColor;
            ColorAverageCount = visionModel.ColorAverageCount;
        }

        public override INodeParam Clone()
        {
            return new ColorCheckerParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (ColorCheckerParam)algorithmBaseParam;
            Name = param.Name;
            FrameMarginL = param.FrameMarginL;
            FrameMarginT = param.FrameMarginT;
            FrameMarginR = param.FrameMarginR;
            FrameMarginB = param.FrameMarginB;
            ThresholdColor = param.ThresholdColor;
            ColorAverageCount = param.ColorAverageCount;
        }
    }

    public class ColorChecker : AlgorithmBase<Inputs, Outputs>
    {
        public new ColorCheckerParam Param => (ColorCheckerParam)base.Param;

        public override int RequiredBufferCount => 0;

        private List<float> meanList;
        private Statistic.Statistic statisticAlgorithm = null;

        public ColorChecker(ModuleInfo moduleInfo, ColorCheckerParam param) : base(moduleInfo, param)
        {
            meanList = new List<float>();

            statisticAlgorithm = new Statistic.Statistic(moduleInfo);
        }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                if (input.RoiMask == null)
                {
                    input.RoiMask = new RoiMask(null, new RectangleF[] { new RectangleF(new PointF(0, 0), input.ImageData.Size) });
                }

                var statInputs = new Statistic.Inputs(input.ImageData, input.RoiMask);
                var statOutputs = new Statistic.Outputs();
                bool good = statisticAlgorithm.Run(statInputs, ref statOutputs, null);
                if (!good)
                {
                    return false;
                }

                int frameNo = input.FrameNo;
                Statistic.StatisticResult[] statResult = statOutputs.GetValue<Statistic.StatisticResult[]>("Values");

                AlgoImage algoImage = input.ImageData.Image;
                RectangleF[] rois = input.RoiMask.ROIs;
                float[] colorDiffs = output.BrightDiffs = new float[rois.Length];
#if DEBUG
                //Directory.CreateDirectory(@"C:\temp\colorChecker_src");
                //algoImage.Save($@"C:\temp\colorChecker_src\{frameNo}.bmp");
                //Array.ForEach(rois, f => Console.WriteLine($"{frameNo} - {f}"));
#endif
                int minimumHeight = 20;
                ImageProcessing ip = ImageProcessingPool.GetImageProcessing(algoImage);
                for (int i = 0; i < rois.Length; i++)
                {
                    // roi 높이에 따른 가중치를 주면 나아지려나..?
                    if (rois[i].Height < minimumHeight)
                    {
                        continue;
                    }

                    Statistic.StatisticResult statistic = statResult[i];
                    float average = statResult[i].Mean;
                    LogHelper.Debug(LoggerType.Inspection, $"ColorChecker::Run - Average: {average}");
                    colorDiffs[i] = 0;
                    if (meanList.Count >= Param.ColorAverageCount)
                    {
                        float averageMean = meanList.Average();
                        colorDiffs[i] = Math.Abs(averageMean - average);
                        LogHelper.Debug(LoggerType.Inspection, $"ColorChecker::Run - Frame: {frameNo}, ROI[{i}]: {colorDiffs[i]}");

                        if (colorDiffs[i] > Param.ThresholdColor)
                        {
                            var rect = Rectangle.Round(rois[i]);
                            if (rect.Width == 0 || rect.Height == 0)
                            {
                                continue;
                            }

                            BitmapSource bitmapSource = null;
                            using (AlgoImage defectImage = algoImage.GetChildImage(rect))
                            {
                                bitmapSource = defectImage.ToBitmapSource();
                            }

                            var dustDefect = new Defect()
                            {
                                FrameIndex = input.FrameNo,
                                ModuleNo = ModuleInfo.ModuleNo,
                                DefectType = EDefectType.Dust,
                                DefectPos = DynMvp.Base.DrawingHelper.CenterPoint(rois[i]),
                                BoundingRect = rois[i],
                                Area = rois[i].Width * rois[i].Height,
                                MinGv = Convert.ToInt32(statistic.Min),
                                MaxGv = Convert.ToInt32(statistic.Max),
                                AvgGv = Convert.ToInt32(statistic.Mean),
                                DefectImage = bitmapSource,

                                ImageClipRect = rect,
                                TouchHeadRect = Rectangle.Intersect(rect, new Rectangle(0, 0, input.ImageData.Size.Width, 1)),
                                TouchTailRect = Rectangle.Intersect(rect, new Rectangle(0, input.ImageData.Size.Height - 1, input.ImageData.Size.Width, 1))
                            };
                            dustDefect.DefectCategories.Add(DefectCategory.GetColorCategory());
                            output.DefectList.Add(dustDefect);
                            LogHelper.Debug(LoggerType.Inspection, $"ColorChecker::Run - Founded");
                        }
                    }

                    if (Array.Exists(rois, f => f.Height > minimumHeight))
                    {
                        meanList.Add(average);
                        meanList.Sort();
                        // 평균에서 먼 친구를 제외함.
                        if (meanList.Count > Param.ColorAverageCount)
                        {
                            int lastIdx = meanList.Count - 1;
                            int firstIdx = 0;
                            double firstDiff = Math.Abs(meanList[firstIdx] - meanList[firstIdx + 1]);
                            double lastDiff = Math.Abs(meanList[lastIdx] - meanList[lastIdx - 1]);
                            if (firstDiff > lastDiff)
                            {
                                meanList.RemoveAt(0);
                            }
                            else
                            {
                                meanList.RemoveAt(meanList.Count - 1);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"ColorChecker::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }
    }
}
