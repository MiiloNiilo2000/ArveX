using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceHistoryController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public InvoiceHistoryController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInvoices(){
            var invoices = await _context.Invoice.ToListAsync();

            if(invoices == null || !invoices.Any()){
                return NotFound("Ühtki varasemat arvet ei leitud.");
            }
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoicesById(int id){
            var invoice = await _context.Invoice.FindAsync(id);

            if(invoice == null){
                return NotFound("Ühtki varasemat arvet ei leitud.");
            }

            return Ok(invoice);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var invoice = _context.Invoice.FirstOrDefault(x => x.InvoiceId == id);

            if (invoice == null){
                return NotFound();
            }

            _context.Invoice.Remove(invoice);

            _context.SaveChanges();

            return NoContent();
        }
    }
}