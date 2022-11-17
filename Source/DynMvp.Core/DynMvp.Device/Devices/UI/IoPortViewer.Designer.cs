namespace DynMvp.Devices.UI
{
    partial class IoPortViewer
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.inportTable = new System.Windows.Forms.DataGridView();
            this.ColumnInputNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInputValue = new System.Windows.Forms.DataGridViewImageColumn();
            this.outportTable = new System.Windows.Forms.DataGridView();
            this.ColumnOutputNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelInput = new System.Windows.Forms.Label();
            this.labelOutput = new System.Windows.Forms.Label();
            this.panelDigitalIo = new System.Windows.Forms.Panel();
            this.labelDigitalIo = new System.Windows.Forms.Label();
            this.comboBoxDigitalIo = new System.Windows.Forms.ComboBox();
            this.panelInPortSelector = new System.Windows.Forms.Panel();
            this.comboBoxInPortGroup = new System.Windows.Forms.ComboBox();
            this.labelInPortGroup = new System.Windows.Forms.Label();
            this.panelOutPortSelector = new System.Windows.Forms.Panel();
            this.comboBoxOutPortGroup = new System.Windows.Forms.ComboBox();
            this.labelOutputPortGroup = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.bottomPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLongRun = new System.Windows.Forms.Button();
            this.buttonOneTimeRun = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.timerLongRun = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.timer5 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inportTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outportTable)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.panelDigitalIo.SuspendLayout();
            this.panelInPortSelector.SuspendLayout();
            this.panelOutPortSelector.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SuspendLayout();
            // 
            // inportTable
            // 
            this.inportTable.AllowUserToAddRows = false;
            this.inportTable.AllowUserToDeleteRows = false;
            this.inportTable.AllowUserToResizeColumns = false;
            this.inportTable.AllowUserToResizeRows = false;
            this.inportTable.ColumnHeadersHeight = 40;
            this.inportTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnInputNo,
            this.ColumnInputName,
            this.ColumnInputValue});
            this.inportTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inportTable.Location = new System.Drawing.Point(4, 100);
            this.inportTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inportTable.MultiSelect = false;
            this.inportTable.Name = "inportTable";
            this.inportTable.ReadOnly = true;
            this.inportTable.RowHeadersVisible = false;
            this.inportTable.RowTemplate.Height = 23;
            this.inportTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inportTable.Size = new System.Drawing.Size(523, 465);
            this.inportTable.TabIndex = 0;
            // 
            // ColumnInputNo
            // 
            this.ColumnInputNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnInputNo.HeaderText = "No";
            this.ColumnInputNo.Name = "ColumnInputNo";
            this.ColumnInputNo.ReadOnly = true;
            this.ColumnInputNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnInputNo.Width = 40;
            // 
            // ColumnInputName
            // 
            this.ColumnInputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnInputName.FillWeight = 155.8074F;
            this.ColumnInputName.HeaderText = "Name";
            this.ColumnInputName.Name = "ColumnInputName";
            this.ColumnInputName.ReadOnly = true;
            this.ColumnInputName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnInputValue
            // 
            this.ColumnInputValue.FillWeight = 44.19263F;
            this.ColumnInputValue.HeaderText = "Value";
            this.ColumnInputValue.Name = "ColumnInputValue";
            this.ColumnInputValue.ReadOnly = true;
            this.ColumnInputValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnInputValue.Width = 78;
            // 
            // outportTable
            // 
            this.outportTable.AllowUserToAddRows = false;
            this.outportTable.AllowUserToDeleteRows = false;
            this.outportTable.AllowUserToResizeColumns = false;
            this.outportTable.AllowUserToResizeRows = false;
            this.outportTable.ColumnHeadersHeight = 40;
            this.outportTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOutputNo,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.outportTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outportTable.Location = new System.Drawing.Point(535, 100);
            this.outportTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outportTable.MultiSelect = false;
            this.outportTable.Name = "outportTable";
            this.outportTable.ReadOnly = true;
            this.outportTable.RowHeadersVisible = false;
            this.outportTable.RowTemplate.Height = 23;
            this.outportTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.outportTable.Size = new System.Drawing.Size(523, 465);
            this.outportTable.TabIndex = 0;
            this.outportTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.outportTable_CellClick);
            // 
            // ColumnOutputNo
            // 
            this.ColumnOutputNo.HeaderText = "No";
            this.ColumnOutputNo.Name = "ColumnOutputNo";
            this.ColumnOutputNo.ReadOnly = true;
            this.ColumnOutputNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnOutputNo.Width = 40;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 2;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainPanel.Controls.Add(this.labelInput, 0, 1);
            this.mainPanel.Controls.Add(this.labelOutput, 1, 1);
            this.mainPanel.Controls.Add(this.panelDigitalIo, 0, 0);
            this.mainPanel.Controls.Add(this.inportTable, 0, 3);
            this.mainPanel.Controls.Add(this.outportTable, 1, 3);
            this.mainPanel.Controls.Add(this.panelInPortSelector, 0, 2);
            this.mainPanel.Controls.Add(this.panelOutPortSelector, 1, 2);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(1, 31);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 4;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainPanel.Size = new System.Drawing.Size(1062, 570);
            this.mainPanel.TabIndex = 1;
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.BackColor = System.Drawing.Color.GreenYellow;
            this.labelInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInput.Location = new System.Drawing.Point(3, 35);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(525, 25);
            this.labelInput.TabIndex = 1;
            this.labelInput.Text = "Input";
            this.labelInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.BackColor = System.Drawing.Color.LightPink;
            this.labelOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOutput.Location = new System.Drawing.Point(534, 35);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(525, 25);
            this.labelOutput.TabIndex = 2;
            this.labelOutput.Text = "Output";
            this.labelOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDigitalIo
            // 
            this.panelDigitalIo.Controls.Add(this.labelDigitalIo);
            this.panelDigitalIo.Controls.Add(this.comboBoxDigitalIo);
            this.panelDigitalIo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDigitalIo.Location = new System.Drawing.Point(3, 3);
            this.panelDigitalIo.Name = "panelDigitalIo";
            this.panelDigitalIo.Size = new System.Drawing.Size(525, 29);
            this.panelDigitalIo.TabIndex = 4;
            // 
            // labelDigitalIo
            // 
            this.labelDigitalIo.AutoSize = true;
            this.labelDigitalIo.Location = new System.Drawing.Point(9, 6);
            this.labelDigitalIo.Name = "labelDigitalIo";
            this.labelDigitalIo.Size = new System.Drawing.Size(78, 20);
            this.labelDigitalIo.TabIndex = 4;
            this.labelDigitalIo.Text = "Digital I/O";
            // 
            // comboBoxDigitalIo
            // 
            this.comboBoxDigitalIo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDigitalIo.FormattingEnabled = true;
            this.comboBoxDigitalIo.Location = new System.Drawing.Point(150, 1);
            this.comboBoxDigitalIo.Name = "comboBoxDigitalIo";
            this.comboBoxDigitalIo.Size = new System.Drawing.Size(146, 28);
            this.comboBoxDigitalIo.TabIndex = 3;
            this.comboBoxDigitalIo.SelectedIndexChanged += new System.EventHandler(this.comboBoxDigitalIo_SelectedIndexChanged);
            // 
            // panelInPortSelector
            // 
            this.panelInPortSelector.Controls.Add(this.comboBoxInPortGroup);
            this.panelInPortSelector.Controls.Add(this.labelInPortGroup);
            this.panelInPortSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInPortSelector.Location = new System.Drawing.Point(3, 63);
            this.panelInPortSelector.Name = "panelInPortSelector";
            this.panelInPortSelector.Size = new System.Drawing.Size(525, 29);
            this.panelInPortSelector.TabIndex = 6;
            // 
            // comboBoxInPortGroup
            // 
            this.comboBoxInPortGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInPortGroup.FormattingEnabled = true;
            this.comboBoxInPortGroup.Location = new System.Drawing.Point(150, 1);
            this.comboBoxInPortGroup.Name = "comboBoxInPortGroup";
            this.comboBoxInPortGroup.Size = new System.Drawing.Size(146, 28);
            this.comboBoxInPortGroup.TabIndex = 3;
            this.comboBoxInPortGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxInPortGroup_SelectedIndexChanged);
            // 
            // labelInPortGroup
            // 
            this.labelInPortGroup.AutoSize = true;
            this.labelInPortGroup.Location = new System.Drawing.Point(7, 5);
            this.labelInPortGroup.Name = "labelInPortGroup";
            this.labelInPortGroup.Size = new System.Drawing.Size(128, 20);
            this.labelInPortGroup.TabIndex = 4;
            this.labelInPortGroup.Text = "Input Port Group";
            // 
            // panelOutPortSelector
            // 
            this.panelOutPortSelector.Controls.Add(this.comboBoxOutPortGroup);
            this.panelOutPortSelector.Controls.Add(this.labelOutputPortGroup);
            this.panelOutPortSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutPortSelector.Location = new System.Drawing.Point(534, 63);
            this.panelOutPortSelector.Name = "panelOutPortSelector";
            this.panelOutPortSelector.Size = new System.Drawing.Size(525, 29);
            this.panelOutPortSelector.TabIndex = 7;
            // 
            // comboBoxOutPortGroup
            // 
            this.comboBoxOutPortGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutPortGroup.FormattingEnabled = true;
            this.comboBoxOutPortGroup.Location = new System.Drawing.Point(157, 0);
            this.comboBoxOutPortGroup.Name = "comboBoxOutPortGroup";
            this.comboBoxOutPortGroup.Size = new System.Drawing.Size(146, 28);
            this.comboBoxOutPortGroup.TabIndex = 3;
            this.comboBoxOutPortGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutPortGroup_SelectedIndexChanged);
            // 
            // labelOutputPortGroup
            // 
            this.labelOutputPortGroup.AutoSize = true;
            this.labelOutputPortGroup.Location = new System.Drawing.Point(13, 5);
            this.labelOutputPortGroup.Name = "labelOutputPortGroup";
            this.labelOutputPortGroup.Size = new System.Drawing.Size(140, 20);
            this.labelOutputPortGroup.TabIndex = 4;
            this.labelOutputPortGroup.Text = "Output Port Group";
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeButton.Location = new System.Drawing.Point(474, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(114, 38);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // bottomPanel
            // 
            this.bottomPanel.ColumnCount = 3;
            this.bottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.bottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomPanel.Controls.Add(this.closeButton, 1, 0);
            this.bottomPanel.Controls.Add(this.buttonLongRun, 0, 0);
            this.bottomPanel.Controls.Add(this.buttonOneTimeRun, 2, 0);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(1, 601);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.RowCount = 1;
            this.bottomPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomPanel.Size = new System.Drawing.Size(1062, 44);
            this.bottomPanel.TabIndex = 3;
            // 
            // buttonLongRun
            // 
            this.buttonLongRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLongRun.Location = new System.Drawing.Point(3, 3);
            this.buttonLongRun.Name = "buttonLongRun";
            this.buttonLongRun.Size = new System.Drawing.Size(465, 38);
            this.buttonLongRun.TabIndex = 1;
            this.buttonLongRun.Text = "Long Run";
            this.buttonLongRun.UseVisualStyleBackColor = true;
            this.buttonLongRun.Click += new System.EventHandler(this.buttonLongRun_Click);
            // 
            // buttonOneTimeRun
            // 
            this.buttonOneTimeRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOneTimeRun.Location = new System.Drawing.Point(594, 3);
            this.buttonOneTimeRun.Name = "buttonOneTimeRun";
            this.buttonOneTimeRun.Size = new System.Drawing.Size(465, 38);
            this.buttonOneTimeRun.TabIndex = 2;
            this.buttonOneTimeRun.Text = "One time Run";
            this.buttonOneTimeRun.UseVisualStyleBackColor = true;
            this.buttonOneTimeRun.Click += new System.EventHandler(this.buttonOneTimeRun_Click);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1064, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 645);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1064, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 614);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1063, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 614);
            // 
            // timerLongRun
            // 
            this.timerLongRun.Tick += new System.EventHandler(this.timerLongRun_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 500;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Interval = 500;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // timer5
            // 
            this.timer5.Interval = 500;
            this.timer5.Tick += new System.EventHandler(this.timer5_Tick);
            // 
            // IoPortViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1064, 646);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IoPortViewer";
            this.Text = "I/O Port";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IoPortViewer_FormClosing);
            this.Load += new System.EventHandler(this.IoPortViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inportTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outportTable)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.panelDigitalIo.ResumeLayout(false);
            this.panelDigitalIo.PerformLayout();
            this.panelInPortSelector.ResumeLayout(false);
            this.panelInPortSelector.PerformLayout();
            this.panelOutPortSelector.ResumeLayout(false);
            this.panelOutPortSelector.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView inportTable;
        private System.Windows.Forms.DataGridView outportTable;
        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.TableLayoutPanel bottomPanel;
        private System.Windows.Forms.ComboBox comboBoxDigitalIo;
        private System.Windows.Forms.Panel panelDigitalIo;
        private System.Windows.Forms.Label labelDigitalIo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOutputNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInputNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInputName;
        private System.Windows.Forms.DataGridViewImageColumn ColumnInputValue;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Panel panelInPortSelector;
        private System.Windows.Forms.ComboBox comboBoxInPortGroup;
        private System.Windows.Forms.Label labelInPortGroup;
        private System.Windows.Forms.Panel panelOutPortSelector;
        private System.Windows.Forms.ComboBox comboBoxOutPortGroup;
        private System.Windows.Forms.Label labelOutputPortGroup;
        private System.Windows.Forms.Button buttonLongRun;
        private System.Windows.Forms.Button buttonOneTimeRun;
        private System.Windows.Forms.Timer timerLongRun;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Timer timer5;
    }
}