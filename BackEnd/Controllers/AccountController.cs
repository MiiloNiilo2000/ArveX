using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Dtos;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Profile> _signInManager;
        public AccountController(UserManager<Profile> userManger, ITokenService tokenService, SignInManager<Profile> signInManager){
            _userManager = userManger;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto){
            try {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var profile = new Profile
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdProfile = await _userManager.CreateAsync(profile, registerDto.Password);

                if(createdProfile.Succeeded) 
                {
                    var roleResult = await _userManager.AddToRoleAsync(profile, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewProfileDto{
                                UserName = profile.UserName,
                                Email = profile.Email,
                                Token = _tokenService.CreateToken(profile)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdProfile.Errors);
                }
                
            }
            catch(Exception e) 
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null){
                 return Unauthorized("Invalid username!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewProfileDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
    }
}