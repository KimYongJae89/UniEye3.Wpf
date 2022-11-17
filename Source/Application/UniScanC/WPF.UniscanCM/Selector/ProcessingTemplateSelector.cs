using System.Windows;
using System.Windows.Controls;
using WPF.UniScanCM.Enums;

namespace WPF.UniScanCM.Selector
{
    public class ProcessingTemplateSelector : DataTemplateSelector
    {
        public DataTemplate progressTemplate { get; set; }
        public DataTemplate successTemplate { get; set; }
        public DataTemplate failTemplate { get; set; }

        public ProcessingTemplateSelector() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            var state = (EProcessingState)item;

            switch (state)
            {
                case EProcessingState.Success:
                    return successTemplate;
                case EProcessingState.Fail:
                    return failTemplate;
                case EProcessingState.Running:
                    return progressTemplate;
            }

            return progressTemplate;
        }
    }
}
