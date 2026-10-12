using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriPlan.API.DTOs;
using NutriPlan.Core.Entities;
using NutriPlan.Infrastructure.Data;

namespace NutriPlan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly NutriPlanDbContext _context;

        public ProfilesController(NutriPlanDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ProfileResponseDto>> CreateProfile(CreateProfileDto request)
        {
            var profileExists = await _context.Profiles.AnyAsync(p => p.UserId == request.UserId);
            if (profileExists)
            {
                return Conflict("El usuario ya tiene un perfil biológico creado. Usa el endpoint PUT para actualizarlo.");
            }
            var userExists = await _context.Users.AnyAsync(u => u.UserId == request.UserId);
            if (!userExists)
            {
                return NotFound("El usuario especificado no existe en el sistema de autenticación.");
            }
            var allergyIds = request.AllergyIds ?? new List<int>();
            var diseaseIds = request.DiseaseIds ?? new List<int>();

            var validAllergiesCount = await _context.Allergies.CountAsync(a => allergyIds.Contains(a.AllergyId));
            var validDiseasesCount = await _context.Diseases.CountAsync(d => diseaseIds.Contains(d.DiseaseId));

            if (validAllergiesCount != allergyIds.Count || validDiseasesCount != diseaseIds.Count)
            {
                return BadRequest("Una o más alergias/enfermedades proporcionadas no existen en los catálogos.");
            }
            var newProfile = new Profile
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                HeightCm = request.HeightCm,
                WeightKg = request.WeightKg,
                ActivityLevel = request.ActivityLevel
            };
            var userAllergies = allergyIds.Select(id => new UserAllergy { UserId = request.UserId, AllergyId = id });
            var userDiseases = diseaseIds.Select(id => new UserDisease { UserId = request.UserId, DiseaseId = id });

            _context.Profiles.Add(newProfile);
            _context.UserAllergies.AddRange(userAllergies);
            _context.UserDiseases.AddRange(userDiseases);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfileByUserId), new { userId = newProfile.UserId }, null);
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileResponseDto>> GetProfileByUserId(int userId)
        {
            var profile = await _context.Profiles
                .AsNoTracking()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return NotFound();
            var userAllergies = await _context.UserAllergies
                .AsNoTracking()
                .Include(ua => ua.Allergy)
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            var userDiseases = await _context.UserDiseases
                .AsNoTracking()
                .Include(ud => ud.Disease)
                .Where(ud => ud.UserId == userId)
                .ToListAsync();
            int? age = profile.BirthDate.HasValue
                ? DateTime.Today.Year - profile.BirthDate.Value.Year - (DateTime.Today.DayOfYear < profile.BirthDate.Value.DayOfYear ? 1 : 0)
                : null;
            var responseDto = new ProfileResponseDto(
                profile.UserId,
                profile.FirstName,
                profile.LastName,
                age,
                profile.HeightCm,
                profile.WeightKg,
                profile.ActivityLevel,
                userAllergies.Select(ua => new HealthConditionDto(ua.AllergyId, ua.Allergy!.Name)).ToList(),
                userDiseases.Select(ud => new HealthConditionDto(ud.DiseaseId, ud.Disease!.Name)).ToList()
            );

            return Ok(responseDto);
        }
    }
}