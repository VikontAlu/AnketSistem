using AnketSistemi.API.Models;

namespace AnketSistemi.API.Services
{
    public interface ISecurityService
    {
        string CreateJwtToken(AppUser user, IList<string> roles);
    }
}