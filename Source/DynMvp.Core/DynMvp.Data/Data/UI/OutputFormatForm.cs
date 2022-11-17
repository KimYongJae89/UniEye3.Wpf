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
    public partial class OutputFormatForm : Form
    {
        public StepModel Model { get; set; }

        public OutputFormatForm()
        {
            InitializeComponent();

            Text = StringManager.GetString(Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            startTypeAscii.Text = StringManager.GetString(startTypeAscii.Text);
            startTypeHex.Text = StringManager.GetString(startTypeHex.Text);
            endTypeAscii.Text = StringManager.GetString(endTypeAscii.Text);
            endTypeHex.Text = StringManager.GetString(endTypeHex.Text);
            separatorTypeAscii.Text = StringManager.GetString(separatorTypeAscii.Text);
            separatorTypeHex.Text = StringManager.GetString(separatorTypeHex.Text);
            useChecksum.Text = StringManager.GetString(useChecksum.Text);

            ColumnProbeId.HeaderText = StringManager.GetString(ColumnProbeId.HeaderText);
            ColumnValueName.HeaderText = StringManager.GetString(ColumnValueName.HeaderText);

            buttonAdd.Text = StringManager.GetString(buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(buttonDelete.Text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = Model.ModelDescription.ExportPacketFormat;
            packetFormat.PacketStart = textBoxStart.Text;
            packetFormat.PacketEnd = textBoxEnd.Text;
            packetFormat.Separator = textBoxSeparator.Text;
            packetFormat.UseCheckSum = useChecksum.Checked;
            if (useChecksum.Checked == true)
            {
                packetFormat.ChecksumSize = Convert.ToInt32(checksumSize.Text);
            }
            else
            {
                packetFormat.ChecksumSize = 0;
            }

            packetFormat.PacketStartType = (startTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);
            packetFormat.PacketEndType = (endTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);
            packetFormat.SeparatorType = (separatorTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);

            packetFormat.ValueDataList.Clear();

            foreach (DataGridViewRow row in gridViewValueData.Rows)
            {
                packetFormat.ValueDataList.Add(new ValueData(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
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

        private void OutputFormatForm_Load(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = Model.ModelDescription.ExportPacketFormat;
            startTypeAscii.Checked = (packetFormat.PacketStartType == DelimiterType.Ascii);
            startTypeHex.Checked = (packetFormat.PacketStartType == DelimiterType.Hex);
            endTypeAscii.Checked = (packetFormat.PacketEndType == DelimiterType.Ascii);
            endTypeHex.Checked = (packetFormat.PacketEndType == DelimiterType.Hex);
            separatorTypeAscii.Checked = (packetFormat.SeparatorType == DelimiterType.Ascii);
            separatorTypeHex.Checked = (packetFormat.SeparatorType == DelimiterType.Hex);

            textBoxStart.Text = packetFormat.PacketStart;
            textBoxEnd.Text = packetFormat.PacketEnd;
            textBoxSeparator.Text = packetFormat.Separator.ToString();
            useChecksum.Checked = packetFormat.UseCheckSum;
            checksumSize.Text = packetFormat.ChecksumSize.ToString();

            foreach (ValueData valueData in packetFormat.ValueDataList)
            {
                gridViewValueData.Rows.Add(valueData.ObjectName, valueData.ValueName);
            }
        }
    }
}
