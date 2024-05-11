using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleMealUI.Models
{
    public class User
    {
        //TODO do validation in the set itself
        private string? _username;
        public string? Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public int ID { get; private set; }
        private string? _bio;
        public string? Bio { get { return _bio; } set { _bio = value; } }
        private byte[]? _password;
        public byte[]? Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private byte[]? _passwordSalt;
        public byte[]? PasswordSalt
        {
            get { return _passwordSalt; }
            set { _passwordSalt = value; }
        }
        private List<Recipe> _favorites = new();

        [InverseProperty("FavoriteBy")]
        public List<Recipe> Favorites
        {
            get { return _favorites; }
            set { _favorites = value; }
        }
        //when having the initilizer in the field like this, 
        //you dont have to do it every single time you make a new constructor
        private List<Recipe> _recipes = new();
        [InverseProperty("Owner")]
        public List<Recipe> Recipes
        {
            get { return _recipes; }
            private set { _recipes = value; }
        }

        private User() { }

        public User(string name, string bio, byte[] password, byte[] passwordSalt)
        {
            Username = name;
            Bio = bio;
            Password = password;
            PasswordSalt = passwordSalt;
        }

        public Recipe this[int index]
        {
            get { return _recipes[index]; }
            set { _recipes[index] = value; }
        }
        //possible to make another indexer for the favorites list?
    }

}


