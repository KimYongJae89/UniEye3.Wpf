using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.PlainFilmCheck
{
    public class Inputs : InputOutputs<ImageData, RoiMask, int>
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }

        public RoiMask RoiMask { get => Item2; set => Item2 = value; }

        public int FrameNo { get => Item3; set => Item3 = value; }

        public Inputs() : base("ImageData", "RoiMask", "FrameNo") { }

        public Inputs(ImageData imageData, RoiMask roiMask, int frameNo) : this()
        {
            SetValues(imageData, roiMask, frameNo); ;
        }
    }

    public class Outputs : InputOutputs<AlgoImage, List<Defect>>, IResultBufferItem
    {
        public AlgoImage BinalImage { get => Item1; set => Item1 = value; }
        public List<Defect> DefectList { get => Item2; set => Item2 = value; }

        public Outputs() : base("BinalImage", "DefectList")
        {
            BinalImage = null;
            DefectList = new List<Defect>();
        }

        public Outputs(AlgoImage binalImage, List<Defect> defectList) : this()
        {
            SetValues(binalImage, defectList);
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            BinalImage = bufferPool.RequestBuffer();
            return BinalImage != null;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            bufferPool.ReturnBuffer(BinalImage);
            BinalImage = null;
        }

        public void CopyFrom(IResultBufferItem from)
        {
        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, $"PlainFilmCheck.Outputs::SaveDebugInfo");
            BinalImage.Save($"{debugContext.FrameNo}.bmp", new DebugContext(true, Path.Combine(debugContext.Path, "PlainFilmCheck")));
        }
    }

    [AlgorithmBaseParam]
    public class PlainFilmCheckerParam : AlgorithmBaseParam<PlainFilmChecker, Inputs, Outputs>
    {
        public float TargetIntensity { get; set; }

        public float ThresholdD { get; set; }
        public float ThresholdL { get; set; }
        public float MinimumSize { get; set; }

        public EDefectPriority DefectPriority { get; set; }
        public int MaxDefectCount { get; set; }
        public int InflateSize { get; set; }

        public List<DefectCategory> DefectCategories { get; set; }
        public bool UseOtherCategory { get; set; }
        public bool SkipJudgement { get; set; }

        public PlainFilmCheckerParam()
        {
            TargetIntensity = -1;

            ThresholdD = 20;
            ThresholdL = 20;
            MinimumSize = 20;

            DefectPriority = EDefectPriority.Ymin;
            MaxDefectCount = 20;
            InflateSize = 80;
            UseOtherCategory = true;
        }

        public PlainFilmCheckerParam(PlainFilmCheckerParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
            TargetIntensity = visionModel.TargetIntensity;

            ThresholdD = visionModel.ThresholdDark;
            ThresholdL = visionModel.ThresholdLight;
            MinimumSize = visionModel.ThresholdSize;

            DefectPriority = visionModel.DefectPriority;
            MaxDefectCount = visionModel.MaxDefectCount;
            InflateSize = visionModel.InflateSize;

            UseOtherCategory = visionModel.UseOtherCategory;

            DefectCategories = new List<DefectCategory>(visionModel.DefectCategories);
        }

        public override INodeParam Clone()
        {
            return new PlainFilmCheckerParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (PlainFilmCheckerParam)algorithmBaseParam;
            Name = param.Name;
            TargetIntensity = param.TargetIntensity;
            ThresholdD = param.ThresholdD;
            ThresholdL = param.ThresholdL;
            MinimumSize = param.MinimumSize;
            DefectPriority = param.DefectPriority;
            MaxDefectCount = param.MaxDefectCount;
            InflateSize = param.InflateSize;
            UseOtherCategory = param.UseOtherCategory;
        }
    }

    public class PlainFilmChecker : AlgorithmBase<Inputs, Outputs>
    {
        public new PlainFilmCheckerParam Param => (PlainFilmCheckerParam)base.Param;

        public override int RequiredBufferCount => 0;

        public PlainFilmChecker(ModuleInfo moduleInfo, PlainFilmCheckerParam param) : base(moduleInfo, param) { }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                AlgoImage srcImage = input.ImageData.Image;
                Size srcImageSize = input.ImageData.Size;
                int frameNo = input.FrameNo;

                //#if DEBUG
                //                Directory.CreateDirectory($@"C:\temp\planeFileChecker\");
                //                srcImage.Save($@"C:\temp\planeFileChecker\{input.FrameNo}.bmp");
                //#endif

                AlgoImage maskBuffer = input.RoiMask.Mask;
                RectangleF[] roiRects = input.RoiMask.ROIs;

                List<Defect> defectList = output.DefectList;

                AlgoImage labelBuffer = output.BinalImage;
                var fullrect = new Rectangle(Point.Empty, srcImageSize);
                //srcImage.Save(@"C:\temp\srcImage.bmp");

                Array.ForEach(roiRects, roiRect =>
                {
                    var childRect = Rectangle.Round(roiRect);
                    if (childRect.Width > 0 && childRect.Height > 0)
                    {
                        AlgoImage srcChild = srcImage.GetChildImage(childRect);
                        AlgoImage maskChild = maskBuffer?.GetChildImage(childRect);
                        AlgoImage labelChild = labelBuffer.GetChildImage(childRect);

                        ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(labelChild);
                        float th = Param.TargetIntensity;
                        if (th < 0)
                        {
                            if (maskChild != null)
                            {
                                th = imageProcessing.GetStatValue(srcChild, maskChild).average;
                            }
                            else
                            {
                                th = imageProcessing.GetStatValue(srcChild).average;
                            }
                        }
                        //th = imageProcessing.Otsu(srcChild, maskChild);

                        imageProcessing.Binarize(srcChild, labelChild,
                                (int)Math.Max(0, th - Param.ThresholdD), (int)Math.Min(255, th + Param.ThresholdL), true);

                        imageProcessing.Close(labelChild, 3);

                        //#if DEBUG
                        //                        Directory.CreateDirectory($@"C:\temp\planeFileChecker_srcChild\");
                        //                        srcChild.Save($@"C:\temp\planeFileChecker_srcChild\{input.FrameNo}.bmp");
                        //                        Directory.CreateDirectory($@"C:\temp\planeFileChecker_labelChild\");
                        //                        labelChild.Save($@"C:\temp\planeFileChecker_labelChild\{input.FrameNo}.bmp");
                        //#endif

                        if (maskChild != null)
                        {
                            imageProcessing.And(labelChild, maskChild, labelChild);
                        }

                        int minSizePxWidth = (int)Math.Round(Helpers.UnitConvertor.Um2Px(Param.MinimumSize, ModuleInfo.ResolutionWidth), 0);
                        int minSizePxHeight = (int)Math.Round(Helpers.UnitConvertor.Um2Px(Param.MinimumSize, ModuleInfo.ResolutionHeight), 0);
                        var blobParam = new BlobParam()
                        {
                            //MaxCount = this.Param.MaxDefectCount,
                            SelectMinAreaBox = true,
                            SelectMeanValue = true,
                            SelectMinValue = true,
                            SelectMaxValue = true,
                            SelectCenterPt = true,
                            SelectArea = true,//sort by Area topdown
                            SelectBoundingRect = true,
                            GroupSameLabelAndTouchingBlobs = false,
                            EraseBorderBlob = false,// !IsInspectStripe;
                            BoundingRectMinWidth = minSizePxWidth,
                            BoundingRectMinHeight = minSizePxHeight
                        };

                        using (BlobRectList blobRectList = imageProcessing.Blob(labelChild, srcChild, blobParam))
                        {
                            IOrderedEnumerable<BlobRect> blobRects;
                            switch (Param.DefectPriority)
                            {
                                case EDefectPriority.Small: blobRects = blobRectList.OrderBy(x => x.Area); break;
                                case EDefectPriority.Big: blobRects = blobRectList.OrderByDescending(x => x.Area); break;
                                case EDefectPriority.Xmin: blobRects = blobRectList.OrderBy(x => x.CenterPt.X); break;
                                case EDefectPriority.Xmax: blobRects = blobRectList.OrderByDescending(x => x.CenterPt.X); break;
                                case EDefectPriority.Ymin: blobRects = blobRectList.OrderBy(x => x.CenterPt.Y); break;
                                case EDefectPriority.Ymax: blobRects = blobRectList.OrderByDescending(x => x.CenterPt.Y); break;
                                default: blobRects = blobRectList.OrderByDescending(x => x.Area); break;
                            }

                            var intersectRect = new Rectangle(Point.Empty, childRect.Size);

                            foreach (BlobRect blobRect in blobRects)
                            {
                                //Console.WriteLine($"PlainFilmChecker::Run - frameNo: {frameNo}");
                                if (Param.MaxDefectCount >= 0 && defectList.Count >= Param.MaxDefectCount)
                                {
                                    break;
                                }

                                if (blobRect.BoundingRect.Width < minSizePxWidth || blobRect.BoundingRect.Height < minSizePxHeight)
                                {
                                    break;
                                }

                                //Console.WriteLine($"PlainFilmChecker::Run - defectrect1: {blobRect.BoundingRect.Size}");
                                var rect = Rectangle.Round(blobRect.BoundingRect);

                                var imageRect = Rectangle.Intersect(Rectangle.Inflate(rect, Param.InflateSize, Param.InflateSize), intersectRect);
                                BitmapSource bitmapSource = null;
                                using (AlgoImage defectImage = srcChild.GetChildImage(imageRect))
                                {
                                    bitmapSource = defectImage.ToBitmapSource();
                                }

                                rect.Offset(childRect.Location);

                                Rectangle touchHead = Rectangle.Empty;
                                if (fullrect.Top == rect.Top)
                                // 머리가 닿은 부분을 구한다.
                                {
                                    var br = new Rectangle(rect.Left, rect.Top, rect.Width, 1);
                                    using (AlgoImage blobbb = labelBuffer.GetChildImage(br))
                                    {
                                        BlobRectList blobRectList2 = imageProcessing.Blob(blobbb, new BlobParam());
                                        touchHead = Rectangle.Round(blobRectList2.Select(f => f.BoundingRect).Aggregate((f, g) => RectangleF.Union(f, g)));
                                        touchHead.Offset(br.Location);
                                    }
                                }

                                Rectangle touchTail = Rectangle.Empty;
                                if (fullrect.Bottom == rect.Bottom)
                                // 꼬리가 닿은 부분을 구한다.
                                {
                                    var br = new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, 1);
                                    using (AlgoImage blobbb = labelBuffer.GetChildImage(br))
                                    {
                                        BlobRectList blobRectList2 = imageProcessing.Blob(blobbb, new BlobParam());
                                        touchTail = Rectangle.Round(blobRectList2.Select(f => f.BoundingRect).Aggregate((f, g) => RectangleF.Union(f, g)));
                                        touchTail.Offset(br.Location);
                                    }
                                }

                                if (defectList.Exists(f => f.BoundingRect.IntersectsWith(rect)))
                                {
                                    continue;
                                }

                                var dustDefect = new Defect()
                                {
                                    FrameIndex = frameNo,
                                    ModuleNo = ModuleInfo.ModuleNo,
                                    DefectType = EDefectType.Dust,
                                    DefectPos = PointF.Add(childRect.Location, new SizeF(blobRect.CenterPt)),
                                    BoundingRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height),
                                    Area = blobRect.Area,
                                    MinGv = Convert.ToInt32(blobRect.MinValue),
                                    MaxGv = Convert.ToInt32(blobRect.MaxValue),
                                    AvgGv = Convert.ToInt32(blobRect.MeanValue),
                                    DefectImage = bitmapSource,

                                    ImageClipRect = imageRect,
                                    TouchHeadRect = touchHead,
                                    TouchTailRect = touchTail
                                };

                                //Console.WriteLine($"PlainFilmChecker::Run - defectrect2: {rect.Size}");
                                var resolution = new SizeF(ModuleInfo.ResolutionWidth, ModuleInfo.ResolutionHeight);
                                dustDefect.CategorySearch(Param.DefectCategories, resolution, Param.UseOtherCategory);

                                if (dustDefect.DefectCategories.Count == 0)
                                {
                                    continue;
                                }

                                defectList.Add(dustDefect);
                                dustDefect.DefectNo = defectList.IndexOf(dustDefect);
                            }
                            LogHelper.Debug(LoggerType.Inspection, $"PlainFilmChecker::Run - defectList.Count - {defectList.Count}");
                        }
                        srcChild.Dispose();
                        labelChild.Dispose();
                        maskChild?.Dispose();
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"PlainFilmChecker::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }

    }
}
