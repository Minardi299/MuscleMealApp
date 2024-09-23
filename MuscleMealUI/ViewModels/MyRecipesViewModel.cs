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
    public class MyRecipesViewModel : ViewModelBase
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }
        private HomeViewModel _homeViewModel;
        public string Name => CurrentUser?.Username;
        public string Description => CurrentUser?.Bio;
        private Recipe _selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => this._selectedRecipe;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedRecipe, value);
                if (value != null)
                {
                    NavigateToRecipe(value);
                }
            }

        }
        public ObservableCollection<Recipe> Recipes { get; }
        public MyRecipesViewModel(User user, HomeViewModel home)
        {
            this._currentUser = user;
            this._homeViewModel = home;
            UserService us = new UserService(this.CurrentUser);
            var recipes = us.GetCurrentUserRecipes();
            Recipes = new ObservableCollection<Recipe>(recipes);
        }



        // Method to handle "Show Details" button click
        public void NavigateToRecipe(Recipe recipe)
        {
            this._homeViewModel.CurrentView = new RecipeViewModel(recipe, _homeViewModel);
        }
        public void AddRecipe()
        {
            AddRecipeViewModel vm = new AddRecipeViewModel(this.CurrentUser);
            Observable.Merge(
                vm.CreateRecipeCommand,
                vm.CancelCommand.Select(_ => (Recipe?)null))
                .Take(1)
                .Subscribe(newRecipe =>
                {
                    if (newRecipe != null)
                    {
                        UserService service = new UserService(this.CurrentUser);
                        service.SubmitRecipe(newRecipe);
                    }
                    this._homeViewModel.NavigateToMyRecipes();
                });
            this._homeViewModel.CurrentView = vm;
        }


    }
}
