using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public delegate void FovChangedDelegate(int fovNo, PointF position);
    public delegate void FovMovedDelegate(List<Figure> figureList);

    public partial class FovNavigator : UserControl
    {
        public FovChangedDelegate FovChanged;
        public FovMovedDelegate FovMoved;
        private AxisHandler robotStage;
        public AxisHandler RobotStage
        {
            set
            {
                robotStage = value;
                if (robotStage != null)
                {
                    workingRect = robotStage.GetWorkingRange();
                }
            }
        }
        public FigureGroup FovList { get; set; } = new FigureGroup();

        private RectangleF workingRect;
        public SizeF FovSize { get; set; }
        public PointF CurrentPosition { get; set; }
        public bool InvertY { get; set; }

        private bool appendMode;
        private Tracker tracker;

        private bool enable = false;
        public bool Enable
        {
            set
            {
                enable = value;
                tracker.Enable = value;
            }
        }
        public float ViewScale { get; set; } = 1;
        public int SelectedFovNo { get; set; } = -1;

        public FovNavigator()
        {
            InitializeComponent();
            tracker = new Tracker(this);
            tracker.Enable = true;
            tracker.RotationLocked = true;
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
        }

        public void ClearFovList()
        {
            FovList.Clear();
            tracker.ClearFigure();
        }

        public Figure AddFovFigure(PointF robotPos)
        {
            var rect = new RectangleF(robotPos.X - FovSize.Width / 2, robotPos.Y - FovSize.Height / 2, FovSize.Width, FovSize.Height);
            var rectangleFigure = new RectangleFigure(rect, new Pen(Color.Red));

            FovList.AddFigure(rectangleFigure);

            return rectangleFigure;
        }

        public void ResetSelection()
        {
            Debug.WriteLine("RobotNavigator.ResetSelection");

            tracker.ClearFigure();
        }

        public void SelectFigure(Figure figure)
        {
            Debug.WriteLine("RobotNavigator.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void SelectFigureByTag(object tag)
        {
            Debug.WriteLine("RobotNavigator.SelectFigureByTag");
            tracker.AddFigure(FovList.GetFigureByTag(tag));
        }

        public void SelectFov(int fovNo)
        {
            Figure fovFigure = FovList.GetFigureByTagStr(fovNo);
            if (fovFigure != null)
            {
                tracker.ClearFigure();
                tracker.AddFigure(fovFigure);


                SelectedFovNo = (int)fovFigure.Tag;
                CurrentPosition = DrawingHelper.CenterPoint(fovFigure.GetRectangle());

                FovChanged?.Invoke(SelectedFovNo, CurrentPosition);

                Invalidate();
            }
        }

        public void SelectFigure(List<Figure> figureList)
        {
            tracker.AddFigure(figureList);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (enable == false)
            {
                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            appendMode = (Control.ModifierKeys == Keys.Control);

            Debug.WriteLine("RobotNavigator.OnMouseDown");

            tracker.CoordTransformer = GetCoordTransformer();

            tracker.MouseDown(e);

            base.OnMouseDown(e);
        }

        private void Tracker_SelectionPointCaptured(Point point)
        {
            Debug.WriteLine("RobotNavigator.Tracker_SelectionPointCaptured");

            if (workingRect.Contains(point) == false)
            {
                return;
            }

            if (appendMode == false)
            {
                ResetSelection();
            }

            point.X = (int)MathHelper.Bound(point.X, workingRect.X, workingRect.X + workingRect.Width);
            point.Y = (int)MathHelper.Bound(point.Y, workingRect.Y, workingRect.Y + workingRect.Height);

            Figure figure = FovList.Select(point);
            if (figure != null)
            {
                tracker.AddFigure(figure);

                if (tracker.IsSingleSelected())
                {
                    SelectedFovNo = (int)figure.Tag;
                    CurrentPosition = DrawingHelper.CenterPoint(figure.GetRectangle());

                    FovChanged?.Invoke(SelectedFovNo, CurrentPosition);
                }
            }
            else
            {
                SelectedFovNo = -1;
                CurrentPosition = new PointF(point.X, point.Y);
            }

            FovChanged?.Invoke(SelectedFovNo, CurrentPosition);

            Invalidate();
        }

        private void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            Debug.WriteLine("RobotNavigator.Tracker_SelectionRectCaptured");

            if (appendMode == false)
            {
                ResetSelection();
            }

            List<Figure> figureList = FovList.Select(rectangle);
            if (figureList.Count() > 0)
            {
                tracker.AddFigure(figureList);

                if (tracker.IsSingleSelected())
                {
                    SelectedFovNo = (int)figureList[0].Tag;

                    PointF centerPoint = DrawingHelper.CenterPoint(figureList[0].GetRectangle());
                    FovChanged?.Invoke(SelectedFovNo, centerPoint);
                }
            }

            Invalidate();
        }

        private void Tracker_FigureMoved()
        {
            Debug.WriteLine("SchemaViewer.Tracker_FigureMoved");

            FovMoved?.Invoke(tracker.GetFigureList());

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            Debug.WriteLine("SchemaViewer.OnMouseUp");

            tracker.MouseUp(e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            Debug.WriteLine("SchemaViewer.OnMouseMove");

            tracker.MouseMove(e);

            base.OnMouseMove(e);
        }

        private CoordTransformer GetCoordTransformer()
        {
            var coordTransformer = new CoordTransformer();
            coordTransformer.SetSrcRect(workingRect);
            coordTransformer.SetDisplayRect(new RectangleF(0, 0, Width, Height));
            coordTransformer.InvertY = true;

            return coordTransformer;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            CoordTransformer coordTransformer = GetCoordTransformer();

            var workingRectFigure = new RectangleFigure(workingRect, new Pen(Color.Black));
            workingRectFigure.Draw(e.Graphics, coordTransformer, enable);

            var fovRect = new RectangleF(CurrentPosition.X - FovSize.Width / 2, CurrentPosition.Y - FovSize.Height / 2, FovSize.Width, FovSize.Height);
            var fovRectFigure = new RectangleFigure(fovRect, new Pen(Color.Blue, 1));
            fovRectFigure.Draw(e.Graphics, coordTransformer, enable);

            FovList.Draw(e.Graphics, coordTransformer, enable);
            tracker.Draw(e.Graphics, coordTransformer);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        public void ResetTargetResult()
        {
            FovList.ResetTempProperty();
            Invalidate();
        }

        public void UpdateTargetResult(int fovNo, bool result)
        {
            Figure fovFigure = FovList.GetFigureByTag(fovNo);

            if (result == true)
            {
                fovFigure.TempBrush = new SolidBrush(Color.LimeGreen);
            }
            else
            {
                fovFigure.TempBrush = new SolidBrush(Color.Red);
            }

            Invalidate();
        }

        private void SchemaViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetTargetResult();
        }
    }
}
