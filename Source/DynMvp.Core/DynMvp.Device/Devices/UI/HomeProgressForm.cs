using DynMvp.Base;
using DynMvp.Devices.MotionController;
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

namespace DynMvp.Devices.UI
{
    public partial class HomeProgressForm : Form
    {
        private string messageText;
        public string MessageText
        {
            set => messageText = value;
        }

        private Task task;
        private CancellationTokenSource cancellationTokenSource;

        public HomeProgressForm()
        {
            InitializeComponent();
            TopMost = true;
            TopLevel = true;
        }

        public void Show(Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            this.cancellationTokenSource = cancellationTokenSource;

            if (cancellationTokenSource == null)
            {
                buttonCancel.Visible = false;
            }
            else
            {
                buttonCancel.Visible = true;
            }

            UpdateAxisList();

            task = new Task(action);
            task.Start();

            base.ShowDialog();
        }

        private void UpdateAxisList()
        {
            // 데이터 초기화
            dgvAxisList.Rows.Clear();

            // 데이터 추가
            int index = 0;

            var allAxisList = new List<Axis>();
            List<AxisHandler> handlerList = DeviceManager.Instance().AxisHandlerList;
            handlerList.ForEach(x => allAxisList.AddRange(x.AxisList));

            int maxOrder = allAxisList.Max(x => x.HomeOrder);

            for (int i = -1; i <= maxOrder; i++)
            {
                List<Axis> findList = allAxisList.FindAll(x => x.HomeOrder == i);

                foreach (Axis x in findList)
                {
                    index = dgvAxisList.Rows.Add();
                    DataGridViewRow row = dgvAxisList.Rows[index];
                    row.Cells[0].Value = x.Name;
                    row.Cells[1].Value = "Home Moving...";
                    row.Tag = x;
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }

            progressBar1.Maximum = index + 1;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (task.IsCompleted)
            {
                taskCheckTimer.Stop();
                Close();
            }
            else
            {
                int count = 0;
                foreach (DataGridViewRow row in dgvAxisList.Rows)
                {
                    var axisHandler = (Axis)row.Tag;
                    if (axisHandler.HomeFound)
                    {
                        count++;
                        row.Cells[1].Value = "Completed";
                        row.DefaultCellStyle.BackColor = Color.Green;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                }

                progressBar1.Value = count;
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
