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
    public partial class Joystick2AxisControl : UserControl, IJoystickControl
    {
        private AxisHandler axisHandler;
        private AxisPosition currentPosition;
        private MovableCheckDelegate movableCheck = null;
        private int stepOffset = 0;
        private int axisNo = -1;
        private bool startMove = false;

        public Joystick2AxisControl()
        {
            InitializeComponent();

            moveStep.Text = "1000";
        }

        public void InitControl()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Size = new System.Drawing.Size(409, 523);
            TabIndex = 8;
            TabStop = false;
        }

        public void Initialize(AxisHandler axisHandler)
        {
            this.axisHandler = axisHandler;
            currentPosition = new AxisPosition();
        }

        public void MoveAxis(int axisNo, int direction)
        {
            if (movableCheck != null && movableCheck() == false)
            {
                return;
            }

            startMove = true;
            axisHandler.StartContinuousMove(axisNo, direction == -1);
        }

        public void MoveAxis(int axisNo, int direction, int step)
        {
            if (movableCheck != null && movableCheck() == false)
            {
                return;
            }

            currentPosition = axisHandler.GetCommandPos();

            stepOffset = direction * step;
            this.axisNo = axisNo;

            startMove = true;
            currentPosition[axisNo] += stepOffset;
            axisHandler.Move(currentPosition);
            startMove = false;

        }

        public void StopAxis()
        {
            if (startMove == true)
            {
                axisHandler.StopMove();
                startMove = false;
            }
        }

        private void buttonAll_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkStepMove.Checked == true)
            {
                return;
            }

            if (startMove)
            {
                StopAxis();
            }
        }

        private void buttonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkStepMove.Checked == true)
            {
                return;
            }

            MoveAxis((int)AxisName.X, (int)Direction.Negative);
        }

        private void buttonRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkStepMove.Checked == true)
            {
                return;
            }

            MoveAxis((int)AxisName.X, (int)Direction.Positive);
        }

        private void buttonUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkStepMove.Checked == true)
            {
                return;
            }

            MoveAxis((int)AxisName.Y, (int)Direction.Positive);
        }

        private void buttonDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkStepMove.Checked == true)
            {
                return;
            }

            MoveAxis((int)AxisName.Y, (int)Direction.Negative);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (checkStepMove.Checked == false)
            {
                return;
            }

            buttonUp.Enabled = false;
            MoveAxis((int)AxisName.Y, (int)Direction.Positive, Convert.ToInt32(moveStep.Text));
            buttonUp.Enabled = true;
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (checkStepMove.Checked == false)
            {
                return;
            }

            buttonRight.Enabled = false;
            MoveAxis((int)AxisName.X, (int)Direction.Positive, Convert.ToInt32(moveStep.Text));
            buttonRight.Enabled = true;
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (checkStepMove.Checked == false)
            {
                return;
            }

            buttonDown.Enabled = false;
            MoveAxis((int)AxisName.Y, (int)Direction.Negative, Convert.ToInt32(moveStep.Text));
            buttonDown.Enabled = true;
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (checkStepMove.Checked == false)
            {
                return;
            }

            buttonLeft.Enabled = false;
            MoveAxis((int)AxisName.X, (int)Direction.Negative, Convert.ToInt32(moveStep.Text));
            buttonLeft.Enabled = true;
        }

        public void SetMovableCheckDelegate(MovableCheckDelegate movableCheckDelegate)
        {
            movableCheck = movableCheckDelegate;
        }

        public MovableCheckDelegate GetMovableCheckDelegate()
        {
            return movableCheck;
        }
    }
}