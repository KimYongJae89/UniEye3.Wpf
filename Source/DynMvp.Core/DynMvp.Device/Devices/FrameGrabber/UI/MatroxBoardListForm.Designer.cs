namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class MatroxBoardListForm
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
            this.cameraInfoGrid = new System.Windows.Forms.DataGridView();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.MatroxBoardListForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.columnSystemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnSystemNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDigitizerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCameraType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnDcfPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.MatroxBoardListForm_Fill_Panel.ClientArea.SuspendLayout();
            this.MatroxBoardListForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.AllowUserToAddRows = false;
            this.cameraInfoGrid.AllowUserToDeleteRows = false;
            this.cameraInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSystemType,
            this.columnSystemNo,
            this.columnDigitizerNo,
            this.columnCameraType,
            this.columnDcfPath,
            this.columnHeight});
            this.cameraInfoGrid.Location = new System.Drawing.Point(5, 3);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.Size = new System.Drawing.Size(883, 190);
            this.cameraInfoGrid.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(675, 196);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(783, 196);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.Location = new System.Drawing.Point(73, 196);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(91, 33);
            this.buttonMoveDown.TabIndex = 163;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveUp.Location = new System.Drawing.Point(4, 196);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 33);
            this.buttonMoveUp.TabIndex = 164;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(894, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 263);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(894, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 232);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(893, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 232);
            // 
            // MatroxBoardListForm_Fill_Panel
            // 
            // 
            // MatroxBoardListForm_Fill_Panel.ClientArea
            // 
            this.MatroxBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveDown);
            this.MatroxBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveUp);
            this.MatroxBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.MatroxBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.MatroxBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.cameraInfoGrid);
            this.MatroxBoardListForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MatroxBoardListForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MatroxBoardListForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.MatroxBoardListForm_Fill_Panel.Name = "MatroxBoardListForm_Fill_Panel";
            this.MatroxBoardListForm_Fill_Panel.Size = new System.Drawing.Size(892, 232);
            this.MatroxBoardListForm_Fill_Panel.TabIndex = 173;
            // 
            // columnSystemType
            // 
            this.columnSystemType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSystemType.HeaderText = "System Type";
            this.columnSystemType.Items.AddRange(new object[] {
            "Solios", "Rapixo"});
            this.columnSystemType.Name = "columnSystemType";
            // 
            // columnSystemNo
            // 
            this.columnSystemNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSystemNo.HeaderText = "System No.";
            this.columnSystemNo.Name = "columnSystemNo";
            this.columnSystemNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnSystemNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnSystemNo.Width = 92;
            // 
            // columnDigitizerNo
            // 
            this.columnDigitizerNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDigitizerNo.HeaderText = "Digitizer No.";
            this.columnDigitizerNo.Name = "columnDigitizerNo";
            this.columnDigitizerNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnDigitizerNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnDigitizerNo.Width = 95;
            // 
            // columnCameraType
            // 
            this.columnCameraType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCameraType.HeaderText = "Camera Type";
            this.columnCameraType.Items.AddRange(new object[] {
            "Jai_GO_5000",
            "PrimeTech_PXCB120VTH",
            "PrimeTech_PXCB16QWTPM",
            "PrimeTech_PXCB16QWTPMCOMPACT",
            "HV_B550CTRG1",
            "HV_B550CTRG2",
            "RaL12288_66km",
            "RaL6144_80km",
            "RaL4096_80km",
            "VH_5MC_C16_2Tap",
            "ELIIXAp_8k",
            "ELIIXAp_16k",
            "VT_6K3_5X_H160"});
            this.columnCameraType.Name = "columnCameraType";
            this.columnCameraType.Width = 103;
            // 
            // columnDcfPath
            // 
            this.columnDcfPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDcfPath.HeaderText = "DCF Path";
            this.columnDcfPath.Name = "columnDcfPath";
            // 
            // columnHeight
            // 
            this.columnHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            // 
            // MatroxBoardListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 264);
            this.Controls.Add(this.MatroxBoardListForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MatroxBoardListForm";
            this.Text = "Matrox Board List";
            this.Load += new System.EventHandler(this.MatroxBoardListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.MatroxBoardListForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MatroxBoardListForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel MatroxBoardListForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnSystemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSystemNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDigitizerNo;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnCameraType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDcfPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
    }
}