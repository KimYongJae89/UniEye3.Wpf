using DynMvp.Devices.Comm;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using Unieye.WPF.Base.ViewModels;

namespace Unieye.WPF.Base.Views
{
    /// <summary>
    /// EncoderSettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EncoderSettingWindow
    {
        public EncoderSettingWindow(SerialEncoder serialEncoder)
        {
            InitializeComponent();

            var viewModel = new EncoderSettingViewModel(serialEncoder);
            DataContext = viewModel;
        }
    }
}
