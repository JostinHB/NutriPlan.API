namespace NutriPlan.Core.Entities
{
    public class Allergy
    {
        public int AllergyId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<UserAllergy> UserAllergies { get; set; } = new List<UserAllergy>();
        public ICollection<AllergyRestrictedIngredient> RestrictedIngredients { get; set; } = new List<AllergyRestrictedIngredient>();
    }
}