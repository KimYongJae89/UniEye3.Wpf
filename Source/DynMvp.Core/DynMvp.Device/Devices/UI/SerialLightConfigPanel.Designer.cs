namespace DynMvp.Devices.UI
{
    partial class SerialLightConfigPanel
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
            this.portList = new System.Windows.Forms.DataGridView();
            this.buttonEditPort = new System.Windows.Forms.Button();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPortInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.portList)).BeginInit();
            this.SuspendLayout();
            // 
            // portList
            // 
            this.portList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.portList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo,
            this.ColumnPortInfo});
            this.portList.Location = new System.Drawing.Point(10, 10);
            this.portList.Name = "portList";
            this.portList.RowHeadersVisible = false;
            this.portList.RowTemplate.Height = 23;
            this.portList.Size = new System.Drawing.Size(190, 251);
            this.portList.TabIndex = 1;
            // 
            // buttonEditPort
            // 
            this.buttonEditPort.Location = new System.Drawing.Point(206, 10);
            this.buttonEditPort.Name = "buttonEditPort";
            this.buttonEditPort.Size = new System.Drawing.Size(102, 43);
            this.buttonEditPort.TabIndex = 2;
            this.buttonEditPort.Text = "Edit";
            this.buttonEditPort.UseVisualStyleBackColor = true;
            // 
            // ColumnNo
            // 
            this.ColumnNo.HeaderText = "No";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.Width = 50;
            // 
            // ColumnPortInfo
            // 
            this.ColumnPortInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPortInfo.HeaderText = "Port Info";
            this.ColumnPortInfo.Name = "ColumnPortInfo";
            // 
            // SerialLightConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonEditPort);
            this.Controls.Add(this.portList);
            this.Name = "SerialLightConfigPanel";
            this.Size = new System.Drawing.Size(322, 276);
            ((System.ComponentModel.ISupportInitialize)(this.portList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView portList;
        private System.Windows.Forms.Button buttonEditPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPortInfo;
    }
}
