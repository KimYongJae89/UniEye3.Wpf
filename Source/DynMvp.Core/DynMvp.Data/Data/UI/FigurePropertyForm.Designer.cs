namespace DynMvp.Data.UI
{
    partial class FigurePropertyForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.alignmentLabel = new System.Windows.Forms.Label();
            this.alignmentCombo = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.labelBackgroundColor = new System.Windows.Forms.Label();
            this.labelLineColor = new System.Windows.Forms.Label();
            this.lineThicknessEdit = new System.Windows.Forms.NumericUpDown();
            this.lineThicknessLabel = new System.Windows.Forms.Label();
            this.labelTextColor = new System.Windows.Forms.Label();
            this.lineColorBox = new System.Windows.Forms.Label();
            this.lineColorButton = new System.Windows.Forms.Button();
            this.backgroundColorBox = new System.Windows.Forms.Label();
            this.backgroundColorButton = new System.Windows.Forms.Button();
            this.textColorBox = new System.Windows.Forms.Label();
            this.textColorButton = new System.Windows.Forms.Button();
            this.labelFont = new System.Windows.Forms.Label();
            this.buttonSelectFont = new System.Windows.Forms.Button();
            this.labelSampleText = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.txtPositionX = new System.Windows.Forms.TextBox();
            this.txtPositionY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager();
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.FigurePropertyForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.lineThicknessEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.FigurePropertyForm_Fill_Panel.ClientArea.SuspendLayout();
            this.FigurePropertyForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // alignmentLabel
            // 
            this.alignmentLabel.AutoSize = true;
            this.alignmentLabel.Location = new System.Drawing.Point(14, 219);
            this.alignmentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.alignmentLabel.Name = "alignmentLabel";
            this.alignmentLabel.Size = new System.Drawing.Size(72, 18);
            this.alignmentLabel.TabIndex = 0;
            this.alignmentLabel.Text = "Alignment";
            // 
            // alignmentCombo
            // 
            this.alignmentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alignmentCombo.FormattingEnabled = true;
            this.alignmentCombo.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right"});
            this.alignmentCombo.Location = new System.Drawing.Point(150, 216);
            this.alignmentCombo.Name = "alignmentCombo";
            this.alignmentCombo.Size = new System.Drawing.Size(158, 26);
            this.alignmentCombo.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(74, 246);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 33);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(165, 246);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(86, 33);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // labelBackgroundColor
            // 
            this.labelBackgroundColor.AutoSize = true;
            this.labelBackgroundColor.Location = new System.Drawing.Point(14, 123);
            this.labelBackgroundColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBackgroundColor.Name = "labelBackgroundColor";
            this.labelBackgroundColor.Size = new System.Drawing.Size(129, 18);
            this.labelBackgroundColor.TabIndex = 0;
            this.labelBackgroundColor.Text = "Background Color";
            // 
            // labelLineColor
            // 
            this.labelLineColor.AutoSize = true;
            this.labelLineColor.Location = new System.Drawing.Point(14, 71);
            this.labelLineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLineColor.Name = "labelLineColor";
            this.labelLineColor.Size = new System.Drawing.Size(76, 18);
            this.labelLineColor.TabIndex = 0;
            this.labelLineColor.Text = "Line Color";
            // 
            // lineThicknessEdit
            // 
            this.lineThicknessEdit.Location = new System.Drawing.Point(150, 94);
            this.lineThicknessEdit.Margin = new System.Windows.Forms.Padding(6);
            this.lineThicknessEdit.Name = "lineThicknessEdit";
            this.lineThicknessEdit.Size = new System.Drawing.Size(75, 24);
            this.lineThicknessEdit.TabIndex = 8;
            // 
            // lineThicknessLabel
            // 
            this.lineThicknessLabel.AutoSize = true;
            this.lineThicknessLabel.Location = new System.Drawing.Point(14, 97);
            this.lineThicknessLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lineThicknessLabel.Name = "lineThicknessLabel";
            this.lineThicknessLabel.Size = new System.Drawing.Size(76, 18);
            this.lineThicknessLabel.TabIndex = 7;
            this.lineThicknessLabel.Text = "Thickness";
            // 
            // labelTextColor
            // 
            this.labelTextColor.AutoSize = true;
            this.labelTextColor.Location = new System.Drawing.Point(14, 148);
            this.labelTextColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTextColor.Name = "labelTextColor";
            this.labelTextColor.Size = new System.Drawing.Size(77, 18);
            this.labelTextColor.TabIndex = 9;
            this.labelTextColor.Text = "Text Color";
            // 
            // lineColorBox
            // 
            this.lineColorBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineColorBox.Location = new System.Drawing.Point(150, 69);
            this.lineColorBox.Name = "lineColorBox";
            this.lineColorBox.Size = new System.Drawing.Size(79, 22);
            this.lineColorBox.TabIndex = 11;
            // 
            // lineColorButton
            // 
            this.lineColorButton.Location = new System.Drawing.Point(233, 69);
            this.lineColorButton.Name = "lineColorButton";
            this.lineColorButton.Size = new System.Drawing.Size(36, 22);
            this.lineColorButton.TabIndex = 12;
            this.lineColorButton.Text = "...";
            this.lineColorButton.UseVisualStyleBackColor = true;
            this.lineColorButton.Click += new System.EventHandler(this.lineColorButton_Click);
            // 
            // backgroundColorBox
            // 
            this.backgroundColorBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.backgroundColorBox.Location = new System.Drawing.Point(150, 121);
            this.backgroundColorBox.Name = "backgroundColorBox";
            this.backgroundColorBox.Size = new System.Drawing.Size(79, 22);
            this.backgroundColorBox.TabIndex = 11;
            // 
            // backgroundColorButton
            // 
            this.backgroundColorButton.Location = new System.Drawing.Point(233, 121);
            this.backgroundColorButton.Name = "backgroundColorButton";
            this.backgroundColorButton.Size = new System.Drawing.Size(36, 22);
            this.backgroundColorButton.TabIndex = 12;
            this.backgroundColorButton.Text = "...";
            this.backgroundColorButton.UseVisualStyleBackColor = true;
            this.backgroundColorButton.Click += new System.EventHandler(this.backgroundColorButton_Click);
            // 
            // textColorBox
            // 
            this.textColorBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textColorBox.Location = new System.Drawing.Point(150, 146);
            this.textColorBox.Name = "textColorBox";
            this.textColorBox.Size = new System.Drawing.Size(79, 22);
            this.textColorBox.TabIndex = 11;
            // 
            // textColorButton
            // 
            this.textColorButton.Location = new System.Drawing.Point(233, 146);
            this.textColorButton.Name = "textColorButton";
            this.textColorButton.Size = new System.Drawing.Size(36, 22);
            this.textColorButton.TabIndex = 12;
            this.textColorButton.Text = "...";
            this.textColorButton.UseVisualStyleBackColor = true;
            this.textColorButton.Click += new System.EventHandler(this.textColorButton_Click);
            // 
            // labelFont
            // 
            this.labelFont.AutoSize = true;
            this.labelFont.Location = new System.Drawing.Point(14, 174);
            this.labelFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFont.Name = "labelFont";
            this.labelFont.Size = new System.Drawing.Size(38, 18);
            this.labelFont.TabIndex = 0;
            this.labelFont.Text = "Font";
            // 
            // buttonSelectFont
            // 
            this.buttonSelectFont.Location = new System.Drawing.Point(275, 170);
            this.buttonSelectFont.Name = "buttonSelectFont";
            this.buttonSelectFont.Size = new System.Drawing.Size(35, 26);
            this.buttonSelectFont.TabIndex = 13;
            this.buttonSelectFont.Text = "...";
            this.buttonSelectFont.UseVisualStyleBackColor = true;
            this.buttonSelectFont.Click += new System.EventHandler(this.buttonSelectFont_Click);
            // 
            // labelSampleText
            // 
            this.labelSampleText.BackColor = System.Drawing.Color.White;
            this.labelSampleText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSampleText.Location = new System.Drawing.Point(150, 170);
            this.labelSampleText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSampleText.Name = "labelSampleText";
            this.labelSampleText.Size = new System.Drawing.Size(122, 43);
            this.labelSampleText.TabIndex = 0;
            this.labelSampleText.Text = "AaBb123";
            this.labelSampleText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(170, 42);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(59, 24);
            this.txtWidth.TabIndex = 16;
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(251, 42);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(59, 24);
            this.txtHeight.TabIndex = 17;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(14, 45);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(37, 18);
            this.labelWidth.TabIndex = 14;
            this.labelWidth.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "W";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(231, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 18);
            this.label2.TabIndex = 18;
            this.label2.Text = "H";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(14, 18);
            this.labelPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(62, 18);
            this.labelPosition.TabIndex = 14;
            this.labelPosition.Text = "Position";
            // 
            // txtPositionX
            // 
            this.txtPositionX.Location = new System.Drawing.Point(172, 12);
            this.txtPositionX.Name = "txtPositionX";
            this.txtPositionX.Size = new System.Drawing.Size(59, 24);
            this.txtPositionX.TabIndex = 16;
            // 
            // txtPositionY
            // 
            this.txtPositionY.Location = new System.Drawing.Point(251, 12);
            this.txtPositionY.Name = "txtPositionY";
            this.txtPositionY.Size = new System.Drawing.Size(59, 24);
            this.txtPositionY.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 18);
            this.label4.TabIndex = 18;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(231, 15);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = "Y";
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(325, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 317);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(325, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 286);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(324, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 286);
            // 
            // FigurePropertyForm_Fill_Panel
            // 
            // 
            // FigurePropertyForm_Fill_Panel.ClientArea
            // 
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label5);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label2);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label4);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.label1);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtPositionY);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtPositionX);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtHeight);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelPosition);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.txtWidth);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelWidth);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.buttonSelectFont);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.textColorButton);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.textColorBox);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.backgroundColorButton);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.backgroundColorBox);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.lineColorButton);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.lineColorBox);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelTextColor);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.lineThicknessEdit);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.lineThicknessLabel);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.alignmentCombo);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelLineColor);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelSampleText);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelFont);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.labelBackgroundColor);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.alignmentLabel);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.cancelButton);
            this.FigurePropertyForm_Fill_Panel.ClientArea.Controls.Add(this.okButton);
            this.FigurePropertyForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FigurePropertyForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FigurePropertyForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.FigurePropertyForm_Fill_Panel.Name = "FigurePropertyForm_Fill_Panel";
            this.FigurePropertyForm_Fill_Panel.Size = new System.Drawing.Size(323, 286);
            this.FigurePropertyForm_Fill_Panel.TabIndex = 27;
            // 
            // FigurePropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 318);
            this.Controls.Add(this.FigurePropertyForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FigurePropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Figure Property";
            this.Load += new System.EventHandler(this.FigurePropertyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lineThicknessEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.FigurePropertyForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.FigurePropertyForm_Fill_Panel.ClientArea.PerformLayout();
            this.FigurePropertyForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label alignmentLabel;
        private System.Windows.Forms.ComboBox alignmentCombo;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelBackgroundColor;
        private System.Windows.Forms.Label labelLineColor;
        private System.Windows.Forms.NumericUpDown lineThicknessEdit;
        private System.Windows.Forms.Label lineThicknessLabel;
        private System.Windows.Forms.Label labelTextColor;
        private System.Windows.Forms.Label lineColorBox;
        private System.Windows.Forms.Button lineColorButton;
        private System.Windows.Forms.Label backgroundColorBox;
        private System.Windows.Forms.Button backgroundColorButton;
        private System.Windows.Forms.Label textColorBox;
        private System.Windows.Forms.Button textColorButton;
        private System.Windows.Forms.Label labelFont;
        private System.Windows.Forms.Button buttonSelectFont;
        private System.Windows.Forms.Label labelSampleText;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox txtPositionX;
        private System.Windows.Forms.TextBox txtPositionY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel FigurePropertyForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
    }
}