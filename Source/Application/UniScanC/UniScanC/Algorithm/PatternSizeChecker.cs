using DynMvp.Base;
using DynMvp.Vision;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
using Point = System.Drawing.Point;

namespace UniScanC.Algorithm.PatternSizeCheck
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
            RoiMask.SetValue("Mask", bufferPool.RequestBuffer());
            return RoiMask.Mask != null;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            bufferPool.ReturnBuffer(RoiMask.Mask);
            RoiMask.SetValue<AlgoImage>("Mask", null);
        }

        public void CopyFrom(IResultBufferItem from)
        {
        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, $"PatternSizeCheck.Outputs::SaveDebugInfo");
            RoiMask.Mask.Save($"{debugContext.FrameNo}.bmp", new DebugContext(true, Path.Combine(debugContext.Path, "PatternSizeCheck")));
        }
    }

    [AlgorithmBaseParam]
    public class PatternSizeCheckerParam : AlgorithmBaseParam<PatternSizeChecker, Inputs, Outputs>
    {
        public float FrameMarginL { get; set; }
        public float FrameMarginT { get; set; }
        public float FrameMarginR { get; set; }
        public float FrameMarginB { get; set; }

        public float PatternMarginX { get; set; }
        public float PatternMarginY { get; set; }

        public PatternSizeCheckerParam()
        {
            FrameMarginL = 0;
            FrameMarginT = 0;
            FrameMarginR = 0;
            FrameMarginB = 0;

            PatternMarginX = 10;
            PatternMarginY = 10;
        }

        public PatternSizeCheckerParam(PatternSizeCheckerParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
            FrameMarginL = visionModel.FrameMarginL;
            FrameMarginT = visionModel.FrameMarginT;
            FrameMarginR = visionModel.FrameMarginR;
            FrameMarginB = visionModel.FrameMarginB;

            PatternMarginX = (float)visionModel.PatternMarginX;
            PatternMarginY = (float)visionModel.PatternMarginY;
        }

        public override INodeParam Clone()
        {
            return new PatternSizeCheckerParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (PatternSizeCheckerParam)algorithmBaseParam;
            Name = param.Name;
            FrameMarginL = param.FrameMarginL;
            FrameMarginT = param.FrameMarginT;
            FrameMarginR = param.FrameMarginR;
            FrameMarginB = param.FrameMarginB;
            PatternMarginX = param.PatternMarginX;
            PatternMarginY = param.PatternMarginY;
        }
    }


    // 검사 ROI 찾아서 Rectangle반환, 영역이 유효하지 않으면, null 리턴
    // 한주기 찾기위한 프로파일(섹션) 정보 큐잉.
    public class PatternSizeChecker : AlgorithmBase<Inputs, Outputs>
    {
        public override int RequiredBufferCount => 0;
        public new PatternSizeCheckerParam Param => (PatternSizeCheckerParam)base.Param;

        /// <summary>
        /// 평균할 패턴의 셈플 개수
        /// </summary>
        public int AverageCount { get; set; } = 5;

        private List<float> vEdgePosAccList = new List<float>();

        public PatternSizeChecker(ModuleInfo moduleInfo, PatternSizeCheckerParam param) : base(moduleInfo, param) { }

        private float GetThresholdY() //length 세로
        {
            //return 10.0f;
            return 6.0f;
        }

        private float GetThresholdX() //cross 가로
        {
            return 6.0f;
        }

        private void EdgeFinder(float[] data, out int falling, out int rising, float threshold)
        {
            int risingEdge = 0;
            int fallingEdge = data.Length;
            Image<Gray, float> profileImg = null;
            Image<Gray, float> edgeImg = null;
            try
            {
                profileImg = new Image<Gray, float>(data.Length, 1);
                for (int i = 0; i < data.Length; ++i)
                {
                    profileImg.Data[0, i, 0] = data[i];
                }
                // data --%로 필터링
                profileImg = profileImg.SmoothBlur(11, 1, true);
                //CvInvoke.MedianBlur(profileImg, profileImg, 5);
                edgeImg = profileImg.Sobel(1, 0, 3);
                edgeImg = edgeImg.SmoothBlur(3, 1, true);

                double min = 0;
                double max = 0;
                var minPnt = new Point();
                var maxPnt = new Point();

                CvInvoke.MinMaxLoc(edgeImg, ref min, ref max, ref minPnt, ref maxPnt);

                float _Threshold = /*255 **/ threshold;

                if (max >= _Threshold)
                {
                    risingEdge = maxPnt.X; //right or bottom(start)
                }
                else
                {
                    risingEdge = -1;
                }

                if (Math.Abs(min) >= _Threshold)
                {
                    fallingEdge = minPnt.X; // left or top (end)
                }
                else
                {
                    fallingEdge = -1;
                }

                rising = risingEdge;
                falling = fallingEdge;
            }
            catch (Exception)
            {
                rising = 0;
                falling = data.Length;
            }
            finally
            {
                edgeImg.Dispose();
                profileImg.Dispose();
            }
        }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                AlgoImage fullImage = input.ImageData.Image;
                Size validSize = input.ImageData.Size;

                AlgoImage maskImage = output.RoiMask.Mask;
                var vEdgePos = new List<float>();

                var validRect = new Rectangle(Point.Empty, validSize);
                using (AlgoImage algoImage = fullImage.GetChildImage(validRect))
                {
                    ImageProcessing ip = ImageProcessingPool.GetImageProcessing(algoImage);

                    // 좌/우 영역 찾기 -> 없으면 이후 검사 통과
                    float[] profileX = ip.Projection(algoImage, TwoWayDirection.Horizontal, ProjectionType.Mean); //위아래로 프로젝션 => 데이터는 가로크기
                    float patternMarginX = Helpers.UnitConvertor.Mm2Px(Param.PatternMarginX, ModuleInfo.ResolutionWidth);
                    EdgeFinder(profileX, out int left, out int right, GetThresholdX());

                    if (left < 0 || right < 0)
                    {
                        return false;
                    }

                    // 상/하 영역 찾기 -> 데이터 누적?
                    float[] profileY = ip.Projection(algoImage, TwoWayDirection.Vertical, ProjectionType.Mean); //좌우로 프로젝션 => 데이터는 세로크기
                    EdgeFinder(profileY, out int top, out int bottom, GetThresholdY());
                    float patternMarginY = Helpers.UnitConvertor.Mm2Px(Param.PatternMarginY, ModuleInfo.ResolutionHeight);

                    var list = new List<RectangleF>();
                    if (top > -1 && bottom > -1) // 엣지가 둘 다 보임. tail = bottom, head = top
                    {
                        list.Add(RectangleF.FromLTRB(0, 0, algoImage.Width, bottom - patternMarginY)); // Upper ROI
                        list.Add(RectangleF.FromLTRB(0, top + patternMarginY, algoImage.Width, algoImage.Height)); // Lower ROI

                        vEdgePos.Add(bottom);
                        vEdgePos.Add(-1);
                        vEdgePos.Add(algoImage.Height - top);
                    }
                    else if (top > -1) //테이프 위쪽 끝이 보이고 있음 (┌┐↓).
                    {
                        list.Add(RectangleF.FromLTRB(0, top + patternMarginY, algoImage.Width, algoImage.Height)); // ROI

                        vEdgePos.Add(algoImage.Height - top);
                    }
                    else if (bottom > -1) //테이프 아래쪽 끝이 보이고 있음. (└┘↓)
                    {
                        list.Add(RectangleF.FromLTRB(0, 0, algoImage.Width, bottom - patternMarginY)); // ROI

                        vEdgePos.Add(bottom);
                        vEdgePos.Add(-1);
                    }
                    else // 엣지없다? 테이프중간 이미지 이거나, 아예 테이프가 없을경우임..
                    {
                        list.Add(RectangleF.FromLTRB(0, 0, algoImage.Width, algoImage.Height)); // ROI

                        vEdgePos.Add(algoImage.Height);
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = RectangleF.Intersect(list[i], validRect);
                    }

                    list.RemoveAll(f => f.Width <= 0 || f.Height <= 0);
                    if (list.Count == 0)
                    {
                        return false;
                    }

                    list = list.Select(f => RectangleF.FromLTRB(left + patternMarginX, f.Top, right - patternMarginX, f.Bottom)).ToList();
                    list.RemoveAll(f => f.Width <= 0 || f.Height <= 0);

                    var hEdgeList = new List<float>(new float[] { left, right });
                    SizeF patternSize = CalculatePatternLength(hEdgeList, vEdgePos);

                    if (maskImage != null)
                    // 마스크 생성
                    {
                        // 이진화 하고 블랍
                        using (AlgoImage subMaskImage = maskImage.GetChildImage(validRect))
                        {
                            // 패턴을 검게. 배경을 희게.
                            ip.Binarize(algoImage, subMaskImage);
                            //float binValue = ip.Otsu(algoImage);
                            float binValue = ip.GetStatValue(algoImage).average;
                            ip.Binarize(algoImage, subMaskImage, (int)binValue);
                            //subMaskImage.Save(@"C:\temp\subMaskImage.bmp");

                            ip.Open(subMaskImage, 50);
                            ip.Not(subMaskImage, subMaskImage);
                            //subMaskImage.Save(@"C:\temp\subMaskImage2.bmp");

                            //using (BlobRectList wBlobRect = ip.Blob(subMaskImage, new BlobParam() { AreaMin = 5 }))
                            //// 배경 + 전극 내 구멍 블랍.
                            //{
                            //    ip.Not(subMaskImage, subMaskImage); // 전극을 희게. 배경을 검게.
                            //    subMaskImage.Save(@"C:\temp\subMaskImage2.bmp");
                            //    using (BlobRectList dBlobRect = ip.Blob(subMaskImage, new BlobParam() { AreaMin = 5 }))
                            //        // 전극 블랍.
                            //    {
                            //        List<BlobRect> blobRectList = dBlobRect.ToList();
                            //        ip.DrawBlob(subMaskImage, dBlobRect, null, new DrawBlobOption() { SelectBlob = true, SelectHoles = true });

                            //        blobRectList.RemoveAll(f => !Array.Exists(rectangles, g => RectangleF.Intersect(g, f.BoundingRect) == f.BoundingRect));
                            //    }
                            //    subMaskImage.Save(@"C:\temp\subMaskImage2.bmp");
                            //}

                            int[,] morpStructW = new int[,] { { 1, 1, 1 } };
                            ip.Erode(subMaskImage, subMaskImage, morpStructW, (int)patternMarginX, false);
                            //subMaskImage.Save(@"C:\temp\subMaskImage3.bmp");

                            int[,] morpStructH = new int[,] { { 1 }, { 1 }, { 1 } };
                            ip.Erode(subMaskImage, subMaskImage, morpStructH, (int)patternMarginY, false);
                            //subMaskImage.Save(@"C:\temp\subMaskImage4.bmp");
                        }
                    }

                    //System.Diagnostics.Debug.Assert(list.Count > 0);
                    output.RoiMask.SetValue(1, list.ToArray());
                    output.PatternSize = patternSize;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"PatternSizeChecker::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }

        private SizeF CalculatePatternLength(List<float> hEdgeList, List<float> vEdgeList)
        {
            var result = new SizeF(-1, -1);

            // Add Width
            var widthList = new List<float>(); //가로방향 (프레임마다의 가로섹션)
            float w = hEdgeList[1] - hEdgeList[0];
            widthList.Add(w);

            // Add Length
            var lengthList = new List<float>();
            vEdgePosAccList.AddRange(vEdgeList);
            while (vEdgePosAccList.Count > 0)
            {
                int dst = vEdgePosAccList.FindIndex(f => f < 0);
                if (dst < 0)
                {
                    break;
                }


                // Bottom이 연달아 나오면 무시함.
                if (dst > 1)
                {
                    lengthList.Add(vEdgePosAccList.GetRange(0, dst).Sum());
                }

                vEdgePosAccList.RemoveRange(0, dst + 1);
            }

            if (widthList.Count == 0 || lengthList.Count == 0)
            {
                return result;
            }

            result.Width = widthList.Average();
            result.Height = lengthList.Average();
            return result;
        }

        #region NotUse
        //private float FindPatternWidth()
        //{
        //    return m_WidthList.Average();
        //}

        //private float FindPatternLength()
        //{
        //    float patternLength = 0;
        //    var data = m_LengthList.ToArray();

        //    PatternEdgeFinder(data, out int falling, out int rising, 8);

        //    if (falling > -1 && rising > -1) //둘다 찾음
        //    {
        //        if (falling < rising) //라이징(시작) 다음 폴링(끝)   비정상 // 라이징 이전 데이터는 의미없다.
        //        {
        //            patternLength = -1.0f;
        //        }
        //        else //정상적으로 다 찾음
        //        {
        //            patternLength = falling - rising;
        //        }
        //    }
        //    else // 엣지없다? 테이프중간 이미지 이거나, 아예 테이프가 없을경우임.. 
        //    {
        //        patternLength = -1.0f;
        //    }

        //    return patternLength;
        //}

        //private void RemovePatternList()
        //{
        //    lock (thisLock)
        //    {
        //        while (m_LengthList.Count > LengthListMaxCount)
        //            m_LengthList.RemoveAt(0);

        //        while (m_WidthList.Count > WidthListMaxCount)
        //            m_WidthList.RemoveAt(0);
        //    }
        //}

        //private bool PatternEdgeFinder(float[] data, out int fallingIndex, out int risingIndex, int maskSize = 4)
        //{
        //    fallingIndex = -1;
        //    risingIndex = -1;

        //    var profileImg = new Image<Gray, float>(data.Length, 1);
        //    try
        //    {
        //        for (int i = 0; i < data.Length; ++i)
        //        {
        //            profileImg.Data[0, i, 0] = data[i];
        //        }
        //        // data --%로 필터링
        //        profileImg = profileImg.SmoothBlur(11, 1, true);

        //        double min = 0;
        //        double max = 0;
        //        Point minPnt = new Point();
        //        Point maxPnt = new Point();

        //        CvInvoke.MinMaxLoc(profileImg, ref min, ref max, ref minPnt, ref maxPnt);
        //        double threshold = (max + min) / 2;

        //        // 이진화
        //        BitArray bin = new BitArray(data.Length);
        //        for (int i = 0; i < data.Length; ++i)
        //        {
        //            if (data[i] > threshold)
        //                bin[i] = true;
        //        }

        //        // 하강 엣지 마스크 생성
        //        BitArray mask = CreateMask(true, maskSize);
        //        // 하강 엣지 찾기
        //        List<int> fallingEdgeIndices = FindMaskEdges(bin, mask);

        //        // 2개의 하강엣지 발견됨
        //        if (fallingEdgeIndices.Count == 2)
        //        {
        //            // 상승 엣지 마스크 생성
        //            mask = CreateMask(false, maskSize);
        //            // 상승 엣지 찾기
        //            List<int> risingEdgeIndices = FindMaskEdges(bin, mask);
        //            // 두 하강 엣지 사이에 있는 상승 엣지 찾기
        //            risingEdgeIndices.FindAll(x => x > fallingEdgeIndices.First() && x < fallingEdgeIndices.Last());
        //            // 예외상황
        //            if (risingEdgeIndices.Count != 1)
        //                return false;
        //            // 찾은 엣지 인덱스 넘겨주기
        //            fallingIndex = fallingEdgeIndices.First();
        //            risingIndex = risingEdgeIndices.First();
        //        }
        //        // 예외상황
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show($"{ex.Message} / {ex.StackTrace}");
        //        return false;
        //    }
        //}

        //private BitArray CreateMask(bool isFalling, int maskSize)
        //{
        //    var mask = new BitArray(maskSize);
        //    for (int i = 0; i < mask.Length / 2; ++i)
        //        mask[i] = isFalling;
        //    for (int i = mask.Length / 2; i < mask.Length; ++i)
        //        mask[i] = !isFalling;
        //    return mask;
        //}

        //private List<int> FindMaskEdges(BitArray bin, BitArray mask)
        //{
        //    List<int> maskEdgeIndices = new List<int>();
        //    for (int i = 0; i < bin.Length - mask.Length; ++i)
        //    {
        //        bool maskEdge = true;
        //        for (int m = 0; m < mask.Length; ++m)
        //        {
        //            if (mask[m] != bin[i + m])
        //            {
        //                maskEdge = false;
        //                break;
        //            }
        //        }
        //        if (maskEdge)
        //            maskEdgeIndices.Add(i + mask.Length / 2);
        //    }
        //    return maskEdgeIndices;
        //}

        #endregion

    }
}
