using AnketSistemi.API.DTOs;
using AnketSistemi.API.Models;
using AnketSistemi.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            await _securityService.SeedRolesAsync();

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "Kayit basarili!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var token = await _securityService.GenerateJwtTokenAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var isAdmin = roles.Contains("Admin");

             
                return Ok(new { token = token, isAdmin = isAdmin, message = "Giris basarili" });
            }

            return Unauthorized(new { message = "E-posta veya sifre hatali!" });
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                fullName = user.FullName,
                email = user.Email
            });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;

            IdentityResult? passwordResult = null;
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {

                var check = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
                if (!check) return BadRequest(new { message = "Mevcut şifre yanlış." });
                passwordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            if (!string.IsNullOrEmpty(dto.FullName))
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    return BadRequest(updateResult.Errors);
            }

            return Ok(new { message = "Profil güncellendi." });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Ok(new { token = "", message = "Eğer bu e-posta kayıtlıysa token oluşturuldu." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new { token = token, email = user.Email, message = "Token oluşturuldu." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest(new { message = "Kullanıcı bulunamadı." });

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (result.Succeeded)
                return Ok(new { message = "Şifre başarıyla sıfırlandı." });

            return BadRequest(result.Errors);
        }
    }
}
