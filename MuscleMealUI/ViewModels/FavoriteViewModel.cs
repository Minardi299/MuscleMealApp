using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleMealUI.ViewModels
{
    public class FavoriteViewModel : ViewModelBase
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        private Recipe _selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => this._selectedRecipe;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedRecipe, value);
                
            }

        }
        public ObservableCollection<Recipe> Recipes { get; }
        public FavoriteViewModel(User user)
        {
            this._currentUser = user;
            UserService us = new UserService(this.CurrentUser);
            var recipes = us.GetCurrentUserFavorites();
            Recipes = new ObservableCollection<Recipe>(recipes);
            

        }

    }
}
