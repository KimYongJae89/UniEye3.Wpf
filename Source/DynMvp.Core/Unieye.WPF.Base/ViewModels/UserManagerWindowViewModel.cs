using Authentication.Core;
using Authentication.Core.Datas;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Views;
using UniEye.Translation.Helpers;

namespace Unieye.WPF.Base.ViewModels
{
    public class UserManagerWindowViewModel : Observable
    {
        public UserHandler UserHandler { get; set; }

        private List<User> users = new List<User>();
        public List<User> Users
        {
            get => users;
            set => Set(ref users, value);
        }

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set => Set(ref selectedUser, value);
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }

        public UserManagerWindowViewModel(UserHandler userHandler)
        {
            UserHandler = userHandler;
            Users = userHandler.Users;

            AddCommand = new RelayCommand(async () =>
            {
                var view = new UserInfoWindowView(UserHandler);
                Tuple<string, string, Dictionary<string, bool>> tuple = await MessageWindowHelper.ShowChildWindow<Tuple<string, string, Dictionary<string, bool>>>(view);
                if (tuple != null)
                {
                    UserHandler.AddUser(new User(tuple.Item1, tuple.Item2, tuple.Item3));
                    Refresh();
                }
            });

            RemoveCommand = new RelayCommand(async () =>
            {
                string header = TranslationHelper.Instance.Translate("Remove");
                string message = TranslationHelper.Instance.Translate("USER_DELETE_WARNING_MESSAGE");
                if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
                {
                    UserHandler.RemoveUser(SelectedUser);
                    Refresh();
                }
            });

            ApplyCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(UserHandler));

            CancelCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close(null));
        }

        public void Refresh()
        {
            Users = null;
            Users = UserHandler.Users;
        }
    }
}
