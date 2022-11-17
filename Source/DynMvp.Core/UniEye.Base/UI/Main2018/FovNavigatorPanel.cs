using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.InspectData;
using DynMvp.UI;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Inspect;

namespace UniEye.Base.UI.Main2018
{
    public partial class FovNavigatorPanel : UserControl, IModellerPane
    {
        private FovNavigator fovNavigator;

        public FovNavigatorPanel()
        {
            InitializeComponent();

            fovNavigator = new FovNavigator();

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            Camera camera = cameraHandler.GetCamera(0);
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;

            fovNavigator.Location = new System.Drawing.Point(96, 95);
            fovNavigator.Name = "fovNavigator";
            fovNavigator.Size = new System.Drawing.Size(74, 101);
            fovNavigator.TabIndex = 0;
            fovNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
            fovNavigator.RobotStage = robotStage;
            fovNavigator.Enable = true;
            fovNavigator.FovChanged += fovNavigator_FovChanged;

            fovNavigator.FovSize = camera.FovSize;
        }

        private void fovNavigator_FovChanged(int fovNo, PointF position)
        {
            if (InvokeRequired)
            {
                Invoke(new FovChangedDelegate(fovNavigator_FovChanged), fovNo, position);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, string.Format("Change FOV - {0}", fovNo + 1));

            if (fovNo > -1)
            {
                InspectStep inspectStepSel = ModelManager.Instance().CurrentStepModel?.GetInspectStep(fovNo, true);
                Debug.Assert(inspectStepSel != null, "Invalid Model Format. InspectionStep must have value");

                //if (inspectStepSel != null)
                //    modellerToolbar.SelectInspectStep(inspectStepSel.StepNo);
            }
            else
            {
                DeviceManager.Instance().RobotStage?.Move(new AxisPosition(position.X, position.Y));
            }
        }

        public string Title => "FOV Navigator";

        public PaneType PaneType => PaneType.FovNavi;

        public Control Control => this;
        public DockedLocation DockedLocation => DockedLocation.DockedLeft;

        public void OnPreSelectedInspect()
        {
        }

        public void OnPostSelectedInspect(ProbeResultList probeResultList)
        {
        }

        public void StepChanged(InspectStep inspectStepSel)
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

            fovNavigator.SelectFov(inspectStepSel.StepNo);
            fovNavigator.Invalidate();
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
    }
}
