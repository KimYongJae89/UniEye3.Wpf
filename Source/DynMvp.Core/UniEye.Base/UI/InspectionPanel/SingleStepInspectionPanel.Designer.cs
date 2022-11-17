using System;

namespace UniEye.Base.UI.InspectionPanel
{
    partial class SingleStepInspectionPanel
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
            this.resultViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // resultViewPanel
            // 
            this.resultViewPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.resultViewPanel.ColumnCount = 3;
            this.resultViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.resultViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.resultViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.resultViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultViewPanel.Location = new System.Drawing.Point(0, 0);
            this.resultViewPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.resultViewPanel.Name = "resultViewPanel";
            this.resultViewPanel.RowCount = 1;
            this.resultViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.resultViewPanel.Size = new System.Drawing.Size(769, 586);
            this.resultViewPanel.TabIndex = 17;
            // 
            // SingleStepInspectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultViewPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SingleStepInspectionPanel";
            this.Size = new System.Drawing.Size(769, 586);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel resultViewPanel;
    }
}
