using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriPlan.API.DTOs;
using NutriPlan.Core.Entities;
using NutriPlan.Infrastructure.Data;

namespace NutriPlan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly NutriPlanDbContext _context;

        public RecipesController(NutriPlanDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponseDto>> CreateRecipe(CreateRecipeDto request)
        {
            var ingredientIds = request.Ingredients.Select(i => i.IngredientId).ToList();
            var existingIngredientsCount = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.IngredientId))
                .CountAsync();

            if (existingIngredientsCount != ingredientIds.Count)
            {
                return BadRequest("Uno o más ingredientes especificados no existen en la base de datos.");
            }

            var newRecipe = new Recipe
            {
                Name = request.Name,
                PrepTimeMinutes = request.PrepTimeMinutes,
                RecipeIngredients = request.Ingredients.Select(i => new RecipeIngredient
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    UnitOfMeasure = i.UnitOfMeasure
                }).ToList(),
                Steps = request.Steps.Select(s => new RecipeStep
                {
                    StepNumber = s.StepNumber,
                    Instruction = s.Instruction
                }).ToList()
            };

            _context.Recipes.Add(newRecipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.RecipeId }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponseDto>> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null) return NotFound();

            var responseDto = new RecipeResponseDto(
                recipe.RecipeId,
                recipe.Name,
                recipe.PrepTimeMinutes,
                recipe.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto(
                    ri.IngredientId,
                    ri.Ingredient!.Name,
                    ri.Quantity,
                    ri.UnitOfMeasure
                )).ToList(),
                recipe.Steps.OrderBy(s => s.StepNumber).Select(s => new RecipeStepResponseDto(
                    s.StepNumber,
                    s.Instruction
                )).ToList()
            );

            return Ok(responseDto);
        }
    }
}