namespace DynMvp.Data.UI
{
    partial class InspectionResultGridView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.defectGridResultList = new System.Windows.Forms.DataGridView();
            this.ColumnInspectionStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTargetGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProbe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProbeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridCurrentResultValue = new System.Windows.Forms.DataGridView();
            this.ColumnValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStandard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlResultValue = new System.Windows.Forms.TabControl();
            this.tabPageCurrentResultValue = new System.Windows.Forms.TabPage();
            this.tabPageLastResultValue = new System.Windows.Forms.TabPage();
            this.dataGridLastResultValue = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.defectGridResultList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCurrentResultValue)).BeginInit();
            this.tabControlResultValue.SuspendLayout();
            this.tabPageCurrentResultValue.SuspendLayout();
            this.tabPageLastResultValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLastResultValue)).BeginInit();
            this.SuspendLayout();
            // 
            // defectGridResultList
            // 
            this.defectGridResultList.AllowUserToAddRows = false;
            this.defectGridResultList.AllowUserToDeleteRows = false;
            this.defectGridResultList.AllowUserToResizeRows = false;
            this.defectGridResultList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectGridResultList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnInspectionStep,
            this.ColumnTargetGroup,
            this.ColumnTarget,
            this.ColumnProbe,
            this.ColumnName,
            this.ColumnProbeType,
            this.ColumnResult});
            this.defectGridResultList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectGridResultList.Location = new System.Drawing.Point(0, 0);
            this.defectGridResultList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.defectGridResultList.MultiSelect = false;
            this.defectGridResultList.Name = "defectGridResultList";
            this.defectGridResultList.ReadOnly = true;
            this.defectGridResultList.RowHeadersVisible = false;
            this.defectGridResultList.RowTemplate.Height = 30;
            this.defectGridResultList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectGridResultList.Size = new System.Drawing.Size(422, 120);
            this.defectGridResultList.TabIndex = 0;
            this.defectGridResultList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.defectGridView_CellClick);
            // 
            // ColumnInspectionStep
            // 
            this.ColumnInspectionStep.HeaderText = "I";
            this.ColumnInspectionStep.Name = "ColumnInspectionStep";
            this.ColumnInspectionStep.ReadOnly = true;
            this.ColumnInspectionStep.Width = 30;
            // 
            // ColumnTargetGroup
            // 
            this.ColumnTargetGroup.HeaderText = "G";
            this.ColumnTargetGroup.Name = "ColumnTargetGroup";
            this.ColumnTargetGroup.ReadOnly = true;
            this.ColumnTargetGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTargetGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnTargetGroup.Width = 30;
            // 
            // ColumnTarget
            // 
            this.ColumnTarget.HeaderText = "T";
            this.ColumnTarget.Name = "ColumnTarget";
            this.ColumnTarget.ReadOnly = true;
            this.ColumnTarget.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTarget.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnTarget.Width = 30;
            // 
            // ColumnProbe
            // 
            this.ColumnProbe.HeaderText = "P";
            this.ColumnProbe.Name = "ColumnProbe";
            this.ColumnProbe.ReadOnly = true;
            this.ColumnProbe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnProbe.Width = 30;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnProbeType
            // 
            this.ColumnProbeType.HeaderText = "Type";
            this.ColumnProbeType.Name = "ColumnProbeType";
            this.ColumnProbeType.ReadOnly = true;
            this.ColumnProbeType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnResult
            // 
            this.ColumnResult.HeaderText = "Result";
            this.ColumnResult.Name = "ColumnResult";
            this.ColumnResult.ReadOnly = true;
            this.ColumnResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridCurrentResultValue
            // 
            this.dataGridCurrentResultValue.AllowUserToAddRows = false;
            this.dataGridCurrentResultValue.AllowUserToDeleteRows = false;
            this.dataGridCurrentResultValue.AllowUserToResizeRows = false;
            this.dataGridCurrentResultValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCurrentResultValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValueName,
            this.ColumnValue,
            this.ColumnStandard});
            this.dataGridCurrentResultValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridCurrentResultValue.Location = new System.Drawing.Point(2, 2);
            this.dataGridCurrentResultValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridCurrentResultValue.Name = "dataGridCurrentResultValue";
            this.dataGridCurrentResultValue.RowHeadersVisible = false;
            this.dataGridCurrentResultValue.RowTemplate.Height = 23;
            this.dataGridCurrentResultValue.Size = new System.Drawing.Size(410, 154);
            this.dataGridCurrentResultValue.TabIndex = 0;
            // 
            // ColumnValueName
            // 
            this.ColumnValueName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnValueName.HeaderText = "Name";
            this.ColumnValueName.Name = "ColumnValueName";
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            // 
            // ColumnStandard
            // 
            this.ColumnStandard.HeaderText = "Standard";
            this.ColumnStandard.Name = "ColumnStandard";
            this.ColumnStandard.Width = 150;
            // 
            // tabControlResultValue
            // 
            this.tabControlResultValue.Controls.Add(this.tabPageCurrentResultValue);
            this.tabControlResultValue.Controls.Add(this.tabPageLastResultValue);
            this.tabControlResultValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControlResultValue.Location = new System.Drawing.Point(0, 120);
            this.tabControlResultValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControlResultValue.Name = "tabControlResultValue";
            this.tabControlResultValue.SelectedIndex = 0;
            this.tabControlResultValue.Size = new System.Drawing.Size(422, 191);
            this.tabControlResultValue.TabIndex = 1;
            // 
            // tabPageCurrentResultValue
            // 
            this.tabPageCurrentResultValue.Controls.Add(this.dataGridCurrentResultValue);
            this.tabPageCurrentResultValue.Location = new System.Drawing.Point(4, 29);
            this.tabPageCurrentResultValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCurrentResultValue.Name = "tabPageCurrentResultValue";
            this.tabPageCurrentResultValue.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCurrentResultValue.Size = new System.Drawing.Size(414, 158);
            this.tabPageCurrentResultValue.TabIndex = 0;
            this.tabPageCurrentResultValue.Text = "Current";
            this.tabPageCurrentResultValue.UseVisualStyleBackColor = true;
            // 
            // tabPageLastResultValue
            // 
            this.tabPageLastResultValue.Controls.Add(this.dataGridLastResultValue);
            this.tabPageLastResultValue.Location = new System.Drawing.Point(4, 33);
            this.tabPageLastResultValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageLastResultValue.Name = "tabPageLastResultValue";
            this.tabPageLastResultValue.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageLastResultValue.Size = new System.Drawing.Size(414, 154);
            this.tabPageLastResultValue.TabIndex = 1;
            this.tabPageLastResultValue.Text = "Last";
            this.tabPageLastResultValue.UseVisualStyleBackColor = true;
            // 
            // dataGridLastResultValue
            // 
            this.dataGridLastResultValue.AllowUserToAddRows = false;
            this.dataGridLastResultValue.AllowUserToDeleteRows = false;
            this.dataGridLastResultValue.AllowUserToResizeRows = false;
            this.dataGridLastResultValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLastResultValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridLastResultValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridLastResultValue.Location = new System.Drawing.Point(2, 2);
            this.dataGridLastResultValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridLastResultValue.Name = "dataGridLastResultValue";
            this.dataGridLastResultValue.RowHeadersVisible = false;
            this.dataGridLastResultValue.RowTemplate.Height = 23;
            this.dataGridLastResultValue.Size = new System.Drawing.Size(410, 150);
            this.dataGridLastResultValue.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Standard";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // InspectionResultGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.defectGridResultList);
            this.Controls.Add(this.tabControlResultValue);
            this.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "InspectionResultGridView";
            this.Size = new System.Drawing.Size(422, 311);
            this.Load += new System.EventHandler(this.InspectionResultForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.defectGridResultList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCurrentResultValue)).EndInit();
            this.tabControlResultValue.ResumeLayout(false);
            this.tabPageCurrentResultValue.ResumeLayout(false);
            this.tabPageLastResultValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLastResultValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView defectGridResultList;
        private System.Windows.Forms.DataGridView dataGridCurrentResultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValueName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStandard;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInspectionStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTargetGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProbe;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProbeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnResult;
        private System.Windows.Forms.TabControl tabControlResultValue;
        private System.Windows.Forms.TabPage tabPageCurrentResultValue;
        private System.Windows.Forms.TabPage tabPageLastResultValue;
        private System.Windows.Forms.DataGridView dataGridLastResultValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}