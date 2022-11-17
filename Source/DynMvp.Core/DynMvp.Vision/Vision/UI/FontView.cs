using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.UI
{
    public partial class FontView : Form
    {
        private CharReader charReader;

        public FontView()
        {
            InitializeComponent();

            deleteFontButton.Text = StringManager.GetString(deleteFontButton.Text);
            okButton.Text = StringManager.GetString(okButton.Text);


        }

        public void Initialize(CharReader charReader)
        {
            this.charReader = charReader;
        }

        private void FontView_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            fontGrid.Rows.Clear();
            fontGrid.Columns.Clear();

            List<CharFont> fontList = charReader.GetFontList();

            for (int i = 0; i < 10; i++)
            {
                var imageColumn = new DataGridViewImageColumn();
                imageColumn.Width = 30;
                imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageColumn.DefaultCellStyle.NullValue = null;
                imageColumn.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);

                fontGrid.Columns.Add(imageColumn);

                var charColumn = new DataGridViewTextBoxColumn();
                charColumn.Width = 20;
                charColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                charColumn.DefaultCellStyle.NullValue = null;

                fontGrid.Columns.Add(charColumn);
            }

            int rowIndex = 0;
            int index = 0;
            foreach (CharFont charFont in fontList)
            {
                if (index == 20)
                {
                    rowIndex = fontGrid.Rows.Add();
                    index = 0;
                }

                fontGrid.Rows[rowIndex].Cells[index].Value = charFont.Image;
                fontGrid.Rows[rowIndex].Cells[index + 1].Value = charFont.Character;
                fontGrid.Rows[rowIndex].Cells[index].Tag = charFont;
                fontGrid.Rows[rowIndex].Cells[index + 1].Tag = null;

                index += 2;
            }
        }

        private void deleteFontButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in fontGrid.SelectedCells)
            {
                if (cell.Tag != null)
                {
                    charReader.RemoveFont((CharFont)cell.Tag);
                }
            }

            RefreshGrid();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var charReaderParam = (CharReaderParam)charReader.Param;

            charReader.SaveFontFile(charReaderParam.FontFileName);
        }
    }
}
