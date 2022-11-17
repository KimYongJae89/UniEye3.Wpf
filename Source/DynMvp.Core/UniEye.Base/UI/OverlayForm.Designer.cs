namespace UniEye.Base.UI
{
    partial class OverlayForm
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
            this.buttonSelectImage2 = new System.Windows.Forms.Button();
            this.textBoxImage1Path = new System.Windows.Forms.TextBox();
            this.buttonSelectImage1 = new System.Windows.Forms.Button();
            this.textBoxImage2Path = new System.Windows.Forms.TextBox();
            this.panelImage = new System.Windows.Forms.Panel();
            this.buttonCam1 = new System.Windows.Forms.Button();
            this.buttonCam2 = new System.Windows.Forms.Button();
            this.panelCam = new System.Windows.Forms.Panel();
            this.txtCamera2 = new System.Windows.Forms.TextBox();
            this.txtCamera1 = new System.Windows.Forms.TextBox();
            this.buttonSetCam2 = new System.Windows.Forms.Button();
            this.buttonSetCam1 = new System.Windows.Forms.Button();
            this.panelImagePath = new System.Windows.Forms.Panel();
            this.panelCam.SuspendLayout();
            this.panelImagePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSelectImage2
            // 
            this.buttonSelectImage2.Location = new System.Drawing.Point(343, 37);
            this.buttonSelectImage2.Name = "buttonSelectImage2";
            this.buttonSelectImage2.Size = new System.Drawing.Size(94, 30);
            this.buttonSelectImage2.TabIndex = 0;
            this.buttonSelectImage2.Text = "Image 2";
            this.buttonSelectImage2.UseVisualStyleBackColor = true;
            this.buttonSelectImage2.Click += new System.EventHandler(this.buttonSelectImage2_Click);
            // 
            // textBoxImage1Path
            // 
            this.textBoxImage1Path.Location = new System.Drawing.Point(12, 10);
            this.textBoxImage1Path.Name = "textBoxImage1Path";
            this.textBoxImage1Path.Size = new System.Drawing.Size(325, 21);
            this.textBoxImage1Path.TabIndex = 1;
            // 
            // buttonSelectImage1
            // 
            this.buttonSelectImage1.Location = new System.Drawing.Point(343, 4);
            this.buttonSelectImage1.Name = "buttonSelectImage1";
            this.buttonSelectImage1.Size = new System.Drawing.Size(94, 30);
            this.buttonSelectImage1.TabIndex = 0;
            this.buttonSelectImage1.Text = "Image 1";
            this.buttonSelectImage1.UseVisualStyleBackColor = true;
            this.buttonSelectImage1.Click += new System.EventHandler(this.buttonSelectImage1_Click);
            // 
            // textBoxImage2Path
            // 
            this.textBoxImage2Path.Location = new System.Drawing.Point(12, 46);
            this.textBoxImage2Path.Name = "textBoxImage2Path";
            this.textBoxImage2Path.Size = new System.Drawing.Size(325, 21);
            this.textBoxImage2Path.TabIndex = 1;
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 148);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(596, 364);
            this.panelImage.TabIndex = 2;
            // 
            // buttonCam1
            // 
            this.buttonCam1.Location = new System.Drawing.Point(11, 8);
            this.buttonCam1.Name = "buttonCam1";
            this.buttonCam1.Size = new System.Drawing.Size(75, 23);
            this.buttonCam1.TabIndex = 0;
            this.buttonCam1.Text = "Camera1";
            this.buttonCam1.UseVisualStyleBackColor = true;
            this.buttonCam1.Click += new System.EventHandler(this.buttonCam1_Click);
            // 
            // buttonCam2
            // 
            this.buttonCam2.Location = new System.Drawing.Point(11, 37);
            this.buttonCam2.Name = "buttonCam2";
            this.buttonCam2.Size = new System.Drawing.Size(75, 23);
            this.buttonCam2.TabIndex = 1;
            this.buttonCam2.Text = "Camera2";
            this.buttonCam2.UseVisualStyleBackColor = true;
            this.buttonCam2.Click += new System.EventHandler(this.buttonCam2_Click);
            // 
            // panelCam
            // 
            this.panelCam.Controls.Add(this.buttonCam1);
            this.panelCam.Controls.Add(this.buttonCam2);
            this.panelCam.Controls.Add(this.txtCamera2);
            this.panelCam.Controls.Add(this.txtCamera1);
            this.panelCam.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCam.Location = new System.Drawing.Point(0, 0);
            this.panelCam.Name = "panelCam";
            this.panelCam.Size = new System.Drawing.Size(596, 70);
            this.panelCam.TabIndex = 2;
            // 
            // txtCamera2
            // 
            this.txtCamera2.Location = new System.Drawing.Point(92, 37);
            this.txtCamera2.Name = "txtCamera2";
            this.txtCamera2.Size = new System.Drawing.Size(361, 21);
            this.txtCamera2.TabIndex = 1;
            // 
            // txtCamera1
            // 
            this.txtCamera1.Location = new System.Drawing.Point(92, 10);
            this.txtCamera1.Name = "txtCamera1";
            this.txtCamera1.Size = new System.Drawing.Size(361, 21);
            this.txtCamera1.TabIndex = 1;
            // 
            // buttonSetCam2
            // 
            this.buttonSetCam2.Location = new System.Drawing.Point(471, 37);
            this.buttonSetCam2.Name = "buttonSetCam2";
            this.buttonSetCam2.Size = new System.Drawing.Size(94, 23);
            this.buttonSetCam2.TabIndex = 0;
            this.buttonSetCam2.Text = "Set";
            this.buttonSetCam2.UseVisualStyleBackColor = true;
            this.buttonSetCam2.Click += new System.EventHandler(this.buttonSetCam2_Click);
            // 
            // buttonSetCam1
            // 
            this.buttonSetCam1.Location = new System.Drawing.Point(471, 8);
            this.buttonSetCam1.Name = "buttonSetCam1";
            this.buttonSetCam1.Size = new System.Drawing.Size(94, 23);
            this.buttonSetCam1.TabIndex = 0;
            this.buttonSetCam1.Text = "Set";
            this.buttonSetCam1.UseVisualStyleBackColor = true;
            this.buttonSetCam1.Click += new System.EventHandler(this.buttonSetCam1_Click);
            // 
            // panelImagePath
            // 
            this.panelImagePath.Controls.Add(this.buttonSetCam2);
            this.panelImagePath.Controls.Add(this.buttonSelectImage1);
            this.panelImagePath.Controls.Add(this.buttonSetCam1);
            this.panelImagePath.Controls.Add(this.textBoxImage2Path);
            this.panelImagePath.Controls.Add(this.buttonSelectImage2);
            this.panelImagePath.Controls.Add(this.textBoxImage1Path);
            this.panelImagePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelImagePath.Location = new System.Drawing.Point(0, 70);
            this.panelImagePath.Name = "panelImagePath";
            this.panelImagePath.Size = new System.Drawing.Size(596, 78);
            this.panelImagePath.TabIndex = 2;
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 512);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.panelImagePath);
            this.Controls.Add(this.panelCam);
            this.DoubleBuffered = true;
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.panelCam.ResumeLayout(false);
            this.panelCam.PerformLayout();
            this.panelImagePath.ResumeLayout(false);
            this.panelImagePath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectImage2;
        private System.Windows.Forms.TextBox textBoxImage1Path;
        private System.Windows.Forms.Button buttonSelectImage1;
        private System.Windows.Forms.TextBox textBoxImage2Path;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Button buttonCam2;
        private System.Windows.Forms.Button buttonCam1;
        private System.Windows.Forms.Panel panelCam;
        private System.Windows.Forms.Panel panelImagePath;
        private System.Windows.Forms.Button buttonSetCam2;
        private System.Windows.Forms.Button buttonSetCam1;
        private System.Windows.Forms.TextBox txtCamera2;
        private System.Windows.Forms.TextBox txtCamera1;
    }
}