using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System.Drawing;

namespace DynMvp.InspectData
{
    public class VisionProbeResult : ProbeResult
    {
        private string algorithmType;
        public string AlgorithmType
        {
            get => (Probe != null ? ((VisionProbe)Probe).InspAlgorithm.GetAlgorithmType() : algorithmType);
            set => algorithmType = value;
        }
        public AlgorithmResult AlgorithmResult { get; set; }

        public VisionProbeResult()
        {
            result.BriefMessage = StringManager.GetString("AlgorithmResult is invalid. Check the probe region.");
        }

        public VisionProbeResult(Probe probe, AlgorithmResult algorithmResult)
        {
            AlgorithmResult = algorithmResult;

            if (algorithmResult != null)
            {
                result = algorithmResult.Result;
            }
            else
            {
                result.BriefMessage = "Algorithm Result is null";
            }

            Probe = probe;
        }

        public VisionProbeResult(Probe probe, string briefMessage)
        {
            AlgorithmResult = null;

            result.Judgement = Judgment.NG;
            result.ResultValueList.AddRange(probe.GetResultValues());
            result.BriefMessage = briefMessage;
            this.probe = probe;
        }

        ~VisionProbeResult()
        {
        }

        public override void AppendResultMessage(Message totalResultMessage)
        {
            base.AppendResultMessage(totalResultMessage);

            if (probe is VisionProbe visionProbe)
            {
                visionProbe.InspAlgorithm.AppendResultMessage(totalResultMessage, AlgorithmResult);
            }
        }

        public override void AppendResultFigures(FigureGroup figureGroup, ResultImageType resultImageType)
        {
            base.AppendResultFigures(figureGroup, resultImageType);

            if (AlgorithmResult != null)
            {
                var offfset = new PointF(0, 0);
                if (resultImageType == ResultImageType.Target)
                {
                    offfset = new PointF(-TargetRegion.X, -TargetRegion.Y);
                }

                AlgorithmResult.AppendResultFigures(figureGroup, offfset);
            }
        }
    }
}
