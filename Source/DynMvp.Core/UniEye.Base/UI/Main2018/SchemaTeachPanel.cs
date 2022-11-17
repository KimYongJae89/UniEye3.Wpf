using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspectData;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI.Main2018
{
    public class SchemaTeachPanel : SchemaViewer, ITeachPanel
    {
        public FigureCreatedDelegate FigureCreated { get; set; }
        public FigureCopiedDelegate FigureCopied { get; set; }
        public FigureDeletedDelegate FigureDeleted { get; set; }
        public FigureModifiedDelegate FigureModified { get; set; }
        public FigurePastedDelegate FigurePasted { get; set; }
        public MouseClickedDelegate ChildMouseClick { get; set; }
        public MouseMovedDelegate ChildMouseMove { get; set; }
        public MouseDblClickedDelegate ChildMouseDblClick { get; set; }
        public MouseDownDelegate ChildMouseDown { get; set; }
        public MouseUpDelegate ChildMouseUp { get; set; }
        public TeachHandler TeachHandler { get; set; }

        private bool rotationLocked;
        public bool RotationLocked
        {
            get => rotationLocked;
            set
            {
                rotationLocked = value;
                TrackerRotationLocked = value;
            }
        }

        public new FigureSelectedDelegate FigureSelected
        {
            get => base.FigureSelected;
            set => base.FigureSelected = value;
        }

        public void ClearSelection()
        {
            throw new NotImplementedException();
        }

        public UserControl Control()
        {
            return this;
        }

        public CoordMapper GetCoordMapper()
        {
            return new CoordMapper();
        }

        public void LockMoveFigure(bool flag)
        {
            TeachHandler.Movable = (flag == false);
            Editable = (flag == false);
        }

        public void SelectFigureByTag(ITeachObject teachObject)
        {
            base.SelectFigureByTag(new List<object>() { teachObject });
        }

        public void StartLive(Camera camera)
        {

        }

        public void StopLive()
        {

        }

        public void UpdateCenterGuide(bool showCenterGuide, Point centerGuidePos, int centerGuideThickness)
        {

        }

        public void UpdateFigure(InspectStep inspectStep, int cameraIndex, PositionAligner positionAligner, ProbeResultList probeResultList)
        {

        }

        public void UpdateImage(ImageD image, bool fReSelect = true)
        {

        }

        public void UpdateTeampFigure(FigureGroup tempFigures)
        {

        }

        public void ZoomFit()
        {

        }

        public void ZoomIn()
        {

        }

        public void ZoomOut()
        {

        }
    }
}
