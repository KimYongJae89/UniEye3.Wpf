using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class DrawingHelper
    {
        public static bool IsValid(Rectangle rectangle, Size srcImageSize)
        {
            if (rectangle.Left < 0 || rectangle.Top < 0)
            {
                return false;
            }

            if (rectangle.Right >= srcImageSize.Width || rectangle.Bottom >= srcImageSize.Height)
            {
                return false;
            }

            return true;
        }

        public static void Arrange(Rectangle rectangle, Size srcImageSize)
        {
            if (rectangle.X < 0)
            {
                rectangle.X = 0;
            }

            if (rectangle.Y < 0)
            {
                rectangle.Y = 0;
            }

            if ((rectangle.X + rectangle.Width) > srcImageSize.Width)
            {
                rectangle.Width = srcImageSize.Width - rectangle.X;
            }

            if ((rectangle.Y + rectangle.Height) > srcImageSize.Height)
            {
                rectangle.Height = srcImageSize.Height - rectangle.Y;
            }
        }

        public static PointF CenterPoint(PointF[] pointArray)
        {
            return new PointF(pointArray.Average(x => x.X), pointArray.Average(y => y.Y));
        }

        public static Point CenterPoint(Rectangle rectangle)
        {
            var centerPt = new Point()
            {
                X = rectangle.X + rectangle.Width / 2,
                Y = rectangle.Y + rectangle.Height / 2
            };

            return centerPt;
        }

        public static PointF CenterPoint(RectangleF rectangle)
        {
            var centerPt = new PointF();

            centerPt.X = rectangle.X + rectangle.Width / 2;
            centerPt.Y = rectangle.Y + rectangle.Height / 2;

            return centerPt;
        }

        public static PointF CenterPoint(RotatedRect rectangle)
        {
            var centerPt = new PointF();

            centerPt.X = rectangle.X + rectangle.Width / 2;
            centerPt.Y = rectangle.Y + rectangle.Height / 2;

            return centerPt;
        }

        public static PointF CenterPoint(PointF pt1, PointF pt2)
        {
            var centerPt = new PointF();

            centerPt.X = (pt1.X + pt2.X) / 2;
            centerPt.Y = (pt1.Y + pt2.Y) / 2;

            return centerPt;
        }

        public static Point ToPoint(PointF pointF)
        {
            return new Point((int)pointF.X, (int)pointF.Y);
        }

        public static PointF ToPointF(Point point)
        {
            return new PointF(point.X, point.Y);
        }

        public static PointF ToPointF(SizeF size)
        {
            return new PointF(size.Width, size.Height);
        }

        public static SizeF ToSizeF(PointF point)
        {
            return new SizeF(point.X, point.Y);
        }

        public static SizeF ToSizeF(Point point)
        {
            return new SizeF(point.X, point.Y);
        }

        public static Size ToSize(PointF point)
        {
            return new Size((int)point.X, (int)point.Y);
        }

        public static Size ToSize(Point point)
        {
            return new Size(point.X, point.Y);
        }

        public static Rectangle ToRect(RectangleF rectangleF)
        {
            return new Rectangle((int)rectangleF.Left, (int)rectangleF.Top, (int)rectangleF.Width, (int)rectangleF.Height);
        }

        public static RectangleF ToRectF(Rectangle rectangle)
        {
            return new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        public static RectangleF FromPoints(PointF point1, PointF point2)
        {
            return FromPoints(point1.X, point1.Y, point2.X, point2.Y);
        }

        public static RectangleF FromPoints(float x1, float y1, float x2, float y2)
        {
            float left = Math.Min(x1, x2);
            float right = Math.Max(x1, x2);
            float top = Math.Min(y1, y2);
            float bottom = Math.Max(y1, y2);

            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        public static RectangleF FromCenterSize(PointF centerPt, SizeF size)
        {
            return new RectangleF(centerPt.X - size.Width / 2, centerPt.Y - size.Height / 2, size.Width, size.Height);
        }

        public static Rectangle FromCenterSize(Point centerPt, Size size)
        {
            return new Rectangle(centerPt.X - (size.Width / 2), centerPt.Y - (size.Height / 2), size.Width, size.Height);
        }

        public static PointF[] GetPoints(RectangleF rectangle, float angle = 0)
        {
            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            var points = new PointF[4];

            points[0] = new PointF(rectangle.Left, rectangle.Top);
            points[1] = new PointF(rectangle.Right, rectangle.Top);
            points[2] = new PointF(rectangle.Right, rectangle.Bottom);
            points[3] = new PointF(rectangle.Left, rectangle.Bottom);

            for (int j = 0; j < 4; j++)
            {
                points[j] = MathHelper.Rotate(points[j], centerPt, angle);
            }

            return points;
        }

        public static RectangleF GetBoundRect(PointF[] points)
        {
            float left = points.Min(x => x.X);
            float right = points.Max(x => x.X);
            float top = points.Min(x => x.Y);
            float bottom = points.Max(x => x.Y);

            var rectangleF = RectangleF.FromLTRB(left, top, right, bottom);
            return rectangleF;
        }

        public static SizeF Subtract(SizeF pt1, SizeF pt2)
        {
            return new SizeF(pt1.Width - pt2.Width, pt1.Height - pt2.Height);
        }

        public static void DrawGrid(RotatedRect region, int rowCount, int columnCount, FigureGroup tempFigures, Pen pen)
        {
            float widthStep = region.Width / columnCount;
            float heightStep = region.Height / rowCount;

            for (float i = widthStep; i < region.Width; i += widthStep)
            {
                tempFigures.AddFigure(new LineFigure(new PointF(region.X + i, region.Top), new PointF(region.X + i, region.Bottom), pen));
            }

            for (float i = heightStep; i < region.Height; i += heightStep)
            {
                tempFigures.AddFigure(new LineFigure(new PointF(region.Left, region.Y + i), new PointF(region.Right, region.Y + i), pen));
            }
        }

        public static SizeF Add(SizeF pt1, SizeF pt2)
        {
            return new SizeF(pt1.Width + pt2.Width, pt1.Height + pt2.Height);
        }

        public static RectangleF Offset(RectangleF rect, SizeF offset)
        {
            return new RectangleF(rect.X + offset.Width, rect.Y + offset.Height, rect.Width, rect.Height);
        }

        public static PointF[] Offset(PointF[] ptArray, SizeF offset)
        {
            var ptList = new List<PointF>();

            foreach (PointF pt in ptArray)
            {
                ptList.Add(new PointF(pt.X + offset.Width, pt.Y + offset.Height));
            }

            return ptList.ToArray();
        }

        public static PointF Subtract(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static PointF Add(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static List<PointF> ClipToFov(RotatedRect clipRegion, List<PointF> pointList)
        {
            var newPointList = new List<PointF>();
            foreach (PointF point in pointList)
            {
                newPointList.Add(ClipToFov(clipRegion, point));
            }

            return newPointList;
        }

        public static PointF ClipToFov(RotatedRect clipRegion, PointF point)
        {
            var regionCenter = new PointF(clipRegion.Width / 2, clipRegion.Height / 2);
            PointF fovPoint = MathHelper.Rotate(point, regionCenter, clipRegion.Angle);
            fovPoint = PointF.Subtract(fovPoint, new SizeF(clipRegion.Width / 2, clipRegion.Height / 2));

            PointF regionFovCenter = DrawingHelper.CenterPoint(clipRegion);
            return PointF.Add(fovPoint, new SizeF(regionFovCenter.X, regionFovCenter.Y));
        }

        public static RotatedRect ClipToFov(RotatedRect clipRegion, RectangleF rectangle)
        {
            PointF rectCenter = ClipToFov(clipRegion, DrawingHelper.CenterPoint(rectangle));
            RectangleF clipRect = DrawingHelper.FromCenterSize(rectCenter, new SizeF(rectangle.Width, rectangle.Height));
            return new RotatedRect(clipRect, clipRegion.Angle);
        }

        public static PointF FovToClip(RotatedRect clipRegion, PointF point)
        {
            PointF regionFovCenter = DrawingHelper.CenterPoint(clipRegion);
            var clipPos = PointF.Subtract(point, new SizeF(regionFovCenter.X, regionFovCenter.Y));

            var regionCenter = new PointF(clipRegion.Width / 2, clipRegion.Height / 2);
            return MathHelper.Rotate(clipPos, regionCenter, -clipRegion.Angle);
        }

        public static RectangleF FovToClip(RotatedRect clipRegion, RotatedRect rectangle)
        {
            PointF rectCenter = FovToClip(clipRegion, DrawingHelper.CenterPoint(rectangle));
            return new RectangleF(rectCenter, new SizeF(rectangle.Width, rectangle.Height));
        }

        public static RectangleF GetUnionRect(RectangleF rectangle1, RectangleF rectangle2)
        {
            if (rectangle1.IsEmpty && rectangle2.IsEmpty)
            {
                return new RectangleF();
            }
            else if (rectangle1.IsEmpty)
            {
                return rectangle2;
            }
            else if (rectangle2.IsEmpty)
            {
                return rectangle1;
            }

            return RectangleF.Union(rectangle1, rectangle2);
        }

        public static bool IsCross(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            PointF intersectionPoint = PointF.Empty;
            if (FindIntersection(point1, point2, point3, point4, ref intersectionPoint) == true)
            {
                float minX1 = Math.Min(point1.X, point2.X);
                float maxX1 = Math.Max(point1.X, point2.X);
                float minY1 = Math.Min(point1.Y, point2.Y);
                float maxY1 = Math.Max(point1.Y, point2.Y);

                float minX2 = Math.Min(point3.X, point4.X);
                float maxX2 = Math.Max(point3.X, point4.X);
                float minY2 = Math.Min(point3.Y, point4.Y);
                float maxY2 = Math.Max(point3.Y, point4.Y);

                float minX = Math.Max(minX1, minX2);
                float maxX = Math.Min(maxX1, maxX2);
                float minY = Math.Max(minY1, minY2);
                float maxY = Math.Min(maxY1, maxY2);

                if (minX <= intersectionPoint.X && maxX >= intersectionPoint.X && minY <= intersectionPoint.Y && maxY >= intersectionPoint.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool FindIntersection(PointF point1, PointF point2, PointF point3, PointF point4, ref PointF intersectionPoint)
        {
            if (point1.X == point2.X)
            {
                if (point3.X == point4.X)
                {
                    if (point1.X == point3.X)
                    {
                        intersectionPoint = new PointF(point1.X, 0);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    float angle = (point4.Y - point3.Y) / (point4.X - point3.X);
                    intersectionPoint = new PointF(point1.X, angle * (point1.X - point3.X) + point3.Y);
                    return true;
                }
            }
            else if (point3.X == point4.X)
            {
                float angle = (point2.Y - point1.Y) / (point2.X - point1.X);
                intersectionPoint = new PointF(point3.X, angle * (point3.X - point1.X) + point1.Y);
                return true;
            }
            else if (point1.Y == point2.Y)
            {
                if (point3.Y == point4.Y)
                {
                    if (point1.Y == point3.Y)
                    {
                        intersectionPoint = new PointF(0, point1.Y);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    float angle = (point4.Y - point3.Y) / (point4.X - point3.X);
                    intersectionPoint = new PointF((point1.Y - point3.Y) / angle + point3.X, point1.Y);
                    return true;
                }
            }
            else if (point3.Y == point4.Y)
            {
                float angle = (point2.Y - point1.Y) / (point2.X - point1.X);
                intersectionPoint = new PointF((point3.Y - point1.Y) / angle + point1.X, point3.Y);
                return true;
            }

            double den = (point1.X - point2.X) * (point3.Y - point4.Y) - (point1.Y - point2.Y) * (point3.X - point4.X);

            double num1 = (point1.X * point2.Y - point1.Y * point2.X);
            double num2 = (point3.X * point4.Y - point3.Y * point4.X);

            double numX = num1 * (point3.X - point4.X) - (point1.X - point2.X) * num2;
            double numY = num1 * (point3.Y - point4.Y) - (point1.Y - point2.Y) * num2;

            intersectionPoint = new PointF((float)(numX / den), (float)(numY / den));

            return true;
        }
    }
}
