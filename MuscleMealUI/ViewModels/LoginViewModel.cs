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

namespace MuscleMealUI.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        public LoginViewModel(MyDbContext dbContext)
        {
            var loginEnabled = this.WhenAnyValue(x => x.Username, x => !string.IsNullOrWhiteSpace(x));
            this.Login = ReactiveCommand.Create(() => { LoginUser(); }, loginEnabled);
            _context = dbContext;
        }
        private MyDbContext _context;
        public bool IsLogin { get; set; } = false;
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
        public ReactiveCommand<Unit, Unit> Login { get; }
        public ReactiveCommand<Unit, Unit> Register{ get; set; }
        
        public User? User { get; private set; }
        public User LoginUser()
        {
            UserManager userManager = new UserManager(_context);
            try 
            {
                if(userManager.Login(Username, Password)) 
                {
                    this.User = userManager.CurrentUser;
                }
            }
            catch (Exception ex) 
            {
                return null;
            }
            return this.User;
        }
    }
}


//xmlns: vm = "clr-namespace:MuscleMealUI.ViewModels;assembly=MuscleMealUI"