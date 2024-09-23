using System.Collections.ObjectModel;
using ReactiveUI;
using MuscleMealUI.Models;
using MuscleMealUI.Services;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MuscleMealUI.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Reactive.Linq;
using System;

namespace MuscleMealUI.ViewModels
{
    public class AllRecipeViewModel : ViewModelBase
    {
        private readonly RecipeService _recipeService;
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }
        private ObservableCollection<Recipe> _allRecipes;
        public ObservableCollection<Recipe> Recipes { get; private set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
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
        private HomeViewModel _homeViewModel;
        public void NavigateToRecipe(Recipe recipe)
        {
            this._homeViewModel.CurrentView = new RecipeViewModel(recipe, _homeViewModel);
        }

        public AllRecipeViewModel(HomeViewModel homeViewModel)
        {
            this._homeViewModel = homeViewModel;
            _recipeService = new RecipeService();
            var recipes = _recipeService.GetAllRecipes();
            _allRecipes = new ObservableCollection<Recipe>(recipes);
            Recipes = new ObservableCollection<Recipe>(_allRecipes);

            this.WhenAnyValue(x => x.SearchQuery)
                .Throttle(System.TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .Subscribe(_ => FilterRecipes());
            _homeViewModel = homeViewModel;
        }
        private void FilterRecipes()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                Recipes = new ObservableCollection<Recipe>(_allRecipes);
            }
            else
            {

                Recipes = new ObservableCollection<Recipe>(_allRecipes.Where(r =>
                    r.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    r.Ingredients.Any(i => i.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                ));
            }

            this.RaisePropertyChanged(nameof(Recipes));
        }

    
    }
}
