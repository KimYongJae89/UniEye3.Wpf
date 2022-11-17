using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Unieye.WPF.Base.Behaviors
{
    public class ListBoxBehavior
    {
        #region ListBox에서 SelectedItem이 보이지 않으면 스크롤을 자동으로 이동하는 옵션
        /// <summary>
        /// Property : Autoscroll
        /// Type : bool
        /// </summary>

        #region Autoscroll Property 정의

        public static readonly DependencyProperty AutoscrollProperty =
            DependencyProperty.RegisterAttached("Autoscroll", typeof(bool), typeof(ListBoxBehavior),
                new PropertyMetadata(default(bool), AutoscrollChangedCallback));

        public static void SetAutoscroll(DependencyObject element, bool value)
        {
            element.SetValue(AutoscrollProperty, value);
        }

        public static bool GetAutoscroll(DependencyObject element)
        {
            return (bool)element.GetValue(AutoscrollProperty);
        }

        #endregion

        #region Autoscroll Property Method

        private static void AutoscrollChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is ListBox ListBox))
            {
                throw new InvalidOperationException("Dependency object is not ListBox.");
            }

            if ((bool)args.NewValue)
            {
                ListBox.SelectionChanged += OnChangeSelectedItem;
            }
            else
            {
                ListBox.SelectionChanged -= OnChangeSelectedItem;
            }
        }

        private static void OnChangeSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            var ListBox = sender as ListBox;

            if (ListBox.SelectedItem == null)
            {
                return;
            }

            ListBox.ScrollIntoView(ListBox.SelectedItem);
        }

        #endregion

        #endregion
    }
}
