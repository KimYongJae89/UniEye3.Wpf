using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
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
    public partial class TryInspectionResultView : UserControl
    {
        private ProductResult selectedTargetInspectionResult;
        private ProductResult lastTargetInspectionResult;
        public bool ShowProbeOwner { get; set; } = false;

        public TryInspectionResultView()
        {
            InitializeComponent();
        }

        private void InspectionResultForm_Load(object sender, EventArgs e)
        {

        }

        public void ClearResult()
        {
            resultText.Text = "";
            Invalidate();
            Update();
        }

        public void AddResult(ProductResult selectedTargetInspectionResult, ProductResult lastTargetInspectionResult)
        {
            this.selectedTargetInspectionResult = selectedTargetInspectionResult;
            this.lastTargetInspectionResult = lastTargetInspectionResult;

            if (lastTargetInspectionResult != null && lastTargetInspectionResult.Count() > 0)
            {
                resultText.AppendText("< Last Inspection Result >" + Environment.NewLine);
                foreach (ProbeResult probeResult in lastTargetInspectionResult)
                {
                    AppendProbeResult(probeResult);
                }

                resultText.AppendText(Environment.NewLine + "< Selected Inspection Result >" + Environment.NewLine);
            }

            foreach (ProbeResult probeResult in selectedTargetInspectionResult)
            {
                AppendProbeResult(probeResult);
            }
        }

        private void AppendProbeResult(ProbeResult probeResult)
        {
            if (ShowProbeOwner)
            {
                string stepNo = probeResult.Probe.Target.InspectStep.StepNo.ToString();

                resultText.AppendText(string.Format("Step : {0} / Target {1}", stepNo, probeResult.Probe.Target.Id));
            }
            resultText.AppendText(Environment.NewLine);

            resultText.AppendText(string.Format("Probe{0} [ {1} ]", probeResult.Probe.Id, probeResult.GetGoodNgStr()));
            resultText.AppendText(Environment.NewLine);
            resultText.AppendText(probeResult.ToString());
        }

        public void UpdateResult(ProductResult selectedTargetInspectionResult, ProductResult lastInspectionResult)
        {
            ClearResult();

            this.selectedTargetInspectionResult = selectedTargetInspectionResult;

            if (lastTargetInspectionResult != null && lastTargetInspectionResult.Count() > 0)
            {
                resultText.AppendText("< Last Inspection Result >" + Environment.NewLine);
                foreach (ProbeResult probeResult in lastTargetInspectionResult)
                {
                    resultText.AppendText(string.Format("Probe{0} - {1} [ {2} ]", probeResult.Probe.Id, probeResult.ToString(), probeResult.GetGoodNgStr()));
                    resultText.AppendText(Environment.NewLine);
                }

                resultText.AppendText(Environment.NewLine + "< Selected Inspection Result >" + Environment.NewLine);
            }

            foreach (ProbeResult probeResult in selectedTargetInspectionResult)
            {
                resultText.AppendText(string.Format("Probe{0} - {1} [ {2} ]\n", probeResult.Probe.Id, probeResult.ToString(), probeResult.GetGoodNgStr()));
                resultText.AppendText(Environment.NewLine);
            }
        }
    }
}
