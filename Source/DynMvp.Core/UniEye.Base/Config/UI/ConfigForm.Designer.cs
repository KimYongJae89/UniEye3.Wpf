namespace UniEye.Base.Settings.UI
{
    partial class ConfigForm
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.labelTitle = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.SettingPage_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.virtualMode = new System.Windows.Forms.CheckBox();
            this.programTitle = new System.Windows.Forms.TextBox();
            this.labelProgramTitle = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.TextBox();
            this.imagingLibrary = new System.Windows.Forms.ComboBox();
            this.labelImagingLibrary = new System.Windows.Forms.Label();
            this.labelCompanyLogo = new System.Windows.Forms.Label();
            this.companyLogo = new System.Windows.Forms.TextBox();
            this.buttonSelectProductLogo = new System.Windows.Forms.Button();
            this.labelProductLogo = new System.Windows.Forms.Label();
            this.buttonSelectCompanyLogo = new System.Windows.Forms.Button();
            this.productLogo = new System.Windows.Forms.TextBox();
            this.language = new System.Windows.Forms.ComboBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.cmbSystemType = new System.Windows.Forms.ComboBox();
            this.labelSystemType = new System.Windows.Forms.Label();
            this.tabPageDeviceNew = new System.Windows.Forms.TabPage();
            this.tabPageInterface = new System.Windows.Forms.TabPage();
            this.tabPageModel = new System.Windows.Forms.TabPage();
            this.cmbImageNameFormat = new System.Windows.Forms.ComboBox();
            this.dataPathType = new System.Windows.Forms.ComboBox();
            this.labelTargetGroupImageFormat = new System.Windows.Forms.Label();
            this.numLightType = new System.Windows.Forms.NumericUpDown();
            this.labelNumLightType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveCameraImage = new System.Windows.Forms.CheckBox();
            this.chkSingleModel = new System.Windows.Forms.CheckBox();
            this.saveProbeImage = new System.Windows.Forms.CheckBox();
            this.saveTargetImage = new System.Windows.Forms.CheckBox();
            this.tabPageUI = new System.Windows.Forms.TabPage();
            this.checkUseSnapshot = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataRestoringDaysNumeric = new System.Windows.Forms.NumericUpDown();
            this.chkUseSaveResultFigure = new System.Windows.Forms.CheckBox();
            this.checkUseLoginForm = new System.Windows.Forms.CheckBox();
            this.checkShowNGImage = new System.Windows.Forms.CheckBox();
            this.checkShowScore = new System.Windows.Forms.CheckBox();
            this.showSelector = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkSaveTargetImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SettingPage_Fill_Panel.ClientArea.SuspendLayout();
            this.SettingPage_Fill_Panel.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLightType)).BeginInit();
            this.tabPageUI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRestoringDaysNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(11, 47);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(38, 20);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Title";
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(625, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 689);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(625, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 658);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(624, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 658);
            // 
            // SettingPage_Fill_Panel
            // 
            // 
            // 
            // 
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.tabMain);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.buttonOk);
            this.SettingPage_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SettingPage_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingPage_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.SettingPage_Fill_Panel.Name = "SettingPage_Fill_Panel";
            this.SettingPage_Fill_Panel.Size = new System.Drawing.Size(623, 658);
            this.SettingPage_Fill_Panel.TabIndex = 161;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPageGeneral);
            this.tabMain.Controls.Add(this.tabPageDeviceNew);
            this.tabMain.Controls.Add(this.tabPageInterface);
            this.tabMain.Controls.Add(this.tabPageModel);
            this.tabMain.Controls.Add(this.tabPageUI);
            this.tabMain.Location = new System.Drawing.Point(5, 6);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(613, 601);
            this.tabMain.TabIndex = 162;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.virtualMode);
            this.tabPageGeneral.Controls.Add(this.programTitle);
            this.tabPageGeneral.Controls.Add(this.labelProgramTitle);
            this.tabPageGeneral.Controls.Add(this.title);
            this.tabPageGeneral.Controls.Add(this.labelTitle);
            this.tabPageGeneral.Controls.Add(this.imagingLibrary);
            this.tabPageGeneral.Controls.Add(this.labelImagingLibrary);
            this.tabPageGeneral.Controls.Add(this.labelCompanyLogo);
            this.tabPageGeneral.Controls.Add(this.companyLogo);
            this.tabPageGeneral.Controls.Add(this.buttonSelectProductLogo);
            this.tabPageGeneral.Controls.Add(this.labelProductLogo);
            this.tabPageGeneral.Controls.Add(this.buttonSelectCompanyLogo);
            this.tabPageGeneral.Controls.Add(this.productLogo);
            this.tabPageGeneral.Controls.Add(this.language);
            this.tabPageGeneral.Controls.Add(this.labelLanguage);
            this.tabPageGeneral.Controls.Add(this.cmbSystemType);
            this.tabPageGeneral.Controls.Add(this.labelSystemType);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 29);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(605, 568);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // virtualMode
            // 
            this.virtualMode.AutoSize = true;
            this.virtualMode.Location = new System.Drawing.Point(17, 244);
            this.virtualMode.Name = "virtualMode";
            this.virtualMode.Size = new System.Drawing.Size(118, 24);
            this.virtualMode.TabIndex = 159;
            this.virtualMode.Text = "Virtual Mode";
            this.virtualMode.UseVisualStyleBackColor = true;
            // 
            // programTitle
            // 
            this.programTitle.Location = new System.Drawing.Point(134, 12);
            this.programTitle.Margin = new System.Windows.Forms.Padding(4);
            this.programTitle.Name = "programTitle";
            this.programTitle.Size = new System.Drawing.Size(261, 27);
            this.programTitle.TabIndex = 1;
            // 
            // labelProgramTitle
            // 
            this.labelProgramTitle.AutoSize = true;
            this.labelProgramTitle.Location = new System.Drawing.Point(11, 15);
            this.labelProgramTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProgramTitle.Name = "labelProgramTitle";
            this.labelProgramTitle.Size = new System.Drawing.Size(101, 20);
            this.labelProgramTitle.TabIndex = 0;
            this.labelProgramTitle.Text = "Program Title";
            // 
            // imagingLibrary
            // 
            this.imagingLibrary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imagingLibrary.FormattingEnabled = true;
            this.imagingLibrary.Items.AddRange(new object[] {
            "Open CV",
            "Open eVision",
            "VisionPro",
            "MIL",
            "Halcon",
            "Custom"});
            this.imagingLibrary.Location = new System.Drawing.Point(135, 205);
            this.imagingLibrary.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.imagingLibrary.Name = "imagingLibrary";
            this.imagingLibrary.Size = new System.Drawing.Size(261, 28);
            this.imagingLibrary.TabIndex = 152;
            // 
            // labelImagingLibrary
            // 
            this.labelImagingLibrary.AutoSize = true;
            this.labelImagingLibrary.Location = new System.Drawing.Point(12, 209);
            this.labelImagingLibrary.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelImagingLibrary.Name = "labelImagingLibrary";
            this.labelImagingLibrary.Size = new System.Drawing.Size(115, 20);
            this.labelImagingLibrary.TabIndex = 151;
            this.labelImagingLibrary.Text = "Imaging Library";
            // 
            // labelCompanyLogo
            // 
            this.labelCompanyLogo.AutoSize = true;
            this.labelCompanyLogo.Location = new System.Drawing.Point(12, 78);
            this.labelCompanyLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCompanyLogo.Name = "labelCompanyLogo";
            this.labelCompanyLogo.Size = new System.Drawing.Size(113, 20);
            this.labelCompanyLogo.TabIndex = 0;
            this.labelCompanyLogo.Text = "Company Logo";
            // 
            // companyLogo
            // 
            this.companyLogo.Location = new System.Drawing.Point(135, 75);
            this.companyLogo.Margin = new System.Windows.Forms.Padding(4);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.Size = new System.Drawing.Size(223, 27);
            this.companyLogo.TabIndex = 1;
            // 
            // buttonSelectProductLogo
            // 
            this.buttonSelectProductLogo.Location = new System.Drawing.Point(361, 106);
            this.buttonSelectProductLogo.Name = "buttonSelectProductLogo";
            this.buttonSelectProductLogo.Size = new System.Drawing.Size(35, 26);
            this.buttonSelectProductLogo.TabIndex = 158;
            this.buttonSelectProductLogo.Text = "...";
            this.buttonSelectProductLogo.UseVisualStyleBackColor = true;
            this.buttonSelectProductLogo.Click += new System.EventHandler(this.buttonSelectProductLogo_Click);
            // 
            // labelProductLogo
            // 
            this.labelProductLogo.AutoSize = true;
            this.labelProductLogo.Location = new System.Drawing.Point(12, 109);
            this.labelProductLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProductLogo.Name = "labelProductLogo";
            this.labelProductLogo.Size = new System.Drawing.Size(101, 20);
            this.labelProductLogo.TabIndex = 0;
            this.labelProductLogo.Text = "Product Logo";
            // 
            // buttonSelectCompanyLogo
            // 
            this.buttonSelectCompanyLogo.Location = new System.Drawing.Point(361, 75);
            this.buttonSelectCompanyLogo.Name = "buttonSelectCompanyLogo";
            this.buttonSelectCompanyLogo.Size = new System.Drawing.Size(35, 26);
            this.buttonSelectCompanyLogo.TabIndex = 158;
            this.buttonSelectCompanyLogo.Text = "...";
            this.buttonSelectCompanyLogo.UseVisualStyleBackColor = true;
            this.buttonSelectCompanyLogo.Click += new System.EventHandler(this.buttonSelectCompanyLogo_Click);
            // 
            // productLogo
            // 
            this.productLogo.Location = new System.Drawing.Point(135, 106);
            this.productLogo.Margin = new System.Windows.Forms.Padding(4);
            this.productLogo.Name = "productLogo";
            this.productLogo.Size = new System.Drawing.Size(223, 27);
            this.productLogo.TabIndex = 1;
            // 
            // language
            // 
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.FormattingEnabled = true;
            this.language.Items.AddRange(new object[] {
            "English",
            "Korean[ko-kr]",
            "Chinese(Simplified)[zh-cn]"});
            this.language.Location = new System.Drawing.Point(135, 137);
            this.language.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.language.Name = "language";
            this.language.Size = new System.Drawing.Size(261, 28);
            this.language.TabIndex = 152;
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(12, 141);
            this.labelLanguage.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(76, 20);
            this.labelLanguage.TabIndex = 151;
            this.labelLanguage.Text = "Language";
            // 
            // cmbSystemType
            // 
            this.cmbSystemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemType.FormattingEnabled = true;
            this.cmbSystemType.Items.AddRange(new object[] {
            "None",
            "ScriberAlign",
            "CurvedGlassAlign"});
            this.cmbSystemType.Location = new System.Drawing.Point(135, 170);
            this.cmbSystemType.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cmbSystemType.Name = "cmbSystemType";
            this.cmbSystemType.Size = new System.Drawing.Size(261, 28);
            this.cmbSystemType.TabIndex = 152;
            // 
            // labelSystemType
            // 
            this.labelSystemType.AutoSize = true;
            this.labelSystemType.Location = new System.Drawing.Point(12, 174);
            this.labelSystemType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelSystemType.Name = "labelSystemType";
            this.labelSystemType.Size = new System.Drawing.Size(93, 20);
            this.labelSystemType.TabIndex = 151;
            this.labelSystemType.Text = "System Type";
            // 
            // tabPageInterface
            // 
            this.tabPageInterface.Location = new System.Drawing.Point(4, 29);
            this.tabPageInterface.Name = "tabPageInterface";
            this.tabPageInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInterface.Size = new System.Drawing.Size(605, 568);
            this.tabPageInterface.TabIndex = 4;
            this.tabPageInterface.Text = "Interface";
            this.tabPageInterface.UseVisualStyleBackColor = true;
            // 
            // tabPageModel
            // 
            this.tabPageModel.Controls.Add(this.cmbImageNameFormat);
            this.tabPageModel.Controls.Add(this.dataPathType);
            this.tabPageModel.Controls.Add(this.labelTargetGroupImageFormat);
            this.tabPageModel.Controls.Add(this.numLightType);
            this.tabPageModel.Controls.Add(this.labelNumLightType);
            this.tabPageModel.Controls.Add(this.label1);
            this.tabPageModel.Controls.Add(this.saveCameraImage);
            this.tabPageModel.Controls.Add(this.chkSingleModel);
            this.tabPageModel.Controls.Add(this.saveProbeImage);
            this.tabPageModel.Controls.Add(this.saveTargetImage);
            this.tabPageModel.Location = new System.Drawing.Point(4, 29);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Size = new System.Drawing.Size(605, 568);
            this.tabPageModel.TabIndex = 3;
            this.tabPageModel.Text = "Model";
            this.tabPageModel.UseVisualStyleBackColor = true;
            // 
            // cmbImageNameFormat
            // 
            this.cmbImageNameFormat.FormattingEnabled = true;
            this.cmbImageNameFormat.Items.AddRange(new object[] {
            "Image_{0:0000}_C{1:00}.bmp",
            "Image_C{0:00}_S{1:000}_L{2:00}.bmp"});
            this.cmbImageNameFormat.Location = new System.Drawing.Point(184, 240);
            this.cmbImageNameFormat.Name = "cmbImageNameFormat";
            this.cmbImageNameFormat.Size = new System.Drawing.Size(192, 28);
            this.cmbImageNameFormat.TabIndex = 165;
            // 
            // dataPathType
            // 
            this.dataPathType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataPathType.FormattingEnabled = true;
            this.dataPathType.Items.AddRange(new object[] {
            "Model/Day",
            "Model/Day/Hour",
            "Day/Model",
            "Day/Hour/Model"});
            this.dataPathType.Location = new System.Drawing.Point(147, 112);
            this.dataPathType.Name = "dataPathType";
            this.dataPathType.Size = new System.Drawing.Size(192, 28);
            this.dataPathType.TabIndex = 165;
            // 
            // labelTargetGroupImageFormat
            // 
            this.labelTargetGroupImageFormat.AutoSize = true;
            this.labelTargetGroupImageFormat.Location = new System.Drawing.Point(11, 243);
            this.labelTargetGroupImageFormat.Name = "labelTargetGroupImageFormat";
            this.labelTargetGroupImageFormat.Size = new System.Drawing.Size(148, 20);
            this.labelTargetGroupImageFormat.TabIndex = 164;
            this.labelTargetGroupImageFormat.Text = "Image Name Format";
            // 
            // numLightType
            // 
            this.numLightType.Location = new System.Drawing.Point(239, 302);
            this.numLightType.Name = "numLightType";
            this.numLightType.Size = new System.Drawing.Size(100, 27);
            this.numLightType.TabIndex = 155;
            // 
            // labelNumLightType
            // 
            this.labelNumLightType.AutoSize = true;
            this.labelNumLightType.Location = new System.Drawing.Point(10, 308);
            this.labelNumLightType.Name = "labelNumLightType";
            this.labelNumLightType.Size = new System.Drawing.Size(118, 20);
            this.labelNumLightType.TabIndex = 154;
            this.labelNumLightType.Text = "Num Light Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 20);
            this.label1.TabIndex = 164;
            this.label1.Text = "Result Path Type";
            // 
            // saveCameraImage
            // 
            this.saveCameraImage.AutoSize = true;
            this.saveCameraImage.Location = new System.Drawing.Point(15, 216);
            this.saveCameraImage.Name = "saveCameraImage";
            this.saveCameraImage.Size = new System.Drawing.Size(163, 24);
            this.saveCameraImage.TabIndex = 163;
            this.saveCameraImage.Text = "Save Camera Image";
            this.saveCameraImage.UseVisualStyleBackColor = true;
            // 
            // chkSingleModel
            // 
            this.chkSingleModel.AutoSize = true;
            this.chkSingleModel.Location = new System.Drawing.Point(14, 15);
            this.chkSingleModel.Name = "chkSingleModel";
            this.chkSingleModel.Size = new System.Drawing.Size(119, 24);
            this.chkSingleModel.TabIndex = 163;
            this.chkSingleModel.Text = "Single Model";
            this.chkSingleModel.UseVisualStyleBackColor = true;
            // 
            // saveProbeImage
            // 
            this.saveProbeImage.AutoSize = true;
            this.saveProbeImage.Location = new System.Drawing.Point(15, 156);
            this.saveProbeImage.Name = "saveProbeImage";
            this.saveProbeImage.Size = new System.Drawing.Size(151, 24);
            this.saveProbeImage.TabIndex = 163;
            this.saveProbeImage.Text = "Save Probe Image";
            this.saveProbeImage.UseVisualStyleBackColor = true;
            // 
            // saveTargetImage
            // 
            this.saveTargetImage.AutoSize = true;
            this.saveTargetImage.Location = new System.Drawing.Point(15, 186);
            this.saveTargetImage.Name = "saveTargetImage";
            this.saveTargetImage.Size = new System.Drawing.Size(154, 24);
            this.saveTargetImage.TabIndex = 163;
            this.saveTargetImage.Text = "Save Target Image";
            this.saveTargetImage.UseVisualStyleBackColor = true;
            // 
            // tabPageUI
            // 
            this.tabPageUI.Controls.Add(this.checkUseSnapshot);
            this.tabPageUI.Controls.Add(this.label3);
            this.tabPageUI.Controls.Add(this.label2);
            this.tabPageUI.Controls.Add(this.dataRestoringDaysNumeric);
            this.tabPageUI.Controls.Add(this.chkUseSaveResultFigure);
            this.tabPageUI.Controls.Add(this.checkUseLoginForm);
            this.tabPageUI.Controls.Add(this.checkShowNGImage);
            this.tabPageUI.Controls.Add(this.checkShowScore);
            this.tabPageUI.Controls.Add(this.showSelector);
            this.tabPageUI.Location = new System.Drawing.Point(4, 29);
            this.tabPageUI.Name = "tabPageUI";
            this.tabPageUI.Size = new System.Drawing.Size(605, 568);
            this.tabPageUI.TabIndex = 2;
            this.tabPageUI.Text = "UI";
            this.tabPageUI.UseVisualStyleBackColor = true;
            // 
            // checkUseSnapshot
            // 
            this.checkUseSnapshot.AutoSize = true;
            this.checkUseSnapshot.Location = new System.Drawing.Point(15, 348);
            this.checkUseSnapshot.Name = "checkUseSnapshot";
            this.checkUseSnapshot.Size = new System.Drawing.Size(128, 24);
            this.checkUseSnapshot.TabIndex = 176;
            this.checkUseSnapshot.Text = "Use Snap Shot";
            this.checkUseSnapshot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUseSnapshot.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 377);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 175;
            this.label3.Text = "Day(s)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 20);
            this.label2.TabIndex = 174;
            this.label2.Text = "Result Restoring Day(s)";
            // 
            // dataRestoringDaysNumeric
            // 
            this.dataRestoringDaysNumeric.Location = new System.Drawing.Point(203, 375);
            this.dataRestoringDaysNumeric.Name = "dataRestoringDaysNumeric";
            this.dataRestoringDaysNumeric.Size = new System.Drawing.Size(120, 27);
            this.dataRestoringDaysNumeric.TabIndex = 173;
            // 
            // chkUseSaveResultFigure
            // 
            this.chkUseSaveResultFigure.AutoSize = true;
            this.chkUseSaveResultFigure.Location = new System.Drawing.Point(15, 318);
            this.chkUseSaveResultFigure.Name = "chkUseSaveResultFigure";
            this.chkUseSaveResultFigure.Size = new System.Drawing.Size(182, 24);
            this.chkUseSaveResultFigure.TabIndex = 172;
            this.chkUseSaveResultFigure.Text = "Use Save Result Figure";
            this.chkUseSaveResultFigure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkUseSaveResultFigure.UseVisualStyleBackColor = true;
            // 
            // checkUseLoginForm
            // 
            this.checkUseLoginForm.AutoSize = true;
            this.checkUseLoginForm.Location = new System.Drawing.Point(15, 228);
            this.checkUseLoginForm.Name = "checkUseLoginForm";
            this.checkUseLoginForm.Size = new System.Drawing.Size(130, 24);
            this.checkUseLoginForm.TabIndex = 160;
            this.checkUseLoginForm.Text = "Use login form";
            this.checkUseLoginForm.UseVisualStyleBackColor = true;
            // 
            // checkShowNGImage
            // 
            this.checkShowNGImage.AutoSize = true;
            this.checkShowNGImage.Location = new System.Drawing.Point(15, 198);
            this.checkShowNGImage.Name = "checkShowNGImage";
            this.checkShowNGImage.Size = new System.Drawing.Size(139, 24);
            this.checkShowNGImage.TabIndex = 160;
            this.checkShowNGImage.Text = "Show NG image";
            this.checkShowNGImage.UseVisualStyleBackColor = true;
            // 
            // checkShowScore
            // 
            this.checkShowScore.AutoSize = true;
            this.checkShowScore.Location = new System.Drawing.Point(15, 168);
            this.checkShowScore.Name = "checkShowScore";
            this.checkShowScore.Size = new System.Drawing.Size(105, 24);
            this.checkShowScore.TabIndex = 160;
            this.checkShowScore.Text = "Show score";
            this.checkShowScore.UseVisualStyleBackColor = true;
            // 
            // showSelector
            // 
            this.showSelector.AutoSize = true;
            this.showSelector.Location = new System.Drawing.Point(15, 12);
            this.showSelector.Name = "showSelector";
            this.showSelector.Size = new System.Drawing.Size(124, 24);
            this.showSelector.TabIndex = 160;
            this.showSelector.Text = "Show Selector";
            this.showSelector.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(312, 613);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(145, 40);
            this.buttonCancel.TabIndex = 153;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(163, 613);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(146, 40);
            this.buttonOk.TabIndex = 153;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // checkSaveTargetImage
            // 
            this.checkSaveTargetImage.AutoSize = true;
            this.checkSaveTargetImage.Location = new System.Drawing.Point(15, 188);
            this.checkSaveTargetImage.Name = "checkSaveTargetImage";
            this.checkSaveTargetImage.Size = new System.Drawing.Size(157, 24);
            this.checkSaveTargetImage.TabIndex = 160;
            this.checkSaveTargetImage.Text = "Save target image";
            this.checkSaveTargetImage.UseVisualStyleBackColor = true;
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(134, 44);
            this.title.Margin = new System.Windows.Forms.Padding(4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(261, 27);
            this.title.TabIndex = 1;
            // 
            // tabPageDeviceNew
            // 
            this.tabPageDeviceNew.Location = new System.Drawing.Point(4, 29);
            this.tabPageDeviceNew.Name = "tabPageDeviceNew";
            this.tabPageDeviceNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDeviceNew.Size = new System.Drawing.Size(605, 568);
            this.tabPageDeviceNew.TabIndex = 5;
            this.tabPageDeviceNew.Text = "Device";
            this.tabPageDeviceNew.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 690);
            this.Controls.Add(this.SettingPage_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.SettingPage_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SettingPage_Fill_Panel.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageModel.ResumeLayout(false);
            this.tabPageModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLightType)).EndInit();
            this.tabPageUI.ResumeLayout(false);
            this.tabPageUI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRestoringDaysNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel SettingPage_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelSystemType;
        private System.Windows.Forms.ComboBox cmbSystemType;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ComboBox language;
        private System.Windows.Forms.Button buttonSelectCompanyLogo;
        private System.Windows.Forms.TextBox companyLogo;
        private System.Windows.Forms.Label labelCompanyLogo;
        private System.Windows.Forms.Button buttonSelectProductLogo;
        private System.Windows.Forms.TextBox productLogo;
        private System.Windows.Forms.Label labelProductLogo;
        private System.Windows.Forms.CheckBox showSelector;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageModel;
        private System.Windows.Forms.TabPage tabPageUI;
        private System.Windows.Forms.Label labelImagingLibrary;
        private System.Windows.Forms.ComboBox imagingLibrary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox dataPathType;
        private System.Windows.Forms.Label labelNumLightType;
        private System.Windows.Forms.NumericUpDown numLightType;
        private System.Windows.Forms.TabPage tabPageInterface;
        private System.Windows.Forms.CheckBox saveCameraImage;
        private System.Windows.Forms.CheckBox saveTargetImage;
        private System.Windows.Forms.Label labelTargetGroupImageFormat;
        private System.Windows.Forms.CheckBox checkShowScore;
        private System.Windows.Forms.CheckBox checkShowNGImage;
        private System.Windows.Forms.CheckBox checkSaveTargetImage;
        private System.Windows.Forms.CheckBox checkUseLoginForm;
        private System.Windows.Forms.CheckBox virtualMode;
        private System.Windows.Forms.CheckBox saveProbeImage;
        private System.Windows.Forms.TextBox programTitle;
        private System.Windows.Forms.Label labelProgramTitle;
        private System.Windows.Forms.CheckBox chkUseSaveResultFigure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown dataRestoringDaysNumeric;
        private System.Windows.Forms.CheckBox checkUseSnapshot;
        private System.Windows.Forms.ComboBox cmbImageNameFormat;
        private System.Windows.Forms.CheckBox chkSingleModel;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.TabPage tabPageDeviceNew;
    }
}
