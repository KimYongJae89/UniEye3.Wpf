namespace DynMvp.Devices.UI
{
    partial class RobotPositionForm
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
            this.panelFovNavigator = new System.Windows.Forms.Panel();
            this.panelPosition = new System.Windows.Forms.Panel();
            this.moveStep = new System.Windows.Forms.ComboBox();
            this.xPosition = new System.Windows.Forms.NumericUpDown();
            this.yPosition = new System.Windows.Forms.NumericUpDown();
            this.labelYPosition = new System.Windows.Forms.Label();
            this.labelStep = new System.Windows.Forms.Label();
            this.labelXPosition = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxUnload = new System.Windows.Forms.GroupBox();
            this.buttonMoveUnloadPosition = new System.Windows.Forms.Button();
            this.buttonSetUnloadPosition = new System.Windows.Forms.Button();
            this.groupBoxLoadPosition = new System.Windows.Forms.GroupBox();
            this.buttonMoveLoadPosition = new System.Windows.Forms.Button();
            this.buttonSetLoadPosition = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.panelFovNavigator.SuspendLayout();
            this.panelPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPosition)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.groupBoxUnload.SuspendLayout();
            this.groupBoxLoadPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFovNavigator
            // 
            this.panelFovNavigator.Controls.Add(this.panelPosition);
            this.panelFovNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFovNavigator.Location = new System.Drawing.Point(1, 31);
            this.panelFovNavigator.Name = "panelFovNavigator";
            this.panelFovNavigator.Size = new System.Drawing.Size(490, 259);
            this.panelFovNavigator.TabIndex = 0;
            // 
            // panelPosition
            // 
            this.panelPosition.Controls.Add(this.moveStep);
            this.panelPosition.Controls.Add(this.xPosition);
            this.panelPosition.Controls.Add(this.yPosition);
            this.panelPosition.Controls.Add(this.labelYPosition);
            this.panelPosition.Controls.Add(this.labelStep);
            this.panelPosition.Controls.Add(this.labelXPosition);
            this.panelPosition.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPosition.Location = new System.Drawing.Point(0, 222);
            this.panelPosition.Name = "panelPosition";
            this.panelPosition.Size = new System.Drawing.Size(490, 37);
            this.panelPosition.TabIndex = 12;
            // 
            // moveStep
            // 
            this.moveStep.FormattingEnabled = true;
            this.moveStep.Items.AddRange(new object[] {
            "100",
            "500",
            "1000",
            "5000",
            "10000"});
            this.moveStep.Location = new System.Drawing.Point(370, 5);
            this.moveStep.Name = "moveStep";
            this.moveStep.Size = new System.Drawing.Size(108, 20);
            this.moveStep.TabIndex = 8;
            // 
            // xPosition
            // 
            this.xPosition.Location = new System.Drawing.Point(47, 5);
            this.xPosition.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.xPosition.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.xPosition.Name = "xPosition";
            this.xPosition.Size = new System.Drawing.Size(97, 21);
            this.xPosition.TabIndex = 4;
            this.xPosition.ValueChanged += new System.EventHandler(this.position_ValueChanged);
            // 
            // yPosition
            // 
            this.yPosition.Location = new System.Drawing.Point(186, 5);
            this.yPosition.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.yPosition.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.yPosition.Name = "yPosition";
            this.yPosition.Size = new System.Drawing.Size(97, 21);
            this.yPosition.TabIndex = 4;
            this.yPosition.ValueChanged += new System.EventHandler(this.position_ValueChanged);
            // 
            // labelYPosition
            // 
            this.labelYPosition.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelYPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelYPosition.Location = new System.Drawing.Point(150, 6);
            this.labelYPosition.Name = "labelYPosition";
            this.labelYPosition.Size = new System.Drawing.Size(30, 20);
            this.labelYPosition.TabIndex = 7;
            this.labelYPosition.Text = "Y";
            this.labelYPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStep
            // 
            this.labelStep.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStep.Location = new System.Drawing.Point(289, 7);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(75, 18);
            this.labelStep.TabIndex = 6;
            this.labelStep.Text = "Step";
            this.labelStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelXPosition
            // 
            this.labelXPosition.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelXPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelXPosition.Location = new System.Drawing.Point(11, 6);
            this.labelXPosition.Name = "labelXPosition";
            this.labelXPosition.Size = new System.Drawing.Size(30, 20);
            this.labelXPosition.TabIndex = 6;
            this.labelXPosition.Text = "X";
            this.labelXPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonOK);
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Controls.Add(this.groupBoxUnload);
            this.panelButtons.Controls.Add(this.groupBoxLoadPosition);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelButtons.Location = new System.Drawing.Point(491, 31);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(178, 259);
            this.panelButtons.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(16, 215);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(68, 35);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(90, 215);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 35);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxUnload
            // 
            this.groupBoxUnload.Controls.Add(this.buttonMoveUnloadPosition);
            this.groupBoxUnload.Controls.Add(this.buttonSetUnloadPosition);
            this.groupBoxUnload.Location = new System.Drawing.Point(10, 83);
            this.groupBoxUnload.Name = "groupBoxUnload";
            this.groupBoxUnload.Size = new System.Drawing.Size(157, 66);
            this.groupBoxUnload.TabIndex = 3;
            this.groupBoxUnload.TabStop = false;
            this.groupBoxUnload.Text = "Unload Position";
            // 
            // buttonMoveUnloadPosition
            // 
            this.buttonMoveUnloadPosition.Location = new System.Drawing.Point(6, 20);
            this.buttonMoveUnloadPosition.Name = "buttonMoveUnloadPosition";
            this.buttonMoveUnloadPosition.Size = new System.Drawing.Size(68, 35);
            this.buttonMoveUnloadPosition.TabIndex = 1;
            this.buttonMoveUnloadPosition.Text = "Move";
            this.buttonMoveUnloadPosition.UseVisualStyleBackColor = true;
            this.buttonMoveUnloadPosition.Click += new System.EventHandler(this.buttonMoveUnloadPosition_Click);
            // 
            // buttonSetUnloadPosition
            // 
            this.buttonSetUnloadPosition.Location = new System.Drawing.Point(80, 20);
            this.buttonSetUnloadPosition.Name = "buttonSetUnloadPosition";
            this.buttonSetUnloadPosition.Size = new System.Drawing.Size(68, 35);
            this.buttonSetUnloadPosition.TabIndex = 0;
            this.buttonSetUnloadPosition.Text = "Set";
            this.buttonSetUnloadPosition.UseVisualStyleBackColor = true;
            this.buttonSetUnloadPosition.Click += new System.EventHandler(this.buttonSetUnloadPosition_Click);
            // 
            // groupBoxLoadPosition
            // 
            this.groupBoxLoadPosition.Controls.Add(this.buttonMoveLoadPosition);
            this.groupBoxLoadPosition.Controls.Add(this.buttonSetLoadPosition);
            this.groupBoxLoadPosition.Location = new System.Drawing.Point(10, 11);
            this.groupBoxLoadPosition.Name = "groupBoxLoadPosition";
            this.groupBoxLoadPosition.Size = new System.Drawing.Size(157, 66);
            this.groupBoxLoadPosition.TabIndex = 2;
            this.groupBoxLoadPosition.TabStop = false;
            this.groupBoxLoadPosition.Text = "Load Position";
            // 
            // buttonMoveLoadPosition
            // 
            this.buttonMoveLoadPosition.Location = new System.Drawing.Point(6, 20);
            this.buttonMoveLoadPosition.Name = "buttonMoveLoadPosition";
            this.buttonMoveLoadPosition.Size = new System.Drawing.Size(68, 35);
            this.buttonMoveLoadPosition.TabIndex = 1;
            this.buttonMoveLoadPosition.Text = "Move";
            this.buttonMoveLoadPosition.UseVisualStyleBackColor = true;
            this.buttonMoveLoadPosition.Click += new System.EventHandler(this.buttonMoveLoadPosition_Click);
            // 
            // buttonSetLoadPosition
            // 
            this.buttonSetLoadPosition.Location = new System.Drawing.Point(80, 20);
            this.buttonSetLoadPosition.Name = "buttonSetLoadPosition";
            this.buttonSetLoadPosition.Size = new System.Drawing.Size(68, 35);
            this.buttonSetLoadPosition.TabIndex = 0;
            this.buttonSetLoadPosition.Text = "Set";
            this.buttonSetLoadPosition.UseVisualStyleBackColor = true;
            this.buttonSetLoadPosition.Click += new System.EventHandler(this.buttonSetLoadPosition_Click);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(670, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 290);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(670, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 259);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(669, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 259);
            // 
            // RobotPositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 291);
            this.Controls.Add(this.panelFovNavigator);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Name = "RobotPositionForm";
            this.Text = "Robot Position";
            this.panelFovNavigator.ResumeLayout(false);
            this.panelPosition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yPosition)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.groupBoxUnload.ResumeLayout(false);
            this.groupBoxLoadPosition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFovNavigator;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelPosition;
        private System.Windows.Forms.ComboBox moveStep;
        private System.Windows.Forms.NumericUpDown xPosition;
        private System.Windows.Forms.NumericUpDown yPosition;
        private System.Windows.Forms.Label labelYPosition;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Label labelXPosition;
        private System.Windows.Forms.Button buttonSetLoadPosition;
        private System.Windows.Forms.Button buttonMoveLoadPosition;
        private System.Windows.Forms.GroupBox groupBoxLoadPosition;
        private System.Windows.Forms.GroupBox groupBoxUnload;
        private System.Windows.Forms.Button buttonMoveUnloadPosition;
        private System.Windows.Forms.Button buttonSetUnloadPosition;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}