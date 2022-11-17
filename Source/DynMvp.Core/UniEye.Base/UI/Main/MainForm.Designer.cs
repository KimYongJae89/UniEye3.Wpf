using System;

namespace UniEye.Base.UI.Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.tabControlMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.title = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelTitle = new Infragistics.Win.Misc.UltraPanel();
            this.btnUser = new Infragistics.Win.Misc.UltraButton();
            this.btnExit = new Infragistics.Win.Misc.UltraButton();
            this.titleLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).BeginInit();
            this.tabControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.panelTitle.ClientArea.SuspendLayout();
            this.panelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            appearance1.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            resources.ApplyResources(appearance1, "appearance1");
            this.tabControlMain.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.FontData.Name = resources.GetString("resource.Name1");
            appearance2.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            this.tabControlMain.ClientAreaAppearance = appearance2;
            this.tabControlMain.Controls.Add(this.ultraTabSharedControlsPage1);
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            appearance3.ForeColor = System.Drawing.Color.White;
            this.tabControlMain.HotTrackAppearance = appearance3;
            this.tabControlMain.ImageSize = new System.Drawing.Size(36, 36);
            this.tabControlMain.MinTabWidth = 80;
            this.tabControlMain.Name = "tabControlMain";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.tabControlMain.SelectedTabAppearance = appearance4;
            this.tabControlMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabControlMain.ShowPartialTabs = Infragistics.Win.DefaultableBoolean.False;
            this.tabControlMain.ShowTabListButton = Infragistics.Win.DefaultableBoolean.False;
            this.tabControlMain.SpaceAfterTabs = new Infragistics.Win.DefaultableInteger(0);
            this.tabControlMain.SpaceBeforeTabs = new Infragistics.Win.DefaultableInteger(0);
            this.tabControlMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Flat;
            this.tabControlMain.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            this.tabControlMain.TabPadding = new System.Drawing.Size(10, 10);
            this.tabControlMain.TabSize = new System.Drawing.Size(100, 0);
            this.tabControlMain.TextOrientation = Infragistics.Win.UltraWinTabs.TextOrientation.Horizontal;
            this.tabControlMain.UseAppStyling = false;
            this.tabControlMain.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.tabControlMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabControlMain.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.tabControlMain_SelectedTabChanging);
            this.tabControlMain.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabControlMain_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            resources.ApplyResources(this.ultraTabSharedControlsPage1, "ultraTabSharedControlsPage1");
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            // 
            // ultraTabSharedControlsPage3
            // 
            resources.ApplyResources(this.ultraTabSharedControlsPage3, "ultraTabSharedControlsPage3");
            this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
            // 
            // ultraTabPageControl10
            // 
            resources.ApplyResources(this.ultraTabPageControl10, "ultraTabPageControl10");
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            // 
            // ultraTabPageControl11
            // 
            resources.ApplyResources(this.ultraTabPageControl11, "ultraTabPageControl11");
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            // 
            // ultraTabPageControl12
            // 
            resources.ApplyResources(this.ultraTabPageControl12, "ultraTabPageControl12");
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            // 
            // ultraTabControl1
            // 
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraTabControl1.Appearance = appearance5;
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl10);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl11);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl12);
            resources.ApplyResources(this.ultraTabControl1, "ultraTabControl1");
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage3;
            // 
            // title
            // 
            resources.ApplyResources(this.title, "title");
            this.title.BackColor = System.Drawing.Color.Transparent;
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Name = "title";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // panelRight
            // 
            resources.ApplyResources(this.panelRight, "panelRight");
            this.panelRight.Name = "panelRight";
            // 
            // panelTitle
            // 
            appearance6.BackColor = System.Drawing.Color.Teal;
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.panelTitle.Appearance = appearance6;
            // 
            // panelTitle.ClientArea
            // 
            this.panelTitle.ClientArea.Controls.Add(this.btnUser);
            this.panelTitle.ClientArea.Controls.Add(this.btnExit);
            this.panelTitle.ClientArea.Controls.Add(this.titleLogo);
            this.panelTitle.ClientArea.Controls.Add(this.title);
            resources.ApplyResources(this.panelTitle, "panelTitle");
            this.panelTitle.Name = "panelTitle";
            // 
            // btnUser
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.Image = global::UniEye.Base.Properties.Resources.user;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnUser.Appearance = appearance7;
            this.btnUser.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            resources.ApplyResources(this.btnUser, "btnUser");
            this.btnUser.ImageSize = new System.Drawing.Size(60, 60);
            this.btnUser.Name = "btnUser";
            this.btnUser.Tag = "Start";
            this.btnUser.UseAppStyling = false;
            this.btnUser.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnExit
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.Image = global::UniEye.Base.Properties.Resources.Exit;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnExit.Appearance = appearance8;
            this.btnExit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ImageSize = new System.Drawing.Size(60, 60);
            this.btnExit.Name = "btnExit";
            this.btnExit.Tag = "Start";
            this.btnExit.UseAppStyling = false;
            this.btnExit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // titleLogo
            // 
            this.titleLogo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.titleLogo, "titleLogo");
            this.titleLogo.Name = "titleLogo";
            this.titleLogo.TabStop = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.panelTitle.ClientArea.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabControlMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private System.Windows.Forms.PictureBox titleLogo;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panelRight;
        private Infragistics.Win.Misc.UltraPanel panelTitle;
        public System.Windows.Forms.Label title;
        private Infragistics.Win.Misc.UltraButton btnUser;
        private Infragistics.Win.Misc.UltraButton btnExit;
    }
}

