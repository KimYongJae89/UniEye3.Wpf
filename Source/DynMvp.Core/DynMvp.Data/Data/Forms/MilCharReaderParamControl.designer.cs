namespace DynMvp.Data.Forms
{
    partial class MilCharReaderParamControl
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
            this.desiredString = new System.Windows.Forms.TextBox();
            this.desiredNumCharacter = new System.Windows.Forms.NumericUpDown();
            this.labelNumCharacter = new System.Windows.Forms.Label();
            this.labelDesiredString = new System.Windows.Forms.Label();
            this.minScore = new System.Windows.Forms.NumericUpDown();
            this.labelMinScore = new System.Windows.Forms.Label();
            this.groupBoxFont = new System.Windows.Forms.GroupBox();
            this.labelStringCalibration = new System.Windows.Forms.Label();
            this.calibrationString = new System.Windows.Forms.TextBox();
            this.CalibrationButton = new System.Windows.Forms.Button();
            this.extractFontButton = new System.Windows.Forms.Button();
            this.showFontbutton = new System.Windows.Forms.Button();
            this.fontFileName = new System.Windows.Forms.TextBox();
            this.labelFontFileName = new System.Windows.Forms.Label();
            this.selectFontFile = new System.Windows.Forms.Button();
            this.fontGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.desiredNumCharacter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScore)).BeginInit();
            this.groupBoxFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // desiredString
            // 
            this.desiredString.Location = new System.Drawing.Point(305, 43);
            this.desiredString.Name = "desiredString";
            this.desiredString.Size = new System.Drawing.Size(218, 26);
            this.desiredString.TabIndex = 30;
            this.desiredString.TextChanged += new System.EventHandler(this.desiredString_TextChanged);
            // 
            // desiredNumCharacter
            // 
            this.desiredNumCharacter.Location = new System.Drawing.Point(449, 9);
            this.desiredNumCharacter.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.desiredNumCharacter.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.desiredNumCharacter.Name = "desiredNumCharacter";
            this.desiredNumCharacter.Size = new System.Drawing.Size(74, 26);
            this.desiredNumCharacter.TabIndex = 29;
            this.desiredNumCharacter.ValueChanged += new System.EventHandler(this.desiredNumCharacter_ValueChanged);
            // 
            // labelNumCharacter
            // 
            this.labelNumCharacter.AutoSize = true;
            this.labelNumCharacter.Location = new System.Drawing.Point(6, 9);
            this.labelNumCharacter.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelNumCharacter.Name = "labelNumCharacter";
            this.labelNumCharacter.Size = new System.Drawing.Size(116, 20);
            this.labelNumCharacter.TabIndex = 27;
            this.labelNumCharacter.Text = "Num Character";
            // 
            // labelDesiredString
            // 
            this.labelDesiredString.AutoSize = true;
            this.labelDesiredString.Location = new System.Drawing.Point(6, 44);
            this.labelDesiredString.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDesiredString.Name = "labelDesiredString";
            this.labelDesiredString.Size = new System.Drawing.Size(110, 20);
            this.labelDesiredString.TabIndex = 28;
            this.labelDesiredString.Text = "Desired String";
            // 
            // minScore
            // 
            this.minScore.Location = new System.Drawing.Point(449, 77);
            this.minScore.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.minScore.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minScore.Name = "minScore";
            this.minScore.Size = new System.Drawing.Size(74, 26);
            this.minScore.TabIndex = 34;
            this.minScore.ValueChanged += new System.EventHandler(this.minScore_ValueChanged);
            // 
            // labelMinScore
            // 
            this.labelMinScore.AutoSize = true;
            this.labelMinScore.Location = new System.Drawing.Point(6, 77);
            this.labelMinScore.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMinScore.Name = "labelMinScore";
            this.labelMinScore.Size = new System.Drawing.Size(80, 20);
            this.labelMinScore.TabIndex = 33;
            this.labelMinScore.Text = "Min Score";
            // 
            // groupBoxFont
            // 
            this.groupBoxFont.Controls.Add(this.labelStringCalibration);
            this.groupBoxFont.Controls.Add(this.calibrationString);
            this.groupBoxFont.Controls.Add(this.CalibrationButton);
            this.groupBoxFont.Controls.Add(this.extractFontButton);
            this.groupBoxFont.Controls.Add(this.showFontbutton);
            this.groupBoxFont.Controls.Add(this.fontFileName);
            this.groupBoxFont.Controls.Add(this.labelFontFileName);
            this.groupBoxFont.Controls.Add(this.selectFontFile);
            this.groupBoxFont.Controls.Add(this.fontGrid);
            this.groupBoxFont.Location = new System.Drawing.Point(10, 111);
            this.groupBoxFont.Name = "groupBoxFont";
            this.groupBoxFont.Size = new System.Drawing.Size(513, 238);
            this.groupBoxFont.TabIndex = 38;
            this.groupBoxFont.TabStop = false;
            this.groupBoxFont.Text = "Font";
            // 
            // labelStringCalibration
            // 
            this.labelStringCalibration.AutoSize = true;
            this.labelStringCalibration.Location = new System.Drawing.Point(11, 56);
            this.labelStringCalibration.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelStringCalibration.Name = "labelStringCalibration";
            this.labelStringCalibration.Size = new System.Drawing.Size(130, 20);
            this.labelStringCalibration.TabIndex = 46;
            this.labelStringCalibration.Text = "String Calibration";
            // 
            // calibrationString
            // 
            this.calibrationString.Location = new System.Drawing.Point(373, 53);
            this.calibrationString.Name = "calibrationString";
            this.calibrationString.Size = new System.Drawing.Size(134, 26);
            this.calibrationString.TabIndex = 45;
            // 
            // CalibrationButton
            // 
            this.CalibrationButton.Location = new System.Drawing.Point(258, 51);
            this.CalibrationButton.Name = "CalibrationButton";
            this.CalibrationButton.Size = new System.Drawing.Size(109, 30);
            this.CalibrationButton.TabIndex = 44;
            this.CalibrationButton.Text = "Calibration";
            this.CalibrationButton.UseVisualStyleBackColor = true;
            this.CalibrationButton.Click += new System.EventHandler(this.calibrationButton_Click);
            // 
            // extractFontButton
            // 
            this.extractFontButton.Location = new System.Drawing.Point(17, 201);
            this.extractFontButton.Name = "extractFontButton";
            this.extractFontButton.Size = new System.Drawing.Size(140, 30);
            this.extractFontButton.TabIndex = 42;
            this.extractFontButton.Text = "Extract Font";
            this.extractFontButton.UseVisualStyleBackColor = true;
            this.extractFontButton.Click += new System.EventHandler(this.extractFontButton_Click);
            // 
            // showFontbutton
            // 
            this.showFontbutton.Location = new System.Drawing.Point(396, 201);
            this.showFontbutton.Name = "showFontbutton";
            this.showFontbutton.Size = new System.Drawing.Size(111, 30);
            this.showFontbutton.TabIndex = 43;
            this.showFontbutton.Text = "Show Font";
            this.showFontbutton.UseVisualStyleBackColor = true;
            this.showFontbutton.Click += new System.EventHandler(this.showFontbutton_Click);
            // 
            // fontFileName
            // 
            this.fontFileName.Location = new System.Drawing.Point(295, 22);
            this.fontFileName.Name = "fontFileName";
            this.fontFileName.Size = new System.Drawing.Size(212, 26);
            this.fontFileName.TabIndex = 41;
            // 
            // labelFontFileName
            // 
            this.labelFontFileName.AutoSize = true;
            this.labelFontFileName.Location = new System.Drawing.Point(11, 28);
            this.labelFontFileName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelFontFileName.Name = "labelFontFileName";
            this.labelFontFileName.Size = new System.Drawing.Size(117, 20);
            this.labelFontFileName.TabIndex = 40;
            this.labelFontFileName.Text = "Font File Name";
            // 
            // selectFontFile
            // 
            this.selectFontFile.Location = new System.Drawing.Point(258, 22);
            this.selectFontFile.Name = "selectFontFile";
            this.selectFontFile.Size = new System.Drawing.Size(31, 26);
            this.selectFontFile.TabIndex = 39;
            this.selectFontFile.Text = "...";
            this.selectFontFile.UseVisualStyleBackColor = true;
            this.selectFontFile.Click += new System.EventHandler(this.selectFontFile_Click);
            // 
            // fontGrid
            // 
            this.fontGrid.AllowUserToAddRows = false;
            this.fontGrid.AllowUserToDeleteRows = false;
            this.fontGrid.AllowUserToResizeColumns = false;
            this.fontGrid.AllowUserToResizeRows = false;
            this.fontGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fontGrid.ColumnHeadersVisible = false;
            this.fontGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.fontGrid.Location = new System.Drawing.Point(13, 87);
            this.fontGrid.Name = "fontGrid";
            this.fontGrid.RowHeadersVisible = false;
            this.fontGrid.RowTemplate.Height = 60;
            this.fontGrid.Size = new System.Drawing.Size(494, 108);
            this.fontGrid.TabIndex = 38;
            this.fontGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.fontGrid_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // MilCharReaderParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.groupBoxFont);
            this.Controls.Add(this.minScore);
            this.Controls.Add(this.labelMinScore);
            this.Controls.Add(this.desiredString);
            this.Controls.Add(this.desiredNumCharacter);
            this.Controls.Add(this.labelNumCharacter);
            this.Controls.Add(this.labelDesiredString);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MilCharReaderParamControl";
            this.Size = new System.Drawing.Size(528, 433);
            ((System.ComponentModel.ISupportInitialize)(this.desiredNumCharacter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScore)).EndInit();
            this.groupBoxFont.ResumeLayout(false);
            this.groupBoxFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox desiredString;
        private System.Windows.Forms.NumericUpDown desiredNumCharacter;
        private System.Windows.Forms.Label labelNumCharacter;
        private System.Windows.Forms.Label labelDesiredString;
        private System.Windows.Forms.NumericUpDown minScore;
        private System.Windows.Forms.Label labelMinScore;
        private System.Windows.Forms.GroupBox groupBoxFont;
        private System.Windows.Forms.DataGridView fontGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.TextBox fontFileName;
        private System.Windows.Forms.Label labelFontFileName;
        private System.Windows.Forms.Button selectFontFile;
        private System.Windows.Forms.Button extractFontButton;
        private System.Windows.Forms.Button showFontbutton;
        private System.Windows.Forms.Button CalibrationButton;
        private System.Windows.Forms.Label labelStringCalibration;
        private System.Windows.Forms.TextBox calibrationString;


    }
}