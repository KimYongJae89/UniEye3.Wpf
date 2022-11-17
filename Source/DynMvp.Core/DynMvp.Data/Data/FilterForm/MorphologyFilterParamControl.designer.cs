namespace DynMvp.Data.FilterForm
{
    partial class MorphologyFilterParamControl
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
            this.labelMorphologyType = new System.Windows.Forms.Label();
            this.numIteration = new System.Windows.Forms.NumericUpDown();
            this.morphologyType = new System.Windows.Forms.ComboBox();
            this.labelNumIteration = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numIteration)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMorphologyType
            // 
            this.labelMorphologyType.AutoSize = true;
            this.labelMorphologyType.Location = new System.Drawing.Point(8, 10);
            this.labelMorphologyType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMorphologyType.Name = "labelMorphologyType";
            this.labelMorphologyType.Size = new System.Drawing.Size(129, 20);
            this.labelMorphologyType.TabIndex = 0;
            this.labelMorphologyType.Text = "Morphology Type";
            // 
            // numIteration
            // 
            this.numIteration.Location = new System.Drawing.Point(145, 41);
            this.numIteration.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numIteration.Name = "numIteration";
            this.numIteration.Size = new System.Drawing.Size(75, 26);
            this.numIteration.TabIndex = 14;
            this.numIteration.ValueChanged += new System.EventHandler(this.numIteration_ValueChanged);
            // 
            // morphologyType
            // 
            this.morphologyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.morphologyType.FormattingEnabled = true;
            this.morphologyType.Location = new System.Drawing.Point(145, 7);
            this.morphologyType.Name = "morphologyType";
            this.morphologyType.Size = new System.Drawing.Size(186, 28);
            this.morphologyType.TabIndex = 18;
            this.morphologyType.SelectedIndexChanged += new System.EventHandler(this.morphologyType_SelectedIndexChanged);
            // 
            // labelNumIteration
            // 
            this.labelNumIteration.AutoSize = true;
            this.labelNumIteration.Location = new System.Drawing.Point(8, 41);
            this.labelNumIteration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumIteration.Name = "labelNumIteration";
            this.labelNumIteration.Size = new System.Drawing.Size(105, 20);
            this.labelNumIteration.TabIndex = 0;
            this.labelNumIteration.Text = "Num Iteration";
            // 
            // MorphologyFilterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.morphologyType);
            this.Controls.Add(this.numIteration);
            this.Controls.Add(this.labelNumIteration);
            this.Controls.Add(this.labelMorphologyType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MorphologyFilterParamControl";
            this.Size = new System.Drawing.Size(340, 128);
            ((System.ComponentModel.ISupportInitialize)(this.numIteration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMorphologyType;
        private System.Windows.Forms.NumericUpDown numIteration;
        private System.Windows.Forms.ComboBox morphologyType;
        private System.Windows.Forms.Label labelNumIteration;
    }
}