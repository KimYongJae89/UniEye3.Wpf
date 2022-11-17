using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.UI
{
    public enum ProjectionGraphDirection
    {
        xProjection, yProjection
    }

    public partial class ProjectionGraphControl : UserControl
    {
        private float[] data;
        public float[] Data
        {
            get => data;
            set
            {
                data = value;

                if (data != null && MinValue == 0 && MaxValue == 0)
                {
                    MinValue = data.Min();
                    MaxValue = data.Max();
                }
            }
        }

        private float[] data2;
        public float[] Data2
        {
            get => data2;
            set
            {
                data2 = value;

                if (data2 != null && MinValue2 == 0 && MaxValue2 == 0)
                {
                    MinValue2 = data2.Min();
                    MaxValue2 = data2.Max();
                }
            }
        }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float MinValue2 { get; set; }
        public float MaxValue2 { get; set; }
        public ProjectionGraphDirection Direction { get; set; }

        public ProjectionGraphControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (data == null)
            {
                return;
            }

            Graphics g = e.Graphics;

            if (Direction == ProjectionGraphDirection.xProjection)
            {
                DrawXProjection(g);
            }
            else
            {
                DrawYProjection(g);
            }

            base.OnPaint(e);
        }

        private void DrawXProjection(Graphics g)
        {
            float scaleX = ((float)Size.Width) / (data.Length - 1);
            float scaleY = Size.Height / (MaxValue - MinValue);

            for (int i = 0; i < data.Length - 1; i++)
            {
                g.DrawLine(new Pen(Color.Red), new PointF(i * scaleX, data[i] * scaleY - MinValue), new PointF((i + 1) * scaleX, data[i + 1] * scaleY - MinValue));
            }

            if (data2 != null)
            {
                float scaleX2 = ((float)Size.Width) / (data2.Length - 1);
                float scaleY2 = Size.Height / (MaxValue2 - MinValue2);

                for (int i = 0; i < data2.Length - 1; i++)
                {
                    g.DrawLine(new Pen(Color.Blue), new PointF(i * scaleX2, data2[i] * scaleY2 - MinValue2), new PointF((i + 1) * scaleX2, data2[i + 1] * scaleY2 - MinValue2));
                }
            }
        }

        private void DrawYProjection(Graphics g)
        {
            float scaleX = Size.Width / (MaxValue - MinValue);
            float scaleY = ((float)Size.Height) / (data.Length - 1);

            for (int i = 0; i < data.Length - 1; i++)
            {
                g.DrawLine(new Pen(Color.Red), new PointF(data[i] * scaleX - MinValue, i * scaleY), new PointF(data[i + 1] * scaleX - MinValue, (i + 1) * scaleY));
            }

            if (data2 != null)
            {
                float scaleX2 = Size.Width / (MaxValue2 - MinValue2);
                float scaleY2 = ((float)Size.Height) / (data2.Length - 1);

                for (int i = 0; i < data2.Length - 1; i++)
                {
                    g.DrawLine(new Pen(Color.Blue), new PointF(data2[i] * scaleX2 - MinValue2, i * scaleY2), new PointF(data2[i + 1] * scaleX2 - MinValue2, (i + 1) * scaleY2));
                }
            }

        }

        private void ProjectionGraphControl_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ProjectionGraphControl_Load(object sender, EventArgs e)
        {

        }

        private void ProjectionGraphControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var histogram = new Histogram();

            if (e.X < Size.Width / 2)
            {
                histogram.Create(data, 10, 2);
            }
            else
            {
                histogram.Create(data2, 10, 10);
            }

            var histogramView = new HistogramView();
            histogramView.Histogram = histogram;
            histogramView.Show();
        }
    }
}
