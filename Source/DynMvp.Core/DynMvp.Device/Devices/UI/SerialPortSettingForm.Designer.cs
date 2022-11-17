namespace DynMvp.Devices.UI
{
    partial class SerialPortSettingForm
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
            this.labelBaudRate = new System.Windows.Forms.Label();
            this.baudRate = new System.Windows.Forms.ComboBox();
            this.labelDataBits = new System.Windows.Forms.Label();
            this.dataBits = new System.Windows.Forms.ComboBox();
            this.labelParity = new System.Windows.Forms.Label();
            this.parity = new System.Windows.Forms.ComboBox();
            this.labelStopBits = new System.Windows.Forms.Label();
            this.stopBits = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxProperty = new System.Windows.Forms.GroupBox();
            this.checkBoxDtrEnable = new System.Windows.Forms.CheckBox();
            this.checkBoxRtsEnable = new System.Windows.Forms.CheckBox();
            this.comboBoxHandshake = new System.Windows.Forms.ComboBox();
            this.labelHandshake = new System.Windows.Forms.Label();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.comboPortName = new System.Windows.Forms.ComboBox();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.SerialPortSettingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.groupBoxTest = new System.Windows.Forms.GroupBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.dataGridViewTest = new System.Windows.Forms.DataGridView();
            this.ColumnDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.groupBoxProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SerialPortSettingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.SerialPortSettingForm_Fill_Panel.SuspendLayout();
            this.groupBoxTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.AutoSize = true;
            this.labelBaudRate.Location = new System.Drawing.Point(20, 65);
            this.labelBaudRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(77, 18);
            this.labelBaudRate.TabIndex = 0;
            this.labelBaudRate.Text = "Baud Rate";
            // 
            // baudRate
            // 
            this.baudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudRate.FormattingEnabled = true;
            this.baudRate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.baudRate.Location = new System.Drawing.Point(116, 61);
            this.baudRate.Margin = new System.Windows.Forms.Padding(4);
            this.baudRate.Name = "baudRate";
            this.baudRate.Size = new System.Drawing.Size(114, 26);
            this.baudRate.TabIndex = 1;
            this.baudRate.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelDataBits
            // 
            this.labelDataBits.AutoSize = true;
            this.labelDataBits.Location = new System.Drawing.Point(20, 100);
            this.labelDataBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(68, 18);
            this.labelDataBits.TabIndex = 0;
            this.labelDataBits.Text = "Data Bits";
            // 
            // dataBits
            // 
            this.dataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataBits.FormattingEnabled = true;
            this.dataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.dataBits.Location = new System.Drawing.Point(116, 96);
            this.dataBits.Margin = new System.Windows.Forms.Padding(4);
            this.dataBits.Name = "dataBits";
            this.dataBits.Size = new System.Drawing.Size(114, 26);
            this.dataBits.TabIndex = 1;
            this.dataBits.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelParity
            // 
            this.labelParity.AutoSize = true;
            this.labelParity.Location = new System.Drawing.Point(20, 133);
            this.labelParity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(45, 18);
            this.labelParity.TabIndex = 0;
            this.labelParity.Text = "Parity";
            // 
            // parity
            // 
            this.parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parity.FormattingEnabled = true;
            this.parity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd",
            "Mark",
            "Space"});
            this.parity.Location = new System.Drawing.Point(116, 129);
            this.parity.Margin = new System.Windows.Forms.Padding(4);
            this.parity.Name = "parity";
            this.parity.Size = new System.Drawing.Size(114, 26);
            this.parity.TabIndex = 1;
            this.parity.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelStopBits
            // 
            this.labelStopBits.AutoSize = true;
            this.labelStopBits.Location = new System.Drawing.Point(20, 166);
            this.labelStopBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(68, 18);
            this.labelStopBits.TabIndex = 0;
            this.labelStopBits.Text = "Stop Bits";
            // 
            // stopBits
            // 
            this.stopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stopBits.FormattingEnabled = true;
            this.stopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.stopBits.Location = new System.Drawing.Point(116, 163);
            this.stopBits.Margin = new System.Windows.Forms.Padding(4);
            this.stopBits.Name = "stopBits";
            this.stopBits.Size = new System.Drawing.Size(114, 26);
            this.stopBits.TabIndex = 1;
            this.stopBits.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(265, 290);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 34);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(396, 290);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 34);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBoxProperty
            // 
            this.groupBoxProperty.Controls.Add(this.checkBoxDtrEnable);
            this.groupBoxProperty.Controls.Add(this.checkBoxRtsEnable);
            this.groupBoxProperty.Controls.Add(this.comboBoxHandshake);
            this.groupBoxProperty.Controls.Add(this.labelHandshake);
            this.groupBoxProperty.Controls.Add(this.labelPortNo);
            this.groupBoxProperty.Controls.Add(this.labelBaudRate);
            this.groupBoxProperty.Controls.Add(this.labelDataBits);
            this.groupBoxProperty.Controls.Add(this.labelParity);
            this.groupBoxProperty.Controls.Add(this.stopBits);
            this.groupBoxProperty.Controls.Add(this.labelStopBits);
            this.groupBoxProperty.Controls.Add(this.parity);
            this.groupBoxProperty.Controls.Add(this.comboPortName);
            this.groupBoxProperty.Controls.Add(this.baudRate);
            this.groupBoxProperty.Controls.Add(this.dataBits);
            this.groupBoxProperty.Location = new System.Drawing.Point(8, 37);
            this.groupBoxProperty.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxProperty.Name = "groupBoxProperty";
            this.groupBoxProperty.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxProperty.Size = new System.Drawing.Size(244, 292);
            this.groupBoxProperty.TabIndex = 3;
            this.groupBoxProperty.TabStop = false;
            this.groupBoxProperty.Text = "Property";
            // 
            // checkBoxDtrEnable
            // 
            this.checkBoxDtrEnable.AutoSize = true;
            this.checkBoxDtrEnable.Location = new System.Drawing.Point(22, 260);
            this.checkBoxDtrEnable.Name = "checkBoxDtrEnable";
            this.checkBoxDtrEnable.Size = new System.Drawing.Size(107, 22);
            this.checkBoxDtrEnable.TabIndex = 5;
            this.checkBoxDtrEnable.Text = "DTR Enable";
            this.checkBoxDtrEnable.UseVisualStyleBackColor = true;
            this.checkBoxDtrEnable.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // checkBoxRtsEnable
            // 
            this.checkBoxRtsEnable.AutoSize = true;
            this.checkBoxRtsEnable.Location = new System.Drawing.Point(22, 232);
            this.checkBoxRtsEnable.Name = "checkBoxRtsEnable";
            this.checkBoxRtsEnable.Size = new System.Drawing.Size(106, 22);
            this.checkBoxRtsEnable.TabIndex = 4;
            this.checkBoxRtsEnable.Text = "RTS Enable";
            this.checkBoxRtsEnable.UseVisualStyleBackColor = true;
            this.checkBoxRtsEnable.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // comboBoxHandshake
            // 
            this.comboBoxHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHandshake.FormattingEnabled = true;
            this.comboBoxHandshake.Items.AddRange(new object[] {
            "None",
            "RequestToSend",
            "RequestToSendXOnXOff",
            "Handshake.XOnXOff"});
            this.comboBoxHandshake.Location = new System.Drawing.Point(116, 197);
            this.comboBoxHandshake.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxHandshake.Name = "comboBoxHandshake";
            this.comboBoxHandshake.Size = new System.Drawing.Size(114, 26);
            this.comboBoxHandshake.TabIndex = 3;
            this.comboBoxHandshake.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelHandshake
            // 
            this.labelHandshake.AutoSize = true;
            this.labelHandshake.Location = new System.Drawing.Point(20, 200);
            this.labelHandshake.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHandshake.Name = "labelHandshake";
            this.labelHandshake.Size = new System.Drawing.Size(83, 18);
            this.labelHandshake.TabIndex = 2;
            this.labelHandshake.Text = "Handshake";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(20, 29);
            this.labelPortNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(60, 18);
            this.labelPortNo.TabIndex = 0;
            this.labelPortNo.Text = "Port No";
            // 
            // comboPortName
            // 
            this.comboPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPortName.FormattingEnabled = true;
            this.comboPortName.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboPortName.Location = new System.Drawing.Point(116, 26);
            this.comboPortName.Margin = new System.Windows.Forms.Padding(4);
            this.comboPortName.Name = "comboPortName";
            this.comboPortName.Size = new System.Drawing.Size(114, 26);
            this.comboPortName.TabIndex = 1;
            this.comboPortName.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(512, 30);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 371);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(512, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 341);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(511, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 341);
            // 
            // SerialPortSettingForm_Fill_Panel
            // 
            // 
            // SerialPortSettingForm_Fill_Panel.ClientArea
            // 
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxTest);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.textName);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxProperty);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnCancel);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.SerialPortSettingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SerialPortSettingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerialPortSettingForm_Fill_Panel.Location = new System.Drawing.Point(1, 30);
            this.SerialPortSettingForm_Fill_Panel.Name = "SerialPortSettingForm_Fill_Panel";
            this.SerialPortSettingForm_Fill_Panel.Size = new System.Drawing.Size(510, 341);
            this.SerialPortSettingForm_Fill_Panel.TabIndex = 12;
            // 
            // groupBoxTest
            // 
            this.groupBoxTest.Controls.Add(this.buttonSend);
            this.groupBoxTest.Controls.Add(this.textBoxSend);
            this.groupBoxTest.Controls.Add(this.dataGridViewTest);
            this.groupBoxTest.Location = new System.Drawing.Point(259, 37);
            this.groupBoxTest.Name = "groupBoxTest";
            this.groupBoxTest.Size = new System.Drawing.Size(243, 246);
            this.groupBoxTest.TabIndex = 6;
            this.groupBoxTest.TabStop = false;
            this.groupBoxTest.Text = "Test";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(167, 216);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(70, 24);
            this.buttonSend.TabIndex = 7;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(6, 216);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(154, 24);
            this.textBoxSend.TabIndex = 6;
            // 
            // dataGridViewTest
            // 
            this.dataGridViewTest.AllowUserToAddRows = false;
            this.dataGridViewTest.AllowUserToDeleteRows = false;
            this.dataGridViewTest.AllowUserToOrderColumns = true;
            this.dataGridViewTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDirection,
            this.ColumnData});
            this.dataGridViewTest.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewTest.Location = new System.Drawing.Point(6, 23);
            this.dataGridViewTest.MultiSelect = false;
            this.dataGridViewTest.Name = "dataGridViewTest";
            this.dataGridViewTest.ReadOnly = true;
            this.dataGridViewTest.RowHeadersVisible = false;
            this.dataGridViewTest.RowTemplate.Height = 23;
            this.dataGridViewTest.Size = new System.Drawing.Size(231, 187);
            this.dataGridViewTest.TabIndex = 0;
            // 
            // ColumnDirection
            // 
            this.ColumnDirection.HeaderText = "Dir";
            this.ColumnDirection.Name = "ColumnDirection";
            this.ColumnDirection.ReadOnly = true;
            this.ColumnDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDirection.Width = 60;
            // 
            // ColumnData
            // 
            this.ColumnData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnData.HeaderText = "Data";
            this.ColumnData.Name = "ColumnData";
            this.ColumnData.ReadOnly = true;
            this.ColumnData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(124, 8);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(114, 24);
            this.textName.TabIndex = 5;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(27, 11);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 18);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "Name";
            // 
            // SerialPortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 372);
            this.Controls.Add(this.SerialPortSettingForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SerialPortSettingForm";
            this.Text = "Port Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortSettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SerialPortSettingForm_Load);
            this.groupBoxProperty.ResumeLayout(false);
            this.groupBoxProperty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.SerialPortSettingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.PerformLayout();
            this.SerialPortSettingForm_Fill_Panel.ResumeLayout(false);
            this.groupBoxTest.ResumeLayout(false);
            this.groupBoxTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBaudRate;
        private System.Windows.Forms.ComboBox baudRate;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox dataBits;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox parity;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox stopBits;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxProperty;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.ComboBox comboPortName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel SerialPortSettingForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ComboBox comboBoxHandshake;
        private System.Windows.Forms.Label labelHandshake;
        private System.Windows.Forms.CheckBox checkBoxDtrEnable;
        private System.Windows.Forms.CheckBox checkBoxRtsEnable;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.GroupBox groupBoxTest;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.DataGridView dataGridViewTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDirection;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnData;
    }
}