using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FullStack.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace FullStack.API.Services
{
    public class TokenService
    {
        private IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Employee employee)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, employee.Name ?? string.Empty),
                    new(ClaimTypes.Email, employee.Email ?? string.Empty),
                    new(ClaimTypes.MobilePhone, employee.Phone.ToString()),
                    new(ClaimTypes.Role, employee.Department ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
