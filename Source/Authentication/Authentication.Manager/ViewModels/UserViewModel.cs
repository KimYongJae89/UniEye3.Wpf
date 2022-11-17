using Authentication.Core.Datas;
using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Services;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;

namespace Authentication.Manager.ViewModels
{
    public class UserViewModel : Observable
    {
        public IEnumerable<User> Users => _authenticationService.Users;
        public IEnumerable<Role> Roles => _authenticationService.Roles;

        User _currentUser = new User();
        public User CurrentUser
        {
            get => _currentUser;
            set => Set(ref _currentUser, value);
        }

        Role _currentRole = new Role();
        public Role CurrentRole
        {
            get => _currentRole;
            set
            {
                Set(ref _currentRole, value);
                //CurrentUser.UserRoleType = value.GetRoleType();
            }
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<User> RemoveCommand { get; private set; }

        AuthenticationService _authenticationService;
        ILoggerFacade<UserViewModel> _logger;

        public PasswordBox PasswordBox { get; set; }
        public PasswordBox ConfirmPasswordBox { get; set; }

        public UserViewModel(AuthenticationService authenticationService, ILoggerFacade<UserViewModel> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;

            AddCommand = new DelegateCommand(Add);
            RemoveCommand = new DelegateCommand<User>((user) => _authenticationService.RemoveUser(user));

            _logger.Info("Initialized");
        }

        private void Add()
        {
            if (string.IsNullOrEmpty(CurrentUser.UserId)
                || string.IsNullOrEmpty(PasswordBox.Password)
                || string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                _logger.Warn("Exist Empty Space");
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                _logger.Warn("Password != ConfirmPassword");
                return;
            }

            var tempUser = new User(CurrentUser.UserId, PasswordBox.Password);
            tempUser.UserRoleType = CurrentUser.UserRoleType;

            if (_authenticationService.AddUser(tempUser))
                _logger.Info("Add User");
            else
                _logger.Warn("Exist User");

            Clear();
        }

        private void Clear()
        {
            CurrentUser = new User();
            CurrentRole = new Role();
            PasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }
    }
}
