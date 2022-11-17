using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.Light.SerialLigth.iCore
{
    public partial class iCoreConfigForm : Form
    {
        private SerialLightCtrlInfoIPulse serialLightInfoIPulse;

        public iCoreConfigForm(SerialLightCtrlInfoIPulse serialLightInfoIPulse)
        {
            InitializeComponent();

            DynMvp.UI.UiHelper.SetNumericMinMax(slaveId, byte.MinValue, byte.MaxValue);
            DynMvp.UI.UiHelper.SetNumericMinMax(maxVoltage, ushort.MinValue, ushort.MaxValue);

            opMode.DataSource = Enum.GetValues(typeof(OperationMode));

            this.serialLightInfoIPulse = serialLightInfoIPulse;
        }

        private void iCoreConfigForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            DynMvp.UI.UiHelper.SetNumericValue(slaveId, serialLightInfoIPulse.SlaveId);
            opMode.SelectedItem = serialLightInfoIPulse.OperationMode;
            DynMvp.UI.UiHelper.SetNumericValue(maxVoltage, serialLightInfoIPulse.MaxVoltage);
            DynMvp.UI.UiHelper.SetNumericValue(timeDuration, (decimal)serialLightInfoIPulse.TimeDuration);
            DynMvp.UI.UiHelper.SetCheckboxChecked(lowPassFilter, serialLightInfoIPulse.LPFMode);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            serialLightInfoIPulse.SlaveId = (byte)slaveId.Value;
            serialLightInfoIPulse.OperationMode = (OperationMode)opMode.SelectedItem;
            serialLightInfoIPulse.MaxVoltage = (ushort)maxVoltage.Value;
            serialLightInfoIPulse.TimeDuration = (float)timeDuration.Value;
            serialLightInfoIPulse.LPFMode = lowPassFilter.Checked;

            Close();
        }

    }
}
