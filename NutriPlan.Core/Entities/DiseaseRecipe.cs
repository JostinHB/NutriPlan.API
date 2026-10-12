namespace NutriPlan.Core.Entities
{
    public class DiseaseRecipe
    {
        public int DiseaseId { get; set; }
        public int RecipeId { get; set; }

        public required string RelationType { get; set; }

        public Disease? Disease { get; set; }
        public Recipe? Recipe { get; set; }
    }
}