namespace DynMvp.Data.Forms
{
    partial class BinaryCounterParamControl
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
            this.minScore = new System.Windows.Forms.NumericUpDown();
            this.labelGridAcceptance = new System.Windows.Forms.GroupBox();
            this.useGrid = new System.Windows.Forms.CheckBox();
            this.gridRowCount = new System.Windows.Forms.NumericUpDown();
            this.labelGridRow = new System.Windows.Forms.Label();
            this.gridColumnCount = new System.Windows.Forms.NumericUpDown();
            this.labelGridColumn = new System.Windows.Forms.Label();
            this.cellAcceptRatio = new System.Windows.Forms.NumericUpDown();
            this.labelCellAcceptRatio = new System.Windows.Forms.Label();
            this.scoreType = new System.Windows.Forms.ComboBox();
            this.labelScore = new System.Windows.Forms.Label();
            this.maxScore = new System.Windows.Forms.NumericUpDown();
            this.labelTilda = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.minScore)).BeginInit();
            this.labelGridAcceptance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellAcceptRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScore)).BeginInit();
            this.SuspendLayout();
            // 
            // minScore
            // 
            this.minScore.Location = new System.Drawing.Point(188, 100);
            this.minScore.Margin = new System.Windows.Forms.Padding(7);
            this.minScore.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minScore.Name = "minScore";
            this.minScore.Size = new System.Drawing.Size(68, 27);
            this.minScore.TabIndex = 4;
            this.minScore.ValueChanged += new System.EventHandler(this.minScore_ValueChanged);
            this.minScore.Enter += new System.EventHandler(this.textBox_Enter);
            this.minScore.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelGridAcceptance
            // 
            this.labelGridAcceptance.Controls.Add(this.useGrid);
            this.labelGridAcceptance.Controls.Add(this.gridRowCount);
            this.labelGridAcceptance.Controls.Add(this.labelGridRow);
            this.labelGridAcceptance.Controls.Add(this.gridColumnCount);
            this.labelGridAcceptance.Controls.Add(this.labelGridColumn);
            this.labelGridAcceptance.Controls.Add(this.cellAcceptRatio);
            this.labelGridAcceptance.Controls.Add(this.labelCellAcceptRatio);
            this.labelGridAcceptance.Location = new System.Drawing.Point(4, 4);
            this.labelGridAcceptance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelGridAcceptance.Name = "labelGridAcceptance";
            this.labelGridAcceptance.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelGridAcceptance.Size = new System.Drawing.Size(357, 89);
            this.labelGridAcceptance.TabIndex = 10;
            this.labelGridAcceptance.TabStop = false;
            this.labelGridAcceptance.Text = "groupBox1";
            // 
            // useGrid
            // 
            this.useGrid.AutoSize = true;
            this.useGrid.Location = new System.Drawing.Point(6, 0);
            this.useGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.useGrid.Name = "useGrid";
            this.useGrid.Size = new System.Drawing.Size(87, 24);
            this.useGrid.TabIndex = 22;
            this.useGrid.Text = "Use Grid";
            this.useGrid.UseVisualStyleBackColor = true;
            this.useGrid.CheckedChanged += new System.EventHandler(this.useGrid_CheckedChanged);
            // 
            // gridRowCount
            // 
            this.gridRowCount.Location = new System.Drawing.Point(92, 24);
            this.gridRowCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridRowCount.Name = "gridRowCount";
            this.gridRowCount.Size = new System.Drawing.Size(68, 27);
            this.gridRowCount.TabIndex = 20;
            this.gridRowCount.ValueChanged += new System.EventHandler(this.gridRowCount_ValueChanged);
            // 
            // labelGridRow
            // 
            this.labelGridRow.AutoSize = true;
            this.labelGridRow.Location = new System.Drawing.Point(8, 28);
            this.labelGridRow.Name = "labelGridRow";
            this.labelGridRow.Size = new System.Drawing.Size(38, 20);
            this.labelGridRow.TabIndex = 17;
            this.labelGridRow.Text = "Row";
            // 
            // gridColumnCount
            // 
            this.gridColumnCount.Location = new System.Drawing.Point(280, 25);
            this.gridColumnCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridColumnCount.Name = "gridColumnCount";
            this.gridColumnCount.Size = new System.Drawing.Size(68, 27);
            this.gridColumnCount.TabIndex = 18;
            this.gridColumnCount.ValueChanged += new System.EventHandler(this.gridColumnCount_ValueChanged);
            // 
            // labelGridColumn
            // 
            this.labelGridColumn.AutoSize = true;
            this.labelGridColumn.Location = new System.Drawing.Point(193, 28);
            this.labelGridColumn.Name = "labelGridColumn";
            this.labelGridColumn.Size = new System.Drawing.Size(63, 20);
            this.labelGridColumn.TabIndex = 16;
            this.labelGridColumn.Text = "Column";
            // 
            // cellAcceptRatio
            // 
            this.cellAcceptRatio.DecimalPlaces = 2;
            this.cellAcceptRatio.Location = new System.Drawing.Point(280, 55);
            this.cellAcceptRatio.Margin = new System.Windows.Forms.Padding(7);
            this.cellAcceptRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.cellAcceptRatio.Name = "cellAcceptRatio";
            this.cellAcceptRatio.Size = new System.Drawing.Size(68, 27);
            this.cellAcceptRatio.TabIndex = 4;
            this.cellAcceptRatio.ValueChanged += new System.EventHandler(this.maxScore_ValueChanged);
            this.cellAcceptRatio.Enter += new System.EventHandler(this.textBox_Enter);
            this.cellAcceptRatio.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelCellAcceptRatio
            // 
            this.labelCellAcceptRatio.AutoSize = true;
            this.labelCellAcceptRatio.Location = new System.Drawing.Point(8, 56);
            this.labelCellAcceptRatio.Name = "labelCellAcceptRatio";
            this.labelCellAcceptRatio.Size = new System.Drawing.Size(126, 20);
            this.labelCellAcceptRatio.TabIndex = 15;
            this.labelCellAcceptRatio.Text = "Cell Accept Ratio";
            // 
            // scoreType
            // 
            this.scoreType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scoreType.FormattingEnabled = true;
            this.scoreType.Items.AddRange(new object[] {
            "Ratio",
            "Count"});
            this.scoreType.Location = new System.Drawing.Point(96, 99);
            this.scoreType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.scoreType.Name = "scoreType";
            this.scoreType.Size = new System.Drawing.Size(90, 28);
            this.scoreType.TabIndex = 21;
            this.scoreType.SelectedIndexChanged += new System.EventHandler(this.scoreType_SelectedIndexChanged);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(12, 101);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(46, 20);
            this.labelScore.TabIndex = 15;
            this.labelScore.Text = "Score";
            // 
            // maxScore
            // 
            this.maxScore.Location = new System.Drawing.Point(284, 100);
            this.maxScore.Margin = new System.Windows.Forms.Padding(7);
            this.maxScore.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxScore.Name = "maxScore";
            this.maxScore.Size = new System.Drawing.Size(68, 27);
            this.maxScore.TabIndex = 4;
            this.maxScore.ValueChanged += new System.EventHandler(this.maxScore_ValueChanged);
            this.maxScore.Enter += new System.EventHandler(this.textBox_Enter);
            this.maxScore.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(260, 104);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(20, 20);
            this.labelTilda.TabIndex = 5;
            this.labelTilda.Text = "~";
            // 
            // BinaryCounterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.labelGridAcceptance);
            this.Controls.Add(this.scoreType);
            this.Controls.Add(this.maxScore);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.minScore);
            this.Controls.Add(this.labelTilda);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BinaryCounterParamControl";
            this.Size = new System.Drawing.Size(364, 279);
            ((System.ComponentModel.ISupportInitialize)(this.minScore)).EndInit();
            this.labelGridAcceptance.ResumeLayout(false);
            this.labelGridAcceptance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellAcceptRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown minScore;
        private System.Windows.Forms.GroupBox labelGridAcceptance;
        private System.Windows.Forms.CheckBox useGrid;
        private System.Windows.Forms.NumericUpDown gridRowCount;
        private System.Windows.Forms.ComboBox scoreType;
        private System.Windows.Forms.Label labelGridRow;
        private System.Windows.Forms.NumericUpDown gridColumnCount;
        private System.Windows.Forms.Label labelGridColumn;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.NumericUpDown maxScore;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.NumericUpDown cellAcceptRatio;
        private System.Windows.Forms.Label labelCellAcceptRatio;
    }
}