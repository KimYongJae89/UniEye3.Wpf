namespace DynMvp.Data.Forms
{
    partial class BlobSubtractorParamControl
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
            this.labelEdgeThreshold = new System.Windows.Forms.Label();
            this.edgeThreshold = new System.Windows.Forms.NumericUpDown();
            this.maxPixelCount = new System.Windows.Forms.NumericUpDown();
            this.minPixelCount = new System.Windows.Forms.NumericUpDown();
            this.labelTilda = new System.Windows.Forms.Label();
            this.labelPixelCount = new System.Windows.Forms.Label();
            this.maskImageSelector = new System.Windows.Forms.DataGridView();
            this.ColumnMaskImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.deleteMaskButton = new System.Windows.Forms.Button();
            this.refreshMaskButton = new System.Windows.Forms.Button();
            this.addMaskButton = new System.Windows.Forms.Button();
            this.labelAreaMinName = new System.Windows.Forms.Label();
            this.numAreaMin = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.edgeThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPixelCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskImageSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMin)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEdgeThreshold
            // 
            this.labelEdgeThreshold.AutoSize = true;
            this.labelEdgeThreshold.Location = new System.Drawing.Point(13, 13);
            this.labelEdgeThreshold.Name = "labelEdgeThreshold";
            this.labelEdgeThreshold.Size = new System.Drawing.Size(121, 20);
            this.labelEdgeThreshold.TabIndex = 0;
            this.labelEdgeThreshold.Text = "Edge Threshold";
            // 
            // edgeThreshold
            // 
            this.edgeThreshold.Location = new System.Drawing.Point(251, 11);
            this.edgeThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.edgeThreshold.Name = "edgeThreshold";
            this.edgeThreshold.Size = new System.Drawing.Size(62, 26);
            this.edgeThreshold.TabIndex = 1;
            this.edgeThreshold.ValueChanged += new System.EventHandler(this.edgeThreshold_ValueChanged);
            // 
            // maxPixelCount
            // 
            this.maxPixelCount.Location = new System.Drawing.Point(250, 45);
            this.maxPixelCount.Margin = new System.Windows.Forms.Padding(7);
            this.maxPixelCount.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.maxPixelCount.Name = "maxPixelCount";
            this.maxPixelCount.Size = new System.Drawing.Size(63, 26);
            this.maxPixelCount.TabIndex = 7;
            this.maxPixelCount.ValueChanged += new System.EventHandler(this.maxPixelCount_ValueChanged);
            // 
            // minPixelCount
            // 
            this.minPixelCount.Location = new System.Drawing.Point(152, 45);
            this.minPixelCount.Margin = new System.Windows.Forms.Padding(7);
            this.minPixelCount.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.minPixelCount.Name = "minPixelCount";
            this.minPixelCount.Size = new System.Drawing.Size(63, 26);
            this.minPixelCount.TabIndex = 8;
            this.minPixelCount.ValueChanged += new System.EventHandler(this.minPixelCount_ValueChanged);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(224, 54);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(18, 20);
            this.labelTilda.TabIndex = 9;
            this.labelTilda.Text = "~";
            // 
            // labelPixelCount
            // 
            this.labelPixelCount.AutoSize = true;
            this.labelPixelCount.Location = new System.Drawing.Point(13, 47);
            this.labelPixelCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPixelCount.Name = "labelPixelCount";
            this.labelPixelCount.Size = new System.Drawing.Size(88, 20);
            this.labelPixelCount.TabIndex = 6;
            this.labelPixelCount.Text = "Pixel Count";
            // 
            // maskImageSelector
            // 
            this.maskImageSelector.AllowUserToAddRows = false;
            this.maskImageSelector.AllowUserToDeleteRows = false;
            this.maskImageSelector.AllowUserToResizeColumns = false;
            this.maskImageSelector.AllowUserToResizeRows = false;
            this.maskImageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.maskImageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.maskImageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.maskImageSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnMaskImage});
            this.maskImageSelector.Location = new System.Drawing.Point(12, 118);
            this.maskImageSelector.Margin = new System.Windows.Forms.Padding(2);
            this.maskImageSelector.MultiSelect = false;
            this.maskImageSelector.Name = "maskImageSelector";
            this.maskImageSelector.ReadOnly = true;
            this.maskImageSelector.RowHeadersVisible = false;
            this.maskImageSelector.RowTemplate.Height = 23;
            this.maskImageSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.maskImageSelector.Size = new System.Drawing.Size(203, 338);
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
            this.deleteMaskButton.Location = new System.Drawing.Point(219, 174);
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
            this.refreshMaskButton.Location = new System.Drawing.Point(219, 147);
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
            this.addMaskButton.Location = new System.Drawing.Point(219, 119);
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
            this.labelAreaMinName.Location = new System.Drawing.Point(13, 84);
            this.labelAreaMinName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAreaMinName.Name = "labelAreaMinName";
            this.labelAreaMinName.Size = new System.Drawing.Size(72, 20);
            this.labelAreaMinName.TabIndex = 14;
            this.labelAreaMinName.Text = "Area Min";
            // 
            // numAreaMin
            // 
            this.numAreaMin.Location = new System.Drawing.Point(250, 82);
            this.numAreaMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numAreaMin.Name = "numAreaMin";
            this.numAreaMin.Size = new System.Drawing.Size(62, 26);
            this.numAreaMin.TabIndex = 15;
            this.numAreaMin.ValueChanged += new System.EventHandler(this.numAreaMin_ValueChanged);
            // 
            // BlobSubtractorParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numAreaMin);
            this.Controls.Add(this.labelAreaMinName);
            this.Controls.Add(this.maskImageSelector);
            this.Controls.Add(this.deleteMaskButton);
            this.Controls.Add(this.refreshMaskButton);
            this.Controls.Add(this.addMaskButton);
            this.Controls.Add(this.maxPixelCount);
            this.Controls.Add(this.minPixelCount);
            this.Controls.Add(this.labelTilda);
            this.Controls.Add(this.labelPixelCount);
            this.Controls.Add(this.labelEdgeThreshold);
            this.Controls.Add(this.edgeThreshold);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BlobSubtractorParamControl";
            this.Size = new System.Drawing.Size(341, 567);
            ((System.ComponentModel.ISupportInitialize)(this.edgeThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPixelCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskImageSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelEdgeThreshold;
        private System.Windows.Forms.NumericUpDown edgeThreshold;
        private System.Windows.Forms.NumericUpDown maxPixelCount;
        private System.Windows.Forms.NumericUpDown minPixelCount;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.Label labelPixelCount;
        private System.Windows.Forms.DataGridView maskImageSelector;
        private System.Windows.Forms.DataGridViewImageColumn ColumnMaskImage;
        private System.Windows.Forms.Button deleteMaskButton;
        private System.Windows.Forms.Button refreshMaskButton;
        private System.Windows.Forms.Button addMaskButton;
        private System.Windows.Forms.Label labelAreaMinName;
        private System.Windows.Forms.NumericUpDown numAreaMin;
    }
}
