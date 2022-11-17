using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniEye.Base.UI.ParamControl
{
    public partial class ComputeParamControl : UserControl
    {
        private UserControl selectedAlgorithmParamControl = null;

        //ComputeItem computeProbeItem;
        //ComputeProbe selectedProbe;
        private ObjectTree objectTree;
        public StepModel Model { get; set; }
        public string AlgorithmType { get; set; }
        public double Length { get; set; }

        public ComputeParamControl()
        {
            InitializeComponent();
            AlgorithmType = "Compute Probe";
            objectTree = new ObjectTree();
            UpdateComputeTypeToList(); // compute타입의 리스트를 추가한다.

            labelType.Text = StringManager.GetString(labelType.Text);
            labelTarget1.Text = StringManager.GetString(labelTarget1.Text);
            labelTarget2.Text = StringManager.GetString(labelTarget2.Text);
        }

        public void Initialize(StepModel model)
        {
            Model = model;
        }

        private void UpdateComputeTypeToList()
        {
            computeTypeList.DataSource = Enum.GetValues(typeof(ComputeType));
        }

        private void buttonTarget1_Click(object sender, EventArgs e)
        {
            ShowSelectTree(txtTarget1);
        }

        private void buttonTarget2_Click(object sender, EventArgs e)
        {
            ShowSelectTree(txtTarget2);
        }

        private void ShowSelectTree(TextBox textBox)
        {
            var form = new SelectResultValueForm();
            form.Model = Model;
            objectTree.Initialize(Model);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string valueName = form.ValueName.Replace("ResultValue.", "");
                textBox.Text = valueName;
            }
        }

        public void SetSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "ComputeProbeParamControl - SetSelectedProbe");


            //ComputeProbe computeProbe = (ComputeProbe)probe
            //if (computeProbe.InspAlgorithm.GetAlgorithmType() == WidthChecker.TypeName)
            //{
            //    selectedProbe = visionProbe;
            //    widthChecker = (WidthChecker)visionProbe.InspAlgorithm;
            //    UpdateData();
            //}
            //else
            //    throw new InvalidOperationException();

        }

        private void ShowAlgorithmParamControl(string algorithmType)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            if (selectedAlgorithmParamControl != null)
            {
                selectedAlgorithmParamControl.Hide();
            }

            //this.algorithmParamPanel.Controls.Clear();
            //this.algorithmParamPanel.Controls.Add(selectedAlgorithmParamControl);

            selectedAlgorithmParamControl.Show();

            //if (selectedProbe != null)
            //{
            //    ((IAlgorithmParamControl)selectedAlgorithmParamControl).ClearSelectedProbe();
            //}
        }

        private void ComputeParamControl_Load(object sender, EventArgs e)
        {
            objectTree.Initialize(Model);
        }
    }
}
