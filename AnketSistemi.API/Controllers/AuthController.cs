using AnketSistemi.API.DTOs;
using AnketSistemi.API.Models;
using AnketSistemi.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISecurityService _securityService;

        public AuthController(UserManager<AppUser> userManager, ISecurityService securityService)
        {
            _userManager = userManager;
            _securityService = securityService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserSignUpDto model)
        {
            var user = new AppUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { status = true, message = "Kayıt başarılı. Giriş yapabilirsiniz." });
            }
            return BadRequest(new { status = false, errors = result.Errors });
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(UserSignInDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _securityService.CreateJwtToken(user, roles);
                return Ok(new { status = true, token = token });
            }
            return Unauthorized(new { status = false, message = "Kullanıcı adı veya şifre hatalı." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("MakeAdmin")]
        public async Task<IActionResult> MakeAdmin(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound(new { status = false, message = "Kullanıcı bulunamadı." });

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { status = true, message = $"{userName} artık bir yönetici!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("RevokeAdmin")]
        public async Task<IActionResult> RevokeAdmin(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound(new { status = false, message = "Kullanıcı bulunamadı." });

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
                return BadRequest(new { status = false, message = "Bu kullanıcı zaten yönetici değil." });


            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return Ok(new { status = true, message = $"{userName} adlı kullanıcının yönetici yetkisi başarıyla alındı!" });
            }

            return BadRequest(new { status = false, message = "Yetki alınırken bir sorun oluştu." });
        }
    }
}