namespace DynMvp.Vision.UI
{
    partial class FontView
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
            this.fontGrid = new System.Windows.Forms.DataGridView();
            this.okButton = new System.Windows.Forms.Button();
            this.deleteFontButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // fontGrid
            // 
            this.fontGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fontGrid.Location = new System.Drawing.Point(2, 3);
            this.fontGrid.Name = "fontGrid";
            this.fontGrid.RowTemplate.Height = 60;
            this.fontGrid.Size = new System.Drawing.Size(637, 455);
            this.fontGrid.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(525, 462);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(114, 45);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // deleteFontButton
            // 
            this.deleteFontButton.Location = new System.Drawing.Point(2, 462);
            this.deleteFontButton.Name = "deleteFontButton";
            this.deleteFontButton.Size = new System.Drawing.Size(114, 45);
            this.deleteFontButton.TabIndex = 1;
            this.deleteFontButton.Text = "Delete";
            this.deleteFontButton.UseVisualStyleBackColor = true;
            this.deleteFontButton.Click += new System.EventHandler(this.deleteFontButton_Click);
            // 
            // FontView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 511);
            this.Controls.Add(this.deleteFontButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fontGrid);
            this.Name = "FontView";
            this.Text = "FontView";
            this.Load += new System.EventHandler(this.FontView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fontGrid;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button deleteFontButton;
    }
}