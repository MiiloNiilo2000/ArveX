using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Data.Repos;
using BackEnd.Dtos;
using BackEnd.Extensions;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController(ApplicationDbContext context, UserManager<Profile> userManager, ICompanyRepo companyRepo) : ControllerBase()
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<Profile> _userManager = userManager;
        private readonly ICompanyRepo _companyRepo = companyRepo;

        [HttpGet("all")]
        public async Task<IActionResult> GetProfiles(){
            var profiles = await _context.Profile.ToListAsync();
            

            if(!profiles.Any()){
                return NotFound();
            }
            return Ok(profiles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfilesById(int id){
            var profiles = await _context.Profile.FindAsync(id.ToString());

            if(profiles == null){
            return NotFound();
            }
            return Ok(profiles);
        }

        [HttpPost]
        public async Task<IActionResult> PostProfile([FromBody] Profile profile)
        {
            _context.Profile.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfiles), new { id = profile.Id }, profile);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var profile = _context.Profile.FirstOrDefault(x => x.Id == id.ToString());

            if (profile == null){
                return NotFound();
            }

            _context.Profile.Remove(profile);

            _context.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpGet("Companies")]
        public async Task<IActionResult> GetUserCompanies()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            if (appUser == null)
            {
                return NotFound("User not found.");
            }
            var userCompanies = await _companyRepo.GetUserCompanies(appUser);
            return Ok(userCompanies);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profile profile){
            bool result = await _context.UpdateProfile(id, profile);
            return result ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpGet("Me")]
        public async Task<IActionResult> GetLoggedInUserDetails()
        {
            var username = User.GetUsername();
            
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return NotFound("User not found.");
            }

            var userDetails = new
            {
                Id = appUser.Id,
                Username = appUser.UserName,
                Email = appUser.Email
            };
            return Ok(userDetails);
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            if (appUser == null)
            {
                return NotFound("User not found.");
            }

            appUser.UserName = updateDto.UserName;
            appUser.Email = updateDto.Email;

            var result = await _userManager.UpdateAsync(appUser);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    userName = appUser.UserName,
                    email = appUser.Email
                });
            }
            
            return BadRequest(result.Errors); 
        }
    }
}