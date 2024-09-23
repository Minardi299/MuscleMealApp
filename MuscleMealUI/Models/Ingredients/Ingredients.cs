using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleMealUI.Models
{
    public class Ingredient
    {
        public int ID { get; private set; }

        private string? _name;
        // [Required(ErrorMessage = "The name of The Ingredient is Required!!")]
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double? _amount;
        public double? Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private string? _unit;
        public string? Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private Recipe? _recipe;
        public Recipe? Recipe
        {
            get { return _recipe; }
            set { _recipe = value; }
        }
        internal Ingredient() { }
        public Ingredient(string? name, double? amount, string? unit)
        {
            this._name = name;
            this._amount = amount;
            this._unit = unit;
        }
    }
}
