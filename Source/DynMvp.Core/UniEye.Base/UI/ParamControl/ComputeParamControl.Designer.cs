namespace UniEye.Base.UI.ParamControl
{
    partial class ComputeParamControl
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
            this.labelTarget1 = new System.Windows.Forms.Label();
            this.labelTarget2 = new System.Windows.Forms.Label();
            this.txtTarget1 = new System.Windows.Forms.TextBox();
            this.buttonTarget1 = new System.Windows.Forms.Button();
            this.buttonTarget2 = new System.Windows.Forms.Button();
            this.computeTypeList = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.txtTarget2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelTarget1
            // 
            this.labelTarget1.AutoSize = true;
            this.labelTarget1.Location = new System.Drawing.Point(3, 62);
            this.labelTarget1.Name = "labelTarget1";
            this.labelTarget1.Size = new System.Drawing.Size(77, 21);
            this.labelTarget1.TabIndex = 0;
            this.labelTarget1.Text = "Target1 :";
            // 
            // labelTarget2
            // 
            this.labelTarget2.AutoSize = true;
            this.labelTarget2.Location = new System.Drawing.Point(3, 92);
            this.labelTarget2.Name = "labelTarget2";
            this.labelTarget2.Size = new System.Drawing.Size(77, 21);
            this.labelTarget2.TabIndex = 1;
            this.labelTarget2.Text = "Target2 :";
            // 
            // txtTarget1
            // 
            this.txtTarget1.Location = new System.Drawing.Point(81, 59);
            this.txtTarget1.Name = "txtTarget1";
            this.txtTarget1.ReadOnly = true;
            this.txtTarget1.Size = new System.Drawing.Size(141, 29);
            this.txtTarget1.TabIndex = 2;
            // 
            // buttonTarget1
            // 
            this.buttonTarget1.Location = new System.Drawing.Point(228, 59);
            this.buttonTarget1.Name = "buttonTarget1";
            this.buttonTarget1.Size = new System.Drawing.Size(32, 26);
            this.buttonTarget1.TabIndex = 4;
            this.buttonTarget1.Text = "...";
            this.buttonTarget1.UseVisualStyleBackColor = true;
            this.buttonTarget1.Click += new System.EventHandler(this.buttonTarget1_Click);
            // 
            // buttonTarget2
            // 
            this.buttonTarget2.Location = new System.Drawing.Point(228, 94);
            this.buttonTarget2.Name = "buttonTarget2";
            this.buttonTarget2.Size = new System.Drawing.Size(32, 26);
            this.buttonTarget2.TabIndex = 5;
            this.buttonTarget2.Text = "...";
            this.buttonTarget2.UseVisualStyleBackColor = true;
            this.buttonTarget2.Click += new System.EventHandler(this.buttonTarget2_Click);
            // 
            // computeTypeList
            // 
            this.computeTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.computeTypeList.FormattingEnabled = true;
            this.computeTypeList.Location = new System.Drawing.Point(81, 15);
            this.computeTypeList.Name = "computeTypeList";
            this.computeTypeList.Size = new System.Drawing.Size(141, 29);
            this.computeTypeList.TabIndex = 6;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(19, 18);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(56, 21);
            this.labelType.TabIndex = 7;
            this.labelType.Text = "Type :";
            // 
            // txtTarget2
            // 
            this.txtTarget2.Location = new System.Drawing.Point(81, 89);
            this.txtTarget2.Name = "txtTarget2";
            this.txtTarget2.ReadOnly = true;
            this.txtTarget2.Size = new System.Drawing.Size(141, 29);
            this.txtTarget2.TabIndex = 8;
            // 
            // ComputeParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTarget2);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.computeTypeList);
            this.Controls.Add(this.buttonTarget2);
            this.Controls.Add(this.buttonTarget1);
            this.Controls.Add(this.txtTarget1);
            this.Controls.Add(this.labelTarget2);
            this.Controls.Add(this.labelTarget1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ComputeParamControl";
            this.Size = new System.Drawing.Size(294, 168);
            this.Load += new System.EventHandler(this.ComputeParamControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTarget1;
        private System.Windows.Forms.Label labelTarget2;
        private System.Windows.Forms.TextBox txtTarget1;
        private System.Windows.Forms.Button buttonTarget1;
        private System.Windows.Forms.Button buttonTarget2;
        private System.Windows.Forms.ComboBox computeTypeList;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.TextBox txtTarget2;
    }
}
