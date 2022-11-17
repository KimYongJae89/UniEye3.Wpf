namespace DynMvp.Devices.UI
{
    partial class IoLightConfigPanel
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
            this.portList = new System.Windows.Forms.DataGridView();
            this.ColumnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPortNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.portList)).BeginInit();
            this.SuspendLayout();
            // 
            // portList
            // 
            this.portList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.portList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNo,
            this.ColumnPortNo});
            this.portList.Location = new System.Drawing.Point(6, 6);
            this.portList.Name = "portList";
            this.portList.RowHeadersVisible = false;
            this.portList.RowTemplate.Height = 23;
            this.portList.Size = new System.Drawing.Size(190, 222);
            this.portList.TabIndex = 0;
            // 
            // ColumnNo
            // 
            this.ColumnNo.HeaderText = "No";
            this.ColumnNo.Name = "ColumnNo";
            this.ColumnNo.Width = 50;
            // 
            // ColumnPortNo
            // 
            this.ColumnPortNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPortNo.HeaderText = "Port No";
            this.ColumnPortNo.Name = "ColumnPortNo";
            // 
            // IoLightConfigPanel
            // 
            this.Controls.Add(this.portList);
            this.Name = "IoLightConfigPanel";
            this.Size = new System.Drawing.Size(364, 349);
            ((System.ComponentModel.ISupportInitialize)(this.portList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView portList;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPortNo;
    }
}