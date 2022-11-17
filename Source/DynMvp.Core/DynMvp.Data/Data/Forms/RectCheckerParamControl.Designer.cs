namespace DynMvp.Data.Forms
{
    partial class RectCheckerParamControl
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
            this.searchRange = new System.Windows.Forms.NumericUpDown();
            this.grayValue = new System.Windows.Forms.NumericUpDown();
            this.projectionHeight = new System.Windows.Forms.NumericUpDown();
            this.labelGrayValue = new System.Windows.Forms.Label();
            this.labelEdgeType = new System.Windows.Forms.Label();
            this.labelSearchRange = new System.Windows.Forms.Label();
            this.labelScan = new System.Windows.Forms.Label();
            this.passRate = new System.Windows.Forms.NumericUpDown();
            this.labelPassRate = new System.Windows.Forms.Label();
            this.labelCardinalPoint = new System.Windows.Forms.Label();
            this.cardinalPoint = new System.Windows.Forms.ComboBox();
            this.outToIn = new System.Windows.Forms.CheckBox();
            this.searchLength = new System.Windows.Forms.NumericUpDown();
            this.labelSearchLength = new System.Windows.Forms.Label();
            this.labelConvexShape = new System.Windows.Forms.Label();
            this.convexShape = new System.Windows.Forms.ComboBox();
            this.edgeType = new System.Windows.Forms.ComboBox();
            this.labelEdgeThickWidth = new System.Windows.Forms.Label();
            this.labelEdgeThickHeight = new System.Windows.Forms.Label();
            this.edgeThickWidth = new System.Windows.Forms.NumericUpDown();
            this.edgeThickHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.searchRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grayValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.passRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edgeThickWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edgeThickHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // searchRange
            // 
            this.searchRange.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchRange.Location = new System.Drawing.Point(260, 12);
            this.searchRange.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRange.Name = "searchRange";
            this.searchRange.Size = new System.Drawing.Size(64, 27);
            this.searchRange.TabIndex = 0;
            this.searchRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.searchRange.ValueChanged += new System.EventHandler(this.searchRange_ValueChanged);
            // 
            // grayValue
            // 
            this.grayValue.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grayValue.Location = new System.Drawing.Point(260, 158);
            this.grayValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.grayValue.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.grayValue.Name = "grayValue";
            this.grayValue.Size = new System.Drawing.Size(64, 27);
            this.grayValue.TabIndex = 0;
            this.grayValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.grayValue.ValueChanged += new System.EventHandler(this.grayValue_ValueChanged);
            // 
            // projectionHeight
            // 
            this.projectionHeight.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.projectionHeight.Location = new System.Drawing.Point(260, 187);
            this.projectionHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.projectionHeight.Name = "projectionHeight";
            this.projectionHeight.Size = new System.Drawing.Size(64, 27);
            this.projectionHeight.TabIndex = 0;
            this.projectionHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.projectionHeight.ValueChanged += new System.EventHandler(this.projectionHeight_ValueChanged);
            // 
            // labelGrayValue
            // 
            this.labelGrayValue.AutoSize = true;
            this.labelGrayValue.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelGrayValue.Location = new System.Drawing.Point(9, 154);
            this.labelGrayValue.Name = "labelGrayValue";
            this.labelGrayValue.Size = new System.Drawing.Size(84, 20);
            this.labelGrayValue.TabIndex = 1;
            this.labelGrayValue.Text = "Gray Value";
            // 
            // labelEdgeType
            // 
            this.labelEdgeType.AutoSize = true;
            this.labelEdgeType.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEdgeType.Location = new System.Drawing.Point(9, 69);
            this.labelEdgeType.Name = "labelEdgeType";
            this.labelEdgeType.Size = new System.Drawing.Size(80, 20);
            this.labelEdgeType.TabIndex = 1;
            this.labelEdgeType.Text = "Edge Type";
            // 
            // labelSearchRange
            // 
            this.labelSearchRange.AutoSize = true;
            this.labelSearchRange.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSearchRange.Location = new System.Drawing.Point(9, 12);
            this.labelSearchRange.Name = "labelSearchRange";
            this.labelSearchRange.Size = new System.Drawing.Size(102, 20);
            this.labelSearchRange.TabIndex = 1;
            this.labelSearchRange.Text = "Search Range";
            // 
            // labelScan
            // 
            this.labelScan.AutoSize = true;
            this.labelScan.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelScan.Location = new System.Drawing.Point(9, 185);
            this.labelScan.Name = "labelScan";
            this.labelScan.Size = new System.Drawing.Size(129, 20);
            this.labelScan.TabIndex = 1;
            this.labelScan.Text = "Projection Height";
            // 
            // passRate
            // 
            this.passRate.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.passRate.Location = new System.Drawing.Point(260, 216);
            this.passRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.passRate.Name = "passRate";
            this.passRate.Size = new System.Drawing.Size(64, 27);
            this.passRate.TabIndex = 0;
            this.passRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.passRate.ValueChanged += new System.EventHandler(this.passRate_ValueChanged);
            // 
            // labelPassRate
            // 
            this.labelPassRate.AutoSize = true;
            this.labelPassRate.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPassRate.Location = new System.Drawing.Point(9, 216);
            this.labelPassRate.Name = "labelPassRate";
            this.labelPassRate.Size = new System.Drawing.Size(73, 20);
            this.labelPassRate.TabIndex = 1;
            this.labelPassRate.Text = "Pass Rate";
            // 
            // labelCardinalPoint
            // 
            this.labelCardinalPoint.AutoSize = true;
            this.labelCardinalPoint.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCardinalPoint.Location = new System.Drawing.Point(9, 247);
            this.labelCardinalPoint.Name = "labelCardinalPoint";
            this.labelCardinalPoint.Size = new System.Drawing.Size(107, 20);
            this.labelCardinalPoint.TabIndex = 1;
            this.labelCardinalPoint.Text = "Cardinal Point";
            // 
            // cardinalPoint
            // 
            this.cardinalPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardinalPoint.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cardinalPoint.FormattingEnabled = true;
            this.cardinalPoint.Items.AddRange(new object[] {
            "East",
            "West",
            "South",
            "North",
            "NorthEast",
            "NorthWest",
            "SouthEast",
            "SouthWest"});
            this.cardinalPoint.Location = new System.Drawing.Point(174, 245);
            this.cardinalPoint.Name = "cardinalPoint";
            this.cardinalPoint.Size = new System.Drawing.Size(150, 28);
            this.cardinalPoint.TabIndex = 2;
            this.cardinalPoint.SelectedIndexChanged += new System.EventHandler(this.cardinalPoint_SelectedIndexChanged);
            // 
            // outToIn
            // 
            this.outToIn.AutoSize = true;
            this.outToIn.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.outToIn.Location = new System.Drawing.Point(13, 307);
            this.outToIn.Name = "outToIn";
            this.outToIn.Size = new System.Drawing.Size(94, 24);
            this.outToIn.TabIndex = 3;
            this.outToIn.Text = "Out To In";
            this.outToIn.UseVisualStyleBackColor = true;
            // 
            // searchLength
            // 
            this.searchLength.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchLength.Location = new System.Drawing.Point(260, 41);
            this.searchLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchLength.Name = "searchLength";
            this.searchLength.Size = new System.Drawing.Size(64, 27);
            this.searchLength.TabIndex = 0;
            this.searchLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.searchLength.ValueChanged += new System.EventHandler(this.searchLength_ValueChanged);
            // 
            // labelSearchLength
            // 
            this.labelSearchLength.AutoSize = true;
            this.labelSearchLength.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSearchLength.Location = new System.Drawing.Point(9, 40);
            this.labelSearchLength.Name = "labelSearchLength";
            this.labelSearchLength.Size = new System.Drawing.Size(106, 20);
            this.labelSearchLength.TabIndex = 1;
            this.labelSearchLength.Text = "Search Length";
            // 
            // labelConvexShape
            // 
            this.labelConvexShape.AutoSize = true;
            this.labelConvexShape.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelConvexShape.Location = new System.Drawing.Point(9, 279);
            this.labelConvexShape.Name = "labelConvexShape";
            this.labelConvexShape.Size = new System.Drawing.Size(106, 20);
            this.labelConvexShape.TabIndex = 1;
            this.labelConvexShape.Text = "Convex Shape";
            // 
            // convexShape
            // 
            this.convexShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.convexShape.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.convexShape.FormattingEnabled = true;
            this.convexShape.Items.AddRange(new object[] {
            "None",
            "Left",
            "Top",
            "Right",
            "Bottom"});
            this.convexShape.Location = new System.Drawing.Point(174, 275);
            this.convexShape.Name = "convexShape";
            this.convexShape.Size = new System.Drawing.Size(150, 28);
            this.convexShape.TabIndex = 2;
            this.convexShape.SelectedIndexChanged += new System.EventHandler(this.convexShape_SelectedIndexChanged);
            // 
            // edgeType
            // 
            this.edgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edgeType.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.edgeType.FormattingEnabled = true;
            this.edgeType.Items.AddRange(new object[] {
            "DarkToLight",
            "LightToDark",
            "Any"});
            this.edgeType.Location = new System.Drawing.Point(174, 70);
            this.edgeType.Name = "edgeType";
            this.edgeType.Size = new System.Drawing.Size(150, 28);
            this.edgeType.TabIndex = 2;
            this.edgeType.SelectedIndexChanged += new System.EventHandler(this.edgeType_SelectedIndexChanged);
            // 
            // labelEdgeThickWidth
            // 
            this.labelEdgeThickWidth.AutoSize = true;
            this.labelEdgeThickWidth.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEdgeThickWidth.Location = new System.Drawing.Point(9, 97);
            this.labelEdgeThickWidth.Name = "labelEdgeThickWidth";
            this.labelEdgeThickWidth.Size = new System.Drawing.Size(130, 20);
            this.labelEdgeThickWidth.TabIndex = 4;
            this.labelEdgeThickWidth.Text = "Edge Thick Width";
            // 
            // labelEdgeThickHeight
            // 
            this.labelEdgeThickHeight.AutoSize = true;
            this.labelEdgeThickHeight.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEdgeThickHeight.Location = new System.Drawing.Point(9, 124);
            this.labelEdgeThickHeight.Name = "labelEdgeThickHeight";
            this.labelEdgeThickHeight.Size = new System.Drawing.Size(135, 20);
            this.labelEdgeThickHeight.TabIndex = 5;
            this.labelEdgeThickHeight.Text = "Edge Thick Height";
            // 
            // edgeThickWidth
            // 
            this.edgeThickWidth.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.edgeThickWidth.Location = new System.Drawing.Point(260, 100);
            this.edgeThickWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.edgeThickWidth.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.edgeThickWidth.Name = "edgeThickWidth";
            this.edgeThickWidth.Size = new System.Drawing.Size(64, 27);
            this.edgeThickWidth.TabIndex = 6;
            this.edgeThickWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.edgeThickWidth.ValueChanged += new System.EventHandler(this.edgeThickWidth_ValueChanged);
            // 
            // edgeThickHeight
            // 
            this.edgeThickHeight.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.edgeThickHeight.Location = new System.Drawing.Point(260, 129);
            this.edgeThickHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.edgeThickHeight.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.edgeThickHeight.Name = "edgeThickHeight";
            this.edgeThickHeight.Size = new System.Drawing.Size(64, 27);
            this.edgeThickHeight.TabIndex = 7;
            this.edgeThickHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.edgeThickHeight.ValueChanged += new System.EventHandler(this.edgeThickHeight_ValueChanged);
            // 
            // RectCheckerParamControl
            // 
            this.Controls.Add(this.edgeThickHeight);
            this.Controls.Add(this.edgeThickWidth);
            this.Controls.Add(this.labelEdgeThickHeight);
            this.Controls.Add(this.labelEdgeThickWidth);
            this.Controls.Add(this.outToIn);
            this.Controls.Add(this.convexShape);
            this.Controls.Add(this.edgeType);
            this.Controls.Add(this.cardinalPoint);
            this.Controls.Add(this.labelSearchRange);
            this.Controls.Add(this.labelConvexShape);
            this.Controls.Add(this.labelEdgeType);
            this.Controls.Add(this.labelCardinalPoint);
            this.Controls.Add(this.labelSearchLength);
            this.Controls.Add(this.labelPassRate);
            this.Controls.Add(this.labelScan);
            this.Controls.Add(this.labelGrayValue);
            this.Controls.Add(this.searchLength);
            this.Controls.Add(this.passRate);
            this.Controls.Add(this.projectionHeight);
            this.Controls.Add(this.grayValue);
            this.Controls.Add(this.searchRange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "RectCheckerParamControl";
            this.Size = new System.Drawing.Size(337, 336);
            this.Load += new System.EventHandler(this.RectCheckerParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.searchRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grayValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.passRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edgeThickWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edgeThickHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown searchRange;
        private System.Windows.Forms.NumericUpDown grayValue;
        private System.Windows.Forms.NumericUpDown projectionHeight;
        private System.Windows.Forms.Label labelGrayValue;
        private System.Windows.Forms.Label labelEdgeType;
        private System.Windows.Forms.Label labelSearchRange;
        private System.Windows.Forms.Label labelScan;
        private System.Windows.Forms.NumericUpDown passRate;
        private System.Windows.Forms.Label labelPassRate;
        private System.Windows.Forms.Label labelCardinalPoint;
        private System.Windows.Forms.ComboBox cardinalPoint;
        private System.Windows.Forms.CheckBox outToIn;
        private System.Windows.Forms.NumericUpDown searchLength;
        private System.Windows.Forms.Label labelSearchLength;
        private System.Windows.Forms.Label labelConvexShape;
        private System.Windows.Forms.ComboBox convexShape;
        private System.Windows.Forms.ComboBox edgeType;
        private System.Windows.Forms.Label labelEdgeThickWidth;
        private System.Windows.Forms.Label labelEdgeThickHeight;
        private System.Windows.Forms.NumericUpDown edgeThickWidth;
        private System.Windows.Forms.NumericUpDown edgeThickHeight;
    }
}
