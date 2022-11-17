using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    internal enum TargetShape
    {
        Standard, Rectangle, Circle
    }

    // StepModel 전용 ShemaEditor
    public partial class SchemaEditor : Form
    {
        private DrawBox cameraImage;
        private SchemaViewer schemaViewer;
        private Schema schema = new Schema();
        private ImageBuffer imageBuffer;

        private StepModel stepModel;
        private FigureType addFigureType;
        private bool modified = false;
        private ObjectTree objectTree;
        public bool ShowInspectionStep { get; set; }

        private bool lockUpdate = false;

        public SchemaEditor()
        {
            schemaViewer = new SchemaViewer();
            cameraImage = new DrawBox();
            objectTree = new ObjectTree();

            InitializeComponent();

            SuspendLayout();

            KeyPreview = true;

            // 
            // objectTree
            // 
            objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            objectTree.Location = new System.Drawing.Point(0, 0);
            objectTree.Margin = new System.Windows.Forms.Padding(5);
            objectTree.Name = "objectTree";
            objectTree.Size = new System.Drawing.Size(294, 468);
            objectTree.TabIndex = 0;
            objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ObjectTree_AfterSelect);
            objectTree.DoubleClick += new System.EventHandler(ObjectTree_DoubleClick);

            panelObjectTree.Controls.Add(objectTree);
            // 
            // schemaViewer
            // 
            schemaViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            schemaViewer.Location = new System.Drawing.Point(246, 0);
            schemaViewer.Name = "schemaViewer";
            schemaViewer.Size = new System.Drawing.Size(511, 507);
            schemaViewer.TabIndex = 1;
            schemaViewer.TabStop = false;
            schemaViewer.Enable = true;
            schemaViewer.AddRegionCaptured = new AddRegionCapturedDelegate(SchemaViewer_AddRegionCaptured);
            schemaViewer.FigureMoved = new FigureMovedDelegate(SchemaViewer_FigureMoved);
            schemaViewer.FigureSelected = SchemaViewer_FigureSelected;
            schemaViewer.FigureCopy = new FigureCopyDelegate(SchemaViewer_FigureCopy);

            splitContainer.Panel1.Controls.Add(cameraImage);

            // 
            // cameraImage
            // 
            cameraImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            cameraImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            cameraImage.Location = new System.Drawing.Point(3, 3);
            cameraImage.Name = "cameraImage";
            cameraImage.Size = new System.Drawing.Size(409, 523);
            cameraImage.TabIndex = 8;
            cameraImage.TabStop = false;
            cameraImage.Enable = true;

            schemaViewPanel.Controls.Add(schemaViewer);
            ResumeLayout(false);

            // change language
            Text = StringManager.GetString(Text);
            toolTip.SetToolTip(addImageButton, StringManager.GetString(toolTip.GetToolTip(addImageButton)));
            toolTip.SetToolTip(addLineButton, StringManager.GetString(toolTip.GetToolTip(addLineButton)));
            toolTip.SetToolTip(addCircleButton, StringManager.GetString(toolTip.GetToolTip(addCircleButton)));
            toolTip.SetToolTip(addRectangleButton, StringManager.GetString(toolTip.GetToolTip(addRectangleButton)));
            toolTip.SetToolTip(saveButton, StringManager.GetString(toolTip.GetToolTip(saveButton)));
            toolTip.SetToolTip(refreshButton, StringManager.GetString(toolTip.GetToolTip(refreshButton)));
        }

        public void Initialize(StepModel stepModel)
        {
            this.stepModel = stepModel;
        }

        private void ModelSchemaEditor_Load(object sender, EventArgs e)
        {
            ObjectPool<ImageBuffer> imageBufferPool = DeviceManager.Instance().ImageBufferPool;
            imageBuffer = imageBufferPool.GetObject();

            InspectStep inspectStep = stepModel.GetInspectStep(0);

            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            imageAcquisition.Acquire(imageBuffer, inspectStep.LightParamSet, inspectStep.GetLightTypeIndexArr());

            schema = stepModel.ModelSchema.Clone();
            //previousSchema = schema;
            lockUpdate = true;

            if (schema.Region == RectangleF.Empty)
            {
                schema.Region = new RectangleF(0, 0, schemaViewer.Width, schemaViewer.Height);
            }

            autoFit.Checked = schema.AutoFit;

            scale.Text = Convert.ToInt32(schema.ViewScale * 100).ToString();
            scale.Enabled = (autoFit.Checked == false);

            schema.ViewScale = 1;
            targetShape.SelectedIndex = 0;

            schemaViewer.Schema = schema;
            //schemaViewer.Gradient(BackColor);
            if (ShowInspectionStep == true)
            {
                objectTree.InspectionStepName = "FOV";
            }

            objectTree.Initialize(stepModel);

            lockUpdate = false;
        }

        private void ObjectTree_DoubleClick(object sender, EventArgs e)
        {
            if (objectTree.SelectedNode == null)
            {
                return;
            }

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                if (obj is Target target)
                {
                    InsertTarget(target);
                }
                else if (obj is Probe probe)
                {
                    InsertProbe(probe);
                }
                else
                {
                    CaseByTypeOfObj(obj);
                }
            }
        }

        private void CaseByTypeOfObj(object obj)
        {
            if (obj.GetType().Name == "string")
            {
                TagTypeIsString((string)obj);
            }
            else
            {
                TagTypeIsBitmap(obj);
            }
        }

        private void TagTypeIsString(string tagName)
        {
            if (tagName.Contains("ResultValue."))
            {
                string probeResultName = tagName.Substring("ResultValue.".Count());
                var probe = (Probe)objectTree.SelectedNode.Parent.Tag;
                InsertProbeResult(probe, probeResultName);
            }
            else if (tagName.Contains("TargetType."))
            {
                string targetType = tagName.Substring("TargetType.".Count());
                InsertTargetType(targetType);
            }
        }

        private void TagTypeIsBitmap(object obj)
        {
            //InsertTargetImage(obj);
        }

        private void InsertTargetImage(Target target)
        {
            if (target.Image == null)
            {
                return;
            }

            Image image = target.Image.ToBitmap();
            Figure figure = null;

            int maxWidth = (int)(schemaViewer.Width * 0.3);
            int maxHeight = (int)(schemaViewer.Height * 0.3);
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            // 이미지 크기가 화면의 1/4보다 작으면 1:1크기로 표시하고, 그 이상이면 1/4크기로 크기를 제한한다.
            int width = image.Width;
            int height = image.Height;
            if (maxWidth < width)
            {
                width = maxWidth;
            }

            if (maxHeight < height)
            {
                height = maxHeight;
            }

            var rectangle = new RotatedRect(centerX - width / 2, centerY - height / 2, width, height, 0);
            figure = new ImageFigure(image, null, rectangle);
            figure.Id = "Image";
            figure.Tag = target.FullId;
            var imageFigure = figure as ImageFigure;

            var figureGroup = new FigureGroup();
            var schemaFigure = new SchemaFigure();
            if (imageFigure != null)
            {
                schemaFigure.AddFigure(imageFigure);
            }

            schemaFigure.Tag = imageFigure.Tag;
            schemaViewer.AddFigure(schemaFigure);
            modified = true;
        }

        private void InsertImage()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                int maxWidth = (int)(schemaViewer.Width * 0.8);
                int maxHeight = (int)(schemaViewer.Height * 0.8);
                int centerX = schemaViewer.Width / 2;
                int centerY = schemaViewer.Height / 2;

                var image = Image.FromFile(dialog.FileName);

                // 이미지 크기가 화면의 1/4보다 작으면 1:1크기로 표시하고, 그 이상이면 1/4크기로 크기를 제한한다.
                int width = image.Width;
                int height = image.Height;
                if (maxWidth < width)
                {
                    width = maxWidth;
                }

                if (maxHeight < height)
                {
                    height = maxHeight;
                }

                string extension = Path.GetExtension(dialog.FileName).TrimStart('.').ToLower();

                var rectangle = new RotatedRect(centerX - width / 2, centerY - height / 2, width, height, 0);
                var imageFigure = new ImageFigure(image, extension, rectangle);

                schemaViewer.AddFigure(imageFigure/*,"Image"*/);

                modified = true;
            }
        }



        private void InsertTarget(Target target)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            TextFigure textFigure = null;
            var rectangle = new RotatedRect((float)(centerX - shapeSize.Value / 2), (float)(centerY - shapeSize.Value / 2), (float)shapeSize.Value, (float)shapeSize.Value, 0);
            if (targetShape.SelectedIndex == (int)TargetShape.Standard)
            {
                var font = (Font)schema.DefaultFigureProperty.Font.Clone();

                textFigure = new TextFigure(target.Name, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
                textFigure.Id = "text";

                rectangle = textFigure.GetRectangle();
                rectangle.Inflate(5, 5);
            }

            Figure figure = null;

            if (targetShape.SelectedIndex == (int)TargetShape.Circle)
            {
                figure = new EllipseFigure(rectangle.ToRectangleF(), (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
                figure.Id = "rectangle";
            }
            else
            {
                figure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
                figure.Id = "rectangle";
            }

            var figureGroup = new FigureGroup();
            figureGroup.AddFigure(figure);
            if (textFigure != null)
            {
                figureGroup.AddFigure(textFigure);
            }

            figureGroup.Tag = target.FullId;

            schemaViewer.AddFigure(figureGroup/*,"Target"*/);

            modified = true;
        }

        private void InsertProbe(Probe probe)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            var font = (Font)schema.DefaultFigureProperty.Font.Clone();

            string probeName = probe.Name;

            var textFigure = new TextFigure(probeName, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();
            rectangle.Inflate(5, 5);

            var rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            var schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.Tag = probe.FullId;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void InsertProbeResult(Probe probe, string probeResultName)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            var font = (Font)schema.DefaultFigureProperty.Font.Clone();

            var textFigure = new TextFigure(probeResultName, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();

            centerY += (int)rectangle.Height / 2;
            var valueFigure = new TextFigure("0", new Point(centerX, centerY + (int)rectangle.Height), font, schema.DefaultFigureProperty.TextColor);
            valueFigure.Id = "value";

            RotatedRect valueRectangle = valueFigure.GetRectangle();

            rectangle = RotatedRect.Union(rectangle, valueRectangle);
            rectangle.Inflate(5, 5);

            var rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            var schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.AddFigure(valueFigure);
            schemaFigure.Tag = probe.FullId + "." + probeResultName;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void InsertTargetType(string targetType)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            var font = (Font)schema.DefaultFigureProperty.Font.Clone();

            var textFigure = new TextFigure(targetType, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();

            centerY += (int)rectangle.Height / 2;
            var valueFigure = new TextFigure("0", new Point(centerX, centerY + (int)rectangle.Height), font, schema.DefaultFigureProperty.TextColor);
            valueFigure.Id = "value";

            RotatedRect valueRectangle = valueFigure.GetRectangle();

            rectangle = RotatedRect.Union(rectangle, valueRectangle);
            rectangle.Inflate(5, 5);

            var rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            var schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.AddFigure(valueFigure);
            schemaFigure.Tag = "TargetType." + targetType;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void ShowCameraImage(Target target)
        {
            var figureGroup = new FigureGroup();
            target.AppendFigures(null, figureGroup, null, new CanvasPanel.Option());
            figureGroup.SetSelectable(false);

            cameraImage.FigureGroup = figureGroup;
            cameraImage.Invalidate();

            var bitmap = imageBuffer.GetImage(target.CameraIndex, 0).ToBitmap();
            cameraImage.UpdateImage(bitmap);
        }

        private void ObjectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            schemaViewer.ResetSelection();

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                string fullId = "";
                if (obj is Target target)
                {
                    ShowCameraImage(target);

                    fullId = target.FullId;
                }
                else if (obj is Probe)
                {
                    var probe = (Probe)obj;
                    ShowCameraImage(probe.Target);

                    fullId = probe.FullId;
                }
                else if (obj is string objectName)
                {
                    if (objectName.Contains("ResultValue.") == true)
                    {
                        var probe = (Probe)objectTree.SelectedNode.Parent.Tag;
                        if (probe != null)
                        {
                            ShowCameraImage(probe.Target);

                            string probeResultName = (string)obj;

                            fullId = probe.FullId + "." + probeResultName;
                        }
                    }
                    else if (objectName.Contains("TargetType.") == true)
                    {
                        fullId = objectName;
                    }
                }

                if (string.IsNullOrEmpty(fullId) == false)
                {
                    List<Figure> figureList = schemaViewer.Schema.GetFigureByTag(fullId);
                    schemaViewer.SelectFigure(figureList);
                }
            }
            else
            {
                cameraImage.UpdateImage(null);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSchema();
        }

        private void SaveSchema()
        {
            stepModel.ModelSchema = schema.Clone();
            stepModel.ModelSchema.ViewScale = Convert.ToSingle(scale.Text) / 100;
            stepModel.ModelSchema.AutoFit = autoFit.Checked;

            stepModel.SaveModelSchema();

            modified = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in schemaViewer.Schema)
            {
                if (figure.Tag != null)
                {
                    string targetFullId = figure.Tag as string;
                    Target target = stepModel.GetTarget(targetFullId);
                    if (target != null)
                    {
                        if (figure is FigureGroup figureGroup)
                        {
                            if (figureGroup.GetFigure("text") is TextFigure textFigure)
                            {
                                textFigure.Text = target.Name;

                                if (figureGroup.GetFigure("rectangle") is RectangleFigure rectangleFigure)
                                {
                                    RotatedRect rectangle = textFigure.GetRectangle();
                                    rectangle.Inflate(5, 5);

                                    rectangleFigure.Rectangle = rectangle;

                                    rectangleFigure.FigureProperty.Brush = new SolidBrush(Color.Ivory);
                                }
                            }
                        }
                    }
                }
            }

            schemaViewer.Invalidate();

            modified = true;
        }

        private void AddLineButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Line;
            schemaViewer.AddFigureMode = true;
            Cursor = Cursors.Cross;
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            InsertImage();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SchemaViewer_FigureMoved(List<Figure> figureList)
        {
            modified = true;
        }

        private void SchemaViewer_FigureCopy(List<Figure> figureList)
        {
            schemaViewer.ResetSelection();

            foreach (Figure figure in figureList)
            {
                var newFigure = (Figure)figure.Clone();

                RotatedRect rectangle = figure.GetRectangle();
                newFigure.SetRectangle(rectangle);

                schemaViewer.AddFigure(newFigure);
                schemaViewer.SelectFigure(newFigure);
            }

            modified = true;
        }

        private void SchemaViewer_FigureSelected(List<Figure> figureList, bool select)
        {
        }

        private void SchemaViewer_AddRegionCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            schemaViewer.AddFigureMode = false;
            Cursor = Cursors.Default;

            if (rectangle.IsEmpty)
            {
                return;
            }

            switch (addFigureType)
            {
                case FigureType.Line:
                    schemaViewer.AddFigure(new LineFigure(startPos, endPos, new Pen(Color.Blue)));
                    break;
                case FigureType.Rectangle:
                    schemaViewer.AddFigure(new RectangleFigure(rectangle, new Pen(Color.Blue)));
                    break;
                case FigureType.Ellipse:
                    schemaViewer.AddFigure(new EllipseFigure(rectangle, new Pen(Color.Blue)));
                    break;
            }

            modified = true;
        }

        private void AddCircleButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Ellipse;
            schemaViewer.AddFigureMode = true;
            Cursor = Cursors.Cross;
        }

        private void AddRectangleButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Rectangle;
            schemaViewer.AddFigureMode = true;
            Cursor = Cursors.Cross;
        }

        private void PanelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void ModelSchemaEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                if (MessageForm.Show(this, StringManager.GetString("Model Schema was changed. Do you want save the changes?"),
                                        StringManager.GetString("Warning"), MessageFormType.YesNo) == DialogResult.Yes)
                {
                    SaveSchema();
                }
            }
        }

        private void ModelSchemaEditor_KeyDown(object sender, KeyEventArgs e)
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
            if (e.KeyCode == Keys.Delete)
            {
                Delete();
            }
        }

        private void Copy()
        {
            schemaViewer.Copy();
        }

        private void Paste()
        {
            schemaViewer.Paste();
            modified = true;
        }

        private void Delete()
        {
            if (MessageForm.Show(this, StringManager.GetString("Do you want to delete the figure(s)?"),
                            "SchemaEditor", MessageFormType.YesNo) == DialogResult.No)
            {
                return;
            }

            schemaViewer.DeleteAll();

            modified = true;
        }

        private void Scale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
            {
                return;
            }

            modified = true;
        }

        private void AddTextButton_Click(object sender, EventArgs e)
        {
            var form = new InputForm("Input Text");
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var font = (Font)schema.DefaultFigureProperty.Font.Clone();

                var textFigure = new TextFigure(form.InputText, new Point(0, 0), font, schema.DefaultFigureProperty.TextColor);
                textFigure.Tag = form.InputText;

                schemaViewer.AddFigure(textFigure);

                modified = true;
            }
        }

        private void AutoFit_CheckedChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
            {
                return;
            }

            scale.Enabled = (autoFit.Checked == false);
            modified = true;
        }

        private void Scale_TextChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
            {
                return;
            }

            modified = true;
        }

        private void AutoSchemaButton_Click(object sender, EventArgs e)
        {
            Enabled = false;

            var cancellationTokenSource = new CancellationTokenSource();

            var loadingForm = new SimpleProgressForm("Generating Schema");
            loadingForm.Show(new Action(() =>
            {
                try
                {
                    schema = stepModel.AutoSchema(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {

                }
            }), cancellationTokenSource);

            Enabled = true;
            loadingForm.Close();

            schemaViewer.Schema = schema;
            modified = true;
        }
    }
}
