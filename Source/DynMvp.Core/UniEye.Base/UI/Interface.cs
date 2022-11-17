using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices.Light;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using Infragistics.Win.UltraWinDock;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public interface IInspectionTimeExpender
    {
        void ProductInspected(ProductResult inspectionResult);
    };

    public interface IModellerToolbar
    {
        void Initialize(IModellerPage modellerPage);
        void SelectInspectStep(int stepNo);
        void ChangeCaption();
        void UpdateButtonState(TeachHandler teachHandler, int cameraIndex, int lightTypeIndex);
        void UpdateInspectStepButton(int stepNoSelected = 0);

        string Title { get; }
        Control Control { get; }
        DockedLocation DockedLocation { get; }
    }

    public interface IInspectPanel : IInspectEventListener
    {
        void Initialize();
        void Initialize(int[] camIdArr);
        void ClearPanel();
    }

    public enum PaneType { Fid, ModelTree, ModellerToolBar, Param, Result, FovNavi }
    public interface IModellerPane
    {
        string Title { get; }
        PaneType PaneType { get; }
        Control Control { get; }
        DockedLocation DockedLocation { get; }

        void OnPreSelectedInspect();
        void OnPostSelectedInspect(ProbeResultList probeResultList);
        void StepChanged(InspectStep inspectStep);

        void Clear();
        void SelectObject(List<ITeachObject> teachObjectList);
        void PointSelected(PointF point, ref bool processingCancelled);
        void UpdateImage(Image2D sourceImage2d, int lightTypeIndex);
    }

    public interface IModellerPage
    {
        string AddObjectTypeStr { get; }

        void ProcessKeyDown(KeyEventArgs e);

        RotatedRect GetDefaultProbeRegion();
        Image2D GetClipImage(RotatedRect clipRegion);
        void ZoomInButtonClicked();
        void ZoomOutButtonClicked();
        void GroupButtonClicked();
        void UngroupButtonClicked();
        void PreviewIndexChanged(int previewIndex);
        void CameraIndexChanged(int deviceIndex);
        void LightTypeChanged(int lightTypeIndex);
        void InspectStepChanged(int stepNo);
        void AddProbe(Probe probe);
        InspectStep CreateInspectionStep();
        void RobotSettingButtonClicked();
        void JoystickButtonClicked();
        void OriginButtonClicked();

        void ToggleLive();
        bool IsOnLive();
        void ToggleLockMove();
        bool IsMoveLocked();
        void ToggleFineMove();
        bool IsOnFineMove();
        void TogglePreview();
        bool IsOnPreview();

        void DeleteStepButtonClicked();
        void SingleShotButtonClicked();
        Probe SearchProbeButtonClicked();
        void ScanButtonClicked();

        void UndoButtonClicked();
        void RedoButtonClicked();

        void StartGrabProcess();

        void EditSchemaButtonClicked();
        void CreateSchemaButtonClicked();

        void AddModeClicked(string addObjectTypeStr);
        void SelectModeClicked();
        void PanningModeClicked();
        void ShowCenterLineButtonClicked(bool showCenterLine);
        void ShowNumberButtonClicked(bool showNumber);

        void CopyButtonClicked();
        void PasteButtonClicked();
        void DeleteButtonClicked();

        void SetFiducialButtonClicked();

        void ZoomModeClicked();
        void ZoomFitButtonClicked();
        void SyncParamButtonClicked();
        void SyncAllButtonClicked();

        void LoadImageSetButtonClicked();

        void ShowLightPanelButtonClicked();
        void ModelPropertyButtonClicked();
        void ExportFormatButtonClicked();

        string EditStepButtonClicked(int stepIndex);
        void PreviewModeChanged(bool previewMode);
        void WholeImageModeChanged(bool wholeImageMode);

        void ModifyTeaching(string imagePath);

        void UpdateState();
        void PrevImageButtonClicked();
        void NextImageButtonClicked();
    }

    public enum StatusBarItem
    {
        Message, Lisence, Ready, Trigger1, Trigger2, Inspection, Grab, Complete, Done, MousePos, MouseOffsetPos, RobotPos
    }

    public enum TabKey
    {
        Model, Inspect, Teach, Report, Statistics, Live, Setting, Log, User, Etc
    }

    public interface IMainTabPage
    {
        TabKey TabKey { get; }
        string TabName { get; }
        Bitmap TabIcon { get; }
        Bitmap TabSelectedIcon { get; }
        Color TabSelectedColor { get; }
        bool IsAdminPage { get; }
        System.Uri Uri { get; }

        void Initialize();
        void ChangeCaption();
        void TabPageVisibleChanged(bool visibleFlag);
        void ProcessKeyDown(KeyEventArgs e);

        void OnIdle();
    }

    public interface IRightPanel
    {
        void Initialize();
        void EnableControls();
        void OnIdle();
        void Destroy();
    }

    public interface IModelExtraPropertyPanel
    {
        void Initialize();
    }

    public interface IMainForm
    {
        /// <summary>
        /// 언어 변경시 호출
        /// </summary>
        void ChangeCaption();
        void ModifyTeaching(string imagePath);
        void ChangeOpMode(OpMode opMode);
        OpMode GetOpMode();
        void TestInspect();
    }

    public interface IParamControl
    {
        void SetLightParamSet(LightParamSet lightParamSet);

        void ClearProbeData();
        void Init(TeachHandler teachHandlerProbe, CommandManager commandManager, AlgorithmValueChangedDelegate valueChanged);
        void PointSelected(Point clickPos, ref bool processingCancelled);
        void SelectObject(List<ITeachObject> teachObjectList);
        void UpdateTargetGroupImage(ImageD targetGroupImage, int lightTypeIndex);
    }

    public interface IReportPanel
    {
        void ClearPanel();
        void ShowResult(string resultPath, int stepNo);
    }
}
