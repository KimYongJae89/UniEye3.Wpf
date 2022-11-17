namespace DynMvp.Devices.Dio.UI
{
    partial class PortSettingPanel
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
            this.groupBoxOutPort = new System.Windows.Forms.GroupBox();
            this.numOutPort = new System.Windows.Forms.NumericUpDown();
            this.labelNumOutPort = new System.Windows.Forms.Label();
            this.outPortStartGroupIndex = new System.Windows.Forms.NumericUpDown();
            this.labelOutPortStartGroupIndex = new System.Windows.Forms.Label();
            this.labelNumOutPortGroup = new System.Windows.Forms.Label();
            this.numOutPortGroup = new System.Windows.Forms.NumericUpDown();
            this.groupBoxInPort = new System.Windows.Forms.GroupBox();
            this.numInPort = new System.Windows.Forms.NumericUpDown();
            this.labelNumInPort = new System.Windows.Forms.Label();
            this.inPortStartGroupIndex = new System.Windows.Forms.NumericUpDown();
            this.labelInPortStartGroupIndex = new System.Windows.Forms.Label();
            this.numInPortGroup = new System.Windows.Forms.NumericUpDown();
            this.labelNumInPortGroup = new System.Windows.Forms.Label();
            this.groupBoxOutPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outPortStartGroupIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPortGroup)).BeginInit();
            this.groupBoxInPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inPortStartGroupIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInPortGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxOutPort
            // 
            this.groupBoxOutPort.Controls.Add(this.numOutPort);
            this.groupBoxOutPort.Controls.Add(this.labelNumOutPort);
            this.groupBoxOutPort.Controls.Add(this.outPortStartGroupIndex);
            this.groupBoxOutPort.Controls.Add(this.labelOutPortStartGroupIndex);
            this.groupBoxOutPort.Controls.Add(this.labelNumOutPortGroup);
            this.groupBoxOutPort.Controls.Add(this.numOutPortGroup);
            this.groupBoxOutPort.Location = new System.Drawing.Point(4, 151);
            this.groupBoxOutPort.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxOutPort.Name = "groupBoxOutPort";
            this.groupBoxOutPort.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxOutPort.Size = new System.Drawing.Size(291, 142);
            this.groupBoxOutPort.TabIndex = 181;
            this.groupBoxOutPort.TabStop = false;
            this.groupBoxOutPort.Text = "Output Port";
            // 
            // numOutPort
            // 
            this.numOutPort.Location = new System.Drawing.Point(204, 104);
            this.numOutPort.Margin = new System.Windows.Forms.Padding(4);
            this.numOutPort.Name = "numOutPort";
            this.numOutPort.Size = new System.Drawing.Size(69, 24);
            this.numOutPort.TabIndex = 175;
            // 
            // labelNumOutPort
            // 
            this.labelNumOutPort.AutoSize = true;
            this.labelNumOutPort.Location = new System.Drawing.Point(24, 103);
            this.labelNumOutPort.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumOutPort.Name = "labelNumOutPort";
            this.labelNumOutPort.Size = new System.Drawing.Size(110, 18);
            this.labelNumOutPort.TabIndex = 158;
            this.labelNumOutPort.Text = "Number of Port";
            // 
            // outPortStartGroupIndex
            // 
            this.outPortStartGroupIndex.Location = new System.Drawing.Point(204, 66);
            this.outPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(4);
            this.outPortStartGroupIndex.Name = "outPortStartGroupIndex";
            this.outPortStartGroupIndex.Size = new System.Drawing.Size(69, 24);
            this.outPortStartGroupIndex.TabIndex = 175;
            // 
            // labelOutPortStartGroupIndex
            // 
            this.labelOutPortStartGroupIndex.AutoSize = true;
            this.labelOutPortStartGroupIndex.Location = new System.Drawing.Point(24, 67);
            this.labelOutPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelOutPortStartGroupIndex.Name = "labelOutPortStartGroupIndex";
            this.labelOutPortStartGroupIndex.Size = new System.Drawing.Size(123, 18);
            this.labelOutPortStartGroupIndex.TabIndex = 158;
            this.labelOutPortStartGroupIndex.Text = "Start Group Index";
            // 
            // labelNumOutPortGroup
            // 
            this.labelNumOutPortGroup.AutoSize = true;
            this.labelNumOutPortGroup.Location = new System.Drawing.Point(24, 31);
            this.labelNumOutPortGroup.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumOutPortGroup.Name = "labelNumOutPortGroup";
            this.labelNumOutPortGroup.Size = new System.Drawing.Size(124, 18);
            this.labelNumOutPortGroup.TabIndex = 158;
            this.labelNumOutPortGroup.Text = "Number of Group";
            // 
            // numOutPortGroup
            // 
            this.numOutPortGroup.Location = new System.Drawing.Point(204, 28);
            this.numOutPortGroup.Margin = new System.Windows.Forms.Padding(4);
            this.numOutPortGroup.Name = "numOutPortGroup";
            this.numOutPortGroup.Size = new System.Drawing.Size(69, 24);
            this.numOutPortGroup.TabIndex = 175;
            // 
            // groupBoxInPort
            // 
            this.groupBoxInPort.Controls.Add(this.numInPort);
            this.groupBoxInPort.Controls.Add(this.labelNumInPort);
            this.groupBoxInPort.Controls.Add(this.inPortStartGroupIndex);
            this.groupBoxInPort.Controls.Add(this.labelInPortStartGroupIndex);
            this.groupBoxInPort.Controls.Add(this.numInPortGroup);
            this.groupBoxInPort.Controls.Add(this.labelNumInPortGroup);
            this.groupBoxInPort.Location = new System.Drawing.Point(4, 8);
            this.groupBoxInPort.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxInPort.Name = "groupBoxInPort";
            this.groupBoxInPort.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxInPort.Size = new System.Drawing.Size(291, 132);
            this.groupBoxInPort.TabIndex = 180;
            this.groupBoxInPort.TabStop = false;
            this.groupBoxInPort.Text = "Input Port";
            // 
            // numInPort
            // 
            this.numInPort.Location = new System.Drawing.Point(207, 97);
            this.numInPort.Margin = new System.Windows.Forms.Padding(4);
            this.numInPort.Name = "numInPort";
            this.numInPort.Size = new System.Drawing.Size(66, 24);
            this.numInPort.TabIndex = 175;
            // 
            // labelNumInPort
            // 
            this.labelNumInPort.AutoSize = true;
            this.labelNumInPort.Location = new System.Drawing.Point(28, 99);
            this.labelNumInPort.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumInPort.Name = "labelNumInPort";
            this.labelNumInPort.Size = new System.Drawing.Size(110, 18);
            this.labelNumInPort.TabIndex = 158;
            this.labelNumInPort.Text = "Number of Port";
            // 
            // inPortStartGroupIndex
            // 
            this.inPortStartGroupIndex.Location = new System.Drawing.Point(207, 63);
            this.inPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(4);
            this.inPortStartGroupIndex.Name = "inPortStartGroupIndex";
            this.inPortStartGroupIndex.Size = new System.Drawing.Size(66, 24);
            this.inPortStartGroupIndex.TabIndex = 175;
            // 
            // labelInPortStartGroupIndex
            // 
            this.labelInPortStartGroupIndex.AutoSize = true;
            this.labelInPortStartGroupIndex.Location = new System.Drawing.Point(28, 65);
            this.labelInPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelInPortStartGroupIndex.Name = "labelInPortStartGroupIndex";
            this.labelInPortStartGroupIndex.Size = new System.Drawing.Size(123, 18);
            this.labelInPortStartGroupIndex.TabIndex = 158;
            this.labelInPortStartGroupIndex.Text = "Start Group Index";
            // 
            // numInPortGroup
            // 
            this.numInPortGroup.Location = new System.Drawing.Point(207, 29);
            this.numInPortGroup.Margin = new System.Windows.Forms.Padding(4);
            this.numInPortGroup.Name = "numInPortGroup";
            this.numInPortGroup.Size = new System.Drawing.Size(66, 24);
            this.numInPortGroup.TabIndex = 175;
            // 
            // labelNumInPortGroup
            // 
            this.labelNumInPortGroup.AutoSize = true;
            this.labelNumInPortGroup.Location = new System.Drawing.Point(28, 31);
            this.labelNumInPortGroup.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumInPortGroup.Name = "labelNumInPortGroup";
            this.labelNumInPortGroup.Size = new System.Drawing.Size(124, 18);
            this.labelNumInPortGroup.TabIndex = 158;
            this.labelNumInPortGroup.Text = "Number of Group";
            // 
            // PortSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxOutPort);
            this.Controls.Add(this.groupBoxInPort);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PortSettingPanel";
            this.Size = new System.Drawing.Size(300, 300);
            this.groupBoxOutPort.ResumeLayout(false);
            this.groupBoxOutPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outPortStartGroupIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPortGroup)).EndInit();
            this.groupBoxInPort.ResumeLayout(false);
            this.groupBoxInPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inPortStartGroupIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInPortGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOutPort;
        private System.Windows.Forms.NumericUpDown numOutPort;
        private System.Windows.Forms.Label labelNumOutPort;
        private System.Windows.Forms.NumericUpDown outPortStartGroupIndex;
        private System.Windows.Forms.Label labelOutPortStartGroupIndex;
        private System.Windows.Forms.Label labelNumOutPortGroup;
        private System.Windows.Forms.NumericUpDown numOutPortGroup;
        private System.Windows.Forms.GroupBox groupBoxInPort;
        private System.Windows.Forms.NumericUpDown numInPort;
        private System.Windows.Forms.Label labelNumInPort;
        private System.Windows.Forms.NumericUpDown inPortStartGroupIndex;
        private System.Windows.Forms.Label labelInPortStartGroupIndex;
        private System.Windows.Forms.NumericUpDown numInPortGroup;
        private System.Windows.Forms.Label labelNumInPortGroup;
    }
}
