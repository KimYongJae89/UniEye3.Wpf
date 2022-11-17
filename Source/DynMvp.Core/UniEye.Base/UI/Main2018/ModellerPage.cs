using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Command;
using UniEye.Base.Config;
using UniEye.Base.UI.Main;

namespace UniEye.Base.UI.Main2018
{
    public partial class ModellerPage : UserControl, IMainTabPage, IModellerPage
    {
        private CommandManager commandManager = new CommandManager();
        private PositionAligner positionAligner = new PositionAligner();

        private ITeachPanel teachPanel;
        private TeachHandler teachHandler;
        private ILightParamForm lightParamForm;

        private ProbeResultList probeResultList = new ProbeResultList();
        private const int padding = 3;

        public AlgorithmValueChangedDelegate ValueChanged = null;
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
        private List<IModellerPane> modellerPaneList;
        private DockAreaPane dockAreaPaneRight;
        private DockAreaPane dockAreaPaneLeft;
        private DockAreaPane dockAreaPaneTop;
        private WindowDockingArea windowDockingAreaRight;
        private WindowDockingArea windowDockingAreaLeft;
        private WindowDockingArea windowDockingAreaTop;
        public string AddObjectTypeStr { get; } = "";

        public ModellerPage()
        {
            InitializeComponent();

            AddDockAreaPane();
            AddWindowDockingArea();

            teachPanel = UiManager.Instance().CreateTeachPanel();

            modellerToolbar = UiManager.Instance().CreateModellerToolbar();
            modellerToolbar.Initialize(this);
            Controls.Add((UserControl)modellerToolbar);
            AddModellerPane(modellerToolbar);

            teachHandler = new TeachHandler();

            SuspendLayout();

            objectContextMenu.Popup += ObjectContextMenu_Popup;
            objectContextMenu.MenuItems.Add(new MenuItem("Select Target", OnClickSelectTarget));

            AddTeachPanel();

            lightParamForm = UiManager.Instance().CreateLightParamForm();

            modellerPaneList = UiManager.Instance().CreateModellerPane();
            foreach (IModellerPane modellerPane in modellerPaneList)
            {
                AddModellerPane(modellerPane);
                if (modellerPane is ParamPanel)
                {
                    ((ParamPanel)modellerPane).Init(teachHandler, commandManager, ParamControl_ValueChanged);
                }
            }

            ResumeLayout(false);
            PerformLayout();

            //ultraDockManager.LoadComponentSettings();
            //ultraDockManager.SaveSettings = true;
        }

        ~ModellerPage()
        {
            //ultraDockManager.SaveComponentSettings();
        }

        private void AddTeachPanel()
        {
            cameraImagePanel.Controls.Add(teachPanel.Control());

            teachPanel.TeachHandler = teachHandler;
            teachPanel.RotationLocked = true;

            teachPanel.ChildMouseMove += TeachPanel_ChildMouseMove;
            teachPanel.ChildMouseDown += TeachPanel_ChildMouseDown;
            teachPanel.ChildMouseUp += TeachPanel_ChildMouseUp;
            teachPanel.FigureDeleted += TeachPanel_FigureDeleted;
            teachPanel.FigureCopied += TeachPanel_FigureCopied;
            teachPanel.FigureCreated += TeachPanel_FigureCreated;
            teachPanel.FigureSelected += TeachPanel_FigureSelected;
            teachPanel.FigureModified += TeachPanel_FigureModified;
            teachPanel.ChildMouseClick += TeachPanel_ChildMouseClicked;
            teachPanel.ChildMouseDblClick += TeachPanel_ChildMouseDblClick;
        }

        private void AddDockAreaPane()
        {
            dockAreaPaneRight = new DockAreaPane(DockedLocation.DockedRight);
            //            dockAreaPaneRight.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            ultraDockManager.DockAreas.Add(dockAreaPaneRight);

            dockAreaPaneLeft = new DockAreaPane(DockedLocation.DockedLeft);
            //            dockAreaPaneLeft.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            ultraDockManager.DockAreas.Add(dockAreaPaneLeft);

            dockAreaPaneTop = new DockAreaPane(DockedLocation.DockedTop);
            //            dockAreaPaneLeft.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            ultraDockManager.DockAreas.Add(dockAreaPaneTop);
        }

