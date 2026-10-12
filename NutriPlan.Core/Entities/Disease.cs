namespace NutriPlan.Core.Entities
{
    public class Disease
    {
        public int DiseaseId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<UserDisease> UserDiseases { get; set; } = new List<UserDisease>();
        public ICollection<DiseaseRestrictedIngredient> RestrictedIngredients { get; set; } = new List<DiseaseRestrictedIngredient>();
        public ICollection<DiseaseRecipe> DiseaseRecipes { get; set; } = new List<DiseaseRecipe>();
    }
}