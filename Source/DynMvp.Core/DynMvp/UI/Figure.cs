using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.UI
{
    public enum FigureType
    {
        None = -1, Group, Grid, Rectangle, Line, Ellipse, Oblong, Polygon, Text, Image, Cross, XRect, Custom
    }

    public class FigureFactory
    {
        public static Figure Create(string figureTypeName)
        {
            var figureType = (FigureType)Enum.Parse(typeof(FigureType), figureTypeName);
            return Create(figureType);
        }

        public static Figure Create(FigureType figureType)
        {
            Figure figure;
            switch (figureType)
            {
                case FigureType.Rectangle:
                    figure = new RectangleFigure();
                    break;
                case FigureType.Text:
                    figure = new TextFigure();
                    break;
                case FigureType.Image:
                    figure = new ImageFigure();
                    break;
                case FigureType.Ellipse:
                    figure = new EllipseFigure();
                    break;
                case FigureType.Line:
                    figure = new LineFigure();
                    break;
                case FigureType.Group:
                    figure = new FigureGroup();
                    break;
                case FigureType.Cross:
                    figure = new CrossFigure();
                    break;
                case FigureType.Polygon:
                    figure = new PolygonFigure();
                    break;
                case FigureType.Oblong:
                    figure = new OblongFigure();
                    break;
                default:
                    throw new InvalidTypeException();
            }

            return figure;
        }
    }

    public interface ITrackRegion
    {
        TrackPos TrackPos { get; }

        void AddGraphic(GraphicsPath gp);
    }

    public class TrackRectangle : ITrackRegion
    {
        private Rectangle rectangle;

        public TrackPos TrackPos { get; } = new TrackPos();

        public TrackRectangle(int X, int Y, int width, int height, TrackPosType trackPosType)
        {
            rectangle = new Rectangle(X, Y, width, height);
            TrackPos.PosType = trackPosType;
        }

        public TrackRectangle(float X, float Y, float width, float height, TrackPosType trackPosType)
        {
            rectangle = new Rectangle((int)X, (int)Y, (int)width, (int)height);
            TrackPos.PosType = trackPosType;
        }

        public TrackRectangle(int X, int Y, int width, int height, TrackPosType trackPosType, int index)
        {
            rectangle = new Rectangle(X, Y, width, height);
            TrackPos.PosType = trackPosType;
            TrackPos.PolygonIndex = index;
        }

        public TrackRectangle(float X, float Y, float width, float height, TrackPosType trackPosType, int index)
        {
            rectangle = new Rectangle((int)X, (int)Y, (int)width, (int)height);
            TrackPos.PosType = trackPosType;
            TrackPos.PolygonIndex = index;
        }

        public void AddGraphic(GraphicsPath gp)
        {
            gp.AddRectangle(rectangle);
        }
    }

    public class TrackPolygon : ITrackRegion
    {
        public List<PointF> PointList { get; set; } = new List<PointF>();
        public TrackPos TrackPos { get; } = new TrackPos();
        public TrackPolygon(List<PointF> pointList, TrackPosType trackPosType, int index)
        {
            TrackPos.PosType = trackPosType;
            TrackPos.PolygonIndex = index;
            PointList = pointList;
        }

        public void AddGraphic(GraphicsPath gp)
        {
            if (PointList.Count() > 0)
            {
                gp.AddPolygon(PointList.ToArray());
            }
        }
    }

    public abstract class Figure : ICloneable
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public object Tag { get; set; }
        public int ObjectLevel { get; set; } = 0;

        protected FigureType type;
        public FigureType Type => type;
        public bool Selectable { get; set; } = true;
        public bool Deletable { get; set; } = true;
        public bool Movable { get; set; } = true;
        public bool Resizable { get; set; } = true;
        public bool Visible { get; set; } = true;

        protected FigureProperty figureProperty;
        public FigureProperty FigureProperty
        {
            get => figureProperty;
            set => figureProperty = value;
        }

        protected Brush tempBrush;
        public virtual Brush TempBrush
        {
            get => tempBrush;
            set => tempBrush = value;
        }
        public CombineMode CombineMode { get; set; }

        public Figure(string figurePropertyName = "")
        {
            if (string.IsNullOrEmpty(figurePropertyName) == true)
            {
                figureProperty = new FigureProperty();
            }
            else
            {
                figureProperty = FigurePropertyPool.Instance().GetFigureProperty(figurePropertyName);
            }
        }

        public Figure(FigureProperty figureProperty)
        {
            this.figureProperty = figureProperty;
        }

        public Figure(Pen pen, Brush brush)
        {
            figureProperty = new FigureProperty();
            figureProperty.Pen = pen;
            figureProperty.Brush = brush;
        }

        public Figure(Font font, Color textColor, StringAlignment stringAlignment)
        {
            figureProperty = new FigureProperty();
            figureProperty.Font = font;
            figureProperty.TextColor = textColor;
            figureProperty.Alignment = stringAlignment;
        }

        public abstract object Clone();

        public virtual void Copy(Figure srcFigure)
        {
            Id = srcFigure.Id;
            Name = srcFigure.Name;
            Tag = srcFigure.Tag;
            Visible = srcFigure.Visible;

            if (string.IsNullOrEmpty(figureProperty.Name) == true)
            {
                figureProperty = srcFigure.figureProperty.Clone();
            }
            else
            {
                figureProperty = srcFigure.figureProperty;
            }

            Movable = srcFigure.Movable;
            Selectable = srcFigure.Selectable;
        }

        public virtual void ResetTempProperty()
        {
            if (tempBrush != null)
            {
                tempBrush.Dispose();
                tempBrush = null;
            }
        }

        public virtual void Load(XmlElement figureElement)
        {
            Id = XmlHelper.GetValue(figureElement, "Id", "");
            Tag = XmlHelper.GetValue(figureElement, "Tag", "");
            Visible = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Visible", "True"));
            Selectable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Selectable", "True"));
            Movable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Movable", "True"));
            Resizable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Resizable", "True"));
            Name = XmlHelper.GetValue(figureElement, "Name", null);

            string figurePropertyName = XmlHelper.GetValue(figureElement, "FigurePropertyName", "");
            if (string.IsNullOrEmpty(figurePropertyName) == true)
            {
                XmlElement figurePropertyElement = figureElement["FigureProperty"];
                if (figurePropertyElement != null)
                {
                    figureProperty = new FigureProperty();
                    figureProperty.Load(figurePropertyElement);
                }
            }
            else
            {
                figureProperty = FigurePropertyPool.Instance().GetFigureProperty(figurePropertyName);
            }
        }

        public virtual void Save(XmlElement figureElement)
        {
            XmlHelper.SetValue(figureElement, "Id", Id);

            if (Tag != null)
            {
                if (Tag is string tagStr)
                {
                    XmlHelper.SetValue(figureElement, "Tag", tagStr);
                }
            }
            XmlHelper.SetValue(figureElement, "Type", type.ToString());
            XmlHelper.SetValue(figureElement, "Visible", Visible.ToString());
            XmlHelper.SetValue(figureElement, "Selectable", Selectable.ToString());
            XmlHelper.SetValue(figureElement, "Movable", Movable.ToString());
            XmlHelper.SetValue(figureElement, "Resizable", Resizable.ToString());
            XmlHelper.SetValue(figureElement, "Name", Name);

            if (string.IsNullOrEmpty(figureProperty.Name) == true)
            {
                XmlElement figurePropertyElement = figureElement.OwnerDocument.CreateElement("", "FigureProperty", "");
                figureElement.AppendChild(figurePropertyElement);

                figureProperty.Save(figurePropertyElement);

            }
            else
            {
                XmlHelper.SetValue(figureElement, "FigurePropertyName", figureProperty.Name);
            }
        }

        public virtual List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = BaseConfig.Instance().TrackerSize / 2;
            int trackerSize = BaseConfig.Instance().TrackerSize;

            RotatedRect rect = GetRectangle();
            if (coordTransformer != null)
            {
                rect = coordTransformer.Transform(rect);
            }

            var centerPt = DrawingHelper.ToPoint(DrawingHelper.CenterPoint(rect));

            var rectangleList = new List<ITrackRegion>();

            if (Tag is ITrackTarget trackTarget)
            {
                if (trackTarget.IsSizable())
                {
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                }

                if (trackTarget.IsRotatable() || rotationLocked == false)
                {
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, centerPt.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Rotate));
                }

                if (trackTarget.IsContainer())
                {
                    rectangleList.Add(new TrackRectangle(rect.Left + trackerSize * 2, rect.Top - trackerHalfSize, trackerSize * 2, trackerSize * 2, TrackPosType.Move));
                }
                else
                {
                    rectangleList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
                }
            }
            else
            {
                if (Resizable == true)
                {
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                }
                else
                {

                }

                if (rotationLocked == false)
                {
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, centerPt.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Rotate));
                }

                rectangleList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
            }

            return rectangleList;
        }

        public TrackPos GetTrackPos(PointF point, ICoordTransform coordTransformer, bool rotationLocked, ref int polygonIndex)
        {
            RotatedRect transformRect = GetRectangle();
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(transformRect);
            }

            List<ITrackRegion> trackerRegionList = GetTrackerRegionList(coordTransformer, rotationLocked);

            polygonIndex = 0;

            foreach (ITrackRegion trackerRegion in trackerRegionList)
            {
                var gp = new GraphicsPath();

                trackerRegion.AddGraphic(gp);

                var rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
                rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
                gp.Transform(rotationTransform);

                if (gp.IsVisible(point))
                {
                    return trackerRegion.TrackPos;
                }
            }

            // Line 일 경우, Outline을 찾는다.
            if (PtInOutline(point, coordTransformer) == true)
            {
                return new TrackPos(TrackPosType.Inner, 0);
            }

            return new TrackPos(TrackPosType.None, 0);
        }

        public virtual RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            RotatedRect rectangle = GetRectangle();

            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            SizeF floatOffset = MathHelper.Rotate(offset, rectangle.Angle);
            SizeF floatOffset2 = MathHelper.Rotate(offset, -rectangle.Angle);

            //Debug.WriteLine(StringManager.GetString("float offset : ") + floatOffset.ToString());
            //Debug.Flush();

            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.Left:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    break;
                case TrackPosType.RightTop:
                    rectangle.Width += floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.Top:
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.RightBottom:
                    rectangle.Width += floatOffset.Width;
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Right:
                    rectangle.Width += floatOffset.Width;
                    break;
                case TrackPosType.LeftBottom:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Bottom:
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Rotate:
                    {
                        if (rotationLocked == false)
                        {
                            var orgRotateTrackPos = new PointF(rectangle.Right, centerPt.Y);
                            PointF curRotateTrackPos = MathHelper.Rotate(orgRotateTrackPos, centerPt, rectangle.Angle);
                            curRotateTrackPos += offset;

                            rectangle.Angle = (float)MathHelper.GetAngle(centerPt, orgRotateTrackPos, curRotateTrackPos);
                        }
                    }
                    break;
                case TrackPosType.Move:
                case TrackPosType.Inner:
                    rectangle.X += floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    break;
            }

            PointF newCenterPt = DrawingHelper.CenterPoint(rectangle);
            PointF newRotatedCenterPt = MathHelper.Rotate(newCenterPt, centerPt, rectangle.Angle);
            var centerOffset = new SizeF(newRotatedCenterPt.X - newCenterPt.X, newRotatedCenterPt.Y - newCenterPt.Y);

            rectangle.Offset(centerOffset);

            return rectangle;
        }

        public virtual void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            RotatedRect rectangle = GetTrackingRect(trackPos, offset, rotationLocked);

            SetRectangle(rectangle);
        }

        public virtual void SetRectangle(RotatedRect rectangle)
        {

        }

        public virtual void SetRectangle(Rectangle rectangle)
        {

        }

        public virtual void SetRectangle(RectangleF rectangle)
        {

        }

        public virtual void DrawSelection(Graphics g, ICoordTransform coordTransformer, bool rotationLocked)
        {
            List<ITrackRegion> trackerRegionList = GetTrackerRegionList(coordTransformer, rotationLocked);

            RotatedRect transformRect = GetRectangle();
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(transformRect);
            }

            var gp = new GraphicsPath();

            var trackerPen = new Pen(Color.Black, 1.0F);
            Brush trackerBrush = new SolidBrush(Color.LightBlue);
            foreach (ITrackRegion trackRegion in trackerRegionList)
            {
                if (trackRegion.TrackPos.PosType != TrackPosType.Inner)
                {
                    trackRegion.AddGraphic(gp);
                }
            }

            var rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            g.FillPath(trackerBrush, gp);
            g.DrawPath(trackerPen, gp);
        }

        public virtual void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false && */Visible == false)
            {
                return;
            }

            GraphicsPath gp = GetGraphicsPath(coordTransformer);

            if (tempBrush != null)
            {
                g.FillPath(tempBrush, gp);
            }
            else if (figureProperty.Brush != null)
            {
                g.FillPath(figureProperty.Brush, gp);
            }

            g.DrawPath(figureProperty.Pen, gp);

            DrawCaption(g, coordTransformer);
        }

        public virtual void DrawCaption(Graphics g, CoordTransformer coordTransformer = null)
        {
            var startPoint = new PointF(GetRectangle().Left, GetRectangle().Top);

            PointF transStartPoint;
            float scaledFontSize = 10;
            if (coordTransformer != null)
            {
                transStartPoint = coordTransformer.Transform(startPoint);
                scaledFontSize = coordTransformer.Transform(new SizeF(10, 0)).Width;
            }
            else
            {
                transStartPoint = startPoint;
            }

            var stringFormat = new StringFormat();

            var scaledFont = new Font(FontFamily.GenericSansSerif, scaledFontSize);
            Brush brush = new SolidBrush(figureProperty.Pen.Color);

            g.DrawString(Name, scaledFont, brush, transStartPoint, stringFormat);
        }

        public virtual GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer = null)
        {
            RotatedRect rectangle = GetRectangle();
            var transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(rectangle);
            }

            PointF[] points = transformRect.GetPoints();
            var gp = new GraphicsPath();
            gp.AddPolygon(points);

            return gp;
        }

        public virtual bool PtInOutline(PointF point, ICoordTransform coordTransformer = null)
        {
            return GetGraphicsPath(coordTransformer).IsOutlineVisible(point, new Pen(Color.Black, BaseConfig.Instance().TrackerSize * 2));
        }

        public virtual bool IsCrossed(PointF startPt, PointF endPt)
        {
            return false;
        }

        public virtual bool IsSame(Figure figure)
        {
            return false;
        }

        public virtual bool PtInRegion(PointF point, CoordTransformer coordTransformer = null)
        {
            return GetGraphicsPath(coordTransformer).IsVisible(point);
        }

        public virtual bool IsFilled()
        {
            return (figureProperty.Brush != null);
        }

        public abstract RotatedRect GetRectangle();
        public abstract void Offset(float x, float y);
        public abstract void Scale(float scaleX, float scaleY);

        public abstract void FlipX();
        public abstract void FlipY();

        public virtual void Rotate(float offAngle)
        {

        }

        public virtual void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            var rectangle = GetRectangle().ToRectangleF();

            var graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new RectangleF(new PointF(rectangle.X + offset.Width, rectangle.Y + offset.Height), rectangle.Size));

            graphicPathList.Add(graphicsPath);
        }
    }

    public class OblongFigure : RectangleFigure
    {
        public OblongFigure()
        {
            type = FigureType.Oblong;
        }

        public OblongFigure(RotatedRect rectangle, Pen pen, Brush brush = null)
            : base(rectangle, pen, brush)
        {
            type = FigureType.Oblong;
        }

        public OblongFigure(RotatedRect rectangle, FigureProperty figureProperty)
            : base(rectangle, figureProperty)
        {
            type = FigureType.Oblong;
        }

        public OblongFigure(RectangleF rectangle, Pen pen, Brush brush = null)
            : base(rectangle, pen, brush)
        {
            type = FigureType.Oblong;
        }

        public OblongFigure(RectangleF rectangle, FigureProperty figureProperty)
            : base(rectangle, figureProperty)
        {
            type = FigureType.Oblong;
        }

        public override object Clone()
        {
            var figure = new OblongFigure();
            figure.Copy(this);

            return figure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer = null)
        {
            RotatedRect rectangle = GetRectangle();
            var transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(rectangle);
            }

            // Point 순서: LT, RT, RB, LB
            PointF[] points = transformRect.GetPoints();
            var gp = new GraphicsPath();

            if (transformRect.Width < 1 || transformRect.Height < 1)
            {
                gp.AddRectangle(rectangle.GetBoundRect());
            }
            else
            {
                float arcDim;
                if (transformRect.Width > transformRect.Height)
                {
                    arcDim = transformRect.Height;
                    transformRect.Inflate(-arcDim / 2, 0);
                }
                else
                {
                    arcDim = transformRect.Width;
                    transformRect.Inflate(0, -arcDim / 2);
                }
                PointF[] point4 = transformRect.GetPoints();
                // Point 순서: LT, RT, RB, LB

                PointF center;
                RectangleF rect;

                // 1
                gp.AddLine(point4[0], point4[1]);

                // 2
                center = DrawingHelper.CenterPoint(point4[1], point4[2]);
                rect = DrawingHelper.FromCenterSize(center, new SizeF(arcDim, arcDim));
                gp.AddArc(rect, -rectangle.Angle - 90, 180);

                // 3
                gp.AddLine(point4[2], point4[3]);

                // 4
                center = DrawingHelper.CenterPoint(point4[3], point4[0]);
                rect = DrawingHelper.FromCenterSize(center, new SizeF(arcDim, arcDim));
                gp.AddArc(rect, -rectangle.Angle + 90, 180);
            }

            return gp;
        }
    }

    public class LineFigure : Figure
    {
        private PointF startPoint = new Point();
        public PointF StartPoint
        {
            get => startPoint;
            set => startPoint = value;
        }

        private PointF endPoint = new Point();
        public PointF EndPoint
        {
            get => endPoint;
            set => endPoint = value;
        }

        public LineFigure()
        {
            type = FigureType.Line;
        }

        public LineFigure(PointF startPoint, PointF endPoint, FigureProperty figureProperty) : base(figureProperty)
        {
            type = FigureType.Line;

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public LineFigure(PointF startPoint, PointF endPoint, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Line;

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public LineFigure(PointF startPoint, PointF endPoint, Pen pen) : base(pen, null)
        {
            type = FigureType.Line;

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public override object Clone()
        {
            var lineFigure = new LineFigure();
            lineFigure.Copy(this);

            return lineFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var lineFigure = (LineFigure)srcFigure;

            startPoint = lineFigure.startPoint;
            endPoint = lineFigure.endPoint;
        }

        public override bool IsFilled()
        {
            return false;
        }

        public override RotatedRect GetRectangle()
        {
            var startRect = new RotatedRect(startPoint.X, startPoint.Y, 0, 0, 0);
            var endRect = new RotatedRect(endPoint.X, endPoint.Y, 0, 0, 0);
            var rectangle = RotatedRect.Union(startRect, endRect);
            if (rectangle.Width == 0)
            {
                rectangle.Inflate(1, 0);
            }
            else if (rectangle.Height == 0)
            {
                rectangle.Inflate(0, 1);
            }

            return rectangle;
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
            {
                return;
            }

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "StartPoint", ref startPoint);
            XmlHelper.GetValue(figureElement, "EndPoint", ref endPoint);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "StartPoint", startPoint);
            XmlHelper.SetValue(figureElement, "EndPoint", endPoint);
        }

        public override void Offset(float x, float y)
        {
            startPoint.X += x;
            startPoint.Y += y;
            endPoint.X += x;
            endPoint.Y += y;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            startPoint.X = Convert.ToInt32(startPoint.X * scaleX);
            startPoint.Y = Convert.ToInt32(startPoint.Y * scaleY);
            endPoint.X = Convert.ToInt32(endPoint.X * scaleX);
            endPoint.Y = Convert.ToInt32(endPoint.Y * scaleY);
        }

        public override void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            var offsetPointList = new List<PointF>();
            if (trackPos.PosType == TrackPosType.Inner)
            {
                offsetPointList.Add(PointF.Add(startPoint, offset));
                offsetPointList.Add(PointF.Add(endPoint, offset));
            }
            else if (trackPos.PosType == TrackPosType.LeftTop)
            {
                offsetPointList.Add(PointF.Add(startPoint, offset));
                offsetPointList.Add(endPoint);
            }
            else if (trackPos.PosType == TrackPosType.RightBottom)
            {
                offsetPointList.Add(startPoint);
                offsetPointList.Add(PointF.Add(endPoint, offset));
            }

            var graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(offsetPointList[0], offsetPointList[1]);

            graphicPathList.Add(graphicsPath);
        }

        public override List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = BaseConfig.Instance().TrackerSize / 2;
            int trackerSize = BaseConfig.Instance().TrackerSize;

            RotatedRect rect = GetRectangle();
            if (coordTransformer != null)
            {
                rect = coordTransformer.Transform(rect);
            }

            var tackRegionList = new List<ITrackRegion>();
            tackRegionList.Add(new TrackRectangle(startPoint.X - trackerHalfSize, startPoint.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
            tackRegionList.Add(new TrackRectangle(endPoint.X - trackerHalfSize, endPoint.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));

            return tackRegionList;
        }

        public override RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            var startRect = new RotatedRect(startPoint.X, startPoint.Y, 0, 0, 0);
            var endRect = new RotatedRect(endPoint.X, endPoint.Y, 0, 0, 0);

            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    startRect.X += offset.Width;
                    startRect.Y += offset.Height;
                    break;
                case TrackPosType.RightBottom:
                    endRect.X += offset.Width;
                    endRect.Y += offset.Height;
                    break;
                case TrackPosType.Inner:
                    startRect.X += offset.Width;
                    startRect.Y += offset.Height;
                    endRect.X += offset.Width;
                    endRect.Y += offset.Height;
                    break;
            }

            return RotatedRect.Union(startRect, endRect);
        }

        public override void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    startPoint.X += offset.Width;
                    startPoint.Y += offset.Height;
                    break;
                case TrackPosType.RightBottom:
                    endPoint.X += offset.Width;
                    endPoint.Y += offset.Height;
                    break;
                case TrackPosType.Inner:
                    startPoint.X += offset.Width;
                    startPoint.Y += offset.Height;
                    endPoint.X += offset.Width;
                    endPoint.Y += offset.Height;
                    break;
            }
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            PointF transStartPoint = startPoint;
            PointF transEndPoint = endPoint;

            if (coordTransformer != null)
            {
                transStartPoint = coordTransformer.Transform(startPoint);
                transEndPoint = coordTransformer.Transform(endPoint);
            }

            var gp = new GraphicsPath();
            gp.AddLine(transStartPoint, transEndPoint);

            return gp;
        }

        public override void FlipX()
        {
            startPoint = new PointF(startPoint.X, -startPoint.Y);
            endPoint = new PointF(endPoint.X, -endPoint.Y);
        }

        public override void FlipY()
        {
            startPoint = new PointF(-startPoint.X, startPoint.Y);
            endPoint = new PointF(-endPoint.X, endPoint.Y);
        }
    }

    public class RectangleFigure : Figure
    {
        private RotatedRect rectangle = new RotatedRect();
        public RotatedRect Rectangle
        {
            get => rectangle;
            set => rectangle = value;
        }

        public RectangleFigure()
        {
            type = FigureType.Rectangle;
        }

        public RectangleFigure(RotatedRect rectangle, FigureProperty figureProperty) : base(figureProperty)
        {
            type = FigureType.Rectangle;
            this.rectangle = rectangle;
        }

        public RectangleFigure(RotatedRect rectangle, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Rectangle;
            this.rectangle = rectangle;
        }

        public RectangleFigure(RotatedRect rectangle, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Rectangle;
            this.rectangle = rectangle;
        }

        public RectangleFigure(RectangleF rectangle, FigureProperty figureProperty) : base(figureProperty)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public RectangleFigure(RectangleF rectangle, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public RectangleFigure(RectangleF rectangle, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public RectangleFigure(Rectangle rectangle, FigureProperty figureProperty) : base(figureProperty)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public RectangleFigure(Rectangle rectangle, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public RectangleFigure(Rectangle rectangle, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Rectangle;

            SetRectangle(rectangle);
        }

        public override object Clone()
        {
            var rectangleFigure = new RectangleFigure();
            rectangleFigure.Copy(this);

            return rectangleFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var rectangleFigure = (RectangleFigure)srcFigure;
            rectangle = rectangleFigure.rectangle;
        }

        public override RotatedRect GetRectangle()
        {
            return new RotatedRect(rectangle);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
            {
                return;
            }

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "Rect", ref rectangle);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Rect", rectangle);
        }

        public override void Offset(float x, float y)
        {
            rectangle.Offset(x, y);
        }

        public override void Rotate(float offAngle)
        {
            rectangle.Angle = (rectangle.Angle + offAngle) % 360;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            rectangle.X = Convert.ToInt32(rectangle.X * scaleX);
            rectangle.Y = Convert.ToInt32(rectangle.Y * scaleY);
            rectangle.Width = Convert.ToInt32(rectangle.Width * scaleX);
            rectangle.Height = Convert.ToInt32(rectangle.Height * scaleY);
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            this.rectangle = rectangle;
        }

        public override void SetRectangle(RectangleF rectangle)
        {
            this.rectangle.X = rectangle.X;
            this.rectangle.Y = rectangle.Y;
            this.rectangle.Width = rectangle.Width;
            this.rectangle.Height = rectangle.Height;
        }

        public override void SetRectangle(Rectangle rectangle)
        {
            this.rectangle.X = rectangle.X;
            this.rectangle.Y = rectangle.Y;
            this.rectangle.Width = rectangle.Width;
            this.rectangle.Height = rectangle.Height;
        }

        public override void FlipX()
        {
            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);
            rectangle.Angle = 360 - rectangle.Angle;
        }

        public override void FlipY()
        {
            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);
            rectangle.Angle = (180 - rectangle.Angle);
            if (rectangle.Angle < 0)
            {
                rectangle.Angle = 360 + rectangle.Angle;
            }
        }
    }

    public class EllipseFigure : RectangleFigure
    {
        private float startAngle = 0;
        private float sweepAngle = 0;

        public EllipseFigure()
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(Rectangle rectangle, FigureProperty figureProperty) : base(rectangle, figureProperty)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(Rectangle rectangle, string figurePropertyName) : base(rectangle, figurePropertyName)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(Rectangle rectangle, Pen pen, Brush brush = null) : base(rectangle, pen, brush)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(RectangleF rectangle, FigureProperty figureProperty) : base(rectangle, figureProperty)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(RectangleF rectangle, string figurePropertyName) : base(rectangle, figurePropertyName)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(RectangleF rectangle, Pen pen, Brush brush = null) : base(rectangle, pen, brush)
        {
            type = FigureType.Ellipse;
        }

        public void SetArcAngle(float startAngle, float sweepAngle)
        {
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
        }

        public override object Clone()
        {
            var ellipseFigure = new EllipseFigure();
            ellipseFigure.Copy(this);

            return ellipseFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect transformRect = Rectangle;
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(Rectangle);
            }

            var gp = new GraphicsPath();
            var drawingRect = transformRect.ToRectangleF();
            if (drawingRect.Width < 1 || drawingRect.Height < 1)
            {
                gp.AddRectangle(drawingRect);
            }
            else
            {
                if (sweepAngle > 0)
                {
                    gp.AddArc(drawingRect, startAngle, sweepAngle);
                }
                else
                {
                    gp.AddEllipse(drawingRect);
                }
            }
            //gp.AddEllipse(new RectangleF(363.0587f,98.42592f,0.9f,0.9f));

            //			Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            //			rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            //			gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public class CrossFigure : RectangleFigure
    {
        public CrossFigure()
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RotatedRect rectangle, FigureProperty figureProperty)
            : base(rectangle, figureProperty)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RotatedRect rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Rectangle rectangle, FigureProperty figureProperty)
            : base(rectangle, figureProperty)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Rectangle rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Rectangle rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RectangleF rectangle, FigureProperty figureProperty)
            : base(rectangle, figureProperty)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RectangleF rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RectangleF rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(PointF centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Point centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.Cross;
        }

        public override object Clone()
        {
            var crossFigure = new CrossFigure();
            crossFigure.Copy(this);

            return crossFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect transformRect = Rectangle;
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(Rectangle);
            }

            PointF centerPt = DrawingHelper.CenterPoint(transformRect);

            var gp = new GraphicsPath();
            gp.AddLine(centerPt.X, transformRect.Y, centerPt.X, transformRect.Bottom);
            gp.CloseFigure();
            gp.AddLine(transformRect.X, centerPt.Y, transformRect.Right, centerPt.Y);

            var rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public class XRectFigure : RectangleFigure
    {
        public XRectFigure()
        {
            type = FigureType.XRect;
        }

        public XRectFigure(Rectangle rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(Rectangle rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RectangleF rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RectangleF rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RotatedRect rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RotatedRect rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public override object Clone()
        {
            var xRectFigure = new XRectFigure();
            xRectFigure.Copy(this);

            return xRectFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect rectangle = GetRectangle();
            var transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(rectangle);
            }

            var gp = new GraphicsPath();
            gp.AddRectangle(transformRect.ToRectangleF());
            gp.CloseFigure();

            PointF[] points = DrawingHelper.GetPoints(transformRect.ToRectangleF(), 0);

            gp.AddLine(points[0].X, points[0].Y, points[2].X, points[2].Y);
            gp.CloseFigure();
            gp.AddLine(points[1].X, points[1].Y, points[3].X, points[3].Y);

            var rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-rectangle.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public enum PolygonNodeType
    {
        Start, Line, Arc
    }

    public abstract class PolygonNode
    {
        public PolygonNodeType NodeType { get; set; }

        public PolygonNode(PolygonNodeType connectType)
        {
            NodeType = connectType;
        }

        public abstract void Load(XmlElement nodeElement);
        public abstract void Save(XmlElement nodeElement);
        public abstract void Offset(float x, float y);
        public abstract void Scale(float scaleX, float scaleY);
        public abstract PointF[] GetTrackPoints();
        public abstract void AddFigure(GraphicsPath gp, ICoordTransform coordTransformer, PointF rotateCenter, float angle, ref PointF lastPoint);
        public abstract void FlipX();
        public abstract void FlipY();
        public abstract PointF GetTransformPoint(ICoordTransform coordTransformer, PointF rotateCenter, float angle);

        public PointF TransformPoint(PointF point, ICoordTransform coordTransformer, PointF rotateCenter, float angle)
        {
            PointF transPoint = PointF.Empty;
            if (coordTransformer != null)
            {
                transPoint = MathHelper.Rotate(point, rotateCenter, coordTransformer.InvertY ? -angle : angle);
                transPoint = coordTransformer.Transform(transPoint);
            }
            else
            {
                transPoint = MathHelper.Rotate(point, rotateCenter, angle);
            }

            return transPoint;
        }

        public static PolygonNode Create(PolygonNodeType nodeType)
        {
            switch (nodeType)
            {
                case PolygonNodeType.Arc:
                    return new ArcPolygonNode();
                case PolygonNodeType.Start:
                case PolygonNodeType.Line:
                    return new PointPolygonNode();
            }

            return null;
        }
    }

    public class PointPolygonNode : PolygonNode
    {
        public PointF Point { get; set; }

        public PointPolygonNode() : base(PolygonNodeType.Line)
        {

        }

        public PointPolygonNode(PointF point) : base(PolygonNodeType.Line)
        {
            Point = point;
        }

        public override void Load(XmlElement nodeElement)
        {
            float x = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "X", "0"));
            float y = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "Y", "0"));
            Point = new PointF(x, y);
        }

        public override void Save(XmlElement nodeElement)
        {
            XmlHelper.SetValue(nodeElement, "X", Point.X.ToString());
            XmlHelper.SetValue(nodeElement, "Y", Point.X.ToString());
        }

        public override void Offset(float x, float y)
        {
            Point = new PointF(Point.X + x, Point.Y + y);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            Point = new PointF(Point.X * scaleX, Point.Y * scaleY);
        }

        public override PointF[] GetTrackPoints()
        {
            return new PointF[] { Point };
        }

        public override void AddFigure(GraphicsPath gp, ICoordTransform coordTransformer, PointF rotateCenter, float angle, ref PointF lastPoint)
        {
            PointF transPoint = TransformPoint(Point, coordTransformer, rotateCenter, angle);

            if (NodeType == PolygonNodeType.Line && lastPoint != PointF.Empty)
            {
                gp.AddLine(lastPoint, transPoint);
            }

            lastPoint = transPoint;
        }

        public override void FlipX()
        {
            Point = new PointF(Point.X, -Point.Y);
        }

        public override void FlipY()
        {
            Point = new PointF(-Point.X, Point.Y);
        }

        public override PointF GetTransformPoint(ICoordTransform coordTransformer, PointF rotateCenter, float angle)
        {
            return TransformPoint(Point, coordTransformer, rotateCenter, angle);
        }
    }

    public class ArcPolygonNode : PolygonNode
    {
        public PointF CenterPt { get; set; }
        public float Radius { get; set; }
        public float StartAngle { get; set; }
        public float SweepAngle { get; set; }

        public ArcPolygonNode() : base(PolygonNodeType.Arc)
        {

        }

        public ArcPolygonNode(PointF centerPt, float radius, float startAngle, float sweepAngle) : base(PolygonNodeType.Arc)
        {
            CenterPt = centerPt;
            Radius = radius;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }

        public RectangleF GetBoundRect()
        {
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(DrawingHelper.FromCenterSize(CenterPt, new SizeF(Radius * 2, Radius * 2)), StartAngle, SweepAngle);
            return graphicsPath.GetBounds();
        }

        public override void Load(XmlElement nodeElement)
        {
            float x = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "CenterX", "0"));
            float y = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "CenterY", "0"));
            CenterPt = new PointF(x, y);

            Radius = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "Radius", "0"));
            StartAngle = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "StartAngle", "0"));
            SweepAngle = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "SweepAngle", "0"));
        }

        public override void Save(XmlElement nodeElement)
        {
            XmlHelper.SetValue(nodeElement, "CenterX", CenterPt.X.ToString());
            XmlHelper.SetValue(nodeElement, "CenterY", CenterPt.X.ToString());
            XmlHelper.SetValue(nodeElement, "Radius", Radius.ToString());
            XmlHelper.SetValue(nodeElement, "StartAngle", StartAngle.ToString());
            XmlHelper.SetValue(nodeElement, "SweepAngle", SweepAngle.ToString());
        }

        public override void Offset(float x, float y)
        {
            CenterPt = new PointF(CenterPt.X + x, CenterPt.Y + y);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            CenterPt = new PointF(CenterPt.X * scaleX, CenterPt.Y * scaleY);
        }

        public override PointF[] GetTrackPoints()
        {
            return new PointF[] { CenterPt, CenterPt, CenterPt };
        }

        public override void AddFigure(GraphicsPath gp, ICoordTransform coordTransformer, PointF rotateCenter, float angle, ref PointF lastPoint)
        {
            PointF transPoint = TransformPoint(CenterPt, coordTransformer, rotateCenter, angle);

            float trasnStartAngle = StartAngle + (coordTransformer.InvertY ? -angle : angle);
            SizeF transSize = coordTransformer.Transform(new SizeF(Radius * 2, Radius * 2));

            gp.AddArc(DrawingHelper.FromCenterSize(transPoint, transSize), trasnStartAngle, SweepAngle);

            lastPoint = gp.GetLastPoint();
        }

        public override void FlipX()
        {
            throw new NotImplementedException();
        }

        public override void FlipY()
        {
            throw new NotImplementedException();
        }

        public override PointF GetTransformPoint(ICoordTransform coordTransformer, PointF rotateCenter, float angle)
        {
            return TransformPoint(CenterPt, coordTransformer, rotateCenter, angle);
        }
    }

    public class PolygonFigure : Figure
    {
        public float Angle { get; set; }
        public List<PolygonNode> PolygonNodeList { get; set; } = new List<PolygonNode>();
        public bool IsEditable { get; set; } = true;

        public PolygonFigure() : base()
        {
            type = FigureType.Polygon;
        }

        public PolygonFigure(List<PointF> pointList, FigureProperty figureProperty) : base(figureProperty)
        {
            Init(pointList);
        }

        public PolygonFigure(List<PointF> pointList, string figurePropertyName) : base(figurePropertyName)
        {
            Init(pointList);
        }

        public PolygonFigure(List<PointF> pointList, Pen pen, Brush brush = null) : base(pen, brush)
        {
            Init(pointList);
        }

        public PolygonFigure(FigureProperty figureProperty) : base(figureProperty)
        {
            type = FigureType.Polygon;
        }

        public PolygonFigure(string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Polygon;
        }

        public PolygonFigure(Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Polygon;
        }

        public void Init(List<PointF> pointList)
        {
            type = FigureType.Polygon;

            foreach (PointF point in pointList)
            {
                PolygonNodeList.Add(new PointPolygonNode(point));
            }
        }

        public override object Clone()
        {
            var polygonFigure = new PolygonFigure();
            polygonFigure.Copy(this);

            return polygonFigure;
        }

        public void AddLIne(PointF point)
        {
            var polygonNode = new PointPolygonNode(point);
            if (PolygonNodeList.Count == 0)
            {
                polygonNode.NodeType = PolygonNodeType.Start;
            }

            PolygonNodeList.Add(polygonNode);
        }

        public void AddArc(PointF centerPt, float radius, float startAngle, float sweepAngle)
        {
            PolygonNodeList.Add(new ArcPolygonNode(centerPt, radius, startAngle, sweepAngle));
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var polygonFigure = (PolygonFigure)srcFigure;

            PolygonNodeList.AddRange(polygonFigure.PolygonNodeList);

            Angle = polygonFigure.Angle;
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false && */Visible == false)
            {
                return;
            }

            GraphicsPath gp = GetGraphicsPath(coordTransformer);

            if (tempBrush != null)
            {
                g.FillPath(tempBrush, gp);
            }
            else if (figureProperty.Brush != null)
            {
                g.FillPath(figureProperty.Brush, gp);
            }

            g.DrawPath(figureProperty.Pen, gp);
        }

        public override RotatedRect GetRectangle()
        {
            RectangleF boundRect = RectangleF.Empty;

            var pointList = new List<PointF>();
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                if (polygonNode.NodeType == PolygonNodeType.Arc)
                {
                    boundRect = RectangleF.Union(boundRect, ((ArcPolygonNode)polygonNode).GetBoundRect());
                }
                else
                {
                    pointList.Add(((PointPolygonNode)polygonNode).Point);
                }
            }

            boundRect = RectangleF.Union(boundRect, DrawingHelper.GetBoundRect(pointList.ToArray()));

            return new RotatedRect(boundRect, Angle);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
            {
                return;
            }

            base.Load(figureElement);

            foreach (XmlElement nodeListElement in figureElement)
            {
                if (nodeListElement.Name != "NodeList")
                {
                    continue;
                }

                foreach (XmlElement nodeElement in nodeListElement)
                {
                    if (nodeElement.Name != "Node")
                    {
                        continue;
                    }

                    if (Enum.TryParse<PolygonNodeType>(XmlHelper.GetValue(nodeElement, "NodeType", "Point"), out PolygonNodeType nodeType) == true)
                    {
                        var polygonNode = PolygonNode.Create(nodeType);
                        polygonNode.Load(nodeElement);

                        PolygonNodeList.Add(polygonNode);
                    }
                }
            }
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlElement nodeListElement = figureElement.OwnerDocument.CreateElement("", "NodeList", "");
            figureElement.AppendChild(nodeListElement);

            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                XmlElement nodeElement = figureElement.OwnerDocument.CreateElement("", "Node", "");
                nodeListElement.AppendChild(nodeElement);

                polygonNode.Save(nodeElement);
            }
        }

        public override void Offset(float x, float y)
        {
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                polygonNode.Offset(x, y);
            }
        }

        public override void Scale(float scaleX, float scaleY)
        {
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                polygonNode.Scale(scaleX, scaleY);
            }
        }

        public override List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = BaseConfig.Instance().TrackerSize / 2;
            int trackerSize = BaseConfig.Instance().TrackerSize;

            RotatedRect rect = GetRectangle();
            var pointList = new List<PointF>();
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                pointList.AddRange(polygonNode.GetTrackPoints());
            }

            if (coordTransformer != null)
            {
                pointList.Clear();
                rect = coordTransformer.Transform(rect);
                pointList.ForEach(f => pointList.Add(coordTransformer.Transform(f)));
            }

            var regionList = new List<ITrackRegion>();
            if (IsEditable)
            {
                for (int index = 0; index < pointList.Count; index++)
                {
                    regionList.Add(new TrackRectangle(pointList[index].X - trackerHalfSize, pointList[index].Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Polygon, index));
                }

                regionList.Add(new TrackPolygon(pointList, TrackPosType.Inner, 0));
            }
            else
            {
                regionList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                regionList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                regionList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                regionList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                regionList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
            }

            return regionList;
        }

        private List<PointF> GetPointList()
        {
            var pointList = new List<PointF>();
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                pointList.AddRange(polygonNode.GetTrackPoints());
            }

            return pointList;
        }

        public override RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            List<PointF> pointList = GetPointList();
            var offsetPointList = new List<PointF>();

            if (trackPos.PosType == TrackPosType.Inner)
            {
                pointList.ForEach(x => offsetPointList.Add(new PointF(x.X + offset.Width, x.Y + offset.Height)));
            }
            else
            {
                offsetPointList.AddRange(pointList);

                PointF point = offsetPointList[trackPos.PolygonIndex];
                point.X += offset.Width;
                point.Y += offset.Height;

                offsetPointList[trackPos.PolygonIndex] = point;
            }

            return new RotatedRect(DrawingHelper.GetBoundRect(offsetPointList.ToArray()), 0);
        }

        public override void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            if (trackPos.PosType == TrackPosType.Inner)
            {
                foreach (PolygonNode polygonNode in PolygonNodeList)
                {
                    polygonNode.Offset(offset.Width, offset.Height);
                }
            }
            else
            {
                if (IsEditable)
                {
                    //PointF point = pointList[trackPos.PolygonIndex];
                    //point.X += offset.Width;
                    //point.Y += offset.Height;

                    //pointList[trackPos.PolygonIndex] = point;
                }
                else
                {

                    //RotatedRect RotatedRect = this.GetRectangle();
                    //float scaleX = (float)(RotatedRect.Width + offset.Width) / (float)RotatedRect.Width;
                    //float scaleY = (float)(RotatedRect.Height+offset.Height) / (float)RotatedRect.Height;

                    //)
                    //PointF scaleRef = DrawingHelper.CenterPoint(this.GetTrackerRectangleList().Find(f => f.TrackPos.PosType == TrackPosType.LeftBottom).Rectangle);
                }
            }
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect rotatedRect = GetRectangle();
            var rotateCenter = new PointF((rotatedRect.Left + rotatedRect.Right) / 2.0f, (rotatedRect.Top + rotatedRect.Bottom) / 2.0f);

            var gp = new GraphicsPath();

            gp.StartFigure();

            PointF lastPoint = PointF.Empty;

            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                if (polygonNode.NodeType == PolygonNodeType.Start)
                {
                    lastPoint = polygonNode.GetTransformPoint(coordTransformer, rotateCenter, Angle);
                }
                else
                {
                    polygonNode.AddFigure(gp, coordTransformer, rotateCenter, Angle, ref lastPoint);
                }
            }

            gp.CloseFigure();

            return gp;
        }

        public override void FlipX()
        {
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                polygonNode.FlipX();
            }
        }

        public override void FlipY()
        {
            foreach (PolygonNode polygonNode in PolygonNodeList)
            {
                polygonNode.FlipY();
            }
        }

        public override void Rotate(float offAngle)
        {
            Angle = (Angle + offAngle) % 360;
        }

        public override bool IsCrossed(PointF startPt, PointF endPt)
        {
            List<PointF> pointList = GetPointList();

            for (int i = 0; i < pointList.Count; i++)
            {
                if (i == (pointList.Count - 1))
                {
                    if (DrawingHelper.IsCross(pointList[i], pointList[0], startPt, endPt) == true)
                    {
                        return true;
                    }
                }
                else
                {
                    if (DrawingHelper.IsCross(pointList[i], pointList[i + 1], startPt, endPt) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            List<PointF> pointList = GetPointList();

            var offsetPointList = new List<PointF>();
            if (trackPos.PosType == TrackPosType.Inner)
            {
                foreach (PointF pt in pointList)
                {
                    offsetPointList.Add(PointF.Add(pt, offset));
                }
            }
            else
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    if (trackPos.PolygonIndex == i)
                    {
                        offsetPointList.Add(PointF.Add(pointList[i], offset));
                    }
                    else
                    {
                        offsetPointList.Add(pointList[i]);
                    }
                }
            }

            var graphicsPath = new GraphicsPath();
            graphicsPath.AddPolygon(offsetPointList.ToArray());

            graphicPathList.Add(graphicsPath);
        }

        public override bool IsSame(Figure figure)
        {
            if (!(figure is PolygonFigure))
            {
                return false;
            }

            List<PointF> pointList = GetPointList();

            PointF[] ptArray1 = pointList.ToArray();
            PointF centerPt1 = DrawingHelper.CenterPoint(ptArray1);
            ptArray1 = DrawingHelper.Offset(ptArray1, new SizeF(-centerPt1.X, -centerPt1.Y));

            var otherFigure = (PolygonFigure)figure;
            PointF[] ptArray2 = otherFigure.GetPointList().ToArray();
            PointF centerPt2 = DrawingHelper.CenterPoint(ptArray2);
            ptArray2 = DrawingHelper.Offset(ptArray2, new SizeF(-centerPt2.X, -centerPt2.Y));

            foreach (PointF pt1 in ptArray1)
            {
                bool found = false;

                foreach (PointF pt2 in ptArray2)
                {
                    var offset = PointF.Subtract(pt1, new SizeF(pt2));
                    if (Math.Abs(offset.X) < 1 && Math.Abs(offset.Y) < 1)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class TextFigure : Figure
    {
        private PointF position;
        public PointF Position
        {
            get => position;
            set => position = value;
        }
        public float Angle { get; set; }
        public string Text { get; set; }

        public TextFigure() : base()
        {
            type = FigureType.Text;
        }

        public TextFigure(string text, Point position, Font font, Color textColor, StringAlignment stringAlignment = StringAlignment.Center) : base(font, textColor, stringAlignment)
        {
            type = FigureType.Text;

            Text = text;
            this.position = position;
        }

        public override object Clone()
        {
            var textFigure = new TextFigure();
            textFigure.Copy(this);

            return textFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var textFigure = (TextFigure)srcFigure;

            Text = textFigure.Text;
            position = textFigure.position;
        }

        public override bool IsFilled()
        {
            return true;
        }

        public override RotatedRect GetRectangle()
        {
            PointF newPosition = position;
            Size textSize = TextRenderer.MeasureText(Text, figureProperty.Font);
            if (figureProperty.Alignment == StringAlignment.Far)
            {
                newPosition.X -= textSize.Width;
            }
            else if (figureProperty.Alignment == StringAlignment.Center)
            {
                newPosition.X -= textSize.Width / 2;
            }

            return new RotatedRect(newPosition, textSize, Angle);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
            {
                return;
            }

            base.Load(figureElement);

            Text = XmlHelper.GetValue(figureElement, "Text", "");
            XmlHelper.GetValue(figureElement, "Position", ref position);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Text", Text);
            XmlHelper.SetValue(figureElement, "Position", position);
        }

        public override void Offset(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            position.X *= scaleX;
            position.Y *= scaleY;

            Font font = figureProperty.Font;
            figureProperty.Font = new Font(font.FontFamily, font.Size * scaleX, font.Style);
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            if (figureProperty.Alignment == StringAlignment.Center)
            {
                position.X = rectangle.X + rectangle.Width / 2;
            }
            else if (figureProperty.Alignment == StringAlignment.Far)
            {
                position.X = rectangle.X + rectangle.Width;
            }
            else
            {
                position.X = rectangle.X;
            }

            position.Y = rectangle.Y;
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
            {
                return;
            }

            RotatedRect transformRect = GetRectangle();
            PointF transformPt = position;
            float scaledFontSize = figureProperty.Font.Size;
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(transformRect);
                transformPt = coordTransformer.Transform(position);
                scaledFontSize = coordTransformer.Transform(new SizeF(figureProperty.Font.Size, 0)).Width;
            }

            if (scaledFontSize < 1)
            {
                scaledFontSize = 1;
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Matrix m = g.Transform.Clone();
            //			m.RotateAt((float)-transformRect.Angle, new PointF(transformRect.X + transformRect.Width / 2.0f, transformRect.Y + transformRect.Height / 2.0f));
            m.RotateAt(-transformRect.Angle, transformPt);

            Matrix preTransform = g.Transform;
            g.Transform = m;

            var stringFormat = new StringFormat()
            {
                Alignment = figureProperty.Alignment,
                LineAlignment = StringAlignment.Near
            };

            var scaledFont = new Font(figureProperty.Font.FontFamily, scaledFontSize);
            Brush brush = new SolidBrush(figureProperty.TextColor);

            g.DrawString(Text, scaledFont, brush, transformPt, stringFormat);

            g.Transform = preTransform;
        }

        public override void FlipX()
        {
            position = new PointF(position.X, -position.Y);
        }

        public override void FlipY()
        {
            position = new PointF(-position.X, position.Y);
        }
    }

    public class ImageFigure : Figure
    {
        public Image Image { get; set; }
        public string ImageFormatName { get; set; } = "bmp";

        private RotatedRect rectangle = new RotatedRect();

        public ImageFigure()
        {
            type = FigureType.Image;
        }

        public ImageFigure(Image image, string imageFormatName, RotatedRect rectangle) // png 파일 불러오기
        {
            type = FigureType.Image;

            Image = image;
            ImageFormatName = imageFormatName.ToLower();
            this.rectangle = rectangle;
        }

        public override object Clone()
        {
            var imageFigure = new ImageFigure();
            imageFigure.Copy(this);

            return imageFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var imageFigure = (ImageFigure)srcFigure;

            rectangle = imageFigure.rectangle;
            ImageFormatName = imageFigure.ImageFormatName;
            Image = ImageHelper.CloneImage(imageFigure.Image);
        }

        public override bool IsFilled()
        {
            return true;
        }

        public override RotatedRect GetRectangle()
        {
            return new RotatedRect(rectangle);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
            {
                return;
            }

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "Rect", ref rectangle);
            ImageFormatName = XmlHelper.GetValue(figureElement, "ImageFormat", "Bmp");

            string imageStr = XmlHelper.GetValue(figureElement, "Image", "");
            if (imageStr != "")
            {
                Image = ImageHelper.Base64StringToImage(imageStr, GetImageFormat(ImageFormatName));
            }
        }

        private ImageFormat GetImageFormat(string imageFormatStr)
        {
            switch (imageFormatStr)
            {
                default:
                case "bmp": return ImageFormat.Bmp;
                case "png": return ImageFormat.Png;
            }
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Rect", rectangle);
            XmlHelper.SetValue(figureElement, "ImageFormat", ImageFormatName.ToString());
            XmlHelper.SetValue(figureElement, "Image", ImageHelper.ImageToBase64String(Image, GetImageFormat(ImageFormatName)));
        }

        public override void Offset(float x, float y)
        {
            rectangle.Offset(x, y);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            rectangle.X = Convert.ToInt32(rectangle.X * scaleX);
            rectangle.Y = Convert.ToInt32(rectangle.Y * scaleY);
            rectangle.Width = Convert.ToInt32(rectangle.Width * scaleX);
            rectangle.Height = Convert.ToInt32(rectangle.Height * scaleY);
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
            {
                return;
            }

            RotatedRect transformRect = rectangle;
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(rectangle);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Transformation matrix
            var m = new Matrix();
            m.RotateAt(-transformRect.Angle, new PointF(transformRect.Width / 2.0f, transformRect.Height / 2.0f));

            Matrix preTransform = g.Transform;
            g.Transform = m;
            g.DrawImage(Image, transformRect.ToRectangleF());
            g.Transform = preTransform;
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            this.rectangle = rectangle;
        }

        public override void FlipX()
        {
            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);
        }

        public override void FlipY()
        {
            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);
        }
    }

    public class FigureGroup : Figure
    {
        public override Brush TempBrush
        {
            set
            {
                foreach (Figure figure in FigureList)
                {
                    figure.TempBrush = value;
                }
            }
        }
        public List<Figure> FigureList { get; } = new List<Figure>();

        public Figure this[string name]
        {
            get => FigureList.Find(f => f.Name == name);
            set => FigureList.Add(value);
        }

        public Figure this[int key] => FigureList[key];

        public int NumFigure => FigureList.Count;

        public FigureGroup()
        {
            type = FigureType.Group;
        }

        public FigureGroup(string name)
        {
            type = FigureType.Group;
            Name = name;
        }

        public bool FigureExist => FigureList.Count > 0;


        public override object Clone()
        {
            var figureGroup = new FigureGroup();
            figureGroup.Copy(this);

            return figureGroup;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            var figureGroup = (FigureGroup)srcFigure;

            foreach (Figure figure in figureGroup.FigureList)
            {
                AddFigure((Figure)figure.Clone());
            }
        }

        public void SetTempBrush(Brush brush)
        {
            foreach (Figure figure in FigureList)
            {
                figure.TempBrush = brush;
            }
        }

        public override void ResetTempProperty()
        {
            foreach (Figure figure in FigureList)
            {
                figure.ResetTempProperty();
            }
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return FigureList.GetEnumerator();
        }

        public Figure GetFigure(string id)
        {
            foreach (Figure figure in FigureList)
            {
                if (figure.Id == id)
                {
                    return figure;
                }
            }

            return null;
        }

        public Figure AddFigure(Figure figure)
        {
            FigureList.Add(figure);
            return figure;
        }

        public FigureGroup AppendFigure(FigureGroup figureGroup)
        {
            FigureList.AddRange(figureGroup.FigureList);
            return figureGroup;
        }

        public void RemoveFigure(Figure figure)
        {
            FigureList.Remove(figure);
        }

        public void Clear()
        {
            FigureList.Clear();
        }

        public override bool IsFilled()
        {
            return false;
        }

        public bool IsExsit(Figure figure)
        {
            return FigureList.IndexOf(figure) >= 0;
        }

        public override RotatedRect GetRectangle()
        {
            var rectangle = new RotatedRect();
            foreach (Figure figure in FigureList)
            {
                if (rectangle.IsEmpty)
                {
                    rectangle = figure.GetRectangle();
                }
                else
                {
                    rectangle = RotatedRect.Union(rectangle, figure.GetRectangle());
                }
            }

            return rectangle;
        }

        public override void Load(XmlElement figureGroupElement)
        {
            if (figureGroupElement == null)
            {
                return;
            }

            base.Load(figureGroupElement);

            foreach (XmlElement figureElement in figureGroupElement)
            {
                if (figureElement.Name == "Figure")
                {
                    string typeStr = XmlHelper.GetValue(figureElement, "Type", "");

                    Figure figure = FigureFactory.Create(typeStr);
                    FigureList.Add(figure);

                    figure.Load(figureElement);
                }
            }
        }

        public override void Save(XmlElement figureGroupElement)
        {
            base.Save(figureGroupElement);

            foreach (Figure figure in FigureList)
            {
                XmlElement figureElement = figureGroupElement.OwnerDocument.CreateElement("", "Figure", "");
                figureGroupElement.AppendChild(figureElement);

                figure.Save(figureElement);
            }
        }

        public Figure GetFigureByName(string name)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.Name == name)
                {
                    return figure;
                }
            }

            return null;

        }
        public Figure GetFigureByTag(object tagObj)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.Tag == tagObj)
                {
                    return figure;
                }
            }

            return null;
        }

        public Figure GetFigureByTagStr(object tagObj)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.Tag.ToString() == tagObj.ToString())
                {
                    return figure;
                }
            }

            return null;
        }

        public Figure GetFigure(Point point)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.PtInRegion(point))
                {
                    return figure;
                }
            }

            return null;
        }

        public void SetSelectable(bool selectable)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                figure.Selectable = selectable;
            }
        }

        public Figure Select(PointF point, Figure searchAfter = null)
        {
            bool startFound = true;
            if (searchAfter != null)
            {
                startFound = false;
            }

            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (startFound == false)
                {
                    if (figure == searchAfter)
                    {
                        startFound = true;
                    }

                    continue;
                }

                if (figure.PtInOutline(point) == true)
                {
                    if (figure.Selectable == true)
                    {
                        return figure;
                    }
                }
            }

            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (startFound == false)
                {
                    if (figure == searchAfter)
                    {
                        startFound = true;
                    }

                    continue;
                }

                if (figure.PtInRegion(point) == true)
                {
                    if (figure.Selectable == true && figure.Visible)
                    {
                        return figure;
                    }
                }
            }

            return null;
        }

        public List<Figure> Select(Rectangle rectangle)
        {
            var selectedFigureList = new List<Figure>();
            // Outline 객체 선택
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                var boundRect = Rectangle.Round(figure.GetRectangle().GetBoundRect());
                if (rectangle.Contains(boundRect) == true)
                {
                    if (figure.Selectable == true)
                    {
                        selectedFigureList.Add(figure);
                    }
                }
            }

            return selectedFigureList;
        }

        public Figure GetTaggedFigure(Point point)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.PtInRegion(point) && figure.Tag != null)
                {
                    return figure;
                }
            }

            return null;
        }

        private static Predicate<Figure> ByRef(Figure figureRef)
        {
            return delegate (Figure figure)
            {
                return figure == figureRef;
            };
        }

        public void MoveUp(Figure figure)
        {
            int index = FigureList.FindIndex(ByRef(figure));
            if (index > -1 && (index + 1) < FigureList.Count)
            {
                FigureList.Remove(figure);
                FigureList.Insert(index + 1, figure);
            }
        }

        public void MoveTop(Figure figure)
        {
            FigureList.Remove(figure);
            FigureList.Add(figure);
        }

        public void MoveDown(Figure figure)
        {
            int index = FigureList.FindIndex(ByRef(figure));
            if ((index > 0) && (index < FigureList.Count))
            {
                FigureList.Remove(figure);
                FigureList.Insert(index - 1, figure);
            }
        }

        public void MoveBottom(Figure figure)
        {
            FigureList.Remove(figure);
            FigureList.Insert(0, figure);
        }

        public override void Offset(float x, float y)
        {
            foreach (Figure figure in FigureList)
            {
                figure.Offset(x, y);
            }
        }

        public override void Scale(float scaleX, float scaleY)
        {
            foreach (Figure figure in FigureList)
            {
                figure.Scale(scaleX, scaleY);
            }
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
            {
                return;
            }

            foreach (Figure figure in FigureList)
            {
                figure.Draw(g, coordTransformer, editable);
            }
        }

        public override void SetRectangle(RotatedRect newRect)
        {
            RotatedRect preRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(preRect);
            Offset(-centerPt.X, -centerPt.Y);

            Scale(newRect.Width / preRect.Width, newRect.Height / preRect.Height);

            PointF newCenterPt = DrawingHelper.CenterPoint(newRect);
            Offset(newCenterPt.X, newCenterPt.Y);
        }

        public override bool PtInRegion(PointF point, CoordTransformer coordTransformer = null)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.PtInRegion(point, coordTransformer) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool PtInOutline(PointF point, ICoordTransform coordTransformer = null)
        {
            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                if (figure.PtInOutline(point, coordTransformer) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public override void FlipX()
        {
            RotatedRect rotatedRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                figure.Offset(0, -centerPt.Y);
                figure.FlipX();
                figure.Offset(0, centerPt.Y);
            }
        }

        public override void FlipY()
        {
            RotatedRect rotatedRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            foreach (Figure figure in FigureList.Reverse<Figure>())
            {
                figure.Offset(-centerPt.X, 0);
                figure.FlipY();
                figure.Offset(centerPt.Y, 0);
            }
        }

        public override void Rotate(float offAngle)
        {
            FigureList.ForEach(f => f.Rotate(offAngle));
        }


        public List<Figure> GetFigureByName(string name, bool recursive, bool wildSearch)
        {
            var figureList = new List<Figure>();
            if (recursive)
            {
                List<Figure> figureGroupList = FigureList.FindAll(f => f is FigureGroup);
                foreach (FigureGroup figureGroup in figureGroupList)
                {
                    figureList.AddRange(figureGroup.GetFigureByName(name, true, wildSearch));
                }
            }

            figureList = FigureList.FindAll(f =>
            {
                if (wildSearch)
                {
                    return f.Name.Contains(name);
                }
                else
                {
                    return f.Name == name;
                }
            });

            return figureList;
        }

        //public List<Figure> GetFigureByName(string name, bool recursive, bool wildSearch)
        //{
        //    List<Figure> figureList = new List<Figure>();

        //    foreach (Figure figure in this.figureList)
        //    {
        //        if (figure is FigureGroup && recursive)
        //        {
        //            figureList.AddRange(((FigureGroup)figure).GetFigureByName(name, recursive, wildSearch));
        //        }

        //        string figureString = figure.Name;

        //        if (wildSearch == true)
        //        {
        //            if (figureString.Contains(name))
        //            {
        //                figureList.Add(figure);
        //            }
        //        }
        //        else
        //        {
        //            if (figureString == name)
        //            {
        //                figureList.Add(figure);
        //            }
        //        }
        //    }

        //    return figureList;
        //}

        public List<Figure> GetFigureByTag(string tag, bool wildSearch)
        {
            var figureList = new List<Figure>();

            foreach (Figure figure in FigureList)
            {
                if (figure is FigureGroup)
                {
                    figureList.AddRange(((FigureGroup)figure).GetFigureByTag(tag, wildSearch));
                }

                string figureString = figure.Tag as string;

                if (wildSearch == true)
                {
                    if (figureString.Contains(tag))
                    {
                        figureList.Add(figure);
                    }
                }
                else
                {
                    if (figureString == tag)
                    {
                        figureList.Add(figure);
                    }
                }
            }

            return figureList;
        }

        public void Delete(List<Figure> figures)
        {
            foreach (Figure figure in figures)
            {
                if (FigureList.Remove(figure))
                {

                }
            }
        }
    }
}
