using DynMvp.Base;
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
    public partial class ProjectionView : Form
    {
        public Bitmap Image { get; set; }
        public DataSet DataSet1 { get; set; } = new DataSet();
        public DataSet DataSet2 { get; set; } = new DataSet();

        private ProjectionGraphControl xProjectionGraph;
        private ProjectionGraphControl yProjectionGraph;

        public ProjectionView()
        {
            xProjectionGraph = new ProjectionGraphControl();
            yProjectionGraph = new ProjectionGraphControl();

            InitializeComponent();

            saveImage.Text = StringManager.GetString(saveImage.Text);

            tableLayoutPanel1.Controls.Add(xProjectionGraph, 0, 1);
            tableLayoutPanel1.Controls.Add(yProjectionGraph, 1, 0);

            xProjectionGraph.BackColor = System.Drawing.SystemColors.ButtonFace;
            xProjectionGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            xProjectionGraph.Location = new System.Drawing.Point(0, 313);
            xProjectionGraph.Name = "xProjectionGraph";
            xProjectionGraph.Size = new System.Drawing.Size(466, 359);
            xProjectionGraph.TabIndex = 0;
            xProjectionGraph.Direction = ProjectionGraphDirection.xProjection;

            yProjectionGraph.BackColor = System.Drawing.SystemColors.ButtonFace;
            yProjectionGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            yProjectionGraph.Location = new System.Drawing.Point(0, 313);
            yProjectionGraph.Name = "yProjectionGraph";
            yProjectionGraph.Size = new System.Drawing.Size(466, 359);
            yProjectionGraph.TabIndex = 0;
            yProjectionGraph.Direction = ProjectionGraphDirection.yProjection;
        }

        private void ProjectionView_Load(object sender, EventArgs e)
        {
            imageBox.Image = Image;

            xProjectionGraph.Data = DataSet1.XProjection;
            xProjectionGraph.MaxValue = DataSet1.MaxValue;
            xProjectionGraph.MinValue = DataSet1.MinValue;
            xProjectionGraph.Data2 = DataSet2.XProjection;
            xProjectionGraph.MaxValue2 = DataSet2.MaxValue;
            xProjectionGraph.MinValue2 = DataSet2.MinValue;
            xProjectionGraph.Invalidate();

            yProjectionGraph.Data = DataSet1.YProjection;
            yProjectionGraph.MaxValue = DataSet1.MaxValue;
            yProjectionGraph.MinValue = DataSet1.MinValue;
            yProjectionGraph.Data2 = DataSet2.YProjection;
            yProjectionGraph.MaxValue2 = DataSet2.MaxValue;
            yProjectionGraph.MinValue2 = DataSet2.MinValue;
            yProjectionGraph.Invalidate();
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                imageBox.Image.Save(dialog.FileName);
            }
        }
    }

    public class DataSet
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float[] XProjection { get; set; }
        public float[] YProjection { get; set; }
    }
}
