using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public interface IObjectInfoPanel
    {
        void SetTeachHandler(TeachHandler teachHandler);

        void SelectTarget(Target target);
        void UpdateTargetImage(ImageD image);
        void SelectProbe(Probe probe);
        void ClearProbeData();
        ParamValueChangedDelegate ParamValueChanged { get; set; }
    }

    public delegate void ParamValueChangedDelegate(ValueChangedType valueChangedType, bool modified);

    public partial class ProbeInfoPanel : UserControl, IObjectInfoPanel
    {
        private bool onValueUpdate = false;
        private bool fClearProbeData = false;
        private Probe selectedProbe;

        public ParamValueChangedDelegate ParamValueChanged { get; set; }

        public ProbeInfoPanel()
        {
            InitializeComponent();

            UpdateProbeNameCombo();
        }

        public void SelectProbe(Probe probe)
        {
            onValueUpdate = true;

            probeId.Text = probe.Id.ToString();

            probeFullId.Text = probe.FullId.ToString();
            probeType.Text = StringManager.GetString(probe.GetProbeTypeDetailed());

            comboBoxProbeName.Text = probe.Name;

            selectedProbe = probe;

            onValueUpdate = false;
        }

        private void UpdateProbeNameCombo()
        {
            string[] probeNames = UiManager.Instance().GetProbeNames();

            if (probeNames != null)
            {
                foreach (string name in probeNames)
                {
                    comboBoxProbeName.Items.Add(name);
                }
            }
        }

        public void ClearProbeData()
        {
            fClearProbeData = true;

            probeId.Text = "";
            probeType.Text = "";
            probeFullId.Text = "";

            selectedProbe = null;
            comboBoxProbeName.Text = "";

            fClearProbeData = false;
        }

        private void comboBoxProbeName_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            if (fClearProbeData == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DefaultProbeInfoPanel - comboBoxProbeName_TextChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("DefaultProbeInfoPanel - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Name = comboBoxProbeName.Text;

            ParamValueChanged(ValueChangedType.Position, true);
        }

        private void comboBoxProbeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DefaultProbeInfoPanel - comboBoxProbeName_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("DefaultProbeInfoPanel - selectedProbe instance is null.");
                return;
            }

            selectedProbe.Name = comboBoxProbeName.Text;

            ParamValueChanged(ValueChangedType.None, true);
        }

        public void SetTeachHandler(TeachHandler teachHandler)
        {

        }

        public void SelectTarget(Target target)
        {

        }

        public void UpdateTargetImage(ImageD image)
        {

        }
    }
}
