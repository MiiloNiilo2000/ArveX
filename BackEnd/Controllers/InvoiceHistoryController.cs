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
using BackEnd.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class InvoiceHistoryController : ControllerBase
    {
        private readonly InvoicesRepoBase<PrivatePersonInvoice> _privatePersonInvoicesRepo;
        private readonly InvoicesRepoBase<CompanyInvoice> _companyInvoicesRepo;
        private readonly UserManager<Profile> _userManager;

        public InvoiceHistoryController(
            InvoicesRepoBase<PrivatePersonInvoice> privatePersonInvoicesRepo,
            InvoicesRepoBase<CompanyInvoice> companyInvoicesRepo,
            UserManager<Profile> userManager) 
        {
            _privatePersonInvoicesRepo = privatePersonInvoicesRepo;
            _companyInvoicesRepo = companyInvoicesRepo;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInvoices([FromQuery] string? invoiceType)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            IEnumerable<PrivatePersonInvoice> invoices = new List<PrivatePersonInvoice>();

            if (string.IsNullOrEmpty(invoiceType)){
                var companyInvoices = await _companyInvoicesRepo.GetAllInvoices();
                var privatePersonInvoices = await _privatePersonInvoicesRepo.GetAllInvoices();
                invoices = companyInvoices.Where(i => i.ProfileId == user.Id)
                                   .Cast<PrivatePersonInvoice>()
                                   .Concat(privatePersonInvoices.Where(i => i.ProfileId == user.Id))
                                   .GroupBy(i => i.Id)
                                   .Select(global => global.First());
            }
            else if (invoiceType.ToLower() == "company"){
                var companyInvoices = await _companyInvoicesRepo.GetAllInvoices();
                invoices = companyInvoices.Cast<PrivatePersonInvoice>(); 
            }
            else if (invoiceType.ToLower() == "privateperson"){
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
        public async Task<IActionResult> GetInvoicesById(int id)
        {
            var companyInvoice = await _companyInvoicesRepo.GetInvoicesById(id);
            if (companyInvoice != null) {
                return Ok(companyInvoice);  
            }

            var privatePersonInvoice = await _privatePersonInvoicesRepo.GetInvoicesById(id);
            if (privatePersonInvoice != null) {
                return Ok(privatePersonInvoice);
            }

            return NotFound("Invoice not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result;

            var companyInvoice = await _companyInvoicesRepo.GetInvoicesById(id);
            if (companyInvoice != null)
            {
                result = await _companyInvoicesRepo.DeleteInvoice(id);
                return result ? NoContent() : NotFound();
            }

            var privatePersonInvoice = await _privatePersonInvoicesRepo.GetInvoicesById(id);
            if (privatePersonInvoice != null)
            {
                result = await _privatePersonInvoicesRepo.DeleteInvoice(id);
                return result ? NoContent() : NotFound();
            }

            return NotFound("Invoice not found.");
        }
    }
}