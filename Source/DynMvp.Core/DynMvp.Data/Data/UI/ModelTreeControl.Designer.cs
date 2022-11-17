namespace DynMvp.Data.UI
{
    partial class ModelTreeControl
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
            this.objectTree = new System.Windows.Forms.TreeView();
            this.targetImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.targetImage)).BeginInit();
            this.SuspendLayout();
            // 
            // objectTree
            // 
            this.objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTree.Font = new System.Drawing.Font("나눔고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.objectTree.Location = new System.Drawing.Point(0, 0);
            this.objectTree.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(251, 237);
            this.objectTree.TabIndex = 1;
            this.objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTree_AfterSelect);
            this.objectTree.DoubleClick += new System.EventHandler(this.objectTree_DoubleClick);
            // 
            // targetImage
            // 
            this.targetImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.targetImage.Location = new System.Drawing.Point(0, 237);
            this.targetImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.targetImage.Name = "targetImage";
            this.targetImage.Size = new System.Drawing.Size(251, 196);
            this.targetImage.TabIndex = 2;
            this.targetImage.TabStop = false;
            // 
            // ModelTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectTree);
            this.Controls.Add(this.targetImage);
            this.Font = new System.Drawing.Font("나눔고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ModelTreeControl";
            this.Size = new System.Drawing.Size(251, 433);
            ((System.ComponentModel.ISupportInitialize)(this.targetImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView objectTree;
        private System.Windows.Forms.PictureBox targetImage;
    }
}
