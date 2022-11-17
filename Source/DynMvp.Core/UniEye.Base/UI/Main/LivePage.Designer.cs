using System;

namespace UniEye.Base.UI.Main
{
    partial class LivePage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LivePage));
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
            this.cameraViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.viewContainer = new System.Windows.Forms.Panel();
            this.measureMode = new System.Windows.Forms.CheckBox();
            this.clearMeasure = new System.Windows.Forms.Button();
            this.txtExposure = new System.Windows.Forms.TextBox();
            this.labelMs = new System.Windows.Forms.Label();
            this.labelExposure = new System.Windows.Forms.Label();
            this.labelStep = new System.Windows.Forms.Label();
            this.comboStep = new System.Windows.Forms.ComboBox();
            this.buttonLightSttting = new System.Windows.Forms.Button();
            this.labelCamera = new System.Windows.Forms.Label();
            this.lstCamera = new System.Windows.Forms.ListBox();
            this.viewContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStart.Appearance = appearance1;
            this.buttonStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.buttonStart.ImageSize = new System.Drawing.Size(70, 90);
            this.buttonStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonStart.Location = new System.Drawing.Point(3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(119, 124);
            this.buttonStart.TabIndex = 11;
            this.buttonStart.Tag = "Start";
            this.buttonStart.UseAppStyling = false;
            this.buttonStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cameraViewPanel
            // 
            this.cameraViewPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cameraViewPanel.ColumnCount = 2;
            this.cameraViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraViewPanel.Location = new System.Drawing.Point(0, 0);
            this.cameraViewPanel.Name = "cameraViewPanel";
            this.cameraViewPanel.RowCount = 2;
            this.cameraViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.Size = new System.Drawing.Size(581, 514);
            this.cameraViewPanel.TabIndex = 17;
            // 
            // viewContainer
            // 
            this.viewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewContainer.Controls.Add(this.cameraViewPanel);
            this.viewContainer.Location = new System.Drawing.Point(127, 3);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(581, 514);
            this.viewContainer.TabIndex = 20;
            // 
            // measureMode
            // 
            this.measureMode.AutoSize = true;
            this.measureMode.Location = new System.Drawing.Point(3, 134);
            this.measureMode.Name = "measureMode";
            this.measureMode.Size = new System.Drawing.Size(110, 16);
            this.measureMode.TabIndex = 21;
            this.measureMode.Text = "Measure Mode";
            this.measureMode.UseVisualStyleBackColor = true;
            this.measureMode.CheckedChanged += new System.EventHandler(this.measureMode_CheckedChanged);
            // 
            // clearMeasure
            // 
            this.clearMeasure.Location = new System.Drawing.Point(3, 156);
            this.clearMeasure.Name = "clearMeasure";
            this.clearMeasure.Size = new System.Drawing.Size(119, 31);
            this.clearMeasure.TabIndex = 22;
            this.clearMeasure.Text = "Clear Measure";
            this.clearMeasure.UseVisualStyleBackColor = true;
            this.clearMeasure.Click += new System.EventHandler(this.clearMeasure_Click);
            // 
            // txtExposure
            // 
            this.txtExposure.Location = new System.Drawing.Point(5, 451);
            this.txtExposure.Name = "txtExposure";
            this.txtExposure.Size = new System.Drawing.Size(86, 21);
            this.txtExposure.TabIndex = 25;
            this.txtExposure.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExposure_KeyDown);
            // 
            // labelMs
            // 
            this.labelMs.AutoSize = true;
            this.labelMs.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMs.Location = new System.Drawing.Point(97, 461);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(21, 11);
            this.labelMs.TabIndex = 26;
            this.labelMs.Text = "ms";
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.Location = new System.Drawing.Point(3, 436);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(92, 12);
            this.labelExposure.TabIndex = 27;
            this.labelExposure.Text = "Exposure Time";
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(3, 316);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(30, 12);
            this.labelStep.TabIndex = 30;
            this.labelStep.Text = "Step";
            // 
            // comboStep
            // 
            this.comboStep.FormattingEnabled = true;
            this.comboStep.Location = new System.Drawing.Point(5, 332);
            this.comboStep.Name = "comboStep";
            this.comboStep.Size = new System.Drawing.Size(117, 20);
            this.comboStep.TabIndex = 31;
            this.comboStep.SelectedIndexChanged += new System.EventHandler(this.comboStep_SelectedIndexChanged);
            // 
            // buttonLightSttting
            // 
            this.buttonLightSttting.Location = new System.Drawing.Point(3, 363);
            this.buttonLightSttting.Name = "buttonLightSttting";
            this.buttonLightSttting.Size = new System.Drawing.Size(119, 31);
            this.buttonLightSttting.TabIndex = 32;
            this.buttonLightSttting.Text = "Light Setting";
            this.buttonLightSttting.UseVisualStyleBackColor = true;
            this.buttonLightSttting.Click += new System.EventHandler(this.buttonLightSttting_Click);
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Location = new System.Drawing.Point(3, 190);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(50, 12);
            this.labelCamera.TabIndex = 30;
            this.labelCamera.Text = "Camera";
            // 
            // lstCamera
            // 
            this.lstCamera.FormattingEnabled = true;
            this.lstCamera.ItemHeight = 12;
            this.lstCamera.Location = new System.Drawing.Point(5, 205);
            this.lstCamera.Name = "lstCamera";
            this.lstCamera.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstCamera.Size = new System.Drawing.Size(117, 88);
            this.lstCamera.TabIndex = 33;
            // 
            // LivePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstCamera);
            this.Controls.Add(this.buttonLightSttting);
            this.Controls.Add(this.comboStep);
            this.Controls.Add(this.labelCamera);
            this.Controls.Add(this.labelStep);
            this.Controls.Add(this.labelExposure);
            this.Controls.Add(this.labelMs);
            this.Controls.Add(this.txtExposure);
            this.Controls.Add(this.clearMeasure);
            this.Controls.Add(this.measureMode);
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.buttonStart);
            this.DoubleBuffered = true;
            this.Name = "LivePage";
            this.Size = new System.Drawing.Size(712, 521);
            this.viewContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.Misc.UltraButton buttonStart;
        private System.Windows.Forms.TableLayoutPanel cameraViewPanel;
        private System.Windows.Forms.Panel viewContainer;
        private System.Windows.Forms.CheckBox measureMode;
        private System.Windows.Forms.Button clearMeasure;
        private System.Windows.Forms.TextBox txtExposure;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.ComboBox comboStep;
        private System.Windows.Forms.Button buttonLightSttting;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.ListBox lstCamera;
    }
}
