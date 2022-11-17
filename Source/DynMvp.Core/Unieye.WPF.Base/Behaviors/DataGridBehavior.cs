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
    public class DataGridBehavior
    {
        #region DataGrid에서 SelectedItem이 Grid안에서 보이지 않으면 스크롤을 자동으로 이동하는 옵션
        /// <summary>
        /// Property : Autoscroll
        /// Type : bool
        /// </summary>

        #region Autoscroll Property 정의

        public static readonly DependencyProperty AutoscrollProperty =
            DependencyProperty.RegisterAttached("Autoscroll", typeof(bool), typeof(DataGridBehavior),
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
            if (!(dependencyObject is DataGrid dataGrid))
            {
                throw new InvalidOperationException("Dependency object is not DataGrid.");
            }

            if ((bool)args.NewValue)
            {
                dataGrid.SelectedCellsChanged += OnChangeSelectedItem;
            }
            else
            {
                dataGrid.SelectedCellsChanged -= OnChangeSelectedItem;
            }
        }

        private static void OnChangeSelectedItem(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (dataGrid.SelectedItem == null)
            {
                return;
            }

            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        #endregion

        #endregion
    }
}
