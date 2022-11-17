using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Data.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.UI
{
    public partial class CircleListForm : Form
    {
        public Point3d[] PointArray { get; } = new Point3d[4];

        private DrawBox drawBox;
        public CircleListForm()
        {
            InitializeComponent();
            buttonSetPosition1.Text = StringManager.GetString(buttonSetPosition1.Text);
            buttonSetPosition2.Text = StringManager.GetString(buttonSetPosition2.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);


            drawBox = new DrawBox();

            panelImage.Controls.Add(drawBox);

            drawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            drawBox.Location = new System.Drawing.Point(246, 0);
            drawBox.Name = "DrawBox";
            drawBox.Size = new System.Drawing.Size(511, 507);
            drawBox.TabIndex = 1;
            drawBox.TabStop = false;
            drawBox.Enable = true;

        }

        public void Initialize(List<Point3d> pointList, Image2D image, List<PointF> foundPxPointList)
        {
            int index = 1;
            foreach (Point3d point in pointList)
            {
                circleListGrid.Rows.Add(index.ToString(), point.X, point.Y, point.Z);
                index++;
            }

            var figureGroup = new FigureGroup();
            foreach (PointF point in foundPxPointList)
            {
                figureGroup.AddFigure(new CrossFigure(point, 5, new Pen(Color.Red)));
            }

            drawBox.UpdateImage(image.ToBitmap());
            drawBox.BackgroundFigures = figureGroup;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int rowIdx = 0; rowIdx < circleListGrid.Rows.Count; rowIdx++)
            {
                int index = Convert.ToInt32(circleListGrid.Rows[rowIdx].Cells[0].Value.ToString()) - 1;
                float x = Convert.ToSingle(circleListGrid.Rows[rowIdx].Cells[1].Value.ToString());
                float y = Convert.ToSingle(circleListGrid.Rows[rowIdx].Cells[2].Value.ToString());
                float z = Convert.ToSingle(circleListGrid.Rows[rowIdx].Cells[3].Value.ToString());

                if (PointArray[index] == null)
                {
                    PointArray[index] = new Point3d();
                }

                PointArray[index].X = x;
                PointArray[index].Y = y;
                PointArray[index].Z = z;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void buttonSetPosition1_Click(object sender, EventArgs e)
        {
            circleListGrid.Rows[0].Cells[0].Value = "2";
            circleListGrid.Rows[1].Cells[0].Value = "1";
            circleListGrid.Rows[2].Cells[0].Value = "3";
            circleListGrid.Rows[3].Cells[0].Value = "4";
        }

        private void buttonSetPosition2_Click(object sender, EventArgs e)
        {
            circleListGrid.Rows[0].Cells[0].Value = "4";
            circleListGrid.Rows[1].Cells[0].Value = "3";
            circleListGrid.Rows[2].Cells[0].Value = "1";
            circleListGrid.Rows[3].Cells[0].Value = "2";
        }

        private void circleListGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
