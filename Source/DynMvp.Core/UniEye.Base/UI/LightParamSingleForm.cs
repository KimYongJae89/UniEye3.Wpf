using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class LightParamSingleForm : Form, ILightParamForm
    {
        private StepModel curModel;
        private InspectStep curInspectStep;
        private int curCameraIndex = -1;
        public int ExposureTimeUs { get; set; }
        public LightParam LightParam { get; set; }

        //bool onValueUpdate = false;
        public LightTypeChangedDelegate LightTypeChanged { get; set; }
        public LightValueChangedDelegate LightValueChanged { get; set; }

        public int LightTypeIndex => 0;

        public LightParamSingleForm()
        {
            InitializeComponent();

            labelExposure.Text = StringManager.GetString(labelExposure.Text);
            applyLightButton.Text = StringManager.GetString(applyLightButton.Text);
        }

        public void Initialize()
        {

        }

        public void InitControls()
        {
            InitLightList();
        }

        private void InitLightList()
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightList");

            string[] lightDeviceNameList = LightConfig.Instance().LightDeviceNameList;

            lightValueGrid.Rows.Clear();
            for (int i = 0; i < lightDeviceNameList.Length; i++)
            {
                lightValueGrid.Rows.Add(lightDeviceNameList[i], 0);
            }
        }

        private void LightParamPanel_Load(object sender, EventArgs e)
        {

        }

        public void SetLightValues(StepModel model, InspectStep inspectStep, int cameraIndex)
        {
            curModel = model;
            curInspectStep = inspectStep;
            curCameraIndex = cameraIndex;

            SetLightValues();
        }

        public void SetLightValues()
        {
            LightParam = (LightParam)curInspectStep.GetLightParamSet()[0].Clone();

            UpdateLightValues();
        }

        private void UpdateLightValues()
        {
            //onValueUpdate = true;

            if (lightValueGrid.Rows.Count > 0)
            {
                for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                {
                    lightValueGrid.Rows[i].Cells[1].Value = LightParam.LightValue.Value[i];
                }
            }

            if (curCameraIndex > -1)
            {
                exposureTimeMs.Value = curModel.ExposureTimeUsList[curCameraIndex] / 1000;
            }
            else
            {
                exposureTimeMs.Value = ExposureTimeUs / 1000;
            }

            if (exposureTimeMs.Value == 0)
            {
                exposureTimeMs.Value = DeviceConfig.Instance().DefaultExposureTimeMs;
            }

            //onValueUpdate = false;
        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = "Light param is invailed";
                MessageBox.Show(message);
                return;
            }

            LightValueChanged?.Invoke(true);
        }

        private bool ApplyLight()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - exposureTime_ValueChanged");

            try
            {
                LightParam.LightParamType = LightParamType.Value;

                string[] lightDeviceNameList = LightConfig.Instance().LightDeviceNameList;
                for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                {
                    LightParam.LightValue.Value[i] = Convert.ToInt32(lightValueGrid.Rows[i].Cells[1].Value);
                    lightDeviceNameList[i] = (string)lightValueGrid.Rows[i].Cells[0].Value;
                }

                if (curCameraIndex > -1)
                {
                    curModel.ExposureTimeUsList[curCameraIndex] = Convert.ToInt32(Convert.ToSingle(exposureTimeMs.Value) * 1000);
                }
                else
                {
                    ExposureTimeUs = Convert.ToInt32(Convert.ToSingle(exposureTimeMs.Value) * 1000);
                }

                LightConfig.Instance().LightParamSet = curInspectStep.GetLightParamSet().Clone();
                LightConfig.Instance().Save();

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        private void LightParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void ToggleVisible()
        {
            Visible = !Visible;
        }
    }
}
