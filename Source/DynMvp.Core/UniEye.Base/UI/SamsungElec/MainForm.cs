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

namespace UniEye.Base.UI.SamsungElec
{
    public partial class MainForm : Form, IMainForm, IModelEventListener
    {
        private IRightPanel rightPanel;
        private AlarmMessageForm alarmMessageForm;
        private IMainTabPage curTabPage;
        private List<IMainTabPage> tabPageList;
        private Dictionary<IMainTabPage, Button> tabButtonList = new Dictionary<IMainTabPage, Button>();

        public MainForm()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init MainForm");

            InitializeComponent();

            tabPageList = UiManager.Instance().CreateMainTabPage();
            tabPageList.Reverse();

            foreach (IMainTabPage tabPage in tabPageList)
            {
                tabPage.Initialize();
                ((UserControl)tabPage).Dock = DockStyle.Fill;

                var button = new Button();
                button.Text = tabPage.TabName;
                button.Tag = tabPage.TabKey.ToString();
                button.BackColor = Color.Transparent;
                button.FlatStyle = FlatStyle.Popup;
                button.Image = tabPage.TabIcon;
                button.Dock = DockStyle.Left;
                button.UseVisualStyleBackColor = true;
                button.TextAlign = ContentAlignment.BottomCenter;
                button.TextImageRelation = TextImageRelation.ImageAboveText;
                button.Click += button_Click;
                panelBottom.Controls.Add(button);

                tabButtonList.Add(tabPage, button);
            }

            alarmMessageForm = new AlarmMessageForm();

            rightPanel = UiManager.Instance().CreateRightPanel();
            if (rightPanel != null)
            {
                Controls.Add((Control)rightPanel);
            }

            LogHelper.Debug(LoggerType.StartUp, "Update Program Title");

            labelTitle.Text = UiConfig.Instance().ProgramTitle;

            UiManager.Instance().SetupStatusStrip(statusStrip);
            if (statusStrip.Items.Count == 0)
            {
                statusStrip.Visible = false;
            }

            ModelManager.Instance().AddListener(this);

            LogHelper.Debug(LoggerType.StartUp, "End MainForm::Ctor");
        }

        private void button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            ShowPage((TabKey)Enum.Parse(typeof(TabKey), (string)button.Tag));
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

            ShowPage(TabKey.Model);

            alarmMessageForm.Show();
            ErrorManager.Instance().OnResetAlarmStatus += errorManager_ResetAlarmStatus;

            DeviceManager.Instance().RobotOrigin();

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

            if (tabPage.IsAdminPage && IsAdminUser() == false)
            {
                MessageForm.Show(null, StringManager.GetString("Operator can not use"));
                return false;
            }

            if (panelMain.Controls.Count > 0)
            {
                // hide panel
                if (panelMain.Controls[0] is IMainTabPage hidePage)
                {
                    hidePage?.TabPageVisibleChanged(false);

                    if (tabButtonList.TryGetValue(hidePage, out Button hideButton))
                    {
                        hideButton.Image = hidePage.TabIcon;
                    }
                }
            }

            curTabPage = tabPage;
            panelMain.Controls.Clear();
            panelMain.Controls.Add((UserControl)tabPage);
            if (rightPanel != null)
            {
                ((UserControl)rightPanel).Dock = System.Windows.Forms.DockStyle.Right;
                panelMain.Controls.Add((UserControl)rightPanel);
            }

            // show panel
            tabPage.TabPageVisibleChanged(true);

            if (tabButtonList.TryGetValue(tabPage, out Button showBtn))
            {
                showBtn.Image = tabPage.TabSelectedIcon;
            }

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
        }

        public void OnIdle()
        {
            bool modelLoaded = (ModelManager.Instance().CurrentModel != null);
            bool inspectionStarted = SystemState.Instance().OnInspection;
            bool livemode = SystemManager.Instance().LiveMode;

            labelModelName.Enabled = !inspectionStarted && !livemode;
            labelLotNo.Enabled = !inspectionStarted && !livemode;

            //panelBottom.Enabled = !inspectionStarted;

            foreach (KeyValuePair<IMainTabPage, Button> pair in tabButtonList)
            {
                if (pair.Key.TabKey != TabKey.Inspect &&
                    pair.Key.TabKey != TabKey.Setting)
                {
                    pair.Value.Enabled = !inspectionStarted && !livemode;
                }
            }

            UiManager.Instance().UpdateStatusStrip();
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

        }

        public void ModelOpen(ModelBase model)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelOpenDelegate(ModelOpen), model);
                return;
            }

            labelModelName.Text = "Model : " + model.Name;

            if (ModelManager.Instance().CurrentModel.IsEmpty() == true)
            {
                if (IsAdminUser())
                {
                    ShowPage(TabKey.Teach);
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
            if (InvokeRequired)
            {
                BeginInvoke(new ModelCloseDelegate(ModelClosed), model);
                return;
            }
            labelModelName.Text = "Model : ";
        }

        public void SetLotNo(string lotNo)
        {
            labelLotNo.Text = "Lot No. : " + lotNo;
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

        public void ChangeOpMode(OpMode opMode)
        {
            switch (opMode)
            {
                case OpMode.Inspect:
                    ShowPage(TabKey.Inspect);
                    break;
                    //case OpMode.Teach:
                    //    ShowPage(TabKey.Teach);
                    //    break;
            }
        }

        public OpMode GetOpMode()
        {
            //if (curTabPage == null)
            //    return OpMode.Etc;

            //switch (curTabPage.TabKey)
            //{
            //    case TabKey.Inspect:
            //        return OpMode.Inspect;
            //    case TabKey.Teach:
            //        return OpMode.Teach;
            //}

            //return OpMode.Etc;
            return OpMode.Idle;
        }

        private void labelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
            WindowState = FormWindowState.Maximized;
        }

        public void TestInspect()
        {

        }
    }
}
