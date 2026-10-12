namespace NutriPlan.Core.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }

        // Payload (Datos extra de la relación)
        public decimal Quantity { get; set; }
        public required string UnitOfMeasure { get; set; }

        // Propiedades de navegación
        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}