        private void AddWindowDockingArea()
        {
            windowDockingAreaRight = new WindowDockingArea();
            windowDockingAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            windowDockingAreaRight.Size = new System.Drawing.Size(257, 660);
            windowDockingAreaRight.Owner = ultraDockManager;
            Controls.Add(windowDockingAreaRight);

            windowDockingAreaLeft = new WindowDockingArea();
            windowDockingAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            windowDockingAreaLeft.Size = new System.Drawing.Size(257, 660);
            windowDockingAreaLeft.Owner = ultraDockManager;
            Controls.Add(windowDockingAreaLeft);

            windowDockingAreaTop = new WindowDockingArea();
            windowDockingAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            windowDockingAreaTop.Size = new System.Drawing.Size(257, 660);
            windowDockingAreaTop.Owner = ultraDockManager;
            Controls.Add(windowDockingAreaTop);
        }

        private DockAreaPane GetDockAreaPane(DockedLocation dockedLocation)
        {
            if (dockedLocation == DockedLocation.DockedLeft)
            {
                return dockAreaPaneLeft;
            }
            else if (dockedLocation == DockedLocation.DockedTop)
            {
                return dockAreaPaneTop;
            }
            else
            {
                return dockAreaPaneRight;
            }
        }

        private WindowDockingArea GetWindowDockingArea(DockedLocation dockedLocation)
        {
            if (dockedLocation == DockedLocation.DockedLeft)
            {
                return windowDockingAreaLeft;
            }
            else if (dockedLocation == DockedLocation.DockedTop)
            {
                return windowDockingAreaTop;
            }
            else
            {
                return windowDockingAreaRight;
            }
        }

        private void AddModellerPane(IModellerPane modellerPane)
        {
            DockAreaPane dockAreaPane = GetDockAreaPane(modellerPane.DockedLocation);

            var dockableControlPane = new DockableControlPane();
            dockableControlPane.Control = modellerPane.Control;
            dockableControlPane.OriginalControlBounds = new System.Drawing.Rectangle(287, 81, 290, 224);
            dockableControlPane.Size = new System.Drawing.Size(200, 100);
            dockableControlPane.Text = modellerPane.Title;

            dockAreaPane.Panes.Add(dockableControlPane);

            WindowDockingArea windowDockingArea = GetWindowDockingArea(modellerPane.DockedLocation);

            var dockableWindow = new DockableWindow();
            windowDockingArea.Controls.Add(dockableWindow);

            dockableWindow.Controls.Add(modellerPane.Control);
            dockableWindow.Owner = ultraDockManager;
            dockableWindow.Size = new System.Drawing.Size(200, 100);
            dockableWindow.Visible = false;
            dockableWindow.Text = modellerPane.Title;
        }

        private void AddModellerPane(IModellerToolbar modellerToorBar)
        {
            DockAreaPane dockAreaPane = GetDockAreaPane(modellerToorBar.DockedLocation);

            var dockableControlPane = new DockableControlPane();
            dockableControlPane.Control = modellerToorBar.Control;
            dockableControlPane.OriginalControlBounds = new System.Drawing.Rectangle(287, 81, 290, 224);
            dockableControlPane.Size = new System.Drawing.Size(200, 100);
            dockableControlPane.Text = modellerToorBar.Title;

            dockAreaPane.Panes.Add(dockableControlPane);

            WindowDockingArea windowDockingArea = GetWindowDockingArea(modellerToorBar.DockedLocation);

            var dockableWindow = new DockableWindow();
            windowDockingArea.Controls.Add(dockableWindow);

            dockableWindow.Controls.Add(modellerToorBar.Control);
            dockableWindow.Owner = ultraDockManager;
            dockableWindow.Size = new System.Drawing.Size(200, 100);
            dockableWindow.Text = modellerToorBar.Title;
        }

        public void ChangeCaption()
        {

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

            teachPanel.ZoomFit();
            UpdatePage();
        }

        private void TeachBox_MouseClicked(DrawBox senderView, Point clickPos, ref bool processingCancelled)
        {
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

            modellerPaneList.ForEach(x => x.SelectObject(teachObjectList));

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

            teachPanel.ClearSelection();

            foreach (Target target in targetList)
            {
                teachHandler.Select(target);
                teachPanel.SelectFigureByTag(target);
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

            teachPanel.LockMoveFigure(true);

            LogHelper.Debug(LoggerType.StartUp, "End ModellerPage_Load");
        }

        private bool Joystick_MovableCheck()
        {
            return true;
        }

        public void Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize");

            //modellerToolbar.Initialize(this);

            LogHelper.Debug(LoggerType.StartUp, "End ModellerPage::Initlaize");
        }

