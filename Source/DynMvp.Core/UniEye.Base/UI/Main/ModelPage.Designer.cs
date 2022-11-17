namespace UniEye.Base.UI.Main
{
    partial class ModelPage
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
            this.ultraTouchProvider1 = new Infragistics.Win.Touch.UltraTouchProvider(this.components);
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelFind = new System.Windows.Forms.Label();
            this.panelCategory = new System.Windows.Forms.Panel();
            this.searchModelName = new System.Windows.Forms.TextBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonFind = new System.Windows.Forms.Button();
            this.buttonCloseMode = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.modelTileControlPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTouchProvider1)).BeginInit();
            this.panelCategory.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTouchProvider1
            // 
            this.ultraTouchProvider1.ContainingControl = this;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(93, 7);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(168, 29);
            this.cmbCategory.TabIndex = 10;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Location = new System.Drawing.Point(11, 11);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(77, 21);
            this.labelCategory.TabIndex = 11;
            this.labelCategory.Text = "Category";
            // 
            // labelFind
            // 
            this.labelFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFind.AutoSize = true;
            this.labelFind.Location = new System.Drawing.Point(558, 11);
            this.labelFind.Name = "labelFind";
            this.labelFind.Size = new System.Drawing.Size(41, 21);
            this.labelFind.TabIndex = 11;
            this.labelFind.Text = "Find";
            // 
            // panelCategory
            // 
            this.panelCategory.BackColor = System.Drawing.SystemColors.Window;
            this.panelCategory.Controls.Add(this.searchModelName);
            this.panelCategory.Controls.Add(this.buttonRefresh);
            this.panelCategory.Controls.Add(this.buttonFind);
            this.panelCategory.Controls.Add(this.cmbCategory);
            this.panelCategory.Controls.Add(this.labelFind);
            this.panelCategory.Controls.Add(this.labelCategory);
            this.panelCategory.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCategory.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panelCategory.Location = new System.Drawing.Point(0, 0);
            this.panelCategory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCategory.Name = "panelCategory";
            this.panelCategory.Size = new System.Drawing.Size(785, 42);
            this.panelCategory.TabIndex = 12;
            // 
            // searchModelName
            // 
            this.searchModelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchModelName.Location = new System.Drawing.Point(599, 6);
            this.searchModelName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchModelName.Name = "searchModelName";
            this.searchModelName.Size = new System.Drawing.Size(148, 29);
            this.searchModelName.TabIndex = 1;
            this.searchModelName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchModelName_KeyDown);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(265, 5);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(83, 31);
            this.buttonRefresh.TabIndex = 12;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(749, 5);
            this.buttonFind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(29, 30);
            this.buttonFind.TabIndex = 12;
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // buttonCloseMode
            // 
            this.buttonCloseMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseMode.Location = new System.Drawing.Point(599, 5);
            this.buttonCloseMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCloseMode.Name = "buttonCloseMode";
            this.buttonCloseMode.Size = new System.Drawing.Size(182, 52);
            this.buttonCloseMode.TabIndex = 12;
            this.buttonCloseMode.Text = "Close Model";
            this.buttonCloseMode.UseVisualStyleBackColor = true;
            this.buttonCloseMode.Click += new System.EventHandler(this.buttonCloseMode_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.SystemColors.Window;
            this.panelBottom.Controls.Add(this.buttonCloseMode);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panelBottom.Location = new System.Drawing.Point(0, 406);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(785, 63);
            this.panelBottom.TabIndex = 13;
            // 
            // modelTileControlPanel
            // 
            this.modelTileControlPanel.BackColor = System.Drawing.SystemColors.Window;
            this.modelTileControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelTileControlPanel.Location = new System.Drawing.Point(0, 42);
            this.modelTileControlPanel.Name = "modelTileControlPanel";
            this.modelTileControlPanel.Size = new System.Drawing.Size(785, 364);
            this.modelTileControlPanel.TabIndex = 14;
            // 
            // ModelPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.modelTileControlPanel);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelCategory);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "ModelPage";
            this.Size = new System.Drawing.Size(785, 469);
            this.Load += new System.EventHandler(this.HomePanel_Load);
            this.VisibleChanged += new System.EventHandler(this.ModelTileControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTouchProvider1)).EndInit();
            this.panelCategory.ResumeLayout(false);
            this.panelCategory.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Touch.UltraTouchProvider ultraTouchProvider1;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label labelFind;
        private System.Windows.Forms.Panel panelCategory;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.TextBox searchModelName;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonCloseMode;
        private System.Windows.Forms.Panel modelTileControlPanel;
    }
}
