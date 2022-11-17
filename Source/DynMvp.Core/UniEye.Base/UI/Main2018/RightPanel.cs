using DynMvp.Base;
using DynMvp.Data;
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

namespace UniEye.Base.UI.Main2018
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

            btnReport.Enabled = !inspectionStarted && !livemode;
            btnLive.Enabled = !inspectionStarted && !livemode;

            if (ErrorManager.Instance().IsAlarmed())
            {
                btnStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            }
            else
            {
                if (SystemState.Instance().OpState == OpState.Idle)
                {
                    btnStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
                }
                else
                {
                    btnStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            InspectRunner inspectRunner = SystemManager.Instance().InspectRunner;
            if (SystemState.Instance().OpState == OpState.Idle)
            {
                if (inspectRunner.EnterWaitInspection() == true)
                {
                    btnStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                }
            }
            else
            {
                inspectRunner.ExitWaitInspection();
                //                repeatInspection.Checked = false;
                btnStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            }
        }
    }
}
