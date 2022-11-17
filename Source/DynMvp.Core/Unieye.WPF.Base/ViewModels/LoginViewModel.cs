using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
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
    public class LoginViewModel : Observable
    {
        #region 변수

        private string userAccount = "";
        public string UserAccount
        {
            get => userAccount;
            set => Set(ref userAccount, value);
        }

        private string userPassword = "";
        public string UserPassword
        {
            get => userPassword;
            set => Set(ref userPassword, value);
        }

        private bool canChangeOption = true;
        public bool CanChangeOption
        {
            get => canChangeOption;
            set => Set(ref canChangeOption, value);
        }

        #endregion

        #region Command

        private System.Windows.Input.ICommand loginButtonClick;
        public System.Windows.Input.ICommand LoginButtonClick => loginButtonClick ?? (loginButtonClick = new RelayCommand<Window>(Login));

        private async void Login(Window wnd)
        {
            Authentication.Core.Datas.User loginUser = UserHandler.Instance.GetUser(UserAccount, UserPassword);
            if (loginUser != null)
            {
                UserHandler.Instance.CurrentUser = loginUser;
                LogHelper.Info(LoggerType.Operation, $"[Main Page] User Change : [{loginUser.UserId}]");
                if (CanChangeOption == false)
                {
                    wnd.DialogResult = true;
                    wnd.Close();
                }
                else
                {
                    var configWindow = new ConfigWindow();
                    configWindow.ShowDialog();
                }
            }
            else
            {
                wnd.Hide();
                string header = TranslationHelper.Instance.Translate("Warning");
                string message = TranslationHelper.Instance.Translate("USERINFO_WARNING_MESSAGE");
                await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                wnd.ShowDialog();
            }
        }

        private System.Windows.Input.ICommand optionsButtonClick;
        public System.Windows.Input.ICommand OptionsButtonClick => optionsButtonClick ?? (optionsButtonClick = new RelayCommand<Window>(Login));

        private System.Windows.Input.ICommand cancelButtonClick;
        public System.Windows.Input.ICommand CancelButtonClick => cancelButtonClick ?? (cancelButtonClick = new RelayCommand<Window>(CancelButtonClickAction));

        private void CancelButtonClickAction(Window wnd)
        {
            wnd.DialogResult = false;
            wnd.Close();
        }

        private System.Windows.Input.ICommand keyPressCommand;
        public System.Windows.Input.ICommand KeyPressCommand => keyPressCommand ?? (keyPressCommand = new RelayCommand<Window>(KeyPress));

        private void KeyPress(Window window)
        {
            Login(window);
        }

        private System.Windows.Input.ICommand userManagerButtonClick;
        public System.Windows.Input.ICommand UserManagerButtonClick => userManagerButtonClick ?? (userManagerButtonClick = new RelayCommand<Window>(UserManagerButtonClickAction));

        private async void UserManagerButtonClickAction(Window wnd)
        {
            wnd.Hide();
            Authentication.Core.Datas.User loginUser = UserHandler.Instance.GetUser(UserAccount, UserPassword);
            if (loginUser != null && loginUser.IsAuth(ERoleType.UserSetting))
            {
                var view = new UserManagerWindowView();
                view.DataContext = new UserManagerWindowViewModel(UserHandler.Instance);
                UserHandler result = await MessageWindowHelper.ShowChildWindow<UserHandler>(view);
                if (result != null)
                {
                    result.Save();
                }
                UserHandler.Instance.Load();
            }
            else if (loginUser == null)
            {
                string header = TranslationHelper.Instance.Translate("Warning");
                string message = TranslationHelper.Instance.Translate("USERINFO_WARNING_MESSAGE");
                await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
            }
            else if (loginUser.IsAuth(ERoleType.UserSetting) == false)
            {
                string header = TranslationHelper.Instance.Translate("Warning");
                string message = TranslationHelper.Instance.Translate("USERAUTHO_WARNING_MESSAGE");
                await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
            }
            wnd.ShowDialog();
        }

        #endregion

        public LoginViewModel(bool canChangeOption = true)
        {
            CanChangeOption = canChangeOption;
#if DEBUG
            UserAccount = "developer";
            UserPassword = "masterkey";
#endif
        }
    }
}
