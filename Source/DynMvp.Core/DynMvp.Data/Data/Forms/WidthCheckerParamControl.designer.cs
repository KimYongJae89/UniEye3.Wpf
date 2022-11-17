namespace DynMvp.Data.Forms
{
    partial class WidthCheckerParamControl
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
            this.scaleValue = new System.Windows.Forms.TextBox();
            this.maxWidthRatio = new System.Windows.Forms.NumericUpDown();
            this.scaleValueLabel = new System.Windows.Forms.Label();
            this.minWidthRatio = new System.Windows.Forms.NumericUpDown();
            this.tildeLabel = new System.Windows.Forms.Label();
            this.widthRatioLabel = new System.Windows.Forms.Label();
            this.detectorPropertyBox = new System.Windows.Forms.GroupBox();
            this.edge2TypeCmb = new System.Windows.Forms.ComboBox();
            this.edge1TypeCmb = new System.Windows.Forms.ComboBox();
            this.searchAngle = new System.Windows.Forms.NumericUpDown();
            this.projectionHeight = new System.Windows.Forms.NumericUpDown();
            this.labelEdge2 = new System.Windows.Forms.Label();
            this.labelEdge1 = new System.Windows.Forms.Label();
            this.edgeTypeLabel = new System.Windows.Forms.Label();
            this.searchAngleLabel = new System.Windows.Forms.Label();
            this.searchLength = new System.Windows.Forms.NumericUpDown();
            this.projectionHeightLabel = new System.Windows.Forms.Label();
            this.searchLengthLabel = new System.Windows.Forms.Label();
            this.numEdgeDetector = new System.Windows.Forms.NumericUpDown();
            this.numEdgeDetectorLabel = new System.Windows.Forms.Label();
            this.maxCenterGap = new System.Windows.Forms.NumericUpDown();
            this.maxCenterGapLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxWidthRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minWidthRatio)).BeginInit();
            this.detectorPropertyBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeDetector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCenterGap)).BeginInit();
            this.SuspendLayout();
            // 
            // scaleValue
            // 
            this.scaleValue.Location = new System.Drawing.Point(203, 289);
            this.scaleValue.Name = "scaleValue";
            this.scaleValue.Size = new System.Drawing.Size(107, 24);
            this.scaleValue.TabIndex = 16;
            this.scaleValue.TextChanged += new System.EventHandler(this.scaleValue_TextChanged);
            // 
            // maxWidthRatio
            // 
            this.maxWidthRatio.Location = new System.Drawing.Point(236, 231);
            this.maxWidthRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxWidthRatio.Name = "maxWidthRatio";
            this.maxWidthRatio.Size = new System.Drawing.Size(74, 24);
            this.maxWidthRatio.TabIndex = 14;
            this.maxWidthRatio.ValueChanged += new System.EventHandler(this.maxWidthRatio_ValueChanged);
            // 
            // scaleValueLabel
            // 
            this.scaleValueLabel.AutoSize = true;
            this.scaleValueLabel.Location = new System.Drawing.Point(9, 292);
            this.scaleValueLabel.Name = "scaleValueLabel";
            this.scaleValueLabel.Size = new System.Drawing.Size(85, 18);
            this.scaleValueLabel.TabIndex = 11;
            this.scaleValueLabel.Text = "Scale Value";
            // 
            // minWidthRatio
            // 
            this.minWidthRatio.Location = new System.Drawing.Point(137, 231);
            this.minWidthRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minWidthRatio.Name = "minWidthRatio";
            this.minWidthRatio.Size = new System.Drawing.Size(69, 24);
            this.minWidthRatio.TabIndex = 15;
            this.minWidthRatio.ValueChanged += new System.EventHandler(this.minWidthRatio_ValueChanged);
            // 
            // tildeLabel
            // 
            this.tildeLabel.AutoSize = true;
            this.tildeLabel.Location = new System.Drawing.Point(212, 233);
            this.tildeLabel.Name = "tildeLabel";
            this.tildeLabel.Size = new System.Drawing.Size(17, 18);
            this.tildeLabel.TabIndex = 12;
            this.tildeLabel.Text = "~";
            // 
            // widthRatioLabel
            // 
            this.widthRatioLabel.AutoSize = true;
            this.widthRatioLabel.Location = new System.Drawing.Point(9, 233);
            this.widthRatioLabel.Name = "widthRatioLabel";
            this.widthRatioLabel.Size = new System.Drawing.Size(85, 18);
            this.widthRatioLabel.TabIndex = 13;
            this.widthRatioLabel.Text = "Width Ratio";
            // 
            // detectorPropertyBox
            // 
            this.detectorPropertyBox.Controls.Add(this.edge2TypeCmb);
            this.detectorPropertyBox.Controls.Add(this.edge1TypeCmb);
            this.detectorPropertyBox.Controls.Add(this.searchAngle);
            this.detectorPropertyBox.Controls.Add(this.projectionHeight);
            this.detectorPropertyBox.Controls.Add(this.labelEdge2);
            this.detectorPropertyBox.Controls.Add(this.labelEdge1);
            this.detectorPropertyBox.Controls.Add(this.edgeTypeLabel);
            this.detectorPropertyBox.Controls.Add(this.searchAngleLabel);
            this.detectorPropertyBox.Controls.Add(this.searchLength);
            this.detectorPropertyBox.Controls.Add(this.projectionHeightLabel);
            this.detectorPropertyBox.Controls.Add(this.searchLengthLabel);
            this.detectorPropertyBox.Location = new System.Drawing.Point(3, 43);
            this.detectorPropertyBox.Name = "detectorPropertyBox";
            this.detectorPropertyBox.Size = new System.Drawing.Size(324, 182);
            this.detectorPropertyBox.TabIndex = 10;
            this.detectorPropertyBox.TabStop = false;
            this.detectorPropertyBox.Text = "Detector Property";
            // 
            // edge2TypeCmb
            // 
            this.edge2TypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edge2TypeCmb.FormattingEnabled = true;
            this.edge2TypeCmb.Items.AddRange(new object[] {
            "DarkToLight",
            "LightToDark",
            "Any"});
            this.edge2TypeCmb.Location = new System.Drawing.Point(155, 144);
            this.edge2TypeCmb.Margin = new System.Windows.Forms.Padding(4);
            this.edge2TypeCmb.Name = "edge2TypeCmb";
            this.edge2TypeCmb.Size = new System.Drawing.Size(152, 26);
            this.edge2TypeCmb.TabIndex = 2;
            this.edge2TypeCmb.SelectedIndexChanged += new System.EventHandler(this.edge2TypeCmb_SelectedIndexChanged);
            // 
            // edge1TypeCmb
            // 
            this.edge1TypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edge1TypeCmb.FormattingEnabled = true;
            this.edge1TypeCmb.Items.AddRange(new object[] {
            "DarkToLight",
            "LightToDark",
            "Any"});
            this.edge1TypeCmb.Location = new System.Drawing.Point(155, 116);
            this.edge1TypeCmb.Margin = new System.Windows.Forms.Padding(4);
            this.edge1TypeCmb.Name = "edge1TypeCmb";
            this.edge1TypeCmb.Size = new System.Drawing.Size(152, 26);
            this.edge1TypeCmb.TabIndex = 2;
            this.edge1TypeCmb.SelectedIndexChanged += new System.EventHandler(this.edge1TypeCmb_SelectedIndexChanged);
            // 
            // searchAngle
            // 
            this.searchAngle.Location = new System.Drawing.Point(214, 87);
            this.searchAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.searchAngle.Name = "searchAngle";
            this.searchAngle.Size = new System.Drawing.Size(93, 24);
            this.searchAngle.TabIndex = 1;
            this.searchAngle.ValueChanged += new System.EventHandler(this.searchAngle_ValueChanged);
            // 
            // projectionHeight
            // 
            this.projectionHeight.Location = new System.Drawing.Point(214, 58);
            this.projectionHeight.Name = "projectionHeight";
            this.projectionHeight.Size = new System.Drawing.Size(93, 24);
            this.projectionHeight.TabIndex = 1;
            this.projectionHeight.ValueChanged += new System.EventHandler(this.projectionHeight_ValueChanged);
            // 
            // labelEdge2
            // 
            this.labelEdge2.AutoSize = true;
            this.labelEdge2.Location = new System.Drawing.Point(102, 147);
            this.labelEdge2.Name = "labelEdge2";
            this.labelEdge2.Size = new System.Drawing.Size(50, 18);
            this.labelEdge2.TabIndex = 0;
            this.labelEdge2.Text = "Edge2";
            this.labelEdge2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelEdge1
            // 
            this.labelEdge1.AutoSize = true;
            this.labelEdge1.Location = new System.Drawing.Point(102, 121);
            this.labelEdge1.Name = "labelEdge1";
            this.labelEdge1.Size = new System.Drawing.Size(50, 18);
            this.labelEdge1.TabIndex = 0;
            this.labelEdge1.Text = "Edge1";
            this.labelEdge1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // edgeTypeLabel
            // 
            this.edgeTypeLabel.AutoSize = true;
            this.edgeTypeLabel.Location = new System.Drawing.Point(17, 121);
            this.edgeTypeLabel.Name = "edgeTypeLabel";
            this.edgeTypeLabel.Size = new System.Drawing.Size(78, 18);
            this.edgeTypeLabel.TabIndex = 0;
            this.edgeTypeLabel.Text = "Edge Type";
            // 
            // searchAngleLabel
            // 
            this.searchAngleLabel.AutoSize = true;
            this.searchAngleLabel.Location = new System.Drawing.Point(17, 89);
            this.searchAngleLabel.Name = "searchAngleLabel";
            this.searchAngleLabel.Size = new System.Drawing.Size(95, 18);
            this.searchAngleLabel.TabIndex = 0;
            this.searchAngleLabel.Text = "Search Angle";
            // 
            // searchLength
            // 
            this.searchLength.Location = new System.Drawing.Point(214, 29);
            this.searchLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchLength.Name = "searchLength";
            this.searchLength.Size = new System.Drawing.Size(93, 24);
            this.searchLength.TabIndex = 1;
            this.searchLength.ValueChanged += new System.EventHandler(this.searchLength_ValueChanged);
            // 
            // projectionHeightLabel
            // 
            this.projectionHeightLabel.AutoSize = true;
            this.projectionHeightLabel.Location = new System.Drawing.Point(17, 59);
            this.projectionHeightLabel.Name = "projectionHeightLabel";
            this.projectionHeightLabel.Size = new System.Drawing.Size(121, 18);
            this.projectionHeightLabel.TabIndex = 0;
            this.projectionHeightLabel.Text = "Projection Height";
            // 
            // searchLengthLabel
            // 
            this.searchLengthLabel.AutoSize = true;
            this.searchLengthLabel.Location = new System.Drawing.Point(17, 31);
            this.searchLengthLabel.Name = "searchLengthLabel";
            this.searchLengthLabel.Size = new System.Drawing.Size(103, 18);
            this.searchLengthLabel.TabIndex = 0;
            this.searchLengthLabel.Text = "Search Length";
            // 
            // numEdgeDetector
            // 
            this.numEdgeDetector.Location = new System.Drawing.Point(217, 12);
            this.numEdgeDetector.Name = "numEdgeDetector";
            this.numEdgeDetector.Size = new System.Drawing.Size(93, 24);
            this.numEdgeDetector.TabIndex = 9;
            this.numEdgeDetector.ValueChanged += new System.EventHandler(this.numEdgeDetector_ValueChanged);
            // 
            // numEdgeDetectorLabel
            // 
            this.numEdgeDetectorLabel.AutoSize = true;
            this.numEdgeDetectorLabel.Location = new System.Drawing.Point(9, 14);
            this.numEdgeDetectorLabel.Name = "numEdgeDetectorLabel";
            this.numEdgeDetectorLabel.Size = new System.Drawing.Size(139, 18);
            this.numEdgeDetectorLabel.TabIndex = 8;
            this.numEdgeDetectorLabel.Text = "Number of Detector";
            // 
            // maxCenterGap
            // 
            this.maxCenterGap.Location = new System.Drawing.Point(236, 260);
            this.maxCenterGap.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxCenterGap.Name = "maxCenterGap";
            this.maxCenterGap.Size = new System.Drawing.Size(74, 24);
            this.maxCenterGap.TabIndex = 18;
            this.maxCenterGap.ValueChanged += new System.EventHandler(this.maxCenterGap_ValueChanged);
            // 
            // maxCenterGapLabel
            // 
            this.maxCenterGapLabel.AutoSize = true;
            this.maxCenterGapLabel.Location = new System.Drawing.Point(9, 262);
            this.maxCenterGapLabel.Name = "maxCenterGapLabel";
            this.maxCenterGapLabel.Size = new System.Drawing.Size(84, 18);
            this.maxCenterGapLabel.TabIndex = 17;
            this.maxCenterGapLabel.Text = "Canter Gap";
            // 
            // WidthCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.maxCenterGap);
            this.Controls.Add(this.maxCenterGapLabel);
            this.Controls.Add(this.scaleValue);
            this.Controls.Add(this.maxWidthRatio);
            this.Controls.Add(this.scaleValueLabel);
            this.Controls.Add(this.minWidthRatio);
            this.Controls.Add(this.tildeLabel);
            this.Controls.Add(this.widthRatioLabel);
            this.Controls.Add(this.detectorPropertyBox);
            this.Controls.Add(this.numEdgeDetector);
            this.Controls.Add(this.numEdgeDetectorLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WidthCheckerParamControl";
            this.Size = new System.Drawing.Size(341, 354);
            ((System.ComponentModel.ISupportInitialize)(this.maxWidthRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minWidthRatio)).EndInit();
            this.detectorPropertyBox.ResumeLayout(false);
            this.detectorPropertyBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeDetector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCenterGap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox scaleValue;
        private System.Windows.Forms.NumericUpDown maxWidthRatio;
        private System.Windows.Forms.Label scaleValueLabel;
        private System.Windows.Forms.NumericUpDown minWidthRatio;
        private System.Windows.Forms.Label tildeLabel;
        private System.Windows.Forms.Label widthRatioLabel;
        private System.Windows.Forms.GroupBox detectorPropertyBox;
        private System.Windows.Forms.ComboBox edge1TypeCmb;
        private System.Windows.Forms.NumericUpDown searchAngle;
        private System.Windows.Forms.NumericUpDown projectionHeight;
        private System.Windows.Forms.Label edgeTypeLabel;
        private System.Windows.Forms.Label searchAngleLabel;
        private System.Windows.Forms.NumericUpDown searchLength;
        private System.Windows.Forms.Label projectionHeightLabel;
        private System.Windows.Forms.Label searchLengthLabel;
        private System.Windows.Forms.NumericUpDown numEdgeDetector;
        private System.Windows.Forms.Label numEdgeDetectorLabel;
        private System.Windows.Forms.ComboBox edge2TypeCmb;
        private System.Windows.Forms.Label labelEdge2;
        private System.Windows.Forms.Label labelEdge1;
        private System.Windows.Forms.NumericUpDown maxCenterGap;
        private System.Windows.Forms.Label maxCenterGapLabel;
    }
}
