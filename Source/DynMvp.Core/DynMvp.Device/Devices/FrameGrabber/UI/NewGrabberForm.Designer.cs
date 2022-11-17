namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class NewGrabberForm
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
            this.cmbGrabberType = new System.Windows.Forms.ComboBox();
            this.labelGrabberType = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.NewGrabberForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.txtNumCamera = new System.Windows.Forms.NumericUpDown();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelNumCamera = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.NewGrabberForm_Fill_Panel.ClientArea.SuspendLayout();
            this.NewGrabberForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbGrabberType
            // 
            this.cmbGrabberType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrabberType.FormattingEnabled = true;
            this.cmbGrabberType.Items.AddRange(new object[] {
            "Virtual",
            "Pylon",
            "MultiCam",
            "uEye",
            "MIL"});
            this.cmbGrabberType.Location = new System.Drawing.Point(108, 45);
            this.cmbGrabberType.Margin = new System.Windows.Forms.Padding(8, 12, 8, 12);
            this.cmbGrabberType.Name = "cmbGrabberType";
            this.cmbGrabberType.Size = new System.Drawing.Size(153, 26);
            this.cmbGrabberType.TabIndex = 156;
            this.cmbGrabberType.SelectedIndexChanged += new System.EventHandler(this.CmbGrabberType_SelectedIndexChanged);
            // 
            // labelGrabberType
            // 
            this.labelGrabberType.AutoSize = true;
            this.labelGrabberType.Location = new System.Drawing.Point(8, 45);
            this.labelGrabberType.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelGrabberType.Name = "labelGrabberType";
            this.labelGrabberType.Size = new System.Drawing.Size(40, 18);
            this.labelGrabberType.TabIndex = 158;
            this.labelGrabberType.Text = "Type";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(148, 126);
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
            this.buttonOK.Location = new System.Drawing.Point(59, 126);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 37);
            this.buttonOK.TabIndex = 160;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(290, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 198);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(290, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 167);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(289, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 167);
            // 
            // NewGrabberForm_Fill_Panel
            // 
            // 
            // NewGrabberForm_Fill_Panel.ClientArea
            // 
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.txtNumCamera);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.txtName);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.labelNumCamera);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.labelGrabberType);
            this.NewGrabberForm_Fill_Panel.ClientArea.Controls.Add(this.cmbGrabberType);
            this.NewGrabberForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.NewGrabberForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewGrabberForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.NewGrabberForm_Fill_Panel.Name = "NewGrabberForm_Fill_Panel";
            this.NewGrabberForm_Fill_Panel.Size = new System.Drawing.Size(288, 167);
            this.NewGrabberForm_Fill_Panel.TabIndex = 169;
            // 
            // txtNumCamera
            // 
            this.txtNumCamera.Location = new System.Drawing.Point(108, 78);
            this.txtNumCamera.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.txtNumCamera.Name = "txtNumCamera";
            this.txtNumCamera.Size = new System.Drawing.Size(153, 24);
            this.txtNumCamera.TabIndex = 177;
            this.txtNumCamera.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumCamera.ValueChanged += new System.EventHandler(this.TxtNumCamera_ValueChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(108, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(153, 24);
            this.txtName.TabIndex = 176;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(8, 18);
            this.labelName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 18);
            this.labelName.TabIndex = 175;
            this.labelName.Text = "Name";
            // 
            // labelNumCamera
            // 
            this.labelNumCamera.AutoSize = true;
            this.labelNumCamera.Location = new System.Drawing.Point(7, 81);
            this.labelNumCamera.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelNumCamera.Name = "labelNumCamera";
            this.labelNumCamera.Size = new System.Drawing.Size(97, 18);
            this.labelNumCamera.TabIndex = 158;
            this.labelNumCamera.Text = "Num Camera";
            // 
            // NewGrabberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 199);
            this.Controls.Add(this.NewGrabberForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewGrabberForm";
            this.Text = "New Grabber";
            this.Load += new System.EventHandler(this.GrabberInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.NewGrabberForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.NewGrabberForm_Fill_Panel.ClientArea.PerformLayout();
            this.NewGrabberForm_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNumCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbGrabberType;
        private System.Windows.Forms.Label labelGrabberType;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel NewGrabberForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.NumericUpDown txtNumCamera;
        private System.Windows.Forms.Label labelNumCamera;
    }
}