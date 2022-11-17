namespace DynMvp.Devices.Devices.UI
{
    partial class TowerLampSettingForm
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
            this.dgvTowerLamp = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RedLamp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.YellowLamp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.GreenLamp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BuzzerLamp = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTowerLamp)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTowerLamp
            // 
            this.dgvTowerLamp.AllowUserToAddRows = false;
            this.dgvTowerLamp.AllowUserToResizeColumns = false;
            this.dgvTowerLamp.AllowUserToResizeRows = false;
            this.dgvTowerLamp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTowerLamp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTowerLamp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.State,
            this.RedLamp,
            this.YellowLamp,
            this.GreenLamp,
            this.BuzzerLamp});
            this.dgvTowerLamp.Location = new System.Drawing.Point(14, 12);
            this.dgvTowerLamp.Name = "dgvTowerLamp";
            this.dgvTowerLamp.RowHeadersVisible = false;
            this.dgvTowerLamp.RowTemplate.Height = 23;
            this.dgvTowerLamp.Size = new System.Drawing.Size(759, 336);
            this.dgvTowerLamp.TabIndex = 0;
            this.dgvTowerLamp.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTowerLamp_CellContentClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(589, 354);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 33);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(684, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // State
            // 
            this.State.FillWeight = 150F;
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.State.Width = 150;
            // 
            // RedLamp
            // 
            this.RedLamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RedLamp.HeaderText = "Red";
            this.RedLamp.Name = "RedLamp";
            this.RedLamp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // YellowLamp
            // 
            this.YellowLamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.YellowLamp.HeaderText = "Yellow";
            this.YellowLamp.Name = "YellowLamp";
            this.YellowLamp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // GreenLamp
            // 
            this.GreenLamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GreenLamp.HeaderText = "Green";
            this.GreenLamp.Name = "GreenLamp";
            this.GreenLamp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // BuzzerLamp
            // 
            this.BuzzerLamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BuzzerLamp.HeaderText = "Buzzer";
            this.BuzzerLamp.Name = "BuzzerLamp";
            this.BuzzerLamp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TowerLampSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 398);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvTowerLamp);
            this.Name = "TowerLampSettingForm";
            this.Text = "TowerLampSettingForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTowerLamp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTowerLamp;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewButtonColumn RedLamp;
        private System.Windows.Forms.DataGridViewButtonColumn YellowLamp;
        private System.Windows.Forms.DataGridViewButtonColumn GreenLamp;
        private System.Windows.Forms.DataGridViewButtonColumn BuzzerLamp;
    }
}