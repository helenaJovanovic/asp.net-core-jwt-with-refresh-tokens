
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mycookingrecepies.Models
{
    public class Ingridient
    {
        [Key]
        public int IngridientId {get;set;}
        [Required]
        public string name {get;set;}

        IList<IngridientRecipe> IngridientRecipes { get; set; }

    }
}