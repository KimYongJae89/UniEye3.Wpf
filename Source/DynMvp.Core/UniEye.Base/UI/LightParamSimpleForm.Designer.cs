namespace UniEye.Base.UI
{
    partial class LightParamSimpleForm
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.lightValueGrid = new System.Windows.Forms.DataGridView();
            this.columnLightName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnValue = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.labelExposure = new System.Windows.Forms.Label();
            this.exposureTimeMs = new System.Windows.Forms.NumericUpDown();
            this.labelExposureMs = new System.Windows.Forms.Label();
            this.comboLightParamSource = new System.Windows.Forms.ComboBox();
            this.lightTypeCombo = new System.Windows.Forms.ComboBox();
            this.applyLightButton = new System.Windows.Forms.Button();
            this.applyAllLightButton = new System.Windows.Forms.Button();
            this.labelLightType = new System.Windows.Forms.Label();
            this.labelLightSource = new System.Windows.Forms.Label();
            this.editTypeNameButton = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.LightParamForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lightValueGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureTimeMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.LightParamForm_Fill_Panel.ClientArea.SuspendLayout();
            this.LightParamForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lightValueGrid
            // 
            this.lightValueGrid.AllowUserToAddRows = false;
            this.lightValueGrid.AllowUserToDeleteRows = false;
            this.lightValueGrid.AllowUserToResizeRows = false;
            this.lightValueGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lightValueGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLightName,
            this.columnValue});
            this.lightValueGrid.Location = new System.Drawing.Point(8, 74);
            this.lightValueGrid.MultiSelect = false;
            this.lightValueGrid.Name = "lightValueGrid";
            this.lightValueGrid.RowHeadersVisible = false;
            this.lightValueGrid.RowTemplate.Height = 23;
            this.lightValueGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lightValueGrid.Size = new System.Drawing.Size(264, 135);
            this.lightValueGrid.TabIndex = 158;
            // 
            // columnLightName
            // 
            this.columnLightName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnLightName.HeaderText = "Name";
            this.columnLightName.Name = "columnLightName";
            this.columnLightName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // columnValue
            // 
            this.columnValue.DefaultNewRowValue = 0;
            this.columnValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnValue.HeaderText = "Value";
            this.columnValue.MaskInput = "nnnn";
            this.columnValue.Name = "columnValue";
            this.columnValue.PadChar = '\0';
            this.columnValue.PromptChar = ' ';
            this.columnValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnValue.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.Location = new System.Drawing.Point(13, 221);
            this.labelExposure.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(103, 20);
            this.labelExposure.TabIndex = 151;
            this.labelExposure.Text = "ExposureTime";
            // 
            // exposureTimeMs
            // 
            this.exposureTimeMs.DecimalPlaces = 2;
            this.exposureTimeMs.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.exposureTimeMs.Location = new System.Drawing.Point(158, 219);
            this.exposureTimeMs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.exposureTimeMs.Name = "exposureTimeMs";
            this.exposureTimeMs.Size = new System.Drawing.Size(81, 27);
            this.exposureTimeMs.TabIndex = 155;
            this.exposureTimeMs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelExposureMs
            // 
            this.labelExposureMs.AutoSize = true;
            this.labelExposureMs.Location = new System.Drawing.Point(240, 221);
            this.labelExposureMs.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelExposureMs.Name = "labelExposureMs";
            this.labelExposureMs.Size = new System.Drawing.Size(28, 20);
            this.labelExposureMs.TabIndex = 150;
            this.labelExposureMs.Text = "ms";
            // 
            // comboLightParamSource
            // 
            this.comboLightParamSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLightParamSource.FormattingEnabled = true;
            this.comboLightParamSource.Items.AddRange(new object[] {
            "Model",
            "InspectionStep",
            "TargetGroup"});
            this.comboLightParamSource.Location = new System.Drawing.Point(110, 8);
            this.comboLightParamSource.Name = "comboLightParamSource";
            this.comboLightParamSource.Size = new System.Drawing.Size(161, 28);
            this.comboLightParamSource.TabIndex = 166;
            this.comboLightParamSource.SelectedIndexChanged += new System.EventHandler(this.comboLightParamSource_SelectedIndexChanged);
            // 
            // lightTypeCombo
            // 
            this.lightTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lightTypeCombo.FormattingEnabled = true;
            this.lightTypeCombo.Location = new System.Drawing.Point(110, 39);
            this.lightTypeCombo.Name = "lightTypeCombo";
            this.lightTypeCombo.Size = new System.Drawing.Size(128, 28);
            this.lightTypeCombo.TabIndex = 164;
            this.lightTypeCombo.SelectedIndexChanged += new System.EventHandler(this.lightTypeCombo_SelectedIndexChanged);
            // 
            // applyLightButton
            // 
            this.applyLightButton.Location = new System.Drawing.Point(11, 254);
            this.applyLightButton.Name = "applyLightButton";
            this.applyLightButton.Size = new System.Drawing.Size(80, 49);
            this.applyLightButton.TabIndex = 162;
            this.applyLightButton.Text = "Apply";
            this.applyLightButton.UseVisualStyleBackColor = true;
            this.applyLightButton.Click += new System.EventHandler(this.applyLightButton_Click);
            // 
            // applyAllLightButton
            // 
            this.applyAllLightButton.Location = new System.Drawing.Point(98, 254);
            this.applyAllLightButton.Name = "applyAllLightButton";
            this.applyAllLightButton.Size = new System.Drawing.Size(83, 49);
            this.applyAllLightButton.TabIndex = 163;
            this.applyAllLightButton.Text = "Apply All";
            this.applyAllLightButton.UseVisualStyleBackColor = true;
            this.applyAllLightButton.Click += new System.EventHandler(this.applyAllLightButton_Click);
            // 
            // labelLightType
            // 
            this.labelLightType.AutoSize = true;
            this.labelLightType.Location = new System.Drawing.Point(4, 42);
            this.labelLightType.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelLightType.Name = "labelLightType";
            this.labelLightType.Size = new System.Drawing.Size(80, 20);
            this.labelLightType.TabIndex = 161;
            this.labelLightType.Text = "Light Type";
            // 
            // labelLightSource
            // 
            this.labelLightSource.AutoSize = true;
            this.labelLightSource.Location = new System.Drawing.Point(4, 11);
            this.labelLightSource.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelLightSource.Name = "labelLightSource";
            this.labelLightSource.Size = new System.Drawing.Size(94, 20);
            this.labelLightSource.TabIndex = 161;
            this.labelLightSource.Text = "Light Source";
            // 
            // editTypeNameButton
            // 
            this.editTypeNameButton.Location = new System.Drawing.Point(240, 39);
            this.editTypeNameButton.Name = "editTypeNameButton";
            this.editTypeNameButton.Size = new System.Drawing.Size(31, 29);
            this.editTypeNameButton.TabIndex = 167;
            this.editTypeNameButton.Text = "E";
            this.editTypeNameButton.UseVisualStyleBackColor = true;
            this.editTypeNameButton.Click += new System.EventHandler(this.editTypeNameButton_Click);
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
            this.ultraFormManager.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedFixed;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(285, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 345);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(285, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 314);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(284, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 314);
            // 
            // LightParamForm_Fill_Panel
            // 
            // 
            // LightParamForm_Fill_Panel.ClientArea
            // 
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.editTypeNameButton);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.labelExposure);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.lightValueGrid);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.comboLightParamSource);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.exposureTimeMs);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.lightTypeCombo);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.labelExposureMs);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.applyLightButton);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.applyAllLightButton);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.labelLightSource);
            this.LightParamForm_Fill_Panel.ClientArea.Controls.Add(this.labelLightType);
            this.LightParamForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.LightParamForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LightParamForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.LightParamForm_Fill_Panel.Name = "LightParamForm_Fill_Panel";
            this.LightParamForm_Fill_Panel.Size = new System.Drawing.Size(283, 314);
            this.LightParamForm_Fill_Panel.TabIndex = 177;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(186, 254);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 49);
            this.buttonCancel.TabIndex = 163;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // LightParamSimpleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 346);
            this.Controls.Add(this.LightParamForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LightParamSimpleForm";
            this.Text = "Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightParamForm_FormClosing);
            this.Load += new System.EventHandler(this.LightParamPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lightValueGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureTimeMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.LightParamForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.LightParamForm_Fill_Panel.ClientArea.PerformLayout();
            this.LightParamForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView lightValueGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightName;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnValue;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.NumericUpDown exposureTimeMs;
        private System.Windows.Forms.Label labelExposureMs;
        private System.Windows.Forms.ComboBox comboLightParamSource;
        private System.Windows.Forms.ComboBox lightTypeCombo;
        private System.Windows.Forms.Button applyLightButton;
        private System.Windows.Forms.Button applyAllLightButton;
        private System.Windows.Forms.Label labelLightType;
        private System.Windows.Forms.Label labelLightSource;
        private System.Windows.Forms.Button editTypeNameButton;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel LightParamForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button buttonCancel;
    }
}
