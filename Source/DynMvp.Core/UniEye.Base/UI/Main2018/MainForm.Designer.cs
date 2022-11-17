using System;

namespace UniEye.Base.UI.Main2018
{
    partial class MainForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.panelTitle = new Infragistics.Win.Misc.UltraPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInspect = new Infragistics.Win.Misc.UltraButton();
            this.panelModelBar = new System.Windows.Forms.Panel();
            this.labelModelName = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.btnConfig = new Infragistics.Win.Misc.UltraButton();
            this.btnExit = new Infragistics.Win.Misc.UltraButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.btnReport = new Infragistics.Win.Misc.UltraButton();
            this.productLogo = new System.Windows.Forms.PictureBox();
            this.picProgramIcon = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.panelTitle.ClientArea.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelModelBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panelTitle.Appearance = appearance1;
            // 
            // panelTitle.ClientArea
            // 
            this.panelTitle.ClientArea.Controls.Add(this.productLogo);
            this.panelTitle.ClientArea.Controls.Add(this.btnReport);
            this.panelTitle.ClientArea.Controls.Add(this.panel1);
            this.panelTitle.ClientArea.Controls.Add(this.panelModelBar);
            this.panelTitle.ClientArea.Controls.Add(this.title);
            this.panelTitle.ClientArea.Controls.Add(this.picProgramIcon);
            this.panelTitle.ClientArea.Controls.Add(this.btnConfig);
            this.panelTitle.ClientArea.Controls.Add(this.btnExit);
            resources.ApplyResources(this.panelTitle, "panelTitle");
            this.panelTitle.Name = "panelTitle";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnInspect);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnInspect
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.Image = global::UniEye.Base.Properties.Resources.Monitoring_integrated_White;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnInspect.Appearance = appearance3;
            resources.ApplyResources(this.btnInspect, "btnInspect");
            this.btnInspect.ImageSize = new System.Drawing.Size(50, 50);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Tag = "Start";
            this.btnInspect.UseAppStyling = false;
            this.btnInspect.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
            // 
            // panelModelBar
            // 
            this.panelModelBar.Controls.Add(this.labelModelName);
            resources.ApplyResources(this.panelModelBar, "panelModelBar");
            this.panelModelBar.Name = "panelModelBar";
            this.panelModelBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelModelBar_MouseDown);
            // 
            // labelModelName
            // 
            this.labelModelName.BackColor = System.Drawing.Color.LemonChiffon;
            this.labelModelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.labelModelName, "labelModelName");
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Click += new System.EventHandler(this.labelModelName_Click);
            // 
            // title
            // 
            this.title.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.title, "title");
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Name = "title";
            this.title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.title_MouseDown);
            // 
            // btnConfig
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.Image = global::UniEye.Base.Properties.Resources.Setting_integrated_White;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnConfig.Appearance = appearance5;
            this.btnConfig.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            resources.ApplyResources(this.btnConfig, "btnConfig");
            this.btnConfig.ImageSize = new System.Drawing.Size(45, 45);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Tag = "Start";
            this.btnConfig.UseAppStyling = false;
            this.btnConfig.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnExit
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.Image = global::UniEye.Base.Properties.Resources.Exit_integrated_White;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnExit.Appearance = appearance6;
            this.btnExit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ImageSize = new System.Drawing.Size(50, 50);
            this.btnExit.Name = "btnExit";
            this.btnExit.Tag = "Start";
            this.btnExit.UseAppStyling = false;
            this.btnExit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // btnReport
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnReport.Appearance = appearance2;
            this.btnReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            resources.ApplyResources(this.btnReport, "btnReport");
            this.btnReport.ImageSize = new System.Drawing.Size(45, 45);
            this.btnReport.Name = "btnReport";
            this.btnReport.Tag = "Start";
            this.btnReport.UseAppStyling = false;
            this.btnReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // productLogo
            // 
            this.productLogo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.productLogo, "productLogo");
            this.productLogo.Name = "productLogo";
            this.productLogo.TabStop = false;
            // 
            // picProgramIcon
            // 
            appearance4.Image = global::UniEye.Base.Properties.Resources.shape;
            this.picProgramIcon.Appearance = appearance4;
            this.picProgramIcon.BorderShadowColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.picProgramIcon, "picProgramIcon");
            this.picProgramIcon.Image = ((object)(resources.GetObject("picProgramIcon.Image")));
            this.picProgramIcon.Name = "picProgramIcon";
            this.picProgramIcon.ScaleImage = Infragistics.Win.ScaleImage.Always;
            this.picProgramIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picProgramIcon_MouseDown);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panelTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.panelTitle.ClientArea.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelModelBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel panelTitle;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox picProgramIcon;
        private System.Windows.Forms.PictureBox productLogo;
        public System.Windows.Forms.Label title;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        private Infragistics.Win.Misc.UltraButton btnConfig;
        private Infragistics.Win.Misc.UltraButton btnExit;
        private System.Windows.Forms.Panel panelModelBar;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnInspect;
        private Infragistics.Win.Misc.UltraButton btnReport;
    }
}

