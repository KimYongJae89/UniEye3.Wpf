namespace UniEye.Base.UI.ParamControl
{
    partial class TensionChekcerParamControl
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
            this.portSelector = new System.Windows.Forms.ComboBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.inverseResult = new System.Windows.Forms.CheckBox();
            this.modelVerification = new System.Windows.Forms.CheckBox();
            this.numLowerValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numUpperValue = new System.Windows.Forms.NumericUpDown();
            this.labelNumReading = new System.Windows.Forms.Label();
            this.numReading = new System.Windows.Forms.NumericUpDown();
            this.labelTensionFilePath = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonTenstionPath = new System.Windows.Forms.Button();
            this.labelValueType = new System.Windows.Forms.Label();
            this.comboBoxTensionUnit = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numLowerValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpperValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReading)).BeginInit();
            this.SuspendLayout();
            // 
            // portSelector
            // 
            this.portSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portSelector.FormattingEnabled = true;
            this.portSelector.Location = new System.Drawing.Point(131, 7);
            this.portSelector.Margin = new System.Windows.Forms.Padding(4);
            this.portSelector.Name = "portSelector";
            this.portSelector.Size = new System.Drawing.Size(148, 28);
            this.portSelector.TabIndex = 1;
            this.portSelector.SelectedIndexChanged += new System.EventHandler(this.portSelector_SelectedIndexChanged);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(12, 9);
            this.labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(38, 20);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "Port";
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.labelRange.Location = new System.Drawing.Point(12, 42);
            this.labelRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(111, 20);
            this.labelRange.TabIndex = 2;
            this.labelRange.Text = "Normal Range";
            // 
            // inverseResult
            // 
            this.inverseResult.AutoSize = true;
            this.inverseResult.Location = new System.Drawing.Point(23, 177);
            this.inverseResult.Margin = new System.Windows.Forms.Padding(2);
            this.inverseResult.Name = "inverseResult";
            this.inverseResult.Size = new System.Drawing.Size(130, 24);
            this.inverseResult.TabIndex = 6;
            this.inverseResult.Text = "Inverse Result";
            this.inverseResult.UseVisualStyleBackColor = true;
            this.inverseResult.Visible = false;
            this.inverseResult.CheckedChanged += new System.EventHandler(this.inverseResult_CheckedChanged);
            // 
            // modelVerification
            // 
            this.modelVerification.AutoSize = true;
            this.modelVerification.Location = new System.Drawing.Point(23, 202);
            this.modelVerification.Margin = new System.Windows.Forms.Padding(2);
            this.modelVerification.Name = "modelVerification";
            this.modelVerification.Size = new System.Drawing.Size(158, 24);
            this.modelVerification.TabIndex = 7;
            this.modelVerification.Text = "Use Model Verifier";
            this.modelVerification.UseVisualStyleBackColor = true;
            this.modelVerification.Visible = false;
            this.modelVerification.CheckedChanged += new System.EventHandler(this.modelVerification_CheckedChanged);
            // 
            // numLowerValue
            // 
            this.numLowerValue.DecimalPlaces = 2;
            this.numLowerValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numLowerValue.Location = new System.Drawing.Point(124, 42);
            this.numLowerValue.Margin = new System.Windows.Forms.Padding(2);
            this.numLowerValue.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numLowerValue.Name = "numLowerValue";
            this.numLowerValue.Size = new System.Drawing.Size(61, 26);
            this.numLowerValue.TabIndex = 8;
            this.numLowerValue.ValueChanged += new System.EventHandler(this.numLowerValue_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "~";
            // 
            // numUpperValue
            // 
            this.numUpperValue.DecimalPlaces = 2;
            this.numUpperValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpperValue.Location = new System.Drawing.Point(209, 42);
            this.numUpperValue.Margin = new System.Windows.Forms.Padding(2);
            this.numUpperValue.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numUpperValue.Name = "numUpperValue";
            this.numUpperValue.Size = new System.Drawing.Size(61, 26);
            this.numUpperValue.TabIndex = 10;
            this.numUpperValue.ValueChanged += new System.EventHandler(this.numMaxValue_ValueChanged);
            // 
            // labelNumReading
            // 
            this.labelNumReading.AutoSize = true;
            this.labelNumReading.Location = new System.Drawing.Point(12, 72);
            this.labelNumReading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumReading.Name = "labelNumReading";
            this.labelNumReading.Size = new System.Drawing.Size(147, 20);
            this.labelNumReading.TabIndex = 2;
            this.labelNumReading.Text = "Number of Reading";
            // 
            // numReading
            // 
            this.numReading.Location = new System.Drawing.Point(209, 70);
            this.numReading.Margin = new System.Windows.Forms.Padding(2);
            this.numReading.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numReading.Name = "numReading";
            this.numReading.Size = new System.Drawing.Size(61, 26);
            this.numReading.TabIndex = 8;
            this.numReading.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numReading.ValueChanged += new System.EventHandler(this.numReading_ValueChanged);
            // 
            // labelTensionFilePath
            // 
            this.labelTensionFilePath.AutoSize = true;
            this.labelTensionFilePath.Location = new System.Drawing.Point(19, 131);
            this.labelTensionFilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTensionFilePath.Name = "labelTensionFilePath";
            this.labelTensionFilePath.Size = new System.Drawing.Size(71, 20);
            this.labelTensionFilePath.TabIndex = 11;
            this.labelTensionFilePath.Text = "File Path";
            this.labelTensionFilePath.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(131, 131);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(116, 26);
            this.textBox1.TabIndex = 12;
            this.textBox1.Visible = false;
            // 
            // buttonTenstionPath
            // 
            this.buttonTenstionPath.Location = new System.Drawing.Point(248, 131);
            this.buttonTenstionPath.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTenstionPath.Name = "buttonTenstionPath";
            this.buttonTenstionPath.Size = new System.Drawing.Size(37, 24);
            this.buttonTenstionPath.TabIndex = 13;
            this.buttonTenstionPath.Text = "...";
            this.buttonTenstionPath.UseVisualStyleBackColor = true;
            this.buttonTenstionPath.Visible = false;
            this.buttonTenstionPath.Click += new System.EventHandler(this.buttonTenstionPath_Click);
            // 
            // labelValueType
            // 
            this.labelValueType.AutoSize = true;
            this.labelValueType.Location = new System.Drawing.Point(12, 102);
            this.labelValueType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelValueType.Name = "labelValueType";
            this.labelValueType.Size = new System.Drawing.Size(98, 20);
            this.labelValueType.TabIndex = 2;
            this.labelValueType.Text = "Tension Unit";
            // 
            // comboBoxTensionUnit
            // 
            this.comboBoxTensionUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTensionUnit.FormattingEnabled = true;
            this.comboBoxTensionUnit.Items.AddRange(new object[] {
            "Newton",
            "mm"});
            this.comboBoxTensionUnit.Location = new System.Drawing.Point(131, 99);
            this.comboBoxTensionUnit.Name = "comboBoxTensionUnit";
            this.comboBoxTensionUnit.Size = new System.Drawing.Size(139, 28);
            this.comboBoxTensionUnit.TabIndex = 14;
            this.comboBoxTensionUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxTensionUnit_SelectedIndexChanged);
            // 
            // TensionChekcerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxTensionUnit);
            this.Controls.Add(this.buttonTenstionPath);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelTensionFilePath);
            this.Controls.Add(this.numUpperValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numReading);
            this.Controls.Add(this.numLowerValue);
            this.Controls.Add(this.modelVerification);
            this.Controls.Add(this.inverseResult);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.portSelector);
            this.Controls.Add(this.labelValueType);
            this.Controls.Add(this.labelNumReading);
            this.Controls.Add(this.labelRange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TensionChekcerParamControl";
            this.Size = new System.Drawing.Size(375, 272);
            this.Load += new System.EventHandler(this.TensionChekcerParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numLowerValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpperValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox portSelector;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelRange;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.CheckBox inverseResult;
        private System.Windows.Forms.CheckBox modelVerification;
        private System.Windows.Forms.NumericUpDown numLowerValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUpperValue;
        private System.Windows.Forms.Label labelNumReading;
        private System.Windows.Forms.NumericUpDown numReading;
        private System.Windows.Forms.Label labelTensionFilePath;
        private System.Windows.Forms.Button buttonTenstionPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelValueType;
        private System.Windows.Forms.ComboBox comboBoxTensionUnit;
    }
}