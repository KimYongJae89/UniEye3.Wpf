using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class AlarmMessageForm : Form
    {
        private bool startup = true;
        private int curIndex;
        private ErrorItem curErrorItem;
        private List<ErrorItem> activeErrorItemList;

        private Point mousePoint;

        public AlarmMessageForm()
        {
            InitializeComponent();
        }

        private void AlarmMessageForm_Load(object sender, EventArgs e)
        {
            errorCheckTimer.Start();
        }

        private void buttonPrevError_Click(object sender, EventArgs e)
        {
            if (curIndex > 0)
            {
                curIndex--;
                curErrorItem = activeErrorItemList[curIndex];

                UpdateData();
            }
        }

        private void buttonNextError_Click(object sender, EventArgs e)
        {
            if ((activeErrorItemList.Count - 1) > curIndex)
            {
                curIndex++;
                curErrorItem = activeErrorItemList[curIndex];
                UpdateData();
            }
        }

        private void UpdateData()
        {
            errorCode.Text = curErrorItem.ErrorCode.ToString();
            errorLevel.Text = curErrorItem.ErrorLevel.ToString();
            errorSection.Text = curErrorItem.SectionStr;
            errorName.Text = curErrorItem.ErrorStr;
            messsage.Text = curErrorItem.Message.ToString();

            curErrorItem.Displayed = true;
        }

        private void errorCheckTimer_Tick(object sender, EventArgs e)
        {
            var errorManager = ErrorManager.Instance();
            if (errorManager.IsAlarmed())
            {
                if (Visible == false || startup == true || errorManager.Updated)
                {
                    CenterToScreen();
                    Show();

                    activeErrorItemList = errorManager.ActiveErrorItemList;

                    curIndex = 0;
                    curErrorItem = activeErrorItemList[curIndex];

                    UpdateData();

                    Focus();

                    startup = false;

                    errorManager.Updated = false;
                }
            }
            else
            {
                if (Visible == true)
                {
                    Hide();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().ResetAlarm();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void buttonAlarmOff_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().BuzzerOn = false;
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().StopProcess();
        }
    }
}
