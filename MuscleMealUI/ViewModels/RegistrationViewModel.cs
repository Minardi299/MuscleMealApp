using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleMealUI.Services;
using ReactiveUI;
using System.Reactive;

namespace MuscleMealUI.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager _userManager;

        private readonly MainWindowViewModel _mainWindowViewModel;

        public RegistrationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _context = MyDbContext.GetInstance();
            _userManager = new UserManager();

            _mainWindowViewModel = mainWindowViewModel;
            var canRegister = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                x => x.Bio,
                (username, password, bio) =>
                    !string.IsNullOrEmpty(username) &&
                    !string.IsNullOrEmpty(password) &&
                    !string.IsNullOrEmpty(bio));

            RegisterCommand = ReactiveCommand.Create(Register,canRegister);
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private string _bio;
        public string Bio
        {
            get { return _bio; }
            set { this.RaiseAndSetIfChanged(ref _bio, value); }
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        private void Register()
        {
            if (_userManager.Register(Username, Bio, Password)) 
            { 
            
                _mainWindowViewModel.NavigateToLogin();
            }
            else
            {
                
            }
        }
    }
}
