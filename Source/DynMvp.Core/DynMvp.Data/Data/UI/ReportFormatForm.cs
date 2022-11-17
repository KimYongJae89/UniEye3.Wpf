using DynMvp.Base;
using DynMvp.Data;
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
    public partial class ReportFormatForm : Form
    {
        public StepModel Model { get; set; }

        public ReportFormatForm()
        {
            InitializeComponent();

            buttonAdd.Text = StringManager.GetString(buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(buttonDelete.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ExportPacketFormat reportPacketFormat = Model.ModelDescription.ReportPacketFormat;

            reportPacketFormat.ValueDataList.Clear();

            foreach (DataGridViewRow row in gridViewValueData.Rows)
            {
                reportPacketFormat.ValueDataList.Add(new ValueData(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new SelectResultValueForm();
            form.Model = Model;
            if (form.ShowDialog() == DialogResult.OK)
            {
                string valueName = form.ValueName.Replace("ResultValue.", "");
                gridViewValueData.Rows.Add(form.ObjectName, valueName);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (gridViewValueData.SelectedRows.Count > 0)
            {
                int index = gridViewValueData.SelectedRows[0].Index;
                gridViewValueData.Rows.RemoveAt(index);
            }
        }

        private void ReportFormatForm_Load(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = Model.ModelDescription.ReportPacketFormat;
            foreach (ValueData valueData in packetFormat.ValueDataList)
            {
                gridViewValueData.Rows.Add(valueData.ObjectName, valueData.ValueName);
            }
        }
    }
}
