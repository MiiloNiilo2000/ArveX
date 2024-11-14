using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Controllers;
using BackEnd.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = BackEnd.Models.LoginRequest;

namespace BackEnd.Data.Repos
{
   public class ProfileRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public ProfileRepo(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        // public async Task<string> Login([FromBody] LoginRequest loginRequest)
        // {
        //     var dbProfile = await _context.Profile!.FirstOrDefaultAsync(profile => profile.UserName == loginRequest.Username);

        //     if (dbProfile == null || dbProfile.Password != HashPassword(loginRequest.Password)){
        //         return "";
        //     }
        //     return GenerateToken(dbProfile.Email);
        // }

        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: [],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        public string GenerateToken(string email) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "IOdjigfnhgSOIJ5835hdufhh2dshguhgArveX"u8.ToArray();

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