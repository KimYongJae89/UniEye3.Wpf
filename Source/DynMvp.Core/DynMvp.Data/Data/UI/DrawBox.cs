using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public delegate void AddRegionCapturedDelegate(Rectangle rectangle, Point startPoint, Point endPoint);
    public delegate bool FigureSelectableDelegate(Figure figure);
    public delegate void FigureAddDelegate(List<PointF> pointList, FigureType figureType);
    public delegate void FigureMovedDelegate(List<Figure> figureList);
    public delegate void FigureCopyDelegate(List<Figure> figureList);
    public delegate Bitmap ImageProcessingDelegate(Bitmap bitmap);
    public delegate void ImageUpdatedDelegate();
    public enum AutoFitStyle { FitAll, FitWidthOnly, FitHeigthOnly, KeepRatio }

    public class DrawBoxOption
    {
        public bool LockDoubleClick { get; set; } = false;
        public bool LockMouseWheel { get; set; } = false;
        public Color BackColor { get; set; }
    }

    public enum DrawBoxMode { Select, Zoom, Measure };
    public class DrawBox : UserControl
    {
        public PictureBox PictureBox { get; private set; }

        private Tracker tracker;

        public FigureAddDelegate FigureAdd;
        public FigureSelectedDelegate FigureSelected;
        public FigureSelectableDelegate FigureSelectable;
        public FigureMovedDelegate FigureMoved;
        public FigureCopyDelegate FigureCopy;
        public MouseClickedDelegate MouseClicked;
        public MouseDblClickedDelegate MouseDblClicked;
        public ImageUpdatedDelegate ImageUpdated;
        public PositionShiftedDelegate PositionShifted;

        public ImageProcessingDelegate ImageProcessing = null;

        private RectangleF displayRect = RectangleF.Empty;
        public RectangleF DisplayRect
        {
            set => displayRect = value;
        }
        public Calibration Calibration { get; set; }
        public Bitmap Image { get; private set; }

        private Image3D image3d;
        public Image3D Image3d
        {
            get => image3d;
            set
            {
                image3d = value;
                UpdateImage(value.ToBitmap());
            }
        }

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
        public SizeF PositionOffset { get; set; }
        public AutoFitStyle AutoFitStyle { get; set; }

        public float ZoomScale { get; set; } = -1;
        public PointF? ZoomCenter { get; set; }
        public FigureGroup FigureGroup { get; set; } = new FigureGroup();
        public FigureGroup BackgroundFigures { get; set; } = new FigureGroup();
        public FigureGroup TempFigureGroup { get; set; } = new FigureGroup();
        public FigureGroup CustomFigure { get; set; } = new FigureGroup();

        public FigureType TrackerShape
        {
            get => tracker.Shape;
            set => tracker.Shape = value;
        }
        public bool ShowCenterGuide { get; set; }
        public Point CenterGuidePos { get; set; }
        public int CenterGuideThickness { get; set; }

        private ToolTip toolTip = new ToolTip();
        public bool ShowTooltip { get; set; }

        public void SetMeasureScale(float scaleX, float scaleY)
        {
            throw new NotImplementedException();
        }

        private bool enable = false;
        public bool Enable
        {
            get => enable;
            set
            {
                enable = value;
                tracker.Enable = value;
            }
        }
        public bool HideLine { get; set; } = false;

        public bool RotationLocked
        {
            get => tracker.RotationLocked;
            set => tracker.RotationLocked = value;
        }

        public bool MoveLocked
        {
            get => tracker.MoveLocked;
            set => tracker.MoveLocked = value;
        }

        private bool measureMode;
        public bool MeasureMode
        {
            set
            {
                measureMode = value;
                tracker.Shape = FigureType.Line;
                tracker.MeasureMode = value;
            }
        }

        private LineFigure measureFigure = null;
        public float MeasureScaleX { get; set; } = 1;
        public float MeasureScaleY { get; set; } = 1;

        private bool appendMode;
        public bool ShiftPositionMode { get; set; }

        private bool breakLineMode;

        private bool addFigureMode;
        public bool AddFigureMode
        {
            get => addFigureMode;
            set
            {
                addFigureMode = value;
                tracker.AddFigureMode = value;
            }
        }

        private bool overlayMoveMode;
        public bool OverlayMoveMode
        {
            get => overlayMoveMode;
            set
            {
                overlayMoveMode = value;
                tracker.Shape = FigureType.Line;
            }
        }
        public Camera LinkedCamera { get; set; } = null;
        public bool LockLiveUpdate { get; set; } = false;
        public bool InvertY { get; set; } = false;
        public bool UseZoom { get; set; } = true;

        private bool onUpdate = false;
        public DrawBox()
        {
            InitializeComponent();
        }

        public DrawBox(Color backgroundColor)
        {
            InitializeComponent();

            PictureBox.BackColor = backgroundColor;
        }

        public DrawBox(DrawBoxOption drawBoxOption)
        {
            InitializeComponent();

            if (drawBoxOption.BackColor != null)
            {
                PictureBox.BackColor = drawBoxOption.BackColor;
            }

            if (drawBoxOption.LockDoubleClick == false)
            {
                PictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            }
        }

        public delegate void UpdateImageDelegate(Bitmap newImage);
        public void UpdateImage(Bitmap newImage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateImageDelegate(UpdateImage), newImage);
                return;
            }

            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }

            if (ImageProcessing != null)
            {
                Image = ImageProcessing(newImage);
            }
            else
            {
                Image = newImage;
            }

            PictureBox.Image = Image;
            ZoomCenter = null;
            if (ZoomScale == -1)
            {
                CalculateZoomScale();
            }
            else
            {
                UpdateZoom();
            }
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            //return base.ScrollToControl(activeControl);
            return DisplayRectangle.Location;
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox.Focus();
        }

        private void PictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        public void DisableMeasureMode()
        {
            throw new NotImplementedException();
        }

        public void SetCamera(Camera camera)
        {
            if (camera != null)
            {
                LinkedCamera = camera;

                if (LinkedCamera != null)
                {
                    LinkedCamera.ImageGrabbed += camera_ImageGrabbed;
                }
            }
            else
            {
                if (LinkedCamera != null)
                {
                    LinkedCamera.ImageGrabbed -= camera_ImageGrabbed;
                }

                LinkedCamera = null;
            }
        }

        public void camera_ImageGrabbed(Camera camera)
        {
            if (LockLiveUpdate == true)
            {
                return;
            }

            if (Visible == false)
            {
                return;
            }

            if (InvokeRequired)
            {
                Thread.Sleep(10);

                LogHelper.Debug(LoggerType.Grab, "Start Invoke ImageGrabbed");
                BeginInvoke(new CameraEventDelegate(camera_ImageGrabbed), camera);
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Start UpdateImage : Camera - {0}", camera.Index));

            var imageD = (Image2D)camera.GetGrabbedImage();

            if (imageD.ImageData != null)
            {
                UpdateImage(imageD.ToBitmap());

                //ImageHelper.SaveImage(Image, @"d:\Test.bmp");

                ImageUpdated?.Invoke();

                Invalidate(true);
                //Update();
            }
            LogHelper.Debug(LoggerType.Grab, string.Format("End UpdateImage : Camera - {0}", camera.Index));
        }

        //private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        // {
        //    StepZoom((((float)e.Delta) / displayRect.Width));
        //}

        private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TempFigureGroup.Clear();

            MouseDblClicked?.Invoke(this);

            Invalidate(true);
        }

        public void AutoFit(bool onOff)
        {
            if (onOff == true)
            {
                PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
                //                ZoomFit();
            }
            else
            {
                PictureBox.Dock = System.Windows.Forms.DockStyle.None;
            }
        }

        public void Offset(float offsetX, float offsetY)
        {
            tracker.Offset((int)offsetX, (int)offsetY);
            Invalidate(true);
        }

        private void Tracker_PositionShifted(System.Drawing.SizeF offset)
        {
            if (ShiftPositionMode == true)
            {
                CoordTransformer coordTransformer = GetCoordTransformer();
                if (coordTransformer == null)
                {
                    return;
                }

                SizeF offsetF = coordTransformer.InverseTransform(offset);

                PositionShifted?.Invoke(offsetF);
            }
            else if (overlayMoveMode == true)
            {
                OverlayPos -= Size.Round(offset);
                Invalidate();
            }
            else
            {
                //zoomOffset += Size.Round(offset);
                UpdateZoom();
            }
        }

        public void SetTrackerShape(FigureType shape, int numGridColumn = 0, int numGridRow = 0)
        {
            tracker.Shape = shape;
            tracker.NumGridColumn = numGridColumn;
            tracker.NumGridRow = numGridRow;
        }

        public void ClearFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ClearFigure");
            Invalidate();
            PictureBox.Controls.Clear();
            tracker.ClearFigure();
        }

        public void ClearFigure2()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ClearFigure");
            Invalidate();
        }

        public void SelectFigure(Figure figure)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void SelectFigureByTag(object tag)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.SelectFigureByTag");
            tracker.AddFigure(FigureGroup.GetFigureByTag(tag));
        }

        public void RemoveSelectedFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.MouseMove");

            foreach (Figure selectedFigure in tracker)
            {
                FigureGroup.RemoveFigure(selectedFigure);
            }

            tracker.ClearFigure();
            Invalidate(true);
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            //LogHelper.Debug(LoggerType.OpDebug, "pictureBox_MouseDown");

            if (enable == false)
            {
                return;
            }

            //if (e.Button != System.Windows.Forms.MouseButtons.Left)
            //    return;

            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
            {
                return;
            }

            breakLineMode = (Control.ModifierKeys == Keys.Alt);
            if (breakLineMode == false)
            {
                appendMode = (Control.ModifierKeys == Keys.Control);
            }

            tracker.CoordTransformer = coordTransformer;
            tracker.ShiftPositionMode = (Control.ModifierKeys == Keys.Shift) || ShiftPositionMode || overlayMoveMode;

            tracker.MouseDown(e);

        }

        public void ResetSelection()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ResetSelection");

            tracker.ClearFigure();

            FigureSelected?.Invoke(null);
        }

        private void Tracker_SelectionPointCaptured(Point point)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_SelectionPointCaptured");

            if (appendMode == false)
            {
                ResetSelection();
            }

            Figure figure = FigureGroup.Select(point);
            if (figure != null)
            {
                bool isSelected = tracker.IsSelected(figure);
                if (isSelected)
                {
                    tracker.RemoveFigure(figure);
                    //Figure nextFigure = figureGroup.Select(point, figure);
                    //if (nextFigure != null)
                    //    figure = nextFigure;
                }
                else if (FigureSelectable == null || FigureSelectable(figure))
                {
                    tracker.AddFigure(figure);


                }
                FigureSelected?.Invoke(new List<Figure>() { figure }, !isSelected);
            }

            Invalidate(true);
        }

        private void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_SelectionRectCaptured");

            if (appendMode == false)
            {
                ResetSelection();
            }

            List<Figure> figureList = FigureGroup.Select(rectangle);

            if (FigureSelected != null)
            {
                FigureSelected(figureList);
                foreach (Figure figure in figureList)
                {
                    tracker.AddFigure(figure);
                }
            }

            //List<Figure> targetFigureList = GetFigureList(figureList, true);
            //List<Figure> probeFigureList = GetFigureList(figureList, false);
            //if (figureList.Count() > 0)
            //{
            //    if(targetFigureList.Count > 1)
            //        tracker.AddFigure(targetFigureList);
            //    else
            //        tracker.AddFigure(probeFigureList);
            //    if (FigureSelected != null)
            //    {
            //        foreach (Figure figure in figureList)
            //        {
            //             FigureSelected(figure);                           
            //        }

            //    }
            //}


            Invalidate(true);
        }

        private void Tracker_AddFigureCaptured(List<PointF> pointList)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_AddFigureCaptured");

            if (addFigureMode == true)
            {
                ResetSelection();
                FigureAdd?.Invoke(pointList, TrackerShape);
            }

            Invalidate(true);
        }

        private List<Figure> GetFigureList(List<Figure> figureList, bool TargetFlag)
        {
            var returnFigureList = new List<Figure>();
            foreach (Figure figure in figureList)
            {
                if (TargetFlag)
                {
                    if (figure.Tag.GetType().Name == "Target")
                    {
                        returnFigureList.Add(figure);
                    }
                }
                else
                {
                    if (figure.Tag.GetType().Name == "VisionProbe")
                    {
                        returnFigureList.Add(figure);
                    }
                }
            }

            return returnFigureList;

        }

        private void Tracker_FigureMoved()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_FigureMoved");

            if (appendMode == true)
            {
                FigureCopy?.Invoke(tracker.GetFigureList());
            }
            else
            {
                FigureMoved?.Invoke(tracker.GetFigureList());
            }

            PictureBox.Invalidate();
            Invalidate(true);
        }

        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
            {
                return;
            }

            //if (e.Button != System.Windows.Forms.MouseButtons.Left)
            //    return;

            bool processingCancelled = false;
            if (e.Button == MouseButtons.Left && MouseClicked != null)
            {
                MouseClicked(this, coordTransformer.InverseTransform(e.Location), e.Button, ref processingCancelled);
            }

            if (processingCancelled)
            {
                return;
            }

            //LogHelper.Debug(LoggerType.OpDebug, "DrawBox.OnMouseUp");

            tracker.MouseUp(e);

            var measureSize = new SizeF(Math.Abs(tracker.EndTrackPos.X - tracker.StartTrackPos.X), Math.Abs(tracker.EndTrackPos.Y - tracker.StartTrackPos.Y));

            if (measureMode)
            {
                if (measureSize != new SizeF(0, 0))
                {
                    var pen = new Pen(Color.Red, 1);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    measureFigure = new LineFigure(coordTransformer.InverseTransform(tracker.StartTrackPos), coordTransformer.InverseTransform(tracker.EndTrackPos), pen);
                    TempFigureGroup.AddFigure(measureFigure);

                    measureSize = coordTransformer.InverseTransform(measureSize);

                    measureSize = new SizeF(measureSize.Width * MeasureScaleX, measureSize.Height * MeasureScaleY);
                    float length = MathHelper.GetLength(measureFigure.StartPoint, measureFigure.EndPoint) * MeasureScaleX;

                    SizeF fontSize = coordTransformer.InverseTransform(new SizeF(10, 0));

                    string text = string.Format("{0} (W{1}, H{2})", length, measureSize.Width, measureSize.Height);
                    PointF centerPos = coordTransformer.InverseTransform(DrawingHelper.CenterPoint(tracker.StartTrackPos, tracker.EndTrackPos));
                    var textFigure = new TextFigure(text, Point.Round(centerPos), new Font("Arial", fontSize.Width), Color.Red);

                    TempFigureGroup.AddFigure(textFigure);
                }
            }

            //base.OnMouseUp(e);
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Image == null && displayRect.IsEmpty)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                if (ShowTooltip == true)
                {
                    CoordTransformer coordTransformer = GetCoordTransformer();
                    if (coordTransformer == null)
                    {
                        return;
                    }

                    Point searchPt = coordTransformer.InverseTransform(new Point(e.X, e.Y));

                    Figure figure = TempFigureGroup.Select(searchPt);
                    if (figure != null && figure.Tag != null)
                    {
                        toolTip.Show(figure.Tag.ToString(), PictureBox, e.X, e.Y, 1000);
                    }
                }
            }
            else
            {
                LogHelper.Debug(LoggerType.Operation, "DrawBox.OnMouseMove");

                tracker.MouseMove(e);
            }

            //base.OnMouseMove(e);
        }

        private RectangleF GetViewPort()
        {
            CoordTransformer transformer = GetCoordTransformer();
            var viewPort = new Rectangle(new Point(0, 0), ClientRectangle.Size);
            viewPort.Offset(-DisplayRectangle.Location.X, -DisplayRectangle.Location.Y);
            viewPort = transformer.InverseTransform(viewPort);
            return viewPort;
        }

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    Focus();
        //}

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{
        //    base.OnMouseWheel(e);

        //    StepZoom((((float)e.Delta) / displayRect.Width));
        //}

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    base.OnMouseClick(e);
        //    if(e.Button == MouseButtons.Middle)
        //    {

        //    }
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //    tempFigureGroup.Clear();

        //    if (AddRegionCaptured != null)
        //        AddRegionCaptured(new Rectangle(), new Point(), new Point());

        //    base.OnMouseDoubleClick(e);

        //    Invalidate(true);
        //}

        public void StepZoom(float value)
        {
            if (ZoomScale == -1)
            {
                CalculateZoomScale();
            }

            UpdateZoom(ZoomScale, ZoomScale + value);
        }

        public void ZoomIn()
        {
            if (ZoomCenter == null)
            {
                ZoomCenter = DrawingHelper.CenterPoint(GetViewPort());
            }

            StepZoom(0.005f);
        }

        public void ZoomOut()
        {
            StepZoom(-0.005f);
        }

        public delegate void ZoomFitDelegate();
        public void ZoomFit()
        {
            if (InvokeRequired)
            {
                Invoke(new ZoomFitDelegate(ZoomFit));
                return;
            }

            CalculateZoomScale();
            AutoScroll = false;
        }

        public void Zoom(float newZoomScale)
        {
            UpdateZoom(ZoomScale, newZoomScale);
        }

        public void Zoom(RectangleF zoomRect)
        {
            if (zoomRect.Size.IsEmpty)
            {
                ZoomFit();
                return;
            }

            float zoomScaleW = ClientRectangle.Width * 0.8f / zoomRect.Width;
            float zoomScaleH = ClientRectangle.Height * 0.8f / zoomRect.Height;
            float newZoomScale = Math.Min(zoomScaleH, zoomScaleW);

            if (newZoomScale > 10)
            {
                newZoomScale = 10;
            }

            ZoomCenter = DrawingHelper.CenterPoint(zoomRect);
            Zoom(newZoomScale);
        }

        private void UpdateZoom()
        {
            UpdateZoom(ZoomScale, ZoomScale);
            //if (image != null)
            //{
            //    if (displayRect.IsEmpty == false)
            //    {
            //        pictureBox.Width = (int)(displayRect.Width * zoomScale);
            //        pictureBox.Height = (int)(displayRect.Height * zoomScale);
            //    }
            //    else if (image != null)
            //    {
            //        pictureBox.Width = (int)(image.Width * zoomScale) ;
            //        pictureBox.Height = (int)(image.Height * zoomScale);
            //    }
            //}
        }

        public delegate void UpdateZoomDelegate(float preZoomScale, float newZoomScale);
        private void UpdateZoom(float preZoomScale, float newZoomScale)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateZoomDelegate(UpdateZoom), preZoomScale, newZoomScale);
                return;
            }

            onUpdate = true;

            PictureBox.SuspendLayout();

            //zoomCenter = new PointF(this.pictureBox.Image.Width / 2, this.pictureBox.Image.Height / 2);
            if (displayRect.IsEmpty == false)
            {
                AutoScroll = true;
                int picBoxW = (int)(displayRect.Width * newZoomScale);
                int picBoxH = (int)(displayRect.Height * newZoomScale);
                PictureBox.Width = picBoxW;
                PictureBox.Height = picBoxH;
            }

            //Debug.WriteLine(string.Format("UpdateZoom: scale {0}", newZoomScale));
            if (ZoomCenter != null)
            {
                //Debug.WriteLine(string.Format("UpdateZoom: CenterPoint {0}", zoomCenter));
                if (PictureBox.Width > Width)
                {
                    int scrollMax = HorizontalScroll.Maximum - Width + 20;
                    if (scrollMax >= 0)
                    {
                        int viewWidth = (int)(ClientRectangle.Width / newZoomScale);
                        int boundMin = (int)(displayRect.Left + viewWidth / 2);
                        int boundMax = (int)(displayRect.Right - viewWidth / 2);
                        int scrollPos;
                        float scrollPosP;

                        if (ZoomCenter.Value.X < boundMin)
                        {
                            scrollPosP = 0;
                            scrollPos = 0;
                        }
                        else if (ZoomCenter.Value.X > boundMax)
                        {
                            scrollPosP = 1;
                            scrollPos = scrollMax;
                        }
                        else
                        {
                            scrollPosP = (ZoomCenter.Value.X - boundMin) * 1.0f / (boundMax - boundMin);
                            scrollPos = (int)(scrollPosP * scrollMax);
                        }

                        scrollPos = (int)(scrollPosP * scrollMax);
                        if (scrollPos < 0)
                        {
                            scrollPos = 0;
                        }

                        if (scrollPos > scrollMax)
                        {
                            scrollPos = scrollMax;
                        }

                        HorizontalScroll.Value = scrollPos;
                        //Debug.WriteLine(string.Format("HorizontalScroll: {0} / {1} ({2})", scrollPos, scrollMax, scrollPosP));
                    }
                }

                if (PictureBox.Height > Height)
                {
                    int scrollMax = VerticalScroll.Maximum - Height + 20;
                    if (scrollMax >= 0)
                    {
                        int viewHeight = (int)(ClientRectangle.Height / newZoomScale);
                        int boundMin = (int)(displayRect.Top + viewHeight / 2);
                        int boundMax = (int)(displayRect.Bottom - viewHeight / 2);
                        int scrollPos;
                        float scrollPosP;
                        if (ZoomCenter.Value.Y < boundMin)
                        {
                            scrollPosP = 0;
                            scrollPos = 0;
                        }
                        else if (ZoomCenter.Value.Y > boundMax)
                        {
                            scrollPosP = 1;
                            scrollPos = scrollMax;
                        }
                        else
                        {
                            scrollPosP = (ZoomCenter.Value.Y - boundMin) * 1.0f / (boundMax - boundMin);
                            scrollPos = (int)(scrollPosP * scrollMax);
                        }


                        if (InvertY)
                        {
                            scrollPosP = 1 - scrollPosP;
                        }

                        scrollPos = (int)(scrollPosP * scrollMax);


                        if (scrollPos < 0)
                        {
                            scrollPos = 0;
                        }

                        if (scrollPos > scrollMax)
                        {
                            scrollPos = scrollMax;
                        }

                        VerticalScroll.Value = scrollPos;
                        //Debug.WriteLine(string.Format("VerticalScroll: {0} / {1} ({2})", scrollPos, scrollMax ,scrollPosP));
                    }
                }
            }

            PictureBox.ResumeLayout();
            ZoomScale = newZoomScale;

            //Debug.WriteLine("UpdateZoom - Start");
            //Debug.WriteLine(string.Format("Scale:{0}, zoomCenter: {1}", zoomScale, zoomCenter?.ToString()));
            //Debug.WriteLine(string.Format("DisplayRect: {0}, ClientRect: {1}, DispRect: {2}", this.DisplayRectangle.ToString(), this.ClientRectangle.ToString(), this.displayRect.ToString()));
            //Debug.WriteLine("UpdateZoom - End");

            onUpdate = false;
        }

        private void CalculateZoomScale()
        {
            float newZoomScale = 1.0f;

            if (displayRect.IsEmpty)
            {
                if (Image == null)
                {
                    return;
                }

                displayRect = new RectangleF(0, 0, Image.Width, Image.Height);
            }

            float zoomScaleWidth = Width / ((float)displayRect.Width);
            float zoomScaleHeight = Height / ((float)displayRect.Height);

            newZoomScale = Math.Min(zoomScaleWidth, zoomScaleHeight);


            //if (displayRect.IsEmpty == false)
            //{
            //    float zoomScaleWidth = Width / ((float)displayRect.Width);
            //    float zoomScaleHeight = Height / ((float)displayRect.Height);

            //    newZoomScale = Math.Min(zoomScaleWidth, zoomScaleHeight);
            //}
            //else if (image != null)
            //{
            //    float zoomScaleWidth = Width / ((float)image.Width);
            //    float zoomScaleHeight = Height / ((float)image.Height);

            //    newZoomScale = Math.Min(zoomScaleWidth, zoomScaleHeight);
            //}

            UpdateZoom(ZoomScale, newZoomScale);
        }

        public CoordTransformer GetCoordTransformer()
        {
            if (displayRect.IsEmpty && Image == null)
            {
                return null;
            }

            var coordTransformer = new CoordTransformer();

            if (displayRect.IsEmpty == false)
            {
                coordTransformer.SetSrcRect(displayRect);
                coordTransformer.SetDisplayRect(new RectangleF(0, 0, PictureBox.Width, PictureBox.Height));
            }
            else if (Image != null)
            {
                coordTransformer.SetSrcRect(new RectangleF(0, 0, Image.Width, Image.Height));
                coordTransformer.SetDisplayRect(new RectangleF(0, 0, PictureBox.Width, PictureBox.Height));
            }
            coordTransformer.InvertY = InvertY;

            return coordTransformer;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (onUpdate)
            {
                return;
            }

            PictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Call base class, invoke Paint handlers
            base.OnPaint(e);


            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
            {
                return;
            }

            if (ShowCenterGuide)
            {
                var pen = new Pen(Color.Blue, CenterGuideThickness);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                e.Graphics.DrawLine(pen, new PointF(0, PictureBox.Height / 2 + CenterGuidePos.X), new PointF(PictureBox.Width, PictureBox.Height / 2 + CenterGuidePos.X));
                e.Graphics.DrawLine(pen, new PointF(PictureBox.Width / 2 + CenterGuidePos.Y, 0), new PointF(PictureBox.Width / 2 + CenterGuidePos.Y, PictureBox.Height));
            }

            if (FigureGroup != null)
            {
                if (HideLine == false)
                {
                    FigureGroup.Draw(e.Graphics, coordTransformer, false);
                    TempFigureGroup.Draw(e.Graphics, coordTransformer, false);
                    BackgroundFigures.Draw(e.Graphics, coordTransformer, false);
                    CustomFigure.Draw(e.Graphics, coordTransformer, false);
                }
            }

            if (overlayImage != null)
            {
                var colorMatrix = new ColorMatrix();
                var ia = new ImageAttributes();

                var imgRect = new Rectangle(OverlayPos.X, OverlayPos.Y, PictureBox.Width, PictureBox.Height);
                colorMatrix.Matrix33 = 0.50f;
                ia.SetColorMatrix(colorMatrix);

                //                e.Graphics.DrawImage(overlayImage, imgRect, 0,0, );
                e.Graphics.DrawImage(overlayImage, imgRect, 0, 0, overlayImage.Width, overlayImage.Height, GraphicsUnit.Pixel, ia);
            }

            if (tracker != null)
            {
                tracker.Draw(e.Graphics, coordTransformer);
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // 
            // PictureBox
            // 
            PictureBox = new PictureBox();
            PictureBox.Dock = System.Windows.Forms.DockStyle.None;
            PictureBox.Name = "pictureBox";
            PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            PictureBox.TabIndex = 8;
            PictureBox.TabStop = false;
            PictureBox.MouseDown += pictureBox_MouseDown;
            PictureBox.MouseUp += pictureBox_MouseUp;
            PictureBox.MouseMove += pictureBox_MouseMove;
            PictureBox.Paint += pictureBox_Paint;
            PictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            PictureBox.MouseEnter += PictureBox_MouseEnter;

            // 
            // DrawBox
            // 
            Name = "DrawBox";
            BackColor = Color.LightGray;
            KeyUp += new System.Windows.Forms.KeyEventHandler(DrawBox_KeyUp);
            Validated += new System.EventHandler(DrawBox_Validated);
            VisibleChanged += new EventHandler(DrawBox_VisibleChanged);
            MouseWheel += DrawBox_MouseWheel;
            Scroll += DrawBox_Scroll;
            MouseDown += DrawBox_MouseDown;
            MouseUp += DrawBox_MouseUp;
            MouseMove += DrawBox_MouseMove;

            Controls.Add(PictureBox);

            // 
            // Tracker
            // 
            tracker = new Tracker(PictureBox);
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = new AddFigureCapturedDelegate(Tracker_AddFigureCaptured);
            tracker.PositionShifted = new PositionShiftedDelegate(Tracker_PositionShifted);
            tracker.SelectionRectZoom = new SelectionRectZoomDelegate(Tracker_SelectionRectZoom);
            ResumeLayout(false);

        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void Tracker_SelectionRectZoom(RectangleF rectangle)
        {
            Zoom(rectangle);
            //if(rectangle.Size.IsEmpty)
            //{
            //    ZoomFit();
            //    return;
            //}

            //float zoomScaleW = this.ClientRectangle.Width * 1.0f / rectangle.Width;
            //float zoomScaleH = this.ClientRectangle.Height * 1.0f / rectangle.Height;
            //float newZoomScale = Math.Min(zoomScaleH, zoomScaleW);

            //this.zoomCenter = DrawingHelper.CenterPoint(rectangle);
            //Zoom(newZoomScale);
        }

        private void DrawBox_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollProperties sp;
            int size;
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                sp = HorizontalScroll;
                size = Width;
            }
            else
            {
                sp = VerticalScroll;
                size = Height;
            }

            ZoomCenter = null;
            //Debug.WriteLine(string.Format("{0}: {1} / {2}", e.ScrollOrientation, e.NewValue.ToString(), (e.NewValue / (sp.Maximum - 20.0f - size)).ToString()));

        }

        private void DrawBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                return;
            }
            Point? mousePos = null;
            Point mousePosScreen = PointToScreen(e.Location);
            Rectangle imageRectScreen = PictureBox.RectangleToScreen(PictureBox.ClientRectangle);

            if (imageRectScreen.Contains(mousePosScreen))
            {
                mousePos = PictureBox.PointToClient(mousePosScreen);
            }

            ZoomCenter = GetCoordTransformer().InverseTransform(e.Location);
            if (e.Delta > 0)
            {
                //zoomScale += 0.01f;
                UpdateZoom(ZoomScale, ZoomScale + 0.01f);
            }
            else
            {
                //zoomScale -= 0.01f;
                UpdateZoom(ZoomScale, ZoomScale - 0.01f);
            }
        }

        private void DrawBox_VisibleChanged(object sender, EventArgs e)
        {
            CalculateZoomScale();
            //UpdateZoom();
        }

        private void DrawBox_Validated(object sender, EventArgs e)
        {
            PictureBox.Invalidate();
        }

        public void SaveImage(string fileName)
        {
            var bmp = new Bitmap(PictureBox.Width, PictureBox.Height);
            PictureBox.DrawToBitmap(bmp, PictureBox.Bounds);

            ImageHelper.SaveImage(bmp, fileName);
        }

        private void DrawBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DrawBox.OnKeyUp");

            var offset = new Size(0, 0);
            if (e.KeyCode == Keys.Down)
            {
                offset.Height = 1;
            }
            else if (e.KeyCode == Keys.Up)
            {
                offset.Height = -1;
            }
            else if (e.KeyCode == Keys.Left)
            {
                offset.Width = -1;
            }
            else if (e.KeyCode == Keys.Right)
            {
                offset.Width = 1;
            }

            tracker.Move(offset);

            if (e.KeyData == Keys.Z)
            {
                UpdateZoom(ZoomScale, ZoomScale + 0.01f);
            }

            if (e.KeyData == Keys.X)
            {
                UpdateZoom(ZoomScale, ZoomScale - 0.01f);
            }
        }

        public delegate void InvalidateDelegate();
        public new void Invalidate()
        {
            if (InvokeRequired)
            {
                Invoke(new InvalidateDelegate(Invalidate));
                return;
            }

            Invalidate(true);
        }
    }
}
