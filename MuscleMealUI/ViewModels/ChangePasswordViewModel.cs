using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace MuscleMealUI.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private string _oldPassword;
        private string _newPassword;
        private string _errorMessage;
        private readonly UserManager _userManager;
        private readonly User _user;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public string OldPassword
        {
            get => _oldPassword;
            set => this.RaiseAndSetIfChanged(ref _oldPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }

        public ChangePasswordViewModel(User user, MainWindowViewModel mainWindowViewModel)
        {
            _user = user;
            _userManager = new UserManager();
            _mainWindowViewModel = mainWindowViewModel;

            ChangePasswordCommand = ReactiveCommand.CreateFromTask(ChangePasswordAsync);
        }

        private async Task ChangePasswordAsync()
        {
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword))
            {
                ErrorMessage = "Both fields are required.";
                return;
            }

            if (!_userManager.VerifyPassword(OldPassword, _user.Password, _user.PasswordSalt))
            {
                ErrorMessage = "Old password is incorrect.";
                return;
            }

            _userManager.ChangePassword(_user.Username, OldPassword, NewPassword);
            ErrorMessage = "Password changed successfully.";

            // _homeViewModel.CurrentView = new ProfileViewModel(_user, _homeViewModel);
            this._mainWindowViewModel.ContentViewModel = new LoginViewModel(this._mainWindowViewModel);
        }
    }
}
