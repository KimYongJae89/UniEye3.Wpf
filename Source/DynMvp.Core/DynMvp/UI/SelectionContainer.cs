using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public class SelectionContainer
    {
        public List<Figure> Figures { get; } = new List<Figure>();

        public SelectionContainer()
        {
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return Figures.GetEnumerator();
        }

        public List<Figure> GetRealFigures()
        {
            var figureList = new List<Figure>();
            foreach (Figure figure in Figures)
            {
                figureList.Add((Figure)figure.Tag);
            }

            return figureList;
        }

        public void ClearSelection()
        {
            LogHelper.Debug(LoggerType.Operation, "SelectionContainer.ClearSelection.");
            Figures.Clear();
        }

        public void AddFigure(Figure figure)
        {
            // Tracking 상태를 표시하기 위해, 현재 Figure를 Clone한다.
            Figure cloneFigure = null;
            if (figure != null)
            {
                LogHelper.Debug(LoggerType.Operation, "SelectionContainer.AddFigure.");
                if (Figures.Count > 0)
                {
                    // Tag에 지정되어 있는 개체의 종류가 동일한 것만 추가 가능하도록 제약
                    if (((Figure)Figures[0].Tag).ObjectLevel == figure.ObjectLevel)
                    {
                        cloneFigure = (Figure)figure.Clone();
                    }
                }
                else
                {
                    cloneFigure = (Figure)figure.Clone();
                }
            }

            if (cloneFigure != null)
            {
                cloneFigure.Tag = figure;
                cloneFigure.FigureProperty.Pen = new Pen(Color.Cyan);
                cloneFigure.FigureProperty.Pen.DashStyle = DashStyle.Dot;
                Figures.Add(cloneFigure);
            }
        }

        public void AddFigure(List<Figure> figureList)
        {
            if (figureList != null && figureList.Count > 0)
            {
                LogHelper.Debug(LoggerType.Operation, "SelectionContainer.AddFigure.List");
                foreach (Figure figure in figureList)
                {
                    AddFigure(figure);
                }
            }
        }

        public bool IsSelected(Figure searchfigure)
        {
            foreach (Figure figure in Figures)
            {
                if (figure.Tag == searchfigure)
                {
                    return true;
                }
            }

            return false;
        }

        public void Offset(SizeF offset)
        {
            foreach (Figure figure in Figures)
            {
                var realFigure = (Figure)figure.Tag;
                realFigure.Offset(offset.Width, offset.Height);
            }
        }

        public void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked, bool confirm)
        {
            foreach (Figure figure in Figures)
            {
                if (confirm)
                {
                    var realFigure = (Figure)figure.Tag;
                    realFigure.TrackMove(trackPos, offset, rotationLocked);
                }
                else
                {
                    figure.TrackMove(trackPos, offset, rotationLocked);
                }
            }
        }

        public void GetTrackPath(List<GraphicsPath> trackPathList, SizeF offset, TrackPos trackPos)
        {
            foreach (Figure figure in Figures)
            {
                figure.GetTrackPath(trackPathList, offset, trackPos);
            }
        }

        public void Draw(Graphics g, CoordMapper coordMapper, bool rotationLocked)
        {
            foreach (Figure figure in Figures)
            {
                figure.DrawSelection(g, coordMapper, rotationLocked);
            }
        }

        public TrackPos GetTrackPos(CoordMapper coordMapper, PointF point, bool rotationLocked)
        {
            var trackPos = new TrackPos(TrackPosType.None, 0);

            int polygonIndex = 0;

            if (Figures.Count == 1)
            {
                trackPos = ((Figure)Figures[0].Tag).GetTrackPos(point, coordMapper, rotationLocked, ref polygonIndex);
            }
            else
            {
                foreach (Figure figure in Figures)
                {
                    trackPos = ((Figure)figure.Tag).GetTrackPos(point, coordMapper, rotationLocked, ref polygonIndex);
                    if (trackPos.PosType != TrackPosType.None)
                    {
                        trackPos.PosType = TrackPosType.Inner;
                        break;
                    }
                }
            }

            return trackPos;
        }
    }
}
