namespace UniEye.Base.UI.LightAutoTune
{
    partial class LightAutoTuneForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelLightValue = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.layoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblLightValue = new System.Windows.Forms.Label();
            this.layoutMain.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.layoutButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.SkyBlue;
            this.layoutMain.SetColumnSpan(this.labelTitle, 3);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.Location = new System.Drawing.Point(1, 1);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(840, 60);
            this.labelTitle.TabIndex = 177;
            this.labelTitle.Text = "Auto Tune";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(360, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(180, 40);
            this.buttonCancel.TabIndex = 183;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(264, 164);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(577, 50);
            this.progressBar.TabIndex = 184;
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.White;
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.buttonStart, 1, 2);
            this.layoutMain.Controls.Add(this.labelLightValue, 0, 1);
            this.layoutMain.Controls.Add(this.labelProgress, 0, 2);
            this.layoutMain.Controls.Add(this.progressBar, 2, 3);
            this.layoutMain.Controls.Add(this.progress, 2, 2);
            this.layoutMain.Controls.Add(this.labelTitle, 0, 0);
            this.layoutMain.Controls.Add(this.panelBottom, 0, 5);
            this.layoutMain.Controls.Add(this.layoutPanel, 0, 4);
            this.layoutMain.Controls.Add(this.lblLightValue, 1, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(10, 10);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 6;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMain.Size = new System.Drawing.Size(842, 589);
            this.layoutMain.TabIndex = 185;
            // 
            // buttonStart
            // 
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStart.Location = new System.Drawing.Point(152, 113);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStart.Name = "buttonStart";
            this.layoutMain.SetRowSpan(this.buttonStart, 2);
            this.buttonStart.Size = new System.Drawing.Size(111, 101);
            this.buttonStart.TabIndex = 185;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelLightValue
            // 
            this.labelLightValue.BackColor = System.Drawing.Color.AliceBlue;
            this.labelLightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightValue.Location = new System.Drawing.Point(1, 62);
            this.labelLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightValue.Name = "labelLightValue";
            this.labelLightValue.Size = new System.Drawing.Size(150, 50);
            this.labelLightValue.TabIndex = 185;
            this.labelLightValue.Text = "Light Value";
            this.labelLightValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.Color.AliceBlue;
            this.labelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProgress.Location = new System.Drawing.Point(1, 113);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(0);
            this.labelProgress.Name = "labelProgress";
            this.layoutMain.SetRowSpan(this.labelProgress, 2);
            this.labelProgress.Size = new System.Drawing.Size(150, 101);
            this.labelProgress.TabIndex = 186;
            this.labelProgress.Text = "Progress";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progress
            // 
            this.progress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progress.Location = new System.Drawing.Point(264, 113);
            this.progress.Margin = new System.Windows.Forms.Padding(0);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(577, 50);
            this.progress.TabIndex = 187;
            this.progress.Text = "0 / 255";
            this.progress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBottom
            // 
            this.layoutMain.SetColumnSpan(this.panelBottom, 3);
            this.panelBottom.Controls.Add(this.layoutButton);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(1, 538);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(150, 5, 150, 5);
            this.panelBottom.Size = new System.Drawing.Size(840, 50);
            this.panelBottom.TabIndex = 189;
            // 
            // layoutButton
            // 
            this.layoutButton.ColumnCount = 3;
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.Controls.Add(this.buttonApply, 0, 0);
            this.layoutButton.Controls.Add(this.buttonCancel, 2, 0);
            this.layoutButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutButton.Location = new System.Drawing.Point(150, 5);
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.RowCount = 1;
            this.layoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButton.Size = new System.Drawing.Size(540, 40);
            this.layoutButton.TabIndex = 184;
            // 
            // buttonApply
            // 
            this.buttonApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonApply.Location = new System.Drawing.Point(0, 0);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(0);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(180, 40);
            this.buttonApply.TabIndex = 184;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // layoutPanel
            // 
            this.layoutPanel.BackColor = System.Drawing.Color.Black;
            this.layoutPanel.ColumnCount = 1;
            this.layoutMain.SetColumnSpan(this.layoutPanel, 3);
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(1, 215);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 1;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 322F));
            this.layoutPanel.Size = new System.Drawing.Size(840, 322);
            this.layoutPanel.TabIndex = 190;
            // 
            // lblLightValue
            // 
            this.layoutMain.SetColumnSpan(this.lblLightValue, 2);
            this.lblLightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLightValue.Location = new System.Drawing.Point(152, 62);
            this.lblLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblLightValue.Name = "lblLightValue";
            this.lblLightValue.Size = new System.Drawing.Size(689, 50);
            this.lblLightValue.TabIndex = 188;
            this.lblLightValue.Text = "Invalid";
            this.lblLightValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LightAutoTuneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(862, 609);
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "LightAutoTuneForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AutoTuneForm_Load);
            this.layoutMain.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.layoutButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelLightValue;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label progress;
        private System.Windows.Forms.Label lblLightValue;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.TableLayoutPanel layoutButton;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonStart;
    }
}