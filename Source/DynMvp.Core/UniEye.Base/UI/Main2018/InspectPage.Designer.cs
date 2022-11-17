using System;

namespace UniEye.Base.UI.Main2018
{
    partial class InspectPage
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            this.labelModuleId = new Infragistics.Win.Misc.UltraLabel();
            this.inspectNo = new Infragistics.Win.Misc.UltraLabel();
            this.labelTotal = new Infragistics.Win.Misc.UltraLabel();
            this.productionTotal = new Infragistics.Win.Misc.UltraLabel();
            this.labelDefect = new Infragistics.Win.Misc.UltraLabel();
            this.productionNg = new Infragistics.Win.Misc.UltraLabel();
            this.labelInspTime = new Infragistics.Win.Misc.UltraLabel();
            this.inspTime = new Infragistics.Win.Misc.UltraLabel();
            this.buttonResetCount = new System.Windows.Forms.Button();
            this.labelLiveView = new Infragistics.Win.Misc.UltraLabel();
            this.viewContainer = new System.Windows.Forms.Panel();
            this.productionGood = new Infragistics.Win.Misc.UltraLabel();
            this.labelAccept = new Infragistics.Win.Misc.UltraLabel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.repeatTimer = new System.Windows.Forms.Timer(this.components);
            this.repeatInspection = new System.Windows.Forms.CheckBox();
            this.buttonTrigger = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelModuleId
            // 
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance23.FontData.BoldAsString = "True";
            appearance23.FontData.Name = "Segoe UI Light";
            appearance23.FontData.SizeInPoints = 12F;
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.TextHAlignAsString = "Center";
            appearance23.TextVAlignAsString = "Middle";
            this.labelModuleId.Appearance = appearance23;
            this.labelModuleId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModuleId.Location = new System.Drawing.Point(5, 81);
            this.labelModuleId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelModuleId.Name = "labelModuleId";
            this.labelModuleId.Size = new System.Drawing.Size(115, 36);
            this.labelModuleId.TabIndex = 13;
            this.labelModuleId.Text = "Inspect No";
            // 
            // inspectNo
            // 
            this.inspectNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance24.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.Name = "Segoe UI Light";
            appearance24.FontData.SizeInPoints = 12F;
            appearance24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            appearance24.TextHAlignAsString = "Left";
            appearance24.TextVAlignAsString = "Middle";
            this.inspectNo.Appearance = appearance24;
            this.inspectNo.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.inspectNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inspectNo.Location = new System.Drawing.Point(124, 81);
            this.inspectNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inspectNo.Name = "inspectNo";
            this.inspectNo.Size = new System.Drawing.Size(260, 36);
            this.inspectNo.TabIndex = 15;
            this.inspectNo.Text = "None";
            // 
            // labelTotal
            // 
            appearance25.BackColor = System.Drawing.Color.DodgerBlue;
            appearance25.FontData.BoldAsString = "True";
            appearance25.FontData.Name = "Segoe UI Light";
            appearance25.FontData.SizeInPoints = 12F;
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.TextHAlignAsString = "Center";
            appearance25.TextVAlignAsString = "Middle";
            this.labelTotal.Appearance = appearance25;
            this.labelTotal.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelTotal.Location = new System.Drawing.Point(0, 2);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(115, 39);
            this.labelTotal.TabIndex = 14;
            this.labelTotal.Text = "Insp. Total";
            // 
            // productionTotal
            // 
            appearance26.BackColor = System.Drawing.Color.Ivory;
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.FontData.BoldAsString = "True";
            appearance26.FontData.Name = "Segoe UI Light";
            appearance26.FontData.SizeInPoints = 12F;
            appearance26.ForeColor = System.Drawing.Color.Black;
            appearance26.TextHAlignAsString = "Center";
            appearance26.TextVAlignAsString = "Middle";
            this.productionTotal.Appearance = appearance26;
            this.productionTotal.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionTotal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionTotal.Location = new System.Drawing.Point(0, 41);
            this.productionTotal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.productionTotal.Name = "productionTotal";
            this.productionTotal.Size = new System.Drawing.Size(115, 39);
            this.productionTotal.TabIndex = 14;
            this.productionTotal.Text = "0";
            // 
            // labelDefect
            // 
            appearance27.BackColor = System.Drawing.Color.Red;
            appearance27.FontData.BoldAsString = "True";
            appearance27.FontData.Name = "Segoe UI Light";
            appearance27.FontData.SizeInPoints = 12F;
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.TextHAlignAsString = "Center";
            appearance27.TextVAlignAsString = "Middle";
            this.labelDefect.Appearance = appearance27;
            this.labelDefect.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelDefect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDefect.Location = new System.Drawing.Point(0, 166);
            this.labelDefect.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(115, 39);
            this.labelDefect.TabIndex = 14;
            this.labelDefect.Text = "NG";
            // 
            // productionNg
            // 
            appearance28.BackColor = System.Drawing.Color.Ivory;
            appearance28.BorderColor = System.Drawing.Color.Black;
            appearance28.FontData.BoldAsString = "True";
            appearance28.FontData.Name = "Segoe UI Light";
            appearance28.FontData.SizeInPoints = 12F;
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.TextHAlignAsString = "Center";
            appearance28.TextVAlignAsString = "Middle";
            this.productionNg.Appearance = appearance28;
            this.productionNg.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionNg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionNg.Location = new System.Drawing.Point(0, 205);
            this.productionNg.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.productionNg.Name = "productionNg";
            this.productionNg.Size = new System.Drawing.Size(115, 39);
            this.productionNg.TabIndex = 14;
            this.productionNg.Text = "0";
            // 
            // labelInspTime
            // 
            appearance29.BackColor = System.Drawing.Color.Gold;
            appearance29.FontData.BoldAsString = "True";
            appearance29.FontData.Name = "Segoe UI Light";
            appearance29.FontData.SizeInPoints = 12F;
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.TextHAlignAsString = "Center";
            appearance29.TextVAlignAsString = "Middle";
            this.labelInspTime.Appearance = appearance29;
            this.labelInspTime.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelInspTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelInspTime.Location = new System.Drawing.Point(0, 248);
            this.labelInspTime.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelInspTime.Name = "labelInspTime";
            this.labelInspTime.Size = new System.Drawing.Size(115, 39);
            this.labelInspTime.TabIndex = 14;
            this.labelInspTime.Text = "Insp Time";
            // 
            // inspTime
            // 
            appearance30.BackColor = System.Drawing.Color.Ivory;
            appearance30.BorderColor = System.Drawing.Color.Black;
            appearance30.FontData.BoldAsString = "True";
            appearance30.FontData.Name = "Segoe UI Light";
            appearance30.FontData.SizeInPoints = 12F;
            appearance30.ForeColor = System.Drawing.Color.Black;
            appearance30.TextHAlignAsString = "Center";
            appearance30.TextVAlignAsString = "Middle";
            this.inspTime.Appearance = appearance30;
            this.inspTime.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.inspTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inspTime.Location = new System.Drawing.Point(0, 287);
            this.inspTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.inspTime.Name = "inspTime";
            this.inspTime.Size = new System.Drawing.Size(115, 39);
            this.inspTime.TabIndex = 14;
            this.inspTime.Text = "0";
            // 
            // buttonResetCount
            // 
            this.buttonResetCount.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonResetCount.Location = new System.Drawing.Point(0, 328);
            this.buttonResetCount.Margin = new System.Windows.Forms.Padding(0);
            this.buttonResetCount.Name = "buttonResetCount";
            this.buttonResetCount.Size = new System.Drawing.Size(115, 45);
            this.buttonResetCount.TabIndex = 18;
            this.buttonResetCount.Text = "Reset";
            this.buttonResetCount.UseVisualStyleBackColor = true;
            this.buttonResetCount.Click += new System.EventHandler(this.buttonResetCount_Click);
            // 
            // labelLiveView
            // 
            this.labelLiveView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance31.BackColor = System.Drawing.Color.LightCoral;
            appearance31.FontData.BoldAsString = "True";
            appearance31.FontData.Name = "Segoe UI Light";
            appearance31.FontData.SizeInPoints = 12F;
            appearance31.ForeColor = System.Drawing.Color.Black;
            appearance31.TextHAlignAsString = "Center";
            appearance31.TextVAlignAsString = "Middle";
            this.labelLiveView.Appearance = appearance31;
            this.labelLiveView.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelLiveView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelLiveView.Location = new System.Drawing.Point(4, 672);
            this.labelLiveView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelLiveView.Name = "labelLiveView";
            this.labelLiveView.Size = new System.Drawing.Size(115, 44);
            this.labelLiveView.TabIndex = 14;
            this.labelLiveView.Text = "Live";
            this.labelLiveView.Visible = false;
            // 
            // viewContainer
            // 
            this.viewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewContainer.Location = new System.Drawing.Point(392, 81);
            this.viewContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(475, 670);
            this.viewContainer.TabIndex = 20;
            // 
            // productionGood
            // 
            appearance32.BackColor = System.Drawing.Color.Ivory;
            appearance32.BorderColor = System.Drawing.Color.Black;
            appearance32.FontData.BoldAsString = "True";
            appearance32.FontData.Name = "Segoe UI Light";
            appearance32.FontData.SizeInPoints = 12F;
            appearance32.ForeColor = System.Drawing.Color.Black;
            appearance32.TextHAlignAsString = "Center";
            appearance32.TextVAlignAsString = "Middle";
            this.productionGood.Appearance = appearance32;
            this.productionGood.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.productionGood.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productionGood.Location = new System.Drawing.Point(0, 123);
            this.productionGood.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.productionGood.Name = "productionGood";
            this.productionGood.Size = new System.Drawing.Size(115, 39);
            this.productionGood.TabIndex = 21;
            this.productionGood.Text = "0";
            // 
            // labelAccept
            // 
            appearance33.BackColor = System.Drawing.Color.YellowGreen;
            appearance33.FontData.BoldAsString = "True";
            appearance33.FontData.Name = "Segoe UI Light";
            appearance33.FontData.SizeInPoints = 12F;
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.TextHAlignAsString = "Center";
            appearance33.TextVAlignAsString = "Middle";
            this.labelAccept.Appearance = appearance33;
            this.labelAccept.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelAccept.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelAccept.Location = new System.Drawing.Point(0, 84);
            this.labelAccept.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelAccept.Name = "labelAccept";
            this.labelAccept.Size = new System.Drawing.Size(115, 39);
            this.labelAccept.TabIndex = 22;
            this.labelAccept.Text = "Good";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.BackColor = System.Drawing.Color.CornflowerBlue;
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStatus.Location = new System.Drawing.Point(4, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(229, 73);
            this.labelStatus.TabIndex = 23;
            this.labelStatus.Text = "Stopped";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelTotal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.productionTotal, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.productionGood, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelAccept, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonResetCount, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelDefect, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.productionNg, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.inspTime, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelInspTime, 0, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(146, 639);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11259F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11259F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11259F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(116, 373);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // repeatTimer
            // 
            this.repeatTimer.Interval = 1000;
            this.repeatTimer.Tick += new System.EventHandler(this.repeatTimer_Tick);
            // 
            // repeatInspection
            // 
            this.repeatInspection.AutoSize = true;
            this.repeatInspection.Location = new System.Drawing.Point(8, 639);
            this.repeatInspection.Name = "repeatInspection";
            this.repeatInspection.Size = new System.Drawing.Size(108, 24);
            this.repeatInspection.TabIndex = 26;
            this.repeatInspection.Text = "Repeat Insp";
            this.repeatInspection.UseVisualStyleBackColor = true;
            this.repeatInspection.Visible = false;
            this.repeatInspection.CheckedChanged += new System.EventHandler(this.repeatInspection_CheckedChanged);
            // 
            // buttonTrigger
            // 
            this.buttonTrigger.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonTrigger.Location = new System.Drawing.Point(100, 591);
            this.buttonTrigger.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrigger.Name = "buttonTrigger";
            this.buttonTrigger.Size = new System.Drawing.Size(115, 45);
            this.buttonTrigger.TabIndex = 18;
            this.buttonTrigger.Text = "Trigger";
            this.buttonTrigger.UseVisualStyleBackColor = true;
            this.buttonTrigger.Click += new System.EventHandler(this.buttonTrigger_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(5, 122);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(380, 268);
            this.dataGridView1.TabIndex = 27;
            // 
            // InspectPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.repeatInspection);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonTrigger);
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.labelModuleId);
            this.Controls.Add(this.labelLiveView);
            this.Controls.Add(this.inspectNo);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "InspectPage";
            this.Size = new System.Drawing.Size(873, 758);
            this.Load += new System.EventHandler(this.InspectionPage_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private Infragistics.Win.Misc.UltraLabel labelModuleId;
        
        private Infragistics.Win.Misc.UltraLabel inspectNo;
        private Infragistics.Win.Misc.UltraLabel labelTotal;
        private Infragistics.Win.Misc.UltraLabel productionTotal;
        private Infragistics.Win.Misc.UltraLabel labelDefect;
        private Infragistics.Win.Misc.UltraLabel productionNg;
        private Infragistics.Win.Misc.UltraLabel inspTime;
        private Infragistics.Win.Misc.UltraLabel labelInspTime;
        private System.Windows.Forms.Button buttonResetCount;
        private Infragistics.Win.Misc.UltraLabel labelLiveView;
        private System.Windows.Forms.Panel viewContainer;
        private Infragistics.Win.Misc.UltraLabel productionGood;
        private Infragistics.Win.Misc.UltraLabel labelAccept;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer repeatTimer;
        private System.Windows.Forms.CheckBox repeatInspection;
        private System.Windows.Forms.Button buttonTrigger;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
