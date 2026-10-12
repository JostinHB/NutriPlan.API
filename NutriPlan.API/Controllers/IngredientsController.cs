using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriPlan.API.DTOs;
using NutriPlan.Infrastructure.Data;

namespace NutriPlan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly NutriPlanDbContext _context;

        public IngredientsController(NutriPlanDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients
                .Where(i => i.IsActive)
                .Select(i => new IngredientDto(
                    i.IngredientId,
                    i.Name,
                    i.CaloriesPer100g,
                    i.ProteinPer100g,
                    i.CarbsPer100g,
                    i.FatPer100g
                ))
                .ToListAsync();

            return Ok(ingredients);
        }
        [HttpPost]
        public async Task<ActionResult<IngredientDto>> CreateIngredient(CreateIngredientDto request)
        {
            var newIngredient = new NutriPlan.Core.Entities.Ingredient
            {
                Name = request.Name,
                CaloriesPer100g = request.CaloriesPer100g,
                ProteinPer100g = request.ProteinPer100g,
                CarbsPer100g = request.CarbsPer100g,
                FatPer100g = request.FatPer100g,
                PricePer100g = request.PricePer100g,
                IsActive = true
            };

            _context.Ingredients.Add(newIngredient);
            await _context.SaveChangesAsync();
            var responseDto = new IngredientDto(
                newIngredient.IngredientId,
                newIngredient.Name,
                newIngredient.CaloriesPer100g,
                newIngredient.ProteinPer100g,
                newIngredient.CarbsPer100g,
                newIngredient.FatPer100g
            );
            return CreatedAtAction(nameof(GetIngredients), new { id = newIngredient.IngredientId }, responseDto);
        }
    }
}