using AnketSistemi.API.Models;

namespace AnketSistemi.API.Services
{
    public interface ISecurityService
    {
        // Rol modeldeki TokenService yetenekleri
        Task<string> GenerateJwtTokenAsync(AppUser user);
        Task<bool> SeedRolesAsync(); // Admin ve User rollerini otomatik oluşturmak için
    }
}