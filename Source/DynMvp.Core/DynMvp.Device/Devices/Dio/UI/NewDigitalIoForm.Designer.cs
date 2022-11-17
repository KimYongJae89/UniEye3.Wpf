namespace DynMvp.Devices.Dio.UI
{
    partial class NewDigitalIoForm
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
            this.cmbDigitalIoType = new System.Windows.Forms.ComboBox();
            this.labelDigitalIoType = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.NewDigitalIoForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.groupBoxOutPort = new System.Windows.Forms.GroupBox();
            this.numOutPort = new System.Windows.Forms.NumericUpDown();
            this.labelNumOutPort = new System.Windows.Forms.Label();
            this.outPortStartGroupIndex = new System.Windows.Forms.NumericUpDown();
            this.labelOutPortStartGroupIndex = new System.Windows.Forms.Label();
            this.labelNumOutPortGroup = new System.Windows.Forms.Label();
            this.numOutPortGroup = new System.Windows.Forms.NumericUpDown();
            this.groupBoxInPort = new System.Windows.Forms.GroupBox();
            this.numInPort = new System.Windows.Forms.NumericUpDown();
            this.labelNumInPort = new System.Windows.Forms.Label();
            this.inPortStartGroupIndex = new System.Windows.Forms.NumericUpDown();
            this.labelInPortStartGroupIndex = new System.Windows.Forms.Label();
            this.numInPortGroup = new System.Windows.Forms.NumericUpDown();
            this.labelNumInPortGroup = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.NewDigitalIoForm_Fill_Panel.ClientArea.SuspendLayout();
            this.NewDigitalIoForm_Fill_Panel.SuspendLayout();
            this.groupBoxOutPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outPortStartGroupIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPortGroup)).BeginInit();
            this.groupBoxInPort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inPortStartGroupIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInPortGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDigitalIoType
            // 
            this.cmbDigitalIoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDigitalIoType.FormattingEnabled = true;
            this.cmbDigitalIoType.Items.AddRange(new object[] {
            "Virtual",
            "Pylon",
            "MultiCam",
            "uEye",
            "MIL"});
            this.cmbDigitalIoType.Location = new System.Drawing.Point(175, 47);
            this.cmbDigitalIoType.Margin = new System.Windows.Forms.Padding(8, 12, 8, 12);
            this.cmbDigitalIoType.Name = "cmbDigitalIoType";
            this.cmbDigitalIoType.Size = new System.Drawing.Size(142, 26);
            this.cmbDigitalIoType.TabIndex = 156;
            this.cmbDigitalIoType.SelectedIndexChanged += new System.EventHandler(this.cmbDigitalIoType_SelectedIndexChanged);
            // 
            // labelDigitalIoType
            // 
            this.labelDigitalIoType.AutoSize = true;
            this.labelDigitalIoType.Location = new System.Drawing.Point(8, 50);
            this.labelDigitalIoType.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelDigitalIoType.Name = "labelDigitalIoType";
            this.labelDigitalIoType.Size = new System.Drawing.Size(40, 18);
            this.labelDigitalIoType.TabIndex = 158;
            this.labelDigitalIoType.Text = "Type";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(164, 339);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 37);
            this.buttonCancel.TabIndex = 159;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(75, 339);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 37);
            this.buttonOK.TabIndex = 160;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(330, 30);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 410);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(330, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 380);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(329, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 380);
            // 
            // NewDigitalIoForm_Fill_Panel
            // 
            // 
            // NewDigitalIoForm_Fill_Panel.ClientArea
            // 
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxOutPort);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxInPort);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.txtName);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.labelDigitalIoType);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.Controls.Add(this.cmbDigitalIoType);
            this.NewDigitalIoForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.NewDigitalIoForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewDigitalIoForm_Fill_Panel.Location = new System.Drawing.Point(1, 30);
            this.NewDigitalIoForm_Fill_Panel.Name = "NewDigitalIoForm_Fill_Panel";
            this.NewDigitalIoForm_Fill_Panel.Size = new System.Drawing.Size(328, 380);
            this.NewDigitalIoForm_Fill_Panel.TabIndex = 169;
            // 
            // groupBoxOutPort
            // 
            this.groupBoxOutPort.Controls.Add(this.numOutPort);
            this.groupBoxOutPort.Controls.Add(this.labelNumOutPort);
            this.groupBoxOutPort.Controls.Add(this.outPortStartGroupIndex);
            this.groupBoxOutPort.Controls.Add(this.labelOutPortStartGroupIndex);
            this.groupBoxOutPort.Controls.Add(this.labelNumOutPortGroup);
            this.groupBoxOutPort.Controls.Add(this.numOutPortGroup);
            this.groupBoxOutPort.Location = new System.Drawing.Point(11, 210);
            this.groupBoxOutPort.Name = "groupBoxOutPort";
            this.groupBoxOutPort.Size = new System.Drawing.Size(311, 123);
            this.groupBoxOutPort.TabIndex = 177;
            this.groupBoxOutPort.TabStop = false;
            this.groupBoxOutPort.Text = "Output Port";
            // 
            // numOutPort
            // 
            this.numOutPort.Location = new System.Drawing.Point(159, 88);
            this.numOutPort.Name = "numOutPort";
            this.numOutPort.Size = new System.Drawing.Size(132, 24);
            this.numOutPort.TabIndex = 175;
            // 
            // labelNumOutPort
            // 
            this.labelNumOutPort.AutoSize = true;
            this.labelNumOutPort.Location = new System.Drawing.Point(11, 90);
            this.labelNumOutPort.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelNumOutPort.Name = "labelNumOutPort";
            this.labelNumOutPort.Size = new System.Drawing.Size(110, 18);
            this.labelNumOutPort.TabIndex = 158;
            this.labelNumOutPort.Text = "Number of Port";
            // 
            // outPortStartGroupIndex
            // 
            this.outPortStartGroupIndex.Location = new System.Drawing.Point(159, 58);
            this.outPortStartGroupIndex.Name = "outPortStartGroupIndex";
            this.outPortStartGroupIndex.Size = new System.Drawing.Size(132, 24);
            this.outPortStartGroupIndex.TabIndex = 175;
            // 
            // labelOutPortStartGroupIndex
            // 
            this.labelOutPortStartGroupIndex.AutoSize = true;
            this.labelOutPortStartGroupIndex.Location = new System.Drawing.Point(14, 60);
            this.labelOutPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelOutPortStartGroupIndex.Name = "labelOutPortStartGroupIndex";
            this.labelOutPortStartGroupIndex.Size = new System.Drawing.Size(123, 18);
            this.labelOutPortStartGroupIndex.TabIndex = 158;
            this.labelOutPortStartGroupIndex.Text = "Start Group Index";
            // 
            // labelNumOutPortGroup
            // 
            this.labelNumOutPortGroup.AutoSize = true;
            this.labelNumOutPortGroup.Location = new System.Drawing.Point(14, 30);
            this.labelNumOutPortGroup.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelNumOutPortGroup.Name = "labelNumOutPortGroup";
            this.labelNumOutPortGroup.Size = new System.Drawing.Size(124, 18);
            this.labelNumOutPortGroup.TabIndex = 158;
            this.labelNumOutPortGroup.Text = "Number of Group";
            // 
            // numOutPortGroup
            // 
            this.numOutPortGroup.Location = new System.Drawing.Point(159, 28);
            this.numOutPortGroup.Name = "numOutPortGroup";
            this.numOutPortGroup.Size = new System.Drawing.Size(132, 24);
            this.numOutPortGroup.TabIndex = 175;
            // 
            // groupBoxInPort
            // 
            this.groupBoxInPort.Controls.Add(this.numInPort);
            this.groupBoxInPort.Controls.Add(this.labelNumInPort);
            this.groupBoxInPort.Controls.Add(this.inPortStartGroupIndex);
            this.groupBoxInPort.Controls.Add(this.labelInPortStartGroupIndex);
            this.groupBoxInPort.Controls.Add(this.numInPortGroup);
            this.groupBoxInPort.Controls.Add(this.labelNumInPortGroup);
            this.groupBoxInPort.Location = new System.Drawing.Point(11, 79);
            this.groupBoxInPort.Name = "groupBoxInPort";
            this.groupBoxInPort.Size = new System.Drawing.Size(311, 125);
            this.groupBoxInPort.TabIndex = 176;
            this.groupBoxInPort.TabStop = false;
            this.groupBoxInPort.Text = "Input Port";
            // 
            // numInPort
            // 
            this.numInPort.Location = new System.Drawing.Point(161, 85);
            this.numInPort.Name = "numInPort";
            this.numInPort.Size = new System.Drawing.Size(132, 24);
            this.numInPort.TabIndex = 175;
            // 
            // labelNumInPort
            // 
            this.labelNumInPort.AutoSize = true;
            this.labelNumInPort.Location = new System.Drawing.Point(14, 87);
            this.labelNumInPort.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelNumInPort.Name = "labelNumInPort";
            this.labelNumInPort.Size = new System.Drawing.Size(110, 18);
            this.labelNumInPort.TabIndex = 158;
            this.labelNumInPort.Text = "Number of Port";
            // 
            // inPortStartGroupIndex
            // 
            this.inPortStartGroupIndex.Location = new System.Drawing.Point(161, 55);
            this.inPortStartGroupIndex.Name = "inPortStartGroupIndex";
            this.inPortStartGroupIndex.Size = new System.Drawing.Size(132, 24);
            this.inPortStartGroupIndex.TabIndex = 175;
            // 
            // labelInPortStartGroupIndex
            // 
            this.labelInPortStartGroupIndex.AutoSize = true;
            this.labelInPortStartGroupIndex.Location = new System.Drawing.Point(16, 57);
            this.labelInPortStartGroupIndex.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelInPortStartGroupIndex.Name = "labelInPortStartGroupIndex";
            this.labelInPortStartGroupIndex.Size = new System.Drawing.Size(123, 18);
            this.labelInPortStartGroupIndex.TabIndex = 158;
            this.labelInPortStartGroupIndex.Text = "Start Group Index";
            // 
            // numInPortGroup
            // 
            this.numInPortGroup.Location = new System.Drawing.Point(161, 25);
            this.numInPortGroup.Name = "numInPortGroup";
            this.numInPortGroup.Size = new System.Drawing.Size(132, 24);
            this.numInPortGroup.TabIndex = 175;
            // 
            // labelNumInPortGroup
            // 
            this.labelNumInPortGroup.AutoSize = true;
            this.labelNumInPortGroup.Location = new System.Drawing.Point(16, 27);
            this.labelNumInPortGroup.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelNumInPortGroup.Name = "labelNumInPortGroup";
            this.labelNumInPortGroup.Size = new System.Drawing.Size(124, 18);
            this.labelNumInPortGroup.TabIndex = 158;
            this.labelNumInPortGroup.Text = "Number of Group";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(175, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(142, 24);
            this.txtName.TabIndex = 174;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(8, 17);
            this.labelName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 18);
            this.labelName.TabIndex = 173;
            this.labelName.Text = "Name";
            // 
            // NewDigitalIoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 411);
            this.Controls.Add(this.NewDigitalIoForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewDigitalIoForm";
            this.Text = "New Digital I/O";
            this.Load += new System.EventHandler(this.GrabberInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.NewDigitalIoForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.NewDigitalIoForm_Fill_Panel.ClientArea.PerformLayout();
            this.NewDigitalIoForm_Fill_Panel.ResumeLayout(false);
            this.groupBoxOutPort.ResumeLayout(false);
            this.groupBoxOutPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outPortStartGroupIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutPortGroup)).EndInit();
            this.groupBoxInPort.ResumeLayout(false);
            this.groupBoxInPort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inPortStartGroupIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInPortGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbDigitalIoType;
        private System.Windows.Forms.Label labelDigitalIoType;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel NewDigitalIoForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.NumericUpDown numOutPort;
        private System.Windows.Forms.NumericUpDown numInPort;
        private System.Windows.Forms.Label labelNumOutPort;
        private System.Windows.Forms.Label labelNumInPort;
        private System.Windows.Forms.GroupBox groupBoxInPort;
        private System.Windows.Forms.NumericUpDown numInPortGroup;
        private System.Windows.Forms.Label labelNumInPortGroup;
        private System.Windows.Forms.NumericUpDown inPortStartGroupIndex;
        private System.Windows.Forms.Label labelInPortStartGroupIndex;
        private System.Windows.Forms.GroupBox groupBoxOutPort;
        private System.Windows.Forms.NumericUpDown outPortStartGroupIndex;
        private System.Windows.Forms.Label labelOutPortStartGroupIndex;
        private System.Windows.Forms.Label labelNumOutPortGroup;
        private System.Windows.Forms.NumericUpDown numOutPortGroup;
    }
}