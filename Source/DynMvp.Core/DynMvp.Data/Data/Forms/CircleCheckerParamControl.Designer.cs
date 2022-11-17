namespace DynMvp.Data.Forms
{
    partial class CircleCheckeParamControl
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.outterRadius = new System.Windows.Forms.NumericUpDown();
            this.labelOutterRadius = new System.Windows.Forms.Label();
            this.innerRadius = new System.Windows.Forms.NumericUpDown();
            this.labelInnerRadius = new System.Windows.Forms.Label();
            this.comboBoxEdgeType = new System.Windows.Forms.ComboBox();
            this.labelEdgeType = new System.Windows.Forms.Label();
            this.checkBoxUseImageCenter = new System.Windows.Forms.CheckBox();
            this.checkBoxShowOffset = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.outterRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.innerRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // outterRadius
            // 
            this.outterRadius.Location = new System.Drawing.Point(254, 51);
            this.outterRadius.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outterRadius.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.outterRadius.Name = "outterRadius";
            this.outterRadius.Size = new System.Drawing.Size(70, 29);
            this.outterRadius.TabIndex = 19;
            this.outterRadius.ValueChanged += new System.EventHandler(this.outterRadius_ValueChanged);
            // 
            // labelOutterRadius
            // 
            this.labelOutterRadius.AutoSize = true;
            this.labelOutterRadius.Location = new System.Drawing.Point(21, 54);
            this.labelOutterRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOutterRadius.Name = "labelOutterRadius";
            this.labelOutterRadius.Size = new System.Drawing.Size(112, 21);
            this.labelOutterRadius.TabIndex = 17;
            this.labelOutterRadius.Text = "Outter Radius";
            // 
            // innerRadius
            // 
            this.innerRadius.Location = new System.Drawing.Point(254, 16);
            this.innerRadius.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.innerRadius.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.innerRadius.Name = "innerRadius";
            this.innerRadius.Size = new System.Drawing.Size(70, 29);
            this.innerRadius.TabIndex = 20;
            this.innerRadius.ValueChanged += new System.EventHandler(this.innerRadius_ValueChanged);
            // 
            // labelInnerRadius
            // 
            this.labelInnerRadius.AutoSize = true;
            this.labelInnerRadius.Location = new System.Drawing.Point(20, 18);
            this.labelInnerRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInnerRadius.Name = "labelInnerRadius";
            this.labelInnerRadius.Size = new System.Drawing.Size(101, 21);
            this.labelInnerRadius.TabIndex = 18;
            this.labelInnerRadius.Text = "Inner Radius";
            // 
            // comboBoxEdgeType
            // 
            this.comboBoxEdgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEdgeType.FormattingEnabled = true;
            this.comboBoxEdgeType.Items.AddRange(new object[] {
            "DarkToLight",
            "LightToDark",
            "Any"});
            this.comboBoxEdgeType.Location = new System.Drawing.Point(203, 86);
            this.comboBoxEdgeType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxEdgeType.Name = "comboBoxEdgeType";
            this.comboBoxEdgeType.Size = new System.Drawing.Size(121, 29);
            this.comboBoxEdgeType.TabIndex = 21;
            this.comboBoxEdgeType.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdgeType_SelectedIndexChanged);
            // 
            // labelEdgeType
            // 
            this.labelEdgeType.AutoSize = true;
            this.labelEdgeType.Location = new System.Drawing.Point(20, 89);
            this.labelEdgeType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEdgeType.Name = "labelEdgeType";
            this.labelEdgeType.Size = new System.Drawing.Size(89, 21);
            this.labelEdgeType.TabIndex = 17;
            this.labelEdgeType.Text = "Edge Type";
            // 
            // checkBoxUseImageCenter
            // 
            this.checkBoxUseImageCenter.AutoSize = true;
            this.checkBoxUseImageCenter.Location = new System.Drawing.Point(24, 127);
            this.checkBoxUseImageCenter.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseImageCenter.Name = "checkBoxUseImageCenter";
            this.checkBoxUseImageCenter.Size = new System.Drawing.Size(162, 25);
            this.checkBoxUseImageCenter.TabIndex = 25;
            this.checkBoxUseImageCenter.Text = "Use Image Center";
            this.checkBoxUseImageCenter.UseVisualStyleBackColor = true;
            this.checkBoxUseImageCenter.CheckedChanged += new System.EventHandler(this.checkBoxUseImageCenter_CheckedChanged);
            // 
            // checkBoxShowOffset
            // 
            this.checkBoxShowOffset.AutoSize = true;
            this.checkBoxShowOffset.Location = new System.Drawing.Point(24, 156);
            this.checkBoxShowOffset.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxShowOffset.Name = "checkBoxShowOffset";
            this.checkBoxShowOffset.Size = new System.Drawing.Size(119, 25);
            this.checkBoxShowOffset.TabIndex = 25;
            this.checkBoxShowOffset.Text = "Show Offset";
            this.checkBoxShowOffset.UseVisualStyleBackColor = true;
            this.checkBoxShowOffset.CheckedChanged += new System.EventHandler(this.checkBoxShowOffset_CheckedChanged);
            // 
            // CircleCheckeParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxShowOffset);
            this.Controls.Add(this.checkBoxUseImageCenter);
            this.Controls.Add(this.comboBoxEdgeType);
            this.Controls.Add(this.outterRadius);
            this.Controls.Add(this.labelEdgeType);
            this.Controls.Add(this.labelOutterRadius);
            this.Controls.Add(this.innerRadius);
            this.Controls.Add(this.labelInnerRadius);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CircleCheckeParamControl";
            this.Size = new System.Drawing.Size(347, 193);
            ((System.ComponentModel.ISupportInitialize)(this.outterRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.innerRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown outterRadius;
        private System.Windows.Forms.Label labelOutterRadius;
        private System.Windows.Forms.NumericUpDown innerRadius;
        private System.Windows.Forms.Label labelInnerRadius;
        private System.Windows.Forms.ComboBox comboBoxEdgeType;
        private System.Windows.Forms.Label labelEdgeType;
        private System.Windows.Forms.CheckBox checkBoxUseImageCenter;
        private System.Windows.Forms.CheckBox checkBoxShowOffset;
    }
}
