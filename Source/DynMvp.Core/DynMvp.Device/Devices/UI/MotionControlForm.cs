using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class MotionControlForm : Form
    {
        private List<AxisHandler> axisHandlerList;
        private AxisHandler curAxisHandler;
        private AxisParam axisParam = new AxisParam();

        public MotionControlForm()
        {
            InitializeComponent();

            labelAxisHandler.Text = StringManager.GetString(labelAxisHandler.Text);
            labelAxisNo.Text = StringManager.GetString(labelAxisNo.Text);
            labelPosition.Text = StringManager.GetString(labelPosition.Text);
            labelStep.Text = StringManager.GetString(labelStep.Text);
            labelMotionDoneWaitTime.Text = StringManager.GetString(labelMotionDoneWaitTime.Text);
            moveButton.Text = StringManager.GetString(moveButton.Text);
            originButton.Text = StringManager.GetString(originButton.Text);
            buttonFindLimit.Text = StringManager.GetString(buttonFindLimit.Text);
            okbutton.Text = StringManager.GetString(okbutton.Text);
        }

        public void Intialize(List<AxisHandler> axisHandlerList)
        {
            this.axisHandlerList = axisHandlerList;
        }

        private void MotionControlForm_Load(object sender, EventArgs e)
        {
            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                comboAxisHandler.Items.Add(axisHandler);
            }

            comboAxisHandler.SelectedIndex = 0;
            movingStep.SelectedIndex = 2;
            paramPropertyGrid.SelectedObject = axisParam;

            checkTimer.Start();
        }

        private void comboAxisHandler_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxisHandler.SelectedIndex;
            curAxisHandler = (AxisHandler)comboAxisHandler.Items[selectedIndex];

            comboAxis.Items.Clear();
            for (int i = 0; i < curAxisHandler.NumAxis; i++)
            {
                comboAxis.Items.Add(curAxisHandler[i]);
            }

            comboAxis.SelectedIndex = 0;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)position.Value);
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)(position.Value + Convert.ToInt32(movingStep.Text)));
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveAxis((float)(position.Value - Convert.ToInt32(movingStep.Text)));
        }

        private void MoveAxis(float pos)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            var axis = (Axis)comboAxis.Items[selectedIndex];

            axis.Move(pos, null, true);

            position.Value = (long)axis.GetActualPulse();
        }

        private void originButton_Click(object sender, EventArgs e)
        {
            originButton.Enabled = false;

            int selectedIndex = comboAxis.SelectedIndex;
            var axis = (Axis)comboAxis.Items[selectedIndex];
            axis.HomeMove();

            position.Value = 0;

            originButton.Enabled = true;
        }

        private void axisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            var axis = (Axis)comboAxis.Items[selectedIndex];

            position.Value = (long)axis.GetActualPulse();
            axisParam = axis.AxisParam;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void MotionControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modal == false)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                axisHandler.Save(BaseConfig.Instance().ConfigPath);
            }

            if (Modal == true)
            {
                Close();
            }
            else
            {
                Hide();
            }
        }

        private void buttonFindLimit_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            var axis = (Axis)comboAxis.Items[selectedIndex];

            float offset = Convert.ToInt32(movingStep.Text);
            MovingParam movingParam = axisParam.MovingParam.Clone();
            //movingParam.MaxVelocity *= -1;
            movingParam.MaxVelocity /= 2;

            axis.ContinuousMove(movingParam, true);

            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(10000);

            try
            {
                while (axis.IsNegativeOn() == false)
                {
                    timeOutTimer.ThrowIfTimeOut();

                    Thread.Sleep(5);
                }
            }
            catch (TimeoutException)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.CantFindNegLimit, ErrorLevel.Error,
                    ErrorSection.Motion.ToString(), MotionError.CantFindNegLimit.ToString(), "Can't Find Neg Limit.");
            }
            axis.StopMove();
            Thread.Sleep(500);

            axis.AxisParam.NegativeLimit = axis.GetActualPulse() /*+ offset*/;

            //movingParam.MaxVelocity *= -1;
            axis.ContinuousMove(movingParam);
            timeOutTimer.Start(20000);

            try
            {
                while (axis.IsPositiveOn() == false)
                {
                    timeOutTimer.ThrowIfTimeOut();

                    Thread.Sleep(5);
                }
            }
            catch (TimeoutException)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.CantFindPosLimit, ErrorLevel.Error,
                    ErrorSection.Motion.ToString(), MotionError.CantFindPosLimit.ToString(), "Can't Find Pos Limit.");
            }

            axis.StopMove();
            Thread.Sleep(500);

            axis.AxisParam.PositiveLimit = axis.GetActualPulse() /*- offset*/;
            axis.HomeMove();

            axisParam = axis.AxisParam;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {
            var axis = (Axis)comboAxis.Items[comboAxis.SelectedIndex];

            MotionStatus motionStatus = axis.GetMotionStatus();

            labelInp.BackColor = (motionStatus.inp ? Color.LightGreen : Color.Transparent);
            labelAlarm.BackColor = (motionStatus.alarm ? Color.Red : Color.Transparent);
            labelEmg.BackColor = (motionStatus.emg ? Color.Red : Color.Transparent);
            labelErr.BackColor = (motionStatus.err ? Color.Red : Color.Transparent);
            labelEz.BackColor = (motionStatus.ez ? Color.LightGreen : Color.Transparent);
            labelHome.BackColor = (motionStatus.home ? Color.LightGreen : Color.Transparent);
            labelLimitNeg.BackColor = (motionStatus.negLimit ? Color.Red : Color.Transparent);
            labelOrg.BackColor = (motionStatus.origin ? Color.LightGreen : Color.Transparent);
            labelLimitPos.BackColor = (motionStatus.posLimit ? Color.Red : Color.Transparent);
            labelRun.BackColor = (motionStatus.run ? Color.LightGreen : Color.Transparent);
            labelSon.BackColor = (motionStatus.servoOn ? Color.LightGreen : Color.Transparent);
        }

        private void buttonServoOn_Click(object sender, EventArgs e)
        {
            var axis = (Axis)comboAxis.Items[comboAxis.SelectedIndex];
            axis.TurnOnServo(axis.IsServoOn() == false);
        }
    }
}
