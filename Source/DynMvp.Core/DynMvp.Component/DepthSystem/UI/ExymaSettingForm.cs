using DynMvp.Base;
using DynMvp.Devices.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.UI
{
    public partial class ExymaSettingForm : Form
    {
        public ExymaSettingForm()
        {
            InitializeComponent();
            labelName.Text = StringManager.GetString(labelName.Text);
            labelCenterCameraIndex.Text = StringManager.GetString(labelCenterCameraIndex.Text);
            labelCameraIndex.Text = StringManager.GetString(labelCameraIndex.Text);
            labelCamera2Index.Text = StringManager.GetString(labelCamera2Index.Text);
            labelControlBoardType.Text = StringManager.GetString(labelControlBoardType.Text);
            buttonControlBoard1Setting.Text = StringManager.GetString(buttonControlBoard1Setting.Text);
            buttonControlBoard2Setting.Text = StringManager.GetString(buttonControlBoard2Setting.Text);
            labelMeasureTriggerMode.Text = StringManager.GetString(labelMeasureTriggerMode.Text);
            labelMeasureMode.Text = StringManager.GetString(labelMeasureMode.Text);
            labelNoiseLevel.Text = StringManager.GetString(labelNoiseLevel.Text);
            labelGain.Text = StringManager.GetString(labelGain.Text);
            labelOffset.Text = StringManager.GetString(labelOffset.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);


        }
        public ExymaScannerInfo ExymaScannerInfo { get; set; }
        public int NumCamera { get; set; }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ExymaScannerInfo.Name = name.Text;
            ExymaScannerInfo.CenterCameraIndex = Convert.ToInt32(centerCameraIndex.Text);
            ExymaScannerInfo.CameraIndex = Convert.ToInt32(cameraIndex.Text);
            ExymaScannerInfo.Camera2Index = Convert.ToInt32(camera2Index.Text);
        }

        private void ExymaSettingForm_Load(object sender, EventArgs e)
        {
            for (int i = -1; i < NumCamera; i++)
            {
                centerCameraIndex.Items.Add(i.ToString());
                cameraIndex.Items.Add(i.ToString());
                camera2Index.Items.Add(i.ToString());
            }

            name.Text = ExymaScannerInfo.Name;
            centerCameraIndex.Text = ExymaScannerInfo.CenterCameraIndex.ToString();
            cameraIndex.Text = ExymaScannerInfo.CameraIndex.ToString();
            camera2Index.Text = ExymaScannerInfo.Camera2Index.ToString();
        }

        private void buttonControlBoard1Setting_Click(object sender, EventArgs e)
        {
            var form = new SerialPortSettingForm();
            form.SerialPortInfo = ExymaScannerInfo.ControlBoardSerialInfo;
            form.EnablePortNo = true;
            form.ShowDialog();
        }

        private void buttonControlBoard2Setting_Click(object sender, EventArgs e)
        {
            var form = new SerialPortSettingForm();
            form.SerialPortInfo = ExymaScannerInfo.ControlBoardSerialInfo2;
            form.EnablePortNo = true;
            form.ShowDialog();
        }
    }
}
