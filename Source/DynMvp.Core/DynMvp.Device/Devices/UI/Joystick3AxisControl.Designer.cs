namespace DynMvp.Devices.UI
{
    public partial class Joystick3AxisControl
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
            this.panelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonFoward = new System.Windows.Forms.Button();
            this.panelOption = new System.Windows.Forms.Panel();
            this.checkStepMove = new System.Windows.Forms.CheckBox();
            this.moveStep = new System.Windows.Forms.ComboBox();
            this.panelButtons.SuspendLayout();
            this.panelOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.ColumnCount = 4;
            this.panelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelButtons.Controls.Add(this.buttonDown, 1, 2);
            this.panelButtons.Controls.Add(this.buttonLeft, 0, 1);
            this.panelButtons.Controls.Add(this.buttonUp, 1, 0);
            this.panelButtons.Controls.Add(this.buttonRight, 2, 1);
            this.panelButtons.Controls.Add(this.buttonBack, 3, 0);
            this.panelButtons.Controls.Add(this.buttonFoward, 3, 2);
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.RowCount = 3;
            this.panelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.panelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.panelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.panelButtons.Size = new System.Drawing.Size(218, 146);
            this.panelButtons.TabIndex = 0;
            // 
            // buttonDown
            // 
            this.buttonDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDown.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonDown.FlatAppearance.BorderSize = 0;
            this.buttonDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDown.Image = global::DynMvp.Devices.Properties.Resources.RotDownArrows;
            this.buttonDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDown.Location = new System.Drawing.Point(57, 100);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(48, 43);
            this.buttonDown.TabIndex = 0;
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonDown_MouseDown);
            this.buttonDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLeft.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonLeft.FlatAppearance.BorderSize = 0;
            this.buttonLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.Image = global::DynMvp.Devices.Properties.Resources.RotLeftArrows;
            this.buttonLeft.Location = new System.Drawing.Point(3, 51);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(48, 43);
            this.buttonLeft.TabIndex = 1;
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonLeft_MouseDown);
            this.buttonLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // buttonUp
            // 
            this.buttonUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUp.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonUp.FlatAppearance.BorderSize = 0;
            this.buttonUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Image = global::DynMvp.Devices.Properties.Resources.RotUpArrows;
            this.buttonUp.Location = new System.Drawing.Point(57, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(48, 42);
            this.buttonUp.TabIndex = 2;
            this.buttonUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonUp_MouseDown);
            this.buttonUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // buttonRight
            // 
            this.buttonRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRight.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonRight.FlatAppearance.BorderSize = 0;
            this.buttonRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.Image = global::DynMvp.Devices.Properties.Resources.RotRightArrows;
            this.buttonRight.Location = new System.Drawing.Point(111, 51);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(48, 43);
            this.buttonRight.TabIndex = 3;
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonRight_MouseDown);
            this.buttonRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // buttonBack
            // 
            this.buttonBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonBack.FlatAppearance.BorderSize = 0;
            this.buttonBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Image = global::DynMvp.Devices.Properties.Resources.nonUpArrows;
            this.buttonBack.Location = new System.Drawing.Point(165, 3);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(50, 42);
            this.buttonBack.TabIndex = 6;
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonBack_MouseDown);
            this.buttonBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // buttonFoward
            // 
            this.buttonFoward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFoward.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonFoward.FlatAppearance.BorderSize = 0;
            this.buttonFoward.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonFoward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFoward.Image = global::DynMvp.Devices.Properties.Resources.nonDownArrows;
            this.buttonFoward.Location = new System.Drawing.Point(165, 100);
            this.buttonFoward.Name = "buttonFoward";
            this.buttonFoward.Size = new System.Drawing.Size(50, 43);
            this.buttonFoward.TabIndex = 5;
            this.buttonFoward.UseVisualStyleBackColor = true;
            this.buttonFoward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonFoward_MouseDown);
            this.buttonFoward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseUp);
            // 
            // panelOption
            // 
            this.panelOption.Controls.Add(this.checkStepMove);
            this.panelOption.Controls.Add(this.moveStep);
            this.panelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOption.Location = new System.Drawing.Point(0, 147);
            this.panelOption.Name = "panelOption";
            this.panelOption.Size = new System.Drawing.Size(218, 31);
            this.panelOption.TabIndex = 1;
            // 
            // checkStepMove
            // 
            this.checkStepMove.AutoSize = true;
            this.checkStepMove.Location = new System.Drawing.Point(3, 7);
            this.checkStepMove.Name = "checkStepMove";
            this.checkStepMove.Size = new System.Drawing.Size(84, 16);
            this.checkStepMove.TabIndex = 11;
            this.checkStepMove.Text = "Step Move";
            this.checkStepMove.UseVisualStyleBackColor = true;
            // 
            // moveStep
            // 
            this.moveStep.FormattingEnabled = true;
            this.moveStep.Items.AddRange(new object[] {
            "100",
            "500",
            "1000",
            "5000",
            "10000"});
            this.moveStep.Location = new System.Drawing.Point(93, 5);
            this.moveStep.Name = "moveStep";
            this.moveStep.Size = new System.Drawing.Size(66, 20);
            this.moveStep.TabIndex = 10;
            // 
            // Joystick2AxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelOption);
            this.Name = "Joystick2AxisControl";
            this.Size = new System.Drawing.Size(218, 178);
            this.panelButtons.ResumeLayout(false);
            this.panelOption.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelButtons;
        private System.Windows.Forms.Panel panelOption;
        private System.Windows.Forms.ComboBox moveStep;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonFoward;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.CheckBox checkStepMove;
    }
}
