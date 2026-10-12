using Microsoft.EntityFrameworkCore;
using NutriPlan.Core.Interfaces;
using NutriPlan.Infrastructure.Data;

namespace NutriPlan.Infrastructure.Services
{
    public class NutritionalValidationService : INutritionalValidationService
    {
        private readonly NutriPlanDbContext _context;

        public NutritionalValidationService(NutriPlanDbContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> ValidateMealPlanAsync(int userId, List<int> recipeIds)
        {
            var errors = new List<string>();
            var userAllergies = await _context.UserAllergies
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AllergyId)
                .ToListAsync();

            var userDiseases = await _context.UserDiseases
                .Where(ud => ud.UserId == userId)
                .Select(ud => ud.DiseaseId)
                .ToListAsync();

            if (!userAllergies.Any() && !userDiseases.Any())
            {
                return new ValidationResult(true, errors);
            }
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients!)
                    .ThenInclude(ri => ri.Ingredient)
                .Where(r => recipeIds.Contains(r.RecipeId))
                .ToListAsync();
            var restrictedIngredientIds = new HashSet<int>();

            if (userAllergies.Any())
            {
                var allergyRestrictions = await _context.AllergyRestrictedIngredients
                    .Where(ari => userAllergies.Contains(ari.AllergyId))
                    .Select(ari => ari.IngredientId)
                    .ToListAsync();

                foreach (var id in allergyRestrictions) restrictedIngredientIds.Add(id);
            }

            if (userDiseases.Any())
            {
                var diseaseRestrictions = await _context.DiseaseRestrictedIngredients
                    .Where(dri => userDiseases.Contains(dri.DiseaseId))
                    .Select(dri => dri.IngredientId)
                    .ToListAsync();

                foreach (var id in diseaseRestrictions) restrictedIngredientIds.Add(id);
            }
            foreach (var recipe in recipes)
            {
                if (recipe.RecipeIngredients == null) continue;

                var conflictingIngredients = recipe.RecipeIngredients
                    .Where(ri => restrictedIngredientIds.Contains(ri.IngredientId))
                    .Select(ri => ri.Ingredient?.Name ?? "Ingrediente Desconocido")
                    .ToList();

                if (conflictingIngredients.Any())
                {
                    errors.Add($"Peligro Nutricional: La receta '{recipe.Name}' contiene ingredientes restringidos por el perfil médico del paciente ({string.Join(", ", conflictingIngredients)}).");
                }
            }

            return new ValidationResult(!errors.Any(), errors);
        }
    }
}