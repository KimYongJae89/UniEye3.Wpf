namespace DynMvp.Data.Forms
{
    partial class BarcodeReaderParamControl
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
            this.labelDesiredString = new System.Windows.Forms.Label();
            this.desiredString = new System.Windows.Forms.TextBox();
            this.desiredNum = new System.Windows.Forms.NumericUpDown();
            this.labelDesiredNum = new System.Windows.Forms.Label();
            this.labelBarcodeType = new System.Windows.Forms.Label();
            this.barcodeTypeList = new System.Windows.Forms.ListBox();
            this.deleteBarcodeTypeButton = new System.Windows.Forms.Button();
            this.addBarcodeTypeButton = new System.Windows.Forms.Button();
            this.comboBoxBarcodeType = new System.Windows.Forms.ComboBox();
            this.minArea = new System.Windows.Forms.NumericUpDown();
            this.labelMaxArea = new System.Windows.Forms.Label();
            this.labelMinArea = new System.Windows.Forms.Label();
            this.useAreaFilter = new System.Windows.Forms.CheckBox();
            this.maxArea = new System.Windows.Forms.NumericUpDown();
            this.labelMorphology = new System.Windows.Forms.Label();
            this.groupBoxAlign = new System.Windows.Forms.GroupBox();
            this.rangeThresholdTop = new System.Windows.Forms.NumericUpDown();
            this.labelRangeTop = new System.Windows.Forms.Label();
            this.rangeThresholdRight = new System.Windows.Forms.NumericUpDown();
            this.labelRangeRight = new System.Windows.Forms.Label();
            this.labelRangeBottom = new System.Windows.Forms.Label();
            this.rangeThresholdBottom = new System.Windows.Forms.NumericUpDown();
            this.rangeThresholdLeft = new System.Windows.Forms.NumericUpDown();
            this.labelRangeLeft = new System.Windows.Forms.Label();
            this.labelSearchRange = new System.Windows.Forms.Label();
            this.searchRangeHeight = new System.Windows.Forms.NumericUpDown();
            this.offsetRange = new System.Windows.Forms.CheckBox();
            this.fiducialProbe = new System.Windows.Forms.CheckBox();
            this.labelW = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.searchRangeWidth = new System.Windows.Forms.NumericUpDown();
            this.groupBoxBlobing = new System.Windows.Forms.GroupBox();
            this.thresholdList = new System.Windows.Forms.ListBox();
            this.deleteThresholdButton = new System.Windows.Forms.Button();
            this.thresholdPercent = new System.Windows.Forms.NumericUpDown();
            this.closeNum = new System.Windows.Forms.NumericUpDown();
            this.addThresholdButton = new System.Windows.Forms.Button();
            this.labelClose = new System.Windows.Forms.Label();
            this.labelThresholdPercent = new System.Windows.Forms.Label();
            this.buttonStringInsert = new System.Windows.Forms.Button();
            this.useBlobing = new System.Windows.Forms.CheckBox();
            this.timeoutTime = new System.Windows.Forms.NumericUpDown();
            this.labelTimeout = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.desiredNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxArea)).BeginInit();
            this.groupBoxAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).BeginInit();
            this.groupBoxBlobing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutTime)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDesiredString
            // 
            this.labelDesiredString.AutoSize = true;
            this.labelDesiredString.Location = new System.Drawing.Point(4, 107);
            this.labelDesiredString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDesiredString.Name = "labelDesiredString";
            this.labelDesiredString.Size = new System.Drawing.Size(110, 20);
            this.labelDesiredString.TabIndex = 4;
            this.labelDesiredString.Text = "Desired String";
            // 
            // desiredString
            // 
            this.desiredString.Location = new System.Drawing.Point(121, 104);
            this.desiredString.Name = "desiredString";
            this.desiredString.Size = new System.Drawing.Size(153, 26);
            this.desiredString.TabIndex = 5;
            this.desiredString.TextChanged += new System.EventHandler(this.desiredString_TextChanged);
            // 
            // desiredNum
            // 
            this.desiredNum.Location = new System.Drawing.Point(286, 73);
            this.desiredNum.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.desiredNum.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.desiredNum.Name = "desiredNum";
            this.desiredNum.Size = new System.Drawing.Size(63, 26);
            this.desiredNum.TabIndex = 7;
            this.desiredNum.ValueChanged += new System.EventHandler(this.desiredNum_ValueChanged);
            // 
            // labelDesiredNum
            // 
            this.labelDesiredNum.AutoSize = true;
            this.labelDesiredNum.Location = new System.Drawing.Point(4, 75);
            this.labelDesiredNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDesiredNum.Name = "labelDesiredNum";
            this.labelDesiredNum.Size = new System.Drawing.Size(101, 20);
            this.labelDesiredNum.TabIndex = 6;
            this.labelDesiredNum.Text = "Desired Num";
            // 
            // labelBarcodeType
            // 
            this.labelBarcodeType.AutoSize = true;
            this.labelBarcodeType.Location = new System.Drawing.Point(4, 9);
            this.labelBarcodeType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBarcodeType.Name = "labelBarcodeType";
            this.labelBarcodeType.Size = new System.Drawing.Size(107, 20);
            this.labelBarcodeType.TabIndex = 30;
            this.labelBarcodeType.Text = "Barcode Type";
            // 
            // barcodeTypeList
            // 
            this.barcodeTypeList.FormattingEnabled = true;
            this.barcodeTypeList.ItemHeight = 20;
            this.barcodeTypeList.Location = new System.Drawing.Point(223, 9);
            this.barcodeTypeList.Name = "barcodeTypeList";
            this.barcodeTypeList.Size = new System.Drawing.Size(125, 44);
            this.barcodeTypeList.TabIndex = 44;
            // 
            // deleteBarcodeTypeButton
            // 
            this.deleteBarcodeTypeButton.Location = new System.Drawing.Point(143, 39);
            this.deleteBarcodeTypeButton.Name = "deleteBarcodeTypeButton";
            this.deleteBarcodeTypeButton.Size = new System.Drawing.Size(74, 28);
            this.deleteBarcodeTypeButton.TabIndex = 42;
            this.deleteBarcodeTypeButton.Text = "Delete";
            this.deleteBarcodeTypeButton.UseVisualStyleBackColor = true;
            this.deleteBarcodeTypeButton.Click += new System.EventHandler(this.DeleteBarcodeTypeButton_Click);
            // 
            // addBarcodeTypeButton
            // 
            this.addBarcodeTypeButton.Location = new System.Drawing.Point(143, 5);
            this.addBarcodeTypeButton.Name = "addBarcodeTypeButton";
            this.addBarcodeTypeButton.Size = new System.Drawing.Size(74, 28);
            this.addBarcodeTypeButton.TabIndex = 43;
            this.addBarcodeTypeButton.Text = "Add";
            this.addBarcodeTypeButton.UseVisualStyleBackColor = true;
            this.addBarcodeTypeButton.Click += new System.EventHandler(this.addBarcodeTypeButton_Click);
            // 
            // comboBoxBarcodeType
            // 
            this.comboBoxBarcodeType.FormattingEnabled = true;
            this.comboBoxBarcodeType.Location = new System.Drawing.Point(7, 40);
            this.comboBoxBarcodeType.Name = "comboBoxBarcodeType";
            this.comboBoxBarcodeType.Size = new System.Drawing.Size(108, 28);
            this.comboBoxBarcodeType.TabIndex = 45;
            // 
            // minArea
            // 
            this.minArea.Enabled = false;
            this.minArea.Location = new System.Drawing.Point(229, 161);
            this.minArea.Margin = new System.Windows.Forms.Padding(4);
            this.minArea.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.minArea.Name = "minArea";
            this.minArea.Size = new System.Drawing.Size(104, 26);
            this.minArea.TabIndex = 27;
            this.minArea.ValueChanged += new System.EventHandler(this.minArea_ValueChanged);
            // 
            // labelMaxArea
            // 
            this.labelMaxArea.AutoSize = true;
            this.labelMaxArea.Enabled = false;
            this.labelMaxArea.Location = new System.Drawing.Point(149, 190);
            this.labelMaxArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxArea.Name = "labelMaxArea";
            this.labelMaxArea.Size = new System.Drawing.Size(76, 20);
            this.labelMaxArea.TabIndex = 28;
            this.labelMaxArea.Text = "Max Area";
            this.labelMaxArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMinArea
            // 
            this.labelMinArea.AutoSize = true;
            this.labelMinArea.Enabled = false;
            this.labelMinArea.Location = new System.Drawing.Point(149, 163);
            this.labelMinArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinArea.Name = "labelMinArea";
            this.labelMinArea.Size = new System.Drawing.Size(72, 20);
            this.labelMinArea.TabIndex = 26;
            this.labelMinArea.Text = "Min Area";
            this.labelMinArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // useAreaFilter
            // 
            this.useAreaFilter.AutoSize = true;
            this.useAreaFilter.Location = new System.Drawing.Point(153, 131);
            this.useAreaFilter.Margin = new System.Windows.Forms.Padding(2);
            this.useAreaFilter.Name = "useAreaFilter";
            this.useAreaFilter.Size = new System.Drawing.Size(134, 24);
            this.useAreaFilter.TabIndex = 24;
            this.useAreaFilter.Text = "Use Area Filter";
            this.useAreaFilter.UseVisualStyleBackColor = true;
            this.useAreaFilter.CheckedChanged += new System.EventHandler(this.useAreaFilter_CheckedChanged);
            // 
            // maxArea
            // 
            this.maxArea.Enabled = false;
            this.maxArea.Location = new System.Drawing.Point(229, 188);
            this.maxArea.Margin = new System.Windows.Forms.Padding(4);
            this.maxArea.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.maxArea.Name = "maxArea";
            this.maxArea.Size = new System.Drawing.Size(104, 26);
            this.maxArea.TabIndex = 29;
            this.maxArea.ValueChanged += new System.EventHandler(this.maxArea_ValueChanged);
            // 
            // labelMorphology
            // 
            this.labelMorphology.AutoSize = true;
            this.labelMorphology.Location = new System.Drawing.Point(10, 132);
            this.labelMorphology.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMorphology.Name = "labelMorphology";
            this.labelMorphology.Size = new System.Drawing.Size(91, 20);
            this.labelMorphology.TabIndex = 25;
            this.labelMorphology.Text = "Morphology";
            // 
            // groupBoxAlign
            // 
            this.groupBoxAlign.Controls.Add(this.rangeThresholdTop);
            this.groupBoxAlign.Controls.Add(this.labelRangeTop);
            this.groupBoxAlign.Controls.Add(this.rangeThresholdRight);
            this.groupBoxAlign.Controls.Add(this.labelRangeRight);
            this.groupBoxAlign.Controls.Add(this.labelRangeBottom);
            this.groupBoxAlign.Controls.Add(this.rangeThresholdBottom);
            this.groupBoxAlign.Controls.Add(this.rangeThresholdLeft);
            this.groupBoxAlign.Controls.Add(this.labelRangeLeft);
            this.groupBoxAlign.Controls.Add(this.labelSearchRange);
            this.groupBoxAlign.Controls.Add(this.searchRangeHeight);
            this.groupBoxAlign.Controls.Add(this.offsetRange);
            this.groupBoxAlign.Controls.Add(this.fiducialProbe);
            this.groupBoxAlign.Controls.Add(this.labelW);
            this.groupBoxAlign.Controls.Add(this.labelH);
            this.groupBoxAlign.Controls.Add(this.searchRangeWidth);
            this.groupBoxAlign.Location = new System.Drawing.Point(8, 165);
            this.groupBoxAlign.Name = "groupBoxAlign";
            this.groupBoxAlign.Size = new System.Drawing.Size(340, 200);
            this.groupBoxAlign.TabIndex = 46;
            this.groupBoxAlign.TabStop = false;
            this.groupBoxAlign.Text = "Align";
            // 
            // rangeThresholdTop
            // 
            this.rangeThresholdTop.Enabled = false;
            this.rangeThresholdTop.Location = new System.Drawing.Point(274, 168);
            this.rangeThresholdTop.Margin = new System.Windows.Forms.Padding(4);
            this.rangeThresholdTop.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.rangeThresholdTop.Name = "rangeThresholdTop";
            this.rangeThresholdTop.Size = new System.Drawing.Size(59, 26);
            this.rangeThresholdTop.TabIndex = 33;
            this.rangeThresholdTop.ValueChanged += new System.EventHandler(this.rangeThresholdTop_ValueChanged);
            // 
            // labelRangeTop
            // 
            this.labelRangeTop.AutoSize = true;
            this.labelRangeTop.Enabled = false;
            this.labelRangeTop.Location = new System.Drawing.Point(225, 170);
            this.labelRangeTop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRangeTop.Name = "labelRangeTop";
            this.labelRangeTop.Size = new System.Drawing.Size(36, 20);
            this.labelRangeTop.TabIndex = 32;
            this.labelRangeTop.Text = "Top";
            this.labelRangeTop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rangeThresholdRight
            // 
            this.rangeThresholdRight.Enabled = false;
            this.rangeThresholdRight.Location = new System.Drawing.Point(274, 139);
            this.rangeThresholdRight.Margin = new System.Windows.Forms.Padding(4);
            this.rangeThresholdRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.rangeThresholdRight.Name = "rangeThresholdRight";
            this.rangeThresholdRight.Size = new System.Drawing.Size(59, 26);
            this.rangeThresholdRight.TabIndex = 31;
            this.rangeThresholdRight.ValueChanged += new System.EventHandler(this.rangeThresholdRight_ValueChanged);
            // 
            // labelRangeRight
            // 
            this.labelRangeRight.AutoSize = true;
            this.labelRangeRight.Enabled = false;
            this.labelRangeRight.Location = new System.Drawing.Point(225, 141);
            this.labelRangeRight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRangeRight.Name = "labelRangeRight";
            this.labelRangeRight.Size = new System.Drawing.Size(47, 20);
            this.labelRangeRight.TabIndex = 30;
            this.labelRangeRight.Text = "Right";
            this.labelRangeRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRangeBottom
            // 
            this.labelRangeBottom.AutoSize = true;
            this.labelRangeBottom.Enabled = false;
            this.labelRangeBottom.Location = new System.Drawing.Point(91, 170);
            this.labelRangeBottom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRangeBottom.Name = "labelRangeBottom";
            this.labelRangeBottom.Size = new System.Drawing.Size(61, 20);
            this.labelRangeBottom.TabIndex = 28;
            this.labelRangeBottom.Text = "Bottom";
            this.labelRangeBottom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rangeThresholdBottom
            // 
            this.rangeThresholdBottom.Enabled = false;
            this.rangeThresholdBottom.Location = new System.Drawing.Point(153, 168);
            this.rangeThresholdBottom.Margin = new System.Windows.Forms.Padding(4);
            this.rangeThresholdBottom.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.rangeThresholdBottom.Name = "rangeThresholdBottom";
            this.rangeThresholdBottom.Size = new System.Drawing.Size(59, 26);
            this.rangeThresholdBottom.TabIndex = 29;
            this.rangeThresholdBottom.ValueChanged += new System.EventHandler(this.rangeThresholdBottom_ValueChanged);
            // 
            // rangeThresholdLeft
            // 
            this.rangeThresholdLeft.Enabled = false;
            this.rangeThresholdLeft.Location = new System.Drawing.Point(153, 139);
            this.rangeThresholdLeft.Margin = new System.Windows.Forms.Padding(4);
            this.rangeThresholdLeft.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.rangeThresholdLeft.Name = "rangeThresholdLeft";
            this.rangeThresholdLeft.Size = new System.Drawing.Size(59, 26);
            this.rangeThresholdLeft.TabIndex = 27;
            this.rangeThresholdLeft.ValueChanged += new System.EventHandler(this.rangeThresholdLeft_ValueChanged);
            // 
            // labelRangeLeft
            // 
            this.labelRangeLeft.AutoSize = true;
            this.labelRangeLeft.Enabled = false;
            this.labelRangeLeft.Location = new System.Drawing.Point(91, 141);
            this.labelRangeLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRangeLeft.Name = "labelRangeLeft";
            this.labelRangeLeft.Size = new System.Drawing.Size(37, 20);
            this.labelRangeLeft.TabIndex = 26;
            this.labelRangeLeft.Text = "Left";
            this.labelRangeLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSearchRange
            // 
            this.labelSearchRange.AutoSize = true;
            this.labelSearchRange.Enabled = false;
            this.labelSearchRange.Location = new System.Drawing.Point(221, 22);
            this.labelSearchRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSearchRange.Name = "labelSearchRange";
            this.labelSearchRange.Size = new System.Drawing.Size(112, 20);
            this.labelSearchRange.TabIndex = 25;
            this.labelSearchRange.Text = "Search Range";
            // 
            // searchRangeHeight
            // 
            this.searchRangeHeight.Location = new System.Drawing.Point(274, 75);
            this.searchRangeHeight.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.searchRangeHeight.Name = "searchRangeHeight";
            this.searchRangeHeight.Size = new System.Drawing.Size(59, 26);
            this.searchRangeHeight.TabIndex = 29;
            this.searchRangeHeight.ValueChanged += new System.EventHandler(this.searchRangeHeight_ValueChanged);
            // 
            // offsetRange
            // 
            this.offsetRange.AutoSize = true;
            this.offsetRange.Location = new System.Drawing.Point(11, 104);
            this.offsetRange.Margin = new System.Windows.Forms.Padding(2);
            this.offsetRange.Name = "offsetRange";
            this.offsetRange.Size = new System.Drawing.Size(124, 24);
            this.offsetRange.TabIndex = 24;
            this.offsetRange.Text = "Offset Range";
            this.offsetRange.UseVisualStyleBackColor = true;
            this.offsetRange.CheckedChanged += new System.EventHandler(this.useRange_CheckedChanged);
            // 
            // fiducialProbe
            // 
            this.fiducialProbe.AutoSize = true;
            this.fiducialProbe.Location = new System.Drawing.Point(11, 24);
            this.fiducialProbe.Margin = new System.Windows.Forms.Padding(2);
            this.fiducialProbe.Name = "fiducialProbe";
            this.fiducialProbe.Size = new System.Drawing.Size(128, 24);
            this.fiducialProbe.TabIndex = 24;
            this.fiducialProbe.Text = "Fiducial Probe";
            this.fiducialProbe.ThreeState = true;
            this.fiducialProbe.UseVisualStyleBackColor = true;
            this.fiducialProbe.CheckedChanged += new System.EventHandler(this.fiducialProbe_CheckedChanged);
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Enabled = false;
            this.labelW.Location = new System.Drawing.Point(232, 50);
            this.labelW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(24, 20);
            this.labelW.TabIndex = 26;
            this.labelW.Text = "W";
            this.labelW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Enabled = false;
            this.labelH.Location = new System.Drawing.Point(235, 77);
            this.labelH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(21, 20);
            this.labelH.TabIndex = 28;
            this.labelH.Text = "H";
            this.labelH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchRangeWidth
            // 
            this.searchRangeWidth.Location = new System.Drawing.Point(274, 48);
            this.searchRangeWidth.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.searchRangeWidth.Name = "searchRangeWidth";
            this.searchRangeWidth.Size = new System.Drawing.Size(59, 26);
            this.searchRangeWidth.TabIndex = 27;
            this.searchRangeWidth.ValueChanged += new System.EventHandler(this.searchRangeWidth_ValueChanged);
            // 
            // groupBoxBlobing
            // 
            this.groupBoxBlobing.Controls.Add(this.thresholdList);
            this.groupBoxBlobing.Controls.Add(this.deleteThresholdButton);
            this.groupBoxBlobing.Controls.Add(this.thresholdPercent);
            this.groupBoxBlobing.Controls.Add(this.closeNum);
            this.groupBoxBlobing.Controls.Add(this.addThresholdButton);
            this.groupBoxBlobing.Controls.Add(this.labelClose);
            this.groupBoxBlobing.Controls.Add(this.labelThresholdPercent);
            this.groupBoxBlobing.Controls.Add(this.labelMorphology);
            this.groupBoxBlobing.Controls.Add(this.maxArea);
            this.groupBoxBlobing.Controls.Add(this.useAreaFilter);
            this.groupBoxBlobing.Controls.Add(this.labelMinArea);
            this.groupBoxBlobing.Controls.Add(this.labelMaxArea);
            this.groupBoxBlobing.Controls.Add(this.minArea);
            this.groupBoxBlobing.Location = new System.Drawing.Point(8, 413);
            this.groupBoxBlobing.Name = "groupBoxBlobing";
            this.groupBoxBlobing.Size = new System.Drawing.Size(340, 221);
            this.groupBoxBlobing.TabIndex = 47;
            this.groupBoxBlobing.TabStop = false;
            this.groupBoxBlobing.Text = "Blobing";
            // 
            // thresholdList
            // 
            this.thresholdList.FormattingEnabled = true;
            this.thresholdList.ItemHeight = 20;
            this.thresholdList.Location = new System.Drawing.Point(83, 62);
            this.thresholdList.Name = "thresholdList";
            this.thresholdList.Size = new System.Drawing.Size(125, 64);
            this.thresholdList.TabIndex = 56;
            // 
            // deleteThresholdButton
            // 
            this.deleteThresholdButton.Location = new System.Drawing.Point(7, 95);
            this.deleteThresholdButton.Name = "deleteThresholdButton";
            this.deleteThresholdButton.Size = new System.Drawing.Size(74, 28);
            this.deleteThresholdButton.TabIndex = 54;
            this.deleteThresholdButton.Text = "Delete";
            this.deleteThresholdButton.UseVisualStyleBackColor = true;
            this.deleteThresholdButton.Click += new System.EventHandler(this.DeleteThresholdButton_Click);
            // 
            // thresholdPercent
            // 
            this.thresholdPercent.Location = new System.Drawing.Point(148, 29);
            this.thresholdPercent.Margin = new System.Windows.Forms.Padding(4);
            this.thresholdPercent.Name = "thresholdPercent";
            this.thresholdPercent.Size = new System.Drawing.Size(59, 26);
            this.thresholdPercent.TabIndex = 51;
            this.thresholdPercent.ValueChanged += new System.EventHandler(this.thresholdPercent_ValueChanged);
            // 
            // closeNum
            // 
            this.closeNum.Location = new System.Drawing.Point(67, 162);
            this.closeNum.Margin = new System.Windows.Forms.Padding(4);
            this.closeNum.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.closeNum.Name = "closeNum";
            this.closeNum.Size = new System.Drawing.Size(59, 26);
            this.closeNum.TabIndex = 50;
            this.closeNum.ValueChanged += new System.EventHandler(this.closeNum_ValueChanged);
            // 
            // addThresholdButton
            // 
            this.addThresholdButton.Location = new System.Drawing.Point(7, 62);
            this.addThresholdButton.Name = "addThresholdButton";
            this.addThresholdButton.Size = new System.Drawing.Size(74, 28);
            this.addThresholdButton.TabIndex = 55;
            this.addThresholdButton.Text = "Add";
            this.addThresholdButton.UseVisualStyleBackColor = true;
            this.addThresholdButton.Click += new System.EventHandler(this.addThresholdButton_Click);
            // 
            // labelClose
            // 
            this.labelClose.AutoSize = true;
            this.labelClose.Location = new System.Drawing.Point(10, 164);
            this.labelClose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(49, 20);
            this.labelClose.TabIndex = 49;
            this.labelClose.Text = "Close";
            this.labelClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelThresholdPercent
            // 
            this.labelThresholdPercent.AutoSize = true;
            this.labelThresholdPercent.Location = new System.Drawing.Point(10, 31);
            this.labelThresholdPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThresholdPercent.Name = "labelThresholdPercent";
            this.labelThresholdPercent.Size = new System.Drawing.Size(138, 20);
            this.labelThresholdPercent.TabIndex = 48;
            this.labelThresholdPercent.Text = "Threshold Percent";
            // 
            // buttonStringInsert
            // 
            this.buttonStringInsert.Location = new System.Drawing.Point(286, 104);
            this.buttonStringInsert.Name = "buttonStringInsert";
            this.buttonStringInsert.Size = new System.Drawing.Size(62, 26);
            this.buttonStringInsert.TabIndex = 47;
            this.buttonStringInsert.Text = "Insert";
            this.buttonStringInsert.UseVisualStyleBackColor = true;
            this.buttonStringInsert.Click += new System.EventHandler(this.buttonStringInsert_Click);
            // 
            // useBlobing
            // 
            this.useBlobing.AutoSize = true;
            this.useBlobing.Enabled = false;
            this.useBlobing.Location = new System.Drawing.Point(15, 384);
            this.useBlobing.Margin = new System.Windows.Forms.Padding(2);
            this.useBlobing.Name = "useBlobing";
            this.useBlobing.Size = new System.Drawing.Size(114, 24);
            this.useBlobing.TabIndex = 57;
            this.useBlobing.Text = "Use Blobing";
            this.useBlobing.UseVisualStyleBackColor = true;
            this.useBlobing.CheckedChanged += new System.EventHandler(this.useBlobing_CheckedChanged);
            // 
            // timeoutTime
            // 
            this.timeoutTime.Enabled = false;
            this.timeoutTime.Location = new System.Drawing.Point(121, 140);
            this.timeoutTime.Margin = new System.Windows.Forms.Padding(4);
            this.timeoutTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.timeoutTime.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.timeoutTime.Name = "timeoutTime";
            this.timeoutTime.Size = new System.Drawing.Size(150, 26);
            this.timeoutTime.TabIndex = 59;
            this.timeoutTime.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.timeoutTime.ValueChanged += new System.EventHandler(this.timeoutTime_ValueChanged);
            // 
            // labelTimeout
            // 
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.Location = new System.Drawing.Point(5, 142);
            this.labelTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(70, 20);
            this.labelTimeout.TabIndex = 58;
            this.labelTimeout.Text = "Time out";
            // 
            // BarcodeReaderParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.timeoutTime);
            this.Controls.Add(this.labelTimeout);
            this.Controls.Add(this.useBlobing);
            this.Controls.Add(this.groupBoxBlobing);
            this.Controls.Add(this.buttonStringInsert);
            this.Controls.Add(this.groupBoxAlign);
            this.Controls.Add(this.comboBoxBarcodeType);
            this.Controls.Add(this.barcodeTypeList);
            this.Controls.Add(this.deleteBarcodeTypeButton);
            this.Controls.Add(this.addBarcodeTypeButton);
            this.Controls.Add(this.labelBarcodeType);
            this.Controls.Add(this.desiredNum);
            this.Controls.Add(this.labelDesiredNum);
            this.Controls.Add(this.desiredString);
            this.Controls.Add(this.labelDesiredString);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BarcodeReaderParamControl";
            this.Size = new System.Drawing.Size(361, 650);
            ((System.ComponentModel.ISupportInitialize)(this.desiredNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxArea)).EndInit();
            this.groupBoxAlign.ResumeLayout(false);
            this.groupBoxAlign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeThresholdLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).EndInit();
            this.groupBoxBlobing.ResumeLayout(false);
            this.groupBoxBlobing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDesiredString;
        private System.Windows.Forms.TextBox desiredString;
        private System.Windows.Forms.NumericUpDown desiredNum;
        private System.Windows.Forms.Label labelDesiredNum;
        private System.Windows.Forms.Label labelBarcodeType;
        private System.Windows.Forms.ListBox barcodeTypeList;
        private System.Windows.Forms.Button deleteBarcodeTypeButton;
        private System.Windows.Forms.Button addBarcodeTypeButton;
        private System.Windows.Forms.ComboBox comboBoxBarcodeType;
        private System.Windows.Forms.NumericUpDown minArea;
        private System.Windows.Forms.Label labelMaxArea;
        private System.Windows.Forms.Label labelMinArea;
        private System.Windows.Forms.CheckBox useAreaFilter;
        private System.Windows.Forms.NumericUpDown maxArea;
        private System.Windows.Forms.Label labelMorphology;
        private System.Windows.Forms.GroupBox groupBoxAlign;
        private System.Windows.Forms.NumericUpDown rangeThresholdTop;
        private System.Windows.Forms.Label labelRangeTop;
        private System.Windows.Forms.NumericUpDown rangeThresholdRight;
        private System.Windows.Forms.Label labelRangeRight;
        private System.Windows.Forms.Label labelRangeBottom;
        private System.Windows.Forms.NumericUpDown rangeThresholdBottom;
        private System.Windows.Forms.NumericUpDown rangeThresholdLeft;
        private System.Windows.Forms.Label labelRangeLeft;
        private System.Windows.Forms.Label labelSearchRange;
        private System.Windows.Forms.NumericUpDown searchRangeHeight;
        private System.Windows.Forms.CheckBox offsetRange;
        private System.Windows.Forms.CheckBox fiducialProbe;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.NumericUpDown searchRangeWidth;
        private System.Windows.Forms.GroupBox groupBoxBlobing;
        private System.Windows.Forms.Label labelThresholdPercent;
        private System.Windows.Forms.NumericUpDown closeNum;
        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.Button buttonStringInsert;
        private System.Windows.Forms.NumericUpDown thresholdPercent;
        private System.Windows.Forms.ListBox thresholdList;
        private System.Windows.Forms.Button deleteThresholdButton;
        private System.Windows.Forms.Button addThresholdButton;
        private System.Windows.Forms.CheckBox useBlobing;
        private System.Windows.Forms.NumericUpDown timeoutTime;
        private System.Windows.Forms.Label labelTimeout;
    }
}