namespace NutriPlan.Core.Entities
{
    public class MealPlanItem
    {
        public int MealPlanItemId { get; set; }
        public int PlanId { get; set; }
        public int RecipeId { get; set; }
        public int? DayOfWeek { get; set; }
        public string? MealType { get; set; } // Breakfast, Lunch, Dinner, Snack

        public MealPlan? MealPlan { get; set; }
        public Recipe? Recipe { get; set; }
    }
}