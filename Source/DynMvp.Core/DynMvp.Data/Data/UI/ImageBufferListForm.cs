using DynMvp.Base;
using DynMvp.Data;
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
    public partial class ImageBufferListForm : Form
    {
        public List<string> FilePathList { get; set; } = new List<string>();

        public ImageBufferListForm()
        {
            InitializeComponent();

            // language change
            addImageBufferButton.Text = StringManager.GetString(addImageBufferButton.Text);
            deleteImageBufferButton.Text = StringManager.GetString(deleteImageBufferButton.Text);
            moveUpButton.Text = StringManager.GetString(moveUpButton.Text);
            moveDownButton.Text = StringManager.GetString(moveDownButton.Text);
            moveDownButton.Text = StringManager.GetString(moveDownButton.Text);
        }

        public void UpdateImageList(List<string> filePathList)
        {
            FilePathList.AddRange(filePathList);

            RefreshList();
        }

        public void RefreshList()
        {
            imageBufferPaths.Items.Clear();

            foreach (string filePath in FilePathList)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                imageBufferPaths.Items.Add(fileName);
            }

            if (imageBufferPaths.Items.Count > 0)
            {
                imageBufferPaths.SelectedIndex = 0;
            }
        }

        private void addImageBufferButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                FilePathList.Add(dialog.SelectedPath);
                int index = imageBufferPaths.Items.Add(Path.GetFileNameWithoutExtension(dialog.SelectedPath));
                imageBufferPaths.SelectedIndex = index;
            }
        }

        private void deleteImageBufferButton_Click(object sender, EventArgs e)
        {
            FilePathList.RemoveAt(imageBufferPaths.SelectedIndex);

            RefreshList();
        }

        private void imageBufferFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {

        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {

        }

        private void ImageBufferListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
