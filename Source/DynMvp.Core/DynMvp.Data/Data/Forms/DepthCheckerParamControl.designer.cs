namespace DynMvp.Data.Forms
{
    partial class DepthCheckerParamControl
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
            this.labelValueRange = new System.Windows.Forms.Label();
            this.labelMeasureType = new System.Windows.Forms.Label();
            this.comboBoxMeasureType = new System.Windows.Forms.ComboBox();
            this.buttonGetDepthValue = new System.Windows.Forms.Button();
            this.marginPercent = new System.Windows.Forms.NumericUpDown();
            this.lowerValue = new System.Windows.Forms.NumericUpDown();
            this.upperValue = new System.Windows.Forms.NumericUpDown();
            this.labelMinValue = new System.Windows.Forms.Label();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.labelMaxValue = new System.Windows.Forms.Label();
            this.labelPct = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.marginPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowerValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperValue)).BeginInit();
            this.SuspendLayout();
            // 
            // labelValueRange
            // 
            this.labelValueRange.AutoSize = true;
            this.labelValueRange.Location = new System.Drawing.Point(11, 44);
            this.labelValueRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelValueRange.Name = "labelValueRange";
            this.labelValueRange.Size = new System.Drawing.Size(102, 20);
            this.labelValueRange.TabIndex = 0;
            this.labelValueRange.Text = "Value Range";
            // 
            // labelMeasureType
            // 
            this.labelMeasureType.AutoSize = true;
            this.labelMeasureType.Location = new System.Drawing.Point(11, 12);
            this.labelMeasureType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMeasureType.Name = "labelMeasureType";
            this.labelMeasureType.Size = new System.Drawing.Size(109, 20);
            this.labelMeasureType.TabIndex = 0;
            this.labelMeasureType.Text = "Measure Type";
            // 
            // comboBoxMeasureType
            // 
            this.comboBoxMeasureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMeasureType.FormattingEnabled = true;
            this.comboBoxMeasureType.Items.AddRange(new object[] {
            "None",
            "Height(Average)",
            "Height(Max)",
            "Height(Min)",
            "Volume"});
            this.comboBoxMeasureType.Location = new System.Drawing.Point(148, 9);
            this.comboBoxMeasureType.Name = "comboBoxMeasureType";
            this.comboBoxMeasureType.Size = new System.Drawing.Size(174, 28);
            this.comboBoxMeasureType.TabIndex = 4;
            this.comboBoxMeasureType.SelectedIndexChanged += new System.EventHandler(this.comboBoxMeasureType_SelectedIndexChanged);
            // 
            // buttonGetDepthValue
            // 
            this.buttonGetDepthValue.Location = new System.Drawing.Point(148, 101);
            this.buttonGetDepthValue.Name = "buttonGetDepthValue";
            this.buttonGetDepthValue.Size = new System.Drawing.Size(58, 26);
            this.buttonGetDepthValue.TabIndex = 5;
            this.buttonGetDepthValue.Text = "Auto";
            this.buttonGetDepthValue.UseVisualStyleBackColor = true;
            this.buttonGetDepthValue.Click += new System.EventHandler(this.buttonGetDepthValue_Click);
            // 
            // marginPercent
            // 
            this.marginPercent.Location = new System.Drawing.Point(212, 102);
            this.marginPercent.Name = "marginPercent";
            this.marginPercent.Size = new System.Drawing.Size(69, 26);
            this.marginPercent.TabIndex = 7;
            // 
            // lowerValue
            // 
            this.lowerValue.DecimalPlaces = 2;
            this.lowerValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.lowerValue.Location = new System.Drawing.Point(185, 42);
            this.lowerValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lowerValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.lowerValue.Name = "lowerValue";
            this.lowerValue.Size = new System.Drawing.Size(96, 26);
            this.lowerValue.TabIndex = 7;
            this.lowerValue.ValueChanged += new System.EventHandler(this.lowerValue_ValueChanged);
            // 
            // upperValue
            // 
            this.upperValue.DecimalPlaces = 2;
            this.upperValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.upperValue.Location = new System.Drawing.Point(185, 72);
            this.upperValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.upperValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.upperValue.Name = "upperValue";
            this.upperValue.Size = new System.Drawing.Size(96, 26);
            this.upperValue.TabIndex = 7;
            this.upperValue.ValueChanged += new System.EventHandler(this.upperValue_ValueChanged);
            // 
            // labelMinValue
            // 
            this.labelMinValue.AutoSize = true;
            this.labelMinValue.Location = new System.Drawing.Point(144, 44);
            this.labelMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinValue.Name = "labelMinValue";
            this.labelMinValue.Size = new System.Drawing.Size(34, 20);
            this.labelMinValue.TabIndex = 0;
            this.labelMinValue.Text = "Min";
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.Location = new System.Drawing.Point(288, 44);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(35, 20);
            this.labelMinUnit.TabIndex = 0;
            this.labelMinUnit.Text = "mm";
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.Location = new System.Drawing.Point(288, 74);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(35, 20);
            this.labelMaxUnit.TabIndex = 0;
            this.labelMaxUnit.Text = "mm";
            // 
            // labelMaxValue
            // 
            this.labelMaxValue.AutoSize = true;
            this.labelMaxValue.Location = new System.Drawing.Point(144, 74);
            this.labelMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxValue.Name = "labelMaxValue";
            this.labelMaxValue.Size = new System.Drawing.Size(34, 20);
            this.labelMaxValue.TabIndex = 0;
            this.labelMaxValue.Text = "Min";
            // 
            // labelPct
            // 
            this.labelPct.AutoSize = true;
            this.labelPct.Location = new System.Drawing.Point(288, 104);
            this.labelPct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPct.Name = "labelPct";
            this.labelPct.Size = new System.Drawing.Size(23, 20);
            this.labelPct.TabIndex = 0;
            this.labelPct.Text = "%";
            // 
            // DepthCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.upperValue);
            this.Controls.Add(this.lowerValue);
            this.Controls.Add(this.marginPercent);
            this.Controls.Add(this.buttonGetDepthValue);
            this.Controls.Add(this.comboBoxMeasureType);
            this.Controls.Add(this.labelMeasureType);
            this.Controls.Add(this.labelPct);
            this.Controls.Add(this.labelMaxUnit);
            this.Controls.Add(this.labelMinUnit);
            this.Controls.Add(this.labelMaxValue);
            this.Controls.Add(this.labelMinValue);
            this.Controls.Add(this.labelValueRange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DepthCheckerParamControl";
            this.Size = new System.Drawing.Size(361, 238);
            ((System.ComponentModel.ISupportInitialize)(this.marginPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowerValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelValueRange;
        private System.Windows.Forms.Label labelMeasureType;
        private System.Windows.Forms.ComboBox comboBoxMeasureType;
        private System.Windows.Forms.Button buttonGetDepthValue;
        private System.Windows.Forms.NumericUpDown marginPercent;
        private System.Windows.Forms.NumericUpDown lowerValue;
        private System.Windows.Forms.NumericUpDown upperValue;
        private System.Windows.Forms.Label labelMinValue;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.Label labelMaxValue;
        private System.Windows.Forms.Label labelPct;
    }
}