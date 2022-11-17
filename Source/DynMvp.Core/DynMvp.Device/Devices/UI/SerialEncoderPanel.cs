using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class SerialEncoderPanel : Form
    {
        private Dictionary<Enum, string> dataSource = new Dictionary<Enum, string>();
        public SerialEncoder SerialEncoder { get; set; }

        public SerialEncoderPanel()
        {
            InitializeComponent();
            Disposed += SerialEncoderPanel_Disposed;

        }

        public void Initialize(SerialEncoder serialEncoder)
        {
            SerialEncoder = serialEncoder;
        }

        private void SerialEncoderPanel_Disposed(object sender, EventArgs e)
        {
        }

        private void SerialEncoderPanel_Load(object sender, EventArgs e)
        {
            UpdateDataSoruce();

            UpdateDataGrid();
            CenterToScreen();
        }

        private void UpdateDataGrid()
        {
            dataGridView1.Columns.Clear();
            DataGridViewColumn colName = new DataGridViewTextBoxColumn();
            colName.ReadOnly = true;
            colName.HeaderText = "Name";
            dataGridView1.Columns.Add(colName);

            DataGridViewColumn colValue = new DataGridViewTextBoxColumn();
            colValue.ReadOnly = false;
            colValue.HeaderText = "Value";
            dataGridView1.Columns.Add(colValue);

            dataGridView1.RowCount = dataSource.Count;

            dataGridView1.Invalidate();
        }

        private void UpdateDataSoruce()
        {
            dataSource.Clear();

            if (SerialEncoder.IsCompatible(SerialEncoderV105.ECommand.GR))
            {
                ExcuteCommand(SerialEncoderV105.ECommand.GR);
            }

            if (SerialEncoder.IsCompatible(SerialEncoderV107.ECommandV2.PC))
            {
                ExcuteCommand(SerialEncoderV107.ECommandV2.PC);
            }
        }

        private bool isWorkingCommand = false;
        private void ExcuteCommand(string packetString)
        {
            if (!isWorkingCommand)
            {
                isWorkingCommand = true;

                string[] token = SerialEncoder.ExcuteCommand(packetString);
                UpdateValue(token);

                if (token != null)
                {
                    // send ok
                    AppendLogText("TX >>    [ OK ] : " + packetString);

                    if (token.Length > 0)
                    {
                        // recive ok
                        var sb = new StringBuilder();
                        sb.Append(token[0]);
                        for (int i = 1; i < token.Length; i++)
                        {
                            sb.AppendFormat(",{0}", token[i]);
                        }

                        string tokenString = sb.ToString().Trim();
                        AppendLogText("   << RX [ OK ] : " + tokenString);
                    }
                    else
                    {
                        // recive fail
                        AppendLogText("   << RX [FAIL] : ");
                    }
                }
                else
                {
                    // send Fail
                    AppendLogText("TX >>    [FAIL] : " + packetString);
                }

                isWorkingCommand = false;
            }
        }

        private delegate void AppendLogTextDelegate(string v);
        private void AppendLogText(string v)
        {
            if (textBox1.IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new AppendLogTextDelegate(AppendLogText), v);
                return;
            }

            textBox1.AppendText("\r\n" + v);
        }

        private void ExcuteCommand(Enum en, params string[] args)
        {
            if (SerialEncoder.IsCompatible(en))
            {
                //serialEncoder.ExcuteCommand(en, args);
                string packetString = SerialEncoder.MakePacket(en.ToString(), args);
                ExcuteCommand(packetString);
            }
        }

        private delegate void UpdateValueDelegate(string[] token);
        private void UpdateValue(string[] token)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateValueDelegate(UpdateValue), token);
                return;
            }

            if (token == null)
            {
                return;
            }

            for (int i = 0; i < token.Count(); i++)
            {
                string s = string.Join("", token[i].Where(char.IsLetter));
                if (SerialEncoder.IsCompatible(s))
                {
                    Enum e = SerialEncoder.GetCommand(s);

                    lock (dataSource)
                    {
                        switch (e)
                        {
                            case SerialEncoderV105.ECommand.AR:
                                dataSource[e] = string.Format("{0},{1}", token[i + 1], token[i + 2]);
                                break;
                            default:
                                dataSource[e] = token[i + 1];
                                break;
                        }
                    }
                }
            }

            if (checkBoxAutoUpdate.Checked)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.AP);
            }

            if (checkBoxAutoUpdate.Checked)
            {
                ExcuteCommand(SerialEncoderV107.ECommandV2.PC);
            }

            dataGridView1.RowCount = dataSource.Count;
            dataGridView1.Invalidate();
        }

        private void button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (button == buttonEn0)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.EN, "0");
            }
            else if (button == buttonEn1)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.EN, "1");
            }
            else if (button == buttonIn0)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.IN, "0");
            }
            else if (button == buttonIn1)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.IN, "1");
            }
            else if (button == buttonGr)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.GR);
            }
            else if (button == buttonCp)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.CP);
            }
            else if (button == buttonAP)
            {
                ExcuteCommand(SerialEncoderV105.ECommand.AP);
            }
            else if (button == buttonCc)
            {
                ExcuteCommand(SerialEncoderV107.ECommandV2.CC);
            }


            //ExcuteCommand(SerialEncoderV105.ECommand.GR);
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = dataSource.ElementAt(e.RowIndex).Key;
                    break;
                case 1:
                    e.Value = dataSource.ElementAt(e.RowIndex).Value;
                    break;
            }
        }

        private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            string[] token = null;
            switch (e.ColumnIndex)
            {
                case 1:
                    Enum en = dataSource.ElementAt(e.RowIndex).Key;
                    //SerialEncoder.ECommand key = dataSource.ElementAt(e.RowIndex).Key;
                    string value = e.Value.ToString();
                    ExcuteCommand(en, value);
                    break;
            }

            token = SerialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.GR);
            UpdateValue(token);
        }

        private void ManualCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string packetString = manualCommand.Text;
                manualCommand.Text = "";
                ExcuteCommand(packetString);
            }
        }

        private Task autoUpdateTask = null;
        private void checkBoxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;

            if (chk.Checked)
            {
                if (autoUpdateTask == null)
                {
                    autoUpdateTask = Task.Factory.StartNew(() =>
                    {
                        while (checkBoxAutoUpdate.Checked)
                        {
                            if (SerialEncoder.IsCompatible(SerialEncoderV105.ECommand.GR))
                            {
                                ExcuteCommand(SerialEncoderV105.ECommand.AP);
                            }

                            if (SerialEncoder.IsCompatible(SerialEncoderV107.ECommandV2.PC))
                            {
                                ExcuteCommand(SerialEncoderV107.ECommandV2.PC);
                            }

                            Thread.Sleep(200);
                        }

                        autoUpdateTask = null;
                    });
                }
            }
        }
    }
}
