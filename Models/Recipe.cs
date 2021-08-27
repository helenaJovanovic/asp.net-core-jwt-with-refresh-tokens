
using System.ComponentModel.DataAnnotations;

namespace mycookingrecepies.Models
{
    public class Recipe
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public string text{get;set;}

        [Required]
        public string usernameId{get;set;}
    }
}