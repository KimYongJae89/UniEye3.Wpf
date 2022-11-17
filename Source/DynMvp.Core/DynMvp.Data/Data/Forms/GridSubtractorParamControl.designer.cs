namespace DynMvp.Data.Forms
{
    partial class GridSubtractorParamControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.minPixelCount = new System.Windows.Forms.NumericUpDown();
            this.maskImageSelector = new System.Windows.Forms.DataGridView();
            this.ColumnMaskImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.deleteMaskButton = new System.Windows.Forms.Button();
            this.refreshMaskButton = new System.Windows.Forms.Button();
            this.addMaskButton = new System.Windows.Forms.Button();
            this.labelAreaMinName = new System.Windows.Forms.Label();
            this.patternScore = new System.Windows.Forms.NumericUpDown();
            this.groupPattern = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchRangeY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.searchRangeX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.labelOffsetY = new System.Windows.Forms.Label();
            this.patternMaxOffsetY = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetX = new System.Windows.Forms.Label();
            this.patternMaxOffsetX = new System.Windows.Forms.NumericUpDown();
            this.labelPatternMaxOffset = new System.Windows.Forms.Label();
            this.labelPatternScore = new System.Windows.Forms.Label();
            this.labelInvert = new System.Windows.Forms.Label();
            this.labelProjectionRatio = new System.Windows.Forms.Label();
            this.groupInspection = new System.Windows.Forms.GroupBox();
            this.invert = new System.Windows.Forms.CheckBox();
            this.projectionRatio = new System.Windows.Forms.NumericUpDown();
            this.labelMinThreshold = new System.Windows.Forms.Label();
            this.minThreshold = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.maxThreshold = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.minPixelCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskImageSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternScore)).BeginInit();
            this.groupPattern.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMaxOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMaxOffsetX)).BeginInit();
            this.groupInspection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectionRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // minPixelCount
            // 
            this.minPixelCount.Location = new System.Drawing.Point(256, 24);
            this.minPixelCount.Margin = new System.Windows.Forms.Padding(7);
            this.minPixelCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minPixelCount.Name = "minPixelCount";
            this.minPixelCount.Size = new System.Drawing.Size(63, 26);
            this.minPixelCount.TabIndex = 8;
            this.minPixelCount.ValueChanged += new System.EventHandler(this.minPixelCount_ValueChanged);
            // 
            // maskImageSelector
            // 
            this.maskImageSelector.AllowUserToAddRows = false;
            this.maskImageSelector.AllowUserToDeleteRows = false;
            this.maskImageSelector.AllowUserToResizeColumns = false;
            this.maskImageSelector.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.maskImageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.maskImageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.maskImageSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnMaskImage});
            this.maskImageSelector.Location = new System.Drawing.Point(11, 135);
            this.maskImageSelector.Margin = new System.Windows.Forms.Padding(2);
            this.maskImageSelector.MultiSelect = false;
            this.maskImageSelector.Name = "maskImageSelector";
            this.maskImageSelector.ReadOnly = true;
            this.maskImageSelector.RowHeadersVisible = false;
            this.maskImageSelector.RowTemplate.Height = 23;
            this.maskImageSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.maskImageSelector.Size = new System.Drawing.Size(203, 186);
            this.maskImageSelector.TabIndex = 10;
            // 
            // ColumnMaskImage
            // 
            this.ColumnMaskImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnMaskImage.HeaderText = "Mask Image";
            this.ColumnMaskImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ColumnMaskImage.Name = "ColumnMaskImage";
            this.ColumnMaskImage.ReadOnly = true;
            this.ColumnMaskImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnMaskImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // deleteMaskButton
            // 
            this.deleteMaskButton.Location = new System.Drawing.Point(220, 205);
            this.deleteMaskButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteMaskButton.Name = "deleteMaskButton";
            this.deleteMaskButton.Size = new System.Drawing.Size(74, 27);
            this.deleteMaskButton.TabIndex = 13;
            this.deleteMaskButton.Text = "Delete";
            this.deleteMaskButton.UseVisualStyleBackColor = true;
            this.deleteMaskButton.Click += new System.EventHandler(this.DeleteMaskButton_Click);
            // 
            // refreshMaskButton
            // 
            this.refreshMaskButton.Location = new System.Drawing.Point(220, 170);
            this.refreshMaskButton.Margin = new System.Windows.Forms.Padding(4);
            this.refreshMaskButton.Name = "refreshMaskButton";
            this.refreshMaskButton.Size = new System.Drawing.Size(74, 27);
            this.refreshMaskButton.TabIndex = 12;
            this.refreshMaskButton.Text = "Refresh";
            this.refreshMaskButton.UseVisualStyleBackColor = true;
            this.refreshMaskButton.Click += new System.EventHandler(this.refreshMaskButton_Click);
            // 
            // addMaskButton
            // 
            this.addMaskButton.Location = new System.Drawing.Point(220, 135);
            this.addMaskButton.Margin = new System.Windows.Forms.Padding(4);
            this.addMaskButton.Name = "addMaskButton";
            this.addMaskButton.Size = new System.Drawing.Size(74, 27);
            this.addMaskButton.TabIndex = 11;
            this.addMaskButton.Text = "Add";
            this.addMaskButton.UseVisualStyleBackColor = true;
            this.addMaskButton.Click += new System.EventHandler(this.addMaskButton_Click);
            // 
            // labelAreaMinName
            // 
            this.labelAreaMinName.AutoSize = true;
            this.labelAreaMinName.Location = new System.Drawing.Point(22, 26);
            this.labelAreaMinName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAreaMinName.Name = "labelAreaMinName";
            this.labelAreaMinName.Size = new System.Drawing.Size(115, 20);
            this.labelAreaMinName.TabIndex = 14;
            this.labelAreaMinName.Text = "Pixel Threshold";
            // 
            // patternScore
            // 
            this.patternScore.Location = new System.Drawing.Point(256, 25);
            this.patternScore.Name = "patternScore";
            this.patternScore.Size = new System.Drawing.Size(62, 26);
            this.patternScore.TabIndex = 15;
            this.patternScore.ValueChanged += new System.EventHandler(this.patternScore_ValueChanged);
            // 
            // groupPattern
            // 
            this.groupPattern.Controls.Add(this.label1);
            this.groupPattern.Controls.Add(this.searchRangeY);
            this.groupPattern.Controls.Add(this.label2);
            this.groupPattern.Controls.Add(this.searchRangeX);
            this.groupPattern.Controls.Add(this.label3);
            this.groupPattern.Controls.Add(this.labelOffsetY);
            this.groupPattern.Controls.Add(this.patternMaxOffsetY);
            this.groupPattern.Controls.Add(this.labelOffsetX);
            this.groupPattern.Controls.Add(this.patternMaxOffsetX);
            this.groupPattern.Controls.Add(this.labelPatternMaxOffset);
            this.groupPattern.Controls.Add(this.labelPatternScore);
            this.groupPattern.Controls.Add(this.maskImageSelector);
            this.groupPattern.Controls.Add(this.patternScore);
            this.groupPattern.Controls.Add(this.deleteMaskButton);
            this.groupPattern.Controls.Add(this.refreshMaskButton);
            this.groupPattern.Controls.Add(this.addMaskButton);
            this.groupPattern.Location = new System.Drawing.Point(7, 180);
            this.groupPattern.Name = "groupPattern";
            this.groupPattern.Size = new System.Drawing.Size(334, 335);
            this.groupPattern.TabIndex = 17;
            this.groupPattern.TabStop = false;
            this.groupPattern.Text = "Pattern";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "Y";
            // 
            // searchRangeY
            // 
            this.searchRangeY.Location = new System.Drawing.Point(257, 56);
            this.searchRangeY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.searchRangeY.Name = "searchRangeY";
            this.searchRangeY.Size = new System.Drawing.Size(62, 26);
            this.searchRangeY.TabIndex = 24;
            this.searchRangeY.ValueChanged += new System.EventHandler(this.searchRangeY_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "X";
            // 
            // searchRangeX
            // 
            this.searchRangeX.Location = new System.Drawing.Point(148, 56);
            this.searchRangeX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.searchRangeX.Name = "searchRangeX";
            this.searchRangeX.Size = new System.Drawing.Size(62, 26);
            this.searchRangeX.TabIndex = 22;
            this.searchRangeX.ValueChanged += new System.EventHandler(this.searchRangeX_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "SearchRange";
            // 
            // labelOffsetY
            // 
            this.labelOffsetY.AutoSize = true;
            this.labelOffsetY.Location = new System.Drawing.Point(230, 90);
            this.labelOffsetY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffsetY.Name = "labelOffsetY";
            this.labelOffsetY.Size = new System.Drawing.Size(20, 20);
            this.labelOffsetY.TabIndex = 20;
            this.labelOffsetY.Text = "Y";
            // 
            // patternMaxOffsetY
            // 
            this.patternMaxOffsetY.Location = new System.Drawing.Point(257, 88);
            this.patternMaxOffsetY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.patternMaxOffsetY.Name = "patternMaxOffsetY";
            this.patternMaxOffsetY.Size = new System.Drawing.Size(62, 26);
            this.patternMaxOffsetY.TabIndex = 19;
            this.patternMaxOffsetY.ValueChanged += new System.EventHandler(this.patternMaxOffsetY_ValueChanged);
            // 
            // labelOffsetX
            // 
            this.labelOffsetX.AutoSize = true;
            this.labelOffsetX.Location = new System.Drawing.Point(121, 90);
            this.labelOffsetX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffsetX.Name = "labelOffsetX";
            this.labelOffsetX.Size = new System.Drawing.Size(20, 20);
            this.labelOffsetX.TabIndex = 18;
            this.labelOffsetX.Text = "X";
            // 
            // patternMaxOffsetX
            // 
            this.patternMaxOffsetX.Location = new System.Drawing.Point(148, 88);
            this.patternMaxOffsetX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.patternMaxOffsetX.Name = "patternMaxOffsetX";
            this.patternMaxOffsetX.Size = new System.Drawing.Size(62, 26);
            this.patternMaxOffsetX.TabIndex = 17;
            this.patternMaxOffsetX.ValueChanged += new System.EventHandler(this.patternMaxOffsetX_ValueChanged);
            // 
            // labelPatternMaxOffset
            // 
            this.labelPatternMaxOffset.AutoSize = true;
            this.labelPatternMaxOffset.Location = new System.Drawing.Point(7, 90);
            this.labelPatternMaxOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPatternMaxOffset.Name = "labelPatternMaxOffset";
            this.labelPatternMaxOffset.Size = new System.Drawing.Size(86, 20);
            this.labelPatternMaxOffset.TabIndex = 16;
            this.labelPatternMaxOffset.Text = "Max Offset";
            // 
            // labelPatternScore
            // 
            this.labelPatternScore.AutoSize = true;
            this.labelPatternScore.Location = new System.Drawing.Point(7, 27);
            this.labelPatternScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPatternScore.Name = "labelPatternScore";
            this.labelPatternScore.Size = new System.Drawing.Size(51, 20);
            this.labelPatternScore.TabIndex = 8;
            this.labelPatternScore.Text = "Score";
            // 
            // labelInvert
            // 
            this.labelInvert.AutoSize = true;
            this.labelInvert.Location = new System.Drawing.Point(25, 145);
            this.labelInvert.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInvert.Name = "labelInvert";
            this.labelInvert.Size = new System.Drawing.Size(49, 20);
            this.labelInvert.TabIndex = 18;
            this.labelInvert.Text = "Invert";
            // 
            // labelProjectionRatio
            // 
            this.labelProjectionRatio.AutoSize = true;
            this.labelProjectionRatio.Location = new System.Drawing.Point(22, 115);
            this.labelProjectionRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProjectionRatio.Name = "labelProjectionRatio";
            this.labelProjectionRatio.Size = new System.Drawing.Size(121, 20);
            this.labelProjectionRatio.TabIndex = 19;
            this.labelProjectionRatio.Text = "Projection Ratio";
            // 
            // groupInspection
            // 
            this.groupInspection.Controls.Add(this.label4);
            this.groupInspection.Controls.Add(this.maxThreshold);
            this.groupInspection.Controls.Add(this.labelMinThreshold);
            this.groupInspection.Controls.Add(this.minThreshold);
            this.groupInspection.Controls.Add(this.invert);
            this.groupInspection.Controls.Add(this.projectionRatio);
            this.groupInspection.Controls.Add(this.labelAreaMinName);
            this.groupInspection.Controls.Add(this.labelProjectionRatio);
            this.groupInspection.Controls.Add(this.minPixelCount);
            this.groupInspection.Controls.Add(this.labelInvert);
            this.groupInspection.Location = new System.Drawing.Point(3, 3);
            this.groupInspection.Name = "groupInspection";
            this.groupInspection.Size = new System.Drawing.Size(334, 171);
            this.groupInspection.TabIndex = 20;
            this.groupInspection.TabStop = false;
            this.groupInspection.Text = "Inspection";
            // 
            // invert
            // 
            this.invert.AutoSize = true;
            this.invert.Location = new System.Drawing.Point(304, 149);
            this.invert.Name = "invert";
            this.invert.Size = new System.Drawing.Size(15, 14);
            this.invert.TabIndex = 21;
            this.invert.UseVisualStyleBackColor = true;
            this.invert.CheckedChanged += new System.EventHandler(this.invert_CheckedChanged);
            // 
            // projectionRatio
            // 
            this.projectionRatio.DecimalPlaces = 3;
            this.projectionRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.projectionRatio.Location = new System.Drawing.Point(256, 113);
            this.projectionRatio.Margin = new System.Windows.Forms.Padding(7);
            this.projectionRatio.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.projectionRatio.Name = "projectionRatio";
            this.projectionRatio.Size = new System.Drawing.Size(63, 26);
            this.projectionRatio.TabIndex = 20;
            this.projectionRatio.ValueChanged += new System.EventHandler(this.projectionRatio_ValueChanged);
            // 
            // labelMinThreshold
            // 
            this.labelMinThreshold.AutoSize = true;
            this.labelMinThreshold.Location = new System.Drawing.Point(22, 55);
            this.labelMinThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinThreshold.Name = "labelMinThreshold";
            this.labelMinThreshold.Size = new System.Drawing.Size(108, 20);
            this.labelMinThreshold.TabIndex = 23;
            this.labelMinThreshold.Text = "Min Threshold";
            // 
            // minThreshold
            // 
            this.minThreshold.Location = new System.Drawing.Point(256, 53);
            this.minThreshold.Margin = new System.Windows.Forms.Padding(7);
            this.minThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.minThreshold.Name = "minThreshold";
            this.minThreshold.Size = new System.Drawing.Size(63, 26);
            this.minThreshold.TabIndex = 22;
            this.minThreshold.ValueChanged += new System.EventHandler(this.minThreshold_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "Max Threshold";
            // 
            // maxThreshold
            // 
            this.maxThreshold.Location = new System.Drawing.Point(256, 81);
            this.maxThreshold.Margin = new System.Windows.Forms.Padding(7);
            this.maxThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maxThreshold.Name = "maxThreshold";
            this.maxThreshold.Size = new System.Drawing.Size(63, 26);
            this.maxThreshold.TabIndex = 24;
            this.maxThreshold.ValueChanged += new System.EventHandler(this.maxThreshold_ValueChanged);
            // 
            // GridSubtractorParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupInspection);
            this.Controls.Add(this.groupPattern);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GridSubtractorParamControl";
            this.Size = new System.Drawing.Size(341, 542);
            ((System.ComponentModel.ISupportInitialize)(this.minPixelCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskImageSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternScore)).EndInit();
            this.groupPattern.ResumeLayout(false);
            this.groupPattern.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMaxOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMaxOffsetX)).EndInit();
            this.groupInspection.ResumeLayout(false);
            this.groupInspection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectionRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxThreshold)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown minPixelCount;
        private System.Windows.Forms.DataGridView maskImageSelector;
        private System.Windows.Forms.DataGridViewImageColumn ColumnMaskImage;
        private System.Windows.Forms.Button deleteMaskButton;
        private System.Windows.Forms.Button refreshMaskButton;
        private System.Windows.Forms.Button addMaskButton;
        private System.Windows.Forms.Label labelAreaMinName;
        private System.Windows.Forms.NumericUpDown patternScore;
        private System.Windows.Forms.GroupBox groupPattern;
        private System.Windows.Forms.NumericUpDown patternMaxOffsetX;
        private System.Windows.Forms.Label labelPatternMaxOffset;
        private System.Windows.Forms.Label labelPatternScore;
        private System.Windows.Forms.Label labelInvert;
        private System.Windows.Forms.Label labelProjectionRatio;
        private System.Windows.Forms.GroupBox groupInspection;
        private System.Windows.Forms.CheckBox invert;
        private System.Windows.Forms.NumericUpDown projectionRatio;
        private System.Windows.Forms.Label labelOffsetY;
        private System.Windows.Forms.NumericUpDown patternMaxOffsetY;
        private System.Windows.Forms.Label labelOffsetX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown searchRangeY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown searchRangeX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxThreshold;
        private System.Windows.Forms.Label labelMinThreshold;
        private System.Windows.Forms.NumericUpDown minThreshold;
    }
}
