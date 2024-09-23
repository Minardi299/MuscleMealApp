using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Reactive;

namespace MuscleMealUI.ViewModels
{
    public class UpdateProfileViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private readonly HomeViewModel _homeViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private string _username;
        private string _bio;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Bio
        {
            get => _bio;
            set => this.RaiseAndSetIfChanged(ref _bio, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

        public UpdateProfileViewModel(User user, HomeViewModel homeViewModel, MainWindowViewModel mainWindowViewModel)
        {
            _userManager = new UserManager();
            _homeViewModel = homeViewModel;
            _mainWindowViewModel = mainWindowViewModel;
            Username = user.Username;
            Bio = user.Bio;

            SubmitCommand = ReactiveCommand.Create(Submit);
        }

        private void Submit()
        {
            try
            {
                var user = _homeViewModel.CurrentUser;
                user.Username = Username;
                user.Bio = Bio;

                _userManager.UpdateUser(user);
                _homeViewModel.CurrentView = new ProfileViewModel(user, _homeViewModel, _mainWindowViewModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }
    }
}
