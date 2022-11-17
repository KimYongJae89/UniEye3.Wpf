using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Models
{
    public enum FigureType
    {
        Point,
        Line,
        Rectangle,
        Ellipse,
    }

    public class FigureModel : Observable
    {
        private FigureType figureType;

        protected Point startPoint;
        protected Point endPoint;
        private double x, y, width, height;

        private IEnumerable<Point> points;
        private Color lineColor = Color.FromRgb(255, 0, 0);
        private int thickness = 1;
        private Color fillColor = Color.FromRgb(255, 0, 0);

        //private SolidColorBrush lineBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        //private SolidColorBrush fillBrush = new SolidColorBrush();

        public FigureType FigureType
        {
            get => figureType;
            set => Set(ref figureType, value);
        }

        public virtual Point StartPoint
        {
            get => startPoint;
            set => Set(ref startPoint, value);
        }

        public virtual Point EndPoint
        {
            get => endPoint;
            set
            {
                Set(ref endPoint, value);
                if (FigureType == FigureType.Point ||
                    FigureType == FigureType.Line)
                {
                    X = 0;
                    Y = 0;
                    Width = EndPoint.X;
                    Height = EndPoint.Y;
                }
                else
                {
                    X = Math.Min(StartPoint.X, EndPoint.X);
                    Y = Math.Min(StartPoint.Y, EndPoint.Y);
                    Width = Math.Abs(EndPoint.X - StartPoint.X);
                    Height = Math.Abs(EndPoint.Y - StartPoint.Y);
                }
            }
        }

        public double X
        {
            get => x;
            set => Set(ref x, value);
        }

        public double Y
        {
            get => y;
            set => Set(ref y, value);
        }

        public double Width
        {
            get => width;
            set => Set(ref width, value);
        }

        public double Height
        {
            get => height;
            set => Set(ref height, value);
        }

        public IEnumerable<Point> Points
        {
            get => points;
            set => Set(ref points, value);
        }

        public Color LineColor
        {
            get => lineColor;
            set => Set(ref lineColor, value);//LineBrush.Color = lineColor;
        }

        public int Thickness
        {
            get => thickness;
            set => Set(ref thickness, value);
        }

        public Color FillColor
        {
            get => fillColor;
            set => Set(ref fillColor, value);//FillBrush.Color = fillColor;
        }

        //public SolidColorBrush LineBrush
        //{
        //    get => lineBrush;
        //    set => Set(ref lineBrush, value);
        //}

        //public SolidColorBrush FillBrush
        //{
        //    get => fillBrush;
        //    set => Set(ref fillBrush, value);
        //}

        private List<Point> pointList = new List<Point>();
        public void AddPoint(Point pt)
        {
            pointList.Add(pt);
            Points = pointList.ToArray();
        }
    }
}
