namespace UniEye.Base.UI.SamsungElec
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelPage));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnScreenModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLastModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnEtc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSelect = new Infragistics.Win.Misc.UltraButton();
            this.buttonNew = new Infragistics.Win.Misc.UltraButton();
            this.buttonCopy = new Infragistics.Win.Misc.UltraButton();
            this.buttonDelete = new Infragistics.Win.Misc.UltraButton();
            this.findModel = new System.Windows.Forms.TextBox();
            this.labelModelList = new Infragistics.Win.Misc.UltraLabel();
            this.panelModelList = new System.Windows.Forms.Panel();
            this.totalModel = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.menuPanel.SuspendLayout();
            this.panelModelList.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeColumns = false;
            this.modelList.AllowUserToResizeRows = false;
            this.modelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.modelList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnScreenModel,
            this.columnRegistrant,
            this.columnRegistrationDate,
            this.columnLastModifiedDate,
            this.columnEtc});
            this.modelList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.modelList.Location = new System.Drawing.Point(3, 99);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.RowHeadersVisible = false;
            this.modelList.RowTemplate.Height = 23;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.Size = new System.Drawing.Size(1008, 550);
            this.modelList.TabIndex = 50;
            this.modelList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ModelList_CellDoubleClick);
            this.modelList.SelectionChanged += new System.EventHandler(this.ModelList_SelectionChanged);
            // 
            // columnNo
            // 
            this.columnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Width = 50;
            // 
            // columnScreenModel
            // 
            this.columnScreenModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnScreenModel.HeaderText = "Model";
            this.columnScreenModel.Name = "columnScreenModel";
            // 
            // columnRegistrant
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnRegistrant.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnRegistrant.HeaderText = "Registrant";
            this.columnRegistrant.Name = "columnRegistrant";
            this.columnRegistrant.Width = 150;
            // 
            // columnRegistrationDate
            // 
            this.columnRegistrationDate.HeaderText = "Registration Date";
            this.columnRegistrationDate.Name = "columnRegistrationDate";
            this.columnRegistrationDate.Width = 150;
            // 
            // columnLastModifiedDate
            // 
            this.columnLastModifiedDate.HeaderText = "Last Modified";
            this.columnLastModifiedDate.Name = "columnLastModifiedDate";
            this.columnLastModifiedDate.Width = 150;
            // 
            // columnEtc
            // 
            this.columnEtc.HeaderText = "Etc.";
            this.columnEtc.Name = "columnEtc";
            this.columnEtc.Width = 150;
            // 
            // menuPanel
            // 
            this.menuPanel.Controls.Add(this.buttonSelect);
            this.menuPanel.Controls.Add(this.buttonNew);
            this.menuPanel.Controls.Add(this.buttonCopy);
            this.menuPanel.Controls.Add(this.buttonDelete);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.menuPanel.Location = new System.Drawing.Point(1018, 3);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.menuPanel.Size = new System.Drawing.Size(108, 655);
            this.menuPanel.TabIndex = 49;
            // 
            // buttonSelect
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 16F;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextVAlignAsString = "Bottom";
            this.buttonSelect.Appearance = appearance1;
            this.buttonSelect.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonSelect.Location = new System.Drawing.Point(3, 6);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(100, 100);
            this.buttonSelect.TabIndex = 27;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonNew
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Malgun Gothic";
            appearance2.FontData.SizeInPoints = 16F;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance2.TextVAlignAsString = "Bottom";
            this.buttonNew.Appearance = appearance2;
            this.buttonNew.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonNew.Location = new System.Drawing.Point(3, 112);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(100, 100);
            this.buttonNew.TabIndex = 26;
            this.buttonNew.Text = "New";
            this.buttonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // buttonCopy
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Malgun Gothic";
            appearance3.FontData.SizeInPoints = 16F;
            appearance3.Image = global::UniEye.Base.Properties.Resources.Copy64;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance3.TextVAlignAsString = "Bottom";
            this.buttonCopy.Appearance = appearance3;
            this.buttonCopy.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonCopy.Location = new System.Drawing.Point(3, 218);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(100, 100);
            this.buttonCopy.TabIndex = 29;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.Click += new System.EventHandler(this.ButtonCopy_Click);
            // 
            // buttonDelete
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 16F;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance4.TextVAlignAsString = "Bottom";
            this.buttonDelete.Appearance = appearance4;
            this.buttonDelete.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonDelete.Location = new System.Drawing.Point(3, 324);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 100);
            this.buttonDelete.TabIndex = 26;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // findModel
            // 
            this.findModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.findModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.findModel.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.findModel.Location = new System.Drawing.Point(124, 53);
            this.findModel.Name = "findModel";
            this.findModel.Size = new System.Drawing.Size(887, 43);
            this.findModel.TabIndex = 50;
            this.findModel.TextChanged += new System.EventHandler(this.FindModel_TextChanged);
            // 
            // labelModelList
            // 
            this.labelModelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance5.FontData.Name = "Malgun Gothic";
            appearance5.FontData.SizeInPoints = 22F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.labelModelList.Appearance = appearance5;
            this.labelModelList.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelModelList.Font = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelModelList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModelList.Location = new System.Drawing.Point(3, 2);
            this.labelModelList.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelModelList.Name = "labelModelList";
            this.labelModelList.Size = new System.Drawing.Size(1008, 45);
            this.labelModelList.TabIndex = 51;
            this.labelModelList.Text = "Model List";
            // 
            // panelModelList
            // 
            this.panelModelList.Controls.Add(this.totalModel);
            this.panelModelList.Controls.Add(this.findModel);
            this.panelModelList.Controls.Add(this.labelModelList);
            this.panelModelList.Controls.Add(this.modelList);
            this.panelModelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModelList.Location = new System.Drawing.Point(3, 3);
            this.panelModelList.Name = "panelModelList";
            this.panelModelList.Size = new System.Drawing.Size(1015, 655);
            this.panelModelList.TabIndex = 52;
            // 
            // totalModel
            // 
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.totalModel.Appearance = appearance6;
            this.totalModel.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.totalModel.Font = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.totalModel.Location = new System.Drawing.Point(3, 53);
            this.totalModel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.totalModel.Name = "totalModel";
            this.totalModel.Size = new System.Drawing.Size(118, 43);
            this.totalModel.TabIndex = 52;
            this.totalModel.Text = "Total : 100";
            // 
            // ModelPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelModelList);
            this.Controls.Add(this.menuPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModelPage";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(1129, 661);
            this.Load += new System.EventHandler(this.ModelManagePage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.menuPanel.ResumeLayout(false);
            this.panelModelList.ResumeLayout(false);
            this.panelModelList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private Infragistics.Win.Misc.UltraButton buttonSelect;
        private Infragistics.Win.Misc.UltraButton buttonNew;
        private Infragistics.Win.Misc.UltraButton buttonDelete;
        private System.Windows.Forms.TextBox findModel;
        private Infragistics.Win.Misc.UltraLabel labelModelList;
        private System.Windows.Forms.Panel panelModelList;
        private Infragistics.Win.Misc.UltraLabel totalModel;
        private Infragistics.Win.Misc.UltraButton buttonCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnScreenModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrant;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLastModifiedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnEtc;
    }
}
