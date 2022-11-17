using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Layout.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class LayoutControlAttribute : Attribute
    {
        public LayoutControlAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LayoutControlViewModelPropertyAttribute : Attribute
    {
        public LayoutControlViewModelPropertyAttribute() { }
    }
}
