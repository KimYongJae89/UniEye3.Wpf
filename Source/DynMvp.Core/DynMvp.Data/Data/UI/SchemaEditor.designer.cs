namespace DynMvp.Data.UI
{
    partial class SchemaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaEditor));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.schemaViewPanel = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelObjectTree = new System.Windows.Forms.Panel();
            this.panelTargetFigureProperty = new System.Windows.Forms.Panel();
            this.shapeSize = new System.Windows.Forms.NumericUpDown();
            this.targetShape = new System.Windows.Forms.ComboBox();
            this.labelShapeSize = new System.Windows.Forms.Label();
            this.labelTargetShape = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.autoSchemaButton = new System.Windows.Forms.Button();
            this.autoFit = new System.Windows.Forms.CheckBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.scale = new System.Windows.Forms.ComboBox();
            this.addImageButton = new System.Windows.Forms.Button();
            this.addTextButton = new System.Windows.Forms.Button();
            this.addRectangleButton = new System.Windows.Forms.Button();
            this.addCircleButton = new System.Windows.Forms.Button();
            this.addLineButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelTargetFigureProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSize)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SuspendLayout();
            // 
            // schemaViewPanel
            // 
            this.schemaViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaViewPanel.Location = new System.Drawing.Point(0, 0);
            this.schemaViewPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.schemaViewPanel.Name = "schemaViewPanel";
            this.schemaViewPanel.Size = new System.Drawing.Size(623, 430);
            this.schemaViewPanel.TabIndex = 2;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(1, 74);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelObjectTree);
            this.splitContainer.Panel1.Controls.Add(this.panelTargetFigureProperty);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.schemaViewPanel);
            this.splitContainer.Size = new System.Drawing.Size(910, 430);
            this.splitContainer.SplitterDistance = 283;
            this.splitContainer.TabIndex = 3;
            // 
            // panelObjectTree
            // 
            this.panelObjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelObjectTree.Location = new System.Drawing.Point(0, 81);
            this.panelObjectTree.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelObjectTree.Name = "panelObjectTree";
            this.panelObjectTree.Size = new System.Drawing.Size(283, 349);
            this.panelObjectTree.TabIndex = 1;
            // 
            // panelTargetFigureProperty
            // 
            this.panelTargetFigureProperty.Controls.Add(this.shapeSize);
            this.panelTargetFigureProperty.Controls.Add(this.targetShape);
            this.panelTargetFigureProperty.Controls.Add(this.labelShapeSize);
            this.panelTargetFigureProperty.Controls.Add(this.labelTargetShape);
            this.panelTargetFigureProperty.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTargetFigureProperty.Location = new System.Drawing.Point(0, 0);
            this.panelTargetFigureProperty.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelTargetFigureProperty.Name = "panelTargetFigureProperty";
            this.panelTargetFigureProperty.Size = new System.Drawing.Size(283, 81);
            this.panelTargetFigureProperty.TabIndex = 0;
            // 
            // shapeSize
            // 
            this.shapeSize.Location = new System.Drawing.Point(117, 44);
            this.shapeSize.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.shapeSize.Name = "shapeSize";
            this.shapeSize.Size = new System.Drawing.Size(58, 27);
            this.shapeSize.TabIndex = 2;
            this.shapeSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // targetShape
            // 
            this.targetShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetShape.FormattingEnabled = true;
            this.targetShape.Items.AddRange(new object[] {
            "Standard",
            "Rectangle",
            "Circle"});
            this.targetShape.Location = new System.Drawing.Point(117, 10);
            this.targetShape.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.targetShape.Name = "targetShape";
            this.targetShape.Size = new System.Drawing.Size(127, 28);
            this.targetShape.TabIndex = 1;
            // 
            // labelShapeSize
            // 
            this.labelShapeSize.AutoSize = true;
            this.labelShapeSize.Location = new System.Drawing.Point(10, 46);
            this.labelShapeSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelShapeSize.Name = "labelShapeSize";
            this.labelShapeSize.Size = new System.Drawing.Size(83, 20);
            this.labelShapeSize.TabIndex = 0;
            this.labelShapeSize.Text = "Shape Size";
            // 
            // labelTargetShape
            // 
            this.labelTargetShape.AutoSize = true;
            this.labelTargetShape.Location = new System.Drawing.Point(10, 13);
            this.labelTargetShape.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTargetShape.Name = "labelTargetShape";
            this.labelTargetShape.Size = new System.Drawing.Size(99, 20);
            this.labelTargetShape.TabIndex = 0;
            this.labelTargetShape.Text = "Target Shape";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelTop.Controls.Add(this.autoSchemaButton);
            this.panelTop.Controls.Add(this.autoFit);
            this.panelTop.Controls.Add(this.labelPercent);
            this.panelTop.Controls.Add(this.labelScale);
            this.panelTop.Controls.Add(this.scale);
            this.panelTop.Controls.Add(this.addImageButton);
            this.panelTop.Controls.Add(this.addTextButton);
            this.panelTop.Controls.Add(this.addRectangleButton);
            this.panelTop.Controls.Add(this.addCircleButton);
            this.panelTop.Controls.Add(this.addLineButton);
            this.panelTop.Controls.Add(this.refreshButton);
            this.panelTop.Controls.Add(this.saveButton);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(1, 31);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(910, 43);
            this.panelTop.TabIndex = 108;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelTop_MouseDown);
            // 
            // autoSchemaButton
            // 
            this.autoSchemaButton.AutoSize = true;
            this.autoSchemaButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.autoSchemaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.autoSchemaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.autoSchemaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.autoSchemaButton.Image = global::DynMvp.Data.Properties.Resources.auto;
            this.autoSchemaButton.Location = new System.Drawing.Point(280, 0);
            this.autoSchemaButton.Margin = new System.Windows.Forms.Padding(2);
            this.autoSchemaButton.Name = "autoSchemaButton";
            this.autoSchemaButton.Size = new System.Drawing.Size(40, 43);
            this.autoSchemaButton.TabIndex = 145;
            this.toolTip.SetToolTip(this.autoSchemaButton, "Auto Build");
            this.autoSchemaButton.UseVisualStyleBackColor = true;
            this.autoSchemaButton.Click += new System.EventHandler(this.AutoSchemaButton_Click);
            // 
            // autoFit
            // 
            this.autoFit.AutoSize = true;
            this.autoFit.ForeColor = System.Drawing.Color.White;
            this.autoFit.Location = new System.Drawing.Point(489, 10);
            this.autoFit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.autoFit.Name = "autoFit";
            this.autoFit.Size = new System.Drawing.Size(82, 24);
            this.autoFit.TabIndex = 144;
            this.autoFit.Text = "Auto Fit";
            this.autoFit.UseVisualStyleBackColor = true;
            this.autoFit.CheckedChanged += new System.EventHandler(this.AutoFit_CheckedChanged);
            // 
            // labelPercent
            // 
            this.labelPercent.AutoSize = true;
            this.labelPercent.ForeColor = System.Drawing.Color.White;
            this.labelPercent.Location = new System.Drawing.Point(458, 11);
            this.labelPercent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(22, 20);
            this.labelPercent.TabIndex = 142;
            this.labelPercent.Text = "%";
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.ForeColor = System.Drawing.Color.White;
            this.labelScale.Location = new System.Drawing.Point(344, 11);
            this.labelScale.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(44, 20);
            this.labelScale.TabIndex = 142;
            this.labelScale.Text = "Scale";
            // 
            // scale
            // 
            this.scale.FormattingEnabled = true;
            this.scale.Items.AddRange(new object[] {
            "800",
            "400",
            "200",
            "100",
            "50",
            "25"});
            this.scale.Location = new System.Drawing.Point(393, 8);
            this.scale.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(61, 28);
            this.scale.TabIndex = 141;
            this.scale.SelectedIndexChanged += new System.EventHandler(this.Scale_SelectedIndexChanged);
            this.scale.TextChanged += new System.EventHandler(this.Scale_TextChanged);
            // 
            // addImageButton
            // 
            this.addImageButton.AutoSize = true;
            this.addImageButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.addImageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addImageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.addImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addImageButton.Image = global::DynMvp.Data.Properties.Resources.Image;
            this.addImageButton.Location = new System.Drawing.Point(240, 0);
            this.addImageButton.Margin = new System.Windows.Forms.Padding(2);
            this.addImageButton.Name = "addImageButton";
            this.addImageButton.Size = new System.Drawing.Size(40, 43);
            this.addImageButton.TabIndex = 138;
            this.toolTip.SetToolTip(this.addImageButton, "Add Image");
            this.addImageButton.UseVisualStyleBackColor = true;
            this.addImageButton.Click += new System.EventHandler(this.AddImageButton_Click);
            // 
            // addTextButton
            // 
            this.addTextButton.AutoSize = true;
            this.addTextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.addTextButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addTextButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.addTextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTextButton.Image = ((System.Drawing.Image)(resources.GetObject("addTextButton.Image")));
            this.addTextButton.Location = new System.Drawing.Point(200, 0);
            this.addTextButton.Margin = new System.Windows.Forms.Padding(2);
            this.addTextButton.Name = "addTextButton";
            this.addTextButton.Size = new System.Drawing.Size(40, 43);
            this.addTextButton.TabIndex = 143;
            this.toolTip.SetToolTip(this.addTextButton, "Add Rectangle");
            this.addTextButton.UseVisualStyleBackColor = true;
            this.addTextButton.Click += new System.EventHandler(this.AddTextButton_Click);
            // 
            // addRectangleButton
            // 
            this.addRectangleButton.AutoSize = true;
            this.addRectangleButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.addRectangleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addRectangleButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.addRectangleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRectangleButton.Image = ((System.Drawing.Image)(resources.GetObject("addRectangleButton.Image")));
            this.addRectangleButton.Location = new System.Drawing.Point(160, 0);
            this.addRectangleButton.Margin = new System.Windows.Forms.Padding(2);
            this.addRectangleButton.Name = "addRectangleButton";
            this.addRectangleButton.Size = new System.Drawing.Size(40, 43);
            this.addRectangleButton.TabIndex = 137;
            this.toolTip.SetToolTip(this.addRectangleButton, "Add Rectangle");
            this.addRectangleButton.UseVisualStyleBackColor = true;
            this.addRectangleButton.Click += new System.EventHandler(this.AddRectangleButton_Click);
            // 
            // addCircleButton
            // 
            this.addCircleButton.AutoSize = true;
            this.addCircleButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.addCircleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addCircleButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.addCircleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addCircleButton.Image = ((System.Drawing.Image)(resources.GetObject("addCircleButton.Image")));
            this.addCircleButton.Location = new System.Drawing.Point(120, 0);
            this.addCircleButton.Margin = new System.Windows.Forms.Padding(2);
            this.addCircleButton.Name = "addCircleButton";
            this.addCircleButton.Size = new System.Drawing.Size(40, 43);
            this.addCircleButton.TabIndex = 136;
            this.toolTip.SetToolTip(this.addCircleButton, "Add Circle");
            this.addCircleButton.UseVisualStyleBackColor = true;
            this.addCircleButton.Click += new System.EventHandler(this.AddCircleButton_Click);
            // 
            // addLineButton
            // 
            this.addLineButton.AutoSize = true;
            this.addLineButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.addLineButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addLineButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.addLineButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addLineButton.Image = ((System.Drawing.Image)(resources.GetObject("addLineButton.Image")));
            this.addLineButton.Location = new System.Drawing.Point(80, 0);
            this.addLineButton.Margin = new System.Windows.Forms.Padding(2);
            this.addLineButton.Name = "addLineButton";
            this.addLineButton.Size = new System.Drawing.Size(40, 43);
            this.addLineButton.TabIndex = 135;
            this.toolTip.SetToolTip(this.addLineButton, "Add Line");
            this.addLineButton.UseVisualStyleBackColor = true;
            this.addLineButton.Click += new System.EventHandler(this.AddLineButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = true;
            this.refreshButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.refreshButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.refreshButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Image = global::DynMvp.Data.Properties.Resources.refresh;
            this.refreshButton.Location = new System.Drawing.Point(40, 0);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(40, 43);
            this.refreshButton.TabIndex = 134;
            this.toolTip.SetToolTip(this.refreshButton, "Refresh Names");
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.AutoSize = true;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.saveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.saveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Image = global::DynMvp.Data.Properties.Resources.save;
            this.saveButton.Location = new System.Drawing.Point(0, 0);
            this.saveButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(40, 43);
            this.saveButton.TabIndex = 133;
            this.toolTip.SetToolTip(this.saveButton, "Save Schema");
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(911, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 473);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 473);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 504);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(912, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(912, 31);
            // 
            // SchemaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 505);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SchemaEditor";
            this.Text = "SchemaEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelSchemaEditor_FormClosing);
            this.Load += new System.EventHandler(this.ModelSchemaEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ModelSchemaEditor_KeyDown);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelTargetFigureProperty.ResumeLayout(false);
            this.panelTargetFigureProperty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSize)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel schemaViewPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button addImageButton;
        private System.Windows.Forms.Button addRectangleButton;
        private System.Windows.Forms.Button addCircleButton;
        private System.Windows.Forms.Button addLineButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.ComboBox scale;
        private System.Windows.Forms.Button addTextButton;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.CheckBox autoFit;
        private System.Windows.Forms.Panel panelTargetFigureProperty;
        private System.Windows.Forms.Panel panelObjectTree;
        private System.Windows.Forms.ComboBox targetShape;
        private System.Windows.Forms.Label labelTargetShape;
        private System.Windows.Forms.NumericUpDown shapeSize;
        private System.Windows.Forms.Label labelShapeSize;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button autoSchemaButton;
    }
}