namespace UniEye.Base.UI
{
    partial class DefectReviewForm
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.panelReport = new System.Windows.Forms.Panel();
            this.panelProbeControl = new System.Windows.Forms.Panel();
            this.probeNgButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.labelDefect = new System.Windows.Forms.Label();
            this.numDefectTarget = new System.Windows.Forms.Label();
            this.probeGoodButton = new System.Windows.Forms.Button();
            this.labelResult = new System.Windows.Forms.Label();
            this.btnAlarmOff = new System.Windows.Forms.Button();
            this.buttonRetry = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.panelProbeControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.panelReport);
            this.mainPanel.Controls.Add(this.panelProbeControl);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(846, 528);
            this.mainPanel.TabIndex = 1;
            // 
            // panelReport
            // 
            this.panelReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReport.Location = new System.Drawing.Point(0, 0);
            this.panelReport.Name = "panelReport";
            this.panelReport.Size = new System.Drawing.Size(844, 459);
            this.panelReport.TabIndex = 11;
            // 
            // panelProbeControl
            // 
            this.panelProbeControl.Controls.Add(this.probeNgButton);
            this.panelProbeControl.Controls.Add(this.nextButton);
            this.panelProbeControl.Controls.Add(this.prevButton);
            this.panelProbeControl.Controls.Add(this.labelDefect);
            this.panelProbeControl.Controls.Add(this.numDefectTarget);
            this.panelProbeControl.Controls.Add(this.probeGoodButton);
            this.panelProbeControl.Controls.Add(this.labelResult);
            this.panelProbeControl.Controls.Add(this.btnAlarmOff);
            this.panelProbeControl.Controls.Add(this.buttonRetry);
            this.panelProbeControl.Controls.Add(this.buttonClose);
            this.panelProbeControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProbeControl.Location = new System.Drawing.Point(0, 459);
            this.panelProbeControl.Margin = new System.Windows.Forms.Padding(2);
            this.panelProbeControl.Name = "panelProbeControl";
            this.panelProbeControl.Size = new System.Drawing.Size(844, 67);
            this.panelProbeControl.TabIndex = 10;
            // 
            // probeNgButton
            // 
            this.probeNgButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.probeNgButton.Location = new System.Drawing.Point(292, 0);
            this.probeNgButton.Margin = new System.Windows.Forms.Padding(2);
            this.probeNgButton.Name = "probeNgButton";
            this.probeNgButton.Size = new System.Drawing.Size(73, 67);
            this.probeNgButton.TabIndex = 11;
            this.probeNgButton.Text = "NG";
            this.probeNgButton.UseVisualStyleBackColor = true;
            this.probeNgButton.Click += new System.EventHandler(this.probeNgButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.nextButton.Location = new System.Drawing.Point(219, 0);
            this.nextButton.Margin = new System.Windows.Forms.Padding(2);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(73, 67);
            this.nextButton.TabIndex = 11;
            this.nextButton.Text = ">>>";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.prevButton.Location = new System.Drawing.Point(146, 0);
            this.prevButton.Margin = new System.Windows.Forms.Padding(2);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(73, 67);
            this.prevButton.TabIndex = 10;
            this.prevButton.Text = "<<<";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // labelDefect
            // 
            this.labelDefect.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDefect.ForeColor = System.Drawing.Color.Red;
            this.labelDefect.Location = new System.Drawing.Point(363, 0);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(81, 67);
            this.labelDefect.TabIndex = 8;
            this.labelDefect.Text = "Defect";
            this.labelDefect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numDefectTarget
            // 
            this.numDefectTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDefectTarget.Dock = System.Windows.Forms.DockStyle.Right;
            this.numDefectTarget.ForeColor = System.Drawing.Color.Red;
            this.numDefectTarget.Location = new System.Drawing.Point(444, 0);
            this.numDefectTarget.Name = "numDefectTarget";
            this.numDefectTarget.Size = new System.Drawing.Size(97, 67);
            this.numDefectTarget.TabIndex = 9;
            this.numDefectTarget.Text = "0";
            this.numDefectTarget.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // probeGoodButton
            // 
            this.probeGoodButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.probeGoodButton.Location = new System.Drawing.Point(73, 0);
            this.probeGoodButton.Margin = new System.Windows.Forms.Padding(2);
            this.probeGoodButton.Name = "probeGoodButton";
            this.probeGoodButton.Size = new System.Drawing.Size(73, 67);
            this.probeGoodButton.TabIndex = 11;
            this.probeGoodButton.Text = "Good";
            this.probeGoodButton.UseVisualStyleBackColor = true;
            this.probeGoodButton.Click += new System.EventHandler(this.probeGoodButton_Click);
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.Color.Red;
            this.labelResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelResult.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelResult.Location = new System.Drawing.Point(0, 0);
            this.labelResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(73, 67);
            this.labelResult.TabIndex = 8;
            this.labelResult.Text = "NG";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAlarmOff
            // 
            this.btnAlarmOff.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAlarmOff.Location = new System.Drawing.Point(541, 0);
            this.btnAlarmOff.Margin = new System.Windows.Forms.Padding(2);
            this.btnAlarmOff.Name = "btnAlarmOff";
            this.btnAlarmOff.Size = new System.Drawing.Size(105, 67);
            this.btnAlarmOff.TabIndex = 11;
            this.btnAlarmOff.Text = "Alarm Off";
            this.btnAlarmOff.UseVisualStyleBackColor = true;
            this.btnAlarmOff.Click += new System.EventHandler(this.btnAlarmOff_Click);
            // 
            // buttonRetry
            // 
            this.buttonRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.buttonRetry.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRetry.Location = new System.Drawing.Point(646, 0);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(101, 67);
            this.buttonRetry.TabIndex = 14;
            this.buttonRetry.Text = "Retry";
            this.buttonRetry.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClose.Location = new System.Drawing.Point(747, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(97, 67);
            this.buttonClose.TabIndex = 12;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // DefectReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 528);
            this.Controls.Add(this.mainPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DefectReviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.ProbeDefectProcessForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.panelProbeControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelProbeControl;
        private System.Windows.Forms.Label labelDefect;
        private System.Windows.Forms.Label numDefectTarget;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button probeNgButton;
        private System.Windows.Forms.Button probeGoodButton;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button btnAlarmOff;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelReport;
        private System.Windows.Forms.Button buttonRetry;
    }
}