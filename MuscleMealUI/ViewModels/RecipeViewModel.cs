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

    public class RecipeViewModel : ViewModelBase
    {
        private Recipe _recipe;
        private HomeViewModel _homeViewModel;

        public Recipe Recipe
        {
            get => _recipe;
            set => this.RaiseAndSetIfChanged(ref _recipe, value);
        }
        public User Owner => Recipe?.Owner;
        public string Name => Recipe?.Name;
        public string Description => Recipe?.Description;
        public string Instruction => Recipe?.Instruction;
        public ObservableCollection<Ingredient> Ingredients => new ObservableCollection<Ingredient>(Recipe?.Ingredients ?? new List<Ingredient>());

        public bool IsOwner => Recipe?.Owner?.Username == _homeViewModel.CurrentUser?.Username;

        public bool IsFavorited => _homeViewModel.CurrentUser?.Favorites?.Contains(Recipe) ?? false;
        public bool IsNotFavorited => !IsFavorited;


        public ReactiveCommand<Unit, Unit> DeleteRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> AddToFavoritesCommand { get; }
        public ReactiveCommand<Unit,Unit> RemoveFromFavoritesCommand { get; }

        public RecipeViewModel(Recipe recipe, HomeViewModel homeViewModel)
        {
            _recipe = recipe;
            _homeViewModel = homeViewModel;
            RemoveFromFavoritesCommand = ReactiveCommand.Create(RemoveFromFavorites);
            AddToFavoritesCommand = ReactiveCommand.Create(AddToFavorites);
            DeleteRecipeCommand = ReactiveCommand.Create(DeleteRecipe);
        }
        private void AddToFavorites()
        {
            UserService userService = new UserService(_homeViewModel.CurrentUser);
            userService.AddRecipeToFavorites(_recipe);
            this._homeViewModel.NavigateToFavorites();
        }
        private void RemoveFromFavorites()
        {
            UserService userService = new UserService(_homeViewModel.CurrentUser);
            userService.RemoveRecipeFromFavorites(_recipe);
            this._homeViewModel.NavigateToFavorites();
        }

        private void DeleteRecipe()
        {
            UserService userService = new UserService(_homeViewModel.CurrentUser);
            userService.RemoveRecipe(_recipe);
            _homeViewModel.NavigateToMyRecipes();
        }
    }
}
