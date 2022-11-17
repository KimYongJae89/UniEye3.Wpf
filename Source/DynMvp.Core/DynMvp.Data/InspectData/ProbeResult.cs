using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;

namespace DynMvp.InspectData
{
    public enum ResultImageType
    {
        Camera, Target, Probe
    }

    public class ProbeResult
    {
        public const int DefaultSequence = -1;

        protected Probe probe;
        public Probe Probe
        {
            get => probe;
            set => probe = value;
        }

        private int stepNo;
        public int StepNo
        {
            get => (probe != null ? probe.Target.InspectStep.StepNo : stepNo);
            set => stepNo = value;
        }

        private int cameraIndex;
        public int CameraIndex
        {
            get => (probe != null ? probe.Target.CameraIndex : cameraIndex);
            set => cameraIndex = value;
        }

        private int targetId;
        public int TargetId
        {
            get => (probe != null ? probe.Target.Id : targetId);
            set => targetId = value;
        }

        private string targetName;
        public string TargetName
        {
            get => (probe != null ? probe.Target.Name : targetName);
            set => targetName = value;
        }

        private string targetType;
        public string TargetType
        {
            get => (probe != null ? probe.Target.TypeName : targetType);
            set => targetType = value;
        }

        private int probeId;
        public int ProbeId
        {
            get => (probe != null ? probe.Id : probeId);
            set => probeId = value;
        }

        private string probeName;
        public string ProbeName
        {
            get => (probe != null ? probe.Name : probeName);
            set => probeName = value;
        }

        private ProbeType probeType;
        public ProbeType ProbeType
        {
            get => (probe != null ? probe.ProbeType : probeType);
            set => probeType = value;
        }
        public int SequenceNo { get; set; }

        protected Result result = new Result();
        public Result Result => result;

        public IEnumerator<ResultValue> GetEnumerator()
        {
            return result.ResultValueList.GetEnumerator();
        }
        public RotatedRect TargetRegion { get; set; }
        public RotatedRect ProbeRegion { get; set; }
        public RotatedRect InspectRegion { get; set; }
        public bool DifferentProductDetected { get; set; }

        public string BriefMessage
        {
            get => result.BriefMessage;
            set => result.BriefMessage = value;
        }

        public ResultValueList ResultValueList => result.ResultValueList;

        public ProbeResult()
        {

        }

        public ProbeResult(Probe probe)
        {
            this.probe = probe;
        }

        public bool IsGood()
        {
            return result.IsGood();
        }

        public bool IsNG()
        {
            return result.IsNG();
        }

        public bool IsOverkill()
        {
            return result.IsOverkill();
        }

        public virtual void InvertJudgment()
        {
            result.InvertJudgment();
        }

        public void SetOverkill()
        {
            result.SetOverkill();
        }

        public string GetGoodNgStr()
        {
            return result.GetGoodNgStr();
        }

        public void SetResult(bool ngGood)
        {
            result.SetResult(ngGood);
        }

        public void SetResult(string ngGoodStr)
        {
            result.SetResult(ngGoodStr);
        }

        public void AddResultValue(ResultValue resultValue)
        {
            result.AddResultValue(resultValue);
        }

        public static ProbeResult CreateProbeResult(ProbeType probeType)
        {
            ProbeResult probeResult = null;
            switch (probeType)
            {
                case ProbeType.Vision:
                    probeResult = new VisionProbeResult();
                    break;
                case ProbeType.Serial:
                    probeResult = new ProbeResult();
                    break;
                case ProbeType.Io:
                    probeResult = new ProbeResult();
                    break;
                case ProbeType.Daq:
                    probeResult = new ProbeResult();
                    break;
            }

            if (probeResult != null)
            {
                probeResult.ProbeType = probeType;
            }

            return probeResult;
        }

        protected Pen CreateResultPen()
        {
            Pen pen;
            if (result.Judgement == Judgment.OK)
            {
                pen = new Pen(Color.LightGreen, 2.0F);
            }
            else if (result.Judgement == Judgment.Overkill)
            {
                pen = new Pen(Color.Yellow, 2.0F);
            }
            else
            {
                pen = new Pen(Color.Red, 2.0F);
            }

            return pen;
        }

        public virtual void AppendResultMessage(Message totalResultMessage)
        {
            totalResultMessage.AddTextLine(result.BriefMessage);
        }

        public virtual void AppendResultFigures(FigureGroup figureGroup, ResultImageType resultImageType)
        {
            Pen pen = CreateResultPen();

            RotatedRect resultRect = ProbeRegion;
            if (resultImageType == ResultImageType.Target)
            {
                resultRect.Offset(new PointF(-TargetRegion.X, -TargetRegion.Y));
            }

            RectangleFigure figure;
            figure = new RectangleFigure(resultRect, pen);
            figure.Tag = this;

            var subFigureGroup = new FigureGroup();
            subFigureGroup.AddFigure(figure);

            figureGroup.AddFigure(subFigureGroup);
        }

        public override string ToString()
        {
            return result.BriefMessage;
        }

        public ResultValue GetResultValue(int index)
        {
            return result.GetResultValue(index);
        }

        public ResultValue GetResultValue(string resultValueName)
        {
            return result.GetResultValue(resultValueName);
        }
    }
}
