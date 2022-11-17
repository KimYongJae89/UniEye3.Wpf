using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Unieye.WPF.Base.Helpers
{
    public class UIElementDropAdorner : Adorner
    {
        public UIElementDropAdorner(UIElement adornedElement) : base(adornedElement)
        {
            Focusable = false;
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UIElement container = AdornedElement;

            if (AdornedElement is Selector)
            {
                var selector = AdornedElement as Selector;
                object item = selector.SelectedItem;
                if (item != null)
                {
                    container = selector.ItemContainerGenerator.ContainerFromItem(selector.SelectedItem) as UIElement;
                }
            }

            var visualBrush = new VisualBrush(container);

            var adornedRect = new Rect(500, 500, 100, 100);
            drawingContext.DrawRectangle(visualBrush, null, adornedRect);
            //BitmapImage image = null;
            //var image = new BitmapImage(
            //      new Uri("pack://application:,,,/DragAndDropBehavior;component/Resources/dropfolder.png",
            //      UriKind.Absolute));

            //var typeface = new Typeface(
            //      new FontFamily("Segoe UI"),
            //         FontStyles.Normal,
            //         FontWeights.Normal, FontStretches.Normal);

            //var formattedText = new FormattedText(
            //      "Drop Items Here",
            //      CultureInfo.CurrentUICulture,
            //      FlowDirection.LeftToRight,
            //      typeface,
            //      24,
            //      Brushes.LightGray);

            //var centre = new Point(
            //      AdornedElement.RenderSize.Width / 2,
            //      AdornedElement.RenderSize.Height / 2);

            //var top = centre.Y - (image.Height + formattedText.Height) / 2;

            //var textLocation = new Point(
            //      centre.X - formattedText.WidthIncludingTrailingWhitespace / 2,
            //      top + image.Height);

            //drawingContext.DrawImage(image,
            //      new Rect(centre.X - image.Width / 2,
            //      top,
            //      image.Width,
            //      image.Height));

            //drawingContext.DrawText(formattedText, textLocation);
        }
    }
}
