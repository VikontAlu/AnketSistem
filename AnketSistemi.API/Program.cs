using AnketSistemi.API.Data;
using AnketSistemi.API.Models;
using AnketSistemi.API.Repositories;
using AnketSistemi.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- CORS (BUG FIX #2: MVC'den gelen isteklere izin veriyoruz) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
        policy
            .WithOrigins(
                "http://localhost:5208",
                "https://localhost:7230",
                "http://localhost:5229",
                "https://localhost:7265")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// --- DbContext ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Identity ---
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// --- JWT Authentication ---
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// --- Generic Repository ---
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// --- Security Service ---
builder.Services.AddScoped<ISecurityService, SecurityService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// BUG FIX #2: CORS middleware - UseAuthentication'dan ÖNCE olmali
app.UseCors("AllowMVC");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- OTOMATİK VERİ EKLEME (SEEDING) İŞLEMİ BAŞLANGICI ---
// --- OTOMATİK VERİ EKLEME (SEEDING) İŞLEMİ BAŞLANGICI ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<AnketSistemi.API.Models.AppUser>>();
        var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<AnketSistemi.API.Models.AppRole>>();

        // 1. Rolleri Kontrol Et ve Yoksa Oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new AnketSistemi.API.Models.AppRole { Name = "Admin" });

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new AnketSistemi.API.Models.AppRole { Name = "User" });

        // 2. Otomatik Admin Hesabı Oluştur
        string adminEmail = "sistem@anketsistemi.com";
        string adminPass = "Anket123*"; // Identity varsayılan olarak büyük/küçük harf, rakam ve özel karakter ister

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new AnketSistemi.API.Models.AppUser
            {   
                UserName = adminEmail, // Giriş için e-posta kullanacağız
                Email = adminEmail,
                FullName = "Sistem Yöneticisi"
            };

            var result = await userManager.CreateAsync(adminUser, adminPass);
            if (result.Succeeded)
            {
                // Hesaba Admin yetkisini ver
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Seeding Hatası: " + ex.Message);
    }
}
// --- OTOMATİK VERİ EKLEME BİTİŞİ ---

app.Run(); // Bu satır zaten en altta vardı
// --- OTOMATİK VERİ EKLEME BİTİŞİ ---

app.Run(); // Bu satır zaten en altta vardı
