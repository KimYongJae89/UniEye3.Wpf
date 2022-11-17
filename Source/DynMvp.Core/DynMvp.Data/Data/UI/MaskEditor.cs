using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class MaskEditor : Form
    {
        private DrawBox drawBox;
        private bool modified = false;
        private bool onUpdate = false;
        private ImageD maskImage;
        private FigureType addFigureType;
        public ImagingLibrary AlgorithmType { get; set; } = ImagingLibrary.OpenCv;

        public MaskEditor()
        {
            InitializeComponent();

            drawBox = new DrawBox();

            drawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            drawBox.Location = new System.Drawing.Point(246, 0);
            drawBox.Name = "DrawBox";
            drawBox.Size = new System.Drawing.Size(511, 507);
            drawBox.TabIndex = 1;
            drawBox.TabStop = false;
            drawBox.Enable = true;
            drawBox.FigureAdd = new FigureAddDelegate(drawBox_FigureAdd);
            drawBox.FigureMoved = new FigureMovedDelegate(drawBox_FigureMoved);
            drawBox.FigureSelected = drawBox_FigureSelected;
            drawBox.FigureCopy = new FigureCopyDelegate(drawBox_FigureCopy);
            drawBox.FigureGroup = new FigureGroup();

            //this.Controls.Add(drawBox);
            //MaskEditor_Fill_Panel.Controls.Add(drawBox);
            MaskEditor_Fill_Panel.ClientArea.Controls.Add(drawBox);
            //MaskEditor_Fill_Panel.Dock = DockStyle.Left;
            //MaskEditor_Fill_Panel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;

            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);
            toolStripButtonCircle.Text = StringManager.GetString(toolStripButtonCircle.Text);
            toolStripButtonRectangle.Text = StringManager.GetString(toolStripButtonRectangle.Text);
            toolStripButtonDelete.Text = StringManager.GetString(toolStripButtonDelete.Text);
        }

        public void SetImage(Image2D image)
        {
            maskImage = image;
            drawBox.UpdateImage(maskImage.ToBitmap());
        }

        public ImageD GetImage()
        {
            return maskImage;
        }

        public void SetMaskFigures(FigureGroup figureGroup)
        {
            drawBox.FigureGroup = figureGroup;
        }

        private void toolStripSplitButtonAddFigure_ButtonClick(object sender, EventArgs e)
        {
            if (drawBox.AddFigureMode)
            {
                SetAddFigureMode(false);
            }
            else
            {
                SetAddFigureMode(true);
            }
        }

        private void toolStripButtonCircle_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Ellipse);
        }

        private void toolStripButtonRectangle_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Rectangle);
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Line);
        }

        private void toolStripButtonFreeLine_Click(object sender, EventArgs e)
        {
            SetAddFigureMode(FigureType.Polygon);
        }

        private void SetAddFigureMode(FigureType figureType)
        {
            if (addFigureType == figureType)
            {
                //addFigureType = FigureType.None;
                drawBox.AddFigureMode = false;
                drawBox.TrackerShape = FigureType.Rectangle;
            }
            else
            {
                addFigureType = figureType;
                drawBox.TrackerShape = figureType;
                drawBox.AddFigureMode = true;
            }

            UpdateButtonState();
        }

        private void SetAddFigureMode(bool addState)
        {
            drawBox.AddFigureMode = addState;
            if (addState)
            {
                drawBox.TrackerShape = addFigureType;
            }
            else
            {
                drawBox.TrackerShape = FigureType.Rectangle;
            }

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            foreach (ToolStripMenuItem item in toolStripSplitButtonAddFigure.DropDownItems)
            {
                item.Checked = false;
            }

            toolStripSplitButtonAddFigure.BackColor = SystemColors.Control;
            MaskEditor_Fill_Panel.Cursor = Cursors.Default;
            if (drawBox.AddFigureMode)
            {
                toolStripSplitButtonAddFigure.BackColor = Color.LightGreen;
                MaskEditor_Fill_Panel.Cursor = Cursors.Cross;
            }

            switch (addFigureType)
            {
                case FigureType.Ellipse:
                    toolStripButtonCircle.Checked = true;
                    break;
                case FigureType.Rectangle:
                    toolStripButtonRectangle.Checked = true;
                    break;
                case FigureType.Line:
                    toolStripButtonLine.Checked = true;
                    break;
                case FigureType.Polygon:
                    toolStripButtonFreeLine.Checked = true;
                    break;
            }
        }

        private void drawBox_FigureMoved(List<Figure> figureList)
        {

        }

        private void drawBox_FigureCopy(List<Figure> figureList)
        {
            drawBox.ResetSelection();

            foreach (Figure figure in figureList)
            {
                var newFigure = (Figure)figure.Clone();

                RotatedRect rectangle = figure.GetRectangle();
                newFigure.SetRectangle(rectangle);

                drawBox.FigureGroup.AddFigure(newFigure);

                drawBox.SelectFigure(newFigure);
            }
        }

        private void drawBox_FigureSelected(List<Figure> figureList, bool select)
        {

        }

        private void drawBox_FigureAdd(List<PointF> pointList, FigureType figureType)
        {
            //drawBox.AddFigureMode = false;
            //this.Cursor = Cursors.Default;

            if (pointList.Count <= 1)
            {
                return;
            }

            PointF pt1 = pointList.First();
            PointF pt2 = pointList.Last();
            var rectangle = RectangleF.FromLTRB(
                Math.Min(pt1.X, pt2.X),
                Math.Min(pt1.Y, pt2.Y),
                Math.Max(pt1.X, pt2.X),
                Math.Max(pt1.Y, pt2.Y));

            switch (addFigureType)
            {
                case FigureType.Rectangle:
                    drawBox.FigureGroup.AddFigure(new RectangleFigure(rectangle, new Pen(Color.Red), new SolidBrush(Color.Red)));
                    break;
                case FigureType.Ellipse:
                    drawBox.FigureGroup.AddFigure(new EllipseFigure(rectangle, new Pen(Color.Red), new SolidBrush(Color.Red)));
                    break;
                case FigureType.Line:
                    drawBox.FigureGroup.AddFigure(new LineFigure(pt1, pt2, new Pen(Color.Red, 5)));
                    break;
                case FigureType.Polygon:
                    //drawBox.FigureGroup.AddFigure(new PolygonFigure(pointList, new Pen(Color.Red, 5), new SolidBrush(Color.Red)));
                    var polygonFigure = (PolygonFigure)drawBox.FigureGroup.AddFigure(new PolygonFigure(pointList, new Pen(Color.Red, 5), null));
                    polygonFigure.IsEditable = false;
                    break;
            }
            modified = true;
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            drawBox.RemoveSelectedFigure();
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            string debugString = "Apply drawing mask figure to image?";
            DialogResult dialogResult = MessageBox.Show(debugString, "Question", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            ApplyMaskFigure();
        }

        private void ApplyMaskFigure()
        {
            AlgoImage algoMaskImage = ImageBuilder.Build("", maskImage, (maskImage.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            //algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));

            ImageD maksImage2 = GetMaskImage();
            //maksImage2.SaveImage(@"D:\maksImage2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            AlgoImage algoMaskImage2 = ImageBuilder.Build("", maksImage2, (maksImage2.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            //algoMaskImage2.Save(@"algoMaskImage2.bmp", new DebugContext(true, @"D:\"));

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoMaskImage);
            imageProcessing.Or(algoMaskImage, algoMaskImage2, algoMaskImage);
            maskImage = algoMaskImage.ToImageD();

            drawBox.FigureGroup.Clear();
            UpdateData();
            modified = false;
        }
        private ImageD GetMaskImage()
        {
            var maskImage = new Bitmap(drawBox.Image.Width, drawBox.Image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            ImageHelper.Clear(maskImage, 0);

            FigureGroup figureGroup = drawBox.FigureGroup;
            //if (figureGroup.FigureExist == false)
            //{

            //    return Image2D.ToImage2D(maskImage);
            //}

            var g = Graphics.FromImage(maskImage);
            figureGroup.SetTempBrush(new SolidBrush(Color.White));
            figureGroup.Draw(g, new CoordTransformer(), true);
            figureGroup.ResetTempProperty();
            g.Dispose();

            //ImageHelper.SaveImage(maskImage, @"d:\maskImage.bmp");
            maskImage = ImageHelper.ConvertGrayImage(maskImage);
            AlgoImage algoImage = ImageBuilder.Build("", Image2D.ToImage2D(maskImage), ImageType.Grey);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.Binarize(algoImage, 1);
            return algoImage.ToImageD();
            //return Image2D.ToImage2D(maskImage);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                string message = "Mask is modified. apply?";
                DialogResult dialogResult = MessageBox.Show(message, "Question", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                {
                    ApplyMaskFigure();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void toolStripButtonInvert_Click(object sender, EventArgs e)
        {
            // 무조건 OpenCV로 변환하게 되어있다... 변경이 필요함
            //ImagingLibrary imagingLibrary = OperationConfig.Instance().ImagingLibrary;
            //AlgoImage algoMaskImage = ImageBuilder.Build("", maskImage, (maskImage.NumBand == 1) ? ImageType.Grey : ImageType.Color);

            AlgoImage algoMaskImage = ImageBuilder.GetInstance(AlgorithmType).Build(maskImage, (maskImage.NumBand == 1) ? ImageType.Grey : ImageType.Color);
            algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoMaskImage);
            imageProcessing.Not(algoMaskImage, algoMaskImage);
            algoMaskImage.Save(@"algoMaskImage.bmp", new DebugContext(true, @"D:\"));
            maskImage = algoMaskImage.ToImageD();

            UpdateData();
        }

        private void UpdateData()
        {
            if (onUpdate)
            {
                return;
            }

            onUpdate = true;

            drawBox.UpdateImage(maskImage.ToBitmap());

            onUpdate = false;
        }
    }
}
