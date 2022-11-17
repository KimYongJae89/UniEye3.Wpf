using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public class CoordMapper : ICoordTransform
    {
        public Matrix Matrix { get; }
        public Matrix InvMatrix { get; }
        public bool InvertY { get; } = false;

        public CoordMapper()
        {
            Matrix = new Matrix();

            InvMatrix = Matrix.Clone();
            InvMatrix.Invert();
        }

        public CoordMapper(Matrix matrix)
        {
            Matrix = matrix;

            InvMatrix = matrix.Clone();
            InvMatrix.Invert();
        }

        ~CoordMapper()
        {
            if (Matrix != null)
            {
                Matrix.Dispose();
            }
        }

        public Point WorldToPixel(Point pt)
        {
            return Point.Round(WorldToPixel(new PointF(pt.X, pt.Y)));
        }

        public Point PixelToWorld(Point pt)
        {
            return Point.Round(PixelToWorld(new PointF(pt.X, pt.Y)));
        }

        public PointF WorldToPixel(PointF pt)
        {
            var ptArr = new PointF[] { pt };
            Matrix.TransformPoints(ptArr);

            return ptArr[0];
        }

        public PointF PixelToWorld(PointF pt)
        {
            var ptArr = new PointF[] { pt };
            InvMatrix.TransformPoints(ptArr);

            return ptArr[0];
        }

        public Rectangle WorldToPixel(Rectangle rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return WorldToPixel(rotatedRect).ToRectangle();
        }

        public Rectangle PixelToWorld(Rectangle rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return PixelToWorld(rotatedRect).ToRectangle();
        }

        public RectangleF WorldToPixel(RectangleF rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return WorldToPixel(rotatedRect).ToRectangleF();
        }

        public RectangleF PixelToWorld(RectangleF rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return PixelToWorld(rotatedRect).ToRectangleF();
        }

        public RotatedRect WorldToPixel(RotatedRect rect)
        {
            PointF topLeftPt = WorldToPixel(new PointF(rect.X, rect.Y));
            SizeF size = WorldToPixel(new SizeF(rect.Width, rect.Height));

            return new RotatedRect(topLeftPt, size, rect.Angle);
        }

        public RotatedRect PixelToWorld(RotatedRect rect)
        {
            PointF topLeftPt = PixelToWorld(new PointF(rect.X, rect.Y));
            SizeF size = PixelToWorld(new SizeF(rect.Width, rect.Height));

            return new RotatedRect(topLeftPt, size, rect.Angle);
        }

        public Size WorldToPixel(Size size)
        {
            var ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            Matrix.TransformVectors(ptArr);

            return DrawingHelper.ToSize(ptArr[0]);
        }

        public Size PixelToWorld(Size size)
        {
            var ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            InvMatrix.TransformVectors(ptArr);

            return DrawingHelper.ToSize(ptArr[0]);
        }

        public SizeF WorldToPixel(SizeF size)
        {
            var ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            Matrix.TransformVectors(ptArr);

            return DrawingHelper.ToSizeF(ptArr[0]);
        }

        public SizeF PixelToWorld(SizeF size)
        {
            var ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            InvMatrix.TransformVectors(ptArr);

            return DrawingHelper.ToSizeF(ptArr[0]);
        }

        public Point Transform(Point pt)
        {
            return WorldToPixel(pt);
        }

        public Point InverseTransform(Point pt)
        {
            return PixelToWorld(pt);
        }

        public PointF Transform(PointF pt)
        {
            return WorldToPixel(pt);
        }

        public PointF InverseTransform(PointF pt)
        {
            return PixelToWorld(pt);
        }

        public Rectangle Transform(Rectangle rect)
        {
            return WorldToPixel(rect);
        }

        public Rectangle InverseTransform(Rectangle rect)
        {
            return PixelToWorld(rect);
        }

        public RotatedRect Transform(RotatedRect rect)
        {
            return WorldToPixel(rect);
        }

        public RotatedRect InverseTransform(RotatedRect rect)
        {
            return PixelToWorld(rect);
        }

        public Size Transform(Size size)
        {
            return WorldToPixel(size);
        }

        public Size InverseTransform(Size size)
        {
            return PixelToWorld(size);
        }

        public SizeF Transform(SizeF size)
        {
            return WorldToPixel(size);
        }

        public SizeF InverseTransform(SizeF size)
        {
            return PixelToWorld(size);
        }
    }
}
