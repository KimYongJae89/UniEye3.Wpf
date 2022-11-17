namespace DynMvp.Devices.MotionController.UI
{
    partial class AxisConfigurationForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.axisList = new System.Windows.Forms.DataGridView();
            this.columnAxisName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnMotionName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnAxisNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOriginOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.jogPlusButton = new System.Windows.Forms.Button();
            this.jogMinusButton = new System.Windows.Forms.Button();
            this.axisHandlerGridView = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager();
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.AxisConfigurationForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.survoButton = new System.Windows.Forms.Button();
            this.position = new System.Windows.Forms.Label();
            this.setOriginOffsetButton = new System.Windows.Forms.Button();
            this.homeMoveButton = new System.Windows.Forms.Button();
            this.positionUpdateTimer = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.axisList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisHandlerGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.AxisConfigurationForm_Fill_Panel.ClientArea.SuspendLayout();
            this.AxisConfigurationForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // axisList
            // 
            this.axisList.AllowUserToResizeColumns = false;
            this.axisList.AllowUserToResizeRows = false;
            this.axisList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axisList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.axisList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnAxisName,
            this.columnMotionName,
            this.columnAxisNo,
            this.columnOriginOffset});
            this.axisList.Location = new System.Drawing.Point(215, 43);
            this.axisList.MultiSelect = false;
            this.axisList.Name = "axisList";
            this.axisList.RowTemplate.Height = 23;
            this.axisList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.axisList.Size = new System.Drawing.Size(477, 162);
            this.axisList.TabIndex = 1;
            // 
            // columnAxisName
            // 
            this.columnAxisName.HeaderText = "Axis Name";
            this.columnAxisName.Name = "columnAxisName";
            this.columnAxisName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // columnMotionName
            // 
            this.columnMotionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnMotionName.HeaderText = "Motion Name";
            this.columnMotionName.Items.AddRange(new object[] {
            "GrabLink_Value",
            "GrabLink_Base",
            "GrabLink_DualBase",
            "Picolo"});
            this.columnMotionName.Name = "columnMotionName";
            // 
            // columnAxisNo
            // 
            this.columnAxisNo.HeaderText = "Axis No";
            this.columnAxisNo.Name = "columnAxisNo";
            this.columnAxisNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnAxisNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnAxisNo.Width = 80;
            // 
            // columnOriginOffset
            // 
            this.columnOriginOffset.HeaderText = "Origin Offset";
            this.columnOriginOffset.Name = "columnOriginOffset";
            this.columnOriginOffset.Width = 150;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(351, 208);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 37);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(238, 208);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 37);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Image = global::DynMvp.Devices.Properties.Resources.arrow_up;
            this.moveUpButton.Location = new System.Drawing.Point(170, 73);
            this.moveUpButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(38, 38);
            this.moveUpButton.TabIndex = 163;
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Image = global::DynMvp.Devices.Properties.Resources.arrow_down;
            this.moveDownButton.Location = new System.Drawing.Point(170, 143);
            this.moveDownButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(38, 38);
            this.moveDownButton.TabIndex = 162;
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // jogPlusButton
            // 
            this.jogPlusButton.Location = new System.Drawing.Point(359, 3);
            this.jogPlusButton.Margin = new System.Windows.Forms.Padding(4);
            this.jogPlusButton.Name = "jogPlusButton";
            this.jogPlusButton.Size = new System.Drawing.Size(56, 38);
            this.jogPlusButton.TabIndex = 163;
            this.jogPlusButton.Text = "Jog+";
            this.jogPlusButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.jogPlusButton.UseVisualStyleBackColor = true;
            this.jogPlusButton.Click += new System.EventHandler(this.jogPlusButton_Click);
            // 
            // jogMinusButton
            // 
            this.jogMinusButton.Location = new System.Drawing.Point(215, 3);
            this.jogMinusButton.Margin = new System.Windows.Forms.Padding(4);
            this.jogMinusButton.Name = "jogMinusButton";
            this.jogMinusButton.Size = new System.Drawing.Size(56, 38);
            this.jogMinusButton.TabIndex = 162;
            this.jogMinusButton.Text = "Jog-";
            this.jogMinusButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.jogMinusButton.UseVisualStyleBackColor = true;
            this.jogMinusButton.Click += new System.EventHandler(this.jogMinusButton_Click);
            // 
            // axisHandlerList
            // 
            this.axisHandlerGridView.AllowUserToAddRows = false;
            this.axisHandlerGridView.AllowUserToDeleteRows = false;
            this.axisHandlerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.axisHandlerGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName});
            this.axisHandlerGridView.Location = new System.Drawing.Point(2, 4);
            this.axisHandlerGridView.Name = "axisHandlerList";
            this.axisHandlerGridView.ReadOnly = true;
            this.axisHandlerGridView.RowHeadersVisible = false;
            this.axisHandlerGridView.RowTemplate.Height = 23;
            this.axisHandlerGridView.Size = new System.Drawing.Size(143, 201);
            this.axisHandlerGridView.TabIndex = 164;
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(697, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 280);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(697, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 249);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(696, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 249);
            // 
            // AxisConfigurationForm_Fill_Panel
            // 
            // 
            // AxisConfigurationForm_Fill_Panel.ClientArea
            // 
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.survoButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.position);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.axisHandlerGridView);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.setOriginOffsetButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.jogMinusButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.jogPlusButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.homeMoveButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.moveDownButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.moveUpButton);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.AxisConfigurationForm_Fill_Panel.ClientArea.Controls.Add(this.axisList);
            this.AxisConfigurationForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AxisConfigurationForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AxisConfigurationForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.AxisConfigurationForm_Fill_Panel.Name = "AxisConfigurationForm_Fill_Panel";
            this.AxisConfigurationForm_Fill_Panel.Size = new System.Drawing.Size(695, 249);
            this.AxisConfigurationForm_Fill_Panel.TabIndex = 173;
            // 
            // survoButton
            // 
            this.survoButton.Location = new System.Drawing.Point(423, 3);
            this.survoButton.Margin = new System.Windows.Forms.Padding(4);
            this.survoButton.Name = "survoButton";
            this.survoButton.Size = new System.Drawing.Size(82, 38);
            this.survoButton.TabIndex = 166;
            this.survoButton.Text = "Survo ON";
            this.survoButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.survoButton.UseVisualStyleBackColor = true;
            this.survoButton.Click += new System.EventHandler(this.survoButton_Click);
            // 
            // position
            // 
            this.position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.position.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.position.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.position.Location = new System.Drawing.Point(584, 4);
            this.position.Name = "position";
            this.position.Size = new System.Drawing.Size(104, 37);
            this.position.TabIndex = 165;
            this.position.Text = "0";
            this.position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // setOriginOffsetButton
            // 
            this.setOriginOffsetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setOriginOffsetButton.Location = new System.Drawing.Point(526, 3);
            this.setOriginOffsetButton.Margin = new System.Windows.Forms.Padding(4);
            this.setOriginOffsetButton.Name = "setOriginOffsetButton";
            this.setOriginOffsetButton.Size = new System.Drawing.Size(56, 38);
            this.setOriginOffsetButton.TabIndex = 162;
            this.setOriginOffsetButton.Text = "Set";
            this.setOriginOffsetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.setOriginOffsetButton.UseVisualStyleBackColor = true;
            this.setOriginOffsetButton.Click += new System.EventHandler(this.setOriginOffsetButton_Click);
            // 
            // homeMoveButton
            // 
            this.homeMoveButton.Location = new System.Drawing.Point(279, 3);
            this.homeMoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.homeMoveButton.Name = "homeMoveButton";
            this.homeMoveButton.Size = new System.Drawing.Size(72, 38);
            this.homeMoveButton.TabIndex = 162;
            this.homeMoveButton.Text = "Home";
            this.homeMoveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.homeMoveButton.UseVisualStyleBackColor = true;
            this.homeMoveButton.Click += new System.EventHandler(this.homeMoveButton_Click);
            // 
            // positionUpdateTimer
            // 
            this.positionUpdateTimer.Tick += new System.EventHandler(this.positionUpdateTimer_Tick);
            // 
            // AxisConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 281);
            this.Controls.Add(this.AxisConfigurationForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AxisConfigurationForm";
            this.Text = "Setup Axis Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.axisList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisHandlerGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.AxisConfigurationForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AxisConfigurationForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView axisList;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button jogPlusButton;
        private System.Windows.Forms.Button jogMinusButton;
        private System.Windows.Forms.DataGridView axisHandlerGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel AxisConfigurationForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnAxisName;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnMotionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAxisNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOriginOffset;
        private System.Windows.Forms.Button homeMoveButton;
        private System.Windows.Forms.Label position;
        private System.Windows.Forms.Button setOriginOffsetButton;
        private System.Windows.Forms.Timer positionUpdateTimer;
        private System.Windows.Forms.Button survoButton;
    }
}