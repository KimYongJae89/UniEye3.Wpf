using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unieye.WPF.Base.Events
{
    public interface IInspectPropertyEvent
    {
        void OnUpdateResult(IEnumerable<ProductResult> productResults);
    }

    public class InspectBehavior
    {
        public static void SetProductResults(DependencyObject d, object value)
        {
            d.SetValue(ProductResultsProperty, value);
        }

        public static IEnumerable<ProductResult> GetProductResults(DependencyObject d)
        {
            return d.GetValue(ProductResultsProperty) as IEnumerable<ProductResult>;
        }

        public static readonly DependencyProperty ProductResultsProperty =
            DependencyProperty.RegisterAttached(
                "ProductResults",
                typeof(IEnumerable<ProductResult>),
                typeof(InspectBehavior),
                new FrameworkPropertyMetadata(ProductResultsProeprtyChanged));

        private static void ProductResultsProeprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as FrameworkElement;
            var dataContext = frame.DataContext as IInspectPropertyEvent;
            dataContext?.OnUpdateResult(e.NewValue as IEnumerable<ProductResult>);
        }
    }
}
