using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;
using Point = System.Windows.Point;

namespace Unieye.WPF.Base.Services
{
    public class MouseWheelUpDown : MouseGesture
    {
        public enum MouseEventType
        {
            None,
            WheelUp,
            WheelDown,
        };

        public MouseEventType mouseEventType { get; set; }

        public MouseWheelUpDown() : base()
        {
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if ((inputEventArgs is MouseWheelEventArgs))
            {
                var args = (MouseWheelEventArgs)inputEventArgs;

                if (args.Delta > 0)
                {
                    mouseEventType = MouseEventType.WheelUp;
                }
                else
                {
                    mouseEventType = MouseEventType.WheelDown;
                }
            }
            else
            {
                mouseEventType = MouseEventType.None;
            }

            return mouseEventType != MouseEventType.None;
        }
    }

    public class ZoomService : Observable
    {
        public delegate void MousePointDelegate(Point pt);
        public delegate void MouseDragRegionDelegate(Rect region);

        public MousePointDelegate MouseLeftDown;
        public MousePointDelegate MouseLeftUp;
        public MouseDragRegionDelegate MouseDragRegion;

        protected FrameworkElement element;
        public FrameworkElement Element
        {
            get => element;
            set => element = value;
        }

        private const double _defaultOverlayThickness = 2;
        private const double _defaultFontThickness = 24;
        private double _overlayThickness;
        public double OverlayThickness
        {
            get => _overlayThickness;
            set
            {
                if (value < 1)
                {
                    return;
                }

                Set(ref _overlayThickness, value);
            }
        }

        private double _fontThickness;
        public double FontThickness
        {
            get => _fontThickness;
            set
            {
                if (value < 1)
                {
                    return;
                }

                if (value > 35791.3940666667)
                {
                    return;
                }

                Set(ref _fontThickness, value);
            }
        }

        private double _scale = 1;
        public double Scale
        {
            get => _scale;
            set
            {
                Set(ref _scale, value);
                OverlayThickness = _defaultOverlayThickness / _scale;
                FontThickness = _defaultFontThickness / _scale;
            }
        }

        private double _translateX;
        public double TranslateX
        {
            get => _translateX;
            set => Set(ref _translateX, value);
        }

        private double _translateY;
        public double TranslateY
        {
            get => _translateY;
            set => Set(ref _translateY, value);
        }

        private Point leftMousePointDownStart;
        public Point LeftMousePointDownStart
        {
            get => leftMousePointDownStart;
            set => Set(ref leftMousePointDownStart, value);
        }

        private Point leftMousePointDownEnd;
        public Point LeftMousePointDownEnd
        {
            get => leftMousePointDownEnd;
            set => Set(ref leftMousePointDownEnd, value);
        }

        private bool isClick = false;

        private Point rightMousePointDownStart;
        public Point RightMousePointDownStart
        {
            get => rightMousePointDownStart;
            set
            {
                Set(ref rightMousePointDownStart, value);
                isClick = true;
            }
        }

        private Point rightMousePointDownEnd;
        public Point RightMousePointDownEnd
        {
            get => rightMousePointDownEnd;
            set
            {
                if (isClick)
                {
                    Set(ref rightMousePointDownEnd, value);
                    Offset();
                }
            }
        }

        protected virtual void Offset()
        {
            double offsetX = (RightMousePointDownEnd.X - RightMousePointDownStart.X) * Scale;
            double offsetY = (RightMousePointDownEnd.Y - RightMousePointDownStart.Y) * Scale;

            TranslateX += offsetX;
            TranslateY += offsetY;
        }

        private double _xOffset;
        private double _yOffset;
        private ICommand zoomInCommand;
        private ICommand zoomOutCommand;
        private ICommand wheelCommand;
        private ICommand leftMouseDownCommand;
        private ICommand leftMouseUpCommand;
        private ICommand rightMouseDownCommand;
        private ICommand rightMouseUpCommand;
        private ICommand mouseMoveCommand;

        public ICommand ZoomInCommand => zoomInCommand ?? (zoomInCommand = new RelayCommand(() => ZoomIn()));
        public ICommand ZoomOutCommand => zoomOutCommand ?? (zoomOutCommand = new RelayCommand(() => ZoomOut()));
        public ICommand WheelCommand => wheelCommand ?? (wheelCommand = new RelayCommand<MouseWheelUpDown>(Wheel));

