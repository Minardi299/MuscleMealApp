using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleMealUI.Models;

namespace MuscleMealUI.Services
{
    public class UserService
    {
        private readonly User _user;
        private readonly MyDbContext _context;
        public UserService(User user)
        {
            this._context = MyDbContext.GetInstance();
            this._user = user;
        }

        public void AddRecipeToFavorites(Recipe recipe)
        {
            this._user.Favorites.Add(recipe);
            this._context.SaveChanges();
        }
        public void DeleteUser()
        {
            _context.User.Remove(this._user);
            _context.SaveChanges();
        }

        public void RemoveRecipeFromFavorites(Recipe recipe)
        {
            this._user.Favorites.Remove(recipe);
            this._context.SaveChanges();
        }
        public void RemoveRecipe(Recipe recipe)
        {
            var ingredients = _context.Ingredients.Where(i => i.Recipe.ID == recipe.ID).ToList();
            _context.Ingredients.RemoveRange(ingredients);

            this._user.Recipes.Remove(recipe);
            _context.Recipe.Remove(recipe);

            _context.SaveChanges();
        }
        // public void AddIngredient(string name, double amount, string unit)
        // {
        //     if (string.IsNullOrWhiteSpace(name))
        //         throw new ArgumentException("Ingredient name cannot be empty.");
        //     if (amount <= 0)
        //         throw new ArgumentException("Amount must be greater than zero.");
        //     if (string.IsNullOrWhiteSpace(unit))
        //         throw new ArgumentException("Unit cannot be empty.");

        //     var ingredient = new Ingredient(name, amount, unit);
        //     _context.Ingredients.Add(ingredient);

        //     try
        //     {
        //         _context.SaveChanges();
        //         Console.WriteLine("Ingredient added successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"An error occurred while adding the ingredient: {ex.Message}");
        //     }
        // }

        public void SubmitRecipe(Recipe recipe)
        {
            this._user.Recipes.Add(recipe);
            _context.SaveChanges();
        }

        public List<Recipe> SearchRecipes(string nameToSearch)
        {
            List<Recipe> recipes = _context.Recipe
                                    .Include(recipe => recipe.Owner)
                                    .Where(r => r.Name != null && r.Name.Contains(nameToSearch))
                                    .ToList();
            return recipes;
        }
        public List<Recipe> GetCurrentUserRecipes()
        {
            // return _context.Recipe.Where(r => r.Owner.ID == this._user.ID).ToList();   
            return _context.Recipe
                  .Where(r => r.Owner.ID == this._user.ID)
                  .Include(r => r.Ingredients)
                  .ToList();
        }

        public List<Recipe> GetCurrentUserFavorites()
        {//not working as expected yet, doesnt get the current usre favorites list
            // return _context.Recipe.Where(r => r.FavoriteBy.Any(f => f.ID == _user.ID)).ToList();   
            /*return this._user.Favorites.ToList();*/
            return _context.Recipe
                .Where(r => r.FavoriteBy.Any(f => f.ID == _user.ID))
                .Include(r => r.Ingredients)
                .Include(r => r.Owner)
                .ToList();
        }
        public static List<Recipe> GetAnotherUserRecipes(User user)
        {
            //doesnt work as expected
            // return _context.Recipe.Where(r=>r.Owner.ID == user.ID).ToList();
            return user.Recipes.ToList();
        }
        public List<Recipe> GetAllRecipes()
        {
            // return this._context.Recipe.ToList();
            return this._context.Recipe
            .Include(recipe => recipe.Owner)
            .Include(r => r.Ingredients)
            .ToList();
        }
    }
}
