using Authentication.Core.Database;
using Authentication.Core.Datas;
using Authentication.Core.Sources;
using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using UniEye.Translation.Helpers;

namespace Authentication.Manager.Services
{
    public class AuthenticationService
    {
        IUserSource _userSource;

        ILoggerFacade<AuthenticationService> _logger;
        
        ICollection<User> _users { get; } = new ObservableCollection<User>();
        public IEnumerable<User> Users => _users;

        public AuthenticationService(ILoggerFacade<AuthenticationService> logger)
        {
            _logger = logger;

            BindingOperations.EnableCollectionSynchronization(_users, new object());

            Load();

            _logger.Info("Initialized");
        }

        public bool Load()
        {
            try
            {
                var type = (AuthDatabaseType)Enum.Parse(typeof(AuthDatabaseType), Settings.Default.DbType);

                switch (type)
                {
                    case AuthDatabaseType.File:
                        _userSource = new UserXmlSource(
                            Path.Combine(Environment.CurrentDirectory, Settings.Default.FileUserPath))
                            .LoadUsers(_users.ToList());
                        break;
                }

                _logger.Info("Db Loading Success");

                return true;
            }
            catch (Exception e)
            {
                _logger.Exception($"Db Loading Fail ( exception - {e.Message}");

                return false;
            }
        }

        public void Save()
        {
            _userSource?.SaveUsers(_users.ToList());

            Settings.Default.Culture = TranslationHelper.Instance.CurrentCultureInfo?.Name;
            Settings.Default.Save();

            _logger.Info("Save");
        }

        public bool ChangeDbType(AuthDatabaseType type)
        {
            try
            {
                switch (type)
                {
                    case AuthDatabaseType.File:
                        _userSource = new UserXmlSource(
                            Path.Combine(Environment.CurrentDirectory, Settings.Default.FileUserPath),
                            Path.Combine(Environment.CurrentDirectory, Settings.Default.FileRolePath));
                        break;
                }

                _logger.Info("Db Loading Success");

                return true;
            }
            catch (Exception e)
            {
                _logger.Exception($"Db Loading Fail ( exception - {e.Message}");

                return false;
            }
        }
        
        public bool AddUser(User user)
        {
            if (_users.Any(u => u.UserId == user.UserId))
                return false;

            _users.Add(user);
            return true;
        }

        public bool RemoveUser(User user)
        {
            if (_users.Any(u => u.UserId == user.UserId) == false)
                return false;

            _users.Remove(user);
            return true;
        }
    }
}
