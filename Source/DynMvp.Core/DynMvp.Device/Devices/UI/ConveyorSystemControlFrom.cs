using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class ConveyorSystemControlForm : Form
    {
        private ConveyorSystem conveyorSystem;

        public ConveyorSystemControlForm()
        {
            InitializeComponent();
        }

        public void Initialize(ConveyorSystem conveyorSystem)
        {
            this.conveyorSystem = conveyorSystem;
        }

        private void ConveyorSystemControlForm_Load(object sender, EventArgs e)
        {

        }

        private void UpdateControlForm()
        {
            if (conveyorSystem == null)
            {
                return;
            }

            labelConveyorState.Text = conveyorSystem.CurrentState.ToString();
            UpdateInputLedSensor(conveyorSystem.EntrySensor.IsSignalOn(), ref labelEntry);
            UpdateInputLedSensor(conveyorSystem.SpeedDownSensor.IsSignalOn(), ref labelSpeedDown);
            UpdateInputLedSensor(conveyorSystem.ReadySensor.IsSignalOn(), ref labelReady);
            UpdateInputLedSensor(conveyorSystem.EjectSensor.IsSignalOn(), ref labelEject);
            UpdateInputLedSensor(conveyorSystem.NextMachineAvailableSensor.IsSignalOn(), ref labelNextAvailable);

            UpdateInputLedSensor(conveyorSystem.ClampUpState, ref labelClampUp);
            UpdateInputLedSensor(conveyorSystem.SidePusherForwardState, ref labelSidePusherOn);
            UpdateInputLedSensor(conveyorSystem.StopperUpState, ref labelStopperUp);
        }

        private void UpdateInputLedSensor(bool signalOn, ref Label label)
        {
            if (signalOn == true)
            {
                label.Image = global::DynMvp.Devices.Properties.Resources.led_on;
            }
            else
            {
                label.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            }
        }

        private void timerSensorUpdate_Tick(object sender, EventArgs e)
        {
            UpdateControlForm();
        }

        private void buttonUpdateStart_Click(object sender, EventArgs e)
        {
            if (timerSensorUpdate.Enabled == false)
            {
                timerSensorUpdate.Start();
                buttonUpdateStart.Text = "Update Stop";
            }
            else
            {
                timerSensorUpdate.Stop();
                buttonUpdateStart.Text = "Update Start";
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            conveyorSystem?.StartWorkingProc();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            conveyorSystem?.StopWorkingProc();
        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            conveyorSystem.WaitReceiving = true;
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            conveyorSystem.EnterFlush();
        }

        private void buttonEject_Click(object sender, EventArgs e)
        {

        }

        private void buttonPause_Click(object sender, EventArgs e)
        {

        }
    }
}
