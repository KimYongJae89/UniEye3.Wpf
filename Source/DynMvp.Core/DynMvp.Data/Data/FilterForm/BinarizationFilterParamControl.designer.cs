namespace DynMvp.Data.FilterForm
{
    partial class BinarizationFilterParamControl
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
            this.components = new System.ComponentModel.Container();
            this.labelBinarizationType = new System.Windows.Forms.Label();
            this.thresholdLower = new System.Windows.Forms.NumericUpDown();
            this.thresholdUpper = new System.Windows.Forms.NumericUpDown();
            this.binarizationType = new System.Windows.Forms.ComboBox();
            this.labelThresholdRange = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBoxInvert = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBinarizationType
            // 
            this.labelBinarizationType.AutoSize = true;
            this.labelBinarizationType.Location = new System.Drawing.Point(8, 10);
            this.labelBinarizationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBinarizationType.Name = "labelBinarizationType";
            this.labelBinarizationType.Size = new System.Drawing.Size(130, 20);
            this.labelBinarizationType.TabIndex = 0;
            this.labelBinarizationType.Text = "Binarization Type";
            // 
            // thresholdLower
            // 
            this.thresholdLower.Location = new System.Drawing.Point(34, 90);
            this.thresholdLower.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdLower.Name = "thresholdLower";
            this.thresholdLower.Size = new System.Drawing.Size(75, 26);
            this.thresholdLower.TabIndex = 14;
            this.thresholdLower.ValueChanged += new System.EventHandler(this.thresholdLower_ValueChanged);
            this.thresholdLower.Validating += new System.ComponentModel.CancelEventHandler(this.thresholdLower_Validating);
            // 
            // thresholdUpper
            // 
            this.thresholdUpper.Enabled = false;
            this.thresholdUpper.Location = new System.Drawing.Point(145, 90);
            this.thresholdUpper.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdUpper.Name = "thresholdUpper";
            this.thresholdUpper.Size = new System.Drawing.Size(75, 26);
            this.thresholdUpper.TabIndex = 13;
            this.thresholdUpper.ValueChanged += new System.EventHandler(this.thresholdUpper_ValueChanged);
            this.thresholdUpper.Validating += new System.ComponentModel.CancelEventHandler(this.thresholdUpper_Validating);
            // 
            // binarizationType
            // 
            this.binarizationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.binarizationType.FormattingEnabled = true;
            this.binarizationType.Location = new System.Drawing.Point(34, 33);
            this.binarizationType.Name = "binarizationType";
            this.binarizationType.Size = new System.Drawing.Size(186, 28);
            this.binarizationType.TabIndex = 18;
            this.binarizationType.SelectedIndexChanged += new System.EventHandler(this.binarizationType_SelectedIndexChanged);
            // 
            // labelThresholdRange
            // 
            this.labelThresholdRange.AutoSize = true;
            this.labelThresholdRange.Location = new System.Drawing.Point(7, 67);
            this.labelThresholdRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThresholdRange.Name = "labelThresholdRange";
            this.labelThresholdRange.Size = new System.Drawing.Size(131, 20);
            this.labelThresholdRange.TabIndex = 0;
            this.labelThresholdRange.Text = "Threshold Range";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // checkBoxInvert
            // 
            this.checkBoxInvert.AutoSize = true;
            this.checkBoxInvert.Location = new System.Drawing.Point(235, 35);
            this.checkBoxInvert.Name = "checkBoxInvert";
            this.checkBoxInvert.Size = new System.Drawing.Size(68, 24);
            this.checkBoxInvert.TabIndex = 19;
            this.checkBoxInvert.Text = "Invert";
            this.checkBoxInvert.UseVisualStyleBackColor = true;
            this.checkBoxInvert.CheckedChanged += new System.EventHandler(this.checkBoxInvert_CheckedChanged);
            // 
            // BinarizationFilterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.checkBoxInvert);
            this.Controls.Add(this.binarizationType);
            this.Controls.Add(this.thresholdLower);
            this.Controls.Add(this.thresholdUpper);
            this.Controls.Add(this.labelThresholdRange);
            this.Controls.Add(this.labelBinarizationType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BinarizationFilterParamControl";
            this.Size = new System.Drawing.Size(317, 133);
            ((System.ComponentModel.ISupportInitialize)(this.thresholdLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBinarizationType;
        private System.Windows.Forms.NumericUpDown thresholdLower;
        private System.Windows.Forms.NumericUpDown thresholdUpper;
        private System.Windows.Forms.ComboBox binarizationType;
        private System.Windows.Forms.Label labelThresholdRange;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox checkBoxInvert;
    }
}