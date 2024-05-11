using System.Drawing.Text;

namespace MuscleMeal
{
    public class UserDisplayer : IDisplayer
    {
        private User _user;
        private UserService _userService;
        private MyDbContext _context;

        public UserDisplayer(User user, MyDbContext context)
        {
            this._user = user;
            this._userService = new UserService(context, user);
            this._context = context;
        }
        public void Display()
        {
            Console.WriteLine($"Username: {this._user.Username}");
            Console.WriteLine($"User_Bio: {this._user.Bio}");
            bool session = true;
            while (session)
            {
                int choice = this.DisplayActions();
                switch (choice)
                {
                    case -1:
                        break;
                    case 1:
                        Console.WriteLine("Enter the name of the recipe:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter the description:");
                        string description = Console.ReadLine();

                        // List<Ingredient> ingredients = new List<Ingredient>();
                        // Console.WriteLine("Enter ingredients (format: name,amount,unit; name,amount,unit; ...):");
                        // string ingredientInput = Console.ReadLine();

                        // if (string.IsNullOrEmpty(ingredientInput))
                        // {
                        //     Console.WriteLine("No ingredients entered. Please try again.");
                        //     break;
                        // }

                        // var ingredientEntries = ingredientInput.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        // foreach (var entry in ingredientEntries)
                        // {
                        //     var parts = entry.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        //     if (parts.Length == 3)
                        //     {
                        //         string ingredientName = parts[0].Trim();
                        //         double amount;
                        //         if (!double.TryParse(parts[1].Trim(), out amount))
                        //         {
                        //             Console.WriteLine($"Invalid amount for {ingredientName}. Please enter a valid number.");
                        //             continue;
                        //         }
                        //         string unit = parts[2].Trim();
                        //         if (string.IsNullOrWhiteSpace(unit))
                        //         {
                        //             Console.WriteLine($"Invalid unit for {ingredientName}. Please enter a valid unit.");
                        //             continue;
                        //         }
                        //         ingredients.Add(new Ingredient(ingredientName, amount, unit));
                        //     }
                        //     else
                        //     {
                        //         Console.WriteLine("One or more ingredients were formatted incorrectly and were skipped.");
                        //     }
                        // }

                        // if (ingredients.Count == 0)
                        // {
                        //     Console.WriteLine("No valid ingredients were provided, recipe not created.");
                        //     break;
                        // }

                        List<Ingredient> ingredients = new List<Ingredient> {
                            new Ingredient("Flour", 500, "grams"),
                            new Ingredient("Sugar", 150, "grams"),
                            new Ingredient("Butter", 100, "grams")
                        };

                        // Create and submit recipe
                        Recipe recipe = new Recipe(name, description, this._user, ingredients);
                        try
                        {
                            this._userService.SubmitRecipe(recipe);
                            Console.WriteLine("Recipe added successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while adding the recipe: {ex.Message}");
                        }
                        break;
                    case 2:
                        this.DisplayRecipes(this._userService.GetCurrentUserRecipes());
                        Console.WriteLine("Choose a name to view");
                        string r = Console.ReadLine();
                        RecipeService recipeService = new RecipeService(this._context);
                        recipeService.SelectRecipe(r);
                        RecipeDisplayer recipeDisplayer = new RecipeDisplayer(this._context, recipeService.CurrentRecipe);
                        recipeDisplayer.Display();
                        break;
                    case 3:
                        this.DisplayRecipes(this._userService.GetAllRecipes());
                        break;
                    case 5:
                        this.DisplayRecipes(this._userService.GetCurrentUserFavorites());
                        break;
                    case 6:
                        this.DisplayRecipes(this._userService.GetAllRecipes());
                        Console.WriteLine("Choose a name to add");
                        string re = Console.ReadLine();
                        RecipeService recipeService2 = new RecipeService(this._context);
                        recipeService2.SelectRecipe(re);
                        this._userService.AddRecipeToFavorites(recipeService2.CurrentRecipe);

                        break;
                    case 7:
                        if (ConfirmDeletAccount())  // Ensure the user really wants to delete their account
                        {
                            this._userService.DeleteUser();
                            Console.WriteLine("Account deleted successfully.");
                            session = false;
                        }
                        break;
                    case 8:
                        Console.WriteLine("Logging out...");
                        session = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
        private void DisplayRecipes(List<Recipe> recipes)
        {
            foreach (var item in recipes)
            {
                Console.WriteLine($"{item.ToString()}");
            }
        }
        private bool ConfirmDeletAccount()
        {
            Console.WriteLine("Are you sure you want to delete your account? This cannot be undone. (yes/no)");
            string confirmation = Console.ReadLine();
            return confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        public int DisplayActions()
        {
            Console.WriteLine("\nUser Options:");
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. Select Recipes");
            Console.WriteLine("3. View Recipes");
            Console.WriteLine("4. Change Password");
            Console.WriteLine("5. View Favorites");
            Console.WriteLine("6. Add a recipe to favorite");
            Console.WriteLine("7. Delete Account");
            Console.WriteLine("8. Logout");
            Console.Write("Select (1-5): ");
            string input = Console.ReadLine() ?? "-1";
            int choice = int.TryParse(input, out int parsedChoice) ? parsedChoice : -1;
            return choice;
        }
        public Recipe GetRecipeInfo()
        {
            //PRINT TO GET THE RECIPE INFO AND RETURN IT

            // Recipe recipe = new Recipe("Steak", "a steak", this._user, );
            // return recipe;
            return null;

        }

        public void userProfileInfo()
        {
            Console.WriteLine($"Username: {this._user.Username}");
            Console.WriteLine($"User_Bio: {this._user.Bio}");
            Console.WriteLine($"User_Pass: {Convert.ToBase64String(this._user.Password)}");
        }
    }
}

