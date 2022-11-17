namespace UniEye.Base.UI.CameraCalibration
{
    partial class CalibrationConstant
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
            this.pelHeight = new System.Windows.Forms.TextBox();
            this.pelWidth = new System.Windows.Forms.TextBox();
            this.labelScaleY = new System.Windows.Forms.Label();
            this.labelScaleX = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pelHeight
            // 
            this.pelHeight.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pelHeight.Location = new System.Drawing.Point(120, 69);
            this.pelHeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pelHeight.Name = "pelHeight";
            this.pelHeight.Size = new System.Drawing.Size(105, 25);
            this.pelHeight.TabIndex = 14;
            // 
            // pelWidth
            // 
            this.pelWidth.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pelWidth.Location = new System.Drawing.Point(120, 25);
            this.pelWidth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pelWidth.Name = "pelWidth";
            this.pelWidth.Size = new System.Drawing.Size(105, 25);
            this.pelWidth.TabIndex = 15;
            // 
            // labelScaleY
            // 
            this.labelScaleY.AutoSize = true;
            this.labelScaleY.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelScaleY.Location = new System.Drawing.Point(16, 72);
            this.labelScaleY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleY.Name = "labelScaleY";
            this.labelScaleY.Size = new System.Drawing.Size(57, 19);
            this.labelScaleY.TabIndex = 12;
            this.labelScaleY.Text = "Scale Y";
            // 
            // labelScaleX
            // 
            this.labelScaleX.AutoSize = true;
            this.labelScaleX.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelScaleX.Location = new System.Drawing.Point(16, 28);
            this.labelScaleX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleX.Name = "labelScaleX";
            this.labelScaleX.Size = new System.Drawing.Size(58, 19);
            this.labelScaleX.TabIndex = 13;
            this.labelScaleX.Text = "Scale X";
            // 
            // CalibrationConstant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pelHeight);
            this.Controls.Add(this.pelWidth);
            this.Controls.Add(this.labelScaleY);
            this.Controls.Add(this.labelScaleX);
            this.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CalibrationConstant";
            this.Size = new System.Drawing.Size(957, 260);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pelHeight;
        private System.Windows.Forms.TextBox pelWidth;
        private System.Windows.Forms.Label labelScaleY;
        private System.Windows.Forms.Label labelScaleX;
    }
}
