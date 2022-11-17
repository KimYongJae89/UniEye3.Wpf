using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
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
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Models;

namespace Unieye.WPF.Base.Controls
{
    public class ModelWindowResult
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelWindow : ChildWindow
    {
        public ModelWindowResult Result { get; } = new ModelWindowResult();

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        private ICommand _acceptCommand;
        public ICommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(Accept));

        public ModelWindow()
        {
            InitializeComponent();
        }

        private async void Accept()
        {
            if (string.IsNullOrEmpty(Result.Name) || string.IsNullOrWhiteSpace(Result.Name))
            {
                await ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync(
                    UniEye.Translation.Helpers.TranslationHelper.Instance.Translate("Error"),
                    UniEye.Translation.Helpers.TranslationHelper.Instance.Translate("ModelWindow_Error"));

                return;
            }

            Close(Result);
        }

        private void Cancel()
        {
            Close();
        }
    }
}
