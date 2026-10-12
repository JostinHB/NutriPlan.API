using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriPlan.API.DTOs;
using NutriPlan.Core.Entities;
using NutriPlan.Core.Interfaces;
using NutriPlan.Infrastructure.Data;
using NutriPlan.API.Extensions;

namespace NutriPlan.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlansController : ControllerBase
    {
        private readonly NutriPlanDbContext _context;
        private readonly INutritionalValidationService _validationService;
        public MealPlansController(NutriPlanDbContext context, INutritionalValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        [HttpPost]
        public async Task<ActionResult<MealPlanResponseDto>> CreateMealPlan(CreateMealPlanDto request)
        {
            var userId = User.GetUserId();

            var profileExists = await _context.Profiles.AnyAsync(p => p.UserId == userId);
            if (!profileExists)
            {
                return NotFound("No se encontró el perfil del usuario. Debe crear un perfil médico primero.");
            }

            var recipeIds = request.Items.Select(i => i.RecipeId).Distinct().ToList();
            var existingRecipesCount = await _context.Recipes.CountAsync(r => recipeIds.Contains(r.RecipeId));

            if (existingRecipesCount != recipeIds.Count)
            {
                return BadRequest("Una o más recetas especificadas no existen en el sistema.");
            }
            var validation = await _validationService.ValidateMealPlanAsync(userId, recipeIds);
            if (!validation.IsValid)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                {
                    Message = "Operación abortada por riesgo de salud.",
                    Details = validation.Errors
                });
            }
            var newPlan = new MealPlan
            {
                UserId = userId,
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Items = request.Items.Select(i => new MealPlanItem
                {
                    RecipeId = i.RecipeId,
                    DayOfWeek = i.DayOfWeek,
                    MealType = i.MealType
                }).ToList()
            };

            _context.MealPlans.Add(newPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMealPlanById), new { id = newPlan.PlanId }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealPlanResponseDto>> GetMealPlanById(int id)
        {
            var userId = User.GetUserId();

            var plan = await _context.MealPlans
                .AsNoTracking()
                .Include(mp => mp.Items)
                    .ThenInclude(mpi => mpi.Recipe)
                .FirstOrDefaultAsync(mp => mp.PlanId == id && mp.UserId == userId);

            if (plan == null) return NotFound("Plan no encontrado o no tienes permisos para verlo.");

            var responseDto = new MealPlanResponseDto(
                plan.PlanId,
                plan.UserId,
                plan.Name,
                plan.StartDate,
                plan.EndDate,
                plan.Items
                    .OrderBy(i => i.DayOfWeek)
                    .Select(i => new MealPlanItemResponseDto(
                        i.MealPlanItemId,
                        i.RecipeId,
                        i.Recipe?.Name ?? "Receta Desconocida",
                        i.DayOfWeek ?? 0,
                        i.MealType ?? "Unknown"
                    )).ToList()
            );

            return Ok(responseDto);
        }
    }
}