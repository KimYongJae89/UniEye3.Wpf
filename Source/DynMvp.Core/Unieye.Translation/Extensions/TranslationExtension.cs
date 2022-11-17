using System;
using System.Windows.Data;
using System.Windows.Markup;
using UniEye.Translation.Converters;
using UniEye.Translation.Helpers;

namespace UniEye.Translation.Extensions
{
    public class TranslationExtension : MarkupExtension
    {
        private object _obj;

        public TranslationExtension(object obj)
        {
            _obj = obj;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Converter = new TranslationConverter();
            multiBinding.Bindings.Add(new Binding("CurrentCultureInfo") { Source = TranslationHelper.Instance });

            if (_obj is string)
            {
                multiBinding.Bindings.Add(new Binding() { Source = _obj });
            }
            else if (_obj is Binding)
            {
                multiBinding.Bindings.Add(_obj as Binding);
            }

            return multiBinding.ProvideValue(serviceProvider);
        }
    }
}
