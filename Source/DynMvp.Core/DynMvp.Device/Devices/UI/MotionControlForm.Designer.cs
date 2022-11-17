namespace DynMvp.Devices.UI
{
    partial class MotionControlForm
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
            this.paramPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.labelOrg = new System.Windows.Forms.Label();
            this.labelEz = new System.Windows.Forms.Label();
            this.labelEmg = new System.Windows.Forms.Label();
            this.labelInp = new System.Windows.Forms.Label();
            this.labelAlarm = new System.Windows.Forms.Label();
            this.labelLimitPos = new System.Windows.Forms.Label();
            this.labelLimitNeg = new System.Windows.Forms.Label();
            this.labelRun = new System.Windows.Forms.Label();
            this.labelHome = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelErr = new System.Windows.Forms.Label();
            this.labelCClr = new System.Windows.Forms.Label();
            this.labelSon = new System.Windows.Forms.Label();
            this.labelARst = new System.Windows.Forms.Label();
            this.comboAxis = new System.Windows.Forms.ComboBox();
            this.labelAxisNo = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.position = new System.Windows.Forms.NumericUpDown();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.originButton = new System.Windows.Forms.Button();
            this.movingStep = new System.Windows.Forms.ComboBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.moveButton = new System.Windows.Forms.Button();
            this.okbutton = new System.Windows.Forms.Button();
            this.buttonFindLimit = new System.Windows.Forms.Button();
            this.labelMotionDoneWaitTime = new System.Windows.Forms.Label();
            this.checkTimer = new System.Windows.Forms.Timer(this.components);
            this.comboAxisHandler = new System.Windows.Forms.ComboBox();
            this.labelAxisHandler = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.MotionControlForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.buttonServoOn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.position)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.MotionControlForm_Fill_Panel.ClientArea.SuspendLayout();
            this.MotionControlForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // paramPropertyGrid
            // 
            this.paramPropertyGrid.HelpVisible = false;
            this.paramPropertyGrid.Location = new System.Drawing.Point(297, 68);
            this.paramPropertyGrid.Margin = new System.Windows.Forms.Padding(4);
            this.paramPropertyGrid.Name = "paramPropertyGrid";
            this.paramPropertyGrid.Size = new System.Drawing.Size(284, 332);
            this.paramPropertyGrid.TabIndex = 4;
            // 
            // labelOrg
            // 
            this.labelOrg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelOrg.Location = new System.Drawing.Point(235, 68);
            this.labelOrg.Name = "labelOrg";
            this.labelOrg.Size = new System.Drawing.Size(55, 21);
            this.labelOrg.TabIndex = 5;
            this.labelOrg.Text = "ORG";
            this.labelOrg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEz
            // 
            this.labelEz.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelEz.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEz.Location = new System.Drawing.Point(235, 92);
            this.labelEz.Name = "labelEz";
            this.labelEz.Size = new System.Drawing.Size(55, 21);
            this.labelEz.TabIndex = 5;
            this.labelEz.Text = "EZ";
            this.labelEz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEmg
            // 
            this.labelEmg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelEmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEmg.Location = new System.Drawing.Point(235, 116);
            this.labelEmg.Name = "labelEmg";
            this.labelEmg.Size = new System.Drawing.Size(55, 21);
            this.labelEmg.TabIndex = 5;
            this.labelEmg.Text = "EMG";
            this.labelEmg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInp
            // 
            this.labelInp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelInp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInp.Location = new System.Drawing.Point(235, 139);
            this.labelInp.Name = "labelInp";
            this.labelInp.Size = new System.Drawing.Size(55, 21);
            this.labelInp.TabIndex = 5;
            this.labelInp.Text = "INP";
            this.labelInp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAlarm
            // 
            this.labelAlarm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAlarm.Location = new System.Drawing.Point(235, 163);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(55, 21);
            this.labelAlarm.TabIndex = 5;
            this.labelAlarm.Text = "ALM";
            this.labelAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLimitPos
            // 
            this.labelLimitPos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLimitPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLimitPos.Location = new System.Drawing.Point(235, 187);
            this.labelLimitPos.Name = "labelLimitPos";
            this.labelLimitPos.Size = new System.Drawing.Size(55, 21);
            this.labelLimitPos.TabIndex = 5;
            this.labelLimitPos.Text = "LMT+";
            this.labelLimitPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLimitNeg
            // 
            this.labelLimitNeg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLimitNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLimitNeg.Location = new System.Drawing.Point(235, 211);
            this.labelLimitNeg.Name = "labelLimitNeg";
            this.labelLimitNeg.Size = new System.Drawing.Size(55, 21);
            this.labelLimitNeg.TabIndex = 5;
            this.labelLimitNeg.Text = "LMT-";
            this.labelLimitNeg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRun
            // 
            this.labelRun.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRun.Location = new System.Drawing.Point(235, 235);
            this.labelRun.Name = "labelRun";
            this.labelRun.Size = new System.Drawing.Size(55, 21);
            this.labelRun.TabIndex = 5;
            this.labelRun.Text = "RUN";
            this.labelRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHome
            // 
            this.labelHome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelHome.Location = new System.Drawing.Point(235, 282);
            this.labelHome.Name = "labelHome";
            this.labelHome.Size = new System.Drawing.Size(55, 21);
            this.labelHome.TabIndex = 5;
            this.labelHome.Text = "HOME";
            this.labelHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(235, 306);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "H-OK";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErr
            // 
            this.labelErr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelErr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErr.Location = new System.Drawing.Point(235, 258);
            this.labelErr.Name = "labelErr";
            this.labelErr.Size = new System.Drawing.Size(55, 21);
            this.labelErr.TabIndex = 5;
            this.labelErr.Text = "ERR";
            this.labelErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCClr
            // 
            this.labelCClr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCClr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCClr.Location = new System.Drawing.Point(235, 330);
            this.labelCClr.Name = "labelCClr";
            this.labelCClr.Size = new System.Drawing.Size(55, 21);
            this.labelCClr.TabIndex = 5;
            this.labelCClr.Text = "C.CLR";
            this.labelCClr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSon
            // 
            this.labelSon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSon.Location = new System.Drawing.Point(235, 354);
            this.labelSon.Name = "labelSon";
            this.labelSon.Size = new System.Drawing.Size(55, 21);
            this.labelSon.TabIndex = 5;
            this.labelSon.Text = "SON";
            this.labelSon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelARst
            // 
            this.labelARst.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelARst.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelARst.Location = new System.Drawing.Point(235, 377);
            this.labelARst.Name = "labelARst";
            this.labelARst.Size = new System.Drawing.Size(55, 21);
            this.labelARst.TabIndex = 5;
            this.labelARst.Text = "A.RST";
            this.labelARst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboAxis
            // 
            this.comboAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAxis.FormattingEnabled = true;
            this.comboAxis.Location = new System.Drawing.Point(124, 37);
            this.comboAxis.Name = "comboAxis";
            this.comboAxis.Size = new System.Drawing.Size(246, 26);
            this.comboAxis.TabIndex = 7;
            this.comboAxis.SelectedIndexChanged += new System.EventHandler(this.axisNo_SelectedIndexChanged);
            // 
            // labelAxisNo
            // 
            this.labelAxisNo.AutoSize = true;
            this.labelAxisNo.Location = new System.Drawing.Point(11, 42);
            this.labelAxisNo.Name = "labelAxisNo";
            this.labelAxisNo.Size = new System.Drawing.Size(35, 18);
            this.labelAxisNo.TabIndex = 8;
            this.labelAxisNo.Text = "Axis";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(11, 69);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(62, 18);
            this.labelPosition.TabIndex = 13;
            this.labelPosition.Text = "Position";
            // 
            // position
            // 
            this.position.Location = new System.Drawing.Point(124, 66);
            this.position.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.position.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.position.Name = "position";
            this.position.Size = new System.Drawing.Size(107, 24);
            this.position.TabIndex = 12;
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(178, 150);
            this.moveDownButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(52, 31);
            this.moveDownButton.TabIndex = 11;
            this.moveDownButton.Text = "-";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(123, 150);
            this.moveUpButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(52, 31);
            this.moveUpButton.TabIndex = 10;
            this.moveUpButton.Text = "+";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // originButton
            // 
            this.originButton.Location = new System.Drawing.Point(124, 184);
            this.originButton.Margin = new System.Windows.Forms.Padding(4);
            this.originButton.Name = "originButton";
            this.originButton.Size = new System.Drawing.Size(107, 31);
            this.originButton.TabIndex = 9;
            this.originButton.Text = "Origin";
            this.originButton.UseVisualStyleBackColor = true;
            this.originButton.Click += new System.EventHandler(this.originButton_Click);
            // 
            // movingStep
            // 
            this.movingStep.FormattingEnabled = true;
            this.movingStep.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "500",
            "1000",
            "5000"});
            this.movingStep.Location = new System.Drawing.Point(123, 125);
            this.movingStep.Name = "movingStep";
            this.movingStep.Size = new System.Drawing.Size(107, 26);
            this.movingStep.TabIndex = 15;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(11, 127);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(38, 18);
            this.labelStep.TabIndex = 14;
            this.labelStep.Text = "Step";
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(124, 91);
            this.moveButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(106, 27);
            this.moveButton.TabIndex = 10;
            this.moveButton.Text = "Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbutton.Location = new System.Drawing.Point(244, 415);
            this.okbutton.Margin = new System.Windows.Forms.Padding(4);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(107, 31);
            this.okbutton.TabIndex = 9;
            this.okbutton.Text = "Close";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // buttonFindLimit
            // 
            this.buttonFindLimit.Location = new System.Drawing.Point(123, 218);
            this.buttonFindLimit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFindLimit.Name = "buttonFindLimit";
            this.buttonFindLimit.Size = new System.Drawing.Size(107, 31);
            this.buttonFindLimit.TabIndex = 9;
            this.buttonFindLimit.Text = "Find Limit";
            this.buttonFindLimit.UseVisualStyleBackColor = true;
            this.buttonFindLimit.Click += new System.EventHandler(this.buttonFindLimit_Click);
            // 
            // labelMotionDoneWaitTime
            // 
            this.labelMotionDoneWaitTime.AutoSize = true;
            this.labelMotionDoneWaitTime.Location = new System.Drawing.Point(11, 297);
            this.labelMotionDoneWaitTime.Name = "labelMotionDoneWaitTime";
            this.labelMotionDoneWaitTime.Size = new System.Drawing.Size(128, 18);
            this.labelMotionDoneWaitTime.TabIndex = 16;
            this.labelMotionDoneWaitTime.Text = "Motion Done Wait";
            // 
            // checkTimer
            // 
            this.checkTimer.Tick += new System.EventHandler(this.checkTimer_Tick);
            // 
            // comboAxisHandler
            // 
            this.comboAxisHandler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAxisHandler.FormattingEnabled = true;
            this.comboAxisHandler.Location = new System.Drawing.Point(124, 7);
            this.comboAxisHandler.Name = "comboAxisHandler";
            this.comboAxisHandler.Size = new System.Drawing.Size(246, 26);
            this.comboAxisHandler.TabIndex = 7;
            this.comboAxisHandler.SelectedIndexChanged += new System.EventHandler(this.comboAxisHandler_SelectedIndexChanged);
            // 
            // labelAxisHandler
            // 
            this.labelAxisHandler.AutoSize = true;
            this.labelAxisHandler.Location = new System.Drawing.Point(11, 12);
            this.labelAxisHandler.Name = "labelAxisHandler";
            this.labelAxisHandler.Size = new System.Drawing.Size(90, 18);
            this.labelAxisHandler.TabIndex = 8;
            this.labelAxisHandler.Text = "Axis Handler";
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(592, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 485);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(592, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 454);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(591, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 454);
            // 
            // MotionControlForm_Fill_Panel
            // 
            // 
            // MotionControlForm_Fill_Panel.ClientArea
            // 
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelMotionDoneWaitTime);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.movingStep);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelStep);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelPosition);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.position);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.moveDownButton);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.moveButton);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.moveUpButton);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.okbutton);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.buttonServoOn);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.buttonFindLimit);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.originButton);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelAxisHandler);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelAxisNo);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.comboAxisHandler);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.comboAxis);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.label2);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelHome);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelRun);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelARst);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelSon);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelCClr);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelErr);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelLimitNeg);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelLimitPos);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelAlarm);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelInp);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelEmg);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelEz);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.labelOrg);
            this.MotionControlForm_Fill_Panel.ClientArea.Controls.Add(this.paramPropertyGrid);
            this.MotionControlForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MotionControlForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MotionControlForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.MotionControlForm_Fill_Panel.Name = "MotionControlForm_Fill_Panel";
            this.MotionControlForm_Fill_Panel.Size = new System.Drawing.Size(590, 454);
            this.MotionControlForm_Fill_Panel.TabIndex = 25;
            // 
            // buttonServoOn
            // 
            this.buttonServoOn.Location = new System.Drawing.Point(123, 252);
            this.buttonServoOn.Margin = new System.Windows.Forms.Padding(4);
            this.buttonServoOn.Name = "buttonServoOn";
            this.buttonServoOn.Size = new System.Drawing.Size(107, 31);
            this.buttonServoOn.TabIndex = 9;
            this.buttonServoOn.Text = "Servo On";
            this.buttonServoOn.UseVisualStyleBackColor = true;
            this.buttonServoOn.Click += new System.EventHandler(this.buttonServoOn_Click);
            // 
            // MotionControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 486);
            this.Controls.Add(this.MotionControlForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MotionControlForm";
            this.Text = "Motion Control";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MotionControlForm_FormClosing);
            this.Load += new System.EventHandler(this.MotionControlForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.position)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.MotionControlForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MotionControlForm_Fill_Panel.ClientArea.PerformLayout();
            this.MotionControlForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid paramPropertyGrid;
        private System.Windows.Forms.Label labelOrg;
        private System.Windows.Forms.Label labelEz;
        private System.Windows.Forms.Label labelEmg;
        private System.Windows.Forms.Label labelInp;
        private System.Windows.Forms.Label labelAlarm;
        private System.Windows.Forms.Label labelLimitPos;
        private System.Windows.Forms.Label labelLimitNeg;
        private System.Windows.Forms.Label labelRun;
        private System.Windows.Forms.Label labelHome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelErr;
        private System.Windows.Forms.Label labelCClr;
        private System.Windows.Forms.Label labelSon;
        private System.Windows.Forms.Label labelARst;
        private System.Windows.Forms.ComboBox comboAxis;
        private System.Windows.Forms.Label labelAxisNo;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.NumericUpDown position;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button originButton;
        private System.Windows.Forms.ComboBox movingStep;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Button buttonFindLimit;
        private System.Windows.Forms.Label labelMotionDoneWaitTime;
        private System.Windows.Forms.Timer checkTimer;
        private System.Windows.Forms.ComboBox comboAxisHandler;
        private System.Windows.Forms.Label labelAxisHandler;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel MotionControlForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button buttonServoOn;
    }
}