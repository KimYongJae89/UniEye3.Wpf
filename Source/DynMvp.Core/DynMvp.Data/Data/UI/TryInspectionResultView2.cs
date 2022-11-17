using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public delegate void TryInspectionResultCellClickedDelegate();

    public partial class TryInspectionResultView2 : UserControl
    {
        public TryInspectionResultCellClickedDelegate TryInspectionResultCellClicked = null;
        public TeachHandler TeachHandlerProbe { get; set; } = null;

        private ProbeResultList probeResultList;
        public bool ShowProbeOwner { get; set; } = false;

        public TryInspectionResultView2()
        {
            InitializeComponent();
        }

        public void ClearResult()
        {
            webBrowser.DocumentText = "";
            resultGrid.Rows.Clear();
        }

        public void SetResult(ProbeResultList probeResultList)
        {
            resultGrid.Rows.Clear();
            this.probeResultList = probeResultList;

            tabControlMain.SelectedTab = tabControlMain.Tabs[0];

            foreach (ProbeResult probeResult in probeResultList)
            {
                int rowIndex = 0;

                if (string.IsNullOrEmpty(probeResult.Probe.Name))
                {
                    rowIndex = resultGrid.Rows.Add(probeResult.Probe.Target.Name, probeResult.Probe.Id, probeResult.GetGoodNgStr(), probeResult.BriefMessage);
                }
                else
                {
                    rowIndex = resultGrid.Rows.Add(probeResult.Probe.Name, "", probeResult.GetGoodNgStr(), probeResult.BriefMessage);
                }

                resultGrid.Rows[rowIndex].Tag = probeResult;
                if (probeResult.IsNG())
                {
                    resultGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
            }
        }

        private void resultGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var message = new DynMvp.UI.Message();
            if (e.RowIndex < 0)
            {
                return;
            }

            var probeResult = (ProbeResult)resultGrid.Rows[e.RowIndex].Tag;
            if (probeResult == null)
            {
                return;
            }

            probeResult.AppendResultMessage(message);

            webBrowser.DocumentText = HtmlMessageBuilder.Build(message);

            tabControlMain.SelectedTab = tabControlMain.Tabs[1];
        }

        private void resultGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var probeResult = (ProbeResult)resultGrid.Rows[e.RowIndex].Tag;

            if (TeachHandlerProbe != null)
            {
                TeachHandlerProbe.Clear();
                TeachHandlerProbe.Select(probeResult.Probe);
            }

            TryInspectionResultCellClicked?.Invoke();
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabControlMain.Tabs[0];
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            //webBrowser.DocumentText.
        }
    }
}
