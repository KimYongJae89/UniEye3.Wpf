namespace UniEye.Base.UI
{
    partial class ModelForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelModelName = new System.Windows.Forms.Label();
            this.modelName = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.extraModelPropertyPanel = new System.Windows.Forms.Panel();
            this.panelProductCode = new System.Windows.Forms.Panel();
            this.labelProductName = new System.Windows.Forms.Label();
            this.productName = new System.Windows.Forms.TextBox();
            this.itemCode = new System.Windows.Forms.TextBox();
            this.labelItemCode = new System.Windows.Forms.Label();
            this.panelDefault = new System.Windows.Forms.Panel();
            this.cmbCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.mainPanel.SuspendLayout();
            this.panelProductCode.SuspendLayout();
            this.panelDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategory)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 38);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOK.Location = new System.Drawing.Point(123, 3);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 37);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelModelName
            // 
            this.labelModelName.BackColor = System.Drawing.Color.Navy;
            this.labelModelName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelModelName.ForeColor = System.Drawing.Color.White;
            this.labelModelName.Location = new System.Drawing.Point(3, 3);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(164, 27);
            this.labelModelName.TabIndex = 0;
            this.labelModelName.Text = "Name";
            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelName
            // 
            this.modelName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.modelName.Location = new System.Drawing.Point(172, 3);
            this.modelName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(284, 26);
            this.modelName.TabIndex = 1;
            // 
            // labelDescription
            // 
            this.labelDescription.BackColor = System.Drawing.Color.Navy;
            this.labelDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDescription.ForeColor = System.Drawing.Color.White;
            this.labelDescription.Location = new System.Drawing.Point(3, 63);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(164, 27);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "Description";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(172, 62);
            this.description.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(284, 120);
            this.description.TabIndex = 7;
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.extraModelPropertyPanel);
            this.mainPanel.Controls.Add(this.panelProductCode);
            this.mainPanel.Controls.Add(this.panelDefault);
            this.mainPanel.Controls.Add(this.panel1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(1, 31);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(462, 508);
            this.mainPanel.TabIndex = 1;
            // 
            // extraModelPropertyPanel
            // 
            this.extraModelPropertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extraModelPropertyPanel.Location = new System.Drawing.Point(0, 247);
            this.extraModelPropertyPanel.Margin = new System.Windows.Forms.Padding(0);
            this.extraModelPropertyPanel.Name = "extraModelPropertyPanel";
            this.extraModelPropertyPanel.Size = new System.Drawing.Size(460, 212);
            this.extraModelPropertyPanel.TabIndex = 15;
            // 
            // panelProductCode
            // 
            this.panelProductCode.Controls.Add(this.labelProductName);
            this.panelProductCode.Controls.Add(this.productName);
            this.panelProductCode.Controls.Add(this.itemCode);
            this.panelProductCode.Controls.Add(this.labelItemCode);
            this.panelProductCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProductCode.Location = new System.Drawing.Point(0, 184);
            this.panelProductCode.Name = "panelProductCode";
            this.panelProductCode.Size = new System.Drawing.Size(460, 63);
            this.panelProductCode.TabIndex = 18;
            // 
            // labelProductName
            // 
            this.labelProductName.BackColor = System.Drawing.Color.Navy;
            this.labelProductName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelProductName.ForeColor = System.Drawing.Color.White;
            this.labelProductName.Location = new System.Drawing.Point(3, 3);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(164, 27);
            this.labelProductName.TabIndex = 2;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productName
            // 
            this.productName.Location = new System.Drawing.Point(172, 3);
            this.productName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.productName.Name = "productName";
            this.productName.Size = new System.Drawing.Size(284, 26);
            this.productName.TabIndex = 3;
            // 
            // itemCode
            // 
            this.itemCode.Location = new System.Drawing.Point(172, 33);
            this.itemCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.itemCode.Name = "itemCode";
            this.itemCode.Size = new System.Drawing.Size(284, 26);
            this.itemCode.TabIndex = 5;
            // 
            // labelItemCode
            // 
            this.labelItemCode.BackColor = System.Drawing.Color.Navy;
            this.labelItemCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelItemCode.ForeColor = System.Drawing.Color.White;
            this.labelItemCode.Location = new System.Drawing.Point(3, 33);
            this.labelItemCode.Margin = new System.Windows.Forms.Padding(0);
            this.labelItemCode.Name = "labelItemCode";
            this.labelItemCode.Size = new System.Drawing.Size(164, 27);
            this.labelItemCode.TabIndex = 4;
            this.labelItemCode.Text = "Item Code";
            this.labelItemCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDefault
            // 
            this.panelDefault.Controls.Add(this.labelModelName);
            this.panelDefault.Controls.Add(this.cmbCategory);
            this.panelDefault.Controls.Add(this.modelName);
            this.panelDefault.Controls.Add(this.label1);
            this.panelDefault.Controls.Add(this.labelDescription);
            this.panelDefault.Controls.Add(this.description);
            this.panelDefault.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDefault.Location = new System.Drawing.Point(0, 0);
            this.panelDefault.Name = "panelDefault";
            this.panelDefault.Size = new System.Drawing.Size(460, 184);
            this.panelDefault.TabIndex = 17;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(172, 32);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(284, 28);
            this.cmbCategory.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Navy;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Category";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 459);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 47);
            this.panel1.TabIndex = 15;
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(464, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 539);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(464, 1);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 508);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(463, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 508);
            // 
            // ModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 540);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Model";
            this.Load += new System.EventHandler(this.ModelForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.panelProductCode.ResumeLayout(false);
            this.panelProductCode.PerformLayout();
            this.panelDefault.ResumeLayout(false);
            this.panelDefault.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategory)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.TextBox modelName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label labelItemCode;
        private System.Windows.Forms.TextBox itemCode;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox productName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Panel extraModelPropertyPanel;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCategory;
        private System.Windows.Forms.Panel panelDefault;
        private System.Windows.Forms.Panel panelProductCode;
        private System.Windows.Forms.Panel panel1;
    }
}