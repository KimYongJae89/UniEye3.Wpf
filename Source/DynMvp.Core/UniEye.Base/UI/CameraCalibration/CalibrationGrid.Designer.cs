namespace UniEye.Base.UI.CameraCalibration
{
    partial class CalibrationGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelNumRow = new System.Windows.Forms.Label();
            this.labelNumCol = new System.Windows.Forms.Label();
            this.colSpace = new System.Windows.Forms.TextBox();
            this.labelRowSpace = new System.Windows.Forms.Label();
            this.numCol = new System.Windows.Forms.NumericUpDown();
            this.labelColSpace = new System.Windows.Forms.Label();
            this.rowSpace = new System.Windows.Forms.TextBox();
            this.numRow = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.calibrationTypeGrid = new System.Windows.Forms.RadioButton();
            this.calibrationTypeChessboard = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelNumRow);
            this.groupBox4.Controls.Add(this.labelNumCol);
            this.groupBox4.Controls.Add(this.colSpace);
            this.groupBox4.Controls.Add(this.labelRowSpace);
            this.groupBox4.Controls.Add(this.numCol);
            this.groupBox4.Controls.Add(this.labelColSpace);
            this.groupBox4.Controls.Add(this.rowSpace);
            this.groupBox4.Controls.Add(this.numRow);
            this.groupBox4.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox4.Location = new System.Drawing.Point(178, 19);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(560, 159);
            this.groupBox4.TabIndex = 22;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Property";
            // 
            // labelNumRow
            // 
            this.labelNumRow.AutoSize = true;
            this.labelNumRow.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelNumRow.Location = new System.Drawing.Point(54, 50);
            this.labelNumRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumRow.Name = "labelNumRow";
            this.labelNumRow.Size = new System.Drawing.Size(76, 19);
            this.labelNumRow.TabIndex = 12;
            this.labelNumRow.Text = "Num Row";
            // 
            // labelNumCol
            // 
            this.labelNumCol.AutoSize = true;
            this.labelNumCol.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelNumCol.Location = new System.Drawing.Point(330, 50);
            this.labelNumCol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumCol.Name = "labelNumCol";
            this.labelNumCol.Size = new System.Drawing.Size(69, 19);
            this.labelNumCol.TabIndex = 11;
            this.labelNumCol.Text = "Num Col";
            // 
            // colSpace
            // 
            this.colSpace.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.colSpace.Location = new System.Drawing.Point(414, 94);
            this.colSpace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.colSpace.Name = "colSpace";
            this.colSpace.Size = new System.Drawing.Size(104, 25);
            this.colSpace.TabIndex = 15;
            this.colSpace.Text = "5";
            // 
            // labelRowSpace
            // 
            this.labelRowSpace.AutoSize = true;
            this.labelRowSpace.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRowSpace.Location = new System.Drawing.Point(40, 97);
            this.labelRowSpace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRowSpace.Name = "labelRowSpace";
            this.labelRowSpace.Size = new System.Drawing.Size(83, 19);
            this.labelRowSpace.TabIndex = 10;
            this.labelRowSpace.Text = "Row Space";
            // 
            // numCol
            // 
            this.numCol.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.numCol.Location = new System.Drawing.Point(414, 47);
            this.numCol.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numCol.Name = "numCol";
            this.numCol.Size = new System.Drawing.Size(104, 25);
            this.numCol.TabIndex = 13;
            // 
            // labelColSpace
            // 
            this.labelColSpace.AutoSize = true;
            this.labelColSpace.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelColSpace.Location = new System.Drawing.Point(314, 97);
            this.labelColSpace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelColSpace.Name = "labelColSpace";
            this.labelColSpace.Size = new System.Drawing.Size(76, 19);
            this.labelColSpace.TabIndex = 9;
            this.labelColSpace.Text = "Col Space";
            // 
            // rowSpace
            // 
            this.rowSpace.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rowSpace.Location = new System.Drawing.Point(174, 94);
            this.rowSpace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rowSpace.Name = "rowSpace";
            this.rowSpace.Size = new System.Drawing.Size(104, 25);
            this.rowSpace.TabIndex = 16;
            this.rowSpace.Text = "5";
            // 
            // numRow
            // 
            this.numRow.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.numRow.Location = new System.Drawing.Point(174, 47);
            this.numRow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numRow.Name = "numRow";
            this.numRow.Size = new System.Drawing.Size(104, 25);
            this.numRow.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.calibrationTypeGrid);
            this.groupBox3.Controls.Add(this.calibrationTypeChessboard);
            this.groupBox3.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.Location = new System.Drawing.Point(10, 19);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(160, 159);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // calibrationTypeGrid
            // 
            this.calibrationTypeGrid.AutoSize = true;
            this.calibrationTypeGrid.Checked = true;
            this.calibrationTypeGrid.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.calibrationTypeGrid.Location = new System.Drawing.Point(21, 48);
            this.calibrationTypeGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.calibrationTypeGrid.Name = "calibrationTypeGrid";
            this.calibrationTypeGrid.Size = new System.Drawing.Size(56, 23);
            this.calibrationTypeGrid.TabIndex = 17;
            this.calibrationTypeGrid.TabStop = true;
            this.calibrationTypeGrid.Text = "Grid";
            this.calibrationTypeGrid.UseVisualStyleBackColor = true;
            // 
            // calibrationTypeChessboard
            // 
            this.calibrationTypeChessboard.AutoSize = true;
            this.calibrationTypeChessboard.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.calibrationTypeChessboard.Location = new System.Drawing.Point(21, 95);
            this.calibrationTypeChessboard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.calibrationTypeChessboard.Name = "calibrationTypeChessboard";
            this.calibrationTypeChessboard.Size = new System.Drawing.Size(105, 23);
            this.calibrationTypeChessboard.TabIndex = 18;
            this.calibrationTypeChessboard.Text = "ChessBoard";
            this.calibrationTypeChessboard.UseVisualStyleBackColor = true;
            // 
            // CalibrationGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CalibrationGrid";
            this.Size = new System.Drawing.Size(830, 261);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelNumRow;
        private System.Windows.Forms.Label labelNumCol;
        private System.Windows.Forms.TextBox colSpace;
        private System.Windows.Forms.Label labelRowSpace;
        private System.Windows.Forms.NumericUpDown numCol;
        private System.Windows.Forms.Label labelColSpace;
        private System.Windows.Forms.TextBox rowSpace;
        private System.Windows.Forms.NumericUpDown numRow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton calibrationTypeGrid;
        private System.Windows.Forms.RadioButton calibrationTypeChessboard;
    }
}
