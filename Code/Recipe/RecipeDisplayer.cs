//Might not need this class with the rework Displayer
//Would be important to display a single recipe and possible action with that specific recipe?
namespace MuscleMeal
{
    public class RecipeDisplayer : IDisplayer
    {
        private Recipe _recipe;
        private MyDbContext _context;
        public RecipeDisplayer(MyDbContext context,Recipe recipe)
        {
            this._context = context;
            this._recipe=recipe;
        }
        public void Display()
        {
            bool session = true;
            while(session)
            {
                UserService u = new UserService(this._context,this._recipe.Owner);
                int choice = this.DisplayActions();
                switch(choice)
                {
                    case -1:
                        break;
                    case 1:
                        u.AddRecipeToFavorites(this._recipe);
                        break;
                    case 2:
                        u.RemoveRecipe(this._recipe);
                        session=false;
                        break;
                    case 3:
                        break;
                    case 4:
                        u.RemoveRecipeFromFavorites(this._recipe);
                        break;
                    case 5:
                        session=false;
                        break;
                    default: 
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        public int DisplayActions()
        {
            Console.WriteLine("Possible actions:");
            Console.WriteLine("1.Add To Favourite");
            Console.WriteLine("2.Delete recipe");
            Console.WriteLine("3.Modify recipe");
            Console.WriteLine("4.Remove recipe from  favourite");
            Console.WriteLine("5.Go back");
            string input = Console.ReadLine() ?? "-1";
            int choice = int.TryParse(input, out int parsedChoice) ? parsedChoice : -1;
            return  choice;
        }
    }
}
