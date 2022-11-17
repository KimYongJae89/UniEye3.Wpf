using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UniEye.StringManagement.Helpers
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
                dataGrid.SelectedCellsChanged += OnAutoscrollChangeSelectedItem;
            }
            else
            {
                dataGrid.SelectedCellsChanged -= OnAutoscrollChangeSelectedItem;
            }
        }

        private static void OnAutoscrollChangeSelectedItem(object sender, SelectedCellsChangedEventArgs e)
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

        #region DataGrid에서 여러개의 셀을 선택하는 옵션
        /// <summary>
        /// Property : SelectedItems
        /// Type : IEnumerable<object>
        /// </summary>

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IEnumerable<object>), typeof(DataGridBehavior),
                new PropertyMetadata(new List<object>(), SelectedItemsChangedCallback));

        public static void SetSelectedItems(DependencyObject element, IEnumerable<object> value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IEnumerable<object> GetSelectedItems(DependencyObject element)
        {
            return (IEnumerable<object>)element.GetValue(SelectedItemsProperty);
        }

        #region SelectedItems Property Method

        private static void SelectedItemsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is DataGrid dataGrid))
            {
                throw new InvalidOperationException("Dependency object is not DataGrid.");
            }

            dataGrid.SelectedCellsChanged -= OnChangeSelectedItems;
            dataGrid.SelectedCellsChanged += OnChangeSelectedItems;
        }

        private static void OnChangeSelectedItems(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (dataGrid.SelectedItem == null)
            {
                return;
            }

            IEnumerable<object> selectedItmes = GetSelectedItems(dataGrid);

            List<object> selectedItemList = null;
            if (selectedItmes == null)
            {
                selectedItemList = new List<object>();
            }
            else
            {
                selectedItemList = selectedItmes.ToList();
            }

            List<object> addList = GetItemListFromCells(e.AddedCells);
            List<object> removeLIst = GetItemListFromCells(e.RemovedCells);

            selectedItemList.AddRange(addList);

            foreach (object removeItem in removeLIst)
            {
                selectedItemList.Remove(removeItem);
            }

            SetSelectedItems(dataGrid, selectedItemList);
        }

        public static List<object> GetItemListFromCells(IList<DataGridCellInfo> cells)
        {
            var list = new List<object>();
            foreach (DataGridCellInfo cellInfo in cells)
            {
                DataGridCell cell = DataGridCellFromDataGridCellInfo(cellInfo);
                if (cell != null)
                {
                    int rowIndex = DataGridRow.GetRowContainingElement(cell).GetIndex();
                    list.Add(cellInfo.Item);
                }
            }

            list = list.Distinct().ToList();

            return list;
        }

        public static DataGridCell DataGridCellFromDataGridCellInfo(DataGridCellInfo cellInfo)
        {
            FrameworkElement cellContent = cellInfo.Column?.GetCellContent(cellInfo.Item);
            if (cellContent != null)
            {
                return (DataGridCell)cellContent.Parent;
            }

            return null;
        }

        #endregion

        #endregion
    }
}
