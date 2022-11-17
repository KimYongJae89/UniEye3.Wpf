namespace UniEye.Base.UI.SamsungElec
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
            this.btnStop = new Infragistics.Win.Misc.UltraButton();
            this.btnHomeMove = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            appearance1.Image = global::UniEye.Base.Properties.Resources.Start;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnStart.Appearance = appearance1;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStart.ImageSize = new System.Drawing.Size(72, 72);
            this.btnStart.Location = new System.Drawing.Point(0, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 90);
            this.btnStart.TabIndex = 0;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            appearance2.Image = global::UniEye.Base.Properties.Resources.Stop;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnStop.Appearance = appearance2;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStop.ImageSize = new System.Drawing.Size(72, 72);
            this.btnStop.Location = new System.Drawing.Point(0, 90);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(95, 90);
            this.btnStop.TabIndex = 3;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnHomeMove
            // 
            appearance3.Image = global::UniEye.Base.Properties.Resources.Initialize;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnHomeMove.Appearance = appearance3;
            this.btnHomeMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHomeMove.ImageSize = new System.Drawing.Size(72, 72);
            this.btnHomeMove.Location = new System.Drawing.Point(0, 180);
            this.btnHomeMove.Name = "btnHomeMove";
            this.btnHomeMove.Size = new System.Drawing.Size(95, 90);
            this.btnHomeMove.TabIndex = 4;
            this.btnHomeMove.Click += new System.EventHandler(this.btnHomeMove_Click);
            // 
            // RightPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnHomeMove);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RightPanel";
            this.Size = new System.Drawing.Size(95, 455);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnStart;
        private Infragistics.Win.Misc.UltraButton btnStop;
        private Infragistics.Win.Misc.UltraButton btnHomeMove;
    }
}
