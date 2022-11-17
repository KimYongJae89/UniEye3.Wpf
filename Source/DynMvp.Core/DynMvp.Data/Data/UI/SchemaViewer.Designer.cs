namespace DynMvp.Data.UI
{
    partial class SchemaViewer
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuMoveTop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDefaultPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColor = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonShowFov = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowPad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowPath = new System.Windows.Forms.ToolStripButton();
            this.contextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMoveTop,
            this.menuMoveUp,
            this.moveDownToolStripMenuItem,
            this.moveBottomToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.visibleToolStripMenuItem,
            this.propertyToolStripMenuItem,
            this.setDefaultPropertyToolStripMenuItem,
            this.backgroundColor});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(184, 208);
            // 
            // menuMoveTop
            // 
            this.menuMoveTop.Name = "menuMoveTop";
            this.menuMoveTop.Size = new System.Drawing.Size(183, 22);
            this.menuMoveTop.Text = "Move Top";
            this.menuMoveTop.Click += new System.EventHandler(this.MenuMoveTop_Click);
            // 
            // menuMoveUp
            // 
            this.menuMoveUp.Name = "menuMoveUp";
            this.menuMoveUp.Size = new System.Drawing.Size(183, 22);
            this.menuMoveUp.Text = "Move Up";
            this.menuMoveUp.Click += new System.EventHandler(this.MenuMoveUp_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.MoveDownToolStripMenuItem_Click);
            // 
            // moveBottomToolStripMenuItem
            // 
            this.moveBottomToolStripMenuItem.Name = "moveBottomToolStripMenuItem";
            this.moveBottomToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.moveBottomToolStripMenuItem.Text = "Move Bottom";
            this.moveBottomToolStripMenuItem.Click += new System.EventHandler(this.MoveBottomToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // visibleToolStripMenuItem
            // 
            this.visibleToolStripMenuItem.Name = "visibleToolStripMenuItem";
            this.visibleToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.visibleToolStripMenuItem.Text = "Visible";
            this.visibleToolStripMenuItem.Click += new System.EventHandler(this.visibleToolStripMenuItem_Click);
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.propertyToolStripMenuItem.Text = "Property";
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.PropertyToolStripMenuItem_Click);
            // 
            // setDefaultPropertyToolStripMenuItem
            // 
            this.setDefaultPropertyToolStripMenuItem.Name = "setDefaultPropertyToolStripMenuItem";
            this.setDefaultPropertyToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setDefaultPropertyToolStripMenuItem.Text = "Set Default Property";
            this.setDefaultPropertyToolStripMenuItem.Click += new System.EventHandler(this.SetDefaultPropertyToolStripMenuItem_Click);
            // 
            // backgroundColor
            // 
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.Size = new System.Drawing.Size(183, 22);
            this.backgroundColor.Text = "BackgroundColor";
            this.backgroundColor.Click += new System.EventHandler(this.BackgroundColor_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowFov,
            this.toolStripButtonShowPad,
            this.toolStripButtonShowPath});
            this.toolStrip.Location = new System.Drawing.Point(692, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(76, 575);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonShowFov
            // 
            this.toolStripButtonShowFov.Image = global::DynMvp.Data.Properties.Resources.FOV32;
            this.toolStripButtonShowFov.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonShowFov.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowFov.Margin = new System.Windows.Forms.Padding(3);
            this.toolStripButtonShowFov.Name = "toolStripButtonShowFov";
            this.toolStripButtonShowFov.Size = new System.Drawing.Size(67, 51);
            this.toolStripButtonShowFov.Text = "Show FOV";
            this.toolStripButtonShowFov.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonShowFov.Click += new System.EventHandler(this.ToolStripButtonShowFov_Click);
            // 
            // toolStripButtonShowPad
            // 
            this.toolStripButtonShowPad.Image = global::DynMvp.Data.Properties.Resources.PAD32;
            this.toolStripButtonShowPad.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonShowPad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowPad.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.toolStripButtonShowPad.Name = "toolStripButtonShowPad";
            this.toolStripButtonShowPad.Size = new System.Drawing.Size(67, 51);
            this.toolStripButtonShowPad.Text = "Show PAD";
            this.toolStripButtonShowPad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonShowPad.Click += new System.EventHandler(this.ToolStripButtonShowPad_Click);
            // 
            // toolStripButtonShowPath
            // 
            this.toolStripButtonShowPath.Image = global::DynMvp.Data.Properties.Resources.PATH32;
            this.toolStripButtonShowPath.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonShowPath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowPath.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.toolStripButtonShowPath.Name = "toolStripButtonShowPath";
            this.toolStripButtonShowPath.Size = new System.Drawing.Size(67, 51);
            this.toolStripButtonShowPath.Text = "Show Path";
            this.toolStripButtonShowPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonShowPath.Click += new System.EventHandler(this.ToolStripButtonShowPath_Click);
            // 
            // SchemaViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Name = "SchemaViewer";
            this.Size = new System.Drawing.Size(768, 575);
            this.Load += new System.EventHandler(this.SchemaViewer_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SchemaViewer_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SchemaViewer_MouseDoubleClick);
            this.contextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem menuMoveTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveBottomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visibleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDefaultPropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundColor;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowFov;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowPad;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowPath;
    }
}
