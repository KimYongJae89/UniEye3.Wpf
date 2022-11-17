using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DynMvp.UI
{
    public class RotatedRectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(RotatedRect))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(string) &&
                 value is RotatedRect)
            {
                var rotatedRect = (RotatedRect)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(RotatedRectConverter))]
    public struct RotatedRect
    {
        private RectangleF rectangle;

        [BrowsableAttribute(false)]
        public float Left => X;

        [BrowsableAttribute(false)]
        public float Top => Y;

        [BrowsableAttribute(false)]
        public float Right => X + Width;

        [BrowsableAttribute(false)]
        public float Bottom => Y + Height;

        public static readonly RotatedRect Empty = new RotatedRect();

        public Rectangle ToRectangle()
        {
            //if (angle == 90 || angle == 270)
            //{
            //    PointF center = DrawingHelper.CenterPoint(this);
            //    return Rectangle.Round(DrawingHelper.FromCenterSize(center, new SizeF(rectangle.Size.Height, rectangle.Size.Width)));
            //}
            //else return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            return Rectangle.Round(ToRectangleF());
        }

        public RectangleF ToRectangleF()
        {
            //if (angle == 90 || angle == 270)
            //{
            //    PointF center = DrawingHelper.CenterPoint(this);
            //    return DrawingHelper.FromCenterSize(center, new SizeF(rectangle.Size.Height, rectangle.Size.Width));
            //}
            //else
            {
                RectangleF boundRect = GetBoundRect();
                return boundRect;
            }

            // return new RectangleF(X, Y, Width, Height);
        }

        public float X
        {
            get => rectangle.X;
            set => rectangle.X = value;
        }
        public float Y
        {
            get => rectangle.Y;
            set => rectangle.Y = value;
        }
        public float Width
        {
            get => rectangle.Width;
            set => rectangle.Width = value;
        }
        public float Height
        {
            get => rectangle.Height;
            set => rectangle.Height = value;
        }
        public float Angle { get; set; }

        public RotatedRect(RotatedRect srcRect)
        {
            rectangle = srcRect.rectangle;
            Angle = srcRect.Angle;
        }

        public RotatedRect(float x, float y, float width, float height, float angle)
        {
            rectangle = new RectangleF(x, y, width, height);
            Angle = angle;
            if (width < 0 || height < 0)
            {
                return;
            }
        }

        public RotatedRect(PointF point, SizeF size, float angle)
        {
            rectangle = new RectangleF(point, size);
            Angle = angle;
        }

        public RotatedRect(RectangleF rectangle, float angle)
        {
            this.rectangle = rectangle;
            Angle = angle;
        }

        public void FromLTRB(float left, float top, float right, float bottom)
        {
            rectangle = RectangleF.FromLTRB(Math.Min(left, right), Math.Min(top, bottom), Math.Max(left, right), Math.Max(top, bottom));
        }

        public void Offset(float offsetX, float offsetY)
        {
            rectangle.Offset(offsetX, offsetY);
        }

        public void Offset(SizeF offset)
        {
            rectangle.Offset(offset.Width, offset.Height);
        }

        public void Offset(PointF offset)
        {
            rectangle.Offset(offset.X, offset.Y);
        }

        public void Inflate(float x, float y)
        {
            rectangle.Inflate(x, y);
        }

        public void Inflate(float left, float top, float right, float bottom)
        {
            rectangle = RectangleF.FromLTRB(rectangle.Left - left, rectangle.Top - top, rectangle.Right + right, rectangle.Bottom + bottom);
        }

        [BrowsableAttribute(false)]
        public bool IsEmpty => rectangle.IsEmpty;

        public PointF[] GetPoints()
        {
            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            var points = new PointF[4];

            points[0] = new PointF(rectangle.Left, rectangle.Top);
            points[1] = new PointF(rectangle.Right, rectangle.Top);
            points[2] = new PointF(rectangle.Right, rectangle.Bottom);
            points[3] = new PointF(rectangle.Left, rectangle.Bottom);

            for (int j = 0; j < 4; j++)
            {
                points[j] = MathHelper.Rotate(points[j], centerPt, Angle);
            }

            return points;
        }

        // 회전이 고려된 버전으로 Revision 필요
        public static RotatedRect Union(RotatedRect rect1, RotatedRect rect2)
        {
            return new RotatedRect(RectangleF.Union(rect1.GetBoundRect(), rect2.GetBoundRect()), 0);
        }

        //// 회전이 고려된 버전으로 Revision 필요
        //public static RotatedRect Intersect(RotatedRect rect1, RotatedRect rect2)
        //{
        //    return new RotatedRect(RectangleF.Intersect(rect1.GetBoundRect(), rect2.GetBoundRect()), 0);
        //}

        public static RotatedRect Inflate(RotatedRect rect, float x, float y)
        {
            var newRect = RectangleF.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            newRect.Inflate(x, y);

            return new RotatedRect(newRect, rect.Angle);
        }

        public RectangleF GetBoundRect()
        {
            PointF[] points = GetPoints();
            return DrawingHelper.GetBoundRect(points);
        }

        public override string ToString()
        {
            //return rectangle.ToString() + " angle : " + angle.ToString();
            return string.Format("X{0} Y{1} W{2} H{3} A{4}", rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, Angle);
        }

        public override bool Equals(object obj)
        {
            if (obj is RotatedRect rotatedRect)
            {
                bool ok1 = Angle == rotatedRect.Angle;
                bool ok2 = rectangle.Equals(rotatedRect.rectangle);

                return ok1 && ok2;
            }
            return false;
        }

        public static RotatedRect Parse(string rect)
        {
            rect = rect.Trim();
            string[] word = rect.Split(' ');
            if (word.Count() != 5)
            {
                return new RotatedRect(new RectangleF(0, 0, 0, 0), 0);
            }

            float x = Convert.ToSingle(word[0].Substring(1));
            float y = Convert.ToSingle(word[1].Substring(1));
            float w = Convert.ToSingle(word[2].Substring(1));
            float h = Convert.ToSingle(word[3].Substring(1));
            float a = Convert.ToSingle(word[4].Substring(1));
            return new RotatedRect(new RectangleF(x, y, w, h), a);
        }

        public override int GetHashCode()
        {
            int hashCode = 2018075475;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<RectangleF>.Default.GetHashCode(rectangle);
            hashCode = hashCode * -1521134295 + Angle.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(RotatedRect rect1, RotatedRect rect2)
        {
            return rect1.Equals(rect2);
        }

        public static bool operator !=(RotatedRect rect1, RotatedRect rect2)
        {
            return rect1.Equals(rect2) == false;
        }
    }

}
