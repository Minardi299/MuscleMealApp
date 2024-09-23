using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using MuscleMealUI.Models;
using MuscleMealUI.Services;
using ReactiveUI;

namespace MuscleMealUI.ViewModels
{
    public class AddRecipeViewModel : ViewModelBase
    {
        private string _description = String.Empty;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _instruction;
        public string Instruction
        {
            get => _instruction;
            set => this.RaiseAndSetIfChanged(ref _instruction, value);
        }

        public ObservableCollection<Ingredient> Ingredients { get; } = new ObservableCollection<Ingredient>();





        public ReactiveCommand<Unit, Recipe> CreateRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        private User _owner;
        public AddRecipeViewModel(User user)
        {
            this._owner = user;
            AddIngredientCommand = ReactiveCommand.Create(AddIngredient);
            var isValidObserbvable = this.WhenAnyValue(
                x => x.Name,
                x => x.Description,
                x => x.Instruction,

                (name, description, instruction) =>
                    !string.IsNullOrEmpty(name) &&
                    !string.IsNullOrEmpty(description) &&
                    !string.IsNullOrEmpty(instruction))
                .CombineLatest(Ingredients
                        .ToObservableChangeSet()
                        .ToCollection()
                        .Select(ingredients => ingredients.Any()),
                    (isValid, hasIngredients) => isValid && hasIngredients);
            CreateRecipeCommand = ReactiveCommand.Create(() => new Recipe 
            {
                Name = Name,
                Description = Description, 
                Owner = this._owner,
                Ingredients = Ingredients.ToList(),
                Instruction = Instruction 
            }, isValidObserbvable);
            CancelCommand = ReactiveCommand.Create(() => { });
        }
        public void SubmitRecipe()
        {

        }
        public ReactiveCommand<Unit, Unit> AddIngredientCommand { get; }
        private string _ingredientName;
        public string IngredientName
        {
            get => _ingredientName;
            set => this.RaiseAndSetIfChanged(ref _ingredientName, value);
        }

        private string _ingredientAmount;
        public string IngredientAmount
        {
            get => _ingredientAmount;
            set => this.RaiseAndSetIfChanged(ref _ingredientAmount, value);
        }

        private string _ingredientUnit;
        public string IngredientUnit
        {
            get => _ingredientUnit;
            set => this.RaiseAndSetIfChanged(ref _ingredientUnit, value);
        }
        private void AddIngredient()
        {
            if (!string.IsNullOrWhiteSpace(IngredientName) &&
                double.TryParse(IngredientAmount, out double amount) &&
                !string.IsNullOrWhiteSpace(IngredientUnit))
            {
                Ingredients.Add(new Ingredient
                {
                    Name = IngredientName,
                    Amount = amount,
                    Unit = IngredientUnit
                });
                IngredientName = string.Empty;
                IngredientAmount = string.Empty;
                IngredientUnit = string.Empty;
            }
        }
    }
 
}
