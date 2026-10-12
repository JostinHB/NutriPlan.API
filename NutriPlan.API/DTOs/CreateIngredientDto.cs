using System.ComponentModel.DataAnnotations;

namespace NutriPlan.API.DTOs
{
    public record CreateIngredientDto(
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        string Name,

        [Range(0, 1000, ErrorMessage = "Las calorías deben ser un valor positivo")]
        decimal CaloriesPer100g,

        [Range(0, 100)]
        decimal ProteinPer100g,

        [Range(0, 100)]
        decimal CarbsPer100g,

        [Range(0, 100)]
        decimal FatPer100g,

        [Range(0, 10000)]
        decimal? PricePer100g
    );
}