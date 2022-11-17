namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class PylonCameraListForm2
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.autoDetectButton = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.cameraInfoGrid = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeviceUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIpAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNumBand = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnRotateFlipType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDown.Location = new System.Drawing.Point(827, 13);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(69, 33);
            this.buttonMoveDown.TabIndex = 171;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUp.Location = new System.Drawing.Point(758, 13);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 33);
            this.buttonMoveUp.TabIndex = 172;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(789, 256);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 168;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // autoDetectButton
            // 
            this.autoDetectButton.Location = new System.Drawing.Point(13, 13);
            this.autoDetectButton.Margin = new System.Windows.Forms.Padding(4);
            this.autoDetectButton.Name = "autoDetectButton";
            this.autoDetectButton.Size = new System.Drawing.Size(107, 33);
            this.autoDetectButton.TabIndex = 169;
            this.autoDetectButton.Text = "Auto Detect";
            this.autoDetectButton.UseVisualStyleBackColor = true;
            this.autoDetectButton.Click += new System.EventHandler(this.AutoDetectButton_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(660, 256);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 170;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
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
            this.ColumnDeviceUserId,
            this.ColumnIpAddress,
            this.ColumnSerialNo,
            this.columnModelName,
            this.ColumnWidth,
            this.ColumnHeight,
            this.ColumnNumBand,
            this.ColumnRotateFlipType});
            this.cameraInfoGrid.Location = new System.Drawing.Point(13, 59);
            this.cameraInfoGrid.MultiSelect = false;
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowHeadersVisible = false;
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cameraInfoGrid.Size = new System.Drawing.Size(883, 190);
            this.cameraInfoGrid.TabIndex = 167;
            this.cameraInfoGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CameraInfoGrid_CellDoubleClick);
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnNo.Width = 50;
            // 
            // ColumnDeviceUserId
            // 
            this.ColumnDeviceUserId.HeaderText = "Device User Id";
            this.ColumnDeviceUserId.Name = "ColumnDeviceUserId";
            this.ColumnDeviceUserId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnDeviceUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDeviceUserId.Width = 133;
            // 
            // ColumnIpAddress
            // 
            this.ColumnIpAddress.HeaderText = "IP Address";
            this.ColumnIpAddress.Name = "ColumnIpAddress";
            this.ColumnIpAddress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnIpAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnIpAddress.Width = 120;
            // 
            // ColumnSerialNo
            // 
            this.ColumnSerialNo.HeaderText = "Serial Number";
            this.ColumnSerialNo.Name = "ColumnSerialNo";
            this.ColumnSerialNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnSerialNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnSerialNo.Width = 132;
            // 
            // columnModelName
            // 
            this.columnModelName.HeaderText = "Model Name";
            this.columnModelName.Name = "columnModelName";
            this.columnModelName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnModelName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnModelName.Width = 133;
            // 
            // ColumnWidth
            // 
            this.ColumnWidth.FillWeight = 50F;
            this.ColumnWidth.HeaderText = "Width";
            this.ColumnWidth.Name = "ColumnWidth";
            this.ColumnWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnWidth.Width = 60;
            // 
            // ColumnHeight
            // 
            this.ColumnHeight.FillWeight = 50F;
            this.ColumnHeight.HeaderText = "Height";
            this.ColumnHeight.Name = "ColumnHeight";
            this.ColumnHeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnHeight.Width = 60;
            // 
            // ColumnNumBand
            // 
            this.ColumnNumBand.FillWeight = 60F;
            this.ColumnNumBand.HeaderText = "Band";
            this.ColumnNumBand.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.ColumnNumBand.Name = "ColumnNumBand";
            this.ColumnNumBand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnNumBand.Width = 60;
            // 
            // ColumnRotateFlipType
            // 
            this.ColumnRotateFlipType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnRotateFlipType.FillWeight = 150F;
            this.ColumnRotateFlipType.HeaderText = "Rotet Flip Type";
            this.ColumnRotateFlipType.Items.AddRange(new object[] {
            "RotateNoneFlipNone",
            "Rotate180FlipXY",
            "Rotate90FlipNone",
            "Rotate270FlipXY",
            "Rotate180FlipNone",
            "RotateNoneFlipXY",
            "Rotate270FlipNone",
            "Rotate90FlipXY",
            "RotateNoneFlipX",
            "Rotate180FlipY",
            "Rotate90FlipX",
            "Rotate270FlipY",
            "Rotate180FlipX",
            "RotateNoneFlipY",
            "Rotate270FlipX",
            "Rotate90FlipY"});
            this.ColumnRotateFlipType.MaxDropDownItems = 16;
            this.ColumnRotateFlipType.Name = "ColumnRotateFlipType";
            // 
            // PylonCameraListForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 293);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.autoDetectButton);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.cameraInfoGrid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PylonCameraListForm2";
            this.Text = "Pylon Camera List";
            this.Load += new System.EventHandler(this.PylonCameraListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button autoDetectButton;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeviceUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIpAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHeight;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnNumBand;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnRotateFlipType;
    }
}