namespace DynMvp.Data.UI
{
    partial class OutputFormatForm
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
            this.gridViewValueData = new System.Windows.Forms.DataGridView();
            this.ColumnProbeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxStart = new System.Windows.Forms.TextBox();
            this.textBoxEnd = new System.Windows.Forms.TextBox();
            this.textBoxSeparator = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.startTypeAscii = new System.Windows.Forms.RadioButton();
            this.groupBoxStart = new System.Windows.Forms.GroupBox();
            this.startTypeHex = new System.Windows.Forms.RadioButton();
            this.groupBoxEnd = new System.Windows.Forms.GroupBox();
            this.endTypeHex = new System.Windows.Forms.RadioButton();
            this.endTypeAscii = new System.Windows.Forms.RadioButton();
            this.groupBoxSeparator = new System.Windows.Forms.GroupBox();
            this.separatorTypeHex = new System.Windows.Forms.RadioButton();
            this.separatorTypeAscii = new System.Windows.Forms.RadioButton();
            this.useChecksum = new System.Windows.Forms.CheckBox();
            this.checksumSize = new System.Windows.Forms.ComboBox();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.OutputFormatForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewValueData)).BeginInit();
            this.groupBoxStart.SuspendLayout();
            this.groupBoxEnd.SuspendLayout();
            this.groupBoxSeparator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.OutputFormatForm_Fill_Panel.ClientArea.SuspendLayout();
            this.OutputFormatForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridViewValueData
            // 
            this.gridViewValueData.AllowUserToAddRows = false;
            this.gridViewValueData.AllowUserToDeleteRows = false;
            this.gridViewValueData.AllowUserToResizeRows = false;
            this.gridViewValueData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewValueData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnProbeId,
            this.ColumnValueName});
            this.gridViewValueData.Location = new System.Drawing.Point(228, 44);
            this.gridViewValueData.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gridViewValueData.MultiSelect = false;
            this.gridViewValueData.Name = "gridViewValueData";
            this.gridViewValueData.ReadOnly = true;
            this.gridViewValueData.RowHeadersVisible = false;
            this.gridViewValueData.RowTemplate.Height = 30;
            this.gridViewValueData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewValueData.Size = new System.Drawing.Size(303, 264);
            this.gridViewValueData.TabIndex = 1;
            // 
            // ColumnProbeId
            // 
            this.ColumnProbeId.HeaderText = "Probe Id";
            this.ColumnProbeId.Name = "ColumnProbeId";
            this.ColumnProbeId.ReadOnly = true;
            this.ColumnProbeId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnProbeId.Width = 150;
            // 
            // ColumnValueName
            // 
            this.ColumnValueName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnValueName.HeaderText = "Value";
            this.ColumnValueName.Name = "ColumnValueName";
            this.ColumnValueName.ReadOnly = true;
            this.ColumnValueName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // textBoxStart
            // 
            this.textBoxStart.Location = new System.Drawing.Point(10, 54);
            this.textBoxStart.Name = "textBoxStart";
            this.textBoxStart.Size = new System.Drawing.Size(192, 27);
            this.textBoxStart.TabIndex = 3;
            // 
            // textBoxEnd
            // 
            this.textBoxEnd.Location = new System.Drawing.Point(10, 54);
            this.textBoxEnd.Name = "textBoxEnd";
            this.textBoxEnd.Size = new System.Drawing.Size(192, 27);
            this.textBoxEnd.TabIndex = 3;
            // 
            // textBoxSeparator
            // 
            this.textBoxSeparator.Location = new System.Drawing.Point(10, 54);
            this.textBoxSeparator.Name = "textBoxSeparator";
            this.textBoxSeparator.Size = new System.Drawing.Size(192, 27);
            this.textBoxSeparator.TabIndex = 3;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(228, 9);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 32);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(309, 9);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 32);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(255, 349);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(98, 38);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(155, 349);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(98, 38);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // startTypeAscii
            // 
            this.startTypeAscii.AutoSize = true;
            this.startTypeAscii.Location = new System.Drawing.Point(10, 24);
            this.startTypeAscii.Name = "startTypeAscii";
            this.startTypeAscii.Size = new System.Drawing.Size(63, 24);
            this.startTypeAscii.TabIndex = 7;
            this.startTypeAscii.TabStop = true;
            this.startTypeAscii.Text = "ASCII";
            this.startTypeAscii.UseVisualStyleBackColor = true;
            // 
            // groupBoxStart
            // 
            this.groupBoxStart.Controls.Add(this.startTypeHex);
            this.groupBoxStart.Controls.Add(this.startTypeAscii);
            this.groupBoxStart.Controls.Add(this.textBoxStart);
            this.groupBoxStart.Location = new System.Drawing.Point(8, 9);
            this.groupBoxStart.Name = "groupBoxStart";
            this.groupBoxStart.Size = new System.Drawing.Size(214, 96);
            this.groupBoxStart.TabIndex = 8;
            this.groupBoxStart.TabStop = false;
            this.groupBoxStart.Text = "Start";
            // 
            // startTypeHex
            // 
            this.startTypeHex.AutoSize = true;
            this.startTypeHex.Location = new System.Drawing.Point(95, 24);
            this.startTypeHex.Name = "startTypeHex";
            this.startTypeHex.Size = new System.Drawing.Size(55, 24);
            this.startTypeHex.TabIndex = 7;
            this.startTypeHex.TabStop = true;
            this.startTypeHex.Text = "HEX";
            this.startTypeHex.UseVisualStyleBackColor = true;
            // 
            // groupBoxEnd
            // 
            this.groupBoxEnd.Controls.Add(this.endTypeHex);
            this.groupBoxEnd.Controls.Add(this.endTypeAscii);
            this.groupBoxEnd.Controls.Add(this.textBoxEnd);
            this.groupBoxEnd.Location = new System.Drawing.Point(8, 111);
            this.groupBoxEnd.Name = "groupBoxEnd";
            this.groupBoxEnd.Size = new System.Drawing.Size(214, 96);
            this.groupBoxEnd.TabIndex = 8;
            this.groupBoxEnd.TabStop = false;
            this.groupBoxEnd.Text = "End";
            // 
            // endTypeHex
            // 
            this.endTypeHex.AutoSize = true;
            this.endTypeHex.Location = new System.Drawing.Point(95, 24);
            this.endTypeHex.Name = "endTypeHex";
            this.endTypeHex.Size = new System.Drawing.Size(55, 24);
            this.endTypeHex.TabIndex = 7;
            this.endTypeHex.TabStop = true;
            this.endTypeHex.Text = "HEX";
            this.endTypeHex.UseVisualStyleBackColor = true;
            // 
            // endTypeAscii
            // 
            this.endTypeAscii.AutoSize = true;
            this.endTypeAscii.Location = new System.Drawing.Point(10, 24);
            this.endTypeAscii.Name = "endTypeAscii";
            this.endTypeAscii.Size = new System.Drawing.Size(63, 24);
            this.endTypeAscii.TabIndex = 7;
            this.endTypeAscii.TabStop = true;
            this.endTypeAscii.Text = "ASCII";
            this.endTypeAscii.UseVisualStyleBackColor = true;
            // 
            // groupBoxSeparator
            // 
            this.groupBoxSeparator.Controls.Add(this.separatorTypeHex);
            this.groupBoxSeparator.Controls.Add(this.separatorTypeAscii);
            this.groupBoxSeparator.Controls.Add(this.textBoxSeparator);
            this.groupBoxSeparator.Location = new System.Drawing.Point(8, 213);
            this.groupBoxSeparator.Name = "groupBoxSeparator";
            this.groupBoxSeparator.Size = new System.Drawing.Size(214, 96);
            this.groupBoxSeparator.TabIndex = 8;
            this.groupBoxSeparator.TabStop = false;
            this.groupBoxSeparator.Text = "Separator";
            // 
            // separatorTypeHex
            // 
            this.separatorTypeHex.AutoSize = true;
            this.separatorTypeHex.Location = new System.Drawing.Point(95, 24);
            this.separatorTypeHex.Name = "separatorTypeHex";
            this.separatorTypeHex.Size = new System.Drawing.Size(55, 24);
            this.separatorTypeHex.TabIndex = 7;
            this.separatorTypeHex.TabStop = true;
            this.separatorTypeHex.Text = "HEX";
            this.separatorTypeHex.UseVisualStyleBackColor = true;
            // 
            // separatorTypeAscii
            // 
            this.separatorTypeAscii.AutoSize = true;
            this.separatorTypeAscii.Location = new System.Drawing.Point(10, 24);
            this.separatorTypeAscii.Name = "separatorTypeAscii";
            this.separatorTypeAscii.Size = new System.Drawing.Size(63, 24);
            this.separatorTypeAscii.TabIndex = 7;
            this.separatorTypeAscii.TabStop = true;
            this.separatorTypeAscii.Text = "ASCII";
            this.separatorTypeAscii.UseVisualStyleBackColor = true;
            // 
            // useChecksum
            // 
            this.useChecksum.AutoSize = true;
            this.useChecksum.Location = new System.Drawing.Point(12, 316);
            this.useChecksum.Name = "useChecksum";
            this.useChecksum.Size = new System.Drawing.Size(128, 24);
            this.useChecksum.TabIndex = 9;
            this.useChecksum.Text = "Use Checksum";
            this.useChecksum.UseVisualStyleBackColor = true;
            // 
            // checksumSize
            // 
            this.checksumSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.checksumSize.FormattingEnabled = true;
            this.checksumSize.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.checksumSize.Location = new System.Drawing.Point(145, 313);
            this.checksumSize.Name = "checksumSize";
            this.checksumSize.Size = new System.Drawing.Size(77, 28);
            this.checksumSize.TabIndex = 10;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(535, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 422);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(535, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 391);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(534, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 391);
            // 
            // OutputFormatForm_Fill_Panel
            // 
            // 
            // OutputFormatForm_Fill_Panel.ClientArea
            // 
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.checksumSize);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.useChecksum);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxSeparator);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxEnd);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxStart);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOk);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.buttonDelete);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.buttonAdd);
            this.OutputFormatForm_Fill_Panel.ClientArea.Controls.Add(this.gridViewValueData);
            this.OutputFormatForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.OutputFormatForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputFormatForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.OutputFormatForm_Fill_Panel.Name = "OutputFormatForm_Fill_Panel";
            this.OutputFormatForm_Fill_Panel.Size = new System.Drawing.Size(533, 391);
            this.OutputFormatForm_Fill_Panel.TabIndex = 19;
            // 
            // OutputFormatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 423);
            this.Controls.Add(this.OutputFormatForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OutputFormatForm";
            this.Text = "Output Format";
            this.Load += new System.EventHandler(this.OutputFormatForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewValueData)).EndInit();
            this.groupBoxStart.ResumeLayout(false);
            this.groupBoxStart.PerformLayout();
            this.groupBoxEnd.ResumeLayout(false);
            this.groupBoxEnd.PerformLayout();
            this.groupBoxSeparator.ResumeLayout(false);
            this.groupBoxSeparator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.OutputFormatForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.OutputFormatForm_Fill_Panel.ClientArea.PerformLayout();
            this.OutputFormatForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridViewValueData;
        private System.Windows.Forms.TextBox textBoxStart;
        private System.Windows.Forms.TextBox textBoxEnd;
        private System.Windows.Forms.TextBox textBoxSeparator;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProbeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValueName;
        private System.Windows.Forms.RadioButton startTypeAscii;
        private System.Windows.Forms.GroupBox groupBoxStart;
        private System.Windows.Forms.RadioButton startTypeHex;
        private System.Windows.Forms.GroupBox groupBoxEnd;
        private System.Windows.Forms.RadioButton endTypeHex;
        private System.Windows.Forms.RadioButton endTypeAscii;
        private System.Windows.Forms.GroupBox groupBoxSeparator;
        private System.Windows.Forms.RadioButton separatorTypeHex;
        private System.Windows.Forms.RadioButton separatorTypeAscii;
        private System.Windows.Forms.CheckBox useChecksum;
        private System.Windows.Forms.ComboBox checksumSize;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel OutputFormatForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}