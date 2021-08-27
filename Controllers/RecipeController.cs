using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycookingrecepies.Data;
using mycookingrecepies.Models;

namespace mycookingrecepies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly ApiDbContext _context;

        public RecipeController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetRecipes()
        {
            //Shows all recipes for now
            //TODO: recipes by user id
            var items = _context.Recipes.ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var item = await _context.Recipes.FirstOrDefaultAsync(z => z.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);

        }
        
        [HttpPost]
        public async Task<IActionResult> CreateItem(Recipe rec)
        {
            if (ModelState.IsValid)
            {
                await _context.Recipes.AddAsync(rec);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRecipe", new {rec.Id}, rec);
            }

            return new JsonResult("Something went wrong") {StatusCode = 500};
        }      
    }
}