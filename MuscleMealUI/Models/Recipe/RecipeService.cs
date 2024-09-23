using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleMealUI.Models;

namespace MuscleMealUI.Services
{
    public class RecipeService
    {
        private readonly MyDbContext _context = MyDbContext.GetInstance();
        private Recipe? _recipe;
        public Recipe? CurrentRecipe { get { return _recipe; } set { } }

        public RecipeService()
        {

        }
        public List<Recipe> GetAllRecipes()
        {
            return this._context.Recipe
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Owner)
                .ToList();
        }
        public void SelectRecipe(string name)
        {
            try
            {
                this._recipe = _context.Recipe
                .Include(recipe => recipe.Owner)
                .FirstOrDefault(recipe => recipe.Name == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No recipe found for {name}:  {ex.Message}");
                this._recipe = null;
            }
        }
        public List<Recipe>? SearchRecipeByName(string name)
        {
            try
            {
                return _context.Recipe.Where(r => r.Name.Contains(name)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No recipe found for {name}: {ex.Message} ");
                return null;
            }
        }
    }
}
