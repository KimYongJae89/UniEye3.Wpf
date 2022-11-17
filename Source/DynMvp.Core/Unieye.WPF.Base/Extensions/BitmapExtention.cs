using System;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Markup;
using Unieye.WPF.Base.Converters;

namespace Unieye.WPF.Base.Extensions
{
    public class BitmapExtention : MarkupExtension
    {
        private Bitmap _bitmap;

        public BitmapExtention(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding()
            {
                Source = _bitmap,
                Converter = new BitmapToImageSourceConverter()
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
