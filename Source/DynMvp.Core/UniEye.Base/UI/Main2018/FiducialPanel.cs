using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspectData;
using DynMvp.UI;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Inspect;

namespace UniEye.Base.UI.Main2018
{
    public partial class FiducialPanel : UserControl, IModellerPane
    {
        private CancellationTokenSource cancellationTokenSource;
        private PositionAligner positionAligner;

        public FiducialPanel()
        {
            InitializeComponent();
        }

        public string Title => "Fiducial";

        public PaneType PaneType => PaneType.Fid;

        public Control Control => this;
        public DockedLocation DockedLocation => DockedLocation.DockedLeft;

        private void fidDistanceTol_ValueChanged(object sender, EventArgs e)
        {
            //StepModel stepModel = ModelManager.Instance().CurrentStepModel;
            //stepModel.ModelDescription.FidDistanceTol = Convert.ToInt32(fidDistanceTol.Value);
        }

        private void buttonAlign_Click(object sender, EventArgs e)
        {
            ModelManager.Instance().CurrentStepModel.CleanImage();

            AlignPosition();
        }


        public void AlignPosition()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var loadingForm = new SimpleProgressForm("Align");
            loadingForm.Show(new Action(AlignPositionProc), cancellationTokenSource);

            // InspectStepChanged(0);
        }

        private void AlignPositionProc()
        {
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

            positionAligner = fiducialSet.Calculate(inspectRunner.ProductResult);

            if (positionAligner != null)
            {
                DisplayAlignInfo(positionAligner);
            }
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
    }
}
