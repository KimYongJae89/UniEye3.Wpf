using DynMvp.Base;
using DynMvp.Properties;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public enum DragMode { Add, Select, Pan, Zoom, Measure, Custom };

    public delegate Figure CreateCustomFigureDelegate(PointF pt1, PointF pt2);
    public delegate void FigureCreatedDelegate(Figure figure, CoordMapper coordMapper, FigureGroup workingFigures, FigureGroup backgroundFigures);
    public delegate void FigureSelectedDelegate(List<Figure> figureList, bool select = true);
    public delegate void FigureDeletedDelegate(List<Figure> figureList);
    public delegate void FigureCopiedDelegate(List<Figure> figureList);
    public delegate void FigurePastedDelegate(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset);
    public delegate void FigureModifiedDelegate(List<Figure> figureList);
    public delegate void MouseClickedDelegate(UserControl sender, PointF point, MouseButtons button, ref bool processingCancelled);
    public delegate void MouseDblClickedDelegate(UserControl sender);
    public delegate void MouseMovedDelegate(PointF point, ref bool processingCancelled);
    public delegate void MouseDownDelegate(PointF point, ref bool processingCancelled);
    public delegate void MouseUpDelegate(PointF point, ref bool processingCancelled);
    public delegate void MeasureDelegate(RectangleF selectedRect);

    public partial class CanvasPanel : UserControl
    {
        public class Option
        {
            public Pen Pen { get; set; } = null;
            public bool ShowProbeNumber { get; set; }
            public int ProbeNumberSize { get; set; } = 20;
            public bool IncludeProbe { get; set; }
        }

        /// <summary>
        /// 내부에서 개체를 생성하고, 생성된 객체를 참조하는 Figure를 반환한다.
        /// </summary>
        public CreateCustomFigureDelegate CreateCustomFigure;
        /// <summary>
        /// Drag완료 후 새로운 Figure를 생성한 후 호출한다. FigureCreated는 Figure에 연관된 객체를 생성하여 Tag에 저장한다.
        /// 추가적인 Figure의 생성이 필요할 경우 새로운 Figure를 생성하여 additionalFIgureList에 추가한다.
        /// </summary>
        public FigureCreatedDelegate FigureCreated;
        public FigureSelectedDelegate FigureSelected;
        public FigureCopiedDelegate FigureCopied;
        public FigureDeletedDelegate FigureDeleted;
        public FigureModifiedDelegate FigureModified;
        /// <summary>
        /// 새로운 Figure의 목록이 생성된 후, 이 Delegation이 호출된다.
        /// 필요에 따라 Figure.Tag에 등록된 정보를 이용하여 Data 객체를 생성해야 한다.
        /// </summary>
        public FigurePastedDelegate FigurePasted;
        /// <summary>
        /// Mouse Clicked
        /// </summary>
        public MouseClickedDelegate ChildMouseClick;
        public MouseDblClickedDelegate ChildMouseDblClick;
        public MouseMovedDelegate ChildMouseMove;
        public MouseDownDelegate ChildMouseDown;
        public MouseUpDelegate ChildMouseUp;
        public MeasureDelegate MeasureResult;
        public DragMode CurDragMode { get; set; } = DragMode.Select;
        public DragMode DragMode { get; set; } = DragMode.Select;
        public Bitmap Image { get; private set; }
        public RectangleF ImageRegion { get; set; }
        public RectangleF CanvasRegion { get; private set; }

        private Bitmap overlayImage;
        public Bitmap OverlayImage
        {
            get => overlayImage;
            set
            {
                overlayImage = value;
                OverlayPos = new Point(0, 0);
            }
        }
        public Point OverlayPos { get; set; }
        public FigureGroup WorkingFigures { get; set; } = new FigureGroup();
        public FigureGroup BackgroundFigures { get; set; } = new FigureGroup();
        public FigureGroup TempFigures { get; set; } = new FigureGroup();

        /// <summary>
        /// 마우스가 지나가고 있는 위치에 있는 Figure
        /// </summary>
        private Figure focusedFigure = null;
        private Figure lastFocusedFigure = null;

        public object lockObject = new object();
        public bool ShowRuler { get; set; }
        public bool ShowCenterGuide { get; set; } = true;
        public Point CenterGuidePos { get; set; }
        public int CenterGuideThickness { get; set; }
        public bool ShowFigure { get; set; } = true;
        public bool UseZoom { get; set; } = true;
        public bool InvertY { get; set; } = false;
        public bool Enable { get; set; } = false;
        public bool ReadOnly { get; set; } = false;
        public bool Editable { get; set; } = false;
        public bool ShowToolbar { get; set; } = false;
        public bool RotationLocked { get; set; } = false;
        public bool SingleAxisTracking { get; set; } = false;
        public bool NoneClickMode { get; set; } = false;
        public List<PointF> TrackPointList { get; } = new List<PointF>();
        public FigureType TrackerShape { get; set; } = FigureType.Rectangle;

        private float zoomScale = 1;
        private bool onDrag = false;
        private PointF dragStart;
        private PointF dragEnd;
        private SizeF dragOffset;
        private SelectionContainer selectionContainer = new SelectionContainer();
        private bool onUpdateStateButton;
        private TrackPos curTrackPos;
        private List<Figure> copyBuffer = new List<Figure>();
        private int copyCount;

        public CanvasPanel()
        {
            InitializeComponent();
        }

        public CanvasPanel(bool noneClickMode)
        {
            InitializeComponent();
            NoneClickMode = noneClickMode;
        }

        public void ClearFigure()
        {
            WorkingFigures.Clear();
            BackgroundFigures.Clear();
            TempFigures?.Clear();
            selectionContainer.ClearSelection();

            Invalidate();
        }

        public void SetAddMode(FigureType trackerShape)
        {
            Cursor = Cursors.Cross;
            DragMode = DragMode.Add;
            TrackerShape = trackerShape;
        }

        public RectangleF GetBoundRect()
        {
            var boundRect = WorkingFigures.GetRectangle().ToRectangleF();
            if (BackgroundFigures != null)
            {
                boundRect = DrawingHelper.GetUnionRect(boundRect, BackgroundFigures.GetRectangle().ToRectangleF());
            }

            if (TempFigures != null)
            {
                boundRect = DrawingHelper.GetUnionRect(boundRect, TempFigures.GetRectangle().ToRectangleF());
            }

            return DrawingHelper.GetUnionRect(boundRect, ImageRegion);
        }

        public void AddFigure(Figure figure)
        {
            selectionContainer.AddFigure(figure);
        }

        public void SelectFigure(Figure figure)
        {
            if (NoneClickMode)
            {
                return;
            }

            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figure);

            FigureSelected?.Invoke(new List<Figure>() { figure });
        }

        public void SelectFigure(List<Figure> figureList)
        {
            if (NoneClickMode)
            {
                return;
            }

            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figureList);

            FigureSelected?.Invoke(figureList);
        }

        public void SelectFigureByTag(List<object> tagList)
        {
            if (NoneClickMode)
            {
                return;
            }

            foreach (object tag in tagList)
            {
                selectionContainer.AddFigure(WorkingFigures.GetFigureByTag(tag));
            }
        }

        public void SelectFigureByTag(object tag)
        {
            if (NoneClickMode)
            {
                return;
            }

            selectionContainer.AddFigure(WorkingFigures.GetFigureByTag(tag));
        }

        public void DeleteSelection()
        {
            List<Figure> figureList = selectionContainer.GetRealFigures();
            //foreach (Figure selectedFigure in figureList)
            //{
            //    workingFigures.RemoveFigure(selectedFigure);
            //}

            FigureDeleted?.Invoke(figureList);

            selectionContainer.ClearSelection();
            Invalidate(true);
        }

        public virtual void ClearSelection()
        {
            selectionContainer.ClearSelection();
            Invalidate();
        }

        public void ResetImage()
        {
            //if (this.image != null)
            //{
            //    this.image.Dispose();
            //    this.image = null;

            //    Invalidate();
            //}

            if (Image != null)
            {
                Image.Dispose();
                Image = null;

                Invalidate();
            }
        }

        public void UpdateImage(Bitmap image)
        {
            if (image != null)
            {
                UpdateImage(image, new RectangleF(0, 0, image.Width, image.Height));
            }
            else
            {
                ResetImage();
            }
        }

        public void UpdateImage(Bitmap image, RectangleF imageRegion)
        {
            Debug.Assert(image != null);

            bool zoomFitRequired = (Image == null);

            lock (lockObject)
            {
                if (Image != null)
                {
                    Image.Dispose();
                    Image = null;
                }

                if (image != null)
                {
                    Image = ImageHelper.CloneImage(image);
                    //this.image = image;

                    ImageRegion = imageRegion;
                }

                if (zoomFitRequired || !UseZoom)
                {
                    ZoomFit();
                }

                Invalidate();
            }
        }

        public void UpdateCenterGuide(bool showCenterGuide, Point guidePos, int thicknes)
        {
            ShowCenterGuide = showCenterGuide;
            if (showCenterGuide)
            {
                CenterGuidePos = guidePos;
                CenterGuideThickness = thicknes;
            }

            Invalidate();
        }

        private List<GraphicsPath> GetTrackPath()
        {
            RotatedRect trackRect;
            var trackPathList = new List<GraphicsPath>();

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);
            var scaledDragOffset = new SizeF(0, 0); // coordMapper.PixelToWorld(dragOffset);

            var graphicsPath = new GraphicsPath();

            if (CurDragMode == DragMode.Pan)
            {
                graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                trackPathList.Add(graphicsPath);

                return trackPathList;
            }
            else if (CurDragMode == DragMode.Select && curTrackPos.PosType != TrackPosType.None)
            {
                selectionContainer.GetTrackPath(trackPathList, scaledDragOffset, curTrackPos);

                return trackPathList;
            }

            trackRect = new RotatedRect();
            trackRect.FromLTRB(Math.Min(scaledDragStart.X, scaledDragEnd.X), Math.Min(scaledDragStart.Y, scaledDragEnd.Y),
                            Math.Max(scaledDragStart.X, scaledDragEnd.X), Math.Max(scaledDragStart.Y, scaledDragEnd.Y));

            switch (TrackerShape)
            {
                default:
                case FigureType.Rectangle:
                    graphicsPath.AddRectangle(trackRect.ToRectangleF());
                    break;
                case FigureType.Ellipse:
                    graphicsPath.AddEllipse(trackRect.ToRectangleF());
                    break;
                case FigureType.Line:
                    if (SingleAxisTracking)
                    {
                        if (Math.Abs(dragOffset.Width) > Math.Abs(dragOffset.Height))
                        {
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragEnd.X, scaledDragStart.Y));
                        }
                        else
                        {
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragStart.X, scaledDragEnd.Y));
                        }
                    }
                    else
                    {
                        graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                    }
                    break;
                case FigureType.Polygon:
                    graphicsPath.AddLines(TrackPointList.ToArray());
                    break;
            }

            trackPathList.Add(graphicsPath);

            return trackPathList;
        }

        public CoordMapper GetCoordMapper()
        {
            var m = new Matrix();

            if (InvertY)
            {
                m.Scale(zoomScale, -zoomScale);
                m.Translate(-CanvasRegion.X, -(CanvasRegion.Height + CanvasRegion.Y));
            }
            else
            {
                m.Scale(zoomScale, zoomScale);
                m.Translate(-CanvasRegion.X, -CanvasRegion.Y);
            }

            if (CurDragMode == DragMode.Pan)
            {
                m.Translate(dragOffset.Width, dragOffset.Height, MatrixOrder.Append);
            }

            return new CoordMapper(m);
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            lock (lockObject)
            {
                int clientHeight = Height - statusBar.Height;

                var clientRect = new Rectangle(0, 0, Width, clientHeight);

                e.Graphics.SetClip(clientRect, CombineMode.Replace);

                Brush bkBrush = new SolidBrush(Color.Black);
                e.Graphics.FillRectangle(bkBrush, clientRect);
                bkBrush.Dispose();

                CoordMapper mapper = GetCoordMapper();

                Matrix preTransform = e.Graphics.Transform;
                e.Graphics.Transform = mapper.Matrix;
                if (Image != null)
                {
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                    //*
                    e.Graphics.DrawImage(Image, ImageRegion);
                    //* 이 부분에 메모리 누수가 대량발생

                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                }

                if (overlayImage != null)
                {
                    var colorMatrix = new ColorMatrix();
                    var ia = new ImageAttributes();

                    var imgRect = new Rectangle(OverlayPos.X, OverlayPos.Y, overlayImage.Width, overlayImage.Height);
                    colorMatrix.Matrix33 = 0.50f;
                    ia.SetColorMatrix(colorMatrix);

                    e.Graphics.DrawImage(overlayImage, imgRect, 0, 0, overlayImage.Width, overlayImage.Height, GraphicsUnit.Pixel, ia);
                    ia.Dispose();
                }

                if (ShowCenterGuide && Image != null)
                {
                    e.Graphics.Transform = preTransform;

                    var pen = new Pen(Color.Blue, 1)
                    {
                        DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                    };

                    var ptOffset = new PointF(0, 0);
                    if (CurDragMode == DragMode.Pan)
                    {
                        ptOffset.X = dragOffset.Width;
                        ptOffset.Y = dragOffset.Height;
                    }

                    float v = e.ClipRectangle.Height / CanvasRegion.Height;

                    e.Graphics.DrawLine(pen,
                        new PointF((0 - CanvasRegion.X) * zoomScale + ptOffset.X, (Image.Height / 2 - CanvasRegion.Y) * zoomScale + ptOffset.Y),
                        new PointF((Image.Width - CanvasRegion.X) * zoomScale + ptOffset.X, (Image.Height / 2 - CanvasRegion.Y) * zoomScale + ptOffset.Y));
                    e.Graphics.DrawLine(pen,
                        new PointF((Image.Width / 2 - CanvasRegion.X) * zoomScale + ptOffset.X, (0 - CanvasRegion.Y) * zoomScale + ptOffset.Y),
                        new PointF((Image.Width / 2 - CanvasRegion.X) * zoomScale + ptOffset.X, (Image.Height - CanvasRegion.Y) * zoomScale + ptOffset.Y));

                    e.Graphics.Transform = mapper.Matrix;

                    pen.Dispose();
                }

                if (onDrag)
                {
                    if (CurDragMode == DragMode.Select)
                    {
                        float penWidth = 3 / zoomScale;
                        using (var p = new Pen(Color.Red, penWidth))
                        {
                            p.DashStyle = DashStyle.Dot;

                            List<GraphicsPath> trackPathList = GetTrackPath();
                            foreach (GraphicsPath graphicsPath in trackPathList)
                            {
                                e.Graphics.DrawPath(p, graphicsPath);
                            }

                            p.Dispose();
                        }
                    }
                }

                if (ShowFigure)
                {
                    if (WorkingFigures != null)
                    {
                        WorkingFigures.Draw(e.Graphics, null, false);
                    }

                    if (TempFigures != null)
                    {
                        TempFigures.Draw(e.Graphics, null, false);
                    }

                    if (BackgroundFigures != null)
                    {
                        BackgroundFigures.Draw(e.Graphics, null, false);
                    }
                }

                if (focusedFigure != null)
                {
                    using (var p = new Pen(Color.OrangeRed, 0))
                    {
                        p.DashStyle = DashStyle.Dot;

                        GraphicsPath focusedFigurePath = focusedFigure.GetGraphicsPath();
                        e.Graphics.DrawPath(p, focusedFigurePath);

                        p.Dispose();
                    }
                }

                e.Graphics.Transform = preTransform;

                selectionContainer.Draw(e.Graphics, mapper, RotationLocked);
            }
        }

        private void CanvasPanel_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void UpdateCursor(DragMode dragMode)
        {
            switch (dragMode)
            {
                case DragMode.Add:
                    Cursor = Cursors.Cross;
                    break;
                case DragMode.Pan:
                    Cursor = Cursors.Hand;
                    break;
                case DragMode.Measure:
                case DragMode.Zoom:
                    //Cursor = new Cursor(new System.IO.MemoryStream(DynMvp.Properties.Resources.zoom_in));
                    //Cursor = new Cursor(GetType(), "zoom_in.cur");
                    //Cursor = new Cursor(@"D:\Project\PrintEye\Source\DynMvp\Resources\zoom-in.cur");
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (NoneClickMode)
            {
                return;
            }

            bool cancelled = false;
            ChildMouseDown?.Invoke(new PointF(e.X, e.Y), ref cancelled);
            if (cancelled == true)
            {
                return;
            }

            dragStart = dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(0, 0);
            onDrag = true;

            if (e.Button == MouseButtons.Left)
            {
                CurDragMode = DragMode.Select;
            }
            else if (e.Button == MouseButtons.Right)
            {
                CurDragMode = DragMode.Pan;
            }

            //if (Control.ModifierKeys == Keys.Shift)
            //    curDragMode = DragMode.Pan;
            //else if (Control.ModifierKeys == Keys.Alt)
            //    curDragMode = DragMode.Select;
            //else
            //    curDragMode = dragMode;

            if (CurDragMode == DragMode.Select)
            {
                CoordMapper coordMapper = GetCoordMapper();
                curTrackPos = selectionContainer.GetTrackPos(coordMapper, dragStart, RotationLocked);
            }

            UpdateCursor(CurDragMode);
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (NoneClickMode)
            {
                return;
            }

            bool cancelled = false;
            ChildMouseMove?.Invoke(new PointF(e.X, e.Y), ref cancelled);
            if (cancelled == true)
            {
                return;
            }

            UpdateCursor(DragMode);

            CoordMapper coordMapper = GetCoordMapper();

            if (onDrag == true)
            {
                dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);
                var dragOffset2 = new SizeF(e.X - dragEnd.X, e.Y - dragEnd.Y);
                dragEnd = new PointF(e.X, e.Y);

                SizeF size = coordMapper.PixelToWorld(dragOffset);
                statusBar.Panels["Size"].Text = string.Format("{0:0.00}, {1:0.00}", size.Width, size.Height);

                SizeF size2 = coordMapper.PixelToWorld(dragOffset2);
                selectionContainer.TrackMove(curTrackPos, size2, true, false);
                Invalidate(true);
            }
            else
            {
                PointF point = coordMapper.PixelToWorld(new PointF(e.X, e.Y));
                statusBar.Panels["Pos"].Text = string.Format("{0:0.00}, {1:0.00}", point.X, point.Y);

                focusedFigure = WorkingFigures.Select(point);
                if (focusedFigure != lastFocusedFigure)
                {
                    Invalidate(true);
                    lastFocusedFigure = focusedFigure;
                }
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (NoneClickMode)
            {
                return;
            }

            bool cancelled = false;
            ChildMouseDown?.Invoke(new PointF(e.X, e.Y), ref cancelled);
            if (cancelled == true)
            {
                return;
            }

            dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            bool processingCancelled = false;
            ChildMouseClick?.Invoke(this, scaledDragEnd, e.Button, ref processingCancelled);
            if (processingCancelled == true)
            {
                return;
            }

            if (onDrag == true)
            {
                switch (CurDragMode)
                {
                    case DragMode.Add:
                        AddFigure();
                        break;
                    case DragMode.Pan:
                        Pan();
                        break;
                    case DragMode.Select:
                        if (/*(focusedFigure != null && selectionContainer.IsSelected(focusedFigure) == false) ||*/ curTrackPos.PosType == TrackPosType.None)
                        {
                            SelectRange();
                        }
                        else
                        {
                            ModifyFigure();
                        }

                        break;
                    case DragMode.Measure:
                        Measure();
                        break;
                    case DragMode.Zoom:
                        ZoomRange();
                        break;
                }

                statusBar.Panels["Size"].Text = "";
                dragOffset = new SizeF(0, 0);
                Invalidate();
                onDrag = false;
                CurDragMode = DragMode.Select;
            }
        }

        private Figure CreateFigure(PointF pt1, PointF pt2)
        {
            Figure figure = null;
            switch (TrackerShape)
            {
                case FigureType.Ellipse:
                    figure = new EllipseFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
                case FigureType.Rectangle:
                    figure = new RectangleFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
            }

            return figure;
        }

        private void AddFigure()
        {
            selectionContainer.ClearSelection();

            if (Math.Abs(dragOffset.Width) < 5 && Math.Abs(dragOffset.Height) < 5)
            {
                return;
            }

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            Figure figure = null;

            if (TrackerShape == FigureType.Custom)
            {
                if (CreateCustomFigure != null)
                {
                    figure = CreateCustomFigure(scaledDragStart, scaledDragEnd);
                }
            }
            else
            {
                figure = CreateFigure(scaledDragStart, scaledDragEnd);
            }

            if (figure != null)
            {
                var workingFigures = new FigureGroup();
                var backgroundFigures = new FigureGroup();
                if (FigureCreated != null)
                {
                    FigureCreated(figure, coordMapper, workingFigures, backgroundFigures);

                    foreach (Figure createdFigure in workingFigures)
                    {
                        WorkingFigures.AddFigure(createdFigure);
                    }

                    foreach (Figure createdFigure in backgroundFigures)
                    {
                        BackgroundFigures.AddFigure(createdFigure);
                    }

                    selectionContainer.AddFigure(workingFigures[0]);
                }
                else
                {
                    workingFigures.AddFigure(figure);
                    selectionContainer.AddFigure(figure);
                }

            }

            Invalidate();
        }

        private void Pan()
        {
            int clientHeight = Height - statusBar.Height;

            var clientRect = new Rectangle(0, 0, Width, clientHeight);

            if (InvertY)
            {
                CanvasRegion = new RectangleF(CanvasRegion.X - dragOffset.Width / zoomScale, CanvasRegion.Y + dragOffset.Height / zoomScale,
                                        CanvasRegion.Width, CanvasRegion.Height);
            }
            else
            {
                CanvasRegion = new RectangleF(CanvasRegion.X - dragOffset.Width / zoomScale, CanvasRegion.Y - dragOffset.Height / zoomScale,
                                        CanvasRegion.Width, CanvasRegion.Height);
            }
        }

        private void ModifyFigure()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(dragOffset);

            if (curTrackPos.PosType == TrackPosType.Inner)
            {
                selectionContainer.Offset(scaledDragOffset);
            }
            else
            {
                selectionContainer.TrackMove(curTrackPos, scaledDragOffset, RotationLocked, true);
            }

            FigureModified?.Invoke(selectionContainer.GetRealFigures());

            Invalidate(true);
        }

        private void SelectRange()
        {
            if (Control.ModifierKeys != Keys.Control)
            {
                selectionContainer.ClearSelection();
            }

            CoordMapper coordMapper = GetCoordMapper();

            var figureList = new List<Figure>();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);

            // 선택 영역이 작으면 시작위치에 있는 객체를 얻어오고
            // 선택 영역이 크면 시작 위치와 종료 위치의 Rectangle 영역안의 객체 목록을 얻어 온다.
            if (Math.Abs(dragOffset.Width) > 5 && Math.Abs(dragOffset.Height) > 5)
            {
                PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

                RectangleF selectedRect = DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd);
                figureList = WorkingFigures.Select(Rectangle.Round(selectedRect));
                figureList.Sort((x, y) => x.ObjectLevel - y.ObjectLevel);

                selectionContainer.AddFigure(figureList);
            }
            else
            {
                Figure figure = WorkingFigures.Select(scaledDragStart);
                if (figure != null)
                {
                    selectionContainer.AddFigure(figure);

                    figureList.Add(figure);
                }
            }

            FigureSelected?.Invoke(figureList);
        }

        public virtual void Measure()
        {
            if (Math.Abs(dragOffset.Width) < 5 && Math.Abs(dragOffset.Height) < 5)
            {
                return;
            }

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            var startPoint = new PointF()
            {
                X = Math.Min(scaledDragStart.X, scaledDragEnd.X),
                Y = Math.Min(scaledDragStart.Y, scaledDragEnd.Y)
            };

            var scaledSize = new SizeF()
            {
                Width = Math.Abs(scaledDragEnd.X - scaledDragStart.X),
                Height = Math.Abs(scaledDragEnd.Y - scaledDragStart.Y)
            };

            MeasureResult?.Invoke(new RectangleF(startPoint, scaledSize));
        }

        private void ZoomRange()
        {
            CoordMapper coordMapper = GetCoordMapper();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            ZoomRange(DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            UpdateZoom((float)(e.Delta > 0 ? 1.1 : 0.9), new PointF(e.X, e.Y));
        }

        private void UpdateZoom(float zoomOffset, PointF zoomCenter)
        {
            float newZoomScale = zoomScale * zoomOffset; // (float)Math.Pow(2, zoomStep);

            int clientHeight = Height - statusBar.Height;

            var clientRect = new Rectangle(0, 0, Width, clientHeight);

            var newCanvasSize = new SizeF(clientRect.Width / newZoomScale, clientRect.Height / newZoomScale);

            PointF curZoomPos;
            PointF newLeftTopPos;

            if (InvertY)
            {
                curZoomPos = new PointF(CanvasRegion.X + zoomCenter.X / zoomScale, CanvasRegion.Y + (clientRect.Height - zoomCenter.Y) / zoomScale);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale, curZoomPos.Y - (clientRect.Height - zoomCenter.Y) / newZoomScale);
            }
            else
            {
                curZoomPos = new PointF(CanvasRegion.X + zoomCenter.X / zoomScale, CanvasRegion.Y + zoomCenter.Y / zoomScale);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale, curZoomPos.Y - zoomCenter.Y / newZoomScale);
            }
            CanvasRegion = new RectangleF(newLeftTopPos, newCanvasSize);

            zoomScale = newZoomScale;

            Invalidate();
        }

        public void ZoomFit()
        {
            RectangleF boundRect = GetBoundRect();
            ZoomRange(boundRect, true);
        }

        public void ZoomRange(RectangleF zoomRange, bool isCenter = false)
        {
            if (zoomRange.Width == 0 || zoomRange.Height == 0)
            {
                return;
            }

            int clientHeight = Height - statusBar.Height;

            float scaleX = Width / zoomRange.Width;
            float scaleY = clientHeight / zoomRange.Height;
            // 555/630
            if (scaleX < scaleY)
            {
                CanvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, zoomRange.Width, clientHeight / scaleX);
                zoomScale = scaleX;

                if (isCenter)
                {
                    CanvasRegion.Offset(0, ((zoomRange.Height / 2) - (clientHeight / 2) / zoomScale));
                }
            }
            else
            {
                CanvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, Width / scaleY, zoomRange.Height);
                zoomScale = scaleY;

                if (isCenter)
                {
                    CanvasRegion.Offset((zoomRange.Width / 2) - (Width / 2) / zoomScale, 0);
                }
            }

            Invalidate();
        }

        public void ZoomIn()
        {
            int clientHeight = Height - statusBar.Height;
            var clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
        }

        public void ZoomOut()
        {
            int clientHeight = Height - statusBar.Height;
            var clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
        }

        private void CanvasPanel_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    workingFigures.Delete(selectionContainer.Figures);
            //    selectionContainer.ClearSelection();
            //    return;
            //}

            //Size offset = new Size(0, 0);
            //if (e.KeyCode == Keys.Down)
            //    offset.Height = 1;
            //else if (e.KeyCode == Keys.Up)
            //    offset.Height = -1;
            //else if (e.KeyCode == Keys.Left)
            //    offset.Width = -1;
            //else if (e.KeyCode == Keys.Right)
            //    offset.Width = 1;

            //if (offset != new Size(0, 0))
            //{
            //    CoordMapper coordMapper = GetCoordMapper();
            //    selectionContainer.Offset(coordMapper.PixelToWorld(offset));
            //}
            //else
            //{
            //    int clientHeight = Height - statusBar.Height;

            //    Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            //    if (e.KeyData == Keys.Z)
            //        UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
            //    else if (e.KeyData == Keys.X)
            //        UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
            //}
        }

        private void StatusBar_ButtonClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            if (onUpdateStateButton == true)
            {
                return;
            }

            switch (e.Panel.Key)
            {
                case "Add":
                    DragMode = DragMode.Add;
                    Cursor = Cursors.Cross;
                    break;
                case "Pan":
                    Cursor = Cursors.Hand;
                    DragMode = DragMode.Pan;
                    break;
                case "None":
                case "Select":
                    Cursor = Cursors.Arrow;
                    DragMode = DragMode.Select;
                    break;
                case "Measure":
                    Cursor = Cursors.Arrow;
                    DragMode = DragMode.Measure;
                    break;
                case "ZoomRange":
                    Cursor = Cursors.Arrow;
                    DragMode = DragMode.Zoom;
                    break;
                case "Cross":
                    ShowCenterGuide = !ShowCenterGuide;
                    Invalidate();
                    break;
                case "ZoomFit":
                    ZoomFit();
                    break;
                case "ZoomIn":
                    ZoomIn();
                    break;
                case "ZoomOut":
                    ZoomOut();
                    break;
                case "Copy":
                    Copy();
                    break;
                case "Paste":
                    Paste();
                    break;
                case "Delete":
                    Delete();
                    break;
            }

            UpdateStateButton();
        }

        public void Copy()
        {
            copyCount = 1;
            copyBuffer.Clear();
            foreach (Figure selectedFigure in selectionContainer)
            {
                copyBuffer.Add((Figure)selectedFigure.Tag);
            }

            FigureCopied?.Invoke(copyBuffer);
        }

        public void Paste()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF pasteOffset = coordMapper.PixelToWorld(new Size(10 * copyCount, 10 * copyCount));

            var workingFigures = new FigureGroup();
            var backgroundFigures = new FigureGroup();
            FigurePasted?.Invoke(copyBuffer, workingFigures, backgroundFigures, pasteOffset);

            foreach (Figure figure in workingFigures)
            {
                WorkingFigures.AddFigure(figure);
            }

            foreach (Figure figure in backgroundFigures)
            {
                BackgroundFigures.AddFigure(figure);
            }

            copyCount++;

            Invalidate(true);
        }

        private void Delete()
        {
            DeleteSelection();
        }

        private void UpdateStateButton()
        {
            onUpdateStateButton = true;

            if (statusBar.Panels["Add"].Checked != (DragMode == DragMode.Add))
            {
                statusBar.Panels["Add"].Checked = DragMode == DragMode.Add;
            }

            if (statusBar.Panels["Pan"].Checked != (DragMode == DragMode.Pan))
            {
                statusBar.Panels["Pan"].Checked = DragMode == DragMode.Pan;
            }

            if (statusBar.Panels["Select"].Checked != (DragMode == DragMode.Select))
            {
                statusBar.Panels["Select"].Checked = DragMode == DragMode.Select;
            }

            if (statusBar.Panels["Measure"].Checked != (DragMode == DragMode.Measure))
            {
                statusBar.Panels["Measure"].Checked = DragMode == DragMode.Measure;
            }

            if (statusBar.Panels["ZoomRange"].Checked != (DragMode == DragMode.Zoom))
            {
                statusBar.Panels["ZoomRange"].Checked = DragMode == DragMode.Zoom;
            }

            onUpdateStateButton = false;
        }

        private void CanvasPanel_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            DragMode = DragMode.Select;

            TempFigures = null;
            Invalidate();

            ChildMouseDblClick?.Invoke(this);
        }

        private void CanvasPanel_Load(object sender, EventArgs e)
        {
            if (ShowToolbar == false)
            {
                statusBar.Height = 0;
            }
        }

        private void CanvasPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            else
            {
                int accel = (Control.ModifierKeys == Keys.Alt ? 5 : 1);

                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        Delete();
                        break;
                    case Keys.Z:
                        ShowFigure = !ShowFigure;
                        Invalidate();
                        break;
                    case Keys.Q:
                        ZoomIn();
                        break;
                    case Keys.W:
                        ZoomOut();
                        break;
                    case Keys.Left:
                        Offset(new SizeF(-accel, 0));
                        break;
                    case Keys.Right:
                        Offset(new SizeF(accel, 0));
                        break;
                    case Keys.Up:
                        Offset(new SizeF(0, -accel));
                        break;
                    case Keys.Down:
                        Offset(new SizeF(0, accel));
                        break;
                }
            }
        }

        private void Offset(SizeF size)
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(size);

            selectionContainer.Offset(scaledDragOffset);
            selectionContainer.TrackMove(new TrackPos(TrackPosType.Inner, 0), scaledDragOffset, true, false);

            FigureModified?.Invoke(selectionContainer.GetRealFigures());

            Invalidate(true);
            Update();
        }
    }
}
