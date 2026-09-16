using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Api.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var adminUsername =
                _configuration["AdminAuth:Username"];

            var adminPassword =
                _configuration["AdminAuth:Password"];

            if (request.Username != adminUsername ||
                request.Password != adminPassword)
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı adı veya şifre hatalı."
                });
            }

            var jwtKey =
                _configuration["Jwt:Key"];

            var issuer =
                _configuration["Jwt:Issuer"];

            var audience =
                _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT Key yapılandırması bulunamadı.");
            }

            var expiresAt = DateTime.UtcNow.AddHours(8);

            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.Name,
                    request.Username),

                new(
                    ClaimTypes.Role,
                    "Admin")
            };

            var signingKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return Ok(new LoginResponse
            {
                Token = tokenString,
                ExpiresAt = expiresAt
            });
        }
    }
}