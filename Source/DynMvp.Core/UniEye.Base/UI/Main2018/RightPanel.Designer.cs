namespace UniEye.Base.UI.Main2018
{
    partial class RightPanel
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnStart = new Infragistics.Win.Misc.UltraButton();
            this.btnLive = new Infragistics.Win.Misc.UltraButton();
            this.btnReport = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance1.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnStart.Appearance = appearance1;
            this.btnStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStart.ImageSize = new System.Drawing.Size(50, 60);
            this.btnStart.Location = new System.Drawing.Point(0, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 72);
            this.btnStart.TabIndex = 0;
            this.btnStart.UseAppStyling = false;
            this.btnStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnLive
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            appearance2.Image = global::UniEye.Base.Properties.Resources.live_white_36;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnLive.Appearance = appearance2;
            this.btnLive.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnLive.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLive.ImageSize = new System.Drawing.Size(50, 60);
            this.btnLive.Location = new System.Drawing.Point(0, 72);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(70, 72);
            this.btnLive.TabIndex = 1;
            this.btnLive.UseAppStyling = false;
            this.btnLive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnReport
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            appearance3.Image = global::UniEye.Base.Properties.Resources.chart_sel;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnReport.Appearance = appearance3;
            this.btnReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReport.ImageSize = new System.Drawing.Size(50, 60);
            this.btnReport.Location = new System.Drawing.Point(0, 144);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(70, 72);
            this.btnReport.TabIndex = 2;
            this.btnReport.UseAppStyling = false;
            this.btnReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // RightPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnLive);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(70, 500);
            this.Name = "RightPanel";
            this.Size = new System.Drawing.Size(70, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnStart;
        private Infragistics.Win.Misc.UltraButton btnLive;
        private Infragistics.Win.Misc.UltraButton btnReport;
    }
}
