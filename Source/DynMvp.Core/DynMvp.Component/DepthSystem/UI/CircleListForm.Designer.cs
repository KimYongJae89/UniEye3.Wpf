namespace DynMvp.Component.DepthSystem.UI
{
    partial class CircleListForm
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
            this.circleListGrid = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelImage = new System.Windows.Forms.Panel();
            this.buttonSetPosition1 = new System.Windows.Forms.Button();
            this.buttonSetPosition2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.circleListGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // circleListGrid
            // 
            this.circleListGrid.AllowUserToAddRows = false;
            this.circleListGrid.AllowUserToDeleteRows = false;
            this.circleListGrid.AllowUserToResizeColumns = false;
            this.circleListGrid.AllowUserToResizeRows = false;
            this.circleListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.circleListGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.ColumnX,
            this.columnY,
            this.columnZ});
            this.circleListGrid.Location = new System.Drawing.Point(5, 30);
            this.circleListGrid.Name = "circleListGrid";
            this.circleListGrid.RowHeadersVisible = false;
            this.circleListGrid.RowTemplate.Height = 23;
            this.circleListGrid.Size = new System.Drawing.Size(363, 133);
            this.circleListGrid.TabIndex = 0;
            this.circleListGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.circleListGrid_CellContentClick);
            // 
            // columnIndex
            // 
            this.columnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.columnIndex.Name = "columnIndex";
            // 
            // ColumnX
            // 
            this.ColumnX.HeaderText = "X";
            this.ColumnX.Name = "ColumnX";
            this.ColumnX.Width = 80;
            // 
            // columnY
            // 
            this.columnY.HeaderText = "Y";
            this.columnY.Name = "columnY";
            this.columnY.Width = 80;
            // 
            // columnZ
            // 
            this.columnZ.HeaderText = "Z";
            this.columnZ.Name = "columnZ";
            this.columnZ.Width = 80;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(83, 456);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(104, 35);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(193, 456);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(104, 35);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelImage
            // 
            this.panelImage.Location = new System.Drawing.Point(5, 169);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(362, 285);
            this.panelImage.TabIndex = 2;
            // 
            // buttonSetPosition1
            // 
            this.buttonSetPosition1.Location = new System.Drawing.Point(6, 4);
            this.buttonSetPosition1.Name = "buttonSetPosition1";
            this.buttonSetPosition1.Size = new System.Drawing.Size(84, 23);
            this.buttonSetPosition1.TabIndex = 3;
            this.buttonSetPosition1.Text = "Position 1";
            this.buttonSetPosition1.UseVisualStyleBackColor = true;
            this.buttonSetPosition1.Click += new System.EventHandler(this.buttonSetPosition1_Click);
            // 
            // buttonSetPosition2
            // 
            this.buttonSetPosition2.Location = new System.Drawing.Point(93, 4);
            this.buttonSetPosition2.Name = "buttonSetPosition2";
            this.buttonSetPosition2.Size = new System.Drawing.Size(83, 23);
            this.buttonSetPosition2.TabIndex = 3;
            this.buttonSetPosition2.Text = "Position 2";
            this.buttonSetPosition2.UseVisualStyleBackColor = true;
            this.buttonSetPosition2.Click += new System.EventHandler(this.buttonSetPosition2_Click);
            // 
            // CircleListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 514);
            this.Controls.Add(this.buttonSetPosition2);
            this.Controls.Add(this.buttonSetPosition1);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.circleListGrid);
            this.Name = "CircleListForm";
            this.Text = "Circle List";
            ((System.ComponentModel.ISupportInitialize)(this.circleListGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView circleListGrid;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnX;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnY;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnZ;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Button buttonSetPosition1;
        private System.Windows.Forms.Button buttonSetPosition2;
    }
}