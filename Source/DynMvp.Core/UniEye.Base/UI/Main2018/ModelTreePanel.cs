using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspectData;
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
using UniEye.Base.Data;

namespace UniEye.Base.UI.Main2018
{
    public partial class ModelTreePanel : UserControl, IModellerPane
    {
        public ModelTreePanel()
        {
            InitializeComponent();
        }

        public string Title => "Model";

        public PaneType PaneType => PaneType.ModelTree;

        public Control Control => this;

        public DockedLocation DockedLocation => DockedLocation.DockedLeft;

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

        public void UpdatetreeViewModel()
        {
            if (!(ModelManager.Instance().CurrentModel is StepModel stepModel))
            {
                return;
            }

            treeViewModel.Nodes.Clear();

            var treeImageList = new ImageList();
            treeImageList.Images.Add(global::UniEye.Base.Properties.Resources.Folder__Adorner_in_Front_64);
            treeImageList.Images.Add(global::UniEye.Base.Properties.Resources.Folder__Adorner_in_front__Edit64);

            treeViewModel.ImageList = treeImageList;
            treeViewModel.ImageIndex = 0;
            treeViewModel.SelectedImageIndex = 1;

            List<InspectStep> inspectSteps = stepModel.GetInspectStepList();
            foreach (InspectStep inspectStep in inspectSteps)
            {
                var stepNode = new TreeNode(inspectStep.GetStepName());//, 0, 1);
                treeViewModel.Nodes.Add(stepNode);
            }
        }

        private void modelPropertyButton_Click(object sender, EventArgs e)
        {
            UpdatetreeViewModel();
        }
    }
}
