namespace DynMvp.Devices.UI
{
    partial class JoystickAxisForm
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
            this.panelImage = new System.Windows.Forms.Panel();
            this.panelJoystick = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelLightType = new System.Windows.Forms.Label();
            this.comboLightType = new System.Windows.Forms.ComboBox();
            this.pictureImage = new System.Windows.Forms.PictureBox();
            this.panelImage.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelImage
            // 
            this.panelImage.Controls.Add(this.pictureImage);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(186, 34);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(315, 226);
            this.panelImage.TabIndex = 1;
            // 
            // panelJoystick
            // 
            this.panelJoystick.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelJoystick.Location = new System.Drawing.Point(0, 0);
            this.panelJoystick.Name = "panelJoystick";
            this.panelJoystick.Size = new System.Drawing.Size(186, 260);
            this.panelJoystick.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelLightType);
            this.panel1.Controls.Add(this.comboLightType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(186, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 34);
            this.panel1.TabIndex = 1;
            // 
            // labelLightType
            // 
            this.labelLightType.AutoSize = true;
            this.labelLightType.Location = new System.Drawing.Point(12, 12);
            this.labelLightType.Name = "labelLightType";
            this.labelLightType.Size = new System.Drawing.Size(65, 12);
            this.labelLightType.TabIndex = 3;
            this.labelLightType.Text = "Light Type";
            // 
            // comboLightType
            // 
            this.comboLightType.FormattingEnabled = true;
            this.comboLightType.Items.AddRange(new object[] {
            "Light Type 1",
            "Light Type 2"});
            this.comboLightType.Location = new System.Drawing.Point(83, 6);
            this.comboLightType.Name = "comboLightType";
            this.comboLightType.Size = new System.Drawing.Size(120, 20);
            this.comboLightType.TabIndex = 2;
            // 
            // pictureImage
            // 
            this.pictureImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureImage.Location = new System.Drawing.Point(0, 0);
            this.pictureImage.Name = "pictureImage";
            this.pictureImage.Size = new System.Drawing.Size(315, 226);
            this.pictureImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureImage.TabIndex = 0;
            this.pictureImage.TabStop = false;
            // 
            // JoystickAxisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(501, 260);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelJoystick);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JoystickAxisForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Robot Joystick";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JoystickAxisForm_FormClosing);
            this.Load += new System.EventHandler(this.JoystickAxisForm_Load);
            this.panelImage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Panel panelJoystick;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelLightType;
        private System.Windows.Forms.ComboBox comboLightType;
        private System.Windows.Forms.PictureBox pictureImage;
    }
}