using Authentication.Core;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;

namespace UniEye.Base.UI.Main
{
    public partial class SettingPage : UserControl, IMainTabPage
    {
        // modaless로 구동하기 위함.
        private IoPortViewer ioPortViewer = null;

        public SettingPage()
        {
            InitializeComponent();
        }

        public void ChangeCaption()
        {
            checkBoxShowCenterGuide.Text = StringManager.GetString(checkBoxShowCenterGuide.Text);
            checkBoxDebugMode.Text = StringManager.GetString(checkBoxDebugMode.Text);
            useDefectReview.Text = StringManager.GetString(useDefectReview.Text);
            labelCenterGuideOffsetX.Text = StringManager.GetString(labelCenterGuideOffsetX.Text);
            labelCenterGuideOffsetY.Text = StringManager.GetString(labelCenterGuideOffsetY.Text);
            labelCenterGuideThickness.Text = StringManager.GetString(labelCenterGuideThickness.Text);
            changeUserButton.Text = StringManager.GetString(changeUserButton.Text);
            saveButton.Text = StringManager.GetString(saveButton.Text);
            labelLocalResultPath.Text = StringManager.GetString(labelLocalResultPath.Text);
            groupBoxRemoteBackup.Text = StringManager.GetString(groupBoxRemoteBackup.Text);
            useRemoteBackup.Text = StringManager.GetString(useRemoteBackup.Text);
            labelRemoteResultPath.Text = StringManager.GetString(labelRemoteResultPath.Text);
            label2.Text = StringManager.GetString(label2.Text);
            label3.Text = StringManager.GetString(label3.Text);
            buttonCameraCalibration.Text = StringManager.GetString(buttonCameraCalibration.Text);
            buttonCameraCalibration.Text = StringManager.GetString(buttonCameraCalibration.Text);
            buttonShowIoPortViewer.Text = StringManager.GetString(buttonShowIoPortViewer.Text);
            buttonUserManager.Text = StringManager.GetString(buttonUserManager.Text);
            labelTrigDelay.Text = StringManager.GetString(labelTrigDelay.Text);
            useRejectPusher.Text = StringManager.GetString(useRejectPusher.Text);

            groupRejectPusher.Text = StringManager.GetString(groupRejectPusher.Text);
            tabPageData.Text = StringManager.GetString(tabPageData.Text);
            tabPageGeneral.Text = StringManager.GetString(tabPageGeneral.Text);
            tabPageMachine.Text = StringManager.GetString(tabPageMachine.Text);
        }

        public void Initialize()
        {

        }

        public string TabName => "Setting";

        public TabKey TabKey => TabKey.Setting;

        public Bitmap TabIcon => Properties.Resources.settings_gray_36;

        public Bitmap TabSelectedIcon => Properties.Resources.settings_white_36;

        public Color TabSelectedColor => Color.FromArgb(255, 128, 128);

        public bool IsAdminPage => true;

        public Uri Uri => throw new NotImplementedException();

