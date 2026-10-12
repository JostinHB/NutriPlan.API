namespace NutriPlan.Core.Entities
{
    public class RecipeStep
    {
        public int StepId { get; set; }
        public int RecipeId { get; set; }
        public int StepNumber { get; set; }
        public required string Instruction { get; set; }

        public Recipe? Recipe { get; set; }
    }
}