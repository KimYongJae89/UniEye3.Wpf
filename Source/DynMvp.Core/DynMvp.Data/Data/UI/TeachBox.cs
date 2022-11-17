using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public delegate void ObjectSelected(ImageD image);
    public delegate void ObjectMultiSelected();
    public delegate void ObjectMoved();
    public delegate void ObjectAdded();

    public partial class TeachBox : UserControl
    {
        public DrawBox DrawBox { get; set; }
        public TeachHandler TeachHandler { get; set; }

        public bool Enable
        {
            get => DrawBox.Enable;
            set => DrawBox.Enable = value;
        }

        public bool RotationLocked
        {
            get => DrawBox.RotationLocked;
            set => DrawBox.RotationLocked = value;
        }
        public bool InplacePreview { get; set; }
        public bool ShowNumber { get; set; } = false;
        public bool IncludeProbe { get; set; } = true;

        public Bitmap Image => DrawBox.Image;

        public bool ShowCenterGuide
        {
            get => DrawBox.ShowCenterGuide;
            set => DrawBox.ShowCenterGuide = value;
        }
        public ProbeResultList ProbeResultList { get; set; }

        private InspectStep inspectStep;
        private int cameraIndex;
        private PositionAligner positionAligner;

        public ObjectSelected ObjectSelected;
        public ObjectMultiSelected ObjectMultiSelected;
        public ObjectMoved ObjectMoved;
        public ObjectAdded ObjectAdded;
        public MouseClickedDelegate MouseClicked;
        public MouseDblClickedDelegate MouseDblClicked;
        public PositionShiftedDelegate PositionShifted;

        public TeachBox()
        {
            InitializeComponent();

            DrawBox = new DrawBox();

            SuspendLayout();

            DrawBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DrawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            DrawBox.Name = "drawBox";
            DrawBox.TabIndex = 0;
            DrawBox.TabStop = false;
            DrawBox.FigureAdd += drawBox_AddFigure;
            DrawBox.FigureMoved += drawBox_FigureMoved;
            DrawBox.FigureSelected += drawBox_FigureSelected;
            DrawBox.FigureSelectable += drawBox_FigureSelectable;
            DrawBox.FigureCopy += drawBox_FigureCopy;
            DrawBox.MouseDblClicked += drawBox_MouseDblClicked;
            DrawBox.MouseClicked += drawBox_MouseClicked;
            DrawBox.PositionShifted += drawBox_PositionShifted;

            Controls.Add(DrawBox);
            ResumeLayout(false);
        }

        private void drawBox_AddFigure(List<PointF> pointList, FigureType figureType)
        {
            throw new NotImplementedException();
        }

        private void drawBox_MouseClicked(UserControl userControl, PointF clickPos, MouseButtons button, ref bool processingCancelled)
        {
            MouseClicked?.Invoke(userControl, clickPos, button, ref processingCancelled);
        }

        private void drawBox_PositionShifted(SizeF offset)
        {
            PositionShifted?.Invoke(offset);
        }

        public void AutoFit(bool onOff)
        {
            DrawBox.AutoFit(onOff);
        }

        public void SetAddMode()
        {
            Cursor = Cursors.Cross;
            DrawBox.AddFigureMode = true;
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

            DrawBox.UpdateImage(image.ToBitmap());

            LogHelper.Debug(LoggerType.Operation, "TeachBox - End UpdateImage");

            DrawBox.Invalidate(true);
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

            this.inspectStep = inspectStep;
            this.cameraIndex = cameraIndex;
            this.positionAligner = positionAligner;
            ProbeResultList = probeResultList;

            // 모든 Target의 Figure 및 각 Target에 속한 Probe의 Figure를 얻어온다.
            if (inspectStep != null)
            {
                inspectStep.AppendFigures(cameraIndex, positionAligner, workingFigures, backgroundFigures, GetOption());
            }

            if (probeResultList != null)
            {
                TeachHandler.AppendResultFigures(probeResultList, backgroundFigures);
            }

            DrawBox.FigureGroup = workingFigures;
            DrawBox.BackgroundFigures = backgroundFigures;
            DrawBox.TempFigureGroup = tempFigures;

            DrawBox.Invalidate(true);
        }

        public void UpdateFigure()
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            var activeFigures = new FigureGroup();
            var backgroundFigures = new FigureGroup();
            var tempFigureGroup = new FigureGroup();

            // 모든 Target의 Figure 및 각 Target에 속한 Probe의 Figure를 얻어온다.
            if (inspectStep != null)
            {
                inspectStep.AppendFigures(cameraIndex, positionAligner, activeFigures, backgroundFigures, GetOption());
            }

            TeachHandler.AppendResultFigures(ProbeResultList, backgroundFigures);

            DrawBox.FigureGroup = activeFigures;
            DrawBox.BackgroundFigures = backgroundFigures;
            DrawBox.TempFigureGroup = tempFigureGroup;

            DrawBox.Invalidate(true);
        }

        private void drawBox_FigureCopy(List<Figure> figureList)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            TeachHandler.Copy(figureList);

            UpdateFigure();

            TeachHandler.ShowTracker(DrawBox);

            ObjectAdded?.Invoke();
        }

        public void ClearSelection()
        {
            TeachHandler.Clear();
            DrawBox.ResetSelection();
        }

        private bool drawBox_FigureSelectable(Figure figure)
        {
            return TeachHandler.IsSelectable(figure);
        }

        private void drawBox_FigureSelected(List<Figure> figureList, bool select)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            if (figureList.Count() == 0)
            {
                TeachHandler.Clear();

                ObjectSelected?.Invoke(null);
                return;
            }

            if (select == false)
            {
                TeachHandler.Unselect(figureList);
            }
            else if (figureList.Count() == 1)
            {
                if (select)
                {
                    TeachHandler.Select(figureList[0]);
                }
            }
            else
            {
                Figure firstFigure = null;

                var filteredList = new List<Figure>();
                foreach (Figure figure in figureList)
                {
                    if (figure.Tag is ITeachObject teachObject)
                    {
                        if (teachObject is Target)
                        {
                            continue;
                        }

                        if (firstFigure == null)
                        {
                            firstFigure = figure;
                        }
                        else
                        {
                            if (IsSameObject((ITeachObject)firstFigure.Tag, (ITeachObject)figure.Tag) == false)
                            {
                                continue;
                            }
                        }
                        TeachHandler.Select(figure);
                        filteredList.Add(figure);
                    }
                }

                figureList.Clear();
                figureList.AddRange(filteredList);
            }

            UpdateFigure();

            ObjectMultiSelected?.Invoke();
        }

        private bool IsSameObject(ITeachObject obj1, ITeachObject obj2)
        {
            if ((obj1 is VisionProbe) && (obj2 is VisionProbe))
            {
                var visionProbe1 = (VisionProbe)obj1;
                var visionProbe2 = (VisionProbe)obj2;
                if (visionProbe1.InspAlgorithm.GetAlgorithmType() == visionProbe2.InspAlgorithm.GetAlgorithmType())
                {
                    return true;
                }
            }

            if (obj1.GetType().Name == obj2.GetType().Name)
            {
                return true;
            }

            return true;
        }

        private void drawBox_FigureMoved(List<Figure> figureList)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            if (TeachHandler.Movable == false)
            {
                return;
            }

            TeachHandler.Move(figureList);

            UpdateFigure();

            ObjectMoved?.Invoke();

            Invalidate(true);
        }

        private void drawBox_AddRegionCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            if (TeachHandler.IsEditable() == false)
            {
                return;
            }

            DrawBox.AddFigureMode = false;
            Cursor = Cursors.Default;

            UpdateFigure();

            TeachHandler.ShowTracker(DrawBox);

            ObjectAdded?.Invoke();
        }

        private void drawBox_MouseDblClicked(UserControl sender)
        {
            ProbeResultList?.Clear();
            DrawBox.TempFigureGroup.Clear();
            DrawBox.Invalidate();

            MouseDblClicked?.Invoke(sender);
        }

        public void Delete()
        {
            TeachHandler.DeleteObject();

            UpdateFigure();
        }

        public void Inspect(InspectStep inspectStep, ImageBuffer imageBuffer, bool saveDebugImage, int cameraIndex, Calibration calibration,
                            DigitalIoHandler digitalIoHandler, ProbeResultList probeResultList, IInspectEventListener inspectEventHandler)
        {
            var newProbeResultList = new ProbeResultList();
            TeachHandler.Inspect(inspectStep, imageBuffer, saveDebugImage, cameraIndex, calibration, newProbeResultList, inspectEventHandler);

            foreach (ProbeResult probeResult in newProbeResultList)
            {
                probeResult.AppendResultFigures(DrawBox.TempFigureGroup, ResultImageType.Camera);
            }

            probeResultList.AddProbeResult(newProbeResultList);

            DrawBox.Invalidate();
        }

        public void ZoomIn()
        {
            DrawBox.ZoomIn();
        }

        public void ZoomOut()
        {
            DrawBox.ZoomOut();
        }

        public void ZoomFit()
        {
            DrawBox.ZoomFit();
        }
    }
}
