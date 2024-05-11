namespace MuscleMeal
{
    public class MainApplication
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new MyDbContext())
            {
                // Initialize UserManager with DbContext
                var userManager = new UserManager(dbContext);

                // Create ProgramDisplayer with UserManager
                var mainDisplayer = new ProgramDisplayer(userManager);

                // Display the main menu and handle user interactions
                mainDisplayer.Display();
            }
        }
    }
}