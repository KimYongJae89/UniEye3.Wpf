using DynMvp.Base;
using DynMvp.Component.DepthSystem.DepthViewer;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Command;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class ModellerPage : UserControl, IMainTabPage, IModellerPage
    {
        private enum ETabPage
        {
            TabPageStep, TabPageCamera, TabPageProbe, TabPageView, TabPageMax
        }

        public SizeF TeachBoxSize => TeachPanel.Size;
        public DragMode DragMode { get; } = DragMode.Select;

        private CommandManager commandManager = new CommandManager();
        public PositionAligner PositionAligner { get; set; } = new PositionAligner();
        public TeachPanel TeachPanel { get; }

        private TeachHandler teachHandler;

        private IParamControl paramControl;
        private TryInspectionResultView2 tryInspectionResultView;
        private FovNavigator fovNavigator;
        private System.Windows.Forms.TabPage tabPageFovNavigator;
        private ILightParamForm lightParamForm;

        private ProbeResultList probeResultList = new ProbeResultList();
        private const int padding = 3;

        public AlgorithmValueChangedDelegate ValueChanged = null;

        private bool lockGrab = true;
        private bool onValueUpdate = false;
        private Image2D sourceImage2d;
        private int cameraIndex = 0;
        private int stepIndex = 0;
        private int lightTypeIndex = 0;
        private int previewIndex = 0;
        private bool fineMoveMode = false;
        private bool onFineMove = false;
        private bool onPreviewMode = false;
        private bool onLiveGrab = false;
        private bool lockMove = false;
        private ContextMenu objectContextMenu = new ContextMenu();
        private ModellerPageExtender modellerPageExtender;
        private IModellerToolbar modellerToolbar;
        private JoystickAxisForm joystick;
        private AxisPosition currentPosition = new AxisPosition();
        private CancellationTokenSource cancellationTokenSource;
        private PointF lastMousePos;
        public string AddObjectTypeStr { get; } = "";

        public ModellerPage()
        {
            InitializeComponent();

            tryInspectionResultView = new TryInspectionResultView2();
            TeachPanel = new TeachPanel();

            modellerToolbar = UiManager.Instance().CreateModellerToolbar();
            modellerToolbar.Initialize(this);
            panelTabMenu.Controls.Add((UserControl)modellerToolbar);

            paramControl = UiManager.Instance().CreateParamControl();
            paramControl.Init(teachHandler, commandManager, new AlgorithmValueChangedDelegate(ParamControl_ValueChanged));

            teachHandler = new TeachHandler();

            SuspendLayout();

            objectContextMenu.Popup += ObjectContextMenu_Popup;
            objectContextMenu.MenuItems.Add(new MenuItem("Select Target", OnClickSelectTarget));

            cameraImagePanel.Controls.Add(TeachPanel);

            TeachPanel.TeachHandler = teachHandler;
            TeachPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TeachPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            TeachPanel.Location = new System.Drawing.Point(3, 3);
            TeachPanel.Name = "targetImage";
            TeachPanel.Size = new System.Drawing.Size(409, 523);
            TeachPanel.TabIndex = 8;
            TeachPanel.TabStop = false;
            TeachPanel.Enable = true;
            TeachPanel.RotationLocked = true;

            TeachPanel.ChildMouseMove += TeachPanel_ChildMouseMove;
            TeachPanel.ChildMouseDown += TeachPanel_ChildMouseDown;
            TeachPanel.ChildMouseUp += TeachPanel_ChildMouseUp;

            TeachPanel.FigureDeleted += TeachPanel_FigureDeleted;
            TeachPanel.FigureCopied += TeachPanel_FigureCopied;
            TeachPanel.FigureCreated += TeachPanel_FigureCreated;
            TeachPanel.FigureSelected += TeachPanel_FigureSelected;
            TeachPanel.FigureModified += TeachPanel_FigureModified;

            TeachPanel.ChildMouseClick += TeachPanel_ChildMouseClicked;
            TeachPanel.ChildMouseDblClick += TeachPanel_ChildMouseDblClick;

            tabPageResult.Controls.Add(tryInspectionResultView);

            tryInspectionResultView.Location = new System.Drawing.Point(96, 95);
            tryInspectionResultView.Name = "tryInspectionResultView";
            tryInspectionResultView.Size = new System.Drawing.Size(74, 101);
            tryInspectionResultView.TabIndex = 0;
            tryInspectionResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            tryInspectionResultView.TeachHandlerProbe = teachHandler;
            tryInspectionResultView.TryInspectionResultCellClicked = UpdateImageFigure;

            paramContainer.Panel1.Controls.Add((UserControl)paramControl);

            lightParamForm = UiManager.Instance().CreateLightParamForm();

            ResumeLayout(false);

            ChangeCaption();
        }

        public void ChangeCaption()
        {
            inspectionButton.Text = StringManager.GetString(inspectionButton.Text);
            saveButton.Text = StringManager.GetString(saveButton.Text);
        }

        public TabKey TabKey => TabKey.Teach;

        public string TabName => "Teach";

        public Bitmap TabIcon => global::UniEye.Base.Properties.Resources.live_gray_36;

        public Bitmap TabSelectedIcon => global::UniEye.Base.Properties.Resources.live_white_36;

        public Color TabSelectedColor => Color.YellowGreen;

        public bool IsAdminPage => false;

        private StepModel CurrentModel => ModelManager.Instance().CurrentModel as StepModel;

        public Uri Uri => throw new NotImplementedException();

        public void PreviewIndexChanged(int previewIndex)
        {
            this.previewIndex = previewIndex;
            UpdateImage();
        }

        public void CameraIndexChanged(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;

            TeachPanel.ZoomFit();
            UpdatePage();
        }

        private void TeachBox_MouseClicked(DrawBox senderView, Point clickPos, ref bool processingCancelled)
        {
            paramControl.PointSelected(clickPos, ref processingCancelled);
        }

        private void TeachPanel_FigureDeleted(List<Figure> figureList)
        {
            ParamControl_ValueChanged(ValueChangedType.ImageProcessing, null, null, true);
        }

        private void TeachPanel_FigureCopied(List<Figure> figureList)
        {

        }

        private void TeachPanel_FigureCreated(Figure figure, CoordMapper coordMapper, FigureGroup worksingFigures, FigureGroup backgroundFigures)
        {
            InspectStep inspectionStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectionStep == null)
            {
                return;
            }

            RotatedRect figureRect = figure.GetRectangle();
            ITeachObject teachObject = CreateObject(figureRect);

            var option = new CanvasPanel.Option();
            option.IncludeProbe = true;
            option.ShowProbeNumber = UiConfig.Instance().ShowNumber;

            Target target = null;
            target = new Target();

            inspectionStep.AddTarget(target);
            target.Add(teachObject);

            target.UpdateRegion();

            var teachObjectList = new List<ITeachObject>();
            teachObjectList.Add(teachObject);

            paramControl.SelectObject(teachObjectList);

            UpdatePage();
        }

        private ITeachObject CreateObject(RotatedRect rect)
        {
            var objectCreator = new ObjectCreator(this);
            ITeachObject teachObject = null;
            switch (AddObjectTypeStr)
            {
                case PatternMatching.TypeName:
                    teachObject = objectCreator.CreatePatternMatching(rect);
                    break;
                default:
                    teachObject = objectCreator.CreateVisionProbe(AddObjectTypeStr, rect);
                    break;
            }

            return teachObject;
        }

        private void TeachPanel_FigureModified(List<Figure> figureList)
        {
            UpdatePage();
        }

        private void OnClickSelectTarget(object sender, EventArgs e)
        {
            List<Target> targetList = teachHandler.GetTargetList();

            UpdateImageFigure();

            TeachPanel.ClearSelection();

            foreach (Target target in targetList)
            {
                teachHandler.Select(target);
                TeachPanel.SelectFigureByTag(target);
            }
        }

        private void ObjectContextMenu_Popup(object sender, EventArgs e)
        {
            objectContextMenu.MenuItems[0].Enabled = teachHandler.IsSelected();
        }

        private void ModellerPage_Load(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage_Load");

            cameraIndex = 0;

            lightParamForm.InitControls();

            lockGrab = false;

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            Camera camera = cameraHandler.GetCamera(0);
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            if (robotStage != null)
            {
                joystick = new JoystickAxisForm(robotStage);
                joystick.Camera = camera;
                joystick.MovableCheck = Joystick_MovableCheck;
            }

            modellerPageExtender = UiManager.Instance().CreateModellerPageExtender();

            if (robotStage != null)
            {
                tabPageFovNavigator = new TabPage();

                tabControlUtil.Controls.Add(tabPageFovNavigator);

                tabPageFovNavigator.Location = new System.Drawing.Point(4, 29);
                tabPageFovNavigator.Name = "tabPageFovNavigator";
                tabPageFovNavigator.Padding = new System.Windows.Forms.Padding(3);
                tabPageFovNavigator.Size = new System.Drawing.Size(490, 96);
                tabPageFovNavigator.TabIndex = 0;
                tabPageFovNavigator.Text = "FOV Navigator";
                tabPageFovNavigator.UseVisualStyleBackColor = true;

                fovNavigator = new FovNavigator();

                tabPageFovNavigator.Controls.Add(fovNavigator);

                fovNavigator.Location = new System.Drawing.Point(96, 95);
                fovNavigator.Name = "fovNavigator";
                fovNavigator.Size = new System.Drawing.Size(74, 101);
                fovNavigator.TabIndex = 0;
                fovNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
                fovNavigator.RobotStage = robotStage;
                fovNavigator.Enable = true;
                fovNavigator.FovChanged += new FovChangedDelegate(FovNavigator_FovChanged);

                fovNavigator.FovSize = camera.FovSize;
            }

            TeachPanel.LockMoveFigure(true);

            LogHelper.Debug(LoggerType.StartUp, "End ModellerPage_Load");
        }

        private bool Joystick_MovableCheck()
        {
            return true;
        }

        private void FovNavigator_FovChanged(int fovNo, PointF position)
        {
            if (InvokeRequired)
            {
                Invoke(new FovChangedDelegate(FovNavigator_FovChanged), fovNo, position);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, string.Format("Change FOV - {0}", fovNo + 1));

            if (fovNo > -1)
            {
                InspectStep inspectStepSel = CurrentModel?.GetInspectStep(fovNo, true);
                Debug.Assert(inspectStepSel != null, "Invalid Model Format. InspectionStep must have value");

                if (inspectStepSel != null)
                {
                    modellerToolbar.SelectInspectStep(inspectStepSel.StepNo);
                }
            }
            else
            {
                DeviceManager.Instance().RobotStage?.Move(new AxisPosition(position.X, position.Y));
            }
        }

        public void Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize");

            modellerToolbar.Initialize(this);

            LogHelper.Debug(LoggerType.StartUp, "End ModellerPage::Initlaize");
        }

        private void UpdateImageFigure()
        {
            TeachPanel.UpdateCenterGuide(UiConfig.Instance().ShowCenterGuide,
                            UiConfig.Instance().CenterGuidePos, UiConfig.Instance().CenterGuideThickness);

            InspectStep inspectionStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectionStep == null)
            {
                return;
            }

            TeachPanel.UpdateFigure(inspectionStep, cameraIndex, PositionAligner, null);
        }

        public void Teach()
        {

        }

        public void Scan()
        {

        }

        private void TeachPanel_FigureSelected(List<Figure> figureList, bool select = true)
        {
            if (InvokeRequired)
            {
                Invoke(new FigureSelectedDelegate(TeachPanel_FigureSelected), figureList, select);
                return;
            }

            paramControl.SelectObject(teachHandler.SelectedObjs);

            lightTypeIndex = teachHandler.GetLightTypeIndex()[0];

            UpdatePage();

            modellerToolbar.UpdateButtonState(teachHandler, cameraIndex, lightTypeIndex);
        }

        private void UpdateLightType(int lightTypeIndex)
        {
            this.lightTypeIndex = lightTypeIndex;

            UpdatePage();
        }

        public void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateData");

            onValueUpdate = true;

            bool itemSelected = teachHandler.IsSelected();
            if (itemSelected == false)
            {
                ClearProbeData();
            }
            else
            {
                paramControl.SelectObject(teachHandler.SelectedObjs);
            }

            modellerToolbar.UpdateButtonState(teachHandler, cameraIndex, lightTypeIndex);

            onValueUpdate = false;
        }

        public void ClearProbeData()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ClearProbeData");

            TeachPanel.ClearSelection();
            paramControl.ClearProbeData();
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm = null, AlgorithmParam newParam = null, bool modified = true)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ParamControl_ValueChanged");

            Debug.Assert(CurrentModel != null);

            if (onValueUpdate == false)
            {
                switch (valueChangedType)
                {
                    case ValueChangedType.Position:
                        UpdateImageFigure();
                        break;
                    case ValueChangedType.ImageProcessing:
                        UpdateImage();
                        break;
                    case ValueChangedType.Light:
                        ITeachObject teachObject = teachHandler.GetSingleSelected();
                        if (teachObject != null)
                        {
                            if (teachObject is VisionProbe)
                            {
                                UpdateLightType(((VisionProbe)teachObject).LightTypeIndexArr[0]);

                                lightTypeIndex = ((VisionProbe)teachObject).PreveiewLightTypeIndex;
                                UpdatePage();
                            }
                        }
                        break;
                }

                if (valueChangedType != ValueChangedType.None)
                {
                    CurrentModel.Modified = true;
                }

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, modified);
            }
        }

        public void ToggleLockMove()
        {
            lockMove = !lockMove;
            TeachPanel.LockMoveFigure(lockMove);
        }

        public bool IsMoveLocked()
        {
            return lockMove;
        }

        public void ToggleLive()
        {
            if (onLiveGrab)
            {
                StopLive();
            }
            else
            {
                StartLive();
            }
        }

        public void StartLive()
        {
            onLiveGrab = true;

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            Camera camera = cameraHandler.GetCamera(cameraIndex);
            if (camera == null)
            {
                return;
            }

            TeachPanel.StartLive(camera);
        }

        public void StopLive()
        {
            onLiveGrab = false;

            TeachPanel.StopLive();
        }

        public bool IsOnLive()
        {
            return onLiveGrab;
        }

        private delegate void UpdateImageDelegate();
        private void Inspect()
        {
            InspectStep inspectionStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectionStep == null)
            {
                return;
            }

            tryInspectionResultView.ClearResult();

            Calibration curCameraCalibration = SystemManager.Instance().GetCameraCalibration(cameraIndex);

            ImageBuffer imageBuffer = DeviceManager.Instance().ImageBufferPool.GetObject();

            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            imageAcquisition.Acquire(imageBuffer, inspectionStep.GetLightParamSet(), inspectionStep.GetLightTypeIndexArr());

            probeResultList.Clear();

            TeachPanel.Inspect(inspectionStep, imageBuffer, true, cameraIndex, curCameraCalibration, probeResultList, null);

            UpdateImageFigure();

            tryInspectionResultView.SetResult(probeResultList);
        }

        public void Delete()
        {
            if (teachHandler.IsSelected() == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - Delete");

            teachHandler.DeleteObject();
            TeachPanel.ClearSelection();
            UpdateImageFigure();

            ParamControl_ValueChanged(ValueChangedType.ImageProcessing, null, null, true);
        }

        public VisionProbe CreateVisionProbe(RotatedRect rect = new RotatedRect())
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateVisionProbe");

            if (rect == null)
            {
                rect = GetDefaultProbeRegion();
                if (rect.IsEmpty)
                {
                    return null;
                }
            }

            var visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);
            visionProbe.BaseRegion = rect;

            return visionProbe;
        }

        public ComputeProbe CreateComputeProbe()
        {
            var computeProbe = (ComputeProbe)ProbeFactory.Create(ProbeType.Compute);

            return computeProbe;
        }

        public DaqProbe CreateDaqProbe()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateDaqProbe");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
            {
                return null;
            }

            var daqProbe = (DaqProbe)ProbeFactory.Create(ProbeType.Daq);
            daqProbe.BaseRegion = rect;

            return daqProbe;
        }

        public RotatedRect GetDefaultProbeRegion()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetDefaultProbeRegion");

            var rectangle = new Rectangle(0, 0, sourceImage2d.Width, sourceImage2d.Height);

            float centerX = sourceImage2d.Width / 2;
            float centerY = sourceImage2d.Height / 2;

            float width = sourceImage2d.Width / 4;
            float height = sourceImage2d.Height / 4;

            float left = centerX - width / 2;
            float top = centerY - height / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        private bool Is3dAlgorithm(Algorithm algorithm)
        {
            if (algorithm is DepthChecker)
            {
                return true;
            }

            return false;
        }

        public void AddProbe(Probe probe)
        {
            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            var newTarget = new Target();
            inspectStep.AddTarget(newTarget);

            commandManager.Execute(new AddProbeCommand(newTarget, probe, PositionAligner));

            var probeList = new ProbeList();
            probeList.AddProbe(probe);

            ProbeAdded(probeList);
        }

        private void TargetAdded(List<Target> targetList)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ProbeAdded");

            UpdateImageFigure();

            foreach (Target target in targetList)
            {
                teachHandler.Select(target);
                TeachPanel.SelectFigureByTag(target);
            }

            ParamControl_ValueChanged(ValueChangedType.None, null, null, true);
        }

        public void OnIdle()
        {
            modellerToolbar.UpdateButtonState(teachHandler, cameraIndex, lightTypeIndex);
        }

        private void ProbeAdded(ProbeList probeList)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ProbeAdded");

            var teachObjectList = new List<ITeachObject>();
            teachHandler.Select(teachObjectList);
            paramControl.SelectObject(teachObjectList);

            ParamControl_ValueChanged(ValueChangedType.None, null, null, true);

            UpdateImageFigure();
        }

        public Image2D GetClipImage(RotatedRect clipRegion)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetClipImage");

            return (Image2D)sourceImage2d.ClipImage(clipRegion);
        }

        public void Copy()
        {
            var objList = new List<ICloneable>();
            foreach (ITeachObject teachObject in teachHandler.SelectedObjs)
            {
                objList.Add((ICloneable)teachObject.Clone());
            }

            CopyBuffer.SetData(objList);
        }

        public void Paste()
        {
            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            List<Target> targetList = CopyBuffer.GetTargetList();

            var newTargetList = new List<Target>();

            teachHandler.Clear();

            foreach (Target srcTarget in targetList)
            {
                try
                {
                    RotatedRect baseRegion = srcTarget.BaseRegion;

                    var newTarget = (Target)srcTarget.Clone();

                    inspectStep.AddTarget(newTarget);

                    newTargetList.Add(newTarget);

                    baseRegion.Offset(10, 10);

                    newTarget.UpdateRegion(baseRegion);
                }
                catch (InvalidCastException)
                {

                }
            }

            TargetAdded(newTargetList);
        }

        public void UpdateImage()
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Invoke UpdateImage");

                Invoke(new UpdateImageDelegate(UpdateImage));
                return;
            }

            sourceImage2d = SystemManager.Instance().ImageSequence.GetImage(cameraIndex, stepIndex, lightTypeIndex);

            if (sourceImage2d != null)
            {
                var cloneImage = (Image2D)sourceImage2d.Clone();

                List<Probe> probeList = teachHandler.GetSelectedProbe();
                foreach (Probe probe in probeList)
                {
                    if (probe != null && onPreviewMode)
                    {
                        probe.PreviewFilterResult(probe.BaseRegion, cloneImage, previewIndex);
                    }
                }

                TeachPanel.UpdateImage(cloneImage);

                paramControl.UpdateTargetGroupImage(sourceImage2d, lightTypeIndex);
            }
        }

        public void StopGrab()
        {
            modellerPageExtender.StopGrab();
        }

        public void ModelPropertyButtonClicked()
        {
            //ModelForm editModelForm = new ModelForm();
            ModelForm editModelForm = UiManager.Instance().CreateModelForm();
            editModelForm.ModelFormType = ModelFormType.Edit;
            editModelForm.ModelDescription = ModelManager.Instance().CurrentModel.ModelDescription;
            if (editModelForm.ShowDialog(this) == DialogResult.OK)
            {
                ModelManager.Instance().EditModel(editModelForm.ModelDescription);
            }
        }

        public void ExportFormatButtonClicked()
        {
            var systemManager = SystemManager.Instance();
            var form = new OutputFormatForm();
            form.Model = ModelManager.Instance().CurrentStepModel;
            if (form.ShowDialog() == DialogResult.OK)
            {
                ModelManager.Instance().SaveModelDescription(ModelManager.Instance().CurrentModel.ModelDescription);
            }
        }

        public string EditStepButtonClicked(int stepIndex)
        {
            if (!(ModelManager.Instance().CurrentModel is StepModel stepModel))
            {
                return "";
            }

            InspectStep inspectStep = stepModel.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return "";
            }

            var form = new InspectStepForm();
            form.Initialize(inspectStep);
            if (form.ShowDialog() == DialogResult.OK)
            {
                return inspectStep.GetStepName();
            }

            return "";
        }

        private void ComboBoxPreviewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - comboBoxPreviewType_SelectedIndexChanged");
            UpdateImage();
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                //Delete();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Inspect();
            }
            else if (e.KeyCode == Keys.L)
            {
                //ToggleViewLabel();
            }
        }

        private void ButtonBatchInspect_Click(object sender, EventArgs e)
        {

        }

        public void LightTypeChanged(int lightTypeIndex)
        {
            this.lightTypeIndex = lightTypeIndex;

            UpdatePage();
            UpdateImageFigure();
        }

        private void LightParamPanel_LightValueChanged(bool imageUpdateRequired)
        {
            if (imageUpdateRequired == true)
            {
                GrabImage();
                UpdateImage();
            }

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public void UpdatePage()
        {
            var curModel = (StepModel)ModelManager.Instance().CurrentModel;
            if (curModel == null)
            {
                return;
            }

            InspectStep inspectionStep = curModel.GetInspectStep(stepIndex);
            if (inspectionStep == null)
            {
                return;
            }

            lightParamForm.SetLightValues(curModel, inspectionStep, cameraIndex);

            modellerToolbar.UpdateButtonState(teachHandler, cameraIndex, lightTypeIndex);

            UpdateImage();
            UpdateImageFigure();

            if (sourceImage2d != null)
            {
                teachHandler.Boundary = new Rectangle(0, 0, sourceImage2d.Width, sourceImage2d.Height);
            }
        }

        private bool GrabImage()
        {
            if (Visible == false)
            {
                return false;
            }

            if (lockGrab == true)
            {
                return false;
            }

            if (!(ModelManager.Instance().CurrentModel is StepModel stepModel))
            {
                return false;
            }

            InspectStep curInspectStep = stepModel.GetInspectStep(stepIndex);
            if (curInspectStep == null)
            {
                return false;
            }

            LightParamSet lightParamSet = curInspectStep.GetLightParamSet();
            LightParam lightParam = lightParamSet[lightTypeIndex];

            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(cameraIndex);
            if (camera == null)
            {
                return false;
            }

            string imagePath = Path.Combine(ModelManager.Instance().CurrentModel.ModelPath, "Image");
            if (Directory.Exists(imagePath) == false)
            {
                Directory.CreateDirectory(imagePath);
            }

            try
            {
                modellerPageExtender.GrabImage(stepIndex, cameraIndex, lightTypeIndex, lightParam);
            }
            catch (AlarmException ex)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Grabber, 0, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), "Grabber Grab Fail", ex.Message);
            }

            return true;
        }

        private void PasteProbeButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        public void ZoomInButtonClicked()
        {
            TeachPanel.ZoomIn();
        }

        public void ZoomOutButtonClicked()
        {
            TeachPanel.ZoomOut();
        }

        public void StartGrabProcess()
        {
            if (!(ModelManager.Instance().CurrentModel is StepModel stepModel))
            {
                return;
            }

            InspectStep curInspectStep = stepModel.GetInspectStep(stepIndex);
            if (curInspectStep == null)
            {
                return;
            }

            LightParamSet lightParamSet = curInspectStep.GetLightParamSet();

            modellerPageExtender.GrabImage(stepIndex, cameraIndex, lightTypeIndex, lightParamSet[lightTypeIndex]);

            UpdatePage();
        }

        public void ShowLightPanelButtonClicked()
        {
            if (LightConfig.Instance().NumLightType == 0)
            {
                return;
            }

            lightParamForm.ToggleVisible();
        }

        public void LoadImageSetButtonClicked()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SystemManager.Instance().ImageSequence.SetImagePath(dialog.SelectedPath);
                UpdateImage();
            }
        }

        public void SingleShotButtonClicked()
        {
            GrabImage();
            UpdateImage();
        }

        public void ZoomFitButtonClicked()
        {
            TeachPanel.ZoomFit();
        }

        public void SyncParamButtonClicked()
        {
            ITeachObject teachObject = teachHandler.GetSingleSelected();
            if (teachObject != null)
            {
                if (teachObject is Probe probe)
                {

                    //IProbeFilter probeFilter;
                    //if (modellerPageExtender.GetProbeFilter(probe, out probeFilter) == true)
                    //{
                    //    StepModel curModel = ModelManager.Instance().CurrentModel as StepModel;

                    //    foreach (InspectStep inspectStep in curModel.InspectStepList)
                    //    {
                    //        inspectStep.SyncParam(probe, (IProbeFilter)probeFilter);
                    //    }
                    //}
                }
            }
        }

        public void SyncAllButtonClicked()
        {
            if (!(ModelManager.Instance().CurrentModel is StepModel stepModel))
            {
                return;
            }

            InspectStep curinspectionStep = stepModel.GetInspectStep(stepIndex);

            foreach (InspectStep inspectStep in stepModel.InspectStepList)
            {
                inspectStep.Copy(curinspectionStep);
            }
        }

        public void CopyButtonClicked()
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Copy target");
            Copy();
        }

        public void PasteButtonClicked()
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Paste target");
            Paste();
        }

        public void DeleteButtonClicked()
        {
            if (MessageForm.Show(ParentForm, StringManager.GetString("Do you want to delete selected probe?"), MessageFormType.YesNo) == DialogResult.No)
            {
                return;
            }

            Delete();
        }

        private void InspectionButton_Click(object sender, EventArgs e)
        {
            Inspect();
        }

        public void SetFiducialButtonClicked()
        {
            Probe selectedProbe = teachHandler.GetSingleSelectedProbe();
            if (selectedProbe != null)
            {
                if (selectedProbe is VisionProbe selectedVisionProbe)
                {
                    if (selectedVisionProbe.InspAlgorithm is Searchable)
                    {
                        selectedProbe.Target.SetFiducialProbe(selectedVisionProbe);
                    }
                }
            }
        }

        private void UpdateFovNavigator(int stepIndex)
        {
            if (fovNavigator == null)
            {
                return;
            }

            fovNavigator.ClearFovList();

            foreach (InspectStep inspectStep in ModelManager.Instance().CurrentStepModel)
            {
                if (inspectStep.NumTargets == 0)
                {
                    continue;
                }

                if (inspectStep.Position == null)
                {
                    continue;
                }

                Figure figure = fovNavigator.AddFovFigure(inspectStep.Position.ToPointF());

                figure.Tag = inspectStep.StepNo;
            }

            fovNavigator.SelectFov(stepIndex);

            fovNavigator.Invalidate();
        }

        public InspectStep CreateInspectionStep()
        {
            var stepModel = (StepModel)ModelManager.Instance().CurrentModel;

            InspectStep inspectStep = stepModel.CreateInspectionStep();
            inspectStep.Position.SetPosition(currentPosition.Position);

            UpdateFovNavigator(inspectStep.StepNo);

            UpdatePage();

            if (fovNavigator != null)
            {
                fovNavigator.SelectFigureByTag(inspectStep);
            }

            ModelManager.Instance().CurrentModel.Modified = true;

            return inspectStep;
        }

        public void DeleteStepButtonClicked()
        {
            if (stepIndex >= 0)
            {
                if (MessageForm.Show(ParentForm, StringManager.GetString("Do you want to delete selected step?"), MessageFormType.YesNo) == DialogResult.No)
                {
                    return;
                }

                var stepModel = (StepModel)ModelManager.Instance().CurrentModel;

                InspectStep inspectStep = stepModel.GetInspectStep(stepIndex);
                stepModel.RemoveInspectStep(inspectStep);

                int newStepIndex = stepIndex - 1;
                if (newStepIndex < 0)
                {
                    newStepIndex = 0;
                }

                modellerToolbar.UpdateInspectStepButton(newStepIndex);

                UpdatePage();

                stepModel.Modified = true;
            }
        }

        public void GroupButtonClicked()
        {
            if (teachHandler.IsSelected() == false)
            {
                return;
            }

            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            var newTarget = new Target();

            List<Target> targetList = teachHandler.GetTargetList();
            foreach (Target target in targetList)
            {
                newTarget.AddProbe(target.ProbeList);
                inspectStep.RemoveTarget(target);
            }

            newTarget.UpdateRegion();

            inspectStep.AddTarget(newTarget);

            UpdateImageFigure();

            TeachPanel.ClearSelection();

            teachHandler.Select(newTarget);
            TeachPanel.SelectFigureByTag(newTarget);
        }

        public void UngroupButtonClicked()
        {
            if (teachHandler.IsSingleSelected() == false)
            {
                return;
            }

            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            List<Target> targetList = teachHandler.GetTargetList();
            foreach (Target target in targetList)
            {
                foreach (Probe probe in target.ProbeList)
                {
                    var newTarget = new Target();
                    newTarget.AddProbe(probe);

                    newTarget.UpdateRegion();

                    inspectStep.AddTarget(newTarget);
                }

                inspectStep.RemoveTarget(target);
            }
        }

        private void Grab3dToolStripButton_Click(object sender, EventArgs e)
        {
            // depthScannerHandler.Scan
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - SaveToolStripButton_Click");
            if (ModelManager.Instance().CurrentModel != null)
            {
                ModelManager.Instance().CurrentModel.SaveModel();
            }
        }

        public void RobotSettingButtonClicked()
        {
            var motionSpeedForm = new MotionSpeedForm();
            motionSpeedForm.Intialize();
            motionSpeedForm.ShowDialog(this);
        }

        public void JoystickButtonClicked()
        {
            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            joystick.ToggleView(this, inspectStep.GetLightParamSet());
        }

        public void OriginButtonClicked()
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            if (robotStage != null)
            {
                cancellationTokenSource = new CancellationTokenSource();

                var loadingForm = new SimpleProgressForm("Move to origin");
                loadingForm.Show(new Action(() => robotStage.HomeMove(cancellationTokenSource.Token)), cancellationTokenSource);
            }
        }

        public void AlignPosition()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var loadingForm = new SimpleProgressForm("Align");
            loadingForm.Show(new Action(AlignPositionProc), cancellationTokenSource);

            InspectStepChanged(0);
        }

        private void AlignPositionProc()
        {
            EnableForm(false);

            var fiducialSet = new FiducialSet();
            StepModel stepModel = ModelManager.Instance().CurrentStepModel;
            stepModel.GetGlobalFiducialStep(fiducialSet);

            if (fiducialSet.Valid == false)
            {
                return;
            }

            // Fiducial 검사로 위치 정렬
            var inspectRunner = new SingleTriggerInspectRunner();
            inspectRunner.InspectRunnerExtender = new InspectRunnerExtender();
            inspectRunner.EnterWaitInspection(fiducialSet.CreateStepModel());
            inspectRunner.Inspect();

            PositionAligner = fiducialSet.Calculate(inspectRunner.ProductResult);

            if (PositionAligner != null)
            {
                DisplayAlignInfo(PositionAligner);
            }

            EnableForm(true);
        }

        public delegate void DisplayAlignInfoDelegate(PositionAligner positionAligner);
        public void DisplayAlignInfo(PositionAligner positionAligner)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayAlignInfoDelegate(DisplayAlignInfo), positionAligner);
                return;
            }

            textBoxDesiredDistance.Text = positionAligner.DesiredFiducialDistance.ToString();
            textBoxFidDistance.Text = positionAligner.FiducialDistance.ToString();
            fidOffset.Text = positionAligner.FiducialDistanceDiff.ToString();
            fidAngle.Text = positionAligner.Angle.ToString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ModelManager.Instance().CurrentModel.SaveModel();

            MessageForm.Show((Form)UiManager.Instance().MainForm, StringManager.GetString("Save OK"), "UniEye");
        }

        public void InspectStepChanged(int stepIndex)
        {
            this.stepIndex = stepIndex;

            TeachPanel.ClearSelection();

            // Robot move...
            InspectStep inspectStep = ModelManager.Instance().CurrentStepModel.GetInspectStep(stepIndex);

            if (inspectStep != null && inspectStep.Position != null
                && DeviceManager.Instance().RobotStage != null)
            {
                DeviceManager.Instance().RobotStage.Move(inspectStep.Position);
            }

            int[] lightTypeIndexArr = inspectStep.GetLightTypeIndexArr(cameraIndex);
            lightTypeIndex = lightTypeIndexArr[0];

            UpdatePage();

            if (fovNavigator != null)
            {
                UpdateFovNavigator(stepIndex);
            }
        }

        public void EditSchemaButtonClicked()
        {
            var modelSchemaEditor = new SchemaEditor();
            modelSchemaEditor.Initialize(ModelManager.Instance().CurrentStepModel);
            modelSchemaEditor.ShowDialog(this);
        }

        public void CreateSchemaButtonClicked()
        {
            RectangleF unionRect = RectangleF.Empty;

            StepModel model = ModelManager.Instance().CurrentStepModel;
            var schema = new Schema();

            Enabled = false;

            var cancellationTokenSource = new CancellationTokenSource();

            var loadingForm = new SimpleProgressForm("Generating Schema");
            loadingForm.Show(new Action(() =>
            {
                try
                {
                    List<InspectStep> inspectStepList = model.GetInspectStepList();

                    InspectStep inspectionStepPrev = null;
                    for (int i = 0; i < inspectStepList.Count; i++)
                    {
                        InspectStep inspectStep = inspectStepList[i];
                        RectangleF rectangle = inspectStep.FovRect;

                        if (unionRect == RectangleF.Empty)
                        {
                            unionRect = rectangle;
                        }
                        else
                        {
                            unionRect = RectangleF.Union(rectangle, unionRect);
                        }

                        inspectStep.AddSchemaFigure(schema);

                        inspectionStepPrev = inspectStep;

                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
                catch (OperationCanceledException)
                {

                }
            }), cancellationTokenSource);

            Enabled = true;
            loadingForm.Close();

            schema.Region = unionRect;
            schema.InvertY = true;
            model.ModelSchema = schema;

            model.UnlinkSchemaFigures();

            ModelManager.Instance().CurrentModel.Modified = true;
        }

        private void AddSchemaFigure(Schema schema, AxisPosition alignedPosition1, AxisPosition alignedPosition2)
        {
            if (alignedPosition1.NumAxis != alignedPosition2.NumAxis)
            {
                return;
            }

            var src = new PointF(alignedPosition1[0], alignedPosition1[1]);
            var dst = new PointF(alignedPosition2[0], alignedPosition2[1]);
            var figure = new LineFigure(src, dst, new Pen(Color.Blue, 1));
            figure.Name = "MovingPath";
            schema.AddFigure(figure);
        }

        public void ToggleFineMove()
        {
            fineMoveMode = !fineMoveMode;
        }

        public bool IsOnFineMove()
        {
            return fineMoveMode;
        }

        private void TeachPanel_ChildMouseDown(PointF point, ref bool processingCancelled)
        {
            if (fineMoveMode)
            {
                lastMousePos = point;
                onFineMove = true;

                processingCancelled = true;
            }
        }

        private void TeachPanel_ChildMouseMove(PointF point, ref bool processingCancelled)
        {
            if (onFineMove)
            {
                var dragOffset = new SizeF(point.X - lastMousePos.X, point.Y - lastMousePos.Y);

                CoordMapper coordMapper = TeachPanel.GetCoordMapper();
                SizeF offsetF = coordMapper.PixelToWorld(dragOffset);

                DeviceManager.Instance().RobotStage.RelativeMove(new AxisPosition(offsetF.Width, offsetF.Height));
                TeachPanel.Invalidate();

                processingCancelled = true;
            }
        }

        private void TeachPanel_ChildMouseUp(PointF point, ref bool processingCancelled)
        {
            if (onFineMove)
            {
                fineMoveMode = onFineMove = false;
                processingCancelled = true;
            }
        }

        private void TeachPanel_ChildMouseClicked(UserControl sender, PointF point, MouseButtons button, ref bool processingCancelled)
        {

        }

        private void TeachPanel_ChildMouseDblClick(UserControl sender)
        {

        }

        public void TogglePreview()
        {
            onPreviewMode = !onPreviewMode;

            if (onPreviewMode == true)
            {
                buttonPreview.BackColor = Color.Green;
            }
            else
            {
                buttonPreview.BackColor = Color.Transparent;
            }

            UpdateImage();
        }

        public bool IsOnPreview()
        {
            return onPreviewMode;
        }

        public void UndoButtonClicked()
        {
            commandManager.Undo();
        }

        public void RedoButtonClicked()
        {
            commandManager.Redo();
        }

        private void ButtonPreview_Click(object sender, EventArgs e)
        {
            TogglePreview();
        }

        private void UpdateFovNavigator(AxisPosition robotPosition)
        {
            if (fovNavigator == null)
            {
                return;
            }
        }

        public void AlignButtonClicked()
        {
            panelAlign.Visible = !panelAlign.Visible;
        }

        public delegate void EnableFormDelegate(bool flag);
        public void EnableForm(bool flag)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableFormDelegate(EnableForm), flag);
                return;
            }
            Enabled = flag;
        }

        private void ButtonCloseAlignPanel_Click(object sender, EventArgs e)
        {
            //StepModel stepModel = ModelManager.Instance().CurrentStepModel;

            //stepModel.ModelDescription.FidDistanceTol = Convert.ToInt32(fidDistanceTol.Value);
            //ModelManager.Instance().SaveModelDescription(stepModel.ModelDescription);

            panelAlign.Visible = false;
        }

        private void ButtonAlign_Click(object sender, EventArgs e)
        {
            ModelManager.Instance().CurrentStepModel.CleanImage();

            AlignPosition();
        }

        private void NumericUpDownDistanceOffset_ValueChanged(object sender, EventArgs e)
        {
            //StepModel stepModel = ModelManager.Instance().CurrentStepModel;
            //stepModel.ModelDescription.FidDistanceTol = Convert.ToInt32(fidDistanceTol.Value);
        }

        public void ScanButtonClicked()
        {
            ModelManager.Instance().CurrentStepModel.CleanImage();

            cancellationTokenSource = new CancellationTokenSource();
            var loadingForm = new SimpleProgressForm("Scan");
            loadingForm.Show(new Action(ScanProc), cancellationTokenSource);
        }

        private void ScanProc()
        {
            EnableForm(false);

            string imagePath = ModelManager.Instance().GetImagePath();

            SystemManager.Instance().InspectRunner.Scan(0, imagePath);

            EnableForm(true);
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == false)
            {
                StopLive();
                StopGrab();

                ModelManager.Instance().CurrentModel?.SaveModel();
            }
            else
            {
                modellerToolbar.UpdateInspectStepButton(0);

                lockMove = true;
                TeachPanel.LockMoveFigure(true);

                string imagePath = ModelManager.Instance().GetImagePath();
                SystemManager.Instance().ImageSequence.SetImagePath(imagePath);

                UpdatePage();
            }
        }

        public Probe SearchProbeButtonClicked()
        {
            var form = new InputForm("Probe Id");
            if (form.ShowDialog() == DialogResult.OK)
            {
                StepModel curModel = ModelManager.Instance().CurrentStepModel;

                Probe probe = curModel?.GetProbe(form.InputText);
                if (probe != null)
                {
                    TeachPanel.SelectFigureByTag(probe);
                    TeachPanel.Invalidate(true);

                    return probe;
                }
            }

            return null;
        }

        public void SelectModeClicked()
        {
        }

        public void PanningModeClicked()
        {
        }

        public void ShowCenterLineButtonClicked(bool showCenterLine)
        {

        }

        public void AddModeClicked(string addObjectTypeStr)
        {

        }

        public void ZoomModeClicked()
        {

        }

        public void PreviewModeChanged(bool previewMode)
        {
            onPreviewMode = previewMode;
        }

        public void WholeImageModeChanged(bool wholeImageMode)
        {

        }

        public void ShowNumberButtonClicked(bool showNumber)
        {

        }

        public void ModifyTeaching(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void UpdateState()
        {

        }

        public void PrevImageButtonClicked()
        {
            SystemManager.Instance().ImageSequence.MovePrev();
        }

        public void NextImageButtonClicked()
        {
            SystemManager.Instance().ImageSequence.MoveNext();
        }
    }
}
