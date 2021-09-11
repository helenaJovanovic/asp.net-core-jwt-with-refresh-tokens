using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using mycookingrecepies.Models;

namespace mycookingrecepies.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingridient> Ingridients{ get; set; }
        public DbSet<IngridientRecipe> IngridientRecipes { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IngridientRecipe>().HasKey(sc => new { sc.RecipeId, sc.IngridientId });
        }

       
    }
}
