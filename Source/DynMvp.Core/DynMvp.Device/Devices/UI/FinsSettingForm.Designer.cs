namespace DynMvp.Devices.UI
{
    partial class FinsSettingForm
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.labelIpAddress = new System.Windows.Forms.Label();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.portNo = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.labelPlcStateAddress = new System.Windows.Forms.Label();
            this.plcStateAddress = new System.Windows.Forms.NumericUpDown();
            this.labelPcStateAddress = new System.Windows.Forms.Label();
            this.pcStateAddress = new System.Windows.Forms.NumericUpDown();
            this.labelResultAddress = new System.Windows.Forms.Label();
            this.resultAddress = new System.Windows.Forms.NumericUpDown();
            this.labelNetworkNo = new System.Windows.Forms.Label();
            this.networkNo = new System.Windows.Forms.NumericUpDown();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.FinsSettingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.portNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcStateAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcStateAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.networkNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.FinsSettingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.FinsSettingForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.AutoSize = true;
            this.labelIpAddress.Location = new System.Drawing.Point(14, 14);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(67, 12);
            this.labelIpAddress.TabIndex = 1;
            this.labelIpAddress.Text = "IP Address";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(14, 37);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(47, 12);
            this.labelPortNo.TabIndex = 1;
            this.labelPortNo.Text = "Port No";
            // 
            // portNo
            // 
            this.portNo.Location = new System.Drawing.Point(136, 33);
            this.portNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.portNo.Name = "portNo";
            this.portNo.Size = new System.Drawing.Size(70, 21);
            this.portNo.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(126, 162);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 34);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(48, 162);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 34);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(134, 8);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(105, 21);
            this.ipAddress.TabIndex = 5;
            // 
            // labelPlcStateAddress
            // 
            this.labelPlcStateAddress.AutoSize = true;
            this.labelPlcStateAddress.Location = new System.Drawing.Point(14, 88);
            this.labelPlcStateAddress.Name = "labelPlcStateAddress";
            this.labelPlcStateAddress.Size = new System.Drawing.Size(112, 12);
            this.labelPlcStateAddress.TabIndex = 1;
            this.labelPlcStateAddress.Text = "PLC State Address";
            // 
            // plcStateAddress
            // 
            this.plcStateAddress.Location = new System.Drawing.Point(136, 84);
            this.plcStateAddress.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.plcStateAddress.Name = "plcStateAddress";
            this.plcStateAddress.Size = new System.Drawing.Size(70, 21);
            this.plcStateAddress.TabIndex = 2;
            // 
            // labelPcStateAddress
            // 
            this.labelPcStateAddress.AutoSize = true;
            this.labelPcStateAddress.Location = new System.Drawing.Point(14, 114);
            this.labelPcStateAddress.Name = "labelPcStateAddress";
            this.labelPcStateAddress.Size = new System.Drawing.Size(105, 12);
            this.labelPcStateAddress.TabIndex = 1;
            this.labelPcStateAddress.Text = "PC State Address";
            // 
            // pcStateAddress
            // 
            this.pcStateAddress.Location = new System.Drawing.Point(136, 110);
            this.pcStateAddress.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.pcStateAddress.Name = "pcStateAddress";
            this.pcStateAddress.Size = new System.Drawing.Size(70, 21);
            this.pcStateAddress.TabIndex = 2;
            // 
            // labelResultAddress
            // 
            this.labelResultAddress.AutoSize = true;
            this.labelResultAddress.Location = new System.Drawing.Point(14, 140);
            this.labelResultAddress.Name = "labelResultAddress";
            this.labelResultAddress.Size = new System.Drawing.Size(91, 12);
            this.labelResultAddress.TabIndex = 1;
            this.labelResultAddress.Text = "Result Address";
            // 
            // resultAddress
            // 
            this.resultAddress.Location = new System.Drawing.Point(136, 136);
            this.resultAddress.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.resultAddress.Name = "resultAddress";
            this.resultAddress.Size = new System.Drawing.Size(70, 21);
            this.resultAddress.TabIndex = 2;
            // 
            // labelNetworkNo
            // 
            this.labelNetworkNo.AutoSize = true;
            this.labelNetworkNo.Location = new System.Drawing.Point(14, 63);
            this.labelNetworkNo.Name = "labelNetworkNo";
            this.labelNetworkNo.Size = new System.Drawing.Size(71, 12);
            this.labelNetworkNo.TabIndex = 1;
            this.labelNetworkNo.Text = "Network No";
            // 
            // networkNo
            // 
            this.networkNo.Location = new System.Drawing.Point(136, 59);
            this.networkNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.networkNo.Name = "networkNo";
            this.networkNo.Size = new System.Drawing.Size(70, 21);
            this.networkNo.TabIndex = 2;
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(247, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 232);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(247, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 201);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(246, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 201);
            // 
            // FinsSettingForm_Fill_Panel
            // 
            // 
            // FinsSettingForm_Fill_Panel.ClientArea
            // 
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.ipAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnCancel);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.resultAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelResultAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.pcStateAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelPcStateAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.plcStateAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelPlcStateAddress);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.networkNo);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelNetworkNo);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.portNo);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelPortNo);
            this.FinsSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelIpAddress);
            this.FinsSettingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FinsSettingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FinsSettingForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.FinsSettingForm_Fill_Panel.Name = "FinsSettingForm_Fill_Panel";
            this.FinsSettingForm_Fill_Panel.Size = new System.Drawing.Size(245, 201);
            this.FinsSettingForm_Fill_Panel.TabIndex = 14;
            // 
            // FinsSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 233);
            this.Controls.Add(this.FinsSettingForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Name = "FinsSettingForm";
            this.Text = "FINS Setting";
            this.Load += new System.EventHandler(this.FinsSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.portNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcStateAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcStateAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.networkNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.FinsSettingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.FinsSettingForm_Fill_Panel.ClientArea.PerformLayout();
            this.FinsSettingForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.NumericUpDown portNo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.Label labelPlcStateAddress;
        private System.Windows.Forms.NumericUpDown plcStateAddress;
        private System.Windows.Forms.Label labelPcStateAddress;
        private System.Windows.Forms.NumericUpDown pcStateAddress;
        private System.Windows.Forms.Label labelResultAddress;
        private System.Windows.Forms.NumericUpDown resultAddress;
        private System.Windows.Forms.Label labelNetworkNo;
        private System.Windows.Forms.NumericUpDown networkNo;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel FinsSettingForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}