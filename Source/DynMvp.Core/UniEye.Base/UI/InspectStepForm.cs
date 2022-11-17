using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using System;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public partial class InspectStepForm : Form
    {
        private InspectStep inspectStep;

        public InspectStepForm()
        {
            InitializeComponent();
            labelStepName.Text = StringManager.GetString(labelStepName.Text);
            labelRobotPosition.Text = StringManager.GetString(labelRobotPosition.Text);
            columnAxis.HeaderText = StringManager.GetString(columnAxis.HeaderText);
            columnPosition.HeaderText = StringManager.GetString(columnPosition.HeaderText);
            okButton.Text = StringManager.GetString(okButton.Text);
            cancelButton.Text = StringManager.GetString(cancelButton.Text);
            refreshButton.Text = StringManager.GetString(refreshButton.Text);
        }

        public void Initialize(InspectStep inspectStep)
        {
            this.inspectStep = inspectStep;
        }

        private void InspectionStepForm_Load(object sender, EventArgs e)
        {
            if (inspectStep == null)
            {
                return;
            }

            stepName.Text = inspectStep.Name;

            AxisHandler robotStage = DeviceManager.Instance().RobotStage;

            if (robotStage != null)
            {
                positionGridView.Rows.Clear();
                foreach (Axis axis in robotStage.UniqueAxisList)
                {
                    positionGridView.Rows.Add(axis.Name);
                }
            }

            if (inspectStep.Position == null)
            {
                if (robotStage != null)
                {
                    UpdatePosition(robotStage.GetActualPos());
                }
            }
            else
            {
                UpdatePosition(inspectStep.Position);
            }
        }

        private void UpdatePosition(AxisPosition axisPosition)
        {
            int index = 0;
            foreach (float value in axisPosition.Position)
            {
                positionGridView.Rows[index].Cells[1].Value = value;
                index++;
            }
        }

        private void MovePosition()
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            if (robotStage == null)
            {
                return;
            }

            int countRow = positionGridView.Rows.Count;
            int i = 0;
            float[] pos = new float[countRow];
            foreach (DataGridViewRow row in positionGridView.Rows)
            {
                pos[i++] = Convert.ToSingle(row.Cells[1].Value.ToString());
            }

            var axisPosition = new AxisPosition(pos);
            robotStage.Move(axisPosition);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            //UpdatePosition(robotStage.GetActualPos());
            UpdatePosition(robotStage.GetCommandPos());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            inspectStep.Name = stepName.Text;

            var axisPosition = new AxisPosition(positionGridView.Rows.Count);

            for (int i = 0; i < axisPosition.NumAxis; i++)
            {
                axisPosition[i] = Convert.ToSingle(positionGridView.Rows[i].Cells[1].Value.ToString());
            }

            //inspectStep.AlignedPosition = axisPosition;
            inspectStep.Position = axisPosition;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            MovePosition();
        }
    }
}