        public ICommand LeftMouseDownCommand => leftMouseDownCommand ?? (leftMouseDownCommand = new RelayCommand(LeftMouseDown));
        public ICommand LeftMouseUpCommand => leftMouseUpCommand ?? (leftMouseUpCommand = new RelayCommand(LeftMouseUp));

        public ICommand RightMouseDownCommand => rightMouseDownCommand ?? (rightMouseDownCommand = new RelayCommand(RightMouseDown));
        public ICommand RightMouseUpCommand => rightMouseUpCommand ?? (rightMouseUpCommand = new RelayCommand(RightMouseUp));

        public ICommand MouseMoveCommand => mouseMoveCommand ?? (mouseMoveCommand = new RelayCommand(MouseMove));

        private void Wheel(MouseWheelUpDown gesture)
        {
            double factor = 1;
            switch (gesture.mouseEventType)
            {
                case MouseWheelUpDown.MouseEventType.None:
                    break;
                case MouseWheelUpDown.MouseEventType.WheelUp:
                    factor = 1.1f;
                    break;
                case MouseWheelUpDown.MouseEventType.WheelDown:
                    factor = 0.9f;
                    break;
            }

            Point pt = Mouse.GetPosition(element);
            ExecuteZoom(pt.X, pt.Y, factor);
        }

        private void LeftMouseDown()
        {
            LeftMousePointDownStart = Mouse.GetPosition(element);
            MouseLeftDown?.Invoke(LeftMousePointDownStart);
        }

        private void LeftMouseUp()
        {
            LeftMousePointDownEnd = Mouse.GetPosition(element);
            MouseLeftUp?.Invoke(LeftMousePointDownEnd);

            var rect = new Rect(LeftMousePointDownStart, LeftMousePointDownEnd);
            if (rect.Width < 1 || rect.Height < 1)
            {
                return;
            }

            MouseDragRegion?.Invoke(rect);
        }

        private void RightMouseDown()
        {
            Point pt = Mouse.GetPosition(element);

            RightMousePointDownStart = pt;
            RightMousePointDownEnd = pt;

            element.CaptureMouse();
        }

        private void MouseMove()
        {
            Point pt = Mouse.GetPosition(element);

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                RightMousePointDownEnd = pt;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                LeftMousePointDownEnd = pt;
                MouseLeftUp?.Invoke(LeftMousePointDownEnd);
            }
        }

        private void RightMouseUp()
        {
            isClick = false;

            element.ReleaseMouseCapture();
        }

        public ZoomService(double xOffset = 0, double yOffset = 0)
        {
            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        public ZoomService(FrameworkElement _element, double xOffset = 0, double yOffset = 0)
        {
            element = _element;
            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        public void FitToSize(double width, double height)
        {
            double scaleX = element.ActualWidth / width;
            double scaleY = element.ActualHeight / height;

            Scale = Math.Min(scaleX, scaleY);

            if (scaleX < scaleY)
            {
                TranslateX = -_xOffset * Scale;
                TranslateY = (element.ActualHeight - (height * Scale)) / 2 - _yOffset * Scale;
            }
            else
            {
                TranslateX = (element.ActualWidth - (width * Scale)) / 2 - _xOffset * Scale;
                TranslateY = -_yOffset * Scale;
            }
        }

        private void ZoomIn()
        {
            ExecuteZoom((element.ActualWidth) / 2, (element.ActualHeight) / 2, 1.1f);
        }

        private void ZoomOut()
        {
            ExecuteZoom((element.ActualWidth) / 2, (element.ActualHeight) / 2, 0.9f);
        }

        public virtual void ExecuteZoom(double centerX, double centerY, double zoomFactor)
        {
            double prevX = centerX * Scale;
            double prevY = centerY * Scale;

            Scale *= zoomFactor;

            double nextX = centerX * Scale;
            double nextY = centerY * Scale;

            TranslateX += (prevX - nextX);
            TranslateY += (prevY - nextY);
        }

        public void Zoom(Rectangle viewRegion)
        {
            int centerX = viewRegion.X + (viewRegion.Width / 2);
            int centerY = viewRegion.Y + (viewRegion.Height / 2);

            double scaleX = element.ActualWidth / viewRegion.Width;
            double scaleY = element.ActualHeight / viewRegion.Height;

            TranslateX = -viewRegion.X;
            TranslateY = -viewRegion.Y;
            Scale = Math.Min(scaleX, scaleY);
        }
    }
}
