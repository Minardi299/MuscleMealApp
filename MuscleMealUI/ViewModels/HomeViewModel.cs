using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;


namespace MuscleMealUI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }
        private readonly MainWindowViewModel _main;
        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public HomeViewModel(MainWindowViewModel main)
        {
            this._main = main;
            this._currentUser = main.CurrentUser;
            TogglePaneCommand = ReactiveCommand.Create(TriggerPane);
            AllRecipesCommand = ReactiveCommand.Create(LoadAllRecipesView);
            MyFavoritesCommand = ReactiveCommand.Create(NavigateToFavorites);
            MyRecipesCommand = ReactiveCommand.Create(NavigateToMyRecipes);
            LogoutCommand = ReactiveCommand.Create(Logout);
            MyProfileCommand = ReactiveCommand.Create(NavigateToProfile);
            MyRecipesViewModel myRecipesViewModel = new MyRecipesViewModel(this.CurrentUser, this);
            this.CurrentView = myRecipesViewModel;
            this._main = main;
        }


        public ReactiveCommand<Unit, Unit> TogglePaneCommand { get; }
        public ReactiveCommand<Unit, Unit> MyProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> AllRecipesCommand { get; }
        public ReactiveCommand<Unit, Unit> MyFavoritesCommand { get; }
        public ReactiveCommand<Unit, Unit> MyRecipesCommand { get; }
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        private void Logout()
        {
            this._main.ContentViewModel = new LoginViewModel(this._main);
        }
        internal void NavigateToMyRecipes()
        {
            this.CurrentView = new MyRecipesViewModel(this._main.CurrentUser, this);
        }
        private void NavigateToProfile()
        {
            ProfileViewModel vm = new ProfileViewModel(this.CurrentUser, this, this._main);
            vm.DeleteProfileCommand.Subscribe(x =>
            {
               
                this._main.NavigateToLogin();

            });
            this.CurrentView = vm;
        }

        public void NavigateToFavorites()
        {
            FavoriteViewModel vm = new FavoriteViewModel(this.CurrentUser);
            vm.WhenAnyValue(vm => vm.SelectedRecipe)
              .Where(recipe => recipe != null)
              .Subscribe(recipe =>
              {
                  this.CurrentView = new RecipeViewModel(recipe, this);
              });
            this.CurrentView = vm;
        }
        private void LoadAllRecipesView()
        {
            this.CurrentView = new AllRecipeViewModel(this);
        }

    }
}
