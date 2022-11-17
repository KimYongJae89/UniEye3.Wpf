namespace UniEye.Base.UI.ParamControl
{
    partial class VisionParamControl
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
            this.imageBandLabel = new System.Windows.Forms.Label();
            this.imageBand = new System.Windows.Forms.ComboBox();
            this.inverseResult = new System.Windows.Forms.CheckBox();
            this.probeHeight = new System.Windows.Forms.NumericUpDown();
            this.probeWidth = new System.Windows.Forms.NumericUpDown();
            this.probePosX = new System.Windows.Forms.NumericUpDown();
            this.probePosY = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPos = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.probePosR = new System.Windows.Forms.NumericUpDown();
            this.labelR = new System.Windows.Forms.Label();
            this.labelLightType = new System.Windows.Forms.Label();
            this.comboFiducialProbe = new System.Windows.Forms.ComboBox();
            this.labelFiducialProbe = new System.Windows.Forms.Label();
            this.contextMenuStripAddFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.comboBoxLightType = new System.Windows.Forms.ComboBox();
            this.buttonFilterUp = new System.Windows.Forms.Button();
            this.buttonFilterDown = new System.Windows.Forms.Button();
            this.filterListBox = new System.Windows.Forms.ListBox();
            this.buttonAddFilter = new System.Windows.Forms.Button();
            this.buttonDeleteFilter = new System.Windows.Forms.Button();
            this.groupboxGeneral = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.groupboxFilter = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.groupboxInspection = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxGeneral)).BeginInit();
            this.groupboxGeneral.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxFilter)).BeginInit();
            this.groupboxFilter.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxInspection)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBandLabel
            // 
            this.imageBandLabel.AutoSize = true;
            this.imageBandLabel.Location = new System.Drawing.Point(197, 7);
            this.imageBandLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.imageBandLabel.Name = "imageBandLabel";
            this.imageBandLabel.Size = new System.Drawing.Size(96, 20);
            this.imageBandLabel.TabIndex = 0;
            this.imageBandLabel.Text = "Image Band";
            // 
            // imageBand
            // 
            this.imageBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageBand.FormattingEnabled = true;
            this.imageBand.Items.AddRange(new object[] {
            "Luminance",
            "Red",
            "Green",
            "Blue"});
            this.imageBand.Location = new System.Drawing.Point(292, 3);
            this.imageBand.Name = "imageBand";
            this.imageBand.Size = new System.Drawing.Size(137, 28);
            this.imageBand.TabIndex = 1;
            this.imageBand.SelectedIndexChanged += new System.EventHandler(this.colorBand_SelectedIndexChanged);
            // 
            // inverseResult
            // 
            this.inverseResult.AutoSize = true;
            this.inverseResult.Location = new System.Drawing.Point(18, 93);
            this.inverseResult.Name = "inverseResult";
            this.inverseResult.Size = new System.Drawing.Size(130, 24);
            this.inverseResult.TabIndex = 5;
            this.inverseResult.Text = "Inverse Result";
            this.inverseResult.UseVisualStyleBackColor = true;
            this.inverseResult.CheckedChanged += new System.EventHandler(this.inverseResult_CheckedChanged);
            // 
            // probeHeight
            // 
            this.probeHeight.Location = new System.Drawing.Point(347, 32);
            this.probeHeight.Name = "probeHeight";
            this.probeHeight.Size = new System.Drawing.Size(82, 26);
            this.probeHeight.TabIndex = 9;
            this.probeHeight.ValueChanged += new System.EventHandler(this.probeHeight_ValueChanged);
            this.probeHeight.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeHeight.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probeWidth
            // 
            this.probeWidth.Location = new System.Drawing.Point(347, 4);
            this.probeWidth.Name = "probeWidth";
            this.probeWidth.Size = new System.Drawing.Size(82, 26);
            this.probeWidth.TabIndex = 7;
            this.probeWidth.ValueChanged += new System.EventHandler(this.probeWidth_ValueChanged);
            this.probeWidth.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeWidth.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosX
            // 
            this.probePosX.Location = new System.Drawing.Point(140, 4);
            this.probePosX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.probePosX.Name = "probePosX";
            this.probePosX.Size = new System.Drawing.Size(82, 26);
            this.probePosX.TabIndex = 2;
            this.probePosX.ValueChanged += new System.EventHandler(this.probePosX_ValueChanged);
            this.probePosX.Enter += new System.EventHandler(this.textBox_Enter);
            this.probePosX.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosY
            // 
            this.probePosY.Location = new System.Drawing.Point(140, 32);
            this.probePosY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.probePosY.Name = "probePosY";
            this.probePosY.Size = new System.Drawing.Size(82, 26);
            this.probePosY.TabIndex = 4;
            this.probePosY.ValueChanged += new System.EventHandler(this.probePosY_ValueChanged);
            this.probePosY.Enter += new System.EventHandler(this.textBox_Enter);
            this.probePosY.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // labelPos
            // 
            this.labelPos.AutoSize = true;
            this.labelPos.Location = new System.Drawing.Point(14, 6);
            this.labelPos.Name = "labelPos";
            this.labelPos.Size = new System.Drawing.Size(65, 20);
            this.labelPos.TabIndex = 0;
            this.labelPos.Text = "Position";
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(240, 7);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(40, 20);
            this.labelSize.TabIndex = 5;
            this.labelSize.Text = "Size";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(91, 7);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(20, 20);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(91, 35);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(20, 20);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y";
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(292, 6);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(24, 20);
            this.labelW.TabIndex = 6;
            this.labelW.Text = "W";
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(292, 35);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(21, 20);
            this.labelH.TabIndex = 8;
            this.labelH.Text = "H";
            // 
            // probePosR
            // 
            this.probePosR.Location = new System.Drawing.Point(140, 60);
            this.probePosR.Name = "probePosR";
            this.probePosR.Size = new System.Drawing.Size(82, 26);
            this.probePosR.TabIndex = 161;
            this.probePosR.ValueChanged += new System.EventHandler(this.probePosR_ValueChanged);
            // 
            // labelR
            // 
            this.labelR.AutoSize = true;
            this.labelR.Location = new System.Drawing.Point(91, 63);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(21, 20);
            this.labelR.TabIndex = 160;
            this.labelR.Text = "R";
            // 
            // labelLightType
            // 
            this.labelLightType.AutoSize = true;
            this.labelLightType.Location = new System.Drawing.Point(14, 120);
            this.labelLightType.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelLightType.Name = "labelLightType";
            this.labelLightType.Size = new System.Drawing.Size(82, 20);
            this.labelLightType.TabIndex = 158;
            this.labelLightType.Text = "Light Type";
            // 
            // comboFiducialProbe
            // 
            this.comboFiducialProbe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFiducialProbe.FormattingEnabled = true;
            this.comboFiducialProbe.Items.AddRange(new object[] {
            "Luminance",
            "Red",
            "Green",
            "Blue"});
            this.comboFiducialProbe.Location = new System.Drawing.Point(347, 88);
            this.comboFiducialProbe.Name = "comboFiducialProbe";
            this.comboFiducialProbe.Size = new System.Drawing.Size(80, 28);
            this.comboFiducialProbe.TabIndex = 1;
            this.comboFiducialProbe.SelectedIndexChanged += new System.EventHandler(this.comboFiducialProbe_SelectedIndexChanged);
            // 
            // labelFiducialProbe
            // 
            this.labelFiducialProbe.AutoSize = true;
            this.labelFiducialProbe.Location = new System.Drawing.Point(232, 93);
            this.labelFiducialProbe.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelFiducialProbe.Name = "labelFiducialProbe";
            this.labelFiducialProbe.Size = new System.Drawing.Size(109, 20);
            this.labelFiducialProbe.TabIndex = 0;
            this.labelFiducialProbe.Text = "Fiducial Probe";
            // 
            // contextMenuStripAddFilter
            // 
            this.contextMenuStripAddFilter.Name = "contextMenuStripAddFilter";
            this.contextMenuStripAddFilter.Size = new System.Drawing.Size(61, 4);
            // 
            // comboBoxLightType
            // 
            this.comboBoxLightType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLightType.FormattingEnabled = true;
            this.comboBoxLightType.Location = new System.Drawing.Point(140, 120);
            this.comboBoxLightType.Name = "comboBoxLightType";
            this.comboBoxLightType.Size = new System.Drawing.Size(289, 28);
            this.comboBoxLightType.TabIndex = 162;
            this.comboBoxLightType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLightType_SelectedIndexChanged);
            // 
            // buttonFilterUp
            // 
            this.buttonFilterUp.Location = new System.Drawing.Point(9, 37);
            this.buttonFilterUp.Name = "buttonFilterUp";
            this.buttonFilterUp.Size = new System.Drawing.Size(45, 47);
            this.buttonFilterUp.TabIndex = 8;
            this.buttonFilterUp.Text = "U";
            this.buttonFilterUp.UseVisualStyleBackColor = true;
            this.buttonFilterUp.Click += new System.EventHandler(this.buttonFilterUp_Click);
            // 
            // buttonFilterDown
            // 
            this.buttonFilterDown.Location = new System.Drawing.Point(9, 90);
            this.buttonFilterDown.Name = "buttonFilterDown";
            this.buttonFilterDown.Size = new System.Drawing.Size(45, 51);
            this.buttonFilterDown.TabIndex = 7;
            this.buttonFilterDown.Text = "D";
            this.buttonFilterDown.UseVisualStyleBackColor = true;
            this.buttonFilterDown.Click += new System.EventHandler(this.buttonFilterDown_Click);
            // 
            // filterListBox
            // 
            this.filterListBox.FormattingEnabled = true;
            this.filterListBox.ItemHeight = 20;
            this.filterListBox.Location = new System.Drawing.Point(60, 37);
            this.filterListBox.Name = "filterListBox";
            this.filterListBox.Size = new System.Drawing.Size(369, 104);
            this.filterListBox.TabIndex = 3;
            this.filterListBox.SelectedIndexChanged += new System.EventHandler(this.filterListBox_SelectedIndexChanged);
            // 
            // buttonAddFilter
            // 
            this.buttonAddFilter.Location = new System.Drawing.Point(9, 2);
            this.buttonAddFilter.Name = "buttonAddFilter";
            this.buttonAddFilter.Size = new System.Drawing.Size(83, 30);
            this.buttonAddFilter.TabIndex = 4;
            this.buttonAddFilter.Text = "Add";
            this.buttonAddFilter.UseVisualStyleBackColor = true;
            this.buttonAddFilter.Click += new System.EventHandler(this.buttonAddFilter_Click);
            // 
            // buttonDeleteFilter
            // 
            this.buttonDeleteFilter.Location = new System.Drawing.Point(100, 2);
            this.buttonDeleteFilter.Name = "buttonDeleteFilter";
            this.buttonDeleteFilter.Size = new System.Drawing.Size(88, 30);
            this.buttonDeleteFilter.TabIndex = 4;
            this.buttonDeleteFilter.Text = "Delete";
            this.buttonDeleteFilter.UseVisualStyleBackColor = true;
            this.buttonDeleteFilter.Click += new System.EventHandler(this.ButtonDeleteFilter_Click);
            // 
            // groupboxGeneral
            // 
            this.groupboxGeneral.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.groupboxGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupboxGeneral.ExpandedSize = new System.Drawing.Size(443, 181);
            this.groupboxGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupboxGeneral.Name = "groupboxGeneral";
            this.groupboxGeneral.Size = new System.Drawing.Size(443, 181);
            this.groupboxGeneral.TabIndex = 6;
            this.groupboxGeneral.Text = "General";
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.comboBoxLightType);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelLightType);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelPos);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelFiducialProbe);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.inverseResult);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.probePosR);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelSize);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.comboFiducialProbe);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.probeWidth);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelR);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.probePosX);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelX);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.probePosY);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelY);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelH);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelW);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.probeHeight);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 23);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(437, 155);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // groupboxFilter
            // 
            this.groupboxFilter.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.groupboxFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupboxFilter.Expanded = false;
            this.groupboxFilter.ExpandedSize = new System.Drawing.Size(443, 21);
            this.groupboxFilter.Location = new System.Drawing.Point(0, 181);
            this.groupboxFilter.Name = "groupboxFilter";
            this.groupboxFilter.Size = new System.Drawing.Size(443, 25);
            this.groupboxFilter.TabIndex = 7;
            this.groupboxFilter.Text = "Filter";
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.buttonFilterUp);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.filterListBox);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.buttonFilterDown);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.imageBand);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.buttonDeleteFilter);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.buttonAddFilter);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.imageBandLabel);
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            this.ultraExpandableGroupBoxPanel2.Visible = false;
            // 
            // groupboxInspection
            // 
            this.groupboxInspection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupboxInspection.Location = new System.Drawing.Point(0, 206);
            this.groupboxInspection.Name = "groupboxInspection";
            this.groupboxInspection.Size = new System.Drawing.Size(443, 384);
            this.groupboxInspection.TabIndex = 8;
            this.groupboxInspection.Text = "Inspection";
            // 
            // VisionParamControl
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.groupboxInspection);
            this.Controls.Add(this.groupboxFilter);
            this.Controls.Add(this.groupboxGeneral);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VisionParamControl";
            this.Size = new System.Drawing.Size(443, 590);
            this.Load += new System.EventHandler(this.VisionParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxGeneral)).EndInit();
            this.groupboxGeneral.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxFilter)).EndInit();
            this.groupboxFilter.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxInspection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown probeHeight;
        private System.Windows.Forms.NumericUpDown probeWidth;
        private System.Windows.Forms.CheckBox inverseResult;
        private System.Windows.Forms.NumericUpDown probePosX;
        private System.Windows.Forms.NumericUpDown probePosY;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelPos;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label imageBandLabel;
        private System.Windows.Forms.ComboBox imageBand;
        private System.Windows.Forms.ComboBox comboFiducialProbe;
        private System.Windows.Forms.Label labelFiducialProbe;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAddFilter;
        private System.Windows.Forms.Label labelLightType;
        private System.Windows.Forms.NumericUpDown probePosR;
        private System.Windows.Forms.Label labelR;
        private System.Windows.Forms.Button buttonFilterUp;
        private System.Windows.Forms.Button buttonFilterDown;
        private System.Windows.Forms.ListBox filterListBox;
        private System.Windows.Forms.Button buttonAddFilter;
        private System.Windows.Forms.Button buttonDeleteFilter;
        private System.Windows.Forms.ComboBox comboBoxLightType;
        private Infragistics.Win.Misc.UltraExpandableGroupBox groupboxGeneral;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox groupboxFilter;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private Infragistics.Win.Misc.UltraGroupBox groupboxInspection;
    }
}