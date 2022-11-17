using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class InspectionResultGridView : UserControl
    {
        public bool ShowDefectOnly { get; set; }
        public ProductResult ProductResult { get; set; } = null;
        public ProductResult LastProductResult { get; set; } = null;

        public InspectionResultGridView()
        {
            InitializeComponent();

            // language change
            ColumnProbeType.HeaderText = StringManager.GetString(ColumnProbeType.HeaderText);
            ColumnValue.HeaderText = StringManager.GetString(ColumnValue.HeaderText);
            ColumnResult.HeaderText = StringManager.GetString(ColumnResult.HeaderText);
            ColumnStandard.HeaderText = StringManager.GetString(ColumnStandard.HeaderText);
        }

        private void InspectionResultForm_Load(object sender, EventArgs e)
        {
            if (ProductResult == null)
            {
                return;
            }

            foreach (ProbeResult probeResult in ProductResult)
            {
                if (ShowDefectOnly == true && probeResult.IsGood())
                {
                    continue;
                }

                Probe probe = probeResult.Probe;

                int index = defectGridResultList.Rows.Add(probe.Target.InspectStep.StepNo,
                        probe.Target.CameraIndex, probe.Target.Id, probe.Id, probe.Target.Name, probe.GetProbeTypeShortName(), probeResult.GetGoodNgStr());

                if (probeResult.IsNG())
                {
                    defectGridResultList.Rows[index].Cells[ColumnResult.Index].Style.BackColor = Color.Red;
                }
                else
                {
                    defectGridResultList.Rows[index].Cells[ColumnResult.Index].Style.BackColor = Color.Green;
                }

                defectGridResultList.Rows[index].Tag = probeResult;
            }
        }

        private void defectGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            var probeResult = (ProbeResult)defectGridResultList.Rows[e.RowIndex].Tag;
            Probe probe = probeResult.Probe;

            dataGridCurrentResultValue.Rows.Clear();
            foreach (ResultValue probeResultValue in probeResult)
            {
                string standardString = "";
                if (string.IsNullOrEmpty(probeResultValue.DesiredString) == false)
                {
                    standardString = probeResultValue.DesiredString;
                }
                else if (probeResultValue.Lcl != probeResultValue.Ucl)
                {
                    standardString = string.Format("{0:0.00} ~ {1:0.00}", probeResultValue.Lcl, probeResultValue.Ucl);
                }

                if (probeResultValue.Value is string)
                {
                    dataGridCurrentResultValue.Rows.Add(probeResultValue.Name, probeResultValue.Value, standardString);
                }
                else
                {
                    dataGridCurrentResultValue.Rows.Add(probeResultValue.Name, string.Format("{0:0.00}", probeResultValue.Value), standardString);
                }
            }
        }
    }
}
