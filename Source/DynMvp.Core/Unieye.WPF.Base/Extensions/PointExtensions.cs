using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unieye.WPF.Base.Extensions
{
    public static class PointExtensions
    {
        public static bool Within(this Point pt, Rect r)
        {
            return pt.X >= r.X &&
               pt.Y >= r.Y &&
               pt.X < r.Width &&
               pt.Y < r.Height;
        }

        public static bool Within(this Point pt, Size s)
        {
            return pt.Within(new Rect(s));
        }

    }
}
