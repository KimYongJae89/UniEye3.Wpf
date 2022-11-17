namespace DynMvp.Component.DepthSystem.UI
{
    partial class ExymaSettingForm
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
            this.labelCameraIndex = new System.Windows.Forms.Label();
            this.cameraIndex = new System.Windows.Forms.ComboBox();
            this.labelControlBoardType = new System.Windows.Forms.Label();
            this.controlBoardType = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.measureTriggerMode = new System.Windows.Forms.ComboBox();
            this.labelMeasureTriggerMode = new System.Windows.Forms.Label();
            this.labelMeasureMode = new System.Windows.Forms.Label();
            this.measureMode = new System.Windows.Forms.ComboBox();
            this.labelNoiseLevel = new System.Windows.Forms.Label();
            this.noiseLevel = new System.Windows.Forms.TextBox();
            this.labelGain = new System.Windows.Forms.Label();
            this.gain = new System.Windows.Forms.NumericUpDown();
            this.labelOffset = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelCamera2Index = new System.Windows.Forms.Label();
            this.camera2Index = new System.Windows.Forms.ComboBox();
            this.buttonControlBoard1Setting = new System.Windows.Forms.Button();
            this.buttonControlBoard2Setting = new System.Windows.Forms.Button();
            this.labelCenterCameraIndex = new System.Windows.Forms.Label();
            this.centerCameraIndex = new System.Windows.Forms.ComboBox();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ExymaSettingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.ExymaSettingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.ExymaSettingForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCameraIndex
            // 
            this.labelCameraIndex.AutoSize = true;
            this.labelCameraIndex.Location = new System.Drawing.Point(9, 74);
            this.labelCameraIndex.Name = "labelCameraIndex";
            this.labelCameraIndex.Size = new System.Drawing.Size(103, 20);
            this.labelCameraIndex.TabIndex = 0;
            this.labelCameraIndex.Text = "Camera Index";
            // 
            // cameraIndex
            // 
            this.cameraIndex.FormattingEnabled = true;
            this.cameraIndex.Location = new System.Drawing.Point(197, 71);
            this.cameraIndex.Name = "cameraIndex";
            this.cameraIndex.Size = new System.Drawing.Size(98, 28);
            this.cameraIndex.TabIndex = 1;
            // 
            // labelControlBoardType
            // 
            this.labelControlBoardType.AutoSize = true;
            this.labelControlBoardType.Location = new System.Drawing.Point(9, 137);
            this.labelControlBoardType.Name = "labelControlBoardType";
            this.labelControlBoardType.Size = new System.Drawing.Size(142, 20);
            this.labelControlBoardType.TabIndex = 0;
            this.labelControlBoardType.Text = "Control Board Type";
            // 
            // controlBoardType
            // 
            this.controlBoardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlBoardType.FormattingEnabled = true;
            this.controlBoardType.Items.AddRange(new object[] {
            "None",
            "Legacy",
            "Ver2013"});
            this.controlBoardType.Location = new System.Drawing.Point(197, 134);
            this.controlBoardType.Name = "controlBoardType";
            this.controlBoardType.Size = new System.Drawing.Size(170, 28);
            this.controlBoardType.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(194, 359);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 29);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(107, 359);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 29);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // measureTriggerMode
            // 
            this.measureTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measureTriggerMode.FormattingEnabled = true;
            this.measureTriggerMode.Items.AddRange(new object[] {
            "None",
            "CameraIo",
            "UartStepByStep",
            "UartSequential"});
            this.measureTriggerMode.Location = new System.Drawing.Point(197, 199);
            this.measureTriggerMode.Name = "measureTriggerMode";
            this.measureTriggerMode.Size = new System.Drawing.Size(170, 28);
            this.measureTriggerMode.TabIndex = 1;
            // 
            // labelMeasureTriggerMode
            // 
            this.labelMeasureTriggerMode.AutoSize = true;
            this.labelMeasureTriggerMode.Location = new System.Drawing.Point(9, 202);
            this.labelMeasureTriggerMode.Name = "labelMeasureTriggerMode";
            this.labelMeasureTriggerMode.Size = new System.Drawing.Size(165, 20);
            this.labelMeasureTriggerMode.TabIndex = 0;
            this.labelMeasureTriggerMode.Text = "Measure Trigger Mode";
            // 
            // labelMeasureMode
            // 
            this.labelMeasureMode.AutoSize = true;
            this.labelMeasureMode.Location = new System.Drawing.Point(9, 233);
            this.labelMeasureMode.Name = "labelMeasureMode";
            this.labelMeasureMode.Size = new System.Drawing.Size(112, 20);
            this.labelMeasureMode.TabIndex = 0;
            this.labelMeasureMode.Text = "Measure Mode";
            // 
            // measureMode
            // 
            this.measureMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measureMode.FormattingEnabled = true;
            this.measureMode.Items.AddRange(new object[] {
            "None",
            "CameraIo",
            "UartStepByStep",
            "UartSequential"});
            this.measureMode.Location = new System.Drawing.Point(197, 231);
            this.measureMode.Name = "measureMode";
            this.measureMode.Size = new System.Drawing.Size(170, 28);
            this.measureMode.TabIndex = 1;
            // 
            // labelNoiseLevel
            // 
            this.labelNoiseLevel.AutoSize = true;
            this.labelNoiseLevel.Location = new System.Drawing.Point(9, 264);
            this.labelNoiseLevel.Name = "labelNoiseLevel";
            this.labelNoiseLevel.Size = new System.Drawing.Size(86, 20);
            this.labelNoiseLevel.TabIndex = 0;
            this.labelNoiseLevel.Text = "Noise Level";
            // 
            // noiseLevel
            // 
            this.noiseLevel.Location = new System.Drawing.Point(197, 263);
            this.noiseLevel.Name = "noiseLevel";
            this.noiseLevel.Size = new System.Drawing.Size(98, 27);
            this.noiseLevel.TabIndex = 4;
            // 
            // labelGain
            // 
            this.labelGain.AutoSize = true;
            this.labelGain.Location = new System.Drawing.Point(9, 295);
            this.labelGain.Name = "labelGain";
            this.labelGain.Size = new System.Drawing.Size(41, 20);
            this.labelGain.TabIndex = 0;
            this.labelGain.Text = "Gain";
            // 
            // gain
            // 
            this.gain.Location = new System.Drawing.Point(197, 294);
            this.gain.Name = "gain";
            this.gain.Size = new System.Drawing.Size(98, 27);
            this.gain.TabIndex = 5;
            // 
            // labelOffset
            // 
            this.labelOffset.AutoSize = true;
            this.labelOffset.Location = new System.Drawing.Point(9, 326);
            this.labelOffset.Name = "labelOffset";
            this.labelOffset.Size = new System.Drawing.Size(50, 20);
            this.labelOffset.TabIndex = 0;
            this.labelOffset.Text = "Offset";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(197, 325);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(98, 27);
            this.numericUpDown1.TabIndex = 5;
            // 
            // labelCamera2Index
            // 
            this.labelCamera2Index.AutoSize = true;
            this.labelCamera2Index.Location = new System.Drawing.Point(9, 105);
            this.labelCamera2Index.Name = "labelCamera2Index";
            this.labelCamera2Index.Size = new System.Drawing.Size(116, 20);
            this.labelCamera2Index.TabIndex = 0;
            this.labelCamera2Index.Text = "Camera 2 Index";
            // 
            // camera2Index
            // 
            this.camera2Index.FormattingEnabled = true;
            this.camera2Index.Location = new System.Drawing.Point(197, 102);
            this.camera2Index.Name = "camera2Index";
            this.camera2Index.Size = new System.Drawing.Size(98, 28);
            this.camera2Index.TabIndex = 1;
            // 
            // buttonControlBoard1Setting
            // 
            this.buttonControlBoard1Setting.Location = new System.Drawing.Point(150, 166);
            this.buttonControlBoard1Setting.Name = "buttonControlBoard1Setting";
            this.buttonControlBoard1Setting.Size = new System.Drawing.Size(104, 29);
            this.buttonControlBoard1Setting.TabIndex = 3;
            this.buttonControlBoard1Setting.Text = "Scanner 1";
            this.buttonControlBoard1Setting.UseVisualStyleBackColor = true;
            this.buttonControlBoard1Setting.Click += new System.EventHandler(this.buttonControlBoard1Setting_Click);
            // 
            // buttonControlBoard2Setting
            // 
            this.buttonControlBoard2Setting.Location = new System.Drawing.Point(260, 166);
            this.buttonControlBoard2Setting.Name = "buttonControlBoard2Setting";
            this.buttonControlBoard2Setting.Size = new System.Drawing.Size(107, 29);
            this.buttonControlBoard2Setting.TabIndex = 3;
            this.buttonControlBoard2Setting.Text = "Scanner 2";
            this.buttonControlBoard2Setting.UseVisualStyleBackColor = true;
            this.buttonControlBoard2Setting.Click += new System.EventHandler(this.buttonControlBoard2Setting_Click);
            // 
            // labelCenterCameraIndex
            // 
            this.labelCenterCameraIndex.AutoSize = true;
            this.labelCenterCameraIndex.Location = new System.Drawing.Point(9, 43);
            this.labelCenterCameraIndex.Name = "labelCenterCameraIndex";
            this.labelCenterCameraIndex.Size = new System.Drawing.Size(153, 20);
            this.labelCenterCameraIndex.TabIndex = 0;
            this.labelCenterCameraIndex.Text = "Center Camera Index";
            // 
            // centerCameraIndex
            // 
            this.centerCameraIndex.FormattingEnabled = true;
            this.centerCameraIndex.Location = new System.Drawing.Point(197, 40);
            this.centerCameraIndex.Name = "centerCameraIndex";
            this.centerCameraIndex.Size = new System.Drawing.Size(98, 28);
            this.centerCameraIndex.TabIndex = 1;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(384, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 428);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(384, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 397);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(383, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 397);
            // 
            // ExymaSettingForm_Fill_Panel
            // 
            // 
            // ExymaSettingForm_Fill_Panel.ClientArea
            // 
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.numericUpDown1);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.gain);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.name);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.noiseLevel);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.buttonControlBoard2Setting);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.buttonControlBoard1Setting);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.measureMode);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.measureTriggerMode);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelOffset);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.controlBoardType);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelGain);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelNoiseLevel);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelMeasureMode);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.camera2Index);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.centerCameraIndex);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.cameraIndex);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelMeasureTriggerMode);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelCamera2Index);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelCenterCameraIndex);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelControlBoardType);
            this.ExymaSettingForm_Fill_Panel.ClientArea.Controls.Add(this.labelCameraIndex);
            this.ExymaSettingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ExymaSettingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExymaSettingForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.ExymaSettingForm_Fill_Panel.Name = "ExymaSettingForm_Fill_Panel";
            this.ExymaSettingForm_Fill_Panel.Size = new System.Drawing.Size(382, 397);
            this.ExymaSettingForm_Fill_Panel.TabIndex = 14;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(197, 10);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(170, 27);
            this.name.TabIndex = 4;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(9, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(49, 20);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // ExymaSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 429);
            this.Controls.Add(this.ExymaSettingForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ExymaSettingForm";
            this.Text = "Exyma Setting";
            this.Load += new System.EventHandler(this.ExymaSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ExymaSettingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ExymaSettingForm_Fill_Panel.ClientArea.PerformLayout();
            this.ExymaSettingForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCameraIndex;
        private System.Windows.Forms.ComboBox cameraIndex;
        private System.Windows.Forms.Label labelControlBoardType;
        private System.Windows.Forms.ComboBox controlBoardType;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox measureTriggerMode;
        private System.Windows.Forms.Label labelMeasureTriggerMode;
        private System.Windows.Forms.Label labelMeasureMode;
        private System.Windows.Forms.ComboBox measureMode;
        private System.Windows.Forms.Label labelNoiseLevel;
        private System.Windows.Forms.TextBox noiseLevel;
        private System.Windows.Forms.Label labelGain;
        private System.Windows.Forms.NumericUpDown gain;
        private System.Windows.Forms.Label labelOffset;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelCamera2Index;
        private System.Windows.Forms.ComboBox camera2Index;
        private System.Windows.Forms.Button buttonControlBoard1Setting;
        private System.Windows.Forms.Button buttonControlBoard2Setting;
        private System.Windows.Forms.Label labelCenterCameraIndex;
        private System.Windows.Forms.ComboBox centerCameraIndex;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel ExymaSettingForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
    }
}