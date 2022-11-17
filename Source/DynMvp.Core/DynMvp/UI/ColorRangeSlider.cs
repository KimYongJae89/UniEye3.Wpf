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
    public enum ColorRangeType
    {
        Red, Green, Blue, Hue, Saturation, Luminance
    }

    public partial class ColorRangeSlider : UserControl
    {
        public ColorRangeType ColorRangeType { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public ColorRangeSlider()
        {
            InitializeComponent();
        }

        private void ColorRangeSlider_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
