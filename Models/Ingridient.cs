
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mycookingrecepies.Models
{
    public class Ingridient
    {
        [Required]
        public int Id {get;set;}
        [Required]
        public string name {get;set;}

        public int recipeId;
        [ForeignKey("recipeId")]
        public Recipe Recipe;

    }
}