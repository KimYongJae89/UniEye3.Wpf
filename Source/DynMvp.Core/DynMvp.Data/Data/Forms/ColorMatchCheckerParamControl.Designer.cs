namespace DynMvp.Data.Forms
{
    partial class ColorMatchCheckerParamControl
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
            this.colorPatternGrid = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnColor = new System.Windows.Forms.DataGridViewImageColumn();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.matchScore = new System.Windows.Forms.NumericUpDown();
            this.labelScore = new System.Windows.Forms.Label();
            this.labelMatchColor = new System.Windows.Forms.Label();
            this.matchColor = new System.Windows.Forms.TextBox();
            this.comboIndex = new System.Windows.Forms.ComboBox();
            this.checkUseColorSet = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelSmoothing = new System.Windows.Forms.Label();
            this.txtSmoothing = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.colorPatternGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSmoothing)).BeginInit();
            this.SuspendLayout();
            // 
            // colorPatternGrid
            // 
            this.colorPatternGrid.AllowUserToAddRows = false;
            this.colorPatternGrid.AllowUserToDeleteRows = false;
            this.colorPatternGrid.AllowUserToResizeColumns = false;
            this.colorPatternGrid.AllowUserToResizeRows = false;
            this.colorPatternGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPatternGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.colorPatternGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnColor});
            this.colorPatternGrid.Location = new System.Drawing.Point(137, 19);
            this.colorPatternGrid.Margin = new System.Windows.Forms.Padding(4);
            this.colorPatternGrid.Name = "colorPatternGrid";
            this.colorPatternGrid.RowHeadersVisible = false;
            this.colorPatternGrid.RowTemplate.Height = 23;
            this.colorPatternGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.colorPatternGrid.Size = new System.Drawing.Size(267, 345);
            this.colorPatternGrid.TabIndex = 0;
            this.colorPatternGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.colorPatternGrid_CellContentClick);
            this.colorPatternGrid.SelectionChanged += new System.EventHandler(this.colorPatternGrid_SelectionChanged);
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnColor
            // 
            this.ColumnColor.HeaderText = "Color";
            this.ColumnColor.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.ColumnColor.Name = "ColumnColor";
            this.ColumnColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(4, 19);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(125, 40);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(4, 67);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(125, 40);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // matchScore
            // 
            this.matchScore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matchScore.Location = new System.Drawing.Point(137, 379);
            this.matchScore.Margin = new System.Windows.Forms.Padding(5);
            this.matchScore.Name = "matchScore";
            this.matchScore.Size = new System.Drawing.Size(81, 24);
            this.matchScore.TabIndex = 14;
            this.matchScore.ValueChanged += new System.EventHandler(this.matchScore_ValueChanged);
            // 
            // labelScore
            // 
            this.labelScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(7, 379);
            this.labelScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(48, 18);
            this.labelScore.TabIndex = 13;
            this.labelScore.Text = "Score";
            // 
            // labelMatchColor
            // 
            this.labelMatchColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMatchColor.AutoSize = true;
            this.labelMatchColor.Location = new System.Drawing.Point(7, 443);
            this.labelMatchColor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMatchColor.Name = "labelMatchColor";
            this.labelMatchColor.Size = new System.Drawing.Size(90, 18);
            this.labelMatchColor.TabIndex = 13;
            this.labelMatchColor.Text = "Match Color";
            // 
            // matchColor
            // 
            this.matchColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matchColor.Location = new System.Drawing.Point(137, 445);
            this.matchColor.Name = "matchColor";
            this.matchColor.ReadOnly = true;
            this.matchColor.Size = new System.Drawing.Size(267, 24);
            this.matchColor.TabIndex = 15;
            this.matchColor.TextChanged += new System.EventHandler(this.matchColor_TextChanged);
            // 
            // comboIndex
            // 
            this.comboIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIndex.FormattingEnabled = true;
            this.comboIndex.Items.AddRange(new object[] {
            "None",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboIndex.Location = new System.Drawing.Point(137, 473);
            this.comboIndex.Name = "comboIndex";
            this.comboIndex.Size = new System.Drawing.Size(197, 26);
            this.comboIndex.TabIndex = 19;
            this.comboIndex.SelectedIndexChanged += new System.EventHandler(this.comboIndex_SelectedIndexChanged);
            this.comboIndex.Validating += new System.ComponentModel.CancelEventHandler(this.comboIndex_Validating);
            // 
            // checkUseColorSet
            // 
            this.checkUseColorSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseColorSet.AutoSize = true;
            this.checkUseColorSet.Location = new System.Drawing.Point(10, 475);
            this.checkUseColorSet.Name = "checkUseColorSet";
            this.checkUseColorSet.Size = new System.Drawing.Size(121, 22);
            this.checkUseColorSet.TabIndex = 20;
            this.checkUseColorSet.Text = "Use Color Set";
            this.checkUseColorSet.UseVisualStyleBackColor = true;
            this.checkUseColorSet.CheckedChanged += new System.EventHandler(this.checkUseColorSet_CheckedChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(341, 470);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(63, 29);
            this.buttonSave.TabIndex = 21;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(4, 115);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(125, 40);
            this.buttonReset.TabIndex = 22;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelSmoothing
            // 
            this.labelSmoothing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSmoothing.AutoSize = true;
            this.labelSmoothing.Location = new System.Drawing.Point(7, 412);
            this.labelSmoothing.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelSmoothing.Name = "labelSmoothing";
            this.labelSmoothing.Size = new System.Drawing.Size(80, 18);
            this.labelSmoothing.TabIndex = 13;
            this.labelSmoothing.Text = "Smoothing";
            // 
            // txtSmoothing
            // 
            this.txtSmoothing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSmoothing.Location = new System.Drawing.Point(137, 416);
            this.txtSmoothing.Margin = new System.Windows.Forms.Padding(5);
            this.txtSmoothing.Name = "txtSmoothing";
            this.txtSmoothing.Size = new System.Drawing.Size(81, 24);
            this.txtSmoothing.TabIndex = 14;
            this.txtSmoothing.ValueChanged += new System.EventHandler(this.matchScore_ValueChanged);
            // 
            // ColorMatchCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkUseColorSet);
            this.Controls.Add(this.comboIndex);
            this.Controls.Add(this.matchColor);
            this.Controls.Add(this.labelMatchColor);
            this.Controls.Add(this.txtSmoothing);
            this.Controls.Add(this.labelSmoothing);
            this.Controls.Add(this.matchScore);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.colorPatternGrid);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ColorMatchCheckerParamControl";
            this.Size = new System.Drawing.Size(411, 529);
            this.Load += new System.EventHandler(this.ColorMatchCheckerParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.colorPatternGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSmoothing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView colorPatternGrid;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.NumericUpDown matchScore;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelMatchColor;
        private System.Windows.Forms.TextBox matchColor;
        private System.Windows.Forms.ComboBox comboIndex;
        private System.Windows.Forms.CheckBox checkUseColorSet;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewImageColumn ColumnColor;
        private System.Windows.Forms.Label labelSmoothing;
        private System.Windows.Forms.NumericUpDown txtSmoothing;
    }
}
