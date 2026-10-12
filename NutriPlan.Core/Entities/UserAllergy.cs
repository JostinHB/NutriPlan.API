namespace NutriPlan.Core.Entities
{
    public class UserAllergy
    {
        public int UserId { get; set; }
        public int AllergyId { get; set; }

        public Profile? Profile { get; set; }
        public Allergy? Allergy { get; set; }
    }
}