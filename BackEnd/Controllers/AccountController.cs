using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Dtos;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Profile> _userManager;
        public AccountController(UserManager<Profile> userManger){
            _userManager = userManger;
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
                        return Ok("User created");
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
    }
}