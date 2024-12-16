using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Egzaminas.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string username, string role, string accountId)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new (ClaimTypes.NameIdentifier, accountId.ToString()),
            };

            var secretToken = _configuration.GetSection("Jwt:Key").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7077",
                audience: "https://localhost:7077",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
