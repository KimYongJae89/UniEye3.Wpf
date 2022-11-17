namespace DynMvp.Data.Forms
{
    partial class GridParamPanel
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
            this.useGrid = new System.Windows.Forms.CheckBox();
            this.comboBoxAcceptanceType = new System.Windows.Forms.ComboBox();
            this.columnCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelAcceptance = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rowCount = new System.Windows.Forms.NumericUpDown();
            this.labelRow = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.columnCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowCount)).BeginInit();
            this.SuspendLayout();
            // 
            // useGrid
            // 
            this.useGrid.AutoSize = true;
            this.useGrid.Location = new System.Drawing.Point(5, 5);
            this.useGrid.Name = "useGrid";
            this.useGrid.Size = new System.Drawing.Size(87, 24);
            this.useGrid.TabIndex = 14;
            this.useGrid.Text = "Use Grid";
            this.useGrid.UseVisualStyleBackColor = true;
            this.useGrid.CheckedChanged += new System.EventHandler(this.useGrid_CheckedChanged);
            // 
            // comboBoxAcceptanceType
            // 
            this.comboBoxAcceptanceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAcceptanceType.FormattingEnabled = true;
            this.comboBoxAcceptanceType.Items.AddRange(new object[] {
            "Ratio",
            "Count"});
            this.comboBoxAcceptanceType.Location = new System.Drawing.Point(121, 68);
            this.comboBoxAcceptanceType.Name = "comboBoxAcceptanceType";
            this.comboBoxAcceptanceType.Size = new System.Drawing.Size(107, 28);
            this.comboBoxAcceptanceType.TabIndex = 13;
            // 
            // columnCount
            // 
            this.columnCount.Location = new System.Drawing.Point(230, 35);
            this.columnCount.Name = "columnCount";
            this.columnCount.Size = new System.Drawing.Size(77, 27);
            this.columnCount.TabIndex = 10;
            this.columnCount.ValueChanged += new System.EventHandler(this.columnCount_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(230, 68);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(77, 27);
            this.numericUpDown1.TabIndex = 11;
            // 
            // labelAcceptance
            // 
            this.labelAcceptance.AutoSize = true;
            this.labelAcceptance.Location = new System.Drawing.Point(1, 70);
            this.labelAcceptance.Name = "labelAcceptance";
            this.labelAcceptance.Size = new System.Drawing.Size(87, 20);
            this.labelAcceptance.TabIndex = 7;
            this.labelAcceptance.Text = "Acceptance";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Column";
            // 
            // rowCount
            // 
            this.rowCount.Location = new System.Drawing.Point(46, 35);
            this.rowCount.Name = "rowCount";
            this.rowCount.Size = new System.Drawing.Size(77, 27);
            this.rowCount.TabIndex = 12;
            this.rowCount.ValueChanged += new System.EventHandler(this.rowCount_ValueChanged);
            // 
            // labelRow
            // 
            this.labelRow.AutoSize = true;
            this.labelRow.Location = new System.Drawing.Point(1, 37);
            this.labelRow.Name = "labelRow";
            this.labelRow.Size = new System.Drawing.Size(38, 20);
            this.labelRow.TabIndex = 9;
            this.labelRow.Text = "Row";
            // 
            // GridParamPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useGrid);
            this.Controls.Add(this.comboBoxAcceptanceType);
            this.Controls.Add(this.columnCount);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelAcceptance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rowCount);
            this.Controls.Add(this.labelRow);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GridParamPanel";
            this.Size = new System.Drawing.Size(314, 106);
            ((System.ComponentModel.ISupportInitialize)(this.columnCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox useGrid;
        private System.Windows.Forms.ComboBox comboBoxAcceptanceType;
        private System.Windows.Forms.NumericUpDown columnCount;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelAcceptance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown rowCount;
        private System.Windows.Forms.Label labelRow;
    }
}
