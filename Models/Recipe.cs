
using System.ComponentModel.DataAnnotations;

namespace mycookingrecepies.Models
{
    public class Recipe
    {
        [Key]
        public int Id {get; set;}
        [Required]
        public string text{get;set;}

        public string usernameId{get;set;}
    }
}