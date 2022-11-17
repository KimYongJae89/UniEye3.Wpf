namespace UniEye.Base.UI.Main2018
{
    partial class ModellerPage
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grab3dToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._ModellerPageUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ModellerPageUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ModellerPageUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ModellerPageUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ModellerPageAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.cameraImagePanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dockingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemModellerToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemParamPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemResultPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemModelTreePanel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFovNaviPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFiducialPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.teachDefaultLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLoadTeachLayoutFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveTeachLayoutFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLoadResultLayoutFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveResultLayoutFile = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            this.cameraImagePanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // grab3dToolStripButton
            // 
            this.grab3dToolStripButton.Image = global::UniEye.Base.Properties.Resources.cube3d_32;
            this.grab3dToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.grab3dToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.grab3dToolStripButton.Name = "grab3dToolStripButton";
            this.grab3dToolStripButton.Size = new System.Drawing.Size(69, 58);
            this.grab3dToolStripButton.Text = "Grab3D";
            this.grab3dToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.grab3dToolStripButton.Click += new System.EventHandler(this.Grab3dToolStripButton_Click);
            // 
            // ultraDockManager
            // 
            this.ultraDockManager.CompressUnpinnedTabs = false;
            this.ultraDockManager.HostControl = this;
            this.ultraDockManager.SettingsKey = "";
            this.ultraDockManager.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2008;
            // 
            // _ModellerPageUnpinnedTabAreaLeft
            // 
            this._ModellerPageUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ModellerPageUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ModellerPageUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._ModellerPageUnpinnedTabAreaLeft.Name = "_ModellerPageUnpinnedTabAreaLeft";
            this._ModellerPageUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._ModellerPageUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 516);
            this._ModellerPageUnpinnedTabAreaLeft.TabIndex = 137;
            // 
            // _ModellerPageUnpinnedTabAreaRight
            // 
            this._ModellerPageUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ModellerPageUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ModellerPageUnpinnedTabAreaRight.Location = new System.Drawing.Point(1176, 0);
            this._ModellerPageUnpinnedTabAreaRight.Name = "_ModellerPageUnpinnedTabAreaRight";
            this._ModellerPageUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._ModellerPageUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 516);
            this._ModellerPageUnpinnedTabAreaRight.TabIndex = 138;
            // 
            // _ModellerPageUnpinnedTabAreaTop
            // 
            this._ModellerPageUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ModellerPageUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ModellerPageUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._ModellerPageUnpinnedTabAreaTop.Name = "_ModellerPageUnpinnedTabAreaTop";
            this._ModellerPageUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._ModellerPageUnpinnedTabAreaTop.Size = new System.Drawing.Size(1176, 0);
            this._ModellerPageUnpinnedTabAreaTop.TabIndex = 139;
            // 
            // _ModellerPageUnpinnedTabAreaBottom
            // 
            this._ModellerPageUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ModellerPageUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ModellerPageUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 516);
            this._ModellerPageUnpinnedTabAreaBottom.Name = "_ModellerPageUnpinnedTabAreaBottom";
            this._ModellerPageUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._ModellerPageUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1176, 0);
            this._ModellerPageUnpinnedTabAreaBottom.TabIndex = 140;
            // 
            // _ModellerPageAutoHideControl
            // 
            this._ModellerPageAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ModellerPageAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._ModellerPageAutoHideControl.Name = "_ModellerPageAutoHideControl";
            this._ModellerPageAutoHideControl.Owner = this.ultraDockManager;
            this._ModellerPageAutoHideControl.TabIndex = 141;
            // 
            // cameraImagePanel
            // 
            this.cameraImagePanel.AutoSize = true;
            this.cameraImagePanel.Controls.Add(this.menuStrip1);
            this.cameraImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraImagePanel.Location = new System.Drawing.Point(0, 0);
            this.cameraImagePanel.Name = "cameraImagePanel";
            this.cameraImagePanel.Size = new System.Drawing.Size(1176, 516);
            this.cameraImagePanel.TabIndex = 113;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dockingToolStripMenuItem,
            this.layoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1176, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dockingToolStripMenuItem
            // 
            this.dockingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemModellerToolbar,
            this.menuItemParamPanel,
            this.menuItemResultPanel,
            this.menuItemModelTreePanel,
            this.menuItemFovNaviPanel,
            this.menuItemFiducialPanel,
            this.toolStripSeparator1,
            this.teachDefaultLayoutToolStripMenuItem,
            this.toolStripMenuItem2});
            this.dockingToolStripMenuItem.Name = "dockingToolStripMenuItem";
            this.dockingToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.dockingToolStripMenuItem.Text = "Docking";
            this.dockingToolStripMenuItem.DropDownOpened += new System.EventHandler(this.DockingToolStripMenuItem_DropDownOpened);
            // 
            // menuItemModellerToolbar
            // 
            this.menuItemModellerToolbar.Name = "menuItemModellerToolbar";
            this.menuItemModellerToolbar.Size = new System.Drawing.Size(189, 22);
            this.menuItemModellerToolbar.Text = "Modeller Toolbar";
            this.menuItemModellerToolbar.Click += new System.EventHandler(this.MenuItemModellerToolbar_Click);
            // 
            // menuItemParamPanel
            // 
            this.menuItemParamPanel.Name = "menuItemParamPanel";
            this.menuItemParamPanel.Size = new System.Drawing.Size(189, 22);
            this.menuItemParamPanel.Text = "Parameter Panel";
            this.menuItemParamPanel.Click += new System.EventHandler(this.MenuItemParamPanel_Click);
            // 
            // menuItemResultPanel
            // 
            this.menuItemResultPanel.Name = "menuItemResultPanel";
            this.menuItemResultPanel.Size = new System.Drawing.Size(189, 22);
            this.menuItemResultPanel.Text = "Result Panel";
            this.menuItemResultPanel.Click += new System.EventHandler(this.MenuItemResultPanel_Click);
            // 
            // menuItemModelTreePanel
            // 
            this.menuItemModelTreePanel.Name = "menuItemModelTreePanel";
            this.menuItemModelTreePanel.Size = new System.Drawing.Size(189, 22);
            this.menuItemModelTreePanel.Text = "Model Tree Panel";
            this.menuItemModelTreePanel.Click += new System.EventHandler(this.MenuItemModelTreePanel_Click);
            // 
            // menuItemFovNaviPanel
            // 
            this.menuItemFovNaviPanel.Name = "menuItemFovNaviPanel";
            this.menuItemFovNaviPanel.Size = new System.Drawing.Size(189, 22);
            this.menuItemFovNaviPanel.Text = "Fov Navigator Panel";
            this.menuItemFovNaviPanel.Click += new System.EventHandler(this.MenuItemFovNaviPanel_Click);
            // 
            // menuItemFiducialPanel
            // 
            this.menuItemFiducialPanel.Name = "menuItemFiducialPanel";
            this.menuItemFiducialPanel.Size = new System.Drawing.Size(189, 22);
            this.menuItemFiducialPanel.Text = "Fiducial Panel";
            this.menuItemFiducialPanel.Click += new System.EventHandler(this.MenuItemFiducialPanel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // teachDefaultLayoutToolStripMenuItem
            // 
            this.teachDefaultLayoutToolStripMenuItem.Name = "teachDefaultLayoutToolStripMenuItem";
            this.teachDefaultLayoutToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.teachDefaultLayoutToolStripMenuItem.Text = "Teach Default Layout";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(189, 22);
            this.toolStripMenuItem2.Text = "Result Defalut Layout";
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLoadTeachLayoutFile,
            this.menuItemSaveTeachLayoutFile,
            this.menuItemLoadResultLayoutFile,
            this.menuItemSaveResultLayoutFile});
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.layoutToolStripMenuItem.Text = "Layout";
            // 
            // menuItemLoadTeachLayoutFile
            // 
            this.menuItemLoadTeachLayoutFile.Name = "menuItemLoadTeachLayoutFile";
            this.menuItemLoadTeachLayoutFile.Size = new System.Drawing.Size(257, 22);
            this.menuItemLoadTeachLayoutFile.Text = "Load Teach Layout File";
            this.menuItemLoadTeachLayoutFile.Click += new System.EventHandler(this.MenuItemLoadTeachLayoutFile_Click);
            // 
            // menuItemSaveTeachLayoutFile
            // 
            this.menuItemSaveTeachLayoutFile.Name = "menuItemSaveTeachLayoutFile";
            this.menuItemSaveTeachLayoutFile.Size = new System.Drawing.Size(257, 22);
            this.menuItemSaveTeachLayoutFile.Text = "Save Teach Layout File";
            this.menuItemSaveTeachLayoutFile.Click += new System.EventHandler(this.MenuItemSaveTeachLayoutFile_Click);
            // 
            // menuItemLoadResultLayoutFile
            // 
            this.menuItemLoadResultLayoutFile.Name = "menuItemLoadResultLayoutFile";
            this.menuItemLoadResultLayoutFile.Size = new System.Drawing.Size(257, 22);
            this.menuItemLoadResultLayoutFile.Text = "Load Inspection Result Layout File";
            this.menuItemLoadResultLayoutFile.Click += new System.EventHandler(this.MenuItemLoadResultLayoutFile_Click);
            // 
            // menuItemSaveResultLayoutFile
            // 
            this.menuItemSaveResultLayoutFile.Name = "menuItemSaveResultLayoutFile";
            this.menuItemSaveResultLayoutFile.Size = new System.Drawing.Size(257, 22);
            this.menuItemSaveResultLayoutFile.Text = "Save Inspectoin Result Layout File";
            this.menuItemSaveResultLayoutFile.Click += new System.EventHandler(this.MenuItemSaveResultLayoutFile_Click);
            // 
            // ModellerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._ModellerPageAutoHideControl);
            this.Controls.Add(this.cameraImagePanel);
            this.Controls.Add(this._ModellerPageUnpinnedTabAreaBottom);
            this.Controls.Add(this._ModellerPageUnpinnedTabAreaTop);
            this.Controls.Add(this._ModellerPageUnpinnedTabAreaRight);
            this.Controls.Add(this._ModellerPageUnpinnedTabAreaLeft);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModellerPage";
            this.Size = new System.Drawing.Size(1176, 516);
            this.Load += new System.EventHandler(this.ModellerPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            this.cameraImagePanel.ResumeLayout(false);
            this.cameraImagePanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }        
        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton grab3dToolStripButton;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ModellerPageAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ModellerPageUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ModellerPageUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ModellerPageUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ModellerPageUnpinnedTabAreaLeft;
        private System.Windows.Forms.Panel cameraImagePanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dockingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemParamPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoadTeachLayoutFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveTeachLayoutFile;
        private System.Windows.Forms.ToolStripMenuItem teachDefaultLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoadResultLayoutFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveResultLayoutFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemFiducialPanel;
        private System.Windows.Forms.ToolStripMenuItem menuItemModellerToolbar;
        private System.Windows.Forms.ToolStripMenuItem menuItemResultPanel;
        private System.Windows.Forms.ToolStripMenuItem menuItemModelTreePanel;
        private System.Windows.Forms.ToolStripMenuItem menuItemFovNaviPanel;
    }
}
