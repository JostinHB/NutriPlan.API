using System.ComponentModel.DataAnnotations;

namespace NutriPlan.API.DTOs
{
    public record CreateRecipeIngredientDto(
        [Required] int IngredientId,
        [Range(0.01, 10000)] decimal Quantity,
        [Required, MaxLength(20)] string UnitOfMeasure
    );

    public record CreateRecipeStepDto(
        [Required, Range(1, 100)] int StepNumber,
        [Required] string Instruction
    );

    public record CreateRecipeDto(
        [Required, MaxLength(150)] string Name,
        [Range(1, 1440)] int? PrepTimeMinutes,
        [Required] List<CreateRecipeIngredientDto> Ingredients,
        [Required] List<CreateRecipeStepDto> Steps
    );
    public record RecipeIngredientResponseDto(
        int IngredientId,
        string IngredientName,
        decimal Quantity,
        string UnitOfMeasure
    );

    public record RecipeStepResponseDto(
        int StepNumber,
        string Instruction
    );

    public record RecipeResponseDto(
        int RecipeId,
        string Name,
        int? PrepTimeMinutes,
        List<RecipeIngredientResponseDto> Ingredients,
        List<RecipeStepResponseDto> Steps
    );
}