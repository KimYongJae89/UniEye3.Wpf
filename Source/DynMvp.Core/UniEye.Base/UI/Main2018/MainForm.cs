using Authentication.Core;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.UltraWinTabControl;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main2018
{
    public partial class MainForm : Form, IMainForm, IModelEventListener
    {
        private IRightPanel rightPanel;
        private AlarmMessageForm alarmMessageForm;
        private IMainTabPage curTabPage = null;
        private List<IMainTabPage> tabPageList;

        public MainForm()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init MainForm");

            InitializeComponent();

            tabPageList = UiManager.Instance().CreateMainTabPage();
            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.Initialize();
                ((UserControl)tabPage).Dock = DockStyle.Fill;
            }

            rightPanel = UiManager.Instance().CreateRightPanel();

            // 모터 관련 임시//
            rightPanel?.Initialize();
            // 모터 관련 임시//

            alarmMessageForm = new AlarmMessageForm();

            LogHelper.Debug(LoggerType.StartUp, "Show Product Logo");

            string productLogoPath = PathConfig.Instance().ProductLogo;
            if (File.Exists(productLogoPath) == true)
            {
                productLogo.Image = new Bitmap(productLogoPath);
            }

            LogHelper.Debug(LoggerType.StartUp, "Update Program Title");

            Text = UiConfig.Instance().ProgramTitle;

            UiManager.Instance().SetupStatusStrip(statusStrip);
            if (statusStrip.Items.Count == 0)
            {
                statusStrip.Visible = false;
            }

            ModelManager.Instance().AddListener(this);

            LogHelper.Debug(LoggerType.StartUp, "End MainForm::Ctor");
        }

        private bool IsAdminUser()
        {
            bool adminUser = true;
            if (OperationConfig.Instance().UseUserManager)
            {
                adminUser = UserHandler.Instance.CurrentUser.UserId != "op";
            }

            return adminUser;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start MainForm_Load");

            title.Text = UiConfig.Instance().Title;

            // 모델페이지가 없으면 가장 먼저 들어있는 페이지를 활성화시킨다.
            if (!ShowPage(TabKey.Model) && tabPageList.Count > 0)
            {
                ShowPage(tabPageList[0].TabKey);
            }

            if (tabPageList.Find(x => x.TabKey == TabKey.Inspect) == null)
            {
                btnInspect.Visible = false;
            }

            if (tabPageList.Find(x => x.TabKey == TabKey.Setting) == null)
            {
                btnConfig.Visible = false;
            }

            if (tabPageList.Find(x => x.TabKey == TabKey.Report) == null)
            {
                btnReport.Visible = false;
            }

            alarmMessageForm.Show();
            ErrorManager.Instance().OnResetAlarmStatus += errorManager_ResetAlarmStatus;

            //if (((UniEyeDeviceManager)DeviceManager.Instance()).RobotStage != null)
            //    DeviceManager.Instance().RobotOrigin();

            Application.Idle += Application_Idle;

            LogHelper.Debug(LoggerType.StartUp, "End MainForm_Load");
        }

        public void ChangeCaption()
        {
            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.ChangeCaption();
            }
        }

        public bool ShowPage(TabKey key)
        {
            IMainTabPage tabPage = tabPageList.Find(x => x.TabKey == key);
            if (tabPage == null)
            {
                return false;
            }

            if (ReferenceEquals(tabPage, curTabPage))
            {
                return true;
            }

            if (tabPage.IsAdminPage && IsAdminUser() == false)
            {
                MessageForm.Show(null, StringManager.GetString("Operator can not use"));
                return false;
            }

            if (panelMain.Controls.Count > 0)
            {
                ((IMainTabPage)panelMain.Controls[0]).TabPageVisibleChanged(false);
            }

            curTabPage = tabPage;
            panelMain.Controls.Clear();
            panelMain.Controls.Add((UserControl)tabPage);
            if (rightPanel != null)
            {
                ((UserControl)rightPanel).Dock = System.Windows.Forms.DockStyle.Right;
                panelMain.Controls.Add((UserControl)rightPanel);
            }

            tabPage.TabPageVisibleChanged(true);

            return true;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            OnIdle();

            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.OnIdle();
            }

            if (rightPanel != null)
            {
                rightPanel.OnIdle();
            }

            UiManager.Instance().UpdateStatusStrip();
        }

        public void OnIdle()
        {
            bool modelLoaded = (ModelManager.Instance().CurrentModel != null);
            bool inspectionStarted = SystemState.Instance().OnInspection;
            bool livemode = SystemManager.Instance().LiveMode;

            labelModelName.Enabled = !inspectionStarted && !livemode;

            //btnInspect.Enabled = modelLoaded && !inspectionStarted && !livemode;
            btnConfig.Enabled = !inspectionStarted && !livemode;
            btnReport.Enabled = !inspectionStarted && !livemode;
            btnExit.Enabled = true;
        }

        private void errorManager_ResetAlarmStatus()
        {
            //IoButton emergencyButton = DeviceManager.Instance().EmergencyButton;
            //if (emergencyButton != null)
            //    emergencyButton.ResetState();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckFormCloseing();

            // 모터 관련 임시//
            rightPanel?.Destroy();
            // 모터 관련 임시//

            if (e.Cancel == false)
            {
                ModelManager.Instance().CloseModel();
            }
        }

        private bool CheckFormCloseing()
        {
            if (SystemState.Instance().OnInspection)
            {
                MessageForm.Show(null, "Please, Stop the inspection.");
                return false;
            }

            if (ModelManager.Instance().CurrentModel != null)
            {
                ModelManager.Instance().CurrentModel.SaveModel();
            }

            if (MessageForm.Show(this, "Do you want to exit program?", MessageFormType.YesNo) == DialogResult.No)
            {
                return false;
            }
            return true;
        }

        public void ModifyTeaching(string imageFileName)
        {
            SystemManager.Instance().ImageSequence.SetImagePath(imageFileName);
            ShowPage(TabKey.Teach);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.ProcessKeyDown(e);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeviceManager.Instance().Release();
            Application.ExitThread();
        }

        public void ModelListChanged()
        {
            //throw new NotImplementedException();
            //MessageBox.Show(null, this.GetType().FullName + "\nModelListChanged()", "Not Implemented");
        }

        public void ModelOpen(ModelBase model)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelOpenDelegate(ModelOpen), model);
                return;
            }

            title.Text = UiConfig.Instance().Title;

            labelModelName.Text = model.Name;

            if (ModelManager.Instance().CurrentModel.IsEmpty() == true)
            {
                if (IsAdminUser())
                {
                    if (ShowPage(TabKey.Teach) == false)
                    {
                        ShowPage(TabKey.Inspect);
                    }
                }
                else
                {
                    MessageBox.Show("There is no teaching data. Please, make the teaching data by Administrator.");
                }
            }
            else
            {
                ShowPage(TabKey.Inspect);
            }
        }

        public void ModelClosed(ModelBase model)
        {
            title.Text = UiConfig.Instance().Title;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            var loginForm = new LogInForm();
            loginForm.ShowDialog();
            if (loginForm.DialogResult == DialogResult.OK)
            {
                UserHandler.Instance.CurrentUser = loginForm.LogInUser;
            }

            if (IsAdminUser() == false)
            {
                if (ModelManager.Instance().CurrentModel == null)
                {
                    ShowPage(TabKey.Model);
                }
                else
                {
                    ShowPage(TabKey.Inspect);
                }
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ShowPage(TabKey.Setting);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ShowPage(TabKey.Report);
        }

        private void title_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
            WindowState = FormWindowState.Maximized;
        }

        private void picProgramIcon_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
            WindowState = FormWindowState.Maximized;
        }

        private void panelModelBar_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
            WindowState = FormWindowState.Maximized;
        }

        private void labelModelName_Click(object sender, EventArgs e)
        {
            ShowPage(TabKey.Model);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ShowPage(TabKey.Teach);
        }

        private void btnInspect_Click(object sender, EventArgs e)
        {
            ShowPage(TabKey.Inspect);
        }

        public void ChangeOpMode(OpMode opMode)
        {
            switch (opMode)
            {
                case OpMode.Inspect:
                    ShowPage(TabKey.Inspect);
                    break;
                case OpMode.Wait:
                    ShowPage(TabKey.Teach);
                    break;
            }
        }

        public OpMode GetOpMode()
        {
            switch (curTabPage.TabKey)
            {
                case TabKey.Inspect:
                    return OpMode.Inspect;
                case TabKey.Teach:
                    return OpMode.Wait;
            }

            return OpMode.Idle;
        }

        public void TestInspect()
        {

        }
    }
}
