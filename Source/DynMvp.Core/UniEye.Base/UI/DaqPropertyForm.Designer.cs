namespace UniEye.Base.UI
{
    partial class DaqPropertyForm
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
            this.labelMinValue = new System.Windows.Forms.Label();
            this.minValue = new System.Windows.Forms.NumericUpDown();
            this.labelMaxValue = new System.Windows.Forms.Label();
            this.maxValue = new System.Windows.Forms.NumericUpDown();
            this.labelSamplingHz = new System.Windows.Forms.Label();
            this.samplingHz = new System.Windows.Forms.NumericUpDown();
            this.labelScaleFactor = new System.Windows.Forms.Label();
            this.scaleFactor = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.valueOffset = new System.Windows.Forms.TextBox();
            this.labelValueOffset = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resisterValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCalculateScale = new System.Windows.Forms.Button();
            this.checkUseCutomScaleFactor = new System.Windows.Forms.CheckBox();
            this.txtMinDistance = new System.Windows.Forms.TextBox();
            this.txtMaxDistance = new System.Windows.Forms.TextBox();
            this.txtMinVoltage = new System.Windows.Forms.TextBox();
            this.txtMaxVoltage = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCalcScaleFactor = new System.Windows.Forms.Button();
            this.buttonUseThisScaleFactor = new System.Windows.Forms.Button();
            this.labelDaqName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelLeftDaqChannel = new System.Windows.Forms.Label();
            this.daqChannelName = new System.Windows.Forms.TextBox();
            this.deviceName = new System.Windows.Forms.TextBox();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.DaqPropertyForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.minValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplingHz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.DaqPropertyForm_Fill_Panel.ClientArea.SuspendLayout();
            this.DaqPropertyForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMinValue
            // 
            this.labelMinValue.AutoSize = true;
            this.labelMinValue.Location = new System.Drawing.Point(13, 94);
            this.labelMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinValue.Name = "labelMinValue";
            this.labelMinValue.Size = new System.Drawing.Size(72, 18);
            this.labelMinValue.TabIndex = 0;
            this.labelMinValue.Text = "Min Value";
            // 
            // minValue
            // 
            this.minValue.Location = new System.Drawing.Point(184, 92);
            this.minValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.minValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.minValue.Name = "minValue";
            this.minValue.Size = new System.Drawing.Size(87, 24);
            this.minValue.TabIndex = 1;
            // 
            // labelMaxValue
            // 
            this.labelMaxValue.AutoSize = true;
            this.labelMaxValue.Location = new System.Drawing.Point(13, 122);
            this.labelMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxValue.Name = "labelMaxValue";
            this.labelMaxValue.Size = new System.Drawing.Size(76, 18);
            this.labelMaxValue.TabIndex = 0;
            this.labelMaxValue.Text = "Max Value";
            // 
            // maxValue
            // 
            this.maxValue.Location = new System.Drawing.Point(184, 121);
            this.maxValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maxValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.maxValue.Name = "maxValue";
            this.maxValue.Size = new System.Drawing.Size(87, 24);
            this.maxValue.TabIndex = 1;
            // 
            // labelSamplingHz
            // 
            this.labelSamplingHz.AutoSize = true;
            this.labelSamplingHz.Location = new System.Drawing.Point(13, 151);
            this.labelSamplingHz.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSamplingHz.Name = "labelSamplingHz";
            this.labelSamplingHz.Size = new System.Drawing.Size(92, 18);
            this.labelSamplingHz.TabIndex = 0;
            this.labelSamplingHz.Text = "Sampling Hz";
            // 
            // samplingHz
            // 
            this.samplingHz.Location = new System.Drawing.Point(184, 149);
            this.samplingHz.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.samplingHz.Name = "samplingHz";
            this.samplingHz.Size = new System.Drawing.Size(87, 24);
            this.samplingHz.TabIndex = 1;
            // 
            // labelScaleFactor
            // 
            this.labelScaleFactor.AutoSize = true;
            this.labelScaleFactor.Location = new System.Drawing.Point(13, 233);
            this.labelScaleFactor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleFactor.Name = "labelScaleFactor";
            this.labelScaleFactor.Size = new System.Drawing.Size(92, 18);
            this.labelScaleFactor.TabIndex = 0;
            this.labelScaleFactor.Text = "Scale Factor";
            // 
            // scaleFactor
            // 
            this.scaleFactor.Location = new System.Drawing.Point(184, 230);
            this.scaleFactor.Name = "scaleFactor";
            this.scaleFactor.Size = new System.Drawing.Size(87, 24);
            this.scaleFactor.TabIndex = 2;
            this.scaleFactor.TextChanged += new System.EventHandler(this.scaleFactor_TextChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(56, 406);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(92, 32);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(151, 406);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(92, 32);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // valueOffset
            // 
            this.valueOffset.Location = new System.Drawing.Point(132, 371);
            this.valueOffset.Name = "valueOffset";
            this.valueOffset.Size = new System.Drawing.Size(87, 24);
            this.valueOffset.TabIndex = 5;
            // 
            // labelValueOffset
            // 
            this.labelValueOffset.AutoSize = true;
            this.labelValueOffset.Location = new System.Drawing.Point(13, 374);
            this.labelValueOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelValueOffset.Name = "labelValueOffset";
            this.labelValueOffset.Size = new System.Drawing.Size(88, 18);
            this.labelValueOffset.TabIndex = 4;
            this.labelValueOffset.Text = "Value Offset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 179);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resister Value";
            // 
            // resisterValue
            // 
            this.resisterValue.Location = new System.Drawing.Point(184, 176);
            this.resisterValue.Name = "resisterValue";
            this.resisterValue.Size = new System.Drawing.Size(87, 24);
            this.resisterValue.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(272, 179);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ω";
            // 
            // buttonCalculateScale
            // 
            this.buttonCalculateScale.Location = new System.Drawing.Point(183, 202);
            this.buttonCalculateScale.Name = "buttonCalculateScale";
            this.buttonCalculateScale.Size = new System.Drawing.Size(87, 23);
            this.buttonCalculateScale.TabIndex = 3;
            this.buttonCalculateScale.Text = "Calculate";
            this.buttonCalculateScale.UseVisualStyleBackColor = true;
            this.buttonCalculateScale.Click += new System.EventHandler(this.buttonCalculateScale_Click);
            // 
            // checkUseCutomScaleFactor
            // 
            this.checkUseCutomScaleFactor.AutoSize = true;
            this.checkUseCutomScaleFactor.Location = new System.Drawing.Point(16, 261);
            this.checkUseCutomScaleFactor.Name = "checkUseCutomScaleFactor";
            this.checkUseCutomScaleFactor.Size = new System.Drawing.Size(164, 22);
            this.checkUseCutomScaleFactor.TabIndex = 6;
            this.checkUseCutomScaleFactor.Text = "Custom ScaleFactor";
            this.checkUseCutomScaleFactor.UseVisualStyleBackColor = true;
            // 
            // txtMinDistance
            // 
            this.txtMinDistance.Location = new System.Drawing.Point(17, 335);
            this.txtMinDistance.Name = "txtMinDistance";
            this.txtMinDistance.Size = new System.Drawing.Size(42, 24);
            this.txtMinDistance.TabIndex = 7;
            // 
            // txtMaxDistance
            // 
            this.txtMaxDistance.Location = new System.Drawing.Point(83, 335);
            this.txtMaxDistance.Name = "txtMaxDistance";
            this.txtMaxDistance.Size = new System.Drawing.Size(42, 24);
            this.txtMaxDistance.TabIndex = 8;
            // 
            // txtMinVoltage
            // 
            this.txtMinVoltage.Location = new System.Drawing.Point(17, 288);
            this.txtMinVoltage.Name = "txtMinVoltage";
            this.txtMinVoltage.Size = new System.Drawing.Size(42, 24);
            this.txtMinVoltage.TabIndex = 9;
            // 
            // txtMaxVoltage
            // 
            this.txtMaxVoltage.Location = new System.Drawing.Point(86, 288);
            this.txtMaxVoltage.Name = "txtMaxVoltage";
            this.txtMaxVoltage.Size = new System.Drawing.Size(42, 24);
            this.txtMaxVoltage.TabIndex = 10;
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(13, 314);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(115, 18);
            this.label.TabIndex = 11;
            this.label.Text = "-----------------------";
            this.label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(134, 314);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "=";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(63, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "-";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(62, 337);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "-";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonCalcScaleFactor
            // 
            this.buttonCalcScaleFactor.Location = new System.Drawing.Point(134, 310);
            this.buttonCalcScaleFactor.Name = "buttonCalcScaleFactor";
            this.buttonCalcScaleFactor.Size = new System.Drawing.Size(30, 28);
            this.buttonCalcScaleFactor.TabIndex = 16;
            this.buttonCalcScaleFactor.Text = "=";
            this.buttonCalcScaleFactor.UseVisualStyleBackColor = true;
            this.buttonCalcScaleFactor.Click += new System.EventHandler(this.buttonCalcScaleFactor_Click);
            // 
            // buttonUseThisScaleFactor
            // 
            this.buttonUseThisScaleFactor.Location = new System.Drawing.Point(180, 310);
            this.buttonUseThisScaleFactor.Name = "buttonUseThisScaleFactor";
            this.buttonUseThisScaleFactor.Size = new System.Drawing.Size(90, 28);
            this.buttonUseThisScaleFactor.TabIndex = 17;
            this.buttonUseThisScaleFactor.Text = "Use this";
            this.buttonUseThisScaleFactor.UseVisualStyleBackColor = true;
            this.buttonUseThisScaleFactor.Click += new System.EventHandler(this.buttonUseThisScaleFactor_Click);
            // 
            // labelDaqName
            // 
            this.labelDaqName.AutoSize = true;
            this.labelDaqName.Location = new System.Drawing.Point(13, 8);
            this.labelDaqName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDaqName.Name = "labelDaqName";
            this.labelDaqName.Size = new System.Drawing.Size(48, 18);
            this.labelDaqName.TabIndex = 167;
            this.labelDaqName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(138, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 24);
            this.txtName.TabIndex = 166;
            // 
            // labelLeftDaqChannel
            // 
            this.labelLeftDaqChannel.AutoSize = true;
            this.labelLeftDaqChannel.Location = new System.Drawing.Point(13, 64);
            this.labelLeftDaqChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeftDaqChannel.Name = "labelLeftDaqChannel";
            this.labelLeftDaqChannel.Size = new System.Drawing.Size(62, 18);
            this.labelLeftDaqChannel.TabIndex = 165;
            this.labelLeftDaqChannel.Text = "Channel";
            // 
            // daqChannelName
            // 
            this.daqChannelName.Location = new System.Drawing.Point(138, 61);
            this.daqChannelName.Name = "daqChannelName";
            this.daqChannelName.Size = new System.Drawing.Size(155, 24);
            this.daqChannelName.TabIndex = 164;
            // 
            // deviceName
            // 
            this.deviceName.Location = new System.Drawing.Point(138, 33);
            this.deviceName.Name = "deviceName";
            this.deviceName.Size = new System.Drawing.Size(155, 24);
            this.deviceName.TabIndex = 166;
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Location = new System.Drawing.Point(13, 36);
            this.labelDeviceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(97, 18);
            this.labelDeviceName.TabIndex = 167;
            this.labelDeviceName.Text = "Device Name";
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(300, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 479);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(300, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 448);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(299, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 448);
            // 
            // DaqPropertyForm_Fill_Panel
            // 
            // 
            // DaqPropertyForm_Fill_Panel.ClientArea
            // 
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelDeviceName);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelDaqName);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.deviceName);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtName);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelLeftDaqChannel);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.daqChannelName);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonUseThisScaleFactor);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCalcScaleFactor);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label5);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label4);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label3);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtMaxVoltage);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtMinVoltage);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtMaxDistance);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtMinDistance);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.checkUseCutomScaleFactor);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.valueOffset);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelValueOffset);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCalculateScale);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOk);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.resisterValue);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.scaleFactor);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label2);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label1);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelScaleFactor);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.samplingHz);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelSamplingHz);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.maxValue);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelMaxValue);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.minValue);
            this.DaqPropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelMinValue);
            this.DaqPropertyForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.DaqPropertyForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaqPropertyForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.DaqPropertyForm_Fill_Panel.Name = "DaqPropertyForm_Fill_Panel";
            this.DaqPropertyForm_Fill_Panel.Size = new System.Drawing.Size(298, 448);
            this.DaqPropertyForm_Fill_Panel.TabIndex = 176;
            // 
            // DaqPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 480);
            this.Controls.Add(this.DaqPropertyForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DaqPropertyForm";
            this.Text = "DAQ Property";
            this.Load += new System.EventHandler(this.DaqPropertyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.minValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplingHz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.DaqPropertyForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.DaqPropertyForm_Fill_Panel.ClientArea.PerformLayout();
            this.DaqPropertyForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMinValue;
        private System.Windows.Forms.NumericUpDown minValue;
        private System.Windows.Forms.Label labelMaxValue;
        private System.Windows.Forms.NumericUpDown maxValue;
        private System.Windows.Forms.Label labelSamplingHz;
        private System.Windows.Forms.NumericUpDown samplingHz;
        private System.Windows.Forms.Label labelScaleFactor;
        private System.Windows.Forms.TextBox scaleFactor;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox valueOffset;
        private System.Windows.Forms.Label labelValueOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox resisterValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCalculateScale;
        private System.Windows.Forms.CheckBox checkUseCutomScaleFactor;
        private System.Windows.Forms.TextBox txtMinDistance;
        private System.Windows.Forms.TextBox txtMaxDistance;
        private System.Windows.Forms.TextBox txtMinVoltage;
        private System.Windows.Forms.TextBox txtMaxVoltage;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCalcScaleFactor;
        private System.Windows.Forms.Button buttonUseThisScaleFactor;
        private System.Windows.Forms.Label labelDaqName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labelLeftDaqChannel;
        private System.Windows.Forms.TextBox daqChannelName;
        private System.Windows.Forms.TextBox deviceName;
        private System.Windows.Forms.Label labelDeviceName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel DaqPropertyForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}