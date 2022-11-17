using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public enum TrackPosType
    {
        None, LeftTop, Top, RightTop, Right, RightBottom, Bottom, LeftBottom, Left, Rotate, Move, Inner, Polygon, Link
    }

    public interface ITrackTarget
    {
        bool IsSizable();
        bool IsRotatable();
        bool IsContainer();
    }

    public class TrackPos
    {
        public TrackPosType PosType { get; set; }
        public int PolygonIndex { get; set; }

        public TrackPos() { }

        public TrackPos(TrackPosType posType, int polygonIndex)
        {
            PosType = posType;
            PolygonIndex = polygonIndex;
        }
    }

    public delegate void TrackerMovedDelegate();
    public delegate void SelectionPointCapturedDelegate(Point point);
    public delegate void SelectionRectCapturedDelegate(Rectangle rectangle, Point startPos, Point endPos);
    public delegate void SelectionRectZoomDelegate(RectangleF rectangle);
    public delegate void AddFigureCapturedDelegate(List<PointF> pointList);
    public delegate void PositionShiftedDelegate(SizeF offset);

    public class Tracker
    {
        private Control ownerControl;
        public TrackerMovedDelegate TrackerMoved;
        public SelectionPointCapturedDelegate SelectionPointCaptured;
        public SelectionRectCapturedDelegate SelectionRectCaptured;
        public SelectionRectZoomDelegate SelectionRectZoom;
        public AddFigureCapturedDelegate AddFigureCaptured;
        public PositionShiftedDelegate PositionShifted;
        public bool Enable { get; set; }
        public bool MeasureMode { get; set; }
        public bool RotationLocked { get; set; } = false;
        public bool MoveLocked { get; set; } = false;

        private bool onSelectRange = false;
        private bool onMoveFigure = false;
        private bool onZoomRange = false;
        public Point StartTrackPos { get; private set; }
        public Point EndTrackPos { get; private set; }
        public List<PointF> TrackPointList { get; } = new List<PointF>();

        private List<Figure> figureList = new List<Figure>();
        internal TrackPos TrackPos { get; set; }
        public CoordTransformer CoordTransformer { get; set; } = null;

        private bool addFigureMode;

        public bool AddFigureMode
        {
            get => addFigureMode;
            set
            {
                if (addFigureMode != value)
                {
                    LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigureMode.");

                    addFigureMode = value;
                    figureList.Clear();
                    ownerControl.Invalidate();
                }
            }
        }
        public bool ShiftPositionMode { get; set; }
        public FigureType Shape { get; set; }
        public int NumGridRow { get; set; } = 0;
        public int NumGridColumn { get; set; } = 0;

        public Tracker(Control control)
        {
            ownerControl = control;
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return figureList.GetEnumerator();
        }

        public void ClearFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "Tracker.ClearFigure.");

            figureList.Clear();
            TrackPos.PosType = TrackPosType.None;
        }

        public void AddFigure(Figure figure)
        {
            if (figure != null)
            {
                bool add = false;
                LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigure.");
                if (figureList.Count > 0)
                {
                    if (figureList[0].Tag == null || figure.Tag == null)
                    {
                        add = true;
                    }
                    else if (figureList[0].Tag.GetType().Name == figure.Tag.GetType().Name)
                    {
                        add = true;
                    }
                }
                else
                {
                    add = true;
                }

                if (add)
                {
                    Figure fig = figureList.Find(f => f.Tag.Equals(figure.Tag));
                    figureList.Add(figure);
                }
            }
        }

        public void AddFigure(List<Figure> figureList)
        {
            if (figureList != null && figureList.Count > 0)
            {
                LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigure.List");
                foreach (Figure figure in figureList)
                {
                    AddFigure(figure);
                }
                //                this.figureList.AddRange(figureList);
            }
        }

        public Figure GetFirstFigure()
        {
            if (figureList.Count > 0)
            {
                return figureList[0];
            }

            return null;
        }

        public bool IsSelected(Figure figure)
        {
            int found = figureList.IndexOf(figure);
            return found >= 0;
        }

        public void MouseDown(MouseEventArgs e)
        {
            Point startTrackPos = e.Location;
            onSelectRange = false;
            onMoveFigure = false;
            onZoomRange = false;

            if (Enable)
            {
                //Debug.WriteLine(string.Format("Tracker.MouseDown. {0}", startTrackPos));

                if (ShiftPositionMode)
                {

                }
                else if (addFigureMode)
                {
                    onSelectRange = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    onZoomRange = true;
                }
                else
                {
                    TrackPos = GetTrackPos(startTrackPos);
                    if (TrackPos.PosType == TrackPosType.None)
                    {
                        onSelectRange = true;
                    }
                    else
                    {
                        // 선택된 Figurer가 있음 && 하나 이상 Movable==false 이면 움직이지 않음.
                        if ((figureList.Count > 0 && (figureList.Find(f => f.Movable == false) == null)))
                        {
                            onMoveFigure = true;
                        }
                    }
                }

                TrackPointList.Clear();
                StartTrackPos = startTrackPos;
                EndTrackPos = startTrackPos;
                TrackPointList.Add(startTrackPos);

                ownerControl.Invalidate();
            }
        }

        public List<Figure> GetFigureList()
        {
            var copyFigureList = new List<Figure>();
            copyFigureList.AddRange(figureList);

            return copyFigureList;
        }

        private TrackPos GetTrackPos(Point point)
        {
            var trackPos = new TrackPos(TrackPosType.None, 0);

            int polygonIndex = 0;

            if (figureList.Count() == 1)
            {
                trackPos = figureList[0].GetTrackPos(point, CoordTransformer, RotationLocked, ref polygonIndex);
            }
            else
            {
                foreach (Figure selectedFigure in figureList)
                {
                    trackPos = selectedFigure.GetTrackPos(point, CoordTransformer, RotationLocked, ref polygonIndex);
                    if (trackPos.PosType != TrackPosType.None)
                    {
                        trackPos.PosType = TrackPosType.Inner;
                        break;
                    }
                }
            }

            return trackPos;
        }

        public bool IsSingleSelected()
        {
            return figureList.Count() == 1;
        }

        public bool IsMultiSelected()
        {
            return figureList.Count() > 1;
        }

        private bool GetSelectRange(ref RotatedRect selectRange)
        {
            var rect = new RotatedRect();
            rect.FromLTRB(Math.Min(StartTrackPos.X, EndTrackPos.X), Math.Min(StartTrackPos.Y, EndTrackPos.Y),
                            Math.Max(StartTrackPos.X, EndTrackPos.X), Math.Max(StartTrackPos.Y, EndTrackPos.Y));

            if (CoordTransformer != null)
            {
                selectRange = CoordTransformer.InverseTransform(rect);
            }
            else
            {
                selectRange = rect;
            }

            return (!(rect.Width == 0 && rect.Height == 0));
        }

        public void Offset(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            SizeF newOffset = offset;
            if (CoordTransformer != null)
            {
                newOffset = CoordTransformer.InverseTransform(offset);
            }

            foreach (Figure selectedFigure in figureList)
            {
                selectedFigure.Offset(newOffset.Width, newOffset.Height);
            }

            TrackerMoved?.Invoke();
        }

        public void Move(Size offset)
        {
            //Debug.WriteLine("Tracker.KeyUp.onMoveFigure");

            //onMoveFigure = true;
            //onSelectRange = true;
            TrackPos.PosType = TrackPosType.Inner;
            if (figureList.Count() > 0 && MoveLocked == false)
            {
                lock (figureList)
                {
                    Size newOffset = offset;
                    if (CoordTransformer != null)
                    {
                        newOffset = CoordTransformer.InverseTransform(offset);
                    }

                    foreach (Figure selectedFigure in figureList)
                    {
                        selectedFigure.TrackMove(TrackPos, newOffset, RotationLocked);
                    }

                    TrackerMoved?.Invoke();
                }
            }
            else
            {
                //Debug.WriteLine("Tracker.KeyUp.onMoveFigure - There is no selected figure.");
            }

            ownerControl.Invalidate();
            TrackPos.PosType = TrackPosType.None;
            //onMoveFigure = false;
            //onSelectRange = false;
        }

        public void MouseUp(MouseEventArgs e)
        {
            Point endTrackPos = e.Location;
            if (Enable == false)
            {
                return;
            }

            //Debug.WriteLine(string.Format("Tracker.MouseUp. {0}", startTrackPos));
            if (ShiftPositionMode)
            {
                var offset = new Size(StartTrackPos.X - endTrackPos.X, StartTrackPos.Y - endTrackPos.Y);

                PositionShifted?.Invoke(offset);
            }
            else if (addFigureMode)
            {
                //Debug.WriteLine("Tracker.MouseUp.addFigureMode");

                var selectRange = new RotatedRect();

                if (GetSelectRange(ref selectRange) == true)
                {
                    if (AddFigureCaptured != null)
                    {
                        var pointList = new List<PointF>();

                        foreach (PointF point in TrackPointList)
                        {
                            if (CoordTransformer != null)
                            {
                                pointList.Add(CoordTransformer.InverseTransform(point));
                            }
                            else
                            {
                                pointList.Add(point);
                            }
                        }
                        AddFigureCaptured(pointList);
                    }
                }
            }
            else if (onSelectRange)
            {
                //Debug.WriteLine("Tracker.MouseUp.onSelectRange");

                var selectRange = new RotatedRect();
                if (GetSelectRange(ref selectRange) == true)
                {
                    if (SelectionRectCaptured != null)
                    {
                        Point startPosTransformed = StartTrackPos;
                        Point endPosTransformed = endTrackPos;
                        if (CoordTransformer != null)
                        {
                            startPosTransformed = CoordTransformer.InverseTransform(StartTrackPos);
                            endPosTransformed = CoordTransformer.InverseTransform(endTrackPos);
                        }

                        SelectionRectCaptured(DrawingHelper.ToRect(selectRange.ToRectangleF()), startPosTransformed, endPosTransformed);
                    }
                }
                else
                {
                    SelectionPointCaptured?.Invoke(new Point((int)selectRange.Left, (int)selectRange.Top));
                }
            }
            else if (onMoveFigure)
            {
                //Debug.WriteLine("Tracker.MouseUp.onMoveFigure");

                if (figureList.Count() > 0 && MoveLocked == false)
                {
                    lock (figureList)
                    {
                        var offset = new Size(endTrackPos.X - StartTrackPos.X, endTrackPos.Y - StartTrackPos.Y);
                        Size newOffset = offset;
                        if (CoordTransformer != null)
                        {
                            newOffset = CoordTransformer.InverseTransform(offset);
                        }

                        foreach (Figure selectedFigure in figureList)
                        {
                            selectedFigure.TrackMove(TrackPos, newOffset, RotationLocked);
                        }

                        TrackerMoved?.Invoke();
                    }
                }
                else
                {
                    //Debug.WriteLine("Tracker.MouseUp.onMoveFigure - There is no selected figure.");
                }
            }
            else if (onZoomRange)
            {
                if (SelectionRectZoom != null)
                {
                    var selectRange = new RotatedRect();
                    //if (GetSelectRange(ref selectRange) == true)
                    GetSelectRange(ref selectRange);
                    SelectionRectZoom(selectRange.ToRectangleF());
                }
            }
            onSelectRange = false;
            onMoveFigure = false;
            onZoomRange = false;

            ownerControl.Invalidate();
        }

        public void RemoveFigure(Figure figure)
        {
            if (figure != null)
            {
                LogHelper.Debug(LoggerType.Operation, "Tracker.RemoveFigure.");
                figureList.Remove(figure);
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            Point point = e.Location;

            if (Enable == false)
            {
                return;
            }

            //Debug.WriteLine("Tracker.MouseMove");

            //Debug.WriteLine(string.Format("Tracker.MouseMove. {0}", point));
            EndTrackPos = point;
            TrackPointList.Add(point);

            //Debug.WriteLine(endTrackPos);

            ownerControl.Invalidate();
        }

        public void Draw(Graphics g, CoordTransformer coordTransformer)
        {
            if (ShiftPositionMode || MeasureMode)
            {
                using (var p = new Pen(Color.Red, 1.0F))
                {
                    p.DashStyle = DashStyle.Dot;

                    g.DrawLine(p, StartTrackPos, EndTrackPos);
                }
            }
            else
            {
                using (var p = new Pen(Color.Blue, 2.0F))
                {
                    if (figureList.Count > 0)
                    {
                        //Debug.WriteLine("figureList.Count > 0 TRUE");
                        foreach (Figure selectedFigure in figureList)
                        {
                            selectedFigure.DrawSelection(g, coordTransformer, RotationLocked);
                        }
                    }

                    if (onSelectRange || onMoveFigure || onZoomRange)
                    {
                        //Debug.WriteLine("Draw Track Line");
                        List<GraphicsPath> trackPathList = GetTrackPath();
                        foreach (GraphicsPath graphicsPath in trackPathList)
                        {
                            g.DrawPath(p, graphicsPath);
                        }
                    }
                }
            }
        }

        private List<GraphicsPath> GetTrackPath()
        {
            RotatedRect trackRect;
            var trackPathList = new List<GraphicsPath>();

            if (onMoveFigure)
            {
                foreach (Figure selectedFigure in figureList)
                {
                    var graphicsPath = new GraphicsPath();

                    Point transStartTrackPos = StartTrackPos;
                    Point transEndTrackPos = EndTrackPos;

                    //                    Debug.WriteLine("Figure Offset : " + offset.ToString());

                    if (CoordTransformer != null)
                    {
                        transStartTrackPos = CoordTransformer.InverseTransform(StartTrackPos);
                        transEndTrackPos = CoordTransformer.InverseTransform(EndTrackPos);
                    }

                    var offset = new Size(transEndTrackPos.X - transStartTrackPos.X, transEndTrackPos.Y - transStartTrackPos.Y);

                    //Debug.WriteLine("Figure Offset : " + offset.ToString());

                    trackRect = selectedFigure.GetTrackingRect(TrackPos, offset, RotationLocked);
                    if (CoordTransformer != null)
                    {
                        trackRect = CoordTransformer.Transform(trackRect);
                    }

                    graphicsPath.AddRectangle(trackRect.ToRectangleF());

                    float angle = trackRect.Angle;

                    //Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
                    //rotationTransform.RotateAt(-angle, DrawingHelper.CenterPoint(trackRect));
                    //graphicsPath.Transform(rotationTransform);

                    trackPathList.Add(graphicsPath);
                }
            }
            else
            {
                trackRect = new RotatedRect();
                trackRect.FromLTRB(Math.Min(StartTrackPos.X, EndTrackPos.X), Math.Min(StartTrackPos.Y, EndTrackPos.Y),
                                Math.Max(StartTrackPos.X, EndTrackPos.X), Math.Max(StartTrackPos.Y, EndTrackPos.Y));

                if (Shape == FigureType.Grid)
                {
                    DrawGrid(trackPathList, trackRect);
                }
                else
                {
                    var graphicsPath = new GraphicsPath();

                    switch (Shape)
                    {
                        default:
                        case FigureType.Rectangle:
                            graphicsPath.AddRectangle(trackRect.ToRectangleF());
                            break;
                        case FigureType.Ellipse:
                            graphicsPath.AddEllipse(trackRect.ToRectangleF());
                            break;
                        case FigureType.Line:
                            graphicsPath.AddLine(StartTrackPos, EndTrackPos);
                            break;
                        case FigureType.Grid:
                            break;
                        case FigureType.Polygon:
                            graphicsPath.AddLines(TrackPointList.ToArray());
                            break;
                    }

                    trackPathList.Add(graphicsPath);
                }
            }

            return trackPathList;
        }

        private void DrawGrid(List<GraphicsPath> trackPathList, RotatedRect trackRect)
        {
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(trackRect.ToRectangleF());
            trackPathList.Add(graphicsPath);

            if (NumGridColumn > 1 && NumGridRow > 1)
            {
                float cellWidth = trackRect.Width / NumGridColumn;
                float cellHeight = trackRect.Height / NumGridRow;

                for (int x = 1; x < NumGridColumn; x++)
                {
                    graphicsPath = new GraphicsPath();
                    graphicsPath.AddLine(new PointF(trackRect.X + x * cellWidth, trackRect.Top), new PointF(trackRect.X + x * cellWidth, trackRect.Bottom));
                    trackPathList.Add(graphicsPath);
                }

                for (int y = 1; y < NumGridRow; y++)
                {
                    graphicsPath = new GraphicsPath();
                    graphicsPath.AddLine(new PointF(trackRect.Left, trackRect.Y + y * cellHeight), new PointF(trackRect.Right, trackRect.Y + y * cellHeight));
                    trackPathList.Add(graphicsPath);
                }
            }
        }
    }
}
