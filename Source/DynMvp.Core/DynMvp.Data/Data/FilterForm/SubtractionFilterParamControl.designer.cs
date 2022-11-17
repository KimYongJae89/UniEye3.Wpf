namespace DynMvp.Data.FilterForm
{
    partial class SubtractionFilterParamControl
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
            this.labelNegativeValueHandle = new System.Windows.Forms.Label();
            this.subtractionType = new System.Windows.Forms.ComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBoxInvert = new System.Windows.Forms.CheckBox();
            this.buttonTrain = new System.Windows.Forms.Button();
            this.pictureBoxTrain = new System.Windows.Forms.PictureBox();
            this.buttonClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrain)).BeginInit();
            this.SuspendLayout();
            // 
            // labelNegativeValueHandle
            // 
            this.labelNegativeValueHandle.AutoSize = true;
            this.labelNegativeValueHandle.Location = new System.Drawing.Point(5, 6);
            this.labelNegativeValueHandle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNegativeValueHandle.Name = "labelNegativeValueHandle";
            this.labelNegativeValueHandle.Size = new System.Drawing.Size(116, 20);
            this.labelNegativeValueHandle.TabIndex = 0;
            this.labelNegativeValueHandle.Text = "Negative Value";
            // 
            // subtractionType
            // 
            this.subtractionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subtractionType.FormattingEnabled = true;
            this.subtractionType.Location = new System.Drawing.Point(142, 3);
            this.subtractionType.Name = "subtractionType";
            this.subtractionType.Size = new System.Drawing.Size(123, 28);
            this.subtractionType.TabIndex = 18;
            this.subtractionType.SelectedIndexChanged += new System.EventHandler(this.subtractionType_SelectedIndexChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // checkBoxInvert
            // 
            this.checkBoxInvert.AutoSize = true;
            this.checkBoxInvert.Location = new System.Drawing.Point(9, 37);
            this.checkBoxInvert.Name = "checkBoxInvert";
            this.checkBoxInvert.Size = new System.Drawing.Size(68, 24);
            this.checkBoxInvert.TabIndex = 19;
            this.checkBoxInvert.Text = "Invert";
            this.checkBoxInvert.UseVisualStyleBackColor = true;
            this.checkBoxInvert.CheckedChanged += new System.EventHandler(this.checkBoxInvert_CheckedChanged);
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(9, 65);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(68, 27);
            this.buttonTrain.TabIndex = 20;
            this.buttonTrain.Text = "Train";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
            // 
            // pictureBoxTrain
            // 
            this.pictureBoxTrain.Location = new System.Drawing.Point(83, 37);
            this.pictureBoxTrain.Name = "pictureBoxTrain";
            this.pictureBoxTrain.Size = new System.Drawing.Size(182, 88);
            this.pictureBoxTrain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTrain.TabIndex = 21;
            this.pictureBoxTrain.TabStop = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(9, 98);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(68, 27);
            this.buttonClear.TabIndex = 22;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // SubtractionFilterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.pictureBoxTrain);
            this.Controls.Add(this.buttonTrain);
            this.Controls.Add(this.checkBoxInvert);
            this.Controls.Add(this.subtractionType);
            this.Controls.Add(this.labelNegativeValueHandle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SubtractionFilterParamControl";
            this.Size = new System.Drawing.Size(268, 128);
            this.VisibleChanged += new System.EventHandler(this.SubtractionFilterParamControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNegativeValueHandle;
        private System.Windows.Forms.ComboBox subtractionType;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox checkBoxInvert;
        private System.Windows.Forms.PictureBox pictureBoxTrain;
        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.Button buttonClear;
    }
}