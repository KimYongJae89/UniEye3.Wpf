namespace UniEye.Base.UI.Main
{
    partial class ModelTileControl
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
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool1 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool();
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool2 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Edit");
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool3 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Delete");
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool4 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Copy");
            Infragistics.Win.UltraWinRadialMenu.RadialMenuTool radialMenuTool5 = new Infragistics.Win.UltraWinRadialMenu.RadialMenuTool("Close");
            this.panelModelList = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonNewModel = new Infragistics.Win.Misc.UltraButton();
            this.modelMenu = new Infragistics.Win.UltraWinRadialMenu.UltraRadialMenu(this.components);
            this.ultraTouchProvider1 = new Infragistics.Win.Touch.UltraTouchProvider(this.components);
            this.timerSearchModelName = new System.Windows.Forms.Timer(this.components);
            this.panelModelList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTouchProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelModelList
            // 
            this.panelModelList.AutoScroll = true;
            this.panelModelList.BackColor = System.Drawing.SystemColors.Window;
            this.panelModelList.Controls.Add(this.buttonNewModel);
            this.panelModelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModelList.Location = new System.Drawing.Point(0, 0);
            this.panelModelList.Name = "panelModelList";
            this.panelModelList.Size = new System.Drawing.Size(785, 469);
            this.panelModelList.TabIndex = 9;
            this.panelModelList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelModelList_MouseUp);
            // 
            // buttonNewModel
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(107)))), ((int)(((byte)(193)))));
            appearance1.FontData.Name = "NanumGothic";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = global::UniEye.Base.Properties.Resources.Circle_Plus;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Bottom";
            this.buttonNewModel.Appearance = appearance1;
            this.buttonNewModel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.buttonNewModel.ImageSize = new System.Drawing.Size(64, 64);
            this.buttonNewModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonNewModel.Location = new System.Drawing.Point(3, 3);
            this.buttonNewModel.Name = "buttonNewModel";
            this.buttonNewModel.Size = new System.Drawing.Size(150, 150);
            this.buttonNewModel.TabIndex = 2;
            this.buttonNewModel.Tag = "New Model";
            this.buttonNewModel.Text = "New Model";
            this.buttonNewModel.UseAppStyling = false;
            this.buttonNewModel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonNewModel.Click += new System.EventHandler(this.buttonNewModel_Click);
            // 
            // modelMenu
            // 
            radialMenuTool2.Key = "Edit";
            radialMenuTool2.Text = "Edit";
            radialMenuTool2.VisiblePosition = 2;
            radialMenuTool3.Key = "Delete";
            radialMenuTool3.Text = "Delete";
            radialMenuTool3.VisiblePosition = 1;
            radialMenuTool4.Key = "Copy";
            radialMenuTool4.Text = "Copy";
            radialMenuTool4.VisiblePosition = 3;
            radialMenuTool5.Key = "Close";
            radialMenuTool5.Text = "Close";
            radialMenuTool5.VisiblePosition = 0;
            radialMenuTool1.Tools.Add(radialMenuTool2);
            radialMenuTool1.Tools.Add(radialMenuTool3);
            radialMenuTool1.Tools.Add(radialMenuTool4);
            radialMenuTool1.Tools.Add(radialMenuTool5);
            this.modelMenu.CenterTool = radialMenuTool1;
            this.modelMenu.MenuSettings.WedgeCount = 4;
            this.modelMenu.OwningControl = this;
            this.modelMenu.ToolClick += new System.EventHandler<Infragistics.Win.UltraWinRadialMenu.RadialMenuToolClickEventArgs>(this.modelMenu_ToolClick);
            // 
            // ultraTouchProvider1
            // 
            this.ultraTouchProvider1.ContainingControl = this;
            // 
            // ModelTileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.panelModelList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "ModelTileControl";
            this.Size = new System.Drawing.Size(785, 469);
            this.Load += new System.EventHandler(this.ModelTileControl_Load);
            this.panelModelList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.modelMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTouchProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelModelList;
        private Infragistics.Win.Misc.UltraButton buttonNewModel;
        private Infragistics.Win.UltraWinRadialMenu.UltraRadialMenu modelMenu;
        private Infragistics.Win.Touch.UltraTouchProvider ultraTouchProvider1;
        private System.Windows.Forms.Timer timerSearchModelName;
    }
}
