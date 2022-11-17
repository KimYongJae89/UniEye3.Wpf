namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class SaperaCameraListForm
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
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.EuresysBoardListForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.columnGain = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnScanLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFrameNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offsetX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClientType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnDirectionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnUseMilBuffer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.binningVertical = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnVMPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.EuresysBoardListForm_Fill_Panel.ClientArea.SuspendLayout();
            this.EuresysBoardListForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.AllowUserToAddRows = false;
            this.cameraInfoGrid.AllowUserToDeleteRows = false;
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnGain,
            this.columnIndex,
            this.columnWidth,
            this.columnHeight,
            this.columnScanLength,
            this.columnFrameNum,
            this.offsetX,
            this.columnClientType,
            this.columnDirectionType,
            this.columnUseMilBuffer,
            this.binningVertical,
            this.columnVMPath});
            this.cameraInfoGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraInfoGrid.Location = new System.Drawing.Point(0, 0);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowHeadersVisible = false;
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.Size = new System.Drawing.Size(1129, 197);
            this.cameraInfoGrid.TabIndex = 0;
            this.cameraInfoGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cameraInfoGrid_CellContentClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(1018, 218);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(903, 218);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 5;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1131, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 286);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1131, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 255);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1130, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 255);
            // 
            // EuresysBoardListForm_Fill_Panel
            // 
            // 
            // EuresysBoardListForm_Fill_Panel.ClientArea
            // 
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.cameraInfoGrid);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.EuresysBoardListForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.EuresysBoardListForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.EuresysBoardListForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EuresysBoardListForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.EuresysBoardListForm_Fill_Panel.Name = "EuresysBoardListForm_Fill_Panel";
            this.EuresysBoardListForm_Fill_Panel.Size = new System.Drawing.Size(1129, 255);
            this.EuresysBoardListForm_Fill_Panel.TabIndex = 171;
            // 
            // columnGain
            // 
            this.columnGain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnGain.HeaderText = "Edit";
            this.columnGain.Name = "columnGain";
            this.columnGain.ReadOnly = true;
            this.columnGain.Width = 60;
            // 
            // columnIndex
            // 
            this.columnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 60;
            // 
            // columnWidth
            // 
            this.columnWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnWidth.HeaderText = "Width";
            this.columnWidth.Name = "columnWidth";
            this.columnWidth.Width = 71;
            // 
            // columnHeight
            // 
            this.columnHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            this.columnHeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnHeight.Width = 75;
            // 
            // columnScanLength
            // 
            this.columnScanLength.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnScanLength.HeaderText = "Scan Length";
            this.columnScanLength.Name = "columnScanLength";
            this.columnScanLength.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnScanLength.Width = 115;
            // 
            // columnFrameNum
            // 
            this.columnFrameNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnFrameNum.HeaderText = "Frame Num";
            this.columnFrameNum.Name = "columnFrameNum";
            this.columnFrameNum.Width = 112;
            // 
            // offsetX
            // 
            this.offsetX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.offsetX.HeaderText = "Offset X";
            this.offsetX.Name = "offsetX";
            this.offsetX.Width = 87;
            // 
            // columnClientType
            // 
            this.columnClientType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnClientType.HeaderText = "Client Type";
            this.columnClientType.Name = "columnClientType";
            this.columnClientType.Width = 87;
            // 
            // columnDirectionType
            // 
            this.columnDirectionType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDirectionType.HeaderText = "Direction";
            this.columnDirectionType.Name = "columnDirectionType";
            this.columnDirectionType.Width = 73;
            // 
            // columnUseMilBuffer
            // 
            this.columnUseMilBuffer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnUseMilBuffer.HeaderText = "Use Mil Buffer";
            this.columnUseMilBuffer.Name = "columnUseMilBuffer";
            this.columnUseMilBuffer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnUseMilBuffer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnUseMilBuffer.Width = 126;
            // 
            // binningVertical
            // 
            this.binningVertical.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.binningVertical.HeaderText = "Binning Vertical";
            this.binningVertical.Name = "binningVertical";
            this.binningVertical.Width = 103;
            // 
            // columnVMPath
            // 
            this.columnVMPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnVMPath.HeaderText = "VirtualPath";
            this.columnVMPath.Name = "columnVMPath";
            // 
            // GenTLCameraListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 287);
            this.Controls.Add(this.EuresysBoardListForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GenTLCameraListForm";
            this.Text = "Euresys Board List";
            this.Load += new System.EventHandler(this.GenTLCameraListForm_Load);
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
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel EuresysBoardListForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.DataGridViewButtonColumn columnGain;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnScanLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFrameNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn offsetX;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnClientType;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnDirectionType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnUseMilBuffer;
        private System.Windows.Forms.DataGridViewCheckBoxColumn binningVertical;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnVMPath;
    }
}