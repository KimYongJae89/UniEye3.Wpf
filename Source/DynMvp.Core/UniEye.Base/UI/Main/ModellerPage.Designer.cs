namespace UniEye.Base.UI.Main
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
            this.saveButton = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.cameraImagePanel = new System.Windows.Forms.Panel();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.paramContainer = new System.Windows.Forms.SplitContainer();
            this.panelAlign = new System.Windows.Forms.Panel();
            this.fidAngle = new System.Windows.Forms.TextBox();
            this.fidOffset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFidDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDesiredDistance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFidAngle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFidOffset = new System.Windows.Forms.Label();
            this.labelDesiredDistance = new System.Windows.Forms.Label();
            this.labelFidDistance = new System.Windows.Forms.Label();
            this.fidDistanceTol = new System.Windows.Forms.NumericUpDown();
            this.labelDistanceOffset = new System.Windows.Forms.Label();
            this.buttonCloseAlignPanel = new System.Windows.Forms.Button();
            this.labelUm = new System.Windows.Forms.Label();
            this.buttonAlign = new System.Windows.Forms.Button();
            this.tabControlUtil = new System.Windows.Forms.TabControl();
            this.tabPageResult = new System.Windows.Forms.TabPage();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.panelTabMenu = new System.Windows.Forms.Panel();
            this.panelFixedMenu = new System.Windows.Forms.Panel();
            this.inspectionButton = new System.Windows.Forms.Button();
            this.panelSubMenu = new System.Windows.Forms.TableLayoutPanel();
            this.grab3dToolStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramContainer)).BeginInit();
            this.paramContainer.Panel1.SuspendLayout();
            this.paramContainer.Panel2.SuspendLayout();
            this.paramContainer.SuspendLayout();
            this.panelAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fidDistanceTol)).BeginInit();
            this.tabControlUtil.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.panelSubMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Image = global::UniEye.Base.Properties.Resources.save_32;
            this.saveButton.Location = new System.Drawing.Point(0, 0);
            this.saveButton.Margin = new System.Windows.Forms.Padding(0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(49, 49);
            this.saveButton.TabIndex = 0;
            this.toolTip.SetToolTip(this.saveButton, "Save");
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Visible = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPreview.Image = global::UniEye.Base.Properties.Resources.preview_32;
            this.buttonPreview.Location = new System.Drawing.Point(0, 49);
            this.buttonPreview.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(49, 49);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonPreview, "Preview");
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Visible = false;
            this.buttonPreview.Click += new System.EventHandler(this.ButtonPreview_Click);
            // 
            // cameraImagePanel
            // 
            this.cameraImagePanel.AutoSize = true;
            this.cameraImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraImagePanel.Location = new System.Drawing.Point(0, 0);
            this.cameraImagePanel.Name = "cameraImagePanel";
            this.cameraImagePanel.Size = new System.Drawing.Size(662, 562);
            this.cameraImagePanel.TabIndex = 113;
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainContainer.Location = new System.Drawing.Point(0, 98);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.cameraImagePanel);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.paramContainer);
            this.mainContainer.Panel2MinSize = 300;
            this.mainContainer.Size = new System.Drawing.Size(1176, 562);
            this.mainContainer.SplitterDistance = 662;
            this.mainContainer.TabIndex = 133;
            // 
            // paramContainer
            // 
            this.paramContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramContainer.Location = new System.Drawing.Point(0, 0);
            this.paramContainer.Name = "paramContainer";
            this.paramContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // paramContainer.Panel1
            // 
            this.paramContainer.Panel1.Controls.Add(this.panelAlign);
            // 
            // paramContainer.Panel2
            // 
            this.paramContainer.Panel2.Controls.Add(this.tabControlUtil);
            this.paramContainer.Size = new System.Drawing.Size(510, 562);
            this.paramContainer.SplitterDistance = 316;
            this.paramContainer.TabIndex = 121;
            // 
            // panelAlign
            // 
            this.panelAlign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAlign.Controls.Add(this.fidAngle);
            this.panelAlign.Controls.Add(this.fidOffset);
            this.panelAlign.Controls.Add(this.label3);
            this.panelAlign.Controls.Add(this.textBoxFidDistance);
            this.panelAlign.Controls.Add(this.label2);
            this.panelAlign.Controls.Add(this.textBoxDesiredDistance);
            this.panelAlign.Controls.Add(this.label5);
            this.panelAlign.Controls.Add(this.labelFidAngle);
            this.panelAlign.Controls.Add(this.label4);
            this.panelAlign.Controls.Add(this.labelFidOffset);
            this.panelAlign.Controls.Add(this.labelDesiredDistance);
            this.panelAlign.Controls.Add(this.labelFidDistance);
            this.panelAlign.Controls.Add(this.fidDistanceTol);
            this.panelAlign.Controls.Add(this.labelDistanceOffset);
            this.panelAlign.Controls.Add(this.buttonCloseAlignPanel);
            this.panelAlign.Controls.Add(this.labelUm);
            this.panelAlign.Controls.Add(this.buttonAlign);
            this.panelAlign.Location = new System.Drawing.Point(4, 6);
            this.panelAlign.Name = "panelAlign";
            this.panelAlign.Size = new System.Drawing.Size(290, 224);
            this.panelAlign.TabIndex = 0;
            this.panelAlign.Visible = false;
            // 
            // fidAngle
            // 
            this.fidAngle.Location = new System.Drawing.Point(154, 138);
            this.fidAngle.Name = "fidAngle";
            this.fidAngle.ReadOnly = true;
            this.fidAngle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidAngle.Size = new System.Drawing.Size(100, 26);
            this.fidAngle.TabIndex = 167;
            // 
            // fidOffset
            // 
            this.fidOffset.Location = new System.Drawing.Point(154, 106);
            this.fidOffset.Name = "fidOffset";
            this.fidOffset.ReadOnly = true;
            this.fidOffset.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidOffset.Size = new System.Drawing.Size(100, 26);
            this.fidOffset.TabIndex = 167;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 20);
            this.label3.TabIndex = 165;
            this.label3.Text = "˚";
            // 
            // textBoxFidDistance
            // 
            this.textBoxFidDistance.Location = new System.Drawing.Point(154, 74);
            this.textBoxFidDistance.Name = "textBoxFidDistance";
            this.textBoxFidDistance.ReadOnly = true;
            this.textBoxFidDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxFidDistance.Size = new System.Drawing.Size(100, 26);
            this.textBoxFidDistance.TabIndex = 167;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
            this.label2.TabIndex = 165;
            this.label2.Text = "um";
            // 
            // textBoxDesiredDistance
            // 
            this.textBoxDesiredDistance.Location = new System.Drawing.Point(154, 42);
            this.textBoxDesiredDistance.Name = "textBoxDesiredDistance";
            this.textBoxDesiredDistance.ReadOnly = true;
            this.textBoxDesiredDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxDesiredDistance.Size = new System.Drawing.Size(100, 26);
            this.textBoxDesiredDistance.TabIndex = 166;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 74);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 20);
            this.label5.TabIndex = 165;
            this.label5.Text = "um";
            // 
            // labelFidAngle
            // 
            this.labelFidAngle.AutoSize = true;
            this.labelFidAngle.Location = new System.Drawing.Point(10, 138);
            this.labelFidAngle.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidAngle.Name = "labelFidAngle";
            this.labelFidAngle.Size = new System.Drawing.Size(80, 20);
            this.labelFidAngle.TabIndex = 162;
            this.labelFidAngle.Text = "Fid. Angle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 45);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 20);
            this.label4.TabIndex = 164;
            this.label4.Text = "um";
            // 
            // labelFidOffset
            // 
            this.labelFidOffset.AutoSize = true;
            this.labelFidOffset.Location = new System.Drawing.Point(10, 106);
            this.labelFidOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidOffset.Name = "labelFidOffset";
            this.labelFidOffset.Size = new System.Drawing.Size(83, 20);
            this.labelFidOffset.TabIndex = 162;
            this.labelFidOffset.Text = "Fid. Offset";
            // 
            // labelDesiredDistance
            // 
            this.labelDesiredDistance.AutoSize = true;
            this.labelDesiredDistance.Location = new System.Drawing.Point(10, 45);
            this.labelDesiredDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDesiredDistance.Name = "labelDesiredDistance";
            this.labelDesiredDistance.Size = new System.Drawing.Size(131, 20);
            this.labelDesiredDistance.TabIndex = 163;
            this.labelDesiredDistance.Text = "Desired Distance";
            // 
            // labelFidDistance
            // 
            this.labelFidDistance.AutoSize = true;
            this.labelFidDistance.Location = new System.Drawing.Point(10, 74);
            this.labelFidDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidDistance.Name = "labelFidDistance";
            this.labelFidDistance.Size = new System.Drawing.Size(102, 20);
            this.labelFidDistance.TabIndex = 162;
            this.labelFidDistance.Text = "Fid. Distance";
            // 
            // fidDistanceTol
            // 
            this.fidDistanceTol.Location = new System.Drawing.Point(154, 9);
            this.fidDistanceTol.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.fidDistanceTol.Name = "fidDistanceTol";
            this.fidDistanceTol.Size = new System.Drawing.Size(100, 26);
            this.fidDistanceTol.TabIndex = 161;
            this.fidDistanceTol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fidDistanceTol.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fidDistanceTol.ValueChanged += new System.EventHandler(this.NumericUpDownDistanceOffset_ValueChanged);
            // 
            // labelDistanceOffset
            // 
            this.labelDistanceOffset.AutoSize = true;
            this.labelDistanceOffset.Location = new System.Drawing.Point(10, 11);
            this.labelDistanceOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDistanceOffset.Name = "labelDistanceOffset";
            this.labelDistanceOffset.Size = new System.Drawing.Size(120, 20);
            this.labelDistanceOffset.TabIndex = 160;
            this.labelDistanceOffset.Text = "Distance Offset";
            // 
            // buttonCloseAlignPanel
            // 
            this.buttonCloseAlignPanel.Location = new System.Drawing.Point(145, 172);
            this.buttonCloseAlignPanel.Name = "buttonCloseAlignPanel";
            this.buttonCloseAlignPanel.Size = new System.Drawing.Size(102, 42);
            this.buttonCloseAlignPanel.TabIndex = 159;
            this.buttonCloseAlignPanel.Text = "Close";
            this.buttonCloseAlignPanel.UseVisualStyleBackColor = true;
            this.buttonCloseAlignPanel.Click += new System.EventHandler(this.ButtonCloseAlignPanel_Click);
            // 
            // labelUm
            // 
            this.labelUm.AutoSize = true;
            this.labelUm.Location = new System.Drawing.Point(254, 11);
            this.labelUm.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelUm.Name = "labelUm";
            this.labelUm.Size = new System.Drawing.Size(31, 20);
            this.labelUm.TabIndex = 159;
            this.labelUm.Text = "um";
            // 
            // buttonAlign
            // 
            this.buttonAlign.Location = new System.Drawing.Point(41, 172);
            this.buttonAlign.Name = "buttonAlign";
            this.buttonAlign.Size = new System.Drawing.Size(102, 42);
            this.buttonAlign.TabIndex = 160;
            this.buttonAlign.Text = "Align";
            this.buttonAlign.UseVisualStyleBackColor = true;
            this.buttonAlign.Click += new System.EventHandler(this.ButtonAlign_Click);
            // 
            // tabControlUtil
            // 
            this.tabControlUtil.Controls.Add(this.tabPageResult);
            this.tabControlUtil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlUtil.Location = new System.Drawing.Point(0, 0);
            this.tabControlUtil.Name = "tabControlUtil";
            this.tabControlUtil.SelectedIndex = 0;
            this.tabControlUtil.Size = new System.Drawing.Size(510, 242);
            this.tabControlUtil.TabIndex = 0;
            // 
            // tabPageResult
            // 
            this.tabPageResult.Location = new System.Drawing.Point(4, 29);
            this.tabPageResult.Name = "tabPageResult";
            this.tabPageResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResult.Size = new System.Drawing.Size(502, 209);
            this.tabPageResult.TabIndex = 0;
            this.tabPageResult.Text = "Result";
            this.tabPageResult.UseVisualStyleBackColor = true;
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.panelTabMenu);
            this.panelMenu.Controls.Add(this.panelFixedMenu);
            this.panelMenu.Controls.Add(this.inspectionButton);
            this.panelMenu.Controls.Add(this.panelSubMenu);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(1176, 98);
            this.panelMenu.TabIndex = 136;
            // 
            // panelTabMenu
            // 
            this.panelTabMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTabMenu.Location = new System.Drawing.Point(175, 0);
            this.panelTabMenu.Name = "panelTabMenu";
            this.panelTabMenu.Size = new System.Drawing.Size(1001, 98);
            this.panelTabMenu.TabIndex = 139;
            // 
            // panelFixedMenu
            // 
            this.panelFixedMenu.AutoSize = true;
            this.panelFixedMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFixedMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelFixedMenu.Location = new System.Drawing.Point(173, 0);
            this.panelFixedMenu.Name = "panelFixedMenu";
            this.panelFixedMenu.Size = new System.Drawing.Size(2, 98);
            this.panelFixedMenu.TabIndex = 138;
            this.panelFixedMenu.Visible = false;
            // 
            // inspectionButton
            // 
            this.inspectionButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.inspectionButton.Image = global::UniEye.Base.Properties.Resources.test_32;
            this.inspectionButton.Location = new System.Drawing.Point(49, 0);
            this.inspectionButton.Name = "inspectionButton";
            this.inspectionButton.Size = new System.Drawing.Size(124, 98);
            this.inspectionButton.TabIndex = 135;
            this.inspectionButton.Text = "Inspection";
            this.inspectionButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.inspectionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.inspectionButton.UseVisualStyleBackColor = true;
            this.inspectionButton.Click += new System.EventHandler(this.InspectionButton_Click);
            // 
            // panelSubMenu
            // 
            this.panelSubMenu.ColumnCount = 1;
            this.panelSubMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSubMenu.Controls.Add(this.saveButton, 0, 0);
            this.panelSubMenu.Controls.Add(this.buttonPreview, 0, 1);
            this.panelSubMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSubMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSubMenu.Name = "panelSubMenu";
            this.panelSubMenu.RowCount = 2;
            this.panelSubMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSubMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSubMenu.Size = new System.Drawing.Size(49, 98);
            this.panelSubMenu.TabIndex = 137;
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
            // ModellerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModellerPage";
            this.Size = new System.Drawing.Size(1176, 660);
            this.Load += new System.EventHandler(this.ModellerPage_Load);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel1.PerformLayout();
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.paramContainer.Panel1.ResumeLayout(false);
            this.paramContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paramContainer)).EndInit();
            this.paramContainer.ResumeLayout(false);
            this.panelAlign.ResumeLayout(false);
            this.panelAlign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fidDistanceTol)).EndInit();
            this.tabControlUtil.ResumeLayout(false);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.panelSubMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }        
        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.Panel cameraImagePanel;
        private System.Windows.Forms.SplitContainer paramContainer;
        private System.Windows.Forms.Button inspectionButton;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.ToolStripButton grab3dToolStripButton;
        private System.Windows.Forms.TabControl tabControlUtil;
        private System.Windows.Forms.TabPage tabPageResult;
        private System.Windows.Forms.TableLayoutPanel panelSubMenu;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Panel panelAlign;
        private System.Windows.Forms.Label labelDesiredDistance;
        private System.Windows.Forms.Label labelFidDistance;
        private System.Windows.Forms.NumericUpDown fidDistanceTol;
        private System.Windows.Forms.Label labelDistanceOffset;
        private System.Windows.Forms.Button buttonCloseAlignPanel;
        private System.Windows.Forms.Label labelUm;
        private System.Windows.Forms.Button buttonAlign;
        private System.Windows.Forms.TextBox textBoxFidDistance;
        private System.Windows.Forms.TextBox textBoxDesiredDistance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fidAngle;
        private System.Windows.Forms.TextBox fidOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFidAngle;
        private System.Windows.Forms.Label labelFidOffset;
        private System.Windows.Forms.Panel panelFixedMenu;
        private System.Windows.Forms.Panel panelTabMenu;
    }
}
