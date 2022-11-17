namespace DynMvp.Data.FilterForm
{
    partial class NoParamFilterParamControl
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
            this.labelNoParameter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNoParameter
            // 
            this.labelNoParameter.AutoSize = true;
            this.labelNoParameter.Location = new System.Drawing.Point(10, 10);
            this.labelNoParameter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNoParameter.Name = "labelNoParameter";
            this.labelNoParameter.Size = new System.Drawing.Size(107, 20);
            this.labelNoParameter.TabIndex = 0;
            this.labelNoParameter.Text = "No Parameter";
            // 
            // NoParamFilterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.labelNoParameter);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NoParamFilterParamControl";
            this.Size = new System.Drawing.Size(340, 82);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNoParameter;

    }
}