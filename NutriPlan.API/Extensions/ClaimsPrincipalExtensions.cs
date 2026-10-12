using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace NutriPlan.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("El token no contiene un identificador de usuario válido.");
        }
    }
}