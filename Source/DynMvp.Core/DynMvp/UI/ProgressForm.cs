using DynMvp.Base;
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
    public delegate void ReportProgressDelegate(int percentage, string messge);

    public interface IReportProgress
    {
        void ReportProgress(int percentage, string messge);
        void SetLastError(string lastError);
        string GetLastError();
    }

    public partial class ProgressForm : Form, IReportProgress
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        private string titleText;
        public string TitleText
        {
            set => titleText = value;
        }

        private string messageText;
        public string MessageText
        {
            set => messageText = value;
        }

        private string lastError;

        public ProgressForm()
        {
            InitializeComponent();

            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;
            BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);
            message.Text = StringManager.GetString(message.Text);
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            message.Text = messageText;
            progressBar.Value = e.ProgressPercentage;
            Invalidate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            BackgroundWorker.CancelAsync();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (messageText != null)
            {
                message.Text = StringManager.GetString(messageText);
            }

            if (titleText != null)
            {
                Text = StringManager.GetString(titleText);
            }

            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            startTimer.Start();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            startTimer.Stop();
            BackgroundWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                message.Text = "Cancelled!";
            }

            else if (!(e.Error == null))
            {
                message.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                message.Text = "Done!";
                Close();
            }
        }

        public void ReportProgress(int percentage, string messge)
        {
            //if (InvokeRequired)
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    BeginInvoke(new ReportProgressDelegate(ReportProgress), percentage, messge);
            //    return;
            //}

            MessageText = messge;
            BackgroundWorker.ReportProgress(percentage);
        }
    }
}
