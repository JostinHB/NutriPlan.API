namespace NutriPlan.Core.Entities
{
    public class UserDisease
    {
        public int UserId { get; set; }
        public int DiseaseId { get; set; }

        public Profile? Profile { get; set; }

        public Disease? Disease { get; set; }
    }
}