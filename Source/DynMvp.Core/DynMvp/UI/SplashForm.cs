using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public delegate bool SplashActionDelegate(IReportProgress progress);

    public partial class SplashForm : Form, IReportProgress
    {
        private string lastError;
        private bool doConfigAction = false;
        private Thread workingThread = null;
        public SplashActionDelegate ConfigAction = null;
        public SplashActionDelegate SetupAction = null;

        public SplashForm()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            title.BackColor = Color.Transparent;
            title.Parent = backgroundImage;
            progressMessage.BackColor = Color.Transparent;
            progressMessage.Parent = backgroundImage;
            copyrightText.BackColor = Color.Transparent;
            copyrightText.Parent = backgroundImage;
            productLogo.BackColor = Color.Transparent;
            productLogo.Parent = backgroundImage;
            companyLogo.BackColor = Color.Transparent;
            companyLogo.Parent = backgroundImage;
            versionText.BackColor = Color.Transparent;
            versionText.Parent = backgroundImage;
            buildText.BackColor = Color.Transparent;
            buildText.Parent = backgroundImage;
            pictureIcon.BackColor = Color.Transparent;
            pictureIcon.Parent = backgroundImage;
            progressMessage.Text = StringManager.GetString(progressMessage.Text);
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "SplashForm_Load Start");
            if (string.IsNullOrEmpty(title.Text) == false)
            {
                title.Font = UiHelper.AutoFontSize(title, title.Text);
            }

            splashActionTimer.Start();
            LogHelper.Debug(LoggerType.StartUp, "SplashForm_Load End");
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        public void ReportProgress(int progressPos, string progressMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReportProgressDelegate(ReportProgress), progressPos, progressMessage);
                return;
            }

            progressBar.Value = progressPos;
            this.progressMessage.Text = progressMessage;
        }

        private void SpalashProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start SpalashProc.");

            if (SetupAction != null && SetupAction(this) == false)
            {
                MessageBox.Show("Some error is occurred. Please, check the configuration.\n\n" + lastError);
                DialogResult = DialogResult.Abort;
            }
        }

        private void SplashForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12 && e.Alt == true)
            {
                if (ConfigAction != null && !doConfigAction)
                {
                    doConfigAction = true;
                    progressMessage.Text = "Wait Configuration";

                    ConfigAction(this);

                    doConfigAction = false;
                }
            }
        }

        private void splashActionTimer_Tick(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "splashActionTimer_Tick Start.");
            if (workingThread == null)
            {
                if (doConfigAction == false)
                {
                    LogHelper.Debug(LoggerType.StartUp, "Start Spalash Thread.");

                    progressMessage.Text = "Start Setup...";

                    workingThread = new Thread(new ThreadStart(SpalashProc));
                    workingThread.IsBackground = true;
                    workingThread.Start();

                    splashActionTimer.Interval = 500;
                }
            }
            else
            {
                if (workingThread.IsAlive == false)
                {
                    Close();
                }
            }
        }
    }
}
