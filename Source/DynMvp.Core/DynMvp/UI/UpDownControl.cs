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
    public partial class UpDownControl : Form
    {
        private const int initDelay = 500;
        private const int repeatDelay = 100;

        private static Dictionary<Control, UpDownControl> upDownControlDictionary = new Dictionary<Control, UpDownControl>();

        private static bool useUpDownControl = false;
        public static bool UseUpDownControl
        {
            get => UpDownControl.useUpDownControl;
            set => UpDownControl.useUpDownControl = value;
        }

        public static void ShowControl(string valueName, Control linkedControl)
        {
            if (useUpDownControl == false)
            {
                return;
            }

            if (upDownControlDictionary.TryGetValue(linkedControl, out UpDownControl upDownControl) == true)
            {
                return;
            }

            upDownControl = new UpDownControl();
            upDownControl.ValueName = valueName;
            upDownControl.LinkedControl = linkedControl;

            Rectangle screenRectangle = Screen.GetBounds(linkedControl);
            Point location = linkedControl.PointToScreen(new Point(0, 0));
            if (location.Y + linkedControl.Height + upDownControl.Height > screenRectangle.Height)
            {
                location.Y -= upDownControl.Height;
            }
            else
            {
                location.Y += linkedControl.Height;
            }

            if (((location.X + upDownControl.Width) - screenRectangle.X) > screenRectangle.Width)
            {
                location.X = screenRectangle.X + screenRectangle.Width - upDownControl.Width;
            }

            upDownControl.Location = location;

            upDownControl.Show(linkedControl);
            linkedControl.Focus();

            upDownControlDictionary.Add(linkedControl, upDownControl);
        }

        public static void HideControl(Control linkedControl)
        {
            if (useUpDownControl == false)
            {
                return;
            }

            if (upDownControlDictionary.TryGetValue(linkedControl, out UpDownControl upDownControl) == true)
            {
                upDownControl.Close();
                upDownControl.Dispose();
                upDownControlDictionary.Remove(linkedControl);
            }
        }

        public static void HideAllControls()
        {
            if (useUpDownControl == false)
            {
                return;
            }

            foreach (KeyValuePair<Control, UpDownControl> keyValuePair in upDownControlDictionary)
            {
                keyValuePair.Value.Close();
                keyValuePair.Value.Dispose();
            }
            upDownControlDictionary.Clear();
        }

        private bool downButtonDown = false;
        private bool upButtonDown = false;
        private string valueName;
        public string ValueName
        {
            set => valueName = value;
        }
        public Control LinkedControl { get; set; }

        public UpDownControl()
        {
            InitializeComponent();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            LinkedControl.Text = (Convert.ToInt32(LinkedControl.Text) - 1).ToString();
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            LinkedControl.Text = (Convert.ToInt32(LinkedControl.Text) + 1).ToString();
        }

        private void keyboardButton_Click(object sender, EventArgs e)
        {
            UiHelper.ShowScreenKeyboard();
        }

        private void UpDownControl_Load(object sender, EventArgs e)
        {
            if (LinkedControl as NumericUpDown == null)
            {
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            HideControl(LinkedControl);
        }

        private void repeatClickTimer_Tick(object sender, EventArgs e)
        {
            repeatClickTimer.Interval = repeatDelay;
            if (downButtonDown)
            {
                downButton.PerformClick();
            }
            else if (upButtonDown)
            {
                upButton.PerformClick();
            }
        }

        private void downButton_MouseDown(object sender, MouseEventArgs e)
        {
            repeatClickTimer.Interval = initDelay;
            repeatClickTimer.Enabled = true;
            downButtonDown = true;
        }

        private void upButton_MouseUp(object sender, MouseEventArgs e)
        {
            repeatClickTimer.Enabled = false;
            upButtonDown = false;
        }

        private void upButton_MouseDown(object sender, MouseEventArgs e)
        {
            repeatClickTimer.Interval = initDelay;
            repeatClickTimer.Enabled = true;
            upButtonDown = true;
        }

        private void downButton_MouseUp(object sender, MouseEventArgs e)
        {
            repeatClickTimer.Enabled = false;
            downButtonDown = false;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (LinkedControl.Parent.SelectNextControl(LinkedControl, true, true, true, true) == false)
            {
                HideControl(LinkedControl);
            }
        }
    }
}
