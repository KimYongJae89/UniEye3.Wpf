namespace DynMvp.Data.Forms
{
    partial class EdgeCheckerParamControl
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
            this.maxOffset = new System.Windows.Forms.NumericUpDown();
            this.labelMaxOffset = new System.Windows.Forms.Label();
            this.comboEdgeDirection = new System.Windows.Forms.ComboBox();
            this.labelEdgeDirection = new System.Windows.Forms.Label();
            this.labelDesiredOffset = new System.Windows.Forms.Label();
            this.desiredOffset = new System.Windows.Forms.NumericUpDown();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.threshold = new System.Windows.Forms.NumericUpDown();
            this.labelGaussianFilterSize = new System.Windows.Forms.Label();
            this.gaussianFilterSize = new System.Windows.Forms.NumericUpDown();
            this.comboEdgeType = new System.Windows.Forms.ComboBox();
            this.labelEdgeType = new System.Windows.Forms.Label();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.medianFilterSize = new System.Windows.Forms.NumericUpDown();
            this.labelMedianFilterSize = new System.Windows.Forms.Label();
            this.labelMorphology = new System.Windows.Forms.Label();
            this.morphologyFilterSize = new System.Windows.Forms.NumericUpDown();
            this.buttonFind = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelAverageCount = new System.Windows.Forms.Label();
            this.averageCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desiredOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gaussianFilterSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medianFilterSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.morphologyFilterSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.averageCount)).BeginInit();
            this.SuspendLayout();
            // 
            // maxOffset
            // 
            this.maxOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxOffset.DecimalPlaces = 2;
            this.maxOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.maxOffset.Location = new System.Drawing.Point(242, 43);
            this.maxOffset.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.maxOffset.Name = "maxOffset";
            this.maxOffset.Size = new System.Drawing.Size(74, 24);
            this.maxOffset.TabIndex = 14;
            this.maxOffset.ValueChanged += new System.EventHandler(this.maxOffset_ValueChanged);
            // 
            // labelMaxOffset
            // 
            this.labelMaxOffset.AutoSize = true;
            this.labelMaxOffset.Location = new System.Drawing.Point(15, 45);
            this.labelMaxOffset.Name = "labelMaxOffset";
            this.labelMaxOffset.Size = new System.Drawing.Size(80, 18);
            this.labelMaxOffset.TabIndex = 13;
            this.labelMaxOffset.Text = "Max Offset";
            // 
            // comboEdgeDirection
            // 
            this.comboEdgeDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEdgeDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEdgeDirection.FormattingEnabled = true;
            this.comboEdgeDirection.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.comboEdgeDirection.Location = new System.Drawing.Point(169, 303);
            this.comboEdgeDirection.Margin = new System.Windows.Forms.Padding(4);
            this.comboEdgeDirection.Name = "comboEdgeDirection";
            this.comboEdgeDirection.Size = new System.Drawing.Size(152, 26);
            this.comboEdgeDirection.TabIndex = 2;
            this.comboEdgeDirection.Visible = false;
            this.comboEdgeDirection.SelectedIndexChanged += new System.EventHandler(this.comboEdgeDirection_SelectedIndexChanged);
            // 
            // labelEdgeDirection
            // 
            this.labelEdgeDirection.AutoSize = true;
            this.labelEdgeDirection.Location = new System.Drawing.Point(15, 306);
            this.labelEdgeDirection.Name = "labelEdgeDirection";
            this.labelEdgeDirection.Size = new System.Drawing.Size(105, 18);
            this.labelEdgeDirection.TabIndex = 0;
            this.labelEdgeDirection.Text = "Edge Direction";
            this.labelEdgeDirection.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelEdgeDirection.Visible = false;
            // 
            // labelDesiredOffset
            // 
            this.labelDesiredOffset.AutoSize = true;
            this.labelDesiredOffset.Location = new System.Drawing.Point(15, 15);
            this.labelDesiredOffset.Name = "labelDesiredOffset";
            this.labelDesiredOffset.Size = new System.Drawing.Size(103, 18);
            this.labelDesiredOffset.TabIndex = 13;
            this.labelDesiredOffset.Text = "Desired Offset";
            // 
            // desiredOffset
            // 
            this.desiredOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.desiredOffset.Location = new System.Drawing.Point(242, 13);
            this.desiredOffset.Maximum = new decimal(new int[] {
            1944,
            0,
            0,
            0});
            this.desiredOffset.Name = "desiredOffset";
            this.desiredOffset.Size = new System.Drawing.Size(74, 24);
            this.desiredOffset.TabIndex = 14;
            this.desiredOffset.ValueChanged += new System.EventHandler(this.desiredOffset_ValueChanged);
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(15, 75);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(74, 18);
            this.labelThreshold.TabIndex = 13;
            this.labelThreshold.Text = "Threshold";
            // 
            // threshold
            // 
            this.threshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.threshold.Location = new System.Drawing.Point(242, 73);
            this.threshold.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(74, 24);
            this.threshold.TabIndex = 14;
            this.threshold.ValueChanged += new System.EventHandler(this.edgeThreshold_ValueChanged);
            // 
            // labelGaussianFilterSize
            // 
            this.labelGaussianFilterSize.AutoSize = true;
            this.labelGaussianFilterSize.Location = new System.Drawing.Point(15, 371);
            this.labelGaussianFilterSize.Name = "labelGaussianFilterSize";
            this.labelGaussianFilterSize.Size = new System.Drawing.Size(140, 18);
            this.labelGaussianFilterSize.TabIndex = 13;
            this.labelGaussianFilterSize.Text = "Gaussian Filter Size";
            this.labelGaussianFilterSize.Visible = false;
            // 
            // gaussianFilterSize
            // 
            this.gaussianFilterSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gaussianFilterSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.gaussianFilterSize.Location = new System.Drawing.Point(247, 369);
            this.gaussianFilterSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.gaussianFilterSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gaussianFilterSize.Name = "gaussianFilterSize";
            this.gaussianFilterSize.Size = new System.Drawing.Size(74, 24);
            this.gaussianFilterSize.TabIndex = 14;
            this.gaussianFilterSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gaussianFilterSize.Visible = false;
            this.gaussianFilterSize.ValueChanged += new System.EventHandler(this.filterSize_ValueChanged);
            // 
            // comboEdgeType
            // 
            this.comboEdgeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEdgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEdgeType.FormattingEnabled = true;
            this.comboEdgeType.Items.AddRange(new object[] {
            "DarkToLight",
            "LightToDark",
            "Any"});
            this.comboEdgeType.Location = new System.Drawing.Point(169, 337);
            this.comboEdgeType.Margin = new System.Windows.Forms.Padding(4);
            this.comboEdgeType.Name = "comboEdgeType";
            this.comboEdgeType.Size = new System.Drawing.Size(152, 26);
            this.comboEdgeType.TabIndex = 16;
            this.comboEdgeType.Visible = false;
            // 
            // labelEdgeType
            // 
            this.labelEdgeType.AutoSize = true;
            this.labelEdgeType.Location = new System.Drawing.Point(15, 340);
            this.labelEdgeType.Name = "labelEdgeType";
            this.labelEdgeType.Size = new System.Drawing.Size(78, 18);
            this.labelEdgeType.TabIndex = 15;
            this.labelEdgeType.Text = "Edge Type";
            this.labelEdgeType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelEdgeType.Visible = false;
            // 
            // buttonCenter
            // 
            this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCenter.Location = new System.Drawing.Point(161, 12);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(75, 25);
            this.buttonCenter.TabIndex = 17;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // medianFilterSize
            // 
            this.medianFilterSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.medianFilterSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.medianFilterSize.Location = new System.Drawing.Point(247, 400);
            this.medianFilterSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.medianFilterSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.medianFilterSize.Name = "medianFilterSize";
            this.medianFilterSize.Size = new System.Drawing.Size(74, 24);
            this.medianFilterSize.TabIndex = 19;
            this.medianFilterSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.medianFilterSize.Visible = false;
            this.medianFilterSize.ValueChanged += new System.EventHandler(this.medianFilterSize_ValueChanged);
            // 
            // labelMedianFilterSize
            // 
            this.labelMedianFilterSize.AutoSize = true;
            this.labelMedianFilterSize.Location = new System.Drawing.Point(15, 402);
            this.labelMedianFilterSize.Name = "labelMedianFilterSize";
            this.labelMedianFilterSize.Size = new System.Drawing.Size(125, 18);
            this.labelMedianFilterSize.TabIndex = 18;
            this.labelMedianFilterSize.Text = "Median Filter Size";
            this.labelMedianFilterSize.Visible = false;
            // 
            // labelMorphology
            // 
            this.labelMorphology.AutoSize = true;
            this.labelMorphology.Location = new System.Drawing.Point(15, 105);
            this.labelMorphology.Name = "labelMorphology";
            this.labelMorphology.Size = new System.Drawing.Size(87, 18);
            this.labelMorphology.TabIndex = 13;
            this.labelMorphology.Text = "Morphology";
            // 
            // morphologyFilterSize
            // 
            this.morphologyFilterSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.morphologyFilterSize.Location = new System.Drawing.Point(242, 103);
            this.morphologyFilterSize.Name = "morphologyFilterSize";
            this.morphologyFilterSize.Size = new System.Drawing.Size(74, 24);
            this.morphologyFilterSize.TabIndex = 14;
            this.morphologyFilterSize.ValueChanged += new System.EventHandler(this.morphologyFilterSize_ValueChanged);
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(161, 72);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 25);
            this.buttonFind.TabIndex = 17;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "(mm)";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "(px)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 18);
            this.label3.TabIndex = 21;
            this.label3.Text = "(px)";
            // 
            // labelAverageCount
            // 
            this.labelAverageCount.AutoSize = true;
            this.labelAverageCount.Location = new System.Drawing.Point(15, 135);
            this.labelAverageCount.Name = "labelAverageCount";
            this.labelAverageCount.Size = new System.Drawing.Size(105, 18);
            this.labelAverageCount.TabIndex = 13;
            this.labelAverageCount.Text = "Average Count";
            // 
            // averageCount
            // 
            this.averageCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.averageCount.Location = new System.Drawing.Point(242, 133);
            this.averageCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.averageCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.averageCount.Name = "averageCount";
            this.averageCount.Size = new System.Drawing.Size(74, 24);
            this.averageCount.TabIndex = 14;
            this.averageCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.averageCount.ValueChanged += new System.EventHandler(this.AverageCount_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(322, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 18);
            this.label5.TabIndex = 21;
            this.label5.Text = "(count)";
            // 
            // EdgeCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.medianFilterSize);
            this.Controls.Add(this.labelMedianFilterSize);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.comboEdgeType);
            this.Controls.Add(this.labelEdgeType);
            this.Controls.Add(this.comboEdgeDirection);
            this.Controls.Add(this.labelEdgeDirection);
            this.Controls.Add(this.desiredOffset);
            this.Controls.Add(this.gaussianFilterSize);
            this.Controls.Add(this.averageCount);
            this.Controls.Add(this.morphologyFilterSize);
            this.Controls.Add(this.threshold);
            this.Controls.Add(this.maxOffset);
            this.Controls.Add(this.labelGaussianFilterSize);
            this.Controls.Add(this.labelDesiredOffset);
            this.Controls.Add(this.labelAverageCount);
            this.Controls.Add(this.labelMorphology);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.labelMaxOffset);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EdgeCheckerParamControl";
            this.Size = new System.Drawing.Size(377, 432);
            ((System.ComponentModel.ISupportInitialize)(this.maxOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desiredOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gaussianFilterSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medianFilterSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.morphologyFilterSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.averageCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown maxOffset;
        private System.Windows.Forms.Label labelMaxOffset;
        private System.Windows.Forms.ComboBox comboEdgeDirection;
        private System.Windows.Forms.Label labelEdgeDirection;
        private System.Windows.Forms.Label labelDesiredOffset;
        private System.Windows.Forms.NumericUpDown desiredOffset;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.NumericUpDown threshold;
        private System.Windows.Forms.Label labelGaussianFilterSize;
        private System.Windows.Forms.NumericUpDown gaussianFilterSize;
        private System.Windows.Forms.ComboBox comboEdgeType;
        private System.Windows.Forms.Label labelEdgeType;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.NumericUpDown medianFilterSize;
        private System.Windows.Forms.Label labelMedianFilterSize;
        private System.Windows.Forms.Label labelMorphology;
        private System.Windows.Forms.NumericUpDown morphologyFilterSize;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelAverageCount;
        private System.Windows.Forms.NumericUpDown averageCount;
        private System.Windows.Forms.Label label5;
    }
}
