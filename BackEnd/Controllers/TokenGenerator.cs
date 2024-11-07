using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Controllers
{
    public class TokenGenerator
    {
        public string GenerateToken(string email) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "OurSecurityKeyThatHasToBeSomeWhatOfALongArrayOfLetters"u8.ToArray();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, email),
                new(JwtRegisteredClaimNames.Email, email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "http://id.localhost:3000/",
                Audience = "http://localhost:3000/",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}