using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// GridSettingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GridSettingWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int rows;
        public int Rows
        {
            get => rows;
            set
            {
                rows = value;
                OnPropertyChanged("Rows");
            }
        }

        private int columns;
        public int Columns
        {
            get => columns;
            set
            {
                columns = value;
                OnPropertyChanged("Columns");
            }
        }

        private ICommand okCommand;
        public ICommand OKCommand => okCommand ?? (okCommand = new RelayCommand(() => DialogResult = true));

        private ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(() => DialogResult = false));

        public GridSettingWindow()
        {
            InitializeComponent();
        }
    }
}
