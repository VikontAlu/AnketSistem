using AnketSistemi.API.DTOs;
using AnketSistemi.API.Models;
using AnketSistemi.API.Services;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            await _securityService.SeedRolesAsync(); // İlk kayıtta rolleri oluşturmayı garantiye alalım

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                // İlk kayıt olanı User yapalım, adminliği biz veritabanından elle veya ayrı metotla veririz.
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "Kayıt başarılı!" });
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
                return Ok(new { token = token, message = "Giriş başarılı" });
            }

            return Unauthorized(new { message = "E-posta veya şifre hatalı!" });
        }
    }
}