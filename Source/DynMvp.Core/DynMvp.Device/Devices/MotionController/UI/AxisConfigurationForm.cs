using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices.MotionController.UI
{
    public partial class AxisConfigurationForm : Form
    {
        private int preSelectedIndex = -1;
        private List<AxisHandler> axisHandlerList;
        private MotionHandler motionHandler;
        private bool lockUpdate = false;
        private bool survoStateOn = true;

        public AxisConfigurationForm()
        {
            InitializeComponent();

            moveUpButton.Text = StringManager.GetString(moveUpButton.Text);
            moveDownButton.Text = StringManager.GetString(moveDownButton.Text);
            homeMoveButton.Text = StringManager.GetString(homeMoveButton.Text);
            jogPlusButton.Text = StringManager.GetString(jogPlusButton.Text);
            jogMinusButton.Text = StringManager.GetString(jogMinusButton.Text);
            setOriginOffsetButton.Text = StringManager.GetString(setOriginOffsetButton.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        public void Initialize(List<AxisHandler> axisHandlerList, MotionHandler motionList)
        {
            lockUpdate = true;

            this.axisHandlerList = axisHandlerList;
            motionHandler = motionList;

            columnAxisName.DataSource = Enum.GetNames(typeof(AxisName));

            columnMotionName.Items.Clear();
            foreach (Motion motion in motionList)
            {
                columnMotionName.Items.Add(motion.Name);
            }

            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                int index = axisHandlerGridView.Rows.Add(axisHandler.Name);
                axisHandlerGridView.Rows[index].Tag = axisHandler;
            }

            lockUpdate = false;

            axisHandlerGridView.Rows[0].Selected = true;

            UpdateAxisList();

            positionUpdateTimer.Start();

            preSelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            UpdateAxisHandler();
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(axisList);
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(axisList);
        }

        private Motion GetMotion()
        {
            object value = axisList.SelectedRows[0].Cells[1].Value;
            if (value == null)
            {
                return null;
            }

            return motionHandler.GetMotion(value.ToString());
        }

        private int GetAxisNo()
        {
            object value = axisList.SelectedRows[0].Cells[2].Value;
            if (value == null)
            {
                return 0;
            }

            return Convert.ToInt32(value.ToString());
        }

        private void jogPlusButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                var movingParam = new MovingParam("", 1000, 100, 100, 10000, 0);
                motion.StartRelativeMove(axisNo, 5000, movingParam);
            }
        }

        private void jogMinusButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                var movingParam = new MovingParam("", 1000, 100, 100, 10000, 0);
                motion.StartRelativeMove(axisNo, -5000, movingParam);
                //while (motion.IsMoveDone(axisNo) == false) ;
            }
        }

        private void UpdateAxisHandler()
        {
            if (preSelectedIndex > -1)
            {
                var preAxisHandler = (AxisHandler)axisHandlerGridView.Rows[preSelectedIndex].Tag;
                preAxisHandler.Clear();

                for (int i = 0; i < axisList.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = axisList.Rows[i].Cells;
                    if (cells[0].Value == null)
                    {
                        break;
                    }

                    string axisName = cells[0].Value.ToString();
                    Motion motion = motionHandler.GetMotion(cells[1].Value.ToString());
                    int axisNo = Convert.ToInt32(cells[2].Value.ToString());
                    float originPulse = 0;
                    if (cells[3].Value != null)
                    {
                        originPulse = Convert.ToSingle(cells[3].Value.ToString());
                    }

                    if (motion != null)
                    {
                        var axis = (Axis)axisList.Rows[i].Tag;
                        if (axis == null)
                        {
                            axis = preAxisHandler.AddAxis(axisName, motion, axisNo);
                        }
                        else
                        {
                            preAxisHandler.AddAxis(axis);
                            axis.Update(axisName, motion, axisNo);
                        }

                        axis.AxisParam.OriginPulse = originPulse;
                    }
                }
            }
        }

        private void UpdateAxisList()
        {
            axisList.Rows.Clear();

            var axisHandler = (AxisHandler)axisHandlerGridView.SelectedRows[0].Tag;
            for (int i = 0; i < axisHandler.NumAxis; i++)
            {
                Axis axis = axisHandler[i];

                string motionName = "";
                if (axis.Motion != null)
                {
                    motionName = axis.Motion.Name;
                }

                int rowIndex = axisList.Rows.Add(axis.Name, motionName, axis.AxisNo, axis.AxisParam.OriginPulse);
                axisList.Rows[rowIndex].Tag = axis;
            }
        }

        private void axisHandlerList_SelectionChanged(object sender, EventArgs e)
        {
            if (axisHandlerGridView.SelectedRows.Count == 0 || lockUpdate == true)
            {
                return;
            }

            UpdateAxisHandler();

            UpdateAxisList();
        }

        private void setOriginOffsetButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                var movingParam = new MovingParam("", 1000, 100, 100, 10000, 0);
                float axisPosition = motion.GetActualPos(axisNo);

                axisList.SelectedRows[0].Cells[3].Value = axisPosition.ToString();
            }
        }

        private void homeMoveButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                var homeParam = new HomeParam();
                homeParam.HighSpeed = new MovingParam("", 5000, 100, 100, 5000 * 2, 0);
                homeParam.MediumSpeed = new MovingParam("", 3000, 100, 100, 3000 * 2, 0);
                homeParam.FineSpeed = new MovingParam("", 1000, 100, 100, 1000 * 2, 0);

                motion.HomeMove(axisNo, homeParam);
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        private void positionUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                position.Text = motion.GetCommandPos(axisNo).ToString();
                //position.Text = motion.GetActualPos(axisNo).ToString();
            }
        }

        private void survoButton_Click(object sender, EventArgs e)
        {
            if (axisList.SelectedRows.Count == 0)
            {
                return;
            }

            int axisNo = GetAxisNo();

            Motion motion = GetMotion();
            if (motion != null)
            {
                survoStateOn = !survoStateOn;
                motion.TurnOnServo(axisNo, survoStateOn);
                survoButton.Text = string.Format("Survo {0}", survoStateOn ? "OFF" : "ON");
            }
        }
    }
}
