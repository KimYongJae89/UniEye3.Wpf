namespace DynMvp.Component.DepthSystem.DepthViewer
{
    partial class DepthViewer
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool1 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool2 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Move");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DepthViewer));
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool3 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Rotate");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool4 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("MoveCad");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool5 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("RotateCad");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool6 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("CadFitting2");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool7 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("CadFitting1");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.currentGL = new OpenTK.GLControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.planeXyTtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.planeXzToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.planeYzToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showDepthToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.showCadDepthToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.showCadToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.polygonModeToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.fillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointCloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorMapToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.rainbowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hsv1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hsv2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.increaseHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.decreaseHeightScaleToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.showCrossSectionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.mouseActionRadialMenu = new Infragistics.Win.UltraWinRadialMenu.UltraRadialMenu(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseActionRadialMenu)).BeginInit();
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
            this.currentGL.Size = new System.Drawing.Size(606, 347);
            this.currentGL.TabIndex = 4;
            this.currentGL.VSync = true;
            this.currentGL.Paint += new System.Windows.Forms.PaintEventHandler(this.CurrentGL_Paint);
            this.currentGL.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CurrentGL_MouseDoubleClick);
            this.currentGL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurrentGL_MouseDown);
            this.currentGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurrentGL_MouseMove);
            this.currentGL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurrentGL_MouseUp);
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
            this.showDepthToolStripButton,
            this.showCadDepthToolStripButton,
            this.showCadToolStripButton,
            this.toolStripSeparator3,
            this.polygonModeToolStripDropDownButton,
            this.colorMapToolStripDropDownButton,
            this.defaultHeightScaleToolStripButton,
            this.increaseHeightScaleToolStripButton,
            this.decreaseHeightScaleToolStripButton,
            this.showCrossSectionToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(606, 39);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
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
            this.loadToolStripButton.ToolTipText = "Open";
            this.loadToolStripButton.Click += new System.EventHandler(this.LoadToolStripButton_Click);
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
            this.saveToolStripButton.ToolTipText = "Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
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
            this.planeXyTtoolStripButton.ToolTipText = "Plane X-Y";
            this.planeXyTtoolStripButton.Click += new System.EventHandler(this.PlaneXyTtoolStripButton_Click);
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
            this.planeXzToolStripButton.ToolTipText = "Plane X-Z";
            this.planeXzToolStripButton.Click += new System.EventHandler(this.PlaneXzToolStripButton_Click);
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
            this.planeYzToolStripButton.ToolTipText = "Plane Y-Z";
            this.planeYzToolStripButton.Click += new System.EventHandler(this.PlaneYzToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // showDepthToolStripButton
            // 
            this.showDepthToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showDepthToolStripButton.Image = global::DynMvp.Component.Properties.Resources.depth;
            this.showDepthToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showDepthToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showDepthToolStripButton.Name = "showDepthToolStripButton";
            this.showDepthToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.showDepthToolStripButton.Text = "toolStripButton1";
            this.showDepthToolStripButton.ToolTipText = "Show Depth";
            this.showDepthToolStripButton.Click += new System.EventHandler(this.ShowDepthToolStripButton_Click);
            // 
            // showCadDepthToolStripButton
            // 
            this.showCadDepthToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showCadDepthToolStripButton.Image = global::DynMvp.Component.Properties.Resources.depth;
            this.showCadDepthToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showCadDepthToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showCadDepthToolStripButton.Name = "showCadDepthToolStripButton";
            this.showCadDepthToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.showCadDepthToolStripButton.Text = "toolStripButton1";
            this.showCadDepthToolStripButton.ToolTipText = "Show Cad Depth";
            this.showCadDepthToolStripButton.Click += new System.EventHandler(this.ShowCadDepthToolStripButton_Click);
            // 
            // showCadToolStripButton
            // 
            this.showCadToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showCadToolStripButton.Image = global::DynMvp.Component.Properties.Resources.cad;
            this.showCadToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showCadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showCadToolStripButton.Name = "showCadToolStripButton";
            this.showCadToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.showCadToolStripButton.Text = "toolStripButton1";
            this.showCadToolStripButton.ToolTipText = "Show CAD";
            this.showCadToolStripButton.Click += new System.EventHandler(this.ShowCadToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
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
            this.fillToolStripMenuItem.Size = new System.Drawing.Size(154, 38);
            this.fillToolStripMenuItem.Text = "Fill";
            this.fillToolStripMenuItem.Click += new System.EventHandler(this.FillToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Image = global::DynMvp.Component.Properties.Resources.Wireframe;
            this.wireframeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(154, 38);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.WireframeToolStripMenuItem_Click);
            // 
            // pointCloudToolStripMenuItem
            // 
            this.pointCloudToolStripMenuItem.Image = global::DynMvp.Component.Properties.Resources.PointCloud;
            this.pointCloudToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pointCloudToolStripMenuItem.Name = "pointCloudToolStripMenuItem";
            this.pointCloudToolStripMenuItem.Size = new System.Drawing.Size(154, 38);
            this.pointCloudToolStripMenuItem.Text = "Point Cloud";
            this.pointCloudToolStripMenuItem.Click += new System.EventHandler(this.PointCloudToolStripMenuItem_Click);
            // 
            // colorMapToolStripDropDownButton
            // 
            this.colorMapToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.colorMapToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rainbowToolStripMenuItem,
            this.hsv1ToolStripMenuItem,
            this.hsv2ToolStripMenuItem,
            this.grayToolStripMenuItem});
            this.colorMapToolStripDropDownButton.Image = global::DynMvp.Component.Properties.Resources.colorchart;
            this.colorMapToolStripDropDownButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.colorMapToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.colorMapToolStripDropDownButton.Name = "colorMapToolStripDropDownButton";
            this.colorMapToolStripDropDownButton.Size = new System.Drawing.Size(45, 36);
            this.colorMapToolStripDropDownButton.Text = "toolStripDropDownButton1";
            // 
            // rainbowToolStripMenuItem
            // 
            this.rainbowToolStripMenuItem.Name = "rainbowToolStripMenuItem";
            this.rainbowToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.rainbowToolStripMenuItem.Text = "Rainbow";
            this.rainbowToolStripMenuItem.Click += new System.EventHandler(this.RainbowToolStripMenuItem_Click);
            // 
            // hsv1ToolStripMenuItem
            // 
            this.hsv1ToolStripMenuItem.Name = "hsv1ToolStripMenuItem";
            this.hsv1ToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.hsv1ToolStripMenuItem.Text = "HSV1";
            this.hsv1ToolStripMenuItem.Click += new System.EventHandler(this.Hsv1ToolStripMenuItem_Click);
            // 
            // hsv2ToolStripMenuItem
            // 
            this.hsv2ToolStripMenuItem.Name = "hsv2ToolStripMenuItem";
            this.hsv2ToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.hsv2ToolStripMenuItem.Text = "HSV2";
            this.hsv2ToolStripMenuItem.Click += new System.EventHandler(this.Hsv2ToolStripMenuItem_Click);
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            this.grayToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.grayToolStripMenuItem.Text = "Gray";
            this.grayToolStripMenuItem.Click += new System.EventHandler(this.GrayToolStripMenuItem_Click);
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
            this.defaultHeightScaleToolStripButton.Click += new System.EventHandler(this.DefaultHeightScaleToolStripButton_Click);
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
            this.increaseHeightScaleToolStripButton.Click += new System.EventHandler(this.IncreaseHeightScaleToolStripButton_Click);
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
            this.decreaseHeightScaleToolStripButton.Click += new System.EventHandler(this.DecreaseHeightScaleToolStripButton_Click);
            // 
            // showCrossSectionToolStripButton
            // 
            this.showCrossSectionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showCrossSectionToolStripButton.Image = global::DynMvp.Component.Properties.Resources.LineChart;
            this.showCrossSectionToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showCrossSectionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showCrossSectionToolStripButton.Name = "showCrossSectionToolStripButton";
            this.showCrossSectionToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.showCrossSectionToolStripButton.Text = "toolStripButton1";
            this.showCrossSectionToolStripButton.Click += new System.EventHandler(this.ShowCrossSectionToolStripButton_Click);
            // 
            // mouseActionRadialMenu
            // 
            radialMenuTool2.Key = "Move";
            radialMenuTool2.Text = "Move";
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            radialMenuTool2.ToolSettings.Appearance = appearance1;
            radialMenuTool2.VisiblePosition = 0;
            radialMenuTool3.Key = "Rotate";
            radialMenuTool3.Text = "Rotate";
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            radialMenuTool3.ToolSettings.Appearance = appearance2;
            radialMenuTool3.VisiblePosition = 1;
            radialMenuTool4.Key = "MoveCad";
            radialMenuTool4.Text = "Move CAD";
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            radialMenuTool4.ToolSettings.Appearance = appearance3;
            radialMenuTool4.VisiblePosition = 2;
            radialMenuTool5.Key = "RotateCad";
            radialMenuTool5.Text = "Rotate CAD";
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            radialMenuTool5.ToolSettings.Appearance = appearance4;
            radialMenuTool5.VisiblePosition = 3;
            radialMenuTool6.Key = "CadFitting2";
            radialMenuTool6.Text = "Fit CAD2";
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            radialMenuTool6.ToolSettings.Appearance = appearance5;
            radialMenuTool6.VisiblePosition = 4;
            radialMenuTool7.Key = "CadFitting1";
            radialMenuTool7.Text = "Fit CAD1";
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            radialMenuTool7.ToolSettings.Appearance = appearance6;
            radialMenuTool7.VisiblePosition = 5;
            radialMenuTool1.Tools.Add(radialMenuTool2);
            radialMenuTool1.Tools.Add(radialMenuTool3);
            radialMenuTool1.Tools.Add(radialMenuTool4);
            radialMenuTool1.Tools.Add(radialMenuTool5);
            radialMenuTool1.Tools.Add(radialMenuTool6);
            radialMenuTool1.Tools.Add(radialMenuTool7);
            this.mouseActionRadialMenu.CenterTool = radialMenuTool1;
            this.mouseActionRadialMenu.Expanded = true;
            this.mouseActionRadialMenu.MenuSettings.WedgeCount = 6;
            this.mouseActionRadialMenu.OwningControl = this;
            this.mouseActionRadialMenu.ToolClick += new System.EventHandler<Infragistics.Win.UltraWinRadialMenu.RadialMenuToolClickEventArgs>(this.MouseActionRadialMenu_ToolClick);
            // 
            // DepthViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentGL);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DepthViewer";
            this.Size = new System.Drawing.Size(606, 386);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseActionRadialMenu)).EndInit();
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton polygonModeToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem fillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointCloudToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton colorMapToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem rainbowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hsv1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hsv2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton defaultHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton increaseHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton decreaseHeightScaleToolStripButton;
        private System.Windows.Forms.ToolStripButton showCrossSectionToolStripButton;
        private Infragistics.Win.UltraWinRadialMenu.UltraRadialMenu mouseActionRadialMenu;
        private System.Windows.Forms.ToolStripButton showDepthToolStripButton;
        private System.Windows.Forms.ToolStripButton showCadToolStripButton;
        private System.Windows.Forms.ToolStripButton showCadDepthToolStripButton;
    }
}
