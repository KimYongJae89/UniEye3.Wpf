using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class SchemaViewer : UserControl, IInspectionResultVisualizer
    {
        public AddRegionCapturedDelegate AddRegionCaptured;
        public FigureSelectedDelegate FigureSelected;
        public FigureMovedDelegate FigureMoved;
        public FigureCopyDelegate FigureCopy;
        private Brush okBrush = new SolidBrush(Color.LimeGreen);
        private Brush ngBrush = new SolidBrush(Color.Red);

        private Schema schema = new Schema();
        public Schema Schema
        {
            get => schema;
            set
            {
                schema = value;
                viewCenter = null;
                viewScale = 1.0f;
                InitShow();
                Invalidate();
            }
        }

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
        public bool Editable { get; set; } = false;

        private bool showResultValue = true;
        public bool ShowResultValue
        {
            set => showResultValue = value;
        }


        private bool addFigureMode;
        public bool AddFigureMode
        {
            set
            {
                addFigureMode = value;
                tracker.AddFigureMode = value;
            }
        }

        private float viewScale = 1;
        private PointF? viewCenter = null;

        public bool TrackerRotationLocked
        {
            set => tracker.RotationLocked = value;
        }

        public SchemaViewer()
        {
            InitializeComponent();
            tracker = new Tracker(this);
            tracker.Enable = true;
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = Tracker_AddFigureCaptured;
            tracker.SelectionRectZoom = Tracker_SelectionRectZoom;

            toolStripButtonShowPad.Checked = true;

        }

        private void Tracker_SelectionRectZoom(RectangleF rectangle)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0)
            {
                viewScale = 1;
                viewCenter = null;
                Invalidate();
                return;
            }

            float zoomW = (schema.Region.Width * schema.ViewScale) / rectangle.Width;
            float zoomH = (schema.Region.Height * schema.ViewScale) / rectangle.Height;
            viewScale = Math.Min(zoomW, zoomH);
            if (viewScale > 100)
            {
                viewScale = 100;
            }

            viewCenter = DrawingHelper.CenterPoint(rectangle);
            Invalidate();
        }

        private void Tracker_AddFigureCaptured(List<PointF> pointList)
        {
            ResetSelection();
            //            AddRegionCaptured(rectangle, startPos, endPos);
        }

        public void AddFigure(Figure figure)
        {
            schema.AddFigure(figure);
            Invalidate();
        }

        public void DeleteAll()
        {
            foreach (Figure figure in tracker)
            {
                schema.RemoveFigure(figure);
            }

            tracker.ClearFigure();

            Invalidate();
        }

        public void Delete(Figure figure)
        {
            schema.RemoveFigure(figure);
            Invalidate();
        }

        public void ResetSelection()
        {
            LogHelper.Debug(LoggerType.Operation, "SchemaViewer.ResetSelection");

            tracker.ClearFigure();

            FigureSelected?.Invoke(null);
        }

        public void SelectFigure(Figure figure)
        {
            //Debug.WriteLine("SchemaViewer.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void LinkSchemaFigures(StepModel currentModel)
        {
            if (schema != null)
            {
                currentModel.LinkSchemaFigures(schema);
            }
        }

        public void SelectFigureByTag(List<object> tagList)
        {
            foreach (object tag in tagList)
            {
                tracker.AddFigure(schema.FigureGroup.GetFigureByTag(tag));
            }
        }

        public void SelectFigure(List<Figure> figureList)
        {
            tracker.AddFigure(figureList);
            Invalidate();
        }

        public void SelectFigureByCrosshair(List<Figure> figureList)
        {
            var unionRect = new RotatedRect();
            bool firstFigure = true;
            foreach (Figure figure in figureList)
            {
                if (firstFigure == true)
                {
                    unionRect = figure.GetRectangle();
                    firstFigure = false;
                }
                else
                {
                    unionRect = RotatedRect.Union(unionRect, figure.GetRectangle());
                }
            }

            PointF centerPt = DrawingHelper.CenterPoint(unionRect);

            var horzStartPt = new PointF(schema.Region.Left, centerPt.Y);
            var horzEndPt = new PointF(schema.Region.Right, centerPt.Y);
            var vertStartPt = new PointF(centerPt.X, schema.Region.Top);
            var vertEndPt = new PointF(centerPt.X, schema.Region.Bottom);

            var crosshairFigureList = new FigureGroup();

            crosshairFigureList.AddFigure(new LineFigure(horzStartPt, horzEndPt, new Pen(Color.Red)));
            crosshairFigureList.AddFigure(new LineFigure(vertStartPt, vertEndPt, new Pen(Color.Red)));

            schema.TempFigureGroup = crosshairFigureList;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (enable == false)
            {
                return;
            }

            //if (e.Button != System.Windows.Forms.MouseButtons.Left)
            //    return;

            //Debug.WriteLine("SchemaViewer.OnMouseDown");

            appendMode = (Control.ModifierKeys == Keys.Control);

            tracker.CoordTransformer = GetCoordTransformer();

            tracker.MouseDown(e);

            base.OnMouseDown(e);
        }

        private void Tracker_SelectionPointCaptured(Point point)
        {
            LogHelper.Debug(LoggerType.Operation, "SchemaViewer.Tracker_SelectionPointCaptured");

            if (appendMode == false)
            {
                ResetSelection();
            }

            Figure figure = schema.FigureGroup.Select(point);
            if (figure != null)
            {
                tracker.AddFigure(figure);

                if (FigureSelected != null)
                {
                    var figureList = new List<Figure>();
                    FigureSelected(figureList);
                }
            }

            Invalidate();
        }

        private void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            LogHelper.Debug(LoggerType.Operation, "SchemaViewer.Tracker_SelectionRectCaptured");

            if (addFigureMode == false)
            {
                if (appendMode == false)
                {
                    ResetSelection();
                }

                //List<Figure> figureList = schema.FigureGroup.Select(rectangle);
                //if (figureList.Count() > 0)
                //{
                //    tracker.AddFigure(figureList);

                //    if (FigureSelected != null)
                //    {
                //        foreach (Figure figure in figureList)
                //            FigureSelected(figure);
                //    }
                //}
            }

            Invalidate();
        }

        private void Tracker_FigureMoved()
        {
            //Debug.WriteLine("SchemaViewer.Tracker_FigureMoved");

            if (appendMode == true)
            {
                FigureCopy?.Invoke(tracker.GetFigureList());
            }
            else
            {
                FigureMoved?.Invoke(tracker.GetFigureList());
            }

            Invalidate();
        }

        public void UpdateResult(ProductResult productResult)
        {
            foreach (Figure figure in schema.FigureGroup.FigureList)
            {
                string tag = figure.Name as string;
                if (string.IsNullOrEmpty(tag))
                {
                    continue;
                }

                if (productResult.GetResult(tag, out object value))
                {
                    if ((bool)value == true)
                    {
                        figure.TempBrush = (Brush)okBrush.Clone();
                    }
                    else
                    {
                        figure.TempBrush = ngBrush;
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //if (e.Button != System.Windows.Forms.MouseButtons.Left)
            //    return;

            //Debug.WriteLine("SchemaViewer.OnMouseUp");

            tracker.MouseUp(e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                return;
            }

            //Debug.WriteLine("SchemaViewer.OnMouseMove");

            tracker.MouseMove(e);

            base.OnMouseMove(e);
        }

        private RectangleF GetViewport()
        {
            float w2 = schema.Region.Width / viewScale / 2;
            float h2 = schema.Region.Height / viewScale / 2;
            var viewPort = RectangleF.FromLTRB(-w2, -h2, w2, h2);
            if (viewCenter == null)
            {
                viewCenter = DrawingHelper.CenterPoint(schema.Region);
            }

            viewPort.Offset(viewCenter.Value);
            return viewPort;
        }

        private CoordTransformer GetCoordTransformer()
        {
            var coordTransformer = new CoordTransformer(schema.ViewScale);
            //            if (schema.AutoFit)
            {
                coordTransformer.InvertY = schema.InvertY;
                coordTransformer.SetSrcRect(GetViewport());
                coordTransformer.SetDisplayRect(new RectangleF(0, 0, Width - toolStrip.Width, Height));
            }

            return coordTransformer;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin OnPaint");

            base.OnPaint(e);

            if (schema != null)
            {
                //Graphics ee = panelView.CreateGraphics();
                CoordTransformer coordTransformer = GetCoordTransformer();

                schema.Draw(e.Graphics, coordTransformer, enable);

                //if (toolStripButtonShowFov.Checked)
                //    schema.Draw(e.Graphics, coordTransformer, enable, "Step");

                //if (toolStripButtonShowPad.Checked)
                //    schema.Draw(e.Graphics, coordTransformer, enable, "Target");

                //if (toolStripButtonShowPath.Checked)
                //    schema.Draw(e.Graphics, coordTransformer, enable, "Path");

                tracker.Draw(e.Graphics, coordTransformer);

                //ee.Dispose();
            }

            //LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End OnPaint");
        }

        private void MenuMoveUp_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                schema.MoveUp(figure);
            }

            Invalidate();
        }

        private void MenuMoveTop_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                schema.MoveTop(figure);
            }

            Invalidate();
        }

        private void SchemaViewer_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Right && enable == true)
            //{
            //    Figure figure = tracker.GetFirstFigure();
            //    if (figure != null)
            //    {
            //        visibleToolStripMenuItem.Checked = figure.Visible;
            //    }

            //    contextMenu.Show(this, e.Location);
            //}
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                if (figure.Deletable == true)
                {
                    schema.RemoveFigure(figure);
                }
            }

            tracker.ClearFigure();

            Invalidate();
        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                schema.MoveDown(figure);
            }

            Invalidate();
        }

        private void MoveBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                schema.MoveBottom(figure);
            }

            Invalidate();
        }

        private void PropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tracker.IsSingleSelected() == true)
            {
                var figurePropertyForm = new FigurePropertyForm();
                figurePropertyForm.Figure = tracker.GetFirstFigure();
                figurePropertyForm.ShowDialog(this);
                Invalidate();
            }
        }

        private void visibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                figure.Visible = !figure.Visible;
            }

            tracker.ClearFigure();

            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            AddRegionCaptured?.Invoke(new Rectangle(), new Point(), new Point());

            base.OnMouseDoubleClick(e);
        }

        public void Copy()
        {
            var objList = new List<ICloneable>();
            foreach (Figure figure in tracker)
            {
                var copyFigure = (Figure)figure.Clone();

                objList.Add(copyFigure);

                figure.Offset(10, 10);
            }

            CopyBuffer.SetData(objList);
        }

        public void Paste()
        {
            List<ICloneable> objList = CopyBuffer.GetData();
            if (objList.Count == 0)
            {
                return;
            }

            foreach (ICloneable obj in objList)
            {
                try
                {
                    if (obj is Figure srcFigure)
                    {
                        var figure = (Figure)srcFigure.Clone();

                        AddFigure(figure);

                        srcFigure.Offset(10, 10);
                    }
                }
                catch (InvalidCastException)
                {

                }
            }

            Invalidate();
        }

        public void Update(StepModel stepModel)
        {
            if (stepModel != null)
            {
                schema = stepModel.ModelSchema;
            }
            else
            {
                schema = new Schema();
            }

            Invalidate();
        }

        private void UpdateJudgmentFigure(List<Figure> figureList, string figureId, bool result)
        {
            foreach (Figure figure in figureList)
            {
                UpdateJudgmentFigure(figure as FigureGroup, figureId, result);
            }
        }

        private void UpdateValueFigure(List<Figure> figureList, string figureId, string valueStr)
        {
            foreach (Figure figure in figureList)
            {
                UpdateValueFigure(figure as FigureGroup, figureId, valueStr);
            }
        }

        private void UpdateTargetResult(Target target, ProductResult targetResult)
        {
            //LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin UpdateTargetResult");

            List<Figure> targetFigureList = target.SchemaFigures;
            if (targetFigureList == null)
            {
                return;
            }

            UpdateJudgmentFigure(targetFigureList, target.FullId, targetResult.IsGood());

            foreach (ProbeResult probeResult in targetResult)
            {
                Probe probe = probeResult.Probe;

                List<Figure> probeFigureList = probe.SchemaFigures;

                UpdateJudgmentFigure(probeFigureList, probe.FullId, probeResult.IsGood());

                if (showResultValue)
                {
                    foreach (ResultValue probeResultValue in probeResult)
                    {
                        string probeResultId = probe.FullId + "." + probeResultValue.Name;

                        List<Figure> probeResultFigureList = schema.GetFigureByTag(probeResultId);
                        if (probeResultValue.Value is string)
                        {
                            UpdateValueFigure(probeResultFigureList, probeResultId, probeResultValue.Value.ToString());
                        }
                        else
                        {
                            try
                            {
                                float value = Convert.ToSingle(probeResultValue.Value);
                                UpdateValueFigure(probeResultFigureList, probeResultId, value.ToString("0.00"));
                            }
                            catch (InvalidCastException)
                            {

                            }
                        }
                    }
                }
            }
        }

        public void UpdateResult(Target target, ProductResult targetResult)
        {
            //LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin UpdateResult Target");

            UpdateTargetResult(target, targetResult);

            Invalidate();

            //LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End UpdateResult Target");
        }

        private int GetFigureTotalCount(ProductResult productResult)
        {
            int count = 0;

            if (count <= 0)
            {
                return 0;
            }
            else
            {
                var resultCount = new ResultCount();
                productResult.GetResultCount(resultCount);
                foreach (KeyValuePair<string, int> pair in resultCount.numTargetTypeDefects)
                {
                    List<Figure> figureList = schema.GetFigureByTag("TargetType." + pair.Key);

                    foreach (Figure figure in figureList)
                    {
                        count++;
                    }
                }
            }
            return count * 100;
        }

        public void UpdateTargetType(ProductResult productResult)
        {
            var resultCount = new ResultCount();
            productResult.GetResultCount(resultCount);

            foreach (KeyValuePair<string, int> pair in resultCount.numTargetTypeDefects)
            {
                List<Figure> figureList = schema.GetFigureByTag("TargetType." + pair.Key);

                foreach (Figure figure in figureList)
                {
                    UpdateValueFigure(figure as FigureGroup, pair.Key, pair.Value.ToString());
                }
            }

            Invalidate();
        }

        private void UpdateJudgmentFigure(FigureGroup figureGroup, string objectId, bool result)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find target figure - {0}", objectId));
                return;
            }

            Figure resultFigure = figureGroup.GetFigure("rectangle") as RectangleFigure;
            if (resultFigure == null)
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find rectangle figure - {0}", objectId));
                return;
            }

            //LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateJudgmentFigure - {0}", objectId));

            if (result == true)
            {
                resultFigure.TempBrush = okBrush;
            }
            else
            {
                resultFigure.TempBrush = ngBrush;
            }
        }

        private void UpdateValueFigure(FigureGroup figureGroup, string objectId, string valueStr)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find target figure - {0}", objectId));
                return;
            }

            if (!(figureGroup.GetFigure("value") is TextFigure textFigure))
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find text figure - {0}", objectId));
                return;
            }

            //LogHelper.Debug(LoggerType.Inspection, String.Format("Update Text - {0}", objectId));

            textFigure.Text = valueStr;
        }

        private void UpdateTargetTypeFigure(FigureGroup figureGroup, string objectId, string valueStr)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find target figure - {0}", objectId));
                return;
            }

            if (!(figureGroup.GetFigure("value") is TextFigure textFigure))
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Can't find text figure - {0}", objectId));
                return;
            }

            //LogHelper.Debug(LoggerType.Inspection, String.Format("Update Text - {0}", objectId));

            textFigure.Text = valueStr;
        }

        public void UpdateImage(ImageBuffer imageBuffer)
        {
            foreach (Figure figure in schema.FigureGroup)
            {
                if (figure is ImageFigure)
                {
                    if (figure.Tag is string fullId)
                    {
                        string[] idList = fullId.Split(new char[] { '.' });
                        if (idList.Count() != 3)
                        {
                            continue;
                        }

                        var imageFigure = (ImageFigure)figure;
                        imageFigure.Image = imageBuffer.GetImage(Convert.ToInt32(idList[1]), Convert.ToInt32(idList[2])).ToBitmap();
                    }
                }
            }

            Invalidate();
        }

        private void SchemaViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //ResetResult();
        }

        public void ResetResult()
        {
            if (schema == null)
            {
                return;
            }

            schema.ResetTempProperty();

            foreach (Figure figure in schema.FigureGroup)
            {
                if (figure is FigureGroup figureGroup)
                {
                    if (!(figureGroup.GetFigure("value") is TextFigure textFigure))
                    {
                        continue;
                    }

                    textFigure.Text = "0";
                }
            }
            Invalidate();
        }

        private void GetDefaultProperty(Figure figure)
        {
            FigureProperty figureProperty = figure.FigureProperty;
            if (figure is TextFigure)
            {
                Schema.DefaultFigureProperty.Font = (Font)figureProperty.Font.Clone();
                Schema.DefaultFigureProperty.TextColor = figureProperty.TextColor;
                Schema.DefaultFigureProperty.Alignment = figureProperty.Alignment;
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                Schema.DefaultFigureProperty.Pen = (Pen)figureProperty.Pen.Clone();

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    Schema.DefaultFigureProperty.Brush = (SolidBrush)figureProperty.Brush.Clone();
                }
            }
        }

        private void SetDefaultPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tracker.IsSingleSelected() == true)
            {
                Figure figure = tracker.GetFirstFigure();
                if (figure is FigureGroup)
                {
                    var figureGroup = figure as FigureGroup;
                    foreach (Figure subFigure in figureGroup)
                    {
                        GetDefaultProperty(subFigure);
                    }
                }
                else
                {
                    GetDefaultProperty(figure);
                }
            }
        }

        private void BackgroundColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            Color color;
            if (result == DialogResult.OK)
            {
                color = colorDialog1.Color;
                //Gradient(color);
                schema.BackColor = color;
            }

        }
        public void Gradient(Color StartColor)
        {
            var canvas = new Bitmap(Width, Height);
            var middleColor = Color.FromArgb(150, 50, 30);
            Color endColor = Color.White;
            var g = Graphics.FromImage(canvas);
            var br = new LinearGradientBrush(ClientRectangle, StartColor,
                                                            System.Drawing.Color.White, 0, false);
            var cb = new ColorBlend();

            br.RotateTransform(90);
            g.FillRectangle(br, ClientRectangle);

            BackgroundImage = canvas;
        }

        private void SchemaViewer_Load(object sender, EventArgs e)
        {
            //Gradient(Color.DarkSlateGray);
        }

        private void InitShow()
        {
            if (schema == null)
            {
                return;
            }

            ShowFigure("Step", toolStripButtonShowFov.Checked);
            ShowFigure("Probe", toolStripButtonShowPad.Checked);
            ShowFigure("Path", toolStripButtonShowPath.Checked);
        }

        private void ToolStripButtonShowFov_Click(object sender, EventArgs e)
        {
            bool onOff = !toolStripButtonShowFov.Checked;
            toolStripButtonShowFov.Checked = onOff;
            ShowFigure("Step", onOff);

            Invalidate();
        }

        private void ToolStripButtonShowPad_Click(object sender, EventArgs e)
        {
            bool onOff = !toolStripButtonShowPad.Checked;
            toolStripButtonShowPad.Checked = onOff;
            ShowFigure("Probe", onOff);

            Invalidate();
        }

        private void ToolStripButtonShowPath_Click(object sender, EventArgs e)
        {
            bool onOff = !toolStripButtonShowPath.Checked;
            toolStripButtonShowPath.Checked = onOff;
            ShowFigure("Path", onOff);

            Invalidate();
        }

        private void ShowFigure(string v, bool onOff)
        {
            int splitCnt = 0;
            switch (v)
            {
                case "Step":
                    splitCnt = 1;
                    break;
                case "Target":
                    splitCnt = 3;
                    break;
                case "Probe":
                    splitCnt = 4;
                    break;
                case "Path":
                    splitCnt = 0;
                    break;

            }
            List<Figure> list = schema.FigureGroup.FigureList.FindAll(f =>
            {
                string str = f.Tag as string;
                int cnt = string.IsNullOrEmpty(str) ? 0 : str.Split('.').Count();
                return splitCnt == cnt;
            }
            );

            list.ForEach(f => f.Visible = onOff);
        }
    }
}
