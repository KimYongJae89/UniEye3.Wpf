using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public delegate void TargetClickedDelegate(Target target);
    public delegate void TargetDoubleClickedDelegate(Target target);

    public partial class ModelTreeControl : UserControl
    {
        private StepModel model;
        public StepModel Model
        {
            set => model = value;
        }

        public ModelTreeControl()
        {
            InitializeComponent();
        }

        private TargetClickedDelegate targetClicked;
        public TargetClickedDelegate TargetClicked
        {
            set => targetClicked = value;
        }

        private TargetDoubleClickedDelegate targetDoubleClicked;
        public TargetDoubleClickedDelegate TargetDoubleClicked
        {
            set => targetDoubleClicked = value;
        }

        public void Update(StepModel model)
        {
            objectTree.Nodes.Clear();

            this.model = model;

            TreeNode modelNode = objectTree.Nodes.Add("Current Model");
            foreach (InspectStep inspectStep in model)
            {
                BuildInspectionStepTree(inspectStep, modelNode);
            }
        }

        public void BuildInspectionStepTree(InspectStep inspectStep, TreeNode modelNode)
        {
            TreeNode inspectStepNode = modelNode.Nodes.Add(string.Format("InspectionStep{0}", inspectStep.StepNo));

            foreach (Target target in inspectStep)
            {
                TreeNode targetNode = inspectStepNode.Nodes.Add(target.Name);
                targetNode.Tag = target;
            }
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var target = (Target)objectTree.SelectedNode.Tag;

            if (target != null)
            {
                if (targetImage.Image != null)
                {
                    targetImage.Image.Dispose();
                }

                targetImage.Image = target.Image.ToBitmap();

                targetClicked?.Invoke(target);
            }
            else
            {
                targetImage.Image = null;
            }
        }

        private void objectTree_DoubleClick(object sender, EventArgs e)
        {
            var target = (Target)objectTree.SelectedNode.Tag;

            if (target != null && targetDoubleClicked != null)
            {
                targetDoubleClicked(target);
            }
        }

        public Target GetSelectedTarget()
        {
            return (Target)objectTree.SelectedNode.Tag;
        }
    }
}
