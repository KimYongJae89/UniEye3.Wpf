namespace UniEye.Base.UI.ParamControl
{
    partial class MarkerParamControl
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
            this.markerType = new System.Windows.Forms.ComboBox();
            this.labelMarkerType = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.mergeSource = new System.Windows.Forms.TextBox();
            this.labelMergeSource = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mergeOffsetX = new System.Windows.Forms.NumericUpDown();
            this.mergeOffsetY = new System.Windows.Forms.NumericUpDown();
            this.mergeOffsetZ = new System.Windows.Forms.NumericUpDown();
            this.labelMergeOffsetX = new System.Windows.Forms.Label();
            this.labelMergeOffsetY = new System.Windows.Forms.Label();
            this.labelMergeOffsetZ = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetZ)).BeginInit();
            this.SuspendLayout();
            // 
            // markerType
            // 
            this.markerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.markerType.FormattingEnabled = true;
            this.markerType.Location = new System.Drawing.Point(131, 6);
            this.markerType.Margin = new System.Windows.Forms.Padding(4);
            this.markerType.Name = "markerType";
            this.markerType.Size = new System.Drawing.Size(148, 26);
            this.markerType.TabIndex = 1;
            this.markerType.SelectedIndexChanged += new System.EventHandler(this.markerType_SelectedIndexChanged);
            // 
            // labelMarkerType
            // 
            this.labelMarkerType.AutoSize = true;
            this.labelMarkerType.Location = new System.Drawing.Point(12, 8);
            this.labelMarkerType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMarkerType.Name = "labelMarkerType";
            this.labelMarkerType.Size = new System.Drawing.Size(91, 18);
            this.labelMarkerType.TabIndex = 0;
            this.labelMarkerType.Text = "Marker Type";
            // 
            // mergeSource
            // 
            this.mergeSource.Location = new System.Drawing.Point(131, 39);
            this.mergeSource.Name = "mergeSource";
            this.mergeSource.Size = new System.Drawing.Size(148, 24);
            this.mergeSource.TabIndex = 2;
            this.mergeSource.TextChanged += new System.EventHandler(this.mergeSource_TextChanged);
            // 
            // labelMergeSource
            // 
            this.labelMergeSource.AutoSize = true;
            this.labelMergeSource.Location = new System.Drawing.Point(12, 42);
            this.labelMergeSource.Name = "labelMergeSource";
            this.labelMergeSource.Size = new System.Drawing.Size(102, 18);
            this.labelMergeSource.TabIndex = 3;
            this.labelMergeSource.Text = "Merge Source";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Merge Offset";
            // 
            // mergeOffsetX
            // 
            this.mergeOffsetX.DecimalPlaces = 1;
            this.mergeOffsetX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.mergeOffsetX.Location = new System.Drawing.Point(200, 72);
            this.mergeOffsetX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mergeOffsetX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.mergeOffsetX.Name = "mergeOffsetX";
            this.mergeOffsetX.Size = new System.Drawing.Size(79, 24);
            this.mergeOffsetX.TabIndex = 5;
            this.mergeOffsetX.ValueChanged += new System.EventHandler(this.mergeOffsetX_ValueChanged);
            // 
            // mergeOffsetY
            // 
            this.mergeOffsetY.DecimalPlaces = 1;
            this.mergeOffsetY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.mergeOffsetY.Location = new System.Drawing.Point(200, 102);
            this.mergeOffsetY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mergeOffsetY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.mergeOffsetY.Name = "mergeOffsetY";
            this.mergeOffsetY.Size = new System.Drawing.Size(79, 24);
            this.mergeOffsetY.TabIndex = 5;
            this.mergeOffsetY.ValueChanged += new System.EventHandler(this.mergeOffsetY_ValueChanged);
            // 
            // mergeOffsetZ
            // 
            this.mergeOffsetZ.DecimalPlaces = 1;
            this.mergeOffsetZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.mergeOffsetZ.Location = new System.Drawing.Point(200, 132);
            this.mergeOffsetZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mergeOffsetZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.mergeOffsetZ.Name = "mergeOffsetZ";
            this.mergeOffsetZ.Size = new System.Drawing.Size(79, 24);
            this.mergeOffsetZ.TabIndex = 5;
            this.mergeOffsetZ.ValueChanged += new System.EventHandler(this.mergeOffsetZ_ValueChanged);
            // 
            // labelMergeOffsetX
            // 
            this.labelMergeOffsetX.AutoSize = true;
            this.labelMergeOffsetX.Location = new System.Drawing.Point(128, 74);
            this.labelMergeOffsetX.Name = "labelMergeOffsetX";
            this.labelMergeOffsetX.Size = new System.Drawing.Size(18, 18);
            this.labelMergeOffsetX.TabIndex = 3;
            this.labelMergeOffsetX.Text = "X";
            // 
            // labelMergeOffsetY
            // 
            this.labelMergeOffsetY.AutoSize = true;
            this.labelMergeOffsetY.Location = new System.Drawing.Point(128, 104);
            this.labelMergeOffsetY.Name = "labelMergeOffsetY";
            this.labelMergeOffsetY.Size = new System.Drawing.Size(17, 18);
            this.labelMergeOffsetY.TabIndex = 3;
            this.labelMergeOffsetY.Text = "Y";
            // 
            // labelMergeOffsetZ
            // 
            this.labelMergeOffsetZ.AutoSize = true;
            this.labelMergeOffsetZ.Location = new System.Drawing.Point(128, 134);
            this.labelMergeOffsetZ.Name = "labelMergeOffsetZ";
            this.labelMergeOffsetZ.Size = new System.Drawing.Size(17, 18);
            this.labelMergeOffsetZ.TabIndex = 3;
            this.labelMergeOffsetZ.Text = "Z";
            // 
            // MarkerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mergeOffsetZ);
            this.Controls.Add(this.mergeOffsetY);
            this.Controls.Add(this.mergeOffsetX);
            this.Controls.Add(this.labelMergeOffsetZ);
            this.Controls.Add(this.labelMergeOffsetY);
            this.Controls.Add(this.labelMergeOffsetX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMergeSource);
            this.Controls.Add(this.mergeSource);
            this.Controls.Add(this.labelMarkerType);
            this.Controls.Add(this.markerType);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MarkerParamControl";
            this.Size = new System.Drawing.Size(290, 328);
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mergeOffsetZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox markerType;
        private System.Windows.Forms.Label labelMarkerType;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox mergeSource;
        private System.Windows.Forms.Label labelMergeSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown mergeOffsetX;
        private System.Windows.Forms.NumericUpDown mergeOffsetY;
        private System.Windows.Forms.NumericUpDown mergeOffsetZ;
        private System.Windows.Forms.Label labelMergeOffsetX;
        private System.Windows.Forms.Label labelMergeOffsetY;
        private System.Windows.Forms.Label labelMergeOffsetZ;
    }
}