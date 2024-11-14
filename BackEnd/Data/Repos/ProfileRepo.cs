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
    }
}