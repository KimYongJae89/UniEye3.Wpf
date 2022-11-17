using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class DefectReportPanel : Form, IDefectReportPanel
    {
        private ProductResult inspectionResult;
        private TargetView targetResultImage;
        private TargetView targetGoodImage;
        private TargetView targetCameraImage;
        public SchemaViewer SchemaView { get; }

        private bool onUpdateData;

        public DefectReportPanel()
        {
            InitializeComponent();

            SchemaView = new SchemaViewer();

            // 
            // schemaView
            // 
            SchemaView.Dock = System.Windows.Forms.DockStyle.Fill;
            SchemaView.Location = new System.Drawing.Point(620, 47);
            SchemaView.Name = "schemaView";
            SchemaView.Size = new System.Drawing.Size(683, 489);
            SchemaView.TabIndex = 177;
            SchemaView.TabStop = false;

            panelSchemaView.Controls.Add(SchemaView);

            targetGoodImage = new TargetView();
            panelProbeGoodImage.Controls.Add(targetGoodImage);

            targetResultImage = new TargetView();
            panelProbeNgImage.Controls.Add(targetResultImage);

            targetCameraImage = new TargetView();
            panelTargetGroupImage.Controls.Add(targetCameraImage);

            targetResultImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetResultImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetResultImage.Location = new System.Drawing.Point(3, 3);
            targetResultImage.Name = "targetViewNg";
            targetResultImage.Size = new System.Drawing.Size(409, 523);
            targetResultImage.TabStop = false;
            targetResultImage.Enable = false;

            targetGoodImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetGoodImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetGoodImage.Location = new System.Drawing.Point(3, 3);
            targetGoodImage.Name = "targetViewGood";
            targetGoodImage.Size = new System.Drawing.Size(409, 523);
            targetGoodImage.TabStop = false;
            targetGoodImage.Enable = false;

            targetCameraImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetCameraImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetCameraImage.Location = new System.Drawing.Point(3, 3);
            targetCameraImage.Name = "targetGroupView";
            targetCameraImage.Size = new System.Drawing.Size(409, 523);
            targetCameraImage.TabStop = false;
            targetCameraImage.Enable = false;
        }

        public void Initialize(ReportMode reportMode, Schema schema, ProductResult inspectionResult)
        {
            if (schema != null)
            {
                SchemaView.Schema = schema.Clone();
                SchemaView.Schema.AutoFit = true;
            }

            this.inspectionResult = inspectionResult;

            onUpdateData = true;

            showNgPad.Checked = true;
            showGoodPad.Checked = true;

            onUpdateData = false;
        }

        private void DefectReportPanel_Load(object sender, EventArgs e)
        {
            if (inspectionResult != null)
            {
                UpdateData(inspectionResult, null);
            }
        }

        public void UpdateData(ProductResult inspectionResult, Schema schema)
        {
            if (schema != null)
            {
                SchemaView.Schema = schema.Clone();
                SchemaView.Schema.AutoFit = true;
            }

            UpdateProbeList(inspectionResult);
        }

        public void UpdateProbeList(ProductResult inspectionResult)
        {
            this.inspectionResult = inspectionResult;

            UpdateProbeList();
            if (probeResultList.Rows.Count > 0)
            {
                probeResultList.Rows[0].Selected = true;
            }
        }

        private void UpdateProbeList()
        {
            onUpdateData = true;

            if (inspectionResult != null)
            {
                probeResultList.Rows.Clear();

                foreach (ProbeResult probeResult in inspectionResult)
                {
                    if ((showNgPad.Checked == true && probeResult.IsNG()) ||
                        (showGoodPad.Checked == true && probeResult.IsGood()))
                    {
                        AddProbeResult(probeResult);
                    }
                }
            }

            onUpdateData = false;
        }

        private void AddProbeResult(ProbeResult padResult)
        {
            int noId = probeResultList.Rows.Count + 1;
            string probeName;
            if (padResult.Probe != null)
            {
                probeName = padResult.Probe.Name;
            }
            else
            {
                probeName = padResult.ProbeName;
            }

            int rowId = probeResultList.Rows.Add(noId, probeName);
            probeResultList.Rows[rowId].Tag = padResult;

            Color cellColor = Color.White;
            if (padResult.IsOverkill())
            {
                cellColor = Color.LightYellow;
            }
            else
            {
                cellColor = Color.LightPink;
            }

            probeResultList.Rows[rowId].Cells[1].Style.BackColor = cellColor;

            if (padResult.Probe != null)
            {
                List<Figure> figureList = SchemaView.Schema.GetFigureByTag(padResult.Probe.FullId);
                foreach (Figure figure in figureList)
                {
                    if (figure is FigureGroup probeFigure)
                    {
                        Figure resultFigure = probeFigure.GetFigure("ProbeRect") as RectangleFigure;
                        if (resultFigure != null)
                        {
                            FigureProperty figureProperty = resultFigure.FigureProperty;
                            figureProperty.Brush = new SolidBrush(cellColor);
                        }
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            if (inspectionResult.IsDefected())
            {
                DialogResult = DialogResult.No;
            }
            else
            {
                bool rejectFlag = false;
                foreach (ProbeResult probeResult in inspectionResult)
                {
                    if (probeResult.IsNG())
                    {
                        rejectFlag = true;
                        break;
                    }
                }
                if (rejectFlag == false)
                {
                    inspectionResult.SetGood();
                }

                DialogResult = DialogResult.OK;
            }

            Close();
        }

        private void reviewButton_Click(object sender, EventArgs e)
        {

        }

        public void MoveNext()
        {
            int rowIndex = 0;

            if (probeResultList.SelectedRows.Count != 0)
            {
                rowIndex = probeResultList.SelectedRows[0].Index;
            }

            if (rowIndex < (probeResultList.Rows.Count - 1))
            {
                probeResultList.Rows[rowIndex + 1].Selected = true;
            }
        }

        public void MovePrev()
        {
            int rowIndex = 0;

            if (probeResultList.SelectedRows.Count != 0)
            {
                rowIndex = probeResultList.SelectedRows[0].Index;
            }

            if (rowIndex > 0)
            {
                probeResultList.Rows[rowIndex - 1].Selected = true;
            }
        }

        public void SetGood()
        {
            if (probeResultList.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = probeResultList.SelectedRows[0].Index;
            var padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;
            padResult.SetOverkill();

            probeResultList.SelectedRows[0].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        public void SetDefect()
        {
            if (probeResultList.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = probeResultList.SelectedRows[0].Index;
            var padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;
            padResult.SetResult(false);

            probeResultList.SelectedRows[0].DefaultCellStyle.BackColor = Color.LightPink;
        }

        private void probeResultList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
            {
                return;
            }

            if (probeResultList.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = probeResultList.SelectedRows[0].Index;
            var padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;

            SelectDefect(padResult);
        }

        private void SelectDefect(ProbeResult probeResult)
        {
            string probeFullId;
            if (probeResult.Probe != null)
            {
                probeFullId = probeResult.Probe.FullId;
            }
            else
            {
                probeFullId = string.Format("{0:00}.{1:00}.{2:000}.{3:000}", probeResult.StepNo, probeResult.CameraIndex, probeResult.TargetId, 1);
            }

            string probeImageFileName = string.Format("{0}\\{1}_{2}.jpg",
                    inspectionResult.ResultPath, probeFullId, probeResult.IsNG() ? "N" : "G");

            if (File.Exists(probeImageFileName) == true)
            {
                targetResultImage.UpdateImage((Bitmap)ImageHelper.LoadImage(probeImageFileName));

                targetResultImage.TempFigureGroup.Clear();
                probeResult.AppendResultFigures(targetResultImage.TempFigureGroup, ResultImageType.Probe);

                targetResultImage.ZoomFit();
            }

            UpdateCameraImage(probeResultList);
            SchemaView.ResetSelection();

            List<Figure> figureList = SchemaView.Schema.GetFigureByTag(probeFullId);
            if (figureList != null)
            {
                SchemaView.SelectFigure(figureList);
                SchemaView.SelectFigureByCrosshair(figureList);
            }
        }

        private void UpdateCameraImage(DataGridView resultList)
        {
            if (resultList.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = resultList.SelectedRows[0].Index;
            var padResult = (ProbeResult)resultList.SelectedRows[0].Tag;

            int lightTypeIndex = 0;

            string cameraImgFileName;

            if (padResult.Probe != null)
            {
                Target target = padResult.Probe.Target;
                if (padResult.Probe is VisionProbe visionProbe)
                {
                    lightTypeIndex = visionProbe.LightTypeIndexArr[0];
                }

                cameraImgFileName = Path.Combine(inspectionResult.ResultPath,
                    string.Format(InspectConfig.Instance().ImageNameFormat, target.CameraIndex, target.InspectStep.StepNo, lightTypeIndex));
            }
            else
            {
                cameraImgFileName = Path.Combine(inspectionResult.ResultPath,
                    string.Format(InspectConfig.Instance().ImageNameFormat, padResult.CameraIndex, padResult.StepNo, 0));
            }

            Image2D targetGroupImage;
            if (File.Exists(cameraImgFileName) == true)
            {
                targetGroupImage = new Image2D(cameraImgFileName);

                targetCameraImage.UpdateImage(targetGroupImage.ToBitmap());

                targetCameraImage.TempFigureGroup.Clear();

                padResult.AppendResultFigures(targetCameraImage.TempFigureGroup, ResultImageType.Camera);

                targetCameraImage.ZoomFit();
            }
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
