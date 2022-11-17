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

namespace UniEye.Base.UI.Main
{
    public partial class MainForm : Form, IMainForm, IModelEventListener
    {
        private IRightPanel rightPanel;
        private AlarmMessageForm alarmMessageForm;
        private List<IMainTabPage> tabPageList;

        public MainForm()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init MainForm");

            InitializeComponent();

            tabPageList = UiManager.Instance().CreateMainTabPage();
            foreach (IMainTabPage tabPage in tabPageList)
            {
                LogHelper.Debug(LoggerType.StartUp, string.Format("Init {0} Page", tabPage.TabName));

                UltraTab ultraTab = tabControlMain.Tabs.Add();

                ultraTab.Appearance.Image = tabPage.TabIcon;
                ultraTab.SelectedAppearance.Image = tabPage.TabSelectedIcon;
                ultraTab.SelectedAppearance.BackColor = tabPage.TabSelectedColor;

                ultraTab.Key = tabPage.TabKey.ToString();
                ultraTab.Text = tabPage.TabName;

                var ultraTabPageControl = new UltraTabPageControl();
                ultraTabPageControl.Controls.Add((UserControl)tabPage);

                ultraTab.TabPage = ultraTabPageControl;

                tabControlMain.Controls.Add(ultraTabPageControl);

                ((UserControl)tabPage).Dock = DockStyle.Fill;
                tabPage.Initialize();
            }

            rightPanel = UiManager.Instance().CreateRightPanel();
            alarmMessageForm = new AlarmMessageForm();

            if (rightPanel != null)
            {
                panelRight.Controls.Add((UserControl)rightPanel);
            }
            else
            {
                panelRight.Visible = false;
            }

            LogHelper.Debug(LoggerType.StartUp, "Show Product Logo");

            string productLogoPath = PathConfig.Instance().ProductLogo;
            if (File.Exists(productLogoPath) == true)
            {
                titleLogo.Image = new Bitmap(productLogoPath);
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

            if (OperationConfig.Instance().SingleModel)
            {
                tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Model.ToString()];
            }

            alarmMessageForm.Show();
            ErrorManager.Instance().OnResetAlarmStatus += errorManager_ResetAlarmStatus;

            DeviceManager.Instance().RobotOrigin();

            Application.Idle += Application_Idle;

            LogHelper.Debug(LoggerType.StartUp, "End MainForm_Load");
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            OnIdle();

            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.OnIdle();
            }

            UiManager.Instance().UpdateStatusStrip();
        }

        public void OnIdle()
        {
            bool modelLoaded = (ModelManager.Instance().CurrentModel != null);
            bool inspectionStarted = SystemState.Instance().OnInspection;
            bool livemode = SystemManager.Instance().LiveMode;

            tabControlMain.Tabs[TabKey.Model.ToString()].Enabled = !inspectionStarted && !livemode;
            tabControlMain.Tabs[TabKey.Inspect.ToString()].Enabled = modelLoaded;

            tabControlMain.Tabs[TabKey.Teach.ToString()].Enabled = modelLoaded && !inspectionStarted && !livemode;

            try
            {
                tabControlMain.Tabs[TabKey.Report.ToString()].Enabled = !inspectionStarted && !livemode;
            }
            catch (ArgumentException) { }

            tabControlMain.Tabs[TabKey.Setting.ToString()].Enabled = !inspectionStarted && !livemode;

            btnUser.Enabled = !inspectionStarted && !livemode;
            btnExit.Enabled = true;

            try
            {
                tabControlMain.Tabs[TabKey.Live.ToString()].Enabled = !inspectionStarted && !livemode;
            }
            catch (ArgumentException) { }
        }

        public void ChangeCaption()
        {
            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.ChangeCaption();
            }
        }

        private void errorManager_ResetAlarmStatus()
        {
            //IoButton emergencyButton = DeviceManager.Instance().EmergencyButton;
            //if (emergencyButton != null)
            //    emergencyButton.ResetState();
        }

        private void tabControlMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Start tabControlMain_SelectedTabChanged.");

            if (e.PreviousSelectedTab != null)
            {
                UltraTabPageControl preTabControl = tabControlMain.Tabs[e.PreviousSelectedTab.Index].TabPage;

                if (preTabControl.Controls.Count > 0 && preTabControl.Controls[0] is IMainTabPage)
                {
                    var preTabPage = (IMainTabPage)preTabControl.Controls[0];
                    preTabPage.TabPageVisibleChanged(false);
                }
            }

            UltraTabPageControl curTabControl = tabControlMain.Tabs[e.Tab.Index].TabPage;

            if (curTabControl.Controls.Count > 0 && curTabControl.Controls[0] is IMainTabPage)
            {
                var curTabPage = (IMainTabPage)curTabControl.Controls[0];
                curTabPage.TabPageVisibleChanged(true);
            }

            LogHelper.Debug(LoggerType.Operation, "End tabControlMain_SelectedTabChanged.");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckFormCloseing();

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
            tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Teach.ToString()];
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.ProcessKeyDown(e);
            }
        }

        private void tabControlMain_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Start tabControlMain_SelectedTabChanging.");

            if (e.Tab.Key == "Setting" || e.Tab.Key == "Model")
            {
                if (IsAdminUser() == false)
                {
                    MessageForm.Show(null, StringManager.GetString("Operator can not use"));
                    e.Cancel = true;
                }
                return;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeviceManager.Instance().Release();
            Application.ExitThread();
        }

        public void ModelListChanged()
        {
            throw new NotImplementedException();
        }

        public void ModelOpen(ModelBase model)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelOpenDelegate(ModelOpen), model);
                return;
            }

            title.Text = UiConfig.Instance().Title + " - " + model.Name;

            if (ModelManager.Instance().CurrentModel.IsEmpty() == true)
            {
                if (IsAdminUser())
                {
                    tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Teach.ToString()];
                }
                else
                {
                    MessageBox.Show("There is no teaching data. Please, make the teaching data by Administrator.");
                }
            }
            else
            {
                tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Inspect.ToString()];
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
                    tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Model.ToString()];
                }
                else
                {
                    tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Inspect.ToString()];
                }
            }
        }

        public void ChangeOpMode(OpMode opMode)
        {
            switch (opMode)
            {
                case OpMode.Inspect:
                    tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Inspect.ToString()];
                    break;
                case OpMode.Wait:
                    tabControlMain.SelectedTab = tabControlMain.Tabs[TabKey.Teach.ToString()];
                    break;
            }
        }

        public OpMode GetOpMode()
        {
            var tabKey = (TabKey)Enum.Parse(typeof(TabKey), tabControlMain.SelectedTab.Key);
            switch (tabKey)
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