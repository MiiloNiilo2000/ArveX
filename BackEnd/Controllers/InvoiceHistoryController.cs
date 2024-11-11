using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.Data.Repos;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceHistoryController(InvoiceRepo repo) : ControllerBase
    {
        private readonly InvoiceRepo repo = repo;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInvoices(){
            var invoices = await repo.GetAllInvoices();

            if(invoices == null || !invoices.Any()){
                return NotFound("Ühtki varasemat arvet ei leitud.");
            }
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoicesById(int id){
            var invoice = await repo.GetInvoicesById(id);

            if(invoice == null){
                return NotFound("Ühtki varasemat arvet ei leitud.");
            }

            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            bool result = await repo.DeleteInvoice(id);

            return result ? NoContent() : NotFound();
        }
    }
}