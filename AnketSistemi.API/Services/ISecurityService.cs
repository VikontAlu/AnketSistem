using AnketSistemi.API.Models;

namespace AnketSistemi.API.Services
{
    public interface ISecurityService
    {
        Task<string> GenerateJwtTokenAsync(AppUser user);
        Task<bool> SeedRolesAsync();
    }
}