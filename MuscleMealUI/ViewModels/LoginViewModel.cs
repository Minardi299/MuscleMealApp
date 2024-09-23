using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System.Reactive;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reactive.Linq;

namespace MuscleMealUI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(MainWindowViewModel main)
        {
            var loginEnabled = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                (name, password) =>
                !string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(password));
            Login = ReactiveCommand.Create(() => { LoginUser(); }, loginEnabled);
            _main = main;
            this._context = MyDbContext.GetInstance();

        }
        private readonly MainWindowViewModel _main;
        private MyDbContext _context;


        private string _username = string.Empty;
        public string Username
        {
            get { return this._username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }

        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> Login { get; }
        public User? User { get; private set; }
        public User LoginUser()
        {
            UserManager userManager = new UserManager();
            try
            {
                if (userManager.Login(Username, Password))
                {
                    this.User = userManager.CurrentUser;

                    this._main.CurrentUser = this.User;
                    this._main.ContentViewModel = new HomeViewModel(_main);
                }
                else
                {
                    ErrorMessage = "Login failed. Wrong username or password. Please try again!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error occurred: {ex.Message}";
            }
            return this.User;
        }

    }
}