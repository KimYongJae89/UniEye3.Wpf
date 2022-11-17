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

namespace UniScanC.Algorithm.PatternDefectChecker
{
    public class Inputs : InputOutputs<ImageData, SizeF, List<Defect>, int>
    {
        public ImageData ImageData { get => Item1; set => Item1 = value; }

        public SizeF PatternSize { get => Item2; set => Item2 = value; }

        public List<Defect> DefectList { get => Item3; set => Item3 = value; }

        public int FrameNo { get => Item4; set => Item4 = value; }

        public Inputs() : base("ImageData", "PatternSize", "DefectList", "FrameNo") { }

        public Inputs(ImageData ImageData, SizeF PatternSize, List<Defect> DefectList, int FrameNo) : this()
        {
            SetValues(ImageData, PatternSize, DefectList, FrameNo);
        }
    }

    public class Outputs : InputOutputs<List<Defect>>, IResultBufferItem
    {
        public List<Defect> DefectList { get => Item1; set => Item1 = value; }

        public Outputs() : base("DefectList")
        {
            DefectList = new List<Defect>();
        }

        public Outputs(List<Defect> defectList) : this()
        {
            SetValues(defectList);
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

        }
    }

    [AlgorithmBaseParam]
    public class PatternDefectCheckerParam : AlgorithmBaseParam<PatternDefectChecker, Inputs, Outputs>
    {
        public int PatternCount { get; set; }

        public PatternDefectCheckerParam()
        {
            PatternCount = 1;
        }

        public PatternDefectCheckerParam(PatternDefectCheckerParam param) : base(param) { }

        public override void SetVisionModel(VisionModel visionModel)
        {
            PatternCount = 1;
        }

        public override INodeParam Clone()
        {
            return new PatternDefectCheckerParam(this);
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var param = (PatternDefectCheckerParam)algorithmBaseParam;
            Name = param.Name;
            PatternCount = param.PatternCount;
        }
    }


    // PatternSiezeChecker 에서 리턴한 패턴 정보가 우효할 시 불량 정보 내보냄
    // PlainFilmChecker 에서 검사한 항목을 저장하고 있다가 조건이 충족되면 내보냄
    public class PatternDefectChecker : AlgorithmBase<Inputs, Outputs>
    {
        public new PatternDefectCheckerParam Param => (PatternDefectCheckerParam)base.Param;

        public override int RequiredBufferCount => 0;

        private List<Defect> DefectList { get; set; } = new List<Defect>();

        public PatternDefectChecker(ModuleInfo moduleInfo, PatternDefectCheckerParam param) : base(moduleInfo, param) { }

        public override bool Run(Inputs input, ref Outputs output, AlgoImage[] workingBuffers)
        {
            try
            {
                DefectList.AddRange(input.DefectList);
                if (input.PatternSize.Width == -1 && input.PatternSize.Height == -1)
                {
                    return true;
                }
                else
                {
                    int newIndex = 0;
                    foreach (Defect defect in DefectList)
                    {
                        Defect newDefect = defect.Clone();
                        newDefect.DefectPos = new PointF(newDefect.DefectPos.X, newDefect.DefectPos.Y - (input.FrameNo - newDefect.FrameIndex) * input.ImageData.Size.Height);
                        newDefect.FrameIndex = input.FrameNo;
                        newDefect.DefectNo = newIndex++;
                        output.DefectList.Add(newDefect);
                    }
                    DefectList.Clear();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"PatternDefectChecker::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }
    }
}
