using DynMvp.Base;
using DynMvp.Devices;
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
    public partial class MotionSpeedForm : Form
    {
        private AxisHandler curAxisHandler;
        private AxisParam axisParam = new AxisParam();

        public MotionSpeedForm()
        {
            InitializeComponent();

            labelAxisHandler.Text = StringManager.GetString(labelAxisHandler.Text);
            labelAxisNo.Text = StringManager.GetString(labelAxisNo.Text);
            labelJog.Text = StringManager.GetString(labelJog.Text);
            okbutton.Text = StringManager.GetString(okbutton.Text);
        }

        public void Intialize(AxisHandler curAxisHandler = null)
        {
            this.curAxisHandler = curAxisHandler;
        }

        private void MotionControlForm_Load(object sender, EventArgs e)
        {
            List<AxisHandler> axisHandlerList = DeviceManager.Instance().AxisHandlerList;

            int axisHandlerIndex = 0;
            int index = 0;
            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                comboAxisHandler.Items.Add(axisHandler);

                if (curAxisHandler != null && axisHandler == curAxisHandler)
                {
                    axisHandlerIndex = index;
                }

                index++;
            }

            comboAxisHandler.SelectedIndex = axisHandlerIndex;
            movingStep.SelectedIndex = 2;
            paramPropertyGrid.SelectedObject = axisParam;
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveAxis(Convert.ToInt32(movingStep.Text));
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveAxis(Convert.ToInt32(movingStep.Text) * (-1));
        }

        private void MoveAxis(float pos)
        {
            curAxisHandler.RelativeMove(comboAxis.Text, pos);
        }

        private void axisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxis.SelectedIndex;
            var axis = (Axis)comboAxis.Items[selectedIndex];

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
            List<AxisHandler> axisHandlerList = DeviceManager.Instance().AxisHandlerList;

            string configDir = DeviceConfig.Instance().ConfigPath;

            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                axisHandler.Save(configDir);
                axisHandler.TurnOnServo(true);
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

        private void comboAxisHandler_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboAxisHandler.SelectedIndex;
            curAxisHandler = (AxisHandler)comboAxisHandler.Items[selectedIndex];

            for (int i = 0; i < curAxisHandler.NumAxis; i++)
            {
                comboAxis.Items.Add(curAxisHandler[i]);
            }

            comboAxis.SelectedIndex = 0;
        }
    }
}
