namespace DynMvp.Devices.Dio.UI
{
    partial class SerialDigitalIoInfoForm
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
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.SerialDigitalIoInfoForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupDoPort1 = new System.Windows.Forms.GroupBox();
            this.doPort1DTR = new System.Windows.Forms.CheckBox();
            this.doPort1RTS = new System.Windows.Forms.CheckBox();
            this.groupDoPort0 = new System.Windows.Forms.GroupBox();
            this.doPort0DTR = new System.Windows.Forms.CheckBox();
            this.doPort0RTS = new System.Windows.Forms.CheckBox();
            this.comPort = new System.Windows.Forms.ComboBox();
            this.labelComPort = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.buttonTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.SuspendLayout();
            this.SerialDigitalIoInfoForm_Fill_Panel.SuspendLayout();
            this.groupDoPort1.SuspendLayout();
            this.groupDoPort0.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager1.FormStyleSettings.CaptionAreaAppearance = appearance1;
            this.ultraFormManager1.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // SerialDigitalIoInfoForm_Fill_Panel
            // 
            // 
            // SerialDigitalIoInfoForm_Fill_Panel.ClientArea
            // 
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonTest);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.groupDoPort1);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.groupDoPort0);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.comPort);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.labelComPort);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.name);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.SerialDigitalIoInfoForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SerialDigitalIoInfoForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerialDigitalIoInfoForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.SerialDigitalIoInfoForm_Fill_Panel.Name = "SerialDigitalIoInfoForm_Fill_Panel";
            this.SerialDigitalIoInfoForm_Fill_Panel.Size = new System.Drawing.Size(345, 254);
            this.SerialDigitalIoInfoForm_Fill_Panel.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(251, 213);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 29);
            this.buttonCancel.TabIndex = 180;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(165, 213);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 29);
            this.buttonOK.TabIndex = 181;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupDoPort1
            // 
            this.groupDoPort1.Controls.Add(this.doPort1DTR);
            this.groupDoPort1.Controls.Add(this.doPort1RTS);
            this.groupDoPort1.Location = new System.Drawing.Point(177, 83);
            this.groupDoPort1.Name = "groupDoPort1";
            this.groupDoPort1.Size = new System.Drawing.Size(158, 119);
            this.groupDoPort1.TabIndex = 179;
            this.groupDoPort1.TabStop = false;
            this.groupDoPort1.Text = "OUT PORT1";
            // 
            // doPort1DTR
            // 
            this.doPort1DTR.AutoSize = true;
            this.doPort1DTR.Location = new System.Drawing.Point(33, 73);
            this.doPort1DTR.Name = "doPort1DTR";
            this.doPort1DTR.Size = new System.Drawing.Size(58, 22);
            this.doPort1DTR.TabIndex = 0;
            this.doPort1DTR.Text = "DTR";
            this.doPort1DTR.UseVisualStyleBackColor = true;
            this.doPort1DTR.CheckedChanged += new System.EventHandler(this.doPort_CheckedChanged);
            // 
            // doPort1RTS
            // 
            this.doPort1RTS.AutoSize = true;
            this.doPort1RTS.Location = new System.Drawing.Point(33, 33);
            this.doPort1RTS.Name = "doPort1RTS";
            this.doPort1RTS.Size = new System.Drawing.Size(57, 22);
            this.doPort1RTS.TabIndex = 0;
            this.doPort1RTS.Text = "RTS";
            this.doPort1RTS.UseVisualStyleBackColor = true;
            this.doPort1RTS.CheckedChanged += new System.EventHandler(this.doPort_CheckedChanged);
            // 
            // groupDoPort0
            // 
            this.groupDoPort0.Controls.Add(this.doPort0DTR);
            this.groupDoPort0.Controls.Add(this.doPort0RTS);
            this.groupDoPort0.Location = new System.Drawing.Point(11, 83);
            this.groupDoPort0.Name = "groupDoPort0";
            this.groupDoPort0.Size = new System.Drawing.Size(158, 119);
            this.groupDoPort0.TabIndex = 179;
            this.groupDoPort0.TabStop = false;
            this.groupDoPort0.Text = "OUT PORT0";
            // 
            // doPort0DTR
            // 
            this.doPort0DTR.AutoSize = true;
            this.doPort0DTR.Location = new System.Drawing.Point(33, 73);
            this.doPort0DTR.Name = "doPort0DTR";
            this.doPort0DTR.Size = new System.Drawing.Size(58, 22);
            this.doPort0DTR.TabIndex = 0;
            this.doPort0DTR.Text = "DTR";
            this.doPort0DTR.UseVisualStyleBackColor = true;
            this.doPort0DTR.CheckedChanged += new System.EventHandler(this.doPort_CheckedChanged);
            // 
            // doPort0RTS
            // 
            this.doPort0RTS.AutoSize = true;
            this.doPort0RTS.Location = new System.Drawing.Point(33, 33);
            this.doPort0RTS.Name = "doPort0RTS";
            this.doPort0RTS.Size = new System.Drawing.Size(57, 22);
            this.doPort0RTS.TabIndex = 0;
            this.doPort0RTS.Text = "RTS";
            this.doPort0RTS.UseVisualStyleBackColor = true;
            this.doPort0RTS.CheckedChanged += new System.EventHandler(this.doPort_CheckedChanged);
            // 
            // comPort
            // 
            this.comPort.FormattingEnabled = true;
            this.comPort.Location = new System.Drawing.Point(183, 39);
            this.comPort.Name = "comPort";
            this.comPort.Size = new System.Drawing.Size(144, 26);
            this.comPort.TabIndex = 178;
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Location = new System.Drawing.Point(13, 43);
            this.labelComPort.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(76, 18);
            this.labelComPort.TabIndex = 177;
            this.labelComPort.Text = "COM Port";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(183, 9);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(144, 24);
            this.name.TabIndex = 176;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(13, 12);
            this.labelName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 18);
            this.labelName.TabIndex = 175;
            this.labelName.Text = "Name";
            // 
            // _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left
            // 
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.Name = "_SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left";
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 254);
            // 
            // _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right
            // 
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(346, 31);
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.Name = "_SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right";
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 254);
            // 
            // _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top
            // 
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.Name = "_SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top";
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(347, 31);
            // 
            // _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 285);
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.Name = "_SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom";
            this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(347, 1);
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonTest.Location = new System.Drawing.Point(11, 213);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(83, 29);
            this.buttonTest.TabIndex = 181;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // SerialDigitalIoInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 286);
            this.Controls.Add(this.SerialDigitalIoInfoForm_Fill_Panel);
            this.Controls.Add(this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SerialDigitalIoInfoForm";
            this.Text = "Serial Digital I/O Info";
            this.Load += new System.EventHandler(this.SerialDigitalIoInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SerialDigitalIoInfoForm_Fill_Panel.ClientArea.PerformLayout();
            this.SerialDigitalIoInfoForm_Fill_Panel.ResumeLayout(false);
            this.groupDoPort1.ResumeLayout(false);
            this.groupDoPort1.PerformLayout();
            this.groupDoPort0.ResumeLayout(false);
            this.groupDoPort0.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel SerialDigitalIoInfoForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SerialDigitalIoInfoForm_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comPort;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.GroupBox groupDoPort1;
        private System.Windows.Forms.CheckBox doPort1DTR;
        private System.Windows.Forms.CheckBox doPort1RTS;
        private System.Windows.Forms.GroupBox groupDoPort0;
        private System.Windows.Forms.CheckBox doPort0DTR;
        private System.Windows.Forms.CheckBox doPort0RTS;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonTest;
    }
}