        private void buttonSelectRemoteFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                remoteResultPath.Text = dlg.SelectedPath;
            }
        }

        public void SaveSettings()
        {
            PathConfig.Instance().RemoteResult = remoteResultPath.Text;
            OperationConfig.Instance().UseRemoteBackup = useRemoteBackup.Checked;
            PathConfig.Instance().Result = localResultPath.Text;
            OperationConfig.Instance().ResultStoringDays = Convert.ToInt32(resultStoringDays.Text);
            OperationConfig.Instance().CpuUsage = Convert.ToInt32(cpuUsage.Text);
            OperationConfig.Instance().UseDefectReview = useDefectReview.Checked;

            UiConfig.Instance().ShowCenterGuide = checkBoxShowCenterGuide.Checked;
            DeviceConfig.Instance().UseRejectPusher = useRejectPusher.Checked;

            DeviceConfig.Instance().DefaultExposureTimeMs = (int)defaultExposureTime.Value;

            UiConfig.Instance().CenterGuidePos = new Point(Convert.ToInt32(txtCenterGuideOffsetX.Text), Convert.ToInt32(txtCenterGuideOffsetY.Text));
            UiConfig.Instance().CenterGuideThickness = Convert.ToInt32(txtCenterGuideThickness.Text);

            TimeConfig.Instance().RejectWaitTime = Convert.ToInt32(rejectWaitTime.Text);
            TimeConfig.Instance().RejectPusherPushTime = Convert.ToInt32(rejectPusherPushTime.Text);
            TimeConfig.Instance().RejectPusherPullTime = Convert.ToInt32(rejectPusherPullTime.Text);

            TimeConfig.Instance().TriggerDelay = Convert.ToInt32(triggerDelay.Text);
            TimeConfig.Instance().InspectionDelay = Convert.ToInt32(inspectionDelayTime.Value);
            DeviceManager.Instance().CameraHandler.SetTriggerDelay(TimeConfig.Instance().TriggerDelay);

            OperationConfig.Instance().Save();
            DeviceConfig.Instance().Save();
            TimeConfig.Instance().Save();
        }

        private void buttonSelectLocalFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                localResultPath.Text = dlg.SelectedPath;
            }
        }

        private void LoadSettings()
        {
            resultStoringDays.Text = OperationConfig.Instance().ResultStoringDays.ToString();
            cpuUsage.Text = OperationConfig.Instance().CpuUsage.ToString();
            useRemoteBackup.Checked = OperationConfig.Instance().UseRemoteBackup;
            localResultPath.Text = PathConfig.Instance().Result;
            remoteResultPath.Text = PathConfig.Instance().RemoteResult;
            useDefectReview.Checked = OperationConfig.Instance().UseDefectReview;
            checkBoxShowCenterGuide.Checked = UiConfig.Instance().ShowCenterGuide;
            useRejectPusher.Checked = DeviceConfig.Instance().UseRejectPusher;

            txtCenterGuideOffsetX.Text = UiConfig.Instance().CenterGuidePos.X.ToString();
            txtCenterGuideOffsetY.Text = UiConfig.Instance().CenterGuidePos.Y.ToString();
            txtCenterGuideThickness.Text = UiConfig.Instance().CenterGuideThickness.ToString();

            rejectWaitTime.Text = TimeConfig.Instance().RejectWaitTime.ToString();
            rejectPusherPushTime.Text = TimeConfig.Instance().RejectPusherPushTime.ToString();
            rejectPusherPullTime.Text = TimeConfig.Instance().RejectPusherPullTime.ToString();

            triggerDelay.Text = TimeConfig.Instance().TriggerDelay.ToString();
            inspectionDelayTime.Value = TimeConfig.Instance().InspectionDelay;
            defaultExposureTime.Value = DeviceConfig.Instance().DefaultExposureTimeMs;
        }

        private void buttonUserManager_Click(object sender, EventArgs e)
        {
            if (File.Exists("UserManager.exe") == false)
            {
                MessageBox.Show("Can't find UserManager.exe");
                return;
            }

            Process.Start("UserManager.exe", "nologinneeded");
        }

        private void buttonCameraCalibration_Click(object sender, EventArgs e)
        {
            var form = new CameraCalibrationForm();
            form.Initialize();
            form.ShowDialog();
        }

        private void buttonShowIoPortViewer_Click(object sender, EventArgs e)
        {
            if (ioPortViewer == null)
            {
                ioPortViewer = new IoPortViewer();
                ioPortViewer.TopMost = true;
            }

            if (ioPortViewer.Visible)
            {
                return;
            }
            ioPortViewer.Show(this);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void changeUserButton_Click(object sender, EventArgs e)
        {
            var loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                UserHandler.Instance.CurrentUser = loginForm.LogInUser;
            }
        }

        private void buttonCameraAlign_Click(object sender, EventArgs e)
        {
            var form = new OverlayForm();
            form.ShowDialog();
        }

        private void buttonMotionController_Click(object sender, EventArgs e)
        {
            var motionController = new MotionControlForm();
            motionController.Intialize(DeviceManager.Instance().AxisHandlerList);
            motionController.ShowDialog();
        }

        private void buttonTowerLampConfig_Click(object sender, EventArgs e)
        {

        }

        private void buttonRobotCalibration_Click(object sender, EventArgs e)
        {
            var robotMapForm = new RobotCalibrationForm();
            robotMapForm.Initialize();
            if (robotMapForm.ShowDialog() == DialogResult.OK)
            {
                // Do something
            }
        }

        public void OnIdle()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == false)
            {
                SaveSettings();
            }
            else
            {
                LoadSettings();
            }
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {

        }
    }
}
