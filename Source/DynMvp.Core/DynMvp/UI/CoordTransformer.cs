using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.UI
{
    public interface ICoordTransform
    {
        bool InvertY { get; }
        Point Transform(Point pt);
        Point InverseTransform(Point pt);
        PointF Transform(PointF pt);
        PointF InverseTransform(PointF pt);
        Rectangle Transform(Rectangle rect);
        Rectangle InverseTransform(Rectangle rect);
        RotatedRect Transform(RotatedRect rect);
        RotatedRect InverseTransform(RotatedRect rect);
        Size Transform(Size size);
        Size InverseTransform(Size size);
        SizeF Transform(SizeF size);
        SizeF InverseTransform(SizeF size);
    }

    public class CoordTransformer : ICoordTransform
    {
        private RectangleF srcRect;
        private RectangleF displayRect;
        private float drawOffsetX = 0;
        private float drawOffsetY = 0;
        private float scaleX = 1;
        private float scaleY = 1;
        public bool InvertY { get; set; }

        public CoordTransformer()
        {
        }

        public CoordTransformer(float scale)
        {
            if (scale == 0)
            {
                scaleX = scaleY = 1;
            }
            else
            {
                scaleX = scaleY = scale;
            }
        }

        public Point Transform(Point pt)
        {
            var pointF = new PointF(pt.X, pt.Y);
            return Point.Truncate(Transform(pointF));
        }

        public Point InverseTransform(Point pt)
        {
            var newPt = new Point();
            newPt.X = (int)(pt.X - drawOffsetX);
            newPt.X = (int)((newPt.X / scaleX) + srcRect.Left);

            if (InvertY == true)
            {
                newPt.Y = (int)(displayRect.Bottom - (pt.Y - drawOffsetY));
            }
            else
            {
                newPt.Y = (int)(pt.Y - drawOffsetY);
            }

            newPt.Y = (int)((newPt.Y / scaleY) + srcRect.Top);

            return newPt;
        }

        public PointF Transform(PointF pt)
        {
            var newPt = new PointF();
            newPt.X = ((pt.X - srcRect.Left) * scaleX);
            newPt.X = (newPt.X + drawOffsetX);

            newPt.Y = ((pt.Y - srcRect.Top) * scaleY);
            //if (invertY == true)
            //    newPt.Y = (displayRect.Bottom - newPt.Y) + drawOffsetY;
            //else
            //    newPt.Y = newPt.Y + drawOffsetY;

            if (InvertY == true)
            {
                newPt.Y = ((srcRect.Bottom - pt.Y) * scaleY);
            }
            else
            {
                newPt.Y = ((pt.Y - srcRect.Top) * scaleY);
            }

            newPt.Y = newPt.Y + drawOffsetY;

            return newPt;
        }

        public PointF InverseTransform(PointF pt)
        {
            var newPt = new PointF();
            newPt.X = (pt.X - drawOffsetX);
            newPt.X = (newPt.X / scaleX) + srcRect.Left;

            //if (invertY == true)
            //    newPt.Y = displayRect.Bottom - (pt.Y - drawOffsetY);
            //else
            //    newPt.Y = pt.Y - drawOffsetY;
            //newPt.Y = (newPt.Y / scaleY) + srcRect.Top;


            newPt.Y = (pt.Y - drawOffsetY);

            if (InvertY == true)
            {
                newPt.Y = -(pt.Y / scaleY) + srcRect.Bottom;
            }
            else
            {
                newPt.Y = (pt.Y / scaleY) + srcRect.Top;
            }

            return newPt;
        }

        public Rectangle Transform(Rectangle rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return Transform(rotatedRect).ToRectangle();
        }

        public Rectangle InverseTransform(Rectangle rect)
        {
            var rotatedRect = new RotatedRect(rect, 0);
            return InverseTransform(rotatedRect).ToRectangle();

        }

        public RotatedRect Transform(RotatedRect rect)
        {
            var newRect = new RotatedRect();
            newRect.X = ((rect.X - srcRect.Left) * scaleX);
            newRect.X = (newRect.X + drawOffsetX);

            if (InvertY == true)
            {
                newRect.Y = ((srcRect.Bottom - rect.Bottom) * scaleY);
            }
            else
            {
                newRect.Y = ((rect.Y - srcRect.Top) * scaleY);
            }

            newRect.Y = newRect.Y + drawOffsetY;
            newRect.Width = (rect.Width * scaleX);
            newRect.Height = (rect.Height * scaleY);
            newRect.Angle = rect.Angle;

            return newRect;
        }

        public RotatedRect InverseTransform(RotatedRect rect)
        {
            var newRect = new RotatedRect();

            newRect.Width = (rect.Width / scaleX);
            newRect.Height = (rect.Height / scaleY);

            newRect.X = (rect.X - drawOffsetX);
            newRect.X = (newRect.X / scaleX) + srcRect.Left;

            if (InvertY == true)
            {
                newRect.Y = (rect.Bottom - drawOffsetY);
                newRect.Y = -(newRect.Y / scaleY) + srcRect.Bottom;
            }
            else
            {
                newRect.Y = (rect.Top - drawOffsetY);
                newRect.Y = (newRect.Y / scaleY) + srcRect.Top;
            }

            newRect.Angle = rect.Angle;

            return newRect;
        }

        public Size Transform(Size size)
        {
            var newSize = new Size();
            newSize.Width = (int)(size.Width * scaleX);
            newSize.Height = (int)(size.Height * scaleY);

            return newSize;
        }

        public Size InverseTransform(Size size)
        {
            var newSize = new Size();
            newSize.Width = (int)(size.Width / scaleX);
            newSize.Height = (int)(size.Height / scaleY); // 초기에 상하가 반전이 되어있었음

            //if (invertY == true)
            //    newSize.Height *= -1;

            return newSize;
        }

        public SizeF Transform(SizeF size)
        {
            var newSize = new SizeF();
            newSize.Width = size.Width * scaleX;
            newSize.Height = size.Height * scaleY;

            return newSize;
        }

        public SizeF InverseTransform(SizeF size)
        {
            var newSize = new SizeF();
            newSize.Width = size.Width / scaleX;
            newSize.Height = size.Height / scaleY;

            return newSize;
        }

        public void UpdateScale()
        {
            if (srcRect.IsEmpty == true || displayRect.IsEmpty == true)
            {
                return;
            }

            float srcAspectRatio = srcRect.Width / srcRect.Height;
            float displayAspectRatio = displayRect.Width / displayRect.Height;

            if (srcAspectRatio > displayAspectRatio)
            {
                scaleX = scaleY = displayRect.Width / srcRect.Width;
                float srcScaledHeight = srcRect.Height * scaleY;
                drawOffsetY = (displayRect.Height - srcScaledHeight) / 2;
                //drawOffsetY = 0;
                //drawOffsetY = (srcRect.Top - displayRect.Top);
            }
            else
            {
                scaleX = scaleY = displayRect.Height / srcRect.Height;
                float srcScaledWidth = srcRect.Width * scaleX;
                drawOffsetX = (displayRect.Width - srcScaledWidth) / 2;
                //drawOffsetX = 0;
                //drawOffsetX = displayRect.Left-srcRect.Left;
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void SetSrcRect(RectangleF srcRect)
        {
            this.srcRect = srcRect;
            UpdateScale();
        }

        public void SetDisplayRect(RectangleF displayRect)
        {
            this.displayRect = displayRect;
            UpdateScale();
        }
    }
}
