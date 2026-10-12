namespace NutriPlan.API.DTOs
{
    public record IngredientDto(
        int IngredientId,
        string Name,
        decimal CaloriesPer100g,
        decimal ProteinPer100g,
        decimal CarbsPer100g,
        decimal FatPer100g
    );
}