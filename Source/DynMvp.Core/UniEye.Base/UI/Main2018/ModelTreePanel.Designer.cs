namespace UniEye.Base.UI.Main2018
{
    partial class ModelTreePanel
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolbarTop = new System.Windows.Forms.ToolStrip();
            this.modelPropertyButton = new System.Windows.Forms.ToolStripButton();
            this.editSchemaButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelStep = new System.Windows.Forms.ToolStripLabel();
            this.movePrevStepButton = new System.Windows.Forms.ToolStripButton();
            this.moveNextStepButton = new System.Windows.Forms.ToolStripButton();
            this.addStepButton = new System.Windows.Forms.ToolStripButton();
            this.deleteStepButton = new System.Windows.Forms.ToolStripButton();
            this.editStepButton = new System.Windows.Forms.ToolStripButton();
            this.treeViewModel = new System.Windows.Forms.TreeView();
            this.toolbarTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarTop
            // 
            this.toolbarTop.AutoSize = false;
            this.toolbarTop.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolbarTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelPropertyButton,
            this.editSchemaButton,
            this.toolStripSeparator1,
            this.toolStripLabelStep,
            this.movePrevStepButton,
            this.moveNextStepButton,
            this.addStepButton,
            this.deleteStepButton,
            this.editStepButton});
            this.toolbarTop.Location = new System.Drawing.Point(0, 0);
            this.toolbarTop.Name = "toolbarTop";
            this.toolbarTop.Size = new System.Drawing.Size(449, 49);
            this.toolbarTop.TabIndex = 3;
            this.toolbarTop.Text = "toolStrip2";
            // 
            // modelPropertyButton
            // 
            this.modelPropertyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.modelPropertyButton.Image = global::UniEye.Base.Properties.Resources.property_32;
            this.modelPropertyButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.modelPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modelPropertyButton.Name = "modelPropertyButton";
            this.modelPropertyButton.Size = new System.Drawing.Size(36, 46);
            this.modelPropertyButton.Text = "Property";
            this.modelPropertyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.modelPropertyButton.Click += new System.EventHandler(this.modelPropertyButton_Click);
            // 
            // editSchemaButton
            // 
            this.editSchemaButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editSchemaButton.Image = global::UniEye.Base.Properties.Resources.schema_32;
            this.editSchemaButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editSchemaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editSchemaButton.Name = "editSchemaButton";
            this.editSchemaButton.Size = new System.Drawing.Size(33, 46);
            this.editSchemaButton.Text = "Schema";
            this.editSchemaButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 49);
            // 
            // toolStripLabelStep
            // 
            this.toolStripLabelStep.Name = "toolStripLabelStep";
            this.toolStripLabelStep.Size = new System.Drawing.Size(44, 46);
            this.toolStripLabelStep.Text = "Step";
            // 
            // movePrevStepButton
            // 
            this.movePrevStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.movePrevStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_left_32;
            this.movePrevStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.movePrevStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.movePrevStepButton.Name = "movePrevStepButton";
            this.movePrevStepButton.Size = new System.Drawing.Size(36, 46);
            this.movePrevStepButton.Text = "toolStripButton1";
            // 
            // moveNextStepButton
            // 
            this.moveNextStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveNextStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.moveNextStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.moveNextStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveNextStepButton.Name = "moveNextStepButton";
            this.moveNextStepButton.Size = new System.Drawing.Size(36, 46);
            this.moveNextStepButton.Text = "toolStripButton2";
            // 
            // addStepButton
            // 
            this.addStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addStepButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addStepButton.Name = "addStepButton";
            this.addStepButton.Size = new System.Drawing.Size(36, 46);
            this.addStepButton.Text = "Add";
            this.addStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteStepButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(36, 46);
            this.deleteStepButton.Text = "Delete";
            this.deleteStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // editStepButton
            // 
            this.editStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editStepButton.Image = global::UniEye.Base.Properties.Resources.edit_32;
            this.editStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editStepButton.Name = "editStepButton";
            this.editStepButton.Size = new System.Drawing.Size(36, 46);
            this.editStepButton.Text = "Edit";
            this.editStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // treeViewModel
            // 
            this.treeViewModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewModel.Location = new System.Drawing.Point(0, 49);
            this.treeViewModel.Name = "treeViewModel";
            this.treeViewModel.Size = new System.Drawing.Size(449, 389);
            this.treeViewModel.TabIndex = 4;
            // 
            // ModelTreePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewModel);
            this.Controls.Add(this.toolbarTop);
            this.Name = "ModelTreePanel";
            this.Size = new System.Drawing.Size(449, 438);
            this.toolbarTop.ResumeLayout(false);
            this.toolbarTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbarTop;
        private System.Windows.Forms.ToolStripButton modelPropertyButton;
        private System.Windows.Forms.ToolStripButton editSchemaButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelStep;
        private System.Windows.Forms.ToolStripButton movePrevStepButton;
        private System.Windows.Forms.ToolStripButton moveNextStepButton;
        private System.Windows.Forms.ToolStripButton addStepButton;
        private System.Windows.Forms.ToolStripButton deleteStepButton;
        private System.Windows.Forms.ToolStripButton editStepButton;
        private System.Windows.Forms.TreeView treeViewModel;
    }
}
