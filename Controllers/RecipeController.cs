using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycookingrecepies.Data;
using mycookingrecepies.Models;

namespace mycookingrecepies.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public RecipeController(ApiDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            //Shows all recipes for now
            //TODO: Get the correct user identifier, jwt claims.sub ~ into what??
            //Have specific entries by user


            //get id of the current user in the controller

            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

            Debug.Write(userId);

            var items = await _context.Recipes.Where(x => x.usernameId == userId).ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {

            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

            var item = await _context.Recipes.FirstOrDefaultAsync(z => z.RecipeId == id && z.usernameId == userId);

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

                var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

                Debug.Write(userId);

                Recipe recipe = new Recipe
                {
                    RecipeId = rec.RecipeId,
                    text = rec.text,
                    usernameId = userId
                };

                await _context.Recipes.AddAsync(recipe);
                await _context.SaveChangesAsync();

                return CreatedAtAction("CreateItem", new {recipe.RecipeId }, recipe);
            }

            return new JsonResult("Something went wrong") {StatusCode = 500};
        }
        
        [HttpPut]
        public async Task<IActionResult> ChangeRecipe(Recipe recipe)
        {

            if (ModelState.IsValid)
            {
                //get current user
                var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

                //find if there is an entry with the same recipe id 
                bool exists = _context.Recipes.Any(p => p.RecipeId == recipe.RecipeId && p.usernameId == userId);
                if (exists == false)
                {
                    return NotFound();
                }

                recipe.usernameId = userId;

                //change the recipe entry in the db and set the state to modified
                _context.Entry(recipe).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return NoContent();
            }

            return new JsonResult("Invalid modelstate") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe.usernameId == userId)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return new JsonResult("The recipe cannot be deleted") { StatusCode = 500 };
        }

        
        [HttpGet("ingr/{RecipeId}")]
        public async Task<ActionResult<IEnumerable<Ingridient>>> GetIngredients(int RecipeId)
        {

            //Find the recipe by the id if the current user id mataches the user id for that recipe in the db
            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

            //Include must be added for Ingridients

            bool exists = _context.Recipes.Any(p => p.RecipeId == RecipeId && p.usernameId == userId);
            if (exists == false)
            {
                return NotFound();
            }

            List<int> ingridientRecipes = await _context.IngridientRecipes.Where(x => x.RecipeId == RecipeId).Select(x => x.IngridientId).ToListAsync();

            List<Ingridient> ingridients = await _context.Ingridients.Where(x => ingridientRecipes.Contains(x.IngridientId)).ToListAsync();


            return ingridients;
        }
        
        
        [HttpPost("ingr/{RecipeId}")]
        public async Task<IActionResult> AddIngredient(int RecipeId, [FromBody] Ingridient ingr)
        {
            if (ModelState.IsValid)
            {
                //prvo da li postoji takav username i recept sa takvim id i userid
                var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

                Recipe recipe = await _context.Recipes.Where(p => p.RecipeId == RecipeId && p.usernameId == userId).SingleOrDefaultAsync();
                if (recipe == null)
                {
                    return NotFound();
                }

                IngridientRecipe ir = new IngridientRecipe { Recipe = recipe, Ingridient = ingr};

                await _context.IngridientRecipes.AddAsync(ir);
                await _context.Ingridients.AddAsync(ingr);

                await _context.SaveChangesAsync();

                return CreatedAtAction("AddIngredient", new { ingr.IngridientId }, ingr);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        //Add an existing Ingridient to a recipe
        [HttpPut("ingr/{RecipeId}/{IngrId}")]
        public async Task<IActionResult> AddExistingIngridientToRecipe(int RecipeId, int IngrId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

                Recipe recipe = await _context.Recipes.Where(p => p.RecipeId == RecipeId && p.usernameId == userId).SingleOrDefaultAsync();
                if (recipe == null)
                {
                    return NotFound("Recipe not found");
                }

                Ingridient ingr = await _context.Ingridients.FindAsync(IngrId);
                if (ingr == null)
                {
                    return NotFound("Ingridient not found");
                }

                IngridientRecipe ir = new IngridientRecipe { Recipe = recipe, Ingridient = ingr };

                await _context.IngridientRecipes.AddAsync(ir);

                await _context.SaveChangesAsync();

                return NoContent();

            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpDelete("ingr/{RecipeId}/{IngrId}")]
        public async Task<IActionResult> DeleteIngridientFromRecipe(int RecipeId, int IngrId)
        {
            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();

            var recipe = await _context.Recipes.FindAsync(RecipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            var ingr = await _context.Ingridients.FindAsync(IngrId);
            
            if(ingr == null)
            {
                return NotFound();
            }

            if(recipe.usernameId == userId)
            {
                var ir = await _context.IngridientRecipes.FindAsync(RecipeId, IngrId);

                _context.Ingridients.Remove(ingr);
                _context.IngridientRecipes.Remove(ir);

                await _context.SaveChangesAsync();

                return NoContent();
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

    }
}