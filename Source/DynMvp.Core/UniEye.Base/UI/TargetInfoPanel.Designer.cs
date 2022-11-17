namespace UniEye.Base.UI
{
    partial class TargetInfoPanel
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
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.labelTargetName = new System.Windows.Forms.Label();
            this.targetPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.targetPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonRefresh.Location = new System.Drawing.Point(71, 112);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(98, 29);
            this.buttonRefresh.TabIndex = 17;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(13, 37);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(156, 29);
            this.txtTargetName.TabIndex = 15;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetName_KeyDown);
            // 
            // labelTargetName
            // 
            this.labelTargetName.AutoSize = true;
            this.labelTargetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelTargetName.Location = new System.Drawing.Point(9, 12);
            this.labelTargetName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTargetName.Name = "labelTargetName";
            this.labelTargetName.Size = new System.Drawing.Size(101, 20);
            this.labelTargetName.TabIndex = 14;
            this.labelTargetName.Text = "Target Name";
            // 
            // targetPictureBox
            // 
            this.targetPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetPictureBox.Location = new System.Drawing.Point(177, 5);
            this.targetPictureBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.targetPictureBox.Name = "targetPictureBox";
            this.targetPictureBox.Size = new System.Drawing.Size(194, 136);
            this.targetPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.targetPictureBox.TabIndex = 16;
            this.targetPictureBox.TabStop = false;
            // 
            // DefaultTargetInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.targetPictureBox);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.labelTargetName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DefaultTargetInfoPanel";
            this.Size = new System.Drawing.Size(380, 147);
            this.Load += new System.EventHandler(this.DefaultTargetInfoControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.targetPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.PictureBox targetPictureBox;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label labelTargetName;
    }
}
