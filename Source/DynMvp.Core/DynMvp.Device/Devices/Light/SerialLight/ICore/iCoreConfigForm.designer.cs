namespace DynMvp.Devices.Light.SerialLigth.iCore
{
    partial class iCoreConfigForm
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
            this.labelSlaveId = new System.Windows.Forms.Label();
            this.slaveId = new System.Windows.Forms.NumericUpDown();
            this.labelOpMode = new System.Windows.Forms.Label();
            this.labelMaxVoltage = new System.Windows.Forms.Label();
            this.labelTimeDuration = new System.Windows.Forms.Label();
            this.labelLowPassFilter = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.timeDuration = new System.Windows.Forms.NumericUpDown();
            this.labelMaxVoltageUnit = new System.Windows.Forms.Label();
            this.labelTimeDurationUnit = new System.Windows.Forms.Label();
            this.opMode = new System.Windows.Forms.ComboBox();
            this.maxVoltage = new System.Windows.Forms.NumericUpDown();
            this.lowPassFilter = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.slaveId)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxVoltage)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSlaveId
            // 
            this.labelSlaveId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSlaveId.Location = new System.Drawing.Point(3, 0);
            this.labelSlaveId.Name = "labelSlaveId";
            this.labelSlaveId.Size = new System.Drawing.Size(137, 27);
            this.labelSlaveId.TabIndex = 0;
            this.labelSlaveId.Text = "Salve ID";
            this.labelSlaveId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // slaveId
            // 
            this.slaveId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slaveId.Location = new System.Drawing.Point(146, 3);
            this.slaveId.Name = "slaveId";
            this.slaveId.Size = new System.Drawing.Size(137, 21);
            this.slaveId.TabIndex = 1;
            // 
            // labelOpMode
            // 
            this.labelOpMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOpMode.Location = new System.Drawing.Point(3, 27);
            this.labelOpMode.Name = "labelOpMode";
            this.labelOpMode.Size = new System.Drawing.Size(137, 27);
            this.labelOpMode.TabIndex = 0;
            this.labelOpMode.Text = "Operation Mode";
            this.labelOpMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaxVoltage
            // 
            this.labelMaxVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxVoltage.Location = new System.Drawing.Point(3, 54);
            this.labelMaxVoltage.Name = "labelMaxVoltage";
            this.labelMaxVoltage.Size = new System.Drawing.Size(137, 27);
            this.labelMaxVoltage.TabIndex = 0;
            this.labelMaxVoltage.Text = "Max Voltage";
            this.labelMaxVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTimeDuration
            // 
            this.labelTimeDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimeDuration.Location = new System.Drawing.Point(3, 81);
            this.labelTimeDuration.Name = "labelTimeDuration";
            this.labelTimeDuration.Size = new System.Drawing.Size(137, 27);
            this.labelTimeDuration.TabIndex = 0;
            this.labelTimeDuration.Text = "Time Duration";
            this.labelTimeDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLowPassFilter
            // 
            this.labelLowPassFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLowPassFilter.Location = new System.Drawing.Point(3, 108);
            this.labelLowPassFilter.Name = "labelLowPassFilter";
            this.labelLowPassFilter.Size = new System.Drawing.Size(137, 27);
            this.labelLowPassFilter.TabIndex = 0;
            this.labelLowPassFilter.Text = "Low Pass Filter";
            this.labelLowPassFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Controls.Add(this.labelSlaveId, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLowPassFilter, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.slaveId, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTimeDuration, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelOpMode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelMaxVoltage, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.timeDuration, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelMaxVoltageUnit, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTimeDurationUnit, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.opMode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.maxVoltage, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lowPassFilter, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 185);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // timeDuration
            // 
            this.timeDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeDuration.Location = new System.Drawing.Point(146, 84);
            this.timeDuration.Name = "timeDuration";
            this.timeDuration.Size = new System.Drawing.Size(137, 21);
            this.timeDuration.TabIndex = 1;
            // 
            // labelMaxVoltageUnit
            // 
            this.labelMaxVoltageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxVoltageUnit.Location = new System.Drawing.Point(289, 54);
            this.labelMaxVoltageUnit.Name = "labelMaxVoltageUnit";
            this.labelMaxVoltageUnit.Size = new System.Drawing.Size(35, 27);
            this.labelMaxVoltageUnit.TabIndex = 0;
            this.labelMaxVoltageUnit.Text = "[V]";
            this.labelMaxVoltageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTimeDurationUnit
            // 
            this.labelTimeDurationUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimeDurationUnit.Location = new System.Drawing.Point(289, 81);
            this.labelTimeDurationUnit.Name = "labelTimeDurationUnit";
            this.labelTimeDurationUnit.Size = new System.Drawing.Size(35, 27);
            this.labelTimeDurationUnit.TabIndex = 0;
            this.labelTimeDurationUnit.Text = "[us]";
            this.labelTimeDurationUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opMode
            // 
            this.opMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opMode.FormattingEnabled = true;
            this.opMode.Location = new System.Drawing.Point(146, 30);
            this.opMode.Name = "opMode";
            this.opMode.Size = new System.Drawing.Size(137, 20);
            this.opMode.TabIndex = 2;
            // 
            // maxVoltage
            // 
            this.maxVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maxVoltage.Location = new System.Drawing.Point(146, 57);
            this.maxVoltage.Name = "maxVoltage";
            this.maxVoltage.Size = new System.Drawing.Size(137, 21);
            this.maxVoltage.TabIndex = 1;
            // 
            // lowPassFilter
            // 
            this.lowPassFilter.AutoSize = true;
            this.lowPassFilter.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lowPassFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lowPassFilter.Location = new System.Drawing.Point(146, 111);
            this.lowPassFilter.Name = "lowPassFilter";
            this.lowPassFilter.Size = new System.Drawing.Size(137, 21);
            this.lowPassFilter.TabIndex = 3;
            this.lowPassFilter.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(3, 138);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(137, 44);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(146, 138);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(137, 44);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // iCoreConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(327, 239);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "iCoreConfigForm";
            this.Text = "iCoreConfigForm";
            this.Load += new System.EventHandler(this.iCoreConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.slaveId)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxVoltage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSlaveId;
        private System.Windows.Forms.NumericUpDown slaveId;
        private System.Windows.Forms.Label labelOpMode;
        private System.Windows.Forms.Label labelMaxVoltage;
        private System.Windows.Forms.Label labelTimeDuration;
        private System.Windows.Forms.Label labelLowPassFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown timeDuration;
        private System.Windows.Forms.Label labelMaxVoltageUnit;
        private System.Windows.Forms.Label labelTimeDurationUnit;
        private System.Windows.Forms.ComboBox opMode;
        private System.Windows.Forms.NumericUpDown maxVoltage;
        private System.Windows.Forms.CheckBox lowPassFilter;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
    }
}