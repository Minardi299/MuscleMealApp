using System.Collections.ObjectModel;
using System.Collections.Generic;
using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;
using System.Reactive;

namespace MuscleMealUI.ViewModels
{
    public class AllRecipeDetailViewModel : ViewModelBase
    {
        private Recipe _recipe;
        private HomeViewModel _homeViewModel;

        public Recipe Recipe
        {
            get => _recipe;
            set => this.RaiseAndSetIfChanged(ref _recipe, value);
        }

        public string Name => Recipe?.Name;
        public string Description => Recipe?.Description;
        public string Instruction => Recipe?.Instruction;
        public string Owner => Recipe?.Owner.Username;
        public ObservableCollection<Ingredient> Ingredients => new ObservableCollection<Ingredient>(Recipe?.Ingredients ?? new List<Ingredient>());

        public ReactiveCommand<Unit, Unit> AddToFavoritesCommand { get; }

        public AllRecipeDetailViewModel(Recipe recipe, HomeViewModel homeViewModel)
        {
            _recipe = recipe;
            _homeViewModel = homeViewModel;

            AddToFavoritesCommand = ReactiveCommand.Create(AddToFavorites);
        }

        private void AddToFavorites()
        {
            UserService userService = new UserService(_homeViewModel.CurrentUser);
            userService.AddRecipeToFavorites(_recipe);
        }
    }
}
