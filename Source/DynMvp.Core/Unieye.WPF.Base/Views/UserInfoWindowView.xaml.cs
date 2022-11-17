using Authentication.Core;
using Authentication.Core.Enums;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;

namespace Unieye.WPF.Base.Views
{
    /// <summary>
    /// UserInfoWindowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfoWindowView : ChildWindow, INotifyPropertyChanged
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

        private UserHandler UserHandler { get; set; }

        private bool IsEditMode { get; set; } = false;

        private string userName;
        public string UserName
        {
            get => userName;
            set => Set(ref userName, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        private ObservableCollection<RolesModel> authorizes = new ObservableCollection<RolesModel>();
        public ObservableCollection<RolesModel> Authorizes
        {
            get => authorizes;
            set => Set(ref authorizes, value);
        }

        public ICommand CancelCommand { get; }
        public ICommand AcceptCommand { get; }

        public UserInfoWindowView(UserHandler userHandler, string userName = "", string password = "", Dictionary<string, bool> AuthDic = null)
        {
            InitializeComponent();

            UserHandler = userHandler;
            UserName = userName;
            Password = password;

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                IsEditMode = true;
            }

            if (AuthDic == null)
            {
                foreach (string type in Enum.GetNames(typeof(ERoleType)))
                {
                    if (type != "TaskSetting")
                    {
                        Authorizes.Add(new RolesModel(type, false));
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, bool> pair in AuthDic)
                {
                    Authorizes.Add(new RolesModel(pair.Key, pair.Value));
                }
            }

            AcceptCommand = new RelayCommand(async () =>
            {
                // 아이디가 비어있을 경우
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    string header = TranslationHelper.Instance.Translate("Error");
                    string message = TranslationHelper.Instance.Translate("USERNAME_MISSING_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);

                    return;
                }
                // 비어있지 않은 경우
                else
                {
                    // 아이디가 중복인 경우
                    if (!IsEditMode && UserHandler.ExistUserName(UserName))
                    {
                        string header = TranslationHelper.Instance.Translate("Error");
                        string message = TranslationHelper.Instance.Translate("USERNAME_EXIST_MESSAGE");
                        await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);

                        return;
                    }
                    // 중복이 아닌 경우
                    else
                    {
                        // 비밀번호가 비어있는 경우
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            string header = TranslationHelper.Instance.Translate("Error");
                            string message = TranslationHelper.Instance.Translate("PASSWORD_MISSING_MESSAGE");
                            await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);

                            return;
                        }
                        // 비어있지 않은 경우
                        else
                        {
                            // 비밀번호 적합성 여부 확인
                            if (IsPwdValidation(Password))
                            {
                                var authList = new Dictionary<string, bool>();
                                foreach (RolesModel auth in Authorizes)
                                {
                                    authList.Add(auth.RoleType, auth.IsAuth);
                                }

                                Close(new Tuple<string, string, Dictionary<string, bool>>(UserName, Password, authList));
                            }
                            else
                            {
                                string header = TranslationHelper.Instance.Translate("Error");
                                string message = TranslationHelper.Instance.Translate("PASSWORD_NOT_SUTABLE_MESSAGE");
                                await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);

                                return;
                            }
                        }
                    }
                }
            });

            CancelCommand = new RelayCommand(() => Close(null));
        }

        public bool IsPwdValidation(string pwd)
        {
            // 6자리 이상 패스워드입력 요청함.
            if (pwd.Length < 6)
            {
                return false;
            }

            var rxPassword = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", RegexOptions.IgnorePatternWhitespace);
            return rxPassword.IsMatch(pwd);
        }
    }

    public class RolesModel
    {
        public RolesModel(string type, bool v)
        {
            RoleType = type;
            IsAuth = v;
        }

        public string RoleType { get; set; }
        public bool IsAuth { get; set; }
    }
}
