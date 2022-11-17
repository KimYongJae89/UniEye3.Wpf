using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class LightConfigForm : Form
    {
        public DigitalIoHandler DigitalIoHandler { get; set; }
        public LightCtrlInfo LightCtrlInfo { get; set; }
        public string LightCtrlName { get; set; }

        private SerialPortInfo serialPortInfo = new SerialPortInfo();
        private bool initialized = false;

        public LightConfigForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelNumLight.Text = StringManager.GetString(labelNumLight.Text);
            useIoLightCtrl.Text = StringManager.GetString(useIoLightCtrl.Text);
            useSerialLightCtrl.Text = StringManager.GetString(useSerialLightCtrl.Text);
            buttonEditLightCtrlPort.Text = StringManager.GetString(buttonEditLightCtrlPort.Text);
            buttonTestLightController.Text = StringManager.GetString(buttonTestLightController.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;

            labelSerialPortInfo.Text = "";
            LightCtrlName = "LightController";

            comboLightControllerVender.DataSource = Enum.GetNames(typeof(LightCtrlSubType));
        }

        private void LightConfigForm_Load(object sender, EventArgs e)
        {
            if (LightCtrlInfo != null)
            {
                LoadInfo();
            }
            else
            {
                txtName.Text = LightCtrlName;
                numLight.Value = 1;
                useIoLightCtrl.Checked = true;
            }

            initialized = true;
        }

        private void LoadInfo()
        {
            if (LightCtrlInfo.Type == LightCtrlType.Serial)
            {
                var serialLightCtrlInfo = (SerialLightCtrlInfo)LightCtrlInfo;

                useSerialLightCtrl.Checked = true;
                comboLightControllerVender.Enabled = true;
                buttonEditLightCtrlPort.Enabled = true;

                comboLightControllerVender.SelectedIndex = (int)serialLightCtrlInfo.SubType;

                serialPortInfo = serialLightCtrlInfo.SerialPortInfo;
                labelSerialPortInfo.Text = serialPortInfo.ToString();
            }
            else if (LightCtrlInfo.Type == LightCtrlType.IO)
            {
                useIoLightCtrl.Checked = true;
            }

            txtName.Text = LightCtrlInfo.Name;
            numLight.Value = LightCtrlInfo.NumChannel;
        }

        private void useIoLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
            {
                return;
            }

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;
        }

        private void buttonEditLightCtrlPort_Click(object sender, EventArgs e)
        {
            var form = new SerialPortSettingForm();
            form.SerialPortInfo = serialPortInfo.Clone();
            form.EnablePortNo = true;
            if (form.ShowDialog() == DialogResult.OK)
            {
                serialPortInfo = form.SerialPortInfo;
                labelSerialPortInfo.Text = serialPortInfo.ToString();
            }
        }

        private LightCtrlInfo CreateLightCtrlInfo()
        {
            var lightCtrlType = (LightCtrlSubType)comboLightControllerVender.SelectedIndex;
            //LightCtrlInfo lightCtrlInfo = null;
            if (useIoLightCtrl.Checked)
            {
                LightCtrlInfo = LightCtrlInfoFactory.Create(LightCtrlType.IO, lightCtrlType);
            }
            else if (useSerialLightCtrl.Checked)
            {
                if (LightCtrlInfo == null)
                {
                    var serialLightCtrlInfo = (SerialLightCtrlInfo)LightCtrlInfoFactory.Create(LightCtrlType.Serial, lightCtrlType);
                    serialLightCtrlInfo.SubType = (LightCtrlSubType)comboLightControllerVender.SelectedIndex;
                    serialLightCtrlInfo.SerialPortInfo = serialPortInfo;
                    LightCtrlInfo = serialLightCtrlInfo;
                }
                else
                {
                    var serialLightCtrlInfo = (SerialLightCtrlInfo)LightCtrlInfo;
                    serialLightCtrlInfo.SerialPortInfo = serialPortInfo;
                }
            }

            if (LightCtrlInfo != null)
            {
                LightCtrlInfo.Name = txtName.Text;
                LightCtrlInfo.NumChannel = (int)numLight.Value;
            }

            return LightCtrlInfo;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            LightCtrlInfo = CreateLightCtrlInfo();
        }

        private void useSerialLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
            {
                return;
            }

            buttonEditLightCtrlPort.Enabled = true;
            comboLightControllerVender.Enabled = true;
        }

        private void buttonTestLightController_Click(object sender, EventArgs e)
        {
            LightCtrlInfo lightCtrlInfo = CreateLightCtrlInfo();
            LightCtrl lightCtrl = LightCtrlFactory.Create(lightCtrlInfo, DigitalIoHandler, false);
            if (lightCtrl == null)
            {
                return;
            }

            var lightValue = new LightValue(lightCtrlInfo.NumChannel, lightCtrl.GetMaxLightLevel());
            lightCtrl.TurnOn(lightValue);

            MessageBox.Show("Please, check the light");

            lightValue.TurnOff();
            lightCtrl.TurnOn(lightValue);

            //lightCtrl.TurnOff();

            lightCtrl.Release();
        }

        private void buttonAdvance_Click(object sender, EventArgs e)
        {
            if (LightCtrlInfo != null)
            {
                Form advForm = LightCtrlInfo.GetAdvancedConfigForm();
                if (advForm == null)
                {
                    return;
                }

                advForm.ShowDialog();
            }
        }
    }
}
