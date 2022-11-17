namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class VirtualCameraListForm
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
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.detectAllButton = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.cameraInfoGrid = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnColor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.Location = new System.Drawing.Point(82, 225);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(76, 33);
            this.buttonMoveDown.TabIndex = 175;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveUp.Location = new System.Drawing.Point(13, 225);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(54, 33);
            this.buttonMoveUp.TabIndex = 176;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // detectAllButton
            // 
            this.detectAllButton.Location = new System.Drawing.Point(13, 8);
            this.detectAllButton.Margin = new System.Windows.Forms.Padding(4);
            this.detectAllButton.Name = "detectAllButton";
            this.detectAllButton.Size = new System.Drawing.Size(145, 33);
            this.detectAllButton.TabIndex = 174;
            this.detectAllButton.Text = "Detect All";
            this.detectAllButton.UseVisualStyleBackColor = true;
            this.detectAllButton.Click += new System.EventHandler(this.autoDetectButton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(361, 225);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(92, 33);
            this.buttonCancel.TabIndex = 172;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(253, 225);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(92, 33);
            this.buttonOK.TabIndex = 173;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.AllowUserToAddRows = false;
            this.cameraInfoGrid.AllowUserToDeleteRows = false;
            this.cameraInfoGrid.AllowUserToResizeColumns = false;
            this.cameraInfoGrid.AllowUserToResizeRows = false;
            this.cameraInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnWidth,
            this.columnHeight,
            this.columnColor,
            this.columnEdit});
            this.cameraInfoGrid.Location = new System.Drawing.Point(13, 49);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.MultiSelect = false;
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cameraInfoGrid.Size = new System.Drawing.Size(440, 168);
            this.cameraInfoGrid.TabIndex = 171;
            this.cameraInfoGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cameraInfoGrid_CellContentClick);
            // 
            // columnNo
            // 
            this.columnNo.FillWeight = 100.8369F;
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnNo.Width = 50;
            // 
            // columnWidth
            // 
            this.columnWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnWidth.FillWeight = 98.82014F;
            this.columnWidth.HeaderText = "Width";
            this.columnWidth.Name = "columnWidth";
            this.columnWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnHeight
            // 
            this.columnHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnHeight.FillWeight = 98.82014F;
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            this.columnHeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnColor
            // 
            this.columnColor.FillWeight = 101.5228F;
            this.columnColor.HeaderText = "Color";
            this.columnColor.Name = "columnColor";
            this.columnColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnColor.Width = 50;
            // 
            // columnEdit
            // 
            this.columnEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnEdit.HeaderText = "Edit";
            this.columnEdit.Name = "columnEdit";
            this.columnEdit.Width = 50;
            // 
            // VirtualCameraListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 271);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.detectAllButton);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.cameraInfoGrid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VirtualCameraListForm";
            this.Text = "Virtual Camera List";
            this.Load += new System.EventHandler(this.VirtualCameraListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button detectAllButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnColor;
        private System.Windows.Forms.DataGridViewButtonColumn columnEdit;
    }
}