        private void UpdateImageFigure()
        {
            teachPanel.UpdateCenterGuide(UiConfig.Instance().ShowCenterGuide,
                            UiConfig.Instance().CenterGuidePos, UiConfig.Instance().CenterGuideThickness);

            InspectStep inspectionStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectionStep == null)
            {
                return;
            }

            teachPanel.UpdateFigure(inspectionStep, cameraIndex, positionAligner, null);
        }

        private void TeachPanel_FigureSelected(List<Figure> figureList, bool select = true)
        {
            if (InvokeRequired)
            {
                Invoke(new FigureSelectedDelegate(TeachPanel_FigureSelected), figureList, select);
                return;
            }

            modellerPaneList.ForEach(x => x.SelectObject(teachHandler.SelectedObjs));

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
                modellerPaneList.ForEach(x => x.SelectObject(teachHandler.SelectedObjs));
            }

            modellerToolbar.UpdateButtonState(teachHandler, cameraIndex, lightTypeIndex);

            onValueUpdate = false;
        }

        public void ClearProbeData()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ClearProbeData");

            teachPanel.ClearSelection();

            modellerPaneList.ForEach(x => x.Clear());
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
            teachPanel.LockMoveFigure(lockMove);
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

            teachPanel.StartLive(camera);
        }

        public void StopLive()
        {
            onLiveGrab = false;

            teachPanel.StopLive();
        }

        public bool IsOnLive()
        {
            return onLiveGrab;
        }

        private delegate void UpdateImageDelegate();
        private void Inspect()
        {
            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            modellerPaneList.ForEach(x => x.OnPreSelectedInspect());

            Calibration curCameraCalibration = SystemManager.Instance().GetCameraCalibration(cameraIndex);

            ImageBuffer imageBuffer = DeviceManager.Instance().ImageBufferPool.GetObject();

            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            imageAcquisition.Acquire(imageBuffer, inspectStep.GetLightParamSet(), inspectStep.GetLightTypeIndexArr());

            probeResultList.Clear();

            var tempFigures = new FigureGroup();

            teachHandler.Inspect(inspectStep, imageBuffer, true, cameraIndex, curCameraCalibration, probeResultList, null);

            foreach (ProbeResult probeResult in probeResultList)
            {
                probeResult.AppendResultFigures(tempFigures, ResultImageType.Camera);
            }

            teachPanel.UpdateTeampFigure(tempFigures);

            UpdateImageFigure();

            modellerPaneList.ForEach(x => x.OnPostSelectedInspect(probeResultList));
        }

        public void Delete()
        {
            if (teachHandler.IsSelected() == false)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - Delete");

            teachHandler.DeleteObject();
            teachPanel.ClearSelection();
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
            if (probe is VisionProbe visionProbe)
            {
                visionProbe.InspAlgorithm.Enabled = true;
                if (sourceImage2d.NumBand == 1)
                {
                    visionProbe.InspAlgorithm.Param.SourceImageType = ImageType.Grey;
                }
                else
                {
                    visionProbe.InspAlgorithm.Param.SourceImageType = ImageType.Color;
                }
            }

            InspectStep inspectStep = CurrentModel?.GetInspectStep(stepIndex);
            if (inspectStep == null)
            {
                return;
            }

            var newTarget = new Target();
            inspectStep.AddTarget(newTarget);

            commandManager.Execute(new AddProbeCommand(newTarget, probe, positionAligner));

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
                teachPanel.SelectFigureByTag(target);
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

            modellerPaneList.ForEach(x => x.SelectObject(teachHandler.SelectedObjs));

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

            var cloneImage = (Image2D)sourceImage2d.Clone();

            List<Probe> probeList = teachHandler.GetSelectedProbe();
            foreach (Probe probe in probeList)
            {
                if (probe != null && onPreviewMode)
                {
                    probe.PreviewFilterResult(probe.BaseRegion, cloneImage, previewIndex);
                }
            }

            teachPanel.UpdateImage(cloneImage);

            modellerPaneList.ForEach(x => x.UpdateImage(sourceImage2d, lightTypeIndex));
        }

        private delegate void UpdateImageTestDelegate(Image2D sourceImage2d);
        public void UpdateImageTest(Image2D sourceImage2d)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Invoke UpdateImageTest");

                Invoke(new UpdateImageTestDelegate(UpdateImageTest), sourceImage2d);
                return;
            }

            //sourceImage2d = SystemManager.Instance().ImageSequence.GetImage(cameraIndex, 0, 0);

            if (sourceImage2d == null)
            {
                return;
            }

            var cloneImage = (Image2D)sourceImage2d.Clone();

            /*
            List<Probe> probeList = teachHandler.GetSelectedProbe();
            foreach (Probe probe in probeList)
            {
                if (probe != null && onPreviewMode)
                {
                    probe.PreviewFilterResult(probe.BaseRegion, cloneImage, previewIndex);
                }
            }
            */

            //teachPanel.Cle
            teachPanel.UpdateImage(cloneImage);
            //cloneImage.SaveImage(@"d:\test01.bmp", ImageFormat.Bmp);

            //modellerPaneList.ForEach(x => x.UpdateImage(sourceImage2d, lightTypeIndex));
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
            StepModel stepModel = ModelManager.Instance().CurrentStepModel;
            if (stepModel == null)
            {
                return;
            }

            var systemManager = SystemManager.Instance();
            var form = new OutputFormatForm();
            form.Model = stepModel;
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

            if (sourceImage2d == null)
            {
                return;
            }

            teachHandler.Boundary = new Rectangle(0, 0, sourceImage2d.Width, sourceImage2d.Height);

            UpdateImage();
            UpdateImageFigure();
        }

        private bool GrabImage()
        {
            if (Visible == false)
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

        private Image2D GrabImageTest()
        {
            var lightParam = new LightParam(0, "test", 0);
            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            Image2D grabImage = imageAcquisition.Acquire(cameraIndex, lightParam);

            return grabImage;
        }

        private void PasteProbeButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        public void ZoomInButtonClicked()
        {
            teachPanel.ZoomIn();
        }

        public void ZoomOutButtonClicked()
        {
            teachPanel.ZoomOut();
        }

        public void StartGrabProcess()
        {
            /*
            StepModel stepModel = ModelManager.Instance().CurrentModel as StepModel;
            if (stepModel == null)
                return;

            InspectStep curInspectStep = stepModel.GetInspectStep(stepIndex);
            if (curInspectStep == null)
                return;

            LightParamSet lightParamSet = curInspectStep.GetLightParamSet();

            modellerPageExtender.GrabImage(stepIndex, cameraIndex, lightTypeIndex, lightParamSet[lightTypeIndex]);

            UpdatePage();
            */
            MoveAndGrabTest();
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
            //GrabImage();
            //UpdateImage();
            Image2D image2d = GrabImageTest();
            UpdateImageTest(image2d);
        }

        public void ZoomFitButtonClicked()
        {
            teachPanel.ZoomFit();
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
            //ultraDockManager.SaveComponentSettings();
            //Inspect();
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

        public InspectStep CreateInspectionStep()
        {
            var stepModel = (StepModel)ModelManager.Instance().CurrentModel;

            InspectStep inspectStep = stepModel.CreateInspectionStep();
            inspectStep.Position.SetPosition(currentPosition.Position);

            modellerPaneList.ForEach(x => x.StepChanged(inspectStep));

            UpdatePage();

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

            teachPanel.ClearSelection();

            teachHandler.Select(newTarget);
            teachPanel.SelectFigureByTag(newTarget);
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ModelManager.Instance().CurrentModel.SaveModel();

            MessageForm.Show((Form)UiManager.Instance().MainForm, StringManager.GetString("Save OK"), "UniEye");
        }

        public void InspectStepChanged(int stepIndex)
        {
            this.stepIndex = stepIndex;

            teachPanel.ClearSelection();

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

            modellerPaneList.ForEach(x => x.StepChanged(inspectStep));
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

                CoordMapper coordMapper = teachPanel.GetCoordMapper();
                SizeF offsetF = coordMapper.PixelToWorld(dragOffset);

                DeviceManager.Instance().RobotStage.RelativeMove(new AxisPosition(offsetF.Width, offsetF.Height));
                teachPanel.Invalidate();

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
            bool flag = false;
            modellerPaneList.ForEach(x => x.PointSelected(point, ref flag));

            processingCancelled = flag;
        }

        private void TeachPanel_ChildMouseDblClick(UserControl sender)
        {

        }

        public void TogglePreview()
        {
            onPreviewMode = !onPreviewMode;

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
            ultraDockManager.ShowAll();

            //            Preview();
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
                teachPanel.LockMoveFigure(true);

                if (ModelManager.Instance().CurrentModel == null)
                {
                    return;
                }

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
                    teachPanel.SelectFigureByTag(probe);
                    teachPanel.Invalidate(true);

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

        public void MoveAndGrabTest()
        {
            AxisHandler robotHandler = null;
            List<AxisHandler> hList = DeviceManager.Instance().AxisHandlerList;
            if (hList.Count > 0)
            {
                robotHandler = DeviceManager.Instance().AxisHandlerList[0];
                //axis = DeviceManager.Instance().AxisHandlerList[0].GetAxis(0);
            }


            for (int index = 0; index < 10; index++)
            {
                var startPositon = new AxisPosition(5000, 170000);
                robotHandler.Move(startPositon);

                float xPitch = 30000;
                float yPitch = 35000;

                for (int xIndex = 0; xIndex < 8; xIndex++)
                {
                    for (int yIndex = 0; yIndex < 9; yIndex++)
                    {
                        var positon = new AxisPosition(5000 + xIndex * xPitch, 170000 + yIndex * yPitch);
                        robotHandler.Move(positon);
                        SingleShotButtonClicked();
                    }
                }
            }
        }

        private void MenuItemLoadTeachLayoutFile_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(BaseConfig.Instance().ConfigPath, "TeachLayout.xml");
            ultraDockManager.LoadFromXML(filePath);
        }

        private void DockingToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            foreach (DockableControlPane controlPane in ultraDockManager.ControlPanes)
            {
                switch (controlPane.Control.GetType().Name)
                {
                    case "ModellerToolbar":
                        menuItemModellerToolbar.Checked = IsControlPaneVisible("ModellerToolbar");
                        break;
                    case "ParamPanel":
                        menuItemParamPanel.Checked = IsControlPaneVisible("ParamPanel");
                        break;
                    case "FiducialPanel":
                        menuItemFiducialPanel.Checked = IsControlPaneVisible("FiducialPanel");
                        break;
                    case "ModelTreePanel":
                        menuItemModelTreePanel.Checked = IsControlPaneVisible("ModelTreePanel");
                        break;
                    case "ResultPanel":
                        menuItemResultPanel.Checked = IsControlPaneVisible("ResultPanel");
                        break;
                    case "FovNavigatorPanel":
                        menuItemFovNaviPanel.Checked = IsControlPaneVisible("FovNavigatorPanel");
                        break;
                }
            }
        }

        private bool IsControlPaneVisible(string className)
        {
            DockableControlPane controlPane = GetControlPane(className);
            if (controlPane != null)
            {
                return controlPane.IsVisible;
            }
            return false;
        }

        private void MenuItemParamPanel_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemParamPanel, "ParamPanel");
        }

        private void ToggleVisibleControlPane(ToolStripMenuItem menuItem, string className)
        {
            DockableControlPane controlPane = GetControlPane(className);
            if (controlPane != null)
            {
                if (!menuItem.Checked)
                {
                    controlPane.Show();
                }
                else
                {
                    controlPane.Close();
                }
            }
        }

        private DockableControlPane GetControlPane(string className)
        {
            foreach (DockableControlPane controlPane in ultraDockManager.ControlPanes)
            {
                if (controlPane.Control.GetType().Name == className)
                {
                    return controlPane;

                }
            }

            return null;
        }

        private void MenuItemModellerToolbar_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemModellerToolbar, "ModellerToolbar");
        }

        private void MenuItemResultPanel_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemResultPanel, "ResultPanel");
        }

        private void MenuItemModelTreePanel_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemModelTreePanel, "ModelTreePanel");
        }

        private void MenuItemFovNaviPanel_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemFovNaviPanel, "FovNavigatorPanel");
        }

        private void MenuItemFiducialPanel_Click(object sender, EventArgs e)
        {
            ToggleVisibleControlPane(menuItemFiducialPanel, "FiducialPanel");
        }

        private void MenuItemSaveTeachLayoutFile_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(BaseConfig.Instance().ConfigPath, "TeachLayout.xml");
            ultraDockManager.SaveAsXML(filePath);
        }

        private void MenuItemLoadResultLayoutFile_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(BaseConfig.Instance().ConfigPath, "InspectionResultLayout.xml");
            ultraDockManager.LoadFromXML(filePath);
        }

        private void MenuItemSaveResultLayoutFile_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(BaseConfig.Instance().ConfigPath, "InspectionResultLayout.xml");
            ultraDockManager.SaveAsXML(filePath);
        }
    }
}
