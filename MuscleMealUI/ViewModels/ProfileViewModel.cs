using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MuscleMealUI.Services;

namespace MuscleMealUI.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private User _user;
        private readonly HomeViewModel _homeViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly UserManager _userManager;
        public User User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }


        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteProfileCommand { get; }

        public ProfileViewModel(User user, HomeViewModel homeViewModel, MainWindowViewModel mainWindowViewModel)
        {
            _user = user;
            _homeViewModel = homeViewModel;
            _userManager = new UserManager();
            _mainWindowViewModel = mainWindowViewModel;

            ChangePasswordCommand = ReactiveCommand.Create(NavigateToChangePassword);
            UpdateProfileCommand = ReactiveCommand.Create(NavigateToUpdateProfile);
            DeleteProfileCommand = ReactiveCommand.Create(DeleteProfile);
        }

        private void NavigateToChangePassword()
        {
            _homeViewModel.CurrentView = new ChangePasswordViewModel(_user, _mainWindowViewModel);
        }

        private void NavigateToUpdateProfile()
        {
            _homeViewModel.CurrentView = new UpdateProfileViewModel(_user, _homeViewModel, _mainWindowViewModel);
        }
        private void DeleteProfile()
        {
            UserService us = new UserService(this.User);
            us.DeleteUser();
        }
    }
}
