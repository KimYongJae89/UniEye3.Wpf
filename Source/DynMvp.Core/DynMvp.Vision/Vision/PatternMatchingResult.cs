using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class MatchPos
    {
        public PointF Pos { get; set; } = new PointF(0, 0);
        public float Score { get; set; }
        public float Scale { get; set; }
        public Size PatternSize { get; set; }
        public float Angle { get; set; }
        public PatternType PatternType { get; set; } = PatternType.Good;

        public MatchPos()
        {
        }

        public MatchPos(PointF pos, float score)
        {
            Pos = pos;
            Score = score;
        }

        public override string ToString()
        {
            return string.Format("(X:{0:0.00}, Y:{1:0.00}), θ:{2:0.0} / Score {3:0.0}", Pos.X, Pos.Y, Angle, Score);
        }
    }

    public class PatternResult
    {
        public bool Good { get; set; } = false;

        public bool Found => MatchPosList.Count() > 0;
        public List<MatchPos> MatchPosList { get; set; } = new List<MatchPos>();
        public string ErrorString { get; set; }
        public RotatedRect ResultRect { get; set; }

        public void AddMatchPos(MatchPos matchPos)
        {
            MatchPosList.Add(matchPos);
        }

        public float MaxScore
        {
            get
            {
                if (MatchPosList.Count > 0)
                {
                    return MatchPosList.Max(x => x.Score);
                }

                return 0;
            }
        }

        public MatchPos MaxMatchPos
        {
            get
            {
                var maxMatchPos = new MatchPos();
                foreach (MatchPos matchPos in MatchPosList)
                {
                    if (matchPos.Score > maxMatchPos.Score)
                    {
                        maxMatchPos = matchPos;
                    }
                }

                return maxMatchPos;
            }
        }

        internal void RemoveInvalidResult(int matchScore)
        {
            MatchPosList = MatchPosList.FindAll(x => x.Score * 100 > matchScore);
        }
    }
}
