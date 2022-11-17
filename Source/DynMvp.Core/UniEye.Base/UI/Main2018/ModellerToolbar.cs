using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.InspectData;
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
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main2018
{
    public partial class ModellerToolbar : UserControl, IModellerToolbar//, IModellerPane
    {
        private IModellerPage modellerPage;
        //private bool onValueUpdate;

        public IModellerPage ModellerPage
        {
            set => modellerPage = value;
        }

        public ModellerToolbar()
        {
            InitializeComponent();

            Dock = DockStyle.Top;
        }

        public void Initialize(IModellerPage modellerPage)
        {
            this.modellerPage = modellerPage;
            BuildAlgorithmTypeMenu();
            UpdateLightTypeCombo();
        }

        public void ChangeCaption()
        {

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

        private void MarkerProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - MarkerProbeParamControlToolStripButton_Click");

            RotatedRect rect = modellerPage.GetDefaultProbeRegion();
            var markerProbe = (MarkerProbe)ProbeFactory.Create(ProbeType.Marker);
            markerProbe.BaseRegion = rect;

            modellerPage.AddProbe(markerProbe);
        }

        private void ColorCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ColorCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new ColorChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void DepthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DepthCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new DepthChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void CharacterReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CharacterReaderToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(AlgorithmFactory.Instance().CreateCharReader());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void DaqProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DaqProbeToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            DaqProbe daqProbe = objectCreator.CreateDaqProbe();
            if (daqProbe == null)
            {
                return;
            }

            modellerPage.AddProbe(daqProbe);
        }

        private void CalibrationCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CalibrationCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CalibrationChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void BarcodeReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - barcodeReaderToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(AlgorithmFactory.Instance().CreateBarcodeReader());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void DistanceCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - distanceCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
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

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new RectChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void BlobCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - blobCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BlobChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void CircleFinderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - circleFinderToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CircleChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void CornerCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - cornerCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new CornerChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void WidthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - WidthCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new WidthChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void LineCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - lineCheckerToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new LineChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void BrightnessCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BrightnessCheckerToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BrightnessChecker());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void BinaryCounterToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BinaryCounterToolStripButton_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            VisionProbe probe = objectCreator.CreateVisionProbe(new BinaryCounter());
            if (probe == null)
            {
                return;
            }

            modellerPage.AddProbe(probe);
        }

        private void PatternMatchinToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - patternMatchingToolStripMenuItem_Click");

            var objectCreator = new ObjectCreator(modellerPage);
            Probe probe = objectCreator.CreatePatternMatching();
            modellerPage.AddProbe(probe);

            // 보정프로브로 설정
            probe.Target.SetFiducialProbe(probe);
        }

        void IModellerToolbar.SelectInspectStep(int stepNo)
        {
            //onValueUpdate = true;

            //comboInspectStep.SelectedIndex = stepNo;

            //onValueUpdate = false;
        }

        void IModellerToolbar.UpdateButtonState(TeachHandler teachHandler, int cameraIndex, int lightTypeIndex)
        {
            // selectCameraButton.Text = String.Format(StringManager.GetString("Camera") + " {0}", cameraIndex + 1);
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

                //onValueUpdate = true;

                previewTypeToolStripButton.DropDownItems.Clear();

                foreach (string previewName in visionProbe.GetPreviewNames())
                {
                    var previewButton = new ToolStripButton(previewName);
                    previewButton.Click += PreviewToolStripButton_Click;
                    previewTypeToolStripButton.DropDownItems.Add(previewButton);
                }

                previewTypeToolStripButton.Text = previewTypeToolStripButton.DropDownItems[0].Text;

                //onValueUpdate = false;
            }

            CheckButton(multiShotToolStripButton, modellerPage.IsOnLive());

            if (OperationConfig.Instance().UseUserManager)
            {
                lockMoveToolStripButton.Visible = UserHandler.Instance.CurrentUser.IsAuth(ERoleType.TeachPage);
            }

            CheckButton(lockMoveToolStripButton, modellerPage.IsMoveLocked());
            CheckButton(previewToolStripButton, modellerPage.IsOnPreview());
        }

        private void CheckButton(ToolStripButton toolStripButton, bool flag)
        {
            toolStripButton.Checked = flag;
            toolStripButton.BackColor = (flag ? Color.LightGreen : Color.Transparent);
        }

        private void PreviewToolStripButton_Click(object sender, EventArgs e)
        {
            var previewButton = (ToolStripButton)sender;

            previewTypeToolStripButton.Text = previewButton.Text;

            int previewIndex = previewTypeToolStripButton.DropDownItems.IndexOf(previewButton);

            modellerPage.PreviewIndexChanged(previewIndex);
        }

        private void UpdateLightTypeCombo()
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
            modellerPage.LightTypeChanged(lightTypeIndex);

            selectLightButton.Text = selectLightButton.DropDownItems[lightTypeIndex].Text;
        }

        public void UpdateInspectStepButton(int stepNoSelected = 0)
        {

        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void addProbeToolStripButton_Click(object sender, EventArgs e)
        {

        }

        public string Title => "ModellerToolBar";

        public Control Control => this;

        public DockedLocation DockedLocation => DockedLocation.DockedTop;

        /*
        public void OnPreSelectedInspect()
        {
            
        }

        public void OnPostSelectedInspect(ProbeResultList probeResultList)
        {
            
        }

        public void StepChanged(InspectStep inspectStep)
        {
        }

        public void Clear()
        {

        }

        public void PointSelected(PointF point, ref bool processingCancelled)
        {

        }

        public void UpdateImage(Image2D sourceImage2d, int lightTypeIndex)
        {

        }

        public void SelectObject(List<ITeachObject> teachObjectList)
        {

        }
        */

        private void singleShotToolStripButton_Click(object sender, EventArgs e)
        {
            modellerPage.SingleShotButtonClicked();
        }

        private void multiShotToolStripButton_Click(object sender, EventArgs e)
        {
            modellerPage.ToggleLive();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            modellerPage.StartGrabProcess();
        }
    }
}
