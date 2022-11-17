using MahApps.Metro.SimpleChildWindow;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Windows.Views
{
    /// <summary>
    /// InputValueControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputValueWindowView : ChildWindow, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string text;
        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }

        public InputValueWindowView()
        {
            InitializeComponent();

            ApplyCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(true));

            CancelCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(false));
        }
    }
}
