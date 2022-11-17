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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class SimpleProgressForm : Form
    {
        private string messageText;
        public string MessageText
        {
            set
            {
                messageText = value;
                labelMessage.Text = StringManager.GetString(messageText);
            }
        }

        private CancellationTokenSource cancellationTokenSource;
        private Task task;

        public SimpleProgressForm()
        {
            InitializeComponent();
            TopMost = true;
            TopLevel = true;
        }

        public SimpleProgressForm(string message)
        {
            InitializeComponent();
            messageText = message;

            TopMost = true;
            TopLevel = true;
            if (messageText != null)
            {
                labelMessage.Text = StringManager.GetString(messageText);
            }
        }

        public void Show(Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            this.cancellationTokenSource = cancellationTokenSource;

            if (cancellationTokenSource == null)
            {
                buttonCancel.Visible = false;
                task = new Task(action);

            }
            else
            {
                buttonCancel.Visible = true;
                task = new Task(action, cancellationTokenSource.Token);
            }
            if (task.IsCanceled == false)
            {
                task.Start();
            }

            base.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (task.IsCompleted)
            {
                Close();
            }
        }

        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            if (cancellationTokenSource == null)
            {
                buttonCancel.Enabled = false;
            }

            taskCheckTimer.Start();
        }
    }
}
