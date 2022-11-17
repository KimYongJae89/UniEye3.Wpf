using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class LivePage : UserControl, IMainTabPage
    {
        private CameraPanel[] cameraViewArray;

        private object drawingLock = new object();
        private bool onLiveGrab = false;

        public LivePage()
        {
            InitializeComponent();
        }

        public void ChangeCaption()
        {
            // Laguage change
            measureMode.Text = StringManager.GetString(measureMode.Text);
            clearMeasure.Text = StringManager.GetString(clearMeasure.Text);
            labelExposure.Text = StringManager.GetString(labelExposure.Text);
        }

        public TabKey TabKey => TabKey.Live;

        public string TabName => "Live";

        public Bitmap TabIcon => global::UniEye.Base.Properties.Resources.live_gray_36;

        public Bitmap TabSelectedIcon => global::UniEye.Base.Properties.Resources.live_white_36;

        public Color TabSelectedColor => Color.YellowGreen;

        public bool IsAdminPage => false;

        public Uri Uri => throw new NotImplementedException();

        public void Initialize()
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

            foreach (Camera camera in cameraHandler)
            {
                lstCamera.Items.Add(camera);
                lstCamera.SelectedItems.Add(camera);
            }
        }

        public void OnIdle()
        {

        }

        private void UpdateLivePanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin LivePage::Initialize");

            int numCamera = lstCamera.SelectedItems.Count;

            cameraViewArray = new CameraPanel[numCamera];

            cameraViewPanel.ColumnStyles.Clear();
            cameraViewPanel.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));
            cameraViewPanel.ColumnCount = numCount;
            cameraViewPanel.RowCount = (int)Math.Floor(Math.Sqrt(numCamera));

            cameraViewPanel.ColumnStyles.Clear();
            cameraViewPanel.RowStyles.Clear();

            for (int i = 0; i < cameraViewPanel.ColumnCount; i++)
            {
                cameraViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / cameraViewPanel.ColumnCount));
            }

            for (int i = 0; i < cameraViewPanel.RowCount; i++)
            {
                cameraViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / cameraViewPanel.RowCount));
            }

            int index = 0;
            foreach (Camera camera in lstCamera.SelectedItems)
            {
                cameraViewArray[index] = new CameraPanel();

                cameraViewArray[index].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                cameraViewArray[index].Dock = System.Windows.Forms.DockStyle.Fill;
                cameraViewArray[index].Name = "targetImage";
                cameraViewArray[index].TabIndex = 8;
                cameraViewArray[index].TabStop = false;
                cameraViewArray[index].Enable = true;
                cameraViewArray[index].Tag = camera;
                cameraViewArray[index].MouseDoubleClick += cameraViewArray_MouseDoubleClick;
                cameraViewArray[index].MouseMove += cameraViewArray_MouseMove;

                int rowIndex = index / numCount;
                int colIndex = index % numCount;

                cameraViewPanel.Controls.Add(cameraViewArray[index], colIndex, rowIndex);

                index++;
            }

            txtExposure.Text = LightConfig.Instance().LiveExposureTimeUs.ToString();

            LogHelper.Debug(LoggerType.StartUp, "End LivePage::Initialize");
        }

        private void cameraViewArray_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cameraViewArray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int numCamera = cameraViewArray.Count();
            if (numCamera < 1)
            {
                return;
            }

            if (!(sender is CanvasPanel senderView))
            {
                return;
            }

            if (senderView.Parent == viewContainer)
            {
                int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));

                if (!(senderView.Tag is Camera camera))
                {
                    return;
                }

                int cameraIndex = camera.Index;

                int rowIndex = cameraIndex / numCount;
                int colIndex = cameraIndex % numCount;

                cameraViewPanel.Controls.Add(senderView, colIndex, rowIndex);
                cameraViewPanel.Show();
            }
            else
            {
                senderView.Parent = viewContainer;
                cameraViewPanel.Hide();
            }

            senderView.ZoomFit();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var systemManager = SystemManager.Instance();

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            LightCtrlHandler lightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;

            LightParam lightParam = LightConfig.Instance().GetLiveLightParam();

            if (onLiveGrab == false)
            {
                if (lightParam != null)
                {
                    lightCtrlHandler?.TurnOn(lightParam.LightValue);
                }

                SystemManager.Instance().LiveMode = true;
                foreach (Camera camera in cameraHandler)
                {
                    camera.Stop();
                    camera.Reset();

                    camera.SetExposureTime(Convert.ToInt32(txtExposure.Text));
                    camera.GrabMulti();
                }

                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                txtExposure.Enabled = false;

                onLiveGrab = true;
            }
            else
            {
                systemManager.LiveMode = false;

                lightCtrlHandler?.TurnOff();

                foreach (Camera camera in cameraHandler)
                {
                    camera.Stop();
                    camera.Reset();
                }

                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;

                txtExposure.Enabled = true;
                onLiveGrab = false;
            }
        }

        private void measureMode_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CameraPanel cameraView in cameraViewArray)
            {
                if (measureMode.Checked == true)
                {
                    if (!(cameraView.Tag is Camera camera))
                    {
                        continue;
                    }

                    Calibration calibration = SystemManager.Instance().GetCameraCalibration(camera.Index);
                    cameraView.TurnOnMeasure(calibration);
                }
                else
                {
                    cameraView.TurnOffMeasure();
                }
            }
        }

        private void clearMeasure_Click(object sender, EventArgs e)
        {
            foreach (CameraPanel cameraView in cameraViewArray)
            {
                cameraView.ClearMeasure();
            }
        }

        private void txtExposure_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (onLiveGrab == false && string.IsNullOrEmpty(txtExposure.Text) == false)
            {
                LightConfig.Instance().LiveExposureTimeUs = Convert.ToInt32(txtExposure.Text);
                LightConfig.Instance().Save();
            }
        }

        private void CameraViewZoomfit()
        {
            if (cameraViewArray == null)
            {
                return;
            }

            foreach (CameraPanel cameraView in cameraViewArray)
            {
                cameraView.ZoomFit();
            }
        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            LightCtrlHandler lightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;

            if (visibleFlag == true)
            {
                comboStep.Items.Clear();

                StepModel stepModel = ModelManager.Instance().CurrentStepModel;
                if (stepModel == null)
                {
                    return;
                }

                foreach (InspectStep inspectioStep in stepModel.InspectStepList)
                {
                    int idx = comboStep.Items.Add(inspectioStep.GetStepName());
                }

                comboStep.Enabled = true;
                if (comboStep.Items.Count == 0)
                {
                    comboStep.Enabled = false;
                }

                CameraViewZoomfit();
            }
            else
            {
                lightCtrlHandler?.TurnOff();

                foreach (Camera camera in cameraHandler)
                {
                    camera.Stop();
                    camera.Reset();
                }
            }

            SystemManager.Instance().LiveMode = false;
        }

        private void comboStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            StepModel stepModel = ModelManager.Instance().CurrentStepModel;
            if (stepModel == null)
            {
                return;
            }

            string selName = comboStep.Text;
            InspectStep inspectStep = stepModel.InspectStepList.Find(f => f.GetStepName() == selName);

            // Robot move...
            if (inspectStep != null && DeviceManager.Instance().RobotStage != null)
            {
                DeviceManager.Instance().RobotStage.Move(inspectStep.Position);
            }
        }

        private void buttonLightSttting_Click(object sender, EventArgs e)
        {
            var lightParamForm = new LightParamSingleForm();
            lightParamForm.InitControls();
            lightParamForm.LightParam = (LightParam)LightConfig.Instance().GetLiveLightParam().Clone();
            lightParamForm.ExposureTimeUs = LightConfig.Instance().LiveExposureTimeUs;

            if (lightParamForm.ShowDialog() == DialogResult.OK)
            {
                LightConfig.Instance().SetLiveLightParam(lightParamForm.LightParam);
                LightConfig.Instance().LiveExposureTimeUs = lightParamForm.ExposureTimeUs;
            }
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {

        }
    }
}