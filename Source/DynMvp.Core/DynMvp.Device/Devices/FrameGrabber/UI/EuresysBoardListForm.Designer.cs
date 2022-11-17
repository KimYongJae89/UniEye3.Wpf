namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class EuresysBoardListForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.EuresysBoardListForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.columnBoardType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnBoardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnConnectorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCameraType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnSurfaceNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPageLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEdgeStarPos = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnROIStartPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnROIWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNativeBuffering = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.EuresysBoardListForm_Fill_Panel.ClientArea.SuspendLayout();
            this.EuresysBoardListForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnBoardType,
            this.columnBoardId,
            this.columnConnectorId,
            this.columnCameraType,
            this.ColumnSurfaceNum,
            this.ColumnPageLength,
            this.ColumnEdgeStarPos,
            this.ColumnROIStartPos,
            this.ColumnROIWidth,
            this.columnNativeBuffering});
            this.cameraInfoGrid.Location = new System.Drawing.Point(5, 3);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.Size = new System.Drawing.Size(1189, 218);
            this.cameraInfoGrid.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(1087, 231);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(979, 231);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.Location = new System.Drawing.Point(73, 229);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(91, 33);
            this.buttonMoveDown.TabIndex = 161;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveUp.Location = new System.Drawing.Point(4, 229);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 33);
            this.buttonMoveUp.TabIndex = 162;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1200, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 299);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1200, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 268);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1199, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 268);
            // 
            // EuresysBoardListForm_Fill_Panel
            // 
            // 
            // EuresysBoardListForm_Fill_Panel.ClientArea
            // 
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveDown);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonMoveUp);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.cameraInfoGrid);
            this.EuresysBoardListForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.EuresysBoardListForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EuresysBoardListForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.EuresysBoardListForm_Fill_Panel.Name = "EuresysBoardListForm_Fill_Panel";
            this.EuresysBoardListForm_Fill_Panel.Size = new System.Drawing.Size(1198, 268);
            this.EuresysBoardListForm_Fill_Panel.TabIndex = 171;
            // 
            // columnBoardType
            // 
            this.columnBoardType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnBoardType.HeaderText = "Board Type";
            this.columnBoardType.Items.AddRange(new object[] {
            "GrabLink_Value",
            "GrabLink_Base",
            "GrabLink_DualBase",
            "GrabLink_Full",
            "Picolo"});
            this.columnBoardType.Name = "columnBoardType";
            // 
            // columnBoardId
            // 
            this.columnBoardId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnBoardId.HeaderText = "Board ID";
            this.columnBoardId.Name = "columnBoardId";
            this.columnBoardId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnBoardId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnConnectorId
            // 
            this.columnConnectorId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnConnectorId.HeaderText = "Connector ID";
            this.columnConnectorId.Name = "columnConnectorId";
            this.columnConnectorId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnConnectorId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnCameraType
            // 
            this.columnCameraType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            "ELIIXAp_16k"});
            this.columnCameraType.Name = "columnCameraType";
            // 
            // ColumnSurfaceNum
            // 
            this.ColumnSurfaceNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnSurfaceNum.HeaderText = "Surface Num";
            this.ColumnSurfaceNum.Name = "ColumnSurfaceNum";
            // 
            // ColumnPageLength
            // 
            this.ColumnPageLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPageLength.HeaderText = "Page Length";
            this.ColumnPageLength.Name = "ColumnPageLength";
            // 
            // ColumnEdgeStarPos
            // 
            this.ColumnEdgeStarPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnEdgeStarPos.HeaderText = "Edge Star Pos";
            this.ColumnEdgeStarPos.Items.AddRange(new object[] {
            "Middle",
            "Left",
            "Right"});
            this.ColumnEdgeStarPos.Name = "ColumnEdgeStarPos";
            // 
            // ColumnROIStartPos
            // 
            this.ColumnROIStartPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnROIStartPos.HeaderText = "ROI Start Position";
            this.ColumnROIStartPos.Name = "ColumnROIStartPos";
            // 
            // ColumnROIWidth
            // 
            this.ColumnROIWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnROIWidth.HeaderText = "ROI Width";
            this.ColumnROIWidth.Name = "ColumnROIWidth";
            // 
            // columnNativeBuffering
            // 
            this.columnNativeBuffering.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnNativeBuffering.HeaderText = "Native Buffering";
            this.columnNativeBuffering.Name = "columnNativeBuffering";
            // 
            // EuresysBoardListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 300);
            this.Controls.Add(this.EuresysBoardListForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EuresysBoardListForm";
            this.Text = "Euresys Board List";
            this.Load += new System.EventHandler(this.EuresysBoardListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.EuresysBoardListForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.EuresysBoardListForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel EuresysBoardListForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnBoardType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBoardId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnConnectorId;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnCameraType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSurfaceNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPageLength;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnEdgeStarPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnROIStartPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnROIWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnNativeBuffering;
    }
}