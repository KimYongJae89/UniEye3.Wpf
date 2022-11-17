using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public class UiHelper
    {
        public static Font AutoFontSize(Label label, string text)
        {
            Font font;
            Graphics gp;
            SizeF size;
            float factor, factorX, factorY;
            gp = label.CreateGraphics();
            size = gp.MeasureString(text, label.Font);
            gp.Dispose();

            factorX = (label.Width) / size.Width;
            factorY = (label.Height) / size.Height;
            if (factorX > factorY)
            {
                factor = factorY;
            }
            else
            {
                factor = factorX;
            }

            font = label.Font;

            return new Font(font.Name, font.SizeInPoints * (factor) - 1);
        }

        public static void ExportCsv(DataTable dataTable)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ExportCsv(dataTable, dialog.FileName);
        }

        public static void ExportCsv(DataTable dataTable, string fileName)
        {
            try
            {
                var csvFileWriter = new StreamWriter(fileName, false, Encoding.Default);

                string oneLine = "";
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (oneLine != "")
                    {
                        oneLine += ",";
                    }

                    oneLine += column.ColumnName;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataRow row in dataTable.Rows)
                {
                    oneLine = "";
                    foreach (object obj in row.ItemArray)
                    {
                        if (oneLine != "")
                        {
                            oneLine += ",";
                        }

                        oneLine += "\"" + obj.ToString() + "\"";
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
        }

        public static void ExportCsv(DataGridView dataGridView)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ExportCsv(dataGridView, dialog.FileName);
        }

        public static void ExportCsv(DataGridView dataGridView, string fileName)
        {

            StreamWriter csvFileWriter = null;

            try
            {
                csvFileWriter = new StreamWriter(fileName, false);

                string oneLine = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (oneLine != "")
                    {
                        oneLine += ",";
                    }

                    oneLine += column.HeaderText;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    oneLine = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (oneLine != "")
                        {
                            oneLine += ",";
                        }

                        oneLine += cell.Value.ToString();
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());

                if (csvFileWriter != null)
                {
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
                }
            }
        }

        public static void MoveUp(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex <= 0)
            {
                return;
            }

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex - 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex - 1].Selected = true;
        }

        public static void MoveDown(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex >= (dataGridView.Rows.Count - 1))
            {
                return;
            }

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex + 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex + 1].Selected = true;
        }

        public static void ShowScreenKeyboard()
        {
            Process.Start("osk.exe");
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static void SuspendDrawing(Control target)
        {
            SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
        }

        public static void ResumeDrawing(Control target) { ResumeDrawing(target, true); }
        public static void ResumeDrawing(Control target, bool redraw)
        {
            SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

            if (redraw)
            {
                target.Refresh();
            }
        }

        private delegate void SetControlTextDelegate(Control control, string value);
        public static void SetControlText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new SetControlTextDelegate(SetControlText), control, text);
                return;
            }
            control.Text = text;
        }

        private delegate void SetCheckboxCheckedDelegate(CheckBox checkBox, bool value);
        public static void SetCheckboxChecked(CheckBox checkBox, bool value)
        {
            if (checkBox.InvokeRequired)
            {
                checkBox.BeginInvoke(new SetCheckboxCheckedDelegate(SetCheckboxChecked), checkBox, value);
                return;
            }
            checkBox.Checked = value;

        }

        private delegate void SetNumericValueDelegate(NumericUpDown numericUpDown, decimal dec);
        public static void SetNumericValue(NumericUpDown numericUpDown, decimal dec)
        {
            if (numericUpDown.InvokeRequired)
            {
                numericUpDown.BeginInvoke(new SetNumericValueDelegate(SetNumericValue), numericUpDown, dec);
                return;
            }

            numericUpDown.Value = Base.MathHelper.Clipping(dec, numericUpDown.Minimum, numericUpDown.Maximum);
        }

        private delegate void SetNumericMinMaxDelegate(NumericUpDown numericUpDown, decimal min, decimal max);
        public static void SetNumericMinMax(NumericUpDown numericUpDown, decimal min, decimal max)
        {
            if (numericUpDown.InvokeRequired)
            {
                numericUpDown.BeginInvoke(new SetNumericMinMaxDelegate(SetNumericMinMax), numericUpDown, min, max);
                return;
            }

            numericUpDown.Minimum = min;
            numericUpDown.Maximum = max;
        }
    }
}
