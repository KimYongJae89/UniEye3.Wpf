using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;

namespace UniEye.Base.UI.SamsungElec
{
    public partial class RightPanel : UserControl, IRightPanel
    {
        public RightPanel()
        {
            InitializeComponent();
        }

        public void EnableControls()
        {

        }

        public void Initialize()
        {

        }
        public void Destroy()
        {

        }
        public void OnIdle()
        {
            bool modelLoaded = (ModelManager.Instance().CurrentModel != null);
            bool inspectionStarted = SystemState.Instance().OnInspection;
            bool livemode = SystemManager.Instance().LiveMode;

            if (ErrorManager.Instance().IsAlarmed())
            {
                btnStart.Enabled = false;
                btnStop.Enabled = false;
                btnHomeMove.Enabled = false;
            }
            else
            {
                btnStart.Enabled = (inspectionStarted == false);
                btnStop.Enabled = (inspectionStarted == true);
                btnHomeMove.Enabled = (inspectionStarted == false);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.EnterWaitInspection();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
        }

        private void btnHomeMove_Click(object sender, EventArgs e)
        {
            DeviceManager.Instance().RobotOrigin();
        }

        public void VisibleHomeMoveButton(bool visible)
        {
            btnHomeMove.Visible = visible;
        }
    }
}
