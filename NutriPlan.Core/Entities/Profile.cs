namespace NutriPlan.Core.Entities
{
    public class Profile
    {
        public int UserId { get; set; } 
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? WeightKg { get; set; }
        public string? ActivityLevel { get; set; }
        public User? User { get; set; }
    }
}