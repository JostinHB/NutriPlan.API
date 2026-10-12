using System.ComponentModel.DataAnnotations;

namespace NutriPlan.API.DTOs
{
    public record CreateMealPlanItemDto(
        [Required] int RecipeId,
        [Required, Range(1, 7, ErrorMessage = "El día debe ser entre 1 (Lunes) y 7 (Domingo)")] int DayOfWeek,
        [Required, RegularExpression("^(Breakfast|Lunch|Dinner|Snack)$", ErrorMessage = "Tipo de comida inválido")] string MealType
    );

    public record CreateMealPlanDto(
        
        [Required, MaxLength(100)] string Name,
        [Required] DateTime StartDate,
        DateTime? EndDate,
        [Required, MinLength(1, ErrorMessage = "El plan debe tener al menos una comida")] List<CreateMealPlanItemDto> Items
    );
    public record MealPlanItemResponseDto(
        int MealPlanItemId,
        int RecipeId,
        string RecipeName,
        int DayOfWeek,
        string MealType
    );

    public record MealPlanResponseDto(
        int PlanId,
        int UserId,
        string Name,
        DateTime StartDate,
        DateTime? EndDate,
        List<MealPlanItemResponseDto> Items
    );
}