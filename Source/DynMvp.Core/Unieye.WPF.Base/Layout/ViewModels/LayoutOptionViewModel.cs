using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;

namespace Unieye.WPF.Base.Layout.ViewModels
{
    public class LayoutOptionViewModel : Observable
    {
        private int row;
        public int Row
        {
            get => row;
            set => Set(ref row, value);
        }

        private int rowSpan;
        public int RowSpan
        {
            get => rowSpan;
            set => Set(ref rowSpan, value);
        }

        private int column;
        public int Column
        {
            get => column;
            set => Set(ref column, value);
        }

        private int columnSpan;
        public int ColumnSpan
        {
            get => columnSpan;
            set => Set(ref columnSpan, value);
        }

        private List<WTuple<PropertyInfo, object>> propertyList = new List<WTuple<PropertyInfo, object>>();
        public List<WTuple<PropertyInfo, object>> PropertyList
        {
            get => propertyList;
            set => Set(ref propertyList, value);
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public LayoutOptionViewModel(LayoutModel model)
        {
            OkCommand = new RelayCommand<ChildWindow>(OKCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            foreach (KeyValuePair<PropertyInfo, object> pair in model.ControlViewModel.PropertyList)
            {
                PropertyList.Add(new WTuple<PropertyInfo, object>(pair.Key, pair.Value));
            }

            Column = model.Column;
            ColumnSpan = model.ColumnSpan;
            Row = model.Row;
            RowSpan = model.RowSpan;
        }

        private void OKCommandAction(ChildWindow wnd)
        {
            string exceptionMessage = "";

            try
            {
                foreach (WTuple<PropertyInfo, object> pair in propertyList)
                {
                    Convert.ChangeType(pair.Item2, pair.Item1.PropertyType);
                }

                wnd.Close(true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                MessageBox.Show(exceptionMessage);
            }
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(false);
        }
    }
}
