namespace NutriPlan.Core.Interfaces
{
    public record ValidationResult(bool IsValid, List<string> Errors);

    public interface INutritionalValidationService
    {
        Task<ValidationResult> ValidateMealPlanAsync(int userId, List<int> recipeIds);
    }
}