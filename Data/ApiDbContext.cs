using Microsoft.EntityFrameworkCore;

using mycookingrecepies.Models;

namespace mycookingrecepies.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Recipe> Recipes {get;set;}
        public DbSet<Ingridient> Ingridients{get;set;}

        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options)
        {

        }
    }
}
