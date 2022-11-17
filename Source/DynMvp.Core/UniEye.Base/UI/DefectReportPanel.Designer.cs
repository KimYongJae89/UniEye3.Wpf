namespace UniEye.Base.UI
{
    partial class DefectReportPanel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSchemaView = new System.Windows.Forms.Panel();
            this.panelImage = new System.Windows.Forms.TableLayoutPanel();
            this.labelCamera = new System.Windows.Forms.Label();
            this.panelTargetGroupImage = new System.Windows.Forms.Panel();
            this.labelNg = new System.Windows.Forms.Label();
            this.panelProbeNgImage = new System.Windows.Forms.Panel();
            this.labelGood = new System.Windows.Forms.Label();
            this.panelProbeGoodImage = new System.Windows.Forms.Panel();
            this.panelResultList = new System.Windows.Forms.Panel();
            this.probeResult = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.showGoodPad = new System.Windows.Forms.CheckBox();
            this.showNgPad = new System.Windows.Forms.CheckBox();
            this.panelProbeInfo = new System.Windows.Forms.TableLayoutPanel();
            this.probeNo = new System.Windows.Forms.Label();
            this.labelProbeNo = new System.Windows.Forms.Label();
            this.targetName = new System.Windows.Forms.Label();
            this.targetGroupNo = new System.Windows.Forms.Label();
            this.stepName = new System.Windows.Forms.Label();
            this.labelStep = new System.Windows.Forms.Label();
            this.labelTargetGroup = new System.Windows.Forms.Label();
            this.labelTarget = new System.Windows.Forms.Label();
            this.probeResultList = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelImage.SuspendLayout();
            this.panelResultList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelProbeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probeResultList)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.splitContainer);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mainPanel.Location = new System.Drawing.Point(234, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(681, 663);
            this.mainPanel.TabIndex = 1;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelSchemaView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelImage);
            this.splitContainer.Size = new System.Drawing.Size(679, 661);
            this.splitContainer.SplitterDistance = 324;
            this.splitContainer.TabIndex = 18;
            // 
            // panelSchemaView
            // 
            this.panelSchemaView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSchemaView.Location = new System.Drawing.Point(0, 0);
            this.panelSchemaView.Name = "panelSchemaView";
            this.panelSchemaView.Size = new System.Drawing.Size(679, 324);
            this.panelSchemaView.TabIndex = 12;
            // 
            // panelImage
            // 
            this.panelImage.ColumnCount = 3;
            this.panelImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelImage.Controls.Add(this.labelCamera, 0, 0);
            this.panelImage.Controls.Add(this.panelTargetGroupImage, 0, 1);
            this.panelImage.Controls.Add(this.labelNg, 2, 0);
            this.panelImage.Controls.Add(this.panelProbeNgImage, 2, 1);
            this.panelImage.Controls.Add(this.labelGood, 1, 0);
            this.panelImage.Controls.Add(this.panelProbeGoodImage, 1, 1);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 0);
            this.panelImage.Name = "panelImage";
            this.panelImage.RowCount = 2;
            this.panelImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.panelImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelImage.Size = new System.Drawing.Size(679, 333);
            this.panelImage.TabIndex = 17;
            // 
            // labelCamera
            // 
            this.labelCamera.BackColor = System.Drawing.Color.CadetBlue;
            this.labelCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCamera.Location = new System.Drawing.Point(3, 0);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(220, 32);
            this.labelCamera.TabIndex = 19;
            this.labelCamera.Text = "Camera";
            this.labelCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTargetGroupImage
            // 
            this.panelTargetGroupImage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelTargetGroupImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTargetGroupImage.Location = new System.Drawing.Point(3, 35);
            this.panelTargetGroupImage.Name = "panelTargetGroupImage";
            this.panelTargetGroupImage.Size = new System.Drawing.Size(220, 295);
            this.panelTargetGroupImage.TabIndex = 18;
            // 
            // labelNg
            // 
            this.labelNg.BackColor = System.Drawing.Color.Red;
            this.labelNg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNg.Location = new System.Drawing.Point(455, 0);
            this.labelNg.Name = "labelNg";
            this.labelNg.Size = new System.Drawing.Size(221, 32);
            this.labelNg.TabIndex = 17;
            this.labelNg.Text = "NG";
            this.labelNg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProbeNgImage
            // 
            this.panelProbeNgImage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelProbeNgImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProbeNgImage.Location = new System.Drawing.Point(455, 35);
            this.panelProbeNgImage.Name = "panelProbeNgImage";
            this.panelProbeNgImage.Size = new System.Drawing.Size(221, 295);
            this.panelProbeNgImage.TabIndex = 16;
            // 
            // labelGood
            // 
            this.labelGood.BackColor = System.Drawing.Color.LightGreen;
            this.labelGood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGood.Location = new System.Drawing.Point(229, 0);
            this.labelGood.Name = "labelGood";
            this.labelGood.Size = new System.Drawing.Size(220, 32);
            this.labelGood.TabIndex = 16;
            this.labelGood.Text = "Good";
            this.labelGood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProbeGoodImage
            // 
            this.panelProbeGoodImage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelProbeGoodImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProbeGoodImage.Location = new System.Drawing.Point(229, 35);
            this.panelProbeGoodImage.Name = "panelProbeGoodImage";
            this.panelProbeGoodImage.Size = new System.Drawing.Size(220, 295);
            this.panelProbeGoodImage.TabIndex = 14;
            // 
            // panelResultList
            // 
            this.panelResultList.BackColor = System.Drawing.Color.Transparent;
            this.panelResultList.Controls.Add(this.probeResult);
            this.panelResultList.Controls.Add(this.panel2);
            this.panelResultList.Controls.Add(this.panelProbeInfo);
            this.panelResultList.Controls.Add(this.probeResultList);
            this.panelResultList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelResultList.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelResultList.Location = new System.Drawing.Point(0, 0);
            this.panelResultList.Name = "panelResultList";
            this.panelResultList.Size = new System.Drawing.Size(234, 663);
            this.panelResultList.TabIndex = 15;
            // 
            // probeResult
            // 
            this.probeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.probeResult.Location = new System.Drawing.Point(0, 405);
            this.probeResult.MinimumSize = new System.Drawing.Size(20, 20);
            this.probeResult.Name = "probeResult";
            this.probeResult.Size = new System.Drawing.Size(234, 221);
            this.probeResult.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.showGoodPad);
            this.panel2.Controls.Add(this.showNgPad);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 626);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 37);
            this.panel2.TabIndex = 4;
            // 
            // showGoodPad
            // 
            this.showGoodPad.AutoSize = true;
            this.showGoodPad.Location = new System.Drawing.Point(70, 9);
            this.showGoodPad.Name = "showGoodPad";
            this.showGoodPad.Size = new System.Drawing.Size(60, 21);
            this.showGoodPad.TabIndex = 0;
            this.showGoodPad.Text = "Good";
            this.showGoodPad.UseVisualStyleBackColor = true;
            // 
            // showNgPad
            // 
            this.showNgPad.AutoSize = true;
            this.showNgPad.Location = new System.Drawing.Point(10, 9);
            this.showNgPad.Name = "showNgPad";
            this.showNgPad.Size = new System.Drawing.Size(46, 21);
            this.showNgPad.TabIndex = 0;
            this.showNgPad.Text = "NG";
            this.showNgPad.UseVisualStyleBackColor = true;
            // 
            // panelProbeInfo
            // 
            this.panelProbeInfo.ColumnCount = 2;
            this.panelProbeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.54701F));
            this.panelProbeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.45299F));
            this.panelProbeInfo.Controls.Add(this.probeNo, 1, 3);
            this.panelProbeInfo.Controls.Add(this.labelProbeNo, 0, 3);
            this.panelProbeInfo.Controls.Add(this.targetName, 1, 2);
            this.panelProbeInfo.Controls.Add(this.targetGroupNo, 1, 1);
            this.panelProbeInfo.Controls.Add(this.stepName, 1, 0);
            this.panelProbeInfo.Controls.Add(this.labelStep, 0, 0);
            this.panelProbeInfo.Controls.Add(this.labelTargetGroup, 0, 1);
            this.panelProbeInfo.Controls.Add(this.labelTarget, 0, 2);
            this.panelProbeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProbeInfo.Location = new System.Drawing.Point(0, 276);
            this.panelProbeInfo.Name = "panelProbeInfo";
            this.panelProbeInfo.RowCount = 4;
            this.panelProbeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelProbeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelProbeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelProbeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelProbeInfo.Size = new System.Drawing.Size(234, 129);
            this.panelProbeInfo.TabIndex = 2;
            // 
            // probeNo
            // 
            this.probeNo.BackColor = System.Drawing.Color.MintCream;
            this.probeNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.probeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.probeNo.Location = new System.Drawing.Point(139, 97);
            this.probeNo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.probeNo.Name = "probeNo";
            this.probeNo.Size = new System.Drawing.Size(92, 31);
            this.probeNo.TabIndex = 9;
            this.probeNo.Text = "0";
            this.probeNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProbeNo
            // 
            this.labelProbeNo.BackColor = System.Drawing.Color.PaleTurquoise;
            this.labelProbeNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProbeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProbeNo.Location = new System.Drawing.Point(3, 97);
            this.labelProbeNo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelProbeNo.Name = "labelProbeNo";
            this.labelProbeNo.Size = new System.Drawing.Size(130, 31);
            this.labelProbeNo.TabIndex = 8;
            this.labelProbeNo.Text = "Probe No";
            this.labelProbeNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // targetName
            // 
            this.targetName.BackColor = System.Drawing.Color.MintCream;
            this.targetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetName.Location = new System.Drawing.Point(139, 65);
            this.targetName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.targetName.Name = "targetName";
            this.targetName.Size = new System.Drawing.Size(92, 30);
            this.targetName.TabIndex = 7;
            this.targetName.Text = "0";
            this.targetName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // targetGroupNo
            // 
            this.targetGroupNo.BackColor = System.Drawing.Color.MintCream;
            this.targetGroupNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetGroupNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetGroupNo.Location = new System.Drawing.Point(139, 33);
            this.targetGroupNo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.targetGroupNo.Name = "targetGroupNo";
            this.targetGroupNo.Size = new System.Drawing.Size(92, 30);
            this.targetGroupNo.TabIndex = 6;
            this.targetGroupNo.Text = "0";
            this.targetGroupNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stepName
            // 
            this.stepName.BackColor = System.Drawing.Color.MintCream;
            this.stepName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepName.Location = new System.Drawing.Point(139, 1);
            this.stepName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.stepName.Name = "stepName";
            this.stepName.Size = new System.Drawing.Size(92, 30);
            this.stepName.TabIndex = 5;
            this.stepName.Text = "PCB 46";
            this.stepName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStep
            // 
            this.labelStep.BackColor = System.Drawing.Color.PaleTurquoise;
            this.labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStep.Location = new System.Drawing.Point(3, 1);
            this.labelStep.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(130, 30);
            this.labelStep.TabIndex = 0;
            this.labelStep.Text = "Step";
            this.labelStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTargetGroup
            // 
            this.labelTargetGroup.BackColor = System.Drawing.Color.PaleTurquoise;
            this.labelTargetGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTargetGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTargetGroup.Location = new System.Drawing.Point(3, 33);
            this.labelTargetGroup.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelTargetGroup.Name = "labelTargetGroup";
            this.labelTargetGroup.Size = new System.Drawing.Size(130, 30);
            this.labelTargetGroup.TabIndex = 1;
            this.labelTargetGroup.Text = "Target Group";
            this.labelTargetGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTarget
            // 
            this.labelTarget.BackColor = System.Drawing.Color.PaleTurquoise;
            this.labelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarget.Location = new System.Drawing.Point(3, 65);
            this.labelTarget.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(130, 30);
            this.labelTarget.TabIndex = 2;
            this.labelTarget.Text = "Target";
            this.labelTarget.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // probeResultList
            // 
            this.probeResultList.AllowUserToAddRows = false;
            this.probeResultList.AllowUserToDeleteRows = false;
            this.probeResultList.AllowUserToResizeColumns = false;
            this.probeResultList.AllowUserToResizeRows = false;
            this.probeResultList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.probeResultList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnPartName});
            this.probeResultList.Dock = System.Windows.Forms.DockStyle.Top;
            this.probeResultList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.probeResultList.Location = new System.Drawing.Point(0, 0);
            this.probeResultList.Margin = new System.Windows.Forms.Padding(2);
            this.probeResultList.MultiSelect = false;
            this.probeResultList.Name = "probeResultList";
            this.probeResultList.RowHeadersVisible = false;
            this.probeResultList.RowTemplate.Height = 23;
            this.probeResultList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.probeResultList.Size = new System.Drawing.Size(234, 276);
            this.probeResultList.TabIndex = 1;
            this.probeResultList.SelectionChanged += new System.EventHandler(this.probeResultList_SelectionChanged);
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No";
            this.columnNo.Name = "columnNo";
            this.columnNo.Width = 50;
            // 
            // columnPartName
            // 
            this.columnPartName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnPartName.HeaderText = "Probe ID";
            this.columnPartName.Name = "columnPartName";
            // 
            // DefectReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 663);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.panelResultList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "DefectReportPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.DefectReportPanel_Load);
            this.mainPanel.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelImage.ResumeLayout(false);
            this.panelResultList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelProbeInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.probeResultList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelSchemaView;
        private System.Windows.Forms.Panel panelResultList;
        private System.Windows.Forms.Panel panelProbeGoodImage;
        private System.Windows.Forms.Panel panelProbeNgImage;
        private System.Windows.Forms.Panel panelTargetGroupImage;
        private System.Windows.Forms.Label labelGood;
        private System.Windows.Forms.Label labelNg;
        private System.Windows.Forms.TableLayoutPanel panelProbeInfo;
        private System.Windows.Forms.Label probeNo;
        private System.Windows.Forms.Label labelProbeNo;
        private System.Windows.Forms.Label targetName;
        private System.Windows.Forms.Label targetGroupNo;
        private System.Windows.Forms.Label stepName;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Label labelTargetGroup;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.DataGridView probeResultList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPartName;
        private System.Windows.Forms.TableLayoutPanel panelImage;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.WebBrowser probeResult;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox showGoodPad;
        private System.Windows.Forms.CheckBox showNgPad;
    }
}