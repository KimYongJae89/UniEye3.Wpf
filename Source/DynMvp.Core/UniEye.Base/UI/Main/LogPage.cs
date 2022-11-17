using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.UltraWinTabControl;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class LogPage : UserControl, IMainTabPage
    {
        private object drawingLock = new object();

        public LogPage()
        {
            InitializeComponent();
            LogHelper.NotifyLog += NotifyLog;
        }

        public void ChangeCaption()
        {

        }

        public TabKey TabKey => TabKey.Log;

        public string TabName => "Log";

        public Bitmap TabIcon => global::UniEye.Base.Properties.Resources.log_gray_36;

        public Bitmap TabSelectedIcon => global::UniEye.Base.Properties.Resources.log_white_36;

        public Color TabSelectedColor => Color.Green;

        public bool IsAdminPage => false;

        public Uri Uri => throw new NotImplementedException();

        public void Initialize()
        {
        }

        public void OnIdle()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {

        }

        public void ProcessKeyDown(KeyEventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            logList.Items.Clear();
        }

        public void NotifyLog(LogLevel logLevel, LoggerType loggerType, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new NotifyLogDelegate(NotifyLog), logLevel, loggerType, message);
                return;
            }

            if (logList.Items.Count > 2000)
            {
                logList.Items.RemoveAt(0);
            }

            if (message.Contains("Packet Sent :") || message.Contains("Data received :"))
            {
                return;
            }

            int index = logList.Items.Add(message);
            logList.TopIndex = index;
        }
    }
}
