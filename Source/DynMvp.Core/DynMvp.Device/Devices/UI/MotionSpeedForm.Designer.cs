namespace DynMvp.Devices.UI
{
    partial class MotionSpeedForm
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
            this.comboAxis = new System.Windows.Forms.ComboBox();
            this.labelAxisNo = new System.Windows.Forms.Label();
            this.okbutton = new System.Windows.Forms.Button();
            this.movingStep = new System.Windows.Forms.ComboBox();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.labelJog = new System.Windows.Forms.Label();
            this.comboAxisHandler = new System.Windows.Forms.ComboBox();
            this.labelAxisHandler = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.MotionSpeedForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.MotionSpeedForm_Fill_Panel.ClientArea.SuspendLayout();
            this.MotionSpeedForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // paramPropertyGrid
            // 
            this.paramPropertyGrid.HelpVisible = false;
            this.paramPropertyGrid.Location = new System.Drawing.Point(13, 115);
            this.paramPropertyGrid.Margin = new System.Windows.Forms.Padding(4);
            this.paramPropertyGrid.Name = "paramPropertyGrid";
            this.paramPropertyGrid.Size = new System.Drawing.Size(456, 332);
            this.paramPropertyGrid.TabIndex = 4;
            // 
            // comboAxis
            // 
            this.comboAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAxis.FormattingEnabled = true;
            this.comboAxis.Location = new System.Drawing.Point(102, 43);
            this.comboAxis.Name = "comboAxis";
            this.comboAxis.Size = new System.Drawing.Size(367, 26);
            this.comboAxis.TabIndex = 7;
            this.comboAxis.SelectedIndexChanged += new System.EventHandler(this.axisNo_SelectedIndexChanged);
            // 
            // labelAxisNo
            // 
            this.labelAxisNo.AutoSize = true;
            this.labelAxisNo.Location = new System.Drawing.Point(11, 45);
            this.labelAxisNo.Name = "labelAxisNo";
            this.labelAxisNo.Size = new System.Drawing.Size(35, 18);
            this.labelAxisNo.TabIndex = 8;
            this.labelAxisNo.Text = "Axis";
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbutton.Location = new System.Drawing.Point(141, 455);
            this.okbutton.Margin = new System.Windows.Forms.Padding(4);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(191, 52);
            this.okbutton.TabIndex = 9;
            this.okbutton.Text = "Close";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
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
            this.movingStep.Location = new System.Drawing.Point(102, 79);
            this.movingStep.Name = "movingStep";
            this.movingStep.Size = new System.Drawing.Size(248, 26);
            this.movingStep.TabIndex = 18;
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(417, 76);
            this.moveDownButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(52, 31);
            this.moveDownButton.TabIndex = 17;
            this.moveDownButton.Text = "-";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(357, 76);
            this.moveUpButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(52, 31);
            this.moveUpButton.TabIndex = 16;
            this.moveUpButton.Text = "+";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // labelJog
            // 
            this.labelJog.AutoSize = true;
            this.labelJog.Location = new System.Drawing.Point(11, 82);
            this.labelJog.Name = "labelJog";
            this.labelJog.Size = new System.Drawing.Size(33, 18);
            this.labelJog.TabIndex = 8;
            this.labelJog.Text = "Jog";
            // 
            // comboAxisHandler
            // 
            this.comboAxisHandler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAxisHandler.FormattingEnabled = true;
            this.comboAxisHandler.Location = new System.Drawing.Point(102, 12);
            this.comboAxisHandler.Name = "comboAxisHandler";
            this.comboAxisHandler.Size = new System.Drawing.Size(367, 26);
            this.comboAxisHandler.TabIndex = 7;
            this.comboAxisHandler.SelectedIndexChanged += new System.EventHandler(this.comboAxisHandler_SelectedIndexChanged);
            // 
            // labelAxisHandler
            // 
            this.labelAxisHandler.AutoSize = true;
            this.labelAxisHandler.Location = new System.Drawing.Point(11, 14);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(488, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 571);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(488, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 540);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(487, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 540);
            // 
            // MotionSpeedForm_Fill_Panel
            // 
            // 
            // MotionSpeedForm_Fill_Panel.ClientArea
            // 
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.movingStep);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.moveDownButton);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.moveUpButton);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.okbutton);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.labelJog);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.labelAxisHandler);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.labelAxisNo);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.comboAxisHandler);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.comboAxis);
            this.MotionSpeedForm_Fill_Panel.ClientArea.Controls.Add(this.paramPropertyGrid);
            this.MotionSpeedForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MotionSpeedForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MotionSpeedForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.MotionSpeedForm_Fill_Panel.Name = "MotionSpeedForm_Fill_Panel";
            this.MotionSpeedForm_Fill_Panel.Size = new System.Drawing.Size(486, 540);
            this.MotionSpeedForm_Fill_Panel.TabIndex = 27;
            // 
            // MotionSpeedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 572);
            this.Controls.Add(this.MotionSpeedForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(1246, 100);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MotionSpeedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Motion Speed";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MotionControlForm_FormClosing);
            this.Load += new System.EventHandler(this.MotionControlForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.MotionSpeedForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MotionSpeedForm_Fill_Panel.ClientArea.PerformLayout();
            this.MotionSpeedForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid paramPropertyGrid;
        private System.Windows.Forms.ComboBox comboAxis;
        private System.Windows.Forms.Label labelAxisNo;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.ComboBox movingStep;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Label labelJog;
        private System.Windows.Forms.ComboBox comboAxisHandler;
        private System.Windows.Forms.Label labelAxisHandler;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel MotionSpeedForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}