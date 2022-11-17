namespace UniEye.Base.UI.Main2018
{
    partial class ModellerToolbar
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModellerToolbar));
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addProbeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.groupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ungroupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.syncParamToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.syncAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.lockMoveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleFiducialToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RedoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.previewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewTypeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.selectLightButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.singleShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.multiShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTop
            // 
            this.toolStripTop.AutoSize = false;
            this.toolStripTop.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.addProbeToolStripButton,
            this.copyProbeToolStripButton,
            this.pasteProbeToolStripButton,
            this.deleteProbeToolStripButton,
            this.toolStripSeparator2,
            this.groupProbeToolStripButton,
            this.ungroupProbeToolStripButton,
            this.toolStripSeparator3,
            this.syncParamToolStripButton,
            this.syncAllToolStripButton,
            this.lockMoveToolStripButton,
            this.toolStripSeparator4,
            this.toggleFiducialToolStripButton,
            this.toolStripSeparator5,
            this.undoToolStripButton,
            this.RedoToolStripButton,
            this.toolStripSeparator6,
            this.previewToolStripButton,
            this.previewTypeToolStripButton,
            this.toolStripSeparator7,
            this.selectLightButton,
            this.toolStripSeparator8,
            this.singleShotToolStripButton,
            this.multiShotToolStripButton});
            this.toolStripTop.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(864, 42);
            this.toolStripTop.TabIndex = 4;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::UniEye.Base.Properties.Resources.test_32;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 39);
            this.toolStripButton1.Text = "Inspect";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 42);
            // 
            // addProbeToolStripButton
            // 
            this.addProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addProbeToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.addProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addProbeToolStripButton.Name = "addProbeToolStripButton";
            this.addProbeToolStripButton.Size = new System.Drawing.Size(45, 39);
            this.addProbeToolStripButton.Text = "Add";
            this.addProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addProbeToolStripButton.Click += new System.EventHandler(this.addProbeToolStripButton_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.AutoSize = false;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.testToolStripMenuItem.Text = "Testfgdfgdfgdfgdfgdfg";
            // 
            // copyProbeToolStripButton
            // 
            this.copyProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.copy_32;
            this.copyProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.copyProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyProbeToolStripButton.Name = "copyProbeToolStripButton";
            this.copyProbeToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.copyProbeToolStripButton.Text = "Copy";
            this.copyProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // pasteProbeToolStripButton
            // 
            this.pasteProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.paste_32;
            this.pasteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pasteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteProbeToolStripButton.Name = "pasteProbeToolStripButton";
            this.pasteProbeToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.pasteProbeToolStripButton.Text = "Paste";
            this.pasteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // deleteProbeToolStripButton
            // 
            this.deleteProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteProbeToolStripButton.Name = "deleteProbeToolStripButton";
            this.deleteProbeToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.deleteProbeToolStripButton.Text = "Delete";
            this.deleteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 42);
            // 
            // groupProbeToolStripButton
            // 
            this.groupProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.groupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.group_32;
            this.groupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.groupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupProbeToolStripButton.Name = "groupProbeToolStripButton";
            this.groupProbeToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.groupProbeToolStripButton.Text = "Group";
            this.groupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // ungroupProbeToolStripButton
            // 
            this.ungroupProbeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ungroupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.ungroup_32;
            this.ungroupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ungroupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ungroupProbeToolStripButton.Name = "ungroupProbeToolStripButton";
            this.ungroupProbeToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.ungroupProbeToolStripButton.Text = "Ungroup";
            this.ungroupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 42);
            // 
            // syncParamToolStripButton
            // 
            this.syncParamToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.syncParamToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncParamToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncParamToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncParamToolStripButton.Name = "syncParamToolStripButton";
            this.syncParamToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.syncParamToolStripButton.Text = "Sync";
            this.syncParamToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // syncAllToolStripButton
            // 
            this.syncAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.syncAllToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncAllToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncAllToolStripButton.Name = "syncAllToolStripButton";
            this.syncAllToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.syncAllToolStripButton.Text = "Sync All";
            this.syncAllToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // lockMoveToolStripButton
            // 
            this.lockMoveToolStripButton.CheckOnClick = true;
            this.lockMoveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lockMoveToolStripButton.Image = global::UniEye.Base.Properties.Resources.no_entry_32;
            this.lockMoveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lockMoveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lockMoveToolStripButton.Name = "lockMoveToolStripButton";
            this.lockMoveToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.lockMoveToolStripButton.Text = "Move";
            this.lockMoveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.lockMoveToolStripButton.ToolTipText = "Don\'t Move";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 42);
            // 
            // toggleFiducialToolStripButton
            // 
            this.toggleFiducialToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toggleFiducialToolStripButton.Image = global::UniEye.Base.Properties.Resources.toggle_32;
            this.toggleFiducialToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toggleFiducialToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleFiducialToolStripButton.Name = "toggleFiducialToolStripButton";
            this.toggleFiducialToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.toggleFiducialToolStripButton.Text = "FIducial";
            this.toggleFiducialToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 42);
            // 
            // undoToolStripButton
            // 
            this.undoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoToolStripButton.Image = global::UniEye.Base.Properties.Resources.undo_32;
            this.undoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.undoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolStripButton.Name = "undoToolStripButton";
            this.undoToolStripButton.Size = new System.Drawing.Size(30, 39);
            this.undoToolStripButton.Text = "Undo";
            this.undoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.undoToolStripButton.ToolTipText = "Undo";
            this.undoToolStripButton.Click += new System.EventHandler(this.undoToolStripButton_Click);
            // 
            // RedoToolStripButton
            // 
            this.RedoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RedoToolStripButton.Image = global::UniEye.Base.Properties.Resources.redo_32;
            this.RedoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RedoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoToolStripButton.Name = "RedoToolStripButton";
            this.RedoToolStripButton.Size = new System.Drawing.Size(30, 39);
            this.RedoToolStripButton.Text = "Redo";
            this.RedoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.RedoToolStripButton.Click += new System.EventHandler(this.RedoToolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 42);
            // 
            // previewToolStripButton
            // 
            this.previewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previewToolStripButton.Image = global::UniEye.Base.Properties.Resources.preview_32;
            this.previewToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.previewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewToolStripButton.Name = "previewToolStripButton";
            this.previewToolStripButton.Size = new System.Drawing.Size(40, 39);
            this.previewToolStripButton.Text = "Preview";
            this.previewToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // previewTypeToolStripButton
            // 
            this.previewTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.previewTypeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("previewTypeToolStripButton.Image")));
            this.previewTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewTypeToolStripButton.Name = "previewTypeToolStripButton";
            this.previewTypeToolStripButton.Size = new System.Drawing.Size(80, 39);
            this.previewTypeToolStripButton.Text = "Preview";
            this.previewTypeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 42);
            // 
            // selectLightButton
            // 
            this.selectLightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectLightButton.Image = ((System.Drawing.Image)(resources.GetObject("selectLightButton.Image")));
            this.selectLightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectLightButton.Name = "selectLightButton";
            this.selectLightButton.Size = new System.Drawing.Size(102, 39);
            this.selectLightButton.Text = "Light Type";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 42);
            // 
            // singleShotToolStripButton
            // 
            this.singleShotToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.singleShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.single_shot_32;
            this.singleShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.singleShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.singleShotToolStripButton.Name = "singleShotToolStripButton";
            this.singleShotToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.singleShotToolStripButton.Text = "Grab";
            this.singleShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.singleShotToolStripButton.ToolTipText = "Select Image Folder";
            this.singleShotToolStripButton.Click += new System.EventHandler(this.singleShotToolStripButton_Click);
            // 
            // multiShotToolStripButton
            // 
            this.multiShotToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.multiShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.multi_shot_32;
            this.multiShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.multiShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.multiShotToolStripButton.Name = "multiShotToolStripButton";
            this.multiShotToolStripButton.Size = new System.Drawing.Size(36, 39);
            this.multiShotToolStripButton.Text = "Live";
            this.multiShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.multiShotToolStripButton.ToolTipText = "Select Image Folder";
            this.multiShotToolStripButton.Click += new System.EventHandler(this.multiShotToolStripButton_Click);
            // 
            // ModellerToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripTop);
            this.Name = "ModellerToolbar";
            this.Size = new System.Drawing.Size(864, 42);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton addProbeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton copyProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteProbeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton groupProbeToolStripButton;
        public System.Windows.Forms.ToolStripButton ungroupProbeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton syncParamToolStripButton;
        private System.Windows.Forms.ToolStripButton syncAllToolStripButton;
        private System.Windows.Forms.ToolStripButton lockMoveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripButton toggleFiducialToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripButton undoToolStripButton;
        public System.Windows.Forms.ToolStripButton RedoToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton previewToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton previewTypeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripDropDownButton selectLightButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton singleShotToolStripButton;
        public System.Windows.Forms.ToolStripButton multiShotToolStripButton;
    }
}
