using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;

namespace MuscleMealUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        internal readonly MyDbContext _context = MyDbContext.GetInstance();
        private ViewModelBase _contentViewModel;
        public ViewModelBase ContentViewModel
        {
            get { return _contentViewModel; }
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }
#pragma warning disable CA1822 // Mark members as static
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }
        private static MainWindowViewModel? _instance = null;

        // For all the recipes
        public ReactiveCommand<Unit, Unit> NavigateToRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }


#pragma warning restore CA1822 // Mark members as static
        public MainWindowViewModel()
        {
            LoginViewModel vm = new LoginViewModel(this);

/*            NavigateToRecipeCommand = ReactiveCommand.Create(NavigateToAllRecipe);*/
            vm.Login.Subscribe(x => this.CurrentUser = vm.User);
            RegisterCommand = ReactiveCommand.Create(NavigateToRegister);
            this.ContentViewModel = vm;
        }
        /*        public void Register()
                {
                    //UserManager userManager = new UserManager(_context);
                    RegistrationViewModel vm = new RegistrationViewModel(this._context);
                    this.ContentViewModel = vm;
                }*/

/*        private void NavigateToAllRecipe()
        {
            Debug.WriteLine("MainWindowViewModel: NavigateToAllRecipe called");
            this.ContentViewModel = new AllRecipeViewModel();
        }
*/
        private void NavigateToRegister()
        {
            Debug.WriteLine("MainWindowViewModel: NavigateToRegister called");
            this.ContentViewModel = new RegistrationViewModel(this);
        }

        // To navigate to the login page(main window)
        public void NavigateToLogin()
        {
            Debug.WriteLine("MainWindowViewModel: NavigateToLogin called");
            this.ContentViewModel = new LoginViewModel(this);
        }
    }
}
