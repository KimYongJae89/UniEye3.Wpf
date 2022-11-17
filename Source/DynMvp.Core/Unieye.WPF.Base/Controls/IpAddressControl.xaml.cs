using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// IpAddressControl.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class IpAddressControl : INotifyPropertyChanged
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

        public static readonly DependencyProperty IpAddressProperty =
            DependencyProperty.Register("IpAddress", typeof(string), typeof(IpAddressControl),
                new FrameworkPropertyMetadata("", OnIpAddressChanged));

        public static void OnIpAddressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = d as IpAddressControl;
            parent.IpAddressToken(new string[] { "0", "0", "0", "0" });

            if (e.NewValue is string ipAddress)
            {
                string[] token = ipAddress.Split('.');
                parent.IpAddressToken(token);
            }
        }

        private void IpAddressToken(string[] token)
        {
            if (token.Length > 0)
            {
                ip1 = token[0];
            }

            if (token.Length > 1)
            {
                ip2 = token[1];
            }

            if (token.Length > 2)
            {
                ip3 = token[2];
            }

            if (token.Length > 3)
            {
                ip4 = token[3];
            }

            OnPropertyChanged("Ip1");
            OnPropertyChanged("Ip2");
            OnPropertyChanged("Ip3");
            OnPropertyChanged("Ip4");
        }

        public string IpAddress
        {
            get => (string)GetValue(IpAddressProperty);
            set => SetValue(IpAddressProperty, value);
        }

        private string ip1;
        public string Ip1
        {
            get => ip1;
            set
            {
                if (!CheckIpAddress(ref value))
                {
                    return;
                }

                if (Set(ref ip1, value))
                {
                    UpdateIpAddress();
                }
            }
        }

        private string ip2;
        public string Ip2
        {
            get => ip2;
            set
            {
                if (!CheckIpAddress(ref value))
                {
                    return;
                }

                if (Set(ref ip2, value))
                {
                    UpdateIpAddress();
                }
            }
        }

        private string ip3;
        public string Ip3
        {
            get => ip3;
            set
            {
                if (!CheckIpAddress(ref value))
                {
                    return;
                }

                if (Set(ref ip3, value))
                {
                    UpdateIpAddress();
                }
            }
        }

        private string ip4;
        public string Ip4
        {
            get => ip4;
            set
            {
                if (!CheckIpAddress(ref value))
                {
                    return;
                }

                if (Set(ref ip4, value))
                {
                    UpdateIpAddress();
                }
            }
        }

        private bool toggleFlag = false;
        public bool ToggleFlag
        {
            get => toggleFlag;
            set => Set(ref toggleFlag, value);
        }

        private bool CheckIpAddress(ref string s)
        {
            if (s == string.Empty)
            {
                s = "0";
                return true;
            }

            if (!int.TryParse(s, out int ip))
            {
                return false;
            }

            if (ip < 0 || ip > 255)
            {
                return false;
            }

            ip = Math.Min(255, Math.Max(0, ip));
            s = ip.ToString();

            return true;
        }

        private ICommand keyDownCommand;
        public ICommand KeyDownCommand => keyDownCommand ?? (keyDownCommand = new RelayCommand<TextBox>(OnKeyDown));

        private void OnKeyDown(TextBox textBox)
        {
            if (((Keyboard.GetKeyStates(Key.LeftShift) == (KeyStates.Down | KeyStates.Toggled)) || (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.Down))
                && Keyboard.IsKeyDown(Key.Tab))
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                ToggleFlag = true;
            }
            else if (Keyboard.IsKeyDown(Key.Tab) || Keyboard.IsKeyDown(Key.OemPeriod) || Keyboard.IsKeyDown(Key.Decimal))
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                ToggleFlag = true;
            }
        }

        private ICommand keyUpCommand;
        public ICommand KeyUpCommand => keyUpCommand ?? (keyUpCommand = new RelayCommand<TextBox>(OnKeyUp));

        private void OnKeyUp(TextBox textBox)
        {
            if (ToggleFlag && Keyboard.IsKeyUp(Key.Tab))
            {
                ToggleFlag = false;
                textBox.AcceptsTab = false;
                textBox.SelectAll();
            }
        }

        private void UpdateIpAddress()
        {
            IpAddress = string.Format("{0}.{1}.{2}.{3}",
                Ip1,
                Ip2,
                Ip3,
                Ip4);
        }

        public IpAddressControl()
        {
            InitializeComponent();
        }
    }
}
