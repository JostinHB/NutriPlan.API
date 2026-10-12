using System.ComponentModel.DataAnnotations;

namespace NutriPlan.API.DTOs
{
    public record CreateProfileDto(
        
        [Required, MaxLength(100)] string FirstName,
        [Required, MaxLength(100)] string LastName,
        DateTime? BirthDate,
        [MaxLength(20)] string? Gender,
        [Range(30, 300)] decimal? HeightCm,
        [Range(2, 500)] decimal? WeightKg,
        [MaxLength(50)] string? ActivityLevel,
        List<int> AllergyIds = null!,
        List<int> DiseaseIds = null!
    );
    public record HealthConditionDto(int Id, string Name);

    public record ProfileResponseDto(
        int UserId,
        string FirstName,
        string LastName,
        int? Age,
        decimal? HeightCm,
        decimal? WeightKg,
        string? ActivityLevel,
        List<HealthConditionDto> Allergies,
        List<HealthConditionDto> Diseases
    );
}