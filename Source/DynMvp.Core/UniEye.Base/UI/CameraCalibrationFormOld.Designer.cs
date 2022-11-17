namespace UniEye.Base.UI
{
    partial class CameraCalibrationFormOld
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
            this.buttonGrab = new System.Windows.Forms.Button();
            this.buttonCalibrate = new System.Windows.Forms.Button();
            this.buttonSaveCalibration = new System.Windows.Forms.Button();
            this.labelCamera = new System.Windows.Forms.Label();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.calibrationImage = new System.Windows.Forms.PictureBox();
            this.labelNumRow = new System.Windows.Forms.Label();
            this.labelNumCol = new System.Windows.Forms.Label();
            this.labelRowSpace = new System.Windows.Forms.Label();
            this.labelColSpace = new System.Windows.Forms.Label();
            this.rowSpace = new System.Windows.Forms.TextBox();
            this.colSpace = new System.Windows.Forms.TextBox();
            this.numRow = new System.Windows.Forms.NumericUpDown();
            this.numCol = new System.Windows.Forms.NumericUpDown();
            this.buttonLoadCalibration = new System.Windows.Forms.Button();
            this.calibrationTypeSingleScale = new System.Windows.Forms.RadioButton();
            this.pelWidth = new System.Windows.Forms.TextBox();
            this.pelHeight = new System.Windows.Forms.TextBox();
            this.labelScaleX = new System.Windows.Forms.Label();
            this.labelScaleY = new System.Windows.Forms.Label();
            this.calibrationTypeGrid = new System.Windows.Forms.RadioButton();
            this.calibrationTypeChessboard = new System.Windows.Forms.RadioButton();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.CameraCalibrationForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.labelLightType = new System.Windows.Forms.Label();
            this.comboLightType = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.calibrationImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.CameraCalibrationForm_Fill_Panel.ClientArea.SuspendLayout();
            this.CameraCalibrationForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGrab
            // 
            this.buttonGrab.Location = new System.Drawing.Point(430, 40);
            this.buttonGrab.Name = "buttonGrab";
            this.buttonGrab.Size = new System.Drawing.Size(108, 85);
            this.buttonGrab.TabIndex = 2;
            this.buttonGrab.Text = "Grab";
            this.buttonGrab.UseVisualStyleBackColor = true;
            this.buttonGrab.Click += new System.EventHandler(this.buttonGrab_Click);
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Location = new System.Drawing.Point(544, 40);
            this.buttonCalibrate.Name = "buttonCalibrate";
            this.buttonCalibrate.Size = new System.Drawing.Size(111, 85);
            this.buttonCalibrate.TabIndex = 2;
            this.buttonCalibrate.Text = "Calibrate";
            this.buttonCalibrate.UseVisualStyleBackColor = true;
            this.buttonCalibrate.Click += new System.EventHandler(this.buttonCalibrate_Click);
            // 
            // buttonSaveCalibration
            // 
            this.buttonSaveCalibration.Location = new System.Drawing.Point(661, 40);
            this.buttonSaveCalibration.Name = "buttonSaveCalibration";
            this.buttonSaveCalibration.Size = new System.Drawing.Size(77, 52);
            this.buttonSaveCalibration.TabIndex = 2;
            this.buttonSaveCalibration.Text = "Save";
            this.buttonSaveCalibration.UseVisualStyleBackColor = true;
            this.buttonSaveCalibration.Click += new System.EventHandler(this.buttonSaveCalibration_Click);
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Location = new System.Drawing.Point(6, 13);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(50, 12);
            this.labelCamera.TabIndex = 1;
            this.labelCamera.Text = "Camera";
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(62, 9);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(120, 20);
            this.comboBoxCamera.TabIndex = 0;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            // 
            // calibrationImage
            // 
            this.calibrationImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calibrationImage.Location = new System.Drawing.Point(5, 142);
            this.calibrationImage.Name = "calibrationImage";
            this.calibrationImage.Size = new System.Drawing.Size(740, 428);
            this.calibrationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.calibrationImage.TabIndex = 3;
            this.calibrationImage.TabStop = false;
            // 
            // labelNumRow
            // 
            this.labelNumRow.AutoSize = true;
            this.labelNumRow.Location = new System.Drawing.Point(121, 81);
            this.labelNumRow.Name = "labelNumRow";
            this.labelNumRow.Size = new System.Drawing.Size(61, 12);
            this.labelNumRow.TabIndex = 1;
            this.labelNumRow.Text = "Num Row";
            // 
            // labelNumCol
            // 
            this.labelNumCol.AutoSize = true;
            this.labelNumCol.Location = new System.Drawing.Point(282, 81);
            this.labelNumCol.Name = "labelNumCol";
            this.labelNumCol.Size = new System.Drawing.Size(55, 12);
            this.labelNumCol.TabIndex = 1;
            this.labelNumCol.Text = "Num Col";
            // 
            // labelRowSpace
            // 
            this.labelRowSpace.AutoSize = true;
            this.labelRowSpace.Location = new System.Drawing.Point(112, 108);
            this.labelRowSpace.Name = "labelRowSpace";
            this.labelRowSpace.Size = new System.Drawing.Size(70, 12);
            this.labelRowSpace.TabIndex = 1;
            this.labelRowSpace.Text = "Row Space";
            // 
            // labelColSpace
            // 
            this.labelColSpace.AutoSize = true;
            this.labelColSpace.Location = new System.Drawing.Point(273, 108);
            this.labelColSpace.Name = "labelColSpace";
            this.labelColSpace.Size = new System.Drawing.Size(64, 12);
            this.labelColSpace.TabIndex = 1;
            this.labelColSpace.Text = "Col Space";
            // 
            // rowSpace
            // 
            this.rowSpace.Location = new System.Drawing.Point(188, 104);
            this.rowSpace.Name = "rowSpace";
            this.rowSpace.Size = new System.Drawing.Size(75, 21);
            this.rowSpace.TabIndex = 5;
            this.rowSpace.Text = "5";
            // 
            // colSpace
            // 
            this.colSpace.Location = new System.Drawing.Point(343, 104);
            this.colSpace.Name = "colSpace";
            this.colSpace.Size = new System.Drawing.Size(75, 21);
            this.colSpace.TabIndex = 5;
            this.colSpace.Text = "5";
            // 
            // numRow
            // 
            this.numRow.Location = new System.Drawing.Point(188, 77);
            this.numRow.Name = "numRow";
            this.numRow.Size = new System.Drawing.Size(75, 21);
            this.numRow.TabIndex = 4;
            // 
            // numCol
            // 
            this.numCol.Location = new System.Drawing.Point(343, 77);
            this.numCol.Name = "numCol";
            this.numCol.Size = new System.Drawing.Size(75, 21);
            this.numCol.TabIndex = 4;
            // 
            // buttonLoadCalibration
            // 
            this.buttonLoadCalibration.Location = new System.Drawing.Point(661, 98);
            this.buttonLoadCalibration.Name = "buttonLoadCalibration";
            this.buttonLoadCalibration.Size = new System.Drawing.Size(77, 27);
            this.buttonLoadCalibration.TabIndex = 2;
            this.buttonLoadCalibration.Text = "Load";
            this.buttonLoadCalibration.UseVisualStyleBackColor = true;
            this.buttonLoadCalibration.Click += new System.EventHandler(this.buttonLoadCalibration_Click);
            // 
            // calibrationTypeSingleScale
            // 
            this.calibrationTypeSingleScale.AutoSize = true;
            this.calibrationTypeSingleScale.Location = new System.Drawing.Point(14, 42);
            this.calibrationTypeSingleScale.Name = "calibrationTypeSingleScale";
            this.calibrationTypeSingleScale.Size = new System.Drawing.Size(94, 16);
            this.calibrationTypeSingleScale.TabIndex = 6;
            this.calibrationTypeSingleScale.TabStop = true;
            this.calibrationTypeSingleScale.Text = "Single Scale";
            this.calibrationTypeSingleScale.UseVisualStyleBackColor = true;
            this.calibrationTypeSingleScale.CheckedChanged += new System.EventHandler(this.CalibrationTypeChanged);
            // 
            // pelWidth
            // 
            this.pelWidth.Location = new System.Drawing.Point(188, 40);
            this.pelWidth.Name = "pelWidth";
            this.pelWidth.Size = new System.Drawing.Size(75, 21);
            this.pelWidth.TabIndex = 7;
            // 
            // pelHeight
            // 
            this.pelHeight.Location = new System.Drawing.Point(343, 40);
            this.pelHeight.Name = "pelHeight";
            this.pelHeight.Size = new System.Drawing.Size(75, 21);
            this.pelHeight.TabIndex = 7;
            // 
            // labelScaleX
            // 
            this.labelScaleX.AutoSize = true;
            this.labelScaleX.Location = new System.Drawing.Point(133, 44);
            this.labelScaleX.Name = "labelScaleX";
            this.labelScaleX.Size = new System.Drawing.Size(49, 12);
            this.labelScaleX.TabIndex = 1;
            this.labelScaleX.Text = "Scale X";
            // 
            // labelScaleY
            // 
            this.labelScaleY.AutoSize = true;
            this.labelScaleY.Location = new System.Drawing.Point(288, 44);
            this.labelScaleY.Name = "labelScaleY";
            this.labelScaleY.Size = new System.Drawing.Size(49, 12);
            this.labelScaleY.TabIndex = 1;
            this.labelScaleY.Text = "Scale Y";
            // 
            // calibrationTypeGrid
            // 
            this.calibrationTypeGrid.AutoSize = true;
            this.calibrationTypeGrid.Location = new System.Drawing.Point(14, 79);
            this.calibrationTypeGrid.Name = "calibrationTypeGrid";
            this.calibrationTypeGrid.Size = new System.Drawing.Size(46, 16);
            this.calibrationTypeGrid.TabIndex = 6;
            this.calibrationTypeGrid.TabStop = true;
            this.calibrationTypeGrid.Text = "Grid";
            this.calibrationTypeGrid.UseVisualStyleBackColor = true;
            this.calibrationTypeGrid.CheckedChanged += new System.EventHandler(this.CalibrationTypeChanged);
            // 
            // calibrationTypeChessboard
            // 
            this.calibrationTypeChessboard.AutoSize = true;
            this.calibrationTypeChessboard.Location = new System.Drawing.Point(14, 109);
            this.calibrationTypeChessboard.Name = "calibrationTypeChessboard";
            this.calibrationTypeChessboard.Size = new System.Drawing.Size(93, 16);
            this.calibrationTypeChessboard.TabIndex = 8;
            this.calibrationTypeChessboard.TabStop = true;
            this.calibrationTypeChessboard.Text = "ChessBoard";
            this.calibrationTypeChessboard.UseVisualStyleBackColor = true;
            this.calibrationTypeChessboard.CheckedChanged += new System.EventHandler(this.CalibrationTypeChanged);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(752, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 604);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(752, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 573);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(751, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 573);
            // 
            // CameraCalibrationForm_Fill_Panel
            // 
            // 
            // CameraCalibrationForm_Fill_Panel.ClientArea
            // 
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.dataGridView1);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.calibrationTypeChessboard);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.pelHeight);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.pelWidth);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.calibrationTypeGrid);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.calibrationTypeSingleScale);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.colSpace);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.numCol);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.rowSpace);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.numRow);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.calibrationImage);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonLoadCalibration);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonSaveCalibration);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCalibrate);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.buttonGrab);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelColSpace);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelScaleY);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelScaleX);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelRowSpace);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelNumCol);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelNumRow);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelLightType);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.labelCamera);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.comboLightType);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.Controls.Add(this.comboBoxCamera);
            this.CameraCalibrationForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.CameraCalibrationForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CameraCalibrationForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.CameraCalibrationForm_Fill_Panel.Name = "CameraCalibrationForm_Fill_Panel";
            this.CameraCalibrationForm_Fill_Panel.Size = new System.Drawing.Size(750, 573);
            this.CameraCalibrationForm_Fill_Panel.TabIndex = 17;
            // 
            // labelLightType
            // 
            this.labelLightType.AutoSize = true;
            this.labelLightType.Location = new System.Drawing.Point(242, 13);
            this.labelLightType.Name = "labelLightType";
            this.labelLightType.Size = new System.Drawing.Size(65, 12);
            this.labelLightType.TabIndex = 1;
            this.labelLightType.Text = "Light Type";
            // 
            // comboLightType
            // 
            this.comboLightType.FormattingEnabled = true;
            this.comboLightType.Items.AddRange(new object[] {
            "Light Type 1",
            "Light Type 2"});
            this.comboLightType.Location = new System.Drawing.Point(313, 7);
            this.comboLightType.Name = "comboLightType";
            this.comboLightType.Size = new System.Drawing.Size(120, 20);
            this.comboLightType.TabIndex = 0;
            this.comboLightType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(287, 247);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(229, 56);
            this.dataGridView1.TabIndex = 9;
            // 
            // CameraCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 605);
            this.Controls.Add(this.CameraCalibrationForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.Name = "CameraCalibrationForm";
            this.Text = "Camera Calibration";
            this.Load += new System.EventHandler(this.CameraCalibrationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.calibrationImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.CameraCalibrationForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.CameraCalibrationForm_Fill_Panel.ClientArea.PerformLayout();
            this.CameraCalibrationForm_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonGrab;
        private System.Windows.Forms.Button buttonCalibrate;
        private System.Windows.Forms.Button buttonSaveCalibration;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.PictureBox calibrationImage;
        private System.Windows.Forms.Label labelNumRow;
        private System.Windows.Forms.Label labelNumCol;
        private System.Windows.Forms.Label labelRowSpace;
        private System.Windows.Forms.Label labelColSpace;
        private System.Windows.Forms.TextBox rowSpace;
        private System.Windows.Forms.TextBox colSpace;
        private System.Windows.Forms.NumericUpDown numRow;
        private System.Windows.Forms.NumericUpDown numCol;
        private System.Windows.Forms.Button buttonLoadCalibration;
        private System.Windows.Forms.RadioButton calibrationTypeSingleScale;
        private System.Windows.Forms.TextBox pelWidth;
        private System.Windows.Forms.TextBox pelHeight;
        private System.Windows.Forms.Label labelScaleX;
        private System.Windows.Forms.Label labelScaleY;
        private System.Windows.Forms.RadioButton calibrationTypeGrid;
        private System.Windows.Forms.RadioButton calibrationTypeChessboard;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel CameraCalibrationForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Label labelLightType;
        private System.Windows.Forms.ComboBox comboLightType;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}