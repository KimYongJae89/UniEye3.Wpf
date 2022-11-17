using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public interface ITeachPanel
    {
        UserControl Control();

        FigureCreatedDelegate FigureCreated { get; set; }
        FigureSelectedDelegate FigureSelected { get; set; }
        FigureCopiedDelegate FigureCopied { get; set; }
        FigureDeletedDelegate FigureDeleted { get; set; }
        FigureModifiedDelegate FigureModified { get; set; }
        FigurePastedDelegate FigurePasted { get; set; }
        MouseClickedDelegate ChildMouseClick { get; set; }
        MouseDblClickedDelegate ChildMouseDblClick { get; set; }
        MouseMovedDelegate ChildMouseMove { get; set; }
        MouseDownDelegate ChildMouseDown { get; set; }
        MouseUpDelegate ChildMouseUp { get; set; }
        TeachHandler TeachHandler { get; set; }
        bool RotationLocked { get; set; }

        CoordMapper GetCoordMapper();
        void LockMoveFigure(bool v);
        void UpdateCenterGuide(bool showCenterGuide, Point centerGuidePos, int centerGuideThickness);
        void UpdateFigure(InspectStep inspectStep, int cameraIndex, PositionAligner positionAligner, ProbeResultList probeResultList);
        void ClearSelection();
        void SelectFigureByTag(ITeachObject teachObject);
        void UpdateTeampFigure(FigureGroup tempFigures);
        void UpdateImage(ImageD image, bool fReSelect = true);
        void Invalidate(bool invalidChild = false);

        void ZoomIn();
        void ZoomOut();
        void ZoomFit();

        void StartLive(Camera camera);
        void StopLive();
    }

    public partial class TeachPanel : CanvasPanel, ITeachPanel
    {
        public delegate ITeachObject CreateObjectDelegate();
        public delegate void ObjectModifiedDelegate();

        private Camera linkedCamera;
        public TeachHandler TeachHandler { get; set; }
        public bool InplacePreview { get; set; }
        public bool ShowNumber { get; set; } = false;
        public bool IncludeProbe { get; set; } = true;
        public ProductResult ProductResult { get; set; }

        public TeachPanel()
        {
            InitializeComponent();

            FigureModified += teachPanel_FigureModified;
            FigurePasted += teachPanel_FigurePasted;
            FigureSelected += teachPanel_FigureSelected;
            FigureDeleted += teachPanel_FigureDeleted;
            ChildMouseDblClick += teachPanel_MouseDblClicked;
        }

        public UserControl Control()
        {
            return this;
        }

        public new FigureCreatedDelegate FigureCreated
        {
            get => base.FigureCreated;
            set => base.FigureCreated = value;
        }

        public new FigureSelectedDelegate FigureSelected
        {
            get => base.FigureSelected;
            set => base.FigureSelected = value;
        }

        public new FigureCopiedDelegate FigureCopied
        {
            get => base.FigureCopied;
            set => base.FigureCopied = value;
        }

        public new FigureDeletedDelegate FigureDeleted
        {
            get => base.FigureDeleted;
            set => base.FigureDeleted = value;
        }

        public new FigureModifiedDelegate FigureModified
        {
            get => base.FigureModified;
            set => base.FigureModified = value;
        }

        public new FigurePastedDelegate FigurePasted
        {
            get => base.FigurePasted;
            set => base.FigurePasted = value;
        }

        public new MouseClickedDelegate ChildMouseClick
        {
            get => base.ChildMouseClick;
            set => base.ChildMouseClick = value;
        }

        public new MouseDblClickedDelegate ChildMouseDblClick
        {
            get => base.ChildMouseDblClick;
            set => base.ChildMouseDblClick = value;
        }

        public new MouseMovedDelegate ChildMouseMove
        {
            get => base.ChildMouseMove;
            set => base.ChildMouseMove = value;
        }

        public new MouseDownDelegate ChildMouseDown
        {
            get => base.ChildMouseDown;
            set => base.ChildMouseDown = value;
        }

        public new MouseUpDelegate ChildMouseUp
        {
            get => base.ChildMouseUp;
            set => base.ChildMouseUp = value;
        }

        private void teachPanel_MouseDblClicked(UserControl sender)
        {
            ProductResult?.Clear();
        }

        private void teachPanel_FigureDeleted(List<Figure> figureList)
        {
            Delete();
        }

        public void SetAddMode()
        {
            Cursor = Cursors.Cross;
            DragMode = DragMode.Add;
            SetAddMode(FigureType.Rectangle);
        }

        public void StartLive(Camera camera)
        {
            if (camera != null)
            {
                linkedCamera = camera;

                if (linkedCamera != null)
                {
                    linkedCamera.ImageGrabbed += camera_ImageGrabbed;
                    linkedCamera.GrabMulti();
                }
            }
        }

        public void StopLive()
        {
            if (linkedCamera != null)
            {
                linkedCamera.ImageGrabbed -= camera_ImageGrabbed;
                linkedCamera = null;
            }
        }

        private void camera_ImageGrabbed(Camera camera)
        {
            if (Visible == false)
            {
                return;
            }

            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Start Invoke ImageGrabbed");
                BeginInvoke(new CameraEventDelegate(camera_ImageGrabbed), camera);
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Start ImageGrabbed : Camera - {0}", camera.Index));

            var imageD = (Image2D)camera.GetGrabbedImage();

            if (imageD.ImageData != null)
            {
                UpdateImage(imageD.ToBitmap());

                Invalidate(true);
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("End UpdateImage : Camera - {0}", camera.Index));
        }

        private delegate void UpdateImageDelegate(ImageD image, bool fReSelect);
        public void UpdateImage(ImageD image, bool fReSelect = true)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateImageDelegate(UpdateImage), image, fReSelect);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "TeachBox - Begin UpdateImage");

            var bitmap = image.ToBitmap();
            UpdateImage(bitmap, new RectangleF(0, 0, bitmap.Width, bitmap.Height));

            LogHelper.Debug(LoggerType.Operation, "TeachBox - End UpdateImage");

            Invalidate(true);
        }

        private CanvasPanel.Option GetOption()
        {
            var option = new CanvasPanel.Option();
            option.IncludeProbe = IncludeProbe;
            option.ShowProbeNumber = ShowNumber;

            return option;
        }

        public void UpdateFigure(InspectStep inspectStep, int cameraIndex, PositionAligner positionAligner, ProbeResultList probeResultList)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            var workingFigures = new FigureGroup();
            var backgroundFigures = new FigureGroup();
            var tempFigures = new FigureGroup();

            // 모든 Target의 Figure 및 각 Target에 속한 Probe의 Figure를 얻어온다.
            if (inspectStep != null)
            {
                inspectStep.AppendFigures(cameraIndex, positionAligner, workingFigures, backgroundFigures, GetOption());
            }

            if (probeResultList != null)
            {
                TeachHandler.AppendResultFigures(probeResultList, backgroundFigures);
            }

            WorkingFigures = workingFigures;
            BackgroundFigures = backgroundFigures;
            TempFigures = tempFigures;

            Invalidate();
        }

        /// <summary>
        /// CanvasPanel에서 새로운 Figure의 목록이 생성된 후, 이 함수가 호출된다.
        /// </summary>
        /// <param name="figureList"></param>
        private void teachPanel_FigurePasted(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            TeachHandler.Paste(figureList, workingFigures, backgroundFigures, pasteOffset);
        }

        public override void ClearSelection()
        {
            TeachHandler.Clear();
            //ClearSelection();
        }

        private void teachPanel_FigureSelected(List<Figure> figureList, bool select = true)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            TeachHandler.Select(figureList);
        }

        private void teachPanel_FigureModified(List<Figure> figureList)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            if (TeachState.Instance().LockMove == true)
            {
                return;
            }

            TeachHandler.Move(figureList);
        }

        public void Delete()
        {
            TeachHandler.DeleteObject();
            ClearSelection();
        }

        public void Inspect(InspectStep inspectStep, ImageBuffer imageBuffer, bool saveDebugImage, int cameraIndex, Calibration calibration,
                                ProbeResultList probeResultList, IInspectEventListener inspectEventHandler)
        {
            var tempFigures = new FigureGroup();

            var newProbeResultList = new ProbeResultList();
            TeachHandler.Inspect(inspectStep, imageBuffer, saveDebugImage, cameraIndex, calibration, newProbeResultList, inspectEventHandler);

            foreach (ProbeResult probeResult in newProbeResultList)
            {
                probeResult.AppendResultFigures(tempFigures, ResultImageType.Camera);
            }

            probeResultList?.AddProbeResult(newProbeResultList);

            TempFigures = tempFigures;

            Invalidate();
        }

        public void Select(ITeachObject teachObject)
        {
            TeachHandler.Select(teachObject);
            SelectFigureByTag(teachObject);
        }

        public void Select(List<ITeachObject> teachObjectList)
        {
            TeachHandler.Select(teachObjectList);
            var tagList = new List<object>();
            foreach (ITeachObject teachObject in teachObjectList)
            {
                tagList.Add(teachObject);
            }

            SelectFigureByTag(tagList);
        }

        public void LockMoveFigure(bool flag)
        {
            TeachHandler.Movable = (flag == false);
            Editable = (flag == false);
        }

        public void UpdateTempFigure(FigureGroup figureGroup)
        {
            TempFigures = figureGroup;
            Invalidate();
        }

        public void SelectFigureByTag(ITeachObject teachObject)
        {
            base.SelectFigureByTag(teachObject);
        }

        public void UpdateTeampFigure(FigureGroup tempFigures)
        {
            TempFigures = tempFigures;
            Invalidate();
        }
    }
}
