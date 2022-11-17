using Authentication.Core.Datas;
using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Services;
using Prism.Commands;
using System.Collections.Generic;
using Unieye.WPF.Base.Helpers;

namespace Authentication.Manager.ViewModels
{
    public class RoleViewModel : Observable
    {
        private Role _currentRole = new Role();
        public Role CurrentRole
        {
            get => _currentRole;
            set => Set(ref _currentRole, value);
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<Role> RemoveCommand { get; private set; }
        public IEnumerable<Role> Roles => _authenticationService.Roles;

        AuthenticationService _authenticationService;
        ILoggerFacade<RoleViewModel> _logger;

        public RoleViewModel(AuthenticationService authenticationService, ILoggerFacade<RoleViewModel> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;

            AddCommand = new DelegateCommand(Add);
            RemoveCommand = new DelegateCommand<Role>((role) => _authenticationService.RemoveRole(role));

            _logger.Info("Initialized");
        }

        private void Add()
        {
            if (_authenticationService.AddRole(CurrentRole.Clone() as Role))
                _logger.Info("Add Role");
            else
                _logger.Warn("Exist Role");

            Clear();
        }

        private void Clear()
        {
            CurrentRole = new Role();
        }
    }
}
