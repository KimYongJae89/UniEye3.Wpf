namespace UniEye.Base.UI.LightAutoTune
{
    partial class LightAutoTunePanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelValueList = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.valueList = new System.Windows.Forms.DataGridView();
            this.columnLightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTuneValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueList)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.labelValueList, 0, 1);
            this.layoutMain.Controls.Add(this.labelTitle, 0, 0);
            this.layoutMain.Controls.Add(this.valueList, 0, 2);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMain.Size = new System.Drawing.Size(338, 330);
            this.layoutMain.TabIndex = 0;
            // 
            // labelValueList
            // 
            this.labelValueList.AutoSize = true;
            this.labelValueList.BackColor = System.Drawing.Color.AliceBlue;
            this.labelValueList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValueList.Location = new System.Drawing.Point(3, 35);
            this.labelValueList.Name = "labelValueList";
            this.labelValueList.Size = new System.Drawing.Size(332, 35);
            this.labelValueList.TabIndex = 15;
            this.labelValueList.Text = "Value List";
            this.labelValueList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.AliceBlue;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(338, 35);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Cam #";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // valueList
            // 
            this.valueList.AllowUserToAddRows = false;
            this.valueList.AllowUserToDeleteRows = false;
            this.valueList.AllowUserToResizeColumns = false;
            this.valueList.AllowUserToResizeRows = false;
            this.valueList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.valueList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.valueList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.valueList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.valueList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLightValue,
            this.columnTuneValue});
            this.valueList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueList.Location = new System.Drawing.Point(0, 70);
            this.valueList.Margin = new System.Windows.Forms.Padding(0);
            this.valueList.MultiSelect = false;
            this.valueList.Name = "valueList";
            this.valueList.ReadOnly = true;
            this.valueList.RowHeadersVisible = false;
            this.valueList.RowTemplate.Height = 23;
            this.valueList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.valueList.Size = new System.Drawing.Size(338, 260);
            this.valueList.TabIndex = 11;
            // 
            // columnLightValue
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnLightValue.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnLightValue.HeaderText = "Light Value";
            this.columnLightValue.Name = "columnLightValue";
            this.columnLightValue.ReadOnly = true;
            this.columnLightValue.Width = 120;
            // 
            // columnTuneValue
            // 
            this.columnTuneValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.columnTuneValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnTuneValue.HeaderText = "Tune Value";
            this.columnTuneValue.Name = "columnTuneValue";
            this.columnTuneValue.ReadOnly = true;
            // 
            // AutoTunePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AutoTunePanel";
            this.Size = new System.Drawing.Size(338, 330);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelValueList;
        private System.Windows.Forms.DataGridView valueList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTuneValue;
    }
}
