using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;

namespace Unieye.WPF.Base.Selectors
{
    public class TypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EnumDataTemplate { get; set; }
        public DataTemplate BooleanDataTemplate { get; set; }
        public DataTemplate DefaultDataTemplate { get; set; }

        public TypeDataTemplateSelector()
        {

        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            object item2 = null;
            if (item is WTuple<PropertyInfo, object>)
            {
                var tuple = item as WTuple<PropertyInfo, object>;
                item2 = tuple.Item2;
            }
            else if (item is WTuple<string, object>)
            {
                var tuple = item as WTuple<string, object>;
                item2 = tuple.Item2;
            }

            if (item2 != null)
            {
                if (item2.GetType().IsEnum)
                {
                    return EnumDataTemplate;
                }
                else if (item2.GetType() == typeof(bool))
                {
                    return BooleanDataTemplate;
                }
                else
                {
                    return DefaultDataTemplate;
                }
            }

            return DefaultDataTemplate;
        }
    }
}
