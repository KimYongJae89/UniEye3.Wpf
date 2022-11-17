using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Struct;

namespace UniScanC.Algorithm.Statistic
{
    public class StatisticResult
    {
        public bool Valid { get; set; } = false;
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 0;
        public float Mean { get; set; } = 0;
        public float StdDev { get; set; } = 0;
    }

    public class Inputs : InputOutputs<ImageData, RoiMask>
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }
        public RoiMask RoiMask { get => Item2; set => Item2 = value; }

        public Inputs() : base("ImageData", "RoiMask") { }

        public Inputs(ImageData imageData, RoiMask roiMask) : this()
        {
            SetValues(imageData, roiMask);
        }
    }

    public class Outputs : InputOutputs<StatisticResult[]>, IResultBufferItem
    {
        public StatisticResult[] Values { get => Item1; set => Item1 = value; }

        public Outputs() : base("Values")
        {
            Values = new StatisticResult[0];
        }

        public void CopyFrom(IResultBufferItem from)
        {
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            // No Buffer use
            return true;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            // No Buffer use
        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, $"Statistic.Outputs::SaveDebugInfo");
            using (var fs = new FileStream(Path.Combine(debugContext.Path, "Statistic.txt"), FileMode.Append, FileAccess.Write))
            {
                foreach (StatisticResult data in Values)
                {
                    int pos = Array.IndexOf(Values, data);
                    string str = $"Frame, {debugContext.FrameNo}, Position, {pos},  Min, {data.Min}, Max, {data.Max}, Average, {data.Mean}, StdDev, {data.StdDev}";
                    str += Environment.NewLine;

                    byte[] bytes = Encoding.Default.GetBytes(str);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }

    //public class StatisticParam : AlgorithmBaseParam<Inputs, Outputs>
    //{
    //    public float FrameMarginL { get; set; }
    //    public float FrameMarginT { get; set; }
    //    public float FrameMarginR { get; set; }
    //    public float FrameMarginB { get; set; }

    //    public StatisticParam()
    //    {
    //        FrameMarginL = 0;
    //        FrameMarginT = 0;
    //        FrameMarginR = 0;
    //        FrameMarginB = 0;
    //    }

    //    public override void SetVisionModel(VisionModel visionModel)
    //    {
    //        FrameMarginL = visionModel.FrameMarginL;
    //        FrameMarginT = visionModel.FrameMarginT;
    //        FrameMarginR = visionModel.FrameMarginR;
    //        FrameMarginB = visionModel.FrameMarginB;
    //    }

    //    public override IAlgorithmBase BuildAlgorithm(ModuleInfo moduleInfo)
    //    {
    //        return new Statistic(moduleInfo, this);
    //    }
    //}

    public class Statistic : AlgorithmBase<Inputs, Outputs>
    {
        public override int RequiredBufferCount => 0;

        public Statistic(ModuleInfo moduleInfo) : base(moduleInfo, null) { }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                AlgoImage algoImage = input.ImageData.Image;
                AlgoImage maskImage = input.RoiMask.Mask;

                output.Values = new StatisticResult[input.RoiMask.ROIs.Length];
                ImageProcessing ip = ImageProcessingPool.GetImageProcessing(algoImage);
                for (int i = 0; i < input.RoiMask.ROIs.Length; i++)
                {
                    output.Values[i] = new StatisticResult();
                    var maskRect = Rectangle.Round(input.RoiMask.ROIs[i]);

                    if (maskRect.Width > 0 && maskRect.Height > 0)
                    {
                        var statResult = new StatResult();
                        AlgoImage roiAlgoImage = algoImage.GetChildImage(maskRect);
                        AlgoImage roiMaskImage = null;
                        if (maskImage != null)
                        {
                            roiMaskImage = maskImage?.GetChildImage(maskRect);
                            statResult = ip.GetStatValue(roiAlgoImage, roiMaskImage);
                        }
                        else
                        {
                            statResult = ip.GetStatValue(roiAlgoImage);
                        }

                        output.Values[i].Min = (byte)statResult.min;
                        output.Values[i].Max = (byte)statResult.max;
                        output.Values[i].Mean = statResult.average;
                        output.Values[i].StdDev = statResult.stdDev;
                        output.Values[i].Valid = true;

                        roiMaskImage?.Dispose();
                        roiAlgoImage.Dispose();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"Statistic::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }
    }
}
