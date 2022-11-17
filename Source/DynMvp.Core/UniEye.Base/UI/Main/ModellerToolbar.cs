using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class ModellerToolbar : UserControl, IModellerToolbar
    {
        public IModellerPage ModellerPage { get; set; }

        private bool onValueUpdate = false;

        public ModellerToolbar()
        {
            InitializeComponent();

            if (DeviceConfig.Instance().VirtualMode == false)
            {
                loadImageSetToolStripButton.Visible = false;
            }
            else
            {
                grabProcessToolStripButton.Visible = false;
                multiShotToolStripButton.Visible = false;
                singleShotToolStripButton.Visible = false;
                toolStripButtonAlign.Visible = false;
                showLightPanelToolStripButton.Visible = false;
                scanButton.Visible = false;
            }

            lockMoveToolStripButton.BackColor = Color.LightGreen;

            ChangeCaption();
        }

        public void ChangeCaption()
        {
            addProbeToolStripButton.Text = StringManager.GetString(addProbeToolStripButton.Text);
            copyProbeToolStripButton.Text = StringManager.GetString(copyProbeToolStripButton.Text);
            pasteProbeToolStripButton.Text = StringManager.GetString(pasteProbeToolStripButton.Text);
            deleteProbeToolStripButton.Text = StringManager.GetString(deleteProbeToolStripButton.Text);
            syncParamToolStripButton.Text = StringManager.GetString(syncParamToolStripButton.Text);
            syncAllToolStripButton.Text = StringManager.GetString(syncAllToolStripButton.Text);
            zoomInToolStripButton.Text = StringManager.GetString(zoomInToolStripButton.Text);
            zoomOutToolStripButton.Text = StringManager.GetString(zoomOutToolStripButton.Text);
            zoomFitToolStripButton.Text = StringManager.GetString(zoomFitToolStripButton.Text);
            grabProcessToolStripButton.Text = StringManager.GetString(grabProcessToolStripButton.Text);
            showLightPanelToolStripButton.Text = StringManager.GetString(showLightPanelToolStripButton.Text);
            loadImageSetToolStripButton.Text = StringManager.GetString(loadImageSetToolStripButton.Text);
            singleShotToolStripButton.Text = StringManager.GetString(singleShotToolStripButton.Text);
            multiShotToolStripButton.Text = StringManager.GetString(multiShotToolStripButton.Text);
            addStepButton.Text = StringManager.GetString(addStepButton.Text);
            deleteStepButton.Text = StringManager.GetString(deleteStepButton.Text);
            groupProbeToolStripButton.Text = StringManager.GetString(groupProbeToolStripButton.Text);
            ungroupProbeToolStripButton.Text = StringManager.GetString(ungroupProbeToolStripButton.Text);
            tabPageFov.Text = StringManager.GetString(tabPageFov.Text);
            tabPageProbe.Text = StringManager.GetString(tabPageProbe.Text);
            tabPageView.Text = StringManager.GetString(tabPageView.Text);

            modelPropertyButton.Text = StringManager.GetString(modelPropertyButton.Text);
            exportFormatButton.Text = StringManager.GetString(exportFormatButton.Text);
            editStepButton.Text = StringManager.GetString(editStepButton.Text);

            toolStripButtonOrigin.Text = StringManager.GetString(toolStripButtonOrigin.Text);
            toolStripButtonJoystick.Text = StringManager.GetString(toolStripButtonJoystick.Text);
            toolStripButtonRobotSetting.Text = StringManager.GetString(toolStripButtonRobotSetting.Text);
            grabProcessToolStripButton.ToolTipText = StringManager.GetString(grabProcessToolStripButton.ToolTipText);
            showLightPanelToolStripButton.ToolTipText = StringManager.GetString(showLightPanelToolStripButton.ToolTipText);
            multiShotToolStripButton.ToolTipText = StringManager.GetString(multiShotToolStripButton.ToolTipText);
            singleShotToolStripButton.ToolTipText = StringManager.GetString(singleShotToolStripButton.ToolTipText);
            lockMoveToolStripButton.ToolTipText = StringManager.GetString(lockMoveToolStripButton.ToolTipText);

            lockMoveToolStripButton.Text = StringManager.GetString(lockMoveToolStripButton.Text);
            toolStripButtonStop.Text = StringManager.GetString(toolStripButtonStop.Text);

            tabControlMain.TabPages[0].Text = StringManager.GetString(tabControlMain.TabPages[0].Text);
            addStepButton.Text = StringManager.GetString(addStepButton.Text);
            deleteStepButton.Text = StringManager.GetString(deleteStepButton.Text);

            // Camera
            tabControlMain.TabPages[1].Text = StringManager.GetString(tabControlMain.TabPages[1].Text);
            selectCameraButton.Text = StringManager.GetString(selectCameraButton.Text);

            // Probe
            tabControlMain.TabPages[2].Text = StringManager.GetString(tabControlMain.TabPages[2].Text);
            addProbeToolStripButton.Text = StringManager.GetString(addProbeToolStripButton.Text);
            copyProbeToolStripButton.Text = StringManager.GetString(copyProbeToolStripButton.Text);
            pasteProbeToolStripButton.Text = StringManager.GetString(pasteProbeToolStripButton.Text);
            deleteProbeToolStripButton.Text = StringManager.GetString(deleteProbeToolStripButton.Text);
            syncParamToolStripButton.Text = StringManager.GetString(syncParamToolStripButton.Text);
            syncAllToolStripButton.Text = StringManager.GetString(syncAllToolStripButton.Text);

            // View
            tabControlMain.TabPages[3].Text = StringManager.GetString(tabControlMain.TabPages[3].Text);
            previewTypeToolStripButton.Text = StringManager.GetString(previewTypeToolStripButton.Text);
        }

        private void ModellerToolbar_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(IModellerPage modellerPage)
        {
            ModellerPage = modellerPage;

            BuildAlgorithmTypeMenu();

            UpdateCameraToolstripButton();
            UpdateLightTypeCombo();
        }

        private void UpdateCameraToolstripButton()
        {
            LogHelper.Debug(LoggerType.Operation, "UpdateCameraToolstripButton");

            selectCameraButton.DropDownItems.Clear();

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            if (cameraHandler.NumCamera < 2)
            {
                selectCameraButton.Visible = false;
                return;
            }

            foreach (Camera camera in cameraHandler)
            {
                var toolStripCameraButton = new ToolStripButton(camera.Name);
                toolStripCameraButton.AutoSize = true;
                toolStripCameraButton.Click += toolStripCameraButton_Click;
                selectCameraButton.DropDownItems.Add(toolStripCameraButton);
            }

            selectCameraButton.DropDown.Width = 200;
            selectCameraButton.DropDown.Height = 1000;

            selectCameraButton.Text = selectCameraButton.DropDownItems[0].Text;
        }

        private void toolStripCameraButton_Click(object sender, EventArgs e)
        {
            var toolStripCameraItem = (ToolStripButton)sender;
            selectCameraButton.Text = toolStripCameraItem.Text;

            int cameraIndex = selectCameraButton.DropDownItems.IndexOf(toolStripCameraItem);

            ModellerPage.CameraIndexChanged(cameraIndex);
        }

        public void UpdateLightTypeCombo()
        {
            LogHelper.Debug(LoggerType.Operation, "UpdateLightTypeCombo");

            LightParamSet lightParamSet = LightConfig.Instance().LightParamSet;

            int numLightType = lightParamSet.NumLightType;
            if (numLightType < 2)
            {
                selectLightButton.Visible = false;
                return;
            }

            selectLightButton.DropDownItems.Clear();
            for (int i = 0; i < numLightType; i++)
            {
                string lightTypeName = lightParamSet[i].Name;
                ToolStripItem lightToolStripItem = selectLightButton.DropDownItems.Add(lightTypeName);
                lightToolStripItem.Tag = i;
                lightToolStripItem.Click += lightToolStripItem_Click;
            }

            selectLightButton.Text = selectLightButton.DropDownItems[0].Text;
        }

        private void lightToolStripItem_Click(object sender, EventArgs e)
        {
            var toolStripItem = (ToolStripItem)sender;

            int lightTypeIndex = (int)toolStripItem.Tag;
            ModellerPage.LightTypeChanged(lightTypeIndex);

            selectLightButton.Text = selectLightButton.DropDownItems[lightTypeIndex].Text;
        }

        private void zoomInToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ZoomInButtonClicked();
        }

        private void zoomOutToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ZoomOutButtonClicked();
        }

        public void SelectInspectStep(int stepNo)
        {
            onValueUpdate = true;

            comboInspectStep.SelectedIndex = stepNo;

            onValueUpdate = false;
        }

        public void UpdateButtonState(TeachHandler teachHandler, int cameraIndex, int lightTypeIndex)
        {
            selectCameraButton.Text = string.Format(StringManager.GetString("Camera") + " {0}", cameraIndex + 1);
            if (selectLightButton.Visible == true)
            {
                selectLightButton.Text = selectLightButton.DropDownItems[lightTypeIndex].Text;
            }

            deleteProbeToolStripButton.Enabled = (teachHandler.Count > 0);

            foreach (ITeachObject teachObject in teachHandler.SelectedObjs)
            {
                if (!(teachObject is VisionProbe visionProbe))
                {
                    continue;
                }

                onValueUpdate = true;

                previewTypeToolStripButton.DropDownItems.Clear();

                foreach (string previewName in visionProbe.GetPreviewNames())
                {
                    var previewButton = new ToolStripButton(previewName);
                    previewButton.Click += PreviewToolStripButton_Click;
                    previewTypeToolStripButton.DropDownItems.Add(previewButton);
                }

                previewTypeToolStripButton.Text = previewTypeToolStripButton.DropDownItems[0].Text;

                onValueUpdate = false;
            }

            bool onLive = ModellerPage.IsOnLive();
            if (onLive)
            {
                multiShotToolStripButton.Checked = true;
                multiShotToolStripButton.BackColor = Color.LightGreen;
            }
            else
            {
                multiShotToolStripButton.Checked = false;
                multiShotToolStripButton.BackColor = Color.Transparent;
            }

            if (OperationConfig.Instance().UseUserManager)
            {
                lockMoveToolStripButton.Visible = UserHandler.Instance.CurrentUser.IsAuth(ERoleType.TeachPage);
            }

            lockMoveToolStripButton.BackColor = Color.LightGreen;

            toolStripButtonFineMove.Checked = ModellerPage.IsOnFineMove();
        }

        private void PreviewToolStripButton_Click(object sender, EventArgs e)
        {
            var previewButton = (ToolStripButton)sender;

            previewTypeToolStripButton.Text = previewButton.Text;

            int previewIndex = previewTypeToolStripButton.DropDownItems.IndexOf(previewButton);

            ModellerPage.PreviewIndexChanged(previewIndex);
        }

        public void UpdateInspectStepButton(int stepNoSelected = 0)
        {
            LogHelper.Debug(LoggerType.Operation, "InitSelectStepButton");

            onValueUpdate = true;

            comboInspectStep.Items.Clear();

            var stepModel = (StepModel)ModelManager.Instance().CurrentModel;
            int numInspectStep = stepModel.NumInspectStep;
            foreach (InspectStep inspectStep in stepModel.InspectStepList)
            {
                comboInspectStep.Items.Add(string.Format("{0}: {1}", inspectStep.StepNo, inspectStep.GetStepName()));
            }

            LogHelper.Debug(LoggerType.Operation, "InitStepList StepList Add");

            if ((comboInspectStep.Items.Count > 0) && (comboInspectStep.Items.Count > stepNoSelected))
            {
                comboInspectStep.SelectedIndex = stepNoSelected;
            }

            onValueUpdate = false;

            LogHelper.Debug(LoggerType.Operation, "InitStepList Finished");
        }

        private void modelPropertyButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ModelPropertyButtonClicked();
        }

        private void exportFormatButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ExportFormatButtonClicked();
        }

        private void editStepButton_Click(object sender, EventArgs e)
        {
            int stepIndex = comboInspectStep.SelectedIndex;

            string stepName = ModellerPage.EditStepButtonClicked(stepIndex);

            if (string.IsNullOrEmpty(stepName) == false)
            {
                comboInspectStep.Items[stepIndex] = stepName;
                comboInspectStep.Text = stepName;
            }
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            DeviceManager.Instance().RobotStage.StopMove();
        }

        public void movePrevStepButton_Click(object sender, EventArgs e)
        {
            int stepIndex = comboInspectStep.SelectedIndex;
            if (stepIndex > 0)
            {
                //movePrevStepButton.Enabled = false;
                comboInspectStep.SelectedIndex = stepIndex - 1;
                //movePrevStepButton.Enabled = true;
            }
        }

        public void moveNextStepButton_Click(object sender, EventArgs e)
        {
            int stepIndex = comboInspectStep.SelectedIndex;
            if (stepIndex < (comboInspectStep.Items.Count - 1))
            {
                comboInspectStep.SelectedIndex = stepIndex + 1;
            }
        }

        private void comboInspectStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
            {
                return;
            }

            ModellerPage.InspectStepChanged(comboInspectStep.SelectedIndex);
        }

        private void toolStripButtonFineMove_Click(object sender, EventArgs e)
        {
            ModellerPage.ToggleFineMove();
        }

        public void BuildAlgorithmTypeMenu()
        {
            addProbeToolStripButton.DropDownItems.Clear();

            BuildLiteAlgorithmTypeMenu();
            BuildDimensionAlgorithmTypeMenu();
            BuildIdentificationAlgorithmTypeMenu();

            addProbeToolStripButton.DropDown.Width = 200;
            addProbeToolStripButton.DropDown.Height = 1000;
        }

        private void MarkerProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - MarkerProbeParamControlToolStripButton_Click");

            RotatedRect rect = ModellerPage.GetDefaultProbeRegion();
            var markerProbe = (MarkerProbe)ProbeFactory.Create(ProbeType.Marker);
            markerProbe.BaseRegion = rect;

            ModellerPage.AddProbe(markerProbe);
        }

        private void BuildLiteAlgorithmTypeMenu()
        {
            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(PatternMatching.TypeName))
            {
                var patternMatchingToolStripButton = new ToolStripButton(StringManager.GetString("Pattern Matching"));
                patternMatchingToolStripButton.Click += PatternMatchinToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(patternMatchingToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(BinaryCounter.TypeName))
            {
                var binaryCounterToolStripButton = new ToolStripButton(StringManager.GetString("Binary Counter"));
                binaryCounterToolStripButton.Click += BinaryCounterToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(binaryCounterToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(BrightnessChecker.TypeName))
            {
                var brightnessCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Brightness Checker"));
                brightnessCheckerToolStripButton.Click += BrightnessCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(brightnessCheckerToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(ColorChecker.TypeName))
            {
                var colorCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Color Checker"));
                colorCheckerToolStripButton.Click += ColorCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(colorCheckerToolStripButton);
            }
        }

        private void BuildDimensionAlgorithmTypeMenu()
        {
            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(LineChecker.TypeName))
            {
                var lineCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Line Checker"));
                lineCheckerToolStripButton.Click += LineCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(lineCheckerToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(WidthChecker.TypeName))
            {
                var widthCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Width Checker"));
                widthCheckerToolStripButton.Click += WidthCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(widthCheckerToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(CornerChecker.TypeName))
            {
                var cornerCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Corner Checker"));
                cornerCheckerToolStripButton.Click += CornerCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(cornerCheckerToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(RectChecker.TypeName))
            {
                var rectCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Rect Checker"));
                rectCheckerToolStripButton.Click += RectCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(rectCheckerToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(CircleChecker.TypeName))
            {
                var circleFinderToolStripButton = new ToolStripButton(StringManager.GetString("Circle Finder"));
                circleFinderToolStripButton.Click += CircleFinderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(circleFinderToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(BlobChecker.TypeName))
            {
                var blobCheckerToolStripButton = new ToolStripButton(StringManager.GetString("Blob Checker"));
                blobCheckerToolStripButton.Click += BlobCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(blobCheckerToolStripButton);
            }
        }

        private void BuildIdentificationAlgorithmTypeMenu()
        {
            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(BarcodeReader.TypeName))
            {
                var barcodeReaderToolStripButton = new ToolStripButton(StringManager.GetString("Barcode Reader"));
                barcodeReaderToolStripButton.Click += BarcodeReaderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(barcodeReaderToolStripButton);
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(CharReader.TypeName))
            {
                var characterReaderToolStripButton = new ToolStripButton(StringManager.GetString("Character Reader"));
                characterReaderToolStripButton.Click += CharacterReaderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(characterReaderToolStripButton);
            }
        }

        private void BuildOcrAlgorithmTypeMenu()
        {

        }

        private void ColorCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ColorCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new ColorChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void DepthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DepthCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new DepthChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void CharacterReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CharacterReaderToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(AlgorithmFactory.Instance().CreateCharReader());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void DaqProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DaqProbeToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            DaqProbe daqProbe = objectCreator.CreateDaqProbe();
            if (daqProbe == null)
            {
                return;
            }

            ModellerPage.AddProbe(daqProbe);
        }

        private void CalibrationCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CalibrationCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CalibrationChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void BarcodeReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - barcodeReaderToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(AlgorithmFactory.Instance().CreateBarcodeReader());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void DistanceCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - distanceCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            ComputeProbe computeProbe = objectCreator.CreateComputeProbe();

            //if (computeProbe == null)
            //    return;            

            //AddComputeProbe(computeProbe);
            //ComputeParamControl computeParamControl = new ComputeParamControl();
            //this.paramContainer.Panel1.Controls.Clear();
            //computeParamControl.Model = SystemManager.Instance().CurrentModel;
            //this.paramContainer.Panel1.Controls.Add(computeParamControl);
        }

        private void RectCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - rectCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new RectChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void BlobCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - blobCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BlobChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void CircleFinderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - circleFinderToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CircleChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void CornerCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - cornerCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CornerChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void WidthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - WidthCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new WidthChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void LineCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - lineCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new LineChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void BrightnessCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BrightnessCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BrightnessChecker());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        public void Teach()
        {
            throw new NotImplementedException();
        }

        private void BinaryCounterToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BinaryCounterToolStripButton_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BinaryCounter());
            if (probe == null)
            {
                return;
            }

            ModellerPage.AddProbe(probe);
        }

        private void PatternMatchinToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - patternMatchingToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(ModellerPage);
            Probe probe = objectCreator.CreatePatternMatching();
            ModellerPage.AddProbe(probe);

            // 보정프로브로 설정
            probe.Target.SetFiducialProbe(probe);
        }

        private void lockMoveToolStripButton_Click(object sender, EventArgs e)
        {
            bool teachMove = true;

            if (teachMove)
            {
                lockMoveToolStripButton.Checked = !lockMoveToolStripButton.Checked;
                if (lockMoveToolStripButton.Checked == false)
                {
                    lockMoveToolStripButton.BackColor = Color.LightGreen;
                }
                else
                {
                    lockMoveToolStripButton.BackColor = Color.WhiteSmoke;
                }
            }
            else
            {
                lockMoveToolStripButton.Checked = false;
                lockMoveToolStripButton.BackColor = Color.LightGreen;
            }

            ModellerPage.ToggleLockMove();
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            if (MessageForm.Show(ParentForm, StringManager.GetString("Do you want to add step?"), MessageFormType.YesNo) == DialogResult.No)
            {
                return;
            }

            InspectStep inspectStep = ModellerPage.CreateInspectionStep();

            UpdateInspectStepButton(inspectStep.StepNo);
        }

        private void deleteStepButton_Click(object sender, EventArgs e)
        {
            ModellerPage.DeleteStepButtonClicked();
        }

        private void multiShotToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ToggleLive();
        }

        private void groupProbeToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.GroupButtonClicked();
        }

        private void ungroupProbeToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.UngroupButtonClicked();
        }

        private void toolStripButtonRobotSetting_Click(object sender, EventArgs e)
        {
            ModellerPage.RobotSettingButtonClicked();
        }

        private void toolStripButtonJoystick_Click(object sender, EventArgs e)
        {
            ModellerPage.JoystickButtonClicked();
        }

        private void toolStripButtonOrigin_Click(object sender, EventArgs e)
        {
            ModellerPage.OriginButtonClicked();
        }

        private void zoomFitToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ZoomFitButtonClicked();
        }

        private void singleShotToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.SingleShotButtonClicked();
        }

        private void toolStripButtonSearchProbe_Click(object sender, EventArgs e)
        {
            Probe probe = ModellerPage.SearchProbeButtonClicked();
            if (probe != null)
            {
                moveNextStepButton.Enabled = false;
                comboInspectStep.SelectedIndex = probe.Target.InspectStep.StepNo;
                moveNextStepButton.Enabled = true;
            }
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ScanButtonClicked();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.UndoButtonClicked();
        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.RedoButtonClicked();
        }

        private void grabProcessToolStripButton_Click(object sender, EventArgs e)
        {
            grabProcessToolStripButton.BackColor = Color.LightGreen;

            ModellerPage.StartGrabProcess();
        }

        private void editSchemaButton_Click(object sender, EventArgs e)
        {
            ModellerPage.EditSchemaButtonClicked();
        }

        private void createSchemaButton_Click(object sender, EventArgs e)
        {
            ModellerPage.CreateSchemaButtonClicked();
        }

        private void copyProbeToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.CopyButtonClicked();
        }

        private void pasteProbeToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.PasteButtonClicked();
        }

        private void deleteProbeToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.DeleteButtonClicked();
        }

        private void setFiducialToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.SetFiducialButtonClicked();
        }

        private void syncParamToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.SyncParamButtonClicked();
        }

        private void syncAllToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.SyncAllButtonClicked();
        }

        private void loadImageSetToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.LoadImageSetButtonClicked();
        }

        private void showLightPanelToolStripButton_Click(object sender, EventArgs e)
        {
            ModellerPage.ShowLightPanelButtonClicked();
        }

        private void toolBtnPrevImage_Click(object sender, EventArgs e)
        {
            ModellerPage.PrevImageButtonClicked();
        }

        private void toolBtnNextImage_Click(object sender, EventArgs e)
        {
            ModellerPage.NextImageButtonClicked();
        }

        public string Title => "ModellerToolBar";

        public Control Control => this;

        public DockedLocation DockedLocation => DockedLocation.DockedTop;
    }
}
