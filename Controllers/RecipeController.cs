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

            var item = await _context.Recipes.FirstOrDefaultAsync(z => z.Id == id && z.usernameId == userId);

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
                    Id = rec.Id,
                    text = rec.text,
                    usernameId = userId
                };

                await _context.Recipes.AddAsync(recipe);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRecipe", new {recipe.Id}, recipe);
            }

            return new JsonResult("Something went wrong") {StatusCode = 500};
        }

        [HttpPut]
        public async Task<IActionResult> ChangeRecipe(Recipe recipe)
        {
            //get current user
            var userId = User.Claims.First(p => p.Type == "id").Value.ToString();
    
            //find if there is an entry with the same recipe id 
            bool exists = _context.Recipes.Any(p => p.Id == recipe.Id);
            if (exists == false)
            {
                return NotFound();
            }

            //Recipe can be updated only if the username of the current user
            //matches the usernameId of that existing recipe in the db
            bool usernameMatches = _context.Recipes.Any(p => p.Id == recipe.Id && p.usernameId == userId);
            //!!! can't track two entities with the same id in the same time!!! use .Any to check values !!!
            if (!usernameMatches)
            {
                return Unauthorized();
            }

            recipe.usernameId = userId;

            //change the recipe entry in the db and set the state to modified
            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

    }
}