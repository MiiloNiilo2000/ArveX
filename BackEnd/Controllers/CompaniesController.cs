using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CompaniesController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCompanies(){
            var companies = await _context.Company.ToListAsync();

            if(!companies.Any()){
                return NotFound();
            }
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompaniesById(int id){
            var companies = await _context.Company.FindAsync(id);

            if(companies == null){
            return NotFound();
            }
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> PostCompany([FromBody] Company company)
        {
            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompanies), new { id = company.CompanyId }, company);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var company = _context.Company.FirstOrDefault(x => x.CompanyId == id);

            if (company == null){
                return NotFound();
            }

            _context.Company.Remove(company);

            _context.SaveChanges();

            return NoContent();
        }
        [HttpGet("{id}/products")]
        public ActionResult<IEnumerable<Product>> GetCompanyProducts(int id)
        {
            var company = _context.Company!
                .Include(x => x.Products)
                .FirstOrDefault(x => x.CompanyId == id);

            if (company == null)
                return NotFound();

            return Ok(company.Products);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Company company){
            bool result = await _context.UpdateCompany(id, company);
            return result ? NoContent() : NotFound();
        }
    }
}