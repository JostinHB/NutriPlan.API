using System.ComponentModel.DataAnnotations;

namespace NutriPlan.API.DTOs
{
    public record RegisterUserDto(
        [Required, EmailAddress] string Email,
        [Required, MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")] string Password
    );

    public record LoginUserDto(
        [Required, EmailAddress] string Email,
        [Required] string Password
    );

    public record AuthResponseDto(
        string Token,
        int UserId,
        string Email
    );
}