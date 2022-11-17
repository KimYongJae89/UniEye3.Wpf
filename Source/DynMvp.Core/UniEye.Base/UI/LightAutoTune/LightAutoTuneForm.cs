using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Vision;

namespace UniEye.Base.UI.LightAutoTune
{
    public partial class LightAutoTuneForm : Form
    {
        private bool autoSet = false;
        private LightAutoTuner autoTuner;
        private List<LightAutoTunePanel> tunePanelList = new List<LightAutoTunePanel>();

        public delegate void UpdateLightValueDelegate(int lightValue);
        public UpdateLightValueDelegate UpdateLightValue;

        public LightAutoTuneForm(LightAutoTuneType tuneType, bool autoSet = false)
        {
            this.autoSet = autoSet;

            var tuneValueCalculator = LightTuneValueCalculator.CreateCalculator(tuneType);
            autoTuner = new LightAutoTuner(tuneValueCalculator);

            InitializeComponent();

            TopMost = true;
            TopLevel = true;

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

            layoutPanel.ColumnStyles.Clear();
            layoutPanel.ColumnCount = cameraHandler.NumCamera;

            foreach (Camera camera in cameraHandler)
            {
                var autoTunePanel = new LightAutoTunePanel(camera.Index);

                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                layoutPanel.Controls.Add(autoTunePanel);

                tunePanelList.Add(autoTunePanel);
            }

            autoTuner.OnTuning = OnTuning;
            autoTuner.TuneDone = TuneDone;
        }


        private void TuneDone(ImageD[] tuneImageArr)
        {
            if (InvokeRequired)
            {
                Invoke(new TuneDoneDelegate(TuneDone), tuneImageArr);
                return;
            }

            progressBar.Value = 100;

            int lightValue = autoTuner.GetLightTuneValue();

            lblLightValue.Text = lightValue.ToString();

            buttonStart.Enabled = true;
            buttonApply.Enabled = true;
            if (autoSet == true)
            {
                UpdateLightValue?.Invoke(lightValue);

                Close();
            }
        }

        private void OnTuning(ImageD[] tuneImageArr)
        {
            if (InvokeRequired)
            {
                Invoke(new OnTuningDelegate(OnTuning), tuneImageArr);
                return;
            }

            progress.Text = string.Format("{0} / 255", autoTuner.CurLightValue);
            progressBar.Value = (int)autoTuner.GetProgressPercent();

            for (int i = 0; i < tunePanelList.Count; i++)
            {
                KeyValuePair<int, float> keyValue = autoTuner.TuneDataArr[i].LastTuneValue;

                tunePanelList[i].UpdateData(keyValue.Key, keyValue.Value);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            UpdateLightValue?.Invoke(autoTuner.GetLightTuneValue());

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            autoTuner.Stop();

            SystemManager.Instance().InspectRunner.ExitWaitInspection();
            SystemState.Instance().SetIdle();
            SystemState.Instance().SetInspectState(InspectState.Ready);
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonApply.Enabled = false;
            buttonStart.Enabled = false;

            foreach (LightAutoTunePanel tunePanel in tunePanelList)
            {
                tunePanel.Clear();
            }

            autoTuner.Start();
        }

        private void AutoTuneForm_Load(object sender, EventArgs e)
        {
            if (autoSet == true)
            {
                buttonApply.Enabled = false;
                buttonStart.Enabled = false;
                foreach (LightAutoTunePanel tunePanel in tunePanelList)
                {
                    tunePanel.Clear();
                }

                autoTuner.Start();
            }
        }
    }

    public static class LightValueHelper
    {
        private static int maxValue = 255;
        public static int Get_PercentageToLightValue(int percent)
        {
            double result = maxValue / (100 / percent);
            if (result >= maxValue)
            {
                result = maxValue;
            }

            return (int)result;
        }

        public static int Get_LightValueToPercentage(int lightValue)
        {
            if (lightValue == 0)
            {
                return 0;
            }

            double result = 100 / (maxValue / lightValue);
            if (result >= 100)
            {
                result = 100;
            }

            return (int)result;
        }
    }
}