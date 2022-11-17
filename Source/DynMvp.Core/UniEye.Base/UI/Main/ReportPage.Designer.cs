namespace UniEye.Base.UI.Main
{
    partial class ReportPage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.endHour = new System.Windows.Forms.ComboBox();
            this.startHour = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.labelEnd = new System.Windows.Forms.Label();
            this.labelStart = new System.Windows.Forms.Label();
            this.modelCombo = new System.Windows.Forms.ComboBox();
            this.labelModel = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.labelInspectionStep = new System.Windows.Forms.Label();
            this.stepNo = new System.Windows.Forms.ComboBox();
            this.searchFalseReject = new System.Windows.Forms.CheckBox();
            this.searchNg = new System.Windows.Forms.CheckBox();
            this.searchGood = new System.Windows.Forms.CheckBox();
            this.serialDataGridView = new System.Windows.Forms.DataGridView();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.labelGood = new System.Windows.Forms.Label();
            this.labelNG = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.productionGood = new Infragistics.Win.Misc.UltraLabel();
            this.productionNg = new Infragistics.Win.Misc.UltraLabel();
            this.productionTotal = new Infragistics.Win.Misc.UltraLabel();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serialDataGridView)).BeginInit();
            this.leftPanel.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // endHour
            // 
            this.endHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endHour.FormattingEnabled = true;
            this.endHour.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.endHour.Location = new System.Drawing.Point(243, 70);
            this.endHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.endHour.Name = "endHour";
            this.endHour.Size = new System.Drawing.Size(86, 28);
            this.endHour.TabIndex = 15;
            // 
            // startHour
            // 
            this.startHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startHour.FormattingEnabled = true;
            this.startHour.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.startHour.Location = new System.Drawing.Point(243, 38);
            this.startHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startHour.Name = "startHour";
            this.startHour.Size = new System.Drawing.Size(86, 28);
            this.startHour.TabIndex = 16;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::UniEye.Base.Properties.Resources.test_32;
            this.btnSearch.Location = new System.Drawing.Point(174, 107);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(155, 53);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // endDate
            // 
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(73, 70);
            this.endDate.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(150, 26);
            this.endDate.TabIndex = 13;
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(73, 38);
            this.startDate.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(150, 26);
            this.startDate.TabIndex = 12;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Location = new System.Drawing.Point(5, 73);
            this.labelEnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(27, 20);
            this.labelEnd.TabIndex = 18;
            this.labelEnd.Text = "To";
            this.labelEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(5, 41);
            this.labelStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(46, 20);
            this.labelStart.TabIndex = 17;
            this.labelStart.Text = "From";
            this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // modelCombo
            // 
            this.modelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelCombo.FormattingEnabled = true;
            this.modelCombo.ItemHeight = 20;
            this.modelCombo.Location = new System.Drawing.Point(79, 5);
            this.modelCombo.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.modelCombo.Name = "modelCombo";
            this.modelCombo.Size = new System.Drawing.Size(250, 28);
            this.modelCombo.TabIndex = 20;
            this.modelCombo.SelectedIndexChanged += new System.EventHandler(this.modelCombo_SelectedIndexChanged);
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(4, 8);
            this.labelModel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(52, 20);
            this.labelModel.TabIndex = 19;
            this.labelModel.Text = "Model";
            this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.labelInspectionStep);
            this.searchPanel.Controls.Add(this.stepNo);
            this.searchPanel.Controls.Add(this.searchFalseReject);
            this.searchPanel.Controls.Add(this.searchNg);
            this.searchPanel.Controls.Add(this.searchGood);
            this.searchPanel.Controls.Add(this.modelCombo);
            this.searchPanel.Controls.Add(this.labelModel);
            this.searchPanel.Controls.Add(this.labelEnd);
            this.searchPanel.Controls.Add(this.labelStart);
            this.searchPanel.Controls.Add(this.endHour);
            this.searchPanel.Controls.Add(this.startHour);
            this.searchPanel.Controls.Add(this.btnSearch);
            this.searchPanel.Controls.Add(this.endDate);
            this.searchPanel.Controls.Add(this.startDate);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(0, 0);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(334, 206);
            this.searchPanel.TabIndex = 21;
            // 
            // labelInspectionStep
            // 
            this.labelInspectionStep.AutoSize = true;
            this.labelInspectionStep.Location = new System.Drawing.Point(5, 166);
            this.labelInspectionStep.Name = "labelInspectionStep";
            this.labelInspectionStep.Size = new System.Drawing.Size(121, 20);
            this.labelInspectionStep.TabIndex = 23;
            this.labelInspectionStep.Text = "Inspection Step";
            this.labelInspectionStep.Visible = false;
            // 
            // stepNo
            // 
            this.stepNo.FormattingEnabled = true;
            this.stepNo.Location = new System.Drawing.Point(174, 163);
            this.stepNo.Name = "stepNo";
            this.stepNo.Size = new System.Drawing.Size(121, 28);
            this.stepNo.TabIndex = 22;
            this.stepNo.Visible = false;
            this.stepNo.SelectedIndexChanged += new System.EventHandler(this.stepNo_SelectedIndexChanged);
            // 
            // searchFalseReject
            // 
            this.searchFalseReject.AutoSize = true;
            this.searchFalseReject.Location = new System.Drawing.Point(9, 137);
            this.searchFalseReject.Name = "searchFalseReject";
            this.searchFalseReject.Size = new System.Drawing.Size(78, 24);
            this.searchFalseReject.TabIndex = 21;
            this.searchFalseReject.Text = "Overkill";
            this.searchFalseReject.UseVisualStyleBackColor = true;
            // 
            // searchNg
            // 
            this.searchNg.AutoSize = true;
            this.searchNg.Location = new System.Drawing.Point(83, 107);
            this.searchNg.Name = "searchNg";
            this.searchNg.Size = new System.Drawing.Size(52, 24);
            this.searchNg.TabIndex = 21;
            this.searchNg.Text = "NG";
            this.searchNg.UseVisualStyleBackColor = true;
            // 
            // searchGood
            // 
            this.searchGood.AutoSize = true;
            this.searchGood.Location = new System.Drawing.Point(9, 107);
            this.searchGood.Name = "searchGood";
            this.searchGood.Size = new System.Drawing.Size(68, 24);
            this.searchGood.TabIndex = 21;
            this.searchGood.Text = "Good";
            this.searchGood.UseVisualStyleBackColor = true;
            // 
            // serialDataGridView
            // 
            this.serialDataGridView.AllowUserToAddRows = false;
            this.serialDataGridView.AllowUserToDeleteRows = false;
            this.serialDataGridView.AllowUserToResizeRows = false;
            this.serialDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.serialDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serialDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serialDataGridView.Location = new System.Drawing.Point(0, 206);
            this.serialDataGridView.MultiSelect = false;
            this.serialDataGridView.Name = "serialDataGridView";
            this.serialDataGridView.ReadOnly = true;
            this.serialDataGridView.RowHeadersVisible = false;
            this.serialDataGridView.RowTemplate.Height = 23;
            this.serialDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serialDataGridView.Size = new System.Drawing.Size(334, 384);
            this.serialDataGridView.TabIndex = 21;
            this.serialDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serialDataGridView_CellContentClick);
            this.serialDataGridView.SelectionChanged += new System.EventHandler(this.serialDataGridView_SelectionChanged);
            this.serialDataGridView.DoubleClick += new System.EventHandler(this.serialDataGridView_DoubleClick);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.serialDataGridView);
            this.leftPanel.Controls.Add(this.panelSummary);
            this.leftPanel.Controls.Add(this.searchPanel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(334, 696);
            this.leftPanel.TabIndex = 23;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.labelGood);
            this.panelSummary.Controls.Add(this.labelNG);
            this.panelSummary.Controls.Add(this.labelTotal);
            this.panelSummary.Controls.Add(this.productionGood);
            this.panelSummary.Controls.Add(this.productionNg);
            this.panelSummary.Controls.Add(this.productionTotal);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Location = new System.Drawing.Point(0, 590);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(334, 106);
            this.panelSummary.TabIndex = 22;
            // 
            // labelGood
            // 
            this.labelGood.BackColor = System.Drawing.Color.SpringGreen;
            this.labelGood.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelGood.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGood.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelGood.Location = new System.Drawing.Point(4, 71);
            this.labelGood.Name = "labelGood";
            this.labelGood.Size = new System.Drawing.Size(206, 32);
            this.labelGood.TabIndex = 20;
            this.labelGood.Text = "Good";
            this.labelGood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNG
            // 
            this.labelNG.BackColor = System.Drawing.Color.Red;
            this.labelNG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNG.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNG.ForeColor = System.Drawing.SystemColors.Control;
            this.labelNG.Location = new System.Drawing.Point(4, 37);
            this.labelNG.Name = "labelNG";
            this.labelNG.Size = new System.Drawing.Size(206, 32);
            this.labelNG.TabIndex = 19;
            this.labelNG.Text = "NG";
            this.labelNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotal
            // 
            this.labelTotal.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.labelTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotal.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTotal.Location = new System.Drawing.Point(4, 3);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(206, 32);
            this.labelTotal.TabIndex = 18;
            this.labelTotal.Text = "Total";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionGood
            // 
            appearance1.BackColor = System.Drawing.Color.Ivory;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Segoe UI Light";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.productionGood.Appearance = appearance1;
            this.productionGood.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionGood.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionGood.Location = new System.Drawing.Point(212, 72);
            this.productionGood.Name = "productionGood";
            this.productionGood.Size = new System.Drawing.Size(119, 31);
            this.productionGood.TabIndex = 15;
            this.productionGood.Text = "0";
            // 
            // productionNg
            // 
            appearance2.BackColor = System.Drawing.Color.Ivory;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Segoe UI Light";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.productionNg.Appearance = appearance2;
            this.productionNg.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionNg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionNg.Location = new System.Drawing.Point(212, 38);
            this.productionNg.Name = "productionNg";
            this.productionNg.Size = new System.Drawing.Size(119, 31);
            this.productionNg.TabIndex = 15;
            this.productionNg.Text = "0";
            // 
            // productionTotal
            // 
            appearance3.BackColor = System.Drawing.Color.Ivory;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Segoe UI Light";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.productionTotal.Appearance = appearance3;
            this.productionTotal.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionTotal.Location = new System.Drawing.Point(212, 4);
            this.productionTotal.Name = "productionTotal";
            this.productionTotal.Size = new System.Drawing.Size(119, 31);
            this.productionTotal.TabIndex = 17;
            this.productionTotal.Text = "0";
            // 
            // ReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.leftPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(738, 696);
            this.Load += new System.EventHandler(this.ReportPage_Load);
            this.VisibleChanged += new System.EventHandler(this.ReportPanel_VisibleChanged);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serialDataGridView)).EndInit();
            this.leftPanel.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox endHour;
        private System.Windows.Forms.ComboBox startHour;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.ComboBox modelCombo;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.DataGridView serialDataGridView;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel panelSummary;
        private Infragistics.Win.Misc.UltraLabel productionGood;
        private Infragistics.Win.Misc.UltraLabel productionNg;
        private Infragistics.Win.Misc.UltraLabel productionTotal;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label labelGood;
        private System.Windows.Forms.Label labelNG;
        private System.Windows.Forms.CheckBox searchNg;
        private System.Windows.Forms.CheckBox searchGood;
        private System.Windows.Forms.CheckBox searchFalseReject;
        private System.Windows.Forms.ComboBox stepNo;
        private System.Windows.Forms.Label labelInspectionStep;
    }
}
