namespace DynMvp.Devices.UI
{
    partial class MelsecSettingForm
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
            this.nudPortNo = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.TcpIpSettingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.labelCpuInspectorData = new System.Windows.Forms.Label();
            this.labelModuleDeviceNo = new System.Windows.Forms.Label();
            this.labelModuleIoNo = new System.Windows.Forms.Label();
            this.labelPlcNo = new System.Windows.Forms.Label();
            this.labelNetworkNo = new System.Windows.Forms.Label();
            this.nudNetworkNo = new System.Windows.Forms.NumericUpDown();
            this.nudPlcNo = new System.Windows.Forms.NumericUpDown();
            this.nudModuleIoNo = new System.Windows.Forms.NumericUpDown();
            this.nudModuleDeviceNo = new System.Windows.Forms.NumericUpDown();
            this.nudCpuInspectorData = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudPortNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.TcpIpSettingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.TcpIpSettingForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNetworkNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlcNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModuleIoNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModuleDeviceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCpuInspectorData)).BeginInit();
            this.SuspendLayout();
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.AutoSize = true;
            this.labelIpAddress.Location = new System.Drawing.Point(11, 16);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(73, 17);
            this.labelIpAddress.TabIndex = 1;
            this.labelIpAddress.Text = "IP Address";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(11, 44);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(55, 17);
            this.labelPortNo.TabIndex = 1;
            this.labelPortNo.Text = "Port No";
            // 
            // nudPortNo
            // 
            this.nudPortNo.Location = new System.Drawing.Point(174, 42);
            this.nudPortNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudPortNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudPortNo.Name = "nudPortNo";
            this.nudPortNo.Size = new System.Drawing.Size(70, 25);
            this.nudPortNo.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(132, 223);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(54, 223);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 32);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(139, 13);
            this.ipAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(105, 25);
            this.ipAddress.TabIndex = 5;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(258, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 293);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(258, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 262);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(257, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 262);
            // 
            // TcpIpSettingForm_Fill_Panel
            // 
            // 
            // TcpIpSettingForm_Fill_Panel.ClientArea
            // 
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelCpuInspectorData);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.ipAddress);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelModuleDeviceNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnCancel);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelModuleIoNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelPlcNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudCpuInspectorData);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudModuleDeviceNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudModuleIoNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudPlcNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudNetworkNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.nudPortNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelNetworkNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelPortNo);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelIpAddress);
            this.TcpIpSettingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.TcpIpSettingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcpIpSettingForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.TcpIpSettingForm_Fill_Panel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TcpIpSettingForm_Fill_Panel.Name = "TcpIpSettingForm_Fill_Panel";
            this.TcpIpSettingForm_Fill_Panel.Size = new System.Drawing.Size(256, 262);
            this.TcpIpSettingForm_Fill_Panel.TabIndex = 14;
            // 
            // labelCpuInspectorData
            // 
            this.labelCpuInspectorData.AutoSize = true;
            this.labelCpuInspectorData.Location = new System.Drawing.Point(11, 190);
            this.labelCpuInspectorData.Name = "labelCpuInspectorData";
            this.labelCpuInspectorData.Size = new System.Drawing.Size(125, 17);
            this.labelCpuInspectorData.TabIndex = 1;
            this.labelCpuInspectorData.Text = "CPU Inspector Data";
            // 
            // labelModuleDeviceNo
            // 
            this.labelModuleDeviceNo.AutoSize = true;
            this.labelModuleDeviceNo.Location = new System.Drawing.Point(11, 161);
            this.labelModuleDeviceNo.Name = "labelModuleDeviceNo";
            this.labelModuleDeviceNo.Size = new System.Drawing.Size(120, 17);
            this.labelModuleDeviceNo.TabIndex = 1;
            this.labelModuleDeviceNo.Text = "Module Device No";
            // 
            // labelModuleIoNo
            // 
            this.labelModuleIoNo.AutoSize = true;
            this.labelModuleIoNo.Location = new System.Drawing.Point(11, 132);
            this.labelModuleIoNo.Name = "labelModuleIoNo";
            this.labelModuleIoNo.Size = new System.Drawing.Size(96, 17);
            this.labelModuleIoNo.TabIndex = 1;
            this.labelModuleIoNo.Text = "Module IO No";
            // 
            // labelPlcNo
            // 
            this.labelPlcNo.AutoSize = true;
            this.labelPlcNo.Location = new System.Drawing.Point(11, 103);
            this.labelPlcNo.Name = "labelPlcNo";
            this.labelPlcNo.Size = new System.Drawing.Size(52, 17);
            this.labelPlcNo.TabIndex = 1;
            this.labelPlcNo.Text = "PLC No";
            // 
            // labelNetworkNo
            // 
            this.labelNetworkNo.AutoSize = true;
            this.labelNetworkNo.Location = new System.Drawing.Point(11, 74);
            this.labelNetworkNo.Name = "labelNetworkNo";
            this.labelNetworkNo.Size = new System.Drawing.Size(82, 17);
            this.labelNetworkNo.TabIndex = 1;
            this.labelNetworkNo.Text = "Network No";
            // 
            // nudNetworkNo
            // 
            this.nudNetworkNo.Location = new System.Drawing.Point(139, 72);
            this.nudNetworkNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudNetworkNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudNetworkNo.Name = "nudNetworkNo";
            this.nudNetworkNo.Size = new System.Drawing.Size(105, 25);
            this.nudNetworkNo.TabIndex = 2;
            // 
            // nudPlcNo
            // 
            this.nudPlcNo.Location = new System.Drawing.Point(139, 101);
            this.nudPlcNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudPlcNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudPlcNo.Name = "nudPlcNo";
            this.nudPlcNo.Size = new System.Drawing.Size(105, 25);
            this.nudPlcNo.TabIndex = 2;
            // 
            // nudModuleIoNo
            // 
            this.nudModuleIoNo.Location = new System.Drawing.Point(139, 130);
            this.nudModuleIoNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudModuleIoNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudModuleIoNo.Name = "nudModuleIoNo";
            this.nudModuleIoNo.Size = new System.Drawing.Size(105, 25);
            this.nudModuleIoNo.TabIndex = 2;
            // 
            // nudModuleDeviceNo
            // 
            this.nudModuleDeviceNo.Location = new System.Drawing.Point(139, 159);
            this.nudModuleDeviceNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudModuleDeviceNo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudModuleDeviceNo.Name = "nudModuleDeviceNo";
            this.nudModuleDeviceNo.Size = new System.Drawing.Size(105, 25);
            this.nudModuleDeviceNo.TabIndex = 2;
            // 
            // nudCpuInspectorData
            // 
            this.nudCpuInspectorData.Location = new System.Drawing.Point(139, 188);
            this.nudCpuInspectorData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudCpuInspectorData.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudCpuInspectorData.Name = "nudCpuInspectorData";
            this.nudCpuInspectorData.Size = new System.Drawing.Size(105, 25);
            this.nudCpuInspectorData.TabIndex = 2;
            // 
            // MelsecSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 294);
            this.Controls.Add(this.TcpIpSettingForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MelsecSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MELSEC Setting";
            this.Load += new System.EventHandler(this.TcpIpSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPortNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.TcpIpSettingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.TcpIpSettingForm_Fill_Panel.ClientArea.PerformLayout();
            this.TcpIpSettingForm_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNetworkNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlcNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModuleIoNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModuleDeviceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCpuInspectorData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.NumericUpDown nudPortNo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox ipAddress;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel TcpIpSettingForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Label labelModuleIoNo;
        private System.Windows.Forms.Label labelPlcNo;
        private System.Windows.Forms.Label labelNetworkNo;
        private System.Windows.Forms.Label labelModuleDeviceNo;
        private System.Windows.Forms.Label labelCpuInspectorData;
        private System.Windows.Forms.NumericUpDown nudModuleIoNo;
        private System.Windows.Forms.NumericUpDown nudPlcNo;
        private System.Windows.Forms.NumericUpDown nudNetworkNo;
        private System.Windows.Forms.NumericUpDown nudModuleDeviceNo;
        private System.Windows.Forms.NumericUpDown nudCpuInspectorData;
    }
}