namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class PylonLineCameraListForm3
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
            this.columnNativeBuffering = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveDown.Location = new System.Drawing.Point(237, 202);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(109, 32);
            this.buttonMoveDown.TabIndex = 171;
            this.buttonMoveDown.Text = "Move Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMoveUp.Location = new System.Drawing.Point(124, 202);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(109, 32);
            this.buttonMoveUp.TabIndex = 172;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(1002, 202);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 32);
            this.buttonCancel.TabIndex = 168;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // autoDetectButton
            // 
            this.autoDetectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoDetectButton.Location = new System.Drawing.Point(11, 202);
            this.autoDetectButton.Margin = new System.Windows.Forms.Padding(2);
            this.autoDetectButton.Name = "autoDetectButton";
            this.autoDetectButton.Size = new System.Drawing.Size(109, 32);
            this.autoDetectButton.TabIndex = 169;
            this.autoDetectButton.Text = "Auto Detect";
            this.autoDetectButton.UseVisualStyleBackColor = true;
            this.autoDetectButton.Click += new System.EventHandler(this.autoDetectButton_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(944, 202);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(54, 32);
            this.buttonOK.TabIndex = 170;
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
            this.ColumnDeviceUserId,
            this.ColumnIpAddress,
            this.ColumnSerialNo,
            this.columnModelName,
            this.ColumnWidth,
            this.ColumnHeight,
            this.ColumnNumBand,
            this.ColumnRotateFlipType,
            this.columnNativeBuffering,
            this.Edit});
            this.cameraInfoGrid.Location = new System.Drawing.Point(11, 11);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(2);
            this.cameraInfoGrid.MultiSelect = false;
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowHeadersVisible = false;
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cameraInfoGrid.Size = new System.Drawing.Size(1059, 184);
            this.cameraInfoGrid.TabIndex = 167;
            this.cameraInfoGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cameraInfoGrid_CellContentClick);
            this.cameraInfoGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cameraInfoGrid_CellContentDoubleClick);
            // 
            // columnNo
            // 
            this.columnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnNo.Width = 38;
            // 
            // ColumnDeviceUserId
            // 
            this.ColumnDeviceUserId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnDeviceUserId.HeaderText = "Device User Id";
            this.ColumnDeviceUserId.Name = "ColumnDeviceUserId";
            this.ColumnDeviceUserId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ColumnIpAddress
            // 
            this.ColumnIpAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnIpAddress.HeaderText = "IP Address";
            this.ColumnIpAddress.Name = "ColumnIpAddress";
            this.ColumnIpAddress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnIpAddress.Width = 96;
            // 
            // ColumnSerialNo
            // 
            this.ColumnSerialNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnSerialNo.HeaderText = "Serial Number";
            this.ColumnSerialNo.Name = "ColumnSerialNo";
            this.ColumnSerialNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnSerialNo.Width = 116;
            // 
            // columnModelName
            // 
            this.columnModelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnModelName.HeaderText = "Model Name";
            this.columnModelName.Name = "columnModelName";
            this.columnModelName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnModelName.Width = 108;
            // 
            // ColumnWidth
            // 
            this.ColumnWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnWidth.FillWeight = 50F;
            this.ColumnWidth.HeaderText = "Width";
            this.ColumnWidth.Name = "ColumnWidth";
            this.ColumnWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWidth.Width = 71;
            // 
            // ColumnHeight
            // 
            this.ColumnHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnHeight.FillWeight = 50F;
            this.ColumnHeight.HeaderText = "Height";
            this.ColumnHeight.Name = "ColumnHeight";
            this.ColumnHeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeight.Width = 75;
            // 
            // ColumnNumBand
            // 
            this.ColumnNumBand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnNumBand.FillWeight = 60F;
            this.ColumnNumBand.HeaderText = "Num Band";
            this.ColumnNumBand.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.ColumnNumBand.Name = "ColumnNumBand";
            this.ColumnNumBand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnNumBand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnNumBand.Width = 95;
            // 
            // ColumnRotateFlipType
            // 
            this.ColumnRotateFlipType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
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
            this.ColumnRotateFlipType.Width = 73;
            // 
            // columnNativeBuffering
            // 
            this.columnNativeBuffering.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnNativeBuffering.HeaderText = "Native Buffering";
            this.columnNativeBuffering.Name = "columnNativeBuffering";
            this.columnNativeBuffering.Width = 105;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Width = 50;
            // 
            // PylonLineCameraListForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1079, 239);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.autoDetectButton);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.cameraInfoGrid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PylonLineCameraListForm3";
            this.Text = "Pylon Line Camera List";
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
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnNativeBuffering;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
    }
}