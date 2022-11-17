namespace DynMvp.Data.FilterForm
{
    partial class EdgeExtractionFilterParamControl
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
            this.labelKernelSize = new System.Windows.Forms.Label();
            this.kernelSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.kernelSize)).BeginInit();
            this.SuspendLayout();
            // 
            // labelKernelSize
            // 
            this.labelKernelSize.AutoSize = true;
            this.labelKernelSize.Location = new System.Drawing.Point(10, 10);
            this.labelKernelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKernelSize.Name = "labelKernelSize";
            this.labelKernelSize.Size = new System.Drawing.Size(89, 20);
            this.labelKernelSize.TabIndex = 0;
            this.labelKernelSize.Text = "Kernel Size";
            // 
            // kernelSize
            // 
            this.kernelSize.Location = new System.Drawing.Point(147, 8);
            this.kernelSize.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.kernelSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.kernelSize.Name = "kernelSize";
            this.kernelSize.Size = new System.Drawing.Size(59, 26);
            this.kernelSize.TabIndex = 1;
            this.kernelSize.ValueChanged += new System.EventHandler(this.lowerValue_ValueChanged);
            this.kernelSize.Enter += new System.EventHandler(this.textBox_Enter);
            this.kernelSize.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // BrightnessCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.kernelSize);
            this.Controls.Add(this.labelKernelSize);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BrightnessCheckerParamControl";
            this.Size = new System.Drawing.Size(340, 82);
            ((System.ComponentModel.ISupportInitialize)(this.kernelSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelKernelSize;
        private System.Windows.Forms.NumericUpDown kernelSize;

    }
}