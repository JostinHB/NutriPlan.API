using Microsoft.EntityFrameworkCore;
using NutriPlan.Core.Entities;

namespace NutriPlan.Infrastructure.Data
{
    public class NutriPlanDbContext : DbContext
    {
        public NutriPlanDbContext(DbContextOptions<NutriPlanDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<UserAllergy> UserAllergies { get; set; }
        public DbSet<UserDisease> UserDiseases { get; set; }
        public DbSet<AllergyRestrictedIngredient> AllergyRestrictedIngredients { get; set; }
        public DbSet<DiseaseRestrictedIngredient> DiseaseRestrictedIngredients { get; set; }
        public DbSet<DiseaseRecipe> DiseaseRecipes { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<MealPlanItem> MealPlanItems { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealPlan>()
                .HasKey(mp => mp.PlanId);

            modelBuilder.Entity<Profile>()
                .HasKey(p => p.UserId);


            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<UserAllergy>()
                .HasKey(ua => new { ua.UserId, ua.AllergyId });

            modelBuilder.Entity<UserAllergy>()
                .HasOne(ua => ua.Profile)
                .WithMany()
                .HasForeignKey(ua => ua.UserId);
            modelBuilder.Entity<UserDisease>()
                .HasKey(ud => new { ud.UserId, ud.DiseaseId });

            modelBuilder.Entity<UserDisease>()
                .HasOne(ud => ud.Profile)
                .WithMany()
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<AllergyRestrictedIngredient>()
                .HasKey(ar => new { ar.AllergyId, ar.IngredientId });

            modelBuilder.Entity<DiseaseRestrictedIngredient>()
                .HasKey(dr => new { dr.DiseaseId, dr.IngredientId });

            modelBuilder.Entity<DiseaseRecipe>()
                .HasKey(dr => new { dr.DiseaseId, dr.RecipeId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany()
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AllergyRestrictedIngredient>()
                .HasOne(ar => ar.Ingredient)
                .WithMany()
                .HasForeignKey(ar => ar.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiseaseRestrictedIngredient>()
                .HasOne(dr => dr.Ingredient)
                .WithMany()
                .HasForeignKey(dr => dr.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RecipeStep>()
                .HasKey(rs => rs.StepId);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);
        }
    }
}