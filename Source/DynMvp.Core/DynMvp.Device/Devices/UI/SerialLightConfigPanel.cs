using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class SerialLightConfigPanel : UserControl
    {
        public SerialLightConfigPanel()
        {
            InitializeComponent();

            buttonEditPort.Text = StringManager.GetString(buttonEditPort.Text);
        }
    }
}
