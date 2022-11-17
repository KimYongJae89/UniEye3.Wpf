namespace UniEye.Base.UI.Main
{
    partial class ModellerToolbar
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModellerToolbar));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageModel = new System.Windows.Forms.TabPage();
            this.toolStripModel = new System.Windows.Forms.ToolStrip();
            this.modelPropertyButton = new System.Windows.Forms.ToolStripButton();
            this.exportFormatButton = new System.Windows.Forms.ToolStripButton();
            this.editSchemaButton = new System.Windows.Forms.ToolStripButton();
            this.createSchemaButton = new System.Windows.Forms.ToolStripButton();
            this.scanButton = new System.Windows.Forms.ToolStripButton();
            this.tabPageFov = new System.Windows.Forms.TabPage();
            this.toolStripStep = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelStep = new System.Windows.Forms.ToolStripLabel();
            this.comboInspectStep = new System.Windows.Forms.ToolStripComboBox();
            this.movePrevStepButton = new System.Windows.Forms.ToolStripButton();
            this.moveNextStepButton = new System.Windows.Forms.ToolStripButton();
            this.addStepButton = new System.Windows.Forms.ToolStripButton();
            this.deleteStepButton = new System.Windows.Forms.ToolStripButton();
            this.editStepButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCameraButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.selectLightButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.showLightPanelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.loadImageSetToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolBtnPrevImage = new System.Windows.Forms.ToolStripButton();
            this.toolBtnNextImage = new System.Windows.Forms.ToolStripButton();
            this.grabProcessToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.singleShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.multiShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAlign = new System.Windows.Forms.ToolStripButton();
            this.tabPageProbe = new System.Windows.Forms.TabPage();
            this.toolStripProbe = new System.Windows.Forms.ToolStrip();
            this.addProbeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.groupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ungroupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorGroup = new System.Windows.Forms.ToolStripSeparator();
            this.syncParamToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.syncAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.lockMoveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RedoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSearchProbe = new System.Windows.Forms.ToolStripButton();
            this.tabPageView = new System.Windows.Forms.TabPage();
            this.toolStripView = new System.Windows.Forms.ToolStrip();
            this.zoomInToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zoomOutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zoomFitToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewTypeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.tabPageRobot = new System.Windows.Forms.TabPage();
            this.toolStripRobot = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOrigin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonJoystick = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRobotSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFineMove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControlMain.SuspendLayout();
            this.tabPageModel.SuspendLayout();
            this.toolStripModel.SuspendLayout();
            this.tabPageFov.SuspendLayout();
            this.toolStripStep.SuspendLayout();
            this.tabPageProbe.SuspendLayout();
            this.toolStripProbe.SuspendLayout();
            this.tabPageView.SuspendLayout();
            this.toolStripView.SuspendLayout();
            this.tabPageRobot.SuspendLayout();
            this.toolStripRobot.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageModel);
            this.tabControlMain.Controls.Add(this.tabPageFov);
            this.tabControlMain.Controls.Add(this.tabPageProbe);
            this.tabControlMain.Controls.Add(this.tabPageView);
            this.tabControlMain.Controls.Add(this.tabPageRobot);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ItemSize = new System.Drawing.Size(70, 25);
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1195, 99);
            this.tabControlMain.TabIndex = 135;
            // 
            // tabPageModel
            // 
            this.tabPageModel.Controls.Add(this.toolStripModel);
            this.tabPageModel.Location = new System.Drawing.Point(4, 29);
            this.tabPageModel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageModel.Size = new System.Drawing.Size(1187, 66);
            this.tabPageModel.TabIndex = 5;
            this.tabPageModel.Text = "Model";
            this.tabPageModel.UseVisualStyleBackColor = true;
            // 
            // toolStripModel
            // 
            this.toolStripModel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripModel.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelPropertyButton,
            this.exportFormatButton,
            this.editSchemaButton,
            this.createSchemaButton,
            this.scanButton});
            this.toolStripModel.Location = new System.Drawing.Point(4, 5);
            this.toolStripModel.Name = "toolStripModel";
            this.toolStripModel.Size = new System.Drawing.Size(1179, 60);
            this.toolStripModel.TabIndex = 2;
            this.toolStripModel.Text = "toolStrip1";
            // 
            // modelPropertyButton
            // 
            this.modelPropertyButton.Image = global::UniEye.Base.Properties.Resources.property_32;
            this.modelPropertyButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.modelPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modelPropertyButton.Name = "modelPropertyButton";
            this.modelPropertyButton.Size = new System.Drawing.Size(78, 57);
            this.modelPropertyButton.Text = "Property";
            this.modelPropertyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.modelPropertyButton.Click += new System.EventHandler(this.modelPropertyButton_Click);
            // 
            // exportFormatButton
            // 
            this.exportFormatButton.Image = global::UniEye.Base.Properties.Resources.format_32;
            this.exportFormatButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.exportFormatButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportFormatButton.Name = "exportFormatButton";
            this.exportFormatButton.Size = new System.Drawing.Size(119, 57);
            this.exportFormatButton.Text = "Export Format";
            this.exportFormatButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exportFormatButton.Click += new System.EventHandler(this.exportFormatButton_Click);
            // 
            // editSchemaButton
            // 
            this.editSchemaButton.Image = global::UniEye.Base.Properties.Resources.schema_32;
            this.editSchemaButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editSchemaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editSchemaButton.Name = "editSchemaButton";
            this.editSchemaButton.Size = new System.Drawing.Size(105, 57);
            this.editSchemaButton.Text = "Edit Schema";
            this.editSchemaButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.editSchemaButton.Click += new System.EventHandler(this.editSchemaButton_Click);
            // 
            // createSchemaButton
            // 
            this.createSchemaButton.Image = global::UniEye.Base.Properties.Resources.schema_32;
            this.createSchemaButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.createSchemaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createSchemaButton.Name = "createSchemaButton";
            this.createSchemaButton.Size = new System.Drawing.Size(125, 57);
            this.createSchemaButton.Text = "Create Schema";
            this.createSchemaButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.createSchemaButton.Click += new System.EventHandler(this.createSchemaButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Image = global::UniEye.Base.Properties.Resources.multi_shot_32;
            this.scanButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scanButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(48, 57);
            this.scanButton.Text = "Scan";
            this.scanButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // tabPageFov
            // 
            this.tabPageFov.Controls.Add(this.toolStripStep);
            this.tabPageFov.Location = new System.Drawing.Point(4, 29);
            this.tabPageFov.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageFov.Name = "tabPageFov";
            this.tabPageFov.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageFov.Size = new System.Drawing.Size(1187, 66);
            this.tabPageFov.TabIndex = 1;
            this.tabPageFov.Text = "FOV";
            this.tabPageFov.UseVisualStyleBackColor = true;
            // 
            // toolStripStep
            // 
            this.toolStripStep.AutoSize = false;
            this.toolStripStep.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripStep.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripStep.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelStep,
            this.comboInspectStep,
            this.movePrevStepButton,
            this.moveNextStepButton,
            this.addStepButton,
            this.deleteStepButton,
            this.editStepButton,
            this.toolStripSeparator7,
            this.selectCameraButton,
            this.selectLightButton,
            this.showLightPanelToolStripButton,
            this.toolStripSeparator9,
            this.loadImageSetToolStripButton,
            this.toolBtnPrevImage,
            this.toolBtnNextImage,
            this.grabProcessToolStripButton,
            this.singleShotToolStripButton,
            this.multiShotToolStripButton,
            this.toolStripSeparator1,
            this.toolStripButtonAlign});
            this.toolStripStep.Location = new System.Drawing.Point(4, 5);
            this.toolStripStep.Name = "toolStripStep";
            this.toolStripStep.Size = new System.Drawing.Size(1179, 62);
            this.toolStripStep.TabIndex = 1;
            this.toolStripStep.Text = "toolStrip2";
            // 
            // toolStripLabelStep
            // 
            this.toolStripLabelStep.Name = "toolStripLabelStep";
            this.toolStripLabelStep.Size = new System.Drawing.Size(44, 59);
            this.toolStripLabelStep.Text = "Step";
            // 
            // comboInspectStep
            // 
            this.comboInspectStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInspectStep.Name = "comboInspectStep";
            this.comboInspectStep.Size = new System.Drawing.Size(154, 62);
            this.comboInspectStep.SelectedIndexChanged += new System.EventHandler(this.comboInspectStep_SelectedIndexChanged);
            // 
            // movePrevStepButton
            // 
            this.movePrevStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.movePrevStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_left_32;
            this.movePrevStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.movePrevStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.movePrevStepButton.Name = "movePrevStepButton";
            this.movePrevStepButton.Size = new System.Drawing.Size(36, 59);
            this.movePrevStepButton.Text = "toolStripButton1";
            this.movePrevStepButton.Click += new System.EventHandler(this.movePrevStepButton_Click);
            // 
            // moveNextStepButton
            // 
            this.moveNextStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveNextStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.moveNextStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.moveNextStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveNextStepButton.Name = "moveNextStepButton";
            this.moveNextStepButton.Size = new System.Drawing.Size(36, 59);
            this.moveNextStepButton.Text = "toolStripButton2";
            this.moveNextStepButton.Click += new System.EventHandler(this.moveNextStepButton_Click);
            // 
            // addStepButton
            // 
            this.addStepButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addStepButton.Name = "addStepButton";
            this.addStepButton.Size = new System.Drawing.Size(45, 59);
            this.addStepButton.Text = "Add";
            this.addStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addStepButton.Click += new System.EventHandler(this.addStepButton_Click);
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(62, 59);
            this.deleteStepButton.Text = "Delete";
            this.deleteStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteStepButton.Click += new System.EventHandler(this.deleteStepButton_Click);
            // 
            // editStepButton
            // 
            this.editStepButton.Image = global::UniEye.Base.Properties.Resources.edit_32;
            this.editStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editStepButton.Name = "editStepButton";
            this.editStepButton.Size = new System.Drawing.Size(42, 59);
            this.editStepButton.Text = "Edit";
            this.editStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.editStepButton.Click += new System.EventHandler(this.editStepButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 62);
            // 
            // selectCameraButton
            // 
            this.selectCameraButton.AutoSize = false;
            this.selectCameraButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectCameraButton.Image = ((System.Drawing.Image)(resources.GetObject("selectCameraButton.Image")));
            this.selectCameraButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectCameraButton.Name = "selectCameraButton";
            this.selectCameraButton.Size = new System.Drawing.Size(100, 58);
            this.selectCameraButton.Text = "Camera";
            // 
            // selectLightButton
            // 
            this.selectLightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectLightButton.Image = ((System.Drawing.Image)(resources.GetObject("selectLightButton.Image")));
            this.selectLightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectLightButton.Name = "selectLightButton";
            this.selectLightButton.Size = new System.Drawing.Size(102, 59);
            this.selectLightButton.Text = "Light Type";
            // 
            // showLightPanelToolStripButton
            // 
            this.showLightPanelToolStripButton.Image = global::UniEye.Base.Properties.Resources.light_32;
            this.showLightPanelToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLightPanelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showLightPanelToolStripButton.Name = "showLightPanelToolStripButton";
            this.showLightPanelToolStripButton.Size = new System.Drawing.Size(51, 59);
            this.showLightPanelToolStripButton.Text = "Light";
            this.showLightPanelToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.showLightPanelToolStripButton.ToolTipText = "Show Light Panel";
            this.showLightPanelToolStripButton.Click += new System.EventHandler(this.showLightPanelToolStripButton_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 62);
            // 
            // loadImageSetToolStripButton
            // 
            this.loadImageSetToolStripButton.Image = global::UniEye.Base.Properties.Resources.picture_folder_32;
            this.loadImageSetToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.loadImageSetToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadImageSetToolStripButton.Name = "loadImageSetToolStripButton";
            this.loadImageSetToolStripButton.Size = new System.Drawing.Size(59, 59);
            this.loadImageSetToolStripButton.Text = "Image";
            this.loadImageSetToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.loadImageSetToolStripButton.ToolTipText = "Select Image Folder";
            this.loadImageSetToolStripButton.Click += new System.EventHandler(this.loadImageSetToolStripButton_Click);
            // 
            // toolBtnPrevImage
            // 
            this.toolBtnPrevImage.Image = global::UniEye.Base.Properties.Resources.arrow_left_32;
            this.toolBtnPrevImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnPrevImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnPrevImage.Name = "toolBtnPrevImage";
            this.toolBtnPrevImage.Size = new System.Drawing.Size(46, 59);
            this.toolBtnPrevImage.Text = "Prev";
            this.toolBtnPrevImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnPrevImage.ToolTipText = "Select Image Folder";
            this.toolBtnPrevImage.Click += new System.EventHandler(this.toolBtnPrevImage_Click);
            // 
            // toolBtnNextImage
            // 
            this.toolBtnNextImage.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.toolBtnNextImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnNextImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnNextImage.Name = "toolBtnNextImage";
            this.toolBtnNextImage.Size = new System.Drawing.Size(48, 59);
            this.toolBtnNextImage.Text = "Next";
            this.toolBtnNextImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnNextImage.ToolTipText = "Select Image Folder";
            this.toolBtnNextImage.Click += new System.EventHandler(this.toolBtnNextImage_Click);
            // 
            // grabProcessToolStripButton
            // 
            this.grabProcessToolStripButton.Image = global::UniEye.Base.Properties.Resources.process_shot_32;
            this.grabProcessToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.grabProcessToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.grabProcessToolStripButton.Name = "grabProcessToolStripButton";
            this.grabProcessToolStripButton.Size = new System.Drawing.Size(49, 59);
            this.grabProcessToolStripButton.Text = "Grab";
            this.grabProcessToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.grabProcessToolStripButton.ToolTipText = "Single Grab";
            this.grabProcessToolStripButton.Click += new System.EventHandler(this.grabProcessToolStripButton_Click);
            // 
            // singleShotToolStripButton
            // 
            this.singleShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.single_shot_32;
            this.singleShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.singleShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.singleShotToolStripButton.Name = "singleShotToolStripButton";
            this.singleShotToolStripButton.Size = new System.Drawing.Size(99, 59);
            this.singleShotToolStripButton.Text = "Single Shot";
            this.singleShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.singleShotToolStripButton.ToolTipText = "Select Image Folder";
            this.singleShotToolStripButton.Click += new System.EventHandler(this.singleShotToolStripButton_Click);
            // 
            // multiShotToolStripButton
            // 
            this.multiShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.multi_shot_32;
            this.multiShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.multiShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.multiShotToolStripButton.Name = "multiShotToolStripButton";
            this.multiShotToolStripButton.Size = new System.Drawing.Size(92, 59);
            this.multiShotToolStripButton.Text = "Multi Shot";
            this.multiShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.multiShotToolStripButton.ToolTipText = "Select Image Folder";
            this.multiShotToolStripButton.Click += new System.EventHandler(this.multiShotToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 62);
            // 
            // toolStripButtonAlign
            // 
            this.toolStripButtonAlign.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
            this.toolStripButtonAlign.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonAlign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlign.Name = "toolStripButtonAlign";
            this.toolStripButtonAlign.Size = new System.Drawing.Size(52, 59);
            this.toolStripButtonAlign.Text = "Align";
            this.toolStripButtonAlign.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonAlign.ToolTipText = "Align";
            // 
            // tabPageProbe
            // 
            this.tabPageProbe.Controls.Add(this.toolStripProbe);
            this.tabPageProbe.Location = new System.Drawing.Point(4, 29);
            this.tabPageProbe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageProbe.Name = "tabPageProbe";
            this.tabPageProbe.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageProbe.Size = new System.Drawing.Size(1187, 66);
            this.tabPageProbe.TabIndex = 4;
            this.tabPageProbe.Text = "Probe";
            this.tabPageProbe.UseVisualStyleBackColor = true;
            // 
            // toolStripProbe
            // 
            this.toolStripProbe.AutoSize = false;
            this.toolStripProbe.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripProbe.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProbe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProbeToolStripButton,
            this.copyProbeToolStripButton,
            this.pasteProbeToolStripButton,
            this.deleteProbeToolStripButton,
            this.toolStripSeparator3,
            this.groupProbeToolStripButton,
            this.ungroupProbeToolStripButton,
            this.toolStripSeparatorGroup,
            this.syncParamToolStripButton,
            this.syncAllToolStripButton,
            this.lockMoveToolStripButton,
            this.toolStripSeparator10,
            this.undoToolStripButton,
            this.RedoToolStripButton,
            this.toolStripButtonSearchProbe});
            this.toolStripProbe.Location = new System.Drawing.Point(4, 5);
            this.toolStripProbe.Name = "toolStripProbe";
            this.toolStripProbe.Size = new System.Drawing.Size(1179, 59);
            this.toolStripProbe.TabIndex = 2;
            this.toolStripProbe.Text = "toolStrip1";
            // 
            // addProbeToolStripButton
            // 
            this.addProbeToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.addProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addProbeToolStripButton.Name = "addProbeToolStripButton";
            this.addProbeToolStripButton.Size = new System.Drawing.Size(54, 56);
            this.addProbeToolStripButton.Text = "Add";
            this.addProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.AutoSize = false;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.testToolStripMenuItem.Text = "Testfgdfgdfgdfgdfgdfg";
            // 
            // copyProbeToolStripButton
            // 
            this.copyProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.copy_32;
            this.copyProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.copyProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyProbeToolStripButton.Name = "copyProbeToolStripButton";
            this.copyProbeToolStripButton.Size = new System.Drawing.Size(52, 56);
            this.copyProbeToolStripButton.Text = "Copy";
            this.copyProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.copyProbeToolStripButton.Click += new System.EventHandler(this.copyProbeToolStripButton_Click);
            // 
            // pasteProbeToolStripButton
            // 
            this.pasteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.paste_32;
            this.pasteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pasteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteProbeToolStripButton.Name = "pasteProbeToolStripButton";
            this.pasteProbeToolStripButton.Size = new System.Drawing.Size(53, 56);
            this.pasteProbeToolStripButton.Text = "Paste";
            this.pasteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pasteProbeToolStripButton.Click += new System.EventHandler(this.pasteProbeToolStripButton_Click);
            // 
            // deleteProbeToolStripButton
            // 
            this.deleteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteProbeToolStripButton.Name = "deleteProbeToolStripButton";
            this.deleteProbeToolStripButton.Size = new System.Drawing.Size(62, 56);
            this.deleteProbeToolStripButton.Text = "Delete";
            this.deleteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteProbeToolStripButton.Click += new System.EventHandler(this.deleteProbeToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 59);
            // 
            // groupProbeToolStripButton
            // 
            this.groupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.group_32;
            this.groupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.groupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupProbeToolStripButton.Name = "groupProbeToolStripButton";
            this.groupProbeToolStripButton.Size = new System.Drawing.Size(60, 56);
            this.groupProbeToolStripButton.Text = "Group";
            this.groupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.groupProbeToolStripButton.Click += new System.EventHandler(this.groupProbeToolStripButton_Click);
            // 
            // ungroupProbeToolStripButton
            // 
            this.ungroupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.ungroup_32;
            this.ungroupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ungroupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ungroupProbeToolStripButton.Name = "ungroupProbeToolStripButton";
            this.ungroupProbeToolStripButton.Size = new System.Drawing.Size(79, 56);
            this.ungroupProbeToolStripButton.Text = "Ungroup";
            this.ungroupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ungroupProbeToolStripButton.Click += new System.EventHandler(this.ungroupProbeToolStripButton_Click);
            // 
            // toolStripSeparatorGroup
            // 
            this.toolStripSeparatorGroup.Name = "toolStripSeparatorGroup";
            this.toolStripSeparatorGroup.Size = new System.Drawing.Size(6, 59);
            // 
            // syncParamToolStripButton
            // 
            this.syncParamToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncParamToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncParamToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncParamToolStripButton.Name = "syncParamToolStripButton";
            this.syncParamToolStripButton.Size = new System.Drawing.Size(48, 56);
            this.syncParamToolStripButton.Text = "Sync";
            this.syncParamToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.syncParamToolStripButton.Click += new System.EventHandler(this.syncParamToolStripButton_Click);
            // 
            // syncAllToolStripButton
            // 
            this.syncAllToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncAllToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncAllToolStripButton.Name = "syncAllToolStripButton";
            this.syncAllToolStripButton.Size = new System.Drawing.Size(73, 56);
            this.syncAllToolStripButton.Text = "Sync All";
            this.syncAllToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.syncAllToolStripButton.Click += new System.EventHandler(this.syncAllToolStripButton_Click);
            // 
            // lockMoveToolStripButton
            // 
            this.lockMoveToolStripButton.CheckOnClick = true;
            this.lockMoveToolStripButton.Image = global::UniEye.Base.Properties.Resources.no_entry_32;
            this.lockMoveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lockMoveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lockMoveToolStripButton.Name = "lockMoveToolStripButton";
            this.lockMoveToolStripButton.Size = new System.Drawing.Size(96, 56);
            this.lockMoveToolStripButton.Text = "Lock Move";
            this.lockMoveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.lockMoveToolStripButton.ToolTipText = "Don\'t Move";
            this.lockMoveToolStripButton.Click += new System.EventHandler(this.lockMoveToolStripButton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 59);
            // 
            // undoToolStripButton
            // 
            this.undoToolStripButton.Image = global::UniEye.Base.Properties.Resources.undo_32;
            this.undoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.undoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolStripButton.Name = "undoToolStripButton";
            this.undoToolStripButton.Size = new System.Drawing.Size(54, 56);
            this.undoToolStripButton.Text = "Undo";
            this.undoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.undoToolStripButton.ToolTipText = "Undo";
            this.undoToolStripButton.Visible = false;
            this.undoToolStripButton.Click += new System.EventHandler(this.undoToolStripButton_Click);
            // 
            // RedoToolStripButton
            // 
            this.RedoToolStripButton.Image = global::UniEye.Base.Properties.Resources.redo_32;
            this.RedoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RedoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoToolStripButton.Name = "RedoToolStripButton";
            this.RedoToolStripButton.Size = new System.Drawing.Size(53, 56);
            this.RedoToolStripButton.Text = "Redo";
            this.RedoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.RedoToolStripButton.Visible = false;
            this.RedoToolStripButton.Click += new System.EventHandler(this.RedoToolStripButton_Click);
            // 
            // toolStripButtonSearchProbe
            // 
            this.toolStripButtonSearchProbe.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.toolStripButtonSearchProbe.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSearchProbe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearchProbe.Name = "toolStripButtonSearchProbe";
            this.toolStripButtonSearchProbe.Size = new System.Drawing.Size(63, 56);
            this.toolStripButtonSearchProbe.Text = "Search";
            this.toolStripButtonSearchProbe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonSearchProbe.ToolTipText = "Select Image Folder";
            this.toolStripButtonSearchProbe.Click += new System.EventHandler(this.toolStripButtonSearchProbe_Click);
            // 
            // tabPageView
            // 
            this.tabPageView.Controls.Add(this.toolStripView);
            this.tabPageView.Location = new System.Drawing.Point(4, 29);
            this.tabPageView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageView.Name = "tabPageView";
            this.tabPageView.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageView.Size = new System.Drawing.Size(1187, 66);
            this.tabPageView.TabIndex = 2;
            this.tabPageView.Text = "View";
            this.tabPageView.UseVisualStyleBackColor = true;
            // 
            // toolStripView
            // 
            this.toolStripView.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripView.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripButton,
            this.zoomOutToolStripButton,
            this.zoomFitToolStripButton,
            this.previewToolStripButton,
            this.previewTypeToolStripButton});
            this.toolStripView.Location = new System.Drawing.Point(4, 5);
            this.toolStripView.Name = "toolStripView";
            this.toolStripView.Size = new System.Drawing.Size(1179, 61);
            this.toolStripView.TabIndex = 1;
            this.toolStripView.Text = "toolStrip1";
            // 
            // zoomInToolStripButton
            // 
            this.zoomInToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_in_32;
            this.zoomInToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomInToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomInToolStripButton.Name = "zoomInToolStripButton";
            this.zoomInToolStripButton.Size = new System.Drawing.Size(76, 58);
            this.zoomInToolStripButton.Text = "Zoom In";
            this.zoomInToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomInToolStripButton.Click += new System.EventHandler(this.zoomInToolStripButton_Click);
            // 
            // zoomOutToolStripButton
            // 
            this.zoomOutToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_out_32;
            this.zoomOutToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomOutToolStripButton.Name = "zoomOutToolStripButton";
            this.zoomOutToolStripButton.Size = new System.Drawing.Size(90, 58);
            this.zoomOutToolStripButton.Text = "Zoom Out";
            this.zoomOutToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomOutToolStripButton.Click += new System.EventHandler(this.zoomOutToolStripButton_Click);
            // 
            // zoomFitToolStripButton
            // 
            this.zoomFitToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_fit_32;
            this.zoomFitToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomFitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomFitToolStripButton.Name = "zoomFitToolStripButton";
            this.zoomFitToolStripButton.Size = new System.Drawing.Size(81, 58);
            this.zoomFitToolStripButton.Text = "Zoom Fit";
            this.zoomFitToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomFitToolStripButton.Click += new System.EventHandler(this.zoomFitToolStripButton_Click);
            // 
            // previewToolStripButton
            // 
            this.previewToolStripButton.Image = global::UniEye.Base.Properties.Resources.preview_32;
            this.previewToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.previewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewToolStripButton.Name = "previewToolStripButton";
            this.previewToolStripButton.Size = new System.Drawing.Size(71, 58);
            this.previewToolStripButton.Text = "Preview";
            this.previewToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.previewToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // previewTypeToolStripButton
            // 
            this.previewTypeToolStripButton.AutoSize = false;
            this.previewTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.previewTypeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("previewTypeToolStripButton.Image")));
            this.previewTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewTypeToolStripButton.Name = "previewTypeToolStripButton";
            this.previewTypeToolStripButton.Size = new System.Drawing.Size(100, 58);
            this.previewTypeToolStripButton.Text = "Preview";
            // 
            // tabPageRobot
            // 
            this.tabPageRobot.Controls.Add(this.toolStripRobot);
            this.tabPageRobot.Location = new System.Drawing.Point(4, 29);
            this.tabPageRobot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageRobot.Name = "tabPageRobot";
            this.tabPageRobot.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageRobot.Size = new System.Drawing.Size(1187, 66);
            this.tabPageRobot.TabIndex = 6;
            this.tabPageRobot.Text = "Robot";
            this.tabPageRobot.UseVisualStyleBackColor = true;
            // 
            // toolStripRobot
            // 
            this.toolStripRobot.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripRobot.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRobot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOrigin,
            this.toolStripButtonJoystick,
            this.toolStripButtonRobotSetting,
            this.toolStripButtonStop,
            this.toolStripButtonFineMove,
            this.toolStripSeparator6});
            this.toolStripRobot.Location = new System.Drawing.Point(4, 5);
            this.toolStripRobot.Name = "toolStripRobot";
            this.toolStripRobot.Size = new System.Drawing.Size(1179, 60);
            this.toolStripRobot.TabIndex = 2;
            this.toolStripRobot.Text = "toolStrip1";
            // 
            // toolStripButtonOrigin
            // 
            this.toolStripButtonOrigin.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
            this.toolStripButtonOrigin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOrigin.Name = "toolStripButtonOrigin";
            this.toolStripButtonOrigin.Size = new System.Drawing.Size(59, 57);
            this.toolStripButtonOrigin.Text = "Origin";
            this.toolStripButtonOrigin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonOrigin.Click += new System.EventHandler(this.toolStripButtonOrigin_Click);
            // 
            // toolStripButtonJoystick
            // 
            this.toolStripButtonJoystick.Image = global::UniEye.Base.Properties.Resources.arrow_all;
            this.toolStripButtonJoystick.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonJoystick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonJoystick.Name = "toolStripButtonJoystick";
            this.toolStripButtonJoystick.Size = new System.Drawing.Size(71, 57);
            this.toolStripButtonJoystick.Text = "Joystick";
            this.toolStripButtonJoystick.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonJoystick.Click += new System.EventHandler(this.toolStripButtonJoystick_Click);
            // 
            // toolStripButtonRobotSetting
            // 
            this.toolStripButtonRobotSetting.Image = global::UniEye.Base.Properties.Resources.config_32;
            this.toolStripButtonRobotSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRobotSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRobotSetting.Name = "toolStripButtonRobotSetting";
            this.toolStripButtonRobotSetting.Size = new System.Drawing.Size(67, 57);
            this.toolStripButtonRobotSetting.Text = "Setting";
            this.toolStripButtonRobotSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonRobotSetting.Click += new System.EventHandler(this.toolStripButtonRobotSetting_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Image = global::UniEye.Base.Properties.Resources.stop_32;
            this.toolStripButtonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(49, 57);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripButtonFineMove
            // 
            this.toolStripButtonFineMove.Image = global::UniEye.Base.Properties.Resources.fine_move_32;
            this.toolStripButtonFineMove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonFineMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFineMove.Name = "toolStripButtonFineMove";
            this.toolStripButtonFineMove.Size = new System.Drawing.Size(86, 57);
            this.toolStripButtonFineMove.Text = "FineMove";
            this.toolStripButtonFineMove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonFineMove.Click += new System.EventHandler(this.toolStripButtonFineMove_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 60);
            // 
            // ModellerToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModellerToolbar";
            this.Size = new System.Drawing.Size(1195, 99);
            this.Load += new System.EventHandler(this.ModellerToolbar_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageModel.ResumeLayout(false);
            this.tabPageModel.PerformLayout();
            this.toolStripModel.ResumeLayout(false);
            this.toolStripModel.PerformLayout();
            this.tabPageFov.ResumeLayout(false);
            this.toolStripStep.ResumeLayout(false);
            this.toolStripStep.PerformLayout();
            this.tabPageProbe.ResumeLayout(false);
            this.toolStripProbe.ResumeLayout(false);
            this.toolStripProbe.PerformLayout();
            this.tabPageView.ResumeLayout(false);
            this.tabPageView.PerformLayout();
            this.toolStripView.ResumeLayout(false);
            this.toolStripView.PerformLayout();
            this.tabPageRobot.ResumeLayout(false);
            this.tabPageRobot.PerformLayout();
            this.toolStripRobot.ResumeLayout(false);
            this.toolStripRobot.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageModel;
        private System.Windows.Forms.ToolStrip toolStripModel;
        private System.Windows.Forms.ToolStripButton modelPropertyButton;
        public System.Windows.Forms.ToolStripButton exportFormatButton;
        private System.Windows.Forms.ToolStripButton editSchemaButton;
        private System.Windows.Forms.ToolStripButton scanButton;
        private System.Windows.Forms.TabPage tabPageFov;
        private System.Windows.Forms.ToolStrip toolStripStep;
        private System.Windows.Forms.ToolStripLabel toolStripLabelStep;
        public System.Windows.Forms.ToolStripComboBox comboInspectStep;
        private System.Windows.Forms.ToolStripButton movePrevStepButton;
        private System.Windows.Forms.ToolStripButton moveNextStepButton;
        private System.Windows.Forms.ToolStripButton addStepButton;
        private System.Windows.Forms.ToolStripButton deleteStepButton;
        private System.Windows.Forms.ToolStripButton editStepButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripDropDownButton selectCameraButton;
        private System.Windows.Forms.ToolStripButton loadImageSetToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton selectLightButton;
        private System.Windows.Forms.ToolStripButton showLightPanelToolStripButton;
        private System.Windows.Forms.ToolStripButton grabProcessToolStripButton;
        private System.Windows.Forms.ToolStripButton singleShotToolStripButton;
        public System.Windows.Forms.ToolStripButton multiShotToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlign;
        private System.Windows.Forms.TabPage tabPageProbe;
        private System.Windows.Forms.ToolStrip toolStripProbe;
        private System.Windows.Forms.ToolStripDropDownButton addProbeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton copyProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteProbeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton groupProbeToolStripButton;
        public System.Windows.Forms.ToolStripButton ungroupProbeToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparatorGroup;
        private System.Windows.Forms.ToolStripButton syncParamToolStripButton;
        private System.Windows.Forms.ToolStripButton syncAllToolStripButton;
        private System.Windows.Forms.ToolStripButton lockMoveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        public System.Windows.Forms.ToolStripButton undoToolStripButton;
        public System.Windows.Forms.ToolStripButton RedoToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearchProbe;
        private System.Windows.Forms.TabPage tabPageView;
        private System.Windows.Forms.ToolStrip toolStripView;
        private System.Windows.Forms.ToolStripButton zoomInToolStripButton;
        private System.Windows.Forms.ToolStripButton zoomOutToolStripButton;
        private System.Windows.Forms.ToolStripButton zoomFitToolStripButton;
        private System.Windows.Forms.ToolStripButton previewToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton previewTypeToolStripButton;
        private System.Windows.Forms.TabPage tabPageRobot;
        private System.Windows.Forms.ToolStrip toolStripRobot;
        private System.Windows.Forms.ToolStripButton toolStripButtonOrigin;
        private System.Windows.Forms.ToolStripButton toolStripButtonJoystick;
        private System.Windows.Forms.ToolStripButton toolStripButtonRobotSetting;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripButton toolStripButtonFineMove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton createSchemaButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnPrevImage;
        private System.Windows.Forms.ToolStripButton toolBtnNextImage;
    }
}
