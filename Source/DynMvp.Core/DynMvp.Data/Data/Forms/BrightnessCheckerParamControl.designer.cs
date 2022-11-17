namespace DynMvp.Data.Forms
{
    partial class BrightnessCheckerParamControl
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
            this.labelBrightnessRange = new System.Windows.Forms.Label();
            this.lowerValue = new System.Windows.Forms.NumericUpDown();
            this.upperValue = new System.Windows.Forms.NumericUpDown();
            this.labelTilda = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lowerValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperValue)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBrightnessRange
            // 
            this.labelBrightnessRange.AutoSize = true;
            this.labelBrightnessRange.Location = new System.Drawing.Point(10, 10);
            this.labelBrightnessRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBrightnessRange.Name = "labelBrightnessRange";
            this.labelBrightnessRange.Size = new System.Drawing.Size(137, 20);
            this.labelBrightnessRange.TabIndex = 0;
            this.labelBrightnessRange.Text = "Brightness Range";
            // 
            // lowerValue
            // 
            this.lowerValue.Location = new System.Drawing.Point(147, 8);
            this.lowerValue.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.lowerValue.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.lowerValue.Name = "lowerValue";
            this.lowerValue.Size = new System.Drawing.Size(59, 26);
            this.lowerValue.TabIndex = 1;
            this.lowerValue.ValueChanged += new System.EventHandler(this.lowerValue_ValueChanged);
            this.lowerValue.Enter += new System.EventHandler(this.textBox_Enter);
            this.lowerValue.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // upperValue
            // 
            this.upperValue.Location = new System.Drawing.Point(235, 8);
            this.upperValue.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.upperValue.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.upperValue.Name = "upperValue";
            this.upperValue.Size = new System.Drawing.Size(59, 26);
            this.upperValue.TabIndex = 3;
            this.upperValue.ValueChanged += new System.EventHandler(this.upperValue_ValueChanged);
            this.upperValue.Enter += new System.EventHandler(this.textBox_Enter);
            this.upperValue.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(212, 10);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(18, 20);
            this.labelTilda.TabIndex = 2;
            this.labelTilda.Text = "~";
            // 
            // BrightnessCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.upperValue);
            this.Controls.Add(this.lowerValue);
            this.Controls.Add(this.labelTilda);
            this.Controls.Add(this.labelBrightnessRange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BrightnessCheckerParamControl";
            this.Size = new System.Drawing.Size(340, 82);
            ((System.ComponentModel.ISupportInitialize)(this.lowerValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBrightnessRange;
        private System.Windows.Forms.NumericUpDown lowerValue;
        private System.Windows.Forms.NumericUpDown upperValue;
        private System.Windows.Forms.Label labelTilda;

    }
}