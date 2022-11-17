using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace Unieye.WPF.Base.Converters
{
    public class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush brush))
            {
                return Colors.Transparent;
            }

            return brush.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush brush))
            {
                return Colors.Transparent;
            }

            return brush.Color;
        }
    }

    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var clr = (Color)value;
            return new SolidColorBrush(clr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var clr = (Color)value;
            return new SolidColorBrush(clr);
        }
    }

    public class DrawingColorToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (DrawingColor)value;
            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (DrawingColor)value;
            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }
    }

    public class MediaColorToDrawingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (MediaColor)value;
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (MediaColor)value;
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }
    }

    public class BrushPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percent = (double)parameter / 100;

            if (!(value is SolidColorBrush brush))
            {
                return Colors.Transparent;
            }

            return new SolidColorBrush(
                Color.FromArgb(
                    brush.Color.A,
                    (byte)(brush.Color.R * percent),
                    (byte)(brush.Color.G * percent),
                    (byte)(brush.Color.B * percent)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percent = (double)parameter / 100;

            if (!(value is SolidColorBrush brush))
            {
                return Colors.Transparent;
            }

            return new SolidColorBrush(
                Color.FromArgb(
                    brush.Color.A,
                    (byte)(brush.Color.R * percent),
                    (byte)(brush.Color.G * percent),
                    (byte)(brush.Color.B * percent)));
        }
    }

    public class BooleanColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush;

            var checkedBrush = Application.Current.Resources["CheckedBrush"] as SolidColorBrush;
            var uncheckedBrush = Application.Current.Resources["UncheckedBrush"] as SolidColorBrush;
            if ((bool)value)
            {
                brush = checkedBrush;
            }
            else
            {
                brush = uncheckedBrush;
            }

            if (parameter != null && (bool)parameter == true)
            {
                if (Equals(brush, checkedBrush))
                {
                    brush = uncheckedBrush;
                }
                else
                {
                    brush = checkedBrush;
                }
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
