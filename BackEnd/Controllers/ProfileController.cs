using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Data.Repos;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController(ProfileRepo repo, ApplicationDbContext context) : ControllerBase()
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ProfileRepo repo = repo;

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
            var profiles = await _context.Profile.FindAsync(id);

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

            return CreatedAtAction(nameof(GetProfiles), new { id = profile.ProfileId }, profile);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var profile = _context.Profile.FirstOrDefault(x => x.ProfileId == id);

            if (profile == null){
                return NotFound();
            }

            _context.Profile.Remove(profile);

            _context.SaveChanges();

            return NoContent();
        }
        [HttpGet("{id}/companies")]
        public ActionResult<IEnumerable<Profile>> GetProfileCompanies(int id)
        {
            var profile = _context.Profile!
                .Include(x => x.Companies)
                .FirstOrDefault(x => x.ProfileId == id);

            if (profile == null)
                return NotFound();

            return Ok(profile.Companies);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profile profile){
            bool result = await _context.UpdateProfile(id, profile);
            return result ? NoContent() : NotFound();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await repo.Login(loginRequest);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}