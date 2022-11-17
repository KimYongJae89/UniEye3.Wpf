using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public partial class DepthViewForm : Form
    {
        private DepthViewer depthViewer;

        public Image3D Image3d
        {
            set => depthViewer.SetDepthData(value, 0.3f);
        }

        public DepthViewForm()
        {
            InitializeComponent();
            DepthViewForm.ActiveForm.Text = StringManager.GetString(DepthViewForm.ActiveForm.Text);

            depthViewer = new DepthViewer();

            DepthViewForm_Fill_Panel.ClientArea.Controls.Add(depthViewer);

            depthViewer.Location = new System.Drawing.Point(3, 3);
            depthViewer.Name = "depthViewer";
            depthViewer.Size = new System.Drawing.Size(409, 523);
            depthViewer.Dock = DockStyle.Fill;
            depthViewer.Visible = true;
            depthViewer.PixelResolution = 0.3f;
        }
    }
}
