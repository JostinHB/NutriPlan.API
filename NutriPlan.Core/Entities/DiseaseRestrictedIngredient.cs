namespace NutriPlan.Core.Entities
{
    public class DiseaseRestrictedIngredient
    {
        public int DiseaseId { get; set; }
        public int IngredientId { get; set; }

        public Disease? Disease { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}