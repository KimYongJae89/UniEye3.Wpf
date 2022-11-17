namespace UniEye.Base.UI.Main
{
    partial class SettingPage
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelRemoteResultPath = new System.Windows.Forms.Label();
            this.remoteResultPath = new System.Windows.Forms.TextBox();
            this.buttonSelectRemoteFolder = new System.Windows.Forms.Button();
            this.buttonSelectLocalFolder = new System.Windows.Forms.Button();
            this.localResultPath = new System.Windows.Forms.TextBox();
            this.labelLocalResultPath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonShowIoPortViewer = new System.Windows.Forms.Button();
            this.groupBoxRemoteBackup = new System.Windows.Forms.GroupBox();
            this.useRemoteBackup = new System.Windows.Forms.CheckBox();
            this.resultStoringDays = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cpuUsage = new System.Windows.Forms.NumericUpDown();
            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelImageCheckInterval = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.checkBoxDebugMode = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCenterGuide = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.labelMs = new System.Windows.Forms.Label();
            this.useDefectReview = new System.Windows.Forms.CheckBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.labelDefaultExposureTime = new System.Windows.Forms.Label();
            this.labelDelayInspectionTime = new System.Windows.Forms.Label();
            this.defaultExposureTime = new System.Windows.Forms.NumericUpDown();
            this.inspectionDelayTime = new System.Windows.Forms.NumericUpDown();
            this.txtCenterGuideThickness = new System.Windows.Forms.TextBox();
            this.labelCenterGuideThickness = new System.Windows.Forms.Label();
            this.txtCenterGuideOffsetY = new System.Windows.Forms.TextBox();
            this.txtCenterGuideOffsetX = new System.Windows.Forms.TextBox();
            this.labelCenterGuideOffsetY = new System.Windows.Forms.Label();
            this.labelCenterGuideOffsetX = new System.Windows.Forms.Label();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.tabPageMachine = new System.Windows.Forms.TabPage();
            this.buttonTowerLampConfig = new System.Windows.Forms.Button();
            this.buttonRobotCalibration = new System.Windows.Forms.Button();
            this.labelTowerLampDelay = new System.Windows.Forms.Label();
            this.groupRejectPusher = new System.Windows.Forms.GroupBox();
            this.useRejectPusher = new System.Windows.Forms.CheckBox();
            this.rejectWaitTime = new System.Windows.Forms.NumericUpDown();
            this.rejectPusherPullTime = new System.Windows.Forms.NumericUpDown();
            this.rejectPusherPushTime = new System.Windows.Forms.NumericUpDown();
            this.labelMs3 = new System.Windows.Forms.Label();
            this.labelMs2 = new System.Windows.Forms.Label();
            this.labelMs1 = new System.Windows.Forms.Label();
            this.labelTimeoutTime = new System.Windows.Forms.Label();
            this.labelElapsedTime = new System.Windows.Forms.Label();
            this.labelInterval = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonMotionController = new System.Windows.Forms.Button();
            this.towerLampDelay = new System.Windows.Forms.NumericUpDown();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.labelTrigDelay = new System.Windows.Forms.Label();
            this.labelMs5 = new System.Windows.Forms.Label();
            this.triggerDelay = new System.Windows.Forms.NumericUpDown();
            this.buttonCameraAlign = new System.Windows.Forms.Button();
            this.buttonCameraCalibration = new System.Windows.Forms.Button();
            this.changeUserButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.buttonUserManager = new System.Windows.Forms.Button();
            this.groupBoxRemoteBackup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultStoringDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpuUsage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defaultExposureTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspectionDelayTime)).BeginInit();
            this.tabPageData.SuspendLayout();
            this.tabPageMachine.SuspendLayout();
            this.groupRejectPusher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rejectWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rejectPusherPullTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rejectPusherPushTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.towerLampDelay)).BeginInit();
            this.tabPageCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRemoteResultPath
            // 
            this.labelRemoteResultPath.AutoSize = true;
            this.labelRemoteResultPath.Location = new System.Drawing.Point(15, 65);
            this.labelRemoteResultPath.Name = "labelRemoteResultPath";
            this.labelRemoteResultPath.Size = new System.Drawing.Size(141, 18);
            this.labelRemoteResultPath.TabIndex = 0;
            this.labelRemoteResultPath.Text = "Remote Result Path";
            // 
            // remoteResultPath
            // 
            this.remoteResultPath.Location = new System.Drawing.Point(161, 62);
            this.remoteResultPath.Name = "remoteResultPath";
            this.remoteResultPath.Size = new System.Drawing.Size(296, 24);
            this.remoteResultPath.TabIndex = 1;
            // 
            // buttonSelectRemoteFolder
            // 
            this.buttonSelectRemoteFolder.Location = new System.Drawing.Point(463, 59);
            this.buttonSelectRemoteFolder.Name = "buttonSelectRemoteFolder";
            this.buttonSelectRemoteFolder.Size = new System.Drawing.Size(45, 28);
            this.buttonSelectRemoteFolder.TabIndex = 2;
            this.buttonSelectRemoteFolder.Text = "...";
            this.buttonSelectRemoteFolder.UseVisualStyleBackColor = true;
            this.buttonSelectRemoteFolder.Click += new System.EventHandler(this.buttonSelectRemoteFolder_Click);
            // 
            // buttonSelectLocalFolder
            // 
            this.buttonSelectLocalFolder.Location = new System.Drawing.Point(469, 20);
            this.buttonSelectLocalFolder.Name = "buttonSelectLocalFolder";
            this.buttonSelectLocalFolder.Size = new System.Drawing.Size(45, 28);
            this.buttonSelectLocalFolder.TabIndex = 5;
            this.buttonSelectLocalFolder.Text = "...";
            this.buttonSelectLocalFolder.UseVisualStyleBackColor = true;
            this.buttonSelectLocalFolder.Click += new System.EventHandler(this.buttonSelectLocalFolder_Click);
            // 
            // localResultPath
            // 
            this.localResultPath.Location = new System.Drawing.Point(167, 20);
            this.localResultPath.Name = "localResultPath";
            this.localResultPath.Size = new System.Drawing.Size(296, 24);
            this.localResultPath.TabIndex = 4;
            // 
            // labelLocalResultPath
            // 
            this.labelLocalResultPath.AutoSize = true;
            this.labelLocalResultPath.Location = new System.Drawing.Point(14, 23);
            this.labelLocalResultPath.Name = "labelLocalResultPath";
            this.labelLocalResultPath.Size = new System.Drawing.Size(124, 18);
            this.labelLocalResultPath.TabIndex = 3;
            this.labelLocalResultPath.Text = "Local Result Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Image Keeping Days";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(265, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "day";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(265, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "%";
            // 
            // buttonShowIoPortViewer
            // 
            this.buttonShowIoPortViewer.Location = new System.Drawing.Point(20, 13);
            this.buttonShowIoPortViewer.Name = "buttonShowIoPortViewer";
            this.buttonShowIoPortViewer.Size = new System.Drawing.Size(172, 40);
            this.buttonShowIoPortViewer.TabIndex = 13;
            this.buttonShowIoPortViewer.Text = "I/O Port Viewer";
            this.buttonShowIoPortViewer.UseVisualStyleBackColor = true;
            this.buttonShowIoPortViewer.Click += new System.EventHandler(this.buttonShowIoPortViewer_Click);
            // 
            // groupBoxRemoteBackup
            // 
            this.groupBoxRemoteBackup.Controls.Add(this.useRemoteBackup);
            this.groupBoxRemoteBackup.Controls.Add(this.labelRemoteResultPath);
            this.groupBoxRemoteBackup.Controls.Add(this.remoteResultPath);
            this.groupBoxRemoteBackup.Controls.Add(this.buttonSelectRemoteFolder);
            this.groupBoxRemoteBackup.Controls.Add(this.label2);
            this.groupBoxRemoteBackup.Controls.Add(this.label5);
            this.groupBoxRemoteBackup.Controls.Add(this.resultStoringDays);
            this.groupBoxRemoteBackup.Controls.Add(this.label4);
            this.groupBoxRemoteBackup.Controls.Add(this.label3);
            this.groupBoxRemoteBackup.Controls.Add(this.cpuUsage);
            this.groupBoxRemoteBackup.Location = new System.Drawing.Point(6, 54);
            this.groupBoxRemoteBackup.Name = "groupBoxRemoteBackup";
            this.groupBoxRemoteBackup.Size = new System.Drawing.Size(518, 193);
            this.groupBoxRemoteBackup.TabIndex = 14;
            this.groupBoxRemoteBackup.TabStop = false;
            this.groupBoxRemoteBackup.Text = "Remote Backup";
            // 
            // useRemoteBackup
            // 
            this.useRemoteBackup.AutoSize = true;
            this.useRemoteBackup.Location = new System.Drawing.Point(18, 29);
            this.useRemoteBackup.Name = "useRemoteBackup";
            this.useRemoteBackup.Size = new System.Drawing.Size(165, 22);
            this.useRemoteBackup.TabIndex = 12;
            this.useRemoteBackup.Text = "Use Remote Backup";
            this.useRemoteBackup.UseVisualStyleBackColor = true;
            // 
            // resultStoringDays
            // 
            this.resultStoringDays.Location = new System.Drawing.Point(191, 120);
            this.resultStoringDays.Name = "resultStoringDays";
            this.resultStoringDays.Size = new System.Drawing.Size(68, 24);
            this.resultStoringDays.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Lower Than CPU Usage ";
            // 
            // cpuUsage
            // 
            this.cpuUsage.Location = new System.Drawing.Point(191, 151);
            this.cpuUsage.Name = "cpuUsage";
            this.cpuUsage.Size = new System.Drawing.Size(68, 24);
            this.cpuUsage.TabIndex = 13;
            // 
            // imageCheckInterval
            // 
            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
            this.imageCheckInterval.Name = "imageCheckInterval";
            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.imageCheckInterval.TabIndex = 13;
            // 
            // labelImageCheckInterval
            // 
            this.labelImageCheckInterval.AutoSize = true;
            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
            this.labelImageCheckInterval.TabIndex = 6;
            this.labelImageCheckInterval.Text = "Image Check Interval";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(286, 126);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(29, 17);
            this.labelSec.TabIndex = 10;
            this.labelSec.Text = "sec";
            // 
            // checkBoxDebugMode
            // 
            this.checkBoxDebugMode.AutoSize = true;
            this.checkBoxDebugMode.Location = new System.Drawing.Point(19, 126);
            this.checkBoxDebugMode.Name = "checkBoxDebugMode";
            this.checkBoxDebugMode.Size = new System.Drawing.Size(112, 22);
            this.checkBoxDebugMode.TabIndex = 16;
            this.checkBoxDebugMode.Text = "Debug Mode";
            this.checkBoxDebugMode.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowCenterGuide
            // 
            this.checkBoxShowCenterGuide.AutoSize = true;
            this.checkBoxShowCenterGuide.Location = new System.Drawing.Point(19, 21);
            this.checkBoxShowCenterGuide.Name = "checkBoxShowCenterGuide";
            this.checkBoxShowCenterGuide.Size = new System.Drawing.Size(156, 22);
            this.checkBoxShowCenterGuide.TabIndex = 16;
            this.checkBoxShowCenterGuide.Text = "Show Center Guide";
            this.checkBoxShowCenterGuide.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(493, 420);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(129, 41);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // labelMs
            // 
            this.labelMs.Location = new System.Drawing.Point(0, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(100, 23);
            this.labelMs.TabIndex = 0;
            // 
            // useDefectReview
            // 
            this.useDefectReview.AutoSize = true;
            this.useDefectReview.Location = new System.Drawing.Point(19, 72);
            this.useDefectReview.Name = "useDefectReview";
            this.useDefectReview.Size = new System.Drawing.Size(122, 22);
            this.useDefectReview.TabIndex = 16;
            this.useDefectReview.Text = "Defect Review";
            this.useDefectReview.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGeneral);
            this.tabControlMain.Controls.Add(this.tabPageData);
            this.tabControlMain.Controls.Add(this.tabPageMachine);
            this.tabControlMain.Controls.Add(this.tabPageCamera);
            this.tabControlMain.Location = new System.Drawing.Point(15, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(607, 402);
            this.tabControlMain.TabIndex = 158;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.labelDefaultExposureTime);
            this.tabPageGeneral.Controls.Add(this.labelDelayInspectionTime);
            this.tabPageGeneral.Controls.Add(this.defaultExposureTime);
            this.tabPageGeneral.Controls.Add(this.inspectionDelayTime);
            this.tabPageGeneral.Controls.Add(this.txtCenterGuideThickness);
            this.tabPageGeneral.Controls.Add(this.labelCenterGuideThickness);
            this.tabPageGeneral.Controls.Add(this.txtCenterGuideOffsetY);
            this.tabPageGeneral.Controls.Add(this.txtCenterGuideOffsetX);
            this.tabPageGeneral.Controls.Add(this.labelCenterGuideOffsetY);
            this.tabPageGeneral.Controls.Add(this.labelCenterGuideOffsetX);
            this.tabPageGeneral.Controls.Add(this.checkBoxShowCenterGuide);
            this.tabPageGeneral.Controls.Add(this.checkBoxDebugMode);
            this.tabPageGeneral.Controls.Add(this.useDefectReview);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 27);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(599, 371);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // labelDefaultExposureTime
            // 
            this.labelDefaultExposureTime.AutoSize = true;
            this.labelDefaultExposureTime.Location = new System.Drawing.Point(16, 154);
            this.labelDefaultExposureTime.Name = "labelDefaultExposureTime";
            this.labelDefaultExposureTime.Size = new System.Drawing.Size(158, 18);
            this.labelDefaultExposureTime.TabIndex = 26;
            this.labelDefaultExposureTime.Text = "Default Exposure Time";
            // 
            // labelDelayInspectionTime
            // 
            this.labelDelayInspectionTime.AutoSize = true;
            this.labelDelayInspectionTime.Location = new System.Drawing.Point(16, 102);
            this.labelDelayInspectionTime.Name = "labelDelayInspectionTime";
            this.labelDelayInspectionTime.Size = new System.Drawing.Size(153, 18);
            this.labelDelayInspectionTime.TabIndex = 26;
            this.labelDelayInspectionTime.Text = "Inspection Delay Time";
            // 
            // defaultExposureTime
            // 
            this.defaultExposureTime.Location = new System.Drawing.Point(190, 152);
            this.defaultExposureTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.defaultExposureTime.Name = "defaultExposureTime";
            this.defaultExposureTime.Size = new System.Drawing.Size(73, 24);
            this.defaultExposureTime.TabIndex = 25;
            // 
            // inspectionDelayTime
            // 
            this.inspectionDelayTime.Location = new System.Drawing.Point(190, 100);
            this.inspectionDelayTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inspectionDelayTime.Name = "inspectionDelayTime";
            this.inspectionDelayTime.Size = new System.Drawing.Size(73, 24);
            this.inspectionDelayTime.TabIndex = 25;
            // 
            // txtCenterGuideThickness
            // 
            this.txtCenterGuideThickness.Location = new System.Drawing.Point(439, 42);
            this.txtCenterGuideThickness.Name = "txtCenterGuideThickness";
            this.txtCenterGuideThickness.Size = new System.Drawing.Size(78, 24);
            this.txtCenterGuideThickness.TabIndex = 23;
            // 
            // labelCenterGuideThickness
            // 
            this.labelCenterGuideThickness.AutoSize = true;
            this.labelCenterGuideThickness.Location = new System.Drawing.Point(356, 45);
            this.labelCenterGuideThickness.Name = "labelCenterGuideThickness";
            this.labelCenterGuideThickness.Size = new System.Drawing.Size(88, 18);
            this.labelCenterGuideThickness.TabIndex = 22;
            this.labelCenterGuideThickness.Text = "Thickness : ";
            // 
            // txtCenterGuideOffsetY
            // 
            this.txtCenterGuideOffsetY.Location = new System.Drawing.Point(272, 42);
            this.txtCenterGuideOffsetY.Name = "txtCenterGuideOffsetY";
            this.txtCenterGuideOffsetY.Size = new System.Drawing.Size(78, 24);
            this.txtCenterGuideOffsetY.TabIndex = 21;
            // 
            // txtCenterGuideOffsetX
            // 
            this.txtCenterGuideOffsetX.Location = new System.Drawing.Point(119, 42);
            this.txtCenterGuideOffsetX.Name = "txtCenterGuideOffsetX";
            this.txtCenterGuideOffsetX.Size = new System.Drawing.Size(69, 24);
            this.txtCenterGuideOffsetX.TabIndex = 19;
            // 
            // labelCenterGuideOffsetY
            // 
            this.labelCenterGuideOffsetY.AutoSize = true;
            this.labelCenterGuideOffsetY.Location = new System.Drawing.Point(194, 45);
            this.labelCenterGuideOffsetY.Name = "labelCenterGuideOffsetY";
            this.labelCenterGuideOffsetY.Size = new System.Drawing.Size(69, 18);
            this.labelCenterGuideOffsetY.TabIndex = 18;
            this.labelCenterGuideOffsetY.Text = "Offset Y :";
            // 
            // labelCenterGuideOffsetX
            // 
            this.labelCenterGuideOffsetX.AutoSize = true;
            this.labelCenterGuideOffsetX.Location = new System.Drawing.Point(41, 45);
            this.labelCenterGuideOffsetX.Name = "labelCenterGuideOffsetX";
            this.labelCenterGuideOffsetX.Size = new System.Drawing.Size(70, 18);
            this.labelCenterGuideOffsetX.TabIndex = 17;
            this.labelCenterGuideOffsetX.Text = "Offset X :";
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.groupBoxRemoteBackup);
            this.tabPageData.Controls.Add(this.labelLocalResultPath);
            this.tabPageData.Controls.Add(this.localResultPath);
            this.tabPageData.Controls.Add(this.buttonSelectLocalFolder);
            this.tabPageData.Location = new System.Drawing.Point(4, 27);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(599, 371);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // tabPageMachine
            // 
            this.tabPageMachine.Controls.Add(this.buttonTowerLampConfig);
            this.tabPageMachine.Controls.Add(this.buttonRobotCalibration);
            this.tabPageMachine.Controls.Add(this.labelTowerLampDelay);
            this.tabPageMachine.Controls.Add(this.groupRejectPusher);
            this.tabPageMachine.Controls.Add(this.buttonShowIoPortViewer);
            this.tabPageMachine.Controls.Add(this.label6);
            this.tabPageMachine.Controls.Add(this.buttonMotionController);
            this.tabPageMachine.Controls.Add(this.towerLampDelay);
            this.tabPageMachine.Location = new System.Drawing.Point(4, 27);
            this.tabPageMachine.Name = "tabPageMachine";
            this.tabPageMachine.Size = new System.Drawing.Size(599, 371);
            this.tabPageMachine.TabIndex = 2;
            this.tabPageMachine.Text = "Machine";
            this.tabPageMachine.UseVisualStyleBackColor = true;
            // 
            // buttonTowerLampConfig
            // 
            this.buttonTowerLampConfig.Location = new System.Drawing.Point(214, 13);
            this.buttonTowerLampConfig.Name = "buttonTowerLampConfig";
            this.buttonTowerLampConfig.Size = new System.Drawing.Size(172, 40);
            this.buttonTowerLampConfig.TabIndex = 172;
            this.buttonTowerLampConfig.Text = "Tower Lamp";
            this.buttonTowerLampConfig.UseVisualStyleBackColor = true;
            this.buttonTowerLampConfig.Click += new System.EventHandler(this.buttonTowerLampConfig_Click);
            // 
            // buttonRobotCalibration
            // 
            this.buttonRobotCalibration.Location = new System.Drawing.Point(20, 290);
            this.buttonRobotCalibration.Name = "buttonRobotCalibration";
            this.buttonRobotCalibration.Size = new System.Drawing.Size(172, 40);
            this.buttonRobotCalibration.TabIndex = 172;
            this.buttonRobotCalibration.Text = "Robot Calibration";
            this.buttonRobotCalibration.UseVisualStyleBackColor = true;
            this.buttonRobotCalibration.Click += new System.EventHandler(this.buttonRobotCalibration_Click);
            // 
            // labelTowerLampDelay
            // 
            this.labelTowerLampDelay.AutoSize = true;
            this.labelTowerLampDelay.Location = new System.Drawing.Point(17, 310);
            this.labelTowerLampDelay.Name = "labelTowerLampDelay";
            this.labelTowerLampDelay.Size = new System.Drawing.Size(132, 18);
            this.labelTowerLampDelay.TabIndex = 167;
            this.labelTowerLampDelay.Text = "Tower Lamp Delay";
            // 
            // groupRejectPusher
            // 
            this.groupRejectPusher.Controls.Add(this.useRejectPusher);
            this.groupRejectPusher.Controls.Add(this.rejectWaitTime);
            this.groupRejectPusher.Controls.Add(this.rejectPusherPullTime);
            this.groupRejectPusher.Controls.Add(this.rejectPusherPushTime);
            this.groupRejectPusher.Controls.Add(this.labelMs3);
            this.groupRejectPusher.Controls.Add(this.labelMs2);
            this.groupRejectPusher.Controls.Add(this.labelMs1);
            this.groupRejectPusher.Controls.Add(this.labelTimeoutTime);
            this.groupRejectPusher.Controls.Add(this.labelElapsedTime);
            this.groupRejectPusher.Controls.Add(this.labelInterval);
            this.groupRejectPusher.Location = new System.Drawing.Point(20, 179);
            this.groupRejectPusher.Name = "groupRejectPusher";
            this.groupRejectPusher.Size = new System.Drawing.Size(241, 117);
            this.groupRejectPusher.TabIndex = 166;
            this.groupRejectPusher.TabStop = false;
            // 
            // useRejectPusher
            // 
            this.useRejectPusher.AutoSize = true;
            this.useRejectPusher.BackColor = System.Drawing.Color.White;
            this.useRejectPusher.Location = new System.Drawing.Point(9, -2);
            this.useRejectPusher.Name = "useRejectPusher";
            this.useRejectPusher.Size = new System.Drawing.Size(151, 22);
            this.useRejectPusher.TabIndex = 169;
            this.useRejectPusher.Text = "Use Reject Pusher";
            this.useRejectPusher.UseVisualStyleBackColor = false;
            // 
            // rejectWaitTime
            // 
            this.rejectWaitTime.Location = new System.Drawing.Point(111, 26);
            this.rejectWaitTime.Name = "rejectWaitTime";
            this.rejectWaitTime.Size = new System.Drawing.Size(74, 24);
            this.rejectWaitTime.TabIndex = 172;
            // 
            // rejectPusherPullTime
            // 
            this.rejectPusherPullTime.Location = new System.Drawing.Point(111, 80);
            this.rejectPusherPullTime.Name = "rejectPusherPullTime";
            this.rejectPusherPullTime.Size = new System.Drawing.Size(74, 24);
            this.rejectPusherPullTime.TabIndex = 172;
            // 
            // rejectPusherPushTime
            // 
            this.rejectPusherPushTime.Location = new System.Drawing.Point(111, 53);
            this.rejectPusherPushTime.Name = "rejectPusherPushTime";
            this.rejectPusherPushTime.Size = new System.Drawing.Size(74, 24);
            this.rejectPusherPushTime.TabIndex = 172;
            // 
            // labelMs3
            // 
            this.labelMs3.AutoSize = true;
            this.labelMs3.Location = new System.Drawing.Point(191, 82);
            this.labelMs3.Name = "labelMs3";
            this.labelMs3.Size = new System.Drawing.Size(29, 18);
            this.labelMs3.TabIndex = 170;
            this.labelMs3.Text = "ms";
            // 
            // labelMs2
            // 
            this.labelMs2.AutoSize = true;
            this.labelMs2.Location = new System.Drawing.Point(191, 57);
            this.labelMs2.Name = "labelMs2";
            this.labelMs2.Size = new System.Drawing.Size(29, 18);
            this.labelMs2.TabIndex = 170;
            this.labelMs2.Text = "ms";
            // 
            // labelMs1
            // 
            this.labelMs1.AutoSize = true;
            this.labelMs1.Location = new System.Drawing.Point(191, 28);
            this.labelMs1.Name = "labelMs1";
            this.labelMs1.Size = new System.Drawing.Size(29, 18);
            this.labelMs1.TabIndex = 170;
            this.labelMs1.Text = "ms";
            // 
            // labelTimeoutTime
            // 
            this.labelTimeoutTime.AutoSize = true;
            this.labelTimeoutTime.Location = new System.Drawing.Point(10, 28);
            this.labelTimeoutTime.Name = "labelTimeoutTime";
            this.labelTimeoutTime.Size = new System.Drawing.Size(75, 18);
            this.labelTimeoutTime.TabIndex = 170;
            this.labelTimeoutTime.Text = "Wait Time";
            // 
            // labelElapsedTime
            // 
            this.labelElapsedTime.AutoSize = true;
            this.labelElapsedTime.Location = new System.Drawing.Point(10, 55);
            this.labelElapsedTime.Name = "labelElapsedTime";
            this.labelElapsedTime.Size = new System.Drawing.Size(79, 18);
            this.labelElapsedTime.TabIndex = 167;
            this.labelElapsedTime.Text = "Push Time";
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(10, 82);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(69, 18);
            this.labelInterval.TabIndex = 166;
            this.labelInterval.Text = "Pull Time";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 18);
            this.label6.TabIndex = 170;
            this.label6.Text = "ms";
            // 
            // buttonMotionController
            // 
            this.buttonMotionController.Location = new System.Drawing.Point(20, 59);
            this.buttonMotionController.Name = "buttonMotionController";
            this.buttonMotionController.Size = new System.Drawing.Size(172, 40);
            this.buttonMotionController.TabIndex = 12;
            this.buttonMotionController.Text = "Motion Controller";
            this.buttonMotionController.UseVisualStyleBackColor = true;
            this.buttonMotionController.Click += new System.EventHandler(this.buttonMotionController_Click);
            // 
            // towerLampDelay
            // 
            this.towerLampDelay.Location = new System.Drawing.Point(171, 308);
            this.towerLampDelay.Name = "towerLampDelay";
            this.towerLampDelay.Size = new System.Drawing.Size(73, 24);
            this.towerLampDelay.TabIndex = 157;
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.labelTrigDelay);
            this.tabPageCamera.Controls.Add(this.labelMs5);
            this.tabPageCamera.Controls.Add(this.triggerDelay);
            this.tabPageCamera.Controls.Add(this.buttonCameraAlign);
            this.tabPageCamera.Controls.Add(this.buttonCameraCalibration);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 27);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCamera.Size = new System.Drawing.Size(599, 371);
            this.tabPageCamera.TabIndex = 3;
            this.tabPageCamera.Text = "Camera";
            this.tabPageCamera.UseVisualStyleBackColor = true;
            // 
            // labelTrigDelay
            // 
            this.labelTrigDelay.AutoSize = true;
            this.labelTrigDelay.Location = new System.Drawing.Point(6, 147);
            this.labelTrigDelay.Name = "labelTrigDelay";
            this.labelTrigDelay.Size = new System.Drawing.Size(95, 18);
            this.labelTrigDelay.TabIndex = 175;
            this.labelTrigDelay.Text = "Tirgger Delay";
            // 
            // labelMs5
            // 
            this.labelMs5.AutoSize = true;
            this.labelMs5.Location = new System.Drawing.Point(226, 143);
            this.labelMs5.Name = "labelMs5";
            this.labelMs5.Size = new System.Drawing.Size(29, 18);
            this.labelMs5.TabIndex = 176;
            this.labelMs5.Text = "ms";
            // 
            // triggerDelay
            // 
            this.triggerDelay.Location = new System.Drawing.Point(147, 141);
            this.triggerDelay.Name = "triggerDelay";
            this.triggerDelay.Size = new System.Drawing.Size(73, 24);
            this.triggerDelay.TabIndex = 173;
            // 
            // buttonCameraAlign
            // 
            this.buttonCameraAlign.Location = new System.Drawing.Point(6, 57);
            this.buttonCameraAlign.Name = "buttonCameraAlign";
            this.buttonCameraAlign.Size = new System.Drawing.Size(214, 40);
            this.buttonCameraAlign.TabIndex = 168;
            this.buttonCameraAlign.Text = "Camera Alignment";
            this.buttonCameraAlign.UseVisualStyleBackColor = true;
            // 
            // buttonCameraCalibration
            // 
            this.buttonCameraCalibration.Location = new System.Drawing.Point(6, 11);
            this.buttonCameraCalibration.Name = "buttonCameraCalibration";
            this.buttonCameraCalibration.Size = new System.Drawing.Size(214, 40);
            this.buttonCameraCalibration.TabIndex = 169;
            this.buttonCameraCalibration.Text = "Camera Calibration";
            this.buttonCameraCalibration.UseVisualStyleBackColor = true;
            this.buttonCameraCalibration.Click += new System.EventHandler(this.buttonCameraCalibration_Click);
            // 
            // changeUserButton
            // 
            this.changeUserButton.Location = new System.Drawing.Point(15, 416);
            this.changeUserButton.Name = "changeUserButton";
            this.changeUserButton.Size = new System.Drawing.Size(129, 41);
            this.changeUserButton.TabIndex = 13;
            this.changeUserButton.Text = "Change User";
            this.changeUserButton.UseVisualStyleBackColor = true;
            this.changeUserButton.Click += new System.EventHandler(this.changeUserButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // buttonUserManager
            // 
            this.buttonUserManager.Location = new System.Drawing.Point(493, 467);
            this.buttonUserManager.Name = "buttonUserManager";
            this.buttonUserManager.Size = new System.Drawing.Size(129, 40);
            this.buttonUserManager.TabIndex = 27;
            this.buttonUserManager.Text = "User Manager";
            this.buttonUserManager.UseVisualStyleBackColor = true;
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonUserManager);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.changeUserButton);
            this.Controls.Add(this.saveButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(694, 572);
            this.groupBoxRemoteBackup.ResumeLayout(false);
            this.groupBoxRemoteBackup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultStoringDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpuUsage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defaultExposureTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspectionDelayTime)).EndInit();
            this.tabPageData.ResumeLayout(false);
            this.tabPageData.PerformLayout();
            this.tabPageMachine.ResumeLayout(false);
            this.tabPageMachine.PerformLayout();
            this.groupRejectPusher.ResumeLayout(false);
            this.groupRejectPusher.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rejectWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rejectPusherPullTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rejectPusherPushTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.towerLampDelay)).EndInit();
            this.tabPageCamera.ResumeLayout(false);
            this.tabPageCamera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelRemoteResultPath;
        private System.Windows.Forms.TextBox remoteResultPath;
        private System.Windows.Forms.Button buttonSelectRemoteFolder;
        private System.Windows.Forms.Button buttonSelectLocalFolder;
        private System.Windows.Forms.TextBox localResultPath;
        private System.Windows.Forms.Label labelLocalResultPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonShowIoPortViewer;
        private System.Windows.Forms.GroupBox groupBoxRemoteBackup;
        private System.Windows.Forms.CheckBox useRemoteBackup;
        private System.Windows.Forms.CheckBox checkBoxDebugMode;
        private System.Windows.Forms.CheckBox checkBoxShowCenterGuide;
        private System.Windows.Forms.CheckBox useDefectReview;
        private System.Windows.Forms.NumericUpDown cpuUsage;
        private System.Windows.Forms.NumericUpDown resultStoringDays;
        private System.Windows.Forms.NumericUpDown imageCheckInterval;
        private System.Windows.Forms.Label labelImageCheckInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.TabPage tabPageMachine;
        private System.Windows.Forms.Button changeUserButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtCenterGuideOffsetY;
        private System.Windows.Forms.TextBox txtCenterGuideOffsetX;
        private System.Windows.Forms.Label labelCenterGuideOffsetY;
        private System.Windows.Forms.Label labelCenterGuideOffsetX;
        private System.Windows.Forms.Label labelCenterGuideThickness;
        private System.Windows.Forms.TextBox txtCenterGuideThickness;
        private System.Windows.Forms.GroupBox groupRejectPusher;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.Label labelElapsedTime;
        private System.Windows.Forms.Label labelTimeoutTime;
        private System.Windows.Forms.CheckBox useRejectPusher;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown inspectionDelayTime;
        private System.Windows.Forms.Label labelDelayInspectionTime;
        private System.Windows.Forms.NumericUpDown rejectPusherPushTime;
        private System.Windows.Forms.NumericUpDown rejectWaitTime;
        private System.Windows.Forms.NumericUpDown rejectPusherPullTime;
        private System.Windows.Forms.Label labelMs3;
        private System.Windows.Forms.Label labelMs2;
        private System.Windows.Forms.Label labelMs1;
        private System.Windows.Forms.Label labelDefaultExposureTime;
        private System.Windows.Forms.NumericUpDown defaultExposureTime;
        private System.Windows.Forms.Button buttonMotionController;
        private System.Windows.Forms.Label labelTowerLampDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown towerLampDelay;
        private System.Windows.Forms.TabPage tabPageCamera;
        private System.Windows.Forms.Label labelTrigDelay;
        private System.Windows.Forms.Label labelMs5;
        private System.Windows.Forms.NumericUpDown triggerDelay;
        private System.Windows.Forms.Button buttonCameraAlign;
        private System.Windows.Forms.Button buttonCameraCalibration;
        private System.Windows.Forms.Button buttonUserManager;
        private System.Windows.Forms.Button buttonTowerLampConfig;
        private System.Windows.Forms.Button buttonRobotCalibration;
    }
}
