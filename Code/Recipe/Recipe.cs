/*
ID primary key 
Name 
Rating 
*/
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

namespace MuscleMeal
{
    //TODO do validation in the set itself
    public class Recipe
    {
        public int ID { get; private set; }

        private string _name;
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        private string _description;
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        private User _owner;
        [InverseProperty("Recipes")]
        public User Owner
        {
            get { return this._owner; }
            set { this._owner = value; }
        }
        [InverseProperty("Favorites")]
        public List<User> FavoriteBy
        {
            get; set;
        }

        private List<Ingredient> _Ingrdients = new();
        public List<Ingredient> Ingredients
        {
            get { return _Ingrdients; }
            set { _Ingrdients = value; }
        }



        public Recipe(string name, string description, User owner, List<Ingredient> ingredients)
        {
            this.Name = name;
            this.Description = description;
            this.Owner = owner;
            this._Ingrdients = ingredients;
        }
        private Recipe() { }
        public override string ToString()
        {
            return $"name: {this.Name}, Desciption {this.Description} by {this.Owner.Username}";
        }
    }
}
