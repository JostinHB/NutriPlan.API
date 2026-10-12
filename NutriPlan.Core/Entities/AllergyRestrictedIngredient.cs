namespace NutriPlan.Core.Entities
{
    public class AllergyRestrictedIngredient
    {
        public int AllergyId { get; set; }
        public int IngredientId { get; set; }

        public Allergy? Allergy { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}