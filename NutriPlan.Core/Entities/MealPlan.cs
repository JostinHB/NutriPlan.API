namespace NutriPlan.Core.Entities
{
    public class MealPlan
    {
        public int PlanId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public User? User { get; set; }
        public ICollection<MealPlanItem> Items { get; set; } = new List<MealPlanItem>();
    }
}