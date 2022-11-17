namespace UniEye.Base.UI
{
    partial class ProbeInfoPanel
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
            this.probeFullId = new System.Windows.Forms.TextBox();
            this.labelProbeType = new System.Windows.Forms.Label();
            this.probeId = new System.Windows.Forms.Label();
            this.labelProbe = new System.Windows.Forms.Label();
            this.labelProbeId = new System.Windows.Forms.Label();
            this.probeType = new System.Windows.Forms.Label();
            this.labelProbeName = new System.Windows.Forms.Label();
            this.comboBoxProbeName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // probeFullId
            // 
            this.probeFullId.Location = new System.Drawing.Point(181, 1);
            this.probeFullId.Name = "probeFullId";
            this.probeFullId.ReadOnly = true;
            this.probeFullId.Size = new System.Drawing.Size(169, 29);
            this.probeFullId.TabIndex = 22;
            // 
            // labelProbeType
            // 
            this.labelProbeType.AutoSize = true;
            this.labelProbeType.Location = new System.Drawing.Point(70, 33);
            this.labelProbeType.Name = "labelProbeType";
            this.labelProbeType.Size = new System.Drawing.Size(46, 21);
            this.labelProbeType.TabIndex = 20;
            this.labelProbeType.Text = "Type";
            this.labelProbeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // probeId
            // 
            this.probeId.BackColor = System.Drawing.Color.Lavender;
            this.probeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeId.ForeColor = System.Drawing.Color.Black;
            this.probeId.Location = new System.Drawing.Point(129, 3);
            this.probeId.Margin = new System.Windows.Forms.Padding(0);
            this.probeId.Name = "probeId";
            this.probeId.Size = new System.Drawing.Size(50, 26);
            this.probeId.TabIndex = 19;
            this.probeId.Text = "ID";
            this.probeId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProbe
            // 
            this.labelProbe.AutoSize = true;
            this.labelProbe.Location = new System.Drawing.Point(3, 9);
            this.labelProbe.Name = "labelProbe";
            this.labelProbe.Size = new System.Drawing.Size(54, 21);
            this.labelProbe.TabIndex = 16;
            this.labelProbe.Text = "Probe";
            // 
            // labelProbeId
            // 
            this.labelProbeId.AutoSize = true;
            this.labelProbeId.Location = new System.Drawing.Point(70, 9);
            this.labelProbeId.Name = "labelProbeId";
            this.labelProbeId.Size = new System.Drawing.Size(25, 21);
            this.labelProbeId.TabIndex = 17;
            this.labelProbeId.Text = "ID";
            // 
            // probeType
            // 
            this.probeType.BackColor = System.Drawing.Color.Lavender;
            this.probeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeType.ForeColor = System.Drawing.Color.Black;
            this.probeType.Location = new System.Drawing.Point(129, 33);
            this.probeType.Margin = new System.Windows.Forms.Padding(0);
            this.probeType.Name = "probeType";
            this.probeType.Size = new System.Drawing.Size(221, 26);
            this.probeType.TabIndex = 21;
            this.probeType.Text = "Type";
            this.probeType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProbeName
            // 
            this.labelProbeName.AutoSize = true;
            this.labelProbeName.Location = new System.Drawing.Point(70, 63);
            this.labelProbeName.Name = "labelProbeName";
            this.labelProbeName.Size = new System.Drawing.Size(53, 21);
            this.labelProbeName.TabIndex = 18;
            this.labelProbeName.Text = "Name";
            // 
            // comboBoxProbeName
            // 
            this.comboBoxProbeName.FormattingEnabled = true;
            this.comboBoxProbeName.Location = new System.Drawing.Point(129, 62);
            this.comboBoxProbeName.Name = "comboBoxProbeName";
            this.comboBoxProbeName.Size = new System.Drawing.Size(221, 29);
            this.comboBoxProbeName.TabIndex = 15;
            this.comboBoxProbeName.SelectedIndexChanged += new System.EventHandler(this.comboBoxProbeName_SelectedIndexChanged);
            this.comboBoxProbeName.TextChanged += new System.EventHandler(this.comboBoxProbeName_TextChanged);
            // 
            // DefaultProbeInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.probeFullId);
            this.Controls.Add(this.labelProbeType);
            this.Controls.Add(this.probeId);
            this.Controls.Add(this.labelProbe);
            this.Controls.Add(this.labelProbeId);
            this.Controls.Add(this.probeType);
            this.Controls.Add(this.labelProbeName);
            this.Controls.Add(this.comboBoxProbeName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DefaultProbeInfoPanel";
            this.Size = new System.Drawing.Size(360, 95);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox probeFullId;
        private System.Windows.Forms.Label labelProbeType;
        private System.Windows.Forms.Label probeId;
        private System.Windows.Forms.Label labelProbe;
        private System.Windows.Forms.Label labelProbeId;
        private System.Windows.Forms.Label probeType;
        private System.Windows.Forms.Label labelProbeName;
        private System.Windows.Forms.ComboBox comboBoxProbeName;
    }
}
