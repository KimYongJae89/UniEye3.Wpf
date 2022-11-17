namespace DynMvp.Devices.UI
{
    partial class ConveyorSystemControlForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelSidePusherOn = new System.Windows.Forms.Label();
            this.labelStopperUp = new System.Windows.Forms.Label();
            this.labelClampUp = new System.Windows.Forms.Label();
            this.labelConveyorFlow = new System.Windows.Forms.Label();
            this.labelEject = new System.Windows.Forms.Label();
            this.labelReady = new System.Windows.Forms.Label();
            this.labelSpeedDown = new System.Windows.Forms.Label();
            this.labelEntry = new System.Windows.Forms.Label();
            this.buttonInit = new System.Windows.Forms.Button();
            this.buttonReceive = new System.Windows.Forms.Button();
            this.buttonEject = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.checkBoxClamp = new System.Windows.Forms.CheckBox();
            this.checkBoxStopper = new System.Windows.Forms.CheckBox();
            this.checkBoxSidePusher = new System.Windows.Forms.CheckBox();
            this.checkBoxRun = new System.Windows.Forms.CheckBox();
            this.checkBoxSpeedDown = new System.Windows.Forms.CheckBox();
            this.checkBoxDirection = new System.Windows.Forms.CheckBox();
            this.labelNextAvailable = new System.Windows.Forms.Label();
            this.timerSensorUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonUpdateStart = new System.Windows.Forms.Button();
            this.labelConveyorState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(66, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(66, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(378, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // labelSidePusherOn
            // 
            this.labelSidePusherOn.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelSidePusherOn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelSidePusherOn.Location = new System.Drawing.Point(247, 158);
            this.labelSidePusherOn.Name = "labelSidePusherOn";
            this.labelSidePusherOn.Size = new System.Drawing.Size(105, 42);
            this.labelSidePusherOn.TabIndex = 15;
            this.labelSidePusherOn.Text = "Side Pusher On";
            this.labelSidePusherOn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelStopperUp
            // 
            this.labelStopperUp.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelStopperUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelStopperUp.Location = new System.Drawing.Point(358, 158);
            this.labelStopperUp.Name = "labelStopperUp";
            this.labelStopperUp.Size = new System.Drawing.Size(68, 42);
            this.labelStopperUp.TabIndex = 14;
            this.labelStopperUp.Text = "Stopper Up";
            this.labelStopperUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelClampUp
            // 
            this.labelClampUp.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelClampUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelClampUp.Location = new System.Drawing.Point(178, 158);
            this.labelClampUp.Name = "labelClampUp";
            this.labelClampUp.Size = new System.Drawing.Size(63, 42);
            this.labelClampUp.TabIndex = 13;
            this.labelClampUp.Text = "Clamp Up";
            this.labelClampUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelConveyorFlow
            // 
            this.labelConveyorFlow.Image = global::DynMvp.Devices.Properties.Resources.Forbidden32;
            this.labelConveyorFlow.Location = new System.Drawing.Point(84, 158);
            this.labelConveyorFlow.Name = "labelConveyorFlow";
            this.labelConveyorFlow.Size = new System.Drawing.Size(44, 42);
            this.labelConveyorFlow.TabIndex = 12;
            // 
            // labelEject
            // 
            this.labelEject.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelEject.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelEject.Location = new System.Drawing.Point(405, 90);
            this.labelEject.Name = "labelEject";
            this.labelEject.Size = new System.Drawing.Size(39, 42);
            this.labelEject.TabIndex = 8;
            this.labelEject.Text = "Eject";
            this.labelEject.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelReady
            // 
            this.labelReady.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelReady.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelReady.Location = new System.Drawing.Point(309, 90);
            this.labelReady.Name = "labelReady";
            this.labelReady.Size = new System.Drawing.Size(43, 42);
            this.labelReady.TabIndex = 7;
            this.labelReady.Text = "Ready";
            this.labelReady.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelSpeedDown
            // 
            this.labelSpeedDown.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelSpeedDown.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelSpeedDown.Location = new System.Drawing.Point(223, 90);
            this.labelSpeedDown.Name = "labelSpeedDown";
            this.labelSpeedDown.Size = new System.Drawing.Size(80, 42);
            this.labelSpeedDown.TabIndex = 6;
            this.labelSpeedDown.Text = "Speed Down";
            this.labelSpeedDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelEntry
            // 
            this.labelEntry.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelEntry.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelEntry.Location = new System.Drawing.Point(66, 90);
            this.labelEntry.Name = "labelEntry";
            this.labelEntry.Size = new System.Drawing.Size(42, 42);
            this.labelEntry.TabIndex = 3;
            this.labelEntry.Text = "Entry";
            this.labelEntry.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(86, 325);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(88, 30);
            this.buttonInit.TabIndex = 16;
            this.buttonInit.Text = "Initialize";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // buttonReceive
            // 
            this.buttonReceive.Location = new System.Drawing.Point(180, 325);
            this.buttonReceive.Name = "buttonReceive";
            this.buttonReceive.Size = new System.Drawing.Size(88, 30);
            this.buttonReceive.TabIndex = 17;
            this.buttonReceive.Text = "Receive";
            this.buttonReceive.UseVisualStyleBackColor = true;
            this.buttonReceive.Click += new System.EventHandler(this.buttonReceive_Click);
            // 
            // buttonEject
            // 
            this.buttonEject.Location = new System.Drawing.Point(274, 325);
            this.buttonEject.Name = "buttonEject";
            this.buttonEject.Size = new System.Drawing.Size(88, 30);
            this.buttonEject.TabIndex = 18;
            this.buttonEject.Text = "Eject";
            this.buttonEject.UseVisualStyleBackColor = true;
            this.buttonEject.Click += new System.EventHandler(this.buttonEject_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(180, 361);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(88, 30);
            this.buttonPause.TabIndex = 19;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(274, 361);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(88, 30);
            this.buttonStop.TabIndex = 20;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(86, 361);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(88, 30);
            this.buttonStart.TabIndex = 21;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // checkBoxClamp
            // 
            this.checkBoxClamp.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxClamp.Location = new System.Drawing.Point(86, 234);
            this.checkBoxClamp.Name = "checkBoxClamp";
            this.checkBoxClamp.Size = new System.Drawing.Size(88, 30);
            this.checkBoxClamp.TabIndex = 22;
            this.checkBoxClamp.Text = "Clamp";
            this.checkBoxClamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxClamp.UseVisualStyleBackColor = true;
            // 
            // checkBoxStopper
            // 
            this.checkBoxStopper.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxStopper.Location = new System.Drawing.Point(180, 234);
            this.checkBoxStopper.Name = "checkBoxStopper";
            this.checkBoxStopper.Size = new System.Drawing.Size(88, 30);
            this.checkBoxStopper.TabIndex = 23;
            this.checkBoxStopper.Text = "Stopper";
            this.checkBoxStopper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxStopper.UseVisualStyleBackColor = true;
            // 
            // checkBoxSidePusher
            // 
            this.checkBoxSidePusher.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSidePusher.Location = new System.Drawing.Point(274, 234);
            this.checkBoxSidePusher.Name = "checkBoxSidePusher";
            this.checkBoxSidePusher.Size = new System.Drawing.Size(88, 30);
            this.checkBoxSidePusher.TabIndex = 24;
            this.checkBoxSidePusher.Text = "Side Pusher";
            this.checkBoxSidePusher.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxSidePusher.UseVisualStyleBackColor = true;
            // 
            // checkBoxRun
            // 
            this.checkBoxRun.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRun.Location = new System.Drawing.Point(86, 270);
            this.checkBoxRun.Name = "checkBoxRun";
            this.checkBoxRun.Size = new System.Drawing.Size(88, 30);
            this.checkBoxRun.TabIndex = 25;
            this.checkBoxRun.Text = "Run";
            this.checkBoxRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRun.UseVisualStyleBackColor = true;
            // 
            // checkBoxSpeedDown
            // 
            this.checkBoxSpeedDown.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSpeedDown.Location = new System.Drawing.Point(274, 270);
            this.checkBoxSpeedDown.Name = "checkBoxSpeedDown";
            this.checkBoxSpeedDown.Size = new System.Drawing.Size(88, 30);
            this.checkBoxSpeedDown.TabIndex = 26;
            this.checkBoxSpeedDown.Text = "Speed Down";
            this.checkBoxSpeedDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxSpeedDown.UseVisualStyleBackColor = true;
            // 
            // checkBoxDirection
            // 
            this.checkBoxDirection.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDirection.Location = new System.Drawing.Point(180, 270);
            this.checkBoxDirection.Name = "checkBoxDirection";
            this.checkBoxDirection.Size = new System.Drawing.Size(88, 30);
            this.checkBoxDirection.TabIndex = 27;
            this.checkBoxDirection.Text = "Forward";
            this.checkBoxDirection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDirection.UseVisualStyleBackColor = true;
            // 
            // labelNextAvailable
            // 
            this.labelNextAvailable.Image = global::DynMvp.Devices.Properties.Resources.led_off;
            this.labelNextAvailable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelNextAvailable.Location = new System.Drawing.Point(451, 158);
            this.labelNextAvailable.Name = "labelNextAvailable";
            this.labelNextAvailable.Size = new System.Drawing.Size(73, 55);
            this.labelNextAvailable.TabIndex = 28;
            this.labelNextAvailable.Text = "Next Available";
            this.labelNextAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // timerSensorUpdate
            // 
            this.timerSensorUpdate.Tick += new System.EventHandler(this.timerSensorUpdate_Tick);
            // 
            // buttonUpdateStart
            // 
            this.buttonUpdateStart.Location = new System.Drawing.Point(86, 397);
            this.buttonUpdateStart.Name = "buttonUpdateStart";
            this.buttonUpdateStart.Size = new System.Drawing.Size(88, 30);
            this.buttonUpdateStart.TabIndex = 29;
            this.buttonUpdateStart.Text = "Update Start";
            this.buttonUpdateStart.UseVisualStyleBackColor = true;
            this.buttonUpdateStart.Click += new System.EventHandler(this.buttonUpdateStart_Click);
            // 
            // labelConveyorState
            // 
            this.labelConveyorState.AutoSize = true;
            this.labelConveyorState.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelConveyorState.Location = new System.Drawing.Point(221, 30);
            this.labelConveyorState.Name = "labelConveyorState";
            this.labelConveyorState.Size = new System.Drawing.Size(0, 24);
            this.labelConveyorState.TabIndex = 30;
            // 
            // ConveyorSystemControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 509);
            this.Controls.Add(this.labelConveyorState);
            this.Controls.Add(this.buttonUpdateStart);
            this.Controls.Add(this.labelNextAvailable);
            this.Controls.Add(this.checkBoxDirection);
            this.Controls.Add(this.checkBoxSpeedDown);
            this.Controls.Add(this.checkBoxRun);
            this.Controls.Add(this.checkBoxSidePusher);
            this.Controls.Add(this.checkBoxStopper);
            this.Controls.Add(this.checkBoxClamp);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonEject);
            this.Controls.Add(this.buttonReceive);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.labelSidePusherOn);
            this.Controls.Add(this.labelStopperUp);
            this.Controls.Add(this.labelClampUp);
            this.Controls.Add(this.labelConveyorFlow);
            this.Controls.Add(this.labelEject);
            this.Controls.Add(this.labelReady);
            this.Controls.Add(this.labelSpeedDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelEntry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ConveyorSystemControlForm";
            this.Text = "ConveyorSystemControlFrom";
            this.Load += new System.EventHandler(this.ConveyorSystemControlForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEntry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSpeedDown;
        private System.Windows.Forms.Label labelReady;
        private System.Windows.Forms.Label labelEject;
        private System.Windows.Forms.Label labelConveyorFlow;
        private System.Windows.Forms.Label labelClampUp;
        private System.Windows.Forms.Label labelStopperUp;
        private System.Windows.Forms.Label labelSidePusherOn;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Button buttonReceive;
        private System.Windows.Forms.Button buttonEject;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.CheckBox checkBoxClamp;
        private System.Windows.Forms.CheckBox checkBoxStopper;
        private System.Windows.Forms.CheckBox checkBoxSidePusher;
        private System.Windows.Forms.CheckBox checkBoxRun;
        private System.Windows.Forms.CheckBox checkBoxSpeedDown;
        private System.Windows.Forms.CheckBox checkBoxDirection;
        private System.Windows.Forms.Label labelNextAvailable;
        private System.Windows.Forms.Timer timerSensorUpdate;
        private System.Windows.Forms.Button buttonUpdateStart;
        private System.Windows.Forms.Label labelConveyorState;
    }
}