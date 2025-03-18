using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocietyManagementApi.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocietyMangementApi.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWTToken(UserModel user)
        {
            // Read JWT settings from appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("JWT SecretKey is missing in appsettings.json");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims with only Username, Password, and Role
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),  // Store UserName
                    
                new Claim(ClaimTypes.Role, user.Role),      // Store Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique Token ID
            };

            // Get expiry time in minutes from appsettings.json
            var tokenExpiryInMinutes = Convert.ToDouble(jwtSettings["ExpiryMinutes"]);

            // Create token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
