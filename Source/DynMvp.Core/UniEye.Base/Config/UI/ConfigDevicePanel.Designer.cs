namespace UniEye.Base.Settings.UI
{
    partial class ConfigDevicePanel
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
            this.buttonSelectGrabber = new System.Windows.Forms.Button();
            this.buttonSelectMotion = new System.Windows.Forms.Button();
            this.buttonSelectLightCtrl = new System.Windows.Forms.Button();
            this.buttonSelectDigitalIo = new System.Windows.Forms.Button();
            this.buttonSelectDaq = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.useDoorSensor = new System.Windows.Forms.CheckBox();
            this.useModelBarcode = new System.Windows.Forms.CheckBox();
            this.useFovNavigator = new System.Windows.Forms.CheckBox();
            this.useRobotStage = new System.Windows.Forms.CheckBox();
            this.useConveyorMotor = new System.Windows.Forms.CheckBox();
            this.columnNumPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDeviceList = new System.Windows.Forms.DataGridView();
            this.panelOption = new System.Windows.Forms.Panel();
            this.buttonMotionConfig = new System.Windows.Forms.Button();
            this.useConveyorSystem = new System.Windows.Forms.CheckBox();
            this.useTowerLamp = new System.Windows.Forms.CheckBox();
            this.useSoundBuzzer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelectGrabber
            // 
            this.buttonSelectGrabber.Location = new System.Drawing.Point(-1, -1);
            this.buttonSelectGrabber.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectGrabber.Name = "buttonSelectGrabber";
            this.buttonSelectGrabber.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectGrabber.TabIndex = 157;
            this.buttonSelectGrabber.Text = "Grabber";
            this.buttonSelectGrabber.UseVisualStyleBackColor = true;
            this.buttonSelectGrabber.Click += new System.EventHandler(this.buttonSelectGrabber_Click);
            // 
            // buttonSelectMotion
            // 
            this.buttonSelectMotion.Location = new System.Drawing.Point(-1, 50);
            this.buttonSelectMotion.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectMotion.Name = "buttonSelectMotion";
            this.buttonSelectMotion.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectMotion.TabIndex = 157;
            this.buttonSelectMotion.Text = "Motion";
            this.buttonSelectMotion.UseVisualStyleBackColor = true;
            this.buttonSelectMotion.Click += new System.EventHandler(this.buttonSelectMotion_Click);
            // 
            // buttonSelectLightCtrl
            // 
            this.buttonSelectLightCtrl.Location = new System.Drawing.Point(-1, 152);
            this.buttonSelectLightCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectLightCtrl.Name = "buttonSelectLightCtrl";
            this.buttonSelectLightCtrl.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectLightCtrl.TabIndex = 157;
            this.buttonSelectLightCtrl.Text = "Light";
            this.buttonSelectLightCtrl.UseVisualStyleBackColor = true;
            this.buttonSelectLightCtrl.Click += new System.EventHandler(this.buttonSelectLightCtrl_Click);
            // 
            // buttonSelectDigitalIo
            // 
            this.buttonSelectDigitalIo.Location = new System.Drawing.Point(-1, 101);
            this.buttonSelectDigitalIo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectDigitalIo.Name = "buttonSelectDigitalIo";
            this.buttonSelectDigitalIo.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectDigitalIo.TabIndex = 157;
            this.buttonSelectDigitalIo.Text = "DIO";
            this.buttonSelectDigitalIo.UseVisualStyleBackColor = true;
            this.buttonSelectDigitalIo.Click += new System.EventHandler(this.buttonSelectDigitalIo_Click);
            // 
            // buttonSelectDaq
            // 
            this.buttonSelectDaq.Location = new System.Drawing.Point(-1, 203);
            this.buttonSelectDaq.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectDaq.Name = "buttonSelectDaq";
            this.buttonSelectDaq.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectDaq.TabIndex = 157;
            this.buttonSelectDaq.Text = "DAQ";
            this.buttonSelectDaq.UseVisualStyleBackColor = true;
            this.buttonSelectDaq.Click += new System.EventHandler(this.buttonSelectDaq_Click);
            // 
            // editButton
            // 
            this.editButton.Enabled = false;
            this.editButton.Image = global::UniEye.Base.Properties.Resources.edit_32;
            this.editButton.Location = new System.Drawing.Point(221, -1);
            this.editButton.Margin = new System.Windows.Forms.Padding(4);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(79, 50);
            this.editButton.TabIndex = 159;
            this.editButton.Text = "Edit";
            this.editButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteButton.Location = new System.Drawing.Point(300, -1);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(92, 50);
            this.deleteButton.TabIndex = 160;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addButton.Location = new System.Drawing.Point(133, -1);
            this.addButton.Margin = new System.Windows.Forms.Padding(4);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(88, 50);
            this.addButton.TabIndex = 161;
            this.addButton.Text = "Add";
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Image = global::UniEye.Base.Properties.Resources.arrow_up;
            this.buttonMoveUp.Location = new System.Drawing.Point(417, -1);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 50);
            this.buttonMoveUp.TabIndex = 160;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Image = global::UniEye.Base.Properties.Resources.arrow_down;
            this.buttonMoveDown.Location = new System.Drawing.Point(494, -1);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(91, 50);
            this.buttonMoveDown.TabIndex = 160;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // useDoorSensor
            // 
            this.useDoorSensor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useDoorSensor.AutoSize = true;
            this.useDoorSensor.Location = new System.Drawing.Point(136, 423);
            this.useDoorSensor.Name = "useDoorSensor";
            this.useDoorSensor.Size = new System.Drawing.Size(144, 22);
            this.useDoorSensor.TabIndex = 166;
            this.useDoorSensor.Text = "Use Door Sensor";
            this.useDoorSensor.UseVisualStyleBackColor = true;
            // 
            // useModelBarcode
            // 
            this.useModelBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useModelBarcode.AutoSize = true;
            this.useModelBarcode.Location = new System.Drawing.Point(278, 423);
            this.useModelBarcode.Name = "useModelBarcode";
            this.useModelBarcode.Size = new System.Drawing.Size(159, 22);
            this.useModelBarcode.TabIndex = 166;
            this.useModelBarcode.Text = "Use Model Barcode";
            this.useModelBarcode.UseVisualStyleBackColor = true;
            // 
            // useFovNavigator
            // 
            this.useFovNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useFovNavigator.AutoSize = true;
            this.useFovNavigator.Location = new System.Drawing.Point(441, 423);
            this.useFovNavigator.Name = "useFovNavigator";
            this.useFovNavigator.Size = new System.Drawing.Size(150, 22);
            this.useFovNavigator.TabIndex = 166;
            this.useFovNavigator.Text = "Use Fov Navigator";
            this.useFovNavigator.UseVisualStyleBackColor = true;
            // 
            // useRobotStage
            // 
            this.useRobotStage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useRobotStage.AutoSize = true;
            this.useRobotStage.Location = new System.Drawing.Point(136, 453);
            this.useRobotStage.Name = "useRobotStage";
            this.useRobotStage.Size = new System.Drawing.Size(141, 22);
            this.useRobotStage.TabIndex = 166;
            this.useRobotStage.Text = "Use Robot Stage";
            this.useRobotStage.UseVisualStyleBackColor = true;
            // 
            // useConveyorMotor
            // 
            this.useConveyorMotor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useConveyorMotor.AutoSize = true;
            this.useConveyorMotor.Location = new System.Drawing.Point(278, 453);
            this.useConveyorMotor.Name = "useConveyorMotor";
            this.useConveyorMotor.Size = new System.Drawing.Size(166, 22);
            this.useConveyorMotor.TabIndex = 166;
            this.useConveyorMotor.Text = "Use Conveyor Motor";
            this.useConveyorMotor.UseVisualStyleBackColor = true;
            // 
            // columnNumPort
            // 
            this.columnNumPort.HeaderText = "Num Port";
            this.columnNumPort.Name = "columnNumPort";
            this.columnNumPort.ReadOnly = true;
            this.columnNumPort.Width = 150;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No";
            this.columnNo.Name = "columnNo";
            this.columnNo.ReadOnly = true;
            this.columnNo.Width = 50;
            // 
            // dataGridViewDeviceList
            // 
            this.dataGridViewDeviceList.AllowUserToAddRows = false;
            this.dataGridViewDeviceList.AllowUserToDeleteRows = false;
            this.dataGridViewDeviceList.AllowUserToResizeColumns = false;
            this.dataGridViewDeviceList.AllowUserToResizeRows = false;
            this.dataGridViewDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDeviceList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnName,
            this.columnType,
            this.columnNumPort});
            this.dataGridViewDeviceList.Location = new System.Drawing.Point(133, 50);
            this.dataGridViewDeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewDeviceList.Name = "dataGridViewDeviceList";
            this.dataGridViewDeviceList.ReadOnly = true;
            this.dataGridViewDeviceList.RowHeadersVisible = false;
            this.dataGridViewDeviceList.RowTemplate.Height = 23;
            this.dataGridViewDeviceList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDeviceList.Size = new System.Drawing.Size(493, 293);
            this.dataGridViewDeviceList.TabIndex = 158;
            this.dataGridViewDeviceList.SelectionChanged += new System.EventHandler(this.dataGridViewDeviceList_SelectionChanged);
            // 
            // panelOption
            // 
            this.panelOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOption.Location = new System.Drawing.Point(133, 350);
            this.panelOption.Name = "panelOption";
            this.panelOption.Size = new System.Drawing.Size(493, 63);
            this.panelOption.TabIndex = 168;
            // 
            // buttonMotionConfig
            // 
            this.buttonMotionConfig.Location = new System.Drawing.Point(-1, 466);
            this.buttonMotionConfig.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMotionConfig.Name = "buttonMotionConfig";
            this.buttonMotionConfig.Size = new System.Drawing.Size(131, 50);
            this.buttonMotionConfig.TabIndex = 157;
            this.buttonMotionConfig.Text = "Motion Config";
            this.buttonMotionConfig.UseVisualStyleBackColor = true;
            this.buttonMotionConfig.Click += new System.EventHandler(this.buttonMotionContfig_Click);
            // 
            // useConveyorSystem
            // 
            this.useConveyorSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useConveyorSystem.AutoSize = true;
            this.useConveyorSystem.Location = new System.Drawing.Point(441, 453);
            this.useConveyorSystem.Name = "useConveyorSystem";
            this.useConveyorSystem.Size = new System.Drawing.Size(176, 22);
            this.useConveyorSystem.TabIndex = 169;
            this.useConveyorSystem.Text = "Use Conveyor System";
            this.useConveyorSystem.UseVisualStyleBackColor = true;
            // 
            // useTowerLamp
            // 
            this.useTowerLamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useTowerLamp.AutoSize = true;
            this.useTowerLamp.Location = new System.Drawing.Point(136, 481);
            this.useTowerLamp.Name = "useTowerLamp";
            this.useTowerLamp.Size = new System.Drawing.Size(141, 22);
            this.useTowerLamp.TabIndex = 166;
            this.useTowerLamp.Text = "Use Tower Lamp";
            this.useTowerLamp.UseVisualStyleBackColor = true;
            // 
            // useSoundBuzzer
            // 
            this.useSoundBuzzer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.useSoundBuzzer.AutoSize = true;
            this.useSoundBuzzer.Location = new System.Drawing.Point(278, 481);
            this.useSoundBuzzer.Name = "useSoundBuzzer";
            this.useSoundBuzzer.Size = new System.Drawing.Size(152, 22);
            this.useSoundBuzzer.TabIndex = 166;
            this.useSoundBuzzer.Text = "Use Sound Buzzer";
            this.useSoundBuzzer.UseVisualStyleBackColor = true;
            // 
            // ConfigDevicePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useConveyorSystem);
            this.Controls.Add(this.panelOption);
            this.Controls.Add(this.useFovNavigator);
            this.Controls.Add(this.useModelBarcode);
            this.Controls.Add(this.useSoundBuzzer);
            this.Controls.Add(this.useConveyorMotor);
            this.Controls.Add(this.useTowerLamp);
            this.Controls.Add(this.useRobotStage);
            this.Controls.Add(this.useDoorSensor);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.dataGridViewDeviceList);
            this.Controls.Add(this.buttonMotionConfig);
            this.Controls.Add(this.buttonSelectDaq);
            this.Controls.Add(this.buttonSelectLightCtrl);
            this.Controls.Add(this.buttonSelectDigitalIo);
            this.Controls.Add(this.buttonSelectMotion);
            this.Controls.Add(this.buttonSelectGrabber);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfigDevicePanel";
            this.Size = new System.Drawing.Size(630, 520);
            this.Load += new System.EventHandler(this.ConfigDevicePanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectGrabber;
        private System.Windows.Forms.Button buttonSelectMotion;
        private System.Windows.Forms.Button buttonSelectLightCtrl;
        private System.Windows.Forms.Button buttonSelectDigitalIo;
        private System.Windows.Forms.Button buttonSelectDaq;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.CheckBox useDoorSensor;
        private System.Windows.Forms.CheckBox useModelBarcode;
        private System.Windows.Forms.CheckBox useFovNavigator;
        private System.Windows.Forms.CheckBox useRobotStage;
        private System.Windows.Forms.CheckBox useConveyorMotor;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNumPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridView dataGridViewDeviceList;
        private System.Windows.Forms.Panel panelOption;
        private System.Windows.Forms.Button buttonMotionConfig;
        private System.Windows.Forms.CheckBox useConveyorSystem;
        private System.Windows.Forms.CheckBox useTowerLamp;
        private System.Windows.Forms.CheckBox useSoundBuzzer;
    }
}
