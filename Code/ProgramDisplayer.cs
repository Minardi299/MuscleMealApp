using System;
using System.Collections.Generic;

namespace MuscleMeal
{
    public class ProgramDisplayer : IDisplayer
    {
        private UserManager _userManager;

        public ProgramDisplayer(UserManager userManager)
        {
            _userManager = userManager;
        }

        public void Display()
        {
            bool session = true;
            while (session)
            {
                int choice = DisplayActions();
                switch (choice)
                {
                    case -1:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                    case 1:
                        RegisterUser();
                        break;
                    case 2:
                        if (LoginUser())
                        {
                            // UserSession(_userManager.CurrentUser);
                            UserDisplayer userDisplayer = new UserDisplayer(_userManager.CurrentUser, _userManager.Context);
                            userDisplayer.Display();
                        }
                        break;
                    case 3:
                        session = false;
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public int DisplayActions()
        {
            // Console.WriteLine(_userManager.CurrentUser);
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter choice (1-3): ");
            string input = Console.ReadLine() ?? "-1";
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 3)
            {
                return choice;
            }
            return -1;
        }

        private List<string> GetInfo()
        {
            List<string> infoList = new List<string>();
            Console.WriteLine("What's the username?");
            infoList.Add(Console.ReadLine());
            Console.Write("Enter your bio: ");
            infoList.Add(Console.ReadLine());
            Console.WriteLine("What's the password?");
            infoList.Add(Console.ReadLine());
            return infoList;
        }

        private void RegisterUser()
        {
            List<string> info = GetInfo();
            bool registered = _userManager.Register(info[0], info[1], info[2]);
            if (registered)
            {
                Console.WriteLine("Registration successful. Please log in.");
            }
            else
            {
                Console.WriteLine("Registration failed. User may already exist or input was invalid.");
            }
        }

        private bool LoginUser()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            return _userManager.Login(username, password);
        }

        // options that the user gets when they login
        private void UserSession(User user)
        {
            bool userSession = true;
            while (userSession)
            {
                Console.WriteLine("\nUser Options:");
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. View Recipes");
                Console.WriteLine("3. Add to Favorites");
                Console.WriteLine("4. Change Password");
                Console.WriteLine("5. Delete Account");
                Console.WriteLine("6. Logout");
                Console.Write("Select (1-5): ");
                string input = Console.ReadLine() ?? "-1";
                int choice = int.TryParse(input, out int parsedChoice) ? parsedChoice : -1;

                switch (choice)
                {
                    case 1:
                        // AddRecipe(user);
                        break;
                    case 2:
                        // ViewRecipes();
                        break;
                    case 3:
                        // AddToFavorites(user);
                        break;
                    case 4:
                        // ChangePassword(user);
                        break;
                    case 5:
                        if (ConfirmDeletAccount())  // Ensure the user really wants to delete their account
                        {
                            if (_userManager.DeleteUserAccount(user.ID))
                            {
                                Console.WriteLine("Account deleted successfully.");
                                userSession = false;  // End session after account deletion
                            }
                        }
                        break;
                    case 6:
                        _userManager.Logout();
                        userSession = false;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private bool ConfirmDeletAccount()
        {
            Console.WriteLine("Are you sure you want to delete your account? This cannot be undone. (yes/no)");
            string confirmation = Console.ReadLine();
            return confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
