using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class Result
    {
        protected Judgment judgement = Judgment.NG;
        public Judgment Judgement
        {
            get => judgement;
            set => judgement = value;
        }
        public string BriefMessage { get; set; } = "";
        public ResultValueList ResultValueList { get; } = new ResultValueList();

        public virtual void InvertJudgment()
        {
            if (judgement == Judgment.OK)
            {
                judgement = Judgment.NG;
            }
            else
            {
                judgement = Judgment.OK;
            }
        }

        public bool IsGood()
        {
            return judgement == Judgment.OK;
        }

        public bool IsNG()
        {
            return judgement == Judgment.NG;
        }

        public bool IsOverkill()
        {
            return judgement == Judgment.Overkill;
        }

        public string GetGoodNgStr()
        {
            return (judgement == Judgment.NG ? StringManager.GetString("NG") : StringManager.GetString("OK"));
        }

        public void AddResultValue(ResultValue resultValue)
        {
            ResultValueList.Add(resultValue);
        }

        public void SetResult(string ngGoodStr)
        {
            judgement = (StringManager.GetString("OK") == ngGoodStr ? Judgment.OK : Judgment.NG);
        }

        public void SetResult(bool ngGood)
        {
            judgement = (ngGood ? Judgment.OK : Judgment.NG);
        }

        public void SetOverkill()
        {
            judgement = Judgment.Overkill;
        }

        public ResultValue GetResultValue(string resultValueName)
        {
            return ResultValueList.GetResultValue(resultValueName);
        }

        public ResultValue GetResultValue(int index)
        {
            return ResultValueList[index];
        }
    }

    public class SubResult
    {
        public bool Good { get; set; } = false;
        public string Name { get; set; }
        public float Value { get; set; }
        public RotatedRect ResultRect { get; set; }
        public FigureGroup FigureGroup { get; set; } = new FigureGroup();

        public SubResult(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }

    public class SubResultList
    {
        private List<SubResult> subResultList = new List<SubResult>();

        public SubResultList()
        {

        }

        public void AddSubResult(SubResult subResult)
        {
            subResultList.Add(subResult);
        }

        public SubResult GetMaxResult()
        {
            SubResult maxSubResult = null;
            foreach (SubResult subResult in subResultList)
            {
                if (maxSubResult == null || subResult.Value > maxSubResult.Value)
                {
                    maxSubResult = subResult;
                }
            }

            return maxSubResult;
        }
    }

    public class AlgorithmResult
    {
        public Result Result { get; } = new Result();
        public FigureGroup ResultFigures { get; } = new FigureGroup();

        public string BriefMessage
        {
            get => Result.BriefMessage;
            set => Result.BriefMessage = value;
        }
        public RotatedRect ResultRect { get; set; }

        public AlgorithmResult()
        {
            Result.SetResult(false);
        }

        public bool IsGood()
        {
            return Result.IsGood();
        }

        public bool IsNG()
        {
            return Result.IsNG();
        }

        public bool IsOverkill()
        {
            return Result.IsOverkill();
        }

        public virtual void InvertJudgment()
        {
            Result.InvertJudgment();
        }

        public void SetOverkill()
        {
            Result.SetOverkill();
        }

        public string GetGoodNgStr()
        {
            return Result.GetGoodNgStr();
        }

        public void SetResult(bool ngGood)
        {
            Result.SetResult(ngGood);
        }

        public void AddResultValue(ResultValue resultValue)
        {
            Result.AddResultValue(resultValue);
        }

        public ResultValue GetResultValue(int index)
        {
            return Result.GetResultValue(index);
        }

        public ResultValue GetResultValue(string resultValueName)
        {
            return Result.GetResultValue(resultValueName);
        }

        public void Offset(float x, float y)
        {
            ResultFigures.Offset(x, y);
        }

        public virtual void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            var figure = (Figure)ResultFigures.Clone();
            //figure.Offset(offset.X, offset.Y);
            figureGroup.AddFigure(figure);

            foreach (ResultValue resultValue in Result.ResultValueList)
            {
                if (resultValue.ResultRect == null)
                {
                    continue;
                }

                Figure rectangleFigure = new RectangleFigure(resultValue.ResultRect.Value, new Pen(Color.Lime, 1));
                rectangleFigure.Offset(offset.X, offset.Y);
                figureGroup.AddFigure(rectangleFigure);

                if (string.IsNullOrEmpty(resultValue.ShortResultMessage) == false)
                {
                    var font = new Font("Arial", 5);
                    var textFigure = new TextFigure(resultValue.ShortResultMessage, new Point((int)rectangleFigure.GetRectangle().Right, (int)rectangleFigure.GetRectangle().Top), font, Color.Red);
                    figureGroup.AddFigure(textFigure);
                }
            }
        }
    }
}
