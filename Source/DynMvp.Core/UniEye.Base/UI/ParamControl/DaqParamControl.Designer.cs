namespace UniEye.Base.UI.ParamControl
{
    partial class DaqParamControl
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
            this.daqSelector = new System.Windows.Forms.ComboBox();
            this.labelDaqChannel = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.lowerValue = new System.Windows.Forms.TextBox();
            this.upperValue = new System.Windows.Forms.TextBox();
            this.labelTilda = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.inverseResult = new System.Windows.Forms.CheckBox();
            this.modelVerification = new System.Windows.Forms.CheckBox();
            this.labelNumSample = new System.Windows.Forms.Label();
            this.numSample = new System.Windows.Forms.NumericUpDown();
            this.localScaleFactor = new System.Windows.Forms.TextBox();
            this.useLocalScaleFactor = new System.Windows.Forms.CheckBox();
            this.valueOffset = new System.Windows.Forms.TextBox();
            this.labelValueOffset = new System.Windows.Forms.Label();
            this.measureType = new System.Windows.Forms.ComboBox();
            this.labelMeasureType = new System.Windows.Forms.Label();
            this.panelMeasureParam = new System.Windows.Forms.Panel();
            this.filterType = new System.Windows.Forms.ComboBox();
            this.labelFilterType = new System.Windows.Forms.Label();
            this.panelProbeSelector = new System.Windows.Forms.Panel();
            this.target2 = new System.Windows.Forms.ComboBox();
            this.labelTarget2 = new System.Windows.Forms.Label();
            this.target1 = new System.Windows.Forms.ComboBox();
            this.labelTarget1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numSample)).BeginInit();
            this.panelMeasureParam.SuspendLayout();
            this.panelProbeSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // daqSelector
            // 
            this.daqSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.daqSelector.FormattingEnabled = true;
            this.daqSelector.Location = new System.Drawing.Point(131, 7);
            this.daqSelector.Margin = new System.Windows.Forms.Padding(4);
            this.daqSelector.Name = "daqSelector";
            this.daqSelector.Size = new System.Drawing.Size(148, 28);
            this.daqSelector.TabIndex = 1;
            this.daqSelector.SelectedIndexChanged += new System.EventHandler(this.portSelector_SelectedIndexChanged);
            // 
            // labelDaqChannel
            // 
            this.labelDaqChannel.AutoSize = true;
            this.labelDaqChannel.Location = new System.Drawing.Point(12, 9);
            this.labelDaqChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDaqChannel.Name = "labelDaqChannel";
            this.labelDaqChannel.Size = new System.Drawing.Size(107, 20);
            this.labelDaqChannel.TabIndex = 0;
            this.labelDaqChannel.Text = "DAQ Channel";
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.labelRange.Location = new System.Drawing.Point(12, 201);
            this.labelRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(111, 20);
            this.labelRange.TabIndex = 4;
            this.labelRange.Text = "Normal Range";
            // 
            // lowerValue
            // 
            this.lowerValue.Location = new System.Drawing.Point(131, 199);
            this.lowerValue.Margin = new System.Windows.Forms.Padding(4);
            this.lowerValue.Name = "lowerValue";
            this.lowerValue.Size = new System.Drawing.Size(64, 26);
            this.lowerValue.TabIndex = 5;
            this.lowerValue.TextChanged += new System.EventHandler(this.lowerValue_TextChanged);
            this.lowerValue.Enter += new System.EventHandler(this.textBox_Enter);
            this.lowerValue.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // upperValue
            // 
            this.upperValue.Location = new System.Drawing.Point(215, 199);
            this.upperValue.Margin = new System.Windows.Forms.Padding(4);
            this.upperValue.Name = "upperValue";
            this.upperValue.Size = new System.Drawing.Size(64, 26);
            this.upperValue.TabIndex = 7;
            this.upperValue.TextChanged += new System.EventHandler(this.upperValue_TextChanged);
            this.upperValue.Enter += new System.EventHandler(this.textBox_Enter);
            this.upperValue.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(196, 201);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(18, 20);
            this.labelTilda.TabIndex = 6;
            this.labelTilda.Text = "~";
            // 
            // inverseResult
            // 
            this.inverseResult.AutoSize = true;
            this.inverseResult.Location = new System.Drawing.Point(16, 230);
            this.inverseResult.Margin = new System.Windows.Forms.Padding(2);
            this.inverseResult.Name = "inverseResult";
            this.inverseResult.Size = new System.Drawing.Size(130, 24);
            this.inverseResult.TabIndex = 8;
            this.inverseResult.Text = "Inverse Result";
            this.inverseResult.UseVisualStyleBackColor = true;
            this.inverseResult.CheckedChanged += new System.EventHandler(this.inverseResult_CheckedChanged);
            // 
            // modelVerification
            // 
            this.modelVerification.AutoSize = true;
            this.modelVerification.Location = new System.Drawing.Point(16, 256);
            this.modelVerification.Margin = new System.Windows.Forms.Padding(2);
            this.modelVerification.Name = "modelVerification";
            this.modelVerification.Size = new System.Drawing.Size(158, 24);
            this.modelVerification.TabIndex = 9;
            this.modelVerification.Text = "Use Model Verifier";
            this.modelVerification.UseVisualStyleBackColor = true;
            this.modelVerification.CheckedChanged += new System.EventHandler(this.modelVerification_CheckedChanged);
            // 
            // labelNumSample
            // 
            this.labelNumSample.AutoSize = true;
            this.labelNumSample.Location = new System.Drawing.Point(13, 2);
            this.labelNumSample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumSample.Name = "labelNumSample";
            this.labelNumSample.Size = new System.Drawing.Size(100, 20);
            this.labelNumSample.TabIndex = 2;
            this.labelNumSample.Text = "Num Sample";
            // 
            // numSample
            // 
            this.numSample.Location = new System.Drawing.Point(215, 3);
            this.numSample.Margin = new System.Windows.Forms.Padding(2);
            this.numSample.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numSample.Name = "numSample";
            this.numSample.Size = new System.Drawing.Size(63, 26);
            this.numSample.TabIndex = 10;
            this.numSample.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSample.ValueChanged += new System.EventHandler(this.numSample_ValueChanged);
            this.numSample.Enter += new System.EventHandler(this.textBox_Enter);
            this.numSample.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // localScaleFactor
            // 
            this.localScaleFactor.Location = new System.Drawing.Point(215, 67);
            this.localScaleFactor.Margin = new System.Windows.Forms.Padding(4);
            this.localScaleFactor.Name = "localScaleFactor";
            this.localScaleFactor.Size = new System.Drawing.Size(64, 26);
            this.localScaleFactor.TabIndex = 5;
            this.localScaleFactor.TextChanged += new System.EventHandler(this.localScaleFactor_TextChanged);
            this.localScaleFactor.Enter += new System.EventHandler(this.textBox_Enter);
            this.localScaleFactor.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // useLocalScaleFactor
            // 
            this.useLocalScaleFactor.AutoSize = true;
            this.useLocalScaleFactor.Location = new System.Drawing.Point(16, 69);
            this.useLocalScaleFactor.Margin = new System.Windows.Forms.Padding(2);
            this.useLocalScaleFactor.Name = "useLocalScaleFactor";
            this.useLocalScaleFactor.Size = new System.Drawing.Size(118, 24);
            this.useLocalScaleFactor.TabIndex = 11;
            this.useLocalScaleFactor.Text = "Scale Factor";
            this.useLocalScaleFactor.UseVisualStyleBackColor = true;
            this.useLocalScaleFactor.CheckedChanged += new System.EventHandler(this.useScaleValue_CheckedChanged);
            // 
            // valueOffset
            // 
            this.valueOffset.Location = new System.Drawing.Point(215, 93);
            this.valueOffset.Margin = new System.Windows.Forms.Padding(4);
            this.valueOffset.Name = "valueOffset";
            this.valueOffset.Size = new System.Drawing.Size(64, 26);
            this.valueOffset.TabIndex = 13;
            this.valueOffset.TextChanged += new System.EventHandler(this.valueOffset_TextChanged);
            this.valueOffset.Enter += new System.EventHandler(this.textBox_Enter);
            this.valueOffset.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelValueOffset
            // 
            this.labelValueOffset.AutoSize = true;
            this.labelValueOffset.Location = new System.Drawing.Point(29, 93);
            this.labelValueOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelValueOffset.Name = "labelValueOffset";
            this.labelValueOffset.Size = new System.Drawing.Size(98, 20);
            this.labelValueOffset.TabIndex = 12;
            this.labelValueOffset.Text = "Value Offset";
            this.labelValueOffset.Click += new System.EventHandler(this.labelValueOffset_Click);
            // 
            // measureType
            // 
            this.measureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measureType.FormattingEnabled = true;
            this.measureType.Items.AddRange(new object[] {
            "Absolute",
            "Difference",
            "Distance"});
            this.measureType.Location = new System.Drawing.Point(131, 37);
            this.measureType.Margin = new System.Windows.Forms.Padding(4);
            this.measureType.Name = "measureType";
            this.measureType.Size = new System.Drawing.Size(148, 28);
            this.measureType.TabIndex = 1;
            this.measureType.SelectedIndexChanged += new System.EventHandler(this.measureType_SelectedIndexChanged);
            // 
            // labelMeasureType
            // 
            this.labelMeasureType.AutoSize = true;
            this.labelMeasureType.Location = new System.Drawing.Point(12, 39);
            this.labelMeasureType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMeasureType.Name = "labelMeasureType";
            this.labelMeasureType.Size = new System.Drawing.Size(109, 20);
            this.labelMeasureType.TabIndex = 0;
            this.labelMeasureType.Text = "Measure Type";
            // 
            // panelMeasureParam
            // 
            this.panelMeasureParam.Controls.Add(this.filterType);
            this.panelMeasureParam.Controls.Add(this.labelFilterType);
            this.panelMeasureParam.Controls.Add(this.labelNumSample);
            this.panelMeasureParam.Controls.Add(this.valueOffset);
            this.panelMeasureParam.Controls.Add(this.localScaleFactor);
            this.panelMeasureParam.Controls.Add(this.labelValueOffset);
            this.panelMeasureParam.Controls.Add(this.numSample);
            this.panelMeasureParam.Controls.Add(this.useLocalScaleFactor);
            this.panelMeasureParam.Location = new System.Drawing.Point(2, 67);
            this.panelMeasureParam.Margin = new System.Windows.Forms.Padding(2);
            this.panelMeasureParam.Name = "panelMeasureParam";
            this.panelMeasureParam.Size = new System.Drawing.Size(286, 126);
            this.panelMeasureParam.TabIndex = 14;
            // 
            // filterType
            // 
            this.filterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterType.FormattingEnabled = true;
            this.filterType.Items.AddRange(new object[] {
            "Average",
            "Median"});
            this.filterType.Location = new System.Drawing.Point(131, 33);
            this.filterType.Margin = new System.Windows.Forms.Padding(4);
            this.filterType.Name = "filterType";
            this.filterType.Size = new System.Drawing.Size(148, 28);
            this.filterType.TabIndex = 16;
            this.filterType.SelectedIndexChanged += new System.EventHandler(this.filterType_SelectedIndexChanged);
            // 
            // labelFilterType
            // 
            this.labelFilterType.AutoSize = true;
            this.labelFilterType.Location = new System.Drawing.Point(13, 39);
            this.labelFilterType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFilterType.Name = "labelFilterType";
            this.labelFilterType.Size = new System.Drawing.Size(82, 20);
            this.labelFilterType.TabIndex = 14;
            this.labelFilterType.Text = "Filter Type";
            // 
            // panelProbeSelector
            // 
            this.panelProbeSelector.Controls.Add(this.target2);
            this.panelProbeSelector.Controls.Add(this.labelTarget2);
            this.panelProbeSelector.Controls.Add(this.target1);
            this.panelProbeSelector.Controls.Add(this.labelTarget1);
            this.panelProbeSelector.Location = new System.Drawing.Point(0, 289);
            this.panelProbeSelector.Margin = new System.Windows.Forms.Padding(2);
            this.panelProbeSelector.Name = "panelProbeSelector";
            this.panelProbeSelector.Size = new System.Drawing.Size(288, 63);
            this.panelProbeSelector.TabIndex = 15;
            this.panelProbeSelector.Visible = false;
            // 
            // target2
            // 
            this.target2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.target2.FormattingEnabled = true;
            this.target2.Items.AddRange(new object[] {
            ""});
            this.target2.Location = new System.Drawing.Point(131, 34);
            this.target2.Margin = new System.Windows.Forms.Padding(4);
            this.target2.Name = "target2";
            this.target2.Size = new System.Drawing.Size(148, 28);
            this.target2.TabIndex = 1;
            this.target2.SelectedIndexChanged += new System.EventHandler(this.target2_SelectedIndexChanged);
            // 
            // labelTarget2
            // 
            this.labelTarget2.AutoSize = true;
            this.labelTarget2.Location = new System.Drawing.Point(12, 37);
            this.labelTarget2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTarget2.Name = "labelTarget2";
            this.labelTarget2.Size = new System.Drawing.Size(64, 20);
            this.labelTarget2.TabIndex = 0;
            this.labelTarget2.Text = "Target2";
            // 
            // target1
            // 
            this.target1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.target1.FormattingEnabled = true;
            this.target1.Items.AddRange(new object[] {
            ""});
            this.target1.Location = new System.Drawing.Point(131, 4);
            this.target1.Margin = new System.Windows.Forms.Padding(4);
            this.target1.Name = "target1";
            this.target1.Size = new System.Drawing.Size(148, 28);
            this.target1.TabIndex = 1;
            this.target1.SelectedIndexChanged += new System.EventHandler(this.target1_SelectedIndexChanged);
            // 
            // labelTarget1
            // 
            this.labelTarget1.AutoSize = true;
            this.labelTarget1.Location = new System.Drawing.Point(12, 7);
            this.labelTarget1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTarget1.Name = "labelTarget1";
            this.labelTarget1.Size = new System.Drawing.Size(64, 20);
            this.labelTarget1.TabIndex = 0;
            this.labelTarget1.Text = "Target1";
            // 
            // DaqParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelProbeSelector);
            this.Controls.Add(this.panelMeasureParam);
            this.Controls.Add(this.modelVerification);
            this.Controls.Add(this.inverseResult);
            this.Controls.Add(this.upperValue);
            this.Controls.Add(this.lowerValue);
            this.Controls.Add(this.labelMeasureType);
            this.Controls.Add(this.labelDaqChannel);
            this.Controls.Add(this.measureType);
            this.Controls.Add(this.daqSelector);
            this.Controls.Add(this.labelTilda);
            this.Controls.Add(this.labelRange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DaqParamControl";
            this.Size = new System.Drawing.Size(290, 365);
            ((System.ComponentModel.ISupportInitialize)(this.numSample)).EndInit();
            this.panelMeasureParam.ResumeLayout(false);
            this.panelMeasureParam.PerformLayout();
            this.panelProbeSelector.ResumeLayout(false);
            this.panelProbeSelector.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox daqSelector;
        private System.Windows.Forms.Label labelDaqChannel;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.TextBox lowerValue;
        private System.Windows.Forms.TextBox upperValue;
        private System.Windows.Forms.Label labelTilda;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.CheckBox inverseResult;
        private System.Windows.Forms.CheckBox modelVerification;
        private System.Windows.Forms.Label labelNumSample;
        private System.Windows.Forms.NumericUpDown numSample;
        private System.Windows.Forms.TextBox localScaleFactor;
        private System.Windows.Forms.CheckBox useLocalScaleFactor;
        private System.Windows.Forms.TextBox valueOffset;
        private System.Windows.Forms.Label labelValueOffset;
        private System.Windows.Forms.ComboBox measureType;
        private System.Windows.Forms.Label labelMeasureType;
        private System.Windows.Forms.Panel panelMeasureParam;
        private System.Windows.Forms.Panel panelProbeSelector;
        private System.Windows.Forms.ComboBox target1;
        private System.Windows.Forms.Label labelTarget1;
        private System.Windows.Forms.ComboBox target2;
        private System.Windows.Forms.Label labelTarget2;
        private System.Windows.Forms.ComboBox filterType;
        private System.Windows.Forms.Label labelFilterType;
    }
}