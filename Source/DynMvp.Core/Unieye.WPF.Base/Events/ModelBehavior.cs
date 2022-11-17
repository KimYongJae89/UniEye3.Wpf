using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unieye.WPF.Base.Events
{
    public interface IModelPropertyEvent
    {
        void OnUpdateModel(ModelBase modelBase);
    }

    public class ModelBehavior
    {
        public static void SetModelBase(DependencyObject d, object value)
        {
            d.SetValue(ModelBaseProperty, value);
        }

        public static ModelBase GetModelBase(DependencyObject d)
        {
            return d.GetValue(ModelBaseProperty) as ModelBase;
        }

        public static readonly DependencyProperty ModelBaseProperty =
            DependencyProperty.RegisterAttached(
                "ModelBase",
                typeof(ModelBase),
                typeof(ModelBehavior),
                new FrameworkPropertyMetadata(ModelBaseProeprtyChanged));

        private static void ModelBaseProeprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as FrameworkElement;
            var dataContext = frame.DataContext as IModelPropertyEvent;
            dataContext?.OnUpdateModel(e.NewValue as ModelBase);
        }
    }
}
