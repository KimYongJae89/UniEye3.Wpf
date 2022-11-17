using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

namespace DynMvp.Vision.UI
{
    public partial class HistogramView : Form
    {
        internal Histogram Histogram { get; set; }

        private ZedGraphControl graphControl = new ZedGraphControl();

        public HistogramView()
        {
            InitializeComponent();

            graphControl.BackColor = System.Drawing.SystemColors.ButtonFace;
            graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            graphControl.Location = new System.Drawing.Point(0, 313);
            graphControl.Name = "graphControl";
            graphControl.Size = new System.Drawing.Size(466, 359);
            graphControl.TabIndex = 0;

            Controls.Clear();

            Controls.Add(graphControl);
        }

        private void HistogramView_Load(object sender, EventArgs e)
        {
            var dataPointPairList = new PointPairList();
            for (int i = 0; i < Histogram.HistogramData.Count(); i++)
            {
                dataPointPairList.Add(i, Histogram.HistogramData[i]);
            }

            graphControl.GraphPane.AddBar("Height", dataPointPairList, Color.Red);

            graphControl.GraphPane.YAxis.Scale.Max = Histogram.HistogramData.Max();
            graphControl.GraphPane.YAxis.Scale.Min = Histogram.HistogramData.Min();
            graphControl.GraphPane.XAxis.Scale.Max = Histogram.HistogramData.Count();
            graphControl.GraphPane.XAxis.Scale.Min = 0;
        }
    }
}
