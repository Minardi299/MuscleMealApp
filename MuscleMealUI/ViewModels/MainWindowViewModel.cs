using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Reactive;

namespace MuscleMealUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MyDbContext _context = new MyDbContext();
#pragma warning disable CA1822 // Mark members as static
        private ViewModelBase _contentViewModel;
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }
        public User CurrentUser { get; private set; }
        public void LoginUser()
        {
            LoginViewModel loginViewModel = new LoginViewModel(this._context);
            loginViewModel.Login.Subscribe(x =>
            {
                this.CurrentUser = loginViewModel.LoginUser();
                if (this.CurrentUser != null)
                {
                    loginViewModel.IsLogin = true;
                }
            });
            ContentViewModel = loginViewModel;
        }
      
        public void Register()
        {
            Console.WriteLine("registration");
        }
        public MainWindowViewModel()
        {
            LoginUser();    

        }
#pragma warning restore CA1822 // Mark members as static
    }
}
