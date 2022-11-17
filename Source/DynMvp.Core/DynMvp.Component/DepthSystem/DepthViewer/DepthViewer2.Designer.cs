namespace DynMvp.Component.DepthSystem.DepthViewer
{
    partial class DepthViewer2
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DepthViewer));
            this.currentGL = new OpenTK.GLControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.loadToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.planeXyTtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.planeXzToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.planeYzToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rotateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.polygonModeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.fillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointCloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.rainbowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hsv1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hsv2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.increaseHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.decreaseHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.showCrossSectionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentGL
            // 
            this.currentGL.BackColor = System.Drawing.Color.Black;
            this.currentGL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentGL.Location = new System.Drawing.Point(0, 39);
            this.currentGL.Margin = new System.Windows.Forms.Padding(4);
            this.currentGL.Name = "currentGL";
            this.currentGL.Size = new System.Drawing.Size(546, 347);
            this.currentGL.TabIndex = 4;
            this.currentGL.VSync = true;
            this.currentGL.Paint += new System.Windows.Forms.PaintEventHandler(this.currentGL_Paint);
            this.currentGL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.currentGL_MouseDown);
            this.currentGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.currentGL_MouseMove);
            this.currentGL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.currentGL_MouseUp);
            this.currentGL.Resize += new System.EventHandler(this.currentGL_Resize);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.planeXyTtoolStripButton,
            this.planeXzToolStripButton,
            this.planeYzToolStripButton,
            this.toolStripSeparator2,
            this.panToolStripButton,
            this.rotateToolStripButton,
            this.toolStripSeparator3,
            this.polygonModeToolStripDropDownButton,
            this.toolStripDropDownButton1,
            this.defaultHeightScaleToolStripButton,
            this.increaseHeightScaleToolStripButton,
            this.decreaseHeightScaleToolStripButton,
            this.showCrossSectionToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(546, 39);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // loadToolStripButton
            // 
            this.loadToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadToolStripButton.Image = global::DynMvp.Component.Properties.Resources.Open;
            this.loadToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.loadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadToolStripButton.Name = "loadToolStripButton";
            this.loadToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.loadToolStripButton.Text = "toolStripButton1";
            this.loadToolStripButton.Click += new System.EventHandler(this.loadToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = global::DynMvp.Component.Properties.Resources.Save;
            this.saveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.saveToolStripButton.Text = "toolStripButton2";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // planeXyTtoolStripButton
            // 
            this.planeXyTtoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.planeXyTtoolStripButton.Image = global::DynMvp.Component.Properties.Resources.PlaneXY;
            this.planeXyTtoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.planeXyTtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.planeXyTtoolStripButton.Name = "planeXyTtoolStripButton";
            this.planeXyTtoolStripButton.Size = new System.Drawing.Size(36, 36);
            this.planeXyTtoolStripButton.Text = "toolStripButton3";
            this.planeXyTtoolStripButton.Click += new System.EventHandler(this.planeXyTtoolStripButton_Click);
            // 
            // planeXzToolStripButton
            // 
            this.planeXzToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.planeXzToolStripButton.Image = global::DynMvp.Component.Properties.Resources.PlaneXZ;
            this.planeXzToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.planeXzToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.planeXzToolStripButton.Name = "planeXzToolStripButton";
            this.planeXzToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.planeXzToolStripButton.Text = "toolStripButton4";
            this.planeXzToolStripButton.Click += new System.EventHandler(this.planeXzToolStripButton_Click);
            // 
            // planeYzToolStripButton
            // 
            this.planeYzToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.planeYzToolStripButton.Image = global::DynMvp.Component.Properties.Resources.PlaneYZ;
            this.planeYzToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.planeYzToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.planeYzToolStripButton.Name = "planeYzToolStripButton";
            this.planeYzToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.planeYzToolStripButton.Text = "toolStripButton5";
            this.planeYzToolStripButton.Click += new System.EventHandler(this.planeYzToolStripButton_Click);
            // 
            // panToolStripButton
            // 
            this.panToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.panToolStripButton.Image = global::DynMvp.Component.Properties.Resources.CubePan;
            this.panToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.panToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.panToolStripButton.Name = "panToolStripButton";
            this.panToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.panToolStripButton.Text = "toolStripButton7";
            this.panToolStripButton.Click += new System.EventHandler(this.panToolStripButton_Click);
            // 
            // rotateToolStripButton
            // 
            this.rotateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateToolStripButton.Image = global::DynMvp.Component.Properties.Resources.CubeRotate;
            this.rotateToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.rotateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateToolStripButton.Name = "rotateToolStripButton";
            this.rotateToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.rotateToolStripButton.Text = "toolStripButton6";
            this.rotateToolStripButton.Click += new System.EventHandler(this.rotateToolStripButton_Click);
            // 
            // polygonModeToolStripDropDownButton
            // 
            this.polygonModeToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.polygonModeToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillToolStripMenuItem,
            this.wireframeToolStripMenuItem,
            this.pointCloudToolStripMenuItem});
            this.polygonModeToolStripDropDownButton.Image = global::DynMvp.Component.Properties.Resources.fill;
            this.polygonModeToolStripDropDownButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.polygonModeToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.polygonModeToolStripDropDownButton.Name = "polygonModeToolStripDropDownButton";
            this.polygonModeToolStripDropDownButton.Size = new System.Drawing.Size(45, 36);
            this.polygonModeToolStripDropDownButton.Text = "toolStripDropDownButton1";
            // 
            // fillToolStripMenuItem
            // 
            this.fillToolStripMenuItem.Image = global::DynMvp.Component.Properties.Resources.fill;
            this.fillToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fillToolStripMenuItem.Name = "fillToolStripMenuItem";
            this.fillToolStripMenuItem.Size = new System.Drawing.Size(168, 38);
            this.fillToolStripMenuItem.Text = "Fill";
            this.fillToolStripMenuItem.Click += new System.EventHandler(this.fillToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Image = global::DynMvp.Component.Properties.Resources.Wireframe;
            this.wireframeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(168, 38);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // pointCloudToolStripMenuItem
            // 
            this.pointCloudToolStripMenuItem.Image = global::DynMvp.Component.Properties.Resources.PointCloud;
            this.pointCloudToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pointCloudToolStripMenuItem.Name = "pointCloudToolStripMenuItem";
            this.pointCloudToolStripMenuItem.Size = new System.Drawing.Size(168, 38);
            this.pointCloudToolStripMenuItem.Text = "Point Cloud";
            this.pointCloudToolStripMenuItem.Click += new System.EventHandler(this.pointCloudToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rainbowToolStripMenuItem,
            this.hsv1ToolStripMenuItem,
            this.hsv2ToolStripMenuItem,
            this.grayToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::DynMvp.Component.Properties.Resources.colorchart;
            this.toolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 36);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // rainbowToolStripMenuItem
            // 
            this.rainbowToolStripMenuItem.Name = "rainbowToolStripMenuItem";
            this.rainbowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rainbowToolStripMenuItem.Text = "Rainbow";
            this.rainbowToolStripMenuItem.Click += new System.EventHandler(this.rainbowToolStripMenuItem_Click);
            // 
            // hsv1ToolStripMenuItem
            // 
            this.hsv1ToolStripMenuItem.Name = "hsv1ToolStripMenuItem";
            this.hsv1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hsv1ToolStripMenuItem.Text = "HSV1";
            this.hsv1ToolStripMenuItem.Click += new System.EventHandler(this.hsv1ToolStripMenuItem_Click);
            // 
            // hsv2ToolStripMenuItem
            // 
            this.hsv2ToolStripMenuItem.Name = "hsv2ToolStripMenuItem";
            this.hsv2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hsv2ToolStripMenuItem.Text = "HSV2";
            this.hsv2ToolStripMenuItem.Click += new System.EventHandler(this.hsv2ToolStripMenuItem_Click);
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            this.grayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grayToolStripMenuItem.Text = "Gray";
            this.grayToolStripMenuItem.Click += new System.EventHandler(this.grayToolStripMenuItem_Click);
            // 
            // defaultHeightScaleToolStripButton
            // 
            this.defaultHeightScaleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.defaultHeightScaleToolStripButton.Image = global::DynMvp.Component.Properties.Resources.Ruler;
            this.defaultHeightScaleToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.defaultHeightScaleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.defaultHeightScaleToolStripButton.Name = "defaultHeightScaleToolStripButton";
            this.defaultHeightScaleToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.defaultHeightScaleToolStripButton.Text = "toolStripButton1";
            this.defaultHeightScaleToolStripButton.Click += new System.EventHandler(this.defaultHeightScaleToolStripButton_Click);
            // 
            // increaseHeightScaleToolStripButton
            // 
            this.increaseHeightScaleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.increaseHeightScaleToolStripButton.Image = global::DynMvp.Component.Properties.Resources.Ruler_inc;
            this.increaseHeightScaleToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.increaseHeightScaleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.increaseHeightScaleToolStripButton.Name = "increaseHeightScaleToolStripButton";
            this.increaseHeightScaleToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.increaseHeightScaleToolStripButton.Text = "toolStripButton2";
            this.increaseHeightScaleToolStripButton.Click += new System.EventHandler(this.increaseHeightScaleToolStripButton_Click);
            // 
            // decreaseHeightScaleToolStripButton
            // 
            this.decreaseHeightScaleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.decreaseHeightScaleToolStripButton.Image = global::DynMvp.Component.Properties.Resources.Ruler_dec;
            this.decreaseHeightScaleToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.decreaseHeightScaleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.decreaseHeightScaleToolStripButton.Name = "decreaseHeightScaleToolStripButton";
            this.decreaseHeightScaleToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.decreaseHeightScaleToolStripButton.Text = "toolStripButton3";
            this.decreaseHeightScaleToolStripButton.Click += new System.EventHandler(this.decreaseHeightScaleToolStripButton_Click);
            // 
            // showCrossSectionToolStripButton
            // 
            this.showCrossSectionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showCrossSectionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("showCrossSectionToolStripButton.Image")));
            this.showCrossSectionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showCrossSectionToolStripButton.Name = "showCrossSectionToolStripButton";
            this.showCrossSectionToolStripButton.Size = new System.Drawing.Size(23, 36);
            this.showCrossSectionToolStripButton.Text = "toolStripButton1";
            this.showCrossSectionToolStripButton.Click += new System.EventHandler(this.showCrossSectionToolStripButton_Click);
            // 
            // DepthViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentGL);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DepthViewer";
            this.Size = new System.Drawing.Size(546, 386);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl currentGL;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton planeXyTtoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton planeXzToolStripButton;
        private System.Windows.Forms.ToolStripButton planeYzToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton rotateToolStripButton;
        private System.Windows.Forms.ToolStripButton panToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton polygonModeToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem fillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointCloudToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem rainbowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hsv1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hsv2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton defaultHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton increaseHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton decreaseHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton showCrossSectionToolStripButton;
    }
}
