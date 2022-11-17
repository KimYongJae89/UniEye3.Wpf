namespace DynMvp.Devices.Devices.UI
{
    partial class TowerLampOptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TowerLampOptionForm));
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnBlink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOff
            // 
            this.btnOff.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOff.Image = global::DynMvp.Devices.Properties.Resources.Lamp_Off;
            this.btnOff.Location = new System.Drawing.Point(15, 12);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(110, 44);
            this.btnOff.TabIndex = 1;
            this.btnOff.Text = "OFF";
            this.btnOff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnLampValue_Click);
            // 
            // btnOn
            // 
            this.btnOn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOn.Image = ((System.Drawing.Image)(resources.GetObject("btnOn.Image")));
            this.btnOn.Location = new System.Drawing.Point(131, 12);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(110, 44);
            this.btnOn.TabIndex = 1;
            this.btnOn.Text = "ON";
            this.btnOn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnLampValue_Click);
            // 
            // btnBlink
            // 
            this.btnBlink.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnBlink.Image = global::DynMvp.Devices.Properties.Resources.Lamp_Blink;
            this.btnBlink.Location = new System.Drawing.Point(247, 12);
            this.btnBlink.Name = "btnBlink";
            this.btnBlink.Size = new System.Drawing.Size(110, 44);
            this.btnBlink.TabIndex = 1;
            this.btnBlink.Text = "BLINK";
            this.btnBlink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBlink.UseVisualStyleBackColor = true;
            this.btnBlink.Click += new System.EventHandler(this.btnLampValue_Click);
            // 
            // TowerLampOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 70);
            this.Controls.Add(this.btnBlink);
            this.Controls.Add(this.btnOn);
            this.Controls.Add(this.btnOff);
            this.Name = "TowerLampOptionForm";
            this.Text = "Lamp Option";
            this.Load += new System.EventHandler(this.TowerLampOptionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Button btnBlink;
    }
}