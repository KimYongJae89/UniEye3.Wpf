using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.RoiFind
{
    public class Inputs : InputOutputs<ImageData>
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }

        public Inputs() : base("ImageData") { }
        public Inputs((AlgoImage image, Size size) imageData) : this()
        {
            SetValues(imageData);
        }
    }

    public class Outputs : InputOutputs<RoiMask, SizeF>, IResultBufferItem
    {
        public RoiMask RoiMask { get => Item1; set => Item1 = value; }
        public SizeF PatternSize { get => Item2; set => Item2 = value; }

        public Outputs() : base("RoiMask", "PatternSize")
        {
            RoiMask = new RoiMask();
            PatternSize = new SizeF(-1, -1);
        }

        public Outputs((AlgoImage algoImage, RectangleF[] rois) mask, SizeF patternSize) : this()
        {
            SetValues(mask, patternSize);
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            //this.RoiMask.SetValue<AlgoImage>("Mask", bufferPool.RequestBuffer());
            //return this.RoiMask.Mask != null;
            return true;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            bufferPool.ReturnBuffer(RoiMask.Mask);
            RoiMask.SetValue<AlgoImage>("Mask", null);
        }

        public void CopyFrom(IResultBufferItem from) { }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, $"RoiFind.Outputs::SaveDebugInfo");
            RoiMask.Mask?.Save($"{debugContext.FrameNo}.bmp", new DebugContext(true, Path.Combine(debugContext.Path, "RoiFind")));
        }
    }

    [AlgorithmBaseParam]
    public class RoiFinderParam : AlgorithmBaseParam<RoiFinder, Inputs, Outputs>
    {
        public float FrameMarginL { get; set; }
        public float FrameMarginT { get; set; }
        public float FrameMarginR { get; set; }
        public float FrameMarginB { get; set; }

        public float PatternMarginX { get; set; }
        public float PatternMarginY { get; set; }

        public float TargetIntensity { get; set; }
        public float OutTargetIntensity { get; set; }

        public RoiFinderParam()
        {
            FrameMarginL = 0;
            FrameMarginT = 0;
            FrameMarginR = 0;
            FrameMarginB = 0;

            PatternMarginX = 10;
            PatternMarginY = 10;

            TargetIntensity = 128;
            OutTargetIntensity = 255;
        }

        public RoiFinderParam(RoiFinderParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
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
            return new RoiFinderParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (RoiFinderParam)algorithmBaseParam;
            Name = param.Name;
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

    public class RoiFinder : AlgorithmBase<Inputs, Outputs>
    {
        public new RoiFinderParam Param => (RoiFinderParam)base.Param;

        private int prevLeft = -1;
        private int prevRight = -1;
        public override int RequiredBufferCount => 0;

        public RoiFinder(ModuleInfo moduleInfo, RoiFinderParam param) : base(moduleInfo, param)
        {
            prevLeft = -1;
            prevRight = -1;
        }

        private Rectangle GetRoiInFrame(Size fullSize)
        {
            float l = Helpers.UnitConvertor.Mm2Px(Param.FrameMarginL, ModuleInfo.ResolutionWidth);
            float t = Helpers.UnitConvertor.Mm2Px(Param.FrameMarginT, ModuleInfo.ResolutionHeight);
            float r = fullSize.Width - Helpers.UnitConvertor.Mm2Px(Param.FrameMarginR, ModuleInfo.ResolutionWidth);
            float b = fullSize.Height - Helpers.UnitConvertor.Mm2Px(Param.FrameMarginB, ModuleInfo.ResolutionHeight);

            return Rectangle.Round(RectangleF.FromLTRB(l, t, r, b));
        }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(input.ImageData.Image);

                Size rawDataSize = input.ImageData.Size;
                LogHelper.Debug(LoggerType.Inspection, $"RoiFinder::Run - rawDataSize: {rawDataSize}");

                Rectangle frameRoi = GetRoiInFrame(rawDataSize);
                if (frameRoi.Width <= 0 || frameRoi.Height <= 0)
                {
                    output.RoiMask.ROIs = new RectangleF[0];
                    output.RoiMask.Mask?.Clear();
                    output.PatternSize = SizeF.Empty;
                    return false;
                }

                float[] profileX, profileY;
                using (AlgoImage projectionChild = input.ImageData.Image.GetChildImage(frameRoi))
                {
                    profileX = imageProcessing.Projection(projectionChild, TwoWayDirection.Horizontal, ProjectionType.Mean);
                    profileY = imageProcessing.Projection(projectionChild, TwoWayDirection.Vertical, ProjectionType.Mean);
                }

                int threshold = (int)Math.Round((Param.TargetIntensity + Param.OutTargetIntensity) / 2.0f);
                (int leftEdge, int rightEdge) = Algorithm.Simple.HorizentalEdgeFinder.Find(profileX, ModuleInfo.CamPos, threshold);
                //if (prevLeft>=0 && Math.Abs(leftEdge - prevLeft) > 50)
                //    leftEdge = prevLeft;
                //if (prevRight>= 0 && Math.Abs(rightEdge - prevRight) > 50)
                //    rightEdge = prevRight;

                prevRight = rightEdge;
                prevLeft = leftEdge;

                int[] lightBars = Algorithm.Simple.VerticalEdgeFinder.Find(profileY);

                // Light Bar의 위 100픽셀부터 아래 100픽셀은 버린다. (ROI 영역으로 잡지 않음)
                int margin = 100;
                var splitPos = new List<int>();
                splitPos.Add(-margin);
                splitPos.AddRange(lightBars);
                splitPos.Add(frameRoi.Height + margin);

                var roiList = new List<Rectangle>();
                splitPos.Aggregate((f, g) =>
                {
                    roiList.Add(Rectangle.FromLTRB(leftEdge, f + margin, rightEdge, g - margin));
                    return g;
                });

                int patternMarginX = (int)Helpers.UnitConvertor.Mm2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                int patternMarginY = (int)Helpers.UnitConvertor.Mm2Px(Param.PatternMarginY, ModuleInfo.ResolutionHeight);

                var rois = roiList.Select(f =>
                {
                    RectangleF inflated = Rectangle.Inflate(f, -patternMarginX, -patternMarginY);
                    inflated.Offset(frameRoi.Location);
                    return inflated;
                }).ToList();
                rois.RemoveAll(f => f.Height < 0 || f.Width < 0);

                //input.ImageData.Image.Save(@"C:\temp\tt.bmp");
                output.RoiMask.ROIs = rois.ToArray();
                if (rois.Count > 0)
                {
                    output.PatternSize = rois.Aggregate((f, g) => RectangleF.Union(f, g)).Size;
                }

                if (output.RoiMask.Mask != null)
                {
                    AlgoImage maskImage = output.RoiMask.Mask;
                    rois.ForEach(f => imageProcessing.DrawRotateRact(maskImage, new DynMvp.UI.RotatedRect(f, 0), Color.White.ToArgb(), true));
                }
                LogHelper.Debug(LoggerType.Inspection, $"RoiFinder::Run - RoiMask.ROIs.Length: {output.RoiMask.ROIs.Length}");

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"RoiFinder::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 밝은 영역 검사 하지 않도록 하는 기능
            //byte[] oneLIne = new byte[rectangle.Width];

            //int startPosition = size.Width * rectangle.Top + rectangle.Left;
            //int endPosition = size.Width * rectangle.Top + size.Width * (size.Height - 1) + rectangle.Left;

            //Buffer.BlockCopy(data, startPosition, oneLIne, 0, rectangle.Width);
            //double topLineAvg = oneLIne.Select(x => (int)x).Average();

            //Buffer.BlockCopy(data, endPosition, oneLIne, 0, rectangle.Width);
            //double bottomLineAvg = oneLIne.Select(x => (int)x).Average();

            //if (topLineAvg < settings.CalibrationValue - 80 || topLineAvg > settings.CalibrationValue + 80 ||
            //bottomLineAvg < settings.CalibrationValue - 80 || bottomLineAvg > settings.CalibrationValue + 80)
            //{
            //    SendDumyResult(size.Width, size.Height, inspectResult, "out of bright level");
            //    Debug.WriteLine(string.Format("out of bright level"));
            //    foreach (Stripe stripe in topStripeList)
            //    {
            //        stripe.isContinue = false;
            //    }
            //    continue;
            //}
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
