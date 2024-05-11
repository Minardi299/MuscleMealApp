using Microsoft.EntityFrameworkCore;

namespace MuscleMeal
{
    public class RecipeService
    {
        private MyDbContext _context;
        private Recipe _recipe;
        public Recipe CurrentRecipe{get{return _recipe;}set{}}

        public RecipeService(MyDbContext context)
        {
            this._context = context;
        } 
        public List<Recipe> GetAllRecipes()
        {
            return this._context.Recipe.ToList();
        }
        public void SelectRecipe(string name)
        {
            try
            {
                this._recipe= _context.Recipe
                .Include(recipe => recipe.Owner)
                .FirstOrDefault(recipe => recipe.Name == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No recipe found for {name}:  ex.Message");
                this._recipe=null;
            }
        }
        public List<Recipe> SearchRecipeByName(string name)
        {
            try
            {
                // Directly returns the result of the query. Null is returned if no user is found.
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