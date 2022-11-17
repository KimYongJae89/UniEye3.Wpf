namespace DynMvp.UI
{
    partial class SplashForm
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
            this.title = new System.Windows.Forms.Label();
            this.copyrightText = new System.Windows.Forms.Label();
            this.versionText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressMessage = new System.Windows.Forms.Label();
            this.splashActionTimer = new System.Windows.Forms.Timer(this.components);
            this.buildText = new System.Windows.Forms.Label();
            this.companyLogo = new System.Windows.Forms.PictureBox();
            this.pictureIcon = new System.Windows.Forms.PictureBox();
            this.productLogo = new System.Windows.Forms.PictureBox();
            this.backgroundImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImage)).BeginInit();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(211)))), ((int)(((byte)(41)))));
            this.title.Location = new System.Drawing.Point(17, 118);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(537, 89);
            this.title.TabIndex = 0;
            this.title.Text = "UniEye";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // copyrightText
            // 
            this.copyrightText.ForeColor = System.Drawing.Color.Black;
            this.copyrightText.Location = new System.Drawing.Point(18, 304);
            this.copyrightText.Name = "copyrightText";
            this.copyrightText.Size = new System.Drawing.Size(364, 23);
            this.copyrightText.TabIndex = 1;
            this.copyrightText.Text = "©2019 UniEye. All right reserved.";
            this.copyrightText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // versionText
            // 
            this.versionText.AutoSize = true;
            this.versionText.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.versionText.ForeColor = System.Drawing.Color.Black;
            this.versionText.Location = new System.Drawing.Point(388, 212);
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(93, 21);
            this.versionText.TabIndex = 1;
            this.versionText.Text = "Version 1.0";
            this.versionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 264);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(364, 37);
            this.progressBar.TabIndex = 4;
            // 
            // progressMessage
            // 
            this.progressMessage.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.progressMessage.Location = new System.Drawing.Point(18, 238);
            this.progressMessage.Name = "progressMessage";
            this.progressMessage.Size = new System.Drawing.Size(364, 23);
            this.progressMessage.TabIndex = 1;
            this.progressMessage.Text = "Loading...";
            this.progressMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splashActionTimer
            // 
            this.splashActionTimer.Interval = 5000;
            this.splashActionTimer.Tick += new System.EventHandler(this.splashActionTimer_Tick);
            // 
            // buildText
            // 
            this.buildText.AutoSize = true;
            this.buildText.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buildText.ForeColor = System.Drawing.Color.Black;
            this.buildText.Location = new System.Drawing.Point(388, 237);
            this.buildText.Name = "buildText";
            this.buildText.Size = new System.Drawing.Size(94, 21);
            this.buildText.TabIndex = 6;
            this.buildText.Text = "Build xxxxxx";
            this.buildText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // companyLogo
            // 
            this.companyLogo.Image = global::DynMvp.Properties.Resources.unieye;
            this.companyLogo.Location = new System.Drawing.Point(388, 264);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.Size = new System.Drawing.Size(167, 63);
            this.companyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.companyLogo.TabIndex = 7;
            this.companyLogo.TabStop = false;
            // 
            // pictureIcon
            // 
            this.pictureIcon.Image = global::DynMvp.Properties.Resources.UniversalEye;
            this.pictureIcon.Location = new System.Drawing.Point(18, 12);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(67, 73);
            this.pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureIcon.TabIndex = 5;
            this.pictureIcon.TabStop = false;
            // 
            // productLogo
            // 
            this.productLogo.Location = new System.Drawing.Point(442, 12);
            this.productLogo.Name = "productLogo";
            this.productLogo.Size = new System.Drawing.Size(113, 97);
            this.productLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productLogo.TabIndex = 2;
            this.productLogo.TabStop = false;
            // 
            // backgroundImage
            // 
            this.backgroundImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundImage.Image = global::DynMvp.Properties.Resources.SplashPanel;
            this.backgroundImage.Location = new System.Drawing.Point(0, 0);
            this.backgroundImage.Name = "backgroundImage";
            this.backgroundImage.Size = new System.Drawing.Size(567, 339);
            this.backgroundImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundImage.TabIndex = 3;
            this.backgroundImage.TabStop = false;
            // 
            // SplashForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(71)))), ((int)(((byte)(112)))));
            this.ClientSize = new System.Drawing.Size(567, 339);
            this.Controls.Add(this.companyLogo);
            this.Controls.Add(this.buildText);
            this.Controls.Add(this.pictureIcon);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.productLogo);
            this.Controls.Add(this.versionText);
            this.Controls.Add(this.progressMessage);
            this.Controls.Add(this.copyrightText);
            this.Controls.Add(this.title);
            this.Controls.Add(this.backgroundImage);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SplashForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox backgroundImage;
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label title;
        public System.Windows.Forms.Label versionText;
        public System.Windows.Forms.Label copyrightText;
        public System.Windows.Forms.Label progressMessage;
        private System.Windows.Forms.Timer splashActionTimer;
        public System.Windows.Forms.PictureBox productLogo;
        private System.Windows.Forms.PictureBox pictureIcon;
        public System.Windows.Forms.Label buildText;
        public System.Windows.Forms.PictureBox companyLogo;
    }
}