using DynMvp.Base;
using DynMvp.InspectData;
using DynMvp.Vision;
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
    public partial class ObjectTree : TreeView
    {
        private string inspectionStepName = "InspectionStep";

        public string InspectionStepName
        {
            set => inspectionStepName = value;
        }
        public bool IncludeTargetType { get; set; } = true;

        public ObjectTree()
        {
            InitializeComponent();
            inspectionStepName = StringManager.GetString(inspectionStepName);
        }

        public void Initialize(StepModel model)
        {
            TreeNode modelNode = Nodes.Add(StringManager.GetString("Current Model"));

            List<InspectStep> inspectStepList = model.GetInspectStepList();
            foreach (InspectStep inspectStep in inspectStepList)
            {
                BuildInspectionStep(inspectStep, modelNode);
            }

            if (IncludeTargetType)
            {
                var targetTypeList = new List<string>();
                model.GetTargetTypes(targetTypeList);

                BuildTargetType(targetTypeList, modelNode);
            }
        }

        private void BuildTargetType(List<string> targetTypeList, TreeNode modelNode)
        {
            TreeNode targetTypeRootNode = modelNode.Nodes.Add(StringManager.GetString("TargetTypes"));

            foreach (string targetType in targetTypeList)
            {
                TreeNode targetTypeNode = targetTypeRootNode.Nodes.Add(targetType);
                targetTypeNode.Tag = "TargetType." + targetType;
            }
        }

        public void BuildInspectionStep(InspectStep inspectStep, TreeNode modelNode)
        {
            TreeNode inspectionStepNode = modelNode.Nodes.Add(string.Format("{0} {1}", inspectionStepName, inspectStep.StepNo + 1));

            foreach (Target target in inspectStep)
            {
                BuildTargetTree(target, inspectionStepNode);
            }
        }

        public void BuildTargetTree(Target target, TreeNode targetGroupNode)
        {
            TreeNode targetNode = targetGroupNode.Nodes.Add(target.Name);
            targetNode.Tag = target;

            foreach (Probe probe in target)
            {
                BuildProbeTree(probe, targetNode);
            }
        }

        public void BuildProbeTree(Probe probe, TreeNode targetNode)
        {
            TreeNode probeNode = targetNode.Nodes.Add(probe.Name);
            probeNode.Tag = probe;

            List<ResultValue> resultValueList = probe.GetResultValues();
            foreach (ResultValue probeResultValue in resultValueList)
            {
                TreeNode probeResultValueNode = probeNode.Nodes.Add(probeResultValue.Name);
                probeResultValueNode.Tag = "ResultValue." + probeResultValue.Name;
            }
        }
    }
}
