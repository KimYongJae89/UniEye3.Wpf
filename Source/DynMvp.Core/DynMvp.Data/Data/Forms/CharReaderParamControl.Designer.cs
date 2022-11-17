namespace DynMvp.Data.Forms
{
    partial class CharReaderParamControl
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
            this.maxWidth = new System.Windows.Forms.NumericUpDown();
            this.maxHeight = new System.Windows.Forms.NumericUpDown();
            this.minWidth = new System.Windows.Forms.NumericUpDown();
            this.minHeight = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelTilda1 = new System.Windows.Forms.Label();
            this.groupBoxCharactorSize = new System.Windows.Forms.GroupBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPolarity = new System.Windows.Forms.Label();
            this.polarity = new System.Windows.Forms.ComboBox();
            this.desiredString = new System.Windows.Forms.TextBox();
            this.labelDesiredString = new System.Windows.Forms.Label();
            this.labelFontFile = new System.Windows.Forms.Label();
            this.fontFileName = new System.Windows.Forms.TextBox();
            this.selectFontFile = new System.Windows.Forms.Button();
            this.trainButton = new System.Windows.Forms.Button();
            this.fontGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxFont = new System.Windows.Forms.GroupBox();
            this.extractFontButton = new System.Windows.Forms.Button();
            this.showFontbutton = new System.Windows.Forms.Button();
            this.autoTuneButton = new System.Windows.Forms.Button();
            this.xOverlapRatio = new System.Windows.Forms.NumericUpDown();
            this.labelXOverlap = new System.Windows.Forms.Label();
            this.threshold = new System.Windows.Forms.NumericUpDown();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.labelPct = new System.Windows.Forms.Label();
            this.addThresholdButton = new System.Windows.Forms.Button();
            this.thresholdList = new System.Windows.Forms.ListBox();
            this.deleteThresholdButton = new System.Windows.Forms.Button();
            this.labelNumCharacter = new System.Windows.Forms.Label();
            this.desiredNumCharacter = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.maxWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeight)).BeginInit();
            this.groupBoxCharactorSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).BeginInit();
            this.groupBoxFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xOverlapRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desiredNumCharacter)).BeginInit();
            this.SuspendLayout();
            // 
            // maxWidth
            // 
            this.maxWidth.Location = new System.Drawing.Point(181, 27);
            this.maxWidth.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.maxWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxWidth.Name = "maxWidth";
            this.maxWidth.Size = new System.Drawing.Size(66, 26);
            this.maxWidth.TabIndex = 19;
            this.maxWidth.ValueChanged += new System.EventHandler(this.maxWidth_ValueChanged);
            // 
            // maxHeight
            // 
            this.maxHeight.Location = new System.Drawing.Point(436, 27);
            this.maxHeight.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.maxHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxHeight.Name = "maxHeight";
            this.maxHeight.Size = new System.Drawing.Size(66, 26);
            this.maxHeight.TabIndex = 20;
            this.maxHeight.ValueChanged += new System.EventHandler(this.maxHeight_ValueChanged);
            // 
            // minWidth
            // 
            this.minWidth.Location = new System.Drawing.Point(89, 27);
            this.minWidth.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.minWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minWidth.Name = "minWidth";
            this.minWidth.Size = new System.Drawing.Size(66, 26);
            this.minWidth.TabIndex = 15;
            this.minWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minWidth.ValueChanged += new System.EventHandler(this.minWidth_ValueChanged);
            // 
            // minHeight
            // 
            this.minHeight.Location = new System.Drawing.Point(339, 26);
            this.minHeight.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.minHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minHeight.Name = "minHeight";
            this.minHeight.Size = new System.Drawing.Size(66, 26);
            this.minHeight.TabIndex = 16;
            this.minHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minHeight.ValueChanged += new System.EventHandler(this.minHeight_ValueChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(22, 28);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(50, 20);
            this.labelWidth.TabIndex = 12;
            this.labelWidth.Text = "Width";
            // 
            // labelTilda1
            // 
            this.labelTilda1.AutoSize = true;
            this.labelTilda1.Location = new System.Drawing.Point(159, 28);
            this.labelTilda1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTilda1.Name = "labelTilda1";
            this.labelTilda1.Size = new System.Drawing.Size(18, 20);
            this.labelTilda1.TabIndex = 14;
            this.labelTilda1.Text = "~";
            // 
            // groupBoxCharactorSize
            // 
            this.groupBoxCharactorSize.Controls.Add(this.labelHeight);
            this.groupBoxCharactorSize.Controls.Add(this.maxHeight);
            this.groupBoxCharactorSize.Controls.Add(this.labelWidth);
            this.groupBoxCharactorSize.Controls.Add(this.minHeight);
            this.groupBoxCharactorSize.Controls.Add(this.maxWidth);
            this.groupBoxCharactorSize.Controls.Add(this.minWidth);
            this.groupBoxCharactorSize.Controls.Add(this.label1);
            this.groupBoxCharactorSize.Controls.Add(this.labelTilda1);
            this.groupBoxCharactorSize.Location = new System.Drawing.Point(10, 9);
            this.groupBoxCharactorSize.Name = "groupBoxCharactorSize";
            this.groupBoxCharactorSize.Size = new System.Drawing.Size(513, 65);
            this.groupBoxCharactorSize.TabIndex = 21;
            this.groupBoxCharactorSize.TabStop = false;
            this.groupBoxCharactorSize.Text = "Charactor Size";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(273, 28);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(56, 20);
            this.labelHeight.TabIndex = 12;
            this.labelHeight.Text = "Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(411, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "~";
            // 
            // labelPolarity
            // 
            this.labelPolarity.AutoSize = true;
            this.labelPolarity.Location = new System.Drawing.Point(19, 108);
            this.labelPolarity.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelPolarity.Name = "labelPolarity";
            this.labelPolarity.Size = new System.Drawing.Size(95, 20);
            this.labelPolarity.TabIndex = 12;
            this.labelPolarity.Text = "Background";
            // 
            // polarity
            // 
            this.polarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.polarity.FormattingEnabled = true;
            this.polarity.Items.AddRange(new object[] {
            "White",
            "Black"});
            this.polarity.Location = new System.Drawing.Point(128, 104);
            this.polarity.Name = "polarity";
            this.polarity.Size = new System.Drawing.Size(174, 28);
            this.polarity.TabIndex = 22;
            this.polarity.SelectedIndexChanged += new System.EventHandler(this.polarity_SelectedIndexChanged);
            // 
            // desiredString
            // 
            this.desiredString.Location = new System.Drawing.Point(150, 188);
            this.desiredString.Name = "desiredString";
            this.desiredString.Size = new System.Drawing.Size(247, 26);
            this.desiredString.TabIndex = 23;
            this.desiredString.TextChanged += new System.EventHandler(this.desiredString_TextChanged);
            // 
            // labelDesiredString
            // 
            this.labelDesiredString.AutoSize = true;
            this.labelDesiredString.Location = new System.Drawing.Point(19, 191);
            this.labelDesiredString.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDesiredString.Name = "labelDesiredString";
            this.labelDesiredString.Size = new System.Drawing.Size(110, 20);
            this.labelDesiredString.TabIndex = 12;
            this.labelDesiredString.Text = "Desired String";
            // 
            // labelFontFile
            // 
            this.labelFontFile.AutoSize = true;
            this.labelFontFile.Location = new System.Drawing.Point(9, 27);
            this.labelFontFile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelFontFile.Name = "labelFontFile";
            this.labelFontFile.Size = new System.Drawing.Size(71, 20);
            this.labelFontFile.TabIndex = 12;
            this.labelFontFile.Text = "Font File";
            // 
            // fontFileName
            // 
            this.fontFileName.Enabled = false;
            this.fontFileName.Location = new System.Drawing.Point(140, 24);
            this.fontFileName.Name = "fontFileName";
            this.fontFileName.Size = new System.Drawing.Size(247, 26);
            this.fontFileName.TabIndex = 23;
            this.fontFileName.TextChanged += new System.EventHandler(this.desiredString_TextChanged);
            // 
            // selectFontFile
            // 
            this.selectFontFile.Location = new System.Drawing.Point(394, 24);
            this.selectFontFile.Name = "selectFontFile";
            this.selectFontFile.Size = new System.Drawing.Size(37, 26);
            this.selectFontFile.TabIndex = 25;
            this.selectFontFile.Text = "...";
            this.selectFontFile.UseVisualStyleBackColor = true;
            this.selectFontFile.Click += new System.EventHandler(this.selectFontFile_Click);
            // 
            // trainButton
            // 
            this.trainButton.Location = new System.Drawing.Point(438, 23);
            this.trainButton.Name = "trainButton";
            this.trainButton.Size = new System.Drawing.Size(63, 26);
            this.trainButton.TabIndex = 25;
            this.trainButton.Text = "Train";
            this.trainButton.UseVisualStyleBackColor = true;
            this.trainButton.Click += new System.EventHandler(this.trainButton_Click);
            // 
            // fontGrid
            // 
            this.fontGrid.AllowUserToAddRows = false;
            this.fontGrid.AllowUserToDeleteRows = false;
            this.fontGrid.AllowUserToResizeColumns = false;
            this.fontGrid.AllowUserToResizeRows = false;
            this.fontGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fontGrid.ColumnHeadersVisible = false;
            this.fontGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.fontGrid.Location = new System.Drawing.Point(12, 54);
            this.fontGrid.Name = "fontGrid";
            this.fontGrid.RowHeadersVisible = false;
            this.fontGrid.RowTemplate.Height = 60;
            this.fontGrid.Size = new System.Drawing.Size(489, 80);
            this.fontGrid.TabIndex = 26;
            this.fontGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.fontGrid_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // groupBoxFont
            // 
            this.groupBoxFont.Controls.Add(this.fontGrid);
            this.groupBoxFont.Controls.Add(this.trainButton);
            this.groupBoxFont.Controls.Add(this.fontFileName);
            this.groupBoxFont.Controls.Add(this.extractFontButton);
            this.groupBoxFont.Controls.Add(this.showFontbutton);
            this.groupBoxFont.Controls.Add(this.selectFontFile);
            this.groupBoxFont.Controls.Add(this.labelFontFile);
            this.groupBoxFont.Location = new System.Drawing.Point(10, 216);
            this.groupBoxFont.Name = "groupBoxFont";
            this.groupBoxFont.Size = new System.Drawing.Size(513, 184);
            this.groupBoxFont.TabIndex = 27;
            this.groupBoxFont.TabStop = false;
            this.groupBoxFont.Text = "Font";
            // 
            // extractFontButton
            // 
            this.extractFontButton.Location = new System.Drawing.Point(13, 140);
            this.extractFontButton.Name = "extractFontButton";
            this.extractFontButton.Size = new System.Drawing.Size(140, 30);
            this.extractFontButton.TabIndex = 25;
            this.extractFontButton.Text = "Extract Font";
            this.extractFontButton.UseVisualStyleBackColor = true;
            this.extractFontButton.Click += new System.EventHandler(this.extractFontButton_Click);
            // 
            // showFontbutton
            // 
            this.showFontbutton.Location = new System.Drawing.Point(392, 140);
            this.showFontbutton.Name = "showFontbutton";
            this.showFontbutton.Size = new System.Drawing.Size(111, 30);
            this.showFontbutton.TabIndex = 25;
            this.showFontbutton.Text = "Show Font";
            this.showFontbutton.UseVisualStyleBackColor = true;
            this.showFontbutton.Click += new System.EventHandler(this.showFontButton_Click);
            // 
            // autoTuneButton
            // 
            this.autoTuneButton.Location = new System.Drawing.Point(404, 188);
            this.autoTuneButton.Name = "autoTuneButton";
            this.autoTuneButton.Size = new System.Drawing.Size(109, 26);
            this.autoTuneButton.TabIndex = 25;
            this.autoTuneButton.Text = "Auto Tune";
            this.autoTuneButton.UseVisualStyleBackColor = true;
            this.autoTuneButton.Click += new System.EventHandler(this.autoTuneButton_Click);
            // 
            // xOverlapRatio
            // 
            this.xOverlapRatio.Location = new System.Drawing.Point(128, 75);
            this.xOverlapRatio.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.xOverlapRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.xOverlapRatio.Name = "xOverlapRatio";
            this.xOverlapRatio.Size = new System.Drawing.Size(74, 26);
            this.xOverlapRatio.TabIndex = 16;
            this.xOverlapRatio.ValueChanged += new System.EventHandler(this.xOverlapRatio_ValueChanged);
            // 
            // labelXOverlap
            // 
            this.labelXOverlap.AutoSize = true;
            this.labelXOverlap.Location = new System.Drawing.Point(19, 77);
            this.labelXOverlap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelXOverlap.Name = "labelXOverlap";
            this.labelXOverlap.Size = new System.Drawing.Size(78, 20);
            this.labelXOverlap.TabIndex = 12;
            this.labelXOverlap.Text = "X Overlap";
            // 
            // threshold
            // 
            this.threshold.Location = new System.Drawing.Point(396, 75);
            this.threshold.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.threshold.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(74, 26);
            this.threshold.TabIndex = 16;
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(307, 77);
            this.labelThreshold.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(79, 20);
            this.labelThreshold.TabIndex = 12;
            this.labelThreshold.Text = "Threshold";
            // 
            // labelPct
            // 
            this.labelPct.AutoSize = true;
            this.labelPct.Location = new System.Drawing.Point(214, 77);
            this.labelPct.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelPct.Name = "labelPct";
            this.labelPct.Size = new System.Drawing.Size(23, 20);
            this.labelPct.TabIndex = 12;
            this.labelPct.Text = "%";
            // 
            // addThresholdButton
            // 
            this.addThresholdButton.Location = new System.Drawing.Point(320, 104);
            this.addThresholdButton.Name = "addThresholdButton";
            this.addThresholdButton.Size = new System.Drawing.Size(74, 28);
            this.addThresholdButton.TabIndex = 28;
            this.addThresholdButton.Text = "Add";
            this.addThresholdButton.UseVisualStyleBackColor = true;
            this.addThresholdButton.Click += new System.EventHandler(this.addThresholdButton_Click);
            // 
            // thresholdList
            // 
            this.thresholdList.FormattingEnabled = true;
            this.thresholdList.ItemHeight = 20;
            this.thresholdList.Location = new System.Drawing.Point(396, 104);
            this.thresholdList.Name = "thresholdList";
            this.thresholdList.Size = new System.Drawing.Size(125, 64);
            this.thresholdList.TabIndex = 29;
            this.thresholdList.SelectedIndexChanged += new System.EventHandler(this.thresholdList_SelectedIndexChanged);
            // 
            // deleteThresholdButton
            // 
            this.deleteThresholdButton.Location = new System.Drawing.Point(320, 137);
            this.deleteThresholdButton.Name = "deleteThresholdButton";
            this.deleteThresholdButton.Size = new System.Drawing.Size(74, 28);
            this.deleteThresholdButton.TabIndex = 28;
            this.deleteThresholdButton.Text = "Delete";
            this.deleteThresholdButton.UseVisualStyleBackColor = true;
            this.deleteThresholdButton.Click += new System.EventHandler(this.DeleteThresholdButton_Click);
            // 
            // labelNumCharacter
            // 
            this.labelNumCharacter.AutoSize = true;
            this.labelNumCharacter.Location = new System.Drawing.Point(19, 153);
            this.labelNumCharacter.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelNumCharacter.Name = "labelNumCharacter";
            this.labelNumCharacter.Size = new System.Drawing.Size(116, 20);
            this.labelNumCharacter.TabIndex = 12;
            this.labelNumCharacter.Text = "Num Character";
            // 
            // desiredNumCharacter
            // 
            this.desiredNumCharacter.Location = new System.Drawing.Point(150, 151);
            this.desiredNumCharacter.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.desiredNumCharacter.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.desiredNumCharacter.Name = "desiredNumCharacter";
            this.desiredNumCharacter.Size = new System.Drawing.Size(74, 26);
            this.desiredNumCharacter.TabIndex = 16;
            this.desiredNumCharacter.ValueChanged += new System.EventHandler(this.desiredNumCharacter_ValueChanged);
            // 
            // CharReaderParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.thresholdList);
            this.Controls.Add(this.deleteThresholdButton);
            this.Controls.Add(this.addThresholdButton);
            this.Controls.Add(this.labelPct);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.labelXOverlap);
            this.Controls.Add(this.groupBoxFont);
            this.Controls.Add(this.desiredString);
            this.Controls.Add(this.polarity);
            this.Controls.Add(this.desiredNumCharacter);
            this.Controls.Add(this.threshold);
            this.Controls.Add(this.xOverlapRatio);
            this.Controls.Add(this.labelNumCharacter);
            this.Controls.Add(this.labelDesiredString);
            this.Controls.Add(this.labelPolarity);
            this.Controls.Add(this.autoTuneButton);
            this.Controls.Add(this.groupBoxCharactorSize);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "CharReaderParamControl";
            this.Size = new System.Drawing.Size(538, 420);
            ((System.ComponentModel.ISupportInitialize)(this.maxWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeight)).EndInit();
            this.groupBoxCharactorSize.ResumeLayout(false);
            this.groupBoxCharactorSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).EndInit();
            this.groupBoxFont.ResumeLayout(false);
            this.groupBoxFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xOverlapRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desiredNumCharacter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown maxWidth;
        private System.Windows.Forms.NumericUpDown maxHeight;
        private System.Windows.Forms.NumericUpDown minWidth;
        private System.Windows.Forms.NumericUpDown minHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelTilda1;
        private System.Windows.Forms.GroupBox groupBoxCharactorSize;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPolarity;
        private System.Windows.Forms.ComboBox polarity;
        private System.Windows.Forms.TextBox desiredString;
        private System.Windows.Forms.Label labelDesiredString;
        private System.Windows.Forms.Label labelFontFile;
        private System.Windows.Forms.TextBox fontFileName;
        private System.Windows.Forms.Button selectFontFile;
        private System.Windows.Forms.Button trainButton;
        private System.Windows.Forms.DataGridView fontGrid;
        private System.Windows.Forms.GroupBox groupBoxFont;
        private System.Windows.Forms.Button showFontbutton;
        private System.Windows.Forms.Button extractFontButton;
        private System.Windows.Forms.Button autoTuneButton;
        private System.Windows.Forms.NumericUpDown xOverlapRatio;
        private System.Windows.Forms.Label labelXOverlap;
        private System.Windows.Forms.NumericUpDown threshold;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.Label labelPct;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button addThresholdButton;
        private System.Windows.Forms.ListBox thresholdList;
        private System.Windows.Forms.Button deleteThresholdButton;
        private System.Windows.Forms.Label labelNumCharacter;
        private System.Windows.Forms.NumericUpDown desiredNumCharacter;


    }
}