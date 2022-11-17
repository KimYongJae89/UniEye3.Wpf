namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class PylonCameraListForm
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
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeviceUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIpAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNumBand = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnRotateFlipType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.autoDetectButton = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.PylonCameraListForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.PylonCameraListForm_Fill_Panel.ClientArea.SuspendLayout();
            this.PylonCameraListForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.AllowUserToAddRows = false;
            this.cameraInfoGrid.AllowUserToDeleteRows = false;
            this.cameraInfoGrid.AllowUserToResizeColumns = false;
            this.cameraInfoGrid.AllowUserToResizeRows = false;
            this.cameraInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.ColumnDeviceUserId,
            this.ColumnIpAddress,
            this.ColumnSerialNo,
            this.columnModelName,
            this.ColumnWidth,
            this.ColumnHeight,
            this.ColumnNumBand,
            this.ColumnRotateFlipType});
            this.cameraInfoGrid.Location = new System.Drawing.Point(4, 42);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.MultiSelect = false;
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowHeadersVisible = false;
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cameraInfoGrid.Size = new System.Drawing.Size(1151, 190);
            this.cameraInfoGrid.TabIndex = 1;
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnNo.Width = 50;
            // 
            // ColumnDeviceUserId
            // 
            this.ColumnDeviceUserId.HeaderText = "Device User Id";
            this.ColumnDeviceUserId.Name = "ColumnDeviceUserId";
            this.ColumnDeviceUserId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnDeviceUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDeviceUserId.Width = 133;
            // 
            // ColumnIpAddress
            // 
            this.ColumnIpAddress.HeaderText = "IP Address";
            this.ColumnIpAddress.Name = "ColumnIpAddress";
            this.ColumnIpAddress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnIpAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnIpAddress.Width = 120;
            // 
            // ColumnSerialNo
            // 
            this.ColumnSerialNo.HeaderText = "Serial Number";
            this.ColumnSerialNo.Name = "ColumnSerialNo";
            this.ColumnSerialNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnSerialNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnSerialNo.Width = 132;
            // 
            // columnModelName
            // 
            this.columnModelName.HeaderText = "Model Name";
            this.columnModelName.Name = "columnModelName";
            this.columnModelName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnModelName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnModelName.Width = 133;
            // 
            // ColumnWidth
            // 
            this.ColumnWidth.FillWeight = 50F;
            this.ColumnWidth.HeaderText = "Width";
            this.ColumnWidth.Name = "ColumnWidth";
            this.ColumnWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnWidth.Width = 60;
            // 
            // ColumnHeight
            // 
            this.ColumnHeight.FillWeight = 50F;
            this.ColumnHeight.HeaderText = "Height";
            this.ColumnHeight.Name = "ColumnHeight";
            this.ColumnHeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnHeight.Width = 60;
            // 
            // ColumnNumBand
            // 
            this.ColumnNumBand.FillWeight = 60F;
            this.ColumnNumBand.HeaderText = "Band";
            this.ColumnNumBand.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.ColumnNumBand.Name = "ColumnNumBand";
            this.ColumnNumBand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnNumBand.Width = 60;
            // 
            // ColumnRotateFlipType
            // 
            this.ColumnRotateFlipType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnRotateFlipType.FillWeight = 150F;
            this.ColumnRotateFlipType.HeaderText = "Rotet Flip Type";
            this.ColumnRotateFlipType.Items.AddRange(new object[] {
            "RotateNoneFlipNone",
            "Rotate180FlipXY",
            "Rotate90FlipNone",
            "Rotate270FlipXY",
            "Rotate180FlipNone",
            "RotateNoneFlipXY",
            "Rotate270FlipNone",
            "Rotate90FlipXY",
            "RotateNoneFlipX",
            "Rotate180FlipY",
            "Rotate90FlipX",
            "Rotate270FlipY",
            "Rotate180FlipX",
            "RotateNoneFlipY",
            "Rotate270FlipX",
            "Rotate90FlipY"});
            this.ColumnRotateFlipType.MaxDropDownItems = 16;
            this.ColumnRotateFlipType.Name = "ColumnRotateFlipType";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(1048, 484);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(940, 484);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // autoDetectButton
            // 
            this.autoDetectButton.Location = new System.Drawing.Point(4, 3);
            this.autoDetectButton.Margin = new System.Windows.Forms.Padding(4);
            this.autoDetectButton.Name = "autoDetectButton";
            this.autoDetectButton.Size = new System.Drawing.Size(107, 33);
            this.autoDetectButton.TabIndex = 3;
            this.autoDetectButton.Text = "Auto Detect";
            this.autoDetectButton.UseVisualStyleBackColor = true;
            this.autoDetectButton.Click += new System.EventHandler(this.autoDetectButton_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.Location = new System.Drawing.Point(73, 484);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(91, 33);
            this.buttonMoveDown.TabIndex = 165;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveUp.Location = new System.Drawing.Point(4, 484);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 33);
            this.buttonMoveUp.TabIndex = 166;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1160, 58);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 577);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1160, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 58);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 519);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1159, 58);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 519);
            // 
            // PylonCameraListForm_Fill_Panel
            // 
            // 
            // PylonCameraListForm_Fill_Panel.ClientArea
            // 
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveDown);
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveUp);
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.autoDetectButton);
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.PylonCameraListForm_Fill_Panel.ClientArea.Controls.Add(this.cameraInfoGrid);
            this.PylonCameraListForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PylonCameraListForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PylonCameraListForm_Fill_Panel.Location = new System.Drawing.Point(1, 58);
            this.PylonCameraListForm_Fill_Panel.Name = "PylonCameraListForm_Fill_Panel";
            this.PylonCameraListForm_Fill_Panel.Size = new System.Drawing.Size(1158, 519);
            this.PylonCameraListForm_Fill_Panel.TabIndex = 175;
            // 
            // PylonCameraListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 578);
            this.Controls.Add(this.PylonCameraListForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PylonCameraListForm";
            this.Text = "Pylon Camera List";
            this.Load += new System.EventHandler(this.PylonCameraListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.PylonCameraListForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.PylonCameraListForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button autoDetectButton;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel PylonCameraListForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeviceUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIpAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHeight;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnNumBand;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnRotateFlipType;
    }
}