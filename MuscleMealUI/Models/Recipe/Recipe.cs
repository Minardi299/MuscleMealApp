using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleMealUI.Models
{
    //TODO do validation in the set itself
    public class Recipe
    {
        public int ID { get; private set; }

        private string? _name;
        public string? Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        private string? _description;
        public string? Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        private string _instruction;
        public string  Instruction
        {
            get { return this._instruction; }
            set { this._instruction = value; }
        }
         #pragma warning disable CS8618
        //this one is for visual studio to stop warning a field is nullable
        private User _owner;
        [InverseProperty("Recipes")]
        public User Owner
        {
            get { return this._owner; }
            set { this._owner = value; }
        }
        [InverseProperty("Favorites")]
        public List<User>? FavoriteBy
        {
            get; set;
        }

        private List<Ingredient> _ingrdients = new();
        public List<Ingredient> Ingredients
        {
            get { return _ingrdients; }
            set { _ingrdients = value; }
        }



        public Recipe(string name, string? description, User owner, List<Ingredient> ingredients, string instruction)
        {
            this._name = name ?? "no name"!;
            this._description = description;
            this._owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this._ingrdients = ingredients ?? new List<Ingredient>();
            this._instruction = instruction;
        }
        internal Recipe() { }
        public override string ToString()
        {
            return $"name: {this.Name}, Desciption {this.Description} by {this.Owner?.Username}";
        }
    }
}
