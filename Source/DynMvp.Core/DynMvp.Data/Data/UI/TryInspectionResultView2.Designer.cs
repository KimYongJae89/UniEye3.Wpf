namespace DynMvp.Data.UI
{
    partial class TryInspectionResultView2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TryInspectionResultView2));
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tabPageResultList = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.resultGrid = new System.Windows.Forms.DataGridView();
            this.tabPageResult = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabControlMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.columnTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProbe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageResultList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultGrid)).BeginInit();
            this.tabPageResult.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageResultList
            // 
            this.tabPageResultList.Controls.Add(this.resultGrid);
            this.tabPageResultList.Location = new System.Drawing.Point(29, 1);
            this.tabPageResultList.Name = "tabPageResultList";
            this.tabPageResultList.Size = new System.Drawing.Size(437, 198);
            // 
            // resultGrid
            // 
            this.resultGrid.AllowUserToAddRows = false;
            this.resultGrid.AllowUserToDeleteRows = false;
            this.resultGrid.AllowUserToResizeColumns = false;
            this.resultGrid.AllowUserToResizeRows = false;
            this.resultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnTarget,
            this.columnProbe,
            this.columnResult,
            this.columnMessage});
            this.resultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultGrid.Location = new System.Drawing.Point(0, 0);
            this.resultGrid.MultiSelect = false;
            this.resultGrid.Name = "resultGrid";
            this.resultGrid.ReadOnly = true;
            this.resultGrid.RowHeadersVisible = false;
            this.resultGrid.RowTemplate.Height = 23;
            this.resultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultGrid.Size = new System.Drawing.Size(437, 198);
            this.resultGrid.TabIndex = 1;
            this.resultGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultGrid_CellClick);
            this.resultGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultGrid_CellDoubleClick);
            // 
            // tabPageResult
            // 
            this.tabPageResult.Controls.Add(this.webBrowser);
            this.tabPageResult.Controls.Add(this.toolStrip1);
            this.tabPageResult.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageResult.Name = "tabPageResult";
            this.tabPageResult.Size = new System.Drawing.Size(437, 198);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBack,
            this.ToolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(437, 46);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBack.Image")));
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Size = new System.Drawing.Size(36, 43);
            this.toolStripButtonBack.Text = "Back";
            this.toolStripButtonBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonBack.ToolTipText = "뒤로";
            this.toolStripButtonBack.Click += new System.EventHandler(this.toolStripButtonBack_Click);
            // 
            // ToolStripButtonSave
            // 
            this.ToolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonSave.Image")));
            this.ToolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonSave.Name = "ToolStripButtonSave";
            this.ToolStripButtonSave.Size = new System.Drawing.Size(36, 43);
            this.ToolStripButtonSave.Text = "Save";
            this.ToolStripButtonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ToolStripButtonSave.Click += new System.EventHandler(this.ToolStripButtonSave_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 46);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(437, 152);
            this.webBrowser.TabIndex = 1;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabControlMain.Controls.Add(this.tabPageResultList);
            this.tabControlMain.Controls.Add(this.tabPageResult);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabControlMain.Size = new System.Drawing.Size(469, 202);
            this.tabControlMain.TabIndex = 2;
            this.tabControlMain.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            ultraTab1.TabPage = this.tabPageResultList;
            ultraTab1.Text = "List";
            ultraTab2.TabPage = this.tabPageResult;
            ultraTab2.Text = "Result";
            this.tabControlMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(437, 198);
            // 
            // columnTarget
            // 
            this.columnTarget.HeaderText = "T";
            this.columnTarget.Name = "columnTarget";
            this.columnTarget.ReadOnly = true;
            this.columnTarget.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnProbe
            // 
            this.columnProbe.HeaderText = "P";
            this.columnProbe.Name = "columnProbe";
            this.columnProbe.ReadOnly = true;
            this.columnProbe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnProbe.Width = 60;
            // 
            // columnResult
            // 
            this.columnResult.HeaderText = "Result";
            this.columnResult.Name = "columnResult";
            this.columnResult.ReadOnly = true;
            this.columnResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnResult.Width = 80;
            // 
            // columnMessage
            // 
            this.columnMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnMessage.HeaderText = "Message";
            this.columnMessage.Name = "columnMessage";
            this.columnMessage.ReadOnly = true;
            this.columnMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TryInspectionResultView2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TryInspectionResultView2";
            this.Size = new System.Drawing.Size(469, 202);
            this.tabPageResultList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultGrid)).EndInit();
            this.tabPageResult.ResumeLayout(false);
            this.tabPageResult.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabControlMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageResultList;
        private System.Windows.Forms.DataGridView resultGrid;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageResult;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;
        private System.Windows.Forms.ToolStripButton ToolStripButtonSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProbe;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMessage;
    }
}