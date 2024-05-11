using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MuscleMealUI.Models;

namespace MuscleMealUI.Services
{
    public class UserManager
    {

        private readonly MyDbContext _context;
        public MyDbContext Context { get { return this._context; } }
        public UserManager(MyDbContext context)
        {
            this._context = context;
            this.CurrentUser = null;
        }
        public User? CurrentUser { get; private set; }

        public bool Register(string username, string bio, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || GetUserByUsername(username) != null)
            {
                return false;
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Create a new user with the hashed password and salt
            var newUser = new User(username, bio, passwordHash, passwordSalt);
            _context.User.Add(newUser);
            _context.SaveChanges();
            return true;
        }

        internal void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Generate a salt
            passwordSalt = new byte[8];
            // this is obsolete and throw a warming to use the static method instead
            //using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetBytes(passwordSalt);
            }

            // Hashing the password using the salt with RFC2898DeriveBytes
            int iterations = 1000;
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, passwordSalt, iterations);
            passwordHash = key.GetBytes(32);
        }

        public bool Login(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user != null && VerifyPassword(password, user.Password, user.PasswordSalt))
            {
                CurrentUser = user;
                Console.WriteLine($"Login successful. Welcome, {username}!");
                return true;

            }
            Console.WriteLine("Login failed. Wrong username or password. Try again!");
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        private bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            // Generate the hash of the entered password using the same salt and method
            int iterations = 1000;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(enteredPassword, storedSalt, iterations);
            byte[] enteredHash = key.GetBytes(32);

            // Compare the new hash pass with the old hash
            return storedHash.SequenceEqual(enteredHash);
        }

        public User? GetUserByUsername(string username)
        {
            try
            {
                // Directly returns the result of the query. Null is returned if no user is found.
                return
                _context.User
                .Include(user => user.Recipes)
                .Include(user => user.Favorites)
                .FirstOrDefault(user => user.Username == username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user {username}: {ex.Message}");
                return null;
            }
        }


        private string HashPassword(string password)
        {
            using var sha256 = new SHA256Managed();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

/*        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
*/
        public void DeleteUser(User user)
        {
            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.User.ToList();
        }

        public bool DeleteUserAccount(int userId)
        {
            var user = _context.User.Include(u => u.Recipes)
            .FirstOrDefault(u => u.ID == userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            try
            {
                // Optionally, we can remove user's recipes if needed
                _context.Recipe.RemoveRange(user.Recipes);

                // Remove the user
                _context.User.Remove(user);
                _context.SaveChanges();
                Console.WriteLine("User account deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user account: {ex.Message}");
                return false;
            }
        }

    }
}
