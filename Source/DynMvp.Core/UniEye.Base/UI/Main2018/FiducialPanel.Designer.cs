namespace UniEye.Base.UI.Main2018
{
    partial class FiducialPanel
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
            this.fidAngle = new System.Windows.Forms.TextBox();
            this.fidOffset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFidDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDesiredDistance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFidAngle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFidOffset = new System.Windows.Forms.Label();
            this.labelDesiredDistance = new System.Windows.Forms.Label();
            this.labelFidDistance = new System.Windows.Forms.Label();
            this.fidDistanceTol = new System.Windows.Forms.NumericUpDown();
            this.labelDistanceOffset = new System.Windows.Forms.Label();
            this.labelUm = new System.Windows.Forms.Label();
            this.buttonAlign = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fidDistanceTol)).BeginInit();
            this.SuspendLayout();
            // 
            // fidAngle
            // 
            this.fidAngle.Location = new System.Drawing.Point(154, 122);
            this.fidAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fidAngle.Name = "fidAngle";
            this.fidAngle.ReadOnly = true;
            this.fidAngle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidAngle.Size = new System.Drawing.Size(100, 25);
            this.fidAngle.TabIndex = 184;
            // 
            // fidOffset
            // 
            this.fidOffset.Location = new System.Drawing.Point(154, 94);
            this.fidOffset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fidOffset.Name = "fidOffset";
            this.fidOffset.ReadOnly = true;
            this.fidOffset.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidOffset.Size = new System.Drawing.Size(100, 25);
            this.fidOffset.TabIndex = 183;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 122);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 17);
            this.label3.TabIndex = 179;
            this.label3.Text = "˚";
            // 
            // textBoxFidDistance
            // 
            this.textBoxFidDistance.Location = new System.Drawing.Point(154, 65);
            this.textBoxFidDistance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxFidDistance.Name = "textBoxFidDistance";
            this.textBoxFidDistance.ReadOnly = true;
            this.textBoxFidDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxFidDistance.Size = new System.Drawing.Size(100, 25);
            this.textBoxFidDistance.TabIndex = 182;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 94);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 17);
            this.label2.TabIndex = 178;
            this.label2.Text = "um";
            // 
            // textBoxDesiredDistance
            // 
            this.textBoxDesiredDistance.Location = new System.Drawing.Point(154, 36);
            this.textBoxDesiredDistance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxDesiredDistance.Name = "textBoxDesiredDistance";
            this.textBoxDesiredDistance.ReadOnly = true;
            this.textBoxDesiredDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxDesiredDistance.Size = new System.Drawing.Size(100, 25);
            this.textBoxDesiredDistance.TabIndex = 181;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 65);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 17);
            this.label5.TabIndex = 180;
            this.label5.Text = "um";
            // 
            // labelFidAngle
            // 
            this.labelFidAngle.AutoSize = true;
            this.labelFidAngle.Location = new System.Drawing.Point(10, 122);
            this.labelFidAngle.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidAngle.Name = "labelFidAngle";
            this.labelFidAngle.Size = new System.Drawing.Size(68, 17);
            this.labelFidAngle.TabIndex = 175;
            this.labelFidAngle.Text = "Fid. Angle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 17);
            this.label4.TabIndex = 177;
            this.label4.Text = "um";
            // 
            // labelFidOffset
            // 
            this.labelFidOffset.AutoSize = true;
            this.labelFidOffset.Location = new System.Drawing.Point(10, 94);
            this.labelFidOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidOffset.Name = "labelFidOffset";
            this.labelFidOffset.Size = new System.Drawing.Size(68, 17);
            this.labelFidOffset.TabIndex = 174;
            this.labelFidOffset.Text = "Fid. Offset";
            // 
            // labelDesiredDistance
            // 
            this.labelDesiredDistance.AutoSize = true;
            this.labelDesiredDistance.Location = new System.Drawing.Point(10, 40);
            this.labelDesiredDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDesiredDistance.Name = "labelDesiredDistance";
            this.labelDesiredDistance.Size = new System.Drawing.Size(108, 17);
            this.labelDesiredDistance.TabIndex = 176;
            this.labelDesiredDistance.Text = "Desired Distance";
            // 
            // labelFidDistance
            // 
            this.labelFidDistance.AutoSize = true;
            this.labelFidDistance.Location = new System.Drawing.Point(10, 65);
            this.labelFidDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidDistance.Name = "labelFidDistance";
            this.labelFidDistance.Size = new System.Drawing.Size(83, 17);
            this.labelFidDistance.TabIndex = 173;
            this.labelFidDistance.Text = "Fid. Distance";
            // 
            // fidDistanceTol
            // 
            this.fidDistanceTol.Location = new System.Drawing.Point(154, 6);
            this.fidDistanceTol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fidDistanceTol.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.fidDistanceTol.Name = "fidDistanceTol";
            this.fidDistanceTol.Size = new System.Drawing.Size(100, 25);
            this.fidDistanceTol.TabIndex = 172;
            this.fidDistanceTol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fidDistanceTol.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fidDistanceTol.ValueChanged += new System.EventHandler(this.fidDistanceTol_ValueChanged);
            // 
            // labelDistanceOffset
            // 
            this.labelDistanceOffset.AutoSize = true;
            this.labelDistanceOffset.Location = new System.Drawing.Point(10, 9);
            this.labelDistanceOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDistanceOffset.Name = "labelDistanceOffset";
            this.labelDistanceOffset.Size = new System.Drawing.Size(98, 17);
            this.labelDistanceOffset.TabIndex = 171;
            this.labelDistanceOffset.Text = "Distance Offset";
            // 
            // labelUm
            // 
            this.labelUm.AutoSize = true;
            this.labelUm.Location = new System.Drawing.Point(254, 9);
            this.labelUm.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelUm.Name = "labelUm";
            this.labelUm.Size = new System.Drawing.Size(27, 17);
            this.labelUm.TabIndex = 169;
            this.labelUm.Text = "um";
            // 
            // buttonAlign
            // 
            this.buttonAlign.Location = new System.Drawing.Point(16, 155);
            this.buttonAlign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonAlign.Name = "buttonAlign";
            this.buttonAlign.Size = new System.Drawing.Size(102, 41);
            this.buttonAlign.TabIndex = 170;
            this.buttonAlign.Text = "Align";
            this.buttonAlign.UseVisualStyleBackColor = true;
            this.buttonAlign.Click += new System.EventHandler(this.buttonAlign_Click);
            // 
            // FiducialPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fidAngle);
            this.Controls.Add(this.fidOffset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxFidDistance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDesiredDistance);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelFidAngle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelFidOffset);
            this.Controls.Add(this.labelDesiredDistance);
            this.Controls.Add(this.labelFidDistance);
            this.Controls.Add(this.fidDistanceTol);
            this.Controls.Add(this.labelDistanceOffset);
            this.Controls.Add(this.labelUm);
            this.Controls.Add(this.buttonAlign);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FiducialPanel";
            this.Size = new System.Drawing.Size(299, 217);
            ((System.ComponentModel.ISupportInitialize)(this.fidDistanceTol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fidAngle;
        private System.Windows.Forms.TextBox fidOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFidDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDesiredDistance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelFidAngle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelFidOffset;
        private System.Windows.Forms.Label labelDesiredDistance;
        private System.Windows.Forms.Label labelFidDistance;
        private System.Windows.Forms.NumericUpDown fidDistanceTol;
        private System.Windows.Forms.Label labelDistanceOffset;
        private System.Windows.Forms.Label labelUm;
        private System.Windows.Forms.Button buttonAlign;
    }
}
