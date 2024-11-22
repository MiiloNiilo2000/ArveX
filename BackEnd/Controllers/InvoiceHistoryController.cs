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
    public class InvoiceHistoryController : ControllerBase
    {
        private readonly InvoicesRepoBase<PrivatePersonInvoice> _privatePersonInvoicesRepo;
        private readonly InvoicesRepoBase<CompanyInvoice> _companyInvoicesRepo;

        public InvoiceHistoryController(
            InvoicesRepoBase<PrivatePersonInvoice> privatePersonInvoicesRepo,
            InvoicesRepoBase<CompanyInvoice> companyInvoicesRepo)
        {
            _privatePersonInvoicesRepo = privatePersonInvoicesRepo;
            _companyInvoicesRepo = companyInvoicesRepo;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInvoices([FromQuery] string? invoiceType)
        {
            IEnumerable<PrivatePersonInvoice> invoices = new List<PrivatePersonInvoice>();

            if (string.IsNullOrEmpty(invoiceType)){
                var companyInvoices = await _companyInvoicesRepo.GetAllInvoices();
                var privatePersonInvoices = await _privatePersonInvoicesRepo.GetAllInvoices();
                invoices = companyInvoices.Cast<PrivatePersonInvoice>().Concat(privatePersonInvoices);
            }
            else if (invoiceType.ToLower() == "company"){
                var companyInvoices = await _companyInvoicesRepo.GetAllInvoices();
                invoices = companyInvoices.Cast<PrivatePersonInvoice>(); 
            }
            else if (invoiceType.ToLower() == "privateperson"){
                // Fetch private person invoices only
                var privatePersonInvoices = await _privatePersonInvoicesRepo.GetAllInvoices();
                invoices = privatePersonInvoices;
            }
            else{
                return BadRequest("Invalid invoice type.");
            }

            if (invoices == null || !invoices.Any()){
                return NotFound("No invoices found.");
            }

            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoicesById(int id, [FromQuery] string? invoiceType)
        {
            if (string.IsNullOrEmpty(invoiceType)){
                return BadRequest("Invoice type is required.");
            }

            PrivatePersonInvoice invoice;

            if (invoiceType.ToLower() == "company"){
                // Fetch company invoice and cast it to PrivatePersonInvoice
                invoice = await _companyInvoicesRepo.GetInvoicesById(id);
            }
            else if (invoiceType.ToLower() == "privateperson"){
                invoice = await _privatePersonInvoicesRepo.GetInvoicesById(id);
            }
            else{
                return BadRequest("Invalid invoice type.");
            }

            if (invoice == null){
                return NotFound("Invoice not found.");
            }

            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string? invoiceType)
        {
            bool result;

            if (invoiceType.ToLower() == "company"){
                result = await _companyInvoicesRepo.DeleteInvoice(id);
            }
            else if (invoiceType.ToLower() == "privateperson"){
                result = await _privatePersonInvoicesRepo.DeleteInvoice(id);
            }
            else{
                return BadRequest("Invalid invoice type.");
            }

            return result ? NoContent() : NotFound();
        }
    }
}