using System;

namespace UniEye.Base.UI.SamsungElec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelRHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelLotNo = new System.Windows.Forms.Label();
            this.labelModelName = new System.Windows.Forms.Label();
            this.tableLayoutPanelLHeader = new System.Windows.Forms.TableLayoutPanel();
            this.panelCompanyLogo = new System.Windows.Forms.Panel();
            this.panelClock = new System.Windows.Forms.Panel();
            this.panelMHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.tableLayoutPanelRHeader.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelLHeader.SuspendLayout();
            this.panelMHeader.SuspendLayout();
            this.SuspendLayout();
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
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraTabControl1.Appearance = appearance1;
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl10);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl11);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl12);
            resources.ApplyResources(this.ultraTabControl1, "ultraTabControl1");
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage3;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.panelBottom, "panelBottom");
            this.panelBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBottom.Controls.Add(this.btnExit);
            this.panelBottom.Name = "panelBottom";
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.Image = global::UniEye.Base.Properties.Resources.Exit;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panelHeader
            // 
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Controls.Add(this.tableLayoutPanelHeader);
            this.panelHeader.Name = "panelHeader";
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanelHeader, "tableLayoutPanelHeader");
            this.tableLayoutPanelHeader.Controls.Add(this.tableLayoutPanelRHeader, 2, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.tableLayoutPanelLHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelMHeader, 1, 0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            // 
            // tableLayoutPanelRHeader
            // 
            resources.ApplyResources(this.tableLayoutPanelRHeader, "tableLayoutPanelRHeader");
            this.tableLayoutPanelRHeader.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanelRHeader.Name = "tableLayoutPanelRHeader";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.labelLotNo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelModelName, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // labelLotNo
            // 
            resources.ApplyResources(this.labelLotNo, "labelLotNo");
            this.labelLotNo.Name = "labelLotNo";
            // 
            // labelModelName
            // 
            resources.ApplyResources(this.labelModelName, "labelModelName");
            this.labelModelName.Name = "labelModelName";
            // 
            // tableLayoutPanelLHeader
            // 
            resources.ApplyResources(this.tableLayoutPanelLHeader, "tableLayoutPanelLHeader");
            this.tableLayoutPanelLHeader.Controls.Add(this.panelCompanyLogo, 0, 0);
            this.tableLayoutPanelLHeader.Controls.Add(this.panelClock, 1, 0);
            this.tableLayoutPanelLHeader.Name = "tableLayoutPanelLHeader";
            // 
            // panelCompanyLogo
            // 
            resources.ApplyResources(this.panelCompanyLogo, "panelCompanyLogo");
            this.panelCompanyLogo.Name = "panelCompanyLogo";
            // 
            // panelClock
            // 
            resources.ApplyResources(this.panelClock, "panelClock");
            this.panelClock.Name = "panelClock";
            // 
            // panelMHeader
            // 
            resources.ApplyResources(this.panelMHeader, "panelMHeader");
            this.panelMHeader.Controls.Add(this.labelTitle);
            this.panelMHeader.Name = "panelMHeader";
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseDown);
            // 
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelHeader);
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
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelRHeader.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanelLHeader.ResumeLayout(false);
            this.panelMHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelLotNo;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLHeader;
        private System.Windows.Forms.Panel panelCompanyLogo;
        private System.Windows.Forms.Panel panelClock;
        private System.Windows.Forms.Panel panelMHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panelMain;
    }
}

