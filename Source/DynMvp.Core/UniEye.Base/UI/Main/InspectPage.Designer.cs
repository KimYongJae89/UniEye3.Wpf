using System;

namespace UniEye.Base.UI.Main
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectPage));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.labelModuleId = new Infragistics.Win.Misc.UltraLabel();
            this.labelModelName = new Infragistics.Win.Misc.UltraLabel();
            this.inspectNo = new Infragistics.Win.Misc.UltraLabel();
            this.modelName = new Infragistics.Win.Misc.UltraLabel();
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
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
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelModuleId
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Segoe UI Light";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.labelModuleId.Appearance = appearance1;
            this.labelModuleId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModuleId.Location = new System.Drawing.Point(4, 42);
            this.labelModuleId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelModuleId.Name = "labelModuleId";
            this.labelModuleId.Size = new System.Drawing.Size(115, 36);
            this.labelModuleId.TabIndex = 13;
            this.labelModuleId.Text = "Inspect No";
            // 
            // labelModelName
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Segoe UI Light";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.labelModelName.Appearance = appearance2;
            this.labelModelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModelName.Location = new System.Drawing.Point(4, 5);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(115, 34);
            this.labelModelName.TabIndex = 14;
            this.labelModelName.Text = "Model";
            this.labelModelName.Click += new System.EventHandler(this.LabelModelName_Click);
            // 
            // inspectNo
            // 
            this.inspectNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Segoe UI Light";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Middle";
            this.inspectNo.Appearance = appearance3;
            this.inspectNo.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.inspectNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inspectNo.Location = new System.Drawing.Point(123, 42);
            this.inspectNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inspectNo.Name = "inspectNo";
            this.inspectNo.Size = new System.Drawing.Size(515, 36);
            this.inspectNo.TabIndex = 15;
            this.inspectNo.Text = "None";
            // 
            // modelName
            // 
            this.modelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Segoe UI Light";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            appearance4.TextHAlignAsString = "Left";
            appearance4.TextVAlignAsString = "Middle";
            this.modelName.Appearance = appearance4;
            this.modelName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.modelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.modelName.Location = new System.Drawing.Point(123, 5);
            this.modelName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(515, 34);
            this.modelName.TabIndex = 16;
            this.modelName.Text = "1111";
            // 
            // buttonStart
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStart.Appearance = appearance5;
            this.buttonStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.buttonStart.ImageSize = new System.Drawing.Size(70, 90);
            this.buttonStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonStart.Location = new System.Drawing.Point(4, 81);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(116, 118);
            this.buttonStart.TabIndex = 11;
            this.buttonStart.Tag = "Start";
            this.buttonStart.UseAppStyling = false;
            this.buttonStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // labelTotal
            // 
            appearance6.BackColor = System.Drawing.Color.DodgerBlue;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Segoe UI Light";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.labelTotal.Appearance = appearance6;
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
            appearance7.BackColor = System.Drawing.Color.Ivory;
            appearance7.BorderColor = System.Drawing.Color.Black;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Segoe UI Light";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.TextHAlignAsString = "Center";
            appearance7.TextVAlignAsString = "Middle";
            this.productionTotal.Appearance = appearance7;
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
            appearance8.BackColor = System.Drawing.Color.Red;
            appearance8.FontData.BoldAsString = "True";
            appearance8.FontData.Name = "Segoe UI Light";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.labelDefect.Appearance = appearance8;
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
            appearance9.BackColor = System.Drawing.Color.Ivory;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.FontData.BoldAsString = "True";
            appearance9.FontData.Name = "Segoe UI Light";
            appearance9.FontData.SizeInPoints = 12F;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.productionNg.Appearance = appearance9;
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
            appearance10.BackColor = System.Drawing.Color.Gold;
            appearance10.FontData.BoldAsString = "True";
            appearance10.FontData.Name = "Segoe UI Light";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.labelInspTime.Appearance = appearance10;
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
            appearance11.BackColor = System.Drawing.Color.Ivory;
            appearance11.BorderColor = System.Drawing.Color.Black;
            appearance11.FontData.BoldAsString = "True";
            appearance11.FontData.Name = "Segoe UI Light";
            appearance11.FontData.SizeInPoints = 12F;
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.TextHAlignAsString = "Center";
            appearance11.TextVAlignAsString = "Middle";
            this.inspTime.Appearance = appearance11;
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
            this.buttonResetCount.Click += new System.EventHandler(this.ButtonResetCount_Click);
            // 
            // labelLiveView
            // 
            this.labelLiveView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance12.BackColor = System.Drawing.Color.LightCoral;
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.Name = "Segoe UI Light";
            appearance12.FontData.SizeInPoints = 12F;
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.TextHAlignAsString = "Center";
            appearance12.TextVAlignAsString = "Middle";
            this.labelLiveView.Appearance = appearance12;
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
            this.viewContainer.Location = new System.Drawing.Point(123, 81);
            this.viewContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(744, 670);
            this.viewContainer.TabIndex = 20;
            // 
            // productionGood
            // 
            appearance13.BackColor = System.Drawing.Color.Ivory;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.Name = "Segoe UI Light";
            appearance13.FontData.SizeInPoints = 12F;
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.productionGood.Appearance = appearance13;
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
            appearance14.BackColor = System.Drawing.Color.YellowGreen;
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.Name = "Segoe UI Light";
            appearance14.FontData.SizeInPoints = 12F;
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextHAlignAsString = "Center";
            appearance14.TextVAlignAsString = "Middle";
            this.labelAccept.Appearance = appearance14;
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
            this.labelStatus.Location = new System.Drawing.Point(639, 5);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 260);
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
            this.repeatTimer.Tick += new System.EventHandler(this.RepeatTimer_Tick);
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
            this.repeatInspection.CheckedChanged += new System.EventHandler(this.RepeatInspection_CheckedChanged);
            // 
            // buttonTrigger
            // 
            this.buttonTrigger.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonTrigger.Location = new System.Drawing.Point(4, 204);
            this.buttonTrigger.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTrigger.Name = "buttonTrigger";
            this.buttonTrigger.Size = new System.Drawing.Size(115, 45);
            this.buttonTrigger.TabIndex = 18;
            this.buttonTrigger.Text = "Trigger";
            this.buttonTrigger.UseVisualStyleBackColor = true;
            this.buttonTrigger.Click += new System.EventHandler(this.ButtonTrigger_Click);
            // 
            // InspectPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.repeatInspection);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonTrigger);
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.labelModuleId);
            this.Controls.Add(this.labelLiveView);
            this.Controls.Add(this.labelModelName);
            this.Controls.Add(this.inspectNo);
            this.Controls.Add(this.modelName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "InspectPage";
            this.Size = new System.Drawing.Size(873, 758);
            this.Load += new System.EventHandler(this.InspectionPage_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private Infragistics.Win.Misc.UltraLabel labelModuleId;
        private Infragistics.Win.Misc.UltraLabel labelModelName;
        
        private Infragistics.Win.Misc.UltraLabel inspectNo;
        private Infragistics.Win.Misc.UltraLabel modelName;
        private Infragistics.Win.Misc.UltraButton buttonStart;
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
    }
}
