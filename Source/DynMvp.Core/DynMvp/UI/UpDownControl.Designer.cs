namespace DynMvp.UI
{
    partial class UpDownControl
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
            this.toolPanel = new System.Windows.Forms.Panel();
            this.downButton = new System.Windows.Forms.Button();
            this.keyboardButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.repeatClickTimer = new System.Windows.Forms.Timer(this.components);
            this.toolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolPanel
            // 
            this.toolPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolPanel.Controls.Add(this.downButton);
            this.toolPanel.Controls.Add(this.keyboardButton);
            this.toolPanel.Controls.Add(this.closeButton);
            this.toolPanel.Controls.Add(this.nextButton);
            this.toolPanel.Controls.Add(this.upButton);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(280, 60);
            this.toolPanel.TabIndex = 1;
            // 
            // downButton
            // 
            this.downButton.Image = global::DynMvp.Properties.Resources.down;
            this.downButton.Location = new System.Drawing.Point(3, 3);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(54, 52);
            this.downButton.TabIndex = 0;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            this.downButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.downButton_MouseDown);
            this.downButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.downButton_MouseUp);
            // 
            // keyboardButton
            // 
            this.keyboardButton.Image = global::DynMvp.Properties.Resources.keyboard;
            this.keyboardButton.Location = new System.Drawing.Point(111, 3);
            this.keyboardButton.Name = "keyboardButton";
            this.keyboardButton.Size = new System.Drawing.Size(54, 52);
            this.keyboardButton.TabIndex = 0;
            this.keyboardButton.UseVisualStyleBackColor = true;
            this.keyboardButton.Click += new System.EventHandler(this.keyboardButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Image = global::DynMvp.Properties.Resources.exit;
            this.closeButton.Location = new System.Drawing.Point(221, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(54, 52);
            this.closeButton.TabIndex = 0;
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Image = global::DynMvp.Properties.Resources.next;
            this.nextButton.Location = new System.Drawing.Point(166, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(54, 52);
            this.nextButton.TabIndex = 0;
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::DynMvp.Properties.Resources.up;
            this.upButton.Location = new System.Drawing.Point(57, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(54, 52);
            this.upButton.TabIndex = 0;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            this.upButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.upButton_MouseDown);
            this.upButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.upButton_MouseUp);
            // 
            // repeatClickTimer
            // 
            this.repeatClickTimer.Tick += new System.EventHandler(this.repeatClickTimer_Tick);
            // 
            // UpDownControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 60);
            this.Controls.Add(this.toolPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpDownControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.UpDownControl_Load);
            this.toolPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button keyboardButton;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Timer repeatClickTimer;
        private System.Windows.Forms.Button closeButton;
    }
}
