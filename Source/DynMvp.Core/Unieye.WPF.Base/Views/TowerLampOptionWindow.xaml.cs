using DynMvp.Devices;
using MahApps.Metro.Controls;
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
using System.Windows.Threading;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Views
{
    /// <summary>
    /// TowerLampOptionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TowerLampOptionWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public Lamp lamp;
        //public Lamp Lamp
        //{
        //    get => lamp;
        //    set
        //    {
        //        if (Equals(lamp, value))
        //            return;

        //        lamp = value;
        //        OnPropertyChanged("Lamp");
        //    }
        //}
        public Lamp Lamp { get; set; }


        #region Command

        private ICommand lampOffCommand;
        public ICommand LampOffCommand => lampOffCommand ?? (lampOffCommand = new RelayCommand(LampOff));

        private void LampOff()
        {
            Close(TowerLampValue.Off);
        }

        private ICommand lampOnCommand;
        public ICommand LampOnCommand => lampOnCommand ?? (lampOnCommand = new RelayCommand(LampOn));

        private void LampOn()
        {
            Close(TowerLampValue.On);
        }

        private ICommand lampBlinkCommand;
        public ICommand LampBlinkCommand => lampBlinkCommand ?? (lampBlinkCommand = new RelayCommand(LampBlink));

        private void LampBlink()
        {
            Close(TowerLampValue.Blink);
        }

        private void Close(TowerLampValue lampValue)
        {
            Lamp.Value = lampValue;
            DialogResult = true;
        }

        #endregion

        public TowerLampOptionWindow(Lamp _lamp)
        {
            InitializeComponent();

            Lamp = _lamp;
            DataContext = this;
        }
    }
}
