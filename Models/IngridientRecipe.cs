using mycookingrecepies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mycookingrecepies.Models
{
    public class IngridientRecipe
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngridientId { get; set; }
        public Ingridient Ingridient { get; set; }
    }
}
