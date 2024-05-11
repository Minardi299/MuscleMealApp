using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
[assembly: InternalsVisibleTo("ProjectTests")]

namespace MuscleMeal
{
    //TODO do validation in the set itself
    public class User
    {

        private string _username;
        public string Username
        {
            get { return this._username; }
            set { this._username = value; }
        }
        public int ID { get; private set; }
        private string _bio;
        public string Bio { get { return this._bio; } set { this._bio = value; } }
        private byte[] _password;
        public byte[] Password
        {
            get { return this._password; }
            set { this._password = value; }
        }
        private byte[] _passwordSalt;
        public byte[] PasswordSalt
        {
            get { return this._passwordSalt; }
            set { this._passwordSalt = value; }
        }
        private List<Recipe> _favorites = new();

        [InverseProperty("FavoriteBy")]
        public List<Recipe> Favorites
        {
            get { return this._favorites; }
            set { this._favorites = value; }
        }
        //when having the initilizer in the field like this, 
        //you dont have to do it every single time you make a new constructor
        private List<Recipe> _recipes = new();
        [InverseProperty("Owner")]
        public List<Recipe> Recipes
        {
            get { return this._recipes; }
            private set { this._recipes = value; }
        }

        private User() { }

        public User(string name, string bio, byte[] password, byte[] passwordSalt)
        {
            this.Username = name;
            this.Bio = bio;
            this.Password = password;
            this.PasswordSalt = passwordSalt;
        }

        public Recipe this[int index]
        {
            get { return _recipes[index]; }
            set { _recipes[index] = value; }
        }
        //possible to make another indexer for the favorites list?
    }
}
