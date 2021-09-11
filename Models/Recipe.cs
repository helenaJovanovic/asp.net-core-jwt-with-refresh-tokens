
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mycookingrecepies.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId {get; set;}
        [Required]
        public string text{get;set;}

        public string usernameId{get;set;}

        public IList<IngridientRecipe> IngridientRecipes { get; set; }

    }
}