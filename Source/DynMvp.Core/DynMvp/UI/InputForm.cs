using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class InputForm : Form
    {
        public string InputText { get; set; }
        public string NowString { get; set; }


        public InputForm(string lableString)
        {
            InitializeComponent();

            textLabel.Text = lableString;
            okButton.Text = StringManager.GetString(okButton.Text);
            cancelButton.Text = StringManager.GetString(cancelButton.Text);
            textLabel.Text = StringManager.GetString(textLabel.Text);
            InputForm.ActiveForm.Text = StringManager.GetString(InputForm.ActiveForm.Text);
        }

        public InputForm(string lableString, string nowString)
        {
            InitializeComponent();

            textLabel.Text = lableString;
            okButton.Text = StringManager.GetString(okButton.Text);
            cancelButton.Text = StringManager.GetString(cancelButton.Text);
            textLabel.Text = StringManager.GetString(textLabel.Text);
            inputTextBox.Text = nowString;
            InputForm.ActiveForm.Text = StringManager.GetString(InputForm.ActiveForm.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            inputTextBox.Text = inputTextBox.Text;
            InputText = inputTextBox.Text;
        }

        private void inputTextBox_Enter(object sender, EventArgs e)
        {
            UpDownControl.ShowControl("Text", inputTextBox);
        }

        private void inputTextBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideAllControls();
        }

        public void ChangeLocation(Point point)
        {
            Location = point;
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
