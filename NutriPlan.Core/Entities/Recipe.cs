namespace NutriPlan.Core.Entities
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public required string Name { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<DiseaseRecipe> DiseaseRecipes { get; set; } = new List<DiseaseRecipe>();
        public ICollection<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
    }
}