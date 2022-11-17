namespace DynMvp.Data.Forms
{
    partial class PatternMatchingParamControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.addPatternButton = new System.Windows.Forms.Button();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.searchRangeHeight = new System.Windows.Forms.NumericUpDown();
            this.searchRangeWidth = new System.Windows.Forms.NumericUpDown();
            this.labelScore = new System.Windows.Forms.Label();
            this.matchScore = new System.Windows.Forms.NumericUpDown();
            this.patternImageSelector = new System.Windows.Forms.DataGridView();
            this.ColumnPatternImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.refreshPatternButton = new System.Windows.Forms.Button();
            this.deletePatternButton = new System.Windows.Forms.Button();
            this.editMaskButton = new System.Windows.Forms.Button();
            this.patternType = new System.Windows.Forms.ComboBox();
            this.maxScale = new System.Windows.Forms.NumericUpDown();
            this.maxAngle = new System.Windows.Forms.NumericUpDown();
            this.minScale = new System.Windows.Forms.NumericUpDown();
            this.minAngle = new System.Windows.Forms.NumericUpDown();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelAngle = new System.Windows.Forms.Label();
            this.labelScaleMax = new System.Windows.Forms.Label();
            this.labelAngleMax = new System.Windows.Forms.Label();
            this.labelScaleMin = new System.Windows.Forms.Label();
            this.labelAngleMin = new System.Windows.Forms.Label();
            this.centerOffset = new System.Windows.Forms.CheckBox();
            this.useWholeImage = new System.Windows.Forms.CheckBox();
            this.numToleranceY = new System.Windows.Forms.NumericUpDown();
            this.numToleranceX = new System.Windows.Forms.NumericUpDown();
            this.labelTolerance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.useAllMatching = new System.Windows.Forms.CheckBox();
            this.labelNumToFind = new System.Windows.Forms.Label();
            this.numToFind = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToleranceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToleranceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToFind)).BeginInit();
            this.SuspendLayout();
            // 
            // addPatternButton
            // 
            this.addPatternButton.Location = new System.Drawing.Point(213, 8);
            this.addPatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.addPatternButton.Name = "addPatternButton";
            this.addPatternButton.Size = new System.Drawing.Size(74, 27);
            this.addPatternButton.TabIndex = 1;
            this.addPatternButton.Text = "Add";
            this.addPatternButton.UseVisualStyleBackColor = true;
            this.addPatternButton.Click += new System.EventHandler(this.addPatternButton_Click);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(288, 11);
            this.labelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(112, 20);
            this.labelSize.TabIndex = 6;
            this.labelSize.Text = "Search Range";
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(292, 65);
            this.labelH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(21, 20);
            this.labelH.TabIndex = 9;
            this.labelH.Text = "H";
            this.labelH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(292, 37);
            this.labelW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(24, 20);
            this.labelW.TabIndex = 7;
            this.labelW.Text = "W";
            this.labelW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchRangeHeight
            // 
            this.searchRangeHeight.Location = new System.Drawing.Point(338, 63);
            this.searchRangeHeight.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeHeight.Name = "searchRangeHeight";
            this.searchRangeHeight.Size = new System.Drawing.Size(62, 26);
            this.searchRangeHeight.TabIndex = 10;
            this.searchRangeHeight.ValueChanged += new System.EventHandler(this.searchRangeHeight_ValueChanged);
            this.searchRangeHeight.Enter += new System.EventHandler(this.textBox_Enter);
            this.searchRangeHeight.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // searchRangeWidth
            // 
            this.searchRangeWidth.Location = new System.Drawing.Point(338, 35);
            this.searchRangeWidth.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeWidth.Name = "searchRangeWidth";
            this.searchRangeWidth.Size = new System.Drawing.Size(62, 26);
            this.searchRangeWidth.TabIndex = 8;
            this.searchRangeWidth.ValueChanged += new System.EventHandler(this.searchRangeWidth_ValueChanged);
            this.searchRangeWidth.Enter += new System.EventHandler(this.textBox_Enter);
            this.searchRangeWidth.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(221, 152);
            this.labelScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(51, 20);
            this.labelScore.TabIndex = 11;
            this.labelScore.Text = "Score";
            // 
            // matchScore
            // 
            this.matchScore.Location = new System.Drawing.Point(213, 176);
            this.matchScore.Margin = new System.Windows.Forms.Padding(4);
            this.matchScore.Name = "matchScore";
            this.matchScore.Size = new System.Drawing.Size(67, 26);
            this.matchScore.TabIndex = 12;
            this.matchScore.ValueChanged += new System.EventHandler(this.matchScore_ValueChanged);
            this.matchScore.Enter += new System.EventHandler(this.textBox_Enter);
            this.matchScore.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // patternImageSelector
            // 
            this.patternImageSelector.AllowUserToAddRows = false;
            this.patternImageSelector.AllowUserToDeleteRows = false;
            this.patternImageSelector.AllowUserToResizeColumns = false;
            this.patternImageSelector.AllowUserToResizeRows = false;
            this.patternImageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.patternImageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.patternImageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patternImageSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPatternImage});
            this.patternImageSelector.Location = new System.Drawing.Point(6, 7);
            this.patternImageSelector.Margin = new System.Windows.Forms.Padding(2);
            this.patternImageSelector.MultiSelect = false;
            this.patternImageSelector.Name = "patternImageSelector";
            this.patternImageSelector.ReadOnly = true;
            this.patternImageSelector.RowHeadersVisible = false;
            this.patternImageSelector.RowTemplate.Height = 23;
            this.patternImageSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.patternImageSelector.Size = new System.Drawing.Size(203, 414);
            this.patternImageSelector.TabIndex = 0;
            this.patternImageSelector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.patternImageSelector_CellClick);
            // 
            // ColumnPatternImage
            // 
            this.ColumnPatternImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPatternImage.HeaderText = "Pattern Image";
            this.ColumnPatternImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ColumnPatternImage.Name = "ColumnPatternImage";
            this.ColumnPatternImage.ReadOnly = true;
            this.ColumnPatternImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnPatternImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // refreshPatternButton
            // 
            this.refreshPatternButton.Location = new System.Drawing.Point(213, 36);
            this.refreshPatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.refreshPatternButton.Name = "refreshPatternButton";
            this.refreshPatternButton.Size = new System.Drawing.Size(74, 27);
            this.refreshPatternButton.TabIndex = 2;
            this.refreshPatternButton.Text = "Refresh";
            this.refreshPatternButton.UseVisualStyleBackColor = true;
            this.refreshPatternButton.Click += new System.EventHandler(this.refreshPatternButton_Click);
            // 
            // deletePatternButton
            // 
            this.deletePatternButton.Location = new System.Drawing.Point(213, 64);
            this.deletePatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.deletePatternButton.Name = "deletePatternButton";
            this.deletePatternButton.Size = new System.Drawing.Size(74, 27);
            this.deletePatternButton.TabIndex = 3;
            this.deletePatternButton.Text = "Delete";
            this.deletePatternButton.UseVisualStyleBackColor = true;
            this.deletePatternButton.Click += new System.EventHandler(this.DeletePatternButton_Click);
            // 
            // editMaskButton
            // 
            this.editMaskButton.Location = new System.Drawing.Point(213, 92);
            this.editMaskButton.Margin = new System.Windows.Forms.Padding(4);
            this.editMaskButton.Name = "editMaskButton";
            this.editMaskButton.Size = new System.Drawing.Size(74, 27);
            this.editMaskButton.TabIndex = 4;
            this.editMaskButton.Text = "Mask";
            this.editMaskButton.UseVisualStyleBackColor = true;
            this.editMaskButton.Click += new System.EventHandler(this.editMaskButton_Click);
            // 
            // patternType
            // 
            this.patternType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patternType.FormattingEnabled = true;
            this.patternType.Items.AddRange(new object[] {
            "Good",
            "NG"});
            this.patternType.Location = new System.Drawing.Point(213, 121);
            this.patternType.Margin = new System.Windows.Forms.Padding(2);
            this.patternType.Name = "patternType";
            this.patternType.Size = new System.Drawing.Size(74, 28);
            this.patternType.TabIndex = 5;
            this.patternType.SelectedIndexChanged += new System.EventHandler(this.patternType_SelectedIndexChanged);
            // 
            // maxScale
            // 
            this.maxScale.DecimalPlaces = 2;
            this.maxScale.Location = new System.Drawing.Point(338, 305);
            this.maxScale.Margin = new System.Windows.Forms.Padding(4);
            this.maxScale.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.maxScale.Name = "maxScale";
            this.maxScale.Size = new System.Drawing.Size(59, 26);
            this.maxScale.TabIndex = 21;
            this.maxScale.ValueChanged += new System.EventHandler(this.maxScale_ValueChanged);
            // 
            // maxAngle
            // 
            this.maxAngle.Location = new System.Drawing.Point(338, 225);
            this.maxAngle.Margin = new System.Windows.Forms.Padding(4);
            this.maxAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.maxAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.maxAngle.Name = "maxAngle";
            this.maxAngle.Size = new System.Drawing.Size(59, 26);
            this.maxAngle.TabIndex = 22;
            this.maxAngle.ValueChanged += new System.EventHandler(this.maxAngle_ValueChanged);
            // 
            // minScale
            // 
            this.minScale.DecimalPlaces = 2;
            this.minScale.Location = new System.Drawing.Point(338, 277);
            this.minScale.Margin = new System.Windows.Forms.Padding(4);
            this.minScale.Name = "minScale";
            this.minScale.Size = new System.Drawing.Size(59, 26);
            this.minScale.TabIndex = 17;
            this.minScale.ValueChanged += new System.EventHandler(this.minScale_ValueChanged);
            // 
            // minAngle
            // 
            this.minAngle.Location = new System.Drawing.Point(338, 197);
            this.minAngle.Margin = new System.Windows.Forms.Padding(4);
            this.minAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.minAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.minAngle.Name = "minAngle";
            this.minAngle.Size = new System.Drawing.Size(59, 26);
            this.minAngle.TabIndex = 18;
            this.minAngle.ValueChanged += new System.EventHandler(this.minAngle_ValueChanged);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(288, 254);
            this.labelScale.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(49, 20);
            this.labelScale.TabIndex = 14;
            this.labelScale.Text = "Scale";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(288, 174);
            this.labelAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.Size = new System.Drawing.Size(50, 20);
            this.labelAngle.TabIndex = 13;
            this.labelAngle.Text = "Angle";
            // 
            // labelScaleMax
            // 
            this.labelScaleMax.AutoSize = true;
            this.labelScaleMax.Location = new System.Drawing.Point(292, 307);
            this.labelScaleMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleMax.Name = "labelScaleMax";
            this.labelScaleMax.Size = new System.Drawing.Size(38, 20);
            this.labelScaleMax.TabIndex = 20;
            this.labelScaleMax.Text = "Max";
            this.labelScaleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAngleMax
            // 
            this.labelAngleMax.AutoSize = true;
            this.labelAngleMax.Location = new System.Drawing.Point(292, 227);
            this.labelAngleMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngleMax.Name = "labelAngleMax";
            this.labelAngleMax.Size = new System.Drawing.Size(38, 20);
            this.labelAngleMax.TabIndex = 19;
            this.labelAngleMax.Text = "Max";
            this.labelAngleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelScaleMin
            // 
            this.labelScaleMin.AutoSize = true;
            this.labelScaleMin.Location = new System.Drawing.Point(292, 280);
            this.labelScaleMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleMin.Name = "labelScaleMin";
            this.labelScaleMin.Size = new System.Drawing.Size(34, 20);
            this.labelScaleMin.TabIndex = 16;
            this.labelScaleMin.Text = "Min";
            this.labelScaleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAngleMin
            // 
            this.labelAngleMin.AutoSize = true;
            this.labelAngleMin.Location = new System.Drawing.Point(292, 200);
            this.labelAngleMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngleMin.Name = "labelAngleMin";
            this.labelAngleMin.Size = new System.Drawing.Size(34, 20);
            this.labelAngleMin.TabIndex = 15;
            this.labelAngleMin.Text = "Min";
            this.labelAngleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // centerOffset
            // 
            this.centerOffset.AutoSize = true;
            this.centerOffset.Location = new System.Drawing.Point(213, 264);
            this.centerOffset.Margin = new System.Windows.Forms.Padding(2);
            this.centerOffset.Name = "centerOffset";
            this.centerOffset.Size = new System.Drawing.Size(76, 24);
            this.centerOffset.TabIndex = 23;
            this.centerOffset.Text = "Center";
            this.centerOffset.UseVisualStyleBackColor = true;
            this.centerOffset.CheckedChanged += new System.EventHandler(this.centerOffset_CheckedChanged);
            // 
            // useWholeImage
            // 
            this.useWholeImage.AutoSize = true;
            this.useWholeImage.Location = new System.Drawing.Point(213, 293);
            this.useWholeImage.Name = "useWholeImage";
            this.useWholeImage.Size = new System.Drawing.Size(73, 24);
            this.useWholeImage.TabIndex = 24;
            this.useWholeImage.Text = "Whole";
            this.useWholeImage.UseVisualStyleBackColor = true;
            this.useWholeImage.CheckedChanged += new System.EventHandler(this.useWholeImage_CheckedChanged);
            // 
            // numToleranceY
            // 
            this.numToleranceY.Location = new System.Drawing.Point(338, 144);
            this.numToleranceY.Margin = new System.Windows.Forms.Padding(4);
            this.numToleranceY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numToleranceY.Name = "numToleranceY";
            this.numToleranceY.Size = new System.Drawing.Size(59, 26);
            this.numToleranceY.TabIndex = 29;
            this.numToleranceY.ValueChanged += new System.EventHandler(this.numToleranceY_ValueChanged);
            // 
            // numToleranceX
            // 
            this.numToleranceX.Location = new System.Drawing.Point(338, 116);
            this.numToleranceX.Margin = new System.Windows.Forms.Padding(4);
            this.numToleranceX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numToleranceX.Name = "numToleranceX";
            this.numToleranceX.Size = new System.Drawing.Size(59, 26);
            this.numToleranceX.TabIndex = 27;
            this.numToleranceX.ValueChanged += new System.EventHandler(this.numToleranceX_ValueChanged);
            // 
            // labelTolerance
            // 
            this.labelTolerance.AutoSize = true;
            this.labelTolerance.Location = new System.Drawing.Point(288, 93);
            this.labelTolerance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTolerance.Name = "labelTolerance";
            this.labelTolerance.Size = new System.Drawing.Size(79, 20);
            this.labelTolerance.TabIndex = 25;
            this.labelTolerance.Text = "Tolerance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 147);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 119);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 20);
            this.label3.TabIndex = 26;
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // useAllMatching
            // 
            this.useAllMatching.AutoSize = true;
            this.useAllMatching.Location = new System.Drawing.Point(213, 323);
            this.useAllMatching.Name = "useAllMatching";
            this.useAllMatching.Size = new System.Drawing.Size(45, 24);
            this.useAllMatching.TabIndex = 24;
            this.useAllMatching.Text = "All";
            this.useAllMatching.UseVisualStyleBackColor = true;
            this.useAllMatching.CheckedChanged += new System.EventHandler(this.useAllMatching_CheckedChanged);
            // 
            // labelNumToFind
            // 
            this.labelNumToFind.AutoSize = true;
            this.labelNumToFind.Location = new System.Drawing.Point(221, 206);
            this.labelNumToFind.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumToFind.Name = "labelNumToFind";
            this.labelNumToFind.Size = new System.Drawing.Size(42, 20);
            this.labelNumToFind.TabIndex = 11;
            this.labelNumToFind.Text = "Num";
            // 
            // numToFind
            // 
            this.numToFind.Location = new System.Drawing.Point(213, 230);
            this.numToFind.Margin = new System.Windows.Forms.Padding(4);
            this.numToFind.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numToFind.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numToFind.Name = "numToFind";
            this.numToFind.Size = new System.Drawing.Size(67, 26);
            this.numToFind.TabIndex = 12;
            this.numToFind.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numToFind.ValueChanged += new System.EventHandler(this.numToFind_ValueChanged);
            this.numToFind.Enter += new System.EventHandler(this.textBox_Enter);
            this.numToFind.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // PatternMatchingParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.numToleranceY);
            this.Controls.Add(this.numToleranceX);
            this.Controls.Add(this.labelTolerance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.useAllMatching);
            this.Controls.Add(this.useWholeImage);
            this.Controls.Add(this.centerOffset);
            this.Controls.Add(this.maxScale);
            this.Controls.Add(this.maxAngle);
            this.Controls.Add(this.minScale);
            this.Controls.Add(this.minAngle);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.labelAngle);
            this.Controls.Add(this.labelScaleMax);
            this.Controls.Add(this.labelAngleMax);
            this.Controls.Add(this.labelScaleMin);
            this.Controls.Add(this.labelAngleMin);
            this.Controls.Add(this.patternType);
            this.Controls.Add(this.patternImageSelector);
            this.Controls.Add(this.numToFind);
            this.Controls.Add(this.matchScore);
            this.Controls.Add(this.searchRangeHeight);
            this.Controls.Add(this.searchRangeWidth);
            this.Controls.Add(this.labelNumToFind);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelH);
            this.Controls.Add(this.labelW);
            this.Controls.Add(this.editMaskButton);
            this.Controls.Add(this.deletePatternButton);
            this.Controls.Add(this.refreshPatternButton);
            this.Controls.Add(this.addPatternButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PatternMatchingParamControl";
            this.Size = new System.Drawing.Size(413, 428);
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToleranceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToleranceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numToFind)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addPatternButton;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.NumericUpDown searchRangeHeight;
        private System.Windows.Forms.NumericUpDown searchRangeWidth;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.NumericUpDown matchScore;
        private System.Windows.Forms.DataGridView patternImageSelector;
        private System.Windows.Forms.Button refreshPatternButton;
        private System.Windows.Forms.Button deletePatternButton;
        private System.Windows.Forms.Button editMaskButton;
        private System.Windows.Forms.ComboBox patternType;
        private System.Windows.Forms.DataGridViewImageColumn ColumnPatternImage;
        private System.Windows.Forms.NumericUpDown maxScale;
        private System.Windows.Forms.NumericUpDown maxAngle;
        private System.Windows.Forms.NumericUpDown minScale;
        private System.Windows.Forms.NumericUpDown minAngle;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Label labelAngle;
        private System.Windows.Forms.Label labelScaleMax;
        private System.Windows.Forms.Label labelAngleMax;
        private System.Windows.Forms.Label labelScaleMin;
        private System.Windows.Forms.Label labelAngleMin;
        private System.Windows.Forms.CheckBox centerOffset;
        private System.Windows.Forms.CheckBox useWholeImage;
        private System.Windows.Forms.NumericUpDown numToleranceY;
        private System.Windows.Forms.NumericUpDown numToleranceX;
        private System.Windows.Forms.Label labelTolerance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox useAllMatching;
        private System.Windows.Forms.Label labelNumToFind;
        private System.Windows.Forms.NumericUpDown numToFind;
    }
}