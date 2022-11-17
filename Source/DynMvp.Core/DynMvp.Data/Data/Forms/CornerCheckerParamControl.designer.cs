namespace DynMvp.Data.Forms
{
    partial class CornerCheckerParamControl
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
            this.detectorPropertyBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeDetector)).BeginInit();
            this.SuspendLayout();
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
            this.detectorPropertyBox.Location = new System.Drawing.Point(3, 48);
            this.detectorPropertyBox.Name = "detectorPropertyBox";
            this.detectorPropertyBox.Size = new System.Drawing.Size(324, 202);
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
            this.edge2TypeCmb.Location = new System.Drawing.Point(155, 160);
            this.edge2TypeCmb.Margin = new System.Windows.Forms.Padding(4);
            this.edge2TypeCmb.Name = "edge2TypeCmb";
            this.edge2TypeCmb.Size = new System.Drawing.Size(152, 28);
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
            this.edge1TypeCmb.Location = new System.Drawing.Point(155, 129);
            this.edge1TypeCmb.Margin = new System.Windows.Forms.Padding(4);
            this.edge1TypeCmb.Name = "edge1TypeCmb";
            this.edge1TypeCmb.Size = new System.Drawing.Size(152, 28);
            this.edge1TypeCmb.TabIndex = 2;
            this.edge1TypeCmb.SelectedIndexChanged += new System.EventHandler(this.edge1TypeCmb_SelectedIndexChanged);
            // 
            // searchAngle
            // 
            this.searchAngle.Location = new System.Drawing.Point(214, 97);
            this.searchAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.searchAngle.Name = "searchAngle";
            this.searchAngle.Size = new System.Drawing.Size(93, 26);
            this.searchAngle.TabIndex = 1;
            this.searchAngle.ValueChanged += new System.EventHandler(this.searchAngle_ValueChanged);
            // 
            // projectionHeight
            // 
            this.projectionHeight.Location = new System.Drawing.Point(214, 64);
            this.projectionHeight.Name = "projectionHeight";
            this.projectionHeight.Size = new System.Drawing.Size(93, 26);
            this.projectionHeight.TabIndex = 1;
            this.projectionHeight.ValueChanged += new System.EventHandler(this.projectionHeight_ValueChanged);
            // 
            // labelEdge2
            // 
            this.labelEdge2.AutoSize = true;
            this.labelEdge2.Location = new System.Drawing.Point(102, 163);
            this.labelEdge2.Name = "labelEdge2";
            this.labelEdge2.Size = new System.Drawing.Size(56, 20);
            this.labelEdge2.TabIndex = 0;
            this.labelEdge2.Text = "Edge2";
            this.labelEdge2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelEdge1
            // 
            this.labelEdge1.AutoSize = true;
            this.labelEdge1.Location = new System.Drawing.Point(102, 134);
            this.labelEdge1.Name = "labelEdge1";
            this.labelEdge1.Size = new System.Drawing.Size(56, 20);
            this.labelEdge1.TabIndex = 0;
            this.labelEdge1.Text = "Edge1";
            this.labelEdge1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // edgeTypeLabel
            // 
            this.edgeTypeLabel.AutoSize = true;
            this.edgeTypeLabel.Location = new System.Drawing.Point(17, 134);
            this.edgeTypeLabel.Name = "edgeTypeLabel";
            this.edgeTypeLabel.Size = new System.Drawing.Size(85, 20);
            this.edgeTypeLabel.TabIndex = 0;
            this.edgeTypeLabel.Text = "Edge Type";
            // 
            // searchAngleLabel
            // 
            this.searchAngleLabel.AutoSize = true;
            this.searchAngleLabel.Location = new System.Drawing.Point(17, 99);
            this.searchAngleLabel.Name = "searchAngleLabel";
            this.searchAngleLabel.Size = new System.Drawing.Size(105, 20);
            this.searchAngleLabel.TabIndex = 0;
            this.searchAngleLabel.Text = "Search Angle";
            // 
            // searchLength
            // 
            this.searchLength.Location = new System.Drawing.Point(214, 32);
            this.searchLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchLength.Name = "searchLength";
            this.searchLength.Size = new System.Drawing.Size(93, 26);
            this.searchLength.TabIndex = 1;
            this.searchLength.ValueChanged += new System.EventHandler(this.searchLength_ValueChanged);
            // 
            // projectionHeightLabel
            // 
            this.projectionHeightLabel.AutoSize = true;
            this.projectionHeightLabel.Location = new System.Drawing.Point(17, 66);
            this.projectionHeightLabel.Name = "projectionHeightLabel";
            this.projectionHeightLabel.Size = new System.Drawing.Size(130, 20);
            this.projectionHeightLabel.TabIndex = 0;
            this.projectionHeightLabel.Text = "Projection Height";
            // 
            // searchLengthLabel
            // 
            this.searchLengthLabel.AutoSize = true;
            this.searchLengthLabel.Location = new System.Drawing.Point(17, 34);
            this.searchLengthLabel.Name = "searchLengthLabel";
            this.searchLengthLabel.Size = new System.Drawing.Size(114, 20);
            this.searchLengthLabel.TabIndex = 0;
            this.searchLengthLabel.Text = "Search Length";
            // 
            // numEdgeDetector
            // 
            this.numEdgeDetector.Location = new System.Drawing.Point(217, 13);
            this.numEdgeDetector.Name = "numEdgeDetector";
            this.numEdgeDetector.Size = new System.Drawing.Size(93, 26);
            this.numEdgeDetector.TabIndex = 9;
            this.numEdgeDetector.ValueChanged += new System.EventHandler(this.numEdgeDetector_ValueChanged);
            // 
            // numEdgeDetectorLabel
            // 
            this.numEdgeDetectorLabel.AutoSize = true;
            this.numEdgeDetectorLabel.Location = new System.Drawing.Point(9, 16);
            this.numEdgeDetectorLabel.Name = "numEdgeDetectorLabel";
            this.numEdgeDetectorLabel.Size = new System.Drawing.Size(149, 20);
            this.numEdgeDetectorLabel.TabIndex = 8;
            this.numEdgeDetectorLabel.Text = "Number of Detector";
            // 
            // CornerCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.detectorPropertyBox);
            this.Controls.Add(this.numEdgeDetector);
            this.Controls.Add(this.numEdgeDetectorLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CornerCheckerParamControl";
            this.Size = new System.Drawing.Size(341, 393);
            this.detectorPropertyBox.ResumeLayout(false);
            this.detectorPropertyBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeDetector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
    }
}
