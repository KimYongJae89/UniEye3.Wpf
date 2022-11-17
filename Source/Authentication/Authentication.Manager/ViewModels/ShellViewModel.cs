using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Services;
using Prism.Commands;
using Prism.Regions;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;

namespace Authentication.Manager.ViewModels
{
    public class ShellViewModel : Observable
    {
        ILoggerFacade<ShellViewModel> _logger;

        IRegionManager _reggionManager;
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public string[] MenuItems { get; } = new string[] { "User", "Role", "Setting", "Log" };

        string _selected;
        public string Selected
        {
            get => _selected;
            set
            {
                Set(ref _selected, value);

                _reggionManager.RequestNavigate("ContentRegion", _selected);
            }
        }

        public ShellViewModel(AuthenticationService authenticationService, IRegionManager regionManager, ILoggerFacade<ShellViewModel> logger)
        {
            _reggionManager = regionManager;
            _logger = logger;

            LoadCommand = new DelegateCommand(() => Task.Run(() => authenticationService.Load()));
            SaveCommand = new DelegateCommand(authenticationService.Save);

            _logger.Info("Initialized");
        }
    